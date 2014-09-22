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
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using MMB;
using AcroPDFLib;

namespace MyNes
{
    public partial class ManualsViewer : UserControl
    {
        public ManualsViewer()
        {
            InitializeComponent();
            // Create new pdf browser !
            try
            {
                pdfBrowser = new AxAcroPDFLib.AxAcroPDF();
                pdfBrowser.Parent = this;
                pdfBrowser.Dock = DockStyle.Fill;
                pdfBrowser.BringToFront();
                initialized = true;
                label1.Visible = false;
            }
            catch
            {
                initialized = false;
                label1.Visible = true;
            }
            Clear();
        }
        private bool initialized = false;
        private AxAcroPDFLib.AxAcroPDF pdfBrowser;
        private string currentID;
        private MyNesDetectEntryInfo[] detects;
        private int fileIndex;
        private List<string> extensions =
            new List<string>(new string[] { ".pdf" });

        private void Clear()
        {
            try
            {
                // TODO: clear adobe reader control.
                if (initialized)
                    pdfBrowser.Visible = false;
            }
            catch { }

            toolStripButton_next.Enabled = false;
            toolStripButton_previous.Enabled = false;
            toolStripButton_add.Enabled = false;
            toolStripButton_delete.Enabled = false;
            toolStripButton_deleteAll.Enabled = false;
            toolStripButton_openLocation.Enabled = false;
            toolStripButton_openDefaultBrowser.Enabled = false;
            StatusLabel.Text = "0 / 0";
            detects = new MyNesDetectEntryInfo[0];
            fileIndex = -1;
        }
        private void ShowCurrentFile()
        {
            try
            {
                if (initialized)
                    pdfBrowser.Visible = false;
            }
            catch { }
            StatusLabel.Text = (fileIndex + 1) + " / " + detects.Length;
            toolStripButton_previous.Enabled = toolStripButton_next.Enabled = detects.Length > 1;
            toolStripButton_delete.Enabled =
            toolStripButton_deleteAll.Enabled =
            toolStripButton_openLocation.Enabled =
            toolStripButton_openDefaultBrowser.Enabled = fileIndex >= 0;

            if (fileIndex >= 0 && fileIndex < detects.Length)
            {
                try
                {
                    if (initialized)
                    {
                        pdfBrowser.Visible = true;
                        pdfBrowser.LoadFile(detects[fileIndex].Path);
                        System.Diagnostics.Trace.WriteLine("PDF FILE LOADED: " + detects[fileIndex].Path);
                    }
                }
                catch
                {
                }
            }
        }
        public void RefreshForEntry(string id)
        {
            currentID = id;
            Clear();
            if (id == "") return;
            // Get images for given game id
            detects = MyNesDB.GetDetects("MANUALS", id);
            toolStripButton_add.Enabled = true;
            if (detects == null) return;
            if (detects.Length > 0)
                fileIndex = 0;
            ShowCurrentFile();
        }
        private void toolStripButton_add_Click(object sender, EventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = Program.ResourceManager.GetString("Title_AddMoreManuals");
            op.Filter = Program.ResourceManager.GetString("Filter_PDF");
            op.Multiselect = true;
            if (op.ShowDialog(this) == DialogResult.OK)
            {
                foreach (string file in op.FileNames)
                {
                    // Make sure this file isn't exist for selected game
                    bool found = false;
                    if (detects != null)
                    {
                        foreach (MyNesDetectEntryInfo inf in detects)
                        {
                            if (inf.Path == file)
                            {
                                found = true; break;
                            }
                        }
                    }
                    if (!found)
                    {
                        // Add it !
                        MyNesDetectEntryInfo newDetect = new MyNesDetectEntryInfo();
                        newDetect.GameID = currentID;
                        newDetect.Path = file;
                        newDetect.Name = Path.GetFileNameWithoutExtension(file);
                        newDetect.FileInfo = "";
                        MyNesDB.AddDetect("MANUALS", newDetect);
                    }
                }
                RefreshForEntry(currentID);
            }
        }
        private void toolStripButton_next_Click(object sender, EventArgs e)
        {
            if (detects == null) return;
            if (detects.Length > 0)
            {
                fileIndex = (fileIndex + 1) % detects.Length;
            }
            else
            {
                fileIndex = -1;
            }
            ShowCurrentFile();
        }
        private void toolStripButton_previous_Click(object sender, EventArgs e)
        {
            if (detects == null) return;
            if (detects.Length > 0)
            {
                fileIndex--;
                if (fileIndex < 0)
                    fileIndex = detects.Length - 1;
            }
            else
            {
                fileIndex = -1;
            }
            ShowCurrentFile();
        }
        private void toolStripButton_openDefaultBrowser_Click(object sender, EventArgs e)
        {
            if (detects == null) return;
            if (fileIndex >= 0 && fileIndex < detects.Length)
            {
                try
                {
                    System.Diagnostics.Process.Start(detects[fileIndex].Path);
                }
                catch { }
            }
        }
        private void toolStripButton_openLocation_Click(object sender, EventArgs e)
        {
            if (detects == null) return;
            if (fileIndex >= 0 && fileIndex < detects.Length)
            {
                try
                {
                    System.Diagnostics.Process.Start("explorer.exe", @"/select, " + detects[fileIndex].Path);
                }
                catch { }
            }
        }
        private void toolStripButton_delete_Click(object sender, EventArgs e)
        {
            if (detects == null) return;
            if (fileIndex >= 0 && fileIndex < detects.Length)
            {
                ManagedMessageBoxResult res = ManagedMessageBox.ShowQuestionMessage(
                    Program.ResourceManager.GetString("Message_AreYouSuretoRemoveManual"),
                    Program.ResourceManager.GetString("MessageCaption_RemoveManual"), true, false,
                    Program.ResourceManager.GetString("MessageCheckBox_RemoveFileFromDiskToo"));
                if (res.ClickedButtonIndex == 0)// Yes !
                {
                    if (res.Checked)
                    {
                        try
                        {
                            File.Delete(detects[fileIndex].Path);
                        }
                        catch (Exception ex)
                        {
                            ManagedMessageBox.ShowErrorMessage(Program.ResourceManager.GetString("Text_UnableToRemoveFile") + ": " + detects[fileIndex].Path + "; ERROR: " + ex.Message);
                        }
                    }
                    // Remove from database.
                    MyNesDB.DeleteDetect("MANUALS", currentID, detects[fileIndex].Path);
                    RefreshForEntry(currentID);
                }
            }
        }
        private void toolStripButton_deleteAll_Click(object sender, EventArgs e)
        {
            if (detects == null) return;
            if (fileIndex >= 0 && fileIndex < detects.Length)
            {
                ManagedMessageBoxResult res = ManagedMessageBox.ShowQuestionMessage(
                   Program.ResourceManager.GetString("Message_AreYousureToRemoveAllManuals"),
                   Program.ResourceManager.GetString("MessageCaption_RemoveManual"), true, false,
                   Program.ResourceManager.GetString("MessageCheckBox_RemoveFileFromDiskToo"));
                if (res.ClickedButtonIndex == 0)// Yes !
                {
                    if (res.Checked)
                    {
                        foreach (MyNesDetectEntryInfo inf in detects)
                        {
                            try
                            {
                                File.Delete(inf.Path);
                            }
                            catch
                            {
                            }
                        }
                    }
                    // Remove from database.
                    MyNesDB.DeleteDetects("MANUALS", currentID);
                    RefreshForEntry(currentID);
                }
            }
        }
        private void ManualsViewer_DragOver(object sender, DragEventArgs e)
        {
            if (detects == null) { e.Effect = DragDropEffects.None; return; }
            if (currentID == "") { e.Effect = DragDropEffects.None; return; }
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.All;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }
        private void ManualsViewer_DragDrop(object sender, DragEventArgs e)
        {
            if (detects == null) return;
            if (currentID == "") return;
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                foreach (string file in files)
                {
                    if (!extensions.Contains(Path.GetExtension(file).ToLower())) continue;
                    // Make sure this file isn't exist for selected game
                    bool found = false;
                    if (detects != null)
                    {
                        foreach (MyNesDetectEntryInfo inf in detects)
                        {
                            if (inf.Path == file)
                            {
                                found = true; break;
                            }
                        }
                    }
                    if (!found)
                    {
                        // Add it !
                        MyNesDetectEntryInfo newDetect = new MyNesDetectEntryInfo();
                        newDetect.GameID = currentID;
                        newDetect.Path = file;
                        newDetect.Name = Path.GetFileNameWithoutExtension(file);
                        newDetect.FileInfo = "";
                        MyNesDB.AddDetect("MANUALS", newDetect);
                    }
                }
                RefreshForEntry(currentID);
            }
        }
    }
}
