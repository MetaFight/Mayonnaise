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
using System.Windows.Forms;
using MyNes.Core;
using SlimDX.DirectSound;
using SlimDX.Multimedia;

namespace MyNes
{
    public class DirectSoundRenderer : IAudioProvider
    {
        public DirectSoundRenderer(IntPtr handle)
        {
            LoadSettings();
            Initialize(handle);
        }
        // slimdx
        public DirectSound _SoundDevice;
        public SecondarySoundBuffer buffer;
        public WaveRecorder Recorder = new WaveRecorder();

        private int volume = 0;
        private bool IsPaused;
        private bool canRender;
        public bool IsRendering = false;
        public int BufferSize = 44100;
        public int latency_in_bytes = 8820;

        public void Initialize(IntPtr handle)
        {
            LoadSettings();
            //Create the device
            Console.WriteLine("DirectSound: Initializing directSound ...");
            _SoundDevice = new DirectSound();
            _SoundDevice.SetCooperativeLevel(handle, CooperativeLevel.Normal);

            //Create the wav format
            WaveFormat wav = new WaveFormat();
            wav.FormatTag = WaveFormatTag.Pcm;
            wav.SamplesPerSecond = Program.Settings.Audio_Frequency;
            wav.Channels = 1;
            wav.BitsPerSample = Program.Settings.Audio_BitsPerSample;
            wav.AverageBytesPerSecond = wav.SamplesPerSecond * wav.Channels * (wav.BitsPerSample / 8);
            wav.BlockAlignment = (short)(wav.Channels * wav.BitsPerSample / 8);

            BufferSize = (int)(wav.AverageBytesPerSecond * ((double)Program.Settings.Audio_BufferSizeInMilliseconds) / (double)1000);
            latency_in_bytes = (int)((double)wav.AverageBytesPerSecond * (double)(Program.Settings.Audio_LatencyInMilliseconds / (double)1000));
            Console.WriteLine("DirectSound: BufferSize = " + BufferSize + " Byte");
            Console.WriteLine("DirectSound: Latency in bytes = " + latency_in_bytes + " Byte");
            //Description
            SoundBufferDescription des = new SoundBufferDescription();
            des.Format = wav;
            des.SizeInBytes = BufferSize;
            des.Flags = BufferFlags.ControlVolume | BufferFlags.ControlFrequency | BufferFlags.ControlPan | BufferFlags.Software;

            buffer = new SecondarySoundBuffer(_SoundDevice, des);

            buffer.Play(0, PlayFlags.Looping);
            // Set volume
            SetVolume(volume);
            Console.WriteLine("DirectSound: DirectSound initialized OK.");

            canRender = true;
        }
        public void LoadSettings()
        {
            volume = Program.Settings.Audio_Volume;
        }
        public void SubmitBuffer(ref byte[] samplesBuffer)
        {
            if (!canRender)
                return;
            if (IsRendering)
                return;
            if (buffer == null)
                return;
            IsRendering = true;

            buffer.Write(samplesBuffer, 0, LockFlags.None);
            IsRendering = false;
        }
        public void Dispose()
        {
            Stop();
            try
            {
                if (_SoundDevice != null)
                    _SoundDevice.Dispose();
                _SoundDevice = null;
                // if (buffer != null)
                //     buffer.Dispose();
                buffer = null;
            }
            catch { }
            GC.Collect();
        }
        public void Play()
        {
            if (!IsPaused)
            { return; }
            if (buffer != null && !buffer.Disposed & !IsRendering)
            {
                IsPaused = false;
                // ResetBuffer();
                try//Sometimes this line throw an exception for unknown reason !!
                {
                    buffer.Play(0, PlayFlags.Looping);
                }
                catch { }
            }
        }
        public void Pause()
        {
            if (IsPaused)
            { return; }
            if (buffer == null)
                return;
            if (!buffer.Disposed & !IsRendering)
            {
                buffer.Stop();
                IsPaused = true;
            }
        }
        public void Stop()
        {
            if (IsPaused)
            { return; }
            if (buffer == null)
                return;
            if (!buffer.Disposed & !IsRendering)
            {
                buffer.Stop();
                IsPaused = true;
            }
        }
        public void Shutdown()
        {
            Stop();
            if (Recorder.IsRecording)
                Recorder.Stop();
        }
        public void SetVolume(int Vol)
        {
            if (buffer != null && !buffer.Disposed & !IsRendering)
            {
                int volLev = (Vol * 3000) / 100;
                buffer.Volume = -3000 + volLev;
            }
        }
        public void SetPan(int Pan)
        {
            if (buffer != null && !buffer.Disposed & !IsRendering)
            {
                buffer.Pan = Pan;
            }
        }
        public bool IsRecording
        {
            get { return Recorder.IsRecording; }
        }
        public void Record(string filePath)
        {
            Recorder.Record(filePath, 1, Program.Settings.Audio_BitsPerSample, Program.Settings.Audio_Frequency);
        }
        public void RecordStop()
        {
            Recorder.Stop();
        }
        public int RecordTime
        {
            get { return Recorder.Time; }
        }
        public bool IsPlaying
        {
            get { return !IsPaused; }
        }
        public int CurrentWritePosition
        {
            get
            {
                if (buffer != null)
                    return buffer.CurrentWritePosition;
                return 0;
            }
        }
        public void RecorderAddSample(ref int sample)
        {
            Recorder.AddSample(ref sample);
        }
    }
}
