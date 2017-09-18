// PS4Macro (File: Classes/ScriptHost.cs)
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

using PS4MacroAPI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace PS4Macro.Classes
{
    public class ScriptHost : IScriptHost, INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        public ScriptBase Script { get; private set; }
        public MacroPlayer MacroPlayer { get; private set; }

        private bool m_IsRunning = false;
        public bool IsRunning
        {
            get { return m_IsRunning; }
            private set
            {
                if (value != m_IsRunning)
                {
                    m_IsRunning = value;
                    NotifyPropertyChanged("IsRunning");
                }
            }
        }

        private bool m_IsPaused = false;
        public bool IsPaused
        {
            get { return m_IsPaused; }
            private set
            {
                if (value != m_IsPaused)
                {
                    m_IsPaused = value;
                    NotifyPropertyChanged("IsPaused");
                }
            }
        }

        public int SuspendCounter { get; private set; }

        public BackgroundWorker Worker { get; private set; }


        /* Constructor */
        public ScriptHost(ScriptBase script)
        {
            Script = script;
            Script.Host = this;
            MacroPlayer = new MacroPlayer();

            var scriptForm = Script.ScriptForm;
            if (scriptForm != null)
            {
                // Intercept form closing
                scriptForm.FormClosing += (_sender, _e) =>
                {
                    if (_e.CloseReason == CloseReason.UserClosing)
                    {
                        _e.Cancel = true;
                        scriptForm.Hide();
                    }
                };
            }
        }

        public void Sleep(int timeout, int checkInterval = 100)
        {
            if (Worker == null || timeout <= 0)
                return;

            int waited = 0;
            while (waited < timeout)
            {
                if (Worker == null || Worker.CancellationPending)
                    break;

                Thread.Sleep(checkInterval);
                waited += checkInterval;
            }
        }

        public void Suspend(int delay)
        {
            SuspendCounter = delay;
        }

        public void PlayMacro(List<DualShockState> sequence, int suspendDelay = 0)
        {
            MacroPlayer.Sequence = ScriptHostUtility.ConvertAPIToInterceptorSequence(sequence);
            MacroPlayer.Play();

            if (suspendDelay > 0)
            {
                Suspend(suspendDelay);
            }
        }

        public void PlayMacro(string path, int suspendDelay = 0)
        {
            MacroPlayer.LoadFile(path);
            MacroPlayer.Play();

            if (suspendDelay > 0)
            {
                Suspend(suspendDelay);
            }
        }

        public void StopMacro()
        {
            MacroPlayer.Stop();
        }

        public BackgroundWorker Start()
        {
            // Ignore if already running
            if (IsRunning)
                return null;

            // Call internal initialize method
            Script.Initialize();

            // Create worker
            Worker = new BackgroundWorker
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true
            };
            Worker.DoWork += MainLoop;
            Worker.RunWorkerAsync();

            IsRunning = true;

            return Worker;
        }

        public void Play()
        {
            Start();

            if (MacroPlayer.IsPaused)
            {
                MacroPlayer.Play();
            }

            IsPaused = false;
        }

        public void Pause()
        {
            IsPaused = true;

            if (MacroPlayer.IsPlaying)
            {
                MacroPlayer.Pause();
            }
        }

        public void Stop()
        {
            if (Worker != null)
            {
                Worker.CancelAsync();
            }

            MacroPlayer.Stop();
            SuspendCounter = 0;
            IsRunning = false;
            IsPaused = false;
        }

        public void OnReceiveData(ref PS4RemotePlayInterceptor.DualShockState state)
        {
            if (MacroPlayer.IsPlaying)
            {
                MacroPlayer.OnReceiveData(ref state);
            }
            else if (Script.CurrentState != null)
            {
                state = ScriptHostUtility.ConvertAPIToInterceptorState(Script.CurrentState);
                state.ReportTimeStamp = DateTime.Now;

                // Replace battery status
                state.Battery = 100;
                state.IsCharging = true;
            }
        }

        #region Main Loop

        private void MainLoop(object sender, DoWorkEventArgs e)
        {
            // Start custom code in script
            Script.Start();

            while (!Worker.CancellationPending)
            {
                try
                {
                    //Console.WriteLine("Tick");

                    // Continue if script is null
                    if (Script == null)
                        continue;

                    // Continue if paused
                    if (IsPaused)
                        continue;

                    // Loop delay
                    var delay = Script.Config.LoopDelay;
                    if (delay > 0) Sleep(delay);

                    // Continue if still suspended
                    if (SuspendCounter > 0)
                    {
                        SuspendCounter -= delay;
                        continue;
                    }

                    // Call update
                    Script.Update();
                }
                catch (Exception ex)
                {
                    #if DEBUG
                    Console.WriteLine("MAIN LOOP ERROR: " + ex.Message);
                    #endif

                    if (Script.Config.ThrowExceptions)
                    {
                        throw;
                    }
                }
            }
        }
        #endregion

        public void ShowForm(Form parent)
        {
            var scriptForm = Script.ScriptForm;

            if (scriptForm == null)
                return;

            // Show form
            scriptForm.Show(parent);
            // Center form to parent
            scriptForm.Location = new Point(parent.Location.X + parent.Width / 2 - scriptForm.Width / 2, parent.Location.Y + parent.Height / 2 - scriptForm.Height / 2);
        }
    }
}
