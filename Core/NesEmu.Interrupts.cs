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
    public partial class NesEmu
    {
        private const int IRQ_APU = 0x1;
        public const int IRQ_BOARD = 0x2;
        private const int IRQ_DMC = 0x4;
        // Represents the current NMI pin (connected to ppu)
        private static bool NMI_Current;
        // Represents the old status if NMI pin, used to generate NMI in raising edge
        private static bool NMI_Old;
        // Determines that NMI is pending (active when NMI pin become true and was false)
        private static bool NMI_Detected;
        // Determines that IRQ flags (pins)
        public static int IRQFlags = 0;
        // Determines that IRQ is pending
        private static bool IRQ_Detected;
        // This is the interrupt vector to jump in the last 2 cycles of BRK/IRQ/NMI
        private static int interrupt_vector;
        // This flag suspend interrupt polling
        private static bool interrupt_suspend;
        private static bool nmi_enabled;
        private static bool nmi_old;
        private static bool vbl_flag;
        private static bool vbl_flag_temp;

        private static void PollInterruptStatus()
        {
            if (!interrupt_suspend)
            {
                // The edge detector, see if nmi occurred. 
                if (NMI_Current & !NMI_Old) // Raising edge, set nmi request
                    NMI_Detected = true;
                NMI_Old = NMI_Current = false;// NMI detected or not, low both lines for this form ___|-|__
                // irq level detector
                IRQ_Detected = (!registers.i && IRQFlags != 0);
                // Update interrupt vector !
                interrupt_vector = NMI_Detected ? 0xFFFA : 0xFFFE;
            }
        }
        private static void CheckNMI()
        {
            // At VBL time
            if ((VClock == vbl_vclock_Start) && (HClock < 3))
            {
                NMI_Current = (vbl_flag_temp & nmi_enabled);
                // normally, ppu question for nmi at first 3 clocks of vblank
            }
        }
    }
}

