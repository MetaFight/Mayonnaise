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
    partial class FormPreferences
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPreferences));
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.checkBox_saveSramAtEmuShutdown = new System.Windows.Forms.CheckBox();
            this.button3 = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.checkBox_tryRunNotSupported = new System.Windows.Forms.CheckBox();
            this.checkBox_showKnownIssues = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox_snapFormat = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
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
            // checkBox_saveSramAtEmuShutdown
            // 
            resources.ApplyResources(this.checkBox_saveSramAtEmuShutdown, "checkBox_saveSramAtEmuShutdown");
            this.checkBox_saveSramAtEmuShutdown.Name = "checkBox_saveSramAtEmuShutdown";
            this.toolTip1.SetToolTip(this.checkBox_saveSramAtEmuShutdown, resources.GetString("checkBox_saveSramAtEmuShutdown.ToolTip"));
            this.checkBox_saveSramAtEmuShutdown.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            resources.ApplyResources(this.button3, "button3");
            this.button3.Name = "button3";
            this.toolTip1.SetToolTip(this.button3, resources.GetString("button3.ToolTip"));
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // checkBox1
            // 
            resources.ApplyResources(this.checkBox1, "checkBox1");
            this.checkBox1.Name = "checkBox1";
            this.toolTip1.SetToolTip(this.checkBox1, resources.GetString("checkBox1.ToolTip"));
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // checkBox_tryRunNotSupported
            // 
            resources.ApplyResources(this.checkBox_tryRunNotSupported, "checkBox_tryRunNotSupported");
            this.checkBox_tryRunNotSupported.Name = "checkBox_tryRunNotSupported";
            this.toolTip1.SetToolTip(this.checkBox_tryRunNotSupported, resources.GetString("checkBox_tryRunNotSupported.ToolTip"));
            this.checkBox_tryRunNotSupported.UseVisualStyleBackColor = true;
            // 
            // checkBox_showKnownIssues
            // 
            resources.ApplyResources(this.checkBox_showKnownIssues, "checkBox_showKnownIssues");
            this.checkBox_showKnownIssues.Name = "checkBox_showKnownIssues";
            this.toolTip1.SetToolTip(this.checkBox_showKnownIssues, resources.GetString("checkBox_showKnownIssues.ToolTip"));
            this.checkBox_showKnownIssues.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // comboBox_snapFormat
            // 
            this.comboBox_snapFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_snapFormat.FormattingEnabled = true;
            this.comboBox_snapFormat.Items.AddRange(new object[] {
            resources.GetString("comboBox_snapFormat.Items"),
            resources.GetString("comboBox_snapFormat.Items1"),
            resources.GetString("comboBox_snapFormat.Items2"),
            resources.GetString("comboBox_snapFormat.Items3"),
            resources.GetString("comboBox_snapFormat.Items4"),
            resources.GetString("comboBox_snapFormat.Items5"),
            resources.GetString("comboBox_snapFormat.Items6"),
            resources.GetString("comboBox_snapFormat.Items7")});
            resources.ApplyResources(this.comboBox_snapFormat, "comboBox_snapFormat");
            this.comboBox_snapFormat.Name = "comboBox_snapFormat";
            this.toolTip1.SetToolTip(this.comboBox_snapFormat, resources.GetString("comboBox_snapFormat.ToolTip"));
            // 
            // FormPreferences
            // 
            this.AcceptButton = this.button1;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.checkBox_showKnownIssues);
            this.Controls.Add(this.checkBox_tryRunNotSupported);
            this.Controls.Add(this.comboBox_snapFormat);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.checkBox_saveSramAtEmuShutdown);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormPreferences";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.CheckBox checkBox_saveSramAtEmuShutdown;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox_snapFormat;
        private System.Windows.Forms.CheckBox checkBox_tryRunNotSupported;
        private System.Windows.Forms.CheckBox checkBox_showKnownIssues;
    }
}