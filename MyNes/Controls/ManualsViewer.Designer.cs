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
    partial class ManualsViewer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ManualsViewer));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton_add = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_delete = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_deleteAll = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton_openDefaultBrowser = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_openLocation = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton_previous = new System.Windows.Forms.ToolStripButton();
            this.StatusLabel = new System.Windows.Forms.ToolStripLabel();
            this.toolStripButton_next = new System.Windows.Forms.ToolStripButton();
            this.label1 = new System.Windows.Forms.Label();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            resources.ApplyResources(this.toolStrip1, "toolStrip1");
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton_add,
            this.toolStripButton_delete,
            this.toolStripButton_deleteAll,
            this.toolStripSeparator1,
            this.toolStripButton_openDefaultBrowser,
            this.toolStripButton_openLocation,
            this.toolStripSeparator2,
            this.toolStripButton_previous,
            this.StatusLabel,
            this.toolStripButton_next});
            this.toolStrip1.Name = "toolStrip1";
            // 
            // toolStripButton_add
            // 
            resources.ApplyResources(this.toolStripButton_add, "toolStripButton_add");
            this.toolStripButton_add.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_add.Image = global::MyNes.Properties.Resources.add;
            this.toolStripButton_add.Name = "toolStripButton_add";
            this.toolStripButton_add.Click += new System.EventHandler(this.toolStripButton_add_Click);
            // 
            // toolStripButton_delete
            // 
            resources.ApplyResources(this.toolStripButton_delete, "toolStripButton_delete");
            this.toolStripButton_delete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_delete.Image = global::MyNes.Properties.Resources.cross;
            this.toolStripButton_delete.Name = "toolStripButton_delete";
            this.toolStripButton_delete.Click += new System.EventHandler(this.toolStripButton_delete_Click);
            // 
            // toolStripButton_deleteAll
            // 
            resources.ApplyResources(this.toolStripButton_deleteAll, "toolStripButton_deleteAll");
            this.toolStripButton_deleteAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_deleteAll.Image = global::MyNes.Properties.Resources.cross_black;
            this.toolStripButton_deleteAll.Name = "toolStripButton_deleteAll";
            this.toolStripButton_deleteAll.Click += new System.EventHandler(this.toolStripButton_deleteAll_Click);
            // 
            // toolStripSeparator1
            // 
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            // 
            // toolStripButton_openDefaultBrowser
            // 
            resources.ApplyResources(this.toolStripButton_openDefaultBrowser, "toolStripButton_openDefaultBrowser");
            this.toolStripButton_openDefaultBrowser.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_openDefaultBrowser.Image = global::MyNes.Properties.Resources.File_PDF;
            this.toolStripButton_openDefaultBrowser.Name = "toolStripButton_openDefaultBrowser";
            this.toolStripButton_openDefaultBrowser.Click += new System.EventHandler(this.toolStripButton_openDefaultBrowser_Click);
            // 
            // toolStripButton_openLocation
            // 
            resources.ApplyResources(this.toolStripButton_openLocation, "toolStripButton_openLocation");
            this.toolStripButton_openLocation.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_openLocation.Image = global::MyNes.Properties.Resources.folder;
            this.toolStripButton_openLocation.Name = "toolStripButton_openLocation";
            this.toolStripButton_openLocation.Click += new System.EventHandler(this.toolStripButton_openLocation_Click);
            // 
            // toolStripSeparator2
            // 
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            // 
            // toolStripButton_previous
            // 
            resources.ApplyResources(this.toolStripButton_previous, "toolStripButton_previous");
            this.toolStripButton_previous.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_previous.Image = global::MyNes.Properties.Resources.arrow_left;
            this.toolStripButton_previous.Name = "toolStripButton_previous";
            this.toolStripButton_previous.Click += new System.EventHandler(this.toolStripButton_previous_Click);
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
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // ManualsViewer
            // 
            resources.ApplyResources(this, "$this");
            this.AllowDrop = true;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::MyNes.Properties.Resources.acrobat_casimir_software;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.toolStrip1);
            this.DoubleBuffered = true;
            this.Name = "ManualsViewer";
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.ManualsViewer_DragDrop);
            this.DragOver += new System.Windows.Forms.DragEventHandler(this.ManualsViewer_DragOver);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton_add;
        private System.Windows.Forms.ToolStripButton toolStripButton_delete;
        private System.Windows.Forms.ToolStripButton toolStripButton_deleteAll;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButton_openDefaultBrowser;
        private System.Windows.Forms.ToolStripButton toolStripButton_openLocation;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton toolStripButton_previous;
        private System.Windows.Forms.ToolStripLabel StatusLabel;
        private System.Windows.Forms.ToolStripButton toolStripButton_next;
        private System.Windows.Forms.Label label1;
    }
}
