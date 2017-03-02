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

    public class Win32
    {
        private const string KERNEL32 = "kernel32.dll";
        private const string USER32 = "user32.dll";

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsWindow(IntPtr hWnd);

        [DllImport(KERNEL32, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern bool WritePrivateProfileString(
            string lpAppName, string lpKeyName, string lpString, string lpFileName);

        [DllImport("kernel32.dll")]
        public static extern bool WritePrivateProfileSection(string lpAppName, string lpString, string lpFileName);

        [DllImport(KERNEL32, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern uint GetPrivateProfileString(
            string lpAppName, string lpKeyName, string lpDefault, StringBuilder lpReturnedString,
            uint nSize, string lpFileName);

        [DllImport("psapi.dll")]
        public static extern uint GetModuleFileNameEx(IntPtr hProcess, IntPtr hModule, [Out] StringBuilder lpBaseName, uint nSize);

        [DllImport("user32.dll")]
        static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);
        public enum WindowMessage:uint
        {
            Close = 0x10,           /// WM_CLOSE
            CopyData = 0x4A,        /// WM_COPYDATA
            ImeControl = 0x283      /// WM_IME_CONTROL
        }

        public static IntPtr SendMessage(IntPtr hWnd, WindowMessage msg, IntPtr wParam, IntPtr lParam)
        {
            return SendMessage(hWnd, (UInt32)msg, wParam, lParam);
        }

        [DllImport("kernel32.dll", SetLastError = true)]
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

        [DllImport(USER32, ExactSpelling = true, CharSet = CharSet.Auto)]
        public static extern IntPtr GetParent(IntPtr hWnd);

        [DllImport(KERNEL32, CallingConvention = CallingConvention.StdCall, 
            CharSet = CharSet.Ansi, SetLastError=true, EntryPoint="GetPrivateProfileSectionA")]
        public static extern UInt32 GetPrivateProfileSection
            (
                [In] [MarshalAs(UnmanagedType.LPStr)] string strSectionName,
                [In] byte[] pReturnedString,
                [In] int nSize,
                [In] [MarshalAs(UnmanagedType.LPStr)] string strFileName
            );

        [DllImport(KERNEL32, EntryPoint="GetPrivateProfileSectionNamesA", CharSet=CharSet.Ansi)]
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

        [DllImport(KERNEL32, SetLastError=true)]
        public static extern IntPtr OpenProcess(ProcessAccess dwDesiredAccess, [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle, int dwProcessId);

        [DllImport(KERNEL32)]
        public static extern uint GetLastError();

        [DllImport(USER32)]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport(USER32)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int Left, Top, Right, Bottom;
        }

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

        [DllImport(KERNEL32, SetLastError = true)]
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

        [DllImport("user32.dll", EntryPoint = "GetClassLong")]
        public static extern uint GetClassLongPtr32(IntPtr hWnd, ClassLongFlags nIndex);

        [DllImport("user32.dll", EntryPoint = "GetClassLongPtr")]
        public static extern IntPtr GetClassLongPtr64(IntPtr hWnd, ClassLongFlags nIndex);

        public enum GWL
        {
            GWL_WNDPROC = (-4),
            GWL_HINSTANCE = (-6),
            GWL_HWNDPARENT = (-8),
            GWL_STYLE = (-16),
            GWL_EXSTYLE = (-20),
            GWL_USERDATA = (-21),
            GWL_ID = (-12)
        }

        [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
        private static extern IntPtr GetWindowLongPtr32(IntPtr hWnd, GWL nIndex);

        [DllImport("user32.dll", EntryPoint = "GetWindowLongPtr")]
        private static extern IntPtr GetWindowLongPtr64(IntPtr hWnd, GWL nIndex);

        // This static method is required because Win32 does not support
        // GetWindowLongPtr directly
        public static IntPtr GetWindowLongPtr(IntPtr hWnd, GWL nIndex)
        {
            if (IntPtr.Size == 8)
                return GetWindowLongPtr64(hWnd, nIndex);
            else
                return GetWindowLongPtr32(hWnd, nIndex);
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

        [DllImport("kernel32.dll", SetLastError = true)]
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

        [DllImport("kernel32.dll")]
        public static extern bool Process32First(IntPtr hSnapshot, ref PROCESSENTRY32 lppe);

        [DllImport("kernel32.dll")]
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

        [DllImport("kernel32.dll")]
        public static extern bool Module32Next(IntPtr hSnapshot, ref MODULEENTRY32 lpme);

        [DllImport("kernel32.dll")]
        public static extern bool Module32First(IntPtr hSnapshot, ref MODULEENTRY32 lpme);

        public enum ShowWindowCmds:int
        {
            Hide = 0,
            Normal = 1,
            ShowMinimized = 2,
            Maxmize = 3,
            ShowMaximized = 3,
            ShowNoActive = 4,
            Show =5,
            Minimize = 6,
            ShowMinNoActive =7,
            ShowNA = 8,
            Restore = 9,
            ShowDefault=10,
            ForceMinimize=11
        }
        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, ShowWindowCmds nCmdShow);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport(USER32, EntryPoint = "FindWindow", CharSet = CharSet.Auto)]
        public static extern IntPtr FindWindow(string className, string windowName);

        [DllImport("user32.dll", EntryPoint="FindWindowEx", CharSet=CharSet.Auto)]
        public static extern IntPtr FindWindowEx(IntPtr hWndParent, IntPtr hWndChildAfter, string className, string windowName);

        [DllImport(KERNEL32, CharSet = CharSet.Auto)]
        public static extern uint RegisterApplicationRestart(string pszCmdLine, int dwFlags);

        [DllImport(USER32)]
        public static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);

        [DllImport("psapi.dll")]
        public static extern int EmptyWorkingSet(IntPtr hwProc);

        [DllImport("user32.dll")]
        [return:MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsIconic(IntPtr hWnd);

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
        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        public static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, AllocationType flAllocationType, MemoryProtection flProtect);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int nSize, out IntPtr lpNumberOfBytesWritten);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

        [DllImport("kernel32.dll")]
        public static extern IntPtr CreateRemoteThread(IntPtr hProcess, IntPtr lpThreadAttributes, uint dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, out IntPtr lpThreadId);

        [DllImport("user32.dll")]
        public static extern bool ClientToScreen(IntPtr hWnd, ref Point lpPoint);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool SetWindowText(IntPtr hwnd, String lpString);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool BlockInput(bool bBlock);

        public static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
        public static readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2);
        public static readonly IntPtr HWND_TOP = new IntPtr(0);
        public static readonly IntPtr HWND_BOTTOM = new IntPtr(1);

        [Flags]
        public enum SetWindowPosFlags : uint
        {
            // ReSharper disable InconsistentNaming

            /// <summary>
            ///     If the calling thread and the thread that owns the window are attached to different input queues, the system posts the request to the thread that owns the window. This prevents the calling thread from blocking its execution while other threads process the request.
            /// </summary>
            SWP_ASYNCWINDOWPOS = 0x4000,

            /// <summary>
            ///     Prevents generation of the WM_SYNCPAINT message.
            /// </summary>
            SWP_DEFERERASE = 0x2000,

            /// <summary>
            ///     Draws a frame (defined in the window's class description) around the window.
            /// </summary>
            SWP_DRAWFRAME = 0x0020,

            /// <summary>
            ///     Applies new frame styles set using the SetWindowLong function. Sends a WM_NCCALCSIZE message to the window, even if the window's size is not being changed. If this flag is not specified, WM_NCCALCSIZE is sent only when the window's size is being changed.
            /// </summary>
            SWP_FRAMECHANGED = 0x0020,

            /// <summary>
            ///     Hides the window.
            /// </summary>
            SWP_HIDEWINDOW = 0x0080,

            /// <summary>
            ///     Does not activate the window. If this flag is not set, the window is activated and moved to the top of either the topmost or non-topmost group (depending on the setting of the hWndInsertAfter parameter).
            /// </summary>
            SWP_NOACTIVATE = 0x0010,

            /// <summary>
            ///     Discards the entire contents of the client area. If this flag is not specified, the valid contents of the client area are saved and copied back into the client area after the window is sized or repositioned.
            /// </summary>
            SWP_NOCOPYBITS = 0x0100,

            /// <summary>
            ///     Retains the current position (ignores X and Y parameters).
            /// </summary>
            SWP_NOMOVE = 0x0002,

            /// <summary>
            ///     Does not change the owner window's position in the Z order.
            /// </summary>
            SWP_NOOWNERZORDER = 0x0200,

            /// <summary>
            ///     Does not redraw changes. If this flag is set, no repainting of any kind occurs. This applies to the client area, the nonclient area (including the title bar and scroll bars), and any part of the parent window uncovered as a result of the window being moved. When this flag is set, the application must explicitly invalidate or redraw any parts of the window and parent window that need redrawing.
            /// </summary>
            SWP_NOREDRAW = 0x0008,

            /// <summary>
            ///     Same as the SWP_NOOWNERZORDER flag.
            /// </summary>
            SWP_NOREPOSITION = 0x0200,

            /// <summary>
            ///     Prevents the window from receiving the WM_WINDOWPOSCHANGING message.
            /// </summary>
            SWP_NOSENDCHANGING = 0x0400,

            /// <summary>
            ///     Retains the current size (ignores the cx and cy parameters).
            /// </summary>
            SWP_NOSIZE = 0x0001,

            /// <summary>
            ///     Retains the current Z order (ignores the hWndInsertAfter parameter).
            /// </summary>
            SWP_NOZORDER = 0x0004,

            /// <summary>
            ///     Displays the window.
            /// </summary>
            SWP_SHOWWINDOW = 0x0040,

            // ReSharper restore InconsistentNaming
        }
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, SetWindowPosFlags uFlags);

        public enum MouseEventFlags:uint
        {
            Move        = 0x0001,
            LeftDown    = 0x0002,
            LeftUp      = 0x0004,
            RightDown   = 0x0008,
            RightUp     = 0x0010,
            MiddleDown  = 0x0020,
            MiddleUp    = 0x0040,
            Absolute    = 0x8000    // 절대좌표
        }

        [DllImport("user32.dll")]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, uint dwExtraInfo);
        
        public static void MouseClick(System.Windows.Point pt)
        {
            BlockInput(true);
            SetCursorPos((int)pt.X, (int)pt.Y);
            uint clickFlag = (uint)(MouseEventFlags.LeftDown | MouseEventFlags.LeftUp);            
            mouse_event(clickFlag, 0,0 , 0, 0);
            BlockInput(false);
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetCursorPos(int x, int y);

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

        private delegate bool EnumWindowsProc(IntPtr hWnd, ArrayList handles);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return:MarshalAs(UnmanagedType.Bool)]
        private static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, ArrayList handles);

        private static bool GetWindowsHandle(IntPtr hWnd, ArrayList handles)
        {
            handles.Add(hWnd);
            return true;
        }

        public static IntPtr[] EnumWindows()
        {
            ArrayList handles = new ArrayList();
            EnumWindowsProc proc = GetWindowsHandle;
            EnumWindows(proc, handles);            
            return (IntPtr[])handles.ToArray(typeof(IntPtr));
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);
        public static string GetClassName(IntPtr hWnd, ref int retVal)
        {
            StringBuilder sb = new StringBuilder(1024);
            retVal = GetClassName(hWnd, sb, 1024);            
            return sb.ToString();
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpStr, int nMaxCount);
        public static string GetWindowText(IntPtr hWnd, ref int retVal)
        {
            StringBuilder sb = new StringBuilder(1024);
            retVal = GetWindowText(hWnd, sb, 1024);
            return sb.ToString();
        }

        [DllImport("user32.dll")]
        public static extern bool GetGUIThreadInfo(uint idThread, ref GUITHREADINFO lpgui);

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct GUITHREADINFO
        {
            public int cbSize;
            public int flags;
            public IntPtr hwndActive;
            public IntPtr hwndFocus;
            public IntPtr hwndCapture;
            public IntPtr hwndMenuOwner;
            public IntPtr hwndMoveSize;
            public IntPtr hwndCaret;
            public System.Drawing.Rectangle rcCaret;
        }

        /// <summary>
        /// Retrieves the default window handle to the IME class.
        /// </summary>
        /// <param name="hWnd">Handle to the window.</param>
        /// <returns>Returns the default window handle to the IME class if successful, or NULL otherwise.</returns>
        /// <remarks>The operating system creates a default IME window for every thread. The window is created based on the IME class. The application can send the WM_IME_CONTROL message to this window.
        /// </remarks>
        [DllImport("imm32.dll")]
        public static extern IntPtr ImmGetDefaultIMEWnd(IntPtr hWnd);

        [Flags]
        public enum ImeConversionMode : uint
        {
            IME_CMODE_ALPHANUMERIC = 0x0000,
            IME_CMODE_NATIVE = 0x0001,
            IME_CMODE_CHINESE = IME_CMODE_NATIVE,
            IME_CMODE_HANGUL = IME_CMODE_NATIVE,
            IME_CMODE_JAPANESE = IME_CMODE_NATIVE,
            IME_CMODE_KATAKANA = 0x0002,            // only effect under IME_CMODE_NATIVE
            IME_CMODE_LANGUAGE = 0x0003,
            IME_CMODE_FULLSHAPE = 0x0008,
            IME_CMODE_ROMAN = 0x0010,
            IME_CMODE_CHARCODE = 0x0020,
            IME_CMODE_HANJACONVERT = 0040,
            IME_CMODE_SOFTKBD = 0x0080,
            IME_CMODE_NOCONVERSION = 0x0100,
            IME_CMODE_EUDC = 0x0200,
            IME_CMODE_SYMBOL = 0x0400,
            IME_CMODE_FIXED = 0x0800,
            IME_CMODE_RESERVED = 0xF0000000
        }
    }
}
