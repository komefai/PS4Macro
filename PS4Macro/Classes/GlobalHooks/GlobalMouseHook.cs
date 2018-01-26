using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;

// https://stackoverflow.com/a/34384189
namespace PS4Macro.Classes.GlobalHooks
{
    public class GlobalMouseHookEventArgs : HandledEventArgs
    {
        public GlobalMouseHook.MouseState MouseState { get; private set; }
        public GlobalMouseHook.LowLevelMouseInputEvent MouseData { get; private set; }

        public GlobalMouseHookEventArgs(
            GlobalMouseHook.LowLevelMouseInputEvent mouseData,
            GlobalMouseHook.MouseState mouseState)
        {
            MouseData = mouseData;
            MouseState = mouseState;
        }
    }

    //Based on https://gist.github.com/Stasonix
    public class GlobalMouseHook : IDisposable
    {
        public event EventHandler<GlobalMouseHookEventArgs> MouseEvent;

        public GlobalMouseHook()
        {
            _windowsHookHandle = IntPtr.Zero;
            _user32LibraryHandle = IntPtr.Zero;
            _hookProc = LowLevelMouseProc; // we must keep alive _hookProc, because GC is not aware about SetWindowsHookEx behaviour.

            _user32LibraryHandle = LoadLibrary("User32");
            if (_user32LibraryHandle == IntPtr.Zero)
            {
                int errorCode = Marshal.GetLastWin32Error();
                throw new Win32Exception(errorCode, $"Failed to load library 'User32.dll'. Error {errorCode}: {new Win32Exception(Marshal.GetLastWin32Error()).Message}.");
            }



            _windowsHookHandle = SetWindowsHookEx(WH_MOUSE_LL, _hookProc, _user32LibraryHandle, 0);
            if (_windowsHookHandle == IntPtr.Zero)
            {
                int errorCode = Marshal.GetLastWin32Error();
                throw new Win32Exception(errorCode, $"Failed to adjust mouse hooks for '{Process.GetCurrentProcess().ProcessName}'. Error {errorCode}: {new Win32Exception(Marshal.GetLastWin32Error()).Message}.");
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // because we can unhook only in the same thread, not in garbage collector thread
                if (_windowsHookHandle != IntPtr.Zero)
                {
                    if (!UnhookWindowsHookEx(_windowsHookHandle))
                    {
                        int errorCode = Marshal.GetLastWin32Error();
                        throw new Win32Exception(errorCode, $"Failed to remove mouse hooks for '{Process.GetCurrentProcess().ProcessName}'. Error {errorCode}: {new Win32Exception(Marshal.GetLastWin32Error()).Message}.");
                    }
                    _windowsHookHandle = IntPtr.Zero;

                    // ReSharper disable once DelegateSubtraction
                    _hookProc -= LowLevelMouseProc;
                }
            }

            if (_user32LibraryHandle != IntPtr.Zero)
            {
                if (!FreeLibrary(_user32LibraryHandle)) // reduces reference to library by 1.
                {
                    int errorCode = Marshal.GetLastWin32Error();
                    throw new Win32Exception(errorCode, $"Failed to unload library 'User32.dll'. Error {errorCode}: {new Win32Exception(Marshal.GetLastWin32Error()).Message}.");
                }
                _user32LibraryHandle = IntPtr.Zero;
            }
        }

        ~GlobalMouseHook()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private IntPtr _windowsHookHandle;
        private IntPtr _user32LibraryHandle;
        private HookProc _hookProc;

        delegate IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll")]
        private static extern IntPtr LoadLibrary(string lpFileName);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern bool FreeLibrary(IntPtr hModule);

        /// <summary>
        /// The SetWindowsHookEx function installs an application-defined hook procedure into a hook chain.
        /// You would install a hook procedure to monitor the system for certain types of events. These events are
        /// associated either with a specific thread or with all threads in the same desktop as the calling thread.
        /// </summary>
        /// <param name="idHook">hook type</param>
        /// <param name="lpfn">hook procedure</param>
        /// <param name="hMod">handle to application instance</param>
        /// <param name="dwThreadId">thread identifier</param>
        /// <returns>If the function succeeds, the return value is the handle to the hook procedure.</returns>
        [DllImport("USER32", SetLastError = true)]
        static extern IntPtr SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hMod, int dwThreadId);

        /// <summary>
        /// The UnhookWindowsHookEx function removes a hook procedure installed in a hook chain by the SetWindowsHookEx function.
        /// </summary>
        /// <param name="hhk">handle to hook procedure</param>
        /// <returns>If the function succeeds, the return value is true.</returns>
        [DllImport("USER32", SetLastError = true)]
        public static extern bool UnhookWindowsHookEx(IntPtr hHook);

        /// <summary>
        /// The CallNextHookEx function passes the hook information to the next hook procedure in the current hook chain.
        /// A hook procedure can call this function either before or after processing the hook information.
        /// </summary>
        /// <param name="hHook">handle to current hook</param>
        /// <param name="code">hook code passed to hook procedure</param>
        /// <param name="wParam">value passed to hook procedure</param>
        /// <param name="lParam">value passed to hook procedure</param>
        /// <returns>If the function succeeds, the return value is true.</returns>
        [DllImport("USER32", SetLastError = true)]
        static extern IntPtr CallNextHookEx(IntPtr hHook, int code, IntPtr wParam, IntPtr lParam);

        [StructLayout(LayoutKind.Sequential)]
        public struct LowLevelMouseInputEvent
        {
            /// <summary>
            /// Coordinates of the mouse
            /// </summary>
            public POINT Point;

            /// <summary>
            /// Mouse code
            /// </summary>
            public int MouseCode;

            /// <summary>
            /// The extended-key flag, event-injected Flags, context code, and transition-state flag. This member is specified as follows. An application can use the following values to test the keystroke Flags. Testing LLKHF_INJECTED (bit 4) will tell you whether the event was injected. If it was, then testing LLKHF_LOWER_IL_INJECTED (bit 1) will tell you whether or not the event was injected from a process running at lower integrity level.
            /// </summary>
            public int Flags;

            /// <summary>
            /// The time stamp stamp for this message, equivalent to what GetMessageTime would return for this message.
            /// </summary>
            public int TimeStamp;

            /// <summary>
            /// Additional information associated with the message. 
            /// </summary>
            public IntPtr AdditionalInformation;
        }

        private const int WH_MOUSE_LL = 14;

        public enum MouseState
        {
            LeftButtonDown = 0x0201,
            LeftButtonUp = 0x0202,
            Move = 0x0200,
            Wheel = 0x020A,
            RightButtonDown = 0x0204,
            RightButtonUp = 0x0205,
            MiddleButtonDown = 0x0207,
            MiddleButtonUp = 0x0208
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;
        }

        public IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            bool fEatMouseStroke = false;

            var wparamTyped = wParam.ToInt32();
            if (Enum.IsDefined(typeof(MouseState), wparamTyped) && nCode >= 0)
            {
                object o = Marshal.PtrToStructure(lParam, typeof(LowLevelMouseInputEvent));
                LowLevelMouseInputEvent p = (LowLevelMouseInputEvent)o;

                var eventArguments = new GlobalMouseHookEventArgs(p, (MouseState)wparamTyped);

                EventHandler<GlobalMouseHookEventArgs> handler = MouseEvent;
                handler?.Invoke(this, eventArguments);

                fEatMouseStroke = eventArguments.Handled;
            }

            return fEatMouseStroke ? (IntPtr)1 : CallNextHookEx(IntPtr.Zero, nCode, wParam, lParam);
        }
    }
}