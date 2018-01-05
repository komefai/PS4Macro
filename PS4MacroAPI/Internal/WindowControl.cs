// PS4MacroAPI (File: Internal/WindowControl.cs)
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
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace PS4MacroAPI.Internal
{
    struct Rect
    {
        public int Left { get; set; }
        public int Top { get; set; }
        public int Right { get; set; }
        public int Bottom { get; set; }
    }

    class WindowControl
    {
        #region Constants
        private const uint WM_GETTEXT = 0x000D;
        private const uint WM_GETTEXTLENGTH = 0x000E;
        private const uint WM_SHOWWINDOW = 0x0040;
        private const uint WM_KEYDOWN = 0x100;
        private const uint WM_KEYUP = 0x101;
        private const uint WM_MOUSEMOVE = 0x0200;
        private const uint WM_LBUTTONDOWN = 0x0201;
        private const uint WM_MBUTTONDOWN = 0x0207;
        private const uint WM_RBUTTONDOWN = 0x0204;
        private const uint WM_MOUSEWHEEL = 0x020A;

        private const uint SWP_NOMOVE = 0x0002;

        private const long WS_SIZEBOX = 0x00040000L;
        private const long WS_OVERLAPPED = 0x00000000L;
        private const long WS_CAPTION = 0x00C00000L;
        private const long WS_SYSMENU = 0x00080000L;
        private const long WS_MINIMIZEBOX = 0x00020000L;
        private const long WS_MAXIMIZEBOX = 0x00010000L;

        private const int GWL_EXSTYLE = -20;
        private const int WS_EX_LAYERED = 0x80000;
        private const int LWA_ALPHA = 0x2;
        private const int LWA_COLORKEY = 0x1;

        private const int SW_HIDE = 0;
        private const int SW_MAXIMIZE = 3;
        private const int SW_SHOW = 5;
        private const int SW_MINIMIZE = 6;
        private const int SW_RESTORE = 9;

        private const int WHEEL_DELTA = 120;
        #endregion

        #region Win32 API
        private delegate bool EnumWindowProc(IntPtr hwnd, IntPtr lParam);

        [DllImport("user32")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool EnumChildWindows(IntPtr window, EnumWindowProc callback, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hwnd, ref Rect rectangle);

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hwnd, int nCmdShow);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindowEx(IntPtr parentHWnd, IntPtr childAfterHWnd, string className, string windowTitle);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern void GetWindowText(IntPtr handle, StringBuilder lpString, int maxTextCapacity);

        [DllImport("User32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern long GetClassName(IntPtr hwnd, StringBuilder lpClassName, long nMaxCount);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr SwitchToThisWindow(IntPtr hWnd, bool fAltTab);

        [DllImport("user32.dll")]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int width, int height, uint uFlags);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr SetActiveWindow(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint msg, uint wParam, uint lParam);

        [DllImport("user32.dll", EntryPoint = "SendMessage", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint msg, uint wParam, StringBuilder text);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr PostMessage(IntPtr hWnd, uint msg, uint wParam, uint lParam);

        [DllImport("user32.dll", EntryPoint = "SetWindowLongA", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("kernel32.dll", EntryPoint = "GetLastError", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        static extern uint GetLastError();

        [DllImport("user32.dll", EntryPoint = "GetWindowLongA", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", EntryPoint = "GetParent", ExactSpelling = true, CharSet = CharSet.Unicode, SetLastError = true)]
        static extern IntPtr GetParent(IntPtr hWnd);

        [DllImport("user32.dll")]
        static extern bool SetLayeredWindowAttributes(IntPtr hwnd, uint crKey, byte bAlpha, uint dwFlags);

        [DllImport("user32.dll")]
        static extern bool GetLayeredWindowAttributes(IntPtr hwnd, out uint crKey, out byte bAlpha, out uint dwFlags);
        #endregion

        public IntPtr Handle { get; private set; }
        public IntPtr ControlHandle { get; private set; }

        public WindowControl(IntPtr handle, IntPtr controlHandle)
        {
            // Assign handles
            this.Handle = handle;
            this.ControlHandle = controlHandle;
        }

        #region Statics
        public static IntPtr GetHandle(String title, String className = null)
        {
            var handle = FindWindow(className, title);

            if (handle.ToInt32() == 0)
                throw new Exception("Handle not found");

            return handle;
        }

        public static IntPtr GetControlHandle(String title, String className, String control)
        {
            var winHandle = GetHandle(title);
            var handle = FindWindowEx(winHandle, IntPtr.Zero, className, control);

            if (handle.ToInt32() == 0)
                throw new Exception("ControlHandle not found");

            return handle;
        }

        public static List<IntPtr> GetAllChildHandles(IntPtr handle)
        {
            List<IntPtr> childHandles = new List<IntPtr>();

            GCHandle gcChildhandlesList = GCHandle.Alloc(childHandles);
            IntPtr pointerChildHandlesList = GCHandle.ToIntPtr(gcChildhandlesList);

            try
            {
                EnumWindowProc childProc = new EnumWindowProc((hWnd, lParam) =>
                {
                    GCHandle _gcChildhandlesList = GCHandle.FromIntPtr(lParam);

                    if (_gcChildhandlesList == null || _gcChildhandlesList.Target == null)
                    {
                        return false;
                    }

                    List<IntPtr> _childHandles = _gcChildhandlesList.Target as List<IntPtr>;
                    _childHandles.Add(hWnd);

                    return true;
                });
                EnumChildWindows(handle, childProc, pointerChildHandlesList);
            }
            finally
            {
                gcChildhandlesList.Free();
            }

            return childHandles;
        }

        public static int ColorToInt(Color color)
        {
            var hexString = String.Format("{0}{1}{2}",
                                 color.R.ToString("X2"),
                                 color.G.ToString("X2"),
                                 color.B.ToString("X2"));

            return Convert.ToInt32(hexString, 16);
        }

        public static int GetPixel(Bitmap image, int x, int y)
        {
            return ColorToInt(image.GetPixel(x, y));
        }
        #endregion

        #region Window
        public Bitmap CaptureFrame(bool backgroundMode = true)
        {
            Bitmap image = null;

            IntPtr targetHandle = this.ControlHandle;

            if (backgroundMode)
            {
                image = BackgroundCapture.CaptureWindow(targetHandle) as Bitmap;
            }
            else
            {
                image = ForegroundCapture.CaptureWindow(targetHandle);
            }

            return image;
        }

        public void Show()
        {
            ShowWindow(this.Handle, SW_SHOW);
        }

        public void Hide()
        {
            ShowWindow(this.Handle, SW_HIDE);
        }

        public void Minimize()
        {
            ShowWindow(this.Handle, SW_MINIMIZE);
        }

        public void Restore()
        {
            ShowWindow(this.Handle, SW_RESTORE);
        }

        public void SetOpacity(int value)
        {
            SetWindowLong(this.Handle, GWL_EXSTYLE, GetWindowLong(Handle, GWL_EXSTYLE) ^ WS_EX_LAYERED);
            SetLayeredWindowAttributes(this.Handle, 0, (byte)value, LWA_ALPHA);
        }

        public byte GetOpacity()
        {
            uint crKey;
            byte bAlpha;
            uint dwFlags;

            GetLayeredWindowAttributes(this.Handle, out crKey, out bAlpha, out dwFlags);
            return bAlpha;
        }

        public Size GetWindowSize()
        {
            Rect rect = new Rect();
            GetWindowRect(this.Handle, ref rect);
            return new Size(rect.Right - rect.Left, rect.Bottom - rect.Top);
        }

        public void ResizeWindow(int width, int height, bool fixedSize = false)
        {
            if (fixedSize)
            {
                // Retrieves styles information about the window:
                int GWL_STYLE = -16; // Window styles request.
                long dwStyle = GetWindowLong(this.Handle, GWL_STYLE);

                //dwStyle &= ~(WS_SIZEBOX | WS_MAXIMIZEBOX);
                dwStyle &= ~(WS_OVERLAPPED | WS_CAPTION | WS_SYSMENU | WS_MINIMIZEBOX | WS_MAXIMIZEBOX);
                int retval = SetWindowLong(this.Handle, GWL_STYLE, (int)dwStyle);
            }

            SetWindowPos(this.Handle, IntPtr.Zero, 0, 0, width, height, SWP_NOMOVE);
        }

        public void ResizeWindow(Size size, bool fixedSize = false)
        {
            ResizeWindow(size.Width, size.Height, fixedSize);
        }

        public void FocusWindow()
        {
            SwitchToThisWindow(this.Handle, false);
            //SetActiveWindow(this.Handle);
            //SetForegroundWindow(this.Handle);

            System.Threading.Thread.Sleep(50);
        }

        public void BringToFront()
        {
            //SwitchToThisWindow(this.Handle, false);
            //SetActiveWindow(this.Handle);
            SetForegroundWindow(this.Handle);

            System.Threading.Thread.Sleep(50);
        }

        public List<string> GetText(int bufferLength = 32)
        {
            var list = new List<string>();

            foreach (var hdl in GetAllChildHandles(this.Handle))
            {
                var sb = new StringBuilder(bufferLength);
                GetWindowText(hdl, sb, bufferLength);

                var wholeString = sb.ToString();
                if (!string.IsNullOrWhiteSpace(wholeString))
                {
                    list.Add(wholeString);
                }
            }

            return list;
        }
        #endregion
    }
}
