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
    [BoardInfo("FDS-Port - Alt. Levels", 50)]
    class Mapper050 : Board
    {
        private int prg_page;
        private int irq_counter;
        private bool irq_enabled;
        public override void HardReset()
        {
            base.HardReset();
            base.Switch08KPRG(0xF, 0x6000, true);
            base.Switch08KPRG(0x8, 0x8000, true);
            base.Switch08KPRG(0x9, 0xA000, true);
            //base.Switch08KPRG(0x0, 0xC000, true);
            base.Switch08KPRG(0xB, 0xE000, true);
        }
        public override void WriteEXP(ref int address, ref byte data)
        {
            switch (address & 0x4120)
            {
                case 0x4020:
                    {
                        prg_page = (data & 0x8) | ((data & 0x1) << 2) | ((data >> 1) & 0x3);
                        Switch08KPRG(prg_page, 0xC000, true);
                        break;
                    }
                case 0x4120:
                    {
                        irq_enabled = (data & 1) == 1;
                        if (!irq_enabled)
                        {
                            irq_counter = 0;
                            NesEmu.IRQFlags &= ~NesEmu.IRQ_BOARD;
                        }
                        break;
                    }
            }
        }
        public override void OnCPUClock()
        {
            if (irq_enabled)
            {
                irq_counter++;
                if (irq_counter == 0x1000)
                {
                    NesEmu.IRQFlags |= NesEmu.IRQ_BOARD;
                    irq_counter = 0;
                }
            }
        }
        public override void SaveState(System.IO.BinaryWriter stream)
        {
            base.SaveState(stream);
            stream.Write(prg_page);
            stream.Write(irq_counter);
            stream.Write(irq_enabled);
        }
        public override void LoadState(System.IO.BinaryReader stream)
        {
            base.LoadState(stream);
            prg_page = stream.ReadInt32();
            irq_counter = stream.ReadInt32();
            irq_enabled = stream.ReadBoolean();
        }
    }
}
