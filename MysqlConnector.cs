using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackupSystemTool
{
    internal class MysqlConnector
    {
        public List<string> GetDatabases(string connectionString) {

            List<string> databases = new List<string>();

            //connectionString = "Server=localhost;User Id=root;Password=;";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("SHOW DATABASES", connection);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        databases.Add(reader.GetString(0));
                    }
                }
                connection.Close();
            }
           return databases;
        }
        public List<string> GetDatabases(string server, string userID, string password)
        {

            List<string> databases = new List<string>();

            string connectionString = $"Server={server} ;User Id={userID};Password={password};";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("SHOW DATABASES", connection);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        databases.Add(reader.GetString(0));
                    }
                }
                connection.Close();
            }
            return databases;
        }
    }
}
