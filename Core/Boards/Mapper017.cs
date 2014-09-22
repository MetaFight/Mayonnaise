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
    [BoardInfo("FFE F8xxx", 17)]
    class Mapper017 : FFE
    {
        public override void HardReset()
        {
            base.HardReset();
            Switch16KPRG(prg_16K_rom_mask, 0xC000, true);
        }
        public override void WriteEXP(ref int address, ref byte data)
        {
            switch (address)
            {
                case 0x4504: base.Switch08KPRG(data, 0x8000, true); break;
                case 0x4505: base.Switch08KPRG(data, 0xA000, true); break;
                case 0x4506: base.Switch08KPRG(data, 0xC000, true); break;
                case 0x4507: base.Switch08KPRG(data, 0xE000, true); break;
                case 0x4510: base.Switch01KCHR(data, 0x0000, chr_01K_rom_count > 0); break;
                case 0x4511: base.Switch01KCHR(data, 0x0400, chr_01K_rom_count > 0); break;
                case 0x4512: base.Switch01KCHR(data, 0x0800, chr_01K_rom_count > 0); break;
                case 0x4513: base.Switch01KCHR(data, 0x0C00, chr_01K_rom_count > 0); break;
                case 0x4514: base.Switch01KCHR(data, 0x1000, chr_01K_rom_count > 0); break;
                case 0x4515: base.Switch01KCHR(data, 0x1400, chr_01K_rom_count > 0); break;
                case 0x4516: base.Switch01KCHR(data, 0x1800, chr_01K_rom_count > 0); break;
                case 0x4517: base.Switch01KCHR(data, 0x1C00, chr_01K_rom_count > 0); break;
            }
        }
    }
}
