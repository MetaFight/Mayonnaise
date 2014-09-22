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
    partial class InfoViewer
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InfoViewer));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton_newFile = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_SaveChanges = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton_addMoreFiles = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_deleteSelected = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_deleteAll = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton_edit = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_openLocation = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton_prevous = new System.Windows.Forms.ToolStripButton();
            this.StatusLabel = new System.Windows.Forms.ToolStripLabel();
            this.toolStripButton_next = new System.Windows.Forms.ToolStripButton();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            resources.ApplyResources(this.toolStrip1, "toolStrip1");
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton_newFile,
            this.toolStripButton_SaveChanges,
            this.toolStripSeparator1,
            this.toolStripButton_addMoreFiles,
            this.toolStripButton_deleteSelected,
            this.toolStripButton_deleteAll,
            this.toolStripSeparator2,
            this.toolStripButton_edit,
            this.toolStripButton_openLocation,
            this.toolStripSeparator3,
            this.toolStripButton_prevous,
            this.StatusLabel,
            this.toolStripButton_next});
            this.toolStrip1.Name = "toolStrip1";
            // 
            // toolStripButton_newFile
            // 
            resources.ApplyResources(this.toolStripButton_newFile, "toolStripButton_newFile");
            this.toolStripButton_newFile.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_newFile.Image = global::MyNes.Properties.Resources.page_white;
            this.toolStripButton_newFile.Name = "toolStripButton_newFile";
            this.toolStripButton_newFile.Click += new System.EventHandler(this.toolStripButton_newFile_Click);
            // 
            // toolStripButton_SaveChanges
            // 
            resources.ApplyResources(this.toolStripButton_SaveChanges, "toolStripButton_SaveChanges");
            this.toolStripButton_SaveChanges.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_SaveChanges.Image = global::MyNes.Properties.Resources.disk;
            this.toolStripButton_SaveChanges.Name = "toolStripButton_SaveChanges";
            this.toolStripButton_SaveChanges.Click += new System.EventHandler(this.toolStripButton_SaveChanges_Click);
            // 
            // toolStripSeparator1
            // 
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            // 
            // toolStripButton_addMoreFiles
            // 
            resources.ApplyResources(this.toolStripButton_addMoreFiles, "toolStripButton_addMoreFiles");
            this.toolStripButton_addMoreFiles.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_addMoreFiles.Image = global::MyNes.Properties.Resources.add;
            this.toolStripButton_addMoreFiles.Name = "toolStripButton_addMoreFiles";
            this.toolStripButton_addMoreFiles.Click += new System.EventHandler(this.toolStripButton_addMoreFiles_Click);
            // 
            // toolStripButton_deleteSelected
            // 
            resources.ApplyResources(this.toolStripButton_deleteSelected, "toolStripButton_deleteSelected");
            this.toolStripButton_deleteSelected.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_deleteSelected.Image = global::MyNes.Properties.Resources.cross;
            this.toolStripButton_deleteSelected.Name = "toolStripButton_deleteSelected";
            this.toolStripButton_deleteSelected.Click += new System.EventHandler(this.toolStripButton_deleteSelected_Click);
            // 
            // toolStripButton_deleteAll
            // 
            resources.ApplyResources(this.toolStripButton_deleteAll, "toolStripButton_deleteAll");
            this.toolStripButton_deleteAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_deleteAll.Image = global::MyNes.Properties.Resources.cross_black;
            this.toolStripButton_deleteAll.Name = "toolStripButton_deleteAll";
            this.toolStripButton_deleteAll.Click += new System.EventHandler(this.toolStripButton_deleteAll_Click);
            // 
            // toolStripSeparator2
            // 
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            // 
            // toolStripButton_edit
            // 
            resources.ApplyResources(this.toolStripButton_edit, "toolStripButton_edit");
            this.toolStripButton_edit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_edit.Image = global::MyNes.Properties.Resources.note_edit;
            this.toolStripButton_edit.Name = "toolStripButton_edit";
            this.toolStripButton_edit.Click += new System.EventHandler(this.toolStripButton_edit_Click);
            // 
            // toolStripButton_openLocation
            // 
            resources.ApplyResources(this.toolStripButton_openLocation, "toolStripButton_openLocation");
            this.toolStripButton_openLocation.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_openLocation.Image = global::MyNes.Properties.Resources.folder;
            this.toolStripButton_openLocation.Name = "toolStripButton_openLocation";
            this.toolStripButton_openLocation.Click += new System.EventHandler(this.toolStripButton_openLocation_Click);
            // 
            // toolStripSeparator3
            // 
            resources.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            // 
            // toolStripButton_prevous
            // 
            resources.ApplyResources(this.toolStripButton_prevous, "toolStripButton_prevous");
            this.toolStripButton_prevous.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_prevous.Image = global::MyNes.Properties.Resources.arrow_left;
            this.toolStripButton_prevous.Name = "toolStripButton_prevous";
            this.toolStripButton_prevous.Click += new System.EventHandler(this.toolStripButton_prevous_Click);
            // 
            // StatusLabel
            // 
            resources.ApplyResources(this.StatusLabel, "StatusLabel");
            this.StatusLabel.Name = "StatusLabel";
            // 
            // toolStripButton_next
            // 
            resources.ApplyResources(this.toolStripButton_next, "toolStripButton_next");
            this.toolStripButton_next.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_next.Image = global::MyNes.Properties.Resources.arrow_right;
            this.toolStripButton_next.Name = "toolStripButton_next";
            this.toolStripButton_next.Click += new System.EventHandler(this.toolStripButton_next_Click);
            // 
            // richTextBox1
            // 
            resources.ApplyResources(this.richTextBox1, "richTextBox1");
            this.richTextBox1.Name = "richTextBox1";
            // 
            // InfoViewer
            // 
            resources.ApplyResources(this, "$this");
            this.AllowDrop = true;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "InfoViewer";
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.InfoViewer_DragDrop);
            this.DragOver += new System.Windows.Forms.DragEventHandler(this.InfoViewer_DragOver);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton_newFile;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.ToolStripButton toolStripButton_SaveChanges;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButton_addMoreFiles;
        private System.Windows.Forms.ToolStripButton toolStripButton_deleteSelected;
        private System.Windows.Forms.ToolStripButton toolStripButton_deleteAll;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton toolStripButton_edit;
        private System.Windows.Forms.ToolStripButton toolStripButton_openLocation;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton toolStripButton_prevous;
        private System.Windows.Forms.ToolStripLabel StatusLabel;
        private System.Windows.Forms.ToolStripButton toolStripButton_next;
    }
}
