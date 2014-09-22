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
using System.Collections.Generic;
using SlimDX.DirectInput;
namespace MyNes
{
    [System.Serializable]
    public class ControlMappingSettings
    {
        // Joypads
        public List<IInputSettingsJoypad> Joypad1Devices = new List<IInputSettingsJoypad>();
        public string Joypad1DeviceGuid;
        public List<IInputSettingsJoypad> Joypad2Devices = new List<IInputSettingsJoypad>();
        public string Joypad2DeviceGuid;
        public List<IInputSettingsJoypad> Joypad3Devices = new List<IInputSettingsJoypad>();
        public string Joypad3DeviceGuid;
        public List<IInputSettingsJoypad> Joypad4Devices = new List<IInputSettingsJoypad>();
        public string Joypad4DeviceGuid;
        public List<IInputSettingsVSUnisystemDIP> VSUnisystemDIPDevices = new List<IInputSettingsVSUnisystemDIP>();
        public string VSUnisystemDIPDeviceGuid;

        public static void BuildDefaultControlSettings()
        {
            Program.Settings.ControlSettings = new ControlMappingSettings();
            Program.Settings.ControlSettings.Joypad1Devices = new List<IInputSettingsJoypad>();
            Program.Settings.ControlSettings.Joypad2Devices = new List<IInputSettingsJoypad>();
            Program.Settings.ControlSettings.Joypad3Devices = new List<IInputSettingsJoypad>();
            Program.Settings.ControlSettings.Joypad4Devices = new List<IInputSettingsJoypad>();
            Program.Settings.ControlSettings.VSUnisystemDIPDevices = new List<IInputSettingsVSUnisystemDIP>();

            DirectInput di = new DirectInput();
            foreach (DeviceInstance ins in di.GetDevices())
            {
                if (ins.Type == DeviceType.Keyboard)
                {
                    // Player 1 joypad
                    IInputSettingsJoypad joy1 = new IInputSettingsJoypad();
                    joy1.DeviceGuid = ins.InstanceGuid.ToString();
                    joy1.ButtonA = "X";
                    joy1.ButtonB = "Z";
                    joy1.ButtonTurboA = "S";
                    joy1.ButtonTurboB = "A";
                    joy1.ButtonDown = "DownArrow";
                    joy1.ButtonLeft = "LeftArrow";
                    joy1.ButtonRight = "RightArrow";
                    joy1.ButtonUp = "UpArrow";
                    joy1.ButtonSelect = "C";
                    joy1.ButtonStart = "V";
                    Program.Settings.ControlSettings.Joypad1Devices.Add(joy1);
                    Program.Settings.ControlSettings.Joypad1DeviceGuid = joy1.DeviceGuid;

                    // Player 2 joypad
                    IInputSettingsJoypad joy2 = new IInputSettingsJoypad();
                    joy2.DeviceGuid = ins.InstanceGuid.ToString();
                    joy2.ButtonA = "K";
                    joy2.ButtonB = "L";
                    joy2.ButtonTurboA = "I";
                    joy2.ButtonTurboB = "O";
                    joy2.ButtonDown = "S";
                    joy2.ButtonLeft = "A";
                    joy2.ButtonRight = "D";
                    joy2.ButtonUp = "W";
                    joy2.ButtonSelect = "B";
                    joy2.ButtonStart = "N";
                    Program.Settings.ControlSettings.Joypad2Devices.Add(joy2);
                    Program.Settings.ControlSettings.Joypad2DeviceGuid = joy2.DeviceGuid;

                    IInputSettingsVSUnisystemDIP vs = new IInputSettingsVSUnisystemDIP();
                    // VSUnisystem
                    vs.DeviceGuid = ins.InstanceGuid.ToString();
                    vs.CreditServiceButton = "End";
                    vs.DIPSwitch1 = "NumberPad1";
                    vs.DIPSwitch2 = "NumberPad2";
                    vs.DIPSwitch3 = "NumberPad3";
                    vs.DIPSwitch4 = "NumberPad4";
                    vs.DIPSwitch5 = "NumberPad5";
                    vs.DIPSwitch6 = "NumberPad6";
                    vs.DIPSwitch7 = "NumberPad7";
                    vs.DIPSwitch8 = "NumberPad8";
                    vs.CreditLeftCoinSlot = "Insert";
                    vs.CreditRightCoinSlot = "Home";
                    Program.Settings.ControlSettings.VSUnisystemDIPDevices.Add(vs);
                    Program.Settings.ControlSettings.VSUnisystemDIPDeviceGuid = vs.DeviceGuid;
                    break;
                }
            }
        }
    }
}
