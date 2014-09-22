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
    [BoardInfo("Unknown", 207)]
    [NotImplementedWell("Mapper 207\nFudou Myouou Den is not assigned as mapper 207 while it should be !")]
    class Mapper207 : Board
    {
        private int mirroring0;
        private int mirroring1;
        public override void HardReset()
        {
            base.HardReset();
            Switch08KPRG(prg_08K_rom_mask, 0xE000, true);
        }
        public override void WriteSRM(ref int address, ref byte data)
        {
            switch (address)
            {
                case 0x7EF0:
                    {
                        Switch02KCHR(data & 0x7F >> 1, 0x0000, chr_01K_rom_count > 0);
                        mirroring0 = data >> 7 & 1;
                        break;
                    }
                case 0x7EF1:
                    {
                        Switch02KCHR(data & 0x7F >> 1, 0x0800, chr_01K_rom_count > 0);
                        mirroring1 = data >> 7 & 1;
                        break;
                    }
                case 0x7EF2: Switch01KCHR(data, 0x1000, chr_01K_rom_count > 0); break;
                case 0x7EF3: Switch01KCHR(data, 0x1400, chr_01K_rom_count > 0); break;
                case 0x7EF4: Switch01KCHR(data, 0x1800, chr_01K_rom_count > 0); break;
                case 0x7EF5: Switch01KCHR(data, 0x1C00, chr_01K_rom_count > 0); break;
                case 0x7EFA:
                case 0x7EFB: Switch08KPRG(data, 0x8000, true); break;
                case 0x7EFC:
                case 0x7EFD: Switch08KPRG(data, 0xA000, true); break;
                case 0x7EFE:
                case 0x7EFF: Switch08KPRG(data, 0xC000, true); break;
            }
        }
        public override byte ReadNMT(ref int address)
        {
            switch ((address >> 10) & 3)
            {
                case 0:
                case 1: return nmt_banks[mirroring0][address & 0x3FF];
                case 2:
                case 3: return nmt_banks[mirroring1][address & 0x3FF];
            }
            return 0;// Make compiler happy.
        }
        public override void WriteNMT(ref int address, ref byte data)
        {
            switch ((address >> 10) & 3)
            {
                case 0:
                case 1: nmt_banks[mirroring0][address & 0x3FF] = data; break;
                case 2:
                case 3: nmt_banks[mirroring1][address & 0x3FF] = data; break;
            }
        }
        public override void SaveState(System.IO.BinaryWriter stream)
        {
            base.SaveState(stream);
            stream.Write(mirroring0);
            stream.Write(mirroring1);
        }
        public override void LoadState(System.IO.BinaryReader stream)
        {
            base.LoadState(stream);
            mirroring0 = stream.ReadInt32();
            mirroring1 = stream.ReadInt32();
        }
    }
}
