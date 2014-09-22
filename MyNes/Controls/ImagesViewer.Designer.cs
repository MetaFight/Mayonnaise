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
    partial class ImagesViewer
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImagesViewer));
            this.toolStrip_bar = new System.Windows.Forms.ToolStrip();
            this.toolStripButton_add = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_delete = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_deleteAll = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton_openDefaultBrowser = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_openLocation = new System.Windows.Forms.ToolStripButton();
            this.toolStrip_status = new System.Windows.Forms.ToolStrip();
            this.toolStripButton_previous = new System.Windows.Forms.ToolStripButton();
            this.StatusLabel = new System.Windows.Forms.ToolStripLabel();
            this.toolStripButton_next = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSplitButton1 = new System.Windows.Forms.ToolStripSplitButton();
            this.normalstretchIfLargerWithAspectRatioToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.alwaysStretchnoAspectRatioToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.alwaysStretchwithAspectRatioToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.imagePanel1 = new MyNes.ImagePanel();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.previousToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nextToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.addMoreImagesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteSelectedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.openWithWindowsDefaultAppToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openLocationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.imageModeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.normalstretchIfLargerWithAspectRatioToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.alwaysStretchwithAspectRatioToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.alwaysStretchnoAspectRatioToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.showToolbarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showStatusbarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip_bar.SuspendLayout();
            this.toolStrip_status.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip_bar
            // 
            resources.ApplyResources(this.toolStrip_bar, "toolStrip_bar");
            this.toolStrip_bar.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip_bar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton_add,
            this.toolStripButton_delete,
            this.toolStripButton_deleteAll,
            this.toolStripSeparator2,
            this.toolStripButton_openDefaultBrowser,
            this.toolStripButton_openLocation});
            this.toolStrip_bar.Name = "toolStrip_bar";
            this.toolTip1.SetToolTip(this.toolStrip_bar, resources.GetString("toolStrip_bar.ToolTip"));
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
            // toolStripSeparator2
            // 
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            // 
            // toolStripButton_openDefaultBrowser
            // 
            resources.ApplyResources(this.toolStripButton_openDefaultBrowser, "toolStripButton_openDefaultBrowser");
            this.toolStripButton_openDefaultBrowser.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_openDefaultBrowser.Image = global::MyNes.Properties.Resources.image;
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
            // toolStrip_status
            // 
            resources.ApplyResources(this.toolStrip_status, "toolStrip_status");
            this.toolStrip_status.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip_status.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton_previous,
            this.StatusLabel,
            this.toolStripButton_next,
            this.toolStripSeparator3,
            this.toolStripSplitButton1});
            this.toolStrip_status.Name = "toolStrip_status";
            this.toolTip1.SetToolTip(this.toolStrip_status, resources.GetString("toolStrip_status.ToolTip"));
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
            // toolStripSeparator3
            // 
            resources.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            // 
            // toolStripSplitButton1
            // 
            resources.ApplyResources(this.toolStripSplitButton1, "toolStripSplitButton1");
            this.toolStripSplitButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripSplitButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.normalstretchIfLargerWithAspectRatioToolStripMenuItem,
            this.alwaysStretchnoAspectRatioToolStripMenuItem,
            this.alwaysStretchwithAspectRatioToolStripMenuItem});
            this.toolStripSplitButton1.Image = global::MyNes.Properties.Resources.images;
            this.toolStripSplitButton1.Name = "toolStripSplitButton1";
            // 
            // normalstretchIfLargerWithAspectRatioToolStripMenuItem
            // 
            resources.ApplyResources(this.normalstretchIfLargerWithAspectRatioToolStripMenuItem, "normalstretchIfLargerWithAspectRatioToolStripMenuItem");
            this.normalstretchIfLargerWithAspectRatioToolStripMenuItem.Checked = true;
            this.normalstretchIfLargerWithAspectRatioToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.normalstretchIfLargerWithAspectRatioToolStripMenuItem.Name = "normalstretchIfLargerWithAspectRatioToolStripMenuItem";
            this.normalstretchIfLargerWithAspectRatioToolStripMenuItem.Click += new System.EventHandler(this.normalstretchIfLargerWithAspectRatioToolStripMenuItem_Click);
            // 
            // alwaysStretchnoAspectRatioToolStripMenuItem
            // 
            resources.ApplyResources(this.alwaysStretchnoAspectRatioToolStripMenuItem, "alwaysStretchnoAspectRatioToolStripMenuItem");
            this.alwaysStretchnoAspectRatioToolStripMenuItem.Name = "alwaysStretchnoAspectRatioToolStripMenuItem";
            this.alwaysStretchnoAspectRatioToolStripMenuItem.Click += new System.EventHandler(this.alwaysStretchnoAspectRatioToolStripMenuItem_Click);
            // 
            // alwaysStretchwithAspectRatioToolStripMenuItem
            // 
            resources.ApplyResources(this.alwaysStretchwithAspectRatioToolStripMenuItem, "alwaysStretchwithAspectRatioToolStripMenuItem");
            this.alwaysStretchwithAspectRatioToolStripMenuItem.Name = "alwaysStretchwithAspectRatioToolStripMenuItem";
            this.alwaysStretchwithAspectRatioToolStripMenuItem.Click += new System.EventHandler(this.alwaysStretchwithAspectRatioToolStripMenuItem_Click);
            // 
            // imagePanel1
            // 
            resources.ApplyResources(this.imagePanel1, "imagePanel1");
            this.imagePanel1.ContextMenuStrip = this.contextMenuStrip1;
            this.imagePanel1.DefaultImage = global::MyNes.Properties.Resources.MyNesImage;
            this.imagePanel1.ImageToView = null;
            this.imagePanel1.ImageViewMode = MyNes.ImageViewMode.StretchIfLarger;
            this.imagePanel1.Name = "imagePanel1";
            this.toolTip1.SetToolTip(this.imagePanel1, resources.GetString("imagePanel1.ToolTip"));
            this.imagePanel1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ImagesViewer_KeyDown);
            this.imagePanel1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.imagePanel1_MouseDoubleClick);
            // 
            // contextMenuStrip1
            // 
            resources.ApplyResources(this.contextMenuStrip1, "contextMenuStrip1");
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.previousToolStripMenuItem,
            this.nextToolStripMenuItem,
            this.toolStripSeparator1,
            this.addMoreImagesToolStripMenuItem,
            this.deleteSelectedToolStripMenuItem,
            this.deleteAllToolStripMenuItem,
            this.toolStripSeparator4,
            this.openWithWindowsDefaultAppToolStripMenuItem,
            this.openLocationToolStripMenuItem,
            this.toolStripSeparator5,
            this.imageModeToolStripMenuItem,
            this.toolStripSeparator6,
            this.showToolbarToolStripMenuItem,
            this.showStatusbarToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.toolTip1.SetToolTip(this.contextMenuStrip1, resources.GetString("contextMenuStrip1.ToolTip"));
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // previousToolStripMenuItem
            // 
            resources.ApplyResources(this.previousToolStripMenuItem, "previousToolStripMenuItem");
            this.previousToolStripMenuItem.Image = global::MyNes.Properties.Resources.arrow_left;
            this.previousToolStripMenuItem.Name = "previousToolStripMenuItem";
            this.previousToolStripMenuItem.Click += new System.EventHandler(this.toolStripButton_previous_Click);
            // 
            // nextToolStripMenuItem
            // 
            resources.ApplyResources(this.nextToolStripMenuItem, "nextToolStripMenuItem");
            this.nextToolStripMenuItem.Image = global::MyNes.Properties.Resources.arrow_right;
            this.nextToolStripMenuItem.Name = "nextToolStripMenuItem";
            this.nextToolStripMenuItem.Click += new System.EventHandler(this.toolStripButton_next_Click);
            // 
            // toolStripSeparator1
            // 
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            // 
            // addMoreImagesToolStripMenuItem
            // 
            resources.ApplyResources(this.addMoreImagesToolStripMenuItem, "addMoreImagesToolStripMenuItem");
            this.addMoreImagesToolStripMenuItem.Image = global::MyNes.Properties.Resources.add;
            this.addMoreImagesToolStripMenuItem.Name = "addMoreImagesToolStripMenuItem";
            this.addMoreImagesToolStripMenuItem.Click += new System.EventHandler(this.toolStripButton_add_Click);
            // 
            // deleteSelectedToolStripMenuItem
            // 
            resources.ApplyResources(this.deleteSelectedToolStripMenuItem, "deleteSelectedToolStripMenuItem");
            this.deleteSelectedToolStripMenuItem.Image = global::MyNes.Properties.Resources.cross;
            this.deleteSelectedToolStripMenuItem.Name = "deleteSelectedToolStripMenuItem";
            this.deleteSelectedToolStripMenuItem.Click += new System.EventHandler(this.toolStripButton_delete_Click);
            // 
            // deleteAllToolStripMenuItem
            // 
            resources.ApplyResources(this.deleteAllToolStripMenuItem, "deleteAllToolStripMenuItem");
            this.deleteAllToolStripMenuItem.Image = global::MyNes.Properties.Resources.cross_black;
            this.deleteAllToolStripMenuItem.Name = "deleteAllToolStripMenuItem";
            this.deleteAllToolStripMenuItem.Click += new System.EventHandler(this.toolStripButton_deleteAll_Click);
            // 
            // toolStripSeparator4
            // 
            resources.ApplyResources(this.toolStripSeparator4, "toolStripSeparator4");
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            // 
            // openWithWindowsDefaultAppToolStripMenuItem
            // 
            resources.ApplyResources(this.openWithWindowsDefaultAppToolStripMenuItem, "openWithWindowsDefaultAppToolStripMenuItem");
            this.openWithWindowsDefaultAppToolStripMenuItem.Image = global::MyNes.Properties.Resources.image;
            this.openWithWindowsDefaultAppToolStripMenuItem.Name = "openWithWindowsDefaultAppToolStripMenuItem";
            this.openWithWindowsDefaultAppToolStripMenuItem.Click += new System.EventHandler(this.toolStripButton_openDefaultBrowser_Click);
            // 
            // openLocationToolStripMenuItem
            // 
            resources.ApplyResources(this.openLocationToolStripMenuItem, "openLocationToolStripMenuItem");
            this.openLocationToolStripMenuItem.Image = global::MyNes.Properties.Resources.folder;
            this.openLocationToolStripMenuItem.Name = "openLocationToolStripMenuItem";
            this.openLocationToolStripMenuItem.Click += new System.EventHandler(this.toolStripButton_openLocation_Click);
            // 
            // toolStripSeparator5
            // 
            resources.ApplyResources(this.toolStripSeparator5, "toolStripSeparator5");
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            // 
            // imageModeToolStripMenuItem
            // 
            resources.ApplyResources(this.imageModeToolStripMenuItem, "imageModeToolStripMenuItem");
            this.imageModeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.normalstretchIfLargerWithAspectRatioToolStripMenuItem1,
            this.alwaysStretchwithAspectRatioToolStripMenuItem1,
            this.alwaysStretchnoAspectRatioToolStripMenuItem1});
            this.imageModeToolStripMenuItem.Image = global::MyNes.Properties.Resources.images;
            this.imageModeToolStripMenuItem.Name = "imageModeToolStripMenuItem";
            // 
            // normalstretchIfLargerWithAspectRatioToolStripMenuItem1
            // 
            resources.ApplyResources(this.normalstretchIfLargerWithAspectRatioToolStripMenuItem1, "normalstretchIfLargerWithAspectRatioToolStripMenuItem1");
            this.normalstretchIfLargerWithAspectRatioToolStripMenuItem1.Name = "normalstretchIfLargerWithAspectRatioToolStripMenuItem1";
            this.normalstretchIfLargerWithAspectRatioToolStripMenuItem1.Click += new System.EventHandler(this.normalstretchIfLargerWithAspectRatioToolStripMenuItem_Click);
            // 
            // alwaysStretchwithAspectRatioToolStripMenuItem1
            // 
            resources.ApplyResources(this.alwaysStretchwithAspectRatioToolStripMenuItem1, "alwaysStretchwithAspectRatioToolStripMenuItem1");
            this.alwaysStretchwithAspectRatioToolStripMenuItem1.Name = "alwaysStretchwithAspectRatioToolStripMenuItem1";
            this.alwaysStretchwithAspectRatioToolStripMenuItem1.Click += new System.EventHandler(this.alwaysStretchwithAspectRatioToolStripMenuItem_Click);
            // 
            // alwaysStretchnoAspectRatioToolStripMenuItem1
            // 
            resources.ApplyResources(this.alwaysStretchnoAspectRatioToolStripMenuItem1, "alwaysStretchnoAspectRatioToolStripMenuItem1");
            this.alwaysStretchnoAspectRatioToolStripMenuItem1.Name = "alwaysStretchnoAspectRatioToolStripMenuItem1";
            this.alwaysStretchnoAspectRatioToolStripMenuItem1.Click += new System.EventHandler(this.alwaysStretchnoAspectRatioToolStripMenuItem_Click);
            // 
            // toolStripSeparator6
            // 
            resources.ApplyResources(this.toolStripSeparator6, "toolStripSeparator6");
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            // 
            // showToolbarToolStripMenuItem
            // 
            resources.ApplyResources(this.showToolbarToolStripMenuItem, "showToolbarToolStripMenuItem");
            this.showToolbarToolStripMenuItem.Name = "showToolbarToolStripMenuItem";
            this.showToolbarToolStripMenuItem.Click += new System.EventHandler(this.showToolbarToolStripMenuItem_Click);
            // 
            // showStatusbarToolStripMenuItem
            // 
            resources.ApplyResources(this.showStatusbarToolStripMenuItem, "showStatusbarToolStripMenuItem");
            this.showStatusbarToolStripMenuItem.Name = "showStatusbarToolStripMenuItem";
            this.showStatusbarToolStripMenuItem.Click += new System.EventHandler(this.showStatusbarToolStripMenuItem_Click);
            // 
            // ImagesViewer
            // 
            resources.ApplyResources(this, "$this");
            this.AllowDrop = true;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.imagePanel1);
            this.Controls.Add(this.toolStrip_status);
            this.Controls.Add(this.toolStrip_bar);
            this.Name = "ImagesViewer";
            this.toolTip1.SetToolTip(this, resources.GetString("$this.ToolTip"));
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.ImagesViewer_DragDrop);
            this.DragOver += new System.Windows.Forms.DragEventHandler(this.ImagesViewer_DragOver);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ImagesViewer_KeyDown);
            this.toolStrip_bar.ResumeLayout(false);
            this.toolStrip_bar.PerformLayout();
            this.toolStrip_status.ResumeLayout(false);
            this.toolStrip_status.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip_bar;
        private System.Windows.Forms.ToolStripButton toolStripButton_add;
        private System.Windows.Forms.ToolStrip toolStrip_status;
        private ImagePanel imagePanel1;
        private System.Windows.Forms.ToolStripButton toolStripButton_delete;
        private System.Windows.Forms.ToolStripButton toolStripButton_deleteAll;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton toolStripButton_openDefaultBrowser;
        private System.Windows.Forms.ToolStripButton toolStripButton_openLocation;
        private System.Windows.Forms.ToolStripButton toolStripButton_previous;
        private System.Windows.Forms.ToolStripLabel StatusLabel;
        private System.Windows.Forms.ToolStripButton toolStripButton_next;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripSplitButton toolStripSplitButton1;
        private System.Windows.Forms.ToolStripMenuItem normalstretchIfLargerWithAspectRatioToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem alwaysStretchnoAspectRatioToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem alwaysStretchwithAspectRatioToolStripMenuItem;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem previousToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nextToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem addMoreImagesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteSelectedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem openWithWindowsDefaultAppToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openLocationToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem imageModeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem normalstretchIfLargerWithAspectRatioToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem alwaysStretchwithAspectRatioToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem alwaysStretchnoAspectRatioToolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripMenuItem showToolbarToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showStatusbarToolStripMenuItem;
    }
}
