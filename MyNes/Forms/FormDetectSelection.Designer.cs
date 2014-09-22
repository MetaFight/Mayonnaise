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
    partial class FormDetectSelection
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDetectSelection));
            this.radioButton_snaps = new System.Windows.Forms.RadioButton();
            this.radioButton_covers = new System.Windows.Forms.RadioButton();
            this.radioButton_infos = new System.Windows.Forms.RadioButton();
            this.radioButton_manuals = new System.Windows.Forms.RadioButton();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // radioButton_snaps
            // 
            resources.ApplyResources(this.radioButton_snaps, "radioButton_snaps");
            this.radioButton_snaps.Checked = true;
            this.radioButton_snaps.Name = "radioButton_snaps";
            this.radioButton_snaps.TabStop = true;
            this.radioButton_snaps.UseVisualStyleBackColor = true;
            // 
            // radioButton_covers
            // 
            resources.ApplyResources(this.radioButton_covers, "radioButton_covers");
            this.radioButton_covers.Name = "radioButton_covers";
            this.radioButton_covers.UseVisualStyleBackColor = true;
            // 
            // radioButton_infos
            // 
            resources.ApplyResources(this.radioButton_infos, "radioButton_infos");
            this.radioButton_infos.Name = "radioButton_infos";
            this.radioButton_infos.UseVisualStyleBackColor = true;
            // 
            // radioButton_manuals
            // 
            resources.ApplyResources(this.radioButton_manuals, "radioButton_manuals");
            this.radioButton_manuals.Name = "radioButton_manuals";
            this.radioButton_manuals.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            resources.ApplyResources(this.button1, "button1");
            this.button1.Name = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            resources.ApplyResources(this.button2, "button2");
            this.button2.Name = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // FormDetectSelection
            // 
            this.AcceptButton = this.button1;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.radioButton_manuals);
            this.Controls.Add(this.radioButton_infos);
            this.Controls.Add(this.radioButton_covers);
            this.Controls.Add(this.radioButton_snaps);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormDetectSelection";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton radioButton_snaps;
        private System.Windows.Forms.RadioButton radioButton_covers;
        private System.Windows.Forms.RadioButton radioButton_infos;
        private System.Windows.Forms.RadioButton radioButton_manuals;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}