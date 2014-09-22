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
using System;

namespace MyNes.Core
{
    struct BankInfo
    {
        public BankInfo(string ID, bool IsRAM, bool Writable, bool Enabled, bool IsBattery, byte[] DATA)
        {
            this.ID = ID;
            this.IsRAM = IsRAM;
            this.Writable = Writable;
            this.Enabled = Enabled;
            this.DATA = DATA;
            this.IsBattery = IsBattery;
        }

        public bool IsRAM;
        public bool Enabled;
        public bool Writable;
        public bool IsBattery;
        public string ID;
        public byte[] DATA;
    }
}

