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
/*Pulse 2 sound channel*/
namespace MyNes.Core
{
    public partial class NesEmu
    {
        /*Envelope and length counter*/
        private static int sq2_envelope;
        private static bool sq2_env_startflag;
        private static int sq2_env_counter;
        private static int sq2_env_devider;
        private static bool sq2_length_counter_halt_flag;
        private static bool sq2_constant_volume_flag;
        private static int sq2_volume_decay_time;
        private static bool sq2_duration_haltRequset = false;
        private static byte sq2_duration_counter;
        private static bool sq2_duration_reloadEnabled;
        private static byte sq2_duration_reload = 0;
        private static bool sq2_duration_reloadRequst = false;

        private static int sq2_dutyForm;
        private static int sq2_dutyStep;
        private static int sq2_sweepDeviderPeriod = 0;
        private static int sq2_sweepShiftCount = 0;
        private static int sq2_sweepCounter = 0;
        private static bool sq2_sweepEnable = false;
        private static bool sq2_sweepReload = false;
        private static bool sq2_sweepNegateFlag = false;
        private static int sq2_frequency;
        private static byte sq2_output;
        private static int sq2_sweep;
        private static int sq2_cycles;

        private static void Sq2Shutdown()
        {

        }
        private static void Sq2HardReset()
        {
            sq2_envelope = 0;
            sq2_env_startflag = false;
            sq2_env_counter = 0;
            sq2_env_devider = 0;
            sq2_length_counter_halt_flag = false;
            sq2_constant_volume_flag = false;
            sq2_volume_decay_time = 0;
            sq2_duration_haltRequset = false;
            sq2_duration_counter = 0;
            sq2_duration_reloadEnabled = false;
            sq2_duration_reload = 0;
            sq2_duration_reloadRequst = false;
            sq2_dutyForm = 0;
            sq2_dutyStep = 0;
            sq2_sweepDeviderPeriod = 0;
            sq2_sweepShiftCount = 0;
            sq2_sweepCounter = 0;
            sq2_output = 0;
            sq2_sweepEnable = false;
            sq2_sweepReload = false;
            sq2_sweepNegateFlag = false;
            sq2_cycles = 0;
            sq2_frequency = 0;
            sq2_sweep = 0;
        }
        private static bool Sq2IsValidFrequency()
        {
            return (sq2_frequency >= 0x8) && ((sq2_sweepNegateFlag) ||
                 (((sq2_frequency + (sq2_frequency >> sq2_sweepShiftCount)) & 0x800) == 0));
        }
        private static void Sq2ClockEnvelope()
        {
            if (sq2_env_startflag)
            {
                sq2_env_startflag = false;
                sq2_env_counter = 0xF;
                sq2_env_devider = sq2_volume_decay_time + 1;
            }
            else
            {
                if (sq2_env_devider > 0)
                    sq2_env_devider--;
                else
                {
                    sq2_env_devider = sq2_volume_decay_time + 1;
                    if (sq2_env_counter > 0)
                        sq2_env_counter--;
                    else if (sq2_length_counter_halt_flag)
                        sq2_env_counter = 0xF;
                }
            }
            sq2_envelope = sq2_constant_volume_flag ? sq2_volume_decay_time : sq2_env_counter;
        }
        private static void Sq2ClockLengthCounter()
        {
            if (!sq2_length_counter_halt_flag)
            {
                if (sq2_duration_counter > 0)
                {
                    sq2_duration_counter--;
                }
            }

            sq2_sweepCounter--;
            if (sq2_sweepCounter == 0)
            {
                sq2_sweepCounter = sq2_sweepDeviderPeriod + 1;
                if (sq2_sweepEnable && (sq2_sweepShiftCount > 0) && Sq2IsValidFrequency())
                {
                    sq2_sweep = sq2_frequency >> sq2_sweepShiftCount;
                    sq2_frequency += sq2_sweepNegateFlag ? -sq2_sweep : sq2_sweep;
                }
            }
            if (sq2_sweepReload)
            {
                sq2_sweepReload = false;
                sq2_sweepCounter = sq2_sweepDeviderPeriod + 1;
            }
        }
        private static void Sq2ClockSingle()
        {
            sq2_length_counter_halt_flag = sq2_duration_haltRequset;
            if (isClockingDuration && sq2_duration_counter > 0)
                sq2_duration_reloadRequst = false;
            if (sq2_duration_reloadRequst)
            {
                if (sq2_duration_reloadEnabled)
                    sq2_duration_counter = sq2_duration_reload;
                sq2_duration_reloadRequst = false;
            }

            if (sq2_cycles > 0)
                sq2_cycles--;
            else
            {
                sq2_cycles = (sq2_frequency << 1) + 2;

                sq2_dutyStep--;
                if (sq2_dutyStep < 0)
                    sq2_dutyStep = 0x7;
                if (sq2_duration_counter > 0 && Sq2IsValidFrequency())
                    sq2_output = (byte)(PulseDutyForms[sq2_dutyForm][sq2_dutyStep] * sq2_envelope);
                else
                    sq2_output = 0;
            }
        }
    }
}
