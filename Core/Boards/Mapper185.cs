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
    [BoardInfo("Unknown", 185)]
    class Mapper185 : Board
    {
        private bool lockchr;
        public override void HardReset()
        {
            base.HardReset();
            lockchr = false;
        }
        public override byte ReadCHR(ref int address, bool spriteFetch)
        {
            if (!lockchr)
                return base.ReadCHR(ref address, spriteFetch);
            else
                return 0xFF;
        }
        public override void WritePRG(ref int address, ref byte data)
        {
            lockchr = ((data & 0xF) == 0) || (data == 0x13);
        }
        public override void SaveState(System.IO.BinaryWriter stream)
        {
            base.SaveState(stream);
            stream.Write(lockchr);
        }
        public override void LoadState(System.IO.BinaryReader stream)
        {
            base.LoadState(stream);
            lockchr = stream.ReadBoolean();
        }
    }
}
