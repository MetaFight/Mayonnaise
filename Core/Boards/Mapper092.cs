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
    [BoardInfo("Jaleco Early Mapper 1", 92)]
    class Mapper092 : Board
    {
        private int chr_reg;
        private int prg_reg;
        public override void HardReset()
        {
            base.HardReset();
            Switch16KPRG(0, 0x8000, true);
            chr_reg = prg_reg = 0;
        }
        public override void WritePRG(ref int address, ref byte data)
        {
            switch ((data >> 6) & 0x3)
            {
                case 0:// Do switches
                    {
                        Switch08KCHR(chr_reg, chr_01K_rom_count > 0);
                        Switch16KPRG(prg_reg, 0xC000, true);
                        break;
                    }
                case 1: chr_reg = data & 0x0F; break;// Set chr reg
                case 2: prg_reg = data & 0x0F; break;// Set prg reg 
            }
        }
        public override void SaveState(System.IO.BinaryWriter stream)
        {
            base.SaveState(stream);
            stream.Write(chr_reg);
            stream.Write(prg_reg);
        }
        public override void LoadState(System.IO.BinaryReader stream)
        {
            base.LoadState(stream);
            chr_reg = stream.ReadInt32();
            prg_reg = stream.ReadInt32();
        }
    }
}
