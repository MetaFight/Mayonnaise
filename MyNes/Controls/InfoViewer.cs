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

namespace MyNes
{
    public partial class InfoViewer : UserControl
    {
        public InfoViewer()
        {
            InitializeComponent();
            Clear();
        }
        private string currentID;
        private MyNesDetectEntryInfo[] detects;
        private int fileIndex;
        private List<string> extensions = new List<string>(new string[] { ".txt", ".rtf", ".doc" });

        private void Clear()
        {
            richTextBox1.Text = "";
            richTextBox1.Enabled = false;
            toolStripButton_newFile.Enabled = false;
            toolStripButton_SaveChanges.Enabled = false;
            toolStripButton_addMoreFiles.Enabled = false;
            toolStripButton_deleteSelected.Enabled = false;
            toolStripButton_deleteAll.Enabled = false;
            toolStripButton_openLocation.Enabled = false;
            toolStripButton_edit.Enabled = false;
            toolStripButton_next.Enabled = false;
            toolStripButton_prevous.Enabled = false;

            StatusLabel.Text = "0 / 0";
            detects = new MyNesDetectEntryInfo[0];
            fileIndex = -1;
        }
        private void ShowCurrentFile()
        {
            richTextBox1.Text = "";
            richTextBox1.Enabled = false;
            StatusLabel.Text = (fileIndex + 1) + " / " + detects.Length;
            toolStripButton_prevous.Enabled = toolStripButton_next.Enabled = detects.Length > 1;

            toolStripButton_deleteSelected.Enabled =
            toolStripButton_deleteAll.Enabled =
            toolStripButton_openLocation.Enabled =
            toolStripButton_edit.Enabled = fileIndex >= 0;

            if (fileIndex >= 0 && fileIndex < detects.Length)
            {
                richTextBox1.Enabled = true;
                toolStripButton_SaveChanges.Enabled = true;
                try
                {
                    string filePath = detects[fileIndex].Path;
                    switch (Path.GetExtension(filePath).ToLower())
                    {
                        case ".rtf":
                        case ".doc": richTextBox1.LoadFile(filePath); break;
                        default: richTextBox1.Lines = File.ReadAllLines(filePath); break;
                    }
                }
                catch (Exception ex)
                {
                    richTextBox1.Text = ex.Message;
                    //richTextBox1.SelectionStart = 0;
                    //richTextBox1.SelectionColor = Color.Red;
                    //richTextBox1.SelectedText = ex.Message;
                }
            }
        }
        public void RefreshForEntry(string id)
        {
            currentID = id;
            Clear();
            if (id == "") return;
            // Get files for given game id
            detects = MyNesDB.GetDetects("INFOS", id);
            toolStripButton_addMoreFiles.Enabled = true;
            toolStripButton_newFile.Enabled = true;
            if (detects == null) return;
            if (detects.Length > 0)
                fileIndex = 0;
            ShowCurrentFile();
        }
        private void toolStripButton_newFile_Click(object sender, EventArgs e)
        {
            if (currentID == "") return;
            SaveFileDialog sav = new SaveFileDialog();
            sav.Title = Program.ResourceManager.GetString("Title_NewInfoFile");
            sav.Filter = Program.ResourceManager.GetString("Filter_InfoFile");
            // Determine file name
            MyNesDBEntryInfo entry = MyNesDB.GetEntry(currentID);
            sav.FileName = entry.Name + ".txt";
            if (sav.ShowDialog(this) == DialogResult.OK)
            {
                // Save the file !
                switch (Path.GetExtension(sav.FileName).ToLower())
                {
                    case ".rtf":
                    case ".doc": richTextBox1.SaveFile(sav.FileName); break;
                    default: File.WriteAllLines(sav.FileName, richTextBox1.Lines, Encoding.UTF8); break;
                }
                // Make sure this file isn't exist for selected game
                bool found = false;
                if (detects != null)
                {
                    for (int i = 0; i < detects.Length; i++)
                    {
                        if (detects[i].Path == sav.FileName)
                        {

                            fileIndex = i;
                            ShowCurrentFile();
                            found = true;
                            return;
                        }
                    }
                }
                if (!found)
                {
                    // Add it !
                    MyNesDetectEntryInfo newDetect = new MyNesDetectEntryInfo();
                    newDetect.GameID = currentID;
                    newDetect.Path = sav.FileName;
                    newDetect.Name = Path.GetFileNameWithoutExtension(sav.FileName);
                    newDetect.FileInfo = "";
                    MyNesDB.AddDetect("INFOS", newDetect);
                    // Refresh
                    RefreshForEntry(currentID);
                    fileIndex = detects.Length - 1;
                    ShowCurrentFile();
                }
            }
        }
        private void toolStripButton_SaveChanges_Click(object sender, EventArgs e)
        {
            if (currentID == "") return;
            if (fileIndex >= 0 && fileIndex < detects.Length)
            {
                try
                {
                    string filePath = detects[fileIndex].Path;
                    switch (Path.GetExtension(filePath).ToLower())
                    {
                        case ".rtf":
                        case ".doc": richTextBox1.SaveFile(filePath); break;
                        default: File.WriteAllLines(filePath, richTextBox1.Lines, Encoding.UTF8); break;
                    }
                }
                catch (Exception ex)
                {
                    ManagedMessageBox.ShowErrorMessage(ex.Message);
                }
            }
        }
        private void toolStripButton_addMoreFiles_Click(object sender, EventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = Program.ResourceManager.GetString("Title_AddMoreInfoFiles");
            op.Filter = Program.ResourceManager.GetString("Filter_InfoFile");
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
                        MyNesDB.AddDetect("INFOS", newDetect);
                    }
                }
                RefreshForEntry(currentID);
            }
        }
        private void toolStripButton_deleteSelected_Click(object sender, EventArgs e)
        {
            if (detects == null) return;
            if (fileIndex >= 0 && fileIndex < detects.Length)
            {
                ManagedMessageBoxResult res = ManagedMessageBox.ShowQuestionMessage(
                    Program.ResourceManager.GetString("Message_AreYouSureToRemoveInfoFile"),
                    Program.ResourceManager.GetString("MessageCaption_RemoveInfoFile"), true, false,
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
                            ManagedMessageBox.ShowErrorMessage(Program.ResourceManager.GetString("Text_UnableToRemoveFile") +
                                ": " + detects[fileIndex].Path + "; ERROR: " + ex.Message);
                        }
                    }
                    // Remove from database.
                    MyNesDB.DeleteDetect("INFOS", currentID, detects[fileIndex].Path);
                    RefreshForEntry(currentID);
                }
            }
        }
        private void toolStripButton_edit_Click(object sender, EventArgs e)
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
        private void toolStripButton_prevous_Click(object sender, EventArgs e)
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
        private void toolStripButton_deleteAll_Click(object sender, EventArgs e)
        {
            if (detects == null) return;
            if (fileIndex >= 0 && fileIndex < detects.Length)
            {
                ManagedMessageBoxResult res = ManagedMessageBox.ShowQuestionMessage(
                Program.ResourceManager.GetString("Message_AreYouSureToRemoveAllInfoFiles"),
                Program.ResourceManager.GetString("MessageCaption_RemoveInfoFile"), true, false,
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
                    MyNesDB.DeleteDetects("INFOS", currentID);
                    RefreshForEntry(currentID);
                }
            }
        }
        private void InfoViewer_DragDrop(object sender, DragEventArgs e)
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
                        MyNesDB.AddDetect("INFOS", newDetect);
                    }
                }
                RefreshForEntry(currentID);
            }
        }
        private void InfoViewer_DragOver(object sender, DragEventArgs e)
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
    }
}
