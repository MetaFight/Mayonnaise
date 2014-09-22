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
    [BoardInfo("Irem - PRG HI", 97)]
    class Mapper097 : Board
    {
        public override void HardReset()
        {
            base.HardReset();
            Switch16KPRG(prg_16K_rom_mask, 0x8000, true);
        }
        public override void WritePRG(ref int address, ref byte data)
        {
            Switch16KPRG(data & 0xF, 0xC000, true);
            switch ((address >> 6) & 0x3)
            {
                case 0: SwitchNMT(Mirroring.OneScA); break;
                case 1: SwitchNMT(Mirroring.Horz); break;
                case 2: SwitchNMT(Mirroring.Vert); break;
                case 3: SwitchNMT(Mirroring.OneScB); break;
            }
        }
    }
}
