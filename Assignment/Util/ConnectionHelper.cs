using System.Data;
using MySql.Data.MySqlClient;

namespace Assignment.Util
{
    public class ConnectionHelper
    {
        private static MySqlConnection _connection;
            private const string Server = "127.0.0.1";
            private const string Username = "root";
            private const string Password = "";
            private const string Database = "t2012e_connect";
            private const string ConnectionString = "server={0};uid={1};pwd={2};database={3};SslMode=none;convert zero datetime=True";

            public static MySqlConnection GetConnect()
            {
                if (_connection == null || _connection.State == ConnectionState.Closed)
                {
                    _connection = new MySqlConnection(
                        string.Format(ConnectionString,Server,Username,Password,Database));    
                }
                return _connection;
            }
    }
}