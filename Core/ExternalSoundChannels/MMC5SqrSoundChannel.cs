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
    class MMC5SqrSoundChannel
    {
        private static byte[] DurationTable = 
        {
            0x0A, 0xFE, 0x14, 0x02, 0x28, 0x04, 0x50, 0x06, 0xA0, 0x08, 0x3C, 0x0A, 0x0E, 0x0C, 0x1A, 0x0E,
            0x0C, 0x10, 0x18, 0x12, 0x30, 0x14, 0x60, 0x16, 0xC0, 0x18, 0x48, 0x1A, 0x10, 0x1C, 0x20, 0x1E,
        };
        private static int[][] DutyForms =
        {
            new int[] { 1, 1, 1, 1, 1, 1, 1, 0 }, // 87.5%
            new int[] { 1, 1, 1, 1, 1, 1, 0, 0 }, // 75.0%
            new int[] { 1, 1, 1, 1, 0, 0, 0, 0 }, // 50.0%
            new int[] { 1, 1, 0, 0, 0, 0, 0, 0 }, // 25.0%
        };
        private int envelope;
        private bool env_startflag;
        private int env_counter;
        private int env_devider;
        private bool length_counter_halt_flag;
        private bool constant_volume_flag;
        private int volume_decay_time;
        private bool duration_haltRequset = false;
        private byte duration_counter;
        private bool duration_reloadEnabled;
        private byte duration_reload = 0;
        private bool duration_reloadRequst = false;
        private int dutyForm;
        private int dutyStep;
        private int freqTimer;
        private int frequency;
        public byte output;
        private int cycles;

        public void HardReset()
        {
            dutyForm = 0;
            dutyStep = 0;
        }
        public void SoftReset()
        {
            dutyForm = 0;
            dutyStep = 0;
        }

        public void ClockEnvelope()
        {
            if (env_startflag)
            {
                env_startflag = false;
                env_counter = 0xF;
                env_devider = volume_decay_time + 1;
            }
            else
            {
                if (env_devider > 0)
                    env_devider--;
                else
                {
                    env_devider = volume_decay_time + 1;
                    if (env_counter > 0)
                        env_counter--;
                    else if (length_counter_halt_flag)
                        env_counter = 0xF;
                }
            }
            envelope = constant_volume_flag ? volume_decay_time : env_counter;
        }
        public void ClockLengthCounter()
        {
            if (!length_counter_halt_flag)
            {
                if (duration_counter > 0)
                {
                    duration_counter--;
                }
            }
        }
        public void Write5000(byte data)
        {
            volume_decay_time = data & 0xF;
            duration_haltRequset = (data & 0x20) != 0;
            constant_volume_flag = (data & 0x10) != 0;
            envelope = constant_volume_flag ? volume_decay_time : env_counter;
            dutyForm = (data & 0xC0) >> 6;
        }
        public void Write5002(byte data)
        {
            frequency = (frequency & 0x700) | data;
        }
        public void Write5003(byte data)
        {
            duration_reload = DurationTable[data >> 3];
            duration_reloadRequst = true;
            env_startflag = true;
            frequency = (frequency & 0x00FF) | ((data & 0x07) << 8);
            dutyStep = 0;
        }
        public void ClockSingle(bool isClockingLength)
        {
            length_counter_halt_flag = duration_haltRequset;
            if (isClockingLength && duration_counter > 0)
                duration_reloadRequst = false;
            if (duration_reloadRequst)
            {
                if (duration_reloadEnabled)
                    duration_counter = duration_reload;
                duration_reloadRequst = false;
            }

            if (frequency <= 8)
            {
                output = 0;
                return;
            }
            if (cycles > 0)
                cycles--;
            else
            {
                cycles = (frequency << 1) + 2;
                dutyStep--;
                if (dutyStep < 0)
                    dutyStep = 0x7;
                if (duration_counter > 0)
                    output = (byte)(DutyForms[dutyForm][dutyStep] * envelope);
                else
                    output = 0;
            }
        }
        public bool Enabled
        {
            get
            {
                return duration_counter > 0;
            }
            set
            {
                duration_reloadEnabled = value;
                if (!duration_reloadEnabled)
                    duration_counter = 0;
            }
        }

        /// <summary>
        /// Save state
        /// </summary>
        /// <param name="stream">The stream that should be used to write data</param>
        public virtual void SaveState(System.IO.BinaryWriter stream)
        {
            stream.Write(envelope);
            stream.Write(env_startflag);
            stream.Write(env_counter);
            stream.Write(env_devider);
            stream.Write(length_counter_halt_flag);
            stream.Write(constant_volume_flag);
            stream.Write(volume_decay_time);
            stream.Write(duration_haltRequset);
            stream.Write(duration_counter);
            stream.Write(duration_reloadEnabled);
            stream.Write(duration_reload);
            stream.Write(duration_reloadRequst);
            stream.Write(dutyForm);
            stream.Write(dutyStep);
            stream.Write(freqTimer);
            stream.Write(frequency);
            stream.Write(output);
            stream.Write(cycles);
        }
        /// <summary>
        /// Load state
        /// </summary>
        /// <param name="stream">The stream that should be used to read data</param>
        public virtual void LoadState(System.IO.BinaryReader stream)
        {
            envelope = stream.ReadInt32();
            env_startflag = stream.ReadBoolean();
            env_counter = stream.ReadInt32();
            env_devider = stream.ReadInt32();
            length_counter_halt_flag = stream.ReadBoolean();
            constant_volume_flag = stream.ReadBoolean();
            volume_decay_time = stream.ReadInt32();
            duration_haltRequset = stream.ReadBoolean();
            duration_counter = stream.ReadByte();
            duration_reloadEnabled = stream.ReadBoolean();
            duration_reload = stream.ReadByte();
            duration_reloadRequst = stream.ReadBoolean();
            dutyForm = stream.ReadInt32();
            dutyStep = stream.ReadInt32();
            freqTimer = stream.ReadInt32();
            frequency = stream.ReadInt32();
            output = stream.ReadByte();
            cycles = stream.ReadInt32();
        }
    }
}
