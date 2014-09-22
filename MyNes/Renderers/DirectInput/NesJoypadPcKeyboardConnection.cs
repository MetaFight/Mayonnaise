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
    class NesJoypadPcKeyboardConnection : IJoypadConnecter
    {
        public NesJoypadPcKeyboardConnection(IntPtr handle, IInputSettingsJoypad settings)
        {
            DirectInput di = new DirectInput();
            keyboard = new Keyboard(di);
            keyboard.SetCooperativeLevel(handle, CooperativeLevel.Nonexclusive | CooperativeLevel.Foreground);

            if (settings.ButtonUp != "")
                KeyUp = (SlimDX.DirectInput.Key)Enum.Parse(typeof(SlimDX.DirectInput.Key), settings.ButtonUp);
            if (settings.ButtonDown != "")
                KeyDown = (SlimDX.DirectInput.Key)Enum.Parse(typeof(SlimDX.DirectInput.Key), settings.ButtonDown);
            if (settings.ButtonLeft != "")
                KeyLeft = (SlimDX.DirectInput.Key)Enum.Parse(typeof(SlimDX.DirectInput.Key), settings.ButtonLeft);
            if (settings.ButtonRight != "")
                KeyRight = (SlimDX.DirectInput.Key)Enum.Parse(typeof(SlimDX.DirectInput.Key), settings.ButtonRight);
            if (settings.ButtonStart != "")
                KeyStart = (SlimDX.DirectInput.Key)Enum.Parse(typeof(SlimDX.DirectInput.Key), settings.ButtonStart);
            if (settings.ButtonSelect != "")
                KeySelect = (SlimDX.DirectInput.Key)Enum.Parse(typeof(SlimDX.DirectInput.Key), settings.ButtonSelect);
            if (settings.ButtonA != "")
                KeyA = (SlimDX.DirectInput.Key)Enum.Parse(typeof(SlimDX.DirectInput.Key), settings.ButtonA);
            if (settings.ButtonB != "")
                KeyB = (SlimDX.DirectInput.Key)Enum.Parse(typeof(SlimDX.DirectInput.Key), settings.ButtonB);
            if (settings.ButtonTurboA != "")
                KeyTurboA = (SlimDX.DirectInput.Key)Enum.Parse(typeof(SlimDX.DirectInput.Key), settings.ButtonTurboA);
            if (settings.ButtonTurboB != "")
                KeyTurboB = (SlimDX.DirectInput.Key)Enum.Parse(typeof(SlimDX.DirectInput.Key), settings.ButtonTurboB);
        }
        private Key KeyUp = Key.Unknown;
        private Key KeyDown = Key.Unknown;
        private Key KeyLeft = Key.Unknown;
        private Key KeyRight = Key.Unknown;
        private Key KeyStart = Key.Unknown;
        private Key KeySelect = Key.Unknown;
        private Key KeyA = Key.Unknown;
        private Key KeyB = Key.Unknown;
        private Key KeyTurboA = Key.Unknown;
        private Key KeyTurboB = Key.Unknown;
        private Keyboard keyboard;
        private KeyboardState state;
        private bool turbo;

        public override void Update()
        {
            turbo = !turbo;
            if (keyboard.Acquire().IsSuccess)
            {
                state = keyboard.GetCurrentState();
                DATA = 0;

                if (state.IsPressed(KeyA))
                    DATA |= 1;

                if (state.IsPressed(KeyB))
                    DATA |= 2;

                if (state.IsPressed(KeyTurboA) && turbo)
                    DATA |= 1;

                if (state.IsPressed(KeyTurboB) && turbo)
                    DATA |= 2;

                if (state.IsPressed(KeySelect))
                    DATA |= 4;

                if (state.IsPressed(KeyStart))
                    DATA |= 8;

                if (state.IsPressed(KeyUp))
                    DATA |= 0x10;

                if (state.IsPressed(KeyDown))
                    DATA |= 0x20;

                if (state.IsPressed(KeyLeft))
                    DATA |= 0x40;

                if (state.IsPressed(KeyRight))
                    DATA |= 0x80;
            }
        }
    }
}
