using MyNes.Core.SoundChannels;
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
using System.IO;

/*APU section*/
namespace MyNes.Core
{
    public class Apu
    {
		[Obsolete("Mega-hack until I can figure out how Memory and sound channel classes should interact.")]
		public readonly NoiseSoundChannel noiseChannel;
		[Obsolete("Mega-hack until I can figure out how Memory and sound channel classes should interact.")]
		public readonly PulseSoundChannel pulse1Channel;
		[Obsolete("Mega-hack until I can figure out how Memory and sound channel classes should interact.")]
		public readonly PulseSoundChannel pulse2Channel;
		[Obsolete("Mega-hack until I can figure out how Memory and sound channel classes should interact.")]
		public readonly TriangleSoundChannel triangleChannel;
		[Obsolete("Mega-hack until I can figure out how Memory and sound channel classes should interact.")]
		public readonly DmcSoundChannel dmcChannel;

		public Apu(NesEmu core, Dma dma, Memory memory)
		{
			this.noiseChannel = new NoiseSoundChannel(this);
			this.pulse1Channel = new PulseSoundChannel(this, 0x4000);
			this.pulse2Channel = new PulseSoundChannel(this, 0x4004);
			this.triangleChannel = new TriangleSoundChannel(this);
			this.dmcChannel = new DmcSoundChannel(this, dma, memory);

			this.core = core;
			this.memory = memory;
		}

        public static readonly int[][] SequenceMode0 =
        { 
            new int[] { 7459, 7456, 7458, 7457, 1, 1, 7457 }, // NTSC
            new int[] { 8315, 8314, 8312, 8313, 1, 1, 8313 }, // PALB
            new int[] { 7459, 7456, 7458, 7457, 1, 1, 7457 }, // DENDY (acts like NTSC)
        };
        public static readonly int[][] SequenceMode1 = 
        { 
            new int[] { 1, 7458, 7456, 7458, 14910 } , // NTSC
            new int[] { 1, 8314, 8314, 8312, 16626 } , // PALB
            new int[] { 1, 7458, 7456, 7458, 14910 } , // DENDY (acts like NTSC)
        };
        public static readonly byte[] DurationTable = 
        {
            0x0A, 0xFE, 0x14, 0x02, 0x28, 0x04, 0x50, 0x06, 0xA0, 0x08, 0x3C, 0x0A, 0x0E, 0x0C, 0x1A, 0x0E,
            0x0C, 0x10, 0x18, 0x12, 0x30, 0x14, 0x60, 0x16, 0xC0, 0x18, 0x48, 0x1A, 0x10, 0x1C, 0x20, 0x1E,
        };

		public bool oddCycle;
        public int Cycles = 0;
        public bool SequencingMode;
        public byte CurrentSeq = 0;
        public bool IsClockingDuration = false;
        public bool FrameIrqEnabled;
        public bool FrameIrqFlag;
        /*Playback*/
        public IAudioProvider AudioOut;
        private static double[][][][][] mix_table;
        public bool SoundEnabled;
        // default to 44.1KHz settings
        private static float audio_playback_sampleCycles;
        public float audio_playback_samplePeriod;
        private static float audio_playback_sampleReload;
        private static float audio_playback_frequency;
        public byte[] audio_playback_buffer = new byte[44100];
        private static int audio_playback_bufferSize;
        private static bool audio_playback_first_render;
        public int audio_playback_w_pos = 0;//Write position
        public int audio_playback_latency = 0;//Write position
        private static int audio_playback_out;
        public int SystemIndex;
        private static double x, x_1, y, y_1;
        private const double R = 1;// 0.995 for 44100 Hz
        private static double amplitude = 160;
		private readonly NesEmu core;
		private readonly Memory memory;

        public void HardReset()
        {
            switch (this.core.TVFormat)
            {
                case TVSystem.NTSC: SystemIndex = 0; audio_playback_samplePeriod = 1789772.67f; break;
                case TVSystem.PALB: SystemIndex = 1; audio_playback_samplePeriod = 1662607f; break;
                case TVSystem.DENDY: SystemIndex = 2; audio_playback_samplePeriod = 1773448f; break;
            }
            audio_playback_sampleReload = audio_playback_samplePeriod / audio_playback_frequency;
            Cycles = SequenceMode0[SystemIndex][0] - 10;
            FrameIrqFlag = false;
            FrameIrqEnabled = true;
            SequencingMode = false;
            CurrentSeq = 0;
            oddCycle = false;
            IsClockingDuration = false;

			this.pulse1Channel.HardReset();
			this.pulse2Channel.HardReset();
			this.triangleChannel.HardReset();
			this.noiseChannel.HardReset();
			this.dmcChannel.HardReset();
        }
        public void SoftReset()
        {
            Cycles = SequenceMode0[SystemIndex][0] - 10;
            FrameIrqFlag = false;
            FrameIrqEnabled = true;
            SequencingMode = false;
            CurrentSeq = 0;
            oddCycle = false;
            IsClockingDuration = false;

            this.pulse1Channel.HardReset();
			this.pulse2Channel.HardReset();
			this.triangleChannel.HardReset();
			this.noiseChannel.HardReset();
			this.dmcChannel.HardReset();
        }
        public void Shutdown()
        {
            if (audio_playback_buffer == null) return;
            // Noise on shutdown; MISC
            Random r = new Random();
            for (int i = 0; i < audio_playback_buffer.Length; i++)
                audio_playback_buffer[i] = (byte)r.Next(0, 20);
        }
        public void InitializeSoundMixTable()
        {
            mix_table = new double[16][][][][];

            for (int sq1 = 0; sq1 < 16; sq1++)
            {
                mix_table[sq1] = new double[16][][][];

                for (int sq2 = 0; sq2 < 16; sq2++)
                {
                    mix_table[sq1][sq2] = new double[16][][];

                    for (int tri = 0; tri < 16; tri++)
                    {
                        mix_table[sq1][sq2][tri] = new double[16][];

                        for (int noi = 0; noi < 16; noi++)
                        {
                            mix_table[sq1][sq2][tri][noi] = new double[128];

                            for (int dmc = 0; dmc < 128; dmc++)
                            {
                                double sqr = (95.88 / (8128.0 / (sq1 + sq2) + 100));
                                double tnd = (159.79 / (1.0 / ((tri / 8227.0) + (noi / 12241.0) + (dmc / 22638.0)) + 100));

                                mix_table[sq1][sq2][tri][noi][dmc] = (sqr + tnd) * amplitude;
                            }
                        }
                    }
                }
            }
        }
        public void SetupSoundPlayback(IAudioProvider AudioOutput, bool soundEnabled, int frequency, int bufferSize,
            int latencyInBytes)
        {
            audio_playback_latency = latencyInBytes;
            audio_playback_bufferSize = bufferSize;
            audio_playback_frequency = frequency;
            audio_playback_sampleReload = audio_playback_samplePeriod / audio_playback_frequency;
            AudioOut = AudioOutput;
            SoundEnabled = soundEnabled;
            x = x_1 = y = y_1 = 0;
            audio_playback_first_render = true;
            audio_playback_buffer = new byte[audio_playback_bufferSize];
        }
        private void APUUpdatePlayback()
        {
            if (audio_playback_sampleCycles > 0)
                audio_playback_sampleCycles--;
            else
            {
                audio_playback_sampleCycles += audio_playback_sampleReload;
                // DC Blocker Filter
                x = mix_table[this.pulse1Channel.Output]
							 [this.pulse2Channel.Output]
                             [this.triangleChannel.Output]
                             [this.noiseChannel.Output]
							 [this.dmcChannel.Output] + (this.memory.board.enable_external_sound ? this.memory.board.APUGetSamples() : 0);
                y = x - x_1 + (0.995 * y_1);// y[n] = x[n] - x[n - 1] + R * y[n - 1]; R = 0.995 for 44100 Hz

                x_1 = x;
                y_1 = y;

                audio_playback_out = (int)Math.Ceiling(y);

                // NO DC Blocker
                //audio_playback_out = (int)(mix_table[sq1_output][sq2_output][trl_output][noz_output][dmc_output] 
                //    + (board.enable_external_sound ? board.APUGetSamples() : 0));

                if (audio_playback_out > 160)
                    audio_playback_out = 160;
                else if (audio_playback_out < -160)
                    audio_playback_out = -160;
                if (audio_playback_first_render)
                {
                    audio_playback_first_render = false;
                    audio_playback_w_pos = AudioOut.CurrentWritePosition;
                }
                // 16 Bit samples
                if (audio_playback_w_pos >= audio_playback_bufferSize)
                    audio_playback_w_pos = 0;
                audio_playback_buffer[audio_playback_w_pos] = (byte)((audio_playback_out & 0xFF00) >> 8);
                audio_playback_w_pos++;

                if (audio_playback_w_pos >= audio_playback_bufferSize)
                    audio_playback_w_pos = 0;
                audio_playback_buffer[audio_playback_w_pos] = (byte)(audio_playback_out & 0xFF);
                audio_playback_w_pos++;

                if (AudioOut.IsRecording)
                    AudioOut.RecorderAddSample(ref audio_playback_out);
            }
        }
        private void APUClockDuration()
        {
            APUClockEnvelope();

            this.pulse1Channel.ClockLengthCounter();
            this.pulse2Channel.ClockLengthCounter();
            this.triangleChannel.ClockLengthCounter();
            this.noiseChannel.ClockLengthCounter();
			if (this.memory.board.enable_external_sound)
				this.memory.board.OnAPUClockDuration();
        }
        private void APUClockEnvelope()
        {
            this.pulse1Channel.ClockEnvelope();
            this.pulse2Channel.ClockEnvelope();
            this.triangleChannel.ClockEnvelope();
            this.noiseChannel.ClockEnvelope();
			if (this.memory.board.enable_external_sound)
                this.memory.board.OnAPUClockEnvelope();
        }

        private void APUCheckIrq()
        {
            if (FrameIrqEnabled)
                FrameIrqFlag = true;
            if (FrameIrqFlag)
				Interrupts.IRQFlags |= Interrupts.IRQ_APU;
        }

        public void Clock()
        {
            this.IsClockingDuration = false;
            Cycles--;
			this.oddCycle = !this.oddCycle;

            if (Cycles == 0)
            {
                if (!SequencingMode)
                {
                    switch (CurrentSeq)
                    {
                        case 0: APUClockEnvelope(); break;
                        case 1: APUClockDuration(); IsClockingDuration = true; break;
                        case 2: APUClockEnvelope(); break;
                        case 3: APUCheckIrq(); break;
                        case 4: APUCheckIrq(); APUClockDuration(); IsClockingDuration = true; break;
                        case 5: APUCheckIrq(); break;
                    }
                    CurrentSeq++;
                    Cycles += SequenceMode0[SystemIndex][CurrentSeq];
                    if (CurrentSeq == 6)
                        CurrentSeq = 0;
                }
                else
                {
                    switch (CurrentSeq)
                    {
                        case 0:
                        case 2: APUClockDuration(); IsClockingDuration = true; break;
                        case 1:
                        case 3: APUClockEnvelope(); break;
                    }
                    CurrentSeq++;
                    Cycles = SequenceMode1[SystemIndex][CurrentSeq];
                    if (CurrentSeq == 4)
                        CurrentSeq = 0;
                }
            }
            // Clock single
            this.pulse1Channel.ClockSingle();
            this.pulse2Channel.ClockSingle();
            this.triangleChannel.ClockSingle();
            this.noiseChannel.ClockSingle();
            this.dmcChannel.ClockSingle();
			if (this.memory.board.enable_external_sound)
				this.memory.board.OnAPUClockSingle(ref IsClockingDuration);
            // Playback
            APUUpdatePlayback();
        }

		internal void SaveState(BinaryWriter bin)
		{
			bin.Write(Cycles);
			bin.Write(SequencingMode);
			bin.Write(CurrentSeq);
			bin.Write(IsClockingDuration);
			bin.Write(FrameIrqEnabled);
			bin.Write(FrameIrqFlag);
			bin.Write(this.oddCycle);
		}

		internal void LoadState(BinaryReader bin)
		{
			Cycles = bin.ReadInt32();
			SequencingMode = bin.ReadBoolean();
			CurrentSeq = bin.ReadByte();
			IsClockingDuration = bin.ReadBoolean();
			FrameIrqEnabled = bin.ReadBoolean();
			FrameIrqFlag = bin.ReadBoolean();
			oddCycle = bin.ReadBoolean();
		}
	}
}

