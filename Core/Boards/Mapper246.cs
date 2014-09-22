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
    [BoardInfo("Unknown", 246)]
    class Mapper246 : Board
    {
        public override void HardReset()
        {
            base.HardReset();
            Switch08KPRG(0xFF, 0xE000, true);
        }
        public override void WriteSRM(ref int address, ref byte data)
        {
            if (address < 0x6800)
            {
                switch (address)
                {
                    case 0x6000: Switch08KPRG(data, 0x8000, true); break;
                    case 0x6001: Switch08KPRG(data, 0xA000, true); break;
                    case 0x6002: Switch08KPRG(data, 0xC000, true); break;
                    case 0x6003: Switch08KPRG(data, 0xE000, true); break;
                    case 0x6004: Switch02KCHR(data, 0x0000, true); break;
                    case 0x6005: Switch02KCHR(data, 0x0800, true); break;
                    case 0x6006: Switch02KCHR(data, 0x1000, true); break;
                    case 0x6007: Switch02KCHR(data, 0x1800, true); break;
                }
            }
            else
                base.WriteSRM(ref address, ref data);
        }
    }
}
