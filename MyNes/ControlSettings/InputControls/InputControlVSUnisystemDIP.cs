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
    public partial class InputControlVSUnisystemDIP : IInputSettingsControl
    {
        public InputControlVSUnisystemDIP()
        {
            InitializeComponent();
        }

        private List<DeviceInstance> devices;

        public override void LoadSettings()
        {
            RefreshDevices();
            // Load settings
            for (int i = 0; i < devices.Count; i++)
            {
                if (devices[i].InstanceGuid.ToString().ToLower() == Program.Settings.ControlSettings.VSUnisystemDIPDeviceGuid.ToLower())
                {
                    // This is it!
                    // Select the device
                    comboBox_device.SelectedIndex = i;
                    break;
                }
            }
            RefreshList();
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
        private void RefreshList()
        {
            listView1.Items.Clear();
            if (comboBox_device.SelectedIndex < 0) return;
            for (int i = 0; i < Program.Settings.ControlSettings.VSUnisystemDIPDevices.Count; i++)
            {
                if (Program.Settings.ControlSettings.VSUnisystemDIPDevices[i].DeviceGuid.ToLower() ==
                    devices[comboBox_device.SelectedIndex].InstanceGuid.ToString().ToLower())
                {
                    // This is it!
                    ListViewItem item = new ListViewItem("Credit Left Coin Slot");
                    item.SubItems.Add(Program.Settings.ControlSettings.VSUnisystemDIPDevices[i].CreditLeftCoinSlot);
                    listView1.Items.Add(item);
                    item = new ListViewItem("Credit Right Coin Slot");
                    item.SubItems.Add(Program.Settings.ControlSettings.VSUnisystemDIPDevices[i].CreditRightCoinSlot);
                    listView1.Items.Add(item);
                    item = new ListViewItem("Credit Service Button");
                    item.SubItems.Add(Program.Settings.ControlSettings.VSUnisystemDIPDevices[i].CreditServiceButton);
                    listView1.Items.Add(item);
                    item = new ListViewItem("DIP Switch 1");
                    item.SubItems.Add(Program.Settings.ControlSettings.VSUnisystemDIPDevices[i].DIPSwitch1);
                    listView1.Items.Add(item);
                    item = new ListViewItem("DIP Switch 2");
                    item.SubItems.Add(Program.Settings.ControlSettings.VSUnisystemDIPDevices[i].DIPSwitch2);
                    listView1.Items.Add(item);
                    item = new ListViewItem("DIP Switch 3");
                    item.SubItems.Add(Program.Settings.ControlSettings.VSUnisystemDIPDevices[i].DIPSwitch3);
                    listView1.Items.Add(item);
                    item = new ListViewItem("DIP Switch 4");
                    item.SubItems.Add(Program.Settings.ControlSettings.VSUnisystemDIPDevices[i].DIPSwitch4);
                    listView1.Items.Add(item);
                    item = new ListViewItem("DIP Switch 5");
                    item.SubItems.Add(Program.Settings.ControlSettings.VSUnisystemDIPDevices[i].DIPSwitch5);
                    listView1.Items.Add(item);
                    item = new ListViewItem("DIP Switch 6");
                    item.SubItems.Add(Program.Settings.ControlSettings.VSUnisystemDIPDevices[i].DIPSwitch6);
                    listView1.Items.Add(item);
                    item = new ListViewItem("DIP Switch 7");
                    item.SubItems.Add(Program.Settings.ControlSettings.VSUnisystemDIPDevices[i].DIPSwitch7);
                    listView1.Items.Add(item);
                    item = new ListViewItem("DIP Switch 8");
                    item.SubItems.Add(Program.Settings.ControlSettings.VSUnisystemDIPDevices[i].DIPSwitch8);
                    listView1.Items.Add(item);
                    return;
                }
            }
            // Not found !!
            ListViewItem item1 = new ListViewItem("Credit Left Coin Slot");
            item1.SubItems.Add("");
            listView1.Items.Add(item1);
            item1 = new ListViewItem("Credit Right Coin Slot");
            item1.SubItems.Add("");
            listView1.Items.Add(item1);
            item1 = new ListViewItem("Credit Service Button");
            item1.SubItems.Add("");
            listView1.Items.Add(item1);
            item1 = new ListViewItem("DIP Switch 1");
            item1.SubItems.Add("");
            listView1.Items.Add(item1);
            item1 = new ListViewItem("DIP Switch 2");
            item1.SubItems.Add("");
            listView1.Items.Add(item1);
            item1 = new ListViewItem("DIP Switch 3");
            item1.SubItems.Add("");
            listView1.Items.Add(item1);
            item1 = new ListViewItem("DIP Switch 4");
            item1.SubItems.Add("");
            listView1.Items.Add(item1);
            item1 = new ListViewItem("DIP Switch 5");
            item1.SubItems.Add("");
            listView1.Items.Add(item1);
            item1 = new ListViewItem("DIP Switch 6");
            item1.SubItems.Add("");
            listView1.Items.Add(item1);
            item1 = new ListViewItem("DIP Switch 7");
            item1.SubItems.Add("");
            listView1.Items.Add(item1);
            item1 = new ListViewItem("DIP Switch 8");
            item1.SubItems.Add("");
            listView1.Items.Add(item1);
        }
        public override void SaveSettings()
        {
            if (comboBox_device.SelectedIndex < 0)
            {
                Program.Settings.ControlSettings.Joypad1DeviceGuid = "";
                return;
            }
            bool found = false;
            for (int i = 0; i < Program.Settings.ControlSettings.VSUnisystemDIPDevices.Count; i++)
            {
                if (Program.Settings.ControlSettings.VSUnisystemDIPDevices[i].DeviceGuid.ToLower() ==
                    devices[comboBox_device.SelectedIndex].InstanceGuid.ToString().ToLower())
                {
                    Program.Settings.ControlSettings.Joypad1DeviceGuid = Program.Settings.ControlSettings.VSUnisystemDIPDevices[i].DeviceGuid;
                    found = true;
                    // Add the inputs
                    Program.Settings.ControlSettings.VSUnisystemDIPDevices[i].CreditLeftCoinSlot = listView1.Items[0].SubItems[1].Text;
                    Program.Settings.ControlSettings.VSUnisystemDIPDevices[i].CreditRightCoinSlot = listView1.Items[1].SubItems[1].Text;
                    Program.Settings.ControlSettings.VSUnisystemDIPDevices[i].CreditServiceButton = listView1.Items[2].SubItems[1].Text;
                    Program.Settings.ControlSettings.VSUnisystemDIPDevices[i].DIPSwitch1 = listView1.Items[3].SubItems[1].Text;
                    Program.Settings.ControlSettings.VSUnisystemDIPDevices[i].DIPSwitch2 = listView1.Items[4].SubItems[1].Text;
                    Program.Settings.ControlSettings.VSUnisystemDIPDevices[i].DIPSwitch3 = listView1.Items[5].SubItems[1].Text;
                    Program.Settings.ControlSettings.VSUnisystemDIPDevices[i].DIPSwitch4 = listView1.Items[6].SubItems[1].Text;
                    Program.Settings.ControlSettings.VSUnisystemDIPDevices[i].DIPSwitch5 = listView1.Items[7].SubItems[1].Text;
                    Program.Settings.ControlSettings.VSUnisystemDIPDevices[i].DIPSwitch6 = listView1.Items[8].SubItems[1].Text;
                    Program.Settings.ControlSettings.VSUnisystemDIPDevices[i].DIPSwitch7 = listView1.Items[9].SubItems[1].Text;
                    Program.Settings.ControlSettings.VSUnisystemDIPDevices[i].DIPSwitch8 = listView1.Items[10].SubItems[1].Text;
                    break;
                }
            }
            if (!found)
            {
                // Add the device
                Program.Settings.ControlSettings.VSUnisystemDIPDeviceGuid = devices[comboBox_device.SelectedIndex].InstanceGuid.ToString();
                IInputSettingsVSUnisystemDIP vs = new IInputSettingsVSUnisystemDIP();
                vs.DeviceGuid = Program.Settings.ControlSettings.VSUnisystemDIPDeviceGuid;
                vs.CreditLeftCoinSlot = listView1.Items[0].SubItems[1].Text;
                vs.CreditRightCoinSlot = listView1.Items[1].SubItems[1].Text;
                vs.CreditServiceButton = listView1.Items[2].SubItems[1].Text;
                vs.DIPSwitch1 = listView1.Items[3].SubItems[1].Text;
                vs.DIPSwitch2 = listView1.Items[4].SubItems[1].Text;
                vs.DIPSwitch3 = listView1.Items[5].SubItems[1].Text;
                vs.DIPSwitch4 = listView1.Items[6].SubItems[1].Text;
                vs.DIPSwitch5 = listView1.Items[7].SubItems[1].Text;
                vs.DIPSwitch6 = listView1.Items[8].SubItems[1].Text;
                vs.DIPSwitch7 = listView1.Items[9].SubItems[1].Text;
                vs.DIPSwitch8 = listView1.Items[10].SubItems[1].Text;
                Program.Settings.ControlSettings.VSUnisystemDIPDevices.Add(vs);
            }
        }
        public override bool CanSaveSettings
        {
            get
            {
                return true;
            }
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
            return "VS Unisystem DIP";
        }
        private void button1_Click(object sender, EventArgs e)
        {
            RefreshDevices();
        }
        private void comboBox_device_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshList();
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
