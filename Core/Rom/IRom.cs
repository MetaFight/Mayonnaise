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
    public abstract class IRom
    {
        /// <summary>
        /// Load the rom.
        /// </summary>
        /// <param name="fileName">File name.</param>
        /// <param name="loadDumps">If set to <c>true</c> load dumps.</param>
        public virtual void Load(string fileName, bool loadDumps)
        {

        }

        #region properties

        /// <summary>
        /// Gets a value indicating whether this rom is valid.
        /// </summary>
        /// <value><c>true</c> if this instance is valid; otherwise, <c>false</c>.</value>
        public bool IsValid{ get; protected set; }

        /// <summary>
        /// Gets the PRG count.
        /// </summary>
        /// <value>The PRG count.</value>
        public int PRGCount{ get; protected set; }

        /// <summary>
        /// Gets the CHR count.
        /// </summary>
        /// <value>The CHR count.</value>
        public int CHRCount{ get; protected set; }

        /// <summary>
        /// Gets the mapper number.
        /// </summary>
        /// <value>The mapper number.</value>
        public int MapperNumber{ get; protected set; }

        /// <summary>
        /// Gets the mirroring.
        /// </summary>
        /// <value>The mirroring.</value>
        public Mirroring Mirroring{ get; protected set; }

        /// <summary>
        /// Gets a value indicating whether this instance has trainer.
        /// </summary>
        /// <value><c>true</c> if this instance has trainer; otherwise, <c>false</c>.</value>
        public bool HasTrainer{ get; protected set; }

        /// <summary>
        /// Gets the PRG dump
        /// </summary>
        /// <value>The PRG dump</value>
        public byte [] PRG{ get; protected set; }

        /// <summary>
        /// Gets the CHR dump
        /// </summary>
        /// <value>The CHR dump</value>
        public byte [] CHR{ get; protected set; }

        /// <summary>
        /// Gets the trainer dump.
        /// </summary>
        /// <value>The trainer dump.</value>
        public byte [] Trainer{ get; protected set; }

        /// <summary>
        /// Gets the SHA1.
        /// </summary>
        /// <value>The SHA1.</value>
        public string SHA1{ get; protected set; }

        #endregion
    }
}

