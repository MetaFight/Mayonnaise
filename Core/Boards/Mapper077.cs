﻿/* This file is part of My Nes
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
    [BoardInfo("Irem", 77)]
    class Mapper077 : Board
    {
        public override void HardReset()
        {
            base.HardReset();
            Switch02KCHR(0, 0x0800, false);
            Switch02KCHR(1, 0x1000, false);
            Switch02KCHR(2, 0x1800, false);
        }
        public override void WritePRG(ref int address, ref byte data)
        {
            Switch02KCHR((data >> 4) & 0xF, 0x0000, chr_01K_rom_count > 0);
            Switch32KPRG(data & 0xF, true);
        }
    }
}
