using System;
using System.Security.Cryptography;
using System.Management;
using Microsoft.Win32;
using System.Diagnostics;

namespace BackupSystemTool
{
    public class KeyGenerator
    {
        private const string KeyName = "BackupSystemTool";
        public string SubKeyName { get; set; }
        private string salt;
        public KeyGenerator(string SubKeyName, string salt) { 
            this.SubKeyName = SubKeyName;
            this.salt = salt;
        }

        public string getKey()
        {   
            // Key exists, get the value
            var key = Registry.CurrentUser.OpenSubKey(KeyName);
            return key.GetValue(SubKeyName).ToString();
        }
        public string setKey() 
        {
            // cryptography class used for security operations
            Cryptograpy cryptograpy = new Cryptograpy();    

            // Combine the CPU ID and MAC address to create the key
            var saltedKey = getCPUId() + getMacAddress() + this.salt;

            // Hash the key to get a fixed-length value
            var hashedKey = cryptograpy.hashText(salt);
            hashedKey = hashedKey.Substring(0, 32);
            // Key doesn't exist, generate a new key
            var newKey = Registry.CurrentUser.CreateSubKey(KeyName);
            newKey.SetValue(this.SubKeyName, hashedKey);
            return getKey();
        }
        private string getCPUId()
        {
            var cpuId = "";
            var mc = new ManagementClass("win32_processor");
            var moc = mc.GetInstances();

            foreach (var mo in moc)
            {
                cpuId = mo.Properties["processorID"].Value.ToString();
                break;
            }

            return cpuId;
        }
        private string getMacAddress()
        {
            var macAddress = "";
            var mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            var moc = mc.GetInstances();

            foreach (var mo in moc)
            {
                if ((bool)mo["IPEnabled"])
                {
                    macAddress = mo["MacAddress"].ToString();
                    break;
                }
            }

            return macAddress;
        }
   
        
    }
}
