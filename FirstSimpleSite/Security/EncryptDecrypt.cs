using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.IO;

namespace FirstSimpleSite.Security
{
    public static class EncryptDecrypt
    {

        public static string Encrypt(string text)
        {
            byte[] clearData =System.Text.Encoding.Unicode.GetBytes(text);
            PasswordDeriveBytes pdb = new PasswordDeriveBytes("simplePassword",
                new byte[] {0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 
                             0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76});
     
            MemoryStream ms = new MemoryStream();
            Rijndael alg = Rijndael.Create();
            alg.Key = pdb.GetBytes(32);
            alg.IV = pdb.GetBytes(16);
            using (CryptoStream cs = new CryptoStream(ms, alg.CreateEncryptor(), CryptoStreamMode.Write))
            {
                cs.Write(clearData, 0, clearData.Length);
            }
            byte[] encryptedData = ms.ToArray();

            return Convert.ToBase64String(encryptedData);
        }

        public static string Decrypt(string encText)
        {
            byte[] data = Convert.FromBase64String(encText);
            PasswordDeriveBytes pdb = new PasswordDeriveBytes("simplePassword",
                new byte[] {0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d,
                             0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76});

            MemoryStream ms = new MemoryStream();
            Rijndael alg = Rijndael.Create();
            alg.Key = pdb.GetBytes(32);
            alg.IV = pdb.GetBytes(16);
            using (CryptoStream cs = new CryptoStream(ms, alg.CreateDecryptor(), CryptoStreamMode.Write))
            {
                cs.Write(data, 0, data.Length);
            }
            byte[] decryptedData = ms.ToArray(); 

            return System.Text.Encoding.Unicode.GetString(decryptedData);
        }

    }
}