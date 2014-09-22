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
using System.Linq;
using System.IO;
using System.Windows.Forms;
using System.Globalization;
using System.Threading;
using System.Resources;
using System.Reflection;
using System.Diagnostics;
using MyNes.Core;
namespace MyNes
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] Args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
        
            // Add listeners
            System.Diagnostics.Trace.Listeners.Add(new System.Diagnostics.ConsoleTraceListener());
            // Detect languages
            DetectSupportedLanguages();
            // Load settings
            Settings = new Properties.Settings();
            Settings.Reload();
            // First run ?
            if (!Program.Settings.FirstRun)
            {
                FixFolders();
                FixPalette();
                ControlMappingSettings.BuildDefaultControlSettings();
                try
                {
                    FormFirstRun frm = new FormFirstRun();
                    frm.ShowDialog();

                    Program.Settings.FirstRun = true;
                }
                catch (Exception ex)
                {
                    MMB.ManagedMessageBox.ShowErrorMessage(ex.Message);
                }
            }
            // Set language
            Language = Settings.Language;
            ResourceManager = new ResourceManager("MyNes.LanguageResources.Resource",
              Assembly.GetExecutingAssembly());

            // Start-up nes emulation engine
            MyNes.Core.NesEmu.WarmUp();

            // Create the main form
            FormMain = new FormMain();

            // Do command lines
            DoCommandLines(Args);

            // Run !
            Application.Run(FormMain);
        }
        private static void DoCommandLines(string[] args)
        {
            if (args == null) return;
            if (args.Length == 0) return;

            // Loop through commands and execute them one by one
            for (int i = 0; i < args.Length; i++)
            {
                switch (args[i].ToLower())
                {
                    case "/audio_on": Program.Settings.Audio_SoundEnabled = true; FormMain.InitializeSoundRenderer(); break;
                    case "/audio_off": Program.Settings.Audio_SoundEnabled = false; FormMain.InitializeSoundRenderer(); break;
                    case "/f_skip_off": Program.Settings.FrameSkip_Enabled = false; break;
                    case "/f_skip_1": Program.Settings.FrameSkip_Enabled = true; Program.Settings.FrameSkip_Reload = 1; break;
                    case "/f_skip_2": Program.Settings.FrameSkip_Enabled = true; Program.Settings.FrameSkip_Reload = 2; break;
                    case "/f_skip_3": Program.Settings.FrameSkip_Enabled = true; Program.Settings.FrameSkip_Reload = 3; break;
                    case "/f_skip_4": Program.Settings.FrameSkip_Enabled = true; Program.Settings.FrameSkip_Reload = 4; break;
                    case "/f_skip_5": Program.Settings.FrameSkip_Enabled = true; Program.Settings.FrameSkip_Reload = 5; break;
                    case "/f_skip_6": Program.Settings.FrameSkip_Enabled = true; Program.Settings.FrameSkip_Reload = 6; break;
                    case "/f_skip_7": Program.Settings.FrameSkip_Enabled = true; Program.Settings.FrameSkip_Reload = 7; break;
                    case "/f_skip_8": Program.Settings.FrameSkip_Enabled = true; Program.Settings.FrameSkip_Reload = 8; break;
                    case "/f_skip_9": Program.Settings.FrameSkip_Enabled = true; Program.Settings.FrameSkip_Reload = 9; break;
                    case "/pal_use_gen": Program.Settings.Palette_UseGenerators = true; break;
                    case "/pal_dontuse_gen": Program.Settings.Palette_UseGenerators = false; break;
                    case "/pal_gen_auto": Program.Settings.Palette_GeneratorUsageMode = PaletteGeneratorUsage.AUTO; break;
                    case "/pal_gen_ntsc": Program.Settings.Palette_GeneratorUsageMode = PaletteGeneratorUsage.NTSC; break;
                    case "/pal_gen_pal": Program.Settings.Palette_GeneratorUsageMode = PaletteGeneratorUsage.PAL; break;
                    case "/sram_save_on": Program.Settings.SaveSramOnShutdown = true; break;
                    case "/sram_save_off": Program.Settings.SaveSramOnShutdown = false; break;
                    case "/show_issues_on": Program.Settings.ShowRomIssuesIfHave = true; break;
                    case "/show_issues_off": Program.Settings.ShowRomIssuesIfHave = true; break;
                    case "/tv_auto": Program.Settings.TVSystemSetting = Core.TVSystemSetting.AUTO; break;
                    case "/tv_ntsc": Program.Settings.TVSystemSetting = Core.TVSystemSetting.NTSC; break;
                    case "/tv_pal": Program.Settings.TVSystemSetting = Core.TVSystemSetting.PALB; break;
                    case "/tv_dendy": Program.Settings.TVSystemSetting = Core.TVSystemSetting.DENDY; break;
                    case "/vid_hide_lines_on": Program.Settings.Video_CutLines = true; break;
                    case "/vid_hide_lines_off": Program.Settings.Video_CutLines = false; break;
                    case "/vid_filter_point": Program.Settings.Video_Filter = SlimDX.Direct3D9.TextureFilter.Point; break;
                    case "/vid_filter_none": Program.Settings.Video_Filter = SlimDX.Direct3D9.TextureFilter.None; break;
                    case "/vid_filter_linear": Program.Settings.Video_Filter = SlimDX.Direct3D9.TextureFilter.Linear; break;
                    case "/vid_vertex_hardware": Program.Settings.Video_HardwareVertexProcessing = true; break;
                    case "/vid_vertex_software": Program.Settings.Video_HardwareVertexProcessing = false; break;
                    case "/vid_keep_apsect_on": Program.Settings.Video_KeepAspectRatio = true; break;
                    case "/vid_keep_apsect_off": Program.Settings.Video_KeepAspectRatio = false; break;
                    case "/vid_fps_on": Program.Settings.Video_ShowFPS = true; break;
                    case "/vid_fps_off": Program.Settings.Video_ShowFPS = false; break;
                    case "/vid_noti_on": Program.Settings.Video_ShowNotifications = true; break;
                    case "/vid_noti_off": Program.Settings.Video_ShowNotifications = false; break;
                    case "/vid_fullscreen": Program.Settings.Video_StartFullscreen = true; break;
                    case "/vid_wind": Program.Settings.Video_StartFullscreen = false; break;
                    case "/vid_stretch_wind_on": Program.Settings.Video_StretchToMulti = true; break;
                    case "/vid_stretch_wind_off": Program.Settings.Video_StretchToMulti = false; break;
                    case "/state_slot_0": NesEmu.STATESlot = 0; break;
                    case "/state_slot_1": NesEmu.STATESlot = 1; break;
                    case "/state_slot_2": NesEmu.STATESlot = 2; break;
                    case "/state_slot_3": NesEmu.STATESlot = 3; break;
                    case "/state_slot_4": NesEmu.STATESlot = 4; break;
                    case "/state_slot_5": NesEmu.STATESlot = 5; break;
                    case "/state_slot_6": NesEmu.STATESlot = 6; break;
                    case "/state_slot_7": NesEmu.STATESlot = 7; break;
                    case "/state_slot_8": NesEmu.STATESlot = 8; break;
                    case "/state_slot_9": NesEmu.STATESlot = 9; break;
                    case "/state_load": NesEmu.LoadState(); break;// Request a state load on the first rendered frame !
                }
            }

            // First command must be file path, run it here to apply commands first.
            if (File.Exists(args[0]))
                FormMain.OpenRom(args[0]);
        }
        public static void FixFolders()
        {
            Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\MyNes\\");
            Program.Settings.Folder_Sram = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\MyNes\\SRAM\\";
            Directory.CreateDirectory(Program.Settings.Folder_Sram);
            Program.Settings.Folder_State = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\MyNes\\STATE\\";
            Directory.CreateDirectory(Program.Settings.Folder_State);
            Program.Settings.Folder_Snapshots = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\MyNes\\SNAPS\\";
            Directory.CreateDirectory(Program.Settings.Folder_Snapshots);
        }
        public static void FixPalette()
        {
            Program.Settings.Palette_UseGenerators = true;
            Directory.CreateDirectory(Application.StartupPath + "\\Palettes\\");
            // Load all palette files
            string[] files = Directory.GetFiles(Application.StartupPath + "\\Palettes\\");
            // Set the first one
            for (int i = 0; i < files.Length; i++)
            {
                if (Path.GetExtension(files[i]).ToLower() == ".pal")
                {
                    Program.Settings.Palette_FilePath = files[i];
                    break;
                }
            }
        }
        private static void DetectSupportedLanguages()
        {
            string[] langsFolders = Directory.GetDirectories(Application.StartupPath);
            List<string> ids = new List<string>();
            List<string> englishNames = new List<string>();
            List<string> NativeNames = new List<string>();
            foreach (string folder in langsFolders)
            {
                try
                {
                    CultureInfo inf = new CultureInfo(Path.GetFileName(folder));
                    // no errors lol add the id
                    ids.Add(Path.GetFileName(folder));
                    englishNames.Add(inf.EnglishName);
                    NativeNames.Add(inf.NativeName);
                }
                catch { }
            }
            if (ids.Count > 0)
            {
                SupportedLanguages = new string[ids.Count, 3];
                for (int i = 0; i < ids.Count; i++)
                {
                    SupportedLanguages[i, 0] = englishNames[i];
                    SupportedLanguages[i, 1] = ids[i];
                    SupportedLanguages[i, 2] = NativeNames[i];
                }
            }
        }
        // Properties
        public static string Version
        {
            get { return Assembly.GetExecutingAssembly().GetName().Version.ToString(); }
        }
        public static Properties.Settings Settings
        { get; private set; }
        public static FormMain FormMain
        { get; private set; }
        public static ResourceManager ResourceManager
        { get; private set; }
        public static string[,] SupportedLanguages
        { get; private set; }
        public static string Language
        {
            get
            {
                return Thread.CurrentThread.CurrentUICulture.NativeName;
            }
            set
            {
                for (int i = 0; i < Program.SupportedLanguages.Length / 3; i++)
                {
                    if (Program.SupportedLanguages[i, 0] == value)
                    {
                        Thread.CurrentThread.CurrentUICulture = new CultureInfo(Program.SupportedLanguages[i, 1]);
                        break;
                    }
                }
            }
        }
        public static CultureInfo CultureInfo
        {
            get
            {
                for (int i = 0; i < Program.SupportedLanguages.Length / 3; i++)
                {
                    if (Program.SupportedLanguages[i, 0] == Program.Settings.Language)
                    {
                        return new CultureInfo(Program.SupportedLanguages[i, 1]);
                    }
                }
                return Thread.CurrentThread.CurrentUICulture;
            }
        }
    }
}
