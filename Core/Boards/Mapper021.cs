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
    [BoardInfo("VRC4", 21)]
    class Mapper021 : Board
    {
        private bool prg_mode;
        private byte prg_reg0;
        private int[] chr_Reg;
        private int irq_reload;
        private int irq_counter;
        private int prescaler;
        private bool irq_mode_cycle;
        private bool irq_enable;
        private bool irq_enable_on_ak;

        public override void HardReset()
        {
            base.HardReset();
            base.Switch16KPRG(prg_16K_rom_mask, 0xC000, true);
            prescaler = 341;
            chr_Reg = new int[8];
        }
        public override void WritePRG(ref int address, ref byte data)
        {
            switch (address)
            {
                case 0x8000:
                case 0x8002:
                case 0x8004:
                case 0x8006:
                case 0x8040:
                case 0x8080:
                case 0x80C0:
                    {
                        prg_reg0 = data;
                        base.Switch08KPRG(prg_mode ? (prg_08K_rom_mask - 1) : (prg_reg0 & 0x1F), 0x8000, true);
                        base.Switch08KPRG(prg_mode ? (prg_reg0 & 0x1F) : (prg_08K_rom_mask - 1), 0xC000, true);
                        break;
                    }
                case 0x9000:
                case 0x9002:
                case 0x9040:
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
                case 0x9004:
                case 0x9006:
                case 0x9080:
                case 0x90C0:
                    {
                        prg_mode = (data & 0x2) == 0x2;
                        base.Switch08KPRG(prg_mode ? (prg_08K_rom_mask - 1) : (prg_reg0 & 0x1F), 0x8000, true);
                        base.Switch08KPRG(prg_mode ? (prg_reg0 & 0x1F) : (prg_08K_rom_mask - 1), 0xC000, true);
                        break;
                    }

                case 0xA000:
                case 0xA002:
                case 0xA004:
                case 0xA006:
                case 0xA040:
                case 0xA080:
                case 0xA0C0:
                    {
                        base.Switch08KPRG(data & 0x1F, 0xA000, true);
                        break;
                    }
                case 0xB000: chr_Reg[0] = (chr_Reg[0] & 0xF0) | (data & 0xF);/*****/ Switch01KCHR(chr_Reg[0], 0x0000, chr_01K_rom_count > 0); break;

                case 0xB002:
                case 0xB040: chr_Reg[0] = (chr_Reg[0] & 0x0F) | ((data & 0xF) << 4); Switch01KCHR(chr_Reg[0], 0x0000, chr_01K_rom_count > 0); break;

                case 0xB004:
                case 0xB080: chr_Reg[1] = (chr_Reg[1] & 0xF0) | (data & 0xF);/*****/ Switch01KCHR(chr_Reg[1], 0x0400, chr_01K_rom_count > 0); break;

                case 0xB006:
                case 0xB0C0: chr_Reg[1] = (chr_Reg[1] & 0x0F) | ((data & 0xF) << 4); Switch01KCHR(chr_Reg[1], 0x0400, chr_01K_rom_count > 0); break;

                case 0xC000: chr_Reg[2] = (chr_Reg[2] & 0xF0) | (data & 0xF);/*****/ Switch01KCHR(chr_Reg[2], 0x0800, chr_01K_rom_count > 0); break;

                case 0xC002:
                case 0xC040: chr_Reg[2] = (chr_Reg[2] & 0x0F) | ((data & 0xF) << 4); Switch01KCHR(chr_Reg[2], 0x0800, chr_01K_rom_count > 0); break;

                case 0xC004:
                case 0xC080: chr_Reg[3] = (chr_Reg[3] & 0xF0) | (data & 0xF);/*****/ Switch01KCHR(chr_Reg[3], 0x0C00, chr_01K_rom_count > 0); break;

                case 0xC006:
                case 0xC0C0: chr_Reg[3] = (chr_Reg[3] & 0x0F) | ((data & 0xF) << 4); Switch01KCHR(chr_Reg[3], 0x0C00, chr_01K_rom_count > 0); break;

                case 0xD000: chr_Reg[4] = (chr_Reg[4] & 0xF0) | (data & 0xF);/*****/ Switch01KCHR(chr_Reg[4], 0x1000, chr_01K_rom_count > 0); break;

                case 0xD002:
                case 0xD040: chr_Reg[4] = (chr_Reg[4] & 0x0F) | ((data & 0xF) << 4); Switch01KCHR(chr_Reg[4], 0x1000, chr_01K_rom_count > 0); break;

                case 0xD004:
                case 0xD080: chr_Reg[5] = (chr_Reg[5] & 0xF0) | (data & 0xF);/*****/ Switch01KCHR(chr_Reg[5], 0x1400, chr_01K_rom_count > 0); break;

                case 0xD006:
                case 0xD0C0: chr_Reg[5] = (chr_Reg[5] & 0x0F) | ((data & 0xF) << 4); Switch01KCHR(chr_Reg[5], 0x1400, chr_01K_rom_count > 0); break;

                case 0xE000: chr_Reg[6] = (chr_Reg[6] & 0xF0) | (data & 0xF);/*****/ Switch01KCHR(chr_Reg[6], 0x1800, chr_01K_rom_count > 0); break;

                case 0xE002:
                case 0xE040: chr_Reg[6] = (chr_Reg[6] & 0x0F) | ((data & 0xF) << 4); Switch01KCHR(chr_Reg[6], 0x1800, chr_01K_rom_count > 0); break;

                case 0xE004:
                case 0xE080: chr_Reg[7] = (chr_Reg[7] & 0xF0) | (data & 0xF);/*****/ Switch01KCHR(chr_Reg[7], 0x1C00, chr_01K_rom_count > 0); break;

                case 0xE006:
                case 0xE0C0: chr_Reg[7] = (chr_Reg[7] & 0x0F) | ((data & 0xF) << 4); Switch01KCHR(chr_Reg[7], 0x1C00, chr_01K_rom_count > 0); break;

                case 0xF000: irq_reload = (irq_reload & 0xF0) | (data & 0xF);/*****/ break;

                case 0xF002:
                case 0xF040: irq_reload = (irq_reload & 0x0F) | ((data & 0xF) << 4); break;

                case 0xF004:
                case 0xF080:
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

                case 0xF006:
                case 0xF0C0: NesEmu.IRQFlags &= ~NesEmu.IRQ_BOARD; irq_enable = irq_enable_on_ak; break;
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
            stream.Write(prg_mode);
            stream.Write(prg_reg0);
            for (int i = 0; i < chr_Reg.Length; i++)
                stream.Write(chr_Reg[i]);
            stream.Write(irq_reload);
            stream.Write(irq_counter);
            stream.Write(prescaler);
            stream.Write(irq_mode_cycle);
            stream.Write(irq_enable);
            stream.Write(irq_enable_on_ak);
        }
        public override void LoadState(System.IO.BinaryReader stream)
        {
            base.LoadState(stream);
            prg_mode = stream.ReadBoolean();
            prg_reg0 = stream.ReadByte();
            for (int i = 0; i < chr_Reg.Length; i++)
                chr_Reg[i] = stream.ReadInt32();
            irq_reload = stream.ReadInt32();
            irq_counter = stream.ReadInt32();
            prescaler = stream.ReadInt32();
            irq_mode_cycle = stream.ReadBoolean();
            irq_enable = stream.ReadBoolean();
            irq_enable_on_ak = stream.ReadBoolean();
        }
    }
}
