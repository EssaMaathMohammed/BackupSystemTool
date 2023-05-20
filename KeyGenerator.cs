using System;
using System.Security.Cryptography;
using System.Management;
using Microsoft.Win32;
using System.Diagnostics;
using System.Windows.Forms;
using System.Text;

namespace BackupSystemTool
{
    public class KeyGenerator
    {

        private const string KeyName = "BackupSystemTool";
        private string SubKeyName { get; set; }
        private string SubKekName { get; set; }

        private string salt;
        public KeyGenerator(string SubKeyName, string salt) { 
            this.SubKeyName = "subkey"+SubKeyName;
            this.SubKekName = "subkek"+ SubKeyName;
            this.salt = salt;
        }
        public KeyGenerator(string SubKeyName)
        {
            this.SubKeyName = "subkey" + SubKeyName;
            this.SubKekName = "subkek" + SubKeyName;
            // creates a random salt
            Cryptograpy cryptograpy = new Cryptograpy();
            this.salt = cryptograpy.generateSalt();
        }

        public void setUserKeyIVReg()
        {
            if (this.salt != null)
            {
                // cryptography class used for security operations
                Cryptograpy cryptograpy = new Cryptograpy();

                // Combine the CPU ID and MAC address to create the key
                var saltedKey = getCPUId() + getMacAddress() + this.salt;

                // Hash the key to get a fixed-length value
                var hashedKey = cryptograpy.hashText(saltedKey);
                hashedKey = hashedKey.Substring(0, 32);

                // Generate a random IV, then cast it to string to be stored in registry 
                byte[] iv = cryptograpy.GenerateSecureIV();
                string ivString = Convert.ToBase64String(iv);

                // Key doesn't exist, generate a new key
                var newKey = Registry.CurrentUser.CreateSubKey(KeyName);
                newKey.SetValue(this.SubKeyName, hashedKey);
                newKey.SetValue(this.SubKeyName + "_IV", ivString); // Store the IV alongside the key in the registry
            }
            else {
                MessageBox.Show("Please Provide a salt to be used in the key");
            }
        }
        // gets the key
        public string getUserKeyReg()
        {   
            // Key exists, get the value
            var key = Registry.CurrentUser.OpenSubKey(KeyName);
            return key.GetValue(SubKeyName).ToString();
        }
        // gets the IV 
        public string getUserIVReg()
        {
            // Get the IV from the registry
            var key = Registry.CurrentUser.OpenSubKey(KeyName);
            return key.GetValue(SubKeyName + "_IV").ToString();
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

        // generates a new DEK
        public string CreateDataEncryptionKey(string uniqueFileName)
        {
            if (this.salt != null)
            {
                // cryptography class used for security operations
                Cryptograpy cryptograpy = new Cryptograpy();

                // Combine the CPU ID, MAC address, backup ID, salt and userkey to create the DEK
                var saltedKey = getCPUId() + getMacAddress() + uniqueFileName + getUserKeyReg() + this.salt;

                // Hash the key to get a fixed-length value
                var hashedKey = cryptograpy.hashText(saltedKey);
                return hashedKey.Substring(0, 32);
            }
            else
            {
                MessageBox.Show("Please Provide a salt to be used in the DEK");
                return "0";
            }
        }

        // sets the kek in registry
        public void SetUserKeyEncryptionKeyReg()
        {
            if (this.salt != null)
            {
                // cryptography class used for security operations
                Cryptograpy cryptograpy = new Cryptograpy();

                // Use a fixed salt value for generating the KEK
                string kekSalt = this.salt;

                // Combine the user key and the fixed salt to create the KEK
                var saltedKey = getUserKeyReg() + kekSalt;

                // Hash the key to get a fixed-length value
                var hashedKey = cryptograpy.hashText(saltedKey);
                hashedKey = hashedKey.Substring(0, 32);

                // Key doesn't exist, generate a new key
                var newKey = Registry.CurrentUser.CreateSubKey(SubKekName);
                newKey.SetValue(this.SubKekName, hashedKey);
            }
            else
            {
                MessageBox.Show("Please Provide a salt to be used in the kek");
            }
        }

        public string GetUserKeyEncryptionKeyReg() {
            // Key exists, get the value
            var key = Registry.CurrentUser.OpenSubKey(SubKekName);
            return key.GetValue(SubKekName).ToString();
        }

    }
}
