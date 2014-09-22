/* This file is part of Managed Message Box
   A message box component with advanced capacities

   Copyright © Ala Ibrahim Hadid 2013

   This program is free software: you can redistribute it and/or modify
   it under the terms of the GNU General Public License as published by
   the Free Software Foundation, either version 3 of the License, or
   (at your option) any later version.

   This program is distributed in the hope that it will be useful,
   but WITHOUT ANY WARRANTY; without even the implied warranty of
   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
   GNU General Public License for more details.

   You should have received a copy of the GNU General Public License
   along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using MMB.Forms;
namespace MMB
{
    /*Implement the custom message methods*/
    public sealed partial class ManagedMessageBox
    {
        /// <summary>
        /// Show customized managed message box
        /// </summary>
        /// <param name="messageText">The message text to show</param>
        /// <param name="messageCaption">The message caption (title of the managed message box window)</param>
        /// <param name="messageTextFont">The font to use for message text</param>
        /// <param name="messageTextColor">The color to use for message text</param>
        /// <param name="backgroundColor">The message box background color</param>
        /// <param name="buttons">The buttons collection to use (one button text must be presented at least)</param>
        /// <param name="defaultButtonIndex">The zero index value indicates the default button in the buttons list.</param>
        /// <returns><see cref="MMB.ManagedMessageBoxResult"/> object includes which button clicked and the check box result.</returns>
        public static ManagedMessageBoxResult ShowCustomMessage(string messageText, string messageCaption,
            Font messageTextFont, Color messageTextColor, Color backgroundColor, string[] buttons, int defaultButtonIndex)
        {
            return ShowCustomMessage(null, messageText, messageCaption, messageTextFont, messageTextColor, backgroundColor,
               buttons, defaultButtonIndex, null, null, false, false, "", _rightToLeft);
        }
        /// <summary>
        /// Show customized managed message box
        /// </summary>
        /// <param name="ParentWindow">The parent window that should handle the managed message box window</param>
        /// <param name="messageText">The message text to show</param>
        /// <param name="messageCaption">The message caption (title of the managed message box window)</param>
        /// <param name="messageTextFont">The font to use for message text</param>
        /// <param name="messageTextColor">The color to use for message text</param>
        /// <param name="backgroundColor">The message box background color</param>
        /// <param name="buttons">The buttons collection to use (one button text must be presented at least)</param>
        /// <param name="defaultButtonIndex">The zero index value indicates the default button in the buttons list.</param>
        /// <returns><see cref="MMB.ManagedMessageBoxResult"/> object includes which button clicked and the check box result.</returns>
        public static ManagedMessageBoxResult ShowCustomMessage(IWin32Window ParentWindow, string messageText, string messageCaption,
            Font messageTextFont, Color messageTextColor, Color backgroundColor, string[] buttons, int defaultButtonIndex)
        {
            return ShowCustomMessage(ParentWindow, messageText, messageCaption, messageTextFont, messageTextColor, backgroundColor,
               buttons, defaultButtonIndex, null, null, false, false, "", _rightToLeft);
        }
        /// <summary>
        /// Show customized managed message box
        /// </summary>
        /// <param name="messageText">The message text to show</param>
        /// <param name="messageCaption">The message caption (title of the managed message box window)</param>
        /// <param name="messageTextFont">The font to use for message text</param>
        /// <param name="messageTextColor">The color to use for message text</param>
        /// <param name="backgroundColor">The message box background color</param>
        /// <param name="buttons">The buttons collection to use (one button text must be presented at least)</param>
        /// <param name="defaultButtonIndex">The zero index value indicates the default button in the buttons list.</param>
        /// <param name="icon">The managed message box icon, PNG format image is recommended</param>
        /// <returns><see cref="MMB.ManagedMessageBoxResult"/> object includes which button clicked and the check box result.</returns>
        public static ManagedMessageBoxResult ShowCustomMessage(string messageText, string messageCaption,
            Font messageTextFont, Color messageTextColor, Color backgroundColor, string[] buttons, int defaultButtonIndex,
            Image icon)
        {
            return ShowCustomMessage(null, messageText, messageCaption, messageTextFont, messageTextColor, backgroundColor,
               buttons, defaultButtonIndex, null, icon, false, false, "", _rightToLeft);
        }
        /// <summary>
        /// Show customized managed message box
        /// </summary>
        /// <param name="ParentWindow">The parent window that should handle the managed message box window</param>
        /// <param name="messageText">The message text to show</param>
        /// <param name="messageCaption">The message caption (title of the managed message box window)</param>
        /// <param name="messageTextFont">The font to use for message text</param>
        /// <param name="messageTextColor">The color to use for message text</param>
        /// <param name="backgroundColor">The message box background color</param>
        /// <param name="buttons">The buttons collection to use (one button text must be presented at least)</param>
        /// <param name="defaultButtonIndex">The zero index value indicates the default button in the buttons list.</param>
        /// <param name="icon">The managed message box icon, PNG format image is recommended</param>
        /// <returns><see cref="MMB.ManagedMessageBoxResult"/> object includes which button clicked and the check box result.</returns>
        public static ManagedMessageBoxResult ShowCustomMessage(IWin32Window ParentWindow, string messageText, string messageCaption,
            Font messageTextFont, Color messageTextColor, Color backgroundColor, string[] buttons, int defaultButtonIndex,
            Image icon)
        {
            return ShowCustomMessage(ParentWindow, messageText, messageCaption, messageTextFont, messageTextColor, backgroundColor,
               buttons, defaultButtonIndex, null, icon, false, false, "", _rightToLeft);
        }
        /// <summary>
        /// Show customized managed message box
        /// </summary>
        /// <param name="messageText">The message text to show</param>
        /// <param name="messageCaption">The message caption (title of the managed message box window)</param>
        /// <param name="messageTextFont">The font to use for message text</param>
        /// <param name="messageTextColor">The color to use for message text</param>
        /// <param name="backgroundColor">The message box background color</param>
        /// <param name="buttons">The buttons collection to use (one button text must be presented at least)</param>
        /// <param name="defaultButtonIndex">The zero index value indicates the default button in the buttons list.</param>
        /// <param name="controls">More control to add long side with buttons. The controls you may add will not effect the result
        /// , The result is for buttons and the check box only. This can be null to add no control.</param>
        /// <param name="icon">The managed message box icon, PNG format image is recommended</param>
        /// <returns><see cref="MMB.ManagedMessageBoxResult"/> object includes which button clicked and the check box result.</returns>
        public static ManagedMessageBoxResult ShowCustomMessage(string messageText, string messageCaption,
            Font messageTextFont, Color messageTextColor, Color backgroundColor, string[] buttons, int defaultButtonIndex,
            Control[] controls, Image icon)
        {
            return ShowCustomMessage(null, messageText, messageCaption, messageTextFont, messageTextColor, backgroundColor,
               buttons, defaultButtonIndex, controls, icon, false, false, "", _rightToLeft);
        }
        /// <summary>
        /// Show customized managed message box
        /// </summary>
        /// <param name="ParentWindow">The parent window that should handle the managed message box window</param>
        /// <param name="messageText">The message text to show</param>
        /// <param name="messageCaption">The message caption (title of the managed message box window)</param>
        /// <param name="messageTextFont">The font to use for message text</param>
        /// <param name="messageTextColor">The color to use for message text</param>
        /// <param name="backgroundColor">The message box background color</param>
        /// <param name="buttons">The buttons collection to use (one button text must be presented at least)</param>
        /// <param name="defaultButtonIndex">The zero index value indicates the default button in the buttons list.</param>
        /// <param name="controls">More control to add long side with buttons. The controls you may add will not effect the result
        /// , The result is for buttons and the check box only. This can be null to add no control.</param>
        /// <param name="icon">The managed message box icon, PNG format image is recommended</param>
        /// <returns><see cref="MMB.ManagedMessageBoxResult"/> object includes which button clicked and the check box result.</returns>
        public static ManagedMessageBoxResult ShowCustomMessage(IWin32Window ParentWindow, string messageText, string messageCaption,
            Font messageTextFont, Color messageTextColor, Color backgroundColor, string[] buttons, int defaultButtonIndex,
            Control[] controls, Image icon)
        {
            return ShowCustomMessage(ParentWindow, messageText, messageCaption, messageTextFont, messageTextColor, backgroundColor,
               buttons, defaultButtonIndex, controls, icon, false, false, "", _rightToLeft);
        }
        /// <summary>
        /// Show customized managed message box
        /// </summary>
        /// <param name="messageText">The message text to show</param>
        /// <param name="messageCaption">The message caption (title of the managed message box window)</param>
        /// <param name="messageTextFont">The font to use for message text</param>
        /// <param name="messageTextColor">The color to use for message text</param>
        /// <param name="backgroundColor">The message box background color</param>
        /// <param name="buttons">The buttons collection to use (one button text must be presented at least)</param>
        /// <param name="defaultButtonIndex">The zero index value indicates the default button in the buttons list.</param>
        /// <param name="controls">More control to add long side with buttons. The controls you may add will not effect the result
        /// , The result is for buttons and the check box only. This can be null to add no control.</param>
        /// <param name="icon">The managed message box icon, PNG format image is recommended</param>
        /// <param name="showCheckBox">Indicate whether to show the check box or not</param>
        /// <param name="checkBoxValue">The default check box value if the check box is enabled (showCheckBox = true).</param>
        /// <param name="checkBoxText">The check box text if the check box is enabled (showCheckBox = true).</param>
        /// <returns><see cref="MMB.ManagedMessageBoxResult"/> object includes which button clicked and the check box result.</returns>
        public static ManagedMessageBoxResult ShowCustomMessage(string messageText, string messageCaption,
            Font messageTextFont, Color messageTextColor, Color backgroundColor, string[] buttons, int defaultButtonIndex,
            Control[] controls, Image icon, bool showCheckBox, bool checkBoxValue, string checkBoxText)
        {
            return ShowCustomMessage(null, messageText, messageCaption, messageTextFont, messageTextColor, backgroundColor,
               buttons, defaultButtonIndex, controls, icon, showCheckBox, checkBoxValue, checkBoxText, _rightToLeft);
        }
        /// <summary>
        /// Show customized managed message box
        /// </summary>
        /// <param name="ParentWindow">The parent window that should handle the managed message box window</param>
        /// <param name="messageText">The message text to show</param>
        /// <param name="messageCaption">The message caption (title of the managed message box window)</param>
        /// <param name="messageTextFont">The font to use for message text</param>
        /// <param name="messageTextColor">The color to use for message text</param>
        /// <param name="backgroundColor">The message box background color</param>
        /// <param name="buttons">The buttons collection to use (one button text must be presented at least)</param>
        /// <param name="defaultButtonIndex">The zero index value indicates the default button in the buttons list.</param>
        /// <param name="controls">More control to add long side with buttons. The controls you may add will not effect the result
        /// , The result is for buttons and the check box only. This can be null to add no control.</param>
        /// <param name="icon">The managed message box icon, PNG format image is recommended</param>
        /// <param name="showCheckBox">Indicate whether to show the check box or not</param>
        /// <param name="checkBoxValue">The default check box value if the check box is enabled (showCheckBox = true).</param>
        /// <param name="checkBoxText">The check box text if the check box is enabled (showCheckBox = true).</param>
        /// <returns><see cref="MMB.ManagedMessageBoxResult"/> object includes which button clicked and the check box result.</returns>
        public static ManagedMessageBoxResult ShowCustomMessage(IWin32Window ParentWindow, string messageText, string messageCaption,
            Font messageTextFont, Color messageTextColor, Color backgroundColor, string[] buttons, int defaultButtonIndex,
            Control[] controls, Image icon, bool showCheckBox, bool checkBoxValue, string checkBoxText)
        {
            return ShowCustomMessage(ParentWindow, messageText, messageCaption, messageTextFont, messageTextColor, backgroundColor,
               buttons, defaultButtonIndex, controls, icon, showCheckBox, checkBoxValue, checkBoxText, _rightToLeft);
        }
        /// <summary>
        /// Show customized managed message box
        /// </summary>
        /// <param name="messageText">The message text to show</param>
        /// <param name="messageCaption">The message caption (title of the managed message box window)</param>
        /// <param name="messageTextFont">The font to use for message text</param>
        /// <param name="messageTextColor">The color to use for message text</param>
        /// <param name="backgroundColor">The message box background color</param>
        /// <param name="buttons">The buttons collection to use (one button text must be presented at least)</param>
        /// <param name="defaultButtonIndex">The zero index value indicates the default button in the buttons list.</param>
        /// <param name="controls">More control to add long side with buttons. The controls you may add will not effect the result
        /// , The result is for buttons and the check box only. This can be null to add no control.</param>
        /// <param name="icon">The managed message box icon, PNG format image is recommended</param>
        /// <param name="showCheckBox">Indicate whether to show the check box or not</param>
        /// <param name="checkBoxValue">The default check box value if the check box is enabled (showCheckBox = true).</param>
        /// <param name="checkBoxText">The check box text if the check box is enabled (showCheckBox = true).</param>
        /// <param name="RightToLeft">Indicate whether the message box should be right to left alignment for right to left languages like Arabic</param>
        /// <returns><see cref="MMB.ManagedMessageBoxResult"/> object includes which button clicked and the check box result.</returns>
        public static ManagedMessageBoxResult ShowCustomMessage(string messageText, string messageCaption,
            Font messageTextFont, Color messageTextColor, Color backgroundColor, string[] buttons, int defaultButtonIndex,
            Control[] controls, Image icon, bool showCheckBox, bool checkBoxValue, string checkBoxText, bool RightToLeft)
        {
            return ShowCustomMessage(null, messageText, messageCaption, messageTextFont, messageTextColor, backgroundColor,
               buttons, defaultButtonIndex, controls, icon, showCheckBox, checkBoxValue, checkBoxText, RightToLeft);
        }
        /// <summary>
        /// Show customized managed message box
        /// </summary>
        /// <param name="ParentWindow">The parent window that should handle the managed message box window</param>
        /// <param name="messageText">The message text to show</param>
        /// <param name="messageCaption">The message caption (title of the managed message box window)</param>
        /// <param name="messageTextFont">The font to use for message text</param>
        /// <param name="messageTextColor">The color to use for message text</param>
        /// <param name="backgroundColor">The message box background color</param>
        /// <param name="buttons">The buttons collection to use (one button text must be presented at least)</param>
        /// <param name="defaultButtonIndex">The zero index value indicates the default button in the buttons list.</param>
        /// <param name="controls">More control to add long side with buttons. The controls you may add will not effect the result
        /// , The result is for buttons and the check box only. This can be null to add no control.</param>
        /// <param name="icon">The managed message box icon, PNG format image is recommended</param>
        /// <param name="showCheckBox">Indicate whether to show the check box or not</param>
        /// <param name="checkBoxValue">The default check box value if the check box is enabled (showCheckBox = true).</param>
        /// <param name="checkBoxText">The check box text if the check box is enabled (showCheckBox = true).</param>
        /// <param name="RightToLeft">Indicate whether the message box should be right to left alignment for right to left languages like Arabic</param>
        /// <returns><see cref="MMB.ManagedMessageBoxResult"/> object includes which button clicked and the check box result.</returns>
        public static ManagedMessageBoxResult ShowCustomMessage(IWin32Window ParentWindow, string messageText, string messageCaption,
             Font messageTextFont, Color messageTextColor, Color backgroundColor, string[] buttons, int defaultButtonIndex,
             Control[] controls, Image icon, bool showCheckBox, bool checkBoxValue, string checkBoxText, bool RightToLeft)
        {
            // Check errors
            if (buttons == null)
                throw new Exception("Unable to display Managed Message Box, no button passed !");
            if (buttons.Length == 0)
                throw new Exception("Unable to display Managed Message Box, no button passed !");
            if (defaultButtonIndex < 0)
                throw new Exception("Invalid default button value. It must be larger or equal 0 and less than buttons count");
            if (defaultButtonIndex >= buttons.Length)
                throw new Exception("Invalid default button value. It must be larger or equal 0 and less than buttons count");
            // Show the form. This form will hold our buttons, icon and the check box
            frm = new Form_ManagedMesssageBox();
            frm.richTextBox1.BackColor = frm.tableLayoutPanel1.BackColor = frm.BackColor = backgroundColor;
            if (RightToLeft)
            {
                frm.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
                frm.RightToLeftLayout = true;
            }
            // Set texts
            frm.Text = messageCaption;
            frm.label_message.Text = messageText;
            frm.label_message.Font = messageTextFont;
            frm.label_message.ForeColor = messageTextColor;
            frm.label_message.BackColor = frm.label_icon.BackColor = Color.Transparent;
            // Icon
            if (icon != null)
                frm.label_icon.Image = icon;
            else
                frm.label_icon.Visible = false;
            // Check box
            frm.checkBox1.Visible = showCheckBox;
            frm.checkBox1.Text = checkBoxText;
            frm.checkBox1.Checked = checkBoxValue;
            // Buttons
            buttonsTemp = new List<string>(buttons);
            int w = 0;
            frm.tableLayoutPanel1.ColumnCount = buttons.Length;
            frm.tableLayoutPanel1.AutoSize = true;
            for (int i = 0; i < buttons.Length; i++)
            {
                Button _button = new Button();
                _button.Click += ManagedMessageBox_Click;
                _button.Text = buttons[i];
                _button.AutoSize = true;
                if (i == defaultButtonIndex)
                    frm.AcceptButton = _button;
                frm.tableLayoutPanel1.Controls.Add(_button);
                frm.tableLayoutPanel1.SetColumn(_button, i);
                w += _button.Width + frm.tableLayoutPanel1.Margin.All;
            }
            // Controls
            if (controls != null)
            {
                frm.tableLayoutPanel1.ColumnCount += controls.Length;
                for (int i = 0; i < controls.Length; i++)
                {
                    frm.tableLayoutPanel1.Controls.Add(controls[i]);
                    frm.tableLayoutPanel1.SetColumn(controls[i], frm.tableLayoutPanel1.Controls.Count - 1);
                    w += controls[i].Width + frm.tableLayoutPanel1.Margin.All;
                }
            }
            // Set size
            int width = frm.Width;
            int height = frm.Height;
            if (w > frm.tableLayoutPanel1.Width)
            {
                width = w - frm.tableLayoutPanel1.Width;
                width += frm.tableLayoutPanel1.Margin.All * 2;

                width = frm.Width + width;
                if (width > 1000)
                    width = 1000;
            }
            int tHeight = 0;
            string[] textLines = messageText.Split(new char[] { '\n' });
            foreach (string line in textLines)
            {
                // see how many lines we can do for this text line
                Size tSize = TextRenderer.MeasureText(messageText, defaultFont);
                double linesCount = (double)tSize.Width / (double)(frm.label_message.Width - 20);
                tHeight += tSize.Height * (int)Math.Ceiling(linesCount);

                if (tHeight > 1000) break;// we handle this later using the rich text box
            }
            if (tHeight > frm.label_message.Size.Height)
            {
                height += tHeight - frm.label_message.Size.Height;
                if (height > 1000)
                {
                    // in this case, we must use the rich text box !
                    frm.label_message.Visible = false;
                    frm.richTextBox1.Visible = true;
                    frm.richTextBox1.Text = messageText;
                    frm.richTextBox1.Font = defaultFont;
                    frm.richTextBox1.ForeColor = defaultTextColor;
                    height = frm.Height + 100;
                }
            }
            frm.Size = new Size(width, height);

            // Show
            if (ParentWindow != null)
                frm.ShowDialog(ParentWindow);
            else
                frm.ShowDialog();
            // After closed, return result to user
            return new ManagedMessageBoxResult(clickedButton, buttonsTemp.IndexOf(clickedButton), checkBoxChecked);
        }
    }
}
