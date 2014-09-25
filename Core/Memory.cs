/* This file is part of My Nes
 * 
 * A Nintendo Entertainment System / Family Computer (Nes/Famicom) 
 * Emulator written in C#.
 *
 * Copyright © Ala Ibrahim Hadid 2009 - 2014
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */
using System;
using System.Reflection;
using System.IO;
using System.Diagnostics;
/*Memory section*/
namespace MyNes.Core
{
	public class Memory
	{
		public Board board;
		// Internal 2K Work RAM (mirrored to 800h-1FFFh)
		public byte[] WRAM;
		public byte[] palettes_bank;
		// Accessed via $2004
		public byte[] oam_ram;
		// The secondary oam...
		public byte[] oam_secondary;
		public int BUS_ADDRESS;
		public bool BUS_RW;
		private bool BUS_RW_P;
		// temps
		private byte temp_4015;
		private byte temp_4016;
		private byte temp_4017;

		private readonly NesEmu core;
		[Obsolete("Mega-hack until I can figure out which bits of DMA and Memory code need to switch classes.")]
		public Dma dma;

		public Memory(NesEmu nesEmu)
		{
			this.core = nesEmu;
		}

		public void MEMInitialize(IRom rom)
		{
			// Find the mapper
			Console.WriteLine("Finding mapper # " + rom.MapperNumber.ToString("D3"));
			bool found = false;
			string mapperName = "MyNes.Core.Mapper" + rom.MapperNumber.ToString("D3");
			Type[] types = Assembly.GetExecutingAssembly().GetTypes();
			foreach (Type tp in types)
			{
				if (tp.FullName == mapperName)
				{
					board = Activator.CreateInstance(tp) as Board;
					board.Initialize(rom.SHA1, rom.PRG, rom.CHR, rom.Trainer, rom.Mirroring);
					found = true;
					Console.WriteLine("Mapper # " + rom.MapperNumber.ToString("D3") + " initialized successfully.");
					break;
				}
			}
			if (!found)
			{
				Console.WriteLine("Mapper # " + rom.MapperNumber.ToString("D3") + " is not implemented yet.");
				throw new MapperNotSupportedException(rom.MapperNumber);
			}
		}
		public void MEMHardReset()
		{
			// memory
			WRAM = new byte[0x800];
			WRAM[0x0008] = 0xF7;
			WRAM[0x0008] = 0xF7;
			WRAM[0x0009] = 0xEF;
			WRAM[0x000A] = 0xDF;
			WRAM[0x000F] = 0xBF;
			palettes_bank = new byte[] // Miscellaneous, real NES loads values similar to these during power up
            {
                0x09, 0x01, 0x00, 0x01, 0x00, 0x02, 0x02, 0x0D, 0x08, 0x10, 0x08, 0x24, 0x00, 0x00, 0x04, 0x2C, // Bkg palette
                0x09, 0x01, 0x34, 0x03, 0x00, 0x04, 0x00, 0x14, 0x08, 0x3A, 0x00, 0x02, 0x00, 0x20, 0x2C, 0x08  // Spr palette
            };
			oam_ram = new byte[256];
			oam_secondary = new byte[32];
			BUS_ADDRESS = 0;
			BUS_RW = false;
			BUS_RW_P = false;
			// Read SRAM if found
			Trace.WriteLine("Reading SRAM");
			if (File.Exists(this.core.SRAMFileName))
			{
				Stream str = new FileStream(this.core.SRAMFileName, FileMode.Open, FileAccess.Read);
				byte[] inData = new byte[str.Length];
				str.Read(inData, 0, inData.Length);
				str.Flush();
				str.Close();

				byte[] outData = new byte[0];
				ZlipWrapper.DecompressData(inData, out outData);

				board.LoadSRAM(outData);

				Trace.WriteLine("SRAM read successfully.");
			}
			else
			{
				Trace.WriteLine("SRAM file not found; rom has no SRAM or file not exist.");
			}
			board.HardReset();
		}
		public void MEMSoftReset()
		{
			board.SoftReset();
		}
		public void MEMShutdown()
		{
			SaveSRAM();
			board = null;
		}
		public void SaveSRAM()
		{
			if (board != null)
				if (this.core.SaveSRAMAtShutdown && board.SRAMSaveRequired)
				{
					Trace.WriteLine("Saving SRAM ...");
					byte[] sramBuffer = new byte[0];
					ZlipWrapper.CompressData(board.GetSRAMBuffer(), out sramBuffer);

					Stream str = new FileStream(this.core.SRAMFileName, FileMode.Create, FileAccess.Write);
					str.Write(sramBuffer, 0, sramBuffer.Length);

					str.Flush();
					str.Close();
					Trace.WriteLine("SRAM saved successfully.");
				}
		}

		public byte Read(int address)
		{
			BUS_RW_P = BUS_RW;
			BUS_ADDRESS = address;
			BUS_RW = true;

			#region Clock Components
			this.core.PPUClock();
			/*
			 * NMI edge detector polls the status of the NMI line during φ2 of each CPU cycle 
			 * (i.e., during the second half of each cycle) 
			 */
			this.core.PollInterruptStatus();
			this.core.PPUClock();
			this.core.PPUClock();
			if (this.core.DoPalAdditionalClock)// In pal system ..
			{
				this.core.palCyc++;
				if (this.core.palCyc == 5)
				{
					this.core.PPUClock();
					this.core.palCyc = 0;
				}
			}
			this.core.APUClock();
			this.dma.DMAClock();

			board.OnCPUClock();
			#endregion

			if (address < 0x2000)// Internal 2K Work RAM (mirrored to 800h-1FFFh)
			{
				return WRAM[address & 0x7FF];
			}
			else if (address < 0x4000)
			{
				#region Internal PPU Registers (mirrored to 2008h-3FFFh)
				switch (address & 7)
				{
					case 2:// $2002
						{
							this.core.ppu_2002_temp = 0;

							if (this.core.vbl_flag)
								this.core.ppu_2002_temp |= 0x80;
							if (this.core.spr_0Hit)
								this.core.ppu_2002_temp |= 0x40;
							if (this.core.spr_overflow)
								this.core.ppu_2002_temp |= 0x20;

							this.core.vbl_flag_temp = false;
							this.core.vram_flipflop = false;

							this.core.CheckNMI();// NMI disable effect only at vbl set period (HClock between 1 and 3)

							return this.core.ppu_2002_temp;
						}
					case 4:// $2004
						{
							this.core.ppu_2004_temp = oam_ram[this.core.oam_address];
							if (this.core.VClock < 240 && this.core.IsRenderingOn())
							{
								if (this.core.HClock < 64)
									this.core.ppu_2004_temp = 0xFF;
								else if (this.core.HClock < 192)
									this.core.ppu_2004_temp = oam_ram[((this.core.HClock - 64) << 1) & 0xFC];
								else if (this.core.HClock < 256)
									this.core.ppu_2004_temp = ((this.core.HClock & 1) == 1) ? oam_ram[0xFC] : oam_ram[((this.core.HClock - 192) << 1) & 0xFC];
								else if (this.core.HClock < 320)
									this.core.ppu_2004_temp = 0xFF;
								else
									this.core.ppu_2004_temp = oam_ram[0];
							}
							return this.core.ppu_2004_temp;
						}
					case 7:// $2007
						{
							this.core.ppu_2007_temp = 0;

							if ((this.core.vram_address & 0x3F00) == 0x3F00)
							{
								// The return value should be from the palettes bank
								this.core.ppu_2007_temp = (byte)(palettes_bank[this.core.vram_address & ((this.core.vram_address & 0x03) == 0 ? 0x0C : 0x1F)] & this.core.grayscale);
								// fill buffer from chr or nametables
								this.core.vram_address_temp_access1 = this.core.vram_address & 0x2FFF;
								if (this.core.vram_address_temp_access1 < 0x2000)
								{
									this.core.reg2007buffer = board.ReadCHR(ref this.core.vram_address_temp_access1, false);
								}
								else
								{
									this.core.reg2007buffer = board.ReadNMT(ref this.core.vram_address_temp_access1);
								}
							}
							else
							{
								this.core.ppu_2007_temp = this.core.reg2007buffer;
								// fill buffer
								this.core.vram_address_temp_access1 = this.core.vram_address & 0x3FFF;
								if (this.core.vram_address_temp_access1 < 0x2000)
								{
									this.core.reg2007buffer = board.ReadCHR(ref this.core.vram_address_temp_access1, false);
								}
								else if (this.core.vram_address_temp_access1 < 0x3F00)
								{
									this.core.reg2007buffer = board.ReadNMT(ref this.core.vram_address_temp_access1);
								}
								else
								{
									this.core.reg2007buffer = palettes_bank[this.core.vram_address_temp_access1 & ((this.core.vram_address_temp_access1 & 0x03) == 0 ? 0x0C : 0x1F)];
								}
							}

							this.core.vram_address = (this.core.vram_address + this.core.vram_increament) & 0x7FFF;
							board.OnPPUAddressUpdate(ref this.core.vram_address);
							return this.core.ppu_2007_temp;
						}
				}
				#endregion
			}
			else if (address < 0x4020)
			{
				#region Internal APU Registers
				switch (address)
				{
					case 0x4015:
						{
							temp_4015 = 0;
							// Channels enable
							if (this.core.pulse1Channel.Duration_counter > 0)
								temp_4015 |= 0x01;
							if (this.core.pulse2Channel.Duration_counter > 0)
								temp_4015 |= 0x02;
							if (this.core.triangleChannel.Duration_counter > 0)
								temp_4015 |= 0x04;
							if (this.core.noiseChannel.Duration_counter > 0)
								temp_4015 |= 0x08;
							if (this.core.dmcChannel.dmc_dmaSize > 0)
								temp_4015 |= 0x10;
							// IRQ
							if (this.core.FrameIrqFlag)
								temp_4015 |= 0x40;
							if (this.core.dmcChannel.DeltaIrqOccur)
								temp_4015 |= 0x80;

							this.core.FrameIrqFlag = false;
							NesEmu.IRQFlags &= ~NesEmu.IRQ_APU;

							return temp_4015;
						}
					case 0x4016:
						{
							temp_4016 = (byte)(this.core.PORT0 & 1);

							this.core.PORT0 >>= 1;

							if (this.core.IsZapperConnected)
								temp_4016 |= this.core.zapper.GetData();

							if (this.core.IsVSUnisystem)
								temp_4016 |= this.core.VSUnisystemDIP.GetData4016();

							return temp_4016;
						}
					case 0x4017:
						{
							temp_4017 = (byte)(this.core.PORT1 & 1);

							this.core.PORT1 >>= 1;

							if (this.core.IsZapperConnected)
								temp_4017 |= this.core.zapper.GetData();
							if (this.core.IsVSUnisystem)
								temp_4017 |= this.core.VSUnisystemDIP.GetData4017();

							return temp_4017;
						}
				}
				#endregion
			}
			else if (address < 0x6000)// Cartridge Expansion Area almost 8K
			{
				return board.ReadEXP(ref address);
			}
			else if (address < 0x8000)// Cartridge SRAM Area 8K
			{
				return board.ReadSRM(ref address);
			}
			else if (address <= 0xFFFF)// Cartridge PRG-ROM Area 32K
			{
				return board.ReadPRG(ref address);
			}
			// Should not reach here !
			return 0;
		}
		public void Write(int address, byte value)
		{
			BUS_RW_P = BUS_RW;
			BUS_ADDRESS = address;
			BUS_RW = false;

			#region Clock Components
			this.core.PPUClock();
			/*
			 * NMI edge detector polls the status of the NMI line during φ2 of each CPU cycle 
			 * (i.e., during the second half of each cycle) 
			 */
			this.core.PollInterruptStatus();
			this.core.PPUClock();
			this.core.PPUClock();
			if (this.core.DoPalAdditionalClock)// In pal system ..
			{
				this.core.palCyc++;
				if (this.core.palCyc == 5)
				{
					this.core.PPUClock();
					this.core.palCyc = 0;
				}
			}
			this.core.APUClock();
			this.dma.DMAClock();

			board.OnCPUClock();
			#endregion

			if (address < 0x2000)// Internal 2K Work RAM (mirrored to 800h-1FFFh)
			{
				WRAM[address & 0x7FF] = value;
			}
			else if (address < 0x4000)
			{
				#region Internal PPU Registers (mirrored to 2008h-3FFFh)
				switch (address & 7)
				{
					case 0:// $2000
						{
							this.core.vram_temp = (this.core.vram_temp & 0x73FF) | ((value & 0x3) << 10);
							this.core.vram_increament = ((value & 0x4) != 0) ? 32 : 1;
							this.core.spr_patternAddress = ((value & 0x8) != 0) ? 0x1000 : 0x0000;
							this.core.bkg_patternAddress = ((value & 0x10) != 0) ? 0x1000 : 0x0000;
							this.core.spr_size16 = (value & 0x20) != 0 ? 0x0010 : 0x0008;

							this.core.nmi_old = this.core.nmi_enabled;
							this.core.nmi_enabled = (value & 0x80) != 0;

							if (!this.core.nmi_enabled)// NMI disable effect only at vbl set period (HClock between 1 and 3)
								this.core.CheckNMI();
							else if (this.core.vbl_flag_temp & !this.core.nmi_old)// Special case ! NMI can be enabled anytime if vbl already set
								this.core.NMI_Current = true;
							break;
						}
					case 1:// $2001
						{
							this.core.grayscale = (value & 0x01) != 0 ? 0x30 : 0x3F;
							this.core.emphasis = (value & 0xE0) << 1;

							this.core.bkg_clipped = (value & 0x02) == 0;
							this.core.spr_clipped = (value & 0x04) == 0;
							this.core.bkg_enabled = (value & 0x08) != 0;
							this.core.spr_enabled = (value & 0x10) != 0;
							break;
						}
					case 3:// $2003
						{
							this.core.oam_address = value;
							break;
						}
					case 4:// $2004
						{
							if (this.core.VClock < 240 && this.core.IsRenderingOn())
								value = 0xFF;
							if ((this.core.oam_address & 0x03) == 0x02)
								value &= 0xE3;
							oam_ram[this.core.oam_address++] = value;
							break;
						}
					case 5:// $2005
						{
							if (!this.core.vram_flipflop)
							{
								this.core.vram_temp = (this.core.vram_temp & 0x7FE0) | ((value & 0xF8) >> 3);
								this.core.vram_fine = (byte)(value & 0x07);
							}
							else
							{
								this.core.vram_temp = (this.core.vram_temp & 0x0C1F) | ((value & 0x7) << 12) | ((value & 0xF8) << 2);
							}
							this.core.vram_flipflop = !this.core.vram_flipflop;
							break;
						}
					case 6:// $2006
						{
							if (!this.core.vram_flipflop)
							{
								this.core.vram_temp = (this.core.vram_temp & 0x00FF) | ((value & 0x3F) << 8);
							}
							else
							{
								this.core.vram_temp = (this.core.vram_temp & 0x7F00) | value;
								this.core.vram_address = this.core.vram_temp;
								board.OnPPUAddressUpdate(ref this.core.vram_address);
							}
							this.core.vram_flipflop = !this.core.vram_flipflop;
							break;
						}
					case 7:// $2007
						{
							this.core.vram_address_temp_access = this.core.vram_address & 0x3FFF;
							if (this.core.vram_address_temp_access < 0x2000)
							{
								board.WriteCHR(ref this.core.vram_address_temp_access, ref value);
							}
							else if (this.core.vram_address_temp_access < 0x3F00)
							{
								board.WriteNMT(ref this.core.vram_address_temp_access, ref value);
							}
							else
							{
								palettes_bank[this.core.vram_address_temp_access & ((this.core.vram_address_temp_access & 0x03) == 0 ? 0x0C : 0x1F)] = value;
							}
							this.core.vram_address = (this.core.vram_address + this.core.vram_increament) & 0x7FFF;
							board.OnPPUAddressUpdate(ref this.core.vram_address);
							break;
						}
				}
				#endregion
			}
			else if (address < 0x4020)
			{
				#region Internal APU Registers
				switch (address)
				{
					/*Pulse 1*/
					case 0x4000:
					case 0x4001:
					case 0x4002:
					case 0x4003:
						{
							this.core.pulse1Channel.WriteByte(address, value);
							break;
						}
					/*Pulse 2*/
					case 0x4004:
					case 0x4005:
					case 0x4006:
					case 0x4007:
						{
							this.core.pulse2Channel.WriteByte(address, value);
							break;
						}
					/*Triangle*/
					case 0x4008:
					case 0x400A:
					case 0x400B:
						{
							this.core.triangleChannel.WriteByte(address, value);
							break;
						}
					/*Noise*/
					case 0x400C:
					case 0x400E:
					case 0x400F:
						{
							this.core.noiseChannel.WriteByte(address, value);
							break;
						}
					/*DMC*/
					case 0x4010:
						{
							this.core.dmcChannel.DMCIrqEnabled = (value & 0x80) != 0;
							this.core.dmcChannel.dmc_dmaLooping = (value & 0x40) != 0;

							if (!this.core.dmcChannel.DMCIrqEnabled)
							{
								this.core.dmcChannel.DeltaIrqOccur = false;
								NesEmu.IRQFlags &= ~NesEmu.IRQ_DMC;
							}
							this.core.dmcChannel.dmc_freqTimer = value & 0x0F;
							break;
						}
					case 0x4011:
						{
							this.core.dmcChannel.Output = (byte)(value & 0x7F);
							break;
						}
					case 0x4012:
						{
							this.core.dmcChannel.dmc_dmaAddrRefresh = (value << 6) | 0xC000;
							break;
						}
					case 0x4013:
						{
							this.core.dmcChannel.dmc_dmaSizeRefresh = (value << 4) | 0x0001;
							break;
						}
					case 0x4014:
						{
							this.dma.dmaOamaddress = value << 8;
							this.dma.AssertOAMDMA();
							break;
						}
					case 0x4015:
						{
							// SQ1
							this.core.pulse1Channel.Duration_reloadEnabled = (value & 0x01) != 0;
							if (!this.core.pulse1Channel.Duration_reloadEnabled)
								this.core.pulse1Channel.Duration_counter = 0;
							// SQ2
							this.core.pulse2Channel.Duration_reloadEnabled = (value & 0x02) != 0;
							if (!this.core.pulse2Channel.Duration_reloadEnabled)
								this.core.pulse2Channel.Duration_counter = 0;
							// TRL
							this.core.triangleChannel.Duration_reloadEnabled = (value & 0x04) != 0;
							if (!this.core.triangleChannel.Duration_reloadEnabled)
								this.core.triangleChannel.Duration_counter = 0;
							// NOZ
							this.core.noiseChannel.Duration_reloadEnabled = (value & 0x08) != 0;
							if (!this.core.noiseChannel.Duration_reloadEnabled)
								this.core.noiseChannel.Duration_counter = 0;
							// DMC
							if ((value & 0x10) != 0)
							{
								if (this.core.dmcChannel.dmc_dmaSize == 0)
								{
									this.core.dmcChannel.dmc_dmaSize = this.core.dmcChannel.dmc_dmaSizeRefresh;
									this.core.dmcChannel.dmc_dmaAddr = this.core.dmcChannel.dmc_dmaAddrRefresh;
								}
							}
							else
							{
								this.core.dmcChannel.dmc_dmaSize = 0;
							}
							// Disable DMC IRQ
							this.core.dmcChannel.DeltaIrqOccur = false;
							NesEmu.IRQFlags &= ~NesEmu.IRQ_DMC;
							// RDY ?
							if (!this.core.dmcChannel.dmc_bufferFull && this.core.dmcChannel.dmc_dmaSize > 0)
							{
								this.dma.AssertDMCDMA();
							}
							break;
						}
					case 0x4016:
						{
							if (this.core.inputStrobe > (value & 0x01))
							{
								if (this.core.IsFourPlayers)
								{
									this.core.PORT0 = this.core.joypad3.GetData() << 8 | this.core.joypad1.GetData() | 0x01010000;
									this.core.PORT1 = this.core.joypad4.GetData() << 8 | this.core.joypad2.GetData() | 0x02020000;
								}
								else
								{
									this.core.PORT0 = this.core.joypad1.GetData() | 0x01010100;// What is this ? see *
									this.core.PORT1 = this.core.joypad2.GetData() | 0x02020200;
								}
							}
							if (this.core.IsVSUnisystem)
								board.VSUnisystem4016RW(ref value);
							this.core.inputStrobe = value & 0x01;
							break;
							// * The data port is 24 bits length
							// Each 8 bits indicates device, if that device is connected, then device data set on it normally...
							// So we have 4 block of data on each register ([] indicate byte block here, let's call these blocks a SEQ)
							// SEQ:
							// [block 3] [block 2] [block 1] [block 0]
							// 0000 0000 0000 0000 0000 0000 0000 0000
							// ^ bit 23                              ^ bit 0
							// Let's say we connect joypad 1 and joypad2, then:
							// In $4016: the data could be like this [00h][00h][00h][joy1]
							// In $4017: the data could be like this [00h][00h][00h][joy2]
							// Instead of having 00h value on other blocks, the read returns a bit set on each unused block
							// to indicate that there's no device (i.e. joypad) is connected :
							// In $4016 the first bit (i.e. bit 0) is set if no device connected on that block
							// Example: [01h][01h][01h][joy1] (we only have joypad 1 connected so other blocks are blocked)
							// In $4017 work the same but with second bit (i.e. bit 1) is set if no device connected on other blocks
							// Example: [02h][02h][02h][joy2] (when we have joypad 2 connected so other blocks are blocked)
							// If we connect 4 joypads then:
							// $4016 : [01h][01h][joy3][joy1]
							// $4017 : [02h][02h][joy4][joy2]
						}
					case 0x4017:
						{
							this.core.SequencingMode = (value & 0x80) != 0;
							this.core.FrameIrqEnabled = (value & 0x40) == 0;

							this.core.CurrentSeq = 0;

							if (!this.core.SequencingMode)
								this.core.Cycles = NesEmu.SequenceMode0[this.core.SystemIndex][0];
							else
								this.core.Cycles = NesEmu.SequenceMode1[this.core.SystemIndex][0];

							if (!this.core.oddCycle)
								this.core.Cycles++;
							else
								this.core.Cycles += 2;

							if (!this.core.FrameIrqEnabled)
							{
								this.core.FrameIrqFlag = false;
								NesEmu.IRQFlags &= ~NesEmu.IRQ_APU;
							}
							break;
						}
				}
				#endregion
			}
			else if (address < 0x6000)// Cartridge Expansion Area almost 8K
			{
				if (this.core.IsVSUnisystem && address == 0x4020)
					this.core.VSUnisystemDIP.Write4020(ref value);
				board.WriteEXP(ref address, ref value);
			}
			else if (address < 0x8000)// Cartridge SRAM Area 8K
			{
				board.WriteSRM(ref address, ref value);
			}
			else if (address <= 0xFFFF)// Cartridge PRG-ROM Area 32K
			{
				board.WritePRG(ref address, ref value);
			}
		}

		internal void LoadState(BinaryReader bin)
		{
			board.LoadState(bin);
			bin.Read(WRAM, 0, this.WRAM.Length);
			bin.Read(palettes_bank, 0, palettes_bank.Length);
			bin.Read(oam_ram, 0, oam_ram.Length);
			bin.Read(oam_secondary, 0, oam_secondary.Length);
			BUS_ADDRESS = bin.ReadInt32();
			BUS_RW = bin.ReadBoolean();
			BUS_RW_P = bin.ReadBoolean();
			temp_4015 = bin.ReadByte();
			temp_4016 = bin.ReadByte();
			temp_4017 = bin.ReadByte();
		}

		internal void SaveState(BinaryWriter bin)
		{
			board.SaveState(bin);
			bin.Write(WRAM);
			bin.Write(palettes_bank);
			bin.Write(oam_ram);
			bin.Write(oam_secondary);
			bin.Write(BUS_ADDRESS);
			bin.Write(BUS_RW);
			bin.Write(BUS_RW_P);
			bin.Write(temp_4015);
			bin.Write(temp_4016);
			bin.Write(temp_4017);
		}
	}
}

