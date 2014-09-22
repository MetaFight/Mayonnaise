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
using MMB;
namespace MyNes
{
    public partial class FormPathsSettings : Form
    {
        public FormPathsSettings()
        {
            InitializeComponent();
            LoadSettings();
        }
        private void LoadSettings()
        {
            textBox_Sram.Text = Program.Settings.Folder_Sram;
            textBox_state.Text = Program.Settings.Folder_State;
            textBox_snapshots.Text = Program.Settings.Folder_Snapshots;
        }
        private void button6_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void button5_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(textBox_Sram.Text))
            {
                ManagedMessageBoxResult res = ManagedMessageBox.ShowQuestionMessage(
                    Program.ResourceManager.GetString("Message_TheSRAMFolderIsntExistAt") + " " +
                    textBox_Sram.Text + ".\n" +
                    Program.ResourceManager.GetString("Message_DoYouWantToCreateThisFolder"));
                if (res.ClickedButtonIndex == 1)
                    return;
                else
                {
                    try
                    {
                        Directory.CreateDirectory(textBox_Sram.Text);
                    }
                    catch (Exception ex)
                    {
                        ManagedMessageBox.ShowErrorMessage(Program.ResourceManager.GetString("Message_UnableToCreateTheFolder") + ": " + ex.Message);
                    }
                }
            }
            if (!Directory.Exists(textBox_state.Text))
            {
                ManagedMessageBoxResult res = ManagedMessageBox.ShowQuestionMessage(
                    Program.ResourceManager.GetString("Message_TheSTATEFolderIsntExistAt") + " " +
                      textBox_state.Text + ".\n" + Program.ResourceManager.GetString("Message_DoYouWantToCreateThisFolder"));
                if (res.ClickedButtonIndex == 1)
                    return;
                else
                {
                    try
                    {
                        Directory.CreateDirectory(textBox_state.Text);
                    }
                    catch (Exception ex)
                    {
                        ManagedMessageBox.ShowErrorMessage(Program.ResourceManager.GetString("Message_UnableToCreateTheFolder") + ": " + ex.Message);
                    }
                }
            }
            if (!Directory.Exists(textBox_snapshots.Text))
            {
                ManagedMessageBoxResult res = ManagedMessageBox.ShowQuestionMessage(Program.ResourceManager.GetString("Message_TheSnapshotsFolderIsntExistAt") + " " +
                     textBox_snapshots.Text + ".\n" + Program.ResourceManager.GetString("Message_DoYouWantToCreateThisFolder"));
                if (res.ClickedButtonIndex == 1)
                    return;
                else
                {
                    try
                    {
                        Directory.CreateDirectory(textBox_snapshots.Text);
                    }
                    catch (Exception ex)
                    {
                        ManagedMessageBox.ShowErrorMessage(Program.ResourceManager.GetString("Message_UnableToCreateTheFolder") + ": " + ex.Message);
                    }
                }
            }
            Program.Settings.Folder_Sram = textBox_Sram.Text;
            Program.Settings.Folder_State = textBox_state.Text;
            Program.Settings.Folder_Snapshots = textBox_snapshots.Text;
            Program.Settings.Save();
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folder = new FolderBrowserDialog();
            folder.Description = Program.ResourceManager.GetString("Title_ChangeTheStateSavesFolder");
            folder.SelectedPath = textBox_state.Text;
            folder.ShowNewFolderButton = true;
            if (folder.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                textBox_state.Text = folder.SelectedPath;
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folder = new FolderBrowserDialog();
            folder.Description = Program.ResourceManager.GetString("Title_ChangeTheSRAMFolder");
            folder.SelectedPath = textBox_Sram.Text;
            folder.ShowNewFolderButton = true;
            if (folder.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                textBox_Sram.Text = folder.SelectedPath;
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(textBox_state.Text);
            }
            catch { }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(textBox_Sram.Text);
            }
            catch { }
        }
        private void button7_Click(object sender, EventArgs e)
        {
            textBox_Sram.Text = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\MyNes\\SRAM\\";
            textBox_state.Text = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\MyNes\\STATE\\";
            textBox_snapshots.Text = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\MyNes\\SNAPS\\";
        }
        private void button9_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folder = new FolderBrowserDialog();
            folder.Description = Program.ResourceManager.GetString("Title_ChangeTheSnaphotsFolder");
            folder.SelectedPath = textBox_snapshots.Text;
            folder.ShowNewFolderButton = true;
            if (folder.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                textBox_snapshots.Text = folder.SelectedPath;
            }
        }
        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(textBox_snapshots.Text);
            }
            catch { }
        }
    }
}
