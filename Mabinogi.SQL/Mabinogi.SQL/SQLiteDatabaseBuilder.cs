using System;
using System.IO;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mabinogi.SQL
{
    public static class SQLiteDatabaseBuilder
    {
        private static readonly string ConnectionString = "Data Source={0};Version=3;";
        private static readonly string DBName = "Mabinogi.db";
        public static void BuildDatabases()
        {
            string dblocation = Directory.GetCurrentDirectory() + @"\"+ DBName;

            var types = System.Reflection.Assembly.GetExecutingAssembly().GetTypes().Where(x => x.BaseType == typeof(BaseTable) || x.BaseType == typeof(ItemIdTable)).ToArray();
            List<BaseTable> tables = new List<BaseTable>(types.Length);
            foreach (var x in types)
            {
                try
                {
                    tables.Add((BaseTable)Activator.CreateInstance(x));
                }
                catch (MissingMethodException ex)
                {
                }
            }

            using (var conn = new SQLiteConnection(string.Format(ConnectionString, dblocation)))
            {
                conn.Open();
                foreach(BaseTable table in tables)
                {
                    BuildTable(conn, table);
                }

                foreach (BaseTable table in tables)
                {
                    BuildIndexes(conn, table);
                }

                conn.Close();
            }

            Console.WriteLine("Built!");
        }

        private readonly static string createIndexCmdStr = "CREATE INDEX 'idx_{1}_{0}' ON `{1}` ({2});";
        private static void BuildIndexes(SQLiteConnection conn, BaseTable table)
        {
            if (table.Keys != null && table.Keys.Count > 0)
            {
                string keyname, columns;

                for (int i = 0; i < table.Keys.Count; i++)
                {
                    keyname = string.Empty;
                    columns = string.Empty;
                    for (int j = 0; j < table.Keys[i].Length; j++)
                    {
                        keyname += table.Keys[i][j];
                        columns += "`" + table.Keys[i][j] + "`";

                        if (table.Keys[i].Length != j + 1)
                        {
                            columns += ", ";
                            keyname += "_";
                        }
                    }
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = string.Format(createIndexCmdStr, keyname, table.TableName, columns); ;
                        Console.WriteLine(cmd.CommandText);
                        cmd.ExecuteNonQuery();
                    }
                }

            }
        }

        private const string createForeignKeyStr = "CONSTRAINT `{0}_ibfk{1}` FOREIGN KEY ({2}) REFERENCES `{3}` ({4})";


        private const string createTableCmdStr = "CREATE TABLE `{0}` (";
        public static void BuildTable(SQLiteConnection conn, BaseTable table)
        {
            string builder = string.Empty;
            builder += string.Format(createTableCmdStr, table.TableName);

            
            for(int i = 0; i < table.Columns.Count; i++)
            {
                builder += string.Format( "`{0}` {1} {2}, ", table.Columns[i].Name, table.Columns[i].TypeToString(true) + (table.Columns[i].Options == 1 ? " AUTO_INCREMENT" : string.Empty) , table.Columns[i].DefaultToString(true));
            }


            if (table.PrimaryKey != null && table.PrimaryKey.Length > 0)
            {
                builder += "PRIMARY KEY (";
                for (int i = 0; i < table.PrimaryKey.Length; i++)
                {
                    builder += "`" + table.PrimaryKey[i] + "`";

                    if (table.PrimaryKey.Length != i + 1)
                        builder += ",";
                }
                builder += "),";
            }

            if (table.ForeignKeys != null && table.ForeignKeys.Length > 0)
            {
                string localColumns, foreignColumns;

                for (int i = 0; i < table.ForeignKeys.Length; i++)
                {
                    if (table.ForeignKeys[i].ForeignColumns.Length == table.ForeignKeys[i].LocalColumns.Length)
                    {
                        localColumns = foreignColumns = string.Empty;
                        for (int j = 0; j < table.ForeignKeys[i].ForeignColumns.Length; j++)
                        {
                            localColumns += "`" + table.ForeignKeys[i].LocalColumns[j] + "`,";
                            foreignColumns += "`" + table.ForeignKeys[i].ForeignColumns[j] + "`,";
                        }
                        localColumns = localColumns.Trim(',');
                        foreignColumns = foreignColumns.Trim(',');
                        builder += string.Format(createForeignKeyStr, table.TableName, i, localColumns, table.ForeignKeys[i].Table, foreignColumns);

                        if (table.ForeignKeys.Length != i + 1)
                            builder += ",";
                    }
                    else
                    {
                        throw new Exception("Foreign and Local key count doesn't match up.");
                    }
                }
            }

            builder = builder.Trim(' ', ',');
            builder += ")";

            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = builder;
                Console.WriteLine(cmd.CommandText);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
