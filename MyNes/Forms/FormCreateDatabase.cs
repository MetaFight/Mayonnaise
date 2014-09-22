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
namespace MyNes
{
    public partial class FormCreateDatabase : Form
    {
        public FormCreateDatabase()
        {
            InitializeComponent();
            textBox1.Text = Path.GetFullPath(Program.Settings.Database_FilePath);
        }
        public string DBName { get { return textBox_dbName.Text; } }
        public string DBPath { get { return textBox1.Text; } }
        public bool DBGenerateNesCart { get { return checkBox_generateNesCart.Checked; } }
        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Program.Settings.Database_FilePath = textBox1.Text;
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }
        private void textBox_dbName_TextChanged(object sender, EventArgs e)
        {
            button2.Enabled = textBox_dbName.Text.Length > 0 && textBox1.Text.Length > 0;
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            button2.Enabled = textBox_dbName.Text.Length > 0 && textBox1.Text.Length > 0;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            SaveFileDialog sav = new SaveFileDialog();
            sav.Title = Program.ResourceManager.GetString("Title_SaveMyNesDatabaseFile");
            sav.Filter = Program.ResourceManager.GetString("Filter_MyNesDatabase");
            sav.FileName = textBox1.Text;
            if (sav.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                textBox1.Text = sav.FileName;
            }
        }
    }
}
