// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Data.Common;
using Microsoft.Data.Sqlite;
using MySql.Data.MySqlClient;

namespace Common_Library.DataBase
{
    public enum DbConnectionType
    {
        MySQL,
        SQLite
    }
    
    public static class DataBase
    {
        public static DbConnection Create(String connection, DbConnectionType type = DbConnectionType.MySQL)
        {
            DbConnection cn = type switch
            {
                DbConnectionType.MySQL => new MySqlConnection(connection),
                DbConnectionType.SQLite => new SqliteConnection(connection),
                _ => throw new ArgumentException(nameof(type))
            };

            try
            {
                cn.Open();
            }
            catch (Exception)
            {
                return null;
            }

            return cn;
        }
        
        public static void Close(DbConnection connection)
        {
            connection.Close();
        }

        public static Object GetValue(this MySqlDataReader reader, String column)
        {
            return reader.GetValue(reader.GetOrdinal(column));
        }
    }
}