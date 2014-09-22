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
    [BoardInfo("Super HIK 300-in-1 (1994)", 212)]
    class Mapper212 : Board
    {
        public override void WritePRG(ref int address, ref byte data)
        {
            Switch08KCHR(address & 0x7, chr_01K_rom_count > 0);
            SwitchNMT((address & 0x8) == 0x8 ? Mirroring.Horz : Mirroring.Vert);
            if ((address & 0x4000) == 0x4000)
            {
                Switch32KPRG(address >> 1 & 0x3, true);
            }
            else
            {
                Switch16KPRG(address & 0x7, 0x8000, true);
                Switch16KPRG(address & 0x7, 0xC000, true);
            }
        }
    }
}
