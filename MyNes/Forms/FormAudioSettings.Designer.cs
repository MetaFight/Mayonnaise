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
    partial class FormAudioSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAudioSettings));
            this.checkBox_enableSound = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.trackBar2 = new System.Windows.Forms.TrackBar();
            this.trackBar_bufferLength = new System.Windows.Forms.TrackBar();
            this.trackBar_latency = new System.Windows.Forms.TrackBar();
            this.button3 = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label_volume = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label_bufferLength = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label_latency = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_bufferLength)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_latency)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // checkBox_enableSound
            // 
            resources.ApplyResources(this.checkBox_enableSound, "checkBox_enableSound");
            this.checkBox_enableSound.Name = "checkBox_enableSound";
            this.toolTip1.SetToolTip(this.checkBox_enableSound, resources.GetString("checkBox_enableSound.ToolTip"));
            this.checkBox_enableSound.UseVisualStyleBackColor = true;
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
            // trackBar2
            // 
            resources.ApplyResources(this.trackBar2, "trackBar2");
            this.trackBar2.Maximum = 100;
            this.trackBar2.Name = "trackBar2";
            this.toolTip1.SetToolTip(this.trackBar2, resources.GetString("trackBar2.ToolTip"));
            this.trackBar2.Value = 1;
            this.trackBar2.Scroll += new System.EventHandler(this.trackBar2_Scroll);
            // 
            // trackBar_bufferLength
            // 
            resources.ApplyResources(this.trackBar_bufferLength, "trackBar_bufferLength");
            this.trackBar_bufferLength.Minimum = 1;
            this.trackBar_bufferLength.Name = "trackBar_bufferLength";
            this.toolTip1.SetToolTip(this.trackBar_bufferLength, resources.GetString("trackBar_bufferLength.ToolTip"));
            this.trackBar_bufferLength.Value = 2;
            this.trackBar_bufferLength.Scroll += new System.EventHandler(this.trackBar_bufferLength_Scroll);
            // 
            // trackBar_latency
            // 
            resources.ApplyResources(this.trackBar_latency, "trackBar_latency");
            this.trackBar_latency.Minimum = 1;
            this.trackBar_latency.Name = "trackBar_latency";
            this.toolTip1.SetToolTip(this.trackBar_latency, resources.GetString("trackBar_latency.ToolTip"));
            this.trackBar_latency.Value = 2;
            this.trackBar_latency.Scroll += new System.EventHandler(this.trackBar_latency_Scroll);
            // 
            // button3
            // 
            resources.ApplyResources(this.button3, "button3");
            this.button3.Name = "button3";
            this.toolTip1.SetToolTip(this.button3, resources.GetString("button3.ToolTip"));
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // groupBox4
            // 
            resources.ApplyResources(this.groupBox4, "groupBox4");
            this.groupBox4.Controls.Add(this.label_volume);
            this.groupBox4.Controls.Add(this.trackBar2);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.TabStop = false;
            this.toolTip1.SetToolTip(this.groupBox4, resources.GetString("groupBox4.ToolTip"));
            // 
            // label_volume
            // 
            resources.ApplyResources(this.label_volume, "label_volume");
            this.label_volume.Name = "label_volume";
            this.toolTip1.SetToolTip(this.label_volume, resources.GetString("label_volume.ToolTip"));
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.label_bufferLength);
            this.groupBox1.Controls.Add(this.trackBar_bufferLength);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            this.toolTip1.SetToolTip(this.groupBox1, resources.GetString("groupBox1.ToolTip"));
            // 
            // label_bufferLength
            // 
            resources.ApplyResources(this.label_bufferLength, "label_bufferLength");
            this.label_bufferLength.Name = "label_bufferLength";
            this.toolTip1.SetToolTip(this.label_bufferLength, resources.GetString("label_bufferLength.ToolTip"));
            // 
            // groupBox2
            // 
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Controls.Add(this.label_latency);
            this.groupBox2.Controls.Add(this.trackBar_latency);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            this.toolTip1.SetToolTip(this.groupBox2, resources.GetString("groupBox2.ToolTip"));
            // 
            // label_latency
            // 
            resources.ApplyResources(this.label_latency, "label_latency");
            this.label_latency.Name = "label_latency";
            this.toolTip1.SetToolTip(this.label_latency, resources.GetString("label_latency.ToolTip"));
            // 
            // FormAudioSettings
            // 
            this.AcceptButton = this.button1;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.button3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.checkBox_enableSound);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormAudioSettings";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.toolTip1.SetToolTip(this, resources.GetString("$this.ToolTip"));
            ((System.ComponentModel.ISupportInitialize)(this.trackBar2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_bufferLength)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_latency)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBox_enableSound;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label_volume;
        private System.Windows.Forms.TrackBar trackBar2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label_bufferLength;
        private System.Windows.Forms.TrackBar trackBar_bufferLength;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label_latency;
        private System.Windows.Forms.TrackBar trackBar_latency;
        private System.Windows.Forms.Button button3;
    }
}