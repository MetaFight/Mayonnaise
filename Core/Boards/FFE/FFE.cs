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
    abstract class FFE : Board
    {
        protected bool irqEnable;
        protected int irqCounter;
        public override void Initialize(string sha1, byte[] prg_dump, byte[] chr_dump, byte[] trainer_dump, MyNes.Core.Mirroring defaultMirroring)
        {
            base.Initialize(sha1, prg_dump, chr_dump, trainer_dump, defaultMirroring);
            // Copy trainer
            if (prg_isram[0])
                trainer_dump.CopyTo(prg_banks[0], 0x0000);
        }
        public override void WriteEXP(ref int address, ref byte data)
        {
            switch (address)
            {
                case 0x4501:
                    {
                        irqEnable = false;
                        NesEmu.IRQFlags &= ~NesEmu.IRQ_BOARD;
                        break;
                    }
                case 0x4502:
                    {
                        irqCounter = (irqCounter & 0xFF00) | data;
                        break;
                    }
                case 0x4503:
                    {
                        irqEnable = true;
                        irqCounter = (irqCounter & 0x00FF) | (data << 8);
                        break;
                    }
            }
        }
        public override void OnCPUClock()
        {
            if (irqEnable)
            {
                irqCounter++;
                if (irqCounter >= 0xFFFF)
                {
                    irqCounter = 0;
                    NesEmu.IRQFlags |= NesEmu.IRQ_BOARD;
                }
            }
        }
        public override void SaveState(System.IO.BinaryWriter stream)
        {
            base.SaveState(stream);
            stream.Write(irqEnable);
            stream.Write(irqCounter);
        }
        public override void LoadState(System.IO.BinaryReader stream)
        {
            base.LoadState(stream);
            irqEnable = stream.ReadBoolean();
            irqCounter = stream.ReadInt32();
        }
    }
}
