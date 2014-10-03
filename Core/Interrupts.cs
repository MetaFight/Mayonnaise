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

/*Interrupts section*/
namespace MyNes.Core
{
	public class Interrupts
    {
        public const int IRQ_APU = 0x1;
        public const int IRQ_BOARD = 0x2;
        public const int IRQ_DMC = 0x4;

		public Interrupts(Ppu ppu)
		{
			this.ppu = ppu;
		}

        // Represents the current NMI pin (connected to ppu)
        public bool NMI_Current;
        // Represents the old status if NMI pin, used to generate NMI in raising edge
        public bool NMI_Old;
        // Determines that NMI is pending (active when NMI pin become true and was false)
        public bool NMI_Detected;
		[Obsolete("Unstaticify")]
        // Determines that IRQ flags (pins)
        public static int IRQFlags = 0;
        // Determines that IRQ is pending
        public bool IRQ_Detected;
        // This is the interrupt vector to jump in the last 2 cycles of BRK/IRQ/NMI
        public int interrupt_vector;
        // This flag suspend interrupt polling
        public bool interrupt_suspend;
        public bool nmi_enabled;
        public bool nmi_old;
        public bool vbl_flag;
        public bool vbl_flag_temp;
		[Obsolete("Mega-hack until I can figure out how the CPU and Interrupts code interact.")]
		public Cpu cpu;
		private readonly Ppu ppu;

        public void PollInterruptStatus()
        {
            if (!interrupt_suspend)
            {
                // The edge detector, see if nmi occurred. 
                if (NMI_Current & !NMI_Old) // Raising edge, set nmi request
                    NMI_Detected = true;
                NMI_Old = NMI_Current = false;// NMI detected or not, low both lines for this form ___|-|__
                // irq level detector
				IRQ_Detected = (!this.cpu.registers.i && IRQFlags != 0);
                // Update interrupt vector !
                interrupt_vector = NMI_Detected ? 0xFFFA : 0xFFFE;
            }
        }
        public void CheckNMI()
        {
            // At VBL time
			if ((this.ppu.VClock == this.ppu.vbl_vclock_Start) && (this.ppu.HClock < 3))
            {
                NMI_Current = (vbl_flag_temp & nmi_enabled);
                // normally, ppu question for nmi at first 3 clocks of vblank
            }
        }

		internal void SaveState(System.IO.BinaryWriter bin)
		{
			bin.Write(NMI_Current);
			bin.Write(NMI_Old);
			bin.Write(NMI_Detected);
			bin.Write(Interrupts.IRQFlags);
			bin.Write(IRQ_Detected);
			bin.Write(interrupt_vector);
			bin.Write(interrupt_suspend);
			bin.Write(nmi_enabled);
			bin.Write(nmi_old);
			bin.Write(vbl_flag);
			bin.Write(vbl_flag_temp);
		}

		internal void LoadState(System.IO.BinaryReader bin)
		{
			NMI_Current = bin.ReadBoolean();
			NMI_Old = bin.ReadBoolean();
			NMI_Detected = bin.ReadBoolean();
			Interrupts.IRQFlags = bin.ReadInt32();
			IRQ_Detected = bin.ReadBoolean();
			interrupt_vector = bin.ReadInt32();
			interrupt_suspend = bin.ReadBoolean();
			nmi_enabled = bin.ReadBoolean();
			nmi_old = bin.ReadBoolean();
			vbl_flag = bin.ReadBoolean();
			vbl_flag_temp = bin.ReadBoolean();
		}
	}
}

