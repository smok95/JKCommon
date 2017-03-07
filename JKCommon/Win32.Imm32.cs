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
        private const string imm32 = "imm32";

        /// <summary>
        /// Retrieves the default window handle to the IME class.
        /// </summary>
        /// <param name="hWnd">Handle to the window.</param>
        /// <returns>Returns the default window handle to the IME class if successful, or NULL otherwise.</returns>
        /// <remarks>The operating system creates a default IME window for every thread. The window is created based on the IME class. The application can send the WM_IME_CONTROL message to this window.
        /// </remarks>
        [DllImport(imm32)]
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

        [Flags]
        public enum ImeSentenceMode : uint
        {
            IME_SMODE_NONE = 0x0000,            // No information for sentence.
            IME_SMODE_PLURALCLAUSE = 0x0001,    // The IME uses plural clause information to carry out conversion processing.
            IME_SMODE_SINGLECONVERT= 0x0002,    // The IME carries out conversion processing in single-character mode.
            IME_SMODE_AUTOMATIC = 0x0004,       // The IME carries out conversion processing in automatic mode.
            IME_SMODE_PHRASEPREDICT = 0x0008,   // The IME uses phrase information to predict the next character.
            IME_SMODE_CONVERSATION = 0x0010     // The IME uses conversation mode. This is useful for chat applications.
        }

        [DllImport(imm32, SetLastError = true)]
        public static extern IntPtr ImmGetContext(IntPtr hWnd);

        [DllImport(imm32)]
        public static extern bool ImmGetConversionStatus(IntPtr himc, ref int lpdwConversion, ref int lpdwSentence);

        [DllImport(imm32)]
        public static extern bool ImmReleaseContext(IntPtr hWnd, IntPtr hIMC);

    }
}
