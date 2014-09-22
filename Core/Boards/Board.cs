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
using System.IO;
namespace MyNes.Core
{
    abstract class Board
    {
        public Board()
        {
            this.enable_external_sound = false;
            LoadAttributes();
        }

        /// <summary>
        /// Initialize the board
        /// </summary>
        /// <param name="sha1">The rom sha1 calculated without the header</param>
        /// <param name="prg_dump">The prg data dump.</param>
        /// <param name="chr_dump">The chr data dump</param>
        /// <param name="trainer_dump">The trainer data dump.</param>
        /// <param name="defaultMirroring">The default mirroring as defined in rom.</param>
        public virtual void Initialize(string sha1, byte[] prg_dump, byte[] chr_dump, byte[] trainer_dump, Mirroring defaultMirroring)
        {
            RomSHA1 = sha1;
            // Find on DB
            GameInfo = NesCartDatabase.Find(sha1, out IsGameFoundOnDB);
            //set cart info
            if (GameInfo.Cartridges != null)
            {
                foreach (NesCartDatabaseCartridgeInfo cartinf in GameInfo.Cartridges)
                    if (cartinf.SHA1.ToLower() == sha1.ToLower())
                    {
                        GameCartInfo = cartinf;
                        break;
                    }
            }
            BoardType = "N/A";
            BoardPCB = "N/A";
            this.Chips = new List<string>();
            if (IsGameFoundOnDB)
            {
                Console.WriteLine("Game found in Database !!");
                Console.WriteLine("> Game name: " + GameInfo.Game_Name);
                Console.WriteLine("> Game alt name: " + GameInfo.Game_AltName);
                BoardType = GameCartInfo.Board_Type;
                Console.WriteLine("> Board Type: " + BoardType);
                BoardPCB = GameCartInfo.Board_Pcb;
                Console.WriteLine("> Board Pcb: " + BoardPCB);
                // Chips ... important for some boards
                if (GameCartInfo.chip_type != null)
                    for (int i = 0; i < GameCartInfo.chip_type.Count; i++)
                    {
                        Console.WriteLine(string.Format("> CHIP {0}: {1}", (i + 1).ToString(),
                           GameCartInfo.chip_type[i]));
                        this.Chips.Add(GameCartInfo.chip_type[i]);
                    }
            }
            // Mirroring
            this.default_mirroring = defaultMirroring;
            nmt_banks = new byte[4][]
            {
                new byte[0x0400], new byte[0x0400], new byte[0x0400], new byte[0x0400]
                /*Only 2 nmt banks should be used in normal state*/
            };
            nmt_indexes = new int[4];
            SwitchNMT(defaultMirroring);
            // PRG data ***********************************************
            List<BankInfo> prgBanks = new List<BankInfo>(GetPRGRAM());
            prg_rom_bank_offset = prgBanks.Count;
            prgBanks.AddRange(GetPRGROM(prg_dump));
            SRAMSaveRequired = false;
            prg_banks = new byte[prgBanks.Count][];
            prg_enable = new bool[prgBanks.Count];
            prg_isram = new bool[prgBanks.Count];
            prg_writable = new bool[prgBanks.Count];
            prg_battery = new bool[prgBanks.Count];
            for (int i = 0; i < prgBanks.Count; i++)
            {
                prg_banks[i] = prgBanks[i].DATA;
                prg_enable[i] = prgBanks[i].Enabled;
                prg_writable[i] = prgBanks[i].Writable;
                prg_isram[i] = prgBanks[i].IsRAM;
                prg_battery[i] = prgBanks[i].IsBattery;
                if (!SRAMSaveRequired)
                {
                    if (prg_battery[i])
                        SRAMSaveRequired = true;
                }
            }
            prg_indexes = new int[5];
            // ********************************************************
            // CHR data ***********************************************
            List<BankInfo> chrBanks = new List<BankInfo>(GetCHRRAM());
            chr_rom_bank_offset = chrBanks.Count;
            chrBanks.AddRange(GetCHRROM(chr_dump));
            chr_banks = new byte[chrBanks.Count][];
            chr_enable = new bool[chrBanks.Count];
            chr_isram = new bool[chrBanks.Count];
            chr_writable = new bool[chrBanks.Count];
            for (int i = 0; i < chrBanks.Count; i++)
            {
                chr_banks[i] = chrBanks[i].DATA;
                chr_enable[i] = chrBanks[i].Enabled;
                chr_writable[i] = chrBanks[i].Writable;
                chr_isram[i] = chrBanks[i].IsRAM;
            }
            chr_indexes = new int[8];
            // ********************************************************
        }
        public virtual void HardReset()
        {
            // PRG switch
            Switch08KPRG(0, 0x6000, false);
            Switch32KPRG(0, true);
            TogglePRGRAMEnable(true);
            // CHR switch
            Switch08KCHR(0, chr_01K_rom_count > 0);
            // Mirroring
            nmt_banks = new byte[4][]
            {
                new byte[0x0400], new byte[0x0400], new byte[0x0400], new byte[0x0400]
                /*Only 2 nmt banks should be used in normal state*/
            };
            nmt_indexes = new int[4];
            SwitchNMT(this.default_mirroring);
        }
        public virtual void SoftReset()
        {

        }
        protected void LoadAttributes()
        {
            this.Supported = true;
            this.NotImplementedWell = false;
            foreach (Attribute attr in Attribute.GetCustomAttributes(this.GetType()))
            {
                if (attr.GetType() == typeof(BoardInfo))
                {
                    BoardInfo inf = (BoardInfo)attr;
                    this.Name = inf.BoardName;
                    this.MapperNumber = inf.INESMapper;
                    this.prg_ram_default_08KB_count = inf.DefaultPRG_RAM_8KB_BanksCount;
                    this.chr_ram_1KB_default_banks_count = inf.DefaultCHR_RAM_1KB_BanksCount;
                    this.enabled_ppuA12ToggleTimer = inf.Enabled_ppuA12ToggleTimer;
                    this.ppuA12TogglesOnRaisingEdge = inf.PPUA12TogglesOnRaisingEdge;
                }
                else if (attr.GetType() == typeof(WithExternalSound))
                {
                    this.enable_external_sound = true;
                    Console.WriteLine("EXTERNAL SOUND CHANNELS INTEGRATION ENABLED");
                }
                else if (attr.GetType() == typeof(NotSupported))
                {
                    this.Supported = false;
                    Console.WriteLine("BOARD FLAGED 'Not Supported'");
                }
                else if (attr.GetType() == typeof(NotImplementedWell))
                {
                    NotImplementedWell inf = (NotImplementedWell)attr;
                    this.Issues = inf.Issues;
                    this.NotImplementedWell = true;
                    Console.WriteLine("BOARD FLAGED 'Not Implemented Well'");
                }
            }
        }

        #region Rom in database
        public bool IsGameFoundOnDB;
        public NesCartDatabaseGameInfo GameInfo;
        public NesCartDatabaseCartridgeInfo GameCartInfo;
        public string BoardType { get; private set; }
        public string BoardPCB { get; private set; }
        public List<string> Chips { get; private set; }
        public string RomSHA1 { get; private set; }
        #endregion

        #region PRG
        /// <summary>
        /// Indicates if the engine should save sram.
        /// </summary>
        public bool SRAMSaveRequired;
        protected byte[][] prg_banks;
        protected int[] prg_indexes;
        protected bool[] prg_enable;
        protected bool[] prg_isram;
        protected bool[] prg_writable;
        protected bool[] prg_battery;
        protected int prg_rom_bank_offset;
        protected int prg_rom_size_KB;
        protected int prg_08K_rom_count;
        protected int prg_16K_rom_count;
        protected int prg_32K_rom_count;
        protected int prg_08K_rom_mask;
        protected int prg_16K_rom_mask;
        protected int prg_32K_rom_mask;
        // Default ram size
        protected int prg_ram_default_08KB_count;
        protected int prg_ram_size_KB;
        protected int prg_08K_ram_count;
        protected int prg_16K_ram_count;
        protected int prg_32K_ram_count;
        protected int prg_08K_ram_mask;
        protected int prg_16K_ram_mask;
        protected int prg_32K_ram_mask;
        protected int prg_bank_index_temp;

        protected BankInfo[] GetPRGRAM()
        {
            List<BankInfo> ramBanks = new List<BankInfo>();
            Console.WriteLine("Retrieving PRG RAM information from database ....");
            if (this.IsGameFoundOnDB)
            {
                // Get the sram size
                if (GameCartInfo.WRAMBanks.Count > 0)
                {
                    int id = 0;
                    foreach (SRAMBankInfo s in GameCartInfo.WRAMBanks)
                    {
                        int wsize = 0;
                        if (int.TryParse(s.SIZE.Replace("k", ""), out wsize))
                        {
                            Console.WriteLine("> Adding " + s.SIZE + " PRG RAM.");
                            if (wsize >= 8)
                            {
                                for (int i = 0; i < wsize / 8; i++)
                                {
                                    ramBanks.Add(new BankInfo(id.ToString(), true, true, true, s.BATTERY, new byte[0x2000]));
                                    id++;
                                }
                            }
                            else
                            {
                                Console.WriteLine("> The value is under 8K, assume 8k ...");
                                // The value is under 8K, assume 8k ...
                                ramBanks.Add(new BankInfo(id.ToString(), true, true, true, s.BATTERY, new byte[0x2000]));
                            }
                        }
                        else
                        {
                            // Unacceptable value, assume 8k
                            ramBanks.Add(new BankInfo(id.ToString(), true, true, true, true, new byte[0x2000]));
                            id++;
                        }
                    }
                }
                else// No info for SRAM; This mean this rom has no sram !
                {
                    Console.WriteLine("This game has no PRG RAM !");
                }
            }
            else// Not in database :(
            {
                Console.WriteLine("Could't find this game in database .... Adding 8K x " + prg_ram_default_08KB_count + " PRG RAM BANKS to avoid exceptions.");
                // Assume 8k
                for (int i = 0; i < prg_ram_default_08KB_count; i++)
                    ramBanks.Add(new BankInfo(i.ToString(), true, true, true, true, new byte[0x2000]));
            }
            // Reverse sort by id !
            ramBanks.Sort(new BankInfoSorter());
            // Calculate values
            prg_ram_size_KB = ramBanks.Count * 8;
            prg_08K_ram_count = ramBanks.Count;
            prg_16K_ram_count = ramBanks.Count / 2;
            prg_32K_ram_count = ramBanks.Count / 4;
            prg_08K_ram_mask = prg_08K_ram_count - 1;
            prg_16K_ram_mask = prg_16K_ram_count - 1;
            prg_32K_ram_mask = prg_32K_ram_count - 1;
            Console.WriteLine("PRG RAM SIZE = " + prg_ram_size_KB + " KB");
            return ramBanks.ToArray();
        }
        protected BankInfo[] GetPRGROM(byte[] prg_dump)
        {
            List<BankInfo> banks = new List<BankInfo>();
            prg_rom_size_KB = prg_dump.Length / 1024;
            prg_08K_rom_count = prg_rom_size_KB / 08;
            prg_16K_rom_count = prg_rom_size_KB / 16;
            prg_32K_rom_count = prg_rom_size_KB / 32;
            prg_08K_rom_mask = prg_08K_rom_count - 1;
            prg_16K_rom_mask = prg_16K_rom_count - 1;
            prg_32K_rom_mask = prg_32K_rom_count - 1;
            for (int i = 0; i < prg_08K_rom_count; i++)
            {
                byte[] data = new byte[0x2000];
                for (int j = 0; j < 0x2000; j++)
                {
                    data[j] = prg_dump[(i * 0x2000) + j];
                }
                banks.Add(new BankInfo(i.ToString(), false, false, true, false, data));
            }
            return banks.ToArray();
        }

        #endregion

        #region CHR

        protected byte[][] chr_banks;
        protected int[] chr_indexes;
        protected bool[] chr_enable;
        protected bool[] chr_isram;
        protected bool[] chr_writable;
        protected int chr_rom_bank_offset;
        protected int chr_01K_rom_count;
        protected int chr_02K_rom_count;
        protected int chr_04K_rom_count;
        protected int chr_08K_rom_count;
        protected int chr_01K_rom_mask;
        protected int chr_02K_rom_mask;
        protected int chr_04K_rom_mask;
        protected int chr_08K_rom_mask;
        protected int chr_ram_1KB_default_banks_count;
        protected int chr_01K_ram_count;
        protected int chr_02K_ram_count;
        protected int chr_04K_ram_count;
        protected int chr_08K_ram_count;
        protected int chr_01K_ram_mask;
        protected int chr_02K_ram_mask;
        protected int chr_04K_ram_mask;
        protected int chr_08K_ram_mask;
        protected int chr_bank_index_temp;

        protected BankInfo[] GetCHRRAM()
        {
            List<BankInfo> ramBanks = new List<BankInfo>();
            if (this.IsGameFoundOnDB)
            {
                bool ramAdded = false;
                if (GameCartInfo.VRAM_sizes != null)
                {
                    Console.WriteLine("Using database to initialize CHR RAM .....");
                    foreach (string vramSize in GameCartInfo.VRAM_sizes)
                    {
                        // Get the vram size
                        int vsize = 0;
                        if (int.TryParse(vramSize.Replace("k", ""), out vsize))
                        {
                            Console.WriteLine(">CHR RAM CHIP SIZE " + vramSize + " KB added");
                            for (int i = 0; i < vsize; i++)
                            {
                                ramBanks.Add(new BankInfo(i.ToString(), true, true, true, false, new byte[0x400]));
                                ramAdded = true;
                            }
                        }
                    }
                }
                if (!ramAdded)
                {
                    ramBanks = new List<BankInfo>();
                    Console.WriteLine("Game not found in database to initialize CHR RAM; CHR RAM size set to " + chr_ram_1KB_default_banks_count + " KB");
                    for (int i = 0; i < chr_ram_1KB_default_banks_count; i++)
                    {
                        ramBanks.Add(new BankInfo(i.ToString(), true, true, true, false, new byte[0x400]));
                    }
                }
            }
            else// Not in database :(
            {
                Console.WriteLine("Game not found in database to initialize CHR RAM; CHR RAM size set to " + chr_ram_1KB_default_banks_count + " KB");
                for (int i = 0; i < chr_ram_1KB_default_banks_count; i++)
                {
                    ramBanks.Add(new BankInfo(i.ToString(), true, true, true, false, new byte[0x400]));
                }
            }
            // Calculate
            chr_01K_ram_count = ramBanks.Count;
            chr_02K_ram_count = ramBanks.Count / 2;
            chr_04K_ram_count = ramBanks.Count / 4;
            chr_08K_ram_count = ramBanks.Count / 8;
            chr_01K_ram_mask = chr_01K_ram_count - 1;
            chr_02K_ram_mask = chr_02K_ram_count - 1;
            chr_04K_ram_mask = chr_04K_ram_count - 1;
            chr_08K_ram_mask = chr_08K_ram_count - 1;
            System.Console.WriteLine("CHR RAM Size = " + chr_01K_ram_count + " KB");
            return ramBanks.ToArray();
        }

        protected BankInfo[] GetCHRROM(byte[] chr_dump)
        {
            List<BankInfo> banks = new List<BankInfo>();
            chr_01K_rom_count = chr_dump.Length / 1024;
            chr_02K_rom_count = chr_dump.Length / (1024 * 2);
            chr_04K_rom_count = chr_dump.Length / (1024 * 4);
            chr_08K_rom_count = chr_dump.Length / (1024 * 8);
            chr_01K_rom_mask = chr_01K_rom_count - 1;
            chr_02K_rom_mask = chr_02K_rom_count - 1;
            chr_04K_rom_mask = chr_04K_rom_count - 1;
            chr_08K_rom_mask = chr_08K_rom_count - 1;
            System.Console.WriteLine("CHR ROM Size = " + chr_01K_rom_count + " KB");

            for (int i = 0; i < chr_01K_rom_count; i++)
            {
                byte[] data = new byte[0x400];
                for (int j = 0; j < 0x400; j++)
                {
                    data[j] = chr_dump[(i * 0x400) + j];
                }
                banks.Add(new BankInfo(i.ToString(), false, false, true, false, data));
            }
            return banks.ToArray();
        }

        #endregion

        #region NMT
        protected byte[][] nmt_banks;
        protected int[] nmt_indexes;
        protected Mirroring default_mirroring;
        #endregion

        #region cpu access methods
        public virtual void WriteEXP(ref int address, ref byte data)
        {
        }
        public virtual void WriteSRM(ref int address, ref byte data)
        {
            if (prg_enable[prg_indexes[0]])
                if (prg_isram[prg_indexes[0]])
                    if (prg_writable[prg_indexes[0]])
                        prg_banks[prg_indexes[0]][address & 0x1FFF] = data;
        }
        public virtual void WritePRG(ref int address, ref byte data)
        {
            prg_bank_index_temp = prg_indexes[((address >> 13) & 3) + 1];
            if (prg_enable[prg_bank_index_temp])
                if (prg_isram[prg_bank_index_temp])
                    if (prg_writable[prg_bank_index_temp])
                        prg_banks[prg_bank_index_temp][address & 0x1FFF] = data;
        }
        public virtual byte ReadEXP(ref int address)
        {
            return 0;
        }
        public virtual byte ReadSRM(ref int address)
        {
            if (prg_enable[prg_indexes[0]])
                return prg_banks[prg_indexes[0]][address & 0x1FFF];
            return 0;
        }
        public virtual byte ReadPRG(ref int address)
        {
            if (IsGameGenieActive)
            {
                foreach (GameGenieCode code in GameGenieCodes)
                {
                    if (code.Enabled)
                    {
                        if (code.Address == address)
                        {
                            if (code.IsCompare)
                            {
                                if (code.Compare == prg_banks[prg_indexes[((address >> 13) & 3) + 1]][address & 0x1FFF])
                                    return code.Value;
                            }
                            else
                            {
                                return code.Value;
                            }

                            break;
                        }
                    }
                }
            }
            return prg_banks[prg_indexes[((address >> 13) & 3) + 1]][address & 0x1FFF];
        }
        protected void TogglePRGRAMEnable(bool enable)
        {
            for (int i = 0; i < prg_banks.Length; i++)
            {
                if (prg_isram[i])
                    prg_enable[i] = enable;
            }
        }
        protected void TogglePRGRAMWritableEnable(bool enable)
        {
            for (int i = 0; i < prg_banks.Length; i++)
            {
                if (prg_isram[i])
                    prg_writable[i] = enable;
            }
        }
        protected void ToggleCHRRAMEnable(bool enable)
        {
            for (int i = 0; i < chr_banks.Length; i++)
            {
                if (chr_isram[i])
                    chr_enable[i] = enable;
            }
        }
        protected void ToggleCHRRAMWritableEnable(bool enable)
        {
            for (int i = 0; i < chr_banks.Length; i++)
            {
                if (chr_isram[i])
                    chr_writable[i] = enable;
            }
        }
        #endregion

        #region ppu access methods
        public virtual void WriteCHR(ref int address, ref byte data)
        {
            chr_bank_index_temp = chr_indexes[(address >> 10) & 7];
            if (chr_enable[chr_bank_index_temp])
                if (chr_isram[chr_bank_index_temp])
                    if (chr_writable[chr_bank_index_temp])
                        chr_banks[chr_bank_index_temp][address & 0x3FF] = data;
        }
        public virtual void WriteNMT(ref int address, ref byte data)
        {
            nmt_banks[nmt_indexes[(address >> 10) & 3]][address & 0x3FF] = data;
        }
        public virtual byte ReadCHR(ref int address, bool spriteFetch)
        {
            return chr_banks[chr_indexes[(address >> 10) & 7]][address & 0x3FF];
        }
        public virtual byte ReadNMT(ref int address)
        {
            return nmt_banks[nmt_indexes[(address >> 10) & 3]][address & 0x3FF];
        }
        #endregion

        #region cpu memory switches
        protected void Switch08KPRG(int index, int where, bool isROM)
        {
            if (!isROM && prg_08K_ram_count == 0)
                return;
            if (where == 0x6000)
            {
                if (isROM)
                    prg_indexes[0] = (index & prg_08K_rom_mask) + prg_rom_bank_offset;
                else
                { prg_indexes[0] = index & prg_08K_ram_mask; }

            }
            else
            {
                if (isROM)
                    prg_indexes[((where >> 13) & 0x3) + 1] = (index & prg_08K_rom_mask) + prg_rom_bank_offset;
                else
                    prg_indexes[((where >> 13) & 0x3) + 1] = index & prg_08K_ram_mask;
            }
        }
        protected void Switch16KPRG(int index, int where, bool isROM)
        {
            if (!isROM && prg_08K_ram_count == 0) return;// Nothing to do if there's no ram
            where >>= 13;
            where &= 3;
            where++;

            index <<= 1;
            if (isROM)
            {
                prg_indexes[where] = (index & prg_08K_rom_mask) + prg_rom_bank_offset; index++; where++;
                prg_indexes[where] = (index & prg_08K_rom_mask) + prg_rom_bank_offset;
            }
            else
            {

                prg_indexes[where] = index & prg_08K_ram_mask; index++; where++;
                prg_indexes[where] = index & prg_08K_ram_mask;
            }
        }
        protected void Switch32KPRG(int index, bool isROM)
        {
            if (!isROM && prg_08K_ram_count == 0)
                return;
            index <<= 2;
            if (isROM)
            {
                prg_indexes[1] = (index & prg_08K_rom_mask) + prg_rom_bank_offset;
                index++;
                prg_indexes[2] = (index & prg_08K_rom_mask) + prg_rom_bank_offset;
                index++;
                prg_indexes[3] = (index & prg_08K_rom_mask) + prg_rom_bank_offset;
                index++;
                prg_indexes[4] = (index & prg_08K_rom_mask) + prg_rom_bank_offset;
            }
            else
            {
                prg_indexes[1] = index & prg_08K_ram_mask;
                index++;
                prg_indexes[2] = index & prg_08K_ram_mask;
                index++;
                prg_indexes[3] = index & prg_08K_ram_mask;
                index++;
                prg_indexes[4] = index & prg_08K_ram_mask;
            }
        }
        #endregion

        #region ppu memory switches
        protected void Switch01KCHR(int index, int where, bool isROM)
        {
            if (!isROM && chr_01K_ram_count == 0)
                return;
            if (isROM)
                chr_indexes[(where >> 10) & 0x07] = (index & chr_01K_rom_mask) + chr_rom_bank_offset;
            else
                chr_indexes[(where >> 10) & 0x07] = index & chr_01K_ram_mask;
        }
        protected void Switch02KCHR(int index, int where, bool isROM)
        {
            if (!isROM && chr_01K_ram_count == 0)
                return;
            where >>= 10;
            where &= 0x7;
            index <<= 1;
            if (isROM)
            {
                chr_indexes[where] = (index & chr_01K_rom_mask) + chr_rom_bank_offset;
                index++; where++;
                chr_indexes[where] = (index & chr_01K_rom_mask) + chr_rom_bank_offset;
            }
            else
            {
                chr_indexes[where] = index & chr_01K_ram_mask;
                index++; where++;
                chr_indexes[where] = index & chr_01K_ram_mask;
            }
        }
        protected void Switch04KCHR(int index, int where, bool isROM)
        {
            if (!isROM && chr_01K_ram_count == 0)
                return;
            where >>= 10;
            where &= 0x7;
            index <<= 2;
            if (isROM)
            {
                chr_indexes[where] = (index & chr_01K_rom_mask) + chr_rom_bank_offset;
                index++; where++;
                chr_indexes[where] = (index & chr_01K_rom_mask) + chr_rom_bank_offset;
                index++; where++;
                chr_indexes[where] = (index & chr_01K_rom_mask) + chr_rom_bank_offset;
                index++; where++;
                chr_indexes[where] = (index & chr_01K_rom_mask) + chr_rom_bank_offset;
            }
            else
            {
                chr_indexes[where] = index & chr_01K_ram_mask;
                index++; where++;
                chr_indexes[where] = index & chr_01K_ram_mask;
                index++; where++;
                chr_indexes[where] = index & chr_01K_ram_mask;
                index++; where++;
                chr_indexes[where] = index & chr_01K_ram_mask;
            }
        }
        protected void Switch08KCHR(int index, bool isROM)
        {
            if (!isROM && chr_01K_ram_count == 0)
                return;
            index <<= 3;

            if (isROM)
            {
                chr_indexes[0] = (index & chr_01K_rom_mask) + chr_rom_bank_offset;
                index++;
                chr_indexes[1] = (index & chr_01K_rom_mask) + chr_rom_bank_offset;
                index++;
                chr_indexes[2] = (index & chr_01K_rom_mask) + chr_rom_bank_offset;
                index++;
                chr_indexes[3] = (index & chr_01K_rom_mask) + chr_rom_bank_offset;
                index++;
                chr_indexes[4] = (index & chr_01K_rom_mask) + chr_rom_bank_offset;
                index++;
                chr_indexes[5] = (index & chr_01K_rom_mask) + chr_rom_bank_offset;
                index++;
                chr_indexes[6] = (index & chr_01K_rom_mask) + chr_rom_bank_offset;
                index++;
                chr_indexes[7] = (index & chr_01K_rom_mask) + chr_rom_bank_offset;
            }
            else
            {
                chr_indexes[0] = index & chr_01K_ram_mask;
                index++;
                chr_indexes[1] = index & chr_01K_ram_mask;
                index++;
                chr_indexes[2] = index & chr_01K_ram_mask;
                index++;
                chr_indexes[3] = index & chr_01K_ram_mask;
                index++;
                chr_indexes[4] = index & chr_01K_ram_mask;
                index++;
                chr_indexes[5] = index & chr_01K_ram_mask;
                index++;
                chr_indexes[6] = index & chr_01K_ram_mask;
                index++;
                chr_indexes[7] = index & chr_01K_ram_mask;
            }
        }
        protected void SwitchNMT(byte value)
        {
            nmt_indexes[0] = (byte)(value >> 0 & 0x03);
            nmt_indexes[1] = (byte)(value >> 2 & 0x03);
            nmt_indexes[2] = (byte)(value >> 4 & 0x03);
            nmt_indexes[3] = (byte)(value >> 6 & 0x03);
        }
        protected void SwitchNMT(Mirroring mirroring)
        {
            SwitchNMT((byte)mirroring);
        }
        #endregion

        #region Board info properties
        public string Name { get; protected set; }
        public int MapperNumber { get; protected set; }
        public bool Supported { get; protected set; }
        public bool NotImplementedWell { get; protected set; }
        public string Issues { get; protected set; }
        #endregion

        #region Clocks
        protected bool enabled_ppuA12ToggleTimer;
        protected bool ppuA12TogglesOnRaisingEdge;
        protected int old_vram_address;
        protected int new_vram_address;
        protected int ppu_cycles_timer;
        public bool enable_external_sound;
        /// <summary>
        /// Call this on VRAM address update
        /// </summary>
        /// <param name="newAddress"></param>
        public virtual void OnPPUAddressUpdate(ref int address)
        {
            if (enabled_ppuA12ToggleTimer)
            {
                old_vram_address = new_vram_address;
                new_vram_address = address & 0x1000;
                if (ppuA12TogglesOnRaisingEdge)
                {
                    if (old_vram_address < new_vram_address)
                    {
                        if (ppu_cycles_timer > 8)
                        {
                            OnPPUA12RaisingEdge();
                        }
                        ppu_cycles_timer = 0;
                    }
                }
                else
                {
                    if (old_vram_address > new_vram_address)
                    {
                        if (ppu_cycles_timer > 8)
                        {
                            OnPPUA12RaisingEdge();
                        }
                        ppu_cycles_timer = 0;
                    }
                }
            }
        }
        /// <summary>
        /// Clocked on CPU cycle clock
        /// </summary>
        public virtual void OnCPUClock()
        {
        }
        /// <summary>
        /// Clocked on ppu clock
        /// </summary>
        public virtual void OnPPUClock()
        {
            if (enabled_ppuA12ToggleTimer)
                ppu_cycles_timer++;
        }
        /// <summary>
        /// Clocked when the PPU A12 rasing edge occur (scanline timer, used in MMC3)
        /// </summary>
        public virtual void OnPPUA12RaisingEdge()
        {
        }
        /// <summary>
        /// Clocked each time ppu makes a new scanline
        /// </summary>
        public virtual void OnPPUScanlineTick()
        {
        }
        /// <summary>
        /// Clocks on APU duration clock
        /// </summary>
        public virtual void OnAPUClockDuration()
        {

        }
        /// <summary>
        /// Clocks on APU envelope clock
        /// </summary>
        public virtual void OnAPUClockEnvelope()
        {

        }
        /// <summary>
        /// Clocks on cpu cycle with additional info from apu
        /// </summary>
        /// <param name="isClockingLength">Indicates if the current clock is for length.</param>
        public virtual void OnAPUClockSingle(ref bool isClockingLength)
        {

        }
        /// <summary>
        /// Get the sound channels output after mix. This value will be added (+) into the nes original output mix.
        /// </summary>
        /// <returns></returns>
        public virtual double APUGetSamples()
        {
            return 0;
        }
        #endregion

        #region Game Genie
        public bool IsGameGenieActive;
        public GameGenieCode[] GameGenieCodes;
        public void SetupGameGenie(bool IsGameGenieActive, GameGenieCode[] GameGenieCodes)
        {
            this.IsGameGenieActive = IsGameGenieActive;
            this.GameGenieCodes = GameGenieCodes;
        }
        #endregion

        #region Save and load
        /// <summary>
        /// Save prg ram banks that marked as baterry.
        /// </summary>
        /// <param name="stream">The stream to use to save. This stream must be initialized and ready to write.</param>
        public void SaveSRAM(Stream stream)
        {
            for (int i = 0; i < prg_banks.Length; i++)
            {
                if (prg_isram[i] && prg_battery[i])
                {
                    stream.Write(prg_banks[i], 0, prg_banks[i].Length);
                }
            }
        }
        /// <summary>
        /// Get prg ram banks (all togather) that marked as battery as one buffer.
        /// </summary>
        /// <returns></returns>
        public byte[] GetSRAMBuffer()
        {
            List<byte> buffer = new List<byte>();
            for (int i = 0; i < prg_banks.Length; i++)
            {
                if (prg_isram[i] && prg_battery[i])
                {
                    buffer.AddRange(prg_banks[i]);
                }
            }
            return buffer.ToArray();
        }
        /// <summary>
        /// Load prg ram banks that marked as battery.
        /// </summary>
        /// <param name="stream">The stream to use to read. This stream must be initialized and ready to read (at the offset of the s-ram).</param>
        public void LoadSRAM(Stream stream)
        {
            for (int i = 0; i < prg_banks.Length; i++)
            {
                if (prg_isram[i] && prg_battery[i])
                {
                    stream.Read(prg_banks[i], 0, prg_banks[i].Length);
                }
            }
        }
        /// <summary>
        /// Load prg ram banks that marked as battery.
        /// </summary>
        /// <param name="buffer"></param>
        public void LoadSRAM(byte[] buffer)
        {
            int o = 0;
            for (int i = 0; i < prg_banks.Length; i++)
            {
                if (prg_isram[i] && prg_battery[i])
                {
                    for (int j = 0; j < 0x2000; j++)
                    {
                        prg_banks[i][j] = buffer[j + o];
                    }
                    o += 0x2000;
                }
            }
        }
        /// <summary>
        /// Save board state
        /// </summary>
        /// <param name="stream">The stream that should be used to write data</param>
        public virtual void SaveState(System.IO.BinaryWriter stream)
        {
            // Write prg ram
            for (int i = 0; i < prg_banks.Length; i++)
            {
                if (prg_isram[i])
                {
                    stream.Write(prg_banks[i]);
                }
            }
            // Write prg indexes, enable ...etc
            for (int i = 0; i < prg_indexes.Length; i++)
                stream.Write(prg_indexes[i]);
            for (int i = 0; i < prg_enable.Length; i++)
                stream.Write(prg_enable[i]);
            for (int i = 0; i < prg_writable.Length; i++)
                stream.Write(prg_writable[i]);
            // Write chr ram
            for (int i = 0; i < chr_banks.Length; i++)
            {
                if (chr_isram[i])
                {
                    stream.Write(chr_banks[i]);
                }
            }
            // Write chr indexes, enable ...etc
            for (int i = 0; i < chr_indexes.Length; i++)
                stream.Write(chr_indexes[i]);
            for (int i = 0; i < chr_enable.Length; i++)
                stream.Write(chr_enable[i]);
            for (int i = 0; i < chr_writable.Length; i++)
                stream.Write(chr_writable[i]);
            // Write nmt
            for (int i = 0; i < nmt_banks.Length; i++)
            {
                stream.Write(nmt_banks[i]);
            }
            // Write chr indexes, enable ...etc
            for (int i = 0; i < nmt_indexes.Length; i++)
                stream.Write(nmt_indexes[i]);
        }
        /// <summary>
        /// Load board state
        /// </summary>
        /// <param name="stream">The stream that should be used to read data</param>
        public virtual void LoadState(System.IO.BinaryReader stream)
        {
            // Read prg ram
            for (int i = 0; i < prg_banks.Length; i++)
            {
                if (prg_isram[i])
                {
                    stream.Read(prg_banks[i], 0, prg_banks[i].Length);
                }
            }
            // Write prg indexes, enable ...etc
            for (int i = 0; i < prg_indexes.Length; i++)
                prg_indexes[i] = stream.ReadInt32();
            for (int i = 0; i < prg_enable.Length; i++)
                prg_enable[i] = stream.ReadBoolean();
            for (int i = 0; i < prg_writable.Length; i++)
                prg_writable[i] = stream.ReadBoolean();
            // Write chr ram
            for (int i = 0; i < chr_banks.Length; i++)
            {
                if (chr_isram[i])
                {
                    stream.Read(chr_banks[i], 0, chr_banks[i].Length);
                }
            }
            // Write chr indexes, enable ...etc
            for (int i = 0; i < chr_indexes.Length; i++)
                chr_indexes[i] = stream.ReadInt32();
            for (int i = 0; i < chr_enable.Length; i++)
                chr_enable[i] = stream.ReadBoolean();
            for (int i = 0; i < chr_writable.Length; i++)
                chr_writable[i] = stream.ReadBoolean();
            // Write nmt
            for (int i = 0; i < nmt_banks.Length; i++)
            {
                stream.Read(nmt_banks[i], 0, nmt_banks[i].Length);
            }
            // Write chr indexes, enable ...etc
            for (int i = 0; i < nmt_indexes.Length; i++)
                nmt_indexes[i] = stream.ReadInt32();
        }
        #endregion

        public virtual void VSUnisystem4016RW(ref byte data)
        {
            if ((data & 0x4) == 0x4)
                Switch08KCHR(1, chr_01K_rom_count > 0);
            else
                Switch08KCHR(0, chr_01K_rom_count > 0);
        }
    }
}

