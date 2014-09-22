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
/*Pulse 1 sound channel*/
using System.IO;
namespace MyNes.Core.SoundChannels
{
	public class PulseSoundChannel
	{
		private static readonly byte[][] DutyForms =
        {
            new byte[] {  0, 1, 0, 0, 0, 0, 0, 0 }, // 12.5%
            new byte[] {  0, 1, 1, 0, 0, 0, 0, 0 }, // 25.0%
            new byte[] {  0, 1, 1, 1, 1, 0, 0, 0 }, // 50.0%
            new byte[] {  1, 0, 0, 1, 1, 1, 1, 1 }, // 75.0% (25.0% negated)
        };

		public PulseSoundChannel(NesEmu core, int addressOffset)
		{
			if (addressOffset != 0x4000 && addressOffset != 0x4004)
			{
				throw new InvalidOperationException("Invalid address offset for Pulse channel.");
			}

			this.core = core;
			this.addressOffset = addressOffset;
		}

		private int sq1_envelope;
		private bool sq1_env_startflag;
		private int sq1_env_counter;
		private int sq1_env_devider;
		private bool sq1_length_counter_halt_flag;
		private bool sq1_constant_volume_flag;
		private int sq1_volume_decay_time;
		private bool sq1_duration_haltRequset = false;
		[Obsolete("Replace with property")]
		public byte Duration_counter;
		[Obsolete("Replace with property")]
		public bool Duration_reloadEnabled;
		private byte sq1_duration_reload = 0;
		private bool sq1_duration_reloadRequst = false;

		private int sq1_dutyForm;
		private int sq1_dutyStep;
		private int sq1_sweepDeviderPeriod = 0;
		private int sq1_sweepShiftCount = 0;
		private int sq1_sweepCounter = 0;
		private bool sq1_sweepEnable = false;
		private bool sq1_sweepReload = false;
		private bool sq1_sweepNegateFlag = false;
		private int sq1_frequency;
		[Obsolete("Replace with property")]
		public byte Output;
		private int sq1_sweep;
		private int sq1_cycles;
		private NesEmu core;
		private int addressOffset;

		private void Sq1Shutdown()
		{

		}
		public void HardReset()
		{
			sq1_envelope = 0;
			sq1_env_startflag = false;
			sq1_env_counter = 0;
			sq1_env_devider = 0;
			sq1_length_counter_halt_flag = false;
			sq1_constant_volume_flag = false;
			sq1_volume_decay_time = 0;
			sq1_duration_haltRequset = false;
			Duration_counter = 0;
			Duration_reloadEnabled = false;
			sq1_duration_reload = 0;
			sq1_duration_reloadRequst = false;
			sq1_dutyForm = 0;
			sq1_dutyStep = 0;
			sq1_sweepDeviderPeriod = 0;
			sq1_sweepShiftCount = 0;
			sq1_sweepCounter = 0;
			sq1_sweepEnable = false;
			sq1_sweepReload = false;
			sq1_sweepNegateFlag = false;
			Output = 0;
			sq1_cycles = 0;
			sq1_sweep = 0;
			sq1_frequency = 0;
		}
		private bool Sq1IsValidFrequency()
		{
			return
				(sq1_frequency >= 0x8) &&
				((sq1_sweepNegateFlag) || (((sq1_frequency + (sq1_frequency >> sq1_sweepShiftCount)) & 0x800) == 0));
		}
		public void ClockEnvelope()
		{
			if (sq1_env_startflag)
			{
				sq1_env_startflag = false;
				sq1_env_counter = 0xF;
				sq1_env_devider = sq1_volume_decay_time + 1;
			}
			else
			{
				if (sq1_env_devider > 0)
					sq1_env_devider--;
				else
				{
					sq1_env_devider = sq1_volume_decay_time + 1;
					if (sq1_env_counter > 0)
						sq1_env_counter--;
					else if (sq1_length_counter_halt_flag)
						sq1_env_counter = 0xF;
				}
			}
			sq1_envelope = sq1_constant_volume_flag ? sq1_volume_decay_time : sq1_env_counter;
		}
		public void ClockLengthCounter()
		{
			if (!sq1_length_counter_halt_flag)
			{
				if (Duration_counter > 0)
				{
					Duration_counter--;
				}
			}

			sq1_sweepCounter--;
			if (sq1_sweepCounter == 0)
			{
				sq1_sweepCounter = sq1_sweepDeviderPeriod + 1;
				if (sq1_sweepEnable && (sq1_sweepShiftCount > 0) && Sq1IsValidFrequency())

				{
				sq1_sweep = sq1_frequency >> sq1_sweepShiftCount;
// HACK
if (this.addressOffset == 0x4000)
{
					sq1_frequency += sq1_sweepNegateFlag ? ~sq1_sweep : sq1_sweep;
}
else
{
					sq1_frequency += sq1_sweepNegateFlag ? -sq1_sweep : sq1_sweep;
}

				}
			}
			if (sq1_sweepReload)
			{
				sq1_sweepReload = false;
				sq1_sweepCounter = sq1_sweepDeviderPeriod + 1;
			}
		}
		public void ClockSingle()
		{
			sq1_length_counter_halt_flag = sq1_duration_haltRequset;
			if (this.core.IsClockingDuration && Duration_counter > 0)
				sq1_duration_reloadRequst = false;
			if (sq1_duration_reloadRequst)
			{
				if (Duration_reloadEnabled)
					Duration_counter = sq1_duration_reload;
				sq1_duration_reloadRequst = false;
			}

// HACK
if (this.addressOffset == 0x4000)
{
					if (sq1_frequency == 0)
					{
						Output = 0;
						return;
					}
}
			
			if (sq1_cycles > 0)
				sq1_cycles--;
			else
			{
				sq1_cycles = (sq1_frequency << 1) + 2;
				sq1_dutyStep--;
				if (sq1_dutyStep < 0)
					sq1_dutyStep = 0x7;
				if (Duration_counter > 0 && Sq1IsValidFrequency())
					Output = (byte)(PulseSoundChannel.DutyForms[sq1_dutyForm][sq1_dutyStep] * sq1_envelope);
				else
					Output = 0;
			}
		}

		internal void SaveState(BinaryWriter bin)
		{
			bin.Write(sq1_envelope);
			bin.Write(sq1_env_startflag);
			bin.Write(sq1_env_counter);
			bin.Write(sq1_env_devider);
			bin.Write(sq1_length_counter_halt_flag);
			bin.Write(sq1_constant_volume_flag);
			bin.Write(sq1_volume_decay_time);
			bin.Write(sq1_duration_haltRequset);
			bin.Write(Duration_counter);
			bin.Write(Duration_reloadEnabled);
			bin.Write(sq1_duration_reload);
			bin.Write(sq1_duration_reloadRequst);
			bin.Write(sq1_dutyForm);
			bin.Write(sq1_dutyStep);
			bin.Write(sq1_sweepDeviderPeriod);
			bin.Write(sq1_sweepShiftCount);
			bin.Write(sq1_sweepCounter);
			bin.Write(sq1_sweepEnable);
			bin.Write(sq1_sweepReload);
			bin.Write(sq1_sweepNegateFlag);
			bin.Write(sq1_frequency);
			bin.Write(Output);
			bin.Write(sq1_sweep);
			bin.Write(sq1_cycles);
		}

		internal void LoadState(BinaryReader bin)
		{
			sq1_envelope = bin.ReadInt32();
			sq1_env_startflag = bin.ReadBoolean();
			sq1_env_counter = bin.ReadInt32();
			sq1_env_devider = bin.ReadInt32();
			sq1_length_counter_halt_flag = bin.ReadBoolean();
			sq1_constant_volume_flag = bin.ReadBoolean();
			sq1_volume_decay_time = bin.ReadInt32();
			sq1_duration_haltRequset = bin.ReadBoolean();
			Duration_counter = bin.ReadByte();
			Duration_reloadEnabled = bin.ReadBoolean();
			sq1_duration_reload = bin.ReadByte();
			sq1_duration_reloadRequst = bin.ReadBoolean();
			sq1_dutyForm = bin.ReadInt32();
			sq1_dutyStep = bin.ReadInt32();
			sq1_sweepDeviderPeriod = bin.ReadInt32();
			sq1_sweepShiftCount = bin.ReadInt32();
			sq1_sweepCounter = bin.ReadInt32();
			sq1_sweepEnable = bin.ReadBoolean();
			sq1_sweepReload = bin.ReadBoolean();
			sq1_sweepNegateFlag = bin.ReadBoolean();
			sq1_frequency = bin.ReadInt32();
			Output = bin.ReadByte();
			sq1_sweep = bin.ReadInt32();
			sq1_cycles = bin.ReadInt32();
		}

		internal void WriteByte(int address, byte value)
		{
			address -= this.addressOffset;

			switch (address)
			{
				case 0x0000:
					sq1_volume_decay_time = value & 0xF;
					sq1_duration_haltRequset = (value & 0x20) != 0;
					sq1_constant_volume_flag = (value & 0x10) != 0;
					sq1_envelope = sq1_constant_volume_flag ? sq1_volume_decay_time : sq1_env_counter;
					sq1_dutyForm = (value & 0xC0) >> 6;
					break;

				case 0x0001:
					sq1_sweepEnable = (value & 0x80) == 0x80;
					sq1_sweepDeviderPeriod = (value >> 4) & 7;
					sq1_sweepNegateFlag = (value & 0x8) == 0x8;
					sq1_sweepShiftCount = value & 7;
					sq1_sweepReload = true;
					break;

				case 0x0002:
					sq1_frequency = (sq1_frequency & 0x0700) | value;
					break;

				case 0x0003:
					sq1_duration_reload = NesEmu.DurationTable[value >> 3];
					sq1_duration_reloadRequst = true;
					sq1_frequency = (sq1_frequency & 0x00FF) | ((value & 7) << 8);
					sq1_dutyStep = 0;
					sq1_env_startflag = true;
					break;

				default:
					throw new NotSupportedException("Address not recognized as Pulse address.");
			}
		}
	}
}
