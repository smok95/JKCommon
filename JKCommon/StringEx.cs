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
using System.Security.Cryptography;

namespace JKCommon
{
    public static class StringEx
    {
        /// <summary>
        /// 데이터 암호화시 사용할 추가엔트로피 값
        /// </summary>
        private static string m_additionalEntropy = "kim,jongkook";
        /// <summary>
        /// 문자열 비교(대소문자 비교안함.)
        /// </summary>
        /// <param name="src"></param>
        /// <param name="value">비교할 문자열</param>
        /// <returns>일치하는 경우 true</returns>
        public static bool EqualsIgnoreCase(this string src, string value)
        {
            return string.Compare(src, value, true) == 0;
        }

        /// <summary>
        /// 문자열을 base64문자열로 인코딩한다.
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static string ToBase64(this string src)
        {
            byte[] data = System.Text.Encoding.UTF8.GetBytes(src);
            return Convert.ToBase64String(data);
        }

        /// <summary>
        /// base64문자열을 디코딩한다.
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static string FromBase64(this string src)
        {
            byte[] data = Convert.FromBase64String(src);
            return System.Text.Encoding.UTF8.GetString(data);
        }

        /// <summary>
        /// 문자열 암호화
        /// 동일한PC의 사용자인 경우에만 암복호화가 되니 주의할것!
        /// </summary>
        /// <param name="src">암호화할 문자열</param>
        /// <returns>암호화된 문자열</returns>
        public static string Encrypt(this string src)
        {
            if (string.IsNullOrEmpty(src)) return "";
            byte[] addEntropy = Encoding.UTF8.GetBytes(m_additionalEntropy);
            return Convert.ToBase64String(ProtectedData.Protect(Encoding.UTF8.GetBytes(src), addEntropy, DataProtectionScope.CurrentUser));
        }

        /// <summary>
        /// 문자열 복호화
        /// 동일한PC의 사용자인 경우에만 암복호화가 되니 주의할것!
        /// </summary>
        /// <param name="src">복호화할 문자열</param>
        /// <returns>복호화된 문자열</returns>
        public static string Decrypt(this string src)
        {
            if (string.IsNullOrEmpty(src)) return "";
            byte[] addEntropy = Encoding.UTF8.GetBytes(m_additionalEntropy);
            return Encoding.UTF8.GetString(ProtectedData.Unprotect(Convert.FromBase64String(src), addEntropy, DataProtectionScope.CurrentUser));
        }
    }
}
