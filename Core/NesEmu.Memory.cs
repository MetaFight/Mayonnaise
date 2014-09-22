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
    public partial class NesEmu
    {
        private static Board board;
        // Internal 2K Work RAM (mirrored to 800h-1FFFh)
        private static byte[] WRAM;
        private static byte[] palettes_bank;
        // Accessed via $2004
        private static byte[] oam_ram;
        // The secondary oam...
        private static byte[] oam_secondary;
        private static int BUS_ADDRESS;
        private static bool BUS_RW;
        private static bool BUS_RW_P;
        // temps
        private static byte temp_4015;
        private static byte temp_4016;
        private static byte temp_4017;

        private static void MEMInitialize(IRom rom)
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
        private static void MEMHardReset()
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
            if (File.Exists(SRAMFileName))
            {
                Stream str = new FileStream(SRAMFileName, FileMode.Open, FileAccess.Read);
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
        private static void MEMSoftReset()
        {
            board.SoftReset();
        }
        private static void MEMShutdown()
        {
            SaveSRAM();
            board = null;
        }
        public static void SaveSRAM()
        {
            if (board != null)
                if (SaveSRAMAtShutdown && board.SRAMSaveRequired)
                {
                    Trace.WriteLine("Saving SRAM ...");
                    byte[] sramBuffer = new byte[0];
                    ZlipWrapper.CompressData(board.GetSRAMBuffer(), out sramBuffer);

                    Stream str = new FileStream(SRAMFileName, FileMode.Create, FileAccess.Write);
                    str.Write(sramBuffer, 0, sramBuffer.Length);

                    str.Flush();
                    str.Close();
                    Trace.WriteLine("SRAM saved successfully.");
                }
        }
        private static byte Read(int address)
        {
            BUS_RW_P = BUS_RW;
            BUS_ADDRESS = address;
            BUS_RW = true;

            #region Clock Components
            PPUClock();
            /*
             * NMI edge detector polls the status of the NMI line during φ2 of each CPU cycle 
             * (i.e., during the second half of each cycle) 
             */
            PollInterruptStatus();
            PPUClock();
            PPUClock();
            if (DoPalAdditionalClock)// In pal system ..
            {
                palCyc++;
                if (palCyc == 5)
                {
                    PPUClock();
                    palCyc = 0;
                }
            }
            APUClock();
            DMAClock();

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
                            ppu_2002_temp = 0;

                            if (vbl_flag)
                                ppu_2002_temp |= 0x80;
                            if (spr_0Hit)
                                ppu_2002_temp |= 0x40;
                            if (spr_overflow)
                                ppu_2002_temp |= 0x20;

                            vbl_flag_temp = false;
                            vram_flipflop = false;

                            CheckNMI();// NMI disable effect only at vbl set period (HClock between 1 and 3)

                            return ppu_2002_temp;
                        }
                    case 4:// $2004
                        {
                            ppu_2004_temp = oam_ram[oam_address];
                            if (VClock < 240 && IsRenderingOn())
                            {
                                if (HClock < 64)
                                    ppu_2004_temp = 0xFF;
                                else if (HClock < 192)
                                    ppu_2004_temp = oam_ram[((HClock - 64) << 1) & 0xFC];
                                else if (HClock < 256)
                                    ppu_2004_temp = ((HClock & 1) == 1) ? oam_ram[0xFC] : oam_ram[((HClock - 192) << 1) & 0xFC];
                                else if (HClock < 320)
                                    ppu_2004_temp = 0xFF;
                                else
                                    ppu_2004_temp = oam_ram[0];
                            }
                            return ppu_2004_temp;
                        }
                    case 7:// $2007
                        {
                            ppu_2007_temp = 0;

                            if ((vram_address & 0x3F00) == 0x3F00)
                            {
                                // The return value should be from the palettes bank
                                ppu_2007_temp = (byte)(palettes_bank[vram_address & ((vram_address & 0x03) == 0 ? 0x0C : 0x1F)] & grayscale);
                                // fill buffer from chr or nametables
                                vram_address_temp_access1 = vram_address & 0x2FFF;
                                if (vram_address_temp_access1 < 0x2000)
                                {
                                    reg2007buffer = board.ReadCHR(ref vram_address_temp_access1, false);
                                }
                                else
                                {
                                    reg2007buffer = board.ReadNMT(ref vram_address_temp_access1);
                                }
                            }
                            else
                            {
                                ppu_2007_temp = reg2007buffer;
                                // fill buffer
                                vram_address_temp_access1 = vram_address & 0x3FFF;
                                if (vram_address_temp_access1 < 0x2000)
                                {
                                    reg2007buffer = board.ReadCHR(ref vram_address_temp_access1, false);
                                }
                                else if (vram_address_temp_access1 < 0x3F00)
                                {
                                    reg2007buffer = board.ReadNMT(ref vram_address_temp_access1);
                                }
                                else
                                {
                                    reg2007buffer = palettes_bank[vram_address_temp_access1 & ((vram_address_temp_access1 & 0x03) == 0 ? 0x0C : 0x1F)];
                                }
                            }

                            vram_address = (vram_address + vram_increament) & 0x7FFF;
                            board.OnPPUAddressUpdate(ref vram_address);
                            return ppu_2007_temp;
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
                            if (sq1_duration_counter > 0) temp_4015 |= 0x01;
                            if (sq2_duration_counter > 0) temp_4015 |= 0x02;
                            if (trl_duration_counter > 0) temp_4015 |= 0x04;
                            if (noz_duration_counter > 0) temp_4015 |= 0x08;
                            if (dmc_dmaSize > 0) temp_4015 |= 0x10;
                            // IRQ
                            if (FrameIrqFlag) temp_4015 |= 0x40;
                            if (DeltaIrqOccur) temp_4015 |= 0x80;

                            FrameIrqFlag = false;
                            IRQFlags &= ~IRQ_APU;

                            return temp_4015;
                        }
                    case 0x4016:
                        {
                            temp_4016 = (byte)(PORT0 & 1);

                            PORT0 >>= 1;

                            if (IsZapperConnected)
                                temp_4016 |= zapper.GetData();

                            if (IsVSUnisystem)
                                temp_4016 |= VSUnisystemDIP.GetData4016();

                            return temp_4016;
                        }
                    case 0x4017:
                        {
                            temp_4017 = (byte)(PORT1 & 1);

                            PORT1 >>= 1;

                            if (IsZapperConnected)
                                temp_4017 |= zapper.GetData();
                            if (IsVSUnisystem)
                                temp_4017 |= VSUnisystemDIP.GetData4017();

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
        private static void Write(int address, byte value)
        {
            BUS_RW_P = BUS_RW;
            BUS_ADDRESS = address;
            BUS_RW = false;

            #region Clock Components
            PPUClock();
            /*
             * NMI edge detector polls the status of the NMI line during φ2 of each CPU cycle 
             * (i.e., during the second half of each cycle) 
             */
            PollInterruptStatus();
            PPUClock();
            PPUClock();
            if (DoPalAdditionalClock)// In pal system ..
            {
                palCyc++;
                if (palCyc == 5)
                {
                    PPUClock();
                    palCyc = 0;
                }
            }
            APUClock();
            DMAClock();

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
                            vram_temp = (vram_temp & 0x73FF) | ((value & 0x3) << 10);
                            vram_increament = ((value & 0x4) != 0) ? 32 : 1;
                            spr_patternAddress = ((value & 0x8) != 0) ? 0x1000 : 0x0000;
                            bkg_patternAddress = ((value & 0x10) != 0) ? 0x1000 : 0x0000;
                            spr_size16 = (value & 0x20) != 0 ? 0x0010 : 0x0008;

                            nmi_old = nmi_enabled;
                            nmi_enabled = (value & 0x80) != 0;

                            if (!nmi_enabled)// NMI disable effect only at vbl set period (HClock between 1 and 3)
                                CheckNMI();
                            else if (vbl_flag_temp & !nmi_old)// Special case ! NMI can be enabled anytime if vbl already set
                                NMI_Current = true;
                            break;
                        }
                    case 1:// $2001
                        {
                            grayscale = (value & 0x01) != 0 ? 0x30 : 0x3F;
                            emphasis = (value & 0xE0) << 1;

                            bkg_clipped = (value & 0x02) == 0;
                            spr_clipped = (value & 0x04) == 0;
                            bkg_enabled = (value & 0x08) != 0;
                            spr_enabled = (value & 0x10) != 0;
                            break;
                        }
                    case 3:// $2003
                        {
                            oam_address = value;
                            break;
                        }
                    case 4:// $2004
                        {
                            if (VClock < 240 && IsRenderingOn())
                                value = 0xFF;
                            if ((oam_address & 0x03) == 0x02)
                                value &= 0xE3;
                            oam_ram[oam_address++] = value;
                            break;
                        }
                    case 5:// $2005
                        {
                            if (!vram_flipflop)
                            {
                                vram_temp = (vram_temp & 0x7FE0) | ((value & 0xF8) >> 3);
                                vram_fine = (byte)(value & 0x07);
                            }
                            else
                            {
                                vram_temp = (vram_temp & 0x0C1F) | ((value & 0x7) << 12) | ((value & 0xF8) << 2);
                            }
                            vram_flipflop = !vram_flipflop;
                            break;
                        }
                    case 6:// $2006
                        {
                            if (!vram_flipflop)
                            {
                                vram_temp = (vram_temp & 0x00FF) | ((value & 0x3F) << 8);
                            }
                            else
                            {
                                vram_temp = (vram_temp & 0x7F00) | value;
                                vram_address = vram_temp;
                                board.OnPPUAddressUpdate(ref vram_address);
                            }
                            vram_flipflop = !vram_flipflop;
                            break;
                        }
                    case 7:// $2007
                        {
                            vram_address_temp_access = vram_address & 0x3FFF;
                            if (vram_address_temp_access < 0x2000)
                            {
                                board.WriteCHR(ref vram_address_temp_access, ref value);
                            }
                            else if (vram_address_temp_access < 0x3F00)
                            {
                                board.WriteNMT(ref vram_address_temp_access, ref value);
                            }
                            else
                            {
                                palettes_bank[vram_address_temp_access & ((vram_address_temp_access & 0x03) == 0 ? 0x0C : 0x1F)] = value;
                            }
                            vram_address = (vram_address + vram_increament) & 0x7FFF;
                            board.OnPPUAddressUpdate(ref vram_address);
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
                        {
                            sq1_volume_decay_time = value & 0xF;
                            sq1_duration_haltRequset = (value & 0x20) != 0;
                            sq1_constant_volume_flag = (value & 0x10) != 0;
                            sq1_envelope = sq1_constant_volume_flag ? sq1_volume_decay_time : sq1_env_counter;
                            sq1_dutyForm = (value & 0xC0) >> 6;
                            break;
                        }
                    case 0x4001:
                        {
                            sq1_sweepEnable = (value & 0x80) == 0x80;
                            sq1_sweepDeviderPeriod = (value >> 4) & 7;
                            sq1_sweepNegateFlag = (value & 0x8) == 0x8;
                            sq1_sweepShiftCount = value & 7;
                            sq1_sweepReload = true;
                            break;
                        }
                    case 0x4002:
                        {
                            sq1_frequency = (sq1_frequency & 0x0700) | value;
                            break;
                        }
                    case 0x4003:
                        {
                            sq1_duration_reload = DurationTable[value >> 3];
                            sq1_duration_reloadRequst = true;
                            sq1_frequency = (sq1_frequency & 0x00FF) | ((value & 7) << 8);
                            sq1_dutyStep = 0;
                            sq1_env_startflag = true;
                            break;
                        }
                    /*Pulse 2*/
                    case 0x4004:
                        {
                            sq2_volume_decay_time = value & 0xF;
                            sq2_duration_haltRequset = (value & 0x20) != 0;
                            sq2_constant_volume_flag = (value & 0x10) != 0;
                            sq2_envelope = sq2_constant_volume_flag ? sq2_volume_decay_time : sq2_env_counter;
                            sq2_dutyForm = (value & 0xC0) >> 6;
                            break;
                        }
                    case 0x4005:
                        {
                            sq2_sweepEnable = (value & 0x80) == 0x80;
                            sq2_sweepDeviderPeriod = (value >> 4) & 7;
                            sq2_sweepNegateFlag = (value & 0x8) == 0x8;
                            sq2_sweepShiftCount = value & 7;
                            sq2_sweepReload = true;
                            break;
                        }
                    case 0x4006:
                        {
                            sq2_frequency = (sq2_frequency & 0x0700) | value;
                            break;
                        }
                    case 0x4007:
                        {
                            sq2_duration_reload = DurationTable[value >> 3];
                            sq2_duration_reloadRequst = true;
                            sq2_frequency = (sq2_frequency & 0x00FF) | ((value & 7) << 8);
                            sq2_dutyStep = 0;
                            sq2_env_startflag = true;
                            break;
                        }
                    /*Triangle*/
                    case 0x4008:
                        {
                            trl_linearCounterHalt = trl_duration_haltRequset = (value & 0x80) != 0;
                            trl_linearCounterReload = (byte)(value & 0x7F);
                            break;
                        }
                    case 0x400A:
                        {
                            trl_frequency = (trl_frequency & 0x700) | value;
                            break;
                        }
                    case 0x400B:
                        {
                            trl_frequency = (trl_frequency & 0x00FF) | ((value & 7) << 8);

                            trl_duration_reload = DurationTable[value >> 3];
                            trl_duration_reloadRequst = true;
                            trl_halt = true;
                            break;
                        }
                    /*Noise*/
                    case 0x400C:
                        {
                            noz_volume_decay_time = value & 0xF;
                            noz_duration_haltRequset = (value & 0x20) != 0;
                            noz_constant_volume_flag = (value & 0x10) != 0;
                            noz_envelope = noz_constant_volume_flag ? noz_volume_decay_time : noz_env_counter;
                            break;
                        }
                    case 0x400E:
                        {
                            noz_freqTimer = value & 0x0F;
                            noz_modeFlag = (value & 0x80) == 0x80;
                            break;
                        }
                    case 0x400F:
                        {
                            noz_duration_reload = DurationTable[value >> 3];
                            noz_duration_reloadRequst = true;
                            noz_env_startflag = true;
                            break;
                        }
                    /*DMC*/
                    case 0x4010:
                        {
                            DMCIrqEnabled = (value & 0x80) != 0;
                            dmc_dmaLooping = (value & 0x40) != 0;

                            if (!DMCIrqEnabled)
                            {
                                DeltaIrqOccur = false;
                                IRQFlags &= ~IRQ_DMC;
                            }
                            dmc_freqTimer = value & 0x0F;
                            break;
                        }
                    case 0x4011: { dmc_output = (byte)(value & 0x7F); break; }
                    case 0x4012: { dmc_dmaAddrRefresh = (value << 6) | 0xC000; break; }
                    case 0x4013: { dmc_dmaSizeRefresh = (value << 4) | 0x0001; break; }
                    case 0x4014:
                        {
                            dmaOamaddress = value << 8;
                            AssertOAMDMA();
                            break;
                        }
                    case 0x4015:
                        {
                            // SQ1
                            sq1_duration_reloadEnabled = (value & 0x01) != 0;
                            if (!sq1_duration_reloadEnabled)
                                sq1_duration_counter = 0;
                            // SQ2
                            sq2_duration_reloadEnabled = (value & 0x02) != 0;
                            if (!sq2_duration_reloadEnabled)
                                sq2_duration_counter = 0;
                            // TRL
                            trl_duration_reloadEnabled = (value & 0x04) != 0;
                            if (!trl_duration_reloadEnabled)
                                trl_duration_counter = 0;
                            // NOZ
                            noz_duration_reloadEnabled = (value & 0x08) != 0;
                            if (!noz_duration_reloadEnabled)
                                noz_duration_counter = 0;
                            // DMC
                            if ((value & 0x10) != 0)
                            {
                                if (dmc_dmaSize == 0)
                                {
                                    dmc_dmaSize = dmc_dmaSizeRefresh;
                                    dmc_dmaAddr = dmc_dmaAddrRefresh;
                                }
                            }
                            else { dmc_dmaSize = 0; }
                            // Disable DMC IRQ
                            DeltaIrqOccur = false;
                            IRQFlags &= ~IRQ_DMC;
                            // RDY ?
                            if (!dmc_bufferFull && dmc_dmaSize > 0)
                            {
                                AssertDMCDMA();
                            }
                            break;
                        }
                    case 0x4016:
                        {
                            if (inputStrobe > (value & 0x01))
                            {
                                if (IsFourPlayers)
                                {
                                    PORT0 = joypad3.GetData() << 8 | joypad1.GetData() | 0x01010000;
                                    PORT1 = joypad4.GetData() << 8 | joypad2.GetData() | 0x02020000;
                                }
                                else
                                {
                                    PORT0 = joypad1.GetData() | 0x01010100;// What is this ? see *
                                    PORT1 = joypad2.GetData() | 0x02020200;
                                }
                            }
                            if (IsVSUnisystem)
                                board.VSUnisystem4016RW(ref value);
                            inputStrobe = value & 0x01;
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
                            SequencingMode = (value & 0x80) != 0;
                            FrameIrqEnabled = (value & 0x40) == 0;

                            CurrentSeq = 0;

                            if (!SequencingMode)
                                Cycles = SequenceMode0[systemIndex][0];
                            else
                                Cycles = SequenceMode1[systemIndex][0];

                            if (!oddCycle)
                                Cycles++;
                            else
                                Cycles += 2;

                            if (!FrameIrqEnabled)
                            {
                                FrameIrqFlag = false;
                                IRQFlags &= ~IRQ_APU;
                            }
                            break;
                        }
                }
                #endregion
            }
            else if (address < 0x6000)// Cartridge Expansion Area almost 8K
            {
                if (IsVSUnisystem && address == 0x4020)
                    VSUnisystemDIP.Write4020(ref value);
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
    }
}

