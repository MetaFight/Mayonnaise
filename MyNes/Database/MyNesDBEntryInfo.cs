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
    struct MyNesDBEntryInfo
    {
        public bool IsDB { get; set; }
        public string Name { get; set; }
        public string AlternativeName { get; set; }
        public int Size { get; set; }
        public string Path { get; set; }
        public int Rating { get; set; }
        public int Played { get; set; }
        public int PlayTime { get; set; }
        public string LastPlayed { get; set; }
        public string Class { get; set; }
        public string Catalog { get; set; }
        public string Publisher { get; set; }
        public string Developer { get; set; }
        public string Region { get; set; }
        public string Players { get; set; }
        public string ReleaseDate { get; set; }

        public string System { get; set; }
        public string CRC { get; set; }
        public string SHA1 { get; set; }
        public string Dump { get; set; }
        public string Dumper { get; set; }
        public string DateDumped { get; set; }
        public string BoardType { get; set; }
        public string BoardPcb { get; set; }
        public int BoardMapper { get; set; }
    }
}
