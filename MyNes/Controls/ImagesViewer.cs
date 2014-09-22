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
    public partial class ImagesViewer : UserControl
    {
        public ImagesViewer()
        {
            InitializeComponent();
            Clear();
            // Load settings
            if (MODE == DetectMode.SNAPS)
            {
                imagePanel1.ImageViewMode = Program.Settings.SnapsView_ImageMode;
                toolStrip_bar.Visible = Program.Settings.SnapsView_ShowBar;
                toolStrip_status.Visible = Program.Settings.SnapsView_ShowStatus;
            }
            else
            {
                imagePanel1.ImageViewMode = Program.Settings.CoversView_ImageMode;
                toolStrip_bar.Visible = Program.Settings.CoversView_ShowBar;
                toolStrip_status.Visible = Program.Settings.CoversView_ShowStatus;
            }
            switch (imagePanel1.ImageViewMode)
            {
                case ImageViewMode.StretchIfLarger:
                    {
                        normalstretchIfLargerWithAspectRatioToolStripMenuItem.Checked = true;
                        alwaysStretchnoAspectRatioToolStripMenuItem.Checked = false;
                        alwaysStretchwithAspectRatioToolStripMenuItem.Checked = false;
                        break;
                    }
                case ImageViewMode.StretchNoAspectRatio:
                    {
                        normalstretchIfLargerWithAspectRatioToolStripMenuItem.Checked = false;
                        alwaysStretchnoAspectRatioToolStripMenuItem.Checked = true;
                        alwaysStretchwithAspectRatioToolStripMenuItem.Checked = false;
                        break;
                    }
                case ImageViewMode.StretchToFit:
                    {
                        normalstretchIfLargerWithAspectRatioToolStripMenuItem.Checked = false;
                        alwaysStretchnoAspectRatioToolStripMenuItem.Checked = false;
                        alwaysStretchwithAspectRatioToolStripMenuItem.Checked = true;
                        break;
                    }
            }
        }
        public DetectMode MODE { get; set; }
        private string currentID;
        private MyNesDetectEntryInfo[] detects;
        private int fileIndex;
        private List<string> extensions =
            new List<string>(new string[] { ".jpg", ".png", ".bmp", ".gif", ".jpeg", ".tiff", ".tif", ".tga", ".ico" });

        private void Clear()
        {
            imagePanel1.ImageToView = null;
            imagePanel1.Invalidate();
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
            imagePanel1.ImageToView = null;
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
                    string filePath = detects[fileIndex].Path;
                    Stream str = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                    byte[] buff = new byte[str.Length];
                    str.Read(buff, 0, (int)str.Length);
                    str.Dispose();
                    str.Close();

                    imagePanel1.zoom = -1;
                    imagePanel1.ImageToView = (Bitmap)Image.FromStream(new MemoryStream(buff));

                    toolTip1.SetToolTip(imagePanel1, filePath);
                    // Reached here means the load is success.
                    imagePanel1.Text = "";
                }
                catch (Exception ex)
                {
                    imagePanel1.ImageToView = null;
                    imagePanel1.ForeColor = Color.Red;
                    imagePanel1.Text = Program.ResourceManager.GetString("Message_UNABLETOSHOWIMAGE") + "\n\n" + ex.Message;
                }
            }
            imagePanel1.Invalidate();
        }
        public void RefreshForEntry(string id)
        {
            currentID = id;
            Clear();
            if (id == "") return;
            // Get images for given game id
            detects = MyNesDB.GetDetects(MODE.ToString(), id);
            toolStripButton_add.Enabled = true;
            if (detects == null) return;
            if (detects.Length > 0)
                fileIndex = 0;
            ShowCurrentFile();
        }

        // Previous !
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
        // Next !
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
        private void normalstretchIfLargerWithAspectRatioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            normalstretchIfLargerWithAspectRatioToolStripMenuItem.Checked = true;
            alwaysStretchnoAspectRatioToolStripMenuItem.Checked = false;
            alwaysStretchwithAspectRatioToolStripMenuItem.Checked = false;
            if (MODE == DetectMode.SNAPS)
                Program.Settings.SnapsView_ImageMode = imagePanel1.ImageViewMode = ImageViewMode.StretchIfLarger;
            else
                Program.Settings.CoversView_ImageMode = imagePanel1.ImageViewMode = ImageViewMode.StretchIfLarger;
            imagePanel1.Invalidate();
        }
        private void alwaysStretchnoAspectRatioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            normalstretchIfLargerWithAspectRatioToolStripMenuItem.Checked = false;
            alwaysStretchnoAspectRatioToolStripMenuItem.Checked = true;
            alwaysStretchwithAspectRatioToolStripMenuItem.Checked = false;
            if (MODE == DetectMode.SNAPS)
                Program.Settings.SnapsView_ImageMode = imagePanel1.ImageViewMode = ImageViewMode.StretchNoAspectRatio;
            else
                Program.Settings.CoversView_ImageMode = imagePanel1.ImageViewMode = ImageViewMode.StretchNoAspectRatio;
            imagePanel1.Invalidate();
        }
        private void alwaysStretchwithAspectRatioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            normalstretchIfLargerWithAspectRatioToolStripMenuItem.Checked = false;
            alwaysStretchnoAspectRatioToolStripMenuItem.Checked = false;
            alwaysStretchwithAspectRatioToolStripMenuItem.Checked = true;
            if (MODE == DetectMode.SNAPS)
                Program.Settings.SnapsView_ImageMode = imagePanel1.ImageViewMode = ImageViewMode.StretchToFit;
            else
                Program.Settings.CoversView_ImageMode = imagePanel1.ImageViewMode = ImageViewMode.StretchToFit;
            imagePanel1.Invalidate();
        }
        private void toolStripButton_add_Click(object sender, EventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = Program.ResourceManager.GetString("Title_AddMoreImages");
            op.Filter = Program.ResourceManager.GetString("Filter_Image");
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
                        MyNesDB.AddDetect(MODE.ToString(), newDetect);
                    }
                }
                RefreshForEntry(currentID);
            }
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
                    Program.ResourceManager.GetString("Message_AreYouSureToRemoveImage"),
                    Program.ResourceManager.GetString("MessageCaption_RemoveImage"), true, false,
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
                                ": " + detects[fileIndex].Path + "; " + Program.ResourceManager.GetString("ERROR") + ": " + ex.Message);
                        }
                    }
                    // Remove from database.
                    MyNesDB.DeleteDetect(MODE.ToString(), currentID, detects[fileIndex].Path);
                    RefreshForEntry(currentID);
                }
            }
        }
        private void ImagesViewer_DragOver(object sender, DragEventArgs e)
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
        private void ImagesViewer_DragDrop(object sender, DragEventArgs e)
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
                        MyNesDB.AddDetect(MODE.ToString(), newDetect);
                    }
                }
                RefreshForEntry(currentID);
            }
        }
        private void toolStripButton_deleteAll_Click(object sender, EventArgs e)
        {
            if (detects == null) return;
            if (fileIndex >= 0 && fileIndex < detects.Length)
            {
                ManagedMessageBoxResult res = ManagedMessageBox.ShowQuestionMessage(
                 Program.ResourceManager.GetString("Message_AreYouSureToRemoveAllImages"),
                 Program.ResourceManager.GetString("MessageCaption_RemoveImage"),
                 true, false,
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
                    MyNesDB.DeleteDetects(MODE.ToString(), currentID);
                    RefreshForEntry(currentID);
                }
            }
        }
        private void imagePanel1_MouseDoubleClick(object sender, MouseEventArgs e)
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
        private void ImagesViewer_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Right)
                toolStripButton_next_Click(this, null);
            else if (e.KeyCode == Keys.Left)
                toolStripButton_previous_Click(this, null);
            else if (e.KeyCode == Keys.Return)
                toolStripButton_openDefaultBrowser_Click(this, null);
        }
        private void showToolbarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStrip_bar.Visible = !toolStrip_bar.Visible;
            if (MODE == DetectMode.SNAPS)
            {
                Program.Settings.SnapsView_ShowBar = toolStrip_bar.Visible;
            }
            else
            {
                Program.Settings.CoversView_ShowBar = toolStrip_bar.Visible;
            }
        }
        private void showStatusbarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStrip_status.Visible = !toolStrip_status.Visible;
            if (MODE == DetectMode.SNAPS)
            {
                Program.Settings.SnapsView_ShowStatus = toolStrip_status.Visible;
            }
            else
            {
                Program.Settings.CoversView_ShowStatus = toolStrip_status.Visible;
            }
        }
        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            nextToolStripMenuItem.Enabled = toolStripButton_next.Enabled;
            previousToolStripMenuItem.Enabled = toolStripButton_previous.Enabled;
            addMoreImagesToolStripMenuItem.Enabled = toolStripButton_add.Enabled;
            deleteAllToolStripMenuItem.Enabled = toolStripButton_deleteAll.Enabled;
            deleteSelectedToolStripMenuItem.Enabled = toolStripButton_delete.Enabled;
            openWithWindowsDefaultAppToolStripMenuItem.Enabled = toolStripButton_openDefaultBrowser.Enabled;
            openLocationToolStripMenuItem.Enabled = toolStripButton_openLocation.Enabled;
            switch (imagePanel1.ImageViewMode)
            {
                case ImageViewMode.StretchIfLarger:
                    {
                        normalstretchIfLargerWithAspectRatioToolStripMenuItem1.Checked = true;
                        alwaysStretchnoAspectRatioToolStripMenuItem1.Checked = false;
                        alwaysStretchwithAspectRatioToolStripMenuItem1.Checked = false;
                        break;
                    }
                case ImageViewMode.StretchNoAspectRatio:
                    {
                        normalstretchIfLargerWithAspectRatioToolStripMenuItem1.Checked = false;
                        alwaysStretchnoAspectRatioToolStripMenuItem1.Checked = true;
                        alwaysStretchwithAspectRatioToolStripMenuItem1.Checked = false;
                        break;
                    }
                case ImageViewMode.StretchToFit:
                    {
                        normalstretchIfLargerWithAspectRatioToolStripMenuItem1.Checked = false;
                        alwaysStretchnoAspectRatioToolStripMenuItem1.Checked = false;
                        alwaysStretchwithAspectRatioToolStripMenuItem1.Checked = true;
                        break;
                    }
            }
            showStatusbarToolStripMenuItem.Checked = toolStrip_status.Visible;
            showToolbarToolStripMenuItem.Checked = toolStrip_bar.Visible;
        }
    }
}
