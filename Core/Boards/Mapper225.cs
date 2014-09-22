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
    [BoardInfo("Unknown", 225)]
    class Mapper225 : Board
    {
        private byte[] RAM;
        public override void HardReset()
        {
            base.HardReset();
            RAM = new byte[4];
        }
        public override void WriteEXP(ref int address, ref byte data)
        {
            if (address >= 0x5800)
                RAM[address & 0x3] = (byte)(data & 0xF);
        }
        public override byte ReadEXP(ref int address)
        {
            if (address >= 0x5800)
                return RAM[address & 0x3];
            return 0;
        }
        public override void WritePRG(ref int address, ref byte data)
        {
            Switch08KCHR(address & 0x3F, chr_01K_rom_count > 0);
            if ((address & 0x1000) == 0x1000)
            {
                Switch16KPRG(address >> 6 & 0x3F, 0x8000, true);
                Switch16KPRG(address >> 6 & 0x3F, 0xC000, true);
            }
            else
                Switch32KPRG(((address >> 6) & 0x3F) >> 1, true);
            SwitchNMT((address & 0x2000) == 0x2000 ? Mirroring.Horz : Mirroring.Vert);
        }
        public override void SaveState(System.IO.BinaryWriter stream)
        {
            base.SaveState(stream);
            stream.Write(RAM);
        }
        public override void LoadState(System.IO.BinaryReader stream)
        {
            base.LoadState(stream);
            stream.Read(RAM, 0, RAM.Length);
        }
    }
}
