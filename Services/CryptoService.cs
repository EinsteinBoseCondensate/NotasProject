using NotasProject.Models.Config;
using NotasProject.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using NotasProject.Properties;

namespace NotasProject.Services
{
    public static class CryptoService
    {
        private static string AES_KEY = ConfigurationManager.AppSettings.Get(Resources.AES_KEY);
        public static string Hash(string chars)
        {
            byte[] salt = Encoding.UTF8.GetBytes(ConfigurationManager.AppSettings.Get("HASH_SALT"));
            using (var hmac = new HMACSHA512(salt))
            {
                return Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(chars)));
            }
        }

        public static string AES_Encrypt(string chars, byte[] nonce)
        {
            using (AesManaged aes = new AesManaged())
            {
                byte[] AES_KEY_BYTE = ArrayHelper.ConvertHexStringToByteArray(AES_KEY);
                ICryptoTransform encryptor = aes.CreateEncryptor(AES_KEY_BYTE, nonce);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {    
                        using (StreamWriter sw = new StreamWriter(cs))
                            sw.Write(chars);
                        return Convert.ToBase64String(ms.ToArray());
                    }
                }
            }
        }

        public static string AES_Decrypt(string strg, byte[] nonce)
        {
            byte[] cypherText = Convert.FromBase64String(strg);
            byte[] AES_KEY_BYTE = ArrayHelper.ConvertHexStringToByteArray(AES_KEY);
            using (AesManaged aes = new AesManaged())
            {
                ICryptoTransform decryptor = aes.CreateDecryptor(AES_KEY_BYTE, nonce);
                using (MemoryStream ms = new MemoryStream(cypherText))
                {
                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        // Create StreamWriter and write data to a stream    
                        using (StreamReader sr = new StreamReader(cs))
                            return sr.ReadToEnd();
                    }
                }
            }
        }

        /// <summary>
        /// Acepta un nonce concreto en Base64
        /// </summary>
        /// <param name="strg"></param>
        /// <param name="nonce"></param>
        /// <returns></returns>
        public static string AES_Decrypt(string strg, string nonce)
        {
            byte[] bitnonce = Convert.FromBase64String(nonce);
            byte[] cypherText = Convert.FromBase64String(strg);
            byte[] AES_KEY_BYTE = ArrayHelper.ConvertHexStringToByteArray(AES_KEY);
            using (AesManaged aes = new AesManaged())
            {
                ICryptoTransform decryptor = aes.CreateDecryptor(AES_KEY_BYTE, bitnonce);
                using (MemoryStream ms = new MemoryStream(cypherText))
                {
                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        // Create StreamWriter and write data to a stream    
                        using (StreamReader sr = new StreamReader(cs))
                            return sr.ReadToEnd();
                    }
                }
            }
        }
        public static AES_Result AES_Encrypt(string chars)
        {
            byte[] nonce = SetUnique16BitAES_IV();
            using (AesManaged aes = new AesManaged())
            {
                byte[] AES_KEY_BYTE = ArrayHelper.ConvertHexStringToByteArray(AES_KEY);
                ICryptoTransform encryptor = aes.CreateEncryptor(AES_KEY_BYTE, nonce);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(cs))
                            sw.Write(chars);
                        return new AES_Result() { CipherData = Convert.ToBase64String(ms.ToArray()), Nonce = Convert.ToBase64String(nonce)};
                    }
                }
            }
        }
        public static byte[] SetUnique16BitAES_IV()
        {
            byte[] preVnonce = ArrayHelper.ConvertHexStringToByteArray((DateTime.Now.Ticks * (15)).ToString("X"));
            byte[] nonce = new byte[16];
            Array.Copy(ArrayHelper.Concat<byte>(preVnonce.Reverse().ToArray(), preVnonce), nonce, 16);
            return nonce;
        }


    }
}