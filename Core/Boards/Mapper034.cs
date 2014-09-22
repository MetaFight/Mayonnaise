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
    [BoardInfo("BxROM/NINA-001", 34)]
    class Mapper034 : Board
    {
        private bool BxROM;
        private byte writeData;
        public override void HardReset()
        {
            base.HardReset();
            //Board Type: IREM-BNROM
            BxROM = true;// Assume BxROM
            if (BoardType.Contains("NINA"))
                BxROM = false;
        }
        public override void WriteSRM(ref int address, ref byte data)
        {
            base.WriteSRM(ref address, ref data);
            if (!BxROM)// NINA-001
            {
                switch (address)
                {
                    case 0x7FFD: base.Switch32KPRG(data, true); break;
                    case 0x7FFE: base.Switch04KCHR(data, 0x0000, chr_01K_rom_count > 0); break;
                    case 0x7FFF: base.Switch04KCHR(data, 0x1000, chr_01K_rom_count > 0); break;
                }
            }
        }
        public override void WritePRG(ref int address, ref byte data)
        {
            //if (BxROM)
            {
                writeData = (byte)(data & ReadPRG(ref address));// Bus Conflicts
                base.Switch32KPRG(writeData, true);
            }
        }
    }
}
