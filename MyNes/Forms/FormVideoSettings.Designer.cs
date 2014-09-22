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
    partial class FormVideoSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormVideoSettings));
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.comboBox_windowedModeSize = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.checkBox_fullscreen = new System.Windows.Forms.CheckBox();
            this.comboBox_fullscreenRes = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.checkBox_showNot = new System.Windows.Forms.CheckBox();
            this.checkBox_showFPS = new System.Windows.Forms.CheckBox();
            this.checkBox_cutLines = new System.Windows.Forms.CheckBox();
            this.checkBox_hardware_vertex_processing = new System.Windows.Forms.CheckBox();
            this.checkBox_keep_aspect_ratio = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBox_filter = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // checkBox1
            // 
            resources.ApplyResources(this.checkBox1, "checkBox1");
            this.checkBox1.Name = "checkBox1";
            this.toolTip1.SetToolTip(this.checkBox1, resources.GetString("checkBox1.ToolTip"));
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // comboBox_windowedModeSize
            // 
            resources.ApplyResources(this.comboBox_windowedModeSize, "comboBox_windowedModeSize");
            this.comboBox_windowedModeSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_windowedModeSize.FormattingEnabled = true;
            this.comboBox_windowedModeSize.Items.AddRange(new object[] {
            resources.GetString("comboBox_windowedModeSize.Items"),
            resources.GetString("comboBox_windowedModeSize.Items1"),
            resources.GetString("comboBox_windowedModeSize.Items2"),
            resources.GetString("comboBox_windowedModeSize.Items3"),
            resources.GetString("comboBox_windowedModeSize.Items4")});
            this.comboBox_windowedModeSize.Name = "comboBox_windowedModeSize";
            this.toolTip1.SetToolTip(this.comboBox_windowedModeSize, resources.GetString("comboBox_windowedModeSize.ToolTip"));
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
            // checkBox_fullscreen
            // 
            resources.ApplyResources(this.checkBox_fullscreen, "checkBox_fullscreen");
            this.checkBox_fullscreen.Name = "checkBox_fullscreen";
            this.toolTip1.SetToolTip(this.checkBox_fullscreen, resources.GetString("checkBox_fullscreen.ToolTip"));
            this.checkBox_fullscreen.UseVisualStyleBackColor = true;
            // 
            // comboBox_fullscreenRes
            // 
            resources.ApplyResources(this.comboBox_fullscreenRes, "comboBox_fullscreenRes");
            this.comboBox_fullscreenRes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_fullscreenRes.FormattingEnabled = true;
            this.comboBox_fullscreenRes.Name = "comboBox_fullscreenRes";
            this.toolTip1.SetToolTip(this.comboBox_fullscreenRes, resources.GetString("comboBox_fullscreenRes.ToolTip"));
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            this.toolTip1.SetToolTip(this.label1, resources.GetString("label1.ToolTip"));
            // 
            // checkBox_showNot
            // 
            resources.ApplyResources(this.checkBox_showNot, "checkBox_showNot");
            this.checkBox_showNot.Name = "checkBox_showNot";
            this.toolTip1.SetToolTip(this.checkBox_showNot, resources.GetString("checkBox_showNot.ToolTip"));
            this.checkBox_showNot.UseVisualStyleBackColor = true;
            // 
            // checkBox_showFPS
            // 
            resources.ApplyResources(this.checkBox_showFPS, "checkBox_showFPS");
            this.checkBox_showFPS.Name = "checkBox_showFPS";
            this.toolTip1.SetToolTip(this.checkBox_showFPS, resources.GetString("checkBox_showFPS.ToolTip"));
            this.checkBox_showFPS.UseVisualStyleBackColor = true;
            // 
            // checkBox_cutLines
            // 
            resources.ApplyResources(this.checkBox_cutLines, "checkBox_cutLines");
            this.checkBox_cutLines.Name = "checkBox_cutLines";
            this.toolTip1.SetToolTip(this.checkBox_cutLines, resources.GetString("checkBox_cutLines.ToolTip"));
            this.checkBox_cutLines.UseVisualStyleBackColor = true;
            // 
            // checkBox_hardware_vertex_processing
            // 
            resources.ApplyResources(this.checkBox_hardware_vertex_processing, "checkBox_hardware_vertex_processing");
            this.checkBox_hardware_vertex_processing.Name = "checkBox_hardware_vertex_processing";
            this.toolTip1.SetToolTip(this.checkBox_hardware_vertex_processing, resources.GetString("checkBox_hardware_vertex_processing.ToolTip"));
            this.checkBox_hardware_vertex_processing.UseVisualStyleBackColor = true;
            // 
            // checkBox_keep_aspect_ratio
            // 
            resources.ApplyResources(this.checkBox_keep_aspect_ratio, "checkBox_keep_aspect_ratio");
            this.checkBox_keep_aspect_ratio.Name = "checkBox_keep_aspect_ratio";
            this.toolTip1.SetToolTip(this.checkBox_keep_aspect_ratio, resources.GetString("checkBox_keep_aspect_ratio.ToolTip"));
            this.checkBox_keep_aspect_ratio.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            this.toolTip1.SetToolTip(this.label2, resources.GetString("label2.ToolTip"));
            // 
            // comboBox_filter
            // 
            resources.ApplyResources(this.comboBox_filter, "comboBox_filter");
            this.comboBox_filter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_filter.FormattingEnabled = true;
            this.comboBox_filter.Items.AddRange(new object[] {
            resources.GetString("comboBox_filter.Items"),
            resources.GetString("comboBox_filter.Items1"),
            resources.GetString("comboBox_filter.Items2")});
            this.comboBox_filter.Name = "comboBox_filter";
            this.toolTip1.SetToolTip(this.comboBox_filter, resources.GetString("comboBox_filter.ToolTip"));
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            this.toolTip1.SetToolTip(this.label3, resources.GetString("label3.ToolTip"));
            // 
            // button3
            // 
            resources.ApplyResources(this.button3, "button3");
            this.button3.Name = "button3";
            this.toolTip1.SetToolTip(this.button3, resources.GetString("button3.ToolTip"));
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // FormVideoSettings
            // 
            this.AcceptButton = this.button1;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.button3);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.comboBox_filter);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.checkBox_keep_aspect_ratio);
            this.Controls.Add(this.checkBox_hardware_vertex_processing);
            this.Controls.Add(this.checkBox_cutLines);
            this.Controls.Add(this.checkBox_showFPS);
            this.Controls.Add(this.checkBox_showNot);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBox_fullscreenRes);
            this.Controls.Add(this.checkBox_fullscreen);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.comboBox_windowedModeSize);
            this.Controls.Add(this.checkBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormVideoSettings";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.toolTip1.SetToolTip(this, resources.GetString("$this.ToolTip"));
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.ComboBox comboBox_windowedModeSize;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.CheckBox checkBox_fullscreen;
        private System.Windows.Forms.ComboBox comboBox_fullscreenRes;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBox_showNot;
        private System.Windows.Forms.CheckBox checkBox_showFPS;
        private System.Windows.Forms.CheckBox checkBox_cutLines;
        private System.Windows.Forms.CheckBox checkBox_hardware_vertex_processing;
        private System.Windows.Forms.CheckBox checkBox_keep_aspect_ratio;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBox_filter;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}