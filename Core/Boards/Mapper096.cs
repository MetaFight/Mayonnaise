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
    [BoardInfo("74161/32", 96, 1, 32)]
    [NotImplementedWell("Mapper 96\nNot implemented well and needs special controller.")]
    class Mapper096 : Board
    {
        private int flag_c;
        public override void HardReset()
        {
            base.HardReset();
            flag_c = 0;
            Switch04KCHR(3, 0x1000, false);
        }
        public override void WritePRG(ref int address, ref byte data)
        {
            Switch32KPRG(data & 0x3, true);
            flag_c = (data & 0x4) == 0x4 ? 0x01 : 0;
            Switch04KCHR(3, 0x1000, false);
        }
        public override void OnPPUAddressUpdate(ref int address)
        {
            if ((address & 0x03FF) < 0x03C0 && (address & 0x1000) == 0x0000)
                Switch04KCHR(((address & 0x0300) >> 8) | flag_c, 0x0000, false);
        }
        public override void SaveState(System.IO.BinaryWriter stream)
        {
            base.SaveState(stream);
            stream.Write(flag_c);
        }
        public override void LoadState(System.IO.BinaryReader stream)
        {
            base.LoadState(stream);
            flag_c = stream.ReadInt32();
        }
    }
}
