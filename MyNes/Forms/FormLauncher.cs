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
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Security.Cryptography;
using MLV;
using MMB;
using MyNes.Core;
using SevenZip;

namespace MyNes
{
    public partial class FormLauncher : Form
    {
        public FormLauncher()
        {
            InitializeComponent();
            this.Tag = "Launcher";
            InitializeControls();
            LoadSettings();
            LoadDatabase();

            NesEmu.EMUShutdown += NesEmu_EMUShutdown;

            if (Program.Settings.LauncherRememberLastSelection)
            {
                if (Program.Settings.LauncherLatestSelection >= 0 && Program.Settings.LauncherLatestSelection < managedListView1.Items.Count)
                {
                    managedListView1.Items[Program.Settings.LauncherLatestSelection].Selected = true;
                    managedListView1.ScrollToItem(managedListView1.Items[Program.Settings.LauncherLatestSelection]);

                    selectionTimer = 3;
                    timer_selection.Start();
                }
            }
        }
        // Controls
        private GameInfoViewer gameInfoViewer;
        private ImagesViewer imagesViewer_snaps;
        private ImagesViewer imagesViewer_covers;
        private InfoViewer infoViewer;
        private ManualsViewer manualsViewer;

        private enum DBMode { ALL, DATABASE, NOTDB, FILES, MISSING }
        private string _generate_dbName;
        private string _generate_dbPath;
        private bool _generate_dbMakeNesCartEntries;
        private Thread _generate_thread;
        private string[] _assign_folders;
        private bool _assign_subfolders;
        private bool _assign_addFilesNotFound;
        private bool _assign_update_entries_already_assigned;
        private bool _thread_busy;// Set to true when a thread in progress. Should be checked before doing any thread.
        private FormGeneratingDatabase frmG;
        private string currentPlayedGameId = "";
        private int currentPlayedGameIndex = -1;
        private int playTime;
        private bool isPlayingGame = false;
        private DBMode mode;
        private int searchTimer = 3;
        private bool performQuickSearch;
        private string quickSearchText;
        private bool performSearch;
        private SearchRequestArgs searchArgs;
        private string status;
        private int progress;
        private int selectionTimer = 0;
        private bool doSort = false;
        private string sortColumnName;
        private bool sortAZ;

        private void InitializeControls()
        {
            // We should do controls initialize in this way 'cause
            // VS designer as stupid as shit !
            this.gameInfoViewer = new GameInfoViewer();
            this.imagesViewer_snaps = new ImagesViewer();
            this.imagesViewer_covers = new ImagesViewer();
            this.infoViewer = new InfoViewer();
            this.manualsViewer = new ManualsViewer();
            // 
            // gameInfoViewer
            // 
            this.gameInfoViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gameInfoViewer.Location = new System.Drawing.Point(3, 3);
            this.gameInfoViewer.Name = "gameInfoViewer";
            this.gameInfoViewer.Size = new System.Drawing.Size(215, 175);
            this.gameInfoViewer.TabIndex = 0;
            this.gameInfoViewer.RatingChanged += gameInfoViewer_RatingChanged;
            this.tabPage_game.Controls.Add(this.gameInfoViewer);
            // 
            // imagesViewer_snaps
            // 
            this.imagesViewer_snaps.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imagesViewer_snaps.Location = new System.Drawing.Point(3, 3);
            this.imagesViewer_snaps.MODE = MyNes.DetectMode.SNAPS;
            this.imagesViewer_snaps.Name = "imagesViewer_snaps";
            this.imagesViewer_snaps.Size = new System.Drawing.Size(215, 175);
            this.imagesViewer_snaps.TabIndex = 0;
            this.tabPage_snapshots.Controls.Add(this.imagesViewer_snaps);
            // 
            // imagesViewer_covers
            // 
            this.imagesViewer_covers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imagesViewer_covers.Location = new System.Drawing.Point(3, 3);
            this.imagesViewer_covers.MODE = MyNes.DetectMode.COVERS;
            this.imagesViewer_covers.Name = "imagesViewer_covers";
            this.imagesViewer_covers.Size = new System.Drawing.Size(215, 175);
            this.imagesViewer_covers.TabIndex = 0;
            this.tabPage_covers.Controls.Add(this.imagesViewer_covers);
            //
            // infoViewer
            //
            this.infoViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.infoViewer.Location = new System.Drawing.Point(3, 3);
            this.infoViewer.Name = "infoViewer";
            this.infoViewer.Size = new System.Drawing.Size(215, 175);
            this.infoViewer.TabIndex = 0;
            this.tabPage_info.Controls.Add(this.infoViewer);
            //
            // manualsViewer
            //
            this.manualsViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.manualsViewer.Location = new System.Drawing.Point(3, 3);
            this.manualsViewer.Name = "manualsViewer";
            this.manualsViewer.Size = new System.Drawing.Size(215, 175);
            this.manualsViewer.TabIndex = 0;
            this.tabPage_manuals.Controls.Add(this.manualsViewer);
        }
        private void LoadSettings()
        {
            this.Location = Program.Settings.LauncherLocation;
            this.Size = Program.Settings.LauncherSize;
            this.splitContainer1.SplitterDistance = Program.Settings.LauncherSpliter1;
            this.splitContainer2.SplitterDistance = Program.Settings.LauncherSpliter2;
        }
        private void SaveSettings()
        {
            Program.Settings.LauncherLocation = this.Location;
            Program.Settings.LauncherSize = this.Size;
            Program.Settings.LauncherSpliter1 = this.splitContainer1.SplitterDistance;
            Program.Settings.LauncherSpliter2 = this.splitContainer2.SplitterDistance;
            if (managedListView1.SelectedItems.Count > 0)
                Program.Settings.LauncherLatestSelection = managedListView1.Items.IndexOf(managedListView1.SelectedItems[0]);
            else
                Program.Settings.LauncherLatestSelection = -1;
        }

        #region Database generate and load
        private void LoadDatabase()
        {
            if (!File.Exists(Path.GetFullPath(Program.Settings.Database_FilePath)))
            {
                GenerateDatabase();
                return;
            }
            else
            {
                // Open the database !
                MyNesDB.OpenDatabase(Path.GetFullPath(Program.Settings.Database_FilePath));
                // Refresh original columns
                RefreshColumns();
                // Refresh the entries.
                RefreshEntries();
            }
        }
        private void GenerateDatabase()
        {
            if (NesEmu.EmulationON)
                NesEmu.EmulationPaused = true;
            FormCreateDatabase frm = new FormCreateDatabase();
            if (frm.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                mode = DBMode.ALL;
                Button_mode_all.Checked = true;
                Button_mode_database.Checked = false;
                Button_mode_files.Checked = false;
                Button_mode_missing.Checked = false;
                Button_mode_notDB.Checked = false;
                _generate_dbName = frm.DBName;
                _generate_dbPath = frm.DBPath;
                _generate_dbMakeNesCartEntries = frm.DBGenerateNesCart;
                _generate_thread = new Thread(new ThreadStart(GenerateDatabaseThreaded));
                _generate_thread.CurrentUICulture = Program.CultureInfo;
                _generate_thread.Start();
                // Show working
                frmG = new FormGeneratingDatabase(true);
                frmG.ShowDialog(this);
            }
        }
        private void GenerateDatabaseThreaded()
        {
            _thread_busy = true;
            // Create the database !
            MyNesDB.CreateDatabase(_generate_dbName, _generate_dbPath);
            // Generate entries from NesCart database !
            if (_generate_dbMakeNesCartEntries)
                GenerateEntriesFromNesCart();
            RefreshColumnsThreaded();
            RefreshEntriesThreaded();
            CloseWorkFormThreaded();
        }
        private void GenerateEntriesFromNesCart()
        {
            // Adding entries from the NesCart database.
            Trace.WriteLine(Program.ResourceManager.GetString("Status_GeneratingDefaultEntriesFromNesCartDB"));
            int x = 0;
            if (NesCartDatabase.DatabaseRoms != null)
            {
                // Get the dataset
                DataSet ds = MyNesDB.GetDataSet("GAMES");
                for (int i = 0; i < NesCartDatabase.DatabaseRoms.Count; i++)
                {
                    if (frmG.CancelRequest)
                    {
                        break;
                    }
                    MyNesDBEntryInfo infEntry = new MyNesDBEntryInfo();
                    infEntry.IsDB = true;
                    infEntry.AlternativeName = NesCartDatabase.DatabaseRoms[i].Game_AltName;
                    if (NesCartDatabase.DatabaseRoms[i].Cartridges != null)
                    {
                        infEntry.BoardMapper = Convert.ToInt32(NesCartDatabase.DatabaseRoms[i].Cartridges[0].Board_Mapper);
                        foreach (NesCartDatabaseCartridgeInfo crt in NesCartDatabase.DatabaseRoms[i].Cartridges)
                        {
                            if (infEntry.BoardPcb != null)
                            {
                                if (!infEntry.BoardPcb.Contains(crt.Board_Pcb))
                                    infEntry.BoardPcb += crt.Board_Pcb + ", ";
                            }
                            else
                                infEntry.BoardPcb = crt.Board_Pcb + ", ";
                            if (infEntry.BoardType != null)
                            {
                                if (!infEntry.BoardType.Contains(crt.Board_Type))
                                    infEntry.BoardType += crt.Board_Type + ", ";
                            }
                            else
                                infEntry.BoardType = crt.Board_Type + ", ";
                        }
                    }

                    infEntry.Catalog = NesCartDatabase.DatabaseRoms[i].Game_Catalog;
                    infEntry.Class = NesCartDatabase.DatabaseRoms[i].Game_Class;
                    infEntry.Developer = NesCartDatabase.DatabaseRoms[i].Game_Developer;
                    infEntry.Name = NesCartDatabase.DatabaseRoms[i].Game_Name;
                    infEntry.Players = NesCartDatabase.DatabaseRoms[i].Game_Players;
                    infEntry.Publisher = NesCartDatabase.DatabaseRoms[i].Game_Publisher;
                    infEntry.Region = NesCartDatabase.DatabaseRoms[i].Game_Region;
                    infEntry.ReleaseDate = NesCartDatabase.DatabaseRoms[i].Game_ReleaseDate;
                    for (int j = 0; j < NesCartDatabase.DatabaseRoms[i].Cartridges.Count; j++)
                    {
                        infEntry.CRC = NesCartDatabase.DatabaseRoms[i].Cartridges[j].CRC;
                        infEntry.DateDumped = NesCartDatabase.DatabaseRoms[i].Cartridges[j].DateDumped;
                        infEntry.Dump = NesCartDatabase.DatabaseRoms[i].Cartridges[j].Dump;
                        infEntry.Dumper = NesCartDatabase.DatabaseRoms[i].Cartridges[j].Dumper;
                        infEntry.System = NesCartDatabase.DatabaseRoms[i].Cartridges[j].System;
                        infEntry.SHA1 = NesCartDatabase.DatabaseRoms[i].Cartridges[j].SHA1;
                        // These info should be set when user detect files.
                        infEntry.Path = "N/A";
                        infEntry.Rating = 0;
                        infEntry.Size = 0;
                        infEntry.Played = 0;
                        infEntry.PlayTime = 0;
                        infEntry.LastPlayed = DateTimeFormater.ToFull(DateTime.MinValue);

                        //MyNesDB.AddEntry(infEntry);
                        DataRow row = ds.Tables[0].NewRow();
                        MyNesDB.SetEntryToRow(infEntry, row);
                        ds.Tables[0].Rows.Add(row);
                    }
                    x = (i * 100) / NesCartDatabase.DatabaseRoms.Count;
                    frmG.WriteStatus(Program.ResourceManager.GetString("Status_AddingEntries") + " ... (" + x + "%)", Color.Black);
                }
                Trace.WriteLine("..... " + Program.ResourceManager.GetString("Status_UpdatingDatabase") + " .....");
                MyNesDB.UpdateTableFromDataSet("GAMES", ds);
                Trace.WriteLine(Program.ResourceManager.GetString("Status_DefaultEntriesFromNesCartDBGeneratedSuccessfully"));
            }
        }
        #endregion
        #region Assign and add files
        private void AssignFiles()
        {
            if (NesEmu.EmulationON)
                NesEmu.EmulationPaused = true;
            FormAssignFilesToDB frm = new FormAssignFilesToDB();
            if (frm.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                mode = DBMode.ALL;
                Button_mode_all.Checked = true;
                Button_mode_database.Checked = false;
                Button_mode_files.Checked = false;
                Button_mode_missing.Checked = false;
                Button_mode_notDB.Checked = false;
                _thread_busy = true;
                _assign_folders = frm.FoldersToScan;
                _assign_subfolders = frm.IncludeSubFolders;
                _assign_addFilesNotFound = frm.AddFilesNotFound;
                _assign_update_entries_already_assigned = frm.UpdateEntriesAlreadyAssigned;
                _generate_thread = new Thread(new ThreadStart(AssignFilesThreaded));
                _generate_thread.CurrentUICulture = Program.CultureInfo;
                _generate_thread.Start();
                // Show working
                frmG = new FormGeneratingDatabase(false);
                frmG.ShowDialog(this);

            }
        }
        private void AssignFilesThreaded()
        {
            Trace.WriteLine(Program.ResourceManager.GetString("Status_CalculatingFiles"));
            List<string> files = new List<string>();
            foreach (string folder in _assign_folders)
                if (Directory.Exists(folder))
                    files.AddRange(Directory.GetFiles(folder, "*",
                        _assign_subfolders ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly));
            Trace.WriteLine(Program.ResourceManager.GetString("Status_TotalOf") + " " + files.Count + " " +
                Program.ResourceManager.GetString("Status_FilesFound"));
            Trace.WriteLine(Program.ResourceManager.GetString("Status_LoadingEntriesFromTheDatabase"));
            AddFiles(files.ToArray());
            // Done !
            RefreshEntriesThreaded();
            CloseWorkFormThreaded();
        }
        private void AddFiles(string[] files)
        {
            // Create temp folder
            string tempFolder = Path.GetTempPath() + "\\MYNES\\";
            string tempFile = tempFolder + "fileTemp";
            Directory.CreateDirectory(tempFolder);
            Trace.WriteLine(Program.ResourceManager.GetString("Status_SearchingFiles"));
            DataSet ds = MyNesDB.GetDataSet("GAMES");
            foreach (string file in files)
            {
                if (frmG.CancelRequest)
                {
                    break;
                }
                switch (Path.GetExtension(file).ToLower())
                {
                    #region INES
                    case ".nes":
                        {
                            // Read header !
                            INes header = new INes();
                            header.Load(file, false);
                            // Search for the match, only NesCartDB get searched.
                            bool found = false;
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                if (ds.Tables[0].Rows[i]["SHA1"].ToString().ToLower() == header.SHA1.ToLower()
                                    && (bool)ds.Tables[0].Rows[i]["IsDB"])
                                {
                                    // This is it !!
                                    // MyNesDB.UpdateEntry(ds.Tables[0].Rows[i]["Id"].ToString(), file, GetFileSize(file));
                                    string path = ds.Tables[0].Rows[i]["Path"].ToString().Replace("&apos;", "'");
                                    if (File.Exists(path) && path != "")
                                    {
                                        if (_assign_update_entries_already_assigned)
                                        {
                                            ds.Tables[0].Rows[i]["Path"] = file.Replace("'", "&apos;");
                                            ds.Tables[0].Rows[i]["Size"] = GetFileSize(file);
                                            Trace.WriteLine(Program.ResourceManager.GetString("Status_EntryUPDATED") +
                                                ": [" + ds.Tables[0].Rows[i]["Name"].ToString() + "]");
                                        }
                                        else
                                        {
                                            Trace.WriteLine(Program.ResourceManager.GetString("Status_EntrySkipped") +
                                               ": [" + ds.Tables[0].Rows[i]["Name"].ToString() + "]");
                                        }
                                    }
                                    else// Force assign; no file assigned or file not exist
                                    {
                                        ds.Tables[0].Rows[i]["Path"] = file;
                                        ds.Tables[0].Rows[i]["Size"] = GetFileSize(file);
                                        Trace.WriteLine(Program.ResourceManager.GetString("Status_EntryUPDATED") +
                                            ": [" + ds.Tables[0].Rows[i]["Name"].ToString() + "]");
                                    }
                                    found = true;
                                    break;
                                }
                            }
                            if (!found && _assign_addFilesNotFound)
                            {
                                // File not found in the database, add brand new entry
                                found = false;
                                // Check if path already exist ...
                                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                                {
                                    if (ds.Tables[0].Rows[i]["Path"].ToString().Replace("&apos;", "'") == file)
                                    {
                                        found = true;
                                        // File is exist here since 'file' value is real
                                        if (_assign_update_entries_already_assigned)
                                        {
                                            ds.Tables[0].Rows[i]["Path"] = file.Replace("'", "&apos;");
                                            ds.Tables[0].Rows[i]["Size"] = GetFileSize(file);
                                            Trace.WriteLine(Program.ResourceManager.GetString("Status_EntryUPDATED") +
                                                ": [" + ds.Tables[0].Rows[i]["Name"].ToString() + "]");
                                        }
                                        else
                                        {
                                            Trace.WriteLine(Program.ResourceManager.GetString("Status_EntrySkipped") +
                                               ": [" + ds.Tables[0].Rows[i]["Name"].ToString() + "]");
                                        }

                                        break;
                                    }
                                }
                                if (!found)
                                {
                                    DataRow row = ds.Tables[0].NewRow();
                                    MyNesDBEntryInfo infEntry = new MyNesDBEntryInfo();
                                    infEntry.IsDB = false;
                                    infEntry.AlternativeName = "";
                                    infEntry.BoardMapper = header.IsValid ? header.MapperNumber : 0;
                                    infEntry.BoardPcb = "";
                                    infEntry.BoardType = "";
                                    infEntry.Catalog = "";
                                    infEntry.Class = "";
                                    infEntry.Developer = "";
                                    infEntry.Name = Path.GetFileNameWithoutExtension(file);
                                    infEntry.Players = "";
                                    infEntry.Publisher = "";
                                    infEntry.Region = "";
                                    infEntry.ReleaseDate = "";
                                    infEntry.CRC = CalculateCRC(file, 16);
                                    infEntry.DateDumped = "";
                                    infEntry.Dump = "";
                                    infEntry.Dumper = "";
                                    infEntry.System = "";
                                    infEntry.SHA1 = header.SHA1.ToUpper();
                                    // These info should be set when user detect files.
                                    infEntry.Path = file;
                                    infEntry.Rating = 0;
                                    infEntry.Size = GetFileSize(file);
                                    infEntry.LastPlayed = DateTimeFormater.ToFull(DateTime.MinValue);
                                    infEntry.Played = 0;
                                    infEntry.PlayTime = 0;

                                    MyNesDB.SetEntryToRow(infEntry, row);
                                    ds.Tables[0].Rows.Add(row);
                                    Trace.WriteLine(Program.ResourceManager.GetString("Status_EntryADDED") +
                                        ":[" + infEntry.Name + "]");
                                }
                            }
                            break;
                        }
                    #endregion
                    #region Archive file
                    case ".7z":
                    case ".zip":
                    case ".rar":
                    case ".gzip":
                    case ".tar":
                    case ".bzip2":
                    case ".xz":
                        {
                            try
                            {
                                // Open the archive
                                SevenZipExtractor extractor = new SevenZipExtractor(file);
                                foreach (ArchiveFileInfo f in extractor.ArchiveFileData)
                                {
                                    if (Path.GetExtension(f.FileName).ToLower() == ".nes")
                                    {
                                        string entryPath = "(" + f.Index + ")" + file;
                                        // Extract it !
                                        Stream aStream = new FileStream(tempFile, FileMode.Create, FileAccess.Write);
                                        extractor.ExtractFile(f.Index, aStream);
                                        aStream.Close();
                                        // Read header !
                                        INes header = new INes();
                                        header.Load(tempFile, false);
                                        // Search for the match
                                        bool found = false;
                                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                                        {
                                            if (ds.Tables[0].Rows[i]["SHA1"].ToString().ToLower() == header.SHA1.ToLower()
                                                 && (bool)ds.Tables[0].Rows[i]["IsDB"])
                                            {
                                                string path = GetFilePathFromArchivePath(ds.Tables[0].Rows[i]["Path"].ToString().Replace("&apos;", "'"));
                                                if (File.Exists(path) && path != "")
                                                {
                                                    // This is it !!
                                                    if (_assign_update_entries_already_assigned)
                                                    {
                                                        // The file path code is (index within archive)archivePath
                                                        ds.Tables[0].Rows[i]["Path"] = entryPath;
                                                        ds.Tables[0].Rows[i]["Size"] = GetFileSize(tempFile);
                                                        Trace.WriteLine(Program.ResourceManager.GetString("Status_EntryUPDATED") +
                                                            ": [" + ds.Tables[0].Rows[i]["Name"].ToString() + "]");
                                                    }
                                                    else
                                                    {
                                                        Trace.WriteLine(Program.ResourceManager.GetString("Status_EntrySkipped") +
                                                           ": [" + ds.Tables[0].Rows[i]["Name"].ToString() + "]");
                                                    }
                                                }
                                                else
                                                {
                                                    // Force assign since the file is not valid
                                                    ds.Tables[0].Rows[i]["Path"] = entryPath;
                                                    ds.Tables[0].Rows[i]["Size"] = GetFileSize(tempFile);
                                                    Trace.WriteLine(Program.ResourceManager.GetString("Status_EntryUPDATED") +
                                                        ": [" + ds.Tables[0].Rows[i]["Name"].ToString() + "]");
                                                }
                                                found = true;
                                                break;
                                            }
                                        }
                                        if (!found && _assign_addFilesNotFound)
                                        {
                                            found = false;
                                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                                            {
                                                if (ds.Tables[0].Rows[i]["Path"].ToString().Replace("&apos;", "'") == entryPath)
                                                {
                                                    found = true;
                                                    // This is it !!
                                                    if (_assign_update_entries_already_assigned)
                                                    {
                                                        ds.Tables[0].Rows[i]["Path"] = entryPath;
                                                        ds.Tables[0].Rows[i]["Size"] = GetFileSize(tempFile);
                                                        Trace.WriteLine(Program.ResourceManager.GetString("Status_EntryUPDATED") +
                                                            ": [" + ds.Tables[0].Rows[i]["Name"].ToString() + "]");
                                                    }
                                                    else
                                                    {
                                                        Trace.WriteLine(Program.ResourceManager.GetString("Status_EntrySkipped") +
                                                           ": [" + ds.Tables[0].Rows[i]["Name"].ToString() + "]");
                                                    }
                                                    break;
                                                }
                                            }
                                            if (!found)
                                            {
                                                DataRow row = ds.Tables[0].NewRow();
                                                MyNesDBEntryInfo infEntry = new MyNesDBEntryInfo();
                                                infEntry.IsDB = false;
                                                infEntry.AlternativeName = "";
                                                infEntry.BoardMapper = header.IsValid ? header.MapperNumber : 0;
                                                infEntry.BoardPcb = "";
                                                infEntry.BoardType = "";
                                                infEntry.Catalog = "";
                                                infEntry.Class = "";
                                                infEntry.Developer = "";
                                                infEntry.Name = Path.GetFileNameWithoutExtension(f.FileName);
                                                infEntry.Players = "";
                                                infEntry.Publisher = "";
                                                infEntry.Region = "";
                                                infEntry.ReleaseDate = "";
                                                infEntry.CRC = CalculateCRC(tempFile, 16);
                                                infEntry.DateDumped = "";
                                                infEntry.Dump = "";
                                                infEntry.Dumper = "";
                                                infEntry.System = "";
                                                infEntry.SHA1 = header.SHA1.ToUpper();
                                                // These info should be set when user detect files.
                                                infEntry.Path = entryPath;
                                                infEntry.Rating = 0;
                                                infEntry.Size = GetFileSize(tempFile);
                                                infEntry.LastPlayed = DateTimeFormater.ToFull(DateTime.MinValue);
                                                infEntry.Played = 0;
                                                infEntry.PlayTime = 0;

                                                MyNesDB.SetEntryToRow(infEntry, row);
                                                ds.Tables[0].Rows.Add(row);
                                                Trace.WriteLine(Program.ResourceManager.GetString("Status_EntryADDED") +
                                                    ":[" + infEntry.Name + "]");
                                            }
                                        }
                                    }
                                }
                            }
                            catch
                            {
                                Trace.WriteLine("File [" + file + "] is archive but can't be opened.");
                            }
                            break;
                        }
                    #endregion
                }
            }
            Trace.WriteLine(".... " + Program.ResourceManager.GetString("Status_UpdatingDatabase") + " ....");
            MyNesDB.UpdateTableFromDataSet("GAMES", ds);
        }
        #endregion
        #region Refresh and closing
        private void CloseWorkFormThreaded()
        {
            if (!this.InvokeRequired)
                CloseWorkForm();
            else
                this.Invoke(new Action(CloseWorkForm));
        }
        private void CloseWorkForm()
        {
            frmG.Close(); ClearViewers();
            _thread_busy = false;
        }
        private void RefreshColumnsThreaded()
        {
            if (!this.InvokeRequired)
                RefreshColumns();
            else
                this.Invoke(new Action(RefreshColumns));
        }
        private void RefreshColumns()
        {
            managedListView1.Columns.Clear();
            contextMenuStrip_list_columns.Items.Clear();
            MyNesDBColumn[] columns = MyNesDB.GetColumns();
            foreach (MyNesDBColumn k in columns)
            {
                if (k.Visible)
                {
                    ManagedListViewColumn col = new ManagedListViewColumn();
                    col.HeaderText = Program.ResourceManager.GetString(k.Name);
                    col.ID = k.Name;
                    col.Width = k.Width;
                    managedListView1.Columns.Add(col);
                }
                ToolStripMenuItem it = new ToolStripMenuItem();
                it.Text = k.Name;
                it.Checked = k.Visible;
                contextMenuStrip_list_columns.Items.Add(it);
            }
        }
        private void RefreshEntriesThreaded()
        {
            if (!this.InvokeRequired)
                RefreshEntries();
            else
                this.Invoke(new Action(RefreshEntries));
        }
        private void RefreshEntries()
        {
            managedListView1.Items.Clear();
            bool checkPath = false;
            bool fileMustExist = false;
            // Get the dataset of GAMES table
            DataSet ds = null;
            switch (mode)
            {
                case DBMode.ALL:
                    {
                        if (!doSort)
                            ds = MyNesDB.GetDataSet("GAMES");
                        else
                        {
                            ds = MyNesDB.GetDataSet("GAMES", sortColumnName, sortAZ);
                        }
                        break;
                    }
                case DBMode.DATABASE:
                    {
                        if (!doSort)
                            ds = MyNesDB.GetDataSet("GAMES", "IsDB", true);
                        else
                            ds = MyNesDB.GetDataSet("GAMES", "IsDB", true, sortColumnName, sortAZ);
                        break;
                    }
                case DBMode.NOTDB:
                    {
                        if (!doSort)
                            ds = MyNesDB.GetDataSet("GAMES", "IsDB", false);
                        else
                            ds = MyNesDB.GetDataSet("GAMES", "IsDB", false, sortColumnName, sortAZ);
                        break;
                    }
                case DBMode.FILES:
                    {
                        checkPath = true;
                        fileMustExist = true;
                        if (!doSort)
                            ds = MyNesDB.GetDataSet("GAMES");
                        else
                        {
                            ds = MyNesDB.GetDataSet("GAMES", sortColumnName, sortAZ);
                        }
                        break;
                    }
                case DBMode.MISSING:
                    {
                        checkPath = true;
                        fileMustExist = false;
                        if (!doSort)
                            ds = MyNesDB.GetDataSet("GAMES");
                        else
                        {
                            ds = MyNesDB.GetDataSet("GAMES", sortColumnName, sortAZ);
                        }
                        break;
                    }
            }
            doSort = false;
            if (ds == null) return;

            // Get columns collection
            MyNesDBColumn[] columns = MyNesDB.GetColumns();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                string path = ds.Tables[0].Rows[i]["Path"].ToString().Replace("&apos;", "'");
                if (path.StartsWith("("))
                {
                    // Decode
                    path = GetFilePathFromArchivePath(path);
                }

                if (checkPath)
                {
                    if (fileMustExist)
                    {
                        if (!File.Exists(path))
                            continue;
                    }
                    else
                    {
                        if (File.Exists(path))
                            continue;
                    }
                }
                // Quick search ?
                if (performQuickSearch)
                {
                    if (quickSearchText.Length > 0)
                    {
                        if (!ds.Tables[0].Rows[i]["Name"].ToString().Replace("&apos;", "'").ToLower().
                            Contains(quickSearchText.ToLower()))
                            continue;
                    }
                }
                else if (performSearch)
                {
                    if (!FilterGame(ds.Tables[0].Rows[i]))
                        continue;
                }
                // Reached here means we can add the item
                ManagedListViewItem item = new ManagedListViewItem();
                // Put the id in the tag
                item.Tag = ds.Tables[0].Rows[i]["Id"].ToString();
                // Add subitems depending on columns
                foreach (MyNesDBColumn k in columns)
                {
                    if (k.Name == "Rating")
                    {
                        ManagedListViewRatingSubItem rat = new ManagedListViewRatingSubItem();
                        rat.ColumnID = k.Name;
                        int val = 0;
                        int.TryParse(ds.Tables[0].Rows[i][k.Name].ToString(), out val);
                        rat.Rating = val;
                        rat.RatingChanged += rat_RatingChanged;
                        item.SubItems.Add(rat);
                    }
                    else if (k.Name == "Played")
                    {
                        ManagedListViewSubItem sub = new ManagedListViewSubItem();
                        sub.ColumnID = k.Name;
                        string played = ds.Tables[0].Rows[i][k.Name].ToString();
                        if (played == "0")
                            sub.Text = Program.ResourceManager.GetString("Text_NeverPlayed");
                        else if (played == "1")
                            sub.Text = Program.ResourceManager.GetString("Text_OneTime");
                        else
                            sub.Text = played + " " + Program.ResourceManager.GetString("Text_Times");

                        item.SubItems.Add(sub);
                    }
                    else if (k.Name == "Play Time")
                    {
                        ManagedListViewSubItem sub = new ManagedListViewSubItem();
                        sub.ColumnID = k.Name;
                        string played = ds.Tables[0].Rows[i][k.Name].ToString();
                        if (played == "0")
                            sub.Text = Program.ResourceManager.GetString("Text_NeverPlayed");
                        else
                        {
                            int val = 0;
                            int.TryParse(played, out val);
                            sub.Text = TimeSpan.FromSeconds(val).ToString();
                        }
                        item.SubItems.Add(sub);
                    }
                    else if (k.Name == "Last Played")
                    {
                        ManagedListViewSubItem sub = new ManagedListViewSubItem();
                        sub.ColumnID = k.Name;
                        DateTime time = (DateTime)ds.Tables[0].Rows[i][k.Name];
                        if (time != DateTime.MinValue)
                            sub.Text = time.ToLocalTime().ToString();
                        else
                            sub.Text = Program.ResourceManager.GetString("Text_NeverPlayed");
                        item.SubItems.Add(sub);
                    }
                    else if (k.Name == "Size")
                    {
                        ManagedListViewSubItem sub = new ManagedListViewSubItem();
                        sub.ColumnID = k.Name;
                        int val = 0;
                        int.TryParse(ds.Tables[0].Rows[i]["Size"].ToString(), out val);
                        sub.Text = GetSize(val);
                        item.SubItems.Add(sub);
                    }
                    else
                    {
                        ManagedListViewSubItem sub = new ManagedListViewSubItem();
                        sub.ColumnID = k.Name;
                        if (k.Name == "Name")
                        {
                            sub.DrawMode = ManagedListViewItemDrawMode.TextAndImage;
                            sub.ImageIndex = File.Exists(path) ? 1 : 0;
                        }
                        sub.Text = ds.Tables[0].Rows[i][k.Name].ToString().Replace("&apos;", "'");

                        item.SubItems.Add(sub);
                    }
                }
                managedListView1.Items.Add(item);
            }
            performQuickSearch = false;
            quickSearchText = "";
            performSearch = false;
        }
        private void Detect()
        {
            if (_thread_busy)
            {
                ManagedMessageBox.ShowErrorMessage(Program.ResourceManager.GetString("Message_AnotherOperationInProgress"));
                return;
            }
            if (!MyNesDB.IsDatabaseLoaded)
            {
                ManagedMessageBox.ShowErrorMessage(Program.ResourceManager.GetString("Message_NoDbLoadedYet"));
                return;
            }
            if (NesEmu.EmulationON)
                NesEmu.EmulationPaused = true;
            FormDetectSelection frm = new FormDetectSelection();
            if (frm.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                FormDetectForDatabase frmDetect = new FormDetectForDatabase(frm.MODE);
                frmDetect.ShowDialog(this);
            }
        }
        private bool FilterGame(DataRow row)
        {
            if (searchArgs == null)
                return false;
            // Let's see what's the mode
            string searchWord = searchArgs.CaseSensitive ? searchArgs.SearchWhat : searchArgs.SearchWhat.ToLower();

            // Do the search
            if (!searchArgs.IsNumber)
            {
                string searchTargetText = row[searchArgs.SearchColumn].ToString().Replace("&apos;", "'");
                if (!searchArgs.CaseSensitive)
                    searchTargetText = searchTargetText.ToLower();
                // Decode archive path
                if (searchArgs.SearchColumn == "Path")
                {
                    if (searchTargetText.StartsWith("("))
                    {
                        // Decode
                        string[] pathCodes = searchTargetText.Split(new char[] { '(', ')' });
                        searchTargetText = pathCodes[2];
                    }
                }
                // Decode user code
                string[] searchCodes = searchWord.Split(new char[] { '+' });
                // Do the search
                switch (searchArgs.TextCondition)
                {
                    case TextSearchCondition.Contains:// The target contains the search word
                        {
                            foreach (string s in searchCodes)
                            {
                                if (searchTargetText.Contains(s))
                                    return true;
                            }
                            return false;
                        }
                    case TextSearchCondition.DoesNotContain:// The target doesn't contain the search word
                        {
                            bool add = true;
                            foreach (string s in searchCodes)
                            {
                                if (searchTargetText.Contains(s))
                                {
                                    add = false; break;
                                }
                            }
                            return add;
                        }
                    case TextSearchCondition.Is:// Match the word !
                        {
                            foreach (string s in searchCodes)
                            {
                                if (searchTargetText == s)
                                    return true;
                            }
                            return false;
                        }
                    case TextSearchCondition.IsNot:// Don't match the word !
                        {
                            bool add = true;
                            foreach (string s in searchCodes)
                            {
                                if (searchTargetText == s)
                                {
                                    add = false; break;
                                }
                            }
                            return add;
                        }
                    case TextSearchCondition.StartWith:// The target starts the search word
                        {
                            foreach (string s in searchCodes)
                            {
                                if (searchTargetText.StartsWith(s))
                                    return true;
                            }
                            return false;
                        }
                    case TextSearchCondition.DoesNotStartWith:// The target doesn't start the search word
                        {
                            bool add = true;
                            foreach (string s in searchCodes)
                            {
                                if (searchTargetText.StartsWith(s))
                                {
                                    add = false; break;
                                }
                            }
                            return add;
                        }
                    case TextSearchCondition.EndWith:// The target ends the search word
                        {
                            foreach (string s in searchCodes)
                            {
                                if (searchTargetText.EndsWith(s))
                                    return true;
                            }
                            return false;
                        }
                    case TextSearchCondition.DoesNotEndWith:// The target doesn't end with the search word
                        {
                            bool add = true;
                            foreach (string s in searchCodes)
                            {
                                if (searchTargetText.EndsWith(s))
                                {
                                    add = false; break;
                                }
                            }
                            return add;
                        }
                }
            }
            else// Number search
            {
                long searchTargetNumber = 0;
                if (searchArgs.SearchColumn == "Size")
                    searchTargetNumber = DecodeSizeLabel(row["Size"].ToString());
                else
                {
                    long.TryParse(row[searchArgs.SearchColumn].ToString(), out searchTargetNumber);
                }
                long searchNumber = DecodeSizeLabel(searchWord);
                switch (searchArgs.NumberCondition)
                {
                    case NumberSearchCondition.Equal:
                        {
                            if (searchTargetNumber == searchNumber)
                                return true;
                            break;
                        }
                    case NumberSearchCondition.DoesNotEqual:
                        {
                            if (searchTargetNumber != searchNumber)
                                return true;
                            break;
                        }
                    case NumberSearchCondition.EqualSmaller:
                        {
                            if (searchTargetNumber <= searchNumber)
                                return true;
                            break;
                        }
                    case NumberSearchCondition.EuqalLarger:
                        {
                            if (searchTargetNumber >= searchNumber)
                                return true;
                            break;
                        }
                    case NumberSearchCondition.Larger:
                        {
                            if (searchTargetNumber > searchNumber)
                                return true;
                            break;
                        }
                    case NumberSearchCondition.Smaller:
                        {
                            if (searchTargetNumber < searchNumber)
                                return true;
                            break;
                        }
                }
            }
            return false;
        }
        private void AfterColumnResizeUpdateThreaded()
        {
            status = Program.ResourceManager.GetString("Status_UpdatingColumnsInTheDatabase");
            if (MyNesDB.IsDatabaseLoaded)
            {
                int i = 0;
                foreach (ManagedListViewColumn col in managedListView1.Columns)
                {
                    MyNesDB.UpdateColumn(col.ID, true, col.Width);
                    progress = (i * 100) / managedListView1.Columns.Count;
                    i++;
                }
            }
            status = "Ready.";
            CloseStatus();
            _thread_busy = false;
        }
        private void AfterColumnReorderUpdateThreaded()
        {
            // Save the column size in the database !
            if (MyNesDB.IsDatabaseLoaded)
            {
                status = Program.ResourceManager.GetString("Status_UpdatingColumnsInTheDatabase");
                List<MyNesDBColumn> columns = new List<MyNesDBColumn>(MyNesDB.GetColumns());
                MyNesDB.DeleteAllColumns();
                // Add the columns with the order of the list !
                int i = 0;
                foreach (ManagedListViewColumn col in managedListView1.Columns)
                {
                    foreach (MyNesDBColumn c in columns)
                    {
                        if (col.ID == c.Name)
                        {
                            columns.Remove(c);
                            break;
                        }
                    }

                    MyNesDB.AddColumn(col.ID, true, col.Width);
                    progress = (i * 100) / managedListView1.Columns.Count;
                    i++;
                }
                // Add the unvisible columns !
                foreach (MyNesDBColumn c in columns)
                {
                    MyNesDB.AddColumn(c.Name, false, c.Width);
                }
            }
            managedListView1.AllowColumnsReorder = true;
            status = Program.ResourceManager.GetString("Status_Ready");
            CloseStatus();
            _thread_busy = false;
        }
        private void CloseStatus()
        {
            if (!this.InvokeRequired)
                CloseStatusThreaded();
            else
                this.Invoke(new Action(CloseStatusThreaded));
        }
        private void CloseStatusThreaded()
        {
            timer_progress.Stop();
            ProgressBar.Visible = false;
            StatusLabel.Text = Program.ResourceManager.GetString("Status_Done");
        }
        private void UpdateSelection()
        {
            toolStripStatusLabel1.Text = managedListView1.SelectedItems.Count + " / " + managedListView1.Items.Count;
            if (managedListView1.SelectedItems.Count == 1)
            {
                string id = managedListView1.SelectedItems[0].Tag.ToString();
                gameInfoViewer.RefreshForEntry(id);
                imagesViewer_covers.RefreshForEntry(id);
                imagesViewer_snaps.RefreshForEntry(id);
                infoViewer.RefreshForEntry(id);
                manualsViewer.RefreshForEntry(id);
            }
            else
            {
                // Clear all viewers
                ClearViewers();
            }
        }
        private void ClearViewers()
        {
            gameInfoViewer.RefreshForEntry("");
            imagesViewer_covers.RefreshForEntry("");
            imagesViewer_snaps.RefreshForEntry("");
            infoViewer.RefreshForEntry("");
            manualsViewer.RefreshForEntry("");
        }
        private void ReturnToNormalWindowState()
        {
            if (this.Visible)
                this.WindowState = FormWindowState.Normal;
        }
        #endregion
        #region Play
        private void PlaySelected()
        {
            if (managedListView1.SelectedItems.Count != 1)
            {
                ManagedMessageBox.ShowErrorMessage(Program.ResourceManager.GetString("Message_PleaseSelectOneGameToPlay"));
                return;
            }
            string id = managedListView1.SelectedItems[0].Tag.ToString();

            bool isArchive = false;
            MyNesDBEntryInfo entry = MyNesDB.GetEntry(id);
            string path = entry.Path;
            string tempFile = path;
            int aIndex = 0;
            if (path.StartsWith("("))
            {
                isArchive = true;
                // Decode
                aIndex = GetFileIndexFromArchivePath(path);
                path = GetFilePathFromArchivePath(path);
            }
            if (isArchive)
            {
                // Extract the archive
                string tempFolder = Path.GetTempPath() + "\\MYNES\\";

                Directory.CreateDirectory(tempFolder);
                // Extract it !
                SevenZipExtractor extractor = new SevenZipExtractor(path);
                tempFile = tempFolder + extractor.ArchiveFileData[aIndex].FileName;
                Stream aStream = new FileStream(tempFile, FileMode.Create, FileAccess.Write);
                extractor.ExtractFile(aIndex, aStream);
                aStream.Close();
            }
            if (File.Exists(tempFile))
            {
                if (NesEmu.EmulationON)
                    UpdatePlayStatus();
                // Play it !
                currentPlayedGameIndex = managedListView1.Items.IndexOf(managedListView1.SelectedItems[0]);
                currentPlayedGameId = id;
                Program.FormMain.OpenRom(tempFile);
                Program.FormMain.Activate();

                if (NesEmu.EmulationON)
                {
                    isPlayingGame = true;
                    playTime = 0;
                    timer_play.Start();
                    if (Program.Settings.LauncherAutoMinimize)
                        this.WindowState = FormWindowState.Minimized;

                    Program.FormMain.AddRecent(path);
                }
            }
            else
            {
                ManagedMessageBox.ShowErrorMessage(Program.ResourceManager.GetString("Message_GameFileNotExistOrNoFileAssignedToThisGameYet"));
            }
        }
        private void UpdatePlayStatus()
        {
            if (!isPlayingGame)
                return;
            if (currentPlayedGameId == "")
                return;
            timer_play.Stop();
            Thread.CurrentThread.CurrentUICulture = Program.CultureInfo;
            // Get original game info:
            MyNesDBEntryInfo inf = MyNesDB.GetEntry(currentPlayedGameId);
            // Advance played times
            inf.Played++;
            // Add play time !
            inf.PlayTime += playTime;
            DateTime time = DateTime.Now;
            // Update db
            MyNesDB.UpdateEntry(currentPlayedGameId, inf.Played, inf.PlayTime, time);
            if (currentPlayedGameIndex >= 0 && currentPlayedGameIndex < managedListView1.Items.Count)
            {
                if (managedListView1.Items[currentPlayedGameIndex].Tag.ToString() == currentPlayedGameId)
                {
                    // Update list item
                    if (inf.Played == 0)
                        managedListView1.Items[currentPlayedGameIndex].GetSubItemByID("Played").Text = Program.ResourceManager.GetString("Text_NeverPlayed");
                    else if (inf.Played == 1)
                        managedListView1.Items[currentPlayedGameIndex].GetSubItemByID("Played").Text = Program.ResourceManager.GetString("Text_OneTime");
                    else
                        managedListView1.Items[currentPlayedGameIndex].GetSubItemByID("Played").Text = inf.Played + " " + Program.ResourceManager.GetString("Text_Times");

                    if (inf.PlayTime == 0)
                        managedListView1.Items[currentPlayedGameIndex].GetSubItemByID("Play Time").Text = Program.ResourceManager.GetString("Text_NeverPlayed");
                    else
                    {
                        managedListView1.Items[currentPlayedGameIndex].GetSubItemByID("Play Time").Text = TimeSpan.FromSeconds(inf.PlayTime).ToString();
                    }

                    if (time != DateTime.MinValue)
                        managedListView1.Items[currentPlayedGameIndex].GetSubItemByID("Last Played").Text = time.ToLocalTime().ToString();
                    else
                        managedListView1.Items[currentPlayedGameIndex].GetSubItemByID("Last Played").Text = Program.ResourceManager.GetString("Text_NeverPlayed");
                }
            }
            isPlayingGame = false;
            _thread_busy = false;
            currentPlayedGameId = "";
            currentPlayedGameIndex = -1;
        }
        #endregion
        #region Helpers
        private string GetFilePathFromArchivePath(string arFile)
        {
            if (arFile.StartsWith("("))
            {
                for (int i = 0; i < arFile.Length; i++)
                {
                    if (arFile[i] == ')')
                    {
                        i++;
                        return arFile.Substring(i, arFile.Length - i);
                    }
                }
            }
            else
            {
                return arFile;
            }
            return "";
        }
        private int GetFileIndexFromArchivePath(string arFile)
        {
            if (arFile.StartsWith("("))
            {
                for (int i = 0; i < arFile.Length; i++)
                {
                    if (arFile[i] == ')')
                    {
                        string number = arFile.Substring(1, i - 1);
                        int val = -1;
                        int.TryParse(number, out val);
                        return val;
                    }
                }
            }
            return -1;
        }
        private string GetFileSizeLabel(string FilePath)
        {
            if (File.Exists(Path.GetFullPath(FilePath)) == true)
            {
                FileInfo Info = new FileInfo(FilePath);
                string Unit = " Byte";
                double Len = Info.Length;
                if (Info.Length >= 1024)
                {
                    Len = Info.Length / 1024.00;
                    Unit = " KB";
                }
                if (Len >= 1024)
                {
                    Len /= 1024.00;
                    Unit = " MB";
                }
                if (Len >= 1024)
                {
                    Len /= 1024.00;
                    Unit = " GB";
                }
                return Len.ToString("F2") + Unit;
            }
            return "";
        }
        private int GetFileSize(string FilePath)
        {
            if (File.Exists(Path.GetFullPath(FilePath)) == true)
            {
                FileInfo Info = new FileInfo(FilePath);

                return (int)Info.Length;
            }
            return 0;
        }
        private string GetSize(long size)
        {
            string Unit = " Byte";
            double Len = size;
            if (size >= 1024)
            {
                Len = size / 1024.00;
                Unit = " KB";
            }
            if (Len >= 1024)
            {
                Len /= 1024.00;
                Unit = " MB";
            }
            if (Len >= 1024)
            {
                Len /= 1024.00;
                Unit = " GB";
            }
            if (Len < 0)
                return "???";
            return Len.ToString("F2") + Unit;
        }
        private string CalculateCRC(string filePath, int bytesToSkip)
        {
            if (File.Exists(filePath))
            {
                Stream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                fileStream.Read(new byte[bytesToSkip], 0, bytesToSkip);
                byte[] fileBuffer = new byte[fileStream.Length - bytesToSkip];
                fileStream.Read(fileBuffer, 0, (int)(fileStream.Length - bytesToSkip));
                fileStream.Close();

                string crc = "";
                Crc32 crc32 = new Crc32();
                byte[] crc32Buffer = crc32.ComputeHash(fileBuffer);

                foreach (byte b in crc32Buffer)
                    crc += b.ToString("x2").ToLower();

                return crc;
            }
            return "";
        }
        private long DecodeSizeLabel(string sizeLabel)
        {
            // Let's see given parameter (size)
            string t = sizeLabel.ToLower();
            t = t.Replace("kb", "");
            t = t.Replace("mb", "");
            t = t.Replace("gb", "");
            t = t.Replace(" ", "");
            double value = 0;
            double.TryParse(t, out value);

            if (sizeLabel.ToLower().Contains("kb"))
                value *= 1024;
            else if (sizeLabel.ToLower().Contains("mb"))
                value *= 1024 * 1024;
            else if (sizeLabel.ToLower().Contains("gb"))
                value *= 1024 * 1024 * 1024;

            return (long)value;
        }
        #endregion

        private void rat_RatingChanged(object sender, ManagedListViewRatingChangedArgs e)
        {
            if (MyNesDB.IsDatabaseLoaded)
                MyNesDB.UpdateEntry(managedListView1.Items[e.ItemIndex].Tag.ToString(), e.Rating);
        }
        private void managedListView1_AfterColumnReorder(object sender, EventArgs e)
        {
            if (_thread_busy) return;
            timer_progress.Start();
            ProgressBar.Visible = true;
            managedListView1.AllowColumnsReorder = false;
            _thread_busy = true;
            Thread th = new Thread(new ThreadStart(AfterColumnReorderUpdateThreaded));
            th.CurrentUICulture = Program.CultureInfo;
            th.Start();
        }
        private void managedListView1_AfterColumnResize(object sender, EventArgs e)
        {
            if (_thread_busy) return;
            timer_progress.Start();
            ProgressBar.Visible = true;
            _thread_busy = true;
            Thread th = new Thread(new ThreadStart(AfterColumnResizeUpdateThreaded));
            th.CurrentUICulture = Program.CultureInfo;
            th.Start();
        }
        private void managedListView1_DrawSubItem(object sender, ManagedListViewSubItemDrawArgs e)
        {
        }
        private void managedListView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectionTimer = 3;
            timer_selection.Start();
        }
        private void gameInfoViewer_RatingChanged(object sender, EventArgs e)
        {
            if (managedListView1.SelectedItems.Count == 1)
            {
                ((ManagedListViewRatingSubItem)managedListView1.SelectedItems[0].GetSubItemByID("Rating")).Rating =
                    ((Rating)sender).rating;
            }
        }
        // Generate new database
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (_thread_busy)
            {
                ManagedMessageBox.ShowErrorMessage(Program.ResourceManager.GetString("Message_AnotherOperationInProgress."));
                return;
            }
            if (MyNesDB.IsDatabaseLoaded)
            {
                ManagedMessageBoxResult res = ManagedMessageBox.ShowQuestionMessage(
                    Program.ResourceManager.GetString("Message_AreYouSureYouWantToCreateNewDB"),
                    Program.ResourceManager.GetString("MessageCaption_CreateNewDatabase"));
                if (res.ClickedButtonIndex == 0)
                    GenerateDatabase();
            }
            else
                GenerateDatabase();
        }
        // Assign files to entries.
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (_thread_busy)
            {
                ManagedMessageBox.ShowErrorMessage(Program.ResourceManager.GetString("Message_AnotherOperationInProgress."));
                return;
            }
            if (!MyNesDB.IsDatabaseLoaded)
            {
                ManagedMessageBox.ShowErrorMessage(Program.ResourceManager.GetString("Message_NoDbLoadedYet"));
                return;
            }
            AssignFiles();
        }
        private void managedListView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            PlaySelected();
        }
        private void managedListView1_EnterPressed(object sender, EventArgs e)
        {
            PlaySelected();
        }
        private void PlayGame_Click(object sender, EventArgs e)
        {
            PlaySelected();
        }
        private void NesEmu_EMUShutdown(object sender, EventArgs e)
        {
            if (isPlayingGame)
            {
                if (!this.InvokeRequired)
                    ReturnToNormalWindowState();
                else
                    this.Invoke(new Action(ReturnToNormalWindowState));
            }
            _thread_busy = true;
            Thread th = new Thread(new ThreadStart(UpdatePlayStatus));
            th.CurrentUICulture = Program.CultureInfo;
            th.Start();
        }
        private void timer_play_Tick(object sender, EventArgs e)
        {
            playTime++;
        }
        private void FormLauncher_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!this.InvokeRequired)
                ReturnToNormalWindowState();
            else
                this.Invoke(new Action(ReturnToNormalWindowState));
            SaveSettings();
            MyNesDB.ReleaseDatabase();
        }
        private void Button_mode_all_Click(object sender, EventArgs e)
        {
            mode = DBMode.ALL;
            Button_mode_all.Checked = true;
            Button_mode_database.Checked = false;
            Button_mode_files.Checked = false;
            Button_mode_missing.Checked = false;
            Button_mode_notDB.Checked = false;
            RefreshEntries();
        }
        private void Button_mode_database_Click(object sender, EventArgs e)
        {
            mode = DBMode.DATABASE;
            Button_mode_all.Checked = false;
            Button_mode_database.Checked = true;
            Button_mode_files.Checked = false;
            Button_mode_missing.Checked = false;
            Button_mode_notDB.Checked = false;
            RefreshEntries();
        }
        private void Button_mode_notDB_Click(object sender, EventArgs e)
        {
            mode = DBMode.NOTDB;
            Button_mode_all.Checked = false;
            Button_mode_database.Checked = false;
            Button_mode_files.Checked = false;
            Button_mode_missing.Checked = false;
            Button_mode_notDB.Checked = true;
            RefreshEntries();
        }
        private void Button_mode_files_Click(object sender, EventArgs e)
        {
            mode = DBMode.FILES;
            Button_mode_all.Checked = false;
            Button_mode_database.Checked = false;
            Button_mode_files.Checked = true;
            Button_mode_missing.Checked = false;
            Button_mode_notDB.Checked = false;
            RefreshEntries();
        }
        private void Button_mode_missing_Click(object sender, EventArgs e)
        {
            mode = DBMode.MISSING;
            Button_mode_all.Checked = false;
            Button_mode_database.Checked = false;
            Button_mode_files.Checked = false;
            Button_mode_missing.Checked = true;
            Button_mode_notDB.Checked = false;
            RefreshEntries();
        }
        private void timer_search_Tick(object sender, EventArgs e)
        {
            if (searchTimer > 0)
                searchTimer--;
            else
            {
                timer_search.Stop();
                performQuickSearch = true;
                quickSearchText = toolStripTextBox_find.Text;

                RefreshEntries();
            }
        }
        private void toolStripTextBox_find_TextChanged(object sender, EventArgs e)
        {
            searchTimer = 3;
            timer_search.Start();
        }
        // Find
        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            foreach (Form frm in this.OwnedForms)
            {
                if (frm.Tag.ToString() == "find")
                {
                    frm.Activate();
                    return;
                }
            }
            FormSearchLauncher newfrm = new FormSearchLauncher();
            newfrm.SearchRequest += newfrm_SearchRequest;
            newfrm.Show(this);
        }
        private void newfrm_SearchRequest(object sender, SearchRequestArgs e)
        {
            if (e.SearchWhat.Length == 0) return;
            performSearch = true;
            searchArgs = e;
            RefreshEntries();
        }
        private void managedListView1_SwitchToColumnsContextMenu(object sender, EventArgs e)
        {
            managedListView1.ContextMenuStrip = contextMenuStrip_list_columns;
        }
        private void managedListView1_SwitchToNormalContextMenu(object sender, EventArgs e)
        {
            managedListView1.ContextMenuStrip = contextMenuStrip_list_main;
        }
        private void contextMenuStrip_list_columns_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            // Change visibility
            ((ToolStripMenuItem)e.ClickedItem).Checked = !((ToolStripMenuItem)e.ClickedItem).Checked;
            // Update database
            MyNesDB.UpdateColumn(e.ClickedItem.Text, ((ToolStripMenuItem)e.ClickedItem).Checked);
            // Update managed list
            if (!((ToolStripMenuItem)e.ClickedItem).Checked)
            {
                for (int i = 0; i < managedListView1.Columns.Count; i++)
                {
                    if (managedListView1.Columns[i].HeaderText == e.ClickedItem.Text)
                    {
                        managedListView1.Columns.RemoveAt(i);

                        break;
                    }
                }
            }
            else
            {
                MyNesDBColumn c = MyNesDB.GetColumn(e.ClickedItem.Text);
                int index = MyNesDB.GetColumnIndex(e.ClickedItem.Text);
                // Add it !
                ManagedListViewColumn col = new ManagedListViewColumn();
                col.HeaderText = e.ClickedItem.Text;
                col.ID = e.ClickedItem.Text;
                col.Width = c.Width;
                managedListView1.Columns.Insert(index, col);
            }
        }
        private void openFileLocationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (managedListView1.SelectedItems.Count != 1)
            {
                ManagedMessageBox.ShowErrorMessage(Program.ResourceManager.GetString("Message_PleaseSelectOneGameToOpenLocation"));
                return;
            }
            string id = managedListView1.SelectedItems[0].Tag.ToString();

            MyNesDBEntryInfo entry = MyNesDB.GetEntry(id);
            if (File.Exists(entry.Path))
            {
                try { Process.Start("explorer.exe", @"/select, " + entry.Path); }
                catch (Exception ex)
                { ManagedMessageBox.ShowErrorMessage(ex.Message); }
            }
            else
            {
                ManagedMessageBox.ShowErrorMessage(Program.ResourceManager.GetString("Message_GameFileNotExistOrNoFileAssignedToThisGameYet"));
            }
        }
        private void timer_progress_Tick(object sender, EventArgs e)
        {
            StatusLabel.Text = status;
            ProgressBar.Value = progress;
        }
        // Detect
        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            Detect();
        }
        private void timer_selection_Tick(object sender, EventArgs e)
        {
            if (_thread_busy) return;
            if (selectionTimer > 0)
                selectionTimer--;
            else { timer_selection.Stop(); UpdateSelection(); }
        }
        // For debug purpose.
        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            FormDetectSelection frm = new FormDetectSelection();
            if (frm.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                // Get the dataset of GAMES table
                DataSet ds = MyNesDB.GetDataSet(frm.MODE.ToString());

                if (ds == null) return;

                // Set columns
                if (ds.Tables[0].Rows.Count > 0)
                {
                    managedListView1.Items.Clear();
                    managedListView1.Columns.Clear();
                    for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
                    {
                        ManagedListViewColumn col = new ManagedListViewColumn();
                        col.HeaderText = col.ID = ds.Tables[0].Columns[i].ToString();
                        managedListView1.Columns.Add(col);
                    }

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        // Reached here means we can add the item
                        ManagedListViewItem item = new ManagedListViewItem();
                        // Put the id in the tag
                        item.Tag = ds.Tables[0].Rows[i]["Id"].ToString();
                        // Add subitems depending on columns
                        foreach (ManagedListViewColumn col in managedListView1.Columns)
                        {
                            ManagedListViewSubItem sub = new ManagedListViewSubItem();
                            sub.ColumnID = col.ID;
                            sub.Text = ds.Tables[0].Rows[i][col.ID].ToString();
                            item.SubItems.Add(sub);
                        }
                        managedListView1.Items.Add(item);
                    }
                }
            }
        }
        // Update settings
        private void toolStripSplitButton1_DropDownOpening(object sender, EventArgs e)
        {
            autoMinimizeToolStripMenuItem.Checked = Program.Settings.LauncherAutoMinimize;
            cycleImagesOnGameInfoToolStripMenuItem.Checked = Program.Settings.LauncherAutoCycleImagesInGameTab;
            rememberSelectionToolStripMenuItem.Checked = Program.Settings.LauncherRememberLastSelection;
            showLauncherAtAppStartToolStripMenuItem.Checked = Program.Settings.LauncherShowAyAppStart;
        }
        private void autoMinimizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.Settings.LauncherAutoMinimize = !Program.Settings.LauncherAutoMinimize;
        }
        private void cycleImagesOnGameInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.Settings.LauncherAutoCycleImagesInGameTab = !Program.Settings.LauncherAutoCycleImagesInGameTab;
        }
        private void rememberSelectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.Settings.LauncherRememberLastSelection = !Program.Settings.LauncherRememberLastSelection;
        }
        private void showLauncherAtAppStartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.Settings.LauncherShowAyAppStart = !Program.Settings.LauncherShowAyAppStart;
        }
        // Add files
        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = Program.ResourceManager.GetString("Title_AddFilesToTheDatabase");
            op.Filter = Program.ResourceManager.GetString("Filter_Rom");
            op.Multiselect = true;
            if (op.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                // Show working
                frmG = new FormGeneratingDatabase(false);
                frmG.Show(this);
                _assign_subfolders = false;
                _assign_addFilesNotFound = true;
                _assign_update_entries_already_assigned = false;
                AddFiles(op.FileNames);
                // Done !
                RefreshEntriesThreaded();
                CloseWorkFormThreaded();
            }
        }
        private void managedListView1_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.All;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }
        private void managedListView1_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                _assign_addFilesNotFound = true;
                AddFiles((string[])e.Data.GetData(DataFormats.FileDrop));
            }
        }
        private void ShowGameInfo(object sender, EventArgs e)
        {
            if (managedListView1.SelectedItems.Count != 1)
            {
                ManagedMessageBox.ShowErrorMessage(Program.ResourceManager.GetString("Message_PleaseSelectOneGameToShowInfoOf"));
                return;
            }
            string id = managedListView1.SelectedItems[0].Tag.ToString();

            bool isArchive = false;
            MyNesDBEntryInfo entry = MyNesDB.GetEntry(id);
            string path = entry.Path;
            string tempFile = path;
            int aIndex = 0;
            if (path.StartsWith("("))
            {
                isArchive = true;
                // Decode
                string[] pathCodes = path.Split(new char[] { '(', ')' });
                int.TryParse(pathCodes[1], out aIndex);
                path = pathCodes[2];
            }
            if (isArchive)
            {
                // Extract the archive
                string tempFolder = Path.GetTempPath() + "\\MYNES\\";

                Directory.CreateDirectory(tempFolder);
                // Extract it !
                SevenZipExtractor extractor = new SevenZipExtractor(path);
                tempFile = tempFolder + extractor.ArchiveFileData[aIndex].FileName;
                Stream aStream = new FileStream(tempFile, FileMode.Create, FileAccess.Write);
                extractor.ExtractFile(aIndex, aStream);
                aStream.Close();
            }
            if (File.Exists(tempFile))
            {
                if (NesEmu.EmulationON)
                    NesEmu.EmulationPaused = true;

                FormRomInfo form = new FormRomInfo(tempFile);
                form.ShowDialog(this);

                if (NesEmu.EmulationON)
                    NesEmu.EmulationPaused = false;
            }
            else
            {
                ManagedMessageBox.ShowErrorMessage(Program.ResourceManager.GetString("Message_GameFileNotExistOrNoFileAssignedToThisGameYet"));
            }
        }
        private void DeleteSelectedEntries(object sender, EventArgs e)
        {
            if (managedListView1.SelectedItems.Count == 0)
            {
                ManagedMessageBox.ShowErrorMessage(Program.ResourceManager.GetString("Message_NoEntrySelectedToDelete"));
                return;
            }

            ManagedMessageBoxResult res = ManagedMessageBox.ShowMessage(
                Program.ResourceManager.GetString("Message_AreYouSureYouWantToDeleteSelectedEntries"),
                Program.ResourceManager.GetString("MessageCaption_DeleteEntries"),
                new string[] { 
                    Program.ResourceManager.GetString("Button_Delete"), 
                    Program.ResourceManager.GetString("Button_DontDelete")}, 1, ManagedMessageBoxIcon.Question, true, true,
                    Program.ResourceManager.GetString("MessageCheckBox_DontDeleteNesCartEntries"));
            if (res.ClickedButtonIndex == 0)
            {
                ProgressBar.Visible = true;
                StatusLabel.Text = Program.ResourceManager.GetString("Status_DeletingEntries");
                int i = 0;
                int m = managedListView1.SelectedItems.Count;
                ManagedListViewItem[] items = managedListView1.SelectedItems.ToArray();
                foreach (ManagedListViewItem item in items)
                {
                    // Get entry
                    string id = item.Tag.ToString();
                    MyNesDBEntryInfo defaultEntry = MyNesDB.GetEntry(id);

                    if (res.Checked && defaultEntry.IsDB)
                        continue;

                    // Delete entry
                    MyNesDB.DeleteEntry(id);
                    managedListView1.Items.Remove(item);

                    ProgressBar.Value = (i * 100) / m;
                    statusStrip1.Refresh();
                    i++;
                }
                ProgressBar.Visible = false;
            }
        }
        // Sort
        private void managedListView1_ColumnClicked(object sender, ManagedListViewColumnClickArgs e)
        {
            if (_thread_busy) return;
            //get column and detect sort information
            ManagedListViewColumn column = managedListView1.Columns.GetColumnByID(e.ColumnID);
            if (column == null) return;
            bool az = false;
            switch (column.SortMode)
            {
                case ManagedListViewSortMode.AtoZ: az = false; break;
                case ManagedListViewSortMode.None:
                case ManagedListViewSortMode.ZtoA: az = true; break;
            }
            foreach (ManagedListViewColumn cl in managedListView1.Columns)
                cl.SortMode = ManagedListViewSortMode.None;
            column.SortMode = az ? ManagedListViewSortMode.AtoZ : ManagedListViewSortMode.ZtoA;
            // Do the sort !
            doSort = true;
            sortColumnName = e.ColumnID;
            sortAZ = az;
            RefreshEntries();
        }
    }
}
