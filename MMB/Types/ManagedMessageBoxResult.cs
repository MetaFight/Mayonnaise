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
using System.Threading.Tasks;

namespace MMB
{
    /// <summary>
    /// The Managed Message Box Result
    /// </summary>
    public struct ManagedMessageBoxResult
    {
        /// <summary>
        /// The Managed Message Box Result
        /// </summary>
        /// <param name="clickedButton">The clicked button</param>
        /// <param name="clickedButtonIndex">The clicked button index</param>
        /// <param name="isChecked">Indicate if the check box is checked.</param>
        public ManagedMessageBoxResult(string clickedButton, int clickedButtonIndex, bool isChecked)
        {
            this.clickedButton = clickedButton;
            this.isChecked = isChecked;
            this.clickedButtonIndex = clickedButtonIndex;
        }

        private bool isChecked;
        private string clickedButton;
        private int clickedButtonIndex;

        /// <summary>
        /// Get if the check box checked.
        /// </summary>
        public bool Checked
        { get { return isChecked; } }
        /// <summary>
        /// Get the clicked button text.
        /// </summary>
        public string ClickedButton
        { get { return clickedButton; } }
        /// <summary>
        /// Get the clicked button zero based index within given buttons array.
        /// </summary>
        public int ClickedButtonIndex
        { get { return clickedButtonIndex; } }
    }
}
