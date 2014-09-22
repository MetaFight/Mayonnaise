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
    [BoardInfo("Unknown", 232)]
    class Mapper232 : Board
    {
        private int prg_reg;
        public override void HardReset()
        {
            base.HardReset();
            Switch16KPRG(3, 0xC000, true);
        }
        public override void WritePRG(ref int address, ref byte data)
        {
            if (address < 0xC000)
                prg_reg = ((data & 0x18) >> 1) | (prg_reg & 0x3);
            else
                prg_reg = (prg_reg & 0xC) | (data & 0x3);

            Switch16KPRG(prg_reg, 0x8000, true);
            Switch16KPRG(3 | (prg_reg & 0xC), 0xC000, true);
        }
        public override void SaveState(System.IO.BinaryWriter stream)
        {
            base.SaveState(stream);
            stream.Write(prg_reg);
        }
        public override void LoadState(System.IO.BinaryReader stream)
        {
            base.LoadState(stream);
            prg_reg = stream.ReadInt32();
        }
    }
}
