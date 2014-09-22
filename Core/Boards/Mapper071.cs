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
    [BoardInfo("Camerica", 71)]
    class Mapper071 : Board
    {
        private bool fireHawk;
        public override void HardReset()
        {
            base.HardReset();
            Switch16KPRG(prg_16K_ram_mask, 0xC000, true);
            // This is not a hack !
            // This is a qoute from 071.txt of [iNES Mappers by Mapper Number v0.6.1 by Disch]:
            // "One in paticular that needs to be noted is the board
            // used by Fire Hawk -- which has mapper controlled 1-screen mirroring.  On other boards, mirroring is
            // hardwired!  This is yet another one of those terrific mapper number incompatibilities."

            fireHawk = (RomSHA1.ToUpper() == "334781C830F135CF30A33E392D8AAA4AFDC223F9");
        }
        public override void WritePRG(ref int address, ref byte data)
        {
            if (address < 0xA000)
            {
                if (fireHawk)
                    SwitchNMT((data & 0x10) == 0x10 ? Mirroring.OneScB : Mirroring.OneScA);
            }
            else if (address >= 0xC000)
                Switch16KPRG(data, 0x8000, true);
        }
    }
}
