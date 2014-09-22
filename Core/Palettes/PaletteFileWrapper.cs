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
using System.IO;
namespace MyNes.Core
{
    public class PaletteFileWrapper
    {
        /// <summary>
        /// Load a palette file (512 indexed palette).
        /// </summary>
        /// <param name="file">The full file path.</param>
        /// <param name="palette">The palette the loaded from file otherwise null if not loaded successful.</param>
        /// <returns>True if palette file loaded successfully otherwise false.</returns>
        public static bool LoadFile(string file, out int[] palette)
        {
            Stream str = new FileStream(file, FileMode.Open, FileAccess.Read);
            if (str.Length == 192 || str.Length == 512 * 3)
            {
                int[] Nes_Palette = new int[512];
                byte[] buffer = new byte[str.Length];
                str.Read(buffer, 0, buffer.Length);

                int j = 0;
                for (int i = 0; i < 512; i++)
                {
                    byte RedValue = buffer[j];
                    j++;
                    if (j == buffer.Length)
                        j = 0;
                    byte GreenValue = buffer[j];
                    j++;
                    if (j == buffer.Length)
                        j = 0;
                    byte BlueValue = buffer[j];
                    j++;
                    if (j == buffer.Length)
                        j = 0;
                    Nes_Palette[i] = (0xFF << 24) | (RedValue << 16) |
                                     (GreenValue << 8) | BlueValue;
                }
                str.Close();
                palette = Nes_Palette;
                return true;
            }
            palette = null;
            return false;
        }
        /// <summary>
        /// Save palette file
        /// </summary>
        /// <param name="file">The complete path where to save the palette</param>
        /// <param name="palette">The palette to save, it prefered to be 512 indexed palette.</param>
        public static void SaveFile(string file, int[] palette)
        {
            Stream str = new FileStream(file, FileMode.Create, FileAccess.Write);
            List<byte> buffer = new List<byte>();

            for (int i = 0; i < palette.Length; i++)
            {
                int color = palette[i];
                buffer.Add((byte)((color >> 16) & 0xFF));//Red
                buffer.Add((byte)((color >> 08) & 0xFF));//Green
                buffer.Add((byte)((color >> 00) & 0xFF));//Blue
            }

            str.Write(buffer.ToArray(), 0, buffer.Count);
            str.Close();
        }
    }
}
