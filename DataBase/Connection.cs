// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.Data.Common;

namespace Common_Library.DataBase
{
    public enum SSLMode
    {
        None
    }
    
    public enum CharSet
    {
        Utf8
    }
    
    public class Connection
    {
        public static implicit operator String(Connection connection)
        {
            return connection.ToString();
        }
        
        public String Host { get; set; }
        public UInt16 Port { get; set; }
        public String User { get; set; }
        public String Password { get; set; }
        public String Database { get; set; }
        public Boolean TrustedConnection { get; set; } = true;
        public UInt16 Timeout { get; set; } = 60;
        public UInt16 CommandTimeout { get; set; } = 180;
        public UInt16 Lifetime { get; set; } = 600;
        public CharSet CharSet { get; set; } = CharSet.Utf8;
        public Boolean Pooling { get; set; } = false;
        public Int32 MinPoolSize { get; set; } = 0;
        public Int32 MaxPoolSize { get; set; } = 10;
        public Boolean AllowZeroDatetime { get; set; } = true;
        public Boolean ConvertZeroDatetime { get; set; } = true;
        public SSLMode SSLMode { get; set; } = SSLMode.None;

        public Connection(String host, UInt16 port, String user, String password, String database)
        {
            Host = host;
            Port = port;
            User = user;
            Password = password;
            Database = database;
        }

        public DbConnection CreateConnection()
        {
            return DataBase.CreateConnection(this);
        }
        
        public override String ToString()
        {
            return $"Server={Host},{Port};User={User};Password={Password};Database={Database};Pooling={Pooling};Min Pool Size={MinPoolSize};Max Pool Size={MaxPoolSize};Connection Timeout={Timeout};Connection Lifetime={Lifetime}";
        }
    }
}