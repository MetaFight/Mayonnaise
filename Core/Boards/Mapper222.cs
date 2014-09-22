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
    [BoardInfo("Unknown", 222)]
    [NotImplementedWell("Mapper 222: This mapper is not tested yet.")]
    class Mapper222 : Board
    {
        public override void WritePRG(ref int address, ref byte data)
        {
            switch (address & 0xF003)
            {
                case 0x8000:
                    Switch08KPRG(data, 0x8000, true);
                    break;
                case 0x9000: SwitchNMT((data & 1) == 1 ? Mirroring.Horz : Mirroring.Vert); break;
                case 0xA000:
                    Switch08KPRG(data, 0xA000, true);
                    break;
                case 0xB000:
                    Switch01KCHR(data, 0x0000, chr_01K_rom_count > 0);
                    break;
                case 0xB002:
                    Switch01KCHR(data, 0x0400, chr_01K_rom_count > 0);
                    break;
                case 0xC000:
                    Switch01KCHR(data, 0x0800, chr_01K_rom_count > 0);
                    break;
                case 0xC002:
                    Switch01KCHR(data, 0x0C00, chr_01K_rom_count > 0);
                    break;
                case 0xD000:
                    Switch01KCHR(data, 0x1000, chr_01K_rom_count > 0);
                    break;
                case 0xD002:
                    Switch01KCHR(data, 0x1400, chr_01K_rom_count > 0);
                    break;
                case 0xE000:
                    Switch01KCHR(data, 0x1800, chr_01K_rom_count > 0);
                    break;
                case 0xE002:
                    Switch01KCHR(data, 0x1C00, chr_01K_rom_count > 0);
                    break;
            }
        }
    }
}
