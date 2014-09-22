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
    [BoardInfo("15-in-1 Color Dreams", 46)]
    class Mapper046 : Board
    {
        private int prg_reg;
        private int chr_reg;

        public override void WriteSRM(ref int address, ref byte data)
        {
            prg_reg = (prg_reg & 0x01) | ((data << 1) & 0x1E);
            chr_reg = (chr_reg & 0x07) | ((data >> 1) & 0x78);
            Switch08KCHR(chr_reg, chr_01K_rom_count > 0);
            Switch32KPRG(prg_reg, true);
        }
        public override void WritePRG(ref int address, ref byte data)
        {
            prg_reg = (data >> 0 & 0x1) | (prg_reg & 0x1E);
            chr_reg = (data >> 4 & 0x7) | (chr_reg & 0x78);
            Switch08KCHR(chr_reg, chr_01K_rom_count > 0);
            Switch32KPRG(prg_reg, true);
        }
        public override void SaveState(System.IO.BinaryWriter stream)
        {
            base.SaveState(stream);
            stream.Write(prg_reg);
            stream.Write(chr_reg);
        }
        public override void LoadState(System.IO.BinaryReader stream)
        {
            base.LoadState(stream);
            prg_reg = stream.ReadInt32();
            chr_reg = stream.ReadInt32();
        }
    }
}
