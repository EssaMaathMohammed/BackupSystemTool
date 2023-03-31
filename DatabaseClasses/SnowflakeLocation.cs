using SQLite;
using System.Text;

namespace BackupSystemTool.DatabaseClasses
{
    public class SnowflakeLocation
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public int job_id { get; set; }
        public string account { get; set; }
        public string user { get; set; }
        public string password { get; set; }
        public string role { get; set; }
        public string database { get; set; }
        public string schema { get; set; }
        public string warehouse { get; set; }

        public string GetConnectionString()
        {
            StringBuilder connectionStringBuilder = new StringBuilder();
            connectionStringBuilder.AppendFormat("account={0};", account);
            connectionStringBuilder.AppendFormat("user={0};", user);
            connectionStringBuilder.AppendFormat("password={0};", password);
            connectionStringBuilder.AppendFormat("role={0};", role);
            connectionStringBuilder.AppendFormat("db={0};", database);
            connectionStringBuilder.AppendFormat("schema={0};", schema);

            return connectionStringBuilder.ToString();
        }
    }
}
