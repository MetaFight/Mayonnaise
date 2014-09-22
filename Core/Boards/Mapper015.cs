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
    [BoardInfo("100-in-1 Contra Function 16", 15)]
    class Mapper015 : Board
    {
        private int temp;
        public override void WritePRG(ref int address, ref byte data)
        {
            switch (address & 0x3)
            {
                case 0:
                    Switch16KPRG(data & 0x3F, 0x8000, true);
                    Switch16KPRG((data & 0x3F) | 1, 0xC000, true);
                    break;
                case 1:
                    Switch16KPRG(data & 0x3F, 0x8000, true);
                    Switch16KPRG(prg_16K_rom_mask, 0xC000, true);
                    break;
                case 2:
                    temp = data << 1;
                    temp = ((data & 0x3F) << 1) | ((data >> 7) & 1);
                    Switch08KPRG(temp, 0x8000, true);
                    Switch08KPRG(temp, 0xA000, true);
                    Switch08KPRG(temp, 0xC000, true);
                    Switch08KPRG(temp, 0xE000, true);
                    break;

                case 3:
                    Switch16KPRG(data & 0x3F, 0x8000, true);
                    Switch16KPRG(data & 0x3F, 0xC000, true);
                    break;

            }
            SwitchNMT((data & 0x40) == 0x40 ? Mirroring.Horz : Mirroring.Vert);
        }
    }
}
