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
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MyNes.Core;
using System.IO;
using SlimDX;
using SlimDX.DirectInput;
using MMB;
namespace MyNes
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
            LoadSettings();
            InitializeVideoRenderer();
            InitializeSoundRenderer();
            InitializeInputRenderer();
            NesEmu.EMUShutdown += NesEmu_EMUShutdown;
        }

        public DirectXVideo video;
        public DirectSoundRenderer audio;
        public ZapperConnecter zapper;

        private void LoadSettings()
        {
            // languages
            for (int i = 0; i < Program.SupportedLanguages.Length / 3; i++)
            {
                ToolStripMenuItem item = new ToolStripMenuItem();
                item.Text = Program.SupportedLanguages[i, 2];
                item.Checked = Program.SupportedLanguages[i, 0] == Program.Settings.Language;
                languageToolStripMenuItem.DropDownItems.Add(item);
            }
            this.Location = Program.Settings.WinLocation;
            this.Size = Program.Settings.WinSize;
            if (Program.Settings.LauncherShowAyAppStart)
                launcherToolStripMenuItem_Click(this, null);
            RefreshRecent();
            InitializePalette();
        }
        public void AddRecent(string recent)
        {
            if (Program.Settings.RecentPlayed == null)
                Program.Settings.RecentPlayed = new System.Collections.Specialized.StringCollection();
            RefreshRecent();
            if (!Program.Settings.RecentPlayed.Contains(recent))
                Program.Settings.RecentPlayed.Insert(0, recent);
            if (Program.Settings.RecentPlayed.Count == 10)
                Program.Settings.RecentPlayed.RemoveAt(9);
            RefreshRecent();
        }
        private void RefreshRecent()
        {
            openRecentToolStripMenuItem.DropDownItems.Clear();
            if (Program.Settings.RecentPlayed == null)
                Program.Settings.RecentPlayed = new System.Collections.Specialized.StringCollection();
            foreach (string recent in Program.Settings.RecentPlayed)
            {
                ToolStripMenuItem item = new ToolStripMenuItem();
                item.Text = Path.GetFileName(recent);
                item.Tag = item.ToolTipText = recent;
                openRecentToolStripMenuItem.DropDownItems.Add(item);
            }
        }
        private void SaveSettings()
        {
            Program.Settings.WinLocation = this.Location;
            Program.Settings.WinSize = this.Size;
            Program.Settings.Save();
        }
        public void OpenRom(string fileName)
        {
            // Pause emulation
            if (NesEmu.EmulationON)
                NesEmu.EmulationPaused = true;
            switch (Path.GetExtension(fileName).ToLower())
            {
                case ".7z":
                case ".zip":
                case ".rar":
                case ".gzip":
                case ".tar":
                case ".bzip2":
                case ".xz":
                    {
                        string tempFolder = Path.GetTempPath() + "\\MYNES\\";
                        SevenZip.SevenZipExtractor EXTRACTOR;
                        try
                        {
                            EXTRACTOR = new SevenZip.SevenZipExtractor(fileName);
                        }
                        catch (Exception ex)
                        {
                            ManagedMessageBox.ShowErrorMessage(ex.Message);
                            return;
                        }
                        if (EXTRACTOR.ArchiveFileData.Count == 1)
                        {
                            if (EXTRACTOR.ArchiveFileData[0].FileName.Substring(EXTRACTOR.ArchiveFileData[0].FileName.Length - 4, 4).ToLower() == ".nes")
                            {
                                EXTRACTOR.ExtractArchive(tempFolder);
                                fileName = tempFolder + EXTRACTOR.ArchiveFileData[0].FileName;
                            }
                        }
                        else
                        {
                            List<string> filenames = new List<string>();
                            foreach (SevenZip.ArchiveFileInfo file in EXTRACTOR.ArchiveFileData)
                            {
                                filenames.Add(file.FileName);
                            }
                            FormFilesList ar = new FormFilesList(filenames.ToArray());
                            if (ar.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                            {
                                string[] fil = { ar.SelectedRom };
                                EXTRACTOR.ExtractFiles(tempFolder, fil);
                                fileName = tempFolder + ar.SelectedRom;
                            }
                            else
                            { return; }
                        }
                    }
                    break;
            }
            try
            {
                NesEmu.EmulationPaused = true;// Make sure it's still paused !
                bool is_supported_mapper;
                bool has_issues;
                string issues;
                if (NesEmu.CheckRom(fileName, out is_supported_mapper, out has_issues, out issues))
                {
                    if (!is_supported_mapper && !Program.Settings.IgnoreNotSupportedMapper)
                    {
                        ManagedMessageBoxResult res = ManagedMessageBox.ShowQuestionMessage(
                            Program.ResourceManager.GetString("Message_ThisGameMapperMarkedAsNotSupported"), "My Nes",
                            null, true, Program.Settings.IgnoreNotSupportedMapper,
                            Program.ResourceManager.GetString("Text_AlwaysTryToRunGameWithNoSupportedMappers"));
                        Program.Settings.IgnoreNotSupportedMapper = res.Checked;
                        if (res.ClickedButtonIndex == 1)// No
                            return;
                    }
                    if (has_issues && Program.Settings.ShowRomIssuesIfHave)
                    {
                        ManagedMessageBoxResult res = ManagedMessageBox.ShowMessage(
                            Program.ResourceManager.GetString("Message_ThisGameHaveKnownIssues") + ":\n" + issues,
                            "My Nes", null, true, Program.Settings.ShowRomIssuesIfHave,
                            Program.ResourceManager.GetString("Text_AlwaysWarnAboutIssues"));
                        Program.Settings.ShowRomIssuesIfHave = res.Checked;
                    }
                    NesEmu.EmulationON = false;
                    // Kill the original thread
                    if (NesEmu.EmulationThread != null)
                        // while (NesEmu.EmulationThread.IsAlive)
                        //  { }
                        if (NesEmu.EmulationThread.IsAlive)
                            NesEmu.EmulationThread.Abort();
                    // Shutdown emu
                    NesEmu.ShutDown();

                    // Settings first
                    ApplyEmuSettings();

                    // Create new
                    NesEmu.CreateNew(fileName, Program.Settings.TVSystemSetting, true);
                    // Stop video for a while
                    video.threadPAUSED = true;
                    // Apply video stretch
                    ApplyVideoStretch();

                    // Initialize renderers
                    //InitializeInputRenderer();
                    //InitializeVideoRenderer();
                    //InitializeSoundRenderer();

                    video.Reset();

                    if (NesEmu.IsGameFoundOnDB)
                    {
                        if (NesEmu.GameInfo.Game_AltName != null && NesEmu.GameInfo.Game_AltName != "")
                            this.Text = NesEmu.GameInfo.Game_Name + " (" + NesEmu.GameInfo.Game_AltName + ") - My Nes";
                        else
                            this.Text = NesEmu.GameInfo.Game_Name + " - My Nes";
                    }
                    else
                    {
                        this.Text = Path.GetFileName(NesEmu.GAMEFILE) + " - My Nes";
                    }
                    NesEmu.EmulationPaused = false;
                }
                else
                {
                    ManagedMessageBox.ShowErrorMessage("My Nes can't run this game.", "My Nes");
                    if (NesEmu.EmulationON)
                        NesEmu.EmulationPaused = false;
                }
            }
            catch (Exception ex)
            {
                ManagedMessageBox.ShowErrorMessage(ex.Message, "My Nes");
                if (NesEmu.EmulationON)
                    NesEmu.EmulationPaused = false;
            }
        }
        public void InitializeInputRenderer()
        {
            // Preper things
            IJoypadConnecter joy1 = null;
            IJoypadConnecter joy2 = null;
            IJoypadConnecter joy3 = null;
            IJoypadConnecter joy4 = null;
            // Refresh input devices !
            DirectInput di = new DirectInput();
            List<DeviceInstance> devices = new List<DeviceInstance>(di.GetDevices());
            #region Player 1
            foreach (DeviceInstance dev in devices)
            {
                if (dev.InstanceGuid.ToString().ToLower() == Program.Settings.ControlSettings.Joypad1DeviceGuid)
                {
                    // We found the device !!
                    // Let's see if we have the settings for this device
                    foreach (IInputSettingsJoypad con in Program.Settings.ControlSettings.Joypad1Devices)
                    {
                        if (con.DeviceGuid.ToLower() == dev.InstanceGuid.ToString().ToLower())
                        {
                            // This is it !
                            switch (dev.Type)
                            {
                                case DeviceType.Keyboard:
                                    {
                                        joy1 = new NesJoypadPcKeyboardConnection(this.Handle, con);
                                        break;
                                    }
                                case DeviceType.Joystick:
                                    {
                                        joy1 = new NesJoypadPcJoystickConnection(this.Handle, dev.InstanceGuid.ToString(), con);
                                        break;
                                    }
                            }
                            break;
                        }
                    }
                    break;
                }
            }
            #endregion
            #region Player 2
            foreach (DeviceInstance dev in devices)
            {
                if (dev.InstanceGuid.ToString().ToLower() == Program.Settings.ControlSettings.Joypad2DeviceGuid)
                {
                    // We found the device !!
                    // Let's see if we have the settings for this device
                    foreach (IInputSettingsJoypad con in Program.Settings.ControlSettings.Joypad2Devices)
                    {
                        if (con.DeviceGuid.ToLower() == dev.InstanceGuid.ToString().ToLower())
                        {
                            // This is it !
                            switch (dev.Type)
                            {
                                case DeviceType.Keyboard:
                                    {
                                        joy2 = new NesJoypadPcKeyboardConnection(this.Handle, con);
                                        break;
                                    }
                                case DeviceType.Joystick:
                                    {
                                        joy2 = new NesJoypadPcJoystickConnection(this.Handle, dev.InstanceGuid.ToString(), con);
                                        break;
                                    }
                            }
                            break;
                        }
                    }
                    break;
                }
            }
            #endregion
            #region Player 3
            foreach (DeviceInstance dev in devices)
            {
                if (dev.InstanceGuid.ToString().ToLower() == Program.Settings.ControlSettings.Joypad3DeviceGuid)
                {
                    // We found the device !!
                    // Let's see if we have the settings for this device
                    foreach (IInputSettingsJoypad con in Program.Settings.ControlSettings.Joypad3Devices)
                    {
                        if (con.DeviceGuid.ToLower() == dev.InstanceGuid.ToString().ToLower())
                        {
                            // This is it !
                            switch (dev.Type)
                            {
                                case DeviceType.Keyboard:
                                    {
                                        joy3 = new NesJoypadPcKeyboardConnection(this.Handle, con);
                                        break;
                                    }
                                case DeviceType.Joystick:
                                    {
                                        joy3 = new NesJoypadPcJoystickConnection(this.Handle, dev.InstanceGuid.ToString(), con);
                                        break;
                                    }
                            }
                            break;
                        }
                    }
                    break;
                }
            }
            #endregion
            #region Player 4
            foreach (DeviceInstance dev in devices)
            {
                if (dev.InstanceGuid.ToString().ToLower() == Program.Settings.ControlSettings.Joypad4DeviceGuid)
                {
                    // We found the device !!
                    // Let's see if we have the settings for this device
                    foreach (IInputSettingsJoypad con in Program.Settings.ControlSettings.Joypad4Devices)
                    {
                        if (con.DeviceGuid.ToLower() == dev.InstanceGuid.ToString().ToLower())
                        {
                            // This is it !
                            switch (dev.Type)
                            {
                                case DeviceType.Keyboard:
                                    {
                                        joy4 = new NesJoypadPcKeyboardConnection(this.Handle, con);
                                        break;
                                    }
                                case DeviceType.Joystick:
                                    {
                                        joy4 = new NesJoypadPcJoystickConnection(this.Handle, dev.InstanceGuid.ToString(), con);
                                        break;
                                    }
                            }
                            break;
                        }
                    }
                    break;
                }
            }
            #endregion
            NesEmu.SetupJoypads(joy1, joy2, joy3, joy4);
            #region VSUnisystem DIP
            foreach (DeviceInstance dev in devices)
            {
                if (dev.InstanceGuid.ToString().ToLower() == Program.Settings.ControlSettings.VSUnisystemDIPDeviceGuid)
                {
                    // We found the device !!
                    // Let's see if we have the settings for this device
                    foreach (IInputSettingsVSUnisystemDIP con in Program.Settings.ControlSettings.VSUnisystemDIPDevices)
                    {
                        if (con.DeviceGuid.ToLower() == dev.InstanceGuid.ToString().ToLower())
                        {
                            // This is it !
                            switch (dev.Type)
                            {
                                case DeviceType.Keyboard:
                                    {
                                        NesEmu.SetupVSUnisystemDIP(new NesVSUnisystemDIPKeyboardConnection(this.Handle, con));
                                        break;
                                    }
                                case DeviceType.Joystick:
                                    {
                                        NesEmu.SetupVSUnisystemDIP(new NesVSUnisystemDIPJoystickConnection(this.Handle, dev.InstanceGuid.ToString(), con));
                                        break;
                                    }
                            }
                            break;
                        }
                    }
                    break;
                }
            }
            #endregion
            // ZAPPER
            NesEmu.SetupZapper(zapper = new ZapperConnecter(this.Handle, this.Location.X, this.Location.Y + menuStrip1.Height,
                panel_surface.Width, panel_surface.Height));
            video.SetupZapperBounds();
        }
        public void InitializeVideoRenderer()
        {
            video = new DirectXVideo(panel_surface);
            video.Initialize();
            NesEmu.SetupVideoRenderer(video);
        }
        public void InitializeSoundRenderer()
        {
            if (audio != null)
                audio.Dispose();
            audio = new DirectSoundRenderer(this.Handle);
            NesEmu.SetupSoundPlayback(audio, Program.Settings.Audio_SoundEnabled, Program.Settings.Audio_Frequency,
                audio.BufferSize, audio.latency_in_bytes);
        }
        public void InitializePalette()
        {
            // Load generator values
            NTSCPaletteGenerator.brightness = Program.Settings.Palette_NTSC_brightness;
            NTSCPaletteGenerator.contrast = Program.Settings.Palette_NTSC_contrast;
            NTSCPaletteGenerator.gamma = Program.Settings.Palette_NTSC_gamma;
            NTSCPaletteGenerator.hue_tweak = Program.Settings.Palette_NTSC_hue_tweak;
            NTSCPaletteGenerator.saturation = Program.Settings.Palette_NTSC_saturation;

            PALBPaletteGenerator.brightness = Program.Settings.Palette_PALB_brightness;
            PALBPaletteGenerator.contrast = Program.Settings.Palette_PALB_contrast;
            PALBPaletteGenerator.gamma = Program.Settings.Palette_PALB_gamma;
            PALBPaletteGenerator.hue_tweak = Program.Settings.Palette_PALB_hue_tweak;
            PALBPaletteGenerator.saturation = Program.Settings.Palette_PALB_saturation;
            // Let's see the settings
            if (Program.Settings.Palette_UseGenerators)
            {
                InitializePaletteGenerators();
            }
            else// Use custom palette
            {
                if (File.Exists(Program.Settings.Palette_FilePath))
                {
                    try
                    {
                        int[] palette = null;
                        if (PaletteFileWrapper.LoadFile(Program.Settings.Palette_FilePath, out palette))
                        {
                            NesEmu.SetPalette(palette);
                        }
                        else
                        {
                            ManagedMessageBox.ShowErrorMessage(Program.ResourceManager.GetString("Message_PaletteFileNotValid"));
                            Program.Settings.Palette_UseGenerators = true;
                            InitializePaletteGenerators();
                        }
                    }
                    catch (Exception ex)
                    {
                        ManagedMessageBox.ShowErrorMessage(ex.Message);
                        ManagedMessageBox.ShowErrorMessage(Program.ResourceManager.GetString("Message_PaletteFileNotValid"));
                        Program.Settings.Palette_UseGenerators = true;
                        InitializePaletteGenerators();
                    }
                }
                else// ERROR ! switch back to generators.
                {
                    ManagedMessageBox.ShowErrorMessage(Program.ResourceManager.GetString("Message_PaletteFileCantBeFound"));
                    Program.Settings.Palette_UseGenerators = true;
                    InitializePaletteGenerators();
                }
            }
        }
        private void InitializePaletteGenerators()
        {
            switch (Program.Settings.Palette_GeneratorUsageMode)
            {
                case PaletteGeneratorUsage.AUTO:
                    {
                        switch (NesEmu.TVFormat)
                        {
                            case TVSystem.DENDY:
                            case TVSystem.NTSC: NesEmu.SetPalette(NTSCPaletteGenerator.GeneratePalette()); break;
                            case TVSystem.PALB: NesEmu.SetPalette(PALBPaletteGenerator.GeneratePalette()); break;
                        }
                        break;
                    }
                case PaletteGeneratorUsage.NTSC: NesEmu.SetPalette(NTSCPaletteGenerator.GeneratePalette()); break;
                case PaletteGeneratorUsage.PAL: NesEmu.SetPalette(PALBPaletteGenerator.GeneratePalette()); break;
            }
        }
        private void ApplyEmuSettings()
        {
            NesEmu.SpeedLimitterON = true;
            NesEmu.SetupFrameSkip(Program.Settings.FrameSkip_Enabled, Program.Settings.FrameSkip_Reload);
            NesEmu.ApplySettings(Program.Settings.SaveSramOnShutdown, Program.Settings.Folder_Sram,
                Program.Settings.Folder_State, Program.Settings.Folder_Snapshots, Program.Settings.SnapshotFormat,
                Program.Settings.ReplaceSnapshot);
        }
        private void ApplyVideoStretch()
        {
            // Windowed stretch !
            if (Program.Settings.Video_StretchToMulti)
            {
                // Determine res
                int w = 256;
                int h = 240;
                if (Program.Settings.Video_CutLines)
                {
                    if (NesEmu.TVFormat == TVSystem.NTSC)
                        h = 224;
                    else
                        h = 238;
                }
                w *= Program.Settings.Video_StretchMulti;
                h *= Program.Settings.Video_StretchMulti;
                // Do the stretch !
                int windowW = w + 16;
                int windowH = h + 39;
                if (menuStrip1.Visible)
                    windowH += menuStrip1.Height;
                //if (toolStrip1.Visible)
                //    windowH += toolStrip1.Height;
                //if (statusStrip1.Visible)
                //    windowH += statusStrip1.Height;
                this.Size = new Size(windowW, windowH);

                //MessageBox.Show("Surface size is "+panel_surface.Width +" x "+panel_surface.Height);
            }
        }
        private void ResetWindowText()
        { try { this.Text = "My Nes"; } catch { } }

        private void NesEmu_EMUShutdown(object sender, EventArgs e)
        {
            if (!this.InvokeRequired)
                ResetWindowText();
            else
                try { this.Invoke(new Action(ResetWindowText)); }
                catch { }
        }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (NesEmu.EmulationON)
                NesEmu.EmulationPaused = true;
            OpenFileDialog op = new OpenFileDialog();
            op.Title = Program.ResourceManager.GetString("Title_OpenRom");
            op.Filter = Program.ResourceManager.GetString("Filter_Rom");
            if (op.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                OpenRom(op.FileName);
                if (NesEmu.EmulationON)
                    AddRecent(op.FileName);
            }
            else
            {
                if (NesEmu.EmulationON)
                    NesEmu.EmulationPaused = false;
            }
        }
        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            NesEmu.EmulationON = false;
            video.CloseThread();
            if (audio != null)
                audio.Dispose();
            SaveSettings();
        }
        private void hardResetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NesEmu.EMUHardReset();
        }
        private void softResetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NesEmu.EMUSoftReset();
        }
        private void audioSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (NesEmu.EmulationON)
                NesEmu.EmulationPaused = true;
            FormAudioSettings set = new FormAudioSettings();
            if (set.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                InitializeSoundRenderer();
            }
            if (NesEmu.EmulationON)
                NesEmu.EmulationPaused = false;
        }
        private void togglePauseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NesEmu.EmulationPaused = !NesEmu.EmulationPaused;
        }
        private void launcherToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form frm in this.OwnedForms)
            {
                if (frm.Tag.ToString() == "Launcher")
                {
                    frm.Activate();
                    return;
                }
            }
            FormLauncher newfrm = new FormLauncher();
            newfrm.Show(this);
        }
        private void shutdownToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (NesEmu.EmulationON)
                NesEmu.EmulationON = false;
        }
        private void romInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (NesEmu.EmulationON)
            {
                NesEmu.EmulationPaused = true;

                FormRomInfo frm = new FormRomInfo(NesEmu.GAMEFILE);
                frm.ShowDialog(this);

                NesEmu.EmulationPaused = false;
            }
            else
            {
                OpenFileDialog op = new OpenFileDialog();
                op.Title = Program.ResourceManager.GetString("Title_OpenRom");
                op.Filter = Program.ResourceManager.GetString("Filter_Rom");
                if (op.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                {
                    FormRomInfo frm = new FormRomInfo(op.FileName);
                    frm.ShowDialog(this);
                }
            }
        }
        private void pathsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (NesEmu.EmulationON)
                NesEmu.EmulationPaused = true;
            FormPathsSettings set = new FormPathsSettings();
            if (set.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                if (NesEmu.EmulationON)
                {
                    ApplyEmuSettings();
                }
            }
            if (NesEmu.EmulationON)
                NesEmu.EmulationPaused = false;
        }
        private void preferencesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (NesEmu.EmulationON)
                NesEmu.EmulationPaused = true;
            FormPreferences set = new FormPreferences();
            if (set.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                if (NesEmu.EmulationON)
                {
                    ApplyEmuSettings();
                }
            }
            if (NesEmu.EmulationON)
                NesEmu.EmulationPaused = false;
        }
        private void toolStripMenuItem8_Click(object sender, EventArgs e)
        {
            NesEmu.UpdateStateSlot(0);
        }
        private void toolStripMenuItem7_Click(object sender, EventArgs e)
        {
            NesEmu.UpdateStateSlot(1);
        }
        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            NesEmu.UpdateStateSlot(2);
        }
        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            NesEmu.UpdateStateSlot(3);
        }
        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            NesEmu.UpdateStateSlot(4);
        }
        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            NesEmu.UpdateStateSlot(5);
        }
        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            NesEmu.UpdateStateSlot(6);
        }
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            NesEmu.UpdateStateSlot(7);
        }
        private void toolStripMenuItem10_Click(object sender, EventArgs e)
        {
            NesEmu.UpdateStateSlot(8);
        }
        private void toolStripMenuItem9_Click(object sender, EventArgs e)
        {
            NesEmu.UpdateStateSlot(9);
        }
        private void stateToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            for (int i = 0; i < stateToolStripMenuItem.DropDownItems.Count; i++)
                if (stateToolStripMenuItem.DropDownItems[i] is ToolStripMenuItem)
                    ((ToolStripMenuItem)stateToolStripMenuItem.DropDownItems[i]).Checked = false;
            switch (NesEmu.STATESlot)
            {
                case 0: toolStripMenuItem8.Checked = true; break;
                case 1: toolStripMenuItem7.Checked = true; break;
                case 2: toolStripMenuItem6.Checked = true; break;
                case 3: toolStripMenuItem5.Checked = true; break;
                case 4: toolStripMenuItem4.Checked = true; break;
                case 5: toolStripMenuItem3.Checked = true; break;
                case 6: toolStripMenuItem2.Checked = true; break;
                case 7: toolStripMenuItem1.Checked = true; break;
                case 8: toolStripMenuItem10.Checked = true; break;
                case 9: toolStripMenuItem9.Checked = true; break;
            }
        }
        private void saveStateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (NesEmu.EmulationON)
                NesEmu.SaveState();
        }
        private void loadStateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (NesEmu.EmulationON)
                NesEmu.LoadState();
        }
        private void saveStateAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (NesEmu.EmulationON)
            {
                NesEmu.EmulationPaused = true;
                try
                {
                    SaveFileDialog sav = new SaveFileDialog();
                    sav.Title = Program.ResourceManager.GetString("Title_SaveStateAs");
                    sav.Filter = Program.ResourceManager.GetString("Filter_MyNesState");
                    if (sav.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                        NesEmu.SaveStateAs(sav.FileName);
                }
                catch (Exception ex)
                {
                    ManagedMessageBox.ShowErrorMessage(ex.Message);
                }
                NesEmu.EmulationPaused = false;
            }
        }
        private void loadStateAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (NesEmu.EmulationON)
            {
                NesEmu.EmulationPaused = true;
                try
                {
                    OpenFileDialog op = new OpenFileDialog();
                    op.Title = Program.ResourceManager.GetString("Title_LoadStateAs");
                    op.Filter = Program.ResourceManager.GetString("Filter_MyNesState");
                    if (op.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                        NesEmu.LoadStateAs(op.FileName);
                }
                catch (Exception ex)
                {
                    ManagedMessageBox.ShowErrorMessage(ex.Message);
                }
                NesEmu.EmulationPaused = false;
            }
        }
        private void FormMain_ResizeBegin(object sender, EventArgs e)
        {
            if (NesEmu.EmulationON)
            {
                NesEmu.EmulationPaused = true;

                NesEmu.videoOut.OnResizeBegin();
            }
            else
            {
                video.threadPAUSED = true;
                video.OnResizeBegin();
            }
        }
        private void FormMain_ResizeEnd(object sender, EventArgs e)
        {
            if (NesEmu.EmulationON)
            {
                NesEmu.EmulationPaused = true;

                NesEmu.videoOut.OnResizeEnd();

                NesEmu.EmulationPaused = false;
            }
            else
            {
                video.OnResizeEnd();
            }
        }
        private void fullscreenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (NesEmu.EmulationON)
            {
                NesEmu.EmulationPaused = true;

                NesEmu.videoOut.SwitchFullscreen();

                NesEmu.EmulationPaused = false;
            }
            else
            {
                Program.Settings.Video_StartFullscreen = !Program.Settings.Video_StartFullscreen;
            }
        }
        private void videoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (NesEmu.EmulationON)
                NesEmu.EmulationPaused = true;
            FormVideoSettings set = new FormVideoSettings();
            if (set.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                if (NesEmu.EmulationON)
                {
                    video.threadPAUSED = true;
                    // Apply video stretch
                    ApplyVideoStretch();
                    if (NesEmu.videoOut != null)
                        video.Reset();
                }
            }
            if (NesEmu.EmulationON)
                NesEmu.EmulationPaused = false;
        }
        private void settingsToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            fullscreenToolStripMenuItem.Checked = Program.Settings.Video_StartFullscreen;
            turboToolStripMenuItem.Checked = !NesEmu.SpeedLimitterON;
            connect4PlayersToolStripMenuItem.Checked = NesEmu.IsFourPlayers;
            connectZapperToolStripMenuItem.Checked = NesEmu.IsZapperConnected;
        }
        private void FormMain_Activated(object sender, EventArgs e)
        {
            if (NesEmu.EmulationON && Program.Settings.PauseAtFocusLost)
                NesEmu.EmulationPaused = false;
        }
        private void FormMain_Deactivate(object sender, EventArgs e)
        {
            if (NesEmu.EmulationON && Program.Settings.PauseAtFocusLost)
                NesEmu.EmulationPaused = true;
        }
        private void takesnapshotToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (NesEmu.EmulationON)
                NesEmu.TakeSnapshot();
        }
        private void inputsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (NesEmu.EmulationON)
                NesEmu.EmulationPaused = true;
            FormInputSettings set = new FormInputSettings();
            if (set.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                if (NesEmu.EmulationON)
                {
                    InitializeInputRenderer();
                }
            }
            if (NesEmu.EmulationON)
                NesEmu.EmulationPaused = false;
        }
        private void palettesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (NesEmu.EmulationON)
                NesEmu.EmulationPaused = true;
            FormPaletteSettings frm = new FormPaletteSettings();
            if (frm.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                InitializePalette();
            }
            if (NesEmu.EmulationON)
                NesEmu.EmulationPaused = false;
        }
        private void fileToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            recordSoundToolStripMenuItem.Enabled = NesEmu.EmulationON;
            recordSoundToolStripMenuItem.Text = audio.IsRecording ?
                Program.ResourceManager.GetString("Button_StopSoundRecording") :
                Program.ResourceManager.GetString("Button_RecordSound");
            openRecentToolStripMenuItem.Enabled = Program.Settings.RecentPlayed.Count > 0;
            takesnapshotToolStripMenuItem.Enabled = NesEmu.EmulationON;
        }
        private void recordSoundToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!NesEmu.EmulationON)
                return;
            if (audio.IsRecording)
                audio.RecordStop();
            else
            {
                NesEmu.EmulationPaused = true;
                SaveFileDialog sav = new SaveFileDialog();
                sav.Title = Program.ResourceManager.GetString("Title_SaveWavFile");
                sav.Filter = Program.ResourceManager.GetString("Filter_Wav");
                if (sav.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                {
                    audio.Record(sav.FileName);
                    NesEmu.EmulationPaused = false;
                }
            }
        }
        private void turboToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NesEmu.SpeedLimitterON = !NesEmu.SpeedLimitterON;
        }
        private void frameSkipToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            for (int i = 0; i < frameSkipToolStripMenuItem.DropDownItems.Count; i++)
                ((ToolStripMenuItem)frameSkipToolStripMenuItem.DropDownItems[i]).Checked = false;
            if (!Program.Settings.FrameSkip_Enabled)
                noneToolStripMenuItem.Checked = true;
            else
            {
                switch (Program.Settings.FrameSkip_Reload)
                {
                    case 1: toolStripMenuItem12.Checked = true; break;
                    case 2: toolStripMenuItem13.Checked = true; break;
                    case 3: toolStripMenuItem14.Checked = true; break;
                    case 4: toolStripMenuItem15.Checked = true; break;
                    case 5: toolStripMenuItem16.Checked = true; break;
                    case 6: toolStripMenuItem17.Checked = true; break;
                    case 7: toolStripMenuItem18.Checked = true; break;
                    case 8: toolStripMenuItem19.Checked = true; break;
                    case 9: toolStripMenuItem20.Checked = true; break;
                }
            }
        }
        private void noneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.Settings.FrameSkip_Enabled = false;
            NesEmu.SetupFrameSkip(Program.Settings.FrameSkip_Enabled, Program.Settings.FrameSkip_Reload);
        }
        private void toolStripMenuItem12_Click(object sender, EventArgs e)
        {
            Program.Settings.FrameSkip_Enabled = true;
            Program.Settings.FrameSkip_Reload = 1;
            NesEmu.SetupFrameSkip(Program.Settings.FrameSkip_Enabled, Program.Settings.FrameSkip_Reload);
        }
        private void toolStripMenuItem13_Click(object sender, EventArgs e)
        {
            Program.Settings.FrameSkip_Enabled = true;
            Program.Settings.FrameSkip_Reload = 2;
            NesEmu.SetupFrameSkip(Program.Settings.FrameSkip_Enabled, Program.Settings.FrameSkip_Reload);
        }
        private void toolStripMenuItem14_Click(object sender, EventArgs e)
        {
            Program.Settings.FrameSkip_Enabled = true;
            Program.Settings.FrameSkip_Reload = 3;
            NesEmu.SetupFrameSkip(Program.Settings.FrameSkip_Enabled, Program.Settings.FrameSkip_Reload);
        }
        private void toolStripMenuItem15_Click(object sender, EventArgs e)
        {
            Program.Settings.FrameSkip_Enabled = true;
            Program.Settings.FrameSkip_Reload = 4;
            NesEmu.SetupFrameSkip(Program.Settings.FrameSkip_Enabled, Program.Settings.FrameSkip_Reload);
        }
        private void toolStripMenuItem16_Click(object sender, EventArgs e)
        {
            Program.Settings.FrameSkip_Enabled = true;
            Program.Settings.FrameSkip_Reload = 5;
            NesEmu.SetupFrameSkip(Program.Settings.FrameSkip_Enabled, Program.Settings.FrameSkip_Reload);
        }
        private void toolStripMenuItem17_Click(object sender, EventArgs e)
        {
            Program.Settings.FrameSkip_Enabled = true;
            Program.Settings.FrameSkip_Reload = 6;
            NesEmu.SetupFrameSkip(Program.Settings.FrameSkip_Enabled, Program.Settings.FrameSkip_Reload);
        }
        private void toolStripMenuItem18_Click(object sender, EventArgs e)
        {
            Program.Settings.FrameSkip_Enabled = true;
            Program.Settings.FrameSkip_Reload = 7;
            NesEmu.SetupFrameSkip(Program.Settings.FrameSkip_Enabled, Program.Settings.FrameSkip_Reload);
        }
        private void toolStripMenuItem19_Click(object sender, EventArgs e)
        {
            Program.Settings.FrameSkip_Enabled = true;
            Program.Settings.FrameSkip_Reload = 8;
            NesEmu.SetupFrameSkip(Program.Settings.FrameSkip_Enabled, Program.Settings.FrameSkip_Reload);
        }
        private void toolStripMenuItem20_Click(object sender, EventArgs e)
        {
            Program.Settings.FrameSkip_Enabled = true;
            Program.Settings.FrameSkip_Reload = 9;
            NesEmu.SetupFrameSkip(Program.Settings.FrameSkip_Enabled, Program.Settings.FrameSkip_Reload);
        }
        private void regionToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            for (int i = 0; i < regionToolStripMenuItem.DropDownItems.Count; i++)
                ((ToolStripMenuItem)regionToolStripMenuItem.DropDownItems[i]).Checked = false;
            switch (Program.Settings.TVSystemSetting)
            {
                case TVSystemSetting.AUTO: aUTOToolStripMenuItem.Checked = true; break;
                case TVSystemSetting.DENDY: dENDYToolStripMenuItem.Checked = true; break;
                case TVSystemSetting.NTSC: nTSCToolStripMenuItem.Checked = true; break;
                case TVSystemSetting.PALB: pALBToolStripMenuItem.Checked = true; break;
            }
        }
        private void aUTOToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.Settings.TVSystemSetting = TVSystemSetting.AUTO;
            if (NesEmu.EmulationON)
            {
                NesEmu.EmulationPaused = true;
                ManagedMessageBox.ShowMessage(Program.ResourceManager.GetString("Message_YouNeedToHardResetCurrentGameToApplyRegionSettings"));
                NesEmu.EmulationPaused = false;
            }
        }
        private void nTSCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.Settings.TVSystemSetting = TVSystemSetting.NTSC;
            if (NesEmu.EmulationON)
            {
                NesEmu.EmulationPaused = true;
                ManagedMessageBox.ShowMessage(Program.ResourceManager.GetString("Message_YouNeedToHardResetCurrentGameToApplyRegionSettings"));
                NesEmu.EmulationPaused = false;
            }
        }
        private void pALBToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.Settings.TVSystemSetting = TVSystemSetting.PALB;
            if (NesEmu.EmulationON)
            {
                NesEmu.EmulationPaused = true;
                ManagedMessageBox.ShowMessage(Program.ResourceManager.GetString("Message_YouNeedToHardResetCurrentGameToApplyRegionSettings"));
                NesEmu.EmulationPaused = false;
            }
        }
        private void dENDYToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.Settings.TVSystemSetting = TVSystemSetting.DENDY;
            if (NesEmu.EmulationON)
            {
                NesEmu.EmulationPaused = true;
                ManagedMessageBox.ShowMessage(Program.ResourceManager.GetString("Message_YouNeedToHardResetCurrentGameToApplyRegionSettings"));
                NesEmu.EmulationPaused = false;
            }
        }
        private void openRecentToolStripMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            string path = e.ClickedItem.Tag.ToString();
            if (File.Exists(path))
            {
                OpenRom(path);
                if (NesEmu.EmulationON)
                    AddRecent(path);
            }
            else
            {
                ManagedMessageBoxResult res = ManagedMessageBox.ShowQuestionMessage(
                    Program.ResourceManager.GetString("Message_ThisFileIsNoLongerExistDoYouWantToDeleteItFromTheRecentList"));
                if (res.ClickedButtonIndex == 0)
                {
                    for (int i = 0; i < Program.Settings.RecentPlayed.Count; i++)
                    {
                        if (Program.Settings.RecentPlayed[i] == path)
                        {
                            Program.Settings.RecentPlayed.RemoveAt(i);
                            break;
                        }
                    }
                    RefreshRecent();
                }
            }
        }
        private void gameGenieToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!NesEmu.EmulationON)
            {
                ManagedMessageBox.ShowErrorMessage(Program.ResourceManager.GetString("Message_GameGenieCantBeUSedWhileEmuOf"));
                return;
            }
            NesEmu.EmulationPaused = true;
            FormGameGenie frm = new FormGameGenie();
            frm.ShowDialog(this);
            NesEmu.EmulationPaused = false;
        }
        private void languageToolStripMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            NesEmu.EmulationPaused = true;
            int i = 0;
            int index = 0;
            foreach (ToolStripMenuItem item in languageToolStripMenuItem.DropDownItems)
            {
                if (item.Text == e.ClickedItem.Text)
                {
                    item.Checked = true;
                    index = i;
                }
                else
                    item.Checked = false;
                i++;
            }
            Program.Settings.Language = Program.SupportedLanguages[index, 0];
            Program.Settings.Save();
            ManagedMessageBox.ShowMessage(Program.ResourceManager.GetString("Message_YouMustRestartTheProgramToApplyLanguage"),
                Program.ResourceManager.GetString("MessageCaption_ApplyLanguage"));
            NesEmu.EmulationPaused = false;
        }
        private void menuStrip1_MenuActivate(object sender, EventArgs e)
        {
            NesEmu.EmulationPaused = true;
        }
        private void menuStrip1_MenuDeactivate(object sender, EventArgs e)
        {
            NesEmu.EmulationPaused = false;
        }
        private void visitWebsiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("http://sourceforge.net/projects/mynes/");
            }
            catch { }
        }
        private void facebookPageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("http://www.facebook.com/pages/My-Nes/427707727244076");
            }
            catch { }
        }
        private void viewHelpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try { System.Diagnostics.Process.Start(".\\Help\\index.htm"); }
            catch(Exception ex)
            { ManagedMessageBox.ShowErrorMessage(ex.Message); }
        }
        private void aboutMyNesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NesEmu.EmulationPaused = true;

            FormAbout frm = new FormAbout();
            frm.ShowDialog(this);

            NesEmu.EmulationPaused = false;
        }
        private void codeprojectArticleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("http://www.codeproject.com/KB/game/MyNes_NitendoEmulator.aspx");
            }
            catch { }
        }
        private void connect4PlayersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NesEmu.IsFourPlayers = !NesEmu.IsFourPlayers;
        }
        private void connectZapperToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NesEmu.IsZapperConnected = !NesEmu.IsZapperConnected;
        }
        private void statusToolStripMenuItem_Click(object sender, EventArgs e)
        {
            video.ShowEmulationStatus();
        }
    }
}
