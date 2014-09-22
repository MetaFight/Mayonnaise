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
/*Triangle sound channel*/
using System.IO;
namespace MyNes.Core.SoundChannels
{
	public class TriangleSoundChannel
	{
		private static readonly byte[] StepSequence =
        {
            0x0F, 0x0E, 0x0D, 0x0C, 0x0B, 0x0A, 0x09, 0x08, 0x07, 0x06, 0x05, 0x04, 0x03, 0x02, 0x01, 0x00,
            0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F,
        };

		public TriangleSoundChannel(NesEmu core)
		{
			this.core = core;
		}

		private static bool trl_length_counter_halt_flag;
		private static bool trl_duration_haltRequset = false;
		public byte Duration_counter;
		public bool Duration_reloadEnabled;
		private static byte trl_duration_reload = 0;
		private static bool trl_duration_reloadRequst = false;
		private static byte trl_linearCounter = 0;
		private static byte trl_linearCounterReload;
		private static byte trl_step;
		private static bool trl_linearCounterHalt;
		private static bool trl_halt;
		private static int trl_frequency;
		public byte Output;
		private static int trl_cycles;
		private NesEmu core;

		private static void TrlShutdown()
		{

		}
		public void HardReset()
		{
			trl_length_counter_halt_flag = false;
			trl_duration_haltRequset = false;
			Duration_counter = 0;
			Duration_reloadEnabled = false;
			trl_duration_reload = 0;
			trl_duration_reloadRequst = false;
			trl_linearCounter = 0;
			trl_linearCounterReload = 0;
			Output = 0;
			trl_step = 0;
			trl_linearCounterHalt = false;
			trl_halt = true;
			trl_frequency = 0;
			trl_cycles = 0;
		}
		public void ClockEnvelope()
		{
			if (trl_halt)
			{
				trl_linearCounter = trl_linearCounterReload;
			}
			else
			{
				if (trl_linearCounter != 0)
				{
					trl_linearCounter--;
				}
			}

			trl_halt &= trl_linearCounterHalt;
		}
		public void ClockLengthCounter()
		{
			if (!trl_length_counter_halt_flag)
			{
				if (Duration_counter > 0)
				{
					Duration_counter--;
				}
			}
		}
		public void ClockSingle()
		{
			trl_length_counter_halt_flag = trl_duration_haltRequset;
			if (this.core.IsClockingDuration && Duration_counter > 0)
				trl_duration_reloadRequst = false;
			if (trl_duration_reloadRequst)
			{
				if (Duration_reloadEnabled)
					Duration_counter = trl_duration_reload;
				trl_duration_reloadRequst = false;
			}
			if (--trl_cycles <= 0)
			{
				trl_cycles = trl_frequency + 1;
				if (Duration_counter > 0 && trl_linearCounter > 0)
				{
					if (trl_frequency >= 4)
					{
						trl_step++;
						trl_step &= 0x1F;
						Output = StepSequence[trl_step];
					}
				}
			}
		}

		internal void SaveState(BinaryWriter bin)
		{
			bin.Write(trl_length_counter_halt_flag);
			bin.Write(trl_duration_haltRequset);
			bin.Write(this.Duration_counter);
			bin.Write(Duration_reloadEnabled);
			bin.Write(trl_duration_reload);
			bin.Write(trl_duration_reloadRequst);
			bin.Write(trl_linearCounter);
			bin.Write(trl_linearCounterReload);
			bin.Write(trl_step);
			bin.Write(trl_linearCounterHalt);
			bin.Write(trl_halt);
			bin.Write(trl_frequency);
			bin.Write(this.Output);
			bin.Write(trl_cycles);
		}

		internal void WriteByte(int address, byte value)
		{
			switch (address)
			{
				case 0x4008:
					trl_linearCounterHalt = trl_duration_haltRequset = (value & 0x80) != 0;
					trl_linearCounterReload = (byte)(value & 0x7F);
					break;

				case 0x400A:
					trl_frequency = (trl_frequency & 0x700) | value;
					break;

				case 0x400B:
					trl_frequency = (trl_frequency & 0x00FF) | ((value & 7) << 8);

					trl_duration_reload = NesEmu.DurationTable[value >> 3];
					trl_duration_reloadRequst = true;
					trl_halt = true;
					break;

				default:
					throw new NotSupportedException("Address not recognized as Triangle address.");
			}
		}

		internal void LoadState(BinaryReader bin)
		{
			trl_length_counter_halt_flag = bin.ReadBoolean();
			trl_duration_haltRequset = bin.ReadBoolean();
			this.Duration_counter = bin.ReadByte();
			Duration_reloadEnabled = bin.ReadBoolean();
			trl_duration_reload = bin.ReadByte();
			trl_duration_reloadRequst = bin.ReadBoolean();
			trl_linearCounter = bin.ReadByte();
			trl_linearCounterReload = bin.ReadByte();
			trl_step = bin.ReadByte();
			trl_linearCounterHalt = bin.ReadBoolean();
			trl_halt = bin.ReadBoolean();
			trl_frequency = bin.ReadInt32();
			this.Output = bin.ReadByte();
			trl_cycles = bin.ReadInt32();
		}
	}
}
