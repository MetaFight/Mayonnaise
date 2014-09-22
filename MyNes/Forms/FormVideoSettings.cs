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
using SlimDX;
using SlimDX.Direct3D9;

namespace MyNes
{
    public partial class FormVideoSettings : Form
    {
        public FormVideoSettings()
        {
            InitializeComponent();
            LoadSettings();
        }
        private void LoadSettings()
        {
            //res
            Direct3D d3d = new Direct3D();
            comboBox_fullscreenRes.Items.Clear();
            for (int i = 0; i < d3d.Adapters[0].GetDisplayModes(SlimDX.Direct3D9.Format.X8R8G8B8).Count; i++)
            {
                comboBox_fullscreenRes.Items.Add(d3d.Adapters[0].GetDisplayModes(SlimDX.Direct3D9.Format.X8R8G8B8)[i].Width + " x " +
                    d3d.Adapters[0].GetDisplayModes(SlimDX.Direct3D9.Format.X8R8G8B8)[i].Height + " " +
                    d3d.Adapters[0].GetDisplayModes(SlimDX.Direct3D9.Format.X8R8G8B8)[i].RefreshRate + " Hz");
            }

            checkBox1.Checked = Program.Settings.Video_StretchToMulti;
            comboBox_windowedModeSize.SelectedIndex = Program.Settings.Video_StretchMulti - 1;
            checkBox_fullscreen.Checked = Program.Settings.Video_StartFullscreen;
            comboBox_fullscreenRes.SelectedIndex = Program.Settings.Video_FullscreenRes;
            checkBox_showNot.Checked = Program.Settings.Video_ShowNotifications;
            checkBox_showFPS.Checked = Program.Settings.Video_ShowFPS;
            checkBox_cutLines.Checked = Program.Settings.Video_CutLines;
            checkBox_hardware_vertex_processing.Checked = Program.Settings.Video_HardwareVertexProcessing;
            checkBox_keep_aspect_ratio.Checked = Program.Settings.Video_KeepAspectRatio;
            switch (Program.Settings.Video_Filter)
            {
                case TextureFilter.None: comboBox_filter.SelectedIndex = 0; break;
                case TextureFilter.Point: comboBox_filter.SelectedIndex = 1; break;
                case TextureFilter.Linear: comboBox_filter.SelectedIndex = 2; break;
            }
        }
        private void SaveSettings()
        {
            Program.Settings.Video_StretchToMulti = checkBox1.Checked;
            Program.Settings.Video_StretchMulti = comboBox_windowedModeSize.SelectedIndex + 1;
            Program.Settings.Video_StartFullscreen = checkBox_fullscreen.Checked;
            Program.Settings.Video_FullscreenRes = comboBox_fullscreenRes.SelectedIndex;
            Program.Settings.Video_ShowNotifications = checkBox_showNot.Checked;
            Program.Settings.Video_ShowFPS = checkBox_showFPS.Checked;
            Program.Settings.Video_CutLines = checkBox_cutLines.Checked;
            Program.Settings.Video_HardwareVertexProcessing = checkBox_hardware_vertex_processing.Checked;
            Program.Settings.Video_KeepAspectRatio = checkBox_keep_aspect_ratio.Checked;
            switch (comboBox_filter.SelectedIndex)
            {
                case 0: Program.Settings.Video_Filter = TextureFilter.None; break;
                case 1: Program.Settings.Video_Filter = TextureFilter.Point; break;
                case 2: Program.Settings.Video_Filter = TextureFilter.Linear; break;
            }
        }
        private void DefaultSettings()
        {
            checkBox1.Checked = true;
            comboBox_windowedModeSize.SelectedIndex = 1;
            checkBox_fullscreen.Checked = false;
            comboBox_fullscreenRes.SelectedIndex = 0;
            checkBox_showNot.Checked = true;
            checkBox_showFPS.Checked = false;
            checkBox_cutLines.Checked = true;
            checkBox_hardware_vertex_processing.Checked = true;
            checkBox_keep_aspect_ratio.Checked = true;
            comboBox_filter.SelectedIndex = 1;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            SaveSettings();
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            DefaultSettings();
        }
    }
}
