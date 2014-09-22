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
namespace MyNes.Core
{
    public partial class NesEmu
    {
        private static bool trl_length_counter_halt_flag;
        private static bool trl_duration_haltRequset = false;
        private static byte trl_duration_counter;
        private static bool trl_duration_reloadEnabled;
        private static byte trl_duration_reload = 0;
        private static bool trl_duration_reloadRequst = false;
        private static byte trl_linearCounter = 0;
        private static byte trl_linearCounterReload;
        private static byte trl_step;
        private static bool trl_linearCounterHalt;
        private static bool trl_halt;
        private static int trl_frequency;
        private static byte trl_output;
        private static int trl_cycles;

        private static void TrlShutdown()
        {

        }
        private static void TrlHardReset()
        {
            trl_length_counter_halt_flag = false;
            trl_duration_haltRequset = false;
            trl_duration_counter = 0;
            trl_duration_reloadEnabled = false;
            trl_duration_reload = 0;
            trl_duration_reloadRequst = false;
            trl_linearCounter = 0;
            trl_linearCounterReload = 0;
            trl_output = 0;
            trl_step = 0;
            trl_linearCounterHalt = false;
            trl_halt = true;
            trl_frequency = 0;
            trl_cycles = 0;
        }
        private static void TrlClockEnvelope()
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
        private static void TrlClockLengthCounter()
        {
            if (!trl_length_counter_halt_flag)
            {
                if (trl_duration_counter > 0)
                {
                    trl_duration_counter--;
                }
            }
        }
        private static void TrlClockSingle()
        {
            trl_length_counter_halt_flag = trl_duration_haltRequset;
            if (isClockingDuration && trl_duration_counter > 0)
                trl_duration_reloadRequst = false;
            if (trl_duration_reloadRequst)
            {
                if (trl_duration_reloadEnabled)
                    trl_duration_counter = trl_duration_reload;
                trl_duration_reloadRequst = false;
            }
            if (--trl_cycles <= 0)
            {
                trl_cycles = trl_frequency + 1;
                if (trl_duration_counter > 0 && trl_linearCounter > 0)
                {
                    if (trl_frequency >= 4)
                    {
                        trl_step++;
                        trl_step &= 0x1F;
                        trl_output = TrlStepSequence[trl_step];
                    }
                }
            }
        }
    }
}
