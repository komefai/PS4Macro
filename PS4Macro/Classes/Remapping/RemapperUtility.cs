// PPS4Macro(File: Classes/Remapping/RemapperUtility.cs)
//
// Copyright (c) 2018 Komefai
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
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace PS4Macro.Classes.Remapping
{
    public class RemapperUtility
    {
        #region SetCursorPosition
        [DllImport("user32.dll")]
        private static extern IntPtr SetCursorPos(int x, int y);

        public static void SetCursorPosition(int x, int y)
        {
            SetCursorPos(x, y);
        }

        public static void SetCursorPosition(Point p)
        {
            SetCursorPos(p.Y, p.Y);
        }
        #endregion

        #region ShowSystemCursor
        [Flags]
        public enum SPIF
        {
            None = 0x00,
            SPIF_UPDATEINIFILE = 0x01,
            SPIF_SENDCHANGE = 0x02,
            SPIF_SENDWININICHANGE = 0x02
        }
        [Flags]
        public enum SPI
        {
            None = 0x00,
            SPIF_UPDATEINIFILE = 0x01,
            SPIF_SENDCHANGE = 0x02,
            SPIF_SENDWININICHANGE = 0x02
        }

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SystemParametersInfo(SPI uiAction, uint uiParam, IntPtr pvParam, SPIF fWinIni);

        // For setting a string parameter
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SystemParametersInfo(uint uiAction, uint uiParam, String pvParam, SPIF fWinIni);

        [DllImport("user32.dll")]
        private static extern bool SetSystemCursor(IntPtr hcur, uint id);
        enum OCR_SYSTEM_CURSORS : uint
        {
            /// <summary>
            /// Standard arrow and small hourglass
            /// </summary>
            OCR_APPSTARTING = 32650,
            /// <summary>
            /// Standard arrow
            /// </summary>
            OCR_NORMAL = 32512,
            /// <summary>
            /// Crosshair
            /// </summary>
            OCR_CROSS = 32515,
            /// <summary>
            /// Windows 2000/XP: Hand
            /// </summary>
            OCR_HAND = 32649,
            /// <summary>
            /// Arrow and question mark
            /// </summary>
            OCR_HELP = 32651,
            /// <summary>
            /// I-beam
            /// </summary>
            OCR_IBEAM = 32513,
            /// <summary>
            /// Slashed circle
            /// </summary>
            OCR_NO = 32648,
            /// <summary>
            /// Four-pointed arrow pointing north, south, east, and west
            /// </summary>
            OCR_SIZEALL = 32646,
            /// <summary>
            /// Double-pointed arrow pointing northeast and southwest
            /// </summary>
            OCR_SIZENESW = 32643,
            /// <summary>
            /// Double-pointed arrow pointing north and south
            /// </summary>
            OCR_SIZENS = 32645,
            /// <summary>
            /// Double-pointed arrow pointing northwest and southeast
            /// </summary>
            OCR_SIZENWSE = 32642,
            /// <summary>
            /// Double-pointed arrow pointing west and east
            /// </summary>
            OCR_SIZEWE = 32644,
            /// <summary>
            /// Vertical arrow
            /// </summary>
            OCR_UP = 32516,
            /// <summary>
            /// Hourglass
            /// </summary>
            OCR_WAIT = 32514
        }

        [DllImport("user32.dll")]
        private static extern IntPtr LoadCursor(IntPtr hInstance, int lpCursorName);
        [DllImport("user32.dll")]
        private static extern IntPtr LoadCursorFromFile(string lpFileName);

        private static byte[] _transparentCursor = CreateTransparentCursor();
        private static byte[] CreateTransparentCursor()
        {
            var b = new byte[326];

            b[2] = 0x02; b[4] = 0x01; b[6] = 0x20; b[7] = 0x02;
            b[14] = 0x30; b[15] = 0x01; b[18] = 0x16; b[22] = 0x28;
            b[26] = 0x20; b[30] = 0x40; b[34] = 0x01; b[36] = 0x01;
            b[42] = 0x80; b[54] = 0x02;

            for (var i = 198; i < 326; i++)
            {
                b[i] = 0xFF;
            }

            return b;
        }

        public static bool ShowSystemCursor(bool show)
        {
            try
            {
                if (!show)
                {
                    // Load the cursor
                    var cursorFile = Path.GetTempFileName();
                    using (var fs = new FileStream(cursorFile, FileMode.Create, FileAccess.Write))
                    {
                        fs.Write(_transparentCursor, 0, _transparentCursor.Length);
                    }

                    // Set the cursor
                    IntPtr cursor = LoadCursorFromFile(cursorFile);
                    SetSystemCursor(cursor, (uint)OCR_SYSTEM_CURSORS.OCR_NORMAL);

                    // Delete temp file
                    File.Delete(cursorFile);
                }
                else
                {
                    // Reset the cursor
                    SystemParametersInfo(0x0057, 0, null, 0);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion

        #region ShowStreamingToolBar
        private const int SW_HIDE = 0;
        private const int SW_SHOW = 5;

        [DllImport("User32")]
        private static extern int ShowWindow(int hwnd, int nCmdShow);

        [DllImport("user32", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private extern static bool EnumThreadWindows(int threadId, EnumWindowsProc callback, IntPtr lParam);

        [DllImport("user32", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool EnumChildWindows(IntPtr hwndParent, EnumWindowsProc lpEnumFunc, IntPtr lParam);

        [DllImport("user32", SetLastError = true, CharSet = CharSet.Auto)]
        private extern static int GetWindowText(IntPtr hWnd, StringBuilder text, int maxCount);

        private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        private static IntPtr FindWindowInProcess(Process process, Func<string, bool> compareTitle)
        {
            IntPtr windowHandle = IntPtr.Zero;

            foreach (ProcessThread t in process.Threads)
            {
                windowHandle = FindWindowInThread(t.Id, compareTitle);
                if (windowHandle != IntPtr.Zero)
                {
                    break;
                }
            }

            return windowHandle;
        }

        private static IntPtr FindWindowInThread(int threadId, Func<string, bool> compareTitle)
        {
            IntPtr windowHandle = IntPtr.Zero;
            EnumThreadWindows(threadId, (hWnd, lParam) =>
            {
                StringBuilder text = new StringBuilder(200);
                GetWindowText(hWnd, text, 200);
                if (compareTitle(text.ToString()))
                {
                    windowHandle = hWnd;
                    return false;
                }
                return true;
            }, IntPtr.Zero);

            return windowHandle;
        }

        public static void ShowStreamingToolBar(Process process, bool show)
        {
            IntPtr streamingToolBar = FindWindowInProcess(process, title => title.Equals("StreamingToolBar"));
            if (streamingToolBar != IntPtr.Zero)
            {
                ShowWindow(streamingToolBar.ToInt32(), show ? SW_SHOW : SW_HIDE);
            }
        }
        #endregion

        #region IsProcessInForeground
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        public static bool IsProcessInForeground(Process process)
        {
            if (process == null)
                return false;

            // Check for focused window
            var activeWindow = GetForegroundWindow();
            if (activeWindow == IntPtr.Zero)
                return false;
            if (activeWindow != process.MainWindowHandle)
                return false;

            return true;
        }
        #endregion

        // https://stackoverflow.com/questions/13270183/type-conversion-issue-when-setting-property-through-reflection
        public static void SetValue(object inputObject, string propertyName, object propertyVal)
        {
            //find out the type
            Type type = inputObject.GetType();

            //get the property information based on the type
            System.Reflection.PropertyInfo propertyInfo = type.GetProperty(propertyName);

            //find the property type
            Type propertyType = propertyInfo.PropertyType;

            //Convert.ChangeType does not handle conversion to nullable types
            //if the property type is nullable, we need to get the underlying type of the property
            var targetType = IsNullableType(propertyInfo.PropertyType) ? Nullable.GetUnderlyingType(propertyInfo.PropertyType) : propertyInfo.PropertyType;

            //Returns an System.Object with the specified System.Type and whose value is
            //equivalent to the specified object.
            propertyVal = Convert.ChangeType(propertyVal, targetType);

            //Set the value of the property
            propertyInfo.SetValue(inputObject, propertyVal, null);

        }

        public static bool IsNullableType(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>));
        }
    }
}