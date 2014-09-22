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

		private bool length_counter_halt_flag;
		private bool duration_haltRequset = false;
		public byte Duration_counter;
		public bool Duration_reloadEnabled;
		private byte duration_reload = 0;
		private bool duration_reloadRequst = false;
		private byte linearCounter = 0;
		private byte linearCounterReload;
		private byte step;
		private bool linearCounterHalt;
		private bool halt;
		private int frequency;
		public byte Output;
		private int cycles;
		private NesEmu core;

		public void HardReset()
		{
			length_counter_halt_flag = false;
			duration_haltRequset = false;
			Duration_counter = 0;
			Duration_reloadEnabled = false;
			duration_reload = 0;
			duration_reloadRequst = false;
			linearCounter = 0;
			linearCounterReload = 0;
			Output = 0;
			step = 0;
			linearCounterHalt = false;
			halt = true;
			frequency = 0;
			cycles = 0;
		}
		public void ClockEnvelope()
		{
			if (halt)
			{
				linearCounter = linearCounterReload;
			}
			else
			{
				if (linearCounter != 0)
				{
					linearCounter--;
				}
			}

			halt &= linearCounterHalt;
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
				cycles = frequency + 1;
				if (Duration_counter > 0 && linearCounter > 0)
				{
					if (frequency >= 4)
					{
						step++;
						step &= 0x1F;
						Output = StepSequence[step];
					}
				}
			}
		}

		internal void SaveState(BinaryWriter bin)
		{
			bin.Write(length_counter_halt_flag);
			bin.Write(duration_haltRequset);
			bin.Write(this.Duration_counter);
			bin.Write(Duration_reloadEnabled);
			bin.Write(duration_reload);
			bin.Write(duration_reloadRequst);
			bin.Write(linearCounter);
			bin.Write(linearCounterReload);
			bin.Write(step);
			bin.Write(linearCounterHalt);
			bin.Write(halt);
			bin.Write(frequency);
			bin.Write(this.Output);
			bin.Write(cycles);
		}

		internal void WriteByte(int address, byte value)
		{
			switch (address)
			{
				case 0x4008:
					linearCounterHalt = duration_haltRequset = (value & 0x80) != 0;
					linearCounterReload = (byte)(value & 0x7F);
					break;

				case 0x400A:
					frequency = (frequency & 0x700) | value;
					break;

				case 0x400B:
					frequency = (frequency & 0x00FF) | ((value & 7) << 8);

					duration_reload = NesEmu.DurationTable[value >> 3];
					duration_reloadRequst = true;
					halt = true;
					break;

				default:
					throw new NotSupportedException("Address not recognized as Triangle address.");
			}
		}

		internal void LoadState(BinaryReader bin)
		{
			length_counter_halt_flag = bin.ReadBoolean();
			duration_haltRequset = bin.ReadBoolean();
			this.Duration_counter = bin.ReadByte();
			Duration_reloadEnabled = bin.ReadBoolean();
			duration_reload = bin.ReadByte();
			duration_reloadRequst = bin.ReadBoolean();
			linearCounter = bin.ReadByte();
			linearCounterReload = bin.ReadByte();
			step = bin.ReadByte();
			linearCounterHalt = bin.ReadBoolean();
			halt = bin.ReadBoolean();
			frequency = bin.ReadInt32();
			this.Output = bin.ReadByte();
			cycles = bin.ReadInt32();
		}
	}
}
