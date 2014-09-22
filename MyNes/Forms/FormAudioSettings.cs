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
    public partial class FormAudioSettings : Form
    {
        public FormAudioSettings()
        {
            InitializeComponent();
            checkBox_enableSound.Checked = Program.Settings.Audio_SoundEnabled;
            trackBar2.Value = Program.Settings.Audio_Volume;
            trackBar_bufferLength.Value = Program.Settings.Audio_BufferSizeInMilliseconds / 100;
            trackBar_latency.Value = Program.Settings.Audio_LatencyInMilliseconds / 100;
            label_volume.Text = trackBar2.Value + "%";
            label_latency.Text = "0." + trackBar_latency.Value;
            label_bufferLength.Text = "0." + trackBar_bufferLength.Value;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Program.Settings.Audio_SoundEnabled = checkBox_enableSound.Checked;

            Program.Settings.Audio_Volume = trackBar2.Value;
            Program.Settings.Audio_BufferSizeInMilliseconds = trackBar_bufferLength.Value * 100;
            Program.Settings.Audio_LatencyInMilliseconds = trackBar_latency.Value * 100;
            Program.Settings.Save();
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }
        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            label_volume.Text = trackBar2.Value + "%";
        }
        private void trackBar_bufferLength_Scroll(object sender, EventArgs e)
        {
            label_bufferLength.Text = trackBar_bufferLength.Value < 10 ? "0." + trackBar_bufferLength.Value : "1";
        }
        private void trackBar_latency_Scroll(object sender, EventArgs e)
        {
            label_latency.Text = trackBar_latency.Value < 10 ? "0." + trackBar_latency.Value : "1";
        }
        private void button3_Click(object sender, EventArgs e)
        {
            checkBox_enableSound.Checked = true;
            trackBar2.Value = 100;
            trackBar_bufferLength.Value = 4;
            trackBar_latency.Value = 2;
            label_volume.Text = trackBar2.Value + "%";
            label_latency.Text = "0." + trackBar_latency.Value;
            label_bufferLength.Text = "0." + trackBar_bufferLength.Value;
        }
    }
}
