// PS4MacroAPI (File: Bridge/IScriptHost.cs)
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
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PS4MacroAPI
{
    /// <summary>
    /// Interface for ScriptHost class
    /// </summary>
    public interface IScriptHost
    {
        /// <summary>
        /// The host form
        /// </summary>
        Form HostForm { get; }

        /// <summary>
        /// Is the host running
        /// </summary>
        bool IsRunning { get; }

        /// <summary>
        /// Is the host paused
        /// </summary>
        bool IsPaused { get; }

        /// <summary>
        /// The background worker
        /// </summary>
        BackgroundWorker Worker { get; }


        /* Relay Methods */

        /// <summary>
        /// Wait and block execution for a certian amount of time
        /// </summary>
        /// <param name="timeout"></param>
        /// <param name="checkInterval"></param>
        void Sleep(int timeout, int checkInterval = 100);

        /// <summary>
        /// Suspend update for a certain amount of time
        /// </summary>
        /// <param name="delay"></param>
        void Suspend(int delay);

        /// <summary>
        /// Play a macro
        /// </summary>
        /// <param name="sequence"></param>
        /// <param name="suspendDelay"></param>
        void PlayMacro(List<DualShockState> sequence, int suspendDelay = 0);

        /// <summary>
        /// Play a macro from path
        /// </summary>
        /// <param name="path"></param>
        /// <param name="suspendDelay"></param>
        void PlayMacro(string path, int suspendDelay = 0);

        /// <summary>
        /// Stop the macro
        /// </summary>
        void StopMacro();

        /* Emergency Methods */

        /// <summary>
        /// Play the script
        /// </summary>
        void Play();

        /// <summary>
        /// Pause the script
        /// </summary>
        void Pause();

        /// <summary>
        /// Stop the script
        /// </summary>
        void Stop();
    }
}
