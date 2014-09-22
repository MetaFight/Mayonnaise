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

namespace MyNes
{
    [System.Serializable]
    public class IInputSettingsVSUnisystemDIP : IInputSettings
    {
        public string CreditServiceButton { get; set; }
        public string DIPSwitch1 { get; set; }
        public string DIPSwitch2 { get; set; }
        public string DIPSwitch3 { get; set; }
        public string DIPSwitch4 { get; set; }
        public string DIPSwitch5 { get; set; }
        public string DIPSwitch6 { get; set; }
        public string DIPSwitch7 { get; set; }
        public string DIPSwitch8 { get; set; }
        public string CreditLeftCoinSlot { get; set; }
        public string CreditRightCoinSlot { get; set; }
    }
}
