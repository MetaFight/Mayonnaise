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
    [BoardInfo("Namco 118", 88)]
    class Mapper088 : Board
    {
        private int address_8001;
        public override void HardReset()
        {
            base.HardReset();
            Switch16KPRG(prg_16K_rom_mask, 0xC000, true);
        }
        public override void WritePRG(ref int address, ref byte data)
        {
            switch (address & 0x8001)
            {
                case 0x8000: address_8001 = data & 0x7; break;
                case 0x8001:
                    {
                        switch (address_8001)
                        {
                            case 0x0: Switch02KCHR((data & 0x3F) >> 1, 0x0000, chr_01K_rom_count > 0); break;
                            case 0x1: Switch02KCHR((data & 0x3F) >> 1, 0x0800, chr_01K_rom_count > 0); break;
                            case 0x2: Switch01KCHR(data | 0x40, 0x1000, chr_01K_rom_count > 0); break;
                            case 0x3: Switch01KCHR(data | 0x40, 0x1400, chr_01K_rom_count > 0); break;
                            case 0x4: Switch01KCHR(data | 0x40, 0x1800, chr_01K_rom_count > 0); break;
                            case 0x5: Switch01KCHR(data | 0x40, 0x1C00, chr_01K_rom_count > 0); break;
                            case 0x6: Switch08KPRG(data, 0x8000, true); break;
                            case 0x7: Switch08KPRG(data, 0xA000, true); break;
                        }
                        break;
                    }
            }
        }
        public override void SaveState(System.IO.BinaryWriter stream)
        {
            base.SaveState(stream);
            stream.Write(address_8001);
        }
        public override void LoadState(System.IO.BinaryReader stream)
        {
            base.LoadState(stream);
            address_8001 = stream.ReadInt32();
        }
    }
}
