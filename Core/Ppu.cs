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

/*PPU section*/
namespace MyNes.Core
{
	public class Ppu
	{
		private readonly static byte[] reverseLookup =
            {
                0x00, 0x80, 0x40, 0xC0, 0x20, 0xA0, 0x60, 0xE0, 0x10, 0x90, 0x50, 0xD0, 0x30, 0xB0, 0x70, 0xF0,
                0x08, 0x88, 0x48, 0xC8, 0x28, 0xA8, 0x68, 0xE8, 0x18, 0x98, 0x58, 0xD8, 0x38, 0xB8, 0x78, 0xF8,
                0x04, 0x84, 0x44, 0xC4, 0x24, 0xA4, 0x64, 0xE4, 0x14, 0x94, 0x54, 0xD4, 0x34, 0xB4, 0x74, 0xF4,
                0x0C, 0x8C, 0x4C, 0xCC, 0x2C, 0xAC, 0x6C, 0xEC, 0x1C, 0x9C, 0x5C, 0xDC, 0x3C, 0xBC, 0x7C, 0xFC,
                0x02, 0x82, 0x42, 0xC2, 0x22, 0xA2, 0x62, 0xE2, 0x12, 0x92, 0x52, 0xD2, 0x32, 0xB2, 0x72, 0xF2,
                0x0A, 0x8A, 0x4A, 0xCA, 0x2A, 0xAA, 0x6A, 0xEA, 0x1A, 0x9A, 0x5A, 0xDA, 0x3A, 0xBA, 0x7A, 0xFA,
                0x06, 0x86, 0x46, 0xC6, 0x26, 0xA6, 0x66, 0xE6, 0x16, 0x96, 0x56, 0xD6, 0x36, 0xB6, 0x76, 0xF6,
                0x0E, 0x8E, 0x4E, 0xCE, 0x2E, 0xAE, 0x6E, 0xEE, 0x1E, 0x9E, 0x5E, 0xDE, 0x3E, 0xBE, 0x7E, 0xFE,
                0x01, 0x81, 0x41, 0xC1, 0x21, 0xA1, 0x61, 0xE1, 0x11, 0x91, 0x51, 0xD1, 0x31, 0xB1, 0x71, 0xF1,
                0x09, 0x89, 0x49, 0xC9, 0x29, 0xA9, 0x69, 0xE9, 0x19, 0x99, 0x59, 0xD9, 0x39, 0xB9, 0x79, 0xF9,
                0x05, 0x85, 0x45, 0xC5, 0x25, 0xA5, 0x65, 0xE5, 0x15, 0x95, 0x55, 0xD5, 0x35, 0xB5, 0x75, 0xF5,
                0x0D, 0x8D, 0x4D, 0xCD, 0x2D, 0xAD, 0x6D, 0xED, 0x1D, 0x9D, 0x5D, 0xDD, 0x3D, 0xBD, 0x7D, 0xFD,
                0x03, 0x83, 0x43, 0xC3, 0x23, 0xA3, 0x63, 0xE3, 0x13, 0x93, 0x53, 0xD3, 0x33, 0xB3, 0x73, 0xF3,
                0x0B, 0x8B, 0x4B, 0xCB, 0x2B, 0xAB, 0x6B, 0xEB, 0x1B, 0x9B, 0x5B, 0xDB, 0x3B, 0xBB, 0x7B, 0xFB,
                0x07, 0x87, 0x47, 0xC7, 0x27, 0xA7, 0x67, 0xE7, 0x17, 0x97, 0x57, 0xD7, 0x37, 0xB7, 0x77, 0xF7,
                0x0F, 0x8F, 0x4F, 0xCF, 0x2F, 0xAF, 0x6F, 0xEF, 0x1F, 0x9F, 0x5F, 0xDF, 0x3F, 0xBF, 0x7F, 0xFF,
            };
		public int[] paletteIndexes;
		[Obsolete("This shouldn't be part of the PPU.")]
		public IVideoProvider videoOut;
		private int[] screen = new int[256 * 240];
		private int[] palette;
		private int[] bkg_pixels;
		private int[] spr_pixels;
		// Frame Timing
		public int vbl_vclock_Start = 241;
		private int vbl_vclock_End = 261;
		private int frameEnd = 262;
		public int VClock;
		public int HClock;
		private bool UseOddFrame = true;
		private bool oddSwap;
		// Frame skip
		private bool FrameSkipEnabled = false;
		private byte FrameSkipTimer;
		private byte FrameSkipReload = 2;
		private int current_pixel;
		private int temp;
		private int temp_comparator;
		private int bkg_pos;
		private int spr_pos;
		private int object0;
		private int infront;
		private int bkgPixel;
		private int sprPixel;
		private int bkg_fetch_address;
		private byte bkg_fetch_nametable;
		private byte bkg_fetch_attr;
		private byte bkg_fetch_bit0;
		private byte bkg_fetch_bit1;
		private int spr_fetch_address;
		private byte spr_fetch_bit0;
		private byte spr_fetch_bit1;
		private byte spr_fetch_attr;
		private bool[] spr_zero_buffer;
		// VRAM Address register
		public int vram_temp;
		public int vram_address;
		public int vram_address_temp_access;
		public int vram_address_temp_access1;
		public int vram_increament;
		public bool vram_flipflop;
		public byte vram_fine;
		public byte reg2007buffer;
		// Background
		public bool bkg_enabled;
		public bool bkg_clipped;
		public int bkg_patternAddress;
		// Sprites
		public bool spr_enabled;
		public bool spr_clipped;
		public int spr_patternAddress;
		public int spr_size16;
		public bool spr_0Hit;
		public bool spr_overflow;
		// Colors
		public int grayscale;
		public int emphasis;
		// Others
		public byte ppu_2002_temp;
		public byte ppu_2004_temp;
		public byte ppu_2007_temp;
		// OAM
		public byte oam_address;
		private byte oam_fetch_data;
		private byte oam_evaluate_slot;
		private byte oam_evaluate_count;
		private bool oam_fetch_mode = false;
		private byte oam_phase_index = 0;
		private int spr_render_i;
		private int bkg_render_i;
		private int spr_evaluation_i;
		private int spr_render_temp_pixel;
		
		private readonly NesEmu core;
		[Obsolete("Mega-hack until I can figure out which bits of PPU and Memory code need to switch classes.")]
		public Memory memory;

		public Ppu(NesEmu core)
		{
			this.core = core;
		}

		public bool IsRenderingOn()
		{
			return (bkg_enabled || spr_enabled);
		}
		public bool IsInRender()
		{
			return (VClock < 240) || (VClock == vbl_vclock_End);
		}
		public void PPUHardReset()
		{
			switch (this.core.TVFormat)
			{
				case TVSystem.NTSC:
					{
						vbl_vclock_Start = 241;//20 scanlines for VBL
						vbl_vclock_End = 261;
						frameEnd = 262;
						UseOddFrame = true;
						break;
					}
				case TVSystem.PALB:
					{
						vbl_vclock_Start = 241;//70 scanlines for VBL
						vbl_vclock_End = 311;
						frameEnd = 312;
						UseOddFrame = false;
						break;
					}
				case TVSystem.DENDY:
					{
						vbl_vclock_Start = 291;//51 dummy scanlines, 20 VBL's
						vbl_vclock_End = 311;
						frameEnd = 312;
						UseOddFrame = false;
						break;
					}
			}
			spr_zero_buffer = new bool[8];
			bkg_pixels = new int[272];
			spr_pixels = new int[256];
			grayscale = 0xF3;
			vram_flipflop = false;
			emphasis = 0;
			HClock = 0;
			VClock = 0;
			// Reset evaluation
			oam_fetch_mode = false;
			oam_phase_index = 0;
			oam_evaluate_slot = 0;
			oam_address = 0;
			oam_evaluate_count = 0;
			oam_fetch_data = 0;

			//spr_pixels = new int[256];
			for (int i = 0; i < 256; i++)
				spr_pixels[i] = 0;

			current_pixel = 0;
			temp = 0;
			temp_comparator = 0;
			bkg_pos = 0;
			spr_pos = 0;
			object0 = 0;
			infront = 0;
			bkgPixel = 0;
			sprPixel = 0;
			bkg_fetch_address = 0;
			bkg_fetch_nametable = 0;
			bkg_fetch_attr = 0;
			bkg_fetch_bit0 = 0;
			bkg_fetch_bit1 = 0;
			spr_fetch_address = 0;
			spr_fetch_bit0 = 0;
			spr_fetch_bit1 = 0;
			spr_fetch_attr = 0;
			// VRAM Address register
			vram_temp = 0;
			vram_address = 0;
			vram_address_temp_access = 0;
			vram_address_temp_access1 = 0;
			vram_increament = 1;
			vram_flipflop = false;
			vram_fine = 0;
			reg2007buffer = 0;
			// Background
			bkg_enabled = false;
			bkg_clipped = false;
			bkg_patternAddress = 0;
			// Sprites
			spr_enabled = false;
			spr_clipped = false;
			spr_patternAddress = 0;
			spr_size16 = 0;
			spr_0Hit = false;
			spr_overflow = false;
			// Others
			ppu_2002_temp = 0;
			ppu_2004_temp = 0;
			ppu_2007_temp = 0;
			// OAM

			spr_render_i = 0;
			bkg_render_i = 0;
			spr_evaluation_i = 0;
			spr_render_temp_pixel = 0;
		}
		public void PPUSoftReset()
		{

		}
		public void Shutdown()
		{

		}

		[Obsolete("This will need to be redesigned.")]
		public void Clock()
		{
			this.memory.board.OnPPUClock();
			if ((VClock < 240) || (VClock == vbl_vclock_End))
			{
				if (bkg_enabled || spr_enabled)
				{
					if (HClock < 256)
					{
						#region BKG FETCHES 0 - 255
						// UNUSED AT 248-255
						switch (HClock & 7)
						{
							case 0:
								{
									// Fetch address of nametable
									bkg_fetch_address = 0x2000 | (vram_address & 0x0FFF);
									this.memory.board.OnPPUAddressUpdate(ref bkg_fetch_address);
									break;
								}
							case 1:
								bkg_fetch_nametable = this.memory.board.ReadNMT(ref bkg_fetch_address);
								break;
							case 2:
								{
									// Fetch address for attr byte
									bkg_fetch_address = 0x23C0 | (vram_address & 0xC00) | (vram_address >> 4 & 0x38) | (vram_address >> 2 & 0x7);
									this.memory.board.OnPPUAddressUpdate(ref bkg_fetch_address);
									break;
								}
							case 3:
								bkg_fetch_attr = (byte)(this.memory.board.ReadNMT(ref bkg_fetch_address) >> ((vram_address >> 4 & 0x04) | (vram_address & 0x02)));
								break;
							case 4:
								{
									// Fetch bit 0 address
									bkg_fetch_address = bkg_patternAddress | (bkg_fetch_nametable << 4) | 0 | (vram_address >> 12 & 7);
									this.memory.board.OnPPUAddressUpdate(ref bkg_fetch_address);
									break;
								}
							case 5:
								bkg_fetch_bit0 = this.memory.board.ReadCHR(ref bkg_fetch_address, false);
								break;
							case 6:
								{
									// Fetch bit 1 address
									bkg_fetch_address = bkg_patternAddress | (bkg_fetch_nametable << 4) | 8 | (vram_address >> 12 & 7);
									this.memory.board.OnPPUAddressUpdate(ref bkg_fetch_address);
									break;
								}
							case 7:
								{
									bkg_fetch_bit1 = this.memory.board.ReadCHR(ref bkg_fetch_address, false);
									if (HClock == 255)
									{
										// Increment Y
										if ((vram_address & 0x7000) != 0x7000)
										{
											vram_address += 0x1000;
										}
										else
										{
											vram_address ^= 0x7000;

											switch (vram_address & 0x3E0)
											{
												case 0x3A0:
													vram_address ^= 0xBA0;
													break;
												case 0x3E0:
													vram_address ^= 0x3E0;
													break;
												default:
													vram_address += 0x20;
													break;
											}
										}
									}
									else
									{
										// Increment X
										if ((vram_address & 0x001F) == 0x001F)
											vram_address ^= 0x041F;
										else
											vram_address++;
									}
									// Render BKG tile
									bkg_pos = (HClock + 9) % 336;

									for (bkg_render_i = 0; bkg_render_i < 8 && bkg_pos < 272; bkg_render_i++, bkg_pos++, bkg_fetch_bit0 <<= 1, bkg_fetch_bit1 <<= 1)
										bkg_pixels[bkg_pos] = (bkg_fetch_attr << 2 & 12) | (bkg_fetch_bit0 >> 7 & 1) | (bkg_fetch_bit1 >> 6 & 2);

									break;
								}
						}
						#endregion
						#region Render Pixel
						if (VClock < 240)
						{
							if (!bkg_enabled || (bkg_clipped && HClock < 8))
								bkgPixel = 0x3F00;
							else
								bkgPixel = 0x3F00 | bkg_pixels[HClock + vram_fine];
							if (!spr_enabled || (spr_clipped && HClock < 8))
								sprPixel = 0x3F10;
							else
								sprPixel = 0x3F10 | spr_pixels[HClock];

							current_pixel = 0;

							//Priority *******************************
							if ((sprPixel & 0x8000) != 0)
								current_pixel = sprPixel;
							else
								current_pixel = bkgPixel;
							//****************************************

							// Transparency **************************
							if ((bkgPixel & 0x03) == 0)
							{
								current_pixel = sprPixel;
								goto render;
							}

							if ((sprPixel & 0x03) == 0)
							{
								current_pixel = bkgPixel;
								goto render;
							}
							//****************************************

							//Sprite 0 Hit
							if ((sprPixel & 0x4000) != 0 & HClock < 255)
								spr_0Hit = true;

						render:
							screen[(VClock * 256) + HClock] = palette[paletteIndexes[this.memory.palettes_bank[current_pixel & ((current_pixel & 0x03) == 0 ? 0x0C : 0x1F)]
								& (grayscale | emphasis)]];
						}
						#endregion
						#region OAM EVALUATION
						switch (HClock & 1)
						{
							case 0:
								{
									if (!oam_fetch_mode)
										oam_fetch_data = 0xFF;
									else
										oam_fetch_data = this.memory.oam_ram[oam_address];
									break;
								}
							case 1:
								{
									switch (oam_phase_index)
									{
										case 0:
											{
												if (HClock <= 64)
												{
													switch (HClock >> 1 & 0x03)
													{
														case 0:
															this.memory.oam_secondary[((HClock >> 3) * 4) + 0] = 0xFF;
															break;
														case 1:
															this.memory.oam_secondary[((HClock >> 3) * 4) + 1] = 0xFF;
															break;
														case 2:
															this.memory.oam_secondary[((HClock >> 3) * 4) + 2] = 0xFF;
															break;
														case 3:
															this.memory.oam_secondary[((HClock >> 3) * 4) + 3] = 0xFF;
															spr_zero_buffer[HClock >> 3 & 7] = false;
															break;
													}
												}
												break;
											}
										case 1:
											{
												if (VClock == vbl_vclock_End)
													break;
												oam_evaluate_count++;
												temp_comparator = (VClock - oam_fetch_data) & int.MaxValue;

												if (temp_comparator >= spr_size16)
												{
													if (oam_evaluate_count != 64)
													{
														oam_address = (byte)(oam_evaluate_count != 2 ? oam_address + 4 : 8);
													}
													else
													{
														oam_address = 0;
														oam_phase_index = 9;
													}
												}
												else
												{
													oam_address++;
													oam_phase_index = 2;
													this.memory.oam_secondary[oam_evaluate_slot * 4] = oam_fetch_data;

													spr_zero_buffer[oam_evaluate_slot] = (oam_evaluate_count == 1);
												}
												break;
											}
										case 2:
											{
												oam_address++;
												oam_phase_index = 3;
												this.memory.oam_secondary[(oam_evaluate_slot * 4) + 1] = oam_fetch_data;
												break;
											}
										case 3:
											{
												oam_address++;
												oam_phase_index = 4;
												this.memory.oam_secondary[(oam_evaluate_slot * 4) + 2] = oam_fetch_data;
												break;
											}
										case 4:
											{
												this.memory.oam_secondary[(oam_evaluate_slot * 4) + 3] = oam_fetch_data;
												oam_evaluate_slot++;

												if (oam_evaluate_count != 64)
												{
													oam_phase_index = (byte)((oam_evaluate_slot != 8) ? 1 : 5);
													if (oam_evaluate_count != 2)
													{
														oam_address++;
													}
													else
													{
														oam_address = 8;
													}
												}
												else
												{
													oam_address = 0;
													oam_phase_index = 9;
												}
												break;
											}
										case 5:
											{
												if (VClock == vbl_vclock_End)
													break;
												temp_comparator = (VClock - oam_fetch_data) & int.MaxValue;

												if (temp_comparator >= spr_size16)
												{
													oam_address = (byte)(((oam_address + 4) & 0xFC) +
														((oam_address + 1) & 0x03));

													if (oam_address <= 5)
													{
														oam_phase_index = 9;
														oam_address &= 0xFC;
													}
												}
												else
												{
													oam_phase_index = 6;
													oam_address += (0x01) & 0xFF;
													spr_overflow = true;
												}
												break;
											}
										case 6:
											{
												oam_phase_index = 7;
												oam_address += (0x01);
												break;
											}
										case 7:
											{
												oam_phase_index = 8;
												oam_address += (0x01);
												break;
											}
										case 8:
											{
												oam_phase_index = 9;
												oam_address += (0x01);

												if ((oam_address & 0x03) == 0x03)
													oam_address += (0x01);

												oam_address &= 0xFC;
												break;
											}
										case 9:
											{
												oam_address += 0x4;
												break;
											}
									}
									break;
								}
						}
						if (HClock == 63)
						{//Evaluation Begin
							oam_fetch_mode = true;
							oam_phase_index = 1;
							oam_evaluate_slot = 0;
							oam_evaluate_count = 0;
						}

						if (HClock == 255)
						{
							// Evaluation Reset
							oam_fetch_mode = false;
							oam_phase_index = 0;
							oam_evaluate_slot = 0;
							oam_address = 0;
							oam_evaluate_count = 0;

							//spr_pixels = new int[256];
							for (spr_evaluation_i = 0; spr_evaluation_i < 256; spr_evaluation_i++)
								spr_pixels[spr_evaluation_i] = 0;
						}
						#endregion
					}
					else if (HClock < 320)
					{
						#region SPRITE FETCHES + GARBAGE BKG FETCHES 256 - 319
						switch (HClock & 7)
						{
							case 0:
								{
									// Fetch address of nametable
									bkg_fetch_address = 0x2000 | (vram_address & 0x0FFF);
									this.memory.board.OnPPUAddressUpdate(ref bkg_fetch_address);
									break;
								}
							case 1:
								bkg_fetch_nametable = this.memory.board.ReadNMT(ref bkg_fetch_address);
								break;
							case 2:
								{
									// Fetch address for attr byte
									bkg_fetch_address = 0x23C0 | (vram_address & 0xC00) | (vram_address >> 4 & 0x38) | (vram_address >> 2 & 0x7);
									this.memory.board.OnPPUAddressUpdate(ref bkg_fetch_address);
									break;
								}
							case 3:
								bkg_fetch_attr = (byte)(this.memory.board.ReadNMT(ref bkg_fetch_address) >> ((vram_address >> 4 & 0x04) | (vram_address & 0x02)));
								break;
							case 4:
								{
									temp = HClock >> 3 & 7;
									temp_comparator = (VClock - this.memory.oam_secondary[temp * 4]) ^ ((this.memory.oam_secondary[(temp * 4) + 2] & 0x80) != 0 ? 0x0F : 0x00);
									if (spr_size16 == 0x10)
									{
										spr_fetch_address = (this.memory.oam_secondary[(temp * 4) + 1] << 0x0C & 0x1000) | (this.memory.oam_secondary[(temp * 4) + 1] << 0x04 & 0x0FE0) |
													 (temp_comparator << 0x01 & 0x0010) | (temp_comparator & 0x0007);
									}
									else
									{
										spr_fetch_address = spr_patternAddress | (this.memory.oam_secondary[(temp * 4) + 1] << 0x04) | (temp_comparator & 0x0007);
									}
									this.memory.board.OnPPUAddressUpdate(ref spr_fetch_address);
									break;
								}
							case 5:
								{
									spr_fetch_bit0 = this.memory.board.ReadCHR(ref spr_fetch_address, true);
									if ((this.memory.oam_secondary[((HClock >> 3 & 7) * 4) + 2] & 0x40) != 0)
										spr_fetch_bit0 = reverseLookup[spr_fetch_bit0];
									break;
								}
							case 6:
								{
									spr_fetch_address = spr_fetch_address | 0x08;
									this.memory.board.OnPPUAddressUpdate(ref spr_fetch_address);
									break;
								}
							case 7:
								{
									spr_fetch_bit1 = this.memory.board.ReadCHR(ref spr_fetch_address, true);
									if ((this.memory.oam_secondary[((HClock >> 3 & 7) * 4) + 2] & 0x40) != 0)
										spr_fetch_bit1 = reverseLookup[spr_fetch_bit1];

									spr_fetch_attr = this.memory.oam_secondary[((HClock >> 3 & 7) * 4) + 2];

									// Render SPR tile
									temp = HClock >> 3 & 7;

									if (this.memory.oam_secondary[(temp * 4) + 3] == 255)
										break;

									spr_pos = this.memory.oam_secondary[(temp * 4) + 3];
									object0 = spr_zero_buffer[temp] ? 0x4000 : 0x0000;
									infront = ((this.memory.oam_secondary[(temp * 4) + 2] & 0x20) == 0) ? 0x8000 : 0x0000;
									for (spr_render_i = 0; spr_render_i < 8 && spr_pos < 256; spr_render_i++, spr_pos++, spr_fetch_bit0 <<= 1, spr_fetch_bit1 <<= 1)
									{
										if (spr_pos > 255)
											break;

										spr_render_temp_pixel = (spr_fetch_attr << 2 & 12) | (spr_fetch_bit0 >> 7 & 1) | (spr_fetch_bit1 >> 6 & 2) | object0 | infront;
										if ((spr_pixels[spr_pos] & 0x03) == 0 && (spr_render_temp_pixel & 0x03) != 0)
											spr_pixels[spr_pos] = spr_render_temp_pixel;
									}
									break;
								}
						}
						#endregion
						if (HClock == 256)
							vram_address = (vram_address & 0x7BE0) | (vram_temp & 0x041F);

						if (VClock == vbl_vclock_End && HClock >= 279 && HClock <= 303)
							vram_address = (vram_address & 0x041F) | (vram_temp & 0x7BE0);
					}
					else if (HClock < 336)
					{
						#region FIRST 2 BKG TILES FOR 1ST SCANLINE 320 - 335
						switch (HClock & 7)
						{
							case 0:
								{
									// Fetch address of nametable
									bkg_fetch_address = 0x2000 | (vram_address & 0x0FFF);
									this.memory.board.OnPPUAddressUpdate(ref bkg_fetch_address);
									break;
								}
							case 1:
								bkg_fetch_nametable = this.memory.board.ReadNMT(ref bkg_fetch_address);
								break;
							case 2:
								{
									// Fetch address for attr byte
									bkg_fetch_address = 0x23C0 | (vram_address & 0xC00) | (vram_address >> 4 & 0x38) | (vram_address >> 2 & 0x7);
									this.memory.board.OnPPUAddressUpdate(ref bkg_fetch_address);
									break;
								}
							case 3:
								bkg_fetch_attr = (byte)(this.memory.board.ReadNMT(ref bkg_fetch_address) >> ((vram_address >> 4 & 0x04) | (vram_address & 0x02)));
								break;
							case 4:
								{
									// Fetch bit 0 address
									bkg_fetch_address = bkg_patternAddress | (bkg_fetch_nametable << 4) | 0 | (vram_address >> 12 & 7);
									this.memory.board.OnPPUAddressUpdate(ref bkg_fetch_address);
									break;
								}
							case 5:
								bkg_fetch_bit0 = this.memory.board.ReadCHR(ref bkg_fetch_address, false);
								break;
							case 6:
								{
									// Fetch bit 1 address
									bkg_fetch_address = bkg_patternAddress | (bkg_fetch_nametable << 4) | 8 | (vram_address >> 12 & 7);
									this.memory.board.OnPPUAddressUpdate(ref bkg_fetch_address);
									break;
								}
							case 7:
								{
									bkg_fetch_bit1 = this.memory.board.ReadCHR(ref bkg_fetch_address, false);

									// Increment X
									if ((vram_address & 0x001F) == 0x001F)
										vram_address ^= 0x041F;
									else
										vram_address++;


									// Render BKG tile
									bkg_pos = (HClock + 9) % 336;

									for (bkg_render_i = 0; bkg_render_i < 8 && bkg_pos < 272; bkg_render_i++, bkg_pos++, bkg_fetch_bit0 <<= 1, bkg_fetch_bit1 <<= 1)
										bkg_pixels[bkg_pos] = (bkg_fetch_attr << 2 & 12) | (bkg_fetch_bit0 >> 7 & 1) | (bkg_fetch_bit1 >> 6 & 2);

									break;
								}
						}
						#endregion
					}
					else if (HClock < 340)
					{
						#region DUMMY FETCHES 336 - 339
						switch (HClock & 7)
						{
							case 0:
								{
									// Fetch address of nametable
									bkg_fetch_address = 0x2000 | (vram_address & 0x0FFF);
									this.memory.board.OnPPUAddressUpdate(ref bkg_fetch_address);
									break;
								}
							case 1:
								{
									bkg_fetch_nametable = this.memory.board.ReadNMT(ref bkg_fetch_address);
									break;
								}
						}
						#endregion
					}
					else
					{
						// Idle cycle
					}
				}
				else
				{
					#region Rendering is off, draw color at vram address if it in range 0x3F00 - 0x3FFF
					if (HClock < 255 & VClock < 240)
					{
						if ((vram_address & 0x3F00) == 0x3F00)
							screen[(VClock * 256) + HClock] = palette[paletteIndexes[this.memory.palettes_bank[vram_address & ((vram_address & 0x03) == 0 ? 0x0C : 0x1F)]
								& (grayscale | emphasis)]];
						else
							screen[(VClock * 256) + HClock] = palette[paletteIndexes[this.memory.palettes_bank[0] & (grayscale | emphasis)]];
					}
					#endregion
				}
			}
			this.core.vbl_flag = this.core.vbl_flag_temp;
			HClock++;
			#region odd frame
			if (HClock == 338)
			{
				if (UseOddFrame)
				{
					if (VClock == vbl_vclock_End)
					{
						oddSwap = !oddSwap;
						if (!oddSwap & bkg_enabled)
						{
							HClock++;
						}
					}
				}
			}
			#endregion
			this.core.CheckNMI();

			#region VBLANK, NMI and frame end
			if (HClock == 341)
			{
				this.memory.board.OnPPUScanlineTick();
				HClock = 0;
				VClock++;
				//set vbl
				if (VClock == vbl_vclock_Start)
				{
					this.core.vbl_flag_temp = true;
				}
				//clear vbl
				else if (VClock == vbl_vclock_End)
				{
					spr_0Hit = false;
					this.core.vbl_flag_temp = false;
					spr_overflow = false;
				}
				else if (VClock == frameEnd)
				{
					VClock = 0;
					#region Render
					if (FrameSkipEnabled)
					{
						if (FrameSkipTimer == 0)
						{
							videoOut.SubmitBuffer(ref screen);
						}
						if (FrameSkipTimer > 0)
							FrameSkipTimer--;
						else
							FrameSkipTimer = FrameSkipReload;
					}
					else
					{
						videoOut.SubmitBuffer(ref screen);
					}

					this.core.OnFinishFrame();
					#endregion
				}
			}
			#endregion
		}

		public void SetupVideoRenderer(IVideoProvider video)
		{
			videoOut = video;
		}
		public void SetupFrameSkip(bool frameSkipEnabled, byte frameSkipReload)
		{
			FrameSkipEnabled = frameSkipEnabled;
			FrameSkipReload = frameSkipReload;
		}
		public int GetPixel(int x, int y)
		{
			return screen[(y * 256) + x];
		}
		/// <summary>
		/// Setup palette
		/// </summary>
		/// <param name="newPalette">The palette to use; must be 512 indexes palette.</param>
		public void SetPalette(int[] newPalette)
		{
			palette = newPalette;
		}

		[Obsolete("Refactor all LoadState/SaveState methods into a IRecordableState interface or something")]
		internal void LoadState(System.IO.BinaryReader bin)
		{
			VClock = bin.ReadInt32();
			HClock = bin.ReadInt32();
			oddSwap = bin.ReadBoolean();
			current_pixel = bin.ReadInt32();
			temp = bin.ReadInt32();
			temp_comparator = bin.ReadInt32();
			bkg_pos = bin.ReadInt32();
			spr_pos = bin.ReadInt32();
			object0 = bin.ReadInt32();
			infront = bin.ReadInt32();
			bkgPixel = bin.ReadInt32();
			sprPixel = bin.ReadInt32();
			bkg_fetch_address = bin.ReadInt32();
			bkg_fetch_nametable = bin.ReadByte();
			bkg_fetch_attr = bin.ReadByte();
			bkg_fetch_bit0 = bin.ReadByte();
			bkg_fetch_bit1 = bin.ReadByte();
			spr_fetch_address = bin.ReadInt32();
			spr_fetch_bit0 = bin.ReadByte();
			spr_fetch_bit1 = bin.ReadByte();
			spr_fetch_attr = bin.ReadByte();
			for (int i = 0; i < spr_zero_buffer.Length; i++)
				spr_zero_buffer[i] = bin.ReadBoolean();
			vram_temp = bin.ReadInt32();
			vram_address = bin.ReadInt32();
			vram_address_temp_access = bin.ReadInt32();
			vram_address_temp_access1 = bin.ReadInt32();
			vram_increament = bin.ReadInt32();
			vram_flipflop = bin.ReadBoolean();
			vram_fine = bin.ReadByte();
			reg2007buffer = bin.ReadByte();
			bkg_enabled = bin.ReadBoolean();
			bkg_clipped = bin.ReadBoolean();
			bkg_patternAddress = bin.ReadInt32();
			spr_enabled = bin.ReadBoolean();
			spr_clipped = bin.ReadBoolean();
			spr_patternAddress = bin.ReadInt32();
			spr_size16 = bin.ReadInt32();
			spr_0Hit = bin.ReadBoolean();
			spr_overflow = bin.ReadBoolean();
			grayscale = bin.ReadInt32();
			emphasis = bin.ReadInt32();
			ppu_2002_temp = bin.ReadByte();
			ppu_2004_temp = bin.ReadByte();
			ppu_2007_temp = bin.ReadByte();
			oam_address = bin.ReadByte();
			oam_fetch_data = bin.ReadByte();
			oam_evaluate_slot = bin.ReadByte();
			oam_evaluate_count = bin.ReadByte();
			oam_fetch_mode = bin.ReadBoolean();
			oam_phase_index = bin.ReadByte();
			spr_render_i = bin.ReadInt32();
			bkg_render_i = bin.ReadInt32();
			spr_evaluation_i = bin.ReadInt32();
			spr_render_temp_pixel = bin.ReadInt32();
		}

		internal void SaveState(System.IO.BinaryWriter bin)
		{
			bin.Write(VClock);
			bin.Write(HClock);
			bin.Write(oddSwap);
			bin.Write(current_pixel);
			bin.Write(temp);
			bin.Write(temp_comparator);
			bin.Write(bkg_pos);
			bin.Write(spr_pos);
			bin.Write(object0);
			bin.Write(infront);
			bin.Write(bkgPixel);
			bin.Write(sprPixel);
			bin.Write(bkg_fetch_address);
			bin.Write(bkg_fetch_nametable);
			bin.Write(bkg_fetch_attr);
			bin.Write(bkg_fetch_bit0);
			bin.Write(bkg_fetch_bit1);
			bin.Write(spr_fetch_address);
			bin.Write(spr_fetch_bit0);
			bin.Write(spr_fetch_bit1);
			bin.Write(spr_fetch_attr);
			for (int i = 0; i < spr_zero_buffer.Length; i++)
			{
				bin.Write(spr_zero_buffer[i]);
			}
			bin.Write(vram_temp);
			bin.Write(vram_address);
			bin.Write(vram_address_temp_access);
			bin.Write(vram_address_temp_access1);
			bin.Write(vram_increament);
			bin.Write(vram_flipflop);
			bin.Write(vram_fine);
			bin.Write(reg2007buffer);
			bin.Write(bkg_enabled);
			bin.Write(bkg_clipped);
			bin.Write(bkg_patternAddress);
			bin.Write(spr_enabled);
			bin.Write(spr_clipped);
			bin.Write(spr_patternAddress);
			bin.Write(spr_size16);
			bin.Write(spr_0Hit);
			bin.Write(spr_overflow);
			bin.Write(grayscale);
			bin.Write(emphasis);
			bin.Write(ppu_2002_temp);
			bin.Write(ppu_2004_temp);
			bin.Write(ppu_2007_temp);
			bin.Write(oam_address);
			bin.Write(oam_fetch_data);
			bin.Write(oam_evaluate_slot);
			bin.Write(oam_evaluate_count);
			bin.Write(oam_fetch_mode);
			bin.Write(oam_phase_index);
			bin.Write(spr_render_i);
			bin.Write(bkg_render_i);
			bin.Write(spr_evaluation_i);
			bin.Write(spr_render_temp_pixel);
		}
	}
}

