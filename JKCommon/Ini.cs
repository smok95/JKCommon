using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.IO;

namespace JKCommon
{
    public class Ini
    {
#region Properties
        public DateTime LastWriteTime { get { return System.IO.File.GetLastWriteTime(this.Filename); } }
        public string Filename { get; private set; }
        /// <summary>
        /// ini파일 존재여부
        /// 150423 kim,jk
        /// </summary>
        public bool FileExists
        {
            get
            {
                return System.IO.File.Exists(Filename);
            }
        }
#endregion
        

        public Ini(string file)
        {
            Filename = file;
        }

        public string[] GetAllSections()
        {
            List<string> list = new List<string>();
            byte[] buffer = new byte[65536];
            Win32.GetPrivateProfileSectionNames(buffer, buffer.Length, Filename);
            string allSections = System.Text.Encoding.Default.GetString(buffer);
            string[] sectionNames = allSections.Split('\0');
            foreach (string sectionName in sectionNames)
            {
                if (sectionName != string.Empty)
                    list.Add(sectionName);
            }
            return list.ToArray();
        }

        /// <summary>
        /// Section내의 모든 키이름을 가져온다.
        /// </summary>
        /// <param name="section"></param>
        /// <returns></returns>
        public string[] GetAllKeyNames(string section)
        {
            List<string> list = new List<string>();
            byte[] buf = new byte[65536];
            Win32.GetPrivateProfileSection(section, buf, buf.Length, Filename);
            string allKeys = System.Text.Encoding.Default.GetString(buf);
            string[] keys = allKeys.Split('\0');
            foreach (string key in keys)
            {
                string[] kv = key.Split('=');
                if(kv[0] != string.Empty)
                    list.Add(kv[0]);
            }
            return list.ToArray();
        }

        /// <summary>
        /// Section내의 모든 키를 가져온다.(value도 포함)
        /// </summary>
        /// <param name="section"></param>
        /// <returns></returns>
        public KeyValuePair<string, string>[] GetAllKeys(string section)
        {
            List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
            byte[] buf = new byte[65536];
            Win32.GetPrivateProfileSection(section, buf, buf.Length, Filename);
            string allKeys = System.Text.Encoding.Default.GetString(buf);
            string[] keys = allKeys.Split('\0');
            foreach (string key in keys)
            {
                string[] kv = key.Split('=');
                // 14.11.11 kjk kv배열크기 검사.. 
                if (kv.Length==2 && kv[0] != string.Empty)
                {   
                    list.Add(new KeyValuePair<string,string>(kv[0], kv[1]));
                }
            }
            return list.ToArray();
        }

        public bool DeleteSection(string section)
        {
            //return Win32.WritePrivateProfileString(section, null, null, Filename);
            return Win32.WritePrivateProfileSection(section, null, Filename);
        }

        public bool DeleteKey(string section, string key)
        {
            return Win32.WritePrivateProfileString(section, key, null, Filename);
        }

        /// <summary>
        /// 값 읽기
        /// </summary>
        /// <param name="section">섹션명</param>
        /// <param name="key">키이름</param>
        /// <param name="defVal">기본값</param>
        /// <returns>읽은 값</returns>
        public string Read(string section, string key, string defVal="")
        {
            StringBuilder ret = new StringBuilder(260);
            Win32.GetPrivateProfileString(section, key, defVal, ret, (uint)ret.Capacity, Filename);
            return ret.ToString();
        }

        public bool Write(string section, string key, string value)
        {
            return Win32.WritePrivateProfileString(section, key, value, Filename);
        }

        public bool Write(string section, string key, int value)
        {
            return Write(section, key, value.ToString());
        }

        /// <summary>
        /// section 존재여부 확인
        /// </summary>
        /// <param name="section">확인할 section 명</param>
        /// <returns>있으면 true</returns>
        public bool IsExistsSection(string section)
        {
            StringBuilder ret = new StringBuilder(260);
            uint r = Win32.GetPrivateProfileString(section, null, String.Empty, ret, (uint)ret.Capacity, Filename);
            return r > 0;
        }
    }
}
