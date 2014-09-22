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

/*DMA section*/
namespace MyNes.Core
{
    public class Dma
    {
		public event EventHandler DmcDma;

		public Dma(NesEmu core)
		{
			this.core = core;
		}

        // I suspect the SN74LS373N chip: "OCTAL TRANSPARENT LATCH WITH 3-STATE OUTPUTS; OCTAL D-TYPE FLIP-FLOP
        // WITH 3-STATE OUTPUT"
        // http://html.alldatasheet.com/html-pdf/28021/TI/SN74LS373N/24/1/SN74LS373N.html
        // This chip (somehow, not confirmed yet) is responsible for dma operations inside nes
        // This class emulate the dma behaviors, not as the real chip.

        private static int dmaDMCDMAWaitCycles;
        private static int dmaOAMDMAWaitCycles;
        private static bool isOamDma;
        private static int oamdma_i;
        private static bool dmaDMCOn;
        private static bool dmaOAMOn;
        private static bool dmaDMC_occurring;
        private static bool dmaOAM_occurring;
        private static int dmaOAMFinishCounter;
        public int dmaOamaddress;
        private static int OAMCYCLE;
        private static byte latch;
		private NesEmu core;

        public void DMAHardReset()
        {
            dmaDMCDMAWaitCycles = 0;
            dmaOAMDMAWaitCycles = 0;
            isOamDma = false;
            oamdma_i = 0;
            dmaDMCOn = false;
            dmaOAMOn = false;
            dmaDMC_occurring = false;
            dmaOAM_occurring = false;
            dmaOAMFinishCounter = 0;
            dmaOamaddress = 0;
            OAMCYCLE = 0;
            latch = 0;
        }
        public void DMASoftReset()
        {
            dmaDMCDMAWaitCycles = 0;
            dmaOAMDMAWaitCycles = 0;
            isOamDma = false;
            oamdma_i = 0;
            dmaDMCOn = false;
            dmaOAMOn = false;
            dmaDMC_occurring = false;
            dmaOAM_occurring = false;
            dmaOAMFinishCounter = 0;
            dmaOamaddress = 0;
            OAMCYCLE = 0;
            latch = 0;
        }
        public void AssertDMCDMA()
        {
            isOamDma = false;
            if (dmaOAM_occurring)
            {

                if (OAMCYCLE < 508)
                    // OAM DMA is occurring here, then use the oam logic for waiting cycles
                    // which depends on apu's odd toggle
                    dmaDMCDMAWaitCycles = this.core.oddCycle ? 0 : 1;
                else
                {
                    // Here the oam dma is about to finish
                    // Remaining cycles of oam dma determines the dmc waiting cycles.
                    dmaDMCDMAWaitCycles = 4 - (512 - OAMCYCLE);
                }
            }
            else if (dmaDMC_occurring)
            {
                // DMC occurring now !!? is that possible ?
                // Anyway, let's ignore this call !
                return;
            }
            else
            {
                // Nothing occurring, initialize brand new dma
                // DMC DMA depends on r/w flag for the wait cycles.
                dmaDMCDMAWaitCycles = this.core.BUS_RW ? 3 : 2;
                // After 2 cycles of oam dma, add extra cycle for the waiting.
                if (dmaOAMFinishCounter == 3)
                    dmaDMCDMAWaitCycles++;
            }
            dmaDMCOn = true;
        }
        public void AssertOAMDMA()
        {
            isOamDma = true;
            // Setup
            // OAM DMA depends on apu odd timer for odd cycles
            if (dmaDMC_occurring)
            {
                // DMC DMA occurring here, use r/w flag
                dmaOAMDMAWaitCycles = this.core.BUS_RW ? 1 : 0;
            }
            else if (dmaOAM_occurring)
            {
                // OAM DMA inside OAM DMA !?? is that possible ?
                // Ignore !
                return;
            }
            else
            {
                // OAM DMA depends on the apu odd timer to add the waiting cycles
                dmaOAMDMAWaitCycles = this.core.oddCycle ? 1 : 2;
            }
            dmaOAMOn = true;
            dmaOAMFinishCounter = 0;
        }
        public void DMAClock()
        {
            if (dmaOAMFinishCounter > 0)
                dmaOAMFinishCounter--;
            if (!this.core.BUS_RW)// Clocks only on reads
            {
                if (dmaDMCDMAWaitCycles > 0)
                    dmaDMCDMAWaitCycles--;
                if (dmaOAMDMAWaitCycles > 0)
                    dmaOAMDMAWaitCycles--;
                return;
            }
            if (dmaDMCOn)
            {
                if (this.core.BUS_RW)// Clocks only on reads
                {
                    dmaDMC_occurring = true;
                    // This is it ! pause the cpu
                    dmaDMCOn = false;
                    // Do wait cycles (extra reads)
                    if (dmaDMCDMAWaitCycles > 0)
                    {
                        if (this.core.BUS_ADDRESS == 0x4016 || this.core.BUS_ADDRESS == 0x4017)
                        {
                            this.core.Read(this.core.BUS_ADDRESS);
                            dmaDMCDMAWaitCycles--;

                            while (dmaDMCDMAWaitCycles > 0)
                            {
                                this.core.ClockComponents();
                                dmaDMCDMAWaitCycles--;
                            }
                        }
                        else
                        {
                            while (dmaDMCDMAWaitCycles > 0)
                            {
                                this.core.Read(this.core.BUS_ADDRESS);
                                dmaDMCDMAWaitCycles--;
                            }
                        }
                    }
                    // Do DMC DMA
					this.FireDmcDma();

                    dmaDMC_occurring = false;
                }
            }
            if (dmaOAMOn)
            {
                if (this.core.BUS_RW)// Clocks only on reads
                {
                    dmaOAM_occurring = true;
                    // This is it ! pause the cpu
                    dmaOAMOn = false;
                    // Do wait cycles (extra reads)
                    if (dmaOAMDMAWaitCycles > 0)
                    {
                        if (this.core.BUS_ADDRESS == 0x4016 || this.core.BUS_ADDRESS == 0x4017)
                        {
							this.core.Read(this.core.BUS_ADDRESS);
                            dmaOAMDMAWaitCycles--;

                            while (dmaOAMDMAWaitCycles > 0)
                            {
								this.core.ClockComponents();
                                dmaOAMDMAWaitCycles--;
                            }
                        }
                        else
                        {
                            while (dmaOAMDMAWaitCycles > 0)
                            {
								this.core.Read(this.core.BUS_ADDRESS);
                                dmaOAMDMAWaitCycles--;
                            }
                        }
                    }

                    // Do OAM DMA
                    OAMCYCLE = 0;
                    for (oamdma_i = 0; oamdma_i < 256; oamdma_i++)
                    {
						latch = this.core.Read(dmaOamaddress);
                        OAMCYCLE++;
						this.core.Write(0x2004, latch);
                        OAMCYCLE++;
                        dmaOamaddress = (++dmaOamaddress) & 0xFFFF;
                    }
                    OAMCYCLE = 0;
                    dmaOAMFinishCounter = 5;
                    dmaOAM_occurring = false;
                }
            }
        }

		private void FireDmcDma()
		{
			var handler = this.DmcDma;

			if (handler != null)
			{
				handler(this, null);
			}
		}

		internal void SaveState(BinaryWriter bin)
		{
			bin.Write(dmaDMCDMAWaitCycles);
			bin.Write(dmaOAMDMAWaitCycles);
			bin.Write(isOamDma);
			bin.Write(oamdma_i);
			bin.Write(dmaDMCOn);
			bin.Write(dmaOAMOn);
			bin.Write(dmaDMC_occurring);
			bin.Write(dmaOAM_occurring);
			bin.Write(dmaOAMFinishCounter);
			bin.Write(dmaOamaddress);
			bin.Write(OAMCYCLE);
			bin.Write(latch);
		}

		internal void LoadState(BinaryReader bin)
		{
			dmaDMCDMAWaitCycles = bin.ReadInt32();
			dmaOAMDMAWaitCycles = bin.ReadInt32();
			isOamDma = bin.ReadBoolean();
			oamdma_i = bin.ReadInt32();
			dmaDMCOn = bin.ReadBoolean();
			dmaOAMOn = bin.ReadBoolean();
			dmaDMC_occurring = bin.ReadBoolean();
			dmaOAM_occurring = bin.ReadBoolean();
			dmaOAMFinishCounter = bin.ReadInt32();
			dmaOamaddress = bin.ReadInt32();
			OAMCYCLE = bin.ReadInt32();
			latch = bin.ReadByte();
		}
	}
}

