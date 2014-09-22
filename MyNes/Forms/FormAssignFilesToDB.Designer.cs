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
    partial class FormAssignFilesToDB
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAssignFilesToDB));
            this.label1 = new System.Windows.Forms.Label();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.checkBox_subfolders = new System.Windows.Forms.CheckBox();
            this.checkBox_addFilesNotFoundInDB = new System.Windows.Forms.CheckBox();
            this.checkBox_updateAlreadyAssigned = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // listBox1
            // 
            resources.ApplyResources(this.listBox1, "listBox1");
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Name = "listBox1";
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
            // button3
            // 
            resources.ApplyResources(this.button3, "button3");
            this.button3.Name = "button3";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            resources.ApplyResources(this.button4, "button4");
            this.button4.Name = "button4";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // checkBox_subfolders
            // 
            resources.ApplyResources(this.checkBox_subfolders, "checkBox_subfolders");
            this.checkBox_subfolders.Checked = true;
            this.checkBox_subfolders.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_subfolders.Name = "checkBox_subfolders";
            this.checkBox_subfolders.UseVisualStyleBackColor = true;
            // 
            // checkBox_addFilesNotFoundInDB
            // 
            resources.ApplyResources(this.checkBox_addFilesNotFoundInDB, "checkBox_addFilesNotFoundInDB");
            this.checkBox_addFilesNotFoundInDB.Checked = true;
            this.checkBox_addFilesNotFoundInDB.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_addFilesNotFoundInDB.Name = "checkBox_addFilesNotFoundInDB";
            this.checkBox_addFilesNotFoundInDB.UseVisualStyleBackColor = true;
            // 
            // checkBox_updateAlreadyAssigned
            // 
            resources.ApplyResources(this.checkBox_updateAlreadyAssigned, "checkBox_updateAlreadyAssigned");
            this.checkBox_updateAlreadyAssigned.Name = "checkBox_updateAlreadyAssigned";
            this.checkBox_updateAlreadyAssigned.UseVisualStyleBackColor = true;
            // 
            // FormAssignFilesToDB
            // 
            this.AcceptButton = this.button3;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.checkBox_updateAlreadyAssigned);
            this.Controls.Add(this.checkBox_addFilesNotFoundInDB);
            this.Controls.Add(this.checkBox_subfolders);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormAssignFilesToDB";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Load += new System.EventHandler(this.FormAssignFilesToDB_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.CheckBox checkBox_subfolders;
        private System.Windows.Forms.CheckBox checkBox_addFilesNotFoundInDB;
        private System.Windows.Forms.CheckBox checkBox_updateAlreadyAssigned;
    }
}