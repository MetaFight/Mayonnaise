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
namespace MyNes.Core
{
    public partial class NesEmu
    {
        // TODO: controllers for zapper and vsunisystem
        private static int PORT0;
        private static int PORT1;
        private static int inputStrobe;
        private static IJoypadConnecter joypad1;
        private static IJoypadConnecter joypad2;
        private static IJoypadConnecter joypad3;
        private static IJoypadConnecter joypad4;
        private static IZapperConnecter zapper;
        private static IVSUnisystemDIPConnecter VSUnisystemDIP;
        public static bool IsFourPlayers;
        public static bool IsZapperConnected;

        private static void InputFinishFrame()
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
            if (IsVSUnisystem)
                VSUnisystemDIP.Update();
        }
        private static void InputInitialize()
        {
            // Initialize all controllers to blank
            joypad1 = new BlankJoypad();
            joypad2 = new BlankJoypad();
            joypad3 = new BlankJoypad();
            joypad4 = new BlankJoypad();
            zapper = new BlankZapper();
            VSUnisystemDIP = new BlankVSUnisystemDIP();
        }
        public static void SetupJoypads(IJoypadConnecter joy1, IJoypadConnecter joy2, IJoypadConnecter joy3, IJoypadConnecter joy4)
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
        public static void SetupZapper(IZapperConnecter zap)
        {
            zapper = zap;
        }
        public static void SetupVSUnisystemDIP(IVSUnisystemDIPConnecter vsUnisystemDIP)
        {
            VSUnisystemDIP = vsUnisystemDIP;
        }
    }
}
