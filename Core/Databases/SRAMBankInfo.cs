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
    public struct SRAMBankInfo
    {
        public SRAMBankInfo(int id, string SIZE, bool BATTERY)
        {
            this.SIZE = SIZE;
            this.BATTERY = BATTERY;
            this.id = id;
        }

        public int id;
        public string SIZE;
        public bool BATTERY;
    }

    class BankInfoSorter : System.Collections.Generic.IComparer<BankInfo>
    {
        public BankInfoSorter()
        {
        }

        public int Compare(BankInfo x, BankInfo y)
        {
            int x_val = 0;
            int y_val = 0;
            int.TryParse(x.ID, out x_val);
            int.TryParse(y.ID, out y_val);
            return y_val - x_val;
        }
    }
}
