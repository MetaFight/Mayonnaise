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
using MyNes.Core;
using SlimDX;
using SlimDX.DirectInput;

namespace MyNes
{
    class NesJoypadPcJoystickConnection : IJoypadConnecter
    {
        public NesJoypadPcJoystickConnection(IntPtr handle, string guid, IInputSettingsJoypad settings)
        {
            DirectInput di = new DirectInput();
            joystick = new Joystick(di, Guid.Parse(guid));
            joystick.SetCooperativeLevel(handle, CooperativeLevel.Nonexclusive | CooperativeLevel.Foreground);


            if (settings.ButtonUp != "")
                KeyUp = ParseKey(settings.ButtonUp);
            if (settings.ButtonDown != "")
                KeyDown = ParseKey(settings.ButtonDown);
            if (settings.ButtonLeft != "")
                KeyLeft = ParseKey(settings.ButtonLeft);
            if (settings.ButtonRight != "")
                KeyRight = ParseKey(settings.ButtonRight);
            if (settings.ButtonStart != "")
                KeyStart = ParseKey(settings.ButtonStart);
            if (settings.ButtonSelect != "")
                KeySelect = ParseKey(settings.ButtonSelect);
            if (settings.ButtonA != "")
                KeyA = ParseKey(settings.ButtonA);
            if (settings.ButtonB != "")
                KeyB = ParseKey(settings.ButtonB);
            if (settings.ButtonTurboA != "")
                KeyTurboA = ParseKey(settings.ButtonTurboA);
            if (settings.ButtonTurboB != "")
                KeyTurboB = ParseKey(settings.ButtonTurboB);
        }
        private int KeyUp = 0;
        private int KeyDown = 0;
        private int KeyLeft = 0;
        private int KeyRight = 0;
        private int KeyStart = 0;
        private int KeySelect = 0;
        private int KeyA = 0;
        private int KeyB = 0;
        private int KeyTurboA = 0;
        private int KeyTurboB = 0;
        private bool turbo;
        private Joystick joystick;
        private JoystickState joystickState;
        public override void Update()
        {
            turbo = !turbo;
            if (joystick.Acquire().IsSuccess)
            {
                joystickState = joystick.GetCurrentState();
                DATA = 0;

                if (IsPressed(KeyA))
                    DATA |= 1;

                if (IsPressed(KeyB))
                    DATA |= 2;

                if (IsPressed(KeyTurboA) && turbo)
                    DATA |= 1;

                if (IsPressed(KeyTurboB) && turbo)
                    DATA |= 2;

                if (IsPressed(KeySelect))
                    DATA |= 4;

                if (IsPressed(KeyStart))
                    DATA |= 8;

                if (IsPressed(KeyUp))
                    DATA |= 0x10;

                if (IsPressed(KeyDown))
                    DATA |= 0x20;

                if (IsPressed(KeyLeft))
                    DATA |= 0x40;

                if (IsPressed(KeyRight))
                    DATA |= 0x80;
            }
        }
        private bool IsPressed(int keyNumber)
        {
            if (keyNumber < 0)
            {
                if (keyNumber == -1)
                    return joystickState.X > 0xC000;
                else if (keyNumber == -2)
                    return joystickState.X < 0x4000;
                else if (keyNumber == -3)
                    return joystickState.Y > 0xC000;
                else if (keyNumber == -4)
                    return joystickState.Y < 0x4000;
            }
            else
            {
                return joystickState.IsPressed(keyNumber);
            }
            return false;
        }
        private int ParseKey(string key)
        {
            if (key == "X+")
                return -1;
            else if (key == "X-")
                return -2;
            else if (key == "Y+")
                return -3;
            else if (key == "Y-")
                return -4;
            else
                return Convert.ToInt32(key);
        }
    }
}
