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
    [BoardInfo("Unknown", 230)]
    [NotImplementedWell("Mapper 230\nOnly Contra work.")]
    class Mapper230 : Board
    {
        private bool contraMode = false;
        public override void HardReset()
        {
            base.HardReset();

            //set contra mode
            contraMode = true;
            Switch16KPRG(0, 0x8000, true);
            Switch16KPRG(7, 0xC000, true);
        }
        public override void SoftReset()
        {
            base.SoftReset();
            contraMode = !contraMode;
            if (contraMode)
            {
                Switch16KPRG(0, 0x8000, true);
                Switch16KPRG(7, 0xC000, true);
                SwitchNMT(Mirroring.Vert);
            }
            else
            {
                Switch08KCHR(0, chr_01K_rom_count > 0);
                Switch16KPRG(8, 0x8000, true);
                Switch16KPRG(39, 0xC000, true);
            }
        }
        public override void WritePRG(ref int address, ref byte data)
        {
            if (contraMode)
            {
                Switch16KPRG(data & 0x7, 0x8000, true);
                Switch16KPRG(7, 0xC000, true);
                SwitchNMT(Mirroring.Vert);
            }
            else
            {
                SwitchNMT((data & 0x40) == 0x40 ? Mirroring.Vert : Mirroring.Horz);

                if ((data & 0x20) == 0x20)
                {
                    Switch16KPRG((data & 0x1F) + 8, 0x8000, true);
                    Switch16KPRG((data & 0x1F) + 8, 0xC000, true);
                }
                else
                    Switch32KPRG(((data & 0x1F) >> 1) + 4, true);
            }
        }
    }
}
