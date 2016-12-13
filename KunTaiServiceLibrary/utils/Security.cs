using System;
using System.Security.Cryptography;
using System.Text;
using FluorineFx;
using System.IO;

namespace KunTaiServiceLibrary
{
    [RemotingService]
    public class Security
    {
        private MD5 md5Hash = MD5.Create();

        public string getMd5Hash(string input)
        {
            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("X2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        // Verify a hash against a string.
        public bool verifyMd5Hash(string input, string hash)
        {
            // Hash the input.
            string hashOfInput = getMd5Hash(input);

            // Create a StringComparer an compare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            if (0 == comparer.Compare(hashOfInput, hash))
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="data">要加密的文本</param>
        /// <param name="key">密钥, 为8位的字符串</param>
        /// <param name="iv">向量</param>
        /// <returns>返回加密后的文本</returns>
        public string encode(string data, string key = "imqw.org", string iv = "WarriorSun")
        {
            string KEY_64 = key;// "VavicApp";
            string IV_64 = iv;// "VavicApp";

            try
            {
                byte[] byKey = ASCIIEncoding.ASCII.GetBytes(KEY_64);
                byte[] byIV = ASCIIEncoding.ASCII.GetBytes(IV_64);
                DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
                int i = cryptoProvider.KeySize;
                MemoryStream ms = new MemoryStream();
                CryptoStream cst = new CryptoStream(ms, cryptoProvider.CreateEncryptor(byKey, byIV), CryptoStreamMode.Write);
                StreamWriter sw = new StreamWriter(cst);
                sw.Write(data);
                sw.Flush();
                cst.FlushFinalBlock();
                sw.Flush();

                return Convert.ToBase64String(ms.GetBuffer(), 0, (int)ms.Length);
            }
            catch (Exception x)
            {
                return x.Message;
            }
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="data">已加密的文本</param>
        /// <param name="key">密钥, 为8位的字符串</param>
        /// <param name="iv">向量</param>
        /// <returns>返回解密的文本</returns>
        public string decode(string data, string key = "imqw.org", string iv = "WarriorSun")
        {
            string KEY_64 = key;// "VavicApp";密钥
            string IV_64 = iv;// "VavicApp"; 向量

            try
            {
                byte[] byKey = ASCIIEncoding.ASCII.GetBytes(KEY_64);
                byte[] byIV = ASCIIEncoding.ASCII.GetBytes(IV_64);
                byte[] byEnc;
                byEnc = Convert.FromBase64String(data); //把需要解密的字符串转为8位无符号数组
                DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
                MemoryStream ms = new MemoryStream(byEnc);
                CryptoStream cst = new CryptoStream(ms, cryptoProvider.CreateDecryptor(byKey, byIV), CryptoStreamMode.Read);
                StreamReader sr = new StreamReader(cst);

                return sr.ReadToEnd();
            }
            catch (Exception x)
            {
                return x.Message;
            }
        }



    }
}
