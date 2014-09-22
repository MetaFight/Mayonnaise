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
using MyNes.Core;
namespace MyNes
{
    public partial class GameInfoViewer : UserControl
    {
        public GameInfoViewer()
        {
            InitializeComponent();
            Clear();
        }
        private string currentID;
        private MyNesDetectEntryInfo[] images;
        private int fileIndex;
        public event EventHandler RatingChanged;

        private void Clear()
        {
            label_name.Text = label_path.Text = label_size.Text = Program.ResourceManager.GetString("Message_NoGameSelected");
            rating1.rating = 0;
            rating1.Enabled = false;
            imagePanel1.ImageToView = null;
            imagePanel1.Invalidate();
            images = new MyNesDetectEntryInfo[0];
            fileIndex = -1;
            timer1.Stop();
            listView1.Items.Clear();
        }
        private void ShowCurrentFile()
        {
            imagePanel1.ImageToView = null;

            if (fileIndex >= 0 && fileIndex < images.Length)
            {
                try
                {
                    string filePath = images[fileIndex].Path;
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
            // Get info for selected game
            MyNesDBEntryInfo inf = MyNesDB.GetEntry(id);
            label_name.Text = inf.Name;
            toolTip1.SetToolTip(label_name, inf.Name);
            label_size.Text = GetSize(inf.Size);
            toolTip1.SetToolTip(label_size, label_size.Text);
            label_path.Text = inf.Path;
            toolTip1.SetToolTip(label_path, inf.Path);
            rating1.rating = inf.Rating;
            rating1.Enabled = true;
            // Add images
            List<MyNesDetectEntryInfo> detects = new List<MyNesDetectEntryInfo>();
            detects.AddRange(MyNesDB.GetDetects("SNAPS", id));
            detects.AddRange(MyNesDB.GetDetects("COVERS", id));
            images = detects.ToArray();
            if (images.Length > 0)
                fileIndex = 0;
            if (Program.Settings.LauncherAutoCycleImagesInGameTab)
                timer1.Start();
            ShowCurrentFile();
            // Load data infos
            DataSet ds = MyNesDB.GetEntryDataSet(currentID);
            for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
            {
                if (ds.Tables[0].Columns[i].ToString() != "Id" && ds.Tables[0].Columns[i].ToString() != "IsDB")
                {
                    string colName = ds.Tables[0].Columns[i].ToString();
                    string subText = ds.Tables[0].Rows[0][ds.Tables[0].Columns[i].ToString()].ToString().Replace("&apos;", "'");
                    listView1.Items.Add(colName);
                    if (colName == "Played")
                    {
                        if (subText == "0")
                            subText = Program.ResourceManager.GetString("Text_NeverPlayed");
                        else if (subText == "1")
                            subText = Program.ResourceManager.GetString("Text_OneTime");
                        else
                            subText = subText + " " + Program.ResourceManager.GetString("Text_Times");
                    }
                    else if (colName == "Play Time")
                    {
                        if (subText == "0")
                            subText = Program.ResourceManager.GetString("Text_NeverPlayed");
                        else
                        {
                            int val = 0;
                            int.TryParse(subText, out val);
                            subText = TimeSpan.FromSeconds(val).ToString();
                        }
                    }
                    else if (colName == "Size")
                    {
                        int val = 0;
                        int.TryParse(subText, out val);
                        subText = GetSize(val);
                    }
                    else if (colName == "Last Played")
                    {
                        DateTime time = (DateTime)ds.Tables[0].Rows[0][colName];
                        if (time != DateTime.MinValue)
                            subText = time.ToLocalTime().ToString();
                        else
                            subText = Program.ResourceManager.GetString("Text_NeverPlayed");
                    }

                    listView1.Items[listView1.Items.Count - 1].SubItems.Add(subText);
                }
            }
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
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!Program.Settings.LauncherAutoCycleImagesInGameTab)
            { timer1.Stop(); return; }
            if (NesEmu.EmulationON)
            { timer1.Stop(); return; }
            if (images == null)
            { timer1.Stop(); return; }
            if (images.Length > 0)
            {
                fileIndex = (fileIndex + 1) % images.Length;
            }
            else
            {
                fileIndex = -1;
            }
            ShowCurrentFile();
        }
        private void rating1_RatingChanged(object sender, EventArgs e)
        {
            if (currentID == "")
            {
                rating1.rating = 0;
                return;
            }
            // Update entry
            if (MyNesDB.IsDatabaseLoaded)
                MyNesDB.UpdateEntry(currentID, rating1.rating);
            if (RatingChanged != null)
                RatingChanged(rating1, new EventArgs());
        }
        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RichTextBox textBox = new RichTextBox();
            foreach (ListViewItem item in listView1.Items)
            {
                textBox.Text += item.Text + ": " + item.SubItems[1].Text + "\n";
            }
            textBox.SelectAll();
            textBox.Copy();
            MessageBox.Show(Program.ResourceManager.GetString("Message_InfoCopiedIntoTheClipboard"));
        }
    }
}
