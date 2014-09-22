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
    [BoardInfo("Super 700-in-1", 62)]
    class Mapper062 : Board
    {
        private int prg_page;
        public override void WritePRG(ref int address, ref byte data)
        {
            prg_page = ((address & 0x3F00) >> 8) | (address & 0x40);
            Switch08KCHR(((address & 0x1F) << 2) | (data & 0x3), chr_01K_rom_count > 0);
            if ((address & 0x20) == 0x20)
            {
                Switch16KPRG(prg_page, 0x8000, true);
                Switch16KPRG(prg_page, 0xC000, true);
            }
            else
                Switch32KPRG(prg_page >> 1, true);
            SwitchNMT((address & 0x80) == 0x80 ? Mirroring.Horz : Mirroring.Vert);
        }
    }
}
