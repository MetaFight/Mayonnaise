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
/*DMC sound channel*/
namespace MyNes.Core.SoundChannels
{
    public class DmcSoundChannel
    {
		private readonly int[][] DMCFrequencyTable = 
        { 
            new int[]//NTSC
            { 
               428, 380, 340, 320, 286, 254, 226, 214, 190, 160, 142, 128, 106,  84,  72,  54
            },
            new int[]//PAL
            { 
               398, 354, 316, 298, 276, 236, 210, 198, 176, 148, 132, 118,  98,  78,  66,  50
            },  
            new int[]//DENDY (same as ntsc for now)
            { 
               428, 380, 340, 320, 286, 254, 226, 214, 190, 160, 142, 128, 106,  84,  72,  54
            },
        };
        public bool DeltaIrqOccur;
        public bool DMCIrqEnabled;
        public bool dmc_dmaLooping;
        private bool dmc_dmaEnabled;
        public bool dmc_bufferFull = false;
        public int dmc_dmaAddrRefresh;
        public int dmc_dmaSizeRefresh;
        public int dmc_dmaSize;
        private int dmc_dmaBits = 0;
        private byte dmc_dmaByte = 0;
        public int dmc_dmaAddr = 0;
        private byte dmc_dmaBuffer = 0;
        public byte Output;
        private int dmc_cycles;
        public int dmc_freqTimer;
		private readonly Apu apu;
		private readonly Dma dma;
		private readonly Memory memory;

		public DmcSoundChannel(Apu apu, Dma dma, Memory memory)
		{
			this.apu = apu;
			this.dma = dma;
			this.memory = memory;
			this.dma.DmcDma += this.OnDmcDma;
		}

		void OnDmcDma(object sender, System.EventArgs e)
		{
			dmc_bufferFull = true;

			dmc_dmaBuffer = this.memory.Read(dmc_dmaAddr);

			if (++dmc_dmaAddr == 0x10000)
				dmc_dmaAddr = 0x8000;
			if (dmc_dmaSize > 0)
				dmc_dmaSize--;

			if (dmc_dmaSize == 0)
			{
				if (dmc_dmaLooping)
				{
					dmc_dmaAddr = dmc_dmaAddrRefresh;
					dmc_dmaSize = dmc_dmaSizeRefresh;
				}
				else if (DMCIrqEnabled)
				{
					NesEmu.IRQFlags |= NesEmu.IRQ_DMC;
					DeltaIrqOccur = true;
				}
			}
		}

        public void HardReset()
        {
            DeltaIrqOccur = false;
            DMCIrqEnabled = false;
            dmc_dmaLooping = false;
            dmc_dmaEnabled = false;
            dmc_bufferFull = false;
            Output = 0;
            dmc_dmaAddr = dmc_dmaAddrRefresh = 0xC000;
            dmc_dmaSizeRefresh = 0;
            dmc_dmaSize = 0;
            dmc_dmaBits = 1;
            dmc_dmaByte = 1;
            dmc_dmaAddr = 0;
            dmc_dmaBuffer = 0;
            dmc_freqTimer = 0;
			dmc_cycles = DMCFrequencyTable[this.apu.SystemIndex][dmc_freqTimer];
        }
        public void ClockSingle()
        {
            if (--dmc_cycles <= 0)
            {
                dmc_cycles = DMCFrequencyTable[this.apu.SystemIndex][dmc_freqTimer];
                if (dmc_dmaEnabled)
                {
                    if ((dmc_dmaByte & 0x01) != 0)
                    {
                        if (Output <= 0x7D)
                            Output += 2;
                    }
                    else
                    {
                        if (Output >= 0x02)
                            Output -= 2;
                    }
                    dmc_dmaByte >>= 1;
                }
                dmc_dmaBits--;
                if (dmc_dmaBits == 0)
                {
                    dmc_dmaBits = 8;
                    if (dmc_bufferFull)
                    {
                        dmc_bufferFull = false;
                        dmc_dmaEnabled = true;
                        dmc_dmaByte = dmc_dmaBuffer;
                        // RDY ?
                        if (dmc_dmaSize > 0)
                        {
                            this.dma.AssertDMCDMA();
                        }
                    }
                    else
                    {
                        dmc_dmaEnabled = false;
                    }
                }
            }
        }

		internal void SaveState(System.IO.BinaryWriter bin)
		{
			bin.Write(DeltaIrqOccur);
			bin.Write(DMCIrqEnabled);
			bin.Write(dmc_dmaLooping);
			bin.Write(dmc_dmaEnabled);
			bin.Write(dmc_bufferFull);
			bin.Write(dmc_dmaAddrRefresh);
			bin.Write(dmc_dmaSizeRefresh);
			bin.Write(dmc_dmaSize);
			bin.Write(dmc_dmaBits);
			bin.Write(dmc_dmaByte);
			bin.Write(dmc_dmaAddr);
			bin.Write(dmc_dmaBuffer);
			bin.Write(Output);
			bin.Write(dmc_cycles);
			bin.Write(dmc_freqTimer);
		}

		internal void LoadState(System.IO.BinaryReader bin)
		{
			DeltaIrqOccur = bin.ReadBoolean();
			DMCIrqEnabled = bin.ReadBoolean();
			dmc_dmaLooping = bin.ReadBoolean();
			dmc_dmaEnabled = bin.ReadBoolean();
			dmc_bufferFull = bin.ReadBoolean();
			dmc_dmaAddrRefresh = bin.ReadInt32();
			dmc_dmaSizeRefresh = bin.ReadInt32();
			dmc_dmaSize = bin.ReadInt32();
			dmc_dmaBits = bin.ReadInt32();
			dmc_dmaByte = bin.ReadByte();
			dmc_dmaAddr = bin.ReadInt32();
			dmc_dmaBuffer = bin.ReadByte();
			Output = bin.ReadByte();
			dmc_cycles = bin.ReadInt32();
			dmc_freqTimer = bin.ReadInt32();
		}
	}
}
