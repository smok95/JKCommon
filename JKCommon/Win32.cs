/*
Copyright 2017, Kim Jongkook d0nzs00n@gmail.com

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and 
associated documentation files (the "Software"), to deal in the Software without restriction, 
including without limitation the rights to use, copy, modify, merge, publish, distribute, 
sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is 
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or 
substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT 
NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND 
NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, 
DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace JKCommon
{
    public partial class Win32
    {
        [DllImport("psapi.dll")]
        public static extern int EmptyWorkingSet(IntPtr hwProc);


        [DllImport("advapi32.dll")]
        static extern bool GetUserName(StringBuilder sb, ref Int32 len);
        public static string GetUserName()
        {
            StringBuilder sb = new StringBuilder(512);
            int nSize = 512;
            GetUserName(sb, ref nSize);
            return sb.ToString();
        }

        public enum ExtendedNameFormat
        {
            /// <summary>
            /// An unknown name type.
            /// </summary>
            NameUnknown = 0,

            /// <summary>
            /// The fully qualified distinguished name
            /// (for example, CN=Jeff Smith,OU=Users,DC=Engineering,DC=Microsoft,DC=Com).
            /// </summary>
            NameFullyQualifiedDN = 1,

            /// <summary>
            /// A legacy account name (for example, Engineering\JSmith).
            /// The domain-only version includes trailing backslashes (\\).
            /// </summary>
            NameSamCompatible = 2,

            /// <summary>
            /// A "friendly" display name (for example, Jeff Smith).
            /// The display name is not necessarily the defining relative distinguished name (RDN).
            /// </summary>
            NameDisplay = 3,

            /// <summary>
            /// A GUID string that the IIDFromString function returns
            /// (for example, {4fa050f0-f561-11cf-bdd9-00aa003a77b6}).
            /// </summary>
            NameUniqueId = 6,

            /// <summary>
            /// The complete canonical name (for example, engineering.microsoft.com/software/someone).
            /// The domain-only version includes a trailing forward slash (/).
            /// </summary>
            NameCanonical = 7,

            /// <summary>
            /// The user principal name (for example, someone@example.com).
            /// </summary>
            NameUserPrincipal = 8,

            /// <summary>
            /// The same as NameCanonical except that the rightmost forward slash (/)
            /// is replaced with a new line character (\n), even in a domain-only case
            /// (for example, engineering.microsoft.com/software\nJSmith).
            /// </summary>
            NameCanonicalEx = 9,

            /// <summary>
            /// The generalized service principal name
            /// (for example, www/www.microsoft.com@microsoft.com).
            /// </summary>
            NameServicePrincipal = 10,

            /// <summary>
            /// The DNS domain name followed by a backward-slash and the SAM user name.
            /// </summary>
            NameDnsDomain = 12
        }

        [DllImport("secur32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern int GetUserNameEx(ExtendedNameFormat nameFormat, StringBuilder userName, ref int userNameSize);
        public static int GetUserNameEx(ExtendedNameFormat nameFormat, ref string userName)
        {
            StringBuilder sb = new StringBuilder(1024);
            int nSize = 1024;
            int ret = GetUserNameEx(nameFormat, sb, ref nSize);
            userName = sb.ToString();
            return ret;
        }
        
        [DllImport("psapi.dll")]
        public static extern uint GetModuleFileNameEx(IntPtr hProcess, IntPtr hModule, [Out] StringBuilder lpBaseName, uint nSize);

    }
}
