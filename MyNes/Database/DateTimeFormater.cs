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
namespace MyNes
{
    class DateTimeFormater
    {
        /// <summary>
        /// To YYYY-MM-DDTHH:MM:SS 
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string ToFull(DateTime time)
        {
            string val = "";
            // YYYY-MM-DDTHH:MM:SS 
            val += time.Year.ToString("D4") + "-";
            val += time.Month.ToString("D2") + "-";
            val += time.Day.ToString("D2") + " ";
            val += time.Hour.ToString("D2") + ":";
            val += time.Minute.ToString("D2") + ":";
            val += time.Second.ToString("D2");
            return val;
        }
        /// <summary>
        /// To YYYY-MM-DD
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string ToDateOnly(DateTime time)
        {
            string val = "";
            // YYYY-MM-DD
            val += time.Year.ToString("D4") + "-";
            val += time.Month.ToString("D2") + "-";
            val += time.Day.ToString("D2");
            return val;
        }
        /// <summary>
        /// From YYYY-MM-DD
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static DateTime FromDateOnly(string time)
        {
            string[] vals = time.Split(new char[] { '-' });
            return new DateTime(int.Parse(vals[0]), int.Parse(vals[1]), int.Parse(vals[2]));
        }
        /// <summary>
        /// From YYYY-MM-DDTHH:MM:SS 
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static DateTime FromFull(string time)
        {
            // "1/1/0001 12:00:00 AM"
            string[] vals = time.Split(new char[] { '/', '-', 'T', ':', ' ' });
            return new DateTime(int.Parse(vals[2]), int.Parse(vals[1]), int.Parse(vals[0]),
                int.Parse(vals[3]), int.Parse(vals[4]), int.Parse(vals[5]));
        }
    }
}
