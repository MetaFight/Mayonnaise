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

		private static int[][] NozFrequencyTable = 
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
		private static int noz_envelope;
		private static bool noz_env_startflag;
		private static int noz_env_counter;
		private static int noz_env_devider;
		private static bool noz_length_counter_halt_flag;
		private static bool noz_constant_volume_flag;
		private static int noz_volume_decay_time;
		private static bool noz_duration_haltRequset = false;
		public byte Duration_counter;
		public bool Duration_reloadEnabled;
		private static byte noz_duration_reload = 0;
		private static bool noz_duration_reloadRequst = false;
		private static bool noz_modeFlag = false;
		private static int noz_shiftRegister = 1;
		private static int noz_feedback;
		private static int noz_freqTimer;
		private static int noz_cycles;
		public int Output;
		private NesEmu core;

		public void HardReset()
		{
			noz_envelope = 0;
			noz_env_startflag = false;
			noz_env_counter = 0;
			noz_env_devider = 0;
			noz_length_counter_halt_flag = false;
			noz_constant_volume_flag = false;
			noz_volume_decay_time = 0;
			noz_duration_haltRequset = false;
			Duration_counter = 0;
			Duration_reloadEnabled = false;
			noz_duration_reload = 0;
			noz_duration_reloadRequst = false;
			noz_modeFlag = false;
			noz_shiftRegister = 1;
			noz_cycles = 0;
			noz_freqTimer = 0;
			noz_feedback = 0;
		}
		public void NozClockEnvelope()
		{
			if (noz_env_startflag)
			{
				noz_env_startflag = false;
				noz_env_counter = 0xF;
				noz_env_devider = noz_volume_decay_time + 1;
			}
			else
			{
				if (noz_env_devider > 0)
					noz_env_devider--;
				else
				{
					noz_env_devider = noz_volume_decay_time + 1;
					if (noz_env_counter > 0)
						noz_env_counter--;
					else if (noz_length_counter_halt_flag)
						noz_env_counter = 0xF;
				}
			}
			noz_envelope = noz_constant_volume_flag ? noz_volume_decay_time : noz_env_counter;
		}
		public void ClockLengthCounter()
		{
			if (!noz_length_counter_halt_flag)
			{
				if (Duration_counter > 0)
				{
					Duration_counter--;
				}
			}
		}
		public void ClockSingle()
		{
			noz_length_counter_halt_flag = noz_duration_haltRequset;
			if (this.core.IsClockingDuration && Duration_counter > 0)
				noz_duration_reloadRequst = false;
			if (noz_duration_reloadRequst)
			{
				if (Duration_reloadEnabled)
					Duration_counter = noz_duration_reload;
				noz_duration_reloadRequst = false;
			}

			if (--noz_cycles <= 0)
			{
				noz_cycles = NozFrequencyTable[this.core.systemIndex][noz_freqTimer];
				if (noz_modeFlag)
					noz_feedback = (noz_shiftRegister >> 6 & 0x1) ^ (noz_shiftRegister & 0x1);
				else
					noz_feedback = (noz_shiftRegister >> 1 & 0x1) ^ (noz_shiftRegister & 0x1);
				noz_shiftRegister >>= 1;
				noz_shiftRegister = ((noz_shiftRegister & 0x3FFF) | (noz_feedback << 14));

				if (Duration_counter > 0 && (noz_shiftRegister & 1) == 0)
					Output = noz_envelope;
				else
					Output = 0;
			}
		}

		internal void SaveState(BinaryWriter bin)
		{
			bin.Write(noz_envelope);
			bin.Write(noz_env_startflag);
			bin.Write(noz_env_counter);
			bin.Write(noz_env_devider);
			bin.Write(noz_length_counter_halt_flag);
			bin.Write(noz_constant_volume_flag);
			bin.Write(noz_volume_decay_time);
			bin.Write(noz_duration_haltRequset);
			bin.Write(Duration_counter);
			bin.Write(Duration_reloadEnabled);
			bin.Write(noz_duration_reload);
			bin.Write(noz_duration_reloadRequst);
			bin.Write(noz_modeFlag);
			bin.Write(noz_shiftRegister);
			bin.Write(noz_feedback);
			bin.Write(noz_freqTimer);
			bin.Write(noz_cycles);
		}

		internal void LoadState(BinaryReader bin)
		{
			noz_envelope = bin.ReadInt32();
			noz_env_startflag = bin.ReadBoolean();
			noz_env_counter = bin.ReadInt32();
			noz_env_devider = bin.ReadInt32();
			noz_length_counter_halt_flag = bin.ReadBoolean();
			noz_constant_volume_flag = bin.ReadBoolean();
			noz_volume_decay_time = bin.ReadInt32();
			noz_duration_haltRequset = bin.ReadBoolean();
			Duration_counter = bin.ReadByte();
			Duration_reloadEnabled = bin.ReadBoolean();
			noz_duration_reload = bin.ReadByte();
			noz_duration_reloadRequst = bin.ReadBoolean();
			noz_modeFlag = bin.ReadBoolean();
			noz_shiftRegister = bin.ReadInt32();
			noz_feedback = bin.ReadInt32();
			noz_freqTimer = bin.ReadInt32();
			noz_cycles = bin.ReadInt32();
		}

		internal void WriteByte(int address, byte value)
		{
			switch (address)
			{
				case 0x400C:
					noz_volume_decay_time = value & 0xF;
					noz_duration_haltRequset = (value & 0x20) != 0;
					noz_constant_volume_flag = (value & 0x10) != 0;
					noz_envelope = noz_constant_volume_flag ? noz_volume_decay_time : noz_env_counter;
					break;

				case 0x400E:
					noz_freqTimer = value & 0x0F;
					noz_modeFlag = (value & 0x80) == 0x80;
					break;

				case 0x400F:
					noz_duration_reload = NesEmu.DurationTable[value >> 3];
					noz_duration_reloadRequst = true;
					noz_env_startflag = true;
					break;

				default:
					throw new NotSupportedException("Address not recognized as Noise address.");
			}
		}
	}
}
