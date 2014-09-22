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
    // TODO: Add support for VRC7 sound channels.
    [BoardInfo("VRC7", 85)]
    [NotImplementedWell("Mapper 85\nVRC7 sound channels not implemented !")]
    class Mapper085 : Board
    {
        private int irq_reload;
        private int irq_counter;
        private int prescaler;
        private bool irq_mode_cycle;
        private bool irq_enable;
        private bool irq_enable_on_ak;
        public override void HardReset()
        {
            base.HardReset();
            base.Switch08KPRG(prg_08K_rom_mask, 0xE000, true);
            irq_reload = 0;
            prescaler = 341;
            irq_counter = 0;
            irq_mode_cycle = false;
            irq_enable = false;
            irq_enable_on_ak = false;
        }
        public override void WritePRG(ref int address, ref byte data)
        {
            switch (address)
            {
                case 0x8000: Switch08KPRG(data, 0x8000, true); break;
                case 0x8008:
                case 0x8010: Switch08KPRG(data, 0xA000, true); break;
                case 0x9000: Switch08KPRG(data, 0xC000, true); break;
                case 0xA000: Switch01KCHR(data, 0x0000, chr_01K_rom_count > 0); break;
                case 0xA008:
                case 0xA010: Switch01KCHR(data, 0x0400, chr_01K_rom_count > 0); break;
                case 0xB000: Switch01KCHR(data, 0x0800, chr_01K_rom_count > 0); break;
                case 0xB008:
                case 0xB010: Switch01KCHR(data, 0x0C00, chr_01K_rom_count > 0); break;
                case 0xC000: Switch01KCHR(data, 0x1000, chr_01K_rom_count > 0); break;
                case 0xC008:
                case 0xC010: Switch01KCHR(data, 0x1400, chr_01K_rom_count > 0); break;
                case 0xD000: Switch01KCHR(data, 0x1800, chr_01K_rom_count > 0); break;
                case 0xD008:
                case 0xD010: Switch01KCHR(data, 0x1C00, chr_01K_rom_count > 0); break;
                case 0xE000:
                    {
                        switch (data & 0x3)
                        {
                            case 0: SwitchNMT(Mirroring.Vert); break;
                            case 1: SwitchNMT(Mirroring.Horz); break;
                            case 2: SwitchNMT(Mirroring.OneScA); break;
                            case 3: SwitchNMT(Mirroring.OneScB); break;
                        }
                        break;
                    }
                case 0xE008:
                case 0xE010: irq_reload = data; break;
                case 0xF000:
                    {
                        irq_mode_cycle = (data & 0x4) == 0x4;
                        irq_enable = (data & 0x2) == 0x2;
                        irq_enable_on_ak = (data & 0x1) == 0x1;
                        if (irq_enable)
                        {
                            irq_counter = irq_reload;
                            prescaler = 341;
                        }
                        NesEmu.IRQFlags &= ~NesEmu.IRQ_BOARD;
                        break;
                    }
                case 0xF008:
                case 0xF010: NesEmu.IRQFlags &= ~NesEmu.IRQ_BOARD; irq_enable = irq_enable_on_ak; break;
            }
        }
        public override void OnCPUClock()
        {
            if (irq_enable)
            {
                if (!irq_mode_cycle)
                {
                    if (prescaler > 0)
                        prescaler -= 3;
                    else
                    {
                        prescaler = 341;
                        irq_counter++;
                        if (irq_counter == 0xFF)
                        {
                            NesEmu.IRQFlags |= NesEmu.IRQ_BOARD;
                            irq_counter = irq_reload;
                        }
                    }
                }
                else
                {
                    irq_counter++;
                    if (irq_counter == 0xFF)
                    {
                        NesEmu.IRQFlags |= NesEmu.IRQ_BOARD;
                        irq_counter = irq_reload;
                    }
                }
            }
        }
        public override void SaveState(System.IO.BinaryWriter stream)
        {
            base.SaveState(stream);
            stream.Write(prescaler);
            stream.Write(irq_counter);
            stream.Write(irq_mode_cycle);
            stream.Write(irq_reload);
            stream.Write(irq_enable);
            stream.Write(irq_enable_on_ak);
        }
        public override void LoadState(System.IO.BinaryReader stream)
        {
            base.LoadState(stream);
            prescaler = stream.ReadInt32();
            irq_counter = stream.ReadInt32();
            irq_mode_cycle = stream.ReadBoolean();
            irq_reload = stream.ReadInt32();
            irq_enable = stream.ReadBoolean();
            irq_enable_on_ak = stream.ReadBoolean();
        }
    }
}
