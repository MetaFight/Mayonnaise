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

		public PulseSoundChannel(Apu apu, int addressOffset)
		{
			if (addressOffset != 0x4000 && addressOffset != 0x4004)
			{
				throw new InvalidOperationException("Invalid address offset for Pulse channel.");
			}

			this.apu = apu;
			this.addressOffset = addressOffset;
		}

		private int envelope;
		private bool env_startflag;
		private int env_counter;
		private int env_devider;
		private bool length_counter_halt_flag;
		private bool constant_volume_flag;
		private int volume_decay_time;
		private bool duration_haltRequset = false;
		[Obsolete("Replace with property")]
		public byte Duration_counter;
		[Obsolete("Replace with property")]
		public bool Duration_reloadEnabled;
		private byte duration_reload = 0;
		private bool duration_reloadRequst = false;

		private int dutyForm;
		private int dutyStep;
		private int sweepDeviderPeriod = 0;
		private int sweepShiftCount = 0;
		private int sweepCounter = 0;
		private bool sweepEnable = false;
		private bool sweepReload = false;
		private bool sweepNegateFlag = false;
		private int frequency;
		[Obsolete("Replace with property")]
		public byte Output;
		private int sweep;
		private int cycles;
		private readonly Apu apu;
		private readonly int addressOffset;

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
			dutyForm = 0;
			dutyStep = 0;
			sweepDeviderPeriod = 0;
			sweepShiftCount = 0;
			sweepCounter = 0;
			sweepEnable = false;
			sweepReload = false;
			sweepNegateFlag = false;
			Output = 0;
			cycles = 0;
			sweep = 0;
			frequency = 0;
		}
		private bool IsValidFrequency()
		{
			return
				(frequency >= 0x8) &&
				((sweepNegateFlag) || (((frequency + (frequency >> sweepShiftCount)) & 0x800) == 0));
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

			sweepCounter--;
			if (sweepCounter == 0)
			{
				sweepCounter = sweepDeviderPeriod + 1;
				if (sweepEnable && (sweepShiftCount > 0) && IsValidFrequency())

				{
				sweep = frequency >> sweepShiftCount;
// HACK
if (this.addressOffset == 0x4000)
{
					frequency += sweepNegateFlag ? ~sweep : sweep;
}
else
{
					frequency += sweepNegateFlag ? -sweep : sweep;
}

				}
			}
			if (sweepReload)
			{
				sweepReload = false;
				sweepCounter = sweepDeviderPeriod + 1;
			}
		}
		public void ClockSingle()
		{
			length_counter_halt_flag = duration_haltRequset;
			if (this.apu.IsClockingDuration && Duration_counter > 0)
				duration_reloadRequst = false;
			if (duration_reloadRequst)
			{
				if (Duration_reloadEnabled)
					Duration_counter = duration_reload;
				duration_reloadRequst = false;
			}

// HACK
if (this.addressOffset == 0x4000)
{
					if (frequency == 0)
					{
						Output = 0;
						return;
					}
}
			
			if (cycles > 0)
				cycles--;
			else
			{
				cycles = (frequency << 1) + 2;
				dutyStep--;
				if (dutyStep < 0)
					dutyStep = 0x7;
				if (Duration_counter > 0 && IsValidFrequency())
					Output = (byte)(PulseSoundChannel.DutyForms[dutyForm][dutyStep] * envelope);
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
			bin.Write(dutyForm);
			bin.Write(dutyStep);
			bin.Write(sweepDeviderPeriod);
			bin.Write(sweepShiftCount);
			bin.Write(sweepCounter);
			bin.Write(sweepEnable);
			bin.Write(sweepReload);
			bin.Write(sweepNegateFlag);
			bin.Write(frequency);
			bin.Write(Output);
			bin.Write(sweep);
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
			dutyForm = bin.ReadInt32();
			dutyStep = bin.ReadInt32();
			sweepDeviderPeriod = bin.ReadInt32();
			sweepShiftCount = bin.ReadInt32();
			sweepCounter = bin.ReadInt32();
			sweepEnable = bin.ReadBoolean();
			sweepReload = bin.ReadBoolean();
			sweepNegateFlag = bin.ReadBoolean();
			frequency = bin.ReadInt32();
			Output = bin.ReadByte();
			sweep = bin.ReadInt32();
			cycles = bin.ReadInt32();
		}

		internal void WriteByte(int address, byte value)
		{
			address -= this.addressOffset;

			switch (address)
			{
				case 0x0000:
					volume_decay_time = value & 0xF;
					duration_haltRequset = (value & 0x20) != 0;
					constant_volume_flag = (value & 0x10) != 0;
					envelope = constant_volume_flag ? volume_decay_time : env_counter;
					dutyForm = (value & 0xC0) >> 6;
					break;

				case 0x0001:
					sweepEnable = (value & 0x80) == 0x80;
					sweepDeviderPeriod = (value >> 4) & 7;
					sweepNegateFlag = (value & 0x8) == 0x8;
					sweepShiftCount = value & 7;
					sweepReload = true;
					break;

				case 0x0002:
					frequency = (frequency & 0x0700) | value;
					break;

				case 0x0003:
					duration_reload = Apu.DurationTable[value >> 3];
					duration_reloadRequst = true;
					frequency = (frequency & 0x00FF) | ((value & 7) << 8);
					dutyStep = 0;
					env_startflag = true;
					break;

				default:
					throw new NotSupportedException("Address not recognized as Pulse address.");
			}
		}
	}
}
