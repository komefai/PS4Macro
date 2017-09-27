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
    /// <summary>
    /// Entry point for all scripts
    /// </summary>
    public abstract class ScriptBase
    {
        /// <summary>
        /// The default pixel tolerance
        /// </summary>
        public const int DEFAULT_PIXEL_TOLERANCE = 2;

        /// <summary>
        /// The default rectangle tolerance
        /// </summary>
        public const double DEFAULT_RECT_TOLERANCE = 95;

        /// <summary>
        /// The default press buttons delay
        /// </summary>
        public const int DEFAULT_PRESS_BUTTONS_DELAY = 150;

        /// <summary>
        /// The default sleep check interval delay
        /// </summary>
        public const int DEFAULT_SLEEP_CHECK_INTERVAL = 100;


        /// <summary>
        /// Thes script host
        /// </summary>
        public IScriptHost Host { get; set; }

        /// <summary>
        /// Script configuration
        /// </summary>
        public ScriptConfig Config { get; protected set; }

        /// <summary>
        /// Script form
        /// </summary>
        public Form ScriptForm { get; protected set; }


        /// <summary>
        /// Is the script initialized
        /// </summary>
        public bool IsInitialized { get; private set; }

        /// <summary>
        /// The current button state
        /// </summary>
        public DualShockState CurrentState { get; private set; }

        /// <summary>
        /// The current screenshot frame
        /// </summary>
        public Bitmap CurrentFrame { get; private set; }

        /// <summary>
        /// Reference to PS4 Remote Play process
        /// </summary>
        public Process RemotePlayProcess { get; private set; }

        /// <summary>
        /// Internal object for controlling windows
        /// </summary>
        private WindowControl WindowControl { get; set; }


        /* Virtual Methods */

        /// <summary>
        /// Called when the user pressed play
        /// </summary>
        public virtual void Start() { }

        /// <summary>
        /// Called every interval set by LoopDelay
        /// </summary>
        public virtual void Update() { }

        /// <summary>
        /// Called when the user pressed stop
        /// </summary>
        public virtual void OnStopped() { }

        /// <summary>
        /// Called when the user pressed pause
        /// </summary>
        public virtual void OnPaused() { }

        /// <summary>
        /// Initializes a new instance of <see cref="ScriptBase"/>
        /// </summary>
        public ScriptBase()
        {
            Host = null;
            Config = new ScriptConfig();
        }

        /// <summary>
        /// Called when loop is created
        /// </summary>
        /// <returns>Returns true on success</returns>
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
            if (Config.EnableCapture && (panelHandle == null || panelHandle == IntPtr.Zero))
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

        /// <summary>
        /// Press buttons
        /// </summary>
        /// <param name="state"></param>
        /// <param name="delay"></param>
        /// <returns>Returns self</returns>
        public ScriptBase Press(DualShockState state, int delay = DEFAULT_PRESS_BUTTONS_DELAY)
        {
            SetButtons(state);
            Host.Sleep(delay);
            ClearButtons();

            return this;
        }

        /// <summary>
        /// Set buttons manually
        /// </summary>
        /// <param name="state"></param>
        /// <returns>Returns self</returns>
        public ScriptBase SetButtons(DualShockState state)
        {
            CurrentState = state;
            return this;
        }

        /// <summary>
        /// Clear buttons manually
        /// </summary>
        /// <returns>Returns self</returns>
        public ScriptBase ClearButtons()
        {
            CurrentState = null;
            return this;
        }

        /// <summary>
        /// Take a screenshot as bitmap
        /// </summary>
        /// <returns>Returns the captured screenshot</returns>
        public Bitmap CaptureFrame()
        {
            // Expose old frame
            CurrentFrame?.Dispose();
            // Capture and cache
            CurrentFrame = WindowControl.CaptureFrame();
            return CurrentFrame;
        }

        /// <summary>
        /// Crop a bitmap
        /// </summary>
        /// <param name="frame"></param>
        /// <param name="rect"></param>
        /// <returns>Returns the cropped bitmap</returns>
        public Bitmap CropFrame(Bitmap frame, Rectangle rect)
        {
            return frame.Clone(rect, frame.PixelFormat);
        }

        /// <summary>
        /// Crop current frame
        /// </summary>
        /// <param name="rect"></param>
        /// <returns>Returns the cropped frame</returns>
        public Bitmap CropFrame(Rectangle rect)
        {
            return CropFrame(CurrentFrame, rect);
        }

        /// <summary>
        /// Get pixel from a bitmap
        /// </summary>
        /// <param name="frame"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>Returns the color</returns>
        public int GetPixel(Bitmap frame, int x, int y)
        {
            try
            {
                return WindowControl.ColorToInt(frame.GetPixel(x, y));
            }
            catch (Exception) { return -1; }
        }

        /// <summary>
        /// Get pixel from current frame
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>Returns the color</returns>
        public int GetPixel(int x, int y)
        {
            return GetPixel(CurrentFrame, x, y);
        }

        /// <summary>
        /// Match color template on a bitmap
        /// </summary>
        /// <param name="frame"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="color"></param>
        /// <param name="tolerance"></param>
        /// <returns>Returns true if matched</returns>
        public bool MatchTemplate(Bitmap frame, int x, int y, int color, int tolerance = DEFAULT_PIXEL_TOLERANCE)
        {
            var pixel = GetPixel(frame, x, y);

            var c1 = Color.FromArgb(pixel);
            var c2 = Color.FromArgb(color);

            var difference = (Math.Max(c1.R, c2.R) - Math.Min(c1.R, c2.R)) +
                             (Math.Max(c1.G, c2.G) - Math.Min(c1.G, c2.G)) +
                             (Math.Max(c1.B, c2.B) - Math.Min(c1.B, c2.B));

            return difference <= tolerance;
        }

        /// <summary>
        /// Match color template on current frame
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="color"></param>
        /// <param name="tolerance"></param>
        /// <returns>Returns true if matched</returns>
        public bool MatchTemplate(int x, int y, int color, int tolerance = DEFAULT_PIXEL_TOLERANCE)
        {
            return MatchTemplate(CurrentFrame, x, y, color, tolerance);
        }

        /// <summary>
        /// Match PixelMap on a bitmap
        /// </summary>
        /// <param name="frame"></param>
        /// <param name="pixel"></param>
        /// <param name="tolerance"></param>
        /// <returns>Returns true if matched</returns>
        public bool MatchTemplate(Bitmap frame, PixelMap pixel, int tolerance = DEFAULT_PIXEL_TOLERANCE)
        {
            return MatchTemplate(frame, pixel.X, pixel.Y, pixel.Color, tolerance);
        }

        /// <summary>
        /// Match PixelMap on current frame
        /// </summary>
        /// <param name="pixel"></param>
        /// <param name="tolerance"></param>
        /// <returns>Returns true if matched</returns>
        public bool MatchTemplate(PixelMap pixel, int tolerance = DEFAULT_PIXEL_TOLERANCE)
        {
            return MatchTemplate(CurrentFrame, pixel.X, pixel.Y, pixel.Color, tolerance);
        }

        /// <summary>
        /// Match rectangle template on a bitmap
        /// </summary>
        /// <param name="frame"></param>
        /// <param name="rect"></param>
        /// <param name="targetHash"></param>
        /// <param name="similarity"></param>
        /// <returns>Returns true if matched</returns>
        public bool MatchTemplate(Bitmap frame, Rectangle rect, ulong targetHash, double similarity = DEFAULT_RECT_TOLERANCE)
        {
            if (frame == null)
                return false;

            var cropped = CropFrame(frame, rect);
            var hash = ImageHashing.AverageHash(cropped);

            return ImageHashing.Similarity(hash, targetHash) >= similarity;
        }

        /// <summary>
        /// Match rectangle template bitmap on current frame
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="targetHash"></param>
        /// <param name="similarity"></param>
        /// <returns>Returns true if matched</returns>
        public bool MatchTemplate(Rectangle rect, ulong targetHash, double similarity = DEFAULT_RECT_TOLERANCE)
        {
            return MatchTemplate(CurrentFrame, rect, targetHash, similarity);
        }

        /// <summary>
        /// Match RectMap on a bitmap
        /// </summary>
        /// <param name="frame"></param>
        /// <param name="rect"></param>
        /// <param name="similarity"></param>
        /// <returns>Returns true if matched</returns>
        public bool MatchTemplate(Bitmap frame, RectMap rect, double similarity = DEFAULT_RECT_TOLERANCE)
        {
            return MatchTemplate(new Rectangle(rect.X, rect.Y, rect.Width, rect.Height), rect.Hash, similarity);
        }

        /// <summary>
        /// Match RectMap on current frame
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="similarity"></param>
        /// <returns>Returns true if matched</returns>
        public bool MatchTemplate(RectMap rect, double similarity = DEFAULT_RECT_TOLERANCE)
        {
            return MatchTemplate(CurrentFrame, new Rectangle(rect.X, rect.Y, rect.Width, rect.Height), rect.Hash, similarity);
        }

        /// <summary>
        /// Helper method for scenes API
        /// </summary>
        /// <param name="beforeEnter"></param>
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

        /// <summary>
        /// Wait and block execution for a certian amount of time
        /// </summary>
        /// <param name="timeout"></param>
        /// <param name="checkInterval"></param>
        /// <returns>Returns self</returns>
        public ScriptBase Sleep(int timeout, int checkInterval = DEFAULT_SLEEP_CHECK_INTERVAL)
        {
            Host.Sleep(timeout, checkInterval);
            return this;
        }

        /// <summary>
        /// Suspend update for a certain amount of time
        /// </summary>
        /// <param name="delay"></param>
        /// <returns>Returns self</returns>
        public ScriptBase Suspend(int delay)
        {
            Host.Suspend(delay);
            return this;
        }

        /// <summary>
        /// Play a macro
        /// </summary>
        /// <param name="sequence"></param>
        /// <param name="suspendDelay"></param>
        /// <returns>Returns self</returns>
        public ScriptBase PlayMacro(List<DualShockState> sequence, int suspendDelay = 0)
        {
            Host.PlayMacro(sequence);
            return this;
        }

        /// <summary>
        /// Play a macro from path
        /// </summary>
        /// <param name="path"></param>
        /// <param name="suspendDelay"></param>
        /// <returns>Returns self</returns>
        public ScriptBase PlayMacro(string path, int suspendDelay = 0)
        {
            Host.PlayMacro(path);
            return this;
        }

        /// <summary>
        /// Stop the macro
        /// </summary>
        /// <returns>Returns self</returns>
        public ScriptBase StopMacro()
        {
            Host.StopMacro();
            return this;
        }
    }
}
