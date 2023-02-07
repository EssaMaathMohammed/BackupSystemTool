using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackupSystemTool.DatabaseClasses
{
    internal class UserItem
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public string ciphertext { get; set; }
        public string salt { get; set; }
    }
}
