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
using System.Text;
using System.IO;
using System.Drawing;

namespace MyNes.Core
{
    /*State section*/
    public partial class NesEmu
    {
        private const byte state_version = 6;// The state version.
        private static bool state_is_saving_state;
        private static bool state_is_loading_state;

        public static void UpdateStateSlot(int slot)
        {
            // Reset state
            STATESlot = slot;
            // Make STATE file name
            STATEFileName = Path.Combine(STATEFolder, Path.GetFileNameWithoutExtension(GAMEFILE) + "_" + STATESlot + "_.mns");
        }
        /// <summary>
        /// Request a state save at specified slot.
        /// </summary>
        public static void SaveState()
        {
            request_pauseAtFrameFinish = true;
            request_state_save = true;
        }
        /// <summary>
        /// Request a state load at specified slot.
        /// </summary>
        public static void LoadState()
        {
            request_pauseAtFrameFinish = true;
            request_state_load = true;
        }
        /// <summary>
        /// Save current game state as
        /// </summary>
        /// <param name="fileName">The complete path where to save the file</param>
        public static void SaveStateAs(string fileName)
        {
            if (state_is_loading_state)
            {
                EmulationPaused = false;
                videoOut.WriteNotification("Can't save state while loading a state !", 120, Color.Red);
                return;
            } 
            if (state_is_saving_state)
            {
                EmulationPaused = false;
                videoOut.WriteNotification("Already saving state !!", 120, Color.Red);
                return;
            }
            state_is_saving_state = true;
            // Create the stream
            Stream stream = new MemoryStream();
            BinaryWriter bin = new BinaryWriter(stream);
            // Write header
            bin.Write(Encoding.ASCII.GetBytes("MNS"));// Write MNS (My Nes State)
            bin.Write(state_version);// Write version (1 byte)
            // Write SHA1 for compare later
            for (int i = 0; i < board.RomSHA1.Length; i += 2)
            {
                string v = board.RomSHA1.Substring(i, 2).ToUpper();
                bin.Write(System.Convert.ToByte(v, 16));
            }
            // Write data
            #region General
            bin.Write(palCyc);
            #endregion
            #region APU
            bin.Write(Cycles);
            bin.Write(SequencingMode);
            bin.Write(CurrentSeq);
            bin.Write(isClockingDuration);
            bin.Write(FrameIrqEnabled);
            bin.Write(FrameIrqFlag);
            bin.Write(oddCycle);
            #endregion
            #region CPU
            bin.Write(registers.a);
            bin.Write(registers.c);
            bin.Write(registers.d);
            bin.Write(registers.eah);
            bin.Write(registers.eal);
            bin.Write(registers.i);
            bin.Write(registers.n);
            bin.Write(registers.pch);
            bin.Write(registers.pcl);
            bin.Write(registers.sph);
            bin.Write(registers.spl);
            bin.Write(registers.v);
            bin.Write(registers.x);
            bin.Write(registers.y);
            bin.Write(registers.z);
            bin.Write(M);
            bin.Write(opcode);
            bin.Write(byte_temp);
            bin.Write(int_temp);
            bin.Write(int_temp1);
            bin.Write(dummy);
            #endregion
            #region DMA
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
            #endregion
            #region DMC
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
            bin.Write(dmc_output);
            bin.Write(dmc_cycles);
            bin.Write(dmc_freqTimer);
            #endregion
            #region Input
            bin.Write(PORT0);
            bin.Write(PORT1);
            bin.Write(inputStrobe);
            #endregion
            #region Interrupts
            bin.Write(NMI_Current);
            bin.Write(NMI_Old);
            bin.Write(NMI_Detected);
            bin.Write(IRQFlags);
            bin.Write(IRQ_Detected);
            bin.Write(interrupt_vector);
            bin.Write(interrupt_suspend);
            bin.Write(nmi_enabled);
            bin.Write(nmi_old);
            bin.Write(vbl_flag);
            bin.Write(vbl_flag_temp);
            #endregion
            #region Memory
            board.SaveState(bin);
            bin.Write(WRAM);
            bin.Write(palettes_bank);
            bin.Write(oam_ram);
            bin.Write(oam_secondary);
            bin.Write(BUS_ADDRESS);
            bin.Write(BUS_RW);
            bin.Write(BUS_RW_P);
            bin.Write(temp_4015);
            bin.Write(temp_4016);
            bin.Write(temp_4017);
            #endregion
            #region Noise
            bin.Write(noz_envelope);
            bin.Write(noz_env_startflag);
            bin.Write(noz_env_counter);
            bin.Write(noz_env_devider);
            bin.Write(noz_length_counter_halt_flag);
            bin.Write(noz_constant_volume_flag);
            bin.Write(noz_volume_decay_time);
            bin.Write(noz_duration_haltRequset);
            bin.Write(noz_duration_counter);
            bin.Write(noz_duration_reloadEnabled);
            bin.Write(noz_duration_reload);
            bin.Write(noz_duration_reloadRequst);
            bin.Write(noz_modeFlag);
            bin.Write(noz_shiftRegister);
            bin.Write(noz_feedback);
            bin.Write(noz_freqTimer);
            bin.Write(noz_cycles);
            #endregion
            #region PPU
            bin.Write(VClock);
            bin.Write(HClock);
            bin.Write(oddSwap);
            bin.Write(current_pixel);
            bin.Write(temp);
            bin.Write(temp_comparator);
            bin.Write(bkg_pos);
            bin.Write(spr_pos);
            bin.Write(object0);
            bin.Write(infront);
            bin.Write(bkgPixel);
            bin.Write(sprPixel);
            bin.Write(bkg_fetch_address);
            bin.Write(bkg_fetch_nametable);
            bin.Write(bkg_fetch_attr);
            bin.Write(bkg_fetch_bit0);
            bin.Write(bkg_fetch_bit1);
            bin.Write(spr_fetch_address);
            bin.Write(spr_fetch_bit0);
            bin.Write(spr_fetch_bit1);
            bin.Write(spr_fetch_attr);
            for (int i = 0; i < spr_zero_buffer.Length; i++)
                bin.Write(spr_zero_buffer[i]);
            bin.Write(vram_temp);
            bin.Write(vram_address);
            bin.Write(vram_address_temp_access);
            bin.Write(vram_address_temp_access1);
            bin.Write(vram_increament);
            bin.Write(vram_flipflop);
            bin.Write(vram_fine);
            bin.Write(reg2007buffer);
            bin.Write(bkg_enabled);
            bin.Write(bkg_clipped);
            bin.Write(bkg_patternAddress);
            bin.Write(spr_enabled);
            bin.Write(spr_clipped);
            bin.Write(spr_patternAddress);
            bin.Write(spr_size16);
            bin.Write(spr_0Hit);
            bin.Write(spr_overflow);
            bin.Write(grayscale);
            bin.Write(emphasis);
            bin.Write(ppu_2002_temp);
            bin.Write(ppu_2004_temp);
            bin.Write(ppu_2007_temp);
            bin.Write(oam_address);
            bin.Write(oam_fetch_data);
            bin.Write(oam_evaluate_slot);
            bin.Write(oam_evaluate_count);
            bin.Write(oam_fetch_mode);
            bin.Write(oam_phase_index);
            bin.Write(spr_render_i);
            bin.Write(bkg_render_i);
            bin.Write(spr_evaluation_i);
            bin.Write(spr_render_temp_pixel);
            #endregion
            #region Pulse 1
            bin.Write(sq1_envelope);
            bin.Write(sq1_env_startflag);
            bin.Write(sq1_env_counter);
            bin.Write(sq1_env_devider);
            bin.Write(sq1_length_counter_halt_flag);
            bin.Write(sq1_constant_volume_flag);
            bin.Write(sq1_volume_decay_time);
            bin.Write(sq1_duration_haltRequset);
            bin.Write(sq1_duration_counter);
            bin.Write(sq1_duration_reloadEnabled);
            bin.Write(sq1_duration_reload);
            bin.Write(sq1_duration_reloadRequst);
            bin.Write(sq1_dutyForm);
            bin.Write(sq1_dutyStep);
            bin.Write(sq1_sweepDeviderPeriod);
            bin.Write(sq1_sweepShiftCount);
            bin.Write(sq1_sweepCounter);
            bin.Write(sq1_sweepEnable);
            bin.Write(sq1_sweepReload);
            bin.Write(sq1_sweepNegateFlag);
            bin.Write(sq1_frequency);
            bin.Write(sq1_output);
            bin.Write(sq1_sweep);
            bin.Write(sq1_cycles);
            #endregion
            #region Pulse 2
            bin.Write(sq2_envelope);
            bin.Write(sq2_env_startflag);
            bin.Write(sq2_env_counter);
            bin.Write(sq2_env_devider);
            bin.Write(sq2_length_counter_halt_flag);
            bin.Write(sq2_constant_volume_flag);
            bin.Write(sq2_volume_decay_time);
            bin.Write(sq2_duration_haltRequset);
            bin.Write(sq2_duration_counter);
            bin.Write(sq2_duration_reloadEnabled);
            bin.Write(sq2_duration_reload);
            bin.Write(sq2_duration_reloadRequst);
            bin.Write(sq2_dutyForm);
            bin.Write(sq2_dutyStep);
            bin.Write(sq2_sweepDeviderPeriod);
            bin.Write(sq2_sweepShiftCount);
            bin.Write(sq2_sweepCounter);
            bin.Write(sq2_sweepEnable);
            bin.Write(sq2_sweepReload);
            bin.Write(sq2_sweepNegateFlag);
            bin.Write(sq2_frequency);
            bin.Write(sq2_output);
            bin.Write(sq2_sweep);
            bin.Write(sq2_cycles);
            #endregion
            #region Triangle
            bin.Write(trl_length_counter_halt_flag);
            bin.Write(trl_duration_haltRequset);
            bin.Write(trl_duration_counter);
            bin.Write(trl_duration_reloadEnabled);
            bin.Write(trl_duration_reload);
            bin.Write(trl_duration_reloadRequst);
            bin.Write(trl_linearCounter);
            bin.Write(trl_linearCounterReload);
            bin.Write(trl_step);
            bin.Write(trl_linearCounterHalt);
            bin.Write(trl_halt);
            bin.Write(trl_frequency);
            bin.Write(trl_output);
            bin.Write(trl_cycles);
            #endregion

            // Compress data !
            byte[] outData = new byte[0];
            ZlipWrapper.CompressData(((MemoryStream)bin.BaseStream).GetBuffer(), out outData);
            // Write file !
            Stream fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write);
            fileStream.Write(outData, 0, outData.Length);
            // Save snapshot
            videoOut.TakeSnapshot(STATEFolder, Path.GetFileNameWithoutExtension(fileName), ".jpg", true);

            // Finished !
            bin.Flush();
            bin.Close();
            fileStream.Flush();
            fileStream.Close();
            state_is_saving_state = false;
            EmulationPaused = false;
            videoOut.WriteNotification("State saved at slot " + STATESlot, 120, Color.Green);
        }
        /// <summary>
        /// Load current game state from file
        /// </summary>
        /// <param name="fileName">The complete path to the state file</param>
        public static void LoadStateAs(string fileName)
        {
            if (state_is_saving_state)
            {
                EmulationPaused = false;
                videoOut.WriteNotification("Can't load state while it's saving state !", 120, Color.Red);
                return;
            } 
            if (state_is_loading_state)
            {
                EmulationPaused = false;
                videoOut.WriteNotification("Already loading a state !", 120, Color.Red);
                return;
            } 
            state_is_loading_state = true;
            // Read the file
            Stream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            // Decompress
            byte[] inData = new byte[stream.Length];
            byte[] outData = new byte[0];
            stream.Read(inData, 0, inData.Length);
            stream.Close();
            ZlipWrapper.DecompressData(inData, out outData);

            // Create the reader
            BinaryReader bin = new BinaryReader(new MemoryStream(outData));
            // Read header
            byte[] header = new byte[3];
            bin.Read(header, 0, header.Length);
            if (Encoding.ASCII.GetString(header) != "MNS")
            {
                EmulationPaused = false;
                videoOut.WriteNotification("Unable load state at slot " + STATESlot + "; Not My Nes State File !", 120, Color.Red);
                state_is_loading_state = false; 
                return;
            }
            // Read version
            if (bin.ReadByte() != state_version)
            {
                EmulationPaused = false;
                videoOut.WriteNotification("Unable load state at slot " + STATESlot + "; Not compatible state file version !", 120, Color.Red);
                state_is_loading_state = false; 
                return;
            }
            string sha1 = "";
            for (int i = 0; i < board.RomSHA1.Length; i += 2)
            {
                sha1 += (bin.ReadByte()).ToString("X2");
            }
            if (sha1.ToLower() != board.RomSHA1.ToLower())
            {
                EmulationPaused = false;
                videoOut.WriteNotification("Unable load state at slot " + STATESlot + "; This state file is not for this game; not same SHA1 !", 120, Color.Red);
                state_is_loading_state = false; 
                return;
            }
            // Read data
            #region General
            palCyc = bin.ReadByte();
            #endregion
            #region APU
            Cycles = bin.ReadInt32();
            SequencingMode = bin.ReadBoolean();
            CurrentSeq = bin.ReadByte();
            isClockingDuration = bin.ReadBoolean();
            FrameIrqEnabled = bin.ReadBoolean();
            FrameIrqFlag = bin.ReadBoolean();
            oddCycle = bin.ReadBoolean();
            #endregion
            #region CPU
            registers.a = bin.ReadByte();
            registers.c = bin.ReadBoolean();
            registers.d = bin.ReadBoolean();
            registers.eah = bin.ReadByte();
            registers.eal = bin.ReadByte();
            registers.i = bin.ReadBoolean();
            registers.n = bin.ReadBoolean();
            registers.pch = bin.ReadByte();
            registers.pcl = bin.ReadByte();
            registers.sph = bin.ReadByte();
            registers.spl = bin.ReadByte();
            registers.v = bin.ReadBoolean();
            registers.x = bin.ReadByte();
            registers.y = bin.ReadByte();
            registers.z = bin.ReadBoolean();
            M = bin.ReadByte();
            opcode = bin.ReadByte();
            byte_temp = bin.ReadByte();
            int_temp = bin.ReadInt32();
            int_temp1 = bin.ReadInt32();
            dummy = bin.ReadByte();
            #endregion
            #region DMA
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
            #endregion
            #region DMC
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
            dmc_output = bin.ReadByte();
            dmc_cycles = bin.ReadInt32();
            dmc_freqTimer = bin.ReadInt32();
            #endregion
            #region Input
            PORT0 = bin.ReadInt32();
            PORT1 = bin.ReadInt32();
            inputStrobe = bin.ReadInt32();
            #endregion
            #region Interrupts
            NMI_Current = bin.ReadBoolean();
            NMI_Old = bin.ReadBoolean();
            NMI_Detected = bin.ReadBoolean();
            IRQFlags = bin.ReadInt32();
            IRQ_Detected = bin.ReadBoolean();
            interrupt_vector = bin.ReadInt32();
            interrupt_suspend = bin.ReadBoolean();
            nmi_enabled = bin.ReadBoolean();
            nmi_old = bin.ReadBoolean();
            vbl_flag = bin.ReadBoolean();
            vbl_flag_temp = bin.ReadBoolean();
            #endregion
            #region Memory
            board.LoadState(bin);
            bin.Read(WRAM, 0, WRAM.Length);
            bin.Read(palettes_bank, 0, palettes_bank.Length);
            bin.Read(oam_ram, 0, oam_ram.Length);
            bin.Read(oam_secondary, 0, oam_secondary.Length);
            BUS_ADDRESS = bin.ReadInt32();
            BUS_RW = bin.ReadBoolean();
            BUS_RW_P = bin.ReadBoolean();
            temp_4015 = bin.ReadByte();
            temp_4016 = bin.ReadByte();
            temp_4017 = bin.ReadByte();
            #endregion
            #region Noise
            noz_envelope = bin.ReadInt32();
            noz_env_startflag = bin.ReadBoolean();
            noz_env_counter = bin.ReadInt32();
            noz_env_devider = bin.ReadInt32();
            noz_length_counter_halt_flag = bin.ReadBoolean();
            noz_constant_volume_flag = bin.ReadBoolean();
            noz_volume_decay_time = bin.ReadInt32();
            noz_duration_haltRequset = bin.ReadBoolean();
            noz_duration_counter = bin.ReadByte();
            noz_duration_reloadEnabled = bin.ReadBoolean();
            noz_duration_reload = bin.ReadByte();
            noz_duration_reloadRequst = bin.ReadBoolean();
            noz_modeFlag = bin.ReadBoolean();
            noz_shiftRegister = bin.ReadInt32();
            noz_feedback = bin.ReadInt32();
            noz_freqTimer = bin.ReadInt32();
            noz_cycles = bin.ReadInt32();
            #endregion
            #region PPU
            VClock = bin.ReadInt32();
            HClock = bin.ReadInt32();
            oddSwap = bin.ReadBoolean();
            current_pixel = bin.ReadInt32();
            temp = bin.ReadInt32();
            temp_comparator = bin.ReadInt32();
            bkg_pos = bin.ReadInt32();
            spr_pos = bin.ReadInt32();
            object0 = bin.ReadInt32();
            infront = bin.ReadInt32();
            bkgPixel = bin.ReadInt32();
            sprPixel = bin.ReadInt32();
            bkg_fetch_address = bin.ReadInt32();
            bkg_fetch_nametable = bin.ReadByte();
            bkg_fetch_attr = bin.ReadByte();
            bkg_fetch_bit0 = bin.ReadByte();
            bkg_fetch_bit1 = bin.ReadByte();
            spr_fetch_address = bin.ReadInt32();
            spr_fetch_bit0 = bin.ReadByte();
            spr_fetch_bit1 = bin.ReadByte();
            spr_fetch_attr = bin.ReadByte();
            for (int i = 0; i < spr_zero_buffer.Length; i++)
                spr_zero_buffer[i] = bin.ReadBoolean();
            vram_temp = bin.ReadInt32();
            vram_address = bin.ReadInt32();
            vram_address_temp_access = bin.ReadInt32();
            vram_address_temp_access1 = bin.ReadInt32();
            vram_increament = bin.ReadInt32();
            vram_flipflop = bin.ReadBoolean();
            vram_fine = bin.ReadByte();
            reg2007buffer = bin.ReadByte();
            bkg_enabled = bin.ReadBoolean();
            bkg_clipped = bin.ReadBoolean();
            bkg_patternAddress = bin.ReadInt32();
            spr_enabled = bin.ReadBoolean();
            spr_clipped = bin.ReadBoolean();
            spr_patternAddress = bin.ReadInt32();
            spr_size16 = bin.ReadInt32();
            spr_0Hit = bin.ReadBoolean();
            spr_overflow = bin.ReadBoolean();
            grayscale = bin.ReadInt32();
            emphasis = bin.ReadInt32();
            ppu_2002_temp = bin.ReadByte();
            ppu_2004_temp = bin.ReadByte();
            ppu_2007_temp = bin.ReadByte();
            oam_address = bin.ReadByte();
            oam_fetch_data = bin.ReadByte();
            oam_evaluate_slot = bin.ReadByte();
            oam_evaluate_count = bin.ReadByte();
            oam_fetch_mode = bin.ReadBoolean();
            oam_phase_index = bin.ReadByte();
            spr_render_i = bin.ReadInt32();
            bkg_render_i = bin.ReadInt32();
            spr_evaluation_i = bin.ReadInt32();
            spr_render_temp_pixel = bin.ReadInt32();
            #endregion
            #region Pulse 1
            sq1_envelope = bin.ReadInt32();
            sq1_env_startflag = bin.ReadBoolean();
            sq1_env_counter = bin.ReadInt32();
            sq1_env_devider = bin.ReadInt32();
            sq1_length_counter_halt_flag = bin.ReadBoolean();
            sq1_constant_volume_flag = bin.ReadBoolean();
            sq1_volume_decay_time = bin.ReadInt32();
            sq1_duration_haltRequset = bin.ReadBoolean();
            sq1_duration_counter = bin.ReadByte();
            sq1_duration_reloadEnabled = bin.ReadBoolean();
            sq1_duration_reload = bin.ReadByte();
            sq1_duration_reloadRequst = bin.ReadBoolean();
            sq1_dutyForm = bin.ReadInt32();
            sq1_dutyStep = bin.ReadInt32();
            sq1_sweepDeviderPeriod = bin.ReadInt32();
            sq1_sweepShiftCount = bin.ReadInt32();
            sq1_sweepCounter = bin.ReadInt32();
            sq1_sweepEnable = bin.ReadBoolean();
            sq1_sweepReload = bin.ReadBoolean();
            sq1_sweepNegateFlag = bin.ReadBoolean();
            sq1_frequency = bin.ReadInt32();
            sq1_output = bin.ReadByte();
            sq1_sweep = bin.ReadInt32();
            sq1_cycles = bin.ReadInt32();
            #endregion
            #region Pulse 2
            sq2_envelope = bin.ReadInt32();
            sq2_env_startflag = bin.ReadBoolean();
            sq2_env_counter = bin.ReadInt32();
            sq2_env_devider = bin.ReadInt32();
            sq2_length_counter_halt_flag = bin.ReadBoolean();
            sq2_constant_volume_flag = bin.ReadBoolean();
            sq2_volume_decay_time = bin.ReadInt32();
            sq2_duration_haltRequset = bin.ReadBoolean();
            sq2_duration_counter = bin.ReadByte();
            sq2_duration_reloadEnabled = bin.ReadBoolean();
            sq2_duration_reload = bin.ReadByte();
            sq2_duration_reloadRequst = bin.ReadBoolean();
            sq2_dutyForm = bin.ReadInt32();
            sq2_dutyStep = bin.ReadInt32();
            sq2_sweepDeviderPeriod = bin.ReadInt32();
            sq2_sweepShiftCount = bin.ReadInt32();
            sq2_sweepCounter = bin.ReadInt32();
            sq2_sweepEnable = bin.ReadBoolean();
            sq2_sweepReload = bin.ReadBoolean();
            sq2_sweepNegateFlag = bin.ReadBoolean();
            sq2_frequency = bin.ReadInt32();
            sq2_output = bin.ReadByte();
            sq2_sweep = bin.ReadInt32();
            sq2_cycles = bin.ReadInt32();
            #endregion
            #region Triangle
            trl_length_counter_halt_flag = bin.ReadBoolean();
            trl_duration_haltRequset = bin.ReadBoolean();
            trl_duration_counter = bin.ReadByte();
            trl_duration_reloadEnabled = bin.ReadBoolean();
            trl_duration_reload = bin.ReadByte();
            trl_duration_reloadRequst = bin.ReadBoolean();
            trl_linearCounter = bin.ReadByte();
            trl_linearCounterReload = bin.ReadByte();
            trl_step = bin.ReadByte();
            trl_linearCounterHalt = bin.ReadBoolean();
            trl_halt = bin.ReadBoolean();
            trl_frequency = bin.ReadInt32();
            trl_output = bin.ReadByte();
            trl_cycles = bin.ReadInt32();
            #endregion

            // Finished !
            bin.Close();
            EmulationPaused = false;
            state_is_loading_state = false; 
            videoOut.WriteNotification("State loaded from slot " + STATESlot, 120, Color.Green);
        }
    }
}
