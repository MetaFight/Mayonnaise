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
    partial class FormMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openRecentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.launcherToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.romInfoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.takesnapshotToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.recordSoundToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.machinToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.togglePauseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.softResetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hardResetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.shutdownToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator16 = new System.Windows.Forms.ToolStripSeparator();
            this.statusToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem8 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem10 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem9 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.saveStateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadStateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.saveStateAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadStateAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.languageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator12 = new System.Windows.Forms.ToolStripSeparator();
            this.regionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aUTOToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nTSCToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pALBToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dENDYToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gameGenieToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.videoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.palettesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.audioSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.inputsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pathsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.preferencesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.connect4PlayersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.connectZapperToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator15 = new System.Windows.Forms.ToolStripSeparator();
            this.fullscreenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator11 = new System.Windows.Forms.ToolStripSeparator();
            this.frameSkipToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.noneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem12 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem13 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem14 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem15 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem16 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem17 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem18 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem19 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem20 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator14 = new System.Windows.Forms.ToolStripSeparator();
            this.turboToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewHelpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.visitWebsiteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.codeprojectArticleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.facebookPageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator13 = new System.Windows.Forms.ToolStripSeparator();
            this.aboutMyNesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel_surface = new System.Windows.Forms.Panel();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            resources.ApplyResources(this.menuStrip1, "menuStrip1");
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.machinToolStripMenuItem,
            this.stateToolStripMenuItem,
            this.settingsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.MenuActivate += new System.EventHandler(this.menuStrip1_MenuActivate);
            this.menuStrip1.MenuDeactivate += new System.EventHandler(this.menuStrip1_MenuDeactivate);
            // 
            // fileToolStripMenuItem
            // 
            resources.ApplyResources(this.fileToolStripMenuItem, "fileToolStripMenuItem");
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.openRecentToolStripMenuItem,
            this.toolStripSeparator1,
            this.launcherToolStripMenuItem,
            this.toolStripSeparator2,
            this.romInfoToolStripMenuItem,
            this.toolStripSeparator5,
            this.takesnapshotToolStripMenuItem,
            this.recordSoundToolStripMenuItem,
            this.toolStripSeparator10,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.DropDownOpening += new System.EventHandler(this.fileToolStripMenuItem_DropDownOpening);
            // 
            // openToolStripMenuItem
            // 
            resources.ApplyResources(this.openToolStripMenuItem, "openToolStripMenuItem");
            this.openToolStripMenuItem.Image = global::MyNes.Properties.Resources.folder;
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // openRecentToolStripMenuItem
            // 
            resources.ApplyResources(this.openRecentToolStripMenuItem, "openRecentToolStripMenuItem");
            this.openRecentToolStripMenuItem.Name = "openRecentToolStripMenuItem";
            this.openRecentToolStripMenuItem.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.openRecentToolStripMenuItem_DropDownItemClicked);
            // 
            // toolStripSeparator1
            // 
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            // 
            // launcherToolStripMenuItem
            // 
            resources.ApplyResources(this.launcherToolStripMenuItem, "launcherToolStripMenuItem");
            this.launcherToolStripMenuItem.Image = global::MyNes.Properties.Resources.database_table;
            this.launcherToolStripMenuItem.Name = "launcherToolStripMenuItem";
            this.launcherToolStripMenuItem.Click += new System.EventHandler(this.launcherToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            // 
            // romInfoToolStripMenuItem
            // 
            resources.ApplyResources(this.romInfoToolStripMenuItem, "romInfoToolStripMenuItem");
            this.romInfoToolStripMenuItem.Image = global::MyNes.Properties.Resources.information;
            this.romInfoToolStripMenuItem.Name = "romInfoToolStripMenuItem";
            this.romInfoToolStripMenuItem.Click += new System.EventHandler(this.romInfoToolStripMenuItem_Click);
            // 
            // toolStripSeparator5
            // 
            resources.ApplyResources(this.toolStripSeparator5, "toolStripSeparator5");
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            // 
            // takesnapshotToolStripMenuItem
            // 
            resources.ApplyResources(this.takesnapshotToolStripMenuItem, "takesnapshotToolStripMenuItem");
            this.takesnapshotToolStripMenuItem.Image = global::MyNes.Properties.Resources.camera;
            this.takesnapshotToolStripMenuItem.Name = "takesnapshotToolStripMenuItem";
            this.takesnapshotToolStripMenuItem.Click += new System.EventHandler(this.takesnapshotToolStripMenuItem_Click);
            // 
            // recordSoundToolStripMenuItem
            // 
            resources.ApplyResources(this.recordSoundToolStripMenuItem, "recordSoundToolStripMenuItem");
            this.recordSoundToolStripMenuItem.Image = global::MyNes.Properties.Resources.sound_none;
            this.recordSoundToolStripMenuItem.Name = "recordSoundToolStripMenuItem";
            this.recordSoundToolStripMenuItem.Click += new System.EventHandler(this.recordSoundToolStripMenuItem_Click);
            // 
            // toolStripSeparator10
            // 
            resources.ApplyResources(this.toolStripSeparator10, "toolStripSeparator10");
            this.toolStripSeparator10.Name = "toolStripSeparator10";
            // 
            // exitToolStripMenuItem
            // 
            resources.ApplyResources(this.exitToolStripMenuItem, "exitToolStripMenuItem");
            this.exitToolStripMenuItem.Image = global::MyNes.Properties.Resources.door_in;
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // machinToolStripMenuItem
            // 
            resources.ApplyResources(this.machinToolStripMenuItem, "machinToolStripMenuItem");
            this.machinToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.togglePauseToolStripMenuItem,
            this.toolStripSeparator3,
            this.softResetToolStripMenuItem,
            this.hardResetToolStripMenuItem,
            this.toolStripSeparator4,
            this.shutdownToolStripMenuItem,
            this.toolStripSeparator16,
            this.statusToolStripMenuItem});
            this.machinToolStripMenuItem.Name = "machinToolStripMenuItem";
            // 
            // togglePauseToolStripMenuItem
            // 
            resources.ApplyResources(this.togglePauseToolStripMenuItem, "togglePauseToolStripMenuItem");
            this.togglePauseToolStripMenuItem.Image = global::MyNes.Properties.Resources.control_pause;
            this.togglePauseToolStripMenuItem.Name = "togglePauseToolStripMenuItem";
            this.togglePauseToolStripMenuItem.Click += new System.EventHandler(this.togglePauseToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            resources.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            // 
            // softResetToolStripMenuItem
            // 
            resources.ApplyResources(this.softResetToolStripMenuItem, "softResetToolStripMenuItem");
            this.softResetToolStripMenuItem.Image = global::MyNes.Properties.Resources.control_repeat;
            this.softResetToolStripMenuItem.Name = "softResetToolStripMenuItem";
            this.softResetToolStripMenuItem.Click += new System.EventHandler(this.softResetToolStripMenuItem_Click);
            // 
            // hardResetToolStripMenuItem
            // 
            resources.ApplyResources(this.hardResetToolStripMenuItem, "hardResetToolStripMenuItem");
            this.hardResetToolStripMenuItem.Image = global::MyNes.Properties.Resources.control_repeat_blue;
            this.hardResetToolStripMenuItem.Name = "hardResetToolStripMenuItem";
            this.hardResetToolStripMenuItem.Click += new System.EventHandler(this.hardResetToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            resources.ApplyResources(this.toolStripSeparator4, "toolStripSeparator4");
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            // 
            // shutdownToolStripMenuItem
            // 
            resources.ApplyResources(this.shutdownToolStripMenuItem, "shutdownToolStripMenuItem");
            this.shutdownToolStripMenuItem.Image = global::MyNes.Properties.Resources.control_eject;
            this.shutdownToolStripMenuItem.Name = "shutdownToolStripMenuItem";
            this.shutdownToolStripMenuItem.Click += new System.EventHandler(this.shutdownToolStripMenuItem_Click);
            // 
            // toolStripSeparator16
            // 
            resources.ApplyResources(this.toolStripSeparator16, "toolStripSeparator16");
            this.toolStripSeparator16.Name = "toolStripSeparator16";
            // 
            // statusToolStripMenuItem
            // 
            resources.ApplyResources(this.statusToolStripMenuItem, "statusToolStripMenuItem");
            this.statusToolStripMenuItem.Image = global::MyNes.Properties.Resources.information;
            this.statusToolStripMenuItem.Name = "statusToolStripMenuItem";
            this.statusToolStripMenuItem.Click += new System.EventHandler(this.statusToolStripMenuItem_Click);
            // 
            // stateToolStripMenuItem
            // 
            resources.ApplyResources(this.stateToolStripMenuItem, "stateToolStripMenuItem");
            this.stateToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem8,
            this.toolStripMenuItem7,
            this.toolStripMenuItem6,
            this.toolStripMenuItem5,
            this.toolStripMenuItem4,
            this.toolStripMenuItem3,
            this.toolStripMenuItem2,
            this.toolStripMenuItem1,
            this.toolStripMenuItem10,
            this.toolStripMenuItem9,
            this.toolStripSeparator7,
            this.saveStateToolStripMenuItem,
            this.loadStateToolStripMenuItem,
            this.toolStripSeparator8,
            this.saveStateAsToolStripMenuItem,
            this.loadStateAsToolStripMenuItem});
            this.stateToolStripMenuItem.Name = "stateToolStripMenuItem";
            this.stateToolStripMenuItem.DropDownOpening += new System.EventHandler(this.stateToolStripMenuItem_DropDownOpening);
            // 
            // toolStripMenuItem8
            // 
            resources.ApplyResources(this.toolStripMenuItem8, "toolStripMenuItem8");
            this.toolStripMenuItem8.Name = "toolStripMenuItem8";
            this.toolStripMenuItem8.Click += new System.EventHandler(this.toolStripMenuItem8_Click);
            // 
            // toolStripMenuItem7
            // 
            resources.ApplyResources(this.toolStripMenuItem7, "toolStripMenuItem7");
            this.toolStripMenuItem7.Name = "toolStripMenuItem7";
            this.toolStripMenuItem7.Click += new System.EventHandler(this.toolStripMenuItem7_Click);
            // 
            // toolStripMenuItem6
            // 
            resources.ApplyResources(this.toolStripMenuItem6, "toolStripMenuItem6");
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Click += new System.EventHandler(this.toolStripMenuItem6_Click);
            // 
            // toolStripMenuItem5
            // 
            resources.ApplyResources(this.toolStripMenuItem5, "toolStripMenuItem5");
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Click += new System.EventHandler(this.toolStripMenuItem5_Click);
            // 
            // toolStripMenuItem4
            // 
            resources.ApplyResources(this.toolStripMenuItem4, "toolStripMenuItem4");
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Click += new System.EventHandler(this.toolStripMenuItem4_Click);
            // 
            // toolStripMenuItem3
            // 
            resources.ApplyResources(this.toolStripMenuItem3, "toolStripMenuItem3");
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Click += new System.EventHandler(this.toolStripMenuItem3_Click);
            // 
            // toolStripMenuItem2
            // 
            resources.ApplyResources(this.toolStripMenuItem2, "toolStripMenuItem2");
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
            // 
            // toolStripMenuItem1
            // 
            resources.ApplyResources(this.toolStripMenuItem1, "toolStripMenuItem1");
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // toolStripMenuItem10
            // 
            resources.ApplyResources(this.toolStripMenuItem10, "toolStripMenuItem10");
            this.toolStripMenuItem10.Name = "toolStripMenuItem10";
            this.toolStripMenuItem10.Click += new System.EventHandler(this.toolStripMenuItem10_Click);
            // 
            // toolStripMenuItem9
            // 
            resources.ApplyResources(this.toolStripMenuItem9, "toolStripMenuItem9");
            this.toolStripMenuItem9.Name = "toolStripMenuItem9";
            this.toolStripMenuItem9.Click += new System.EventHandler(this.toolStripMenuItem9_Click);
            // 
            // toolStripSeparator7
            // 
            resources.ApplyResources(this.toolStripSeparator7, "toolStripSeparator7");
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            // 
            // saveStateToolStripMenuItem
            // 
            resources.ApplyResources(this.saveStateToolStripMenuItem, "saveStateToolStripMenuItem");
            this.saveStateToolStripMenuItem.Image = global::MyNes.Properties.Resources.drive_disk;
            this.saveStateToolStripMenuItem.Name = "saveStateToolStripMenuItem";
            this.saveStateToolStripMenuItem.Click += new System.EventHandler(this.saveStateToolStripMenuItem_Click);
            // 
            // loadStateToolStripMenuItem
            // 
            resources.ApplyResources(this.loadStateToolStripMenuItem, "loadStateToolStripMenuItem");
            this.loadStateToolStripMenuItem.Image = global::MyNes.Properties.Resources.drive_go;
            this.loadStateToolStripMenuItem.Name = "loadStateToolStripMenuItem";
            this.loadStateToolStripMenuItem.Click += new System.EventHandler(this.loadStateToolStripMenuItem_Click);
            // 
            // toolStripSeparator8
            // 
            resources.ApplyResources(this.toolStripSeparator8, "toolStripSeparator8");
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            // 
            // saveStateAsToolStripMenuItem
            // 
            resources.ApplyResources(this.saveStateAsToolStripMenuItem, "saveStateAsToolStripMenuItem");
            this.saveStateAsToolStripMenuItem.Image = global::MyNes.Properties.Resources.disk;
            this.saveStateAsToolStripMenuItem.Name = "saveStateAsToolStripMenuItem";
            this.saveStateAsToolStripMenuItem.Click += new System.EventHandler(this.saveStateAsToolStripMenuItem_Click);
            // 
            // loadStateAsToolStripMenuItem
            // 
            resources.ApplyResources(this.loadStateAsToolStripMenuItem, "loadStateAsToolStripMenuItem");
            this.loadStateAsToolStripMenuItem.Image = global::MyNes.Properties.Resources.folder;
            this.loadStateAsToolStripMenuItem.Name = "loadStateAsToolStripMenuItem";
            this.loadStateAsToolStripMenuItem.Click += new System.EventHandler(this.loadStateAsToolStripMenuItem_Click);
            // 
            // settingsToolStripMenuItem
            // 
            resources.ApplyResources(this.settingsToolStripMenuItem, "settingsToolStripMenuItem");
            this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.languageToolStripMenuItem,
            this.toolStripSeparator12,
            this.regionToolStripMenuItem,
            this.gameGenieToolStripMenuItem,
            this.videoToolStripMenuItem,
            this.palettesToolStripMenuItem,
            this.audioSettingsToolStripMenuItem,
            this.inputsToolStripMenuItem,
            this.pathsToolStripMenuItem,
            this.preferencesToolStripMenuItem,
            this.toolStripSeparator9,
            this.connect4PlayersToolStripMenuItem,
            this.connectZapperToolStripMenuItem,
            this.toolStripSeparator15,
            this.fullscreenToolStripMenuItem,
            this.toolStripSeparator11,
            this.frameSkipToolStripMenuItem,
            this.toolStripSeparator14,
            this.turboToolStripMenuItem});
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.DropDownOpening += new System.EventHandler(this.settingsToolStripMenuItem_DropDownOpening);
            // 
            // languageToolStripMenuItem
            // 
            resources.ApplyResources(this.languageToolStripMenuItem, "languageToolStripMenuItem");
            this.languageToolStripMenuItem.Name = "languageToolStripMenuItem";
            this.languageToolStripMenuItem.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.languageToolStripMenuItem_DropDownItemClicked);
            // 
            // toolStripSeparator12
            // 
            resources.ApplyResources(this.toolStripSeparator12, "toolStripSeparator12");
            this.toolStripSeparator12.Name = "toolStripSeparator12";
            // 
            // regionToolStripMenuItem
            // 
            resources.ApplyResources(this.regionToolStripMenuItem, "regionToolStripMenuItem");
            this.regionToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aUTOToolStripMenuItem,
            this.nTSCToolStripMenuItem,
            this.pALBToolStripMenuItem,
            this.dENDYToolStripMenuItem});
            this.regionToolStripMenuItem.Image = global::MyNes.Properties.Resources.world;
            this.regionToolStripMenuItem.Name = "regionToolStripMenuItem";
            this.regionToolStripMenuItem.DropDownOpening += new System.EventHandler(this.regionToolStripMenuItem_DropDownOpening);
            // 
            // aUTOToolStripMenuItem
            // 
            resources.ApplyResources(this.aUTOToolStripMenuItem, "aUTOToolStripMenuItem");
            this.aUTOToolStripMenuItem.Name = "aUTOToolStripMenuItem";
            this.aUTOToolStripMenuItem.Click += new System.EventHandler(this.aUTOToolStripMenuItem_Click);
            // 
            // nTSCToolStripMenuItem
            // 
            resources.ApplyResources(this.nTSCToolStripMenuItem, "nTSCToolStripMenuItem");
            this.nTSCToolStripMenuItem.Name = "nTSCToolStripMenuItem";
            this.nTSCToolStripMenuItem.Click += new System.EventHandler(this.nTSCToolStripMenuItem_Click);
            // 
            // pALBToolStripMenuItem
            // 
            resources.ApplyResources(this.pALBToolStripMenuItem, "pALBToolStripMenuItem");
            this.pALBToolStripMenuItem.Name = "pALBToolStripMenuItem";
            this.pALBToolStripMenuItem.Click += new System.EventHandler(this.pALBToolStripMenuItem_Click);
            // 
            // dENDYToolStripMenuItem
            // 
            resources.ApplyResources(this.dENDYToolStripMenuItem, "dENDYToolStripMenuItem");
            this.dENDYToolStripMenuItem.Name = "dENDYToolStripMenuItem";
            this.dENDYToolStripMenuItem.Click += new System.EventHandler(this.dENDYToolStripMenuItem_Click);
            // 
            // gameGenieToolStripMenuItem
            // 
            resources.ApplyResources(this.gameGenieToolStripMenuItem, "gameGenieToolStripMenuItem");
            this.gameGenieToolStripMenuItem.Image = global::MyNes.Properties.Resources.FileGame_genie_nes_front;
            this.gameGenieToolStripMenuItem.Name = "gameGenieToolStripMenuItem";
            this.gameGenieToolStripMenuItem.Click += new System.EventHandler(this.gameGenieToolStripMenuItem_Click);
            // 
            // videoToolStripMenuItem
            // 
            resources.ApplyResources(this.videoToolStripMenuItem, "videoToolStripMenuItem");
            this.videoToolStripMenuItem.Image = global::MyNes.Properties.Resources.monitor;
            this.videoToolStripMenuItem.Name = "videoToolStripMenuItem";
            this.videoToolStripMenuItem.Click += new System.EventHandler(this.videoToolStripMenuItem_Click);
            // 
            // palettesToolStripMenuItem
            // 
            resources.ApplyResources(this.palettesToolStripMenuItem, "palettesToolStripMenuItem");
            this.palettesToolStripMenuItem.Image = global::MyNes.Properties.Resources.color_wheel;
            this.palettesToolStripMenuItem.Name = "palettesToolStripMenuItem";
            this.palettesToolStripMenuItem.Click += new System.EventHandler(this.palettesToolStripMenuItem_Click);
            // 
            // audioSettingsToolStripMenuItem
            // 
            resources.ApplyResources(this.audioSettingsToolStripMenuItem, "audioSettingsToolStripMenuItem");
            this.audioSettingsToolStripMenuItem.Image = global::MyNes.Properties.Resources.sound;
            this.audioSettingsToolStripMenuItem.Name = "audioSettingsToolStripMenuItem";
            this.audioSettingsToolStripMenuItem.Click += new System.EventHandler(this.audioSettingsToolStripMenuItem_Click);
            // 
            // inputsToolStripMenuItem
            // 
            resources.ApplyResources(this.inputsToolStripMenuItem, "inputsToolStripMenuItem");
            this.inputsToolStripMenuItem.Image = global::MyNes.Properties.Resources.controller;
            this.inputsToolStripMenuItem.Name = "inputsToolStripMenuItem";
            this.inputsToolStripMenuItem.Click += new System.EventHandler(this.inputsToolStripMenuItem_Click);
            // 
            // pathsToolStripMenuItem
            // 
            resources.ApplyResources(this.pathsToolStripMenuItem, "pathsToolStripMenuItem");
            this.pathsToolStripMenuItem.Image = global::MyNes.Properties.Resources.folder_wrench;
            this.pathsToolStripMenuItem.Name = "pathsToolStripMenuItem";
            this.pathsToolStripMenuItem.Click += new System.EventHandler(this.pathsToolStripMenuItem_Click);
            // 
            // preferencesToolStripMenuItem
            // 
            resources.ApplyResources(this.preferencesToolStripMenuItem, "preferencesToolStripMenuItem");
            this.preferencesToolStripMenuItem.Image = global::MyNes.Properties.Resources.wrench;
            this.preferencesToolStripMenuItem.Name = "preferencesToolStripMenuItem";
            this.preferencesToolStripMenuItem.Click += new System.EventHandler(this.preferencesToolStripMenuItem_Click);
            // 
            // toolStripSeparator9
            // 
            resources.ApplyResources(this.toolStripSeparator9, "toolStripSeparator9");
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            // 
            // connect4PlayersToolStripMenuItem
            // 
            resources.ApplyResources(this.connect4PlayersToolStripMenuItem, "connect4PlayersToolStripMenuItem");
            this.connect4PlayersToolStripMenuItem.Name = "connect4PlayersToolStripMenuItem";
            this.connect4PlayersToolStripMenuItem.Click += new System.EventHandler(this.connect4PlayersToolStripMenuItem_Click);
            // 
            // connectZapperToolStripMenuItem
            // 
            resources.ApplyResources(this.connectZapperToolStripMenuItem, "connectZapperToolStripMenuItem");
            this.connectZapperToolStripMenuItem.Name = "connectZapperToolStripMenuItem";
            this.connectZapperToolStripMenuItem.Click += new System.EventHandler(this.connectZapperToolStripMenuItem_Click);
            // 
            // toolStripSeparator15
            // 
            resources.ApplyResources(this.toolStripSeparator15, "toolStripSeparator15");
            this.toolStripSeparator15.Name = "toolStripSeparator15";
            // 
            // fullscreenToolStripMenuItem
            // 
            resources.ApplyResources(this.fullscreenToolStripMenuItem, "fullscreenToolStripMenuItem");
            this.fullscreenToolStripMenuItem.Name = "fullscreenToolStripMenuItem";
            this.fullscreenToolStripMenuItem.Click += new System.EventHandler(this.fullscreenToolStripMenuItem_Click);
            // 
            // toolStripSeparator11
            // 
            resources.ApplyResources(this.toolStripSeparator11, "toolStripSeparator11");
            this.toolStripSeparator11.Name = "toolStripSeparator11";
            // 
            // frameSkipToolStripMenuItem
            // 
            resources.ApplyResources(this.frameSkipToolStripMenuItem, "frameSkipToolStripMenuItem");
            this.frameSkipToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.noneToolStripMenuItem,
            this.toolStripMenuItem12,
            this.toolStripMenuItem13,
            this.toolStripMenuItem14,
            this.toolStripMenuItem15,
            this.toolStripMenuItem16,
            this.toolStripMenuItem17,
            this.toolStripMenuItem18,
            this.toolStripMenuItem19,
            this.toolStripMenuItem20});
            this.frameSkipToolStripMenuItem.Name = "frameSkipToolStripMenuItem";
            this.frameSkipToolStripMenuItem.DropDownOpening += new System.EventHandler(this.frameSkipToolStripMenuItem_DropDownOpening);
            // 
            // noneToolStripMenuItem
            // 
            resources.ApplyResources(this.noneToolStripMenuItem, "noneToolStripMenuItem");
            this.noneToolStripMenuItem.Name = "noneToolStripMenuItem";
            this.noneToolStripMenuItem.Click += new System.EventHandler(this.noneToolStripMenuItem_Click);
            // 
            // toolStripMenuItem12
            // 
            resources.ApplyResources(this.toolStripMenuItem12, "toolStripMenuItem12");
            this.toolStripMenuItem12.Name = "toolStripMenuItem12";
            this.toolStripMenuItem12.Click += new System.EventHandler(this.toolStripMenuItem12_Click);
            // 
            // toolStripMenuItem13
            // 
            resources.ApplyResources(this.toolStripMenuItem13, "toolStripMenuItem13");
            this.toolStripMenuItem13.Name = "toolStripMenuItem13";
            this.toolStripMenuItem13.Click += new System.EventHandler(this.toolStripMenuItem13_Click);
            // 
            // toolStripMenuItem14
            // 
            resources.ApplyResources(this.toolStripMenuItem14, "toolStripMenuItem14");
            this.toolStripMenuItem14.Name = "toolStripMenuItem14";
            this.toolStripMenuItem14.Click += new System.EventHandler(this.toolStripMenuItem14_Click);
            // 
            // toolStripMenuItem15
            // 
            resources.ApplyResources(this.toolStripMenuItem15, "toolStripMenuItem15");
            this.toolStripMenuItem15.Name = "toolStripMenuItem15";
            this.toolStripMenuItem15.Click += new System.EventHandler(this.toolStripMenuItem15_Click);
            // 
            // toolStripMenuItem16
            // 
            resources.ApplyResources(this.toolStripMenuItem16, "toolStripMenuItem16");
            this.toolStripMenuItem16.Name = "toolStripMenuItem16";
            this.toolStripMenuItem16.Click += new System.EventHandler(this.toolStripMenuItem16_Click);
            // 
            // toolStripMenuItem17
            // 
            resources.ApplyResources(this.toolStripMenuItem17, "toolStripMenuItem17");
            this.toolStripMenuItem17.Name = "toolStripMenuItem17";
            this.toolStripMenuItem17.Click += new System.EventHandler(this.toolStripMenuItem17_Click);
            // 
            // toolStripMenuItem18
            // 
            resources.ApplyResources(this.toolStripMenuItem18, "toolStripMenuItem18");
            this.toolStripMenuItem18.Name = "toolStripMenuItem18";
            this.toolStripMenuItem18.Click += new System.EventHandler(this.toolStripMenuItem18_Click);
            // 
            // toolStripMenuItem19
            // 
            resources.ApplyResources(this.toolStripMenuItem19, "toolStripMenuItem19");
            this.toolStripMenuItem19.Name = "toolStripMenuItem19";
            this.toolStripMenuItem19.Click += new System.EventHandler(this.toolStripMenuItem19_Click);
            // 
            // toolStripMenuItem20
            // 
            resources.ApplyResources(this.toolStripMenuItem20, "toolStripMenuItem20");
            this.toolStripMenuItem20.Name = "toolStripMenuItem20";
            this.toolStripMenuItem20.Click += new System.EventHandler(this.toolStripMenuItem20_Click);
            // 
            // toolStripSeparator14
            // 
            resources.ApplyResources(this.toolStripSeparator14, "toolStripSeparator14");
            this.toolStripSeparator14.Name = "toolStripSeparator14";
            // 
            // turboToolStripMenuItem
            // 
            resources.ApplyResources(this.turboToolStripMenuItem, "turboToolStripMenuItem");
            this.turboToolStripMenuItem.Name = "turboToolStripMenuItem";
            this.turboToolStripMenuItem.Click += new System.EventHandler(this.turboToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            resources.ApplyResources(this.helpToolStripMenuItem, "helpToolStripMenuItem");
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewHelpToolStripMenuItem,
            this.toolStripSeparator6,
            this.visitWebsiteToolStripMenuItem,
            this.codeprojectArticleToolStripMenuItem,
            this.facebookPageToolStripMenuItem,
            this.toolStripSeparator13,
            this.aboutMyNesToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            // 
            // viewHelpToolStripMenuItem
            // 
            resources.ApplyResources(this.viewHelpToolStripMenuItem, "viewHelpToolStripMenuItem");
            this.viewHelpToolStripMenuItem.Image = global::MyNes.Properties.Resources.help;
            this.viewHelpToolStripMenuItem.Name = "viewHelpToolStripMenuItem";
            this.viewHelpToolStripMenuItem.Click += new System.EventHandler(this.viewHelpToolStripMenuItem_Click);
            // 
            // toolStripSeparator6
            // 
            resources.ApplyResources(this.toolStripSeparator6, "toolStripSeparator6");
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            // 
            // visitWebsiteToolStripMenuItem
            // 
            resources.ApplyResources(this.visitWebsiteToolStripMenuItem, "visitWebsiteToolStripMenuItem");
            this.visitWebsiteToolStripMenuItem.Name = "visitWebsiteToolStripMenuItem";
            this.visitWebsiteToolStripMenuItem.Click += new System.EventHandler(this.visitWebsiteToolStripMenuItem_Click);
            // 
            // codeprojectArticleToolStripMenuItem
            // 
            resources.ApplyResources(this.codeprojectArticleToolStripMenuItem, "codeprojectArticleToolStripMenuItem");
            this.codeprojectArticleToolStripMenuItem.Name = "codeprojectArticleToolStripMenuItem";
            this.codeprojectArticleToolStripMenuItem.Click += new System.EventHandler(this.codeprojectArticleToolStripMenuItem_Click);
            // 
            // facebookPageToolStripMenuItem
            // 
            resources.ApplyResources(this.facebookPageToolStripMenuItem, "facebookPageToolStripMenuItem");
            this.facebookPageToolStripMenuItem.Name = "facebookPageToolStripMenuItem";
            this.facebookPageToolStripMenuItem.Click += new System.EventHandler(this.facebookPageToolStripMenuItem_Click);
            // 
            // toolStripSeparator13
            // 
            resources.ApplyResources(this.toolStripSeparator13, "toolStripSeparator13");
            this.toolStripSeparator13.Name = "toolStripSeparator13";
            // 
            // aboutMyNesToolStripMenuItem
            // 
            resources.ApplyResources(this.aboutMyNesToolStripMenuItem, "aboutMyNesToolStripMenuItem");
            this.aboutMyNesToolStripMenuItem.Image = global::MyNes.Properties.Resources.MyNes;
            this.aboutMyNesToolStripMenuItem.Name = "aboutMyNesToolStripMenuItem";
            this.aboutMyNesToolStripMenuItem.Click += new System.EventHandler(this.aboutMyNesToolStripMenuItem_Click);
            // 
            // panel_surface
            // 
            resources.ApplyResources(this.panel_surface, "panel_surface");
            this.panel_surface.BackColor = System.Drawing.Color.Black;
            this.panel_surface.Name = "panel_surface";
            // 
            // FormMain
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel_surface);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FormMain";
            this.Activated += new System.EventHandler(this.FormMain_Activated);
            this.Deactivate += new System.EventHandler(this.FormMain_Deactivate);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.ResizeBegin += new System.EventHandler(this.FormMain_ResizeBegin);
            this.ResizeEnd += new System.EventHandler(this.FormMain_ResizeEnd);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewHelpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutMyNesToolStripMenuItem;
        private System.Windows.Forms.Panel panel_surface;
        private System.Windows.Forms.ToolStripMenuItem machinToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hardResetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem softResetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem audioSettingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem togglePauseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem launcherToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem shutdownToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem romInfoToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripMenuItem pathsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem preferencesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem8;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem7;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem6;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem10;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem9;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripMenuItem saveStateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadStateToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripMenuItem saveStateAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadStateAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        private System.Windows.Forms.ToolStripMenuItem fullscreenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem videoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem takesnapshotToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator10;
        private System.Windows.Forms.ToolStripMenuItem inputsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem palettesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem recordSoundToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator11;
        private System.Windows.Forms.ToolStripMenuItem turboToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem frameSkipToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem noneToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem12;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem13;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem14;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem15;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem16;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem17;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem18;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem19;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem20;
        private System.Windows.Forms.ToolStripMenuItem languageToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator12;
        private System.Windows.Forms.ToolStripMenuItem regionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aUTOToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nTSCToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pALBToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dENDYToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openRecentToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gameGenieToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem visitWebsiteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem facebookPageToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator13;
        private System.Windows.Forms.ToolStripMenuItem codeprojectArticleToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator14;
        private System.Windows.Forms.ToolStripMenuItem connect4PlayersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem connectZapperToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator15;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator16;
        private System.Windows.Forms.ToolStripMenuItem statusToolStripMenuItem;
    }
}

