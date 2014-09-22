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
    abstract class MMC2 : Board
    {
        public override void HardReset()
        {
            base.HardReset();
            base.Switch08KPRG(prg_08K_rom_mask - 2, 0xA000, true);
            base.Switch08KPRG(prg_08K_rom_mask - 1, 0xC000, true);
            base.Switch08KPRG(prg_08K_rom_mask, 0xE000, true);
            chr_reg0B = 4;
        }
        private byte chr_reg0A;
        private byte chr_reg0B;
        private byte chr_reg1A;
        private byte chr_reg1B;
        private byte latch_a = 0xFE;
        private byte latch_b = 0xFE;

        public override void WritePRG(ref int address, ref byte data)
        {
            switch (address & 0xF000)
            {
                case 0xA000:
                    {
                        base.Switch08KPRG(data, 0x8000, true);
                        break;
                    }
                case 0xB000:
                    {
                        chr_reg0A = data;
                        if (latch_a == 0xFD)
                            base.Switch04KCHR(chr_reg0A, 0x0000, chr_01K_rom_count > 0);
                        break;
                    }
                case 0xC000:
                    {
                        chr_reg0B = data;
                        if (latch_a == 0xFE)
                            base.Switch04KCHR(chr_reg0B, 0x0000, chr_01K_rom_count > 0);
                        break;
                    }
                case 0xD000:
                    {
                        chr_reg1A = data;
                        if (latch_b == 0xFD)
                            base.Switch04KCHR(chr_reg1A, 0x1000, chr_01K_rom_count > 0);
                        break;
                    }
                case 0xE000:
                    {
                        chr_reg1B = data;
                        if (latch_b == 0xFE)
                            base.Switch04KCHR(chr_reg1B, 0x1000, chr_01K_rom_count > 0);
                        break;
                    }
                case 0xF000:
                    {
                        base.SwitchNMT((data & 0x1) == 0x1 ? Mirroring.Horz : Mirroring.Vert);
                        break;
                    }
            }
        }
        public override byte ReadCHR(ref int address, bool spriteFetch)
        {
            if ((address & 0x1FF0) == 0x0FD0 && latch_a != 0xFD)
            {
                latch_a = 0xFD;
                base.Switch04KCHR(chr_reg0A, 0x0000, chr_01K_rom_count > 0);
            }
            else if ((address & 0x1FF0) == 0x0FE0 && latch_a != 0xFE)
            {
                latch_a = 0xFE;
                base.Switch04KCHR(chr_reg0B, 0x0000, chr_01K_rom_count > 0);
            }
            else if ((address & 0x1FF0) == 0x1FD0 && latch_b != 0xFD)
            {
                latch_b = 0xFD;
                base.Switch04KCHR(chr_reg1A, 0x1000, chr_01K_rom_count > 0);
            }
            else if ((address & 0x1FF0) == 0x1FE0 && latch_b != 0xFE)
            {
                latch_b = 0xFE;
                base.Switch04KCHR(chr_reg1B, 0x1000, chr_01K_rom_count > 0);
            }
            return base.ReadCHR(ref address, spriteFetch);
        }
        public override void SaveState(System.IO.BinaryWriter stream)
        {
            base.SaveState(stream);
            stream.Write(chr_reg0A);
            stream.Write(chr_reg0B);
            stream.Write(chr_reg1A);
            stream.Write(chr_reg1B);
            stream.Write(latch_a);
            stream.Write(latch_b);
        }
        public override void LoadState(System.IO.BinaryReader stream)
        {
            base.LoadState(stream);
            chr_reg0A = stream.ReadByte();
            chr_reg0B = stream.ReadByte();
            chr_reg1A = stream.ReadByte();
            chr_reg1B = stream.ReadByte();
            latch_a = stream.ReadByte();
            latch_b = stream.ReadByte();
        }
    }
}
