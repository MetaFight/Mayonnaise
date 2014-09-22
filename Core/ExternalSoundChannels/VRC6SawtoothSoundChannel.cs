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
    class VRC6SawtoothSoundChannel
    {
        private byte AccumRate;
        private int accumClock;
        private byte accumulationRegister;
        private int frequency;
        private int freqTimer;
        private int cycles;
        private bool enabled;
        public byte output;

        private void UpdateFrequency()
        {
            freqTimer = (frequency + 1) * 2;
        }
        public void Write0(ref byte data)
        {
            AccumRate = (byte)(data & 0x3F);
        }
        public void Write1(ref byte data)
        {
            frequency = (frequency & 0x0F00) | data;
            UpdateFrequency();
        }
        public void Write2(ref byte data)
        {
            frequency = (frequency & 0x00FF) | ((data & 0xF) << 8);
            enabled = (data & 0x80) == 0x80;
            UpdateFrequency();
        }
        public void ClockSingle()
        {
            if (--cycles <= 0)
            {
                cycles = freqTimer;
                if (enabled)
                {
                    accumClock++;
                    switch (++accumClock)
                    {
                        case 2:
                        case 4:
                        case 6:
                        case 8:
                        case 10:
                        case 12:
                            accumulationRegister += AccumRate;
                            break;

                        case 14:
                            accumulationRegister = 0;
                            accumClock = 0;
                            break;
                    }

                    output = (byte)((accumulationRegister >> 3) & 0x1F);
                }
                else
                    output = 0;
            }
        }
        /// <summary>
        /// Save state
        /// </summary>
        /// <param name="stream">The stream that should be used to write data</param>
        public void SaveState(System.IO.BinaryWriter stream)
        {
            stream.Write(AccumRate);
            stream.Write(accumClock);
            stream.Write(accumulationRegister);
            stream.Write(frequency);
            stream.Write(freqTimer);
            stream.Write(cycles);
            stream.Write(enabled);
            stream.Write(output);
        }
        /// <summary>
        /// Load state
        /// </summary>
        /// <param name="stream">The stream that should be used to read data</param>
        public void LoadState(System.IO.BinaryReader stream)
        {
            AccumRate = stream.ReadByte();
            accumClock = stream.ReadInt32();
            accumulationRegister = stream.ReadByte();
            frequency = stream.ReadInt32();
            freqTimer = stream.ReadInt32();
            cycles = stream.ReadInt32();
            enabled = stream.ReadBoolean();
            output = stream.ReadByte();
        }
    }
}
