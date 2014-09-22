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
    partial class FormLauncher
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormLauncher));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.managedListView1 = new MLV.ManagedListView();
            this.contextMenuStrip_list_main = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.playToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.infoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.openFileLocationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList_items = new System.Windows.Forms.ImageList(this.components);
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage_game = new System.Windows.Forms.TabPage();
            this.tabPage_snapshots = new System.Windows.Forms.TabPage();
            this.tabPage_covers = new System.Windows.Forms.TabPage();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage_info = new System.Windows.Forms.TabPage();
            this.tabPage_manuals = new System.Windows.Forms.TabPage();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton7 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton5 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
            this.toolStripTextBox_find = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSplitButton1 = new System.Windows.Forms.ToolStripSplitButton();
            this.autoMinimizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cycleImagesOnGameInfoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rememberSelectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showLauncherAtAppStartToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripButton6 = new System.Windows.Forms.ToolStripButton();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.StatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.ProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.timer_play = new System.Windows.Forms.Timer(this.components);
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.Button_mode_all = new System.Windows.Forms.ToolStripButton();
            this.Button_mode_database = new System.Windows.Forms.ToolStripButton();
            this.Button_mode_notDB = new System.Windows.Forms.ToolStripButton();
            this.Button_mode_files = new System.Windows.Forms.ToolStripButton();
            this.Button_mode_missing = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton8 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton9 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.timer_search = new System.Windows.Forms.Timer(this.components);
            this.contextMenuStrip_list_columns = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.timer_progress = new System.Windows.Forms.Timer(this.components);
            this.timer_selection = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.contextMenuStrip_list_main.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            resources.ApplyResources(this.splitContainer1, "splitContainer1");
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            resources.ApplyResources(this.splitContainer1.Panel1, "splitContainer1.Panel1");
            this.splitContainer1.Panel1.Controls.Add(this.managedListView1);
            // 
            // splitContainer1.Panel2
            // 
            resources.ApplyResources(this.splitContainer1.Panel2, "splitContainer1.Panel2");
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            // 
            // managedListView1
            // 
            resources.ApplyResources(this.managedListView1, "managedListView1");
            this.managedListView1.AllowColumnsReorder = true;
            this.managedListView1.AllowDrop = true;
            this.managedListView1.AllowItemsDragAndDrop = false;
            this.managedListView1.AutoSetWheelScrollSpeed = true;
            this.managedListView1.BackgroundRenderMode = MLV.ManagedListViewBackgroundRenderMode.NormalStretchNoAspectRatio;
            this.managedListView1.ChangeColumnSortModeWhenClick = false;
            this.managedListView1.ColumnClickColor = System.Drawing.Color.PaleVioletRed;
            this.managedListView1.ColumnColor = System.Drawing.Color.Silver;
            this.managedListView1.ColumnHighlightColor = System.Drawing.Color.LightSkyBlue;
            this.managedListView1.ContextMenuStrip = this.contextMenuStrip_list_main;
            this.managedListView1.DrawHighlight = true;
            this.managedListView1.ImagesList = this.imageList_items;
            this.managedListView1.ItemHighlightColor = System.Drawing.Color.LightSkyBlue;
            this.managedListView1.ItemMouseOverColor = System.Drawing.Color.LightGray;
            this.managedListView1.ItemSpecialColor = System.Drawing.Color.YellowGreen;
            this.managedListView1.Name = "managedListView1";
            this.managedListView1.ShowSubItemToolTip = true;
            this.managedListView1.StretchThumbnailsToFit = false;
            this.managedListView1.ThunmbnailsHeight = 36;
            this.managedListView1.ThunmbnailsWidth = 36;
            this.managedListView1.ViewMode = MLV.ManagedListViewViewMode.Details;
            this.managedListView1.WheelScrollSpeed = 18;
            this.managedListView1.SelectedIndexChanged += new System.EventHandler(this.managedListView1_SelectedIndexChanged);
            this.managedListView1.ColumnClicked += new System.EventHandler<MLV.ManagedListViewColumnClickArgs>(this.managedListView1_ColumnClicked);
            this.managedListView1.EnterPressed += new System.EventHandler(this.managedListView1_EnterPressed);
            this.managedListView1.SwitchToColumnsContextMenu += new System.EventHandler(this.managedListView1_SwitchToColumnsContextMenu);
            this.managedListView1.SwitchToNormalContextMenu += new System.EventHandler(this.managedListView1_SwitchToNormalContextMenu);
            this.managedListView1.AfterColumnResize += new System.EventHandler(this.managedListView1_AfterColumnResize);
            this.managedListView1.AfterColumnReorder += new System.EventHandler(this.managedListView1_AfterColumnReorder);
            this.managedListView1.DragDrop += new System.Windows.Forms.DragEventHandler(this.managedListView1_DragDrop);
            this.managedListView1.DragOver += new System.Windows.Forms.DragEventHandler(this.managedListView1_DragOver);
            this.managedListView1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.managedListView1_MouseDoubleClick);
            // 
            // contextMenuStrip_list_main
            // 
            resources.ApplyResources(this.contextMenuStrip_list_main, "contextMenuStrip_list_main");
            this.contextMenuStrip_list_main.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.playToolStripMenuItem,
            this.toolStripSeparator3,
            this.deleteToolStripMenuItem,
            this.toolStripSeparator9,
            this.infoToolStripMenuItem,
            this.toolStripSeparator8,
            this.openFileLocationToolStripMenuItem});
            this.contextMenuStrip_list_main.Name = "contextMenuStrip_list_main";
            // 
            // playToolStripMenuItem
            // 
            resources.ApplyResources(this.playToolStripMenuItem, "playToolStripMenuItem");
            this.playToolStripMenuItem.Image = global::MyNes.Properties.Resources.control_play;
            this.playToolStripMenuItem.Name = "playToolStripMenuItem";
            // 
            // toolStripSeparator3
            // 
            resources.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            // 
            // deleteToolStripMenuItem
            // 
            resources.ApplyResources(this.deleteToolStripMenuItem, "deleteToolStripMenuItem");
            this.deleteToolStripMenuItem.Image = global::MyNes.Properties.Resources.cross_black;
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.DeleteSelectedEntries);
            // 
            // toolStripSeparator9
            // 
            resources.ApplyResources(this.toolStripSeparator9, "toolStripSeparator9");
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            // 
            // infoToolStripMenuItem
            // 
            resources.ApplyResources(this.infoToolStripMenuItem, "infoToolStripMenuItem");
            this.infoToolStripMenuItem.Image = global::MyNes.Properties.Resources.information;
            this.infoToolStripMenuItem.Name = "infoToolStripMenuItem";
            this.infoToolStripMenuItem.Click += new System.EventHandler(this.ShowGameInfo);
            // 
            // toolStripSeparator8
            // 
            resources.ApplyResources(this.toolStripSeparator8, "toolStripSeparator8");
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            // 
            // openFileLocationToolStripMenuItem
            // 
            resources.ApplyResources(this.openFileLocationToolStripMenuItem, "openFileLocationToolStripMenuItem");
            this.openFileLocationToolStripMenuItem.Name = "openFileLocationToolStripMenuItem";
            this.openFileLocationToolStripMenuItem.Click += new System.EventHandler(this.openFileLocationToolStripMenuItem_Click);
            // 
            // imageList_items
            // 
            this.imageList_items.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList_items.ImageStream")));
            this.imageList_items.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList_items.Images.SetKeyName(0, "cross.png");
            this.imageList_items.Images.SetKeyName(1, "INES.ico");
            // 
            // splitContainer2
            // 
            resources.ApplyResources(this.splitContainer2, "splitContainer2");
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            resources.ApplyResources(this.splitContainer2.Panel1, "splitContainer2.Panel1");
            this.splitContainer2.Panel1.Controls.Add(this.tabControl1);
            // 
            // splitContainer2.Panel2
            // 
            resources.ApplyResources(this.splitContainer2.Panel2, "splitContainer2.Panel2");
            this.splitContainer2.Panel2.Controls.Add(this.tabControl2);
            // 
            // tabControl1
            // 
            resources.ApplyResources(this.tabControl1, "tabControl1");
            this.tabControl1.Controls.Add(this.tabPage_game);
            this.tabControl1.Controls.Add(this.tabPage_snapshots);
            this.tabControl1.Controls.Add(this.tabPage_covers);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            // 
            // tabPage_game
            // 
            resources.ApplyResources(this.tabPage_game, "tabPage_game");
            this.tabPage_game.Name = "tabPage_game";
            this.tabPage_game.UseVisualStyleBackColor = true;
            // 
            // tabPage_snapshots
            // 
            resources.ApplyResources(this.tabPage_snapshots, "tabPage_snapshots");
            this.tabPage_snapshots.Name = "tabPage_snapshots";
            this.tabPage_snapshots.UseVisualStyleBackColor = true;
            // 
            // tabPage_covers
            // 
            resources.ApplyResources(this.tabPage_covers, "tabPage_covers");
            this.tabPage_covers.Name = "tabPage_covers";
            this.tabPage_covers.UseVisualStyleBackColor = true;
            // 
            // tabControl2
            // 
            resources.ApplyResources(this.tabControl2, "tabControl2");
            this.tabControl2.Controls.Add(this.tabPage_info);
            this.tabControl2.Controls.Add(this.tabPage_manuals);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            // 
            // tabPage_info
            // 
            resources.ApplyResources(this.tabPage_info, "tabPage_info");
            this.tabPage_info.Name = "tabPage_info";
            this.tabPage_info.UseVisualStyleBackColor = true;
            // 
            // tabPage_manuals
            // 
            resources.ApplyResources(this.tabPage_manuals, "tabPage_manuals");
            this.tabPage_manuals.Name = "tabPage_manuals";
            this.tabPage_manuals.UseVisualStyleBackColor = true;
            // 
            // toolStrip1
            // 
            resources.ApplyResources(this.toolStrip1, "toolStrip1");
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripButton2,
            this.toolStripButton7,
            this.toolStripButton5,
            this.toolStripSeparator1,
            this.toolStripButton4,
            this.toolStripTextBox_find,
            this.toolStripSeparator4,
            this.toolStripSplitButton1,
            this.toolStripButton6});
            this.toolStrip1.Name = "toolStrip1";
            // 
            // toolStripButton1
            // 
            resources.ApplyResources(this.toolStripButton1, "toolStripButton1");
            this.toolStripButton1.Image = global::MyNes.Properties.Resources.database_add;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripButton2
            // 
            resources.ApplyResources(this.toolStripButton2, "toolStripButton2");
            this.toolStripButton2.Image = global::MyNes.Properties.Resources.database_table;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // toolStripButton7
            // 
            resources.ApplyResources(this.toolStripButton7, "toolStripButton7");
            this.toolStripButton7.Image = global::MyNes.Properties.Resources.add;
            this.toolStripButton7.Name = "toolStripButton7";
            this.toolStripButton7.Click += new System.EventHandler(this.toolStripButton7_Click);
            // 
            // toolStripButton5
            // 
            resources.ApplyResources(this.toolStripButton5, "toolStripButton5");
            this.toolStripButton5.Image = global::MyNes.Properties.Resources.database_go;
            this.toolStripButton5.Name = "toolStripButton5";
            this.toolStripButton5.Click += new System.EventHandler(this.toolStripButton5_Click);
            // 
            // toolStripSeparator1
            // 
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            // 
            // toolStripButton4
            // 
            resources.ApplyResources(this.toolStripButton4, "toolStripButton4");
            this.toolStripButton4.Image = global::MyNes.Properties.Resources.find;
            this.toolStripButton4.Name = "toolStripButton4";
            this.toolStripButton4.Click += new System.EventHandler(this.toolStripButton4_Click);
            // 
            // toolStripTextBox_find
            // 
            resources.ApplyResources(this.toolStripTextBox_find, "toolStripTextBox_find");
            this.toolStripTextBox_find.Name = "toolStripTextBox_find";
            this.toolStripTextBox_find.TextChanged += new System.EventHandler(this.toolStripTextBox_find_TextChanged);
            // 
            // toolStripSeparator4
            // 
            resources.ApplyResources(this.toolStripSeparator4, "toolStripSeparator4");
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            // 
            // toolStripSplitButton1
            // 
            resources.ApplyResources(this.toolStripSplitButton1, "toolStripSplitButton1");
            this.toolStripSplitButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.autoMinimizeToolStripMenuItem,
            this.cycleImagesOnGameInfoToolStripMenuItem,
            this.rememberSelectionToolStripMenuItem,
            this.showLauncherAtAppStartToolStripMenuItem});
            this.toolStripSplitButton1.Image = global::MyNes.Properties.Resources.wrench;
            this.toolStripSplitButton1.Name = "toolStripSplitButton1";
            this.toolStripSplitButton1.DropDownOpening += new System.EventHandler(this.toolStripSplitButton1_DropDownOpening);
            // 
            // autoMinimizeToolStripMenuItem
            // 
            resources.ApplyResources(this.autoMinimizeToolStripMenuItem, "autoMinimizeToolStripMenuItem");
            this.autoMinimizeToolStripMenuItem.Checked = true;
            this.autoMinimizeToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.autoMinimizeToolStripMenuItem.Name = "autoMinimizeToolStripMenuItem";
            this.autoMinimizeToolStripMenuItem.Click += new System.EventHandler(this.autoMinimizeToolStripMenuItem_Click);
            // 
            // cycleImagesOnGameInfoToolStripMenuItem
            // 
            resources.ApplyResources(this.cycleImagesOnGameInfoToolStripMenuItem, "cycleImagesOnGameInfoToolStripMenuItem");
            this.cycleImagesOnGameInfoToolStripMenuItem.Checked = true;
            this.cycleImagesOnGameInfoToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cycleImagesOnGameInfoToolStripMenuItem.Name = "cycleImagesOnGameInfoToolStripMenuItem";
            this.cycleImagesOnGameInfoToolStripMenuItem.Click += new System.EventHandler(this.cycleImagesOnGameInfoToolStripMenuItem_Click);
            // 
            // rememberSelectionToolStripMenuItem
            // 
            resources.ApplyResources(this.rememberSelectionToolStripMenuItem, "rememberSelectionToolStripMenuItem");
            this.rememberSelectionToolStripMenuItem.Checked = true;
            this.rememberSelectionToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.rememberSelectionToolStripMenuItem.Name = "rememberSelectionToolStripMenuItem";
            this.rememberSelectionToolStripMenuItem.Click += new System.EventHandler(this.rememberSelectionToolStripMenuItem_Click);
            // 
            // showLauncherAtAppStartToolStripMenuItem
            // 
            resources.ApplyResources(this.showLauncherAtAppStartToolStripMenuItem, "showLauncherAtAppStartToolStripMenuItem");
            this.showLauncherAtAppStartToolStripMenuItem.Name = "showLauncherAtAppStartToolStripMenuItem";
            this.showLauncherAtAppStartToolStripMenuItem.Click += new System.EventHandler(this.showLauncherAtAppStartToolStripMenuItem_Click);
            // 
            // toolStripButton6
            // 
            resources.ApplyResources(this.toolStripButton6, "toolStripButton6");
            this.toolStripButton6.Image = global::MyNes.Properties.Resources.application_view_list;
            this.toolStripButton6.Name = "toolStripButton6";
            this.toolStripButton6.Click += new System.EventHandler(this.toolStripButton6_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "table.png");
            this.imageList1.Images.SetKeyName(1, "database.png");
            this.imageList1.Images.SetKeyName(2, "database_table.png");
            this.imageList1.Images.SetKeyName(3, "INES.ico");
            this.imageList1.Images.SetKeyName(4, "cross.png");
            // 
            // statusStrip1
            // 
            resources.ApplyResources(this.statusStrip1, "statusStrip1");
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2,
            this.StatusLabel,
            this.ProgressBar});
            this.statusStrip1.Name = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            resources.ApplyResources(this.toolStripStatusLabel1, "toolStripStatusLabel1");
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            // 
            // toolStripStatusLabel2
            // 
            resources.ApplyResources(this.toolStripStatusLabel2, "toolStripStatusLabel2");
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            // 
            // StatusLabel
            // 
            resources.ApplyResources(this.StatusLabel, "StatusLabel");
            this.StatusLabel.Name = "StatusLabel";
            // 
            // ProgressBar
            // 
            resources.ApplyResources(this.ProgressBar, "ProgressBar");
            this.ProgressBar.Name = "ProgressBar";
            // 
            // timer_play
            // 
            this.timer_play.Interval = 1000;
            this.timer_play.Tick += new System.EventHandler(this.timer_play_Tick);
            // 
            // toolStrip2
            // 
            resources.ApplyResources(this.toolStrip2, "toolStrip2");
            this.toolStrip2.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator6,
            this.toolStripLabel1,
            this.toolStripSeparator7,
            this.Button_mode_all,
            this.Button_mode_database,
            this.Button_mode_notDB,
            this.Button_mode_files,
            this.Button_mode_missing,
            this.toolStripSeparator2,
            this.toolStripButton3,
            this.toolStripButton8,
            this.toolStripButton9,
            this.toolStripSeparator5});
            this.toolStrip2.Name = "toolStrip2";
            // 
            // toolStripSeparator6
            // 
            resources.ApplyResources(this.toolStripSeparator6, "toolStripSeparator6");
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            // 
            // toolStripLabel1
            // 
            resources.ApplyResources(this.toolStripLabel1, "toolStripLabel1");
            this.toolStripLabel1.ForeColor = System.Drawing.Color.DarkGray;
            this.toolStripLabel1.Name = "toolStripLabel1";
            // 
            // toolStripSeparator7
            // 
            resources.ApplyResources(this.toolStripSeparator7, "toolStripSeparator7");
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            // 
            // Button_mode_all
            // 
            resources.ApplyResources(this.Button_mode_all, "Button_mode_all");
            this.Button_mode_all.Checked = true;
            this.Button_mode_all.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Button_mode_all.Image = global::MyNes.Properties.Resources.server_database;
            this.Button_mode_all.Name = "Button_mode_all";
            this.Button_mode_all.Click += new System.EventHandler(this.Button_mode_all_Click);
            // 
            // Button_mode_database
            // 
            resources.ApplyResources(this.Button_mode_database, "Button_mode_database");
            this.Button_mode_database.Image = global::MyNes.Properties.Resources.database;
            this.Button_mode_database.Name = "Button_mode_database";
            this.Button_mode_database.Click += new System.EventHandler(this.Button_mode_database_Click);
            // 
            // Button_mode_notDB
            // 
            resources.ApplyResources(this.Button_mode_notDB, "Button_mode_notDB");
            this.Button_mode_notDB.Image = global::MyNes.Properties.Resources.database_delete;
            this.Button_mode_notDB.Name = "Button_mode_notDB";
            this.Button_mode_notDB.Click += new System.EventHandler(this.Button_mode_notDB_Click);
            // 
            // Button_mode_files
            // 
            resources.ApplyResources(this.Button_mode_files, "Button_mode_files");
            this.Button_mode_files.Image = global::MyNes.Properties.Resources.INES;
            this.Button_mode_files.Name = "Button_mode_files";
            this.Button_mode_files.Click += new System.EventHandler(this.Button_mode_files_Click);
            // 
            // Button_mode_missing
            // 
            resources.ApplyResources(this.Button_mode_missing, "Button_mode_missing");
            this.Button_mode_missing.Image = global::MyNes.Properties.Resources.cross;
            this.Button_mode_missing.Name = "Button_mode_missing";
            this.Button_mode_missing.Click += new System.EventHandler(this.Button_mode_missing_Click);
            // 
            // toolStripSeparator2
            // 
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            // 
            // toolStripButton3
            // 
            resources.ApplyResources(this.toolStripButton3, "toolStripButton3");
            this.toolStripButton3.Image = global::MyNes.Properties.Resources.control_play;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Click += new System.EventHandler(this.PlayGame_Click);
            // 
            // toolStripButton8
            // 
            resources.ApplyResources(this.toolStripButton8, "toolStripButton8");
            this.toolStripButton8.Image = global::MyNes.Properties.Resources.information;
            this.toolStripButton8.Name = "toolStripButton8";
            this.toolStripButton8.Click += new System.EventHandler(this.ShowGameInfo);
            // 
            // toolStripButton9
            // 
            resources.ApplyResources(this.toolStripButton9, "toolStripButton9");
            this.toolStripButton9.Image = global::MyNes.Properties.Resources.cross_black;
            this.toolStripButton9.Name = "toolStripButton9";
            this.toolStripButton9.Click += new System.EventHandler(this.DeleteSelectedEntries);
            // 
            // toolStripSeparator5
            // 
            resources.ApplyResources(this.toolStripSeparator5, "toolStripSeparator5");
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            // 
            // timer_search
            // 
            this.timer_search.Tick += new System.EventHandler(this.timer_search_Tick);
            // 
            // contextMenuStrip_list_columns
            // 
            resources.ApplyResources(this.contextMenuStrip_list_columns, "contextMenuStrip_list_columns");
            this.contextMenuStrip_list_columns.Name = "contextMenuStrip_list_columns";
            this.contextMenuStrip_list_columns.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.contextMenuStrip_list_columns_ItemClicked);
            // 
            // timer_progress
            // 
            this.timer_progress.Tick += new System.EventHandler(this.timer_progress_Tick);
            // 
            // timer_selection
            // 
            this.timer_selection.Interval = 50;
            this.timer_selection.Tick += new System.EventHandler(this.timer_selection_Tick);
            // 
            // FormLauncher
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.toolStrip2);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "FormLauncher";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormLauncher_FormClosing);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.contextMenuStrip_list_main.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private MLV.ManagedListView managedListView1;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ImageList imageList_items;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.Timer timer_play;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBox_find;
        private System.Windows.Forms.ToolStripButton toolStripButton4;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripButton Button_mode_all;
        private System.Windows.Forms.ToolStripButton Button_mode_database;
        private System.Windows.Forms.ToolStripButton Button_mode_files;
        private System.Windows.Forms.ToolStripButton Button_mode_missing;
        private System.Windows.Forms.ToolStripButton Button_mode_notDB;
        private System.Windows.Forms.Timer timer_search;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip_list_main;
        private System.Windows.Forms.ToolStripMenuItem playToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip_list_columns;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem openFileLocationToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton toolStripButton5;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel StatusLabel;
        private System.Windows.Forms.ToolStripProgressBar ProgressBar;
        private System.Windows.Forms.Timer timer_progress;

        private System.Windows.Forms.Timer timer_selection;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton toolStripButton6;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage_game;
        private System.Windows.Forms.TabPage tabPage_snapshots;
        private System.Windows.Forms.TabPage tabPage_covers;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage_info;
        private System.Windows.Forms.TabPage tabPage_manuals;
        private System.Windows.Forms.ToolStripSplitButton toolStripSplitButton1;
        private System.Windows.Forms.ToolStripMenuItem autoMinimizeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cycleImagesOnGameInfoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rememberSelectionToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton toolStripButton7;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton toolStripButton3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripButton toolStripButton8;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripMenuItem infoToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton toolStripButton9;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        private System.Windows.Forms.ToolStripMenuItem showLauncherAtAppStartToolStripMenuItem;
    }
}