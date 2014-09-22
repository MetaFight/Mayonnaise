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
/*Noise sound channel*/
namespace MyNes.Core
{
    public partial class NesEmu
    {
        private static int[][] NozFrequencyTable = 
        { 
            new int [] //NTSC
            {  
                4, 8, 16, 32, 64, 96, 128, 160, 202, 254, 380, 508, 762, 1016, 2034, 4068
            },
            new int [] //PAL
            {  
                4, 8, 14, 30, 60, 88, 118, 148, 188, 236, 354, 472, 708,  944, 1890, 3778
            },
            new int [] //DENDY (same as ntsc for now)
            {  
                4, 8, 16, 32, 64, 96, 128, 160, 202, 254, 380, 508, 762, 1016, 2034, 4068
            }
        };
        private static int noz_envelope;
        private static bool noz_env_startflag;
        private static int noz_env_counter;
        private static int noz_env_devider;
        private static bool noz_length_counter_halt_flag;
        private static bool noz_constant_volume_flag;
        private static int noz_volume_decay_time;
        private static bool noz_duration_haltRequset = false;
        private static byte noz_duration_counter;
        private static bool noz_duration_reloadEnabled;
        private static byte noz_duration_reload = 0;
        private static bool noz_duration_reloadRequst = false;
        private static bool noz_modeFlag = false;
        private static int noz_shiftRegister = 1;
        private static int noz_feedback;
        private static int noz_freqTimer;
        private static int noz_cycles;
        private static int noz_output;

        private static void NOZShutdown()
        {

        }
        private static void NozHardReset()
        {
            noz_envelope = 0;
            noz_env_startflag = false;
            noz_env_counter = 0;
            noz_env_devider = 0;
            noz_length_counter_halt_flag = false;
            noz_constant_volume_flag = false;
            noz_volume_decay_time = 0;
            noz_duration_haltRequset = false;
            noz_duration_counter = 0;
            noz_duration_reloadEnabled = false;
            noz_duration_reload = 0;
            noz_duration_reloadRequst = false;
            noz_modeFlag = false;
            noz_shiftRegister = 1;
            noz_cycles = 0;
            noz_freqTimer = 0;
            noz_feedback = 0;
        }
        private static void NozClockEnvelope()
        {
            if (noz_env_startflag)
            {
                noz_env_startflag = false;
                noz_env_counter = 0xF;
                noz_env_devider = noz_volume_decay_time + 1;
            }
            else
            {
                if (noz_env_devider > 0)
                    noz_env_devider--;
                else
                {
                    noz_env_devider = noz_volume_decay_time + 1;
                    if (noz_env_counter > 0)
                        noz_env_counter--;
                    else if (noz_length_counter_halt_flag)
                        noz_env_counter = 0xF;
                }
            }
            noz_envelope = noz_constant_volume_flag ? noz_volume_decay_time : noz_env_counter;
        }
        private static void NozClockLengthCounter()
        {
            if (!noz_length_counter_halt_flag)
            {
                if (noz_duration_counter > 0)
                {
                    noz_duration_counter--;
                }
            }
        }
        private static void NozClockSingle()
        {
            noz_length_counter_halt_flag = noz_duration_haltRequset;
            if (isClockingDuration && noz_duration_counter > 0)
                noz_duration_reloadRequst = false;
            if (noz_duration_reloadRequst)
            {
                if (noz_duration_reloadEnabled)
                    noz_duration_counter = noz_duration_reload;
                noz_duration_reloadRequst = false;
            }

            if (--noz_cycles <= 0)
            {
                noz_cycles = NozFrequencyTable[systemIndex][noz_freqTimer];
                if (noz_modeFlag)
                    noz_feedback = (noz_shiftRegister >> 6 & 0x1) ^ (noz_shiftRegister & 0x1);
                else
                    noz_feedback = (noz_shiftRegister >> 1 & 0x1) ^ (noz_shiftRegister & 0x1);
                noz_shiftRegister >>= 1;
                noz_shiftRegister = ((noz_shiftRegister & 0x3FFF) | (noz_feedback << 14));

                if (noz_duration_counter > 0 && (noz_shiftRegister & 1) == 0)
                    noz_output = noz_envelope;
                else
                    noz_output = 0;
            }
        }
    }
}
