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
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MyNes.Core;
using System.IO;
using System.Diagnostics;
using MMB;
namespace MyNes
{
    public partial class FormPaletteSettings : Form
    {
        public FormPaletteSettings()
        {
            InitializeComponent();
            LoadSettings();
        }

        private void LoadSettings()
        {
            isLoadingSettings = true;
            Trace.WriteLine("Loading palette generator settings ...");
            // Load generator settings.
            NTSCPaletteGenerator.brightness = Program.Settings.Palette_NTSC_brightness;
            NTSCPaletteGenerator.contrast = Program.Settings.Palette_NTSC_contrast;
            NTSCPaletteGenerator.gamma = Program.Settings.Palette_NTSC_gamma;
            NTSCPaletteGenerator.hue_tweak = Program.Settings.Palette_NTSC_hue_tweak;
            NTSCPaletteGenerator.saturation = Program.Settings.Palette_NTSC_saturation;

            PALBPaletteGenerator.brightness = Program.Settings.Palette_PALB_brightness;
            PALBPaletteGenerator.contrast = Program.Settings.Palette_PALB_contrast;
            PALBPaletteGenerator.gamma = Program.Settings.Palette_PALB_gamma;
            PALBPaletteGenerator.hue_tweak = Program.Settings.Palette_PALB_hue_tweak;
            PALBPaletteGenerator.saturation = Program.Settings.Palette_PALB_saturation;
            // Load settings
            Trace.WriteLine("Loading palette settings ...");
            radioButton_useGenerators.Checked = Program.Settings.Palette_UseGenerators;
            radioButton_usaPaletteFile.Checked = !Program.Settings.Palette_UseGenerators;
            switch (Program.Settings.Palette_GeneratorUsageMode)
            {
                case PaletteGeneratorUsage.AUTO: radioButton_gen_auto.Checked = true; break;
                case PaletteGeneratorUsage.NTSC: radioButton_gen_ntsc.Checked = true; break;
                case PaletteGeneratorUsage.PAL: radioButton_gen_pal.Checked = true; break;
            }
            currentPalette = new int[512];
            
            groupBox2.Enabled = radioButton_useGenerators.Checked;
            comboBox1.Enabled = !radioButton_useGenerators.Checked;
            linkLabel1.Enabled = !radioButton_useGenerators.Checked;

            RefreshPaletteFilesList();
            isLoadingSettings = false;
            UpdatePreview();
            LoadGeneratorSliders();
        }
        private int[] currentPalette;
        private bool isLoadingSettings;
        private void RefreshPaletteFilesList()
        {
            Trace.WriteLine("Refresh palette file list ...");
            // Load all palette files
            string[] files = Directory.GetFiles(Application.StartupPath + "\\Palettes\\");

            comboBox1.Items.Clear();
            if (files == null) return;
            // Set the first one
            for (int i = 0; i < files.Length; i++)
            {
                if (Path.GetExtension(files[i]).ToLower() == ".pal")
                {
                    comboBox1.Items.Add(Path.GetFileName(files[i]));
                    if (Program.Settings.Palette_FilePath == files[i])
                        comboBox1.SelectedIndex = i;
                }
            }
            Trace.WriteLine("Palette files list refreshed successfully.");
        }
        private void LoadGeneratorSliders()
        {
            if (radioButton_ntsc.Checked)
            {
                hScrollBar_brightness.Value = (int)(Program.Settings.Palette_NTSC_brightness * 1000);
                hScrollBar_contrast.Value = (int)(Program.Settings.Palette_NTSC_contrast * 1000);
                hScrollBar_gamma.Value = (int)(Program.Settings.Palette_NTSC_gamma * 1000);
                hScrollBar_hue_tweak.Value = (int)(Program.Settings.Palette_NTSC_hue_tweak * 1000);
                hScrollBar_saturation.Value = (int)(Program.Settings.Palette_NTSC_saturation * 1000);

                label_bright.Text = Program.Settings.Palette_NTSC_brightness.ToString("F3");
                label_const.Text = Program.Settings.Palette_NTSC_contrast.ToString("F3");
                label_gamma.Text = Program.Settings.Palette_NTSC_gamma.ToString("F3");
                label_hue.Text = Program.Settings.Palette_NTSC_hue_tweak.ToString("F3");
                label_satur.Text = Program.Settings.Palette_NTSC_saturation.ToString("F3");
            }
            else
            {
                hScrollBar_brightness.Value = (int)(Program.Settings.Palette_PALB_brightness * 1000);
                hScrollBar_contrast.Value = (int)(Program.Settings.Palette_PALB_contrast * 1000);
                hScrollBar_gamma.Value = (int)(Program.Settings.Palette_PALB_gamma * 1000);
                hScrollBar_hue_tweak.Value = (int)(Program.Settings.Palette_PALB_hue_tweak * 1000);
                hScrollBar_saturation.Value = (int)(Program.Settings.Palette_PALB_saturation * 1000);

                label_bright.Text = Program.Settings.Palette_PALB_brightness.ToString("F3");
                label_const.Text = Program.Settings.Palette_PALB_contrast.ToString("F3");
                label_gamma.Text = Program.Settings.Palette_PALB_gamma.ToString("F3");
                label_hue.Text = Program.Settings.Palette_PALB_hue_tweak.ToString("F3");
                label_satur.Text = Program.Settings.Palette_PALB_saturation.ToString("F3");
            }
        }
        private void ApplyGeneratorSliders()
        {
            float v = hScrollBar_saturation.Value;
            if (radioButton_ntsc.Checked)
            {
                NTSCPaletteGenerator.saturation = (v / 1000);
                label_satur.Text = NTSCPaletteGenerator.saturation.ToString("F3");
            }
            else
            {
                PALBPaletteGenerator.saturation = (v / 1000);
                label_satur.Text = PALBPaletteGenerator.saturation.ToString("F3");
            }
            v = hScrollBar_hue_tweak.Value;
            if (radioButton_ntsc.Checked)
            {
                NTSCPaletteGenerator.hue_tweak = (v / 1000);
                label_hue.Text = NTSCPaletteGenerator.hue_tweak.ToString("F3");
            }
            else
            {
                PALBPaletteGenerator.hue_tweak = (v / 1000);
                label_hue.Text = PALBPaletteGenerator.hue_tweak.ToString("F3");
            }
            v = hScrollBar_contrast.Value;
            if (radioButton_ntsc.Checked)
            {
                NTSCPaletteGenerator.contrast = (v / 1000);
                label_const.Text = NTSCPaletteGenerator.contrast.ToString("F3");
            }
            else
            {
                PALBPaletteGenerator.contrast = (v / 1000);
                label_const.Text = PALBPaletteGenerator.contrast.ToString("F3");
            }
            v = hScrollBar_brightness.Value;
            if (radioButton_ntsc.Checked)
            {
                NTSCPaletteGenerator.brightness = (v / 1000);
                label_bright.Text = NTSCPaletteGenerator.brightness.ToString("F3");
            }
            else
            {
                PALBPaletteGenerator.brightness = (v / 1000);
                label_bright.Text = PALBPaletteGenerator.brightness.ToString("F3");
            }
            v = hScrollBar_gamma.Value;
            if (radioButton_ntsc.Checked)
            {
                NTSCPaletteGenerator.gamma = (v / 1000);
                label_gamma.Text = NTSCPaletteGenerator.gamma.ToString("F3");
            }
            else
            {
                PALBPaletteGenerator.gamma = (v / 1000);
                label_gamma.Text = PALBPaletteGenerator.gamma.ToString("F3");
            }
        }
        private void SaveSettings()
        {
            // Save values
            Program.Settings.Palette_NTSC_brightness = NTSCPaletteGenerator.brightness;
            Program.Settings.Palette_NTSC_contrast = NTSCPaletteGenerator.contrast;
            Program.Settings.Palette_NTSC_gamma = NTSCPaletteGenerator.gamma;
            Program.Settings.Palette_NTSC_hue_tweak = NTSCPaletteGenerator.hue_tweak;
            Program.Settings.Palette_NTSC_saturation = NTSCPaletteGenerator.saturation;

            Program.Settings.Palette_PALB_brightness = PALBPaletteGenerator.brightness;
            Program.Settings.Palette_PALB_contrast = PALBPaletteGenerator.contrast;
            Program.Settings.Palette_PALB_gamma = PALBPaletteGenerator.gamma;
            Program.Settings.Palette_PALB_hue_tweak = PALBPaletteGenerator.hue_tweak;
            Program.Settings.Palette_PALB_saturation = PALBPaletteGenerator.saturation;

            // Save modes !
            Program.Settings.Palette_FilePath = Application.StartupPath + "\\Palettes\\" + comboBox1.SelectedItem.ToString();
            if (radioButton_gen_auto.Checked)
                Program.Settings.Palette_GeneratorUsageMode = PaletteGeneratorUsage.AUTO;
            else if (radioButton_gen_ntsc.Checked)
                Program.Settings.Palette_GeneratorUsageMode = PaletteGeneratorUsage.NTSC;
            else if (radioButton_gen_pal.Checked)
                Program.Settings.Palette_GeneratorUsageMode = PaletteGeneratorUsage.PAL;
            Program.Settings.Palette_UseGenerators = radioButton_useGenerators.Checked;
            Program.Settings.Save();
        }
        private void UpdatePreview()
        {
            if (isLoadingSettings) return;
            if (tabControl1.SelectedIndex == 0)
            {
                // Settings tab
                if (radioButton_useGenerators.Checked)
                {
                    if (radioButton_gen_auto.Checked || radioButton_gen_ntsc.Checked)
                    {
                        currentPalette = NTSCPaletteGenerator.GeneratePalette();
                        panel1.Invalidate();
                    }
                    else
                    {
                        currentPalette = PALBPaletteGenerator.GeneratePalette();
                        panel1.Invalidate();
                    }
                }
                else
                {
                    if (comboBox1 == null) return;
                    if (comboBox1.SelectedIndex < 0)
                        currentPalette = new int[512];
                    try
                    {
                        if (PaletteFileWrapper.LoadFile(Application.StartupPath + "\\Palettes\\" + comboBox1.SelectedItem.ToString(),
                            out currentPalette))
                        {
                            panel1.Invalidate();
                        }
                        else
                        {
                            ManagedMessageBox.ShowErrorMessage(Program.ResourceManager.GetString("Message_UnableToLoadPaletteFile"));
                            currentPalette = new int[512];
                            panel1.Invalidate();
                        }
                    }
                    catch (Exception ex)
                    {
                        ManagedMessageBox.ShowErrorMessage(ex.Message);
                        ManagedMessageBox.ShowErrorMessage(Program.ResourceManager.GetString("Message_UnableToLoadPaletteFile"));
                        currentPalette = new int[512];
                        panel1.Invalidate();
                    }
                }
            }
            else
            {
                if (radioButton_ntsc.Checked)
                {
                    currentPalette = NTSCPaletteGenerator.GeneratePalette();
                    panel1.Invalidate();
                }
                else
                {
                    currentPalette = PALBPaletteGenerator.GeneratePalette();
                    panel1.Invalidate();
                }
            }
        }
        private void UpdatePreviewGenerator()
        {
            if (radioButton_ntsc.Checked)
            {
                currentPalette = NTSCPaletteGenerator.GeneratePalette();
                panel1.Invalidate();
            }
            else
            {
                currentPalette = PALBPaletteGenerator.GeneratePalette();
                panel1.Invalidate();
            }
        }
        private unsafe void ShowPalette(Graphics gr, int[] PaletteFormat)
        {
            gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            int y = 0;
            int x = 0;
            int w = 16;
            int H = 10;
            for (int j = 0; j < PaletteFormat.Length; j++)
            {
                y = (j / 16) * H;
                x = (j % w) * w;
                Bitmap bmp = new Bitmap(w, H);
                BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, w, H),
                    ImageLockMode.WriteOnly, PixelFormat.Format32bppRgb);
                int* numPtr = (int*)bmpData.Scan0;
                for (int i = 0; i < w * H; i++)
                {
                    numPtr[i] = PaletteFormat[j];
                }
                bmp.UnlockBits(bmpData);
                gr.DrawImage(bmp, x, y, w, H);
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            ShowPalette(e.Graphics, currentPalette);
        }
        private void hScrollBar_saturation_Scroll(object sender, ScrollEventArgs e)
        {
            ApplyGeneratorSliders();
            UpdatePreview();
        }
        private void hScrollBar_hue_tweak_Scroll(object sender, ScrollEventArgs e)
        {
            ApplyGeneratorSliders();
            UpdatePreview();
        }
        private void hScrollBar_contrast_Scroll(object sender, ScrollEventArgs e)
        {
            ApplyGeneratorSliders();
            UpdatePreview();
        }
        private void hScrollBar_brightness_Scroll(object sender, ScrollEventArgs e)
        {
            ApplyGeneratorSliders();
            UpdatePreview();
        }
        private void hScrollBar_gamma_Scroll(object sender, ScrollEventArgs e)
        {
            ApplyGeneratorSliders();
            UpdatePreview();
        }
        //load
        private void button9_Click(object sender, EventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Filter = Program.ResourceManager.GetString("FilterMyNes_PaletteFile");
            if (op.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                string[] lines = File.ReadAllLines(op.FileName);

                for (int i = 0; i < lines.Length; i++)
                {
                    string[] pars = lines[i].Split(new char[] { '=' });
                    switch (pars[0])
                    {
                        case "Brightness":
                            hScrollBar_brightness.Value = (int)(float.Parse(pars[1]) * 1000);
                            break;
                        case "Contrast":
                            hScrollBar_contrast.Value = (int)(float.Parse(pars[1]) * 1000);
                            break;
                        case "Gamma":
                            hScrollBar_gamma.Value = (int)(float.Parse(pars[1]) * 1000);
                            break;
                        case "Hue":
                            hScrollBar_hue_tweak.Value = (int)(float.Parse(pars[1]) * 1000);
                            break;
                        case "Saturation":
                            hScrollBar_saturation.Value = (int)(float.Parse(pars[1]) * 1000);
                            break;
                    }
                }

                ApplyGeneratorSliders();
                UpdatePreview();
            }
        }
        //save
        private void button8_Click(object sender, EventArgs e)
        {
            SaveFileDialog sav = new SaveFileDialog();
            sav.Filter = Program.ResourceManager.GetString("FilterMyNes_PaletteFile");
            if (sav.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                List<string> lines = new List<string>();
                lines.Add("Brightness=" + NTSCPaletteGenerator.brightness);
                lines.Add("Contrast=" + NTSCPaletteGenerator.contrast);
                lines.Add("Gamma=" + NTSCPaletteGenerator.gamma);
                lines.Add("Hue=" + NTSCPaletteGenerator.hue_tweak);
                lines.Add("Saturation=" + NTSCPaletteGenerator.saturation);
                File.WriteAllLines(sav.FileName, lines.ToArray());
            }
        }
        //flat
        private void button6_Click(object sender, EventArgs e)
        {
            hScrollBar_saturation.Value = 1000;
            hScrollBar_hue_tweak.Value = 0;
            hScrollBar_contrast.Value = 1000;
            hScrollBar_brightness.Value = 1000;
            hScrollBar_gamma.Value = 1800;
            ApplyGeneratorSliders();
            UpdatePreview();
        }
        //save as .pal
        private void button7_Click(object sender, EventArgs e)
        {
            SaveFileDialog sav = new SaveFileDialog();
            sav.Filter = Program.ResourceManager.GetString("Filter_PaletteFile");
            if (sav.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                //get pallete
                List<byte> palette = new List<byte>();

                for (int i = 0; i < ((sav.FilterIndex == 1) ? 64 : 512); i++)
                {
                    int color = currentPalette[i];
                    palette.Add((byte)((color >> 16) & 0xFF));//Red
                    palette.Add((byte)((color >> 8) & 0xFF));//Green
                    palette.Add((byte)((color >> 0) & 0xFF));//Blue
                }

                Stream str = new FileStream(sav.FileName, FileMode.Create, FileAccess.Write);
                str.Write(palette.ToArray(), 0, palette.Count);
                str.Close();
            }
        }
        //cancel
        private void button2_Click(object sender, EventArgs e)
        {
            //reset and close
            NTSCPaletteGenerator.brightness = Program.Settings.Palette_NTSC_brightness;
            NTSCPaletteGenerator.contrast = Program.Settings.Palette_NTSC_contrast;
            NTSCPaletteGenerator.gamma = Program.Settings.Palette_NTSC_gamma;
            NTSCPaletteGenerator.hue_tweak = Program.Settings.Palette_NTSC_hue_tweak;
            NTSCPaletteGenerator.saturation = Program.Settings.Palette_NTSC_saturation;

            PALBPaletteGenerator.brightness = Program.Settings.Palette_PALB_brightness;
            PALBPaletteGenerator.contrast = Program.Settings.Palette_PALB_contrast;
            PALBPaletteGenerator.gamma = Program.Settings.Palette_PALB_gamma;
            PALBPaletteGenerator.hue_tweak = Program.Settings.Palette_PALB_hue_tweak;
            PALBPaletteGenerator.saturation = Program.Settings.Palette_PALB_saturation;

            Close();
        }
        //ok
        private void button1_Click(object sender, EventArgs e)
        {
            if (radioButton_usaPaletteFile.Checked)
            {
                if (comboBox1.SelectedIndex <= 0)
                {
                    ManagedMessageBox.ShowErrorMessage(Program.ResourceManager.GetString("Message_PaletteFileNotExistOrSelected"));
                    return;
                }
                if (!File.Exists(Application.StartupPath + "\\Palettes\\" + comboBox1.SelectedItem.ToString()))
                {
                    ManagedMessageBox.ShowErrorMessage(Program.ResourceManager.GetString("Message_PaletteFileNotExistOrSelected"));
                    return;
                }
            }
            SaveSettings();
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }
        // Default
        private void button3_Click(object sender, EventArgs e)
        {
            hScrollBar_brightness.Value = (int)(1.07f * 1000);
            hScrollBar_contrast.Value = (int)(1.2f * 1000);
            hScrollBar_gamma.Value = (int)(1.8f * 1000);
            hScrollBar_hue_tweak.Value = (int)(0 * 1000);
            hScrollBar_saturation.Value = (int)(1.5f * 1000);

            ApplyGeneratorSliders();
            UpdatePreview();
        }
        private void radioButton_useGenerators_CheckedChanged(object sender, EventArgs e)
        {
            UpdatePreview();
            groupBox2.Enabled = radioButton_useGenerators.Checked;
            comboBox1.Enabled = !radioButton_useGenerators.Checked;
            linkLabel1.Enabled = !radioButton_useGenerators.Checked;
        }
        private void radioButton_gen_auto_CheckedChanged(object sender, EventArgs e)
        {
            UpdatePreview();
        }
        private void radioButton_gen_ntsc_CheckedChanged(object sender, EventArgs e)
        {
            UpdatePreview();
        }
        private void radioButton_gen_pal_CheckedChanged(object sender, EventArgs e)
        {
            UpdatePreview();
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdatePreview();
        }
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdatePreview();
        }
        private void radioButton_ntsc_CheckedChanged(object sender, EventArgs e)
        {
            ApplyGeneratorSliders();
            UpdatePreview();
        }
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Filter = Program.ResourceManager.GetString("Filter_PaletteFile");
            if (op.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    if (!File.Exists(Application.StartupPath + "\\Palettes\\" + Path.GetFileName(op.FileName)))
                    {
                        ManagedMessageBoxResult res =
                            ManagedMessageBox.ShowQuestionMessage(Program.ResourceManager.GetString("Message_ReplacePaletteFile"));
                        if (res.ClickedButtonIndex == 0)
                        {
                            File.Delete(Application.StartupPath + "\\Palettes\\" + Path.GetFileName(op.FileName));
                        }
                        else
                            return;
                    }
                    // Copy the file into the palettes file
                    File.Copy(op.FileName, Application.StartupPath + "\\Palettes\\" + Path.GetFileName(op.FileName));
                    // Refresh the combo box
                    RefreshPaletteFilesList();
                    // Select this file
                    comboBox1.SelectedItem = Application.StartupPath + "\\Palettes\\" + Path.GetFileName(op.FileName);
                }
                catch (Exception ex)
                {
                    ManagedMessageBox.ShowErrorMessage(ex.Message);
                }
            }
        }
    }
    public enum PaletteGeneratorUsage
    {
        AUTO, NTSC, PAL
    }
}
