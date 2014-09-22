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
    [BoardInfo("Irem H-3001", 65)]
    class Mapper065 : Board
    {
        private bool irq_enable;
        private int irq_reload;
        private int irq_counter;
        public override void HardReset()
        {
            base.HardReset();
            Switch08KPRG(0x00, 0x8000, true);
            Switch08KPRG(0x01, 0xA000, true);
            Switch08KPRG(0xFE, 0xC000, true);
            Switch08KPRG(prg_08K_rom_mask, 0xE000, true);
        }
        public override void WritePRG(ref int address, ref byte data)
        {
            switch (address)
            {
                case 0x8000: Switch08KPRG(data, 0x8000, true); break;
                case 0x9001: SwitchNMT((data & 0x80) == 0x80 ? Mirroring.Horz : Mirroring.Vert); break;
                case 0x9003: irq_enable = (data & 0x80) == 0x80; NesEmu.IRQFlags &= ~NesEmu.IRQ_BOARD; break;
                case 0x9004: irq_counter = irq_reload; NesEmu.IRQFlags &= ~NesEmu.IRQ_BOARD; break;
                case 0x9005: irq_reload = (irq_reload & 0x00FF) | (data << 8); break;
                case 0x9006: irq_reload = (irq_reload & 0xFF00) | data; break;
                case 0xA000: Switch08KPRG(data, 0xA000, true); break;
                case 0xC000: Switch08KPRG(data, 0xC000, true); break;
                case 0xB000: Switch01KCHR(data, 0x0000, chr_01K_rom_count > 0); break;
                case 0xB001: Switch01KCHR(data, 0x0400, chr_01K_rom_count > 0); break;
                case 0xB002: Switch01KCHR(data, 0x0800, chr_01K_rom_count > 0); break;
                case 0xB003: Switch01KCHR(data, 0x0C00, chr_01K_rom_count > 0); break;
                case 0xB004: Switch01KCHR(data, 0x1000, chr_01K_rom_count > 0); break;
                case 0xB005: Switch01KCHR(data, 0x1400, chr_01K_rom_count > 0); break;
                case 0xB006: Switch01KCHR(data, 0x1800, chr_01K_rom_count > 0); break;
                case 0xB007: Switch01KCHR(data, 0x1C00, chr_01K_rom_count > 0); break;
            }
        }
        public override void OnCPUClock()
        {
            if (irq_enable)
            {
                if (irq_counter > 0)
                    irq_counter--;
                else if (irq_counter == 0)
                {
                    irq_counter = -1;
                    NesEmu.IRQFlags |= NesEmu.IRQ_BOARD;
                }
            }
        }
    }
}
