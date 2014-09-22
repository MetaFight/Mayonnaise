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
using System.Text;
using System.IO;
namespace MyNes
{
    /// <summary>
    /// The sound recorder to wav format. Can be used in any platform.
    /// </summary>
    public class WaveRecorder
    {
        private Stream STR;
        private bool _IsRecording = false;
        private int SIZE = 0;
        private int NoOfSamples = 0;
        private int _Time = 0;
        private int TimeSamples = 0;

        private short channels;
        private short bitsPerSample;
        private int Frequency;
        /// <summary>
        /// Start record operation
        /// </summary>
        /// <param name="FilePath">The wav file path</param>
        /// <param name="channels">Channels number, 1 for mono or 2 for Stereo</param>
        /// <param name="bitsPerSample">Bits per sample, 8, 16 or 32</param>
        /// <param name="Frequency">Frequency, 22100 - 96000</param>
        public void Record(string FilePath, short channels, short bitsPerSample, int Frequency)
        {
            this.channels = channels;
            this.bitsPerSample = bitsPerSample;
            this.Frequency = Frequency;
            _Time = 0;
            //Create the stream first
            STR = new FileStream(FilePath, FileMode.Create);
            ASCIIEncoding ASCII = new ASCIIEncoding();
            //1 Write the header "RIFF"
            STR.Write(ASCII.GetBytes("RIFF"), 0, 4);
            //2 Write Chunck Size (0 for now)
            STR.WriteByte(0x00);
            STR.WriteByte(0x00);
            STR.WriteByte(0x00);
            STR.WriteByte(0x00);
            //3 Write WAVE
            STR.Write(ASCII.GetBytes("WAVE"), 0, 4);
            //4 Write "fmt "
            STR.Write(ASCII.GetBytes("fmt "), 0, 4);
            //5 Write Chunck Size (16 for PCM)
            STR.WriteByte(0x10);
            STR.WriteByte(0x00);
            STR.WriteByte(0x00);
            STR.WriteByte(0x00);
            //6 Write audio format (1 = PCM)
            STR.WriteByte(0x01);
            STR.WriteByte(0x00);
            //7 Number of channels
            STR.WriteByte((byte)((channels & 0x00FF) >> 00));
            STR.WriteByte((byte)((channels & 0xFF00) >> 08));

            //8 Sample Rate (frequncy)
            STR.WriteByte((byte)((Frequency & 0x000000FF) >> 00));
            STR.WriteByte((byte)((Frequency & 0x0000FF00) >> 08));
            STR.WriteByte((byte)((Frequency & 0x00FF0000) >> 16));
            STR.WriteByte((byte)((Frequency & 0xFF000000) >> 24));
            //9 Byte Rate 
            int BRate = Frequency * channels * (bitsPerSample / 8);//[= Frequency * Channels * (bps / 8)] while bps is "Bits Per Sample"
            STR.WriteByte((byte)((BRate & 0x000000FF) >> 00));
            STR.WriteByte((byte)((BRate & 0x0000FF00) >> 08));
            STR.WriteByte((byte)((BRate & 0x00FF0000) >> 16));
            STR.WriteByte((byte)((BRate & 0xFF000000) >> 24));

            //10 Block Align (=Channels * bps / 8)
            short block = (short)(channels * (bitsPerSample / 8));
            STR.WriteByte((byte)((block & 0x00FF) >> 00));
            STR.WriteByte((byte)((block & 0xFF00) >> 08));

            //11 Bits Per Sample
            STR.WriteByte((byte)((bitsPerSample & 0x00FF) >> 00));
            STR.WriteByte((byte)((bitsPerSample & 0xFF00) >> 08));
            //12 Write "data"
            STR.Write(ASCII.GetBytes("data"), 0, 4);
            //13 Write Chunck Size (0 for now)
            STR.WriteByte(0x00);
            STR.WriteByte(0x00);
            STR.WriteByte(0x00);
            STR.WriteByte(0x00);
            //Confirm
            _IsRecording = true;
        }
        public void AddBuffer(ref byte[] buffer)
        {
            for (int i = 0; i < buffer.Length; i++)
            {
                switch (channels)
                {
                    case 1:
                        {
                            switch (bitsPerSample)
                            {
                                case 08:
                                    {
                                        STR.WriteByte(buffer[i]);

                                        NoOfSamples++;
                                        TimeSamples++;
                                        if (TimeSamples >= Frequency)
                                        {
                                            _Time++;
                                            TimeSamples = 0;
                                        }
                                        break;
                                    }
                                case 16:
                                    {
                                        STR.WriteByte(buffer[i]); i++;
                                        STR.WriteByte(buffer[i]);

                                        NoOfSamples++;
                                        TimeSamples++;
                                        if (TimeSamples >= Frequency)
                                        {
                                            _Time++;
                                            TimeSamples = 0;
                                        }
                                        break;
                                    }
                                case 32:
                                    {
                                        STR.WriteByte(buffer[i]); i++;
                                        STR.WriteByte(buffer[i]); i++;
                                        STR.WriteByte(buffer[i]); i++;
                                        STR.WriteByte(buffer[i]);

                                        NoOfSamples++;
                                        TimeSamples++;
                                        if (TimeSamples >= Frequency)
                                        {
                                            _Time++;
                                            TimeSamples = 0;
                                        }
                                        break;
                                    }
                            }
                            break;
                        }
                    case 2:
                        {
                            switch (bitsPerSample)
                            {
                                case 08:
                                    {
                                        STR.WriteByte(buffer[i]);
                                        // Same sample twice
                                        STR.WriteByte(buffer[i]);

                                        NoOfSamples++;
                                        TimeSamples++;
                                        if (TimeSamples >= Frequency)
                                        {
                                            _Time++;
                                            TimeSamples = 0;
                                        }
                                        break;
                                    }
                                case 16:
                                    {
                                        STR.WriteByte(buffer[i]);
                                        STR.WriteByte(buffer[i]); i++;
                                        // Same sample twice
                                        STR.WriteByte(buffer[i]); 
                                        STR.WriteByte(buffer[i]);

                                        NoOfSamples++;
                                        TimeSamples++;
                                        if (TimeSamples >= Frequency)
                                        {
                                            _Time++;
                                            TimeSamples = 0;
                                        }
                                        break;
                                    }
                                case 32:
                                    {
                                        STR.WriteByte(buffer[i]); i++;
                                        STR.WriteByte(buffer[i]); 
                                        STR.WriteByte(buffer[i]); i++;
                                        STR.WriteByte(buffer[i]);
                                        // Same sample twice
                                        STR.WriteByte(buffer[i]); i++;
                                        STR.WriteByte(buffer[i]); 
                                        STR.WriteByte(buffer[i]); i++;
                                        STR.WriteByte(buffer[i]);

                                        NoOfSamples++;
                                        TimeSamples++;
                                        if (TimeSamples >= Frequency)
                                        {
                                            _Time++;
                                            TimeSamples = 0;
                                        }
                                        break;
                                    }
                            }
                            break;
                        }
                }
            }
        }
        public void AddSample(ref int Sample)
        {
            if (!_IsRecording)
                return;
            switch (channels)
            {
                case 1:
                    {
                        switch (bitsPerSample)
                        {
                            case 08: AddSample_mono_08(Sample); break;
                            case 16: AddSample_mono_16(Sample); break;
                            case 32: AddSample_mono_32(Sample); break;
                        }
                        break;
                    }
                case 2:
                    {
                        switch (bitsPerSample)
                        {
                            case 08: AddSample_stereo_08(Sample); break;
                            case 16: AddSample_stereo_16(Sample); break;
                            case 32: AddSample_stereo_32(Sample); break;
                        }
                        break;
                    }
            }
            NoOfSamples++;
            TimeSamples++;
            if (TimeSamples >= Frequency)
            {
                _Time++;
                TimeSamples = 0;
            }
        }
        private void AddSample_mono_08(int Sample)
        {
            STR.WriteByte((byte)(Sample & 0xFF));
        }
        private void AddSample_mono_16(int Sample)
        {
            STR.WriteByte((byte)((Sample & 0xFF00) >> 8));
            STR.WriteByte((byte)(Sample & 0xFF));
        }
        private void AddSample_mono_32(int Sample)
        {
            STR.WriteByte((byte)((Sample & 0xFF000000) >> 24));
            STR.WriteByte((byte)((Sample & 0x00FF0000) >> 16));
            STR.WriteByte((byte)((Sample & 0x0000FF00) >> 08));
            STR.WriteByte((byte)((Sample & 0x000000FF) >> 00));
        }
        private void AddSample_stereo_08(int Sample)
        {
            STR.WriteByte((byte)(Sample & 0xFF));
            // Same sample twice
            STR.WriteByte((byte)(Sample & 0xFF));
        }
        private void AddSample_stereo_16(int Sample)
        {
            STR.WriteByte((byte)((Sample & 0xFF00) >> 8));
            STR.WriteByte((byte)(Sample & 0xFF));
            // Same sample twice
            STR.WriteByte((byte)((Sample & 0xFF00) >> 8));
            STR.WriteByte((byte)(Sample & 0xFF));
        }
        private void AddSample_stereo_32(int Sample)
        {
            STR.WriteByte((byte)((Sample & 0xFF000000) >> 24));
            STR.WriteByte((byte)((Sample & 0x00FF0000) >> 16));
            STR.WriteByte((byte)((Sample & 0x0000FF00) >> 08));
            STR.WriteByte((byte)((Sample & 0x000000FF) >> 00));
            // Same sample twice
            STR.WriteByte((byte)((Sample & 0xFF000000) >> 24));
            STR.WriteByte((byte)((Sample & 0x00FF0000) >> 16));
            STR.WriteByte((byte)((Sample & 0x0000FF00) >> 08));
            STR.WriteByte((byte)((Sample & 0x000000FF) >> 00));
        }
        public void Stop()
        {
            if (_IsRecording & STR != null)
            {
                NoOfSamples *= channels * (bitsPerSample / 8);

                SIZE = NoOfSamples + 36;
                byte[] buff_size = new byte[4];
                byte[] buff_NoOfSammples = new byte[4];
                buff_size = BitConverter.GetBytes(SIZE);
                if (!BitConverter.IsLittleEndian)
                    Array.Reverse(buff_size);
                buff_NoOfSammples = BitConverter.GetBytes(NoOfSamples);
                if (!BitConverter.IsLittleEndian)
                    Array.Reverse(buff_NoOfSammples);
                _IsRecording = false;
                STR.Position = 4;
                STR.Write(buff_size, 0, 4);
                STR.Position = 40;
                STR.Write(buff_NoOfSammples, 0, 4);
                STR.Close();
            }
        }
        public int Time
        {
            get { return _Time; }
        }
        public bool IsRecording
        {
            get { return _IsRecording; }
        }
    }
}
