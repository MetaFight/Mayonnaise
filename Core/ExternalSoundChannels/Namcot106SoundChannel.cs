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
    class Namcot106SoundChannel
    {
        // TODO: this channel is working, the problem is with the output mixer.
        //       don't know how to mix these channels (up to 8) specially when 
        //       game run more than one channel at time.
        public Namcot106SoundChannel(Namcot106 namcot)
        {
            this.namcot = namcot;
        }
        private Namcot106 namcot;
        private int freqTimer;
        private int frequency;
        private byte output;
        private int cycles;
        private int InstrumentLength;
        private byte InstrumentAddress;
        private int startPoint;// the in point in bytes
        private int endPoint;// The end point in bytes (not 4-bit length)
        private int readPoint;
        public int volume;
        private bool freez;

        public void HardReset()
        {

        }
        private void UpdateFrequency()
        {
            if (frequency > 0)
            {
                freqTimer = (0xF0000 * (namcot.enabledChannels + 1)) / frequency;
                freez = false;
            }
            else
            {
                freez = true;
                output = 0;
            }
        }
        private void UpdatePlaybackParameters()
        {
            // Determine in and out points
            startPoint = InstrumentAddress;
            endPoint = InstrumentAddress + (4 * (8 - InstrumentLength));
            // Reset reader
            readPoint = InstrumentAddress;
        }
        public void WriteA(ref byte data)
        {
            frequency = (frequency & 0xFFFF00) | data;
            UpdateFrequency();
        }
        public void WriteB(ref byte data)
        {
            frequency = (frequency & 0xFF00FF) | (data << 8);
            UpdateFrequency();
        }
        public void WriteC(ref byte data)
        {
            frequency = (frequency & 0x00FFFF) | ((data & 0x3) << 12);
            InstrumentLength = (data >> 2) & 0x7;
            UpdateFrequency();
            UpdatePlaybackParameters();
        }
        public void WriteD(ref byte data)
        {
            InstrumentAddress = data;
            UpdatePlaybackParameters();
        }
        public void WriteE(ref byte data)
        {
            volume = data & 0xF;
        }
        public void ClockSingle()
        {
            if (freez) return;
            if (--cycles <= 0)
            {
                cycles = freqTimer;
                if (readPoint >= startPoint && readPoint <= endPoint)
                {
                    // Get the current value
                    if ((readPoint & 1) == 0)
                        output = (byte)(namcot.EXRAM[readPoint] & 0xF);// Low bits
                    else
                    {
                        output = (byte)((namcot.EXRAM[readPoint] >> 4) & 0xF);// high bits
                    }
                    // Increament reader
                    readPoint++;
                }
                else
                {
                    output = 0;
                    // Reset reader
                    readPoint = startPoint;
                }
            }
        }
        public byte GetSample()
        {
            if (Enabled && !freez)
                return (byte)(output * volume);
            return 0;
        }

        public bool Enabled { get; set; }
        public void SaveState(System.IO.BinaryWriter stream)
        {
            stream.Write(freqTimer);
            stream.Write(frequency);
            stream.Write(output);
            stream.Write(cycles);
            stream.Write(InstrumentLength);
            stream.Write(InstrumentAddress);
            stream.Write(startPoint);
            stream.Write(endPoint);
            stream.Write(readPoint);
            stream.Write(volume);
            stream.Write(freez);
        }
        public void LoadState(System.IO.BinaryReader stream)
        {
            freqTimer = stream.ReadInt32();
            frequency = stream.ReadInt32();
            output = stream.ReadByte();
            cycles = stream.ReadInt32();
            InstrumentLength = stream.ReadInt32();
            InstrumentAddress = stream.ReadByte();
            startPoint = stream.ReadInt32();
            endPoint = stream.ReadInt32();
            readPoint = stream.ReadInt32();
            volume = stream.ReadInt32();
            freez = stream.ReadBoolean();
        }
    }
}
