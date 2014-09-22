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
    /// <summary>
    /// Presentation of this attribute tells that this element is not implemented well and has known issues.
    /// </summary>
    public class NotImplementedWell : Attribute
    {
        /// <summary>
        /// Presentation of this attribute tells that this element is not implemented well and has known issues.
        /// </summary>
        public NotImplementedWell() { }
        /// <summary>
        /// Presentation of this attribute tells that this element is not implemented well and has known issues.
        /// </summary>
        /// <param name="issues">The issues with this element.</param>
        public NotImplementedWell(string issues)
        { this.Issues = issues; }
        public string Issues { get; private set; }
    }
}
