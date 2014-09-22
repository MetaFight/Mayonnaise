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
    [BoardInfo("HK-SF3", 91, true, true)]
    class Mapper091 : Board
    {
        // IRQ
        private bool irq_enabled;
        private byte irq_counter;
        private int old_irq_counter;
        private byte irq_reload;
        private bool irq_clear;
        public override void HardReset()
        {
            base.HardReset();
            Switch16KPRG(prg_16K_rom_mask, 0xC000, true);
        }
        public override void WriteSRM(ref int address, ref byte data)
        {
            switch (address & 0x7003)
            {
                case 0x6000: Switch02KCHR(data, 0x0000, chr_01K_rom_count > 0); break;
                case 0x6001: Switch02KCHR(data, 0x0800, chr_01K_rom_count > 0); break;
                case 0x6002: Switch02KCHR(data, 0x1000, chr_01K_rom_count > 0); break;
                case 0x6003: Switch02KCHR(data, 0x1800, chr_01K_rom_count > 0); break;
                case 0x7000: Switch08KPRG(data & 0xF, 0x8000, true); break;
                case 0x7001: Switch08KPRG(data & 0xF, 0xA000, true); break;
                case 0x7002: irq_enabled = false; NesEmu.IRQFlags &= ~NesEmu.IRQ_BOARD; break;
                case 0x7003:
                    {
                        irq_enabled = true;
                        irq_reload = 0x7;
                        irq_counter = 0;
                        break;
                    }
            }
        }
        public override void OnPPUA12RaisingEdge()
        {
            old_irq_counter = irq_counter;

            if (irq_counter == 0 || irq_clear)
                irq_counter = irq_reload;
            else
                irq_counter = (byte)(irq_counter - 1);

            if ((old_irq_counter != 0 || irq_clear) && irq_counter == 0 && irq_enabled)
                NesEmu.IRQFlags |= NesEmu.IRQ_BOARD;

            irq_clear = false;
        }
        public override void SaveState(System.IO.BinaryWriter stream)
        {
            base.SaveState(stream);
            stream.Write(irq_enabled);
            stream.Write(irq_counter);
            stream.Write(old_irq_counter);
            stream.Write(irq_reload);
            stream.Write(irq_clear);
        }
        public override void LoadState(System.IO.BinaryReader stream)
        {
            base.LoadState(stream);
            irq_enabled = stream.ReadBoolean();
            irq_counter = stream.ReadByte();
            old_irq_counter = stream.ReadInt32();
            irq_reload = stream.ReadByte();
            irq_clear = stream.ReadBoolean();
        }
    }
}
