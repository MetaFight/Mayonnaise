namespace MyNes
{
    partial class FormFirstRun
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBox_language = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.checkBox_runLauncher = new System.Windows.Forms.CheckBox();
            this.checkBox_resize = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.checkBox_soundPlayback = new System.Windows.Forms.CheckBox();
            this.checkBox_fullscreen = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Image = global::MyNes.Properties.Resources.MyNes;
            this.label1.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(326, 39);
            this.label1.TabIndex = 0;
            this.label1.Text = "Welcome to My Nes !!\r\nThis dialog appears once at the first time you run My Nes.\r" +
    "\nPlease take a moment to configure My Nes.";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.Image = global::MyNes.Properties.Resources.world;
            this.label2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label2.Location = new System.Drawing.Point(12, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(145, 22);
            this.label2.TabIndex = 1;
            this.label2.Text = "Interface Language:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // comboBox_language
            // 
            this.comboBox_language.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_language.FormattingEnabled = true;
            this.comboBox_language.Location = new System.Drawing.Point(12, 80);
            this.comboBox_language.Name = "comboBox_language";
            this.comboBox_language.Size = new System.Drawing.Size(326, 21);
            this.comboBox_language.TabIndex = 2;
            this.toolTip1.SetToolTip(this.comboBox_language, "Select the interface language that will be used\r\nfor My Nes windows and dialogs.");
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(263, 246);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "&OK";
            this.toolTip1.SetToolTip(this.button1, "OK !");
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // checkBox_runLauncher
            // 
            this.checkBox_runLauncher.Image = global::MyNes.Properties.Resources.application_view_list;
            this.checkBox_runLauncher.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.checkBox_runLauncher.Location = new System.Drawing.Point(12, 107);
            this.checkBox_runLauncher.Name = "checkBox_runLauncher";
            this.checkBox_runLauncher.Size = new System.Drawing.Size(181, 26);
            this.checkBox_runLauncher.TabIndex = 4;
            this.checkBox_runLauncher.Text = "&Open launcher at start-up";
            this.checkBox_runLauncher.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.checkBox_runLauncher, "Indicates if the lancher should open each time \r\nMy Nes starts");
            this.checkBox_runLauncher.UseVisualStyleBackColor = true;
            // 
            // checkBox_resize
            // 
            this.checkBox_resize.Checked = true;
            this.checkBox_resize.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_resize.Image = global::MyNes.Properties.Resources.application_get;
            this.checkBox_resize.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.checkBox_resize.Location = new System.Drawing.Point(12, 175);
            this.checkBox_resize.Name = "checkBox_resize";
            this.checkBox_resize.Size = new System.Drawing.Size(212, 25);
            this.checkBox_resize.TabIndex = 5;
            this.checkBox_resize.Text = "&Resize window to windowed size";
            this.checkBox_resize.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.checkBox_resize, "Indicates if the main window should be resized\r\nto the windowed size when a game " +
        "starts.\r\nThe windowed size can be chaged later via Video\r\nsettings.");
            this.checkBox_resize.UseVisualStyleBackColor = true;
            // 
            // checkBox_soundPlayback
            // 
            this.checkBox_soundPlayback.Checked = true;
            this.checkBox_soundPlayback.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_soundPlayback.Image = global::MyNes.Properties.Resources.sound;
            this.checkBox_soundPlayback.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.checkBox_soundPlayback.Location = new System.Drawing.Point(12, 139);
            this.checkBox_soundPlayback.Name = "checkBox_soundPlayback";
            this.checkBox_soundPlayback.Size = new System.Drawing.Size(166, 30);
            this.checkBox_soundPlayback.TabIndex = 6;
            this.checkBox_soundPlayback.Text = "&Enable sound playback";
            this.checkBox_soundPlayback.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.checkBox_soundPlayback, "Indicates if the sound playback is enabled.");
            this.checkBox_soundPlayback.UseVisualStyleBackColor = true;
            // 
            // checkBox_fullscreen
            // 
            this.checkBox_fullscreen.Image = global::MyNes.Properties.Resources.monitor;
            this.checkBox_fullscreen.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.checkBox_fullscreen.Location = new System.Drawing.Point(12, 206);
            this.checkBox_fullscreen.Name = "checkBox_fullscreen";
            this.checkBox_fullscreen.Size = new System.Drawing.Size(166, 25);
            this.checkBox_fullscreen.TabIndex = 7;
            this.checkBox_fullscreen.Text = "&Run game in fullscreen";
            this.checkBox_fullscreen.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.checkBox_fullscreen, "Indicates if the emu should run game in fullscreen\r\nmode.");
            this.checkBox_fullscreen.UseVisualStyleBackColor = true;
            // 
            // FormFirstRun
            // 
            this.AcceptButton = this.button1;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(350, 281);
            this.ControlBox = false;
            this.Controls.Add(this.checkBox_fullscreen);
            this.Controls.Add(this.checkBox_soundPlayback);
            this.Controls.Add(this.checkBox_resize);
            this.Controls.Add(this.checkBox_runLauncher);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.comboBox_language);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormFirstRun";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "First Run";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBox_language;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox checkBox_runLauncher;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.CheckBox checkBox_resize;
        private System.Windows.Forms.CheckBox checkBox_soundPlayback;
        private System.Windows.Forms.CheckBox checkBox_fullscreen;
    }
}