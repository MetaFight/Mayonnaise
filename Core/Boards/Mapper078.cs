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
    [BoardInfo("Irem 74HC161/32", 78)]
    class Mapper078 : Board
    {
        private bool mirroring_mode_single;
        public override void HardReset()
        {
            base.HardReset();
            Switch16KPRG(prg_16K_rom_mask, 0xC000, true);
            mirroring_mode_single = false;
            if (BoardType == "JALECO-JF-16")
                mirroring_mode_single = true;
        }
        public override void WritePRG(ref int address, ref byte data)
        {
            Switch08KCHR((data >> 4) & 0xF, chr_01K_rom_count > 0);
            Switch16KPRG(data & 0x7, 0x8000, true);
            if (mirroring_mode_single)
                SwitchNMT((data & 0x8) == 0x8 ? Mirroring.OneScB : Mirroring.OneScA);
            else
                SwitchNMT((data & 0x8) == 0x8 ? Mirroring.Vert : Mirroring.Horz);
        }
    }
}
