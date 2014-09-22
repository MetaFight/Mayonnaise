using System;
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
/*Noise sound channel*/
using System.IO;
namespace MyNes.Core.SoundChannels
{
	public class NoiseSoundChannel
	{
		public NoiseSoundChannel(NesEmu core)
		{
			this.core = core;
		}

		private static readonly int[][] FrequencyTable = 
        { 
            new int [] //NTSC
            {  
                4, 8, 16, 32, 64, 96, 128, 160, 202, 254, 380, 508, 762, 1016, 2034, 4068
            },
            new int [] //PAL
            {  
                4, 8, 14, 30, 60, 88, 118, 148, 188, 236, 354, 472, 708,  944, 1890, 3778
            },
            new int [] //DENDY (same as ntsc for now)
            {  
                4, 8, 16, 32, 64, 96, 128, 160, 202, 254, 380, 508, 762, 1016, 2034, 4068
            }
        };
		private int envelope;
		private bool env_startflag;
		private int env_counter;
		private int env_devider;
		private bool length_counter_halt_flag;
		private bool constant_volume_flag;
		private int volume_decay_time;
		private bool duration_haltRequset = false;
		public byte Duration_counter;
		public bool Duration_reloadEnabled;
		private byte duration_reload = 0;
		private bool duration_reloadRequst = false;
		private bool modeFlag = false;
		private int shiftRegister = 1;
		private int feedback;
		private int freqTimer;
		private int cycles;
		public int Output;
		private NesEmu core;

		public void HardReset()
		{
			envelope = 0;
			env_startflag = false;
			env_counter = 0;
			env_devider = 0;
			length_counter_halt_flag = false;
			constant_volume_flag = false;
			volume_decay_time = 0;
			duration_haltRequset = false;
			Duration_counter = 0;
			Duration_reloadEnabled = false;
			duration_reload = 0;
			duration_reloadRequst = false;
			modeFlag = false;
			shiftRegister = 1;
			cycles = 0;
			freqTimer = 0;
			feedback = 0;
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
				if (Duration_counter > 0)
				{
					Duration_counter--;
				}
			}
		}
		public void ClockSingle()
		{
			length_counter_halt_flag = duration_haltRequset;
			if (this.core.IsClockingDuration && Duration_counter > 0)
				duration_reloadRequst = false;
			if (duration_reloadRequst)
			{
				if (Duration_reloadEnabled)
					Duration_counter = duration_reload;
				duration_reloadRequst = false;
			}

			if (--cycles <= 0)
			{
				cycles = FrequencyTable[this.core.systemIndex][freqTimer];
				if (modeFlag)
					feedback = (shiftRegister >> 6 & 0x1) ^ (shiftRegister & 0x1);
				else
					feedback = (shiftRegister >> 1 & 0x1) ^ (shiftRegister & 0x1);
				shiftRegister >>= 1;
				shiftRegister = ((shiftRegister & 0x3FFF) | (feedback << 14));

				if (Duration_counter > 0 && (shiftRegister & 1) == 0)
					Output = envelope;
				else
					Output = 0;
			}
		}

		internal void SaveState(BinaryWriter bin)
		{
			bin.Write(envelope);
			bin.Write(env_startflag);
			bin.Write(env_counter);
			bin.Write(env_devider);
			bin.Write(length_counter_halt_flag);
			bin.Write(constant_volume_flag);
			bin.Write(volume_decay_time);
			bin.Write(duration_haltRequset);
			bin.Write(Duration_counter);
			bin.Write(Duration_reloadEnabled);
			bin.Write(duration_reload);
			bin.Write(duration_reloadRequst);
			bin.Write(modeFlag);
			bin.Write(shiftRegister);
			bin.Write(feedback);
			bin.Write(freqTimer);
			bin.Write(cycles);
		}

		internal void LoadState(BinaryReader bin)
		{
			envelope = bin.ReadInt32();
			env_startflag = bin.ReadBoolean();
			env_counter = bin.ReadInt32();
			env_devider = bin.ReadInt32();
			length_counter_halt_flag = bin.ReadBoolean();
			constant_volume_flag = bin.ReadBoolean();
			volume_decay_time = bin.ReadInt32();
			duration_haltRequset = bin.ReadBoolean();
			Duration_counter = bin.ReadByte();
			Duration_reloadEnabled = bin.ReadBoolean();
			duration_reload = bin.ReadByte();
			duration_reloadRequst = bin.ReadBoolean();
			modeFlag = bin.ReadBoolean();
			shiftRegister = bin.ReadInt32();
			feedback = bin.ReadInt32();
			freqTimer = bin.ReadInt32();
			cycles = bin.ReadInt32();
		}

		internal void WriteByte(int address, byte value)
		{
			switch (address)
			{
				case 0x400C:
					volume_decay_time = value & 0xF;
					duration_haltRequset = (value & 0x20) != 0;
					constant_volume_flag = (value & 0x10) != 0;
					envelope = constant_volume_flag ? volume_decay_time : env_counter;
					break;

				case 0x400E:
					freqTimer = value & 0x0F;
					modeFlag = (value & 0x80) == 0x80;
					break;

				case 0x400F:
					duration_reload = NesEmu.DurationTable[value >> 3];
					duration_reloadRequst = true;
					env_startflag = true;
					break;

				default:
					throw new NotSupportedException("Address not recognized as Noise address.");
			}
		}
	}
}
