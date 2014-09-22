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
    [BoardInfo("Unknown", 228)]
    class Mapper228 : Board
    {
        private byte[] RAM;
        private int bank;
        public override void HardReset()
        {
            base.HardReset();
            RAM = new byte[4];
        }
        public override void WriteEXP(ref int address, ref byte data)
        {
            RAM[address & 0x3] = (byte)(data & 0xF);
        }
        public override byte ReadEXP(ref int address)
        {
            return RAM[address & 0x3];
        }
        public override void WritePRG(ref int address, ref byte data)
        {
            Switch08KCHR(((address & 0xF) << 2) | (data & 0x3), chr_01K_rom_count > 0);
            SwitchNMT((address & 0x2000) == 0x2000 ? Mirroring.Horz : Mirroring.Vert);
            bank = (address >> 7 & 0x1F) + (address >> 7 & address >> 8 & 0x10);
            if ((address & 0x0020) == 0x0020)
            {
                Switch16KPRG((bank << 2) | (address >> 5 & 0x2), 0x8000, true);
                Switch16KPRG((bank << 2) | (address >> 5 & 0x2), 0xC000, true);
            }
            else
            {
                Switch32KPRG(bank, true);
            }
        }
        public override void SaveState(System.IO.BinaryWriter stream)
        {
            base.SaveState(stream);
            stream.Write(RAM);
            stream.Write(bank);
        }
        public override void LoadState(System.IO.BinaryReader stream)
        {
            base.LoadState(stream);
            stream.Read(RAM, 0, RAM.Length);
            bank = stream.ReadInt32();
        }
    }
}
