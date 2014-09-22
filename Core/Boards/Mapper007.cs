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
    [BoardInfo("AxROM", 7)]
    class Mapper007 : Board
    {
        private bool enableBusConflicts;
        private byte writeData;
        public override void Initialize(string sha1, byte[] prg_dump, byte[] chr_dump, byte[] trainer_dump, MyNes.Core.Mirroring defaultMirroring)
        {
            base.Initialize(sha1, prg_dump, chr_dump, trainer_dump, defaultMirroring);
            enableBusConflicts = false;
            // This is not a hack. We need to see if this board type uses bus conflicts.
            if (BoardPCB.Contains("AMROM") || BoardPCB.Contains("AOROM"))
            {
                // TODO: bus conflicts in mapper 7
                enableBusConflicts = true;
                System.Console.WriteLine("AxROM: Bus Conflicts enabled [Board type = " + BoardPCB + "]");
            }
        }
        public override void WritePRG(ref int address, ref byte data)
        {
            if (enableBusConflicts)
                writeData = (byte)(data | ReadPRG(ref address));
            else
                writeData = data;
            base.SwitchNMT(((writeData & 0x10) == 0x10) ? Mirroring.OneScB : Mirroring.OneScA);
            base.Switch32KPRG(writeData & 0x7, true);
        }
    }
}
