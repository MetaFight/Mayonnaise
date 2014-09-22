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
    [BoardInfo("Unknown", 233)]
    class Mapper233 : Board
    {
        private int title = 0;
        private int bank;
        public override void HardReset()
        {
            base.HardReset();

            bank = title = 0;
        }
        public override void SoftReset()
        {
            base.SoftReset();
            bank = 0;
            title ^= 0x20;
        }
        public override void WritePRG(ref int address, ref byte data)
        {

            bank = data & 0x1F;

            if ((data & 0x20) == 0x20)
            {
                Switch16KPRG(title | bank, 0x8000, true);
                Switch16KPRG(title | bank, 0xC000, true);
            }
            else
                Switch32KPRG(title >> 1 | bank >> 1, true);

            switch ((data >> 6) & 0x3)
            {
                case 0: SwitchNMT(0x80); break;
                case 1: SwitchNMT(Mirroring.Vert); break;
                case 2: SwitchNMT(Mirroring.Horz); break;
                case 3: SwitchNMT(Mirroring.OneScB); break;
            }
        }
        public override void SaveState(System.IO.BinaryWriter stream)
        {
            base.SaveState(stream);
            stream.Write(title);
            stream.Write(bank);
        }
        public override void LoadState(System.IO.BinaryReader stream)
        {
            base.LoadState(stream);
            title = stream.ReadInt32();
            bank = stream.ReadInt32();
        }
    }
}
