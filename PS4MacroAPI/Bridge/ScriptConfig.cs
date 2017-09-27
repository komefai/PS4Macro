// PS4MacroAPI (File: Bridge/ScriptConfig.cs)
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
using System.Drawing;
using System.Linq;
using System.Text;

namespace PS4MacroAPI
{
    /// <summary>
    /// Config for scripts
    /// </summary>
    public class ScriptConfig
    {
        /// <summary>
        /// Default width for TargetSize
        /// </summary>
        public const int DEFAULT_SCREEN_WIDTH = 1024;
        /// <summary>
        /// Default height for TargetSize
        /// </summary>
        public const int DEFAULT_SCREEN_HEIGHT = 768;

        /// <summary>
        /// Name of the script
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Delay between updates
        /// </summary>
        public int LoopDelay { get; set; }

        /// <summary>
        /// Target size for PS4 Remote Play
        /// </summary>
        public Size TargetSize { get; set; }

        /// <summary>
        /// Should throw exceptions
        /// </summary>
        public bool ThrowExceptions { get; set; }

        /// <summary>
        /// Show stack trace when exception is thrown
        /// </summary>
        public bool ShowStackTrace { get; set; }

        /// <summary>
        /// Show error message box
        /// </summary>
        public bool ShowError { get; set; }

        /// <summary>
        /// Enable capture screenshots
        /// </summary>
        public bool EnableCapture { get; set; }

        /// <summary>
        /// Show form when script is started
        /// </summary>
        public bool ShowFormOnStart { get; set; }

        /// <summary>
        /// Automatically move form when shown
        /// </summary>
        public bool AutoFormLocation { get; set; }

        /// <summary>
        /// List of scenes for scenes API
        /// </summary>
        public List<Scene> Scenes { get; set; }

        /// <summary>
        /// Initializes a new instance of <see cref="ScriptConfig"/>
        /// </summary>
        public ScriptConfig()
        {
            Name = "Untitled Script";
            LoopDelay = 500;
            TargetSize = new Size(DEFAULT_SCREEN_WIDTH, DEFAULT_SCREEN_HEIGHT);
            ThrowExceptions = true;
            ShowStackTrace = true;
            ShowError = true;
            EnableCapture = true;
            ShowFormOnStart = true;
            AutoFormLocation = true;
            Scenes = null;
        }
    }
}
