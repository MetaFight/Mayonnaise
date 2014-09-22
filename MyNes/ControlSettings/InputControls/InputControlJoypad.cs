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
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SlimDX.DirectInput;
using MMB;
namespace MyNes
{
    public partial class InputControlJoypad : IInputSettingsControl
    {
        public InputControlJoypad(int playerIndex)
        {
            this.playerIndex = playerIndex;
            InitializeComponent();
        }
        private int playerIndex;
        private List<DeviceInstance> devices;
        public override void LoadSettings()
        {
            RefreshDevices();
            // Load settings
            for (int i = 0; i < devices.Count; i++)
            {
                if (devices[i].InstanceGuid.ToString().ToLower() == Program.Settings.ControlSettings.Joypad1DeviceGuid.ToLower())
                {
                    // This is it!
                    // Select the device
                    comboBox_device.SelectedIndex = i;
                    break;
                }
            }
            switch (playerIndex)
            {
                case 0: RefreshListPlayer1(); break;
                case 1: RefreshListPlayer2(); break;
                case 2: RefreshListPlayer3(); break;
                case 3: RefreshListPlayer4(); break;
            }
        }
        public override void SaveSettings()
        {
            switch (playerIndex)
            {
                case 0: SavePlayer1(); break;
                case 1: SavePlayer2(); break;
                case 2: SavePlayer3(); break;
                case 3: SavePlayer4(); break;
            }
        }
        private void SavePlayer1()
        {
            if (comboBox_device.SelectedIndex < 0)
            {
                Program.Settings.ControlSettings.Joypad1DeviceGuid = "";
                return;
            }
            bool found = false;
            for (int i = 0; i < Program.Settings.ControlSettings.Joypad1Devices.Count; i++)
            {
                if (Program.Settings.ControlSettings.Joypad1Devices[i].DeviceGuid.ToLower() ==
                    devices[comboBox_device.SelectedIndex].InstanceGuid.ToString().ToLower())
                {
                    Program.Settings.ControlSettings.Joypad1DeviceGuid = Program.Settings.ControlSettings.Joypad1Devices[i].DeviceGuid;
                    found = true;
                    // Add the inputs
                    Program.Settings.ControlSettings.Joypad1Devices[i].ButtonA = listView1.Items[0].SubItems[1].Text;
                    Program.Settings.ControlSettings.Joypad1Devices[i].ButtonB = listView1.Items[1].SubItems[1].Text;
                    Program.Settings.ControlSettings.Joypad1Devices[i].ButtonTurboA = listView1.Items[2].SubItems[1].Text;
                    Program.Settings.ControlSettings.Joypad1Devices[i].ButtonTurboB = listView1.Items[3].SubItems[1].Text;
                    Program.Settings.ControlSettings.Joypad1Devices[i].ButtonStart = listView1.Items[4].SubItems[1].Text;
                    Program.Settings.ControlSettings.Joypad1Devices[i].ButtonSelect = listView1.Items[5].SubItems[1].Text;
                    Program.Settings.ControlSettings.Joypad1Devices[i].ButtonLeft = listView1.Items[6].SubItems[1].Text;
                    Program.Settings.ControlSettings.Joypad1Devices[i].ButtonUp = listView1.Items[7].SubItems[1].Text;
                    Program.Settings.ControlSettings.Joypad1Devices[i].ButtonRight = listView1.Items[8].SubItems[1].Text;
                    Program.Settings.ControlSettings.Joypad1Devices[i].ButtonDown = listView1.Items[9].SubItems[1].Text;
                    break;
                }
            }
            if (!found)
            {
                // Add the device
                Program.Settings.ControlSettings.Joypad1DeviceGuid = devices[comboBox_device.SelectedIndex].InstanceGuid.ToString();
                IInputSettingsJoypad joy1 = new IInputSettingsJoypad();
                joy1.DeviceGuid = Program.Settings.ControlSettings.Joypad1DeviceGuid;
                joy1.ButtonA = listView1.Items[0].SubItems[1].Text;
                joy1.ButtonB = listView1.Items[1].SubItems[1].Text;
                joy1.ButtonTurboA = listView1.Items[2].SubItems[1].Text;
                joy1.ButtonTurboB = listView1.Items[3].SubItems[1].Text;
                joy1.ButtonStart = listView1.Items[4].SubItems[1].Text;
                joy1.ButtonSelect = listView1.Items[5].SubItems[1].Text;
                joy1.ButtonLeft = listView1.Items[6].SubItems[1].Text;
                joy1.ButtonUp = listView1.Items[7].SubItems[1].Text;
                joy1.ButtonRight = listView1.Items[8].SubItems[1].Text;
                joy1.ButtonDown = listView1.Items[9].SubItems[1].Text;
                Program.Settings.ControlSettings.Joypad1Devices.Add(joy1);
            }
        }
        private void SavePlayer2()
        {
            if (comboBox_device.SelectedIndex < 0)
            {
                Program.Settings.ControlSettings.Joypad2DeviceGuid = "";
                return;
            }
            bool found = false;
            for (int i = 0; i < Program.Settings.ControlSettings.Joypad2Devices.Count; i++)
            {
                if (Program.Settings.ControlSettings.Joypad2Devices[i].DeviceGuid.ToLower() ==
                    devices[comboBox_device.SelectedIndex].InstanceGuid.ToString().ToLower())
                {
                    Program.Settings.ControlSettings.Joypad2DeviceGuid = Program.Settings.ControlSettings.Joypad2Devices[i].DeviceGuid;
                    found = true;
                    // Add the inputs
                    Program.Settings.ControlSettings.Joypad2Devices[i].ButtonA = listView1.Items[0].SubItems[1].Text;
                    Program.Settings.ControlSettings.Joypad2Devices[i].ButtonB = listView1.Items[1].SubItems[1].Text;
                    Program.Settings.ControlSettings.Joypad2Devices[i].ButtonTurboA = listView1.Items[2].SubItems[1].Text;
                    Program.Settings.ControlSettings.Joypad2Devices[i].ButtonTurboB = listView1.Items[3].SubItems[1].Text;
                    Program.Settings.ControlSettings.Joypad2Devices[i].ButtonStart = listView1.Items[4].SubItems[1].Text;
                    Program.Settings.ControlSettings.Joypad2Devices[i].ButtonSelect = listView1.Items[5].SubItems[1].Text;
                    Program.Settings.ControlSettings.Joypad2Devices[i].ButtonLeft = listView1.Items[6].SubItems[1].Text;
                    Program.Settings.ControlSettings.Joypad2Devices[i].ButtonUp = listView1.Items[7].SubItems[1].Text;
                    Program.Settings.ControlSettings.Joypad2Devices[i].ButtonRight = listView1.Items[8].SubItems[1].Text;
                    Program.Settings.ControlSettings.Joypad2Devices[i].ButtonDown = listView1.Items[9].SubItems[1].Text;
                    break;
                }
            }
            if (!found)
            {
                // Add the device
                Program.Settings.ControlSettings.Joypad2DeviceGuid = devices[comboBox_device.SelectedIndex].InstanceGuid.ToString();
                IInputSettingsJoypad joy2 = new IInputSettingsJoypad();
                joy2.DeviceGuid = Program.Settings.ControlSettings.Joypad2DeviceGuid;
                joy2.ButtonA = listView1.Items[0].SubItems[1].Text;
                joy2.ButtonB = listView1.Items[1].SubItems[1].Text;
                joy2.ButtonTurboA = listView1.Items[2].SubItems[1].Text;
                joy2.ButtonTurboB = listView1.Items[3].SubItems[1].Text;
                joy2.ButtonStart = listView1.Items[4].SubItems[1].Text;
                joy2.ButtonSelect = listView1.Items[5].SubItems[1].Text;
                joy2.ButtonLeft = listView1.Items[6].SubItems[1].Text;
                joy2.ButtonUp = listView1.Items[7].SubItems[1].Text;
                joy2.ButtonRight = listView1.Items[8].SubItems[1].Text;
                joy2.ButtonDown = listView1.Items[9].SubItems[1].Text;
                Program.Settings.ControlSettings.Joypad2Devices.Add(joy2);
            }
        }
        private void SavePlayer3()
        {
            if (comboBox_device.SelectedIndex < 0)
            {
                Program.Settings.ControlSettings.Joypad3DeviceGuid = "";
                return;
            }
            bool found = false;
            for (int i = 0; i < Program.Settings.ControlSettings.Joypad3Devices.Count; i++)
            {
                if (Program.Settings.ControlSettings.Joypad3Devices[i].DeviceGuid.ToLower() ==
                    devices[comboBox_device.SelectedIndex].InstanceGuid.ToString().ToLower())
                {
                    Program.Settings.ControlSettings.Joypad3DeviceGuid = Program.Settings.ControlSettings.Joypad3Devices[i].DeviceGuid;
                    found = true;
                    // Add the inputs
                    Program.Settings.ControlSettings.Joypad3Devices[i].ButtonA = listView1.Items[0].SubItems[1].Text;
                    Program.Settings.ControlSettings.Joypad3Devices[i].ButtonB = listView1.Items[1].SubItems[1].Text;
                    Program.Settings.ControlSettings.Joypad3Devices[i].ButtonTurboA = listView1.Items[2].SubItems[1].Text;
                    Program.Settings.ControlSettings.Joypad3Devices[i].ButtonTurboB = listView1.Items[3].SubItems[1].Text;
                    Program.Settings.ControlSettings.Joypad3Devices[i].ButtonStart = listView1.Items[4].SubItems[1].Text;
                    Program.Settings.ControlSettings.Joypad3Devices[i].ButtonSelect = listView1.Items[5].SubItems[1].Text;
                    Program.Settings.ControlSettings.Joypad3Devices[i].ButtonLeft = listView1.Items[6].SubItems[1].Text;
                    Program.Settings.ControlSettings.Joypad3Devices[i].ButtonUp = listView1.Items[7].SubItems[1].Text;
                    Program.Settings.ControlSettings.Joypad3Devices[i].ButtonRight = listView1.Items[8].SubItems[1].Text;
                    Program.Settings.ControlSettings.Joypad3Devices[i].ButtonDown = listView1.Items[9].SubItems[1].Text;
                    break;
                }
            }
            if (!found)
            {
                // Add the device
                Program.Settings.ControlSettings.Joypad3DeviceGuid = devices[comboBox_device.SelectedIndex].InstanceGuid.ToString();
                IInputSettingsJoypad joy3 = new IInputSettingsJoypad();
                joy3.DeviceGuid = Program.Settings.ControlSettings.Joypad3DeviceGuid;
                joy3.ButtonA = listView1.Items[0].SubItems[1].Text;
                joy3.ButtonB = listView1.Items[1].SubItems[1].Text;
                joy3.ButtonTurboA = listView1.Items[2].SubItems[1].Text;
                joy3.ButtonTurboB = listView1.Items[3].SubItems[1].Text;
                joy3.ButtonStart = listView1.Items[4].SubItems[1].Text;
                joy3.ButtonSelect = listView1.Items[5].SubItems[1].Text;
                joy3.ButtonLeft = listView1.Items[6].SubItems[1].Text;
                joy3.ButtonUp = listView1.Items[7].SubItems[1].Text;
                joy3.ButtonRight = listView1.Items[8].SubItems[1].Text;
                joy3.ButtonDown = listView1.Items[9].SubItems[1].Text;
                Program.Settings.ControlSettings.Joypad3Devices.Add(joy3);
            }
        }
        private void SavePlayer4()
        {
            if (comboBox_device.SelectedIndex < 0)
            {
                Program.Settings.ControlSettings.Joypad4DeviceGuid = "";
                return;
            }
            bool found = false;
            for (int i = 0; i < Program.Settings.ControlSettings.Joypad4Devices.Count; i++)
            {
                if (Program.Settings.ControlSettings.Joypad4Devices[i].DeviceGuid.ToLower() ==
                    devices[comboBox_device.SelectedIndex].InstanceGuid.ToString().ToLower())
                {
                    Program.Settings.ControlSettings.Joypad4DeviceGuid = Program.Settings.ControlSettings.Joypad4Devices[i].DeviceGuid;
                    found = true;
                    // Add the inputs
                    Program.Settings.ControlSettings.Joypad4Devices[i].ButtonA = listView1.Items[0].SubItems[1].Text;
                    Program.Settings.ControlSettings.Joypad4Devices[i].ButtonB = listView1.Items[1].SubItems[1].Text;
                    Program.Settings.ControlSettings.Joypad4Devices[i].ButtonTurboA = listView1.Items[2].SubItems[1].Text;
                    Program.Settings.ControlSettings.Joypad4Devices[i].ButtonTurboB = listView1.Items[3].SubItems[1].Text;
                    Program.Settings.ControlSettings.Joypad4Devices[i].ButtonStart = listView1.Items[4].SubItems[1].Text;
                    Program.Settings.ControlSettings.Joypad4Devices[i].ButtonSelect = listView1.Items[5].SubItems[1].Text;
                    Program.Settings.ControlSettings.Joypad4Devices[i].ButtonLeft = listView1.Items[6].SubItems[1].Text;
                    Program.Settings.ControlSettings.Joypad4Devices[i].ButtonUp = listView1.Items[7].SubItems[1].Text;
                    Program.Settings.ControlSettings.Joypad4Devices[i].ButtonRight = listView1.Items[8].SubItems[1].Text;
                    Program.Settings.ControlSettings.Joypad4Devices[i].ButtonDown = listView1.Items[9].SubItems[1].Text;
                    break;
                }
            }
            if (!found)
            {
                // Add the device
                Program.Settings.ControlSettings.Joypad4DeviceGuid = devices[comboBox_device.SelectedIndex].InstanceGuid.ToString();
                IInputSettingsJoypad joy4 = new IInputSettingsJoypad();
                joy4.DeviceGuid = Program.Settings.ControlSettings.Joypad4DeviceGuid;
                joy4.ButtonA = listView1.Items[0].SubItems[1].Text;
                joy4.ButtonB = listView1.Items[1].SubItems[1].Text;
                joy4.ButtonTurboA = listView1.Items[2].SubItems[1].Text;
                joy4.ButtonTurboB = listView1.Items[3].SubItems[1].Text;
                joy4.ButtonStart = listView1.Items[4].SubItems[1].Text;
                joy4.ButtonSelect = listView1.Items[5].SubItems[1].Text;
                joy4.ButtonLeft = listView1.Items[6].SubItems[1].Text;
                joy4.ButtonUp = listView1.Items[7].SubItems[1].Text;
                joy4.ButtonRight = listView1.Items[8].SubItems[1].Text;
                joy4.ButtonDown = listView1.Items[9].SubItems[1].Text;
                Program.Settings.ControlSettings.Joypad4Devices.Add(joy4);
            }
        }
        public override bool CanSaveSettings
        {
            get
            {
                return true;
            }
        }
        private void RefreshDevices()
        {
            comboBox_device.Items.Clear();
            DirectInput di = new DirectInput();
            devices = new List<DeviceInstance>();
            foreach (DeviceInstance ins in di.GetDevices())
            {
                if (ins.Type == DeviceType.Joystick || ins.Type == DeviceType.Keyboard)
                {
                    comboBox_device.Items.Add(ins.InstanceName);
                    devices.Add(ins);
                }
            }
        }
        private void RefreshListPlayer1()
        {
            listView1.Items.Clear();
            if (comboBox_device.SelectedIndex < 0) return;
            for (int i = 0; i < Program.Settings.ControlSettings.Joypad1Devices.Count; i++)
            {
                if (Program.Settings.ControlSettings.Joypad1Devices[i].DeviceGuid.ToLower() ==
                    devices[comboBox_device.SelectedIndex].InstanceGuid.ToString().ToLower())
                {
                    // This is it!
                    ListViewItem item = new ListViewItem("A");
                    item.SubItems.Add(Program.Settings.ControlSettings.Joypad1Devices[i].ButtonA);
                    listView1.Items.Add(item);
                    item = new ListViewItem("B");
                    item.SubItems.Add(Program.Settings.ControlSettings.Joypad1Devices[i].ButtonB);
                    listView1.Items.Add(item);
                    item = new ListViewItem("Turbo A");
                    item.SubItems.Add(Program.Settings.ControlSettings.Joypad1Devices[i].ButtonTurboA);
                    listView1.Items.Add(item);
                    item = new ListViewItem("Turbo B");
                    item.SubItems.Add(Program.Settings.ControlSettings.Joypad1Devices[i].ButtonTurboB);
                    listView1.Items.Add(item);
                    item = new ListViewItem("Start");
                    item.SubItems.Add(Program.Settings.ControlSettings.Joypad1Devices[i].ButtonStart);
                    listView1.Items.Add(item);
                    item = new ListViewItem("Select");
                    item.SubItems.Add(Program.Settings.ControlSettings.Joypad1Devices[i].ButtonSelect);
                    listView1.Items.Add(item);
                    item = new ListViewItem("Left");
                    item.SubItems.Add(Program.Settings.ControlSettings.Joypad1Devices[i].ButtonLeft);
                    listView1.Items.Add(item);
                    item = new ListViewItem("Up");
                    item.SubItems.Add(Program.Settings.ControlSettings.Joypad1Devices[i].ButtonUp);
                    listView1.Items.Add(item);
                    item = new ListViewItem("Right");
                    item.SubItems.Add(Program.Settings.ControlSettings.Joypad1Devices[i].ButtonRight);
                    listView1.Items.Add(item);
                    item = new ListViewItem("Down");
                    item.SubItems.Add(Program.Settings.ControlSettings.Joypad1Devices[i].ButtonDown);
                    listView1.Items.Add(item);
                    return;
                }
            }
            // Not found !!
            ListViewItem item1 = new ListViewItem("A");
            item1.SubItems.Add("");
            listView1.Items.Add(item1);
            item1 = new ListViewItem("B");
            item1.SubItems.Add("");
            listView1.Items.Add(item1);
            item1 = new ListViewItem("Turbo A");
            item1.SubItems.Add("");
            listView1.Items.Add(item1);
            item1 = new ListViewItem("Turbo B");
            item1.SubItems.Add("");
            listView1.Items.Add(item1);
            item1 = new ListViewItem("Start");
            item1.SubItems.Add("");
            listView1.Items.Add(item1);
            item1 = new ListViewItem("Select");
            item1.SubItems.Add("");
            listView1.Items.Add(item1);
            item1 = new ListViewItem("Left");
            item1.SubItems.Add("");
            listView1.Items.Add(item1);
            item1 = new ListViewItem("Up");
            item1.SubItems.Add("");
            listView1.Items.Add(item1);
            item1 = new ListViewItem("Right");
            item1.SubItems.Add("");
            listView1.Items.Add(item1);
            item1 = new ListViewItem("Down");
            item1.SubItems.Add("");
            listView1.Items.Add(item1);
        }
        private void RefreshListPlayer2()
        {
            listView1.Items.Clear();
            if (comboBox_device.SelectedIndex < 0) return;
            for (int i = 0; i < Program.Settings.ControlSettings.Joypad2Devices.Count; i++)
            {
                if (Program.Settings.ControlSettings.Joypad2Devices[i].DeviceGuid.ToLower() ==
                    devices[comboBox_device.SelectedIndex].InstanceGuid.ToString().ToLower())
                {
                    // This is it!
                    ListViewItem item = new ListViewItem("A");
                    item.SubItems.Add(Program.Settings.ControlSettings.Joypad2Devices[i].ButtonA);
                    listView1.Items.Add(item);
                    item = new ListViewItem("B");
                    item.SubItems.Add(Program.Settings.ControlSettings.Joypad2Devices[i].ButtonB);
                    listView1.Items.Add(item);
                    item = new ListViewItem("Turbo A");
                    item.SubItems.Add(Program.Settings.ControlSettings.Joypad2Devices[i].ButtonTurboA);
                    listView1.Items.Add(item);
                    item = new ListViewItem("Turbo B");
                    item.SubItems.Add(Program.Settings.ControlSettings.Joypad2Devices[i].ButtonTurboB);
                    listView1.Items.Add(item);
                    item = new ListViewItem("Start");
                    item.SubItems.Add(Program.Settings.ControlSettings.Joypad2Devices[i].ButtonStart);
                    listView1.Items.Add(item);
                    item = new ListViewItem("Select");
                    item.SubItems.Add(Program.Settings.ControlSettings.Joypad2Devices[i].ButtonSelect);
                    listView1.Items.Add(item);
                    item = new ListViewItem("Left");
                    item.SubItems.Add(Program.Settings.ControlSettings.Joypad2Devices[i].ButtonLeft);
                    listView1.Items.Add(item);
                    item = new ListViewItem("Up");
                    item.SubItems.Add(Program.Settings.ControlSettings.Joypad2Devices[i].ButtonUp);
                    listView1.Items.Add(item);
                    item = new ListViewItem("Right");
                    item.SubItems.Add(Program.Settings.ControlSettings.Joypad2Devices[i].ButtonRight);
                    listView1.Items.Add(item);
                    item = new ListViewItem("Down");
                    item.SubItems.Add(Program.Settings.ControlSettings.Joypad2Devices[i].ButtonDown);
                    listView1.Items.Add(item);
                    return;
                }
            }
            // Not found !!
            ListViewItem item1 = new ListViewItem("A");
            item1.SubItems.Add("");
            listView1.Items.Add(item1);
            item1 = new ListViewItem("B");
            item1.SubItems.Add("");
            listView1.Items.Add(item1);
            item1 = new ListViewItem("Turbo A");
            item1.SubItems.Add("");
            listView1.Items.Add(item1);
            item1 = new ListViewItem("Turbo B");
            item1.SubItems.Add("");
            listView1.Items.Add(item1);
            item1 = new ListViewItem("Start");
            item1.SubItems.Add("");
            listView1.Items.Add(item1);
            item1 = new ListViewItem("Select");
            item1.SubItems.Add("");
            listView1.Items.Add(item1);
            item1 = new ListViewItem("Left");
            item1.SubItems.Add("");
            listView1.Items.Add(item1);
            item1 = new ListViewItem("Up");
            item1.SubItems.Add("");
            listView1.Items.Add(item1);
            item1 = new ListViewItem("Right");
            item1.SubItems.Add("");
            listView1.Items.Add(item1);
            item1 = new ListViewItem("Down");
            item1.SubItems.Add("");
            listView1.Items.Add(item1);
        }
        private void RefreshListPlayer3()
        {
            listView1.Items.Clear();
            if (comboBox_device.SelectedIndex < 0) return;
            for (int i = 0; i < Program.Settings.ControlSettings.Joypad3Devices.Count; i++)
            {
                if (Program.Settings.ControlSettings.Joypad3Devices[i].DeviceGuid.ToLower() ==
                    devices[comboBox_device.SelectedIndex].InstanceGuid.ToString().ToLower())
                {
                    // This is it!
                    ListViewItem item = new ListViewItem("A");
                    item.SubItems.Add(Program.Settings.ControlSettings.Joypad3Devices[i].ButtonA);
                    listView1.Items.Add(item);
                    item = new ListViewItem("B");
                    item.SubItems.Add(Program.Settings.ControlSettings.Joypad3Devices[i].ButtonB);
                    listView1.Items.Add(item);
                    item = new ListViewItem("Turbo A");
                    item.SubItems.Add(Program.Settings.ControlSettings.Joypad3Devices[i].ButtonTurboA);
                    listView1.Items.Add(item);
                    item = new ListViewItem("Turbo B");
                    item.SubItems.Add(Program.Settings.ControlSettings.Joypad3Devices[i].ButtonTurboB);
                    listView1.Items.Add(item);
                    item = new ListViewItem("Start");
                    item.SubItems.Add(Program.Settings.ControlSettings.Joypad3Devices[i].ButtonStart);
                    listView1.Items.Add(item);
                    item = new ListViewItem("Select");
                    item.SubItems.Add(Program.Settings.ControlSettings.Joypad3Devices[i].ButtonSelect);
                    listView1.Items.Add(item);
                    item = new ListViewItem("Left");
                    item.SubItems.Add(Program.Settings.ControlSettings.Joypad3Devices[i].ButtonLeft);
                    listView1.Items.Add(item);
                    item = new ListViewItem("Up");
                    item.SubItems.Add(Program.Settings.ControlSettings.Joypad3Devices[i].ButtonUp);
                    listView1.Items.Add(item);
                    item = new ListViewItem("Right");
                    item.SubItems.Add(Program.Settings.ControlSettings.Joypad3Devices[i].ButtonRight);
                    listView1.Items.Add(item);
                    item = new ListViewItem("Down");
                    item.SubItems.Add(Program.Settings.ControlSettings.Joypad3Devices[i].ButtonDown);
                    listView1.Items.Add(item);
                    return;
                }
            }
            // Not found !!
            ListViewItem item1 = new ListViewItem("A");
            item1.SubItems.Add("");
            listView1.Items.Add(item1);
            item1 = new ListViewItem("B");
            item1.SubItems.Add("");
            listView1.Items.Add(item1);
            item1 = new ListViewItem("Turbo A");
            item1.SubItems.Add("");
            listView1.Items.Add(item1);
            item1 = new ListViewItem("Turbo B");
            item1.SubItems.Add("");
            listView1.Items.Add(item1);
            item1 = new ListViewItem("Start");
            item1.SubItems.Add("");
            listView1.Items.Add(item1);
            item1 = new ListViewItem("Select");
            item1.SubItems.Add("");
            listView1.Items.Add(item1);
            item1 = new ListViewItem("Left");
            item1.SubItems.Add("");
            listView1.Items.Add(item1);
            item1 = new ListViewItem("Up");
            item1.SubItems.Add("");
            listView1.Items.Add(item1);
            item1 = new ListViewItem("Right");
            item1.SubItems.Add("");
            listView1.Items.Add(item1);
            item1 = new ListViewItem("Down");
            item1.SubItems.Add("");
            listView1.Items.Add(item1);
        }
        private void RefreshListPlayer4()
        {
            listView1.Items.Clear();
            if (comboBox_device.SelectedIndex < 0) return;
            for (int i = 0; i < Program.Settings.ControlSettings.Joypad4Devices.Count; i++)
            {
                if (Program.Settings.ControlSettings.Joypad4Devices[i].DeviceGuid.ToLower() ==
                    devices[comboBox_device.SelectedIndex].InstanceGuid.ToString().ToLower())
                {
                    // This is it!
                    ListViewItem item = new ListViewItem("A");
                    item.SubItems.Add(Program.Settings.ControlSettings.Joypad4Devices[i].ButtonA);
                    listView1.Items.Add(item);
                    item = new ListViewItem("B");
                    item.SubItems.Add(Program.Settings.ControlSettings.Joypad4Devices[i].ButtonB);
                    listView1.Items.Add(item);
                    item = new ListViewItem("Turbo A");
                    item.SubItems.Add(Program.Settings.ControlSettings.Joypad4Devices[i].ButtonTurboA);
                    listView1.Items.Add(item);
                    item = new ListViewItem("Turbo B");
                    item.SubItems.Add(Program.Settings.ControlSettings.Joypad4Devices[i].ButtonTurboB);
                    listView1.Items.Add(item);
                    item = new ListViewItem("Start");
                    item.SubItems.Add(Program.Settings.ControlSettings.Joypad4Devices[i].ButtonStart);
                    listView1.Items.Add(item);
                    item = new ListViewItem("Select");
                    item.SubItems.Add(Program.Settings.ControlSettings.Joypad4Devices[i].ButtonSelect);
                    listView1.Items.Add(item);
                    item = new ListViewItem("Left");
                    item.SubItems.Add(Program.Settings.ControlSettings.Joypad4Devices[i].ButtonLeft);
                    listView1.Items.Add(item);
                    item = new ListViewItem("Up");
                    item.SubItems.Add(Program.Settings.ControlSettings.Joypad4Devices[i].ButtonUp);
                    listView1.Items.Add(item);
                    item = new ListViewItem("Right");
                    item.SubItems.Add(Program.Settings.ControlSettings.Joypad4Devices[i].ButtonRight);
                    listView1.Items.Add(item);
                    item = new ListViewItem("Down");
                    item.SubItems.Add(Program.Settings.ControlSettings.Joypad4Devices[i].ButtonDown);
                    listView1.Items.Add(item);
                    return;
                }
            }
            // Not found !!
            ListViewItem item1 = new ListViewItem("A");
            item1.SubItems.Add("");
            listView1.Items.Add(item1);
            item1 = new ListViewItem("B");
            item1.SubItems.Add("");
            listView1.Items.Add(item1);
            item1 = new ListViewItem("Turbo A");
            item1.SubItems.Add("");
            listView1.Items.Add(item1);
            item1 = new ListViewItem("Turbo B");
            item1.SubItems.Add("");
            listView1.Items.Add(item1);
            item1 = new ListViewItem("Start");
            item1.SubItems.Add("");
            listView1.Items.Add(item1);
            item1 = new ListViewItem("Select");
            item1.SubItems.Add("");
            listView1.Items.Add(item1);
            item1 = new ListViewItem("Left");
            item1.SubItems.Add("");
            listView1.Items.Add(item1);
            item1 = new ListViewItem("Up");
            item1.SubItems.Add("");
            listView1.Items.Add(item1);
            item1 = new ListViewItem("Right");
            item1.SubItems.Add("");
            listView1.Items.Add(item1);
            item1 = new ListViewItem("Down");
            item1.SubItems.Add("");
            listView1.Items.Add(item1);
        }
        private void SetPlayer()
        {
            FormKey frm = new FormKey(devices[comboBox_device.SelectedIndex].Type,
                devices[comboBox_device.SelectedIndex].InstanceGuid.ToString(), listView1.SelectedItems[0].Text);
            frm.Location = new Point(this.Parent.Parent.Parent.Location.X + this.Parent.Parent.Location.X + button2.Location.X,
            this.Parent.Parent.Parent.Location.Y + this.Parent.Parent.Location.Y + button2.Location.Y + 30);
            if (frm.ShowDialog(this) == DialogResult.OK)
            {
                listView1.SelectedItems[0].SubItems[1].Text = frm.InputName;
            }
        }
        private void SetAllPlayer()
        {
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                FormKey frm = new FormKey(devices[comboBox_device.SelectedIndex].Type,
                    devices[comboBox_device.SelectedIndex].InstanceGuid.ToString(),
                    listView1.Items[i].Text);
                frm.Location = new Point(this.Parent.Parent.Parent.Location.X + this.Parent.Parent.Location.X + button3.Location.X,
                this.Parent.Parent.Parent.Location.Y + this.Parent.Parent.Location.Y + button3.Location.Y + 30);
                if (frm.ShowDialog(this) == DialogResult.OK)
                {
                    listView1.Items[i].SubItems[1].Text = frm.InputName;
                }
                else
                {
                    break;
                }
            }
        }
        public override string ToString()
        {
            return "Joypad (Player " + (playerIndex + 1) + ")";
        }
        private void button1_Click(object sender, EventArgs e)
        {
            RefreshDevices();
        }
        private void comboBox_device_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (playerIndex)
            {
                case 0: RefreshListPlayer1(); break;
                case 1: RefreshListPlayer2(); break;
                case 2: RefreshListPlayer3(); break;
                case 3: RefreshListPlayer4(); break;
            }
        }
        // Set
        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox_device.SelectedIndex < 0)
            {
                ManagedMessageBox.ShowErrorMessage(Program.ResourceManager.GetString("Message_PleaseSelectTheInputDeviceFirst"));
                return;
            }
            if (listView1.SelectedItems.Count != 1)
                return;

            SetPlayer();
        }
        // Set all
        private void button3_Click(object sender, EventArgs e)
        {
            SetAllPlayer();
        }
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            button2.Enabled = listView1.SelectedItems.Count == 1;
        }
    }
}
