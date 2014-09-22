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
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MyNes
{
    public partial class FormPreferences : Form
    {
        public FormPreferences()
        {
            InitializeComponent();
            LoadSettings();
        }
        private void LoadSettings()
        {
            checkBox_saveSramAtEmuShutdown.Checked = Program.Settings.SaveSramOnShutdown;
            comboBox_snapFormat.SelectedItem = Program.Settings.SnapshotFormat;
            checkBox1.Checked = Program.Settings.PauseAtFocusLost;
            checkBox_showKnownIssues.Checked = Program.Settings.ShowRomIssuesIfHave;
            checkBox_tryRunNotSupported.Checked = Program.Settings.IgnoreNotSupportedMapper;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Program.Settings.SaveSramOnShutdown = checkBox_saveSramAtEmuShutdown.Checked;
            Program.Settings.PauseAtFocusLost = checkBox1.Checked;
            Program.Settings.SnapshotFormat = comboBox_snapFormat.SelectedItem.ToString();
            Program.Settings.ShowRomIssuesIfHave = checkBox_showKnownIssues.Checked;
            Program.Settings.IgnoreNotSupportedMapper = checkBox_tryRunNotSupported.Checked;
            Program.Settings.Save();
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            checkBox_saveSramAtEmuShutdown.Checked = true;
            checkBox1.Checked = true;
            checkBox_showKnownIssues.Checked = true;
            checkBox_tryRunNotSupported.Checked = false;
        }
    }
}
