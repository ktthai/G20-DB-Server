using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;

namespace Mabinogi.SQL
{
    public static class MySqlDatabaseBuilder
    {

        private static Dictionary<string, byte> Databases = new Dictionary<string, byte>();

        public static bool Test(string connectionStr)
        {
            try
            {
                using (var conn = new MySqlConnection(connectionStr))
                {
                    conn.Open();
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public static void BuildDatabases(string connectionStr)
        {
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

            using (MySqlConnection conn = new MySqlConnection(connectionStr))
            {
                conn.Open();

                foreach (BaseTable table in tables)
                {
                    BuildTable(conn, table);
                }

                foreach (BaseTable table in tables)
                {
                    BuildForeignKeys(conn, table);
                }

                conn.Close();
            }
        }

        public static void AddDataBase(MySqlConnection conn, string database)
        {
            var cmd = conn.CreateCommand();
            cmd.CommandText = string.Format(DefaultDatabaseSQL, database);

            cmd.ExecuteNonQuery();

            Databases.Add(database.ToLower(), 0);
        }

        private readonly static string CreateTableCmdStr = "USE {0}; CREATE TABLE `{1}` ( {2} ) ENGINE=InnoDB DEFAULT CHARSET utf8mb4;";

        public static void BuildTable(MySqlConnection conn, BaseTable table)
        {
            if (Databases.ContainsKey(table.DBName.ToLower()) == false)
            {
                AddDataBase(conn, table.DBName);
            }

            string builder = string.Empty;


            for (int i = 0; i < table.Columns.Count; i++)
            {
                builder += string.Format("`{0}` {1} {2}, ", table.Columns[i].Name, table.Columns[i].TypeToString(true), table.Columns[i].DefaultToString());
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

                    builder += string.Format("KEY `key_{0}` ({1}),", keyname, columns);
                }

            }


            builder = builder.Trim(' ', ',');

            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = string.Format(CreateTableCmdStr, table.DBName, table.TableName, builder);
                Console.WriteLine(cmd.CommandText);
                cmd.ExecuteNonQuery();
            }
        }
        private const string alterTableStr = "USE {0}; ALTER TABLE `{1}` ";
        private const string createForeignKeyStr = "ADD CONSTRAINT `{0}_ibfk{1}` FOREIGN KEY ({2}) REFERENCES `{3}` ({4}) {5}";
        private const string checkStr = " ";
        private const string deleteStr = " ON DELETE CASCADE";
        private const string updateStr = " ON UPDATE CASCADE";

        private static void BuildForeignKeys(MySqlConnection conn, BaseTable table)
        {
            if (table.ForeignKeys != null && table.ForeignKeys.Length > 0)
            {
                string builder = string.Empty;
                string localColumns, foreignColumns, defaultValue;

                for (int i = 0; i < table.ForeignKeys.Length; i++)
                {
                    if (table.ForeignKeys[i].ForeignColumns.Length == table.ForeignKeys[i].LocalColumns.Length)
                    {
                        defaultValue = localColumns = foreignColumns = string.Empty;
                        for (int j = 0; j < table.ForeignKeys[i].ForeignColumns.Length; j++)
                        {
                            localColumns += "`" + table.ForeignKeys[i].LocalColumns[j] + "`,";
                            foreignColumns += "`" + table.ForeignKeys[i].ForeignColumns[j] + "`,";
                        }
                        localColumns = localColumns.Trim(',');
                        foreignColumns = foreignColumns.Trim(',');
                        builder += string.Format(createForeignKeyStr, table.TableName, i, localColumns, table.ForeignKeys[i].Table, foreignColumns, defaultValue);

                        if (table.ForeignKeys[i].Delete)
                            builder += deleteStr;

                        if (table.ForeignKeys[i].Update)
                            builder += updateStr;

                        if (table.ForeignKeys.Length != i + 1)
                            builder += ",";
                        else
                            builder += ";";


                    }
                    else
                    {
                        throw new Exception("Foreign and Local key count doesn't match up.");
                    }
                }

                var cmd = conn.CreateCommand();
                cmd.CommandText = string.Format(alterTableStr, table.DBName, table.TableName) + builder;
                cmd.ExecuteNonQuery();
            }
        }

        private readonly static string DefaultDatabaseSQL = "CREATE DATABASE `{0}` DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci ";
    }
}
