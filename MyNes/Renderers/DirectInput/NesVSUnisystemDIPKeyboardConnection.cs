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
    class NesVSUnisystemDIPKeyboardConnection : IVSUnisystemDIPConnecter
    {
        public NesVSUnisystemDIPKeyboardConnection(IntPtr handle, IInputSettingsVSUnisystemDIP settings)
        {
            DirectInput di = new DirectInput();
            keyboard = new Keyboard(di);
            keyboard.SetCooperativeLevel(handle, CooperativeLevel.Nonexclusive | CooperativeLevel.Foreground);

            if (settings.CreditServiceButton != "")
                CreditServiceButton = (SlimDX.DirectInput.Key)Enum.Parse(typeof(SlimDX.DirectInput.Key), settings.CreditServiceButton);
            if (settings.DIPSwitch1 != "")
                DIPSwitch1 = (SlimDX.DirectInput.Key)Enum.Parse(typeof(SlimDX.DirectInput.Key), settings.DIPSwitch1);
            if (settings.DIPSwitch2 != "")
                DIPSwitch2 = (SlimDX.DirectInput.Key)Enum.Parse(typeof(SlimDX.DirectInput.Key), settings.DIPSwitch2);
            if (settings.DIPSwitch3 != "")
                DIPSwitch3 = (SlimDX.DirectInput.Key)Enum.Parse(typeof(SlimDX.DirectInput.Key), settings.DIPSwitch3);
            if (settings.DIPSwitch4 != "")
                DIPSwitch4 = (SlimDX.DirectInput.Key)Enum.Parse(typeof(SlimDX.DirectInput.Key), settings.DIPSwitch4);
            if (settings.DIPSwitch5 != "")
                DIPSwitch5 = (SlimDX.DirectInput.Key)Enum.Parse(typeof(SlimDX.DirectInput.Key), settings.DIPSwitch5);
            if (settings.DIPSwitch6 != "")
                DIPSwitch6 = (SlimDX.DirectInput.Key)Enum.Parse(typeof(SlimDX.DirectInput.Key), settings.DIPSwitch6);
            if (settings.DIPSwitch7 != "")
                DIPSwitch7 = (SlimDX.DirectInput.Key)Enum.Parse(typeof(SlimDX.DirectInput.Key), settings.DIPSwitch7);
            if (settings.DIPSwitch8 != "")
                DIPSwitch8 = (SlimDX.DirectInput.Key)Enum.Parse(typeof(SlimDX.DirectInput.Key), settings.DIPSwitch8);
            if (settings.CreditLeftCoinSlot != "")
                CreditLeftCoinSlot = (SlimDX.DirectInput.Key)Enum.Parse(typeof(SlimDX.DirectInput.Key), settings.CreditLeftCoinSlot);
            if (settings.CreditRightCoinSlot != "")
                CreditRightCoinSlot = (SlimDX.DirectInput.Key)Enum.Parse(typeof(SlimDX.DirectInput.Key), settings.CreditRightCoinSlot);
            NesEmu.EMUShutdown += NesEmu_EMUShutdown;
        }
        private Key CreditServiceButton = Key.Unknown;
        private Key DIPSwitch1 = Key.Unknown;
        private Key DIPSwitch2 = Key.Unknown;
        private Key DIPSwitch3 = Key.Unknown;
        private Key DIPSwitch4 = Key.Unknown;
        private Key DIPSwitch5 = Key.Unknown;
        private Key DIPSwitch6 = Key.Unknown;
        private Key DIPSwitch7 = Key.Unknown;
        private Key DIPSwitch8 = Key.Unknown;
        private Key CreditLeftCoinSlot = Key.Unknown;
        private bool leftCoin = false;
        private Key CreditRightCoinSlot = Key.Unknown;
        private bool rightCoin = false;
        private Keyboard keyboard;
        private KeyboardState state;
        private byte data4016;
        private byte data4017;

        private void NesEmu_EMUShutdown(object sender, EventArgs e)
        {
            leftCoin = false;
            rightCoin = false;
        }
        public override void Update()
        {
            if (keyboard.Acquire().IsSuccess)
            {
                state = keyboard.GetCurrentState();

                data4016 = 0;

                if (state.IsPressed(CreditServiceButton))
                    data4016 |= 0x04;
                if (state.IsPressed(DIPSwitch1))
                    data4016 |= 0x08;
                if (state.IsPressed(DIPSwitch2))
                    data4016 |= 0x10;
                if (state.IsPressed(CreditLeftCoinSlot))
                    leftCoin = true;
                if (leftCoin)
                    data4016 |= 0x20;
                if (state.IsPressed(CreditRightCoinSlot))
                    rightCoin = true;
                if (rightCoin)
                    data4016 |= 0x40;

                data4017 = 0;
                if (state.IsPressed(DIPSwitch3))
                    data4017 |= 0x04;
                if (state.IsPressed(DIPSwitch4))
                    data4017 |= 0x08;
                if (state.IsPressed(DIPSwitch5))
                    data4017 |= 0x10;
                if (state.IsPressed(DIPSwitch6))
                    data4017 |= 0x20;
                if (state.IsPressed(DIPSwitch7))
                    data4017 |= 0x40;
                if (state.IsPressed(DIPSwitch8))
                    data4017 |= 0x80;
            }
        }
        public override byte GetData4016()
        {
            return data4016;
        }
        public override byte GetData4017()
        {
            return data4017;
        }
        public override void Write4020(ref byte data)
        {
            if ((data & 0x1) == 0x1)
            {
                leftCoin = false;
                rightCoin = false;
            }
        }
    }
}
