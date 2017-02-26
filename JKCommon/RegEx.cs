using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;

namespace JKCommon
{
    public static class RegistryExtension
    {
        /// <summary>
        /// Registry Key Name 변경
        /// </summary>
        /// <param name="src">이름을 변경할 키</param>
        /// <param name="newName">새로운 키 이름</param>
        /// <returns>이름이 변경된 키</returns>
        public static RegistryKey Rename(this RegistryKey src, string newName)
        {
            string newPath = src.ToString();
            string oldPath = "";
            string[] s = newPath.Split('\\');
            newPath = "";
            string rootName = s[0];
            for (int i = 1; i < s.Length - 1; i++)
            {
                newPath += s[i] + "\\";
            }
            oldPath = newPath + s[s.Length - 1];
            newPath += newName;
            RegistryKey rootKey = null;

            if (rootName == Registry.ClassesRoot.ToString())
                rootKey = Registry.ClassesRoot;
            else if (rootName == Registry.CurrentConfig.ToString())
                rootKey = Registry.CurrentConfig;
            else if (rootName == Registry.CurrentUser.ToString())
                rootKey = Registry.CurrentUser;
            else if (rootName == Registry.LocalMachine.ToString())
                rootKey = Registry.LocalMachine;
            else if (rootName == Registry.PerformanceData.ToString())
                rootKey = Registry.PerformanceData;
            else if (rootName == Registry.Users.ToString())
                rootKey = Registry.Users;
            if (rootKey == null) return src;

            RegistryKey k = rootKey.CreateSubKey(newPath);
            string srcName = src.ToString();
            src.CopyTo(k);
            src.Close();
            rootKey.DeleteSubKeyTree(oldPath);
            return k;
        }
        public static void CopyTo(this RegistryKey src, RegistryKey dst)
        {
            // copy the values
            foreach (string name in src.GetValueNames())
            {
                dst.SetValue(name, src.GetValue(name), src.GetValueKind(name));
            }

            // copy the subkeys
            foreach (string name in src.GetSubKeyNames())
            {
                using (RegistryKey srcSubKey = src.OpenSubKey(name, false))
                {
                    RegistryKey dstSubKey = dst.CreateSubKey(name);
                    srcSubKey.CopyTo(dstSubKey);
                }
            }
        }
    }
}
