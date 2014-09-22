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
using System.IO;
using System.Windows.Forms;
using MMB;

namespace MyNes
{
    public partial class FormAssignFilesToDB : Form
    {
        public FormAssignFilesToDB()
        {
            InitializeComponent();
            if (Program.Settings.Database_FoldersScanned != null)
                foreach (string folder in Program.Settings.Database_FoldersScanned)
                    if (Directory.Exists(folder))
                        listBox1.Items.Add(folder);
        }
        public string[] FoldersToScan
        {
            get
            {
                List<string> folders = new List<string>();
                foreach (string item in listBox1.Items)
                    folders.Add(item);
                return folders.ToArray();
            }
        }
        public bool IncludeSubFolders { get { return checkBox_subfolders.Checked; } }
        public bool AddFilesNotFound { get { return checkBox_addFilesNotFoundInDB.Checked; } }
        public bool UpdateEntriesAlreadyAssigned { get { return checkBox_updateAlreadyAssigned.Checked; } }
        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folder = new FolderBrowserDialog();
            folder.Description = Program.ResourceManager.GetString("Text_AddFolderToTheScanList");
            folder.ShowNewFolderButton = true;

            if (listBox1.SelectedIndex >= 0)
                folder.SelectedPath = (string)listBox1.Items[listBox1.SelectedIndex];

            if (folder.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                if (!listBox1.Items.Contains(folder.SelectedPath))
                    listBox1.Items.Add(folder.SelectedPath);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0)
                listBox1.Items.RemoveAt(listBox1.SelectedIndex);
        }
        private void button4_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (listBox1.Items.Count == 0)
            {
                ManagedMessageBox.ShowErrorMessage(Program.ResourceManager.GetString("Message_YouMustSekectOneFolderToSearch"));
                return;
            }
            Program.Settings.Database_FoldersScanned = new System.Collections.Specialized.StringCollection();
            foreach (string item in listBox1.Items)
                Program.Settings.Database_FoldersScanned.Add(item);
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }

        private void FormAssignFilesToDB_Load(object sender, EventArgs e)
        {

        }
    }
}
