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
using System.Runtime.InteropServices;

namespace MyNes.Core
{
    [StructLayout(LayoutKind.Explicit)]
    struct CPURegisters
    {
        [FieldOffset(0)]
        public byte a;
        [FieldOffset(1)]
        public byte x;
        [FieldOffset(2)]
        public byte y;
        // PC
        [FieldOffset(3)]
        public byte pcl;
        [FieldOffset(4)]
        public byte pch;
        [FieldOffset(3)]
        public ushort pc;
        // EA
        [FieldOffset(5)]
        public byte eal;
        [FieldOffset(6)]
        public byte eah;
        [FieldOffset(5)]
        public ushort ea;
        // SP
        [FieldOffset(7)]
        public byte spl;
        [FieldOffset(8)]
        public byte sph;
        [FieldOffset(7)]
        public ushort sp;
        // flags
        [MarshalAs(UnmanagedType.I1), FieldOffset(9)]
        public bool n;
        [MarshalAs(UnmanagedType.I1), FieldOffset(10)]
        public bool v;
        [MarshalAs(UnmanagedType.I1), FieldOffset(11)]
        public bool d;
        [MarshalAs(UnmanagedType.I1), FieldOffset(12)]
        public bool i;
        [MarshalAs(UnmanagedType.I1), FieldOffset(13)]
        public bool z;
        [MarshalAs(UnmanagedType.I1), FieldOffset(14)]
        public bool c;

        public byte p
        {
            get
            {
                return (byte)(
                    (n ? 0x80 : 0) |
                    (v ? 0x40 : 0) |
                    (d ? 0x08 : 0) |
                    (i ? 0x04 : 0) |
                    (z ? 0x02 : 0) |
                    (c ? 0x01 : 0) | 0x20);
            }
            set
            {
                n = (value & 0x80) != 0;
                v = (value & 0x40) != 0;
                d = (value & 0x08) != 0;
                i = (value & 0x04) != 0;
                z = (value & 0x02) != 0;
                c = (value & 0x01) != 0;
            }
        }

        public byte pb()
        {
            return (byte)(
                (n ? 0x80 : 0) |
                (v ? 0x40 : 0) |
                (d ? 0x08 : 0) |
                (i ? 0x04 : 0) |
                (z ? 0x02 : 0) |
                (c ? 0x01 : 0) | 0x30);
        }
    }
}

