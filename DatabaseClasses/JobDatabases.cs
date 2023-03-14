using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackupSystemTool.DatabaseClasses
{
    internal class JobDatabases
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public int job_id { get; set; }
        public string database_name { get; set; }
    }
}
