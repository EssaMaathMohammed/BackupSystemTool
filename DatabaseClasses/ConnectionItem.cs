using Microsoft.VisualBasic.ApplicationServices;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BackupSystemTool.DatabaseClasses
{
    public class ConnectionItem
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [MaxLength(50)]

        public int UserId { get; set; }

        public string ConnectionName { get; set; }

        [MaxLength(50)]
        public string Username { get; set; }

        [MaxLength(50)]
        public string Password { get; set; }

        [MaxLength(50)]
        public string ServerName { get; set; }

        public string PortNumber { get; set; }

        [Ignore]
        public string ConnectionString
        {
            get
            {
                return $"Server={ServerName} ;User Id={Username};Password={Password};";
            }
        }
    }


}
