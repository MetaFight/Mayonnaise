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
    [BoardInfo("FFE F4xxx", 6)]
    [NotImplementedWell("Mapper 6:\nThis mapper is not implemented well and not tested.")]
    class Mapper006 : FFE
    {
        // TODO: mapper 6 not implemented well.
        public override void HardReset()
        {
            base.HardReset();
            base.Switch16KPRG(7, 0xC000, true);
        }
        public override void WritePRG(ref int address, ref byte data)
        {
            base.Switch08KCHR(data & 3, chr_01K_rom_count > 0);
            base.Switch16KPRG((data >> 2) & 0xF, 0x8000, true);
        }
    }
}
