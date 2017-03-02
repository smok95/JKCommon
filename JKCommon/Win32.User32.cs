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
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace JKCommon
{
    public partial class Win32
    {
        private const string user32 = "user32";

        [DllImport(user32)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsWindow(IntPtr hWnd);


        [DllImport(user32, ExactSpelling = true, CharSet = CharSet.Auto)]
        public static extern IntPtr GetParent(IntPtr hWnd);


        [DllImport(user32)]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport(user32)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left, Top, Right, Bottom;
        }

        public enum ShowWindowCmds : int
        {
            Hide = 0,
            Normal = 1,
            ShowMinimized = 2,
            Maxmize = 3,
            ShowMaximized = 3,
            ShowNoActive = 4,
            Show = 5,
            Minimize = 6,
            ShowMinNoActive = 7,
            ShowNA = 8,
            Restore = 9,
            ShowDefault = 10,
            ForceMinimize = 11
        }
        [DllImport(user32)]
        public static extern bool ShowWindow(IntPtr hWnd, ShowWindowCmds nCmdShow);

        [DllImport(user32)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport(user32, EntryPoint = "FindWindow", CharSet = CharSet.Auto)]
        public static extern IntPtr FindWindow(string className, string windowName);

        [DllImport(user32, EntryPoint = "FindWindowEx", CharSet = CharSet.Auto)]
        public static extern IntPtr FindWindowEx(IntPtr hWndParent, IntPtr hWndChildAfter, string className, string windowName);

        [DllImport(user32)]
        public static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);

        [DllImport(user32)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsIconic(IntPtr hWnd);

        [DllImport(user32)]
        public static extern bool ClientToScreen(IntPtr hWnd, ref Point lpPoint);

        [DllImport(user32, SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool SetWindowText(IntPtr hwnd, String lpString);

        [DllImport(user32, SetLastError = true)]
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
        [DllImport(user32, SetLastError = true)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, SetWindowPosFlags uFlags);

        public enum MouseEventFlags : uint
        {
            Move = 0x0001,
            LeftDown = 0x0002,
            LeftUp = 0x0004,
            RightDown = 0x0008,
            RightUp = 0x0010,
            MiddleDown = 0x0020,
            MiddleUp = 0x0040,
            Absolute = 0x8000    // 절대좌표
        }

        [DllImport(user32)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, uint dwExtraInfo);

        public static void MouseClick(System.Windows.Point pt)
        {
            BlockInput(true);
            SetCursorPos((int)pt.X, (int)pt.Y);
            uint clickFlag = (uint)(MouseEventFlags.LeftDown | MouseEventFlags.LeftUp);
            mouse_event(clickFlag, 0, 0, 0, 0);
            BlockInput(false);
        }

        [DllImport(user32)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetCursorPos(int x, int y);


        private delegate bool EnumWindowsProc(IntPtr hWnd, ArrayList handles);

        [DllImport(user32, CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
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

        [DllImport(user32, CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);
        public static string GetClassName(IntPtr hWnd, ref int retVal)
        {
            StringBuilder sb = new StringBuilder(1024);
            retVal = GetClassName(hWnd, sb, 1024);
            return sb.ToString();
        }

        [DllImport(user32, CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpStr, int nMaxCount);
        public static string GetWindowText(IntPtr hWnd, ref int retVal)
        {
            StringBuilder sb = new StringBuilder(1024);
            retVal = GetWindowText(hWnd, sb, 1024);
            return sb.ToString();
        }

        [DllImport(user32)]
        public static extern bool GetGUIThreadInfo(uint idThread, ref GUITHREADINFO lpgui);

        [StructLayout(LayoutKind.Sequential)]
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
            public RECT rcCaret;
        }
        
        [DllImport(user32)]
        static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);
        public enum WindowMessage : uint
        {
            Close = 0x10,           /// WM_CLOSE
            CopyData = 0x4A,        /// WM_COPYDATA
            ImeControl = 0x283      /// WM_IME_CONTROL
        }

        [DllImport(user32, EntryPoint = "GetClassLong")]
        public static extern uint GetClassLongPtr32(IntPtr hWnd, ClassLongFlags nIndex);

        [DllImport(user32, EntryPoint = "GetClassLongPtr")]
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

        [DllImport(user32, EntryPoint = "GetWindowLong")]
        private static extern IntPtr GetWindowLongPtr32(IntPtr hWnd, GWL nIndex);

        [DllImport(user32, EntryPoint = "GetWindowLongPtr")]
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

        /// <summary>
        /// Retrieves a handle to the foreground window (the window with which the user is currently working). 
        /// The system assigns a slightly higher priority to the thread that creates the foreground window than it does to other threads.
        /// </summary>
        /// <returns>The return value is a handle to the foreground window. 
        /// The foreground window can be NULL in certain circumstances, such as when a window is losing activation.</returns>
        [DllImport(user32)]
        public static extern IntPtr GetForegroundWindow();
    }
}
