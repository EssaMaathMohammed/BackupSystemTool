using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackupSystemTool.DatabaseClasses
{
    // BackupInfo class for the backupInfo table
    public class BackupInfo
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string BackupName { get; set; }
        public string EncryptedDEK { get; set; }
        public string dekIV { get; set; }
        public string dataIV { get; set; }

    }
}
