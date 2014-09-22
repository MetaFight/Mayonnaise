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
using MyNes.Core;
using System.Reflection;
using MMB;
namespace MyNes
{
    public partial class FormRomInfo : Form
    {
        public FormRomInfo(string fileName)
        {
            InitializeComponent();
            LoadFile(fileName);
        }
        public void LoadFile(string fileName)
        {
            // Clear tabs
            tabControl1.TabPages.Clear();
            // See what header it is
            switch (Path.GetExtension(fileName).ToLower())
            {
                case ".nes":
                    {
                        // INES INFO !!
                        INes header = new INes();
                        header.Load(fileName, false);
                        if (header.IsValid)
                        {
                            ListView listView = new ListView();
                            listView.View = View.Details;
                            listView.ShowItemToolTips = true;
                            listView.GridLines = true;
                            listView.Columns.Add(Program.ResourceManager.GetString("Column_Property"), 100);
                            listView.Columns.Add(Program.ResourceManager.GetString("Column_Value"), 100);
                            listView.Dock = DockStyle.Fill;
                            TabPage page = new TabPage();
                            page.Controls.Add(listView);
                            page.Text = Program.ResourceManager.GetString("Title_INESHeader");
                            tabControl1.TabPages.Add(page);
                            // Add list view items
                            ListViewItem item = new ListViewItem(Program.ResourceManager.GetString("Title_Path"));
                            item.SubItems.Add(fileName);
                            listView.Items.Add(item);
                            item = new ListViewItem(Program.ResourceManager.GetString("Title_Size"));
                            item.SubItems.Add(GetFileSize(fileName));
                            listView.Items.Add(item);
                            item = new ListViewItem("SHA1");
                            item.SubItems.Add(header.SHA1);
                            listView.Items.Add(item);
                            item = new ListViewItem("CRC32");
                            item.SubItems.Add(CalculateCRC(fileName, 16));
                            listView.Items.Add(item);
                            item = new ListViewItem(Program.ResourceManager.GetString("Title_MapperNumber"));
                            item.SubItems.Add(header.MapperNumber.ToString());
                            listView.Items.Add(item);
                            item = new ListViewItem(Program.ResourceManager.GetString("Title_CHRCount"));
                            item.SubItems.Add(header.CHRCount.ToString());
                            listView.Items.Add(item);
                            item = new ListViewItem(Program.ResourceManager.GetString("Title_CHRSize"));
                            item.SubItems.Add(GetSize(header.CHRCount * 0x2000));
                            listView.Items.Add(item);
                            item = new ListViewItem(Program.ResourceManager.GetString("Title_PRGCount"));
                            item.SubItems.Add(header.PRGCount.ToString());
                            listView.Items.Add(item);
                            item = new ListViewItem(Program.ResourceManager.GetString("Title_PRGSize"));
                            item.SubItems.Add(GetSize(header.PRGCount * 0x4000));
                            listView.Items.Add(item);
                            item = new ListViewItem(Program.ResourceManager.GetString("Title_HasBattery"));
                            item.SubItems.Add(header.HasBattery.ToString());
                            listView.Items.Add(item);
                            item = new ListViewItem(Program.ResourceManager.GetString("Title_HasTrainer"));
                            item.SubItems.Add(header.HasTrainer.ToString());
                            listView.Items.Add(item);
                            item = new ListViewItem(Program.ResourceManager.GetString("Title_IsPlaychoice10"));
                            item.SubItems.Add(header.IsPlaychoice10.ToString());
                            listView.Items.Add(item);
                            item = new ListViewItem(Program.ResourceManager.GetString("Title_IsVSUnisystem"));
                            item.SubItems.Add(header.IsVSUnisystem.ToString());
                            listView.Items.Add(item);
                            item = new ListViewItem(Program.ResourceManager.GetString("Title_Mirroring"));
                            item.SubItems.Add(header.Mirroring.ToString());
                            listView.Items.Add(item);
                        }
                        else
                        {
                            // Add normal file info
                            ListView listView = new ListView();
                            listView.View = View.Details;
                            listView.ShowItemToolTips = true;
                            listView.GridLines = true;
                            listView.Columns.Add(Program.ResourceManager.GetString("Column_Property"), 100);
                            listView.Columns.Add(Program.ResourceManager.GetString("Column_Value"), 100);
                            listView.Dock = DockStyle.Fill;
                            TabPage page = new TabPage();
                            page.Controls.Add(listView);
                            page.Text = Program.ResourceManager.GetString("Title_FileInfo");
                            tabControl1.TabPages.Add(page);
                            // Add list view items
                            ListViewItem item = new ListViewItem(Program.ResourceManager.GetString("Title_Path"));
                            item.SubItems.Add(fileName);
                            listView.Items.Add(item);
                            item = new ListViewItem(Program.ResourceManager.GetString("Title_Size"));
                            item.SubItems.Add(GetFileSize(fileName));
                            listView.Items.Add(item);
                            item = new ListViewItem("SHA1");
                            item.SubItems.Add(header.SHA1);
                            listView.Items.Add(item);
                            item = new ListViewItem("CRC32");
                            item.SubItems.Add(CalculateCRC(fileName, 16));
                            listView.Items.Add(item);
                        }
                        // Add database info if found !
                        //Get database info
                        bool found = false;
                        NesCartDatabaseGameInfo info = NesCartDatabase.Find(header.SHA1, out found);
                        NesCartDatabaseCartridgeInfo cart = new NesCartDatabaseCartridgeInfo();
                        if (info.Cartridges != null)
                        {
                            foreach (NesCartDatabaseCartridgeInfo cartinf in info.Cartridges)
                                if (cartinf.SHA1.ToLower() == header.SHA1.ToLower())
                                {
                                    cart = cartinf;
                                    break;
                                }
                        }
                        if (found)
                        {
                            ListView listView = new ListView();
                            listView.View = View.Details;
                            listView.ShowItemToolTips = true;
                            listView.GridLines = true;
                            listView.Columns.Add(Program.ResourceManager.GetString("Column_Property"), 100);
                            listView.Columns.Add(Program.ResourceManager.GetString("Column_Value"), 100);
                            listView.Dock = DockStyle.Fill;
                            TabPage page = new TabPage();
                            page.Controls.Add(listView);
                            page.Text = Program.ResourceManager.GetString("Title_GameInfoFromNesCartDatabase");
                            tabControl1.TabPages.Add(page);
                            //Game info
                            ListViewGroup gr = new ListViewGroup(Program.ResourceManager.GetString("title_GameInfo"));
                            listView.Groups.Add(gr);
                            FieldInfo[] Fields = typeof(NesCartDatabaseGameInfo).GetFields(BindingFlags.Public
                            | BindingFlags.Instance);
                            bool ColorOr = false;
                            for (int i = 0; i < Fields.Length; i++)
                            {
                                if (Fields[i].FieldType == typeof(System.String))
                                {
                                    listView.Items.Add(Fields[i].Name.Replace("_", " "));
                                    gr.Items.Add(listView.Items[listView.Items.Count - 1]);
                                    try
                                    {
                                        listView.Items[listView.Items.Count - 1].SubItems.Add(Fields[i].GetValue
                                            (info).ToString());
                                    }
                                    catch
                                    {
                                        listView.Items[listView.Items.Count - 1].SubItems.Add("");
                                    }
                                    if (ColorOr)
                                        listView.Items[listView.Items.Count - 1].BackColor = Color.WhiteSmoke;
                                    ColorOr = !ColorOr;
                                }
                            }

                            //chips
                            if (cart.chip_type != null)
                            {
                                for (int i = 0; i < cart.chip_type.Count; i++)
                                {
                                    listView.Items.Add("Chip " + (i + 1));
                                    gr.Items.Add(listView.Items[listView.Items.Count - 1]);
                                    listView.Items[listView.Items.Count - 1].SubItems.Add(cart.chip_type[i]);
                                    if (ColorOr)
                                        listView.Items[listView.Items.Count - 1].BackColor = Color.WhiteSmoke;
                                    ColorOr = !ColorOr;
                                }
                            }

                            //Cartridge
                            gr = new ListViewGroup(Program.ResourceManager.GetString("Title_Cartridge"));
                            listView.Groups.Add(gr);
                            Fields = typeof(NesCartDatabaseCartridgeInfo).GetFields(BindingFlags.Public
                            | BindingFlags.Instance);
                            ColorOr = false;
                            for (int i = 0; i < Fields.Length; i++)
                            {
                                if (Fields[i].FieldType == typeof(System.String))
                                {
                                    listView.Items.Add(Fields[i].Name.Replace("_", " "));
                                    gr.Items.Add(listView.Items[listView.Items.Count - 1]);
                                    try
                                    {
                                        listView.Items[listView.Items.Count - 1].SubItems.Add(Fields[i].GetValue(cart).ToString());
                                    }
                                    catch
                                    {
                                        listView.Items[listView.Items.Count - 1].SubItems.Add("");
                                    }
                                    if (ColorOr)
                                        listView.Items[listView.Items.Count - 1].BackColor = Color.WhiteSmoke;
                                    ColorOr = !ColorOr;
                                }
                            }

                            //DataBase
                            gr = new ListViewGroup(Program.ResourceManager.GetString("Title_DataBase"));
                            listView.Groups.Add(gr);
                            Fields = typeof(NesCartDatabase).GetFields(BindingFlags.Public
                          | BindingFlags.Static);
                            ColorOr = false;
                            for (int i = 0; i < Fields.Length; i++)
                            {
                                if (Fields[i].FieldType == typeof(System.String))
                                {
                                    listView.Items.Add(Fields[i].Name.Remove(0, 2));
                                    gr.Items.Add(listView.Items[listView.Items.Count - 1]);
                                    try
                                    {
                                        listView.Items[listView.Items.Count - 1].SubItems.Add(Fields[i].GetValue(info).ToString());
                                    }
                                    catch
                                    {
                                        listView.Items[listView.Items.Count - 1].SubItems.Add("");
                                    }
                                    if (ColorOr)
                                        listView.Items[listView.Items.Count - 1].BackColor = Color.WhiteSmoke;
                                    ColorOr = !ColorOr;
                                }
                            }
                        }
                        break;
                    }
            }
        }
        private string GetFileSize(string FilePath)
        {
            if (File.Exists(Path.GetFullPath(FilePath)) == true)
            {
                FileInfo Info = new FileInfo(FilePath);
                string Unit = " " + "Byte";
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
        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
        // Copy to clipboard
        private void button2_Click(object sender, EventArgs e)
        {
            ListView listView1 = (ListView)tabControl1.TabPages[tabControl1.SelectedIndex].Controls[0];
            RichTextBox textBox = new RichTextBox();
            foreach (ListViewItem item in listView1.Items)
            {
                textBox.Text += item.Text + ": " + item.SubItems[1].Text + "\n";
            }
            textBox.SelectAll();
            textBox.Copy();
            ManagedMessageBox.ShowMessage(Program.ResourceManager.GetString("Message_Done"));
        }
    }
}
