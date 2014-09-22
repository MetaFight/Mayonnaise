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
    [BoardInfo("Sachen Poker", 243)]
    [NotImplementedWell("Mapper 243\nShows glitches in Poker III 5-in-1 main menu although the mapper is implemented well.")]
    class Mapper243 : Board
    {
        private int addr;
        private int chr_reg;
        public override void HardReset()
        {
            base.HardReset();
            addr = chr_reg = 0;
        }
        public override void WriteEXP(ref int address, ref byte data)
        {
            if (address < 0x5000)
                switch (address & 0x4101)
                {
                    case 0x4100: addr = data & 0x7; break;
                    case 0x4101:
                        {
                            switch (addr)
                            {
                                case 2: chr_reg = ((data << 3) & 0x8) | (chr_reg & 0x7); Switch08KCHR(chr_reg, chr_01K_rom_count > 0); break;
                                case 4: chr_reg = (data & 1) | (chr_reg & 0xE); Switch08KCHR(chr_reg, chr_01K_rom_count > 0); break;
                                case 5: Switch32KPRG(data & 0x7, true); break;
                                case 6: chr_reg = ((data << 1) & 0x6) | (chr_reg & 0x9); Switch08KCHR(chr_reg, chr_01K_rom_count > 0); break;
                                case 7:
                                    {
                                        switch (data >> 1 & 3)
                                        {
                                            case 0: SwitchNMT(Mirroring.Horz); break;
                                            case 1: SwitchNMT(Mirroring.Vert); break;
                                            case 2: SwitchNMT(0xE); break;
                                            case 3: SwitchNMT(Mirroring.OneScB); break;
                                        }
                                        break;
                                    }
                            }
                            break;
                        }
                }
        }
        public override void SaveState(System.IO.BinaryWriter stream)
        {
            base.SaveState(stream);
            stream.Write(addr);
            stream.Write(chr_reg);
        }
        public override void LoadState(System.IO.BinaryReader stream)
        {
            base.LoadState(stream);
            addr = stream.ReadInt32();
            chr_reg = stream.ReadInt32();
        }
    }
}
