using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackupSystemTool.DatabaseClasses
{
    internal class BackupSchedule
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public int job_id { get; set; }
        public string DatabaseName { get; set; }
        public int Interval { get; set; }
        public string IntervalType { get; set; }
    }
}
