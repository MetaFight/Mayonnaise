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
    class NesVSUnisystemDIPJoystickConnection : IVSUnisystemDIPConnecter
    {
        public NesVSUnisystemDIPJoystickConnection(IntPtr handle, string guid, IInputSettingsVSUnisystemDIP settings)
        {
            DirectInput di = new DirectInput();
            joystick = new Joystick(di, Guid.Parse(guid));
            joystick.SetCooperativeLevel(handle, CooperativeLevel.Nonexclusive | CooperativeLevel.Foreground);

            if (settings.CreditServiceButton != "")
                CreditServiceButton = ParseKey(settings.CreditServiceButton);
            if (settings.DIPSwitch1 != "")
                DIPSwitch1 = ParseKey(settings.DIPSwitch1);
            if (settings.DIPSwitch2 != "")
                DIPSwitch2 = ParseKey(settings.DIPSwitch2);
            if (settings.DIPSwitch3 != "")
                DIPSwitch3 = ParseKey(settings.DIPSwitch3);
            if (settings.DIPSwitch4 != "")
                DIPSwitch4 = ParseKey(settings.DIPSwitch4);
            if (settings.DIPSwitch5 != "")
                DIPSwitch5 = ParseKey(settings.DIPSwitch5);
            if (settings.DIPSwitch6 != "")
                DIPSwitch6 = ParseKey(settings.DIPSwitch6);
            if (settings.DIPSwitch7 != "")
                DIPSwitch7 = ParseKey(settings.DIPSwitch7);
            if (settings.DIPSwitch8 != "")
                DIPSwitch8 = ParseKey(settings.DIPSwitch8);
            if (settings.CreditLeftCoinSlot != "")
                CreditLeftCoinSlot = ParseKey(settings.CreditLeftCoinSlot);
            if (settings.CreditRightCoinSlot != "")
                CreditRightCoinSlot = ParseKey(settings.CreditRightCoinSlot);
            NesEmu.EMUShutdown += NesEmu_EMUShutdown;
        }
        private int CreditServiceButton = 0;
        private int DIPSwitch1 = 0;
        private int DIPSwitch2 = 0;
        private int DIPSwitch3 = 0;
        private int DIPSwitch4 = 0;
        private int DIPSwitch5 = 0;
        private int DIPSwitch6 = 0;
        private int DIPSwitch7 = 0;
        private int DIPSwitch8 = 0;
        private int CreditLeftCoinSlot = 0;
        private bool leftCoin = false;
        private int CreditRightCoinSlot = 0;
        private bool rightCoin = false;
        private Joystick joystick;
        private JoystickState joystickState;
        private byte data4016;
        private byte data4017;

        private void NesEmu_EMUShutdown(object sender, EventArgs e)
        {
            leftCoin = false;
            rightCoin = false;
        }
        public override void Update()
        {
            if (joystick.Acquire().IsSuccess)
            {
                joystickState = joystick.GetCurrentState();

                data4016 = 0;

                if (IsPressed(CreditServiceButton))
                    data4016 |= 0x04;
                if (IsPressed(DIPSwitch1))
                    data4016 |= 0x08;
                if (IsPressed(DIPSwitch2))
                    data4016 |= 0x10;
                if (IsPressed(CreditLeftCoinSlot))
                    leftCoin = true;
                if (leftCoin)
                    data4016 |= 0x20;
                if (IsPressed(CreditRightCoinSlot))
                    rightCoin = true;
                if (rightCoin)
                    data4016 |= 0x40;

                data4017 = 0;
                if (IsPressed(DIPSwitch3))
                    data4017 |= 0x04;
                if (IsPressed(DIPSwitch4))
                    data4017 |= 0x08;
                if (IsPressed(DIPSwitch5))
                    data4017 |= 0x10;
                if (IsPressed(DIPSwitch6))
                    data4017 |= 0x20;
                if (IsPressed(DIPSwitch7))
                    data4017 |= 0x40;
                if (IsPressed(DIPSwitch8))
                    data4017 |= 0x80;
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
