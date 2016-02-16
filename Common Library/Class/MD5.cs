using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace DamirM.CommonLibrary
{
    public class MD5
    {

        public static string MD5FromText(string input)
        {
            // step 1, calculate MD5 hash from input
            System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }

        public static string MD5FromFile(string sFilePath)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider md5Provider
            = new System.Security.Cryptography.MD5CryptoServiceProvider();
            FileStream fs
            = new FileStream(sFilePath, FileMode.Open, FileAccess.Read);
            Byte[] hashCode
            = md5Provider.ComputeHash(fs);

            string ret = "";

            foreach (byte a in hashCode)
            {
                if (a < 16)
                    ret += "0" + a.ToString("x");
                else
                    ret += a.ToString("x");
            }

            fs.Close();
            return ret;
        }
    }
}
