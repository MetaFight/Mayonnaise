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
    [BoardInfo("6-in-1 (SuperGK)", 57)]
    class Mapper057 : Board
    {
        private int chr_aaa;
        private int chr_bbb;
        private int chr_hhh;
        public override void WritePRG(ref int address, ref byte data)
        {
            switch (address & 0x8800)
            {
                case 0x8000:
                    {
                        chr_aaa = data & 0x7;
                        chr_hhh = (data & 0x40) >> 3;
                        break;
                    }
                case 0x8800:
                    {
                        chr_bbb = data & 0x7;
                        if ((data & 0x10) == 0x10)
                        {
                            Switch32KPRG((data & 0xE0) >> 6, true);
                        }
                        else
                        {
                            Switch16KPRG((data & 0xE0) >> 5, 0x8000, true);
                            Switch16KPRG((data & 0xE0) >> 5, 0xC000, true);
                        }
                        SwitchNMT((data & 0x8) == 0x8 ? Mirroring.Horz : Mirroring.Vert);
                        break;
                    }
            }
            Switch08KCHR(chr_hhh | (chr_aaa | chr_bbb), chr_01K_rom_count > 0);
        }
        public override void SaveState(System.IO.BinaryWriter stream)
        {
            base.SaveState(stream);
            stream.Write(chr_aaa);
            stream.Write(chr_bbb);
            stream.Write(chr_hhh);
        }
        public override void LoadState(System.IO.BinaryReader stream)
        {
            base.LoadState(stream);
            chr_aaa = stream.ReadInt32();
            chr_bbb = stream.ReadInt32();
            chr_hhh = stream.ReadInt32();
        }
    }
}
