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
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;

namespace MyNes.Core
{
    public class NesCartDatabase
    {
        static List<NesCartDatabaseGameInfo> _databaseRoms = new List<NesCartDatabaseGameInfo>();
        //Database file info
        public static string DBVersion = "";
        public static string DBConformance = "";
        public static string DBAgent = "";
        public static string DBAuthor = "";
        public static string DBTimeStamp = "";

        /// <summary>
        /// Load data base from file
        /// </summary>
        /// <param name="fileName">The full path of database file</param>
        public static void LoadDatabase(string fileName)
        {
            if (!File.Exists(fileName))
                return;
            //1 clear the database
            _databaseRoms.Clear();
            //2 read the xml file
            Stream databaseStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);

            XmlReaderSettings sett = new XmlReaderSettings();
            sett.DtdProcessing = DtdProcessing.Ignore;
            sett.IgnoreWhitespace = true;
            XmlReader XMLread = XmlReader.Create(databaseStream, sett);

            NesCartDatabaseGameInfo drom = new NesCartDatabaseGameInfo();
            // To avoid nulls ..
            drom.Cartridges = new List<NesCartDatabaseCartridgeInfo>();
            drom.Game_AltName = "";
            drom.Game_Catalog = "";
            drom.Game_Class = "";
            drom.Game_Developer = "";
            drom.Game_Name = "";
            drom.Game_Players = "";
            drom.Game_Publisher = "";
            drom.Game_Region = "";
            drom.Game_ReleaseDate = "";

            while (XMLread.Read())
            {
                //database Game info
                if (XMLread.Name == "xml" & XMLread.IsStartElement())
                {
                    if (XMLread.MoveToAttribute("version"))
                        DBVersion = XMLread.Value;
                    if (XMLread.MoveToAttribute("conformance"))
                        DBConformance = XMLread.Value;
                    if (XMLread.MoveToAttribute("agent"))
                        DBAgent = XMLread.Value;
                    if (XMLread.MoveToAttribute("author"))
                        DBAuthor = XMLread.Value;
                    if (XMLread.MoveToAttribute("timestamp"))
                        DBTimeStamp = XMLread.Value;
                }
                //Is it a game ?
                else if (XMLread.Name == "game" & XMLread.IsStartElement())
                {
                    drom = new NesCartDatabaseGameInfo();
                    
                    if (XMLread.MoveToAttribute("name"))
                        drom.Game_Name = XMLread.Value;
                    if (XMLread.MoveToAttribute("altname"))
                        drom.Game_AltName = XMLread.Value;
                    if (XMLread.MoveToAttribute("class"))
                        drom.Game_Class = XMLread.Value;
                    if (XMLread.MoveToAttribute("catalog"))
                        drom.Game_Catalog = XMLread.Value;
                    if (XMLread.MoveToAttribute("publisher"))
                        drom.Game_Publisher = XMLread.Value;
                    if (XMLread.MoveToAttribute("developer"))
                        drom.Game_Developer = XMLread.Value;
                    if (XMLread.MoveToAttribute("region"))
                        drom.Game_Region = XMLread.Value;
                    if (XMLread.MoveToAttribute("players"))
                        drom.Game_Players = XMLread.Value;
                    if (XMLread.MoveToAttribute("date"))
                        drom.Game_ReleaseDate = XMLread.Value;

                    NesCartDatabaseCartridgeInfo crt = new NesCartDatabaseCartridgeInfo();
                    crt.PAD_h = "";
                    crt.PAD_v = "";
                    crt.PRG_crc = "";
                    crt.PRG_name = "";
                    crt.PRG_sha1 = "";
                    crt.PRG_size = "";
                    crt.chip_type = new List<string>();
                    crt.CHR_crc = "";
                    crt.CHR_name = "";
                    crt.CHR_sha1 = "";
                    crt.CHR_size = "";
                    crt.CIC_type = "";
                    crt.Board_Mapper = "";
                    crt.Board_Pcb = "";
                    crt.Board_Type = "";
                    crt.VRAM_sizes = new List<string>();
                    crt.WRAMBanks = new List<SRAMBankInfo>();
                    // Load carts
                    while (XMLread.Read())
                    {
                        //End of game info ?
                        if (XMLread.Name == "game" & !XMLread.IsStartElement())
                        {
                            _databaseRoms.Add(drom);
                            break;
                        }
                        //cartridge info
                        if (XMLread.Name == "cartridge" & XMLread.IsStartElement())
                        {
                            if (drom.Cartridges == null)
                                drom.Cartridges = new List<NesCartDatabaseCartridgeInfo>();
                            crt = new NesCartDatabaseCartridgeInfo();
                            crt.PAD_h = "";
                            crt.PAD_v = "";
                            crt.PRG_crc = "";
                            crt.PRG_name = "";
                            crt.PRG_sha1 = "";
                            crt.PRG_size = "";
                            crt.chip_type = new List<string>();
                            crt.CHR_crc = "";
                            crt.CHR_name = "";
                            crt.CHR_sha1 = "";
                            crt.CHR_size = "";
                            crt.CIC_type = "";
                            crt.Board_Mapper = "";
                            crt.Board_Pcb = "";
                            crt.Board_Type = "";
                            crt.VRAM_sizes = new List<string>();
                            crt.WRAMBanks = new List<SRAMBankInfo>();
                            if (XMLread.MoveToAttribute("system"))
                                crt.System = XMLread.Value;
                            if (XMLread.MoveToAttribute("crc"))
                                crt.CRC = XMLread.Value;
                            if (XMLread.MoveToAttribute("sha1"))
                                crt.SHA1 = XMLread.Value;
                            if (XMLread.MoveToAttribute("dump"))
                                crt.Dump = XMLread.Value;
                            if (XMLread.MoveToAttribute("dumper"))
                                crt.Dumper = XMLread.Value;
                            if (XMLread.MoveToAttribute("datedumped"))
                                crt.DateDumped = XMLread.Value;
                        }
                        else if (XMLread.Name == "cartridge" & !XMLread.IsStartElement())
                        {
                            drom.Cartridges.Add(crt); continue;
                        }
                        //board info
                        else if (XMLread.Name == "board" & XMLread.IsStartElement())
                        {
                            if (XMLread.MoveToAttribute("type"))
                                crt.Board_Type  = XMLread.Value;
                            if (XMLread.MoveToAttribute("pcb"))
                                crt.Board_Pcb = XMLread.Value;
                            if (XMLread.MoveToAttribute("mapper"))
                                crt.Board_Mapper = XMLread.Value;
                        }
                        //prg info
                        else if (XMLread.Name == "prg" & XMLread.IsStartElement())
                        {
                            if (XMLread.MoveToAttribute("name"))
                                crt.PRG_name = XMLread.Value;
                            if (XMLread.MoveToAttribute("size"))
                                crt.PRG_size = XMLread.Value;
                            if (XMLread.MoveToAttribute("crc"))
                                crt.PRG_crc = XMLread.Value;
                            if (XMLread.MoveToAttribute("sha1"))
                                crt.PRG_sha1 = XMLread.Value;
                        }
                        //chr info
                        else if (XMLread.Name == "chr" & XMLread.IsStartElement())
                        {
                            if (XMLread.MoveToAttribute("name"))
                                crt.CHR_name = XMLread.Value;
                            if (XMLread.MoveToAttribute("size"))
                                crt.CHR_size = XMLread.Value;
                            if (XMLread.MoveToAttribute("crc"))
                                crt.CHR_crc = XMLread.Value;
                            if (XMLread.MoveToAttribute("sha1"))
                                crt.CHR_sha1 = XMLread.Value;
                        }
                        //vram info
                        else if (XMLread.Name == "vram" & XMLread.IsStartElement())
                        {
                            if (XMLread.MoveToAttribute("size"))
                                crt.VRAM_sizes.Add(XMLread.Value);
                        }
                        //wram info
                        else if (XMLread.Name == "wram" & XMLread.IsStartElement())
                        {
                            string wsize = "";
                            bool battery = false;
                            int id = 0;

                            if (XMLread.MoveToAttribute("size"))
                                wsize = XMLread.Value;
                            if (XMLread.MoveToAttribute("battery"))
                                battery = XMLread.Value == "1";
                            if (XMLread.MoveToAttribute("id"))
                                int.TryParse(XMLread.Value, out id);
                            crt.WRAMBanks.Add(new SRAMBankInfo(id, wsize, battery));
                        }
                        //chip info
                        else if (XMLread.Name == "chip" & XMLread.IsStartElement())
                        {
                            if (XMLread.MoveToAttribute("type"))
                            {
                                if (crt.chip_type == null)
                                    crt.chip_type = new List<string>();
                                crt.chip_type.Add(XMLread.Value);
                            }
                        }
                        //cic info
                        else if (XMLread.Name == "cic" & XMLread.IsStartElement())
                        {
                            if (XMLread.MoveToAttribute("type"))
                                crt.CIC_type = XMLread.Value;
                        }
                        //pad info
                        else if (XMLread.Name == "pad" & XMLread.IsStartElement())
                        {
                            if (XMLread.MoveToAttribute("h"))
                                crt.PAD_h = XMLread.Value;
                            if (XMLread.MoveToAttribute("v"))
                                crt.PAD_v = XMLread.Value;
                        }
                    }
                }
            }
            XMLread.Close();
            databaseStream.Close();
        }

        /// <summary>
        /// Get the data base roms collection (found in xml file)
        /// </summary>
        public static List<NesCartDatabaseGameInfo> DatabaseRoms
        {
            get { return _databaseRoms; }
        }

        /// <summary>
        /// Find a NesDatabaseRomInfo element
        /// </summary>
        /// <param name="Cart_sha1">The sha1 to match, file sha1 without header of INES (start index 16)</param>
        /// <param name="found">Set if the game found</param>
        /// <returns>The matched element if found, otherwise null</returns>
        public static NesCartDatabaseGameInfo Find(string Cart_sha1, out bool found)
        {
            found = false;
            foreach (NesCartDatabaseGameInfo item in _databaseRoms)
            {
                foreach (NesCartDatabaseCartridgeInfo crt in item.Cartridges)
                    if (crt.SHA1.ToLower() == Cart_sha1.ToLower())
                    {
                        found = true;
                        return item;
                    }
            }
            return new NesCartDatabaseGameInfo();//null
        }
    }
}
