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

namespace MyNes.Core
{
    [BoardInfo("MMC5", 5, 8, 16)]
    [WithExternalSound]
    [NotImplementedWell("Mapper 5 (MMC5)\nSplit screen not implemented.\nUchuu Keibitai SDF game graphic corruption for unknown reason in the intro (not in the split screen).")]
    class Mapper005 : Board
    {
        // TODO: split screen and Uchuu Keibitai SDF chr corruption for unknown reason in the intro (not in the split screen).
        private int ram_protectA;
        private int ram_protectB;
        private int ExRAM_mode;
        private int[] CHROffset_spr;// The indexes to use with spr in spr fetches.
        private int[] CHROffsetEX;// For extra attr
        private int[] CHROffsetSP;// For split screen
        private int[] chrRegA;
        private int[] chrRegB;

        private int[] prgReg;
        private bool useSRAMmirroring;
        private int chr_high;
        private int chr_mode;
        private int prg_mode;
        private bool chr_setB_last;
        private byte temp_val;
        private byte temp_fill;
        private int lastAccessVRAM;
        private int paletteNo;
        private int shift;
        private int EXtilenumber;
        private byte multiplicand;
        private byte multiplier;
        private ushort product;
        private bool split_enable;
        private bool split_right;
        private int split_tile;
        private int split_yscroll;// The y scroll value for split.
        private bool split_doit;// Set to true to make nt changes; get split happening. Used in CHR read access.
        private int split_watch_tile;// A temp tile number, for the split.
        private byte irq_line;
        private byte irq_enable;
        private int irq_pending;
        private int irq_current_counter;
        private int irq_current_inframe;
        /*Sound channels*/
        private MMC5SqrSoundChannel channel_sq1;
        private MMC5SqrSoundChannel channel_sq2;
        private MMC5PcmSoundChannel channel_pcm;
        private double[][][] mix_table;

        public override void Initialize(string sha1, byte[] prg_dump, byte[] chr_dump, byte[] trainer_dump, Mirroring defaultMirroring)
        {
            base.Initialize(sha1, prg_dump, chr_dump, trainer_dump, defaultMirroring);
            InitializeSoundMixTable();
            channel_sq1 = new MMC5SqrSoundChannel();
            channel_sq2 = new MMC5SqrSoundChannel();
            channel_pcm = new MMC5PcmSoundChannel();
        }
        private void InitializeSoundMixTable()
        {
            mix_table = new double[16][][];

            for (int sq1 = 0; sq1 < 16; sq1++)
            {
                mix_table[sq1] = new double[16][];

                for (int sq2 = 0; sq2 < 16; sq2++)
                {
                    mix_table[sq1][sq2] = new double[256];

                    for (int dmc = 0; dmc < 256; dmc++)
                    {
                        double sqr = (95.88 / (8128.0 / (sq1 + sq2) + 100));
                        double tnd = (159.79 / (1.0 / (dmc / 22638.0) + 100));

                        mix_table[sq1][sq2][dmc] = (sqr + tnd) * 160;
                    }
                }
            }
        }
        public override void HardReset()
        {
            base.HardReset();
            // This is not a hack, "Uncharted Waters" title actually use 2 chips of SRAM which depends on bit 2 of
            // $5113 register instead of first 2 bits for switching.
            // There's no other way to make switching right.
            switch (RomSHA1.ToUpper())
            {
                // Uncharted Waters
                case "37267833C984F176DB4B0BC9D45DABA0FFF45304": useSRAMmirroring = true; break;
                // Daikoukai Jidai (J)
                case "800AEFE756E85A0A78CCB4DAE68EBBA5DF24BF41": useSRAMmirroring = true; break;
            }
            System.Console.WriteLine("MMC5: using PRG RAM mirroring = " + useSRAMmirroring);

            CHROffset_spr = new int[8];
            CHROffsetEX = new int[8];
            CHROffsetSP = new int[8];
            chrRegA = new int[8];
            chrRegB = new int[4];
            prgReg = new int[4];

            prgReg[3] = prg_08K_rom_mask;
            prg_mode = 3;
            base.Switch08KPRG(prg_08K_rom_mask, 0x8000, true);
            base.Switch08KPRG(prg_08K_rom_mask, 0xA000, true);
            base.Switch08KPRG(prg_08K_rom_mask, 0xC000, true);
            base.Switch08KPRG(prg_08K_rom_mask, 0xE000, true);

            Switch04kCHREX(0, 0x0000);
            Switch04kCHRSP(0, 0x0000);
            Switch08kCHR_spr(0);

            TogglePRGRAMWritableEnable(true);
            TogglePRGRAMEnable(true);

            channel_sq1.HardReset();
            channel_sq2.HardReset();
            channel_pcm.HardReset();
        }
        public override void SoftReset()
        {
            base.SoftReset();
            channel_sq1.SoftReset();
            channel_sq2.SoftReset();
            channel_pcm.SoftReset();
        }
        // All registers writes here
        public override void WriteEXP(ref int address, ref byte value)
        {
            if (address >= 0x5C00)
            {
                if (ExRAM_mode == 2)// Only EX2 is writable.
                    base.nmt_banks[2][address & 0x3FF] = value;
                else if (ExRAM_mode < 2)
                {
                    if (irq_current_inframe == 0x40)
                        base.nmt_banks[2][address & 0x3FF] = value;
                    else
                        base.nmt_banks[2][address & 0x3FF] = 0;
                }
                return;
            }
            switch (address)
            {
                #region Sound Channels
                case 0x5000: channel_sq1.Write5000(value); break;
                case 0x5002: channel_sq1.Write5002(value); break;
                case 0x5003: channel_sq1.Write5003(value); break;
                case 0x5004: channel_sq2.Write5000(value); break;
                case 0x5006: channel_sq2.Write5002(value); break;
                case 0x5007: channel_sq2.Write5003(value); break;
                //case 0x5010: channel_pcm.Write5010(value); break;
                case 0x5011: channel_pcm.Write5011(value); break;
                case 0x5015:
                    {
                        channel_sq1.Enabled = (value & 0x1) != 0;
                        channel_sq2.Enabled = (value & 0x2) != 0;
                        break;
                    }
                #endregion
                case 0x5100: prg_mode = value & 0x3; break;
                case 0x5101: chr_mode = value & 0x3; break;
                case 0x5102: ram_protectA = value & 0x3; UpdateRamProtect(); break;
                case 0x5103: ram_protectB = value & 0x3; UpdateRamProtect(); break;
                case 0x5104: ExRAM_mode = value & 0x3; break;
                case 0x5105: SwitchNMT(value); break;
                #region PRG
                case 0x5113:
                    {
                        if (!useSRAMmirroring)
                            base.Switch08KPRG(value & 0x7, 0x6000, false);
                        else// Use chip switching (bit 2)...
                            base.Switch08KPRG((value >> 2) & 1, 0x6000, false);
                        break;
                    }
                case 0x5114:
                    {
                        if (prg_mode == 3)
                            base.Switch08KPRG(value & 0x7F, 0x8000, (value & 0x80) == 0x80);
                        break;
                    }
                case 0x5115:
                    {
                        switch (prg_mode)
                        {
                            case 1: base.Switch16KPRG((value & 0x7F) >> 1, 0x8000, (value & 0x80) == 0x80); break;
                            case 2: base.Switch16KPRG((value & 0x7F) >> 1, 0x8000, (value & 0x80) == 0x80); break;
                            case 3: base.Switch08KPRG(value & 0x7F, 0xA000, (value & 0x80) == 0x80); break;
                        }
                        break;
                    }
                case 0x5116:
                    {
                        switch (prg_mode)
                        {
                            case 2:
                            case 3: base.Switch08KPRG(value & 0x7F, 0xC000, (value & 0x80) == 0x80); break;
                        }
                        break;
                    }
                case 0x5117:
                    {
                        switch (prg_mode)
                        {
                            case 0: base.Switch32KPRG((value & 0x7C) >> 2, true); break;
                            case 1: base.Switch16KPRG((value & 0x7F) >> 1, 0xC000, true); break;
                            case 2: base.Switch08KPRG(value & 0x7F, 0xE000, true); break;
                            case 3: base.Switch08KPRG(value & 0x7F, 0xE000, true); break;
                        }
                        break;
                    }
                #endregion
                #region CHR
                // SPR SET
                case 0x5120:
                    {
                        chr_setB_last = false;
                        if (chr_mode == 3)
                            Switch01kCHR_spr(value | chr_high, 0x0000);
                        break;
                    }
                case 0x5121:
                    {
                        chr_setB_last = false;
                        switch (chr_mode)
                        {
                            case 2: Switch02kCHR_spr(value | chr_high, 0x0000); break;
                            case 3: Switch01kCHR_spr(value | chr_high, 0x0400); break;
                        }
                        break;
                    }
                case 0x5122:
                    {
                        chr_setB_last = false;
                        if (chr_mode == 3)
                            Switch01kCHR_spr(value | chr_high, 0x0800);
                        break;
                    }
                case 0x5123:
                    {
                        chr_setB_last = false;
                        switch (chr_mode)
                        {
                            case 1: Switch04kCHR_spr(value | chr_high, 0x0000); break;
                            case 2: Switch02kCHR_spr(value | chr_high, 0x0800); break;
                            case 3: Switch01kCHR_spr(value | chr_high, 0x0C00); break;
                        }
                        break;
                    }
                case 0x5124:
                    {
                        chr_setB_last = false;
                        if (chr_mode == 3)
                            Switch01kCHR_spr(value | chr_high, 0x1000);
                        break;
                    }
                case 0x5125:
                    {
                        chr_setB_last = false;
                        switch (chr_mode)
                        {
                            case 2: Switch02kCHR_spr(value | chr_high, 0x1000); break;
                            case 3: Switch01kCHR_spr(value | chr_high, 0x1400); break;
                        }
                        break;
                    }
                case 0x5126:
                    {
                        chr_setB_last = false;
                        if (chr_mode == 3)
                            Switch01kCHR_spr(value | chr_high, 0x1800);
                        break;
                    }
                case 0x5127:
                    {
                        chr_setB_last = false;
                        switch (chr_mode)
                        {
                            case 0: Switch08kCHR_spr(value | chr_high); break;
                            case 1: Switch04kCHR_spr(value | chr_high, 0x1000); break;
                            case 2: Switch02kCHR_spr(value | chr_high, 0x1800); break;
                            case 3: Switch01kCHR_spr(value | chr_high, 0x1C00); break;
                        }
                        break;
                    }
                // BKG SET
                case 0x5128:
                    {
                        chr_setB_last = true;
                        if (chr_mode == 3)
                        {
                            Switch01KCHR(value | chr_high, 0x0000, chr_01K_rom_count > 0);
                            Switch01KCHR(value | chr_high, 0x1000, chr_01K_rom_count > 0);
                        }
                        break;
                    }
                case 0x5129:
                    {
                        chr_setB_last = true;
                        switch (chr_mode)
                        {
                            case 2:
                                {
                                    Switch02KCHR(value | chr_high, 0x0000, chr_01K_rom_count > 0);
                                    Switch02KCHR(value | chr_high, 0x1000, chr_01K_rom_count > 0);
                                    break;
                                }
                            case 3:
                                {
                                    Switch01KCHR(value | chr_high, 0x0400, chr_01K_rom_count > 0);
                                    Switch01KCHR(value | chr_high, 0x1400, chr_01K_rom_count > 0);
                                    break;
                                }
                        }
                        break;
                    }
                case 0x512A:
                    {
                        chr_setB_last = true;
                        if (chr_mode == 3)
                        {
                            Switch01KCHR(value | chr_high, 0x0800, chr_01K_rom_count > 0);
                            Switch01KCHR(value | chr_high, 0x1800, chr_01K_rom_count > 0);
                        }
                        break;
                    }
                case 0x512B:
                    {
                        chr_setB_last = true;
                        switch (chr_mode)
                        {
                            case 0:
                                {
                                    Switch04kCHR_bkg((value | chr_high), 0x0000);
                                    Switch04kCHR_bkg((value | chr_high), 0x1000);
                                    break;
                                }
                            case 1:
                                {
                                    Switch04KCHR(value | chr_high, 0x0000, chr_01K_rom_count > 0);
                                    Switch04KCHR(value | chr_high, 0x1000, chr_01K_rom_count > 0);
                                    break;
                                }
                            case 2:
                                {
                                    Switch02KCHR(value | chr_high, 0x0800, chr_01K_rom_count > 0);
                                    Switch02KCHR(value | chr_high, 0x1800, chr_01K_rom_count > 0);
                                    break;
                                }
                            case 3:
                                {
                                    Switch01KCHR(value | chr_high, 0x0C00, chr_01K_rom_count > 0);
                                    Switch01KCHR(value | chr_high, 0x1C00, chr_01K_rom_count > 0);
                                    break;
                                }
                        }
                        break;
                    }
                case 0x5130:
                    {
                        chr_high = (value & 0x3) << 8;
                        break;
                    }
                #endregion
                //Fill-mode tile
                case 0x5106:
                    for (int i = 0; i < 0x3C0; i++)
                        base.nmt_banks[3][i] = value;
                    break;
                //Fill-mode attr
                case 0x5107:
                    for (int i = 0x3C0; i < (0x3C0 + 0x40); i++)
                    {
                        temp_fill = (byte)((2 << (value & 0x03)) | (value & 0x03));
                        temp_fill |= (byte)((temp_fill & 0x0F) << 4);
                        base.nmt_banks[3][i] = temp_fill;
                    }
                    break;
                case 0x5200:
                    {
                        split_tile = value & 0x1F;
                        split_enable = (value & 0x80) == 0x80;
                        split_right = (value & 0x40) == 0x40;
                        break;
                    }
                case 0x5201:
                    {
                        split_yscroll = value;
                        break;
                    }
                case 0x5202:
                    {
                        Switch04kCHRSP(value, address & 0x0000);
                        Switch04kCHRSP(value, address & 0x1000);
                        break;
                    }
                case 0x5203: irq_line = value; break;
                case 0x5204: irq_enable = value; break;
                case 0x5205: multiplicand = value; product = (ushort)(multiplicand * multiplier); break;
                case 0x5206: multiplier = value; product = (ushort)(multiplicand * multiplier); break;
            }
        }
        public override byte ReadEXP(ref int address)
        {
            if (address >= 0x5C00)
            {
                if (ExRAM_mode >= 2)
                    return base.nmt_banks[2][address & 0x3FF];
            }
            switch (address)
            {
                case 0x5204:
                    {
                        temp_val = (byte)(irq_current_inframe | irq_pending);
                        irq_pending = 0;
                        NesEmu.IRQFlags &= ~NesEmu.IRQ_BOARD;
                        return temp_val;
                    }
                case 0x5205: return (byte)(product & 0xFF);
                case 0x5206: return (byte)((product & 0xFF00) >> 8);
                case 0x5015: return (byte)((channel_sq1.Enabled ? 0x1 : 0) | (channel_sq2.Enabled ? 0x2 : 0));
            }
            return 0;
        }
        public override byte ReadCHR(ref int address, bool spriteFetch)
        {
            if (!spriteFetch)
            {
                if (split_enable)
                {
                    if (ExRAM_mode < 2)// Split screen mode is only allowed in Ex0 or Ex1
                    {
                        split_watch_tile = address & 0x3FF / 16;
                        if (!split_right)// Left
                            split_doit = split_watch_tile < split_tile;// Tiles 0 to T-1 are the split.
                        else// Right
                            split_doit = split_watch_tile >= split_tile;// Tiles 0 to T-1 are rendered normally. Tiles T and on are the split.
                        if (split_doit)
                        {
                            //  return CHR[((address & 0x03FF) | CHROffsetSP[address >> 10 & 0x07]) & CHRMaxSizeInBytesMask];
                        }
                    }
                }
            }
            if (ExRAM_mode == 1)// Extended Attribute mode
            {
                if (!spriteFetch)
                {
                    EXtilenumber = base.nmt_banks[2][lastAccessVRAM] & 0x3F;
                    Switch04kCHREX(EXtilenumber | chr_high, address & 0x1000);
                    return base.chr_banks[CHROffsetEX[(address >> 10) & 0x7]][address & 0x03FF];
                }
                else// Sprites not effected
                {
                    return base.chr_banks[CHROffset_spr[(address >> 10) & 0x7]][address & 0x03FF];
                }
            }
            else
            {
                if (NesEmu.spr_size16 == 0x10)
                {
                    // When in 8x16 sprite mode, both sets of registers are used. 
                    // The 'A' set is used for sprite tiles, and the 'B' set is used for BG.
                    if (!spriteFetch)
                        return base.chr_banks[base.chr_indexes[(address >> 10) & 0x7]][address & 0x03FF];
                    else
                        return base.chr_banks[CHROffset_spr[(address >> 10) & 0x7]][address & 0x03FF];
                }
                else
                {
                    // When in 8x8 sprite mode, only one set is used for both BG and sprites.  
                    // Either 'A' or 'B', depending on which set is written to last
                    if (chr_setB_last)
                        return base.chr_banks[base.chr_indexes[(address >> 10) & 0x7]][address & 0x03FF];
                    else
                        return base.chr_banks[CHROffset_spr[(address >> 10) & 0x7]][address & 0x03FF];
                }
            }
        }
        public override byte ReadNMT(ref int address)
        {
            /*
             *  %00 = Extra Nametable mode    ("Ex0")
                %01 = Extended Attribute mode ("Ex1")
                %10 = CPU access mode         ("Ex2")
                %11 = CPU read-only mode      ("Ex3")
             
             * NT Values can be the following:
                  %00 = NES internal NTA
                  %01 = NES internal NTB
                  %10 = use ExRAM as NT
                  %11 = Fill Mode
             */
            if (split_doit)
            {
                // TODO: MMC5 split
                // ExRAM is always used as the nametable in split screen mode.
                // return base.NMT[2][address & 0x03FF];
            }
            if (ExRAM_mode == 1)// Extended Attribute mode
            {
                if ((address & 0x03FF) <= 0x3BF)
                {
                    lastAccessVRAM = address & 0x03FF;
                }
                else
                {
                    paletteNo = base.nmt_banks[2][lastAccessVRAM] & 0xC0;
                    // Fix Attribute bits
                    shift = ((lastAccessVRAM >> 4 & 0x04) | (lastAccessVRAM & 0x02));
                    switch (shift)
                    {
                        case 0: return (byte)(paletteNo >> 6);
                        case 2: return (byte)(paletteNo >> 4);
                        case 4: return (byte)(paletteNo >> 2);
                        case 6: return (byte)(paletteNo >> 0);
                    }
                }
            }
            return base.nmt_banks[base.nmt_indexes[(address >> 10) & 0x03]][address & 0x03FF];// Reached here in some cases above.
        }
        public override void WriteNMT(ref int address, ref byte value)
        {
            if (ExRAM_mode == 1)
            {
                if ((address & 0x03FF) <= 0x3BF)
                {
                    lastAccessVRAM = address & 0x03FF;
                }
            }
            base.nmt_banks[base.nmt_indexes[(address >> 10) & 0x03]][address & 0x03FF] = value;
        }

        private void UpdateRamProtect()
        {
            TogglePRGRAMWritableEnable((ram_protectA == 0x2) && (ram_protectB == 0x1));
        }
        private void Switch04kCHR_bkg(int index, int where)
        {
            int area = (where >> 10) & 0x07;
            index <<= 2;

            chr_indexes[area] = (index & chr_01K_rom_mask) + chr_rom_bank_offset;
            area++;
            index++;
            chr_indexes[area] = (index & chr_01K_rom_mask) + chr_rom_bank_offset;
            area++;
            index++;
            chr_indexes[area] = (index & chr_01K_rom_mask) + chr_rom_bank_offset;
            area++;
            index++;
            chr_indexes[area] = (index & chr_01K_rom_mask) + chr_rom_bank_offset;
        }
        private void Switch01kCHR_spr(int index, int where)
        {
            CHROffset_spr[(where >> 10) & 0x07] = (index & chr_01K_rom_mask) + chr_rom_bank_offset;
        }
        private void Switch02kCHR_spr(int index, int where)
        {
            int area = (where >> 10) & 0x07;
            index <<= 1;

            CHROffset_spr[area] = (index & chr_01K_rom_mask) + chr_rom_bank_offset; index++;
            CHROffset_spr[area + 1] = (index & chr_01K_rom_mask) + chr_rom_bank_offset;
        }
        private void Switch04kCHR_spr(int index, int where)
        {
            int area = (where >> 10) & 0x07;
            index <<= 2;

            CHROffset_spr[area] = (index & chr_01K_rom_mask) + chr_rom_bank_offset;
            area++;
            index++;
            CHROffset_spr[area] = (index & chr_01K_rom_mask) + chr_rom_bank_offset;
            area++;
            index++;
            CHROffset_spr[area] = (index & chr_01K_rom_mask) + chr_rom_bank_offset;
            area++;
            index++;
            CHROffset_spr[area] = (index & chr_01K_rom_mask) + chr_rom_bank_offset;
        }
        private void Switch08kCHR_spr(int index)
        {
            index <<= 3;
            CHROffset_spr[0] = (index & chr_01K_rom_mask) + chr_rom_bank_offset;
            index++;
            CHROffset_spr[1] = (index & chr_01K_rom_mask) + chr_rom_bank_offset;
            index++;
            CHROffset_spr[2] = (index & chr_01K_rom_mask) + chr_rom_bank_offset;
            index++;
            CHROffset_spr[3] = (index & chr_01K_rom_mask) + chr_rom_bank_offset;
            index++;
            CHROffset_spr[4] = (index & chr_01K_rom_mask) + chr_rom_bank_offset;
            index++;
            CHROffset_spr[5] = (index & chr_01K_rom_mask) + chr_rom_bank_offset;
            index++;
            CHROffset_spr[6] = (index & chr_01K_rom_mask) + chr_rom_bank_offset;
            index++;
            CHROffset_spr[7] = (index & chr_01K_rom_mask) + chr_rom_bank_offset;
        }
        private void Switch04kCHREX(int index, int where)
        {
            int area = (where >> 10) & 0x07;
            index <<= 2;

            CHROffsetEX[area] = (index & chr_01K_rom_mask) + chr_rom_bank_offset;
            area++;
            index++;
            CHROffsetEX[area] = (index & chr_01K_rom_mask) + chr_rom_bank_offset;
            area++;
            index++;
            CHROffsetEX[area] = (index & chr_01K_rom_mask) + chr_rom_bank_offset;
            area++;
            index++;
            CHROffsetEX[area] = (index & chr_01K_rom_mask) + chr_rom_bank_offset;
        }
        private void Switch04kCHRSP(int index, int where)
        {
            int area = (where >> 10) & 0x07;
            index <<= 2;

            CHROffsetSP[area] = (index & chr_01K_rom_mask) + chr_rom_bank_offset;
            area++;
            index++;
            CHROffsetSP[area] = (index & chr_01K_rom_mask) + chr_rom_bank_offset;
            area++;
            index++;
            CHROffsetSP[area] = (index & chr_01K_rom_mask) + chr_rom_bank_offset;
            area++;
            index++;
            CHROffsetSP[area] = (index & chr_01K_rom_mask) + chr_rom_bank_offset;
        }

        // IRQ
        public override void OnPPUScanlineTick()
        {
            // In frame signal
            irq_current_inframe = (NesEmu.IsInRender() && NesEmu.IsRenderingOn()) ? 0x40 : 0x00;
            if (irq_current_inframe == 0)
            {
                irq_current_inframe = 0x40;
                irq_current_counter = 0;
                irq_pending = 0;
                NesEmu.IRQFlags &= ~NesEmu.IRQ_BOARD;
            }
            else
            {
                irq_current_counter++;
                if (irq_current_counter == irq_line)
                {
                    irq_pending = 0x80;// IRQ pending flag is raised *regardless* of whether or not IRQs are enabled.
                    if (irq_enable == 0x80)// Trigger an IRQ on the 6502 if both this flag *and* the IRQ enable flag is set.
                        NesEmu.IRQFlags |= NesEmu.IRQ_BOARD;
                }
            }
        }
        public override void OnAPUClockDuration()
        {
            channel_sq1.ClockLengthCounter();
            channel_sq2.ClockLengthCounter();
        }
        public override void OnAPUClockEnvelope()
        {
            channel_sq1.ClockEnvelope();
            channel_sq2.ClockEnvelope();
        }
        public override void OnAPUClockSingle(ref bool isClockingLength)
        {
            channel_sq1.ClockSingle(isClockingLength);
            channel_sq2.ClockSingle(isClockingLength);
        }
        public override double APUGetSamples()
        {
            return mix_table[channel_sq1.output]
                            [channel_sq2.output]
                            [channel_pcm.GetSample()];
        }

        public override void SaveState(System.IO.BinaryWriter stream)
        {
            base.SaveState(stream);
            stream.Write(ram_protectA);
            stream.Write(ram_protectB);
            stream.Write(ExRAM_mode);
            for (int i = 0; i < CHROffset_spr.Length; i++)
                stream.Write(CHROffset_spr[i]);
            for (int i = 0; i < CHROffsetEX.Length; i++)
                stream.Write(CHROffsetEX[i]);
            for (int i = 0; i < CHROffsetSP.Length; i++)
                stream.Write(CHROffsetSP[i]);
            for (int i = 0; i < chrRegA.Length; i++)
                stream.Write(chrRegA[i]);
            for (int i = 0; i < chrRegB.Length; i++)
                stream.Write(chrRegB[i]);
            for (int i = 0; i < prgReg.Length; i++)
                stream.Write(prgReg[i]);
            stream.Write(useSRAMmirroring);
            stream.Write(chr_high);
            stream.Write(chr_mode);
            stream.Write(prg_mode);
            stream.Write(chr_setB_last);
            stream.Write(temp_val);
            stream.Write(temp_fill);
            stream.Write(lastAccessVRAM);
            stream.Write(paletteNo);
            stream.Write(shift);
            stream.Write(EXtilenumber);
            stream.Write(multiplicand);
            stream.Write(multiplier);
            stream.Write(product);
            stream.Write(split_enable);
            stream.Write(split_right);
            stream.Write(split_tile);
            stream.Write(split_yscroll);
            stream.Write(split_doit);
            stream.Write(split_watch_tile);
            stream.Write(irq_line);
            stream.Write(irq_enable);
            stream.Write(irq_pending);
            stream.Write(irq_current_counter);
            stream.Write(irq_current_inframe);
            channel_sq1.SaveState(stream);
            channel_sq2.SaveState(stream);
            channel_pcm.SaveState(stream);
        }
        public override void LoadState(System.IO.BinaryReader stream)
        {
            base.LoadState(stream);
            ram_protectA = stream.ReadInt32();
            ram_protectB = stream.ReadInt32();
            ExRAM_mode = stream.ReadInt32();
            for (int i = 0; i < CHROffset_spr.Length; i++)
                CHROffset_spr[i] = stream.ReadInt32();
            for (int i = 0; i < CHROffsetEX.Length; i++)
                CHROffsetEX[i] = stream.ReadInt32();
            for (int i = 0; i < CHROffsetSP.Length; i++)
                CHROffsetSP[i] = stream.ReadInt32();
            for (int i = 0; i < chrRegA.Length; i++)
                chrRegA[i] = stream.ReadInt32();
            for (int i = 0; i < chrRegB.Length; i++)
                chrRegB[i] = stream.ReadInt32();
            for (int i = 0; i < prgReg.Length; i++)
                prgReg[i] = stream.ReadInt32();
            useSRAMmirroring = stream.ReadBoolean();
            chr_high = stream.ReadInt32();
            chr_mode = stream.ReadInt32();
            prg_mode = stream.ReadInt32();
            chr_setB_last = stream.ReadBoolean();
            temp_val = stream.ReadByte();
            temp_fill = stream.ReadByte();
            lastAccessVRAM = stream.ReadInt32();
            paletteNo = stream.ReadInt32();
            shift = stream.ReadInt32();
            EXtilenumber = stream.ReadInt32();
            multiplicand = stream.ReadByte();
            multiplier = stream.ReadByte();
            product = stream.ReadUInt16();
            split_enable = stream.ReadBoolean();
            split_right = stream.ReadBoolean();
            split_tile = stream.ReadInt32();
            split_yscroll = stream.ReadInt32();
            split_doit = stream.ReadBoolean();
            split_watch_tile = stream.ReadInt32();
            irq_line = stream.ReadByte();
            irq_enable = stream.ReadByte();
            irq_pending = stream.ReadInt32();
            irq_current_counter = stream.ReadInt32();
            irq_current_inframe = stream.ReadInt32();
            channel_sq1.LoadState(stream);
            channel_sq2.LoadState(stream);
            channel_pcm.LoadState(stream);
        }
    }
}
