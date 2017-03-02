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
using System.Text;
using System.Windows;
using System.Windows.Forms;

namespace JKCommon
{    
    public static class DllInjector
    {
        
        /// <summary>
        /// Dll인젝션
        /// </summary>
        /// <param name="pid">대상 프로세스ID</param>
        /// <param name="dllName">주입할 Dll파일명</param>
        /// <returns>성공시 true</returns>
        public static bool Inject(int pid, string dllName)
        {
            bool bOk = false;
            IntPtr hProcess = IntPtr.Zero;
            IntPtr pRemoteBuf = IntPtr.Zero;
            IntPtr hThread = IntPtr.Zero;
            // 51949 = euc-kr
            byte[] data = System.Text.Encoding.GetEncoding(51949).GetBytes(dllName); //System.Text.Encoding.ASCII.GetBytes(dllName);
            uint dwBufSize = (uint)(dllName.Length + 1);

            hProcess = Win32.OpenProcess(Win32.ProcessAccess.AllAccess, false, pid);
            if (hProcess == IntPtr.Zero)
            {
                MessageBox.Show("Failed to OpenProcess, code=" + Win32.GetLastError().ToString());
                goto Cleanup;
            }

            
            pRemoteBuf = Win32.VirtualAllocEx(hProcess, IntPtr.Zero, (uint)data.Length, Win32.AllocationType.Commit, Win32.MemoryProtection.ReadWrite);
            if (pRemoteBuf == IntPtr.Zero)
            {
                MessageBox.Show("Failed to VirtualAllocEx, code=" + Win32.GetLastError().ToString());
                goto Cleanup;
            }

            IntPtr bytesWritten = IntPtr.Zero;
            if (!Win32.WriteProcessMemory(hProcess, pRemoteBuf, data, data.Length, out bytesWritten))
            {
                MessageBox.Show("Failed to WriteProcessMemory, code=" + Win32.GetLastError().ToString());
                goto Cleanup;
            }

            IntPtr hMod = Win32.GetModuleHandle("kernel32.dll");
            IntPtr pThreadProc = Win32.GetProcAddress(hMod, "LoadLibraryA");
            if (pThreadProc == IntPtr.Zero)
            {
                MessageBox.Show("Failed to GetProcAddress('LoadLibraryA'), code=" + Win32.GetLastError().ToString());
                goto Cleanup;
            }

            IntPtr threadId = IntPtr.Zero;
            hThread = Win32.CreateRemoteThread(hProcess, IntPtr.Zero, 0, pThreadProc, pRemoteBuf, 0, out threadId);
            if (hThread == IntPtr.Zero)
            {
                MessageBox.Show("Failed to CreateRemoteThread, code=" + Win32.GetLastError().ToString());
                goto Cleanup;
            }

            bOk = true;
        Cleanup:
            if (hThread != IntPtr.Zero)
                Win32.CloseHandle(hThread);
            if (hProcess != IntPtr.Zero)
                Win32.CloseHandle(hProcess);
            return bOk;
        }
    }
}
