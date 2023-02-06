using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BackupSystemTool
{
    public class Cryptograpy
    {
        // return a ciphertext of the inputted plaintext using AES 265.
        public string encryptStringAES(string plainText, string key)
        {
            byte[] encrypted;
            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                // Initialization vector, setted to random 16 bytes
                aes.IV = new byte[16];
                encrypted = encryptStringToBytesAes(plainText, aes.Key, aes.IV);
            }
            return Convert.ToBase64String(encrypted);
        }

        // return an encrypted array of bytes using a combination of memory stream, crypto stream and stream writer.
        private static byte[] encryptStringToBytesAes(string plainText, byte[] key, byte[] iv)
        {
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (key == null || key.Length <= 0)
                throw new ArgumentNullException("key");
            if (iv == null || iv.Length <= 0)
                throw new ArgumentNullException("iv");

            byte[] encrypted;
            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;

                // Initializing a new MemoryStream object with the plaintext as a byte array
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    // Creating a new CryptoStream object with the memory stream and the encryption algorithm
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        // Writing the plaintext to the crypto stream using a StreamWriter object
                        using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                        {
                            streamWriter.Write(plainText);
                        }
                        // Getting the encrypted data from the memory stream as a byte array
                        encrypted = memoryStream.ToArray();
                    }
                }

            }
            // return the encrypted array of bytes
            return encrypted;
        }

        // returns the hashed result of the inputted text.
        public string hashText(string text)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedKey = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(text));
                //The "-" characters are used as separators when converting a byte array to a string representation using the BitConverter.ToString method.
                //The "-" characters are not necessary for the purpose of storing or using the hashed key as a cryptographic key,
                //and so they are often removed to make the resulting string more compact and usable as a key.
                return BitConverter.ToString(hashedKey).Replace("-", "");
            }
        }
    }
}
