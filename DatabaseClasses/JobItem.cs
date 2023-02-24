using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackupSystemTool.DatabaseClasses
{
    internal class JobItem
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }

        public int connection_id { get; set; }

        public string location { get; set; }

        [ForeignKey("connection_id")]
        public ConnectionItem Connection { get; set; }
    }
}
