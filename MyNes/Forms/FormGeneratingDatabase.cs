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
using System.Diagnostics;
namespace MyNes
{
    public partial class FormGeneratingDatabase : Form
    {
        public FormGeneratingDatabase(bool isCenter)
        {
            InitializeComponent();
            label2.TextAlign = isCenter ? ContentAlignment.MiddleCenter : ContentAlignment.TopLeft;
            Trace.Listeners.Add(listner = new FormGeneratingDatabaseTraceListner(this));
            timer1.Start();
            CancelRequest = false;
        }
        private FormGeneratingDatabaseTraceListner listner;
        public bool CancelRequest;
        private delegate void WriteStatusDelegate(string mess, Color col);
        public void WriteStatus(string message, Color color)
        {
            if (!this.InvokeRequired)
                WriteStatus1(message, color);
            else
                this.Invoke(new WriteStatusDelegate(WriteStatus1), message, color);
        }
        private void WriteStatus1(string message, Color color)
        {
            label2.Text = message;
            label2.ForeColor = color;
            label2.Refresh();
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            int val = (progressBar1.Value + 1);
            if (val > 100) val = 0;
            progressBar1.Value = val;
            progressBar1.Refresh();
        }
        private void FormGeneratingDatabase_FormClosing(object sender, FormClosingEventArgs e)
        {
            Trace.Listeners.Remove(listner);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            CancelRequest = true;
        }
    }
    public class FormGeneratingDatabaseTraceListner : TraceListener
    {
        public FormGeneratingDatabaseTraceListner(FormGeneratingDatabase form)
        {
            this.form = form;
        }
        private FormGeneratingDatabase form;
        public override void Write(string message)
        {
            if (form == null) return;

            form.WriteStatus(message, System.Drawing.Color.Black);
        }
        public override void WriteLine(string message)
        {
            if (form == null) return;

            form.WriteStatus(message, System.Drawing.Color.Black);
        }
        public override void WriteLine(string message, string category)
        {
            if (form == null) return;

            form.WriteStatus(category + ": " + message, System.Drawing.Color.Black);
        }
        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string message)
        {
            if (form == null) return;

            switch (eventType)
            {
                case TraceEventType.Information: form.WriteStatus(source + ": " + message, System.Drawing.Color.Black); break;
                case TraceEventType.Warning: form.WriteStatus(source + ": " + message, System.Drawing.Color.Yellow); break;
                case TraceEventType.Error: form.WriteStatus(source + ": " + message, System.Drawing.Color.Red); break;
                default: form.WriteStatus(source + ": " + message, System.Drawing.Color.Black); break;
            }
        }
    }
}
