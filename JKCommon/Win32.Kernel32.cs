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
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace JKCommon
{
    public class Win32Window : IWin32Window
    {
        private IntPtr handle;
        public Win32Window(IntPtr handle)
        {
            this.handle = handle;
        }
        public IntPtr Handle
        {
            get { return handle; }
        }
    }

    public partial class Win32
    {
        private const string kernel32 = "kernel32";
        

        [DllImport(kernel32, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern bool WritePrivateProfileString(
            string lpAppName, string lpKeyName, string lpString, string lpFileName);

        [DllImport(kernel32)]
        public static extern bool WritePrivateProfileSection(string lpAppName, string lpString, string lpFileName);

        [DllImport(kernel32, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern uint GetPrivateProfileString(
            string lpAppName, string lpKeyName, string lpDefault, StringBuilder lpReturnedString,
            uint nSize, string lpFileName);

        public static IntPtr SendMessage(IntPtr hWnd, WindowMessage msg, IntPtr wParam, IntPtr lParam)
        {
            return SendMessage(hWnd, (UInt32)msg, wParam, lParam);
        }

        [DllImport(kernel32, SetLastError = true)]
        [PreserveSig]
        public static extern uint GetModuleFileName
        (
            [In]
            IntPtr hModule,

            [Out] 
            StringBuilder lpFilename,

            [In]
            [MarshalAs(UnmanagedType.U4)]
            int nSize
        );

        [DllImport(kernel32, CallingConvention = CallingConvention.StdCall, 
            CharSet = CharSet.Ansi, SetLastError=true, EntryPoint="GetPrivateProfileSectionA")]
        public static extern UInt32 GetPrivateProfileSection
            (
                [In] [MarshalAs(UnmanagedType.LPStr)] string strSectionName,
                [In] byte[] pReturnedString,
                [In] int nSize,
                [In] [MarshalAs(UnmanagedType.LPStr)] string strFileName
            );

        [DllImport(kernel32, EntryPoint="GetPrivateProfileSectionNamesA", CharSet=CharSet.Ansi)]
        public static extern int GetPrivateProfileSectionNames (byte[] lpszReturnBuffer, int nSize, string lpFileName);

        public enum ProcessAccess
        {
            /// <summary>
            /// Required to create a thread.
            /// </summary>
            CreateThread = 0x0002,            
            SetSessionId = 0x0004,
            /// <summary>
            /// Required to perform an operation on the address space of a process 
            /// </summary>
            VmOperation = 0x0008,
            /// <summary>
            /// Required to read memory in a process using ReadProcessMemory.
            /// </summary>
            VmRead = 0x0010,
            /// <summary>
            /// Required to write to memory in a process using WriteProcessMemory.
            /// </summary>
            VmWrite = 0x0020,
            /// <summary>
            /// Required to duplicate a handle using DuplicateHandle.
            /// </summary>
            DupHandle = 0x0040,
            /// <summary>
            /// Required to create a process.
            /// </summary>
            CreateProcess = 0x0080,
            /// <summary>
            /// Required to set memory limits using SetProcessWorkingSetSize.
            /// </summary>
            SetQuota = 0x0100,
            /// <summary>
            /// Required to set certain information about a process, such as its priority class (see SetPriorityClass).
            /// </summary>
            SetInformation = 0x0200,
            /// <summary>
            /// Required to retrieve certain information about a process, such as its token, exit code, and priority class (see OpenProcessToken).
            /// </summary>
            QueryInformation = 0x0400,
            /// <summary>
            /// Required to suspend or resume a process.
            /// </summary>
            SuspendResume = 0x0800,
            /// <summary>
            /// Required to retrieve certain information about a process (see GetExitCodeProcess, GetPriorityClass, IsProcessInJob, QueryFullProcessImageName). 
            /// A handle that has the PROCESS_QUERY_INFORMATION access right is automatically granted PROCESS_QUERY_LIMITED_INFORMATION.
            /// </summary>
            QueryLimitedInformation = 0x1000,

            /// <summary>
            /// Required to wait for the process to terminate using the wait functions.
            /// </summary>
            Synchronize = 0x100000,

            /// <summary>
            /// Required to delete the object.
            /// </summary>
            Delete = 0x00010000,

            /// <summary>
            /// Required to read information in the security descriptor for the object, not including the information in the SACL. 
            /// To read or write the SACL, you must request the ACCESS_SYSTEM_SECURITY access right. For more information, see SACL Access Right.
            /// </summary>
            ReadControl = 0x00020000,

            /// <summary>
            /// Required to modify the DACL in the security descriptor for the object.
            /// </summary>
            WriteDac = 0x00040000,

            /// <summary>
            /// Required to change the owner in the security descriptor for the object.
            /// </summary>
            WriteOwner = 0x00080000,

            StandardRightsRequired = 0x000F0000,

            /// <summary>
            /// All possible access rights for a process object.
            /// </summary>
            AllAccess = StandardRightsRequired | Synchronize | 0xFFFF
        }

        [DllImport(kernel32, SetLastError=true)]
        public static extern IntPtr OpenProcess(ProcessAccess dwDesiredAccess, [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle, int dwProcessId);

        [DllImport(kernel32)]
        public static extern uint GetLastError();


        [StructLayout(LayoutKind.Sequential)]
        public struct COPYDATASTRUCT
        {
            public IntPtr dwData;
            public int cbData;      /// The count of bytes in the message.
            public IntPtr lpData;   /// The address of the message.
        }

        public static Rectangle GetWindowRect(IntPtr hWnd)
        {
            RECT r;
            GetWindowRect(hWnd, out r);
            return new Rectangle(r.Left, r.Top, r.Right - r.Left, r.Bottom - r.Top);
        }

        [DllImport(kernel32, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool CloseHandle(IntPtr hObject);

        public static string GetModuleFilenameFromPID(int pid)
        {
            IntPtr hProcess = OpenProcess(ProcessAccess.QueryInformation | ProcessAccess.VmRead, false, pid);
            if (hProcess == IntPtr.Zero)
            {
                int err = Marshal.GetLastWin32Error();
                int a = err;
                return "";
            }
            StringBuilder sb = new StringBuilder(1024);
            GetModuleFileNameEx(hProcess, IntPtr.Zero, sb, (uint)sb.Capacity);
            CloseHandle(hProcess);
            return sb.ToString();
        }

        public enum ClassLongFlags : int
        {
            GCLP_MENUNAME = -8,
            GCLP_HBRBACKGROUND = -10,
            GCLP_HCURSOR = -12,
            GCLP_HICON = -14,
            GCLP_HMODULE = -16,
            GCL_CBWNDEXTRA = -18,
            GCL_CBCLSEXTRA = -20,
            GCLP_WNDPROC = -24,
            GCL_STYLE = -26,
            GCLP_HICONSM = -34,
            GCW_ATOM = -32
        }

        public static IntPtr GetClassLongPtr(IntPtr hWnd, ClassLongFlags nIndex)
        {
            if (IntPtr.Size > 4)
                return GetClassLongPtr64(hWnd, nIndex);
            else
                return new IntPtr(GetClassLongPtr32(hWnd, nIndex));
        }
        
        public enum SnapshotFlags : uint
        {
            HeapList = 0x00000001,
            Process = 0x00000002,
            Thread = 0x00000004,
            Module = 0x00000008,
            Module32 = 0x00000010,
            All = (HeapList | Process | Thread | Module),
            Inherit = 0x80000000,
            NoHeaps = 0x40000000

        }

        [DllImport(kernel32,SetLastError = true)]
        public static extern IntPtr CreateToolhelp32Snapshot(SnapshotFlags dwFlags, uint th32ProcessID);

        [StructLayout(LayoutKind.Sequential)]
        public struct PROCESSENTRY32
        {
            public uint dwSize;
            public uint cntUsage;
            public uint th32ProcessID;
            public IntPtr th32DefaultHeapID;
            public uint th32ModuleID;
            public uint cntThreads;
            public uint th32ParentProcessID;
            public int pcPriClassBase;
            public uint dwFlags;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szExeFile;
        }; 

        [DllImport(kernel32)]
        public static extern bool Process32First(IntPtr hSnapshot, ref PROCESSENTRY32 lppe);

        [DllImport(kernel32)]
        public static extern bool Process32Next(IntPtr hSnapshot, ref PROCESSENTRY32 lppe);

        public struct MODULEENTRY32
        {
            private const int MAX_PATH = 255;
#pragma warning disable 0649
            internal uint dwSize;    
            internal uint th32ModuleID;
            internal uint th32ProcessID;
            internal uint GlblcntUsage;
            internal uint ProccntUsage;
            internal IntPtr modBaseAddr;
            internal uint modBaseSize;
            internal IntPtr hModule;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_PATH + 1)]
            internal string szModule;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_PATH + 5)]
            internal string szExePath;
#pragma warning restore 0649
        }

        [DllImport(kernel32)]
        public static extern bool Module32Next(IntPtr hSnapshot, ref MODULEENTRY32 lpme);

        [DllImport(kernel32)]
        public static extern bool Module32First(IntPtr hSnapshot, ref MODULEENTRY32 lpme);
                
        [DllImport(kernel32, CharSet = CharSet.Auto)]
        public static extern uint RegisterApplicationRestart(string pszCmdLine, int dwFlags);
                
        [Flags]
        public enum AllocationType
        {
            Commit = 0x1000,
            Reserve = 0x2000,
            Decommit = 0x4000,
            Release = 0x8000,
            Reset = 0x80000,
            Physical = 0x400000,
            TopDown = 0x100000,
            WriteWatch = 0x200000,
            LargePages = 0x20000000
        }

        [Flags]
        public enum MemoryProtection
        {
            Execute = 0x10,
            ExecuteRead = 0x20,
            ExecuteReadWrite = 0x40,
            ExecuteWriteCopy = 0x80,
            NoAccess = 0x01,
            ReadOnly = 0x02,
            ReadWrite = 0x04,
            WriteCopy = 0x08,
            GuardModifierflag = 0x100,
            NoCacheModifierflag = 0x200,
            WriteCombineModifierflag = 0x400
        }
        [DllImport(kernel32, SetLastError = true, ExactSpelling = true)]
        public static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, AllocationType flAllocationType, MemoryProtection flProtect);

        [DllImport(kernel32, SetLastError = true)]
        public static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int nSize, out IntPtr lpNumberOfBytesWritten);

        [DllImport(kernel32, CharSet = CharSet.Auto)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport(kernel32, CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

        [DllImport(kernel32)]
        public static extern IntPtr CreateRemoteThread(IntPtr hProcess, IntPtr lpThreadAttributes, uint dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, out IntPtr lpThreadId);        
    }
}
