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
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNes
{
    public enum ImageViewMode : int
    {
        /// <summary>
        /// Normal, if image bounds are larger than the image viewer panel show the scroll bars
        /// </summary>
        Normal = 0,
        /// <summary>
        /// Stretch image only if the image is larger than than the viewer
        /// </summary>
        StretchIfLarger = 1,
        /// <summary>
        /// Always stretch the image
        /// </summary>
        StretchToFit = 2,
        /// <summary>
        /// Always stretch the image without aspect ratio. All other options uses aspect ratio.
        /// </summary>
        StretchNoAspectRatio = 3
    }
}
