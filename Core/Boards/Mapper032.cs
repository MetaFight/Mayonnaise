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
    [BoardInfo("Irem G-101", 32)]
    class Mapper032 : Board
    {
        private bool prg_mode;
        private byte prg_reg0;
        private bool enable_mirroring_switch;
        public override void HardReset()
        {
            base.HardReset();
            enable_mirroring_switch = true;
            // This is not a hack !! this is from mapper docs v0.6.1 by Disch, 032.txt:
            /*"Major League seems to want hardwired 1-screen mirroring.  As far as I know, there is no seperate mapper
               number assigned to address this issue, so you'll have to rely on a CRC or hash check or something for
               treating Major League as a special case."
             */
            if (RomSHA1 == "7E4180432726A433C46BA2206D9E13B32761C11E")
            { enable_mirroring_switch = false; SwitchNMT(Mirroring.OneScA); }

            base.Switch08KPRG(prg_08K_rom_mask, 0xE000, true);
        }
        public override void WritePRG(ref int address, ref byte data)
        {
            switch (address & 0xF007)
            {
                case 0x8000:
                case 0x8001:
                case 0x8002:
                case 0x8003:
                case 0x8004:
                case 0x8005:
                case 0x8006:
                case 0x8007:
                    {
                        prg_reg0 = data;
                        base.Switch08KPRG(prg_mode ? 0 : prg_reg0, 0x8000, true);
                        base.Switch08KPRG(prg_mode ? prg_reg0 : (prg_08K_rom_mask - 1), 0xC000, true);
                        break;
                    }
                case 0x9000:
                case 0x9001:
                case 0x9002:
                case 0x9003:
                case 0x9004:
                case 0x9005:
                case 0x9006:
                case 0x9007:
                    {
                        prg_mode = (data & 0x2) == 0x2;
                        base.Switch08KPRG(prg_mode ? 0 : prg_reg0, 0x8000, true);
                        base.Switch08KPRG(prg_mode ? prg_reg0 : (prg_08K_rom_mask - 1), 0xC000, true);
                        if (enable_mirroring_switch)
                            SwitchNMT((data & 1) == 1 ? Mirroring.Horz : Mirroring.Vert);
                        break;
                    }
                case 0xA000:
                case 0xA001:
                case 0xA002:
                case 0xA003:
                case 0xA004:
                case 0xA005:
                case 0xA006:
                case 0xA007: base.Switch08KPRG(data, 0xA000, true); break;
                case 0xB000: base.Switch01KCHR(data, 0x0000, chr_01K_rom_count > 0); break;
                case 0xB001: base.Switch01KCHR(data, 0x0400, chr_01K_rom_count > 0); break;
                case 0xB002: base.Switch01KCHR(data, 0x0800, chr_01K_rom_count > 0); break;
                case 0xB003: base.Switch01KCHR(data, 0x0C00, chr_01K_rom_count > 0); break;
                case 0xB004: base.Switch01KCHR(data, 0x1000, chr_01K_rom_count > 0); break;
                case 0xB005: base.Switch01KCHR(data, 0x1400, chr_01K_rom_count > 0); break;
                case 0xB006: base.Switch01KCHR(data, 0x1800, chr_01K_rom_count > 0); break;
                case 0xB007: base.Switch01KCHR(data, 0x1C00, chr_01K_rom_count > 0); break;
            }
        }
        public override void SaveState(System.IO.BinaryWriter stream)
        {
            base.SaveState(stream);
            stream.Write(prg_mode);
            stream.Write(prg_reg0);
        }
        public override void LoadState(System.IO.BinaryReader stream)
        {
            base.LoadState(stream);
            prg_mode = stream.ReadBoolean();
            prg_reg0 = stream.ReadByte();
        }
    }
}
