using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BackupSystemTool
{
    public class Cryptograpy
    {
        public string EncryptStringAES(string plainText, string key, byte[] iv)
        {
            byte[] encrypted;

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(cs))
                        {
                            sw.Write(plainText);
                        }
                        encrypted = ms.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(encrypted);
        }

        public string DecryptStringAES(string cipherText, string key, byte[] iv)
        {
            string decrypted;

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;

                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(cipherText)))
                {
                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader sr = new StreamReader(cs))
                        {
                            decrypted = sr.ReadToEnd();
                        }
                    }
                }
            }

            return decrypted;
        }


        // returns the hashed result of the inputted text.
        public string hashText(string text)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedKey = sha256.ComputeHash(Encoding.UTF8.GetBytes(text));
                //The "-" characters are used as separators when converting a byte array to a string representation using the BitConverter.ToString method.
                //The "-" characters are not necessary for the purpose of storing or using the hashed key as a cryptographic key,
                //and so they are often removed to make the resulting string more compact and usable as a key.
                return BitConverter.ToString(hashedKey).Replace("-", "");
            }
        }

        // generate a random number or bytes (16 length is specified) to be used as salt
        public string generateSalt()
        {
            byte[] salt = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return Convert.ToBase64String(salt);
        }

        public byte[] GenerateSecureIV()
        {
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                byte[] iv = new byte[16]; // 16 bytes for a 128-bit IV
                rng.GetBytes(iv);
                return iv;
            }
        }

    }
}
