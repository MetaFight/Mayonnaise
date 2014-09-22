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
using System.Diagnostics;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
namespace MyNes
{
    /// <summary>
    /// Class can be used to manage My Nes database using SQLite.
    /// </summary>
    class MyNesDB
    {
        private static SQLiteConnection myconnection;
        public static string FilePath { get; set; }
        public static bool IsDatabaseLoaded { get; private set; }
        public static string DBName { get; set; }

        /// <summary>
        /// Create a database
        /// </summary>
        /// <param name="databaseName">The name of the database</param>
        /// <param name="fileName">The full path of the database file</param>
        public static void CreateDatabase(string databaseName, string fileName)
        {
            ReleaseDatabase();
            if (File.Exists(fileName))
            {
                Trace.WriteLine("My Nes database file already exists, deleting file...");
                try
                {
                    File.Delete(fileName);
                }
                catch { }
            }
            try
            {
                Trace.WriteLine("Creating new SQLite connection...");
                //DbProviderFactory fact = DbProviderFactories.GetFactory("System.Data.SQLite");
                //myconnection = (SQLiteConnection)fact.CreateConnection();
                myconnection = new SQLiteConnection();
                myconnection.ConnectionString = string.Format("Data Source={0};Version=3;New=True;Compress=True;UseUTF16Encoding=true;foreign keys=true;", fileName);
                myconnection.Open();
                Trace.WriteLine("SQLite connection opened successfully.");
                // Create info table

                using (SQLiteTransaction mytransaction = myconnection.BeginTransaction())
                {
                    using (SQLiteCommand mycommand = new SQLiteCommand(myconnection))
                    {
                        // Build default table
                        string[] lines = Properties.Resources.DBTables.Split('\n');
                        foreach (string line in lines)
                        {
                            if (line.Length > 0)
                            {
                                if (!line.StartsWith(";"))
                                {
                                    Trace.WriteLine("Executing command: " + line);
                                    mycommand.CommandText = line;
                                    mycommand.ExecuteNonQuery();
                                }
                            }
                        }
                        // Insert information to the info table
                        Trace.WriteLine("Creating SQLite info table ...");
                        mycommand.CommandText = string.Format("INSERT INTO info VALUES ('{0}', '{1}');",
                            databaseName, DateTimeFormater.ToFull(DateTime.Now));
                        mycommand.ExecuteNonQuery();
                    }
                    mytransaction.Commit();
                }
                Trace.WriteLine("SQLite info table created successfully.");
                Trace.WriteLine("Applying...");
                FilePath = fileName;

                Trace.WriteLine("SQLite database now ready to use !");
            }
            catch (Exception ex)
            {
                Trace.TraceError("Unable to create database file !");
                Trace.TraceError("Exception: " + ex.Message);

                throw ex;// make the caller handle this exception
            }
            // FOR TEST
            SQLiteDataAdapter db = new SQLiteDataAdapter("select * from info", myconnection);
            DataSet set = new DataSet();
            DataTable DT = new DataTable();
            db.Fill(set);
            DT = set.Tables[0];
            DBName = DT.Rows[0][0].ToString();
            Trace.WriteLine("'" + DT.Rows[0][0] + "' database created at " + DT.Rows[0][1]);

            IsDatabaseLoaded = true;
        }
        /// <summary>
        /// Open database file
        /// </summary>
        /// <param name="fileName">The full path of the database file</param>
        public static void OpenDatabase(string fileName)
        {
            ReleaseDatabase();
            try
            {
                Trace.WriteLine("Creating new SQLite connection...");
                //DbProviderFactory fact = DbProviderFactories.GetFactory("System.Data.SQLite");
                //myconnection = (SQLiteConnection)fact.CreateConnection();
                myconnection = new SQLiteConnection();
                myconnection.ConnectionString = string.Format("Data Source={0};Version=3;New=False;Compress=True;foreign keys=true;", fileName);
                myconnection.Open();
                Trace.WriteLine("SQLite connection opened successfully.");

                Trace.WriteLine("Applying...");
                FilePath = fileName;

                Trace.WriteLine("SQLite database now ready to use !");
            }
            catch (Exception ex)
            {
                Trace.TraceError("Unable to open database file !");
                Trace.TraceError("Exception: " + ex.Message);

                throw ex;// make the caller handle this exception
            }
            // FOR TEST
            SQLiteDataAdapter db = new SQLiteDataAdapter("select * from info", myconnection);
            DataSet set = new DataSet();
            DataTable DT = new DataTable();
            db.Fill(set);
            DT = set.Tables[0];
            DBName = DT.Rows[0][0].ToString();
            Trace.WriteLine("Database opened: '" + DT.Rows[0][0] + "' that created at " + DT.Rows[0][1]);
            IsDatabaseLoaded = true;
        }
        /// <summary>
        /// Release and close current loaded database.
        /// </summary>
        public static void ReleaseDatabase()
        {
            if (myconnection != null)
            {
                Trace.WriteLine("Closing SQLite connection...");
                myconnection.Close();
                myconnection.Dispose();
                myconnection = null;
                Trace.WriteLine("SQLite connection closed successfully.");
            }
        }

        /// <summary>
        /// Get data set for table from the database
        /// </summary>
        /// <param name="tableName">The table name</param>
        /// <returns>DataSet object for given table name if found.</returns>
        public static DataSet GetDataSet(string tableName)
        {
            if (myconnection == null)
            {
                throw new Exception("The SQLite connection is not running, can't make any requests.");
            }
            SQLiteDataAdapter db = new SQLiteDataAdapter("select * from " + @"""" + tableName + @"""", myconnection);
            DataSet set = new DataSet();
            db.Fill(set);

            return set;
        }
        public static DataSet GetDataSet(string tableName, string columnName, bool sortAZ)
        {
            if (myconnection == null)
            {
                throw new Exception("The SQLite connection is not running, can't make any requests.");
            }
            SQLiteDataAdapter db = new SQLiteDataAdapter("select * from " + @"""" + tableName + @"""" + " ORDER BY [" + columnName + (sortAZ ? "];" : "] DESC;"), myconnection);
            DataSet set = new DataSet();
            db.Fill(set);

            return set;
        }
        public static DataSet GetDataSet(string tableName, string colCondition, object condition)
        {
            if (myconnection == null)
            {
                throw new Exception("The SQLite connection is not running, can't make any requests.");
            }
            SQLiteDataAdapter db = new SQLiteDataAdapter("select * from " + @"""" + tableName + @"""" + " WHERE [" + colCondition + "] = '" + condition + "';", myconnection);
            DataSet set = new DataSet();
            db.Fill(set);

            return set;
        }
        public static DataSet GetDataSet(string tableName, string colCondition, object condition, string columnName, bool sortAZ)
        {
            if (myconnection == null)
            {
                throw new Exception("The SQLite connection is not running, can't make any requests.");
            }
            SQLiteDataAdapter db = new SQLiteDataAdapter("select * from " + @"""" + tableName + @"""" + " WHERE [" + colCondition + "] = '" + condition + "' ORDER BY [" + columnName + (sortAZ ? "];" : "] DESC;"), myconnection);
            DataSet set = new DataSet();
            db.Fill(set);

            return set;
        }
        /// <summary>
        /// Update a table in the database using dataset
        /// </summary>
        /// <param name="tableName">The table name</param>
        /// <param name="dataset">The dataset to use</param>
        public static void UpdateTableFromDataSet(string tableName, DataSet dataset)
        {
            SQLiteDataAdapter db = new SQLiteDataAdapter("select * from " + @"""" + tableName + @"""", myconnection);
            SQLiteCommandBuilder cmdBuilder = new SQLiteCommandBuilder(db);
            db.Update(dataset);
        }
        /// <summary>
        /// Add new entry to the database.
        /// </summary>
        /// <param name="entry">The entry info</param>
        /// <returns>True if the entry added successfully otherwise false.</returns>
        public static bool AddEntry(MyNesDBEntryInfo entry)
        {
            // Fix
            if (entry.Name == null)
                entry.Name = "";
            if (entry.AlternativeName == null)
                entry.AlternativeName = "";
            if (entry.Path == null)
                entry.Path = "";
            if (entry.Class == null)
                entry.Class = "";
            if (entry.Publisher == null)
                entry.Publisher = "";
            if (entry.Developer == null)
                entry.Developer = "";
            if (entry.Region == null)
                entry.Region = "";
            if (entry.Players == null)
                entry.Players = "";
            if (entry.ReleaseDate == null)
                entry.ReleaseDate = "";
            if (entry.System == null)
                entry.System = "";
            if (entry.CRC == null)
                entry.CRC = "";
            if (entry.SHA1 == null)
                entry.SHA1 = "";
            if (entry.Dump == null)
                entry.Dump = "";
            if (entry.Dumper == null)
                entry.Dumper = "";
            if (entry.DateDumped == null)
                entry.DateDumped = "";
            if (entry.BoardType == null)
                entry.BoardType = "";
            if (entry.BoardPcb == null)
                entry.BoardPcb = "";
            // try
            {
                using (SQLiteTransaction mytransaction = myconnection.BeginTransaction())
                {
                    using (SQLiteCommand mycommand = new SQLiteCommand(myconnection))
                    {
                        // Main info
                        string command = string.Format(
                            "INSERT INTO GAMES([IsDB], [Name], [Alternative Name], [Size], [Path], [Rating], [Played], [Play Time]," +
                            " [Last Played], [Class], [Catalog], [Publisher], [Developer], [Region], [Players], [Release Date]," +
                            " [System], [CRC], [SHA1], [Dump], [Dumper], [Date Dumped], [Board Type], [Board Pcb], [Board Mapper])" +
                            " VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}'," +
                            " '{10}', '{11}', '{12}', '{13}', '{14}', '{15}', '{16}', '{17}', '{18}', '{19}'," +
                            " '{20}', '{21}', '{22}', '{23}', '{24}');",
                            entry.IsDB,
                            entry.Name.Replace("'", "&apos;"),
                            entry.AlternativeName.Replace("'", "&apos;"),
                            entry.Size,
                            entry.Path.Replace("'", "&apos;"),
                            entry.Rating,
                            entry.Played,
                            entry.PlayTime,
                            entry.LastPlayed,
                            entry.Class.Replace("'", "&apos;"),
                            entry.Catalog.Replace("'", "&apos;"),
                            entry.Publisher.Replace("'", "&apos;"),
                            entry.Developer.Replace("'", "&apos;"),
                            entry.Region.Replace("'", "&apos;"),
                            entry.Players.Replace("'", "&apos;"),
                            entry.ReleaseDate,
                            entry.System.Replace("'", "&apos;"),
                            entry.CRC,
                            entry.SHA1,
                            entry.Dump.Replace("'", "&apos;"),
                            entry.Dumper.Replace("'", "&apos;"),
                            entry.DateDumped.Replace("'", "&apos;"),
                            entry.BoardType.Replace("'", "&apos;"),
                            entry.BoardPcb.Replace("'", "&apos;"),
                            entry.BoardMapper);

                        //"INSERT INTO GAMES([Name], [Alternative Name], [Size], [Path], [Rating], [Class], [Publisher], [Developer], [Region], [Players], [Release Date], [System], [CRC], [SHA1], [Dump], [Date Dumped], [Board Type], [Board Pcb], [Board Mapper]) VALUES ('Muppet Adventure: Chaos at the Carnival', '', 'N/A', 'N/A', '0', 'Licensed', 'Hi Tech Expressions', 'Mind's Eye', 'USA', '1', '1990-11', 'NES-NTSC', '7156CB4D', '03A111AC0FA78E566814D6F9296454BFC34E7B3C', 'ok', '2006-03-18', 'NES-SGROM', 'NES-SGROM-04', '1');
                        mycommand.CommandText = command;
                        mycommand.ExecuteNonQuery();
                    }
                    mytransaction.Commit();
                }
            }
            // catch (Exception ex)
            // {
            //     Trace.TraceError(ex.Message);
            //     return false;
            //}
            return true;
        }
        public static void SetEntryToRow(MyNesDBEntryInfo entry, DataRow row)
        {
            // Fix
            if (entry.Name == null)
                entry.Name = "";
            if (entry.AlternativeName == null)
                entry.AlternativeName = "";
            if (entry.Path == null)
                entry.Path = "";
            if (entry.Class == null)
                entry.Class = "";
            if (entry.Publisher == null)
                entry.Publisher = "";
            if (entry.Developer == null)
                entry.Developer = "";
            if (entry.Region == null)
                entry.Region = "";
            if (entry.Players == null)
                entry.Players = "";
            if (entry.ReleaseDate == null)
                entry.ReleaseDate = "";
            if (entry.System == null)
                entry.System = "";
            if (entry.CRC == null)
                entry.CRC = "";
            if (entry.SHA1 == null)
                entry.SHA1 = "";
            if (entry.Dump == null)
                entry.Dump = "";
            if (entry.Dumper == null)
                entry.Dumper = "";
            if (entry.DateDumped == null)
                entry.DateDumped = "";
            if (entry.BoardType == null)
                entry.BoardType = "";
            if (entry.BoardPcb == null)
                entry.BoardPcb = "";
            row["IsDB"] = entry.IsDB;
            row["Name"] = entry.Name.Replace("'", "&apos;");
            row["Alternative Name"] = entry.AlternativeName.Replace("'", "&apos;");
            row["Size"] = entry.Size;
            row["Path"] = entry.Path.Replace("'", "&apos;");
            row["Rating"] = entry.Rating;
            row["Played"] = entry.Played;
            row["Play Time"] = entry.PlayTime;
            row["Last Played"] = entry.LastPlayed;
            row["Class"] = entry.Class.Replace("'", "&apos;");
            row["Catalog"] = entry.Catalog.Replace("'", "&apos;");
            row["Publisher"] = entry.Publisher.Replace("'", "&apos;");
            row["Developer"] = entry.Developer.Replace("'", "&apos;");
            row["Region"] = entry.Region.Replace("'", "&apos;");
            row["Players"] = entry.Players.Replace("'", "&apos;");
            row["Release Date"] = entry.ReleaseDate;
            row["System"] = entry.System.Replace("'", "&apos;");
            row["CRC"] = entry.CRC;
            row["SHA1"] = entry.SHA1;
            row["Dump"] = entry.Dump.Replace("'", "&apos;");
            row["Dumper"] = entry.Dumper.Replace("'", "&apos;");
            row["Date Dumped"] = entry.DateDumped.Replace("'", "&apos;");
            row["Board Type"] = entry.BoardType.Replace("'", "&apos;");
            row["Board Pcb"] = entry.BoardPcb.Replace("'", "&apos;");
            row["Board Mapper"] = entry.BoardMapper;
        }

        /*Entries*/
        public static bool UpdateEntry(string id, int rating)
        {
            try
            {
                using (SQLiteTransaction mytransaction = myconnection.BeginTransaction())
                {
                    using (SQLiteCommand mycommand = new SQLiteCommand(myconnection))
                    {
                        mycommand.CommandText = string.Format(
                            "UPDATE GAMES SET [Rating] = '{0}' WHERE [Id] = '{1}';", rating, id);
                        mycommand.ExecuteNonQuery();
                    }
                    mytransaction.Commit();
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                return false;
            }
            return true;
        }
        public static bool UpdateEntry(string id, string Path, int Size)
        {
            //try
            {
                Path = Path.Replace("'", "&apos;");
                using (SQLiteTransaction mytransaction = myconnection.BeginTransaction())
                {
                    using (SQLiteCommand mycommand = new SQLiteCommand(myconnection))
                    {
                        mycommand.CommandText = string.Format(
                            "UPDATE GAMES SET [Path] = '{0}', [Size] = '{1}' WHERE [Id] = '{2}';", Path, Size, id);
                        mycommand.ExecuteNonQuery();
                    }
                    mytransaction.Commit();
                }
            }
            //catch (Exception ex)
            // {
            //    Trace.TraceError(ex.Message);
            //    return false;
            // }
            return true;
        }
        public static bool UpdateEntry(string id, int Played, int PlayedTime, DateTime lastPlayed)
        {
            //try
            {
                using (SQLiteTransaction mytransaction = myconnection.BeginTransaction())
                {
                    using (SQLiteCommand mycommand = new SQLiteCommand(myconnection))
                    {
                        mycommand.CommandText = string.Format(
                            "UPDATE GAMES SET [Played] = '{0}', [Play Time] = '{1}', [Last Played] = '{2}' WHERE [Id] = '{3}';",
                            Played, PlayedTime, DateTimeFormater.ToFull(lastPlayed), id);
                        mycommand.ExecuteNonQuery();
                    }
                    mytransaction.Commit();
                }
            }
            //catch (Exception ex)
            // {
            //    Trace.TraceError(ex.Message);
            //    return false;
            // }
            return true;
        }
        public static MyNesDBEntryInfo GetEntry(string id)
        {
            if (myconnection == null)
            {
                throw new Exception("The SQLite connection is not running, can't make any requests.");
            }
            MyNesDBEntryInfo entry = new MyNesDBEntryInfo();
            using (SQLiteTransaction mytransaction = myconnection.BeginTransaction())
            {
                using (SQLiteCommand mycommand = new SQLiteCommand(myconnection))
                {
                    mycommand.CommandText = "SELECT [Name] FROM GAMES WHERE [Id]='" + id + "';";
                    entry.Name = mycommand.ExecuteScalar().ToString().Replace("&apos;", "'");

                    mycommand.CommandText = "SELECT [Size] FROM GAMES WHERE [Id]='" + id + "';";
                    int val = 0;
                    int.TryParse(mycommand.ExecuteScalar().ToString(), out val);
                    entry.Size = val;

                    mycommand.CommandText = "SELECT [Path] FROM GAMES WHERE [Id]='" + id + "';";
                    entry.Path = mycommand.ExecuteScalar().ToString().Replace("&apos;", "'");

                    mycommand.CommandText = "SELECT [Rating] FROM GAMES WHERE [Id]='" + id + "';";
                    val = 0;
                    int.TryParse(mycommand.ExecuteScalar().ToString(), out val);
                    entry.Rating = val;

                    mycommand.CommandText = "SELECT [IsDB] FROM GAMES WHERE [Id]='" + id + "';";
                    bool bval = false;
                    bool.TryParse(mycommand.ExecuteScalar().ToString(), out bval);
                    entry.IsDB = bval;

                    mycommand.CommandText = "SELECT [Played] FROM GAMES WHERE [Id]='" + id + "';";
                    val = 0;
                    int.TryParse(mycommand.ExecuteScalar().ToString(), out val);
                    entry.Played = val;

                    mycommand.CommandText = "SELECT [Play Time] FROM GAMES WHERE [Id]='" + id + "';";
                    val = 0;
                    int.TryParse(mycommand.ExecuteScalar().ToString(), out val);
                    entry.PlayTime = val;

                    mycommand.CommandText = "SELECT [Last Played] FROM GAMES WHERE [Id]='" + id + "';";
                    entry.LastPlayed = mycommand.ExecuteScalar().ToString();
                }
                mytransaction.Commit();
            }
            return entry;
        }
        public static DataSet GetEntryDataSet(string id)
        {
            if (myconnection == null)
            {
                throw new Exception("The SQLite connection is not running, can't make any requests.");
            }
            SQLiteDataAdapter db = new SQLiteDataAdapter("select * from GAMES WHERE [Id] = '" + id + "';", myconnection);
            DataSet set = new DataSet();
            db.Fill(set);

            return set;
        }
        /// <summary>
        /// Delete an entry from the database.
        /// </summary>
        /// <param name="id">The entry id</param>
        /// <returns>True if the entry deleted successfully otherwise false.</returns>
        public static bool DeleteEntry(string id)
        {
            try
            {
                Trace.WriteLine("Deleting entry ....");

                // Add the table to the database...
                using (SQLiteTransaction mytransaction = myconnection.BeginTransaction())
                {
                    using (SQLiteCommand mycommand = new SQLiteCommand(myconnection))
                    {
                        Trace.WriteLine("Sending insert command..");
                        mycommand.CommandText = "DELETE FROM GAMES WHERE ID=" + id + ";";
                        mycommand.ExecuteNonQuery();
                    }
                    mytransaction.Commit();
                }
                Trace.WriteLine("Entry deleted successfully.");
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                return false;
            }
            return true;
        }
        public static void DeleteEntries()
        {
            Trace.WriteLine("Clearing entries ....");
            using (SQLiteTransaction mytransaction = myconnection.BeginTransaction())
            {
                using (SQLiteCommand mycommand = new SQLiteCommand(myconnection))
                {
                    Trace.WriteLine("Sending delete command..");
                    mycommand.CommandText = "DELETE FROM GAMES;";
                    mycommand.ExecuteNonQuery();
                }
                mytransaction.Commit();
            }
            Trace.WriteLine("Entries cleared successfully.");
        }
        /*Columns*/
        public static bool AddColumn(string colName, bool colVisible, int width)
        {
            try
            {
                Trace.WriteLine("Adding new column entry ....");
                using (SQLiteTransaction mytransaction = myconnection.BeginTransaction())
                {
                    using (SQLiteCommand mycommand = new SQLiteCommand(myconnection))
                    {
                        Trace.WriteLine("Sending insert command..");
                        // Main info
                        mycommand.CommandText = string.Format(
                            "INSERT INTO COLUMNS([Column Name], [Visible], [Width]) VALUES ('{0}', '{1}', '{2}');",
                           colName, colVisible, width);
                        mycommand.ExecuteNonQuery();
                    }
                    mytransaction.Commit();
                }
                Trace.WriteLine("Column entry added successfully.");
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                return false;
            }
            return true;
        }
        public static bool DeleteColumn(string colName)
        {
            try
            {
                Trace.WriteLine("Deleting column entry ....");
                using (SQLiteTransaction mytransaction = myconnection.BeginTransaction())
                {
                    using (SQLiteCommand mycommand = new SQLiteCommand(myconnection))
                    {
                        Trace.WriteLine("Sending insert command..");
                        mycommand.CommandText = "DELETE FROM COLUMNS WHERE [Column Name]=" + colName + ";";
                        mycommand.ExecuteNonQuery();
                    }
                    mytransaction.Commit();
                }
                Trace.WriteLine("Column entry deleted successfully.");
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                return false;
            }
            return true;
        }
        public static void DeleteAllColumns()
        {
            Trace.WriteLine("Clearing columns ....");
            using (SQLiteTransaction mytransaction = myconnection.BeginTransaction())
            {
                using (SQLiteCommand mycommand = new SQLiteCommand(myconnection))
                {
                    Trace.WriteLine("Sending delete command..");
                    mycommand.CommandText = "DELETE FROM COLUMNS;";
                    mycommand.ExecuteNonQuery();
                }
                mytransaction.Commit();
            }
            Trace.WriteLine("Columns cleared successfully.");
        }
        public static MyNesDBColumn[] GetColumns()
        {
            if (myconnection == null)
            {
                throw new Exception("The SQLite connection is not running, can't make any requests.");
            }
            List<MyNesDBColumn> columns = new List<MyNesDBColumn>();

            SQLiteDataAdapter db = new SQLiteDataAdapter("select * from COLUMNS;", myconnection);
            DataSet set = new DataSet();
            db.Fill(set);
            for (int i = 0; i < set.Tables[0].Rows.Count; i++)
            {
                MyNesDBColumn col = new MyNesDBColumn();
                col.Name = (string)set.Tables[0].Rows[i]["Column Name"];

                bool vis = true;
                bool.TryParse(set.Tables[0].Rows[i]["Visible"].ToString(), out vis);
                col.Visible = vis;

                int w = 70;
                int.TryParse(set.Tables[0].Rows[i]["Width"].ToString(), out w);
                col.Width = w;
                columns.Add(col);
            }
            return columns.ToArray();
        }
        public static int GetColumnIndex(string colName)
        {
            if (myconnection == null)
            {
                throw new Exception("The SQLite connection is not running, can't make any requests.");
            }
            int index = 0;
            using (SQLiteTransaction mytransaction = myconnection.BeginTransaction())
            {
                using (SQLiteCommand mycommand = new SQLiteCommand(myconnection))
                {
                    // TODO: get column index fix.
                    mycommand.CommandText = "SELECT COUNT(*) FROM COLUMNS WHERE [Column Name] = '" + colName + "';";
                    index = Convert.ToInt32(mycommand.ExecuteScalar());
                }
                mytransaction.Commit();
            }
            return index;
        }
        public static MyNesDBColumn GetColumn(string colName)
        {
            if (myconnection == null)
            {
                throw new Exception("The SQLite connection is not running, can't make any requests.");
            }
            MyNesDBColumn entry = new MyNesDBColumn();
            entry.Name = colName;
            using (SQLiteTransaction mytransaction = myconnection.BeginTransaction())
            {
                using (SQLiteCommand mycommand = new SQLiteCommand(myconnection))
                {
                    mycommand.CommandText = "SELECT [Visible] FROM COLUMNS WHERE [Column Name]='" + colName + "';";
                    bool bval = false;
                    bool.TryParse(mycommand.ExecuteScalar().ToString(), out bval);
                    entry.Visible = bval;

                    mycommand.CommandText = "SELECT [Width] FROM COLUMNS WHERE [Column Name]='" + colName + "';";
                    int val = 0;
                    int.TryParse(mycommand.ExecuteScalar().ToString(), out val);
                    entry.Width = val;
                }
                mytransaction.Commit();
            }
            return entry;
        }
        public static bool UpdateColumn(string colName, bool colVisible, int width)
        {
            try
            {
                Trace.WriteLine("Updating column entry ....");
                using (SQLiteTransaction mytransaction = myconnection.BeginTransaction())
                {
                    using (SQLiteCommand mycommand = new SQLiteCommand(myconnection))
                    {
                        Trace.WriteLine("Sending update command..");
                        // Main info
                        mycommand.CommandText = string.Format(
                            "UPDATE COLUMNS SET [Visible] = '{0}', [Width] = '{1}' WHERE [Column Name] = '{2}';", colVisible, width, colName);
                        mycommand.ExecuteNonQuery();
                    }
                    mytransaction.Commit();
                }
                Trace.WriteLine("Column entry updated successfully.");
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                return false;
            }
            return true;
        }
        public static bool UpdateColumn(string colName, bool colVisible)
        {
            try
            {
                Trace.WriteLine("Updating column entry ....");
                using (SQLiteTransaction mytransaction = myconnection.BeginTransaction())
                {
                    using (SQLiteCommand mycommand = new SQLiteCommand(myconnection))
                    {
                        Trace.WriteLine("Sending update command..");
                        // Main info
                        mycommand.CommandText = string.Format(
                            "UPDATE COLUMNS SET [Visible] = '{0}' WHERE [Column Name] = '{1}';", colVisible, colName);
                        mycommand.ExecuteNonQuery();
                    }
                    mytransaction.Commit();
                }
                Trace.WriteLine("Column entry updated successfully.");
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                return false;
            }
            return true;
        }
        /*Snapshots*/
        public static void AddDetect(string table, string gameId, string name, string path, string fileInfo)
        {
            using (SQLiteTransaction mytransaction = myconnection.BeginTransaction())
            {
                using (SQLiteCommand mycommand = new SQLiteCommand(myconnection))
                {
                    // Main info
                    mycommand.CommandText = string.Format(
                        "INSERT INTO {0}([Game ID], [Name], [Path], [File Info]) VALUES ('{1}', '{2}', '{3}', '{4}');",
                       table, gameId, name.Replace("'", "&apos;"), path.Replace("'", "&apos;"), fileInfo);
                    mycommand.ExecuteNonQuery();
                }
                mytransaction.Commit();
            }
        }
        public static void AddDetect(string table, MyNesDetectEntryInfo info)
        {

            using (SQLiteTransaction mytransaction = myconnection.BeginTransaction())
            {
                using (SQLiteCommand mycommand = new SQLiteCommand(myconnection))
                {
                    // Main info
                    mycommand.CommandText = string.Format(
                        "INSERT INTO {0}([Game ID], [Name], [Path], [File Info]) VALUES ('{1}', '{2}', '{3}', '{4}');",
                       table, info.GameID, info.Name.Replace("'", "&apos;"), info.Path.Replace("'", "&apos;"), info.FileInfo);
                    mycommand.ExecuteNonQuery();
                }
                mytransaction.Commit();
            }
        }
        public static MyNesDetectEntryInfo[] GetDetects(string table, string gameId)
        {
            if (myconnection == null)
            {
                throw new Exception("The SQLite connection is not running, can't make any requests.");
            }
            List<MyNesDetectEntryInfo> detects = new List<MyNesDetectEntryInfo>();

            SQLiteDataAdapter db = new SQLiteDataAdapter(string.Format("select * from {0} WHERE [Game ID] = '{1}';", table, gameId), myconnection);
            DataSet set = new DataSet();
            db.Fill(set);
            for (int i = 0; i < set.Tables[0].Rows.Count; i++)
            {
                MyNesDetectEntryInfo inf = new MyNesDetectEntryInfo();
                inf.GameID = gameId;
                inf.Name = set.Tables[0].Rows[i]["Name"].ToString().Replace("&apos;", "'");
                inf.Path = set.Tables[0].Rows[i]["Path"].ToString().Replace("&apos;", "'");
                inf.FileInfo = set.Tables[0].Rows[i]["File Info"].ToString();
                detects.Add(inf);
            }
            return detects.ToArray();
        }
        public static void DeleteDetects(string table, string gameId)
        {
            using (SQLiteTransaction mytransaction = myconnection.BeginTransaction())
            {
                using (SQLiteCommand mycommand = new SQLiteCommand(myconnection))
                {
                    mycommand.CommandText = string.Format("DELETE FROM {0} WHERE [Game ID] = '{1}';", table, gameId);
                    mycommand.ExecuteNonQuery();
                }
                mytransaction.Commit();
            }
        }
        public static void DeleteDetect(string table, string gameId, string filePath)
        {
            using (SQLiteTransaction mytransaction = myconnection.BeginTransaction())
            {
                using (SQLiteCommand mycommand = new SQLiteCommand(myconnection))
                {
                    mycommand.CommandText = string.Format("DELETE FROM {0} WHERE [Game ID] = '{1}' AND [Path] = '{2}';",
                        table, gameId, filePath.Replace("'", "&apos;"));
                    mycommand.ExecuteNonQuery();
                }
                mytransaction.Commit();
            }
        }
        public static void DeleteDetects(string table)
        {
            using (SQLiteTransaction mytransaction = myconnection.BeginTransaction())
            {
                using (SQLiteCommand mycommand = new SQLiteCommand(myconnection))
                {
                    mycommand.CommandText = string.Format("DELETE FROM {0};", table);
                    mycommand.ExecuteNonQuery();
                }
                mytransaction.Commit();
            }
        }
    }
}
