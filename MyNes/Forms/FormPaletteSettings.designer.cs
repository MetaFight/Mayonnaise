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
namespace MyNes
{
    partial class FormPaletteSettings
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPaletteSettings));
            this.button9 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.hScrollBar_saturation = new System.Windows.Forms.HScrollBar();
            this.label1 = new System.Windows.Forms.Label();
            this.hScrollBar_hue_tweak = new System.Windows.Forms.HScrollBar();
            this.label2 = new System.Windows.Forms.Label();
            this.label_gamma = new System.Windows.Forms.Label();
            this.label_bright = new System.Windows.Forms.Label();
            this.hScrollBar_contrast = new System.Windows.Forms.HScrollBar();
            this.label_const = new System.Windows.Forms.Label();
            this.label_hue = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.hScrollBar_brightness = new System.Windows.Forms.HScrollBar();
            this.label5 = new System.Windows.Forms.Label();
            this.label_satur = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.hScrollBar_gamma = new System.Windows.Forms.HScrollBar();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButton_palb = new System.Windows.Forms.RadioButton();
            this.radioButton_ntsc = new System.Windows.Forms.RadioButton();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.button3 = new System.Windows.Forms.Button();
            this.radioButton_gen_pal = new System.Windows.Forms.RadioButton();
            this.radioButton_gen_ntsc = new System.Windows.Forms.RadioButton();
            this.radioButton_gen_auto = new System.Windows.Forms.RadioButton();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.radioButton_usaPaletteFile = new System.Windows.Forms.RadioButton();
            this.radioButton_useGenerators = new System.Windows.Forms.RadioButton();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // button9
            // 
            resources.ApplyResources(this.button9, "button9");
            this.button9.Name = "button9";
            this.toolTip1.SetToolTip(this.button9, resources.GetString("button9.ToolTip"));
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new System.EventHandler(this.button9_Click);
            // 
            // button7
            // 
            resources.ApplyResources(this.button7, "button7");
            this.button7.Name = "button7";
            this.toolTip1.SetToolTip(this.button7, resources.GetString("button7.ToolTip"));
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // button8
            // 
            resources.ApplyResources(this.button8, "button8");
            this.button8.Name = "button8";
            this.toolTip1.SetToolTip(this.button8, resources.GetString("button8.ToolTip"));
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // button6
            // 
            resources.ApplyResources(this.button6, "button6");
            this.button6.Name = "button6";
            this.toolTip1.SetToolTip(this.button6, resources.GetString("button6.ToolTip"));
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // groupBox4
            // 
            resources.ApplyResources(this.groupBox4, "groupBox4");
            this.groupBox4.Controls.Add(this.hScrollBar_saturation);
            this.groupBox4.Controls.Add(this.label1);
            this.groupBox4.Controls.Add(this.hScrollBar_hue_tweak);
            this.groupBox4.Controls.Add(this.label2);
            this.groupBox4.Controls.Add(this.label_gamma);
            this.groupBox4.Controls.Add(this.label_bright);
            this.groupBox4.Controls.Add(this.hScrollBar_contrast);
            this.groupBox4.Controls.Add(this.label_const);
            this.groupBox4.Controls.Add(this.label_hue);
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Controls.Add(this.hScrollBar_brightness);
            this.groupBox4.Controls.Add(this.label5);
            this.groupBox4.Controls.Add(this.label_satur);
            this.groupBox4.Controls.Add(this.label6);
            this.groupBox4.Controls.Add(this.hScrollBar_gamma);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.TabStop = false;
            this.toolTip1.SetToolTip(this.groupBox4, resources.GetString("groupBox4.ToolTip"));
            // 
            // hScrollBar_saturation
            // 
            resources.ApplyResources(this.hScrollBar_saturation, "hScrollBar_saturation");
            this.hScrollBar_saturation.Maximum = 5000;
            this.hScrollBar_saturation.Name = "hScrollBar_saturation";
            this.toolTip1.SetToolTip(this.hScrollBar_saturation, resources.GetString("hScrollBar_saturation.ToolTip"));
            this.hScrollBar_saturation.Value = 1000;
            this.hScrollBar_saturation.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBar_saturation_Scroll);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            this.toolTip1.SetToolTip(this.label1, resources.GetString("label1.ToolTip"));
            // 
            // hScrollBar_hue_tweak
            // 
            resources.ApplyResources(this.hScrollBar_hue_tweak, "hScrollBar_hue_tweak");
            this.hScrollBar_hue_tweak.Maximum = 1000;
            this.hScrollBar_hue_tweak.Minimum = -1000;
            this.hScrollBar_hue_tweak.Name = "hScrollBar_hue_tweak";
            this.toolTip1.SetToolTip(this.hScrollBar_hue_tweak, resources.GetString("hScrollBar_hue_tweak.ToolTip"));
            this.hScrollBar_hue_tweak.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBar_hue_tweak_Scroll);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            this.toolTip1.SetToolTip(this.label2, resources.GetString("label2.ToolTip"));
            // 
            // label_gamma
            // 
            resources.ApplyResources(this.label_gamma, "label_gamma");
            this.label_gamma.Name = "label_gamma";
            this.toolTip1.SetToolTip(this.label_gamma, resources.GetString("label_gamma.ToolTip"));
            // 
            // label_bright
            // 
            resources.ApplyResources(this.label_bright, "label_bright");
            this.label_bright.Name = "label_bright";
            this.toolTip1.SetToolTip(this.label_bright, resources.GetString("label_bright.ToolTip"));
            // 
            // hScrollBar_contrast
            // 
            resources.ApplyResources(this.hScrollBar_contrast, "hScrollBar_contrast");
            this.hScrollBar_contrast.Maximum = 2000;
            this.hScrollBar_contrast.Minimum = 500;
            this.hScrollBar_contrast.Name = "hScrollBar_contrast";
            this.toolTip1.SetToolTip(this.hScrollBar_contrast, resources.GetString("hScrollBar_contrast.ToolTip"));
            this.hScrollBar_contrast.Value = 1000;
            this.hScrollBar_contrast.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBar_contrast_Scroll);
            // 
            // label_const
            // 
            resources.ApplyResources(this.label_const, "label_const");
            this.label_const.Name = "label_const";
            this.toolTip1.SetToolTip(this.label_const, resources.GetString("label_const.ToolTip"));
            // 
            // label_hue
            // 
            resources.ApplyResources(this.label_hue, "label_hue");
            this.label_hue.Name = "label_hue";
            this.toolTip1.SetToolTip(this.label_hue, resources.GetString("label_hue.ToolTip"));
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            this.toolTip1.SetToolTip(this.label3, resources.GetString("label3.ToolTip"));
            // 
            // hScrollBar_brightness
            // 
            resources.ApplyResources(this.hScrollBar_brightness, "hScrollBar_brightness");
            this.hScrollBar_brightness.Maximum = 2000;
            this.hScrollBar_brightness.Minimum = 500;
            this.hScrollBar_brightness.Name = "hScrollBar_brightness";
            this.toolTip1.SetToolTip(this.hScrollBar_brightness, resources.GetString("hScrollBar_brightness.ToolTip"));
            this.hScrollBar_brightness.Value = 1000;
            this.hScrollBar_brightness.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBar_brightness_Scroll);
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            this.toolTip1.SetToolTip(this.label5, resources.GetString("label5.ToolTip"));
            // 
            // label_satur
            // 
            resources.ApplyResources(this.label_satur, "label_satur");
            this.label_satur.Name = "label_satur";
            this.toolTip1.SetToolTip(this.label_satur, resources.GetString("label_satur.ToolTip"));
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            this.toolTip1.SetToolTip(this.label6, resources.GetString("label6.ToolTip"));
            // 
            // hScrollBar_gamma
            // 
            resources.ApplyResources(this.hScrollBar_gamma, "hScrollBar_gamma");
            this.hScrollBar_gamma.Maximum = 2500;
            this.hScrollBar_gamma.Minimum = 1000;
            this.hScrollBar_gamma.Name = "hScrollBar_gamma";
            this.toolTip1.SetToolTip(this.hScrollBar_gamma, resources.GetString("hScrollBar_gamma.ToolTip"));
            this.hScrollBar_gamma.Value = 1800;
            this.hScrollBar_gamma.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBar_gamma_Scroll);
            // 
            // groupBox3
            // 
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Controls.Add(this.panel1);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            this.toolTip1.SetToolTip(this.groupBox3, resources.GetString("groupBox3.ToolTip"));
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.BackColor = System.Drawing.Color.Black;
            this.panel1.Name = "panel1";
            this.toolTip1.SetToolTip(this.panel1, resources.GetString("panel1.ToolTip"));
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // button1
            // 
            resources.ApplyResources(this.button1, "button1");
            this.button1.Name = "button1";
            this.toolTip1.SetToolTip(this.button1, resources.GetString("button1.ToolTip"));
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            resources.ApplyResources(this.button2, "button2");
            this.button2.Name = "button2";
            this.toolTip1.SetToolTip(this.button2, resources.GetString("button2.ToolTip"));
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.radioButton_palb);
            this.groupBox1.Controls.Add(this.radioButton_ntsc);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            this.toolTip1.SetToolTip(this.groupBox1, resources.GetString("groupBox1.ToolTip"));
            // 
            // radioButton_palb
            // 
            resources.ApplyResources(this.radioButton_palb, "radioButton_palb");
            this.radioButton_palb.Name = "radioButton_palb";
            this.toolTip1.SetToolTip(this.radioButton_palb, resources.GetString("radioButton_palb.ToolTip"));
            this.radioButton_palb.UseVisualStyleBackColor = true;
            // 
            // radioButton_ntsc
            // 
            resources.ApplyResources(this.radioButton_ntsc, "radioButton_ntsc");
            this.radioButton_ntsc.Checked = true;
            this.radioButton_ntsc.Name = "radioButton_ntsc";
            this.radioButton_ntsc.TabStop = true;
            this.toolTip1.SetToolTip(this.radioButton_ntsc, resources.GetString("radioButton_ntsc.ToolTip"));
            this.radioButton_ntsc.UseVisualStyleBackColor = true;
            this.radioButton_ntsc.CheckedChanged += new System.EventHandler(this.radioButton_ntsc_CheckedChanged);
            // 
            // button3
            // 
            resources.ApplyResources(this.button3, "button3");
            this.button3.Name = "button3";
            this.toolTip1.SetToolTip(this.button3, resources.GetString("button3.ToolTip"));
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // radioButton_gen_pal
            // 
            resources.ApplyResources(this.radioButton_gen_pal, "radioButton_gen_pal");
            this.radioButton_gen_pal.Name = "radioButton_gen_pal";
            this.toolTip1.SetToolTip(this.radioButton_gen_pal, resources.GetString("radioButton_gen_pal.ToolTip"));
            this.radioButton_gen_pal.UseVisualStyleBackColor = true;
            this.radioButton_gen_pal.CheckedChanged += new System.EventHandler(this.radioButton_gen_pal_CheckedChanged);
            // 
            // radioButton_gen_ntsc
            // 
            resources.ApplyResources(this.radioButton_gen_ntsc, "radioButton_gen_ntsc");
            this.radioButton_gen_ntsc.Name = "radioButton_gen_ntsc";
            this.toolTip1.SetToolTip(this.radioButton_gen_ntsc, resources.GetString("radioButton_gen_ntsc.ToolTip"));
            this.radioButton_gen_ntsc.UseVisualStyleBackColor = true;
            this.radioButton_gen_ntsc.CheckedChanged += new System.EventHandler(this.radioButton_gen_ntsc_CheckedChanged);
            // 
            // radioButton_gen_auto
            // 
            resources.ApplyResources(this.radioButton_gen_auto, "radioButton_gen_auto");
            this.radioButton_gen_auto.Checked = true;
            this.radioButton_gen_auto.Name = "radioButton_gen_auto";
            this.radioButton_gen_auto.TabStop = true;
            this.toolTip1.SetToolTip(this.radioButton_gen_auto, resources.GetString("radioButton_gen_auto.ToolTip"));
            this.radioButton_gen_auto.UseVisualStyleBackColor = true;
            this.radioButton_gen_auto.CheckedChanged += new System.EventHandler(this.radioButton_gen_auto_CheckedChanged);
            // 
            // comboBox1
            // 
            resources.ApplyResources(this.comboBox1, "comboBox1");
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Name = "comboBox1";
            this.toolTip1.SetToolTip(this.comboBox1, resources.GetString("comboBox1.ToolTip"));
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // radioButton_usaPaletteFile
            // 
            resources.ApplyResources(this.radioButton_usaPaletteFile, "radioButton_usaPaletteFile");
            this.radioButton_usaPaletteFile.Name = "radioButton_usaPaletteFile";
            this.toolTip1.SetToolTip(this.radioButton_usaPaletteFile, resources.GetString("radioButton_usaPaletteFile.ToolTip"));
            this.radioButton_usaPaletteFile.UseVisualStyleBackColor = true;
            // 
            // radioButton_useGenerators
            // 
            resources.ApplyResources(this.radioButton_useGenerators, "radioButton_useGenerators");
            this.radioButton_useGenerators.Checked = true;
            this.radioButton_useGenerators.Name = "radioButton_useGenerators";
            this.radioButton_useGenerators.TabStop = true;
            this.toolTip1.SetToolTip(this.radioButton_useGenerators, resources.GetString("radioButton_useGenerators.ToolTip"));
            this.radioButton_useGenerators.UseVisualStyleBackColor = true;
            this.radioButton_useGenerators.CheckedChanged += new System.EventHandler(this.radioButton_useGenerators_CheckedChanged);
            // 
            // tabControl1
            // 
            resources.ApplyResources(this.tabControl1, "tabControl1");
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.toolTip1.SetToolTip(this.tabControl1, resources.GetString("tabControl1.ToolTip"));
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            resources.ApplyResources(this.tabPage1, "tabPage1");
            this.tabPage1.Controls.Add(this.linkLabel1);
            this.tabPage1.Controls.Add(this.comboBox1);
            this.tabPage1.Controls.Add(this.radioButton_usaPaletteFile);
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Controls.Add(this.radioButton_useGenerators);
            this.tabPage1.Name = "tabPage1";
            this.toolTip1.SetToolTip(this.tabPage1, resources.GetString("tabPage1.ToolTip"));
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // linkLabel1
            // 
            resources.ApplyResources(this.linkLabel1, "linkLabel1");
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.TabStop = true;
            this.toolTip1.SetToolTip(this.linkLabel1, resources.GetString("linkLabel1.ToolTip"));
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // groupBox2
            // 
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Controls.Add(this.radioButton_gen_pal);
            this.groupBox2.Controls.Add(this.radioButton_gen_ntsc);
            this.groupBox2.Controls.Add(this.radioButton_gen_auto);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            this.toolTip1.SetToolTip(this.groupBox2, resources.GetString("groupBox2.ToolTip"));
            // 
            // tabPage2
            // 
            resources.ApplyResources(this.tabPage2, "tabPage2");
            this.tabPage2.Controls.Add(this.groupBox1);
            this.tabPage2.Controls.Add(this.button3);
            this.tabPage2.Controls.Add(this.button6);
            this.tabPage2.Controls.Add(this.groupBox4);
            this.tabPage2.Controls.Add(this.button8);
            this.tabPage2.Controls.Add(this.button9);
            this.tabPage2.Name = "tabPage2";
            this.toolTip1.SetToolTip(this.tabPage2, resources.GetString("tabPage2.ToolTip"));
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // FormPaletteSettings
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ControlBox = false;
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.button7);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FormPaletteSettings";
            this.toolTip1.SetToolTip(this, resources.GetString("$this.ToolTip"));
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label_gamma;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label_bright;
        private System.Windows.Forms.Label label_const;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label_hue;
        private System.Windows.Forms.Label label_satur;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButton_palb;
        private System.Windows.Forms.RadioButton radioButton_ntsc;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton radioButton_gen_pal;
        private System.Windows.Forms.RadioButton radioButton_gen_ntsc;
        private System.Windows.Forms.RadioButton radioButton_gen_auto;
        private System.Windows.Forms.RadioButton radioButton_useGenerators;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.RadioButton radioButton_usaPaletteFile;
        private System.Windows.Forms.HScrollBar hScrollBar_saturation;
        private System.Windows.Forms.HScrollBar hScrollBar_hue_tweak;
        private System.Windows.Forms.HScrollBar hScrollBar_contrast;
        private System.Windows.Forms.HScrollBar hScrollBar_brightness;
        private System.Windows.Forms.HScrollBar hScrollBar_gamma;
    }
}