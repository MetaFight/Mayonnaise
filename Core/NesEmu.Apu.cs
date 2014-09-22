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

/*APU section*/
namespace MyNes.Core
{
    public partial class NesEmu
    {
        private static int[][] SequenceMode0 =
        { 
            new int[] { 7459, 7456, 7458, 7457, 1, 1, 7457 }, // NTSC
            new int[] { 8315, 8314, 8312, 8313, 1, 1, 8313 }, // PALB
            new int[] { 7459, 7456, 7458, 7457, 1, 1, 7457 }, // DENDY (acts like NTSC)
        };
        private static int[][] SequenceMode1 = 
        { 
            new int[] { 1, 7458, 7456, 7458, 14910 } , // NTSC
            new int[] { 1, 8314, 8314, 8312, 16626 } , // PALB
            new int[] { 1, 7458, 7456, 7458, 14910 } , // DENDY (acts like NTSC)
        };
        private static byte[] DurationTable = 
        {
            0x0A, 0xFE, 0x14, 0x02, 0x28, 0x04, 0x50, 0x06, 0xA0, 0x08, 0x3C, 0x0A, 0x0E, 0x0C, 0x1A, 0x0E,
            0x0C, 0x10, 0x18, 0x12, 0x30, 0x14, 0x60, 0x16, 0xC0, 0x18, 0x48, 0x1A, 0x10, 0x1C, 0x20, 0x1E,
        };
        private static byte[][] PulseDutyForms =
        {
            new byte[] {  0, 1, 0, 0, 0, 0, 0, 0 }, // 12.5%
            new byte[] {  0, 1, 1, 0, 0, 0, 0, 0 }, // 25.0%
            new byte[] {  0, 1, 1, 1, 1, 0, 0, 0 }, // 50.0%
            new byte[] {  1, 0, 0, 1, 1, 1, 1, 1 }, // 75.0% (25.0% negated)
        };
        private static byte[] TrlStepSequence =
        {
            0x0F, 0x0E, 0x0D, 0x0C, 0x0B, 0x0A, 0x09, 0x08, 0x07, 0x06, 0x05, 0x04, 0x03, 0x02, 0x01, 0x00,
            0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F,
        };
        private static int Cycles = 0;
        private static bool SequencingMode;
        private static byte CurrentSeq = 0;
        private static bool isClockingDuration = false;
        private static bool FrameIrqEnabled;
        private static bool FrameIrqFlag;
        private static bool oddCycle;
        /*Playback*/
        private static IAudioProvider AudioOut;
        private static double[][][][][] mix_table;
        private static bool SoundEnabled;
        // default to 44.1KHz settings
        private static float audio_playback_sampleCycles;
        private static float audio_playback_samplePeriod;
        private static float audio_playback_sampleReload;
        private static float audio_playback_frequency;
        private static byte[] audio_playback_buffer = new byte[44100];
        private static int audio_playback_bufferSize;
        private static bool audio_playback_first_render;
        public static int audio_playback_w_pos = 0;//Write position
        public static int audio_playback_latency = 0;//Write position
        private static int audio_playback_out;
        private static int systemIndex;
        private static double x, x_1, y, y_1;
        private const double R = 1;// 0.995 for 44100 Hz
        private static double amplitude = 160;

        private static void APUHardReset()
        {
            switch (TVFormat)
            {
                case TVSystem.NTSC: systemIndex = 0; audio_playback_samplePeriod = 1789772.67f; break;
                case TVSystem.PALB: systemIndex = 1; audio_playback_samplePeriod = 1662607f; break;
                case TVSystem.DENDY: systemIndex = 2; audio_playback_samplePeriod = 1773448f; break;
            }
            audio_playback_sampleReload = audio_playback_samplePeriod / audio_playback_frequency;
            Cycles = SequenceMode0[systemIndex][0] - 10;
            FrameIrqFlag = false;
            FrameIrqEnabled = true;
            SequencingMode = false;
            CurrentSeq = 0;
            oddCycle = false;
            isClockingDuration = false;

            Sq1HardReset();
            Sq2HardReset();
            TrlHardReset();
            NozHardReset();
            DMCHardReset();
        }
        private static void APUSoftReset()
        {
            Cycles = SequenceMode0[systemIndex][0] - 10;
            FrameIrqFlag = false;
            FrameIrqEnabled = true;
            SequencingMode = false;
            CurrentSeq = 0;
            oddCycle = false;
            isClockingDuration = false;

            Sq1HardReset();
            Sq2HardReset();
            TrlHardReset();
            NozHardReset();
            DMCHardReset();
        }
        private static void APUShutdown()
        {
            if (audio_playback_buffer == null) return;
            // Noise on shutdown; MISC
            Random r = new Random();
            for (int i = 0; i < audio_playback_buffer.Length; i++)
                audio_playback_buffer[i] = (byte)r.Next(0, 20);
        }
        private static void InitializeSoundMixTable()
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
        public static void SetupSoundPlayback(IAudioProvider AudioOutput, bool soundEnabled, int frequency, int bufferSize,
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
        private static void APUUpdatePlayback()
        {
            if (audio_playback_sampleCycles > 0)
                audio_playback_sampleCycles--;
            else
            {
                audio_playback_sampleCycles += audio_playback_sampleReload;
                // DC Blocker Filter
                x = mix_table[sq1_output]
                             [sq2_output]
                             [trl_output]
                             [noz_output]
                             [dmc_output] + (board.enable_external_sound ? board.APUGetSamples() : 0);
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
        private static void APUClockDuration()
        {
            APUClockEnvelope();

            Sq1ClockLengthCounter();
            Sq2ClockLengthCounter();
            TrlClockLengthCounter();
            NozClockLengthCounter();
            if (board.enable_external_sound)
                board.OnAPUClockDuration();
        }
        private static void APUClockEnvelope()
        {
            Sq1ClockEnvelope();
            Sq2ClockEnvelope();
            TrlClockEnvelope();
            NozClockEnvelope();
            if (board.enable_external_sound)
                board.OnAPUClockEnvelope();
        }

        private static void APUCheckIrq()
        {
            if (FrameIrqEnabled)
                FrameIrqFlag = true;
            if (FrameIrqFlag)
                IRQFlags |= IRQ_APU;
        }
        private static void APUClock()
        {
            isClockingDuration = false;
            Cycles--;
            oddCycle = !oddCycle;

            if (Cycles == 0)
            {
                if (!SequencingMode)
                {
                    switch (CurrentSeq)
                    {
                        case 0: APUClockEnvelope(); break;
                        case 1: APUClockDuration(); isClockingDuration = true; break;
                        case 2: APUClockEnvelope(); break;
                        case 3: APUCheckIrq(); break;
                        case 4: APUCheckIrq(); APUClockDuration(); isClockingDuration = true; break;
                        case 5: APUCheckIrq(); break;
                    }
                    CurrentSeq++;
                    Cycles += SequenceMode0[systemIndex][CurrentSeq];
                    if (CurrentSeq == 6)
                        CurrentSeq = 0;
                }
                else
                {
                    switch (CurrentSeq)
                    {
                        case 0:
                        case 2: APUClockDuration(); isClockingDuration = true; break;
                        case 1:
                        case 3: APUClockEnvelope(); break;
                    }
                    CurrentSeq++;
                    Cycles = SequenceMode1[systemIndex][CurrentSeq];
                    if (CurrentSeq == 4)
                        CurrentSeq = 0;
                }
            }
            // Clock single
            Sq1ClockSingle();
            Sq2ClockSingle();
            TrlClockSingle();
            NozClockSingle();
            DMCClockSingle();
            if (board.enable_external_sound)
                board.OnAPUClockSingle(ref isClockingDuration);
            // Playback
            APUUpdatePlayback();
        }
    }
}

