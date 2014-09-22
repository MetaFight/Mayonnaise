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

namespace MyNes.Core
{
    /// <summary>
    /// Attributes descripes a board or mapper
    /// </summary>
    class BoardInfo : Attribute
    {
        /// <summary>
        /// Attributes descripes a board or mapper
        /// </summary>
        /// <param name="boardName">The board name.</param>
        /// <param name="inesMapperNumber">Ines mapper number.</param>
        public BoardInfo(string boardName, int inesMapperNumber)
        {
            this.BoardName = boardName;
            this.INESMapper = inesMapperNumber;
            this.DefaultPRG_RAM_8KB_BanksCount = 1;
            this.DefaultCHR_RAM_1KB_BanksCount = 8;
            this.Enabled_ppuA12ToggleTimer = this.PPUA12TogglesOnRaisingEdge = false;
        }
        /// <summary>
        /// Attributes descripes a board or mapper
        /// </summary>
        /// <param name="boardName">Board name.</param>
        /// <param name="inesMapperNumber">Ines mapper number.</param>
        /// <param name="defaultPRG_RAM_8KB_BanksCount">Default PRG RAM (SRAM) banks count. Default value is 1.</param>
        /// <param name="defaultCHR_RAM_1KB_BanksCount">Default CHR RAM banks count. Default value is 8.</param>
        public BoardInfo(string boardName, int inesMapperNumber, int defaultPRG_RAM_8KB_BanksCount, int defaultCHR_RAM_1KB_BanksCount)
        {
            this.BoardName = boardName;
            this.INESMapper = inesMapperNumber;
            this.DefaultPRG_RAM_8KB_BanksCount = defaultPRG_RAM_8KB_BanksCount;
            this.DefaultCHR_RAM_1KB_BanksCount = defaultCHR_RAM_1KB_BanksCount;
            this.Enabled_ppuA12ToggleTimer = this.PPUA12TogglesOnRaisingEdge = false;
        }
        /// <summary>
        /// Attributes descripes a board or mapper
        /// </summary>
        /// <param name="boardName">Board name.</param>
        /// <param name="inesMapperNumber">Ines mapper number.</param>
        /// <param name="Enabled_ppuA12ToggleTimer">Toggle the scanline timer (clocked on PPU A12 raising edge, used in MMC3)</param>
        /// <param name="PPUA12TogglesOnRaisingEdge">Indicate if the scanline timer clock on raising edge(true) of A12 or not(false)</param>
        public BoardInfo(string boardName, int inesMapperNumber, bool Enabled_ppuA12ToggleTimer, bool PPUA12TogglesOnRaisingEdge)
        {
            this.BoardName = boardName;
            this.INESMapper = inesMapperNumber;
            this.DefaultPRG_RAM_8KB_BanksCount = 1;
            this.DefaultCHR_RAM_1KB_BanksCount = 8;
            this.Enabled_ppuA12ToggleTimer = Enabled_ppuA12ToggleTimer;
            this.PPUA12TogglesOnRaisingEdge = PPUA12TogglesOnRaisingEdge;
        }
        /// <summary>
        /// Attributes descripes a board or mapper
        /// </summary>
        /// <param name="boardName">Board name.</param>
        /// <param name="inesMapperNumber">Ines mapper number.</param>
        /// <param name="defaultPRG_RAM_8KB_BanksCount">Default PRG RAM (SRAM) banks count. Default value is 1.</param>
        /// <param name="defaultCHR_RAM_1KB_BanksCount">Default CHR RAM banks count. Default value is 8.</param>
        /// <param name="Enabled_ppuA12ToggleTimer">Toggle the scanline timer (clocked on PPU A12 raising edge, used in MMC3)</param>
        /// <param name="PPUA12TogglesOnRaisingEdge">Indicate if the scanline timer clock on raising edge(true) of A12 or not(false)</param>
        public BoardInfo(string boardName, int inesMapperNumber, int defaultPRG_RAM_8KB_BanksCount, int defaultCHR_RAM_1KB_BanksCount, bool Enabled_ppuA12ToggleTimer, bool PPUA12TogglesOnRaisingEdge)
        {
            this.BoardName = boardName;
            this.INESMapper = inesMapperNumber;
            this.DefaultPRG_RAM_8KB_BanksCount = defaultPRG_RAM_8KB_BanksCount;
            this.DefaultCHR_RAM_1KB_BanksCount = defaultCHR_RAM_1KB_BanksCount;
            this.Enabled_ppuA12ToggleTimer = Enabled_ppuA12ToggleTimer;
            this.PPUA12TogglesOnRaisingEdge = PPUA12TogglesOnRaisingEdge;
        }
        /// <summary>
        /// Gets the name of the board.
        /// </summary>
        /// <value>The name of the board.</value>
        public string BoardName { get; private set; }
        /// <summary>
        /// Gets the INES mapper number.
        /// </summary>
        /// <value>The INES mapper.</value>
        public int INESMapper { get; private set; }
        /// <summary>
        /// Gets the default PRG RAM (SRAM) banks count.
        /// </summary>
        /// <value>The default PRG RAM (SRAM) banks count.</value>
        public int DefaultPRG_RAM_8KB_BanksCount { get; private set; }
        /// <summary>
        /// Gets the default CHR RAM banks count
        /// </summary>
        /// <value>The default CHR RAM banks count</value>
        public int DefaultCHR_RAM_1KB_BanksCount { get; private set; }
        public bool Enabled_ppuA12ToggleTimer { get; private set; }
        public bool PPUA12TogglesOnRaisingEdge { get; private set; }
    }
}

