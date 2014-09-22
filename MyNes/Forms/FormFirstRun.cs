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
    public partial class FormFirstRun : Form
    {
        public FormFirstRun()
        {
            InitializeComponent();
            for (int i = 0; i < Program.SupportedLanguages.Length / 3; i++)
            {
                ToolStripMenuItem item = new ToolStripMenuItem();
                comboBox_language.Items.Add(Program.SupportedLanguages[i, 2]);

                if (Program.SupportedLanguages[i, 0] == System.Globalization.CultureInfo.InstalledUICulture.EnglishName)
                    comboBox_language.SelectedIndex = i;
            }
            if (comboBox_language.SelectedIndex < 0)
                comboBox_language.SelectedIndex = 0;

            checkBox_runLauncher.Checked = Program.Settings.LauncherShowAyAppStart;
            checkBox_resize.Checked = Program.Settings.Video_StretchToMulti;
            checkBox_soundPlayback.Checked = Program.Settings.Audio_SoundEnabled;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Program.Settings.Language = Program.SupportedLanguages[comboBox_language.SelectedIndex, 0];

            Program.Settings.LauncherShowAyAppStart = checkBox_runLauncher.Checked;
            Program.Settings.Video_StretchToMulti = checkBox_resize.Checked;
            Program.Settings.Audio_SoundEnabled = checkBox_soundPlayback.Checked;
            Close();
        }
    }
}
