// PS4Macro (File: Classes/ConsoleHelper.cs)
//
// Copyright (c) 2017 Komefai
//
// Visit http://komefai.com for more information
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32.SafeHandles;

namespace PS4Macro.Classes
{
    class ConsoleHelper
    {
        #region Win32 API
        [DllImport("kernel32.dll",
            EntryPoint = "AllocConsole",
            SetLastError = true,
            CharSet = CharSet.Auto,
            CallingConvention = CallingConvention.StdCall)]
        public static extern int AllocConsole();

        [DllImport("kernel32.dll",
            CallingConvention = CallingConvention.StdCall,
            CharSet = CharSet.Auto,
            ExactSpelling = false,
            SetLastError = true)]
        public static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr CreateFile(
            string lpFileName,
            uint dwDesiredAccess,
            uint dwShareMode,
            uint lpSecurityAttributes,
            uint dwCreationDisposition,
            uint dwFlagsAndAttributes,
            uint hTemplateFile);

        private const int MY_CODE_PAGE = 437;
        private const string CONSOLE_FILENAME = "CONOUT$";
        private const uint GENERIC_WRITE = 0x40000000;
        private const uint FILE_SHARE_WRITE = 0x2;
        private const uint OPEN_EXISTING = 0x3;
        #endregion

        internal static void SetOut()
        {
            // Without debugger
            try
            {
                SafeFileHandle safeFileHandle = new SafeFileHandle(GetStdHandle(-11), true);
                FileStream fileStream = new FileStream(safeFileHandle, FileAccess.Write);
                StreamWriter standardOutput = new StreamWriter(fileStream, Encoding.GetEncoding(MY_CODE_PAGE))
                {
                    AutoFlush = true
                };
                Console.SetOut(standardOutput);
            }
            // With debugger
            catch (Exception)
            {
                IntPtr stdHandle = CreateFile(
                    CONSOLE_FILENAME,
                    GENERIC_WRITE,
                    FILE_SHARE_WRITE,
                    0, OPEN_EXISTING, 0, 0
                );

                SafeFileHandle safeFileHandle = new SafeFileHandle(stdHandle, true);
                FileStream fileStream = new FileStream(safeFileHandle, FileAccess.Write);
                StreamWriter standardOutput = new StreamWriter(fileStream, Encoding.GetEncoding(MY_CODE_PAGE))
                {
                    AutoFlush = true
                };
                Console.SetOut(standardOutput);
            }
        }
    }
}
