using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

/// File method need to be in seperate file !!!
using DamirM.CommonLibrary;

namespace DamirM.CommonLibrary
{
    public class Common
    {
        public static bool MakeAllSubFolders(string folderPath)
        {
            bool result = true;
            string buildPath = "";
            string[] pathSplits = folderPath.Split(new char[] { '\\' });
            try
            {
                foreach (string splitPath in pathSplits)
                {
                    buildPath += splitPath + @"\";
                    // If directory not exists, then make it
                    if (!Directory.Exists(buildPath))
                    {
                        Directory.CreateDirectory(buildPath);
                    }
                }
            }
            catch(Exception ex)
            {
                Log.Write(buildPath, typeof(Common), "MakeAllSubFolders", Log.LogType.DEBUG);
                Log.Write(ex, typeof(Common), "MakeAllSubFolders", Log.LogType.ERROR);
                result = false;
            }
            return result;  
        }
        public static string BuildPath(params string[] paths)
        {
            string path = "";
            string fullPath = "";
            foreach (string pathPart in paths)
            {
                // if pathPart start with \ then cut it
                if (pathPart.StartsWith(@"\"))
                {
                    path = pathPart.Substring(1, pathPart.Length - 1);
                }
                else
                {
                    path = pathPart;
                }
                fullPath += SetSlashOnEndOfDirectory(path); ;
            }
            return fullPath;
        }
        public static string ExtractFolderFromPath(string path)
        {
            int indexOf = path.LastIndexOf('\\');
            string result = path;
            if (indexOf > 0)
            {
                result = path.Substring(0, indexOf);
            }

            return result;
        }
        public static string ExtractFileFromPath(string path)
        {
            string[] pathArray = path.Split(new char[] { '\\' });
            if (pathArray.Length > 0)
            {
                return pathArray[pathArray.Length - 1];
            }
            else
            {
                Log.Write("No file name to extract", typeof(Common), "ExtractFileFromPath", Log.LogType.ERROR);
                return "";
            }
        }
        public static string SetSlashOnEndOfDirectory(string path)
        {
            path = path.Trim();
            if (!path.EndsWith("\\") && !path.Equals(""))
            {
                return path + "\\";
            }
            else
            {
                return path;
            }
        }

        /// <summary>
        /// Marge two string array in one, if one of array is null then just copy
        /// </summary>
        /// <param name="stringArray1"></param>
        /// <param name="stringArray2"></param>
        /// <returns></returns>
        public static string[] MargeTwoStringArray(string[] stringArray1, string[] stringArray2)
        {
            string[] buff = null;
            if (stringArray1 != null)
            {
                if (stringArray2 != null)
                {
                    buff = new string[stringArray1.Length + stringArray2.Length];

                    stringArray1.CopyTo(buff, 0);
                    stringArray2.CopyTo(buff, stringArray1.Length);
                }
                else
                {
                    buff = new string[stringArray1.Length];

                    stringArray1.CopyTo(buff, 0);
                }
            }
            else
            {
                if (stringArray2 != null)
                {
                    buff = new string[stringArray2.Length];

                    stringArray2.CopyTo(buff, 0);
                }
            }
            return buff;
        }

        public string GetMD5Hash(string text)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider x = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] bs = System.Text.Encoding.UTF8.GetBytes(text);
            bs = x.ComputeHash(bs);
            System.Text.StringBuilder s = new System.Text.StringBuilder();
            foreach (byte b in bs)
            {
                s.Append(b.ToString("x2").ToLower());
            }
            string password = s.ToString();
            return password;
        }


    }
}
