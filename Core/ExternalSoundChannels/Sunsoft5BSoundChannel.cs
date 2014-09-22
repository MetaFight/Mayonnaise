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
    class Sunsoft5BSoundChannel
    {
        public bool Enabled;
        public byte Volume;
        private byte output = 0;
        private int dutyStep = 0;
        private int freqTimer;
        private int frequency;
        private int cycles;

        public void HardReset() { }
        public void SoftReset() { }
        public void Write0(ref byte data)
        {
            frequency = (frequency & 0x0F00) | data;
            freqTimer = (frequency + 1) * 2;
        }
        public void Write1(ref byte data)
        {
            frequency = (frequency & 0x00FF) | ((data & 0xF) << 8);
            freqTimer = (frequency + 1) * 2;
        }
        public byte GetSample()
        {
            if (Enabled)
                return output;
            return 0;
        }
        public void ClockSingle()
        {
            if (--cycles <= 0)
            {
                cycles = freqTimer;
                dutyStep = (dutyStep + 1) & 0x1F;

                if (dutyStep <= 15)
                {
                    output = Volume;
                }
                else
                    output = 0;
            }
        }
        /// <summary>
        /// Save state
        /// </summary>
        /// <param name="stream">The stream that should be used to write data</param>
        public virtual void SaveState(System.IO.BinaryWriter stream)
        {
            stream.Write(Enabled);
            stream.Write(Volume);
            stream.Write(output);
            stream.Write(dutyStep);
            stream.Write(freqTimer);
            stream.Write(frequency);
            stream.Write(cycles);
        }
        /// <summary>
        /// Load state
        /// </summary>
        /// <param name="stream">The stream that should be used to read data</param>
        public virtual void LoadState(System.IO.BinaryReader stream)
        {
            Enabled = stream.ReadBoolean();
            Volume = stream.ReadByte();
            output = stream.ReadByte();
            dutyStep = stream.ReadInt32();
            freqTimer = stream.ReadInt32();
            frequency = stream.ReadInt32();
            cycles = stream.ReadInt32();
        }
    }
}
