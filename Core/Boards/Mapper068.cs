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
    [BoardInfo("Sunsoft 4", 68)]
    class Mapper068 : Board
    {
        private bool flag_r;
        private bool flag_m;
        private int nt_reg0, nt_reg1;
        private int temp;
        public override void HardReset()
        {
            base.HardReset();
            Switch16KPRG(prg_16K_rom_mask, 0xC000, true);
        }
        public override void WritePRG(ref int address, ref byte data)
        {
            switch (address & 0xF000)
            {
                case 0x8000: Switch02KCHR(data, 0x0000, chr_01K_rom_count > 0); break;
                case 0x9000: Switch02KCHR(data, 0x0800, chr_01K_rom_count > 0); break;
                case 0xA000: Switch02KCHR(data, 0x1000, chr_01K_rom_count > 0); break;
                case 0xB000: Switch02KCHR(data, 0x1800, chr_01K_rom_count > 0); break;
                case 0xC000: nt_reg0 = (data & 0x7F) | 0x80; break;
                case 0xD000: nt_reg1 = (data & 0x7F) | 0x80; break;
                case 0xE000:
                    {
                        flag_r = (data & 0x10) == 0x10;
                        flag_m = (data & 0x01) == 0x01;
                        SwitchNMT(flag_m ? Mirroring.Horz : Mirroring.Vert);
                        break;
                    }
                case 0xF000: Switch16KPRG(data, 0x8000, true); break;
            }
        }
        public override byte ReadNMT(ref int address)
        {
            if (!flag_r)
                return nmt_banks[nmt_indexes[(address >> 10) & 3]][address & 0x3FF];
            else
            {
                switch ((address >> 10) & 3)
                {
                    case 0: return chr_banks[nt_reg0 + chr_rom_bank_offset][address & 0x3FF];
                    case 1: return chr_banks[(flag_m ? nt_reg0 : nt_reg1) + chr_rom_bank_offset][address & 0x3FF];
                    case 2: return chr_banks[(flag_m ? nt_reg1 : nt_reg0) + chr_rom_bank_offset][address & 0x3FF];
                    case 3: return chr_banks[nt_reg1 + chr_rom_bank_offset][address & 0x3FF];
                }
            }
            return 0;// make compiler happy.
        }
        public override void WriteNMT(ref int address, ref byte data)
        {
            if (!flag_r)
                base.WriteNMT(ref address, ref data);
        }
        public override void SaveState(System.IO.BinaryWriter stream)
        {
            base.SaveState(stream);
            stream.Write(flag_r);
            stream.Write(flag_m);
            stream.Write(nt_reg0);
            stream.Write(nt_reg1);
            stream.Write(temp);
        }
        public override void LoadState(System.IO.BinaryReader stream)
        {
            base.LoadState(stream);
            flag_r = stream.ReadBoolean();
            flag_m = stream.ReadBoolean();
            nt_reg0 = stream.ReadInt32();
            nt_reg1 = stream.ReadInt32();
            temp = stream.ReadInt32();
        }
    }
}
