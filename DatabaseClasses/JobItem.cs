using SQLite;

namespace BackupSystemTool.DatabaseClasses
{
    public class JobItem
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public string job_name { get; set; }
        public string connection_name { get; set; }
        public int connection_id { get; set; }
        public string location_type { get; set; }
    }
}
