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
    abstract class Bandai : Board
    {
        private bool irq_enable;
        private int irq_counter;
        private Eprom eprom;

        public override void Initialize(string sha1, byte[] prg_dump, byte[] chr_dump, byte[] trainer_dump, MyNes.Core.Mirroring defaultMirroring)
        {
            base.Initialize(sha1, prg_dump, chr_dump, trainer_dump, defaultMirroring);
            if (BoardType.ToLower().Contains("24c01"))// mapper 159
            {
                eprom = new Eprom(128);
            }
            else
            {
                eprom = new Eprom(base.MapperNumber == 16 ? 256 : 128);
            }
        }
        public override void HardReset()
        {
            base.HardReset();
            base.Switch16KPRG(prg_16K_rom_mask, 0xC000, true);
            irq_enable = false;
            irq_counter = 0;
            eprom.HardReset();
        }
        public override void WriteSRM(ref int address, ref byte data)
        {
            WritePRG(ref address, ref data);
        }
        public override void WritePRG(ref int address, ref byte data)
        {
            switch (address & 0x000F)
            {
                case 0x0: Switch01KCHR(data, 0x0000, chr_01K_rom_count > 0); break;
                case 0x1: Switch01KCHR(data, 0x0400, chr_01K_rom_count > 0); break;
                case 0x2: Switch01KCHR(data, 0x0800, chr_01K_rom_count > 0); break;
                case 0x3: Switch01KCHR(data, 0x0C00, chr_01K_rom_count > 0); break;
                case 0x4: Switch01KCHR(data, 0x1000, chr_01K_rom_count > 0); break;
                case 0x5: Switch01KCHR(data, 0x1400, chr_01K_rom_count > 0); break;
                case 0x6: Switch01KCHR(data, 0x1800, chr_01K_rom_count > 0); break;
                case 0x7: Switch01KCHR(data, 0x1C00, chr_01K_rom_count > 0); break;
                case 0x8: Switch16KPRG(data, 0x8000, true); break;
                case 0x9:
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
                case 0xA:
                    {
                        irq_enable = (data & 0x1) == 0x1; NesEmu.IRQFlags &= ~NesEmu.IRQ_BOARD;
                        break;
                    }
                case 0xB:
                    {
                        irq_counter = (irq_counter & 0xFF00) | data;
                        break;
                    }
                case 0xC:
                    {
                        irq_counter = (irq_counter & 0x00FF) | (data << 8);
                        break;
                    }
                case 0xD: eprom.Write(address, data); break;
            }
        }
        public override byte ReadSRM(ref int address)
        {
            return eprom.Read(address);
        }
        public override void OnCPUClock()
        {
            if (irq_enable)
            {
                irq_counter--;
                if (irq_counter == 0)
                {
                    NesEmu.IRQFlags |= NesEmu.IRQ_BOARD;
                }
                if (irq_counter < 0)
                {
                    irq_counter = 0xFFFF;
                }
            }
        }

        public override void SaveState(System.IO.BinaryWriter stream)
        {
            base.SaveState(stream);
            stream.Write(irq_enable);
            stream.Write(irq_counter);
            eprom.SaveState(stream);
        }
        public override void LoadState(System.IO.BinaryReader stream)
        {
            base.LoadState(stream);
            irq_enable = stream.ReadBoolean();
            irq_counter = stream.ReadInt32();
            eprom.LoadState(stream);
        }
    }
}
