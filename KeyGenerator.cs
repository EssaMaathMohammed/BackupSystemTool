using System;
using System.Security.Cryptography;
using System.Management;
using Microsoft.Win32;
using System.Diagnostics;

namespace BackupSystemTool
{
    public class KeyGenerator
    {
        const string KeyName = "BackupSystemTool";
        const string SubKeyName = "EncryptionKey";

        // checks whether a key exists in the registry
        // if the key exists the getKey method will be called and the key will be returned
        // if the key doenst exist a new key will be generated and save, then the getKey 
        // method will be called
        public string evaluateKey() {
            if (Registry.CurrentUser.OpenSubKey(KeyName) == null)
            {
                return setKey();
            }
            return getKey();
        }
        private string getKey()
        {   
            // Key exists, get the value
            var key = Registry.CurrentUser.OpenSubKey(KeyName);
            return key.GetValue(SubKeyName).ToString();
        }
        private string setKey() 
        {
            // Combine the CPU ID and MAC address to create the key
            var key = getCPUId() + getMacAddress() + generateSalt();

            // Hash the key to get a fixed-length value
            var hashedKey = hashKey(key);

            // Key doesn't exist, generate a new key
            var newKey = Registry.CurrentUser.CreateSubKey(KeyName);
            newKey.SetValue(SubKeyName, hashedKey);
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
        private string hashKey(string key)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedKey = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(key));
                //The "-" characters are used as separators when converting a byte array to a string representation using the BitConverter.ToString method.
                //The "-" characters are not necessary for the purpose of storing or using the hashed key as a cryptographic key,
                //and so they are often removed to make the resulting string more compact and usable as a key.
                return BitConverter.ToString(hashedKey).Replace("-", "");
            }
        }

        // generate a random number or bytes (16 length is specified) to be used as salt
        private string generateSalt()
        {
            byte[] salt = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return Convert.ToBase64String(salt);
        }
    }
}
