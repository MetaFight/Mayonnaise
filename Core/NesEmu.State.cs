﻿/* This file is part of My Nes
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

        public void UpdateStateSlot(int slot)
        {
            // Reset state
            STATESlot = slot;
            // Make STATE file name
            STATEFileName = Path.Combine(STATEFolder, Path.GetFileNameWithoutExtension(GAMEFILE) + "_" + STATESlot + "_.mns");
        }
        /// <summary>
        /// Request a state save at specified slot.
        /// </summary>
        public void SaveState()
        {
            request_pauseAtFrameFinish = true;
            request_state_save = true;
        }
        /// <summary>
        /// Request a state load at specified slot.
        /// </summary>
        public void LoadState()
        {
            request_pauseAtFrameFinish = true;
            request_state_load = true;
        }
        /// <summary>
        /// Save current game state as
        /// </summary>
        /// <param name="fileName">The complete path where to save the file</param>
        public void SaveStateAs(string fileName)
        {
            if (state_is_loading_state)
            {
                EmulationPaused = false;
				this.ppu.videoOut.WriteNotification("Can't save state while loading a state !", 120, Color.Red);
                return;
            } 
            if (state_is_saving_state)
            {
                EmulationPaused = false;
				this.ppu.videoOut.WriteNotification("Already saving state !!", 120, Color.Red);
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
			for (int i = 0; i < this.memory.board.RomSHA1.Length; i += 2)
            {
				string v = this.memory.board.RomSHA1.Substring(i, 2).ToUpper();
                bin.Write(System.Convert.ToByte(v, 16));
            }
            // Write data
            #region General
            bin.Write(palCyc);
            #endregion
            #region APU
			this.apu.SaveState(bin);
            #endregion
            #region CPU
			this.cpu.SaveState(bin);
            #endregion
            #region DMA
			this.dma.SaveState(bin);
            #endregion
            #region DMC
			this.apu.dmcChannel.SaveState(bin);
            #endregion
            #region Input
            bin.Write(PORT0);
            bin.Write(PORT1);
            bin.Write(inputStrobe);
            #endregion
            #region Interrupts
			this.interrupts.SaveState(bin);
            #endregion
            #region Memory
			this.memory.SaveState(bin);
            #endregion
            #region Noise
			this.apu.noiseChannel.SaveState(bin);
            #endregion
            #region PPU
			this.ppu.SaveState(bin);
            #endregion
            #region Pulse 1
			this.apu.pulse1Channel.SaveState(bin);
            #endregion
            #region Pulse 2
			this.apu.pulse2Channel.SaveState(bin);
            #endregion
            #region Triangle
			this.apu.triangleChannel.SaveState(bin);
            #endregion

            // Compress data !
            byte[] outData = new byte[0];
            ZlipWrapper.CompressData(((MemoryStream)bin.BaseStream).GetBuffer(), out outData);
            // Write file !
            Stream fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write);
            fileStream.Write(outData, 0, outData.Length);
            // Save snapshot
            this.ppu.videoOut.TakeSnapshot(STATEFolder, Path.GetFileNameWithoutExtension(fileName), ".jpg", true);

            // Finished !
            bin.Flush();
            bin.Close();
            fileStream.Flush();
            fileStream.Close();
            state_is_saving_state = false;
            EmulationPaused = false;
            this.ppu.videoOut.WriteNotification("State saved at slot " + STATESlot, 120, Color.Green);
        }
        /// <summary>
        /// Load current game state from file
        /// </summary>
        /// <param name="fileName">The complete path to the state file</param>
        public void LoadStateAs(string fileName)
        {
            if (state_is_saving_state)
            {
                EmulationPaused = false;
				this.ppu.videoOut.WriteNotification("Can't load state while it's saving state !", 120, Color.Red);
                return;
            } 
            if (state_is_loading_state)
            {
                EmulationPaused = false;
				this.ppu.videoOut.WriteNotification("Already loading a state !", 120, Color.Red);
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
				this.ppu.videoOut.WriteNotification("Unable load state at slot " + STATESlot + "; Not My Nes State File !", 120, Color.Red);
                state_is_loading_state = false; 
                return;
            }
            // Read version
            if (bin.ReadByte() != state_version)
            {
                EmulationPaused = false;
				this.ppu.videoOut.WriteNotification("Unable load state at slot " + STATESlot + "; Not compatible state file version !", 120, Color.Red);
                state_is_loading_state = false; 
                return;
            }
            string sha1 = "";
			for (int i = 0; i < this.memory.board.RomSHA1.Length; i += 2)
            {
                sha1 += (bin.ReadByte()).ToString("X2");
            }
			if (sha1.ToLower() != this.memory.board.RomSHA1.ToLower())
            {
                EmulationPaused = false;
				this.ppu.videoOut.WriteNotification("Unable load state at slot " + STATESlot + "; This state file is not for this game; not same SHA1 !", 120, Color.Red);
                state_is_loading_state = false; 
                return;
            }
            // Read data
            #region General
            palCyc = bin.ReadByte();
            #endregion
            #region APU
			this.apu.LoadState(bin);
            #endregion
            #region CPU
			this.cpu.LoadState(bin);
            #endregion
            #region DMA
			this.dma.LoadState(bin);
            #endregion
            #region DMC
			this.apu.dmcChannel.LoadState(bin);
            #endregion
            #region Input
            PORT0 = bin.ReadInt32();
            PORT1 = bin.ReadInt32();
            inputStrobe = bin.ReadInt32();
            #endregion
            #region Interrupts
			this.interrupts.LoadState(bin);
            #endregion
            #region Memory
			this.memory.LoadState(bin);
            #endregion
            #region Noise
			this.apu.noiseChannel.LoadState(bin);
            #endregion
            #region PPU
			this.ppu.LoadState(bin);
            #endregion
            #region Pulse
			this.apu.pulse1Channel.LoadState(bin);
			this.apu.pulse2Channel.LoadState(bin);
            #endregion
            #region Triangle
			this.apu.triangleChannel.LoadState(bin);
            #endregion

            // Finished !
            bin.Close();
            EmulationPaused = false;
            state_is_loading_state = false; 
            this.ppu.videoOut.WriteNotification("State loaded from slot " + STATESlot, 120, Color.Green);
        }
    }
}
