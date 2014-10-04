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
/*Input section*/
using System;
namespace MyNes.Core
{
	[Obsolete("Reminder to split this into two classes:  1) Emulated NES input logic; 2) Bridging emulator input with real input devices")]
    public class Input
    {
        // TODO: controllers for zapper and vsunisystem
        public int PORT0;
        public int PORT1;
        public int inputStrobe;
        public IJoypadConnecter joypad1;
        public IJoypadConnecter joypad2;
        public IJoypadConnecter joypad3;
        public IJoypadConnecter joypad4;
        public IZapperConnecter zapper;
		[Obsolete("Should this be moved the the (eventual) VSUnisystem class?")]
        public IVSUnisystemDIPConnecter VSUnisystemDIP;
        public bool IsFourPlayers;
        public bool IsZapperConnected;

		private readonly LegacyNesEmu legacy;

		public Input(LegacyNesEmu legacy)
		{
			this.legacy = legacy;
		}

        public void FinishFrame()
        {
            joypad1.Update();
            joypad2.Update();
            if (IsFourPlayers)
            {
                joypad3.Update();
                joypad4.Update();
            }
            if (IsZapperConnected)
                zapper.Update();
            if (this.legacy.IsVSUnisystem)
                VSUnisystemDIP.Update();
        }
        public void InitializeInput()
        {
            // Initialize all controllers to blank
            joypad1 = new BlankJoypad();
            joypad2 = new BlankJoypad();
            joypad3 = new BlankJoypad();
            joypad4 = new BlankJoypad();
            zapper = new BlankZapper();
            VSUnisystemDIP = new BlankVSUnisystemDIP();
        }
        public void SetupJoypads(IJoypadConnecter joy1, IJoypadConnecter joy2, IJoypadConnecter joy3, IJoypadConnecter joy4)
        {
            joypad1 = joy1;
            joypad2 = joy2;
            joypad3 = joy3;
            joypad4 = joy4;
            if (joypad1 == null)
                joypad1 = new BlankJoypad();
            if (joypad2 == null)
                joypad2 = new BlankJoypad();
            if (joypad3 == null)
                joypad3 = new BlankJoypad();
            if (joypad4 == null)
                joypad4 = new BlankJoypad();
        }
        public void SetupZapper(IZapperConnecter zap)
        {
            zapper = zap;
        }
        public void SetupVSUnisystemDIP(IVSUnisystemDIPConnecter vsUnisystemDIP)
        {
            VSUnisystemDIP = vsUnisystemDIP;
        }

		internal void LoadState(System.IO.BinaryReader bin)
		{
			this.PORT0 = bin.ReadInt32();
			this.PORT1 = bin.ReadInt32();
			this.inputStrobe = bin.ReadInt32();
		}
	}
}
