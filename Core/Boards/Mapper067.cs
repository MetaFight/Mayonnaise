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
    [BoardInfo("Sunsoft 3", 67)]
    class Mapper067 : Board
    {
        private bool irq_enabled;
        private int irq_counter;
        private bool odd;
        public override void HardReset()
        {
            base.HardReset();
            Switch16KPRG(prg_16K_rom_mask, 0xC000, true);
            irq_enabled = false;
            irq_counter = 0xFFFF;
            odd = false;
        }
        public override void WritePRG(ref int address, ref byte data)
        {
            switch (address & 0xF800)
            {
                case 0x8800: Switch02KCHR(data, 0x0000, chr_01K_rom_count > 0); break;
                case 0x9800: Switch02KCHR(data, 0x0800, chr_01K_rom_count > 0); break;
                case 0xA800: Switch02KCHR(data, 0x1000, chr_01K_rom_count > 0); break;
                case 0xB800: Switch02KCHR(data, 0x1800, chr_01K_rom_count > 0); break;
                case 0xC800:
                    {
                        if (!odd)
                            irq_counter = (irq_counter & 0x00FF) | (data << 8);
                        else
                            irq_counter = (irq_counter & 0xFF00) | data;
                        odd = !odd;
                        break;
                    }
                case 0xD800:
                    {
                        irq_enabled = (data & 0x10) == 0x10;
                        odd = false;
                        NesEmu.IRQFlags &= ~NesEmu.IRQ_BOARD;
                        break;
                    }
                case 0xE800:
                    {
                        switch (data & 3)
                        {
                            case 0: SwitchNMT(Mirroring.Vert); break;
                            case 1: SwitchNMT(Mirroring.Horz); break;
                            case 2: SwitchNMT(Mirroring.OneScA); break;
                            case 3: SwitchNMT(Mirroring.OneScB); break;
                        }
                        break;
                    }
                case 0xF800: Switch16KPRG(data, 0x8000, true); break;
            }
        }
        public override void OnCPUClock()
        {
            if (irq_enabled)
            {
                irq_counter--;
                if (irq_counter == 0)
                {
                    irq_counter = 0xFFFF;
                    NesEmu.IRQFlags |= NesEmu.IRQ_BOARD;
                    irq_enabled = false;
                }
            }
        }
        public override void SaveState(System.IO.BinaryWriter stream)
        {
            base.SaveState(stream);
            stream.Write(irq_enabled);
            stream.Write(irq_counter);
            stream.Write(odd);
        }
        public override void LoadState(System.IO.BinaryReader stream)
        {
            base.LoadState(stream);
            irq_enabled = stream.ReadBoolean();
            irq_counter = stream.ReadInt32();
            odd = stream.ReadBoolean();
        }
    }
}
