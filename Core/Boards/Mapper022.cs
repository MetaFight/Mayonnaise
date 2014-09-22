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
    [BoardInfo("VRC2", 22)]
    class Mapper022 : Board
    {
        private int[] chr_Reg;

        public override void HardReset()
        {
            base.HardReset();
            base.Switch16KPRG(prg_16K_rom_mask, 0xC000, true);
            chr_Reg = new int[8];
        }
        public override void WritePRG(ref int address, ref byte data)
        {
            switch (address)
            {
                case 0x8000:
                case 0x8002:
                case 0x8001:
                case 0x8003: base.Switch08KPRG(data & 0x0F, 0x8000, true); break;
                case 0x9000:
                case 0x9002:
                case 0x9001:
                case 0x9003:
                    {
                        switch (data & 0x3)
                        {
                            case 0: SwitchNMT(Mirroring.Vert); break;
                            case 1: SwitchNMT(Mirroring.Horz); break;
                            case 2: SwitchNMT(Mirroring.OneScA); break;
                            case 3: SwitchNMT(Mirroring.OneScB); break;
                        }
                        break;
                    }
                case 0xA000:
                case 0xA002:
                case 0xA001:
                case 0xA003: base.Switch08KPRG(data & 0x0F, 0xA000, true); break;
                case 0xB000: chr_Reg[0] = (chr_Reg[0] & 0xF0) | (data & 0xF);/*****/ Switch01KCHR(chr_Reg[0] >> 1, 0x0000, chr_01K_rom_count > 0); break;
                case 0xB002: chr_Reg[0] = (chr_Reg[0] & 0x0F) | ((data & 0xF) << 4); Switch01KCHR(chr_Reg[0] >> 1, 0x0000, chr_01K_rom_count > 0); break;
                case 0xB001: chr_Reg[1] = (chr_Reg[1] & 0xF0) | (data & 0xF);/*****/ Switch01KCHR(chr_Reg[1] >> 1, 0x0400, chr_01K_rom_count > 0); break;
                case 0xB003: chr_Reg[1] = (chr_Reg[1] & 0x0F) | ((data & 0xF) << 4); Switch01KCHR(chr_Reg[1] >> 1, 0x0400, chr_01K_rom_count > 0); break;
                case 0xC000: chr_Reg[2] = (chr_Reg[2] & 0xF0) | (data & 0xF);/*****/ Switch01KCHR(chr_Reg[2] >> 1, 0x0800, chr_01K_rom_count > 0); break;
                case 0xC002: chr_Reg[2] = (chr_Reg[2] & 0x0F) | ((data & 0xF) << 4); Switch01KCHR(chr_Reg[2] >> 1, 0x0800, chr_01K_rom_count > 0); break;
                case 0xC001: chr_Reg[3] = (chr_Reg[3] & 0xF0) | (data & 0xF);/*****/ Switch01KCHR(chr_Reg[3] >> 1, 0x0C00, chr_01K_rom_count > 0); break;
                case 0xC003: chr_Reg[3] = (chr_Reg[3] & 0x0F) | ((data & 0xF) << 4); Switch01KCHR(chr_Reg[3] >> 1, 0x0C00, chr_01K_rom_count > 0); break;
                case 0xD000: chr_Reg[4] = (chr_Reg[4] & 0xF0) | (data & 0xF);/*****/ Switch01KCHR(chr_Reg[4] >> 1, 0x1000, chr_01K_rom_count > 0); break;
                case 0xD002: chr_Reg[4] = (chr_Reg[4] & 0x0F) | ((data & 0xF) << 4); Switch01KCHR(chr_Reg[4] >> 1, 0x1000, chr_01K_rom_count > 0); break;
                case 0xD001: chr_Reg[5] = (chr_Reg[5] & 0xF0) | (data & 0xF);/*****/ Switch01KCHR(chr_Reg[5] >> 1, 0x1400, chr_01K_rom_count > 0); break;
                case 0xD003: chr_Reg[5] = (chr_Reg[5] & 0x0F) | ((data & 0xF) << 4); Switch01KCHR(chr_Reg[5] >> 1, 0x1400, chr_01K_rom_count > 0); break;
                case 0xE000: chr_Reg[6] = (chr_Reg[6] & 0xF0) | (data & 0xF);/*****/ Switch01KCHR(chr_Reg[6] >> 1, 0x1800, chr_01K_rom_count > 0); break;
                case 0xE002: chr_Reg[6] = (chr_Reg[6] & 0x0F) | ((data & 0xF) << 4); Switch01KCHR(chr_Reg[6] >> 1, 0x1800, chr_01K_rom_count > 0); break;
                case 0xE001: chr_Reg[7] = (chr_Reg[7] & 0xF0) | (data & 0xF);/*****/ Switch01KCHR(chr_Reg[7] >> 1, 0x1C00, chr_01K_rom_count > 0); break;
                case 0xE003: chr_Reg[7] = (chr_Reg[7] & 0x0F) | ((data & 0xF) << 4); Switch01KCHR(chr_Reg[7] >> 1, 0x1C00, chr_01K_rom_count > 0); break;
            }
        }
        public override void SaveState(System.IO.BinaryWriter stream)
        {
            base.SaveState(stream);
            for (int i = 0; i < chr_Reg.Length; i++)
                stream.Write(chr_Reg[i]);
        }
        public override void LoadState(System.IO.BinaryReader stream)
        {
            base.LoadState(stream);
            for (int i = 0; i < chr_Reg.Length; i++)
                chr_Reg[i] = stream.ReadInt32();
        }
    }
}
