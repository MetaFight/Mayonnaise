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
using System.IO;
using System.Security.Cryptography;

namespace MyNes.Core
{
    public class INes : IRom
    {
        public override void Load(string fileName, bool loadDumps)
        {
            FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read);

            if (stream.Length < 16)
            {
                stream.Close();
                base.IsValid = false;
                return;
            }
            // Read header
            byte[] header = new byte[16];
            stream.Read(header, 0, 16);
            // Read SHA1 buffer
            byte[] buffer = new byte[stream.Length - 16];
            stream.Read(buffer, 0, (int)(stream.Length - 16));

            //SHA1
            base.SHA1 = "";
            SHA1Managed managedSHA1 = new SHA1Managed();
            byte[] shaBuffer = managedSHA1.ComputeHash(buffer);
            foreach (byte b in shaBuffer)
                base.SHA1 += b.ToString("x2").ToLower();
            // Header
            if (header[0] != 'N' ||
                header[1] != 'E' ||
                header[2] != 'S' ||
                header[3] != 0x1A)
            {
                stream.Close();
                base.IsValid = false;
                return;
            }
            base.PRGCount = header[4];
            base.CHRCount = header[5];

            switch (header[6] & 0x9)
            {
                case 0x0:
                    base.Mirroring = Mirroring.Horz;
                    break;
                case 0x1:
                    base.Mirroring = Mirroring.Vert;
                    break;
                case 0x8:
                case 0x9:
                    base.Mirroring = Mirroring.Full;
                    break;
            }

            this.HasBattery = (header[6] & 0x2) != 0x0;
            base.HasTrainer = (header[6] & 0x4) != 0x0;

            if ((header[7] & 0x0F) == 0)
                base.MapperNumber = (byte)((header[7] & 0xF0) | (header[6] >> 4));
            else
                base.MapperNumber = (byte)((header[6] >> 4));

            this.IsVSUnisystem = (header[7] & 0x01) != 0;
            this.IsPlaychoice10 = (header[7] & 0x02) != 0;

            // Read dumps
            if (loadDumps)
            {
                stream.Seek(16L, SeekOrigin.Begin);

                if (this.HasTrainer)
                {
                    this.Trainer = new byte[512];
                    stream.Read(this.Trainer, 0, 512);
                }
                else
                    base.Trainer = new byte[0];

                this.PRG = new byte[this.PRGCount * 0x4000];
                stream.Read(this.PRG, 0, this.PRGCount * 0x4000);

                if (CHRCount > 0)
                {
                    base.CHR = new byte[this.CHRCount * 0x2000];
                    stream.Read(this.CHR, 0, this.CHRCount * 0x2000);
                }
                else
                    base.CHR = new byte[0];
            }
            base.IsValid = true;
            stream.Close();
        }

        public bool HasBattery { get; private set; }

        public bool IsPlaychoice10 { get; private set; }

        public bool IsVSUnisystem { get; private set; }

    }
}

