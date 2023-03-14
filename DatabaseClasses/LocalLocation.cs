using SQLite;

namespace BackupSystemTool.DatabaseClasses
{
    public class LocalLocation
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public int job_id { get; set; }
        public string local_path { get; set; }
    }
}
