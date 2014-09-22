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
namespace MMB
{
    /// <summary>
    /// Present const arrays for buttons.
    /// </summary>
    public sealed class ManagedMessageBoxButtons
    {
        // TODO: add more button possibilities
        /// <summary>
        /// Ok button
        /// </summary>
        public static string[] OK = { "&OK" };
        /// <summary>
        /// Ok + No buttons
        /// </summary>
        public static string[] OKNo = { "&OK", "&No" };
        /// <summary>
        /// Ok + No + Cancel buttons
        /// </summary>
        public static string[] OKNoCancel = { "&OK", "&No", "&Cancel" };
        /// <summary>
        /// Ok + Cancel buttons
        /// </summary>
        public static string[] OKCancel = { "&OK", "&Cancel" };
        /// <summary>
        /// Yes button
        /// </summary>
        public static string[] Yes = { "&Yes" };
        /// <summary>
        /// Yes + No buttons
        /// </summary>
        public static string[] YesNo = { "&Yes", "&No" };
        /// <summary>
        /// Yes + No + Cancel buttons
        /// </summary>
        public static string[] YesNoCancel = { "&Yes", "&No", "&Cancel" };
        /// <summary>
        /// Save + Don't save + Cancel buttons
        /// </summary>
        public static string[] SaveDontsaveCancel = { "&Save", "&Don't save", "&Cancel" };
        /// <summary>
        /// Save + Don't save buttons
        /// </summary>
        public static string[] SaveDontsave = { "&Save", "&Don't save" };
        /// <summary>
        /// Abort button
        /// </summary>
        public static string[] Abort = { "&Abort" };
        /// <summary>
        /// Abort + Retry + Ignore buttons
        /// </summary>
        public static string[] AbortRetryIgnore = { "&Abort", "&Retry", "&Ignore" };
        /// <summary>
        /// Retry + Cancel buttons
        /// </summary>
        public static string[] RetryCancel = { "&Retry", "&Cancel" };
    }
}
