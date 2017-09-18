// PS4MacroAPI (File: Bridge/ScriptBase.cs)
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

using PS4MacroAPI.Internal;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PS4MacroAPI
{
    public abstract class ScriptBase
    {
        public const int DEFAULT_PIXEL_TOLERANCE = 2;
        public const double DEFAULT_RECT_TOLERANCE = 95;

        public const int DEFAULT_PRESS_BUTTONS_DELAY = 150;
        public const int DEFAULT_SLEEP_CHECK_INTERVAL = 100;

        public IScriptHost Host { get; set; }
        public ScriptConfig Config { get; protected set; }
        public Form ScriptForm { get; protected set; }

        public bool IsInitialized { get; private set; }
        public DualShockState CurrentState { get; private set; }
        public Bitmap CurrentFrame { get; private set; }
        public Process RemotePlayProcess { get; private set; }

        private WindowControl WindowControl { get; set; }


        /* Virtual Methods */
        public virtual void Start() { }
        public virtual void Update() { }

        /* Constructor */
        public ScriptBase()
        {
            Host = null;
            Config = new ScriptConfig();
        }

        // Called when loop is created
        public bool Initialize()
        {
            // Find the process
            if (RemotePlayProcess == null || (RemotePlayProcess != null && RemotePlayProcess.HasExited))
            {
                RemotePlayProcess = ScriptUtility.FindRemotePlayProcess();
            }

            // Find panel in process
            var panelHandle = ScriptUtility.FindStreamingPanel(RemotePlayProcess);

            // Check for panel
            if (panelHandle == null || panelHandle == IntPtr.Zero)
            {
                throw new Exception("Streaming panel not found in child handles");
            }

            // Create WindowControl
            WindowControl = new WindowControl(RemotePlayProcess.MainWindowHandle, panelHandle);

            // Resize window
            WindowControl.ResizeWindow(Config.TargetSize);

            // Remember that this happened
            IsInitialized = true;
            return true;
        }


        public ScriptBase Press(DualShockState state, int delay = DEFAULT_PRESS_BUTTONS_DELAY)
        {
            SetButtons(state);
            Host.Sleep(delay);
            ClearButtons();

            return this;
        }

        public ScriptBase SetButtons(DualShockState state)
        {
            CurrentState = state;
            return this;
        }

        public ScriptBase ClearButtons()
        {
            CurrentState = null;
            return this;
        }

        public Bitmap CaptureFrame()
        {
            // Expose old frame
            CurrentFrame?.Dispose();
            // Capture and cache
            CurrentFrame = WindowControl.CaptureFrame();
            return CurrentFrame;
        }

        public Bitmap CropFrame(Bitmap frame, Rectangle rect)
        {
            return frame.Clone(rect, frame.PixelFormat);
        }

        public Bitmap CropFrame(Rectangle rect)
        {
            return CropFrame(CurrentFrame, rect);
        }

        public int GetPixel(int x, int y)
        {
            try
            {
                return WindowControl.ColorToInt(CurrentFrame.GetPixel(x, y));
            }
            catch (Exception) { return -1; }
        }

        public bool MatchTemplate(int x, int y, int color, int tolerance = DEFAULT_PIXEL_TOLERANCE)
        {
            var pixel = GetPixel(x, y);

            var c1 = Color.FromArgb(pixel);
            var c2 = Color.FromArgb(color);

            var difference = (Math.Max(c1.R, c2.R) - Math.Min(c1.R, c2.R)) +
                             (Math.Max(c1.G, c2.G) - Math.Min(c1.G, c2.G)) +
                             (Math.Max(c1.B, c2.B) - Math.Min(c1.B, c2.B));

            return difference <= tolerance;
        }

        public bool MatchTemplate(PixelMap pixel, int tolerance = DEFAULT_PIXEL_TOLERANCE)
        {
            return MatchTemplate(pixel.X, pixel.Y, pixel.Color, tolerance);
        }

        public bool MatchTemplate(Rectangle rect, ulong targetHash, double similarity = DEFAULT_RECT_TOLERANCE)
        {
            if (CurrentFrame == null)
                return false;

            var cropped = CropFrame(rect);
            var hash = ImageHashing.AverageHash(cropped);

            return ImageHashing.Similarity(hash, targetHash) >= similarity;
        }

        public bool MatchTemplate(RectMap rect, double similarity = DEFAULT_RECT_TOLERANCE)
        {
            return MatchTemplate(new Rectangle(rect.X, rect.Y, rect.Width, rect.Height), rect.Hash, similarity);
        }

        public void HandleScenes(Action<Scene> beforeEnter = null)
        {
            CaptureFrame();

            var scene = Scene.Search(this);
            if (scene != null)
            {
                beforeEnter?.Invoke(scene);
                scene.OnMatched(this);
            }
        }

        /**/
        /* Relay to Host
        /**/

        public ScriptBase Sleep(int timeout, int checkInterval = DEFAULT_SLEEP_CHECK_INTERVAL)
        {
            Host.Sleep(timeout, checkInterval);
            return this;
        }

        public ScriptBase Suspend(int delay)
        {
            Host.Suspend(delay);
            return this;
        }

        public ScriptBase PlayMacro(List<DualShockState> sequence, int suspendDelay = 0)
        {
            Host.PlayMacro(sequence);
            return this;
        }

        public ScriptBase PlayMacro(string path, int suspendDelay = 0)
        {
            Host.PlayMacro(path);
            return this;
        }

        public ScriptBase StopMacro()
        {
            Host.StopMacro();
            return this;
        }
    }
}
