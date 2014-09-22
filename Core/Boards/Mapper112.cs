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
    [BoardInfo("Asder", 112)]
    class Mapper112 : Board
    {
        private int address_A000;

        public override void HardReset()
        {
            base.HardReset();
            Switch16KPRG(prg_16K_rom_mask, 0xC000, true);
        }
        public override void WritePRG(ref int address, ref byte data)
        {
            switch (address & 0xE001)
            {
                case 0x8000:
                    {
                        address_A000 = data & 0x7;
                        break;
                    }
                case 0xA000:
                    {
                        switch (address_A000)
                        {
                            case 0: Switch08KPRG(data, 0x8000, true); break;
                            case 1: Switch08KPRG(data, 0xA000, true); break;
                            case 2: Switch02KCHR(data >> 1, 0x0000, chr_01K_rom_count > 0); break;
                            case 3: Switch02KCHR(data >> 1, 0x0800, chr_01K_rom_count > 0); break;
                            case 4: Switch01KCHR(data, 0x1000, chr_01K_rom_count > 0); break;
                            case 5: Switch01KCHR(data, 0x1400, chr_01K_rom_count > 0); break;
                            case 6: Switch01KCHR(data, 0x1800, chr_01K_rom_count > 0); break;
                            case 7: Switch01KCHR(data, 0x1C00, chr_01K_rom_count > 0); break;
                        }
                        break;
                    }
                case 0xE000: base.SwitchNMT((data & 1) == 1 ? Mirroring.Horz : Mirroring.Vert); break;

            }
        }
        public override void SaveState(System.IO.BinaryWriter stream)
        {
            base.SaveState(stream);
            stream.Write(address_A000);
        }
        public override void LoadState(System.IO.BinaryReader stream)
        {
            base.LoadState(stream);
            address_A000 = stream.ReadInt32();
        }
    }
}
