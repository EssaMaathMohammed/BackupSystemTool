using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BackupSystemTool.DatabaseClasses
{
    public class S3Item
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int JobId { get; set; }
        public string AccessKeyId { get; set; }
        public string SecretAccessKey { get; set; }
        public string BucketName { get; set; }
        public string Region { get; set; }
    }
}
