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
    [BoardInfo("Taito TC0190/TC0350", 33)]
    class Mapper033 : Board
    {
        private bool MODE;// Mapper 33 [TC0350FMR] mode ?
        private bool irq_enabled;
        private byte irq_counter;
        private int old_irq_counter;
        private byte irq_reload;
        private bool irq_clear;
        private bool mmc3_alt_behavior;

        public override void HardReset()
        {
            base.HardReset();
            base.Switch16KPRG(prg_16K_rom_mask, 0xC000, true);
            // This is not a hack, some games are mapper 48 and assigned as mapper 33
            // We need to confirm which type given game it is ...
            MODE = true;// Set as mapper 33 mode [board TC0350XXX]
            if (IsGameFoundOnDB)
            {
                foreach (string chip in Chips)
                {
                    if (chip.Contains("TC0190"))
                    {
                        // Board TC0190XXX mode, mapper 48 ....
                        MODE = false;
                        break;
                    }
                }
            }
            // IRQ
            irq_enabled = false;
            irq_counter = 0;
            irq_reload = 0xFF;
            old_irq_counter = 0;
            mmc3_alt_behavior = false;
            irq_clear = false;
        }
        public override void WritePRG(ref int address, ref byte data)
        {
            if (!MODE)
            {
                // Mapper 48 mode
                switch (address & 0xE003)
                {
                    case 0x8000: base.Switch08KPRG(data, 0x8000, true); break;
                    case 0x8001: base.Switch08KPRG(data, 0xA000, true); break;
                    case 0x8002: base.Switch02KCHR(data, 0x0000, chr_01K_rom_count > 0); break;
                    case 0x8003: base.Switch02KCHR(data, 0x0800, chr_01K_rom_count > 0); break;
                    case 0xA000: base.Switch01KCHR(data, 0x1000, chr_01K_rom_count > 0); break;
                    case 0xA001: base.Switch01KCHR(data, 0x1400, chr_01K_rom_count > 0); break;
                    case 0xA002: base.Switch01KCHR(data, 0x1800, chr_01K_rom_count > 0); break;
                    case 0xA003: base.Switch01KCHR(data, 0x1C00, chr_01K_rom_count > 0); break;
                    case 0xC000: irq_reload = (byte)(data ^ 0xFF); break;
                    case 0xC001: if (mmc3_alt_behavior) irq_clear = true; irq_counter = 0; break;
                    case 0xC002: irq_enabled = false; NesEmu.IRQFlags &= ~NesEmu.IRQ_BOARD; break;
                    case 0xC003: irq_enabled = true; break;
                    case 0xE000: SwitchNMT((data & 0x40) == 0x40 ? Mirroring.Horz : Mirroring.Vert); break;
                }
            }
            else
            {
                // Mapper 33 mode
                switch (address & 0xA003)
                {
                    case 0x8000:
                        {
                            base.SwitchNMT((data & 0x40) == 0x40 ? Mirroring.Horz : Mirroring.Vert);
                            base.Switch08KPRG((data & 0x3F), 0x8000, true);
                            break;
                        }
                    case 0x8001: base.Switch08KPRG((data & 0x3F), 0xA000, true); break;
                    case 0x8002: base.Switch02KCHR(data, 0x0000, chr_01K_rom_count > 0); break;
                    case 0x8003: base.Switch02KCHR(data, 0x0800, chr_01K_rom_count > 0); break;
                    case 0xA000: base.Switch01KCHR(data, 0x1000, chr_01K_rom_count > 0); break;
                    case 0xA001: base.Switch01KCHR(data, 0x1400, chr_01K_rom_count > 0); break;
                    case 0xA002: base.Switch01KCHR(data, 0x1800, chr_01K_rom_count > 0); break;
                    case 0xA003: base.Switch01KCHR(data, 0x1C00, chr_01K_rom_count > 0); break;
                }
            }
        }
        // The scanline timer, clocked on PPU A12 raising edge ...
        public override void OnPPUA12RaisingEdge()
        {
            if (MODE) return;
            old_irq_counter = irq_counter;

            if (irq_counter == 0 || irq_clear)
                irq_counter = irq_reload;
            else
                irq_counter = (byte)(irq_counter - 1);

            if ((!mmc3_alt_behavior || old_irq_counter != 0 || irq_clear) && irq_counter == 0 && irq_enabled)
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
