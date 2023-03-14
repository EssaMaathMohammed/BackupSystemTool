using SQLite;

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
        public string database { get; set; }
        public string schema { get; set; }
        public string warehouse { get; set; }
    }
}
