// PS4Macro (File: Forms/MainForm.cs)
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

using PS4Macro.Classes;
using PS4Macro.Classes.GlobalHooks;
using PS4Macro.Classes.Remapping;
using PS4RemotePlayInterceptor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PS4Macro.Forms
{
    enum ControlMode
    {
        Macro,
        Script,
        Remapper,
        StatusChecker
    }

    public partial class MainForm : Form
    {
        private const string CURRENT_TICK_DEFAULT_TEXT = "-";

        private GlobalKeyboardHook m_GlobalKeyboardHook;
        private GlobalMouseHook m_GlobalMouseHook;

        private MacroPlayer m_MacroPlayer;
        private Remapper m_Remapper;
        private StatusChecker m_StatusChecker;

        private ControlMode m_ControlMode;

        private PS4MacroAPI.ScriptBase m_SelectedScript;
        private ScriptHost m_ScriptHost;

        private SaveLoadHelper m_SaveLoadHelper;

        private Process m_RemotePlayProcess;

        /* Constructor */
        public MainForm()
        {
            InitializeComponent();

            // Setup global keyboard hook
            m_GlobalKeyboardHook = new GlobalKeyboardHook();
            m_GlobalKeyboardHook.KeyboardPressed += OnKeyPressed;

            // Setup global mouse hook
            m_GlobalMouseHook = new GlobalMouseHook();
            m_GlobalMouseHook.MouseEvent += OnMouseEvent;

            // Create macro player
            m_MacroPlayer = new MacroPlayer();
            m_MacroPlayer.Loop = true;
            m_MacroPlayer.RecordShortcut = true;
            m_MacroPlayer.PropertyChanged += MacroPlayer_PropertyChanged;

            // Create remapper
            m_Remapper = new Remapper();

            // Create status checker
            m_StatusChecker = new StatusChecker();

            // Set control mode
            SetControlMode(ControlMode.Macro);

            // Create save/load helper
            m_SaveLoadHelper = new SaveLoadHelper(this, m_MacroPlayer);
            m_SaveLoadHelper.PropertyChanged += SaveLoadHelper_PropertyChanged;

            // Initialize interceptor
            InitInterceptor();
        }

        private void InitInterceptor()
        {
            // Set controller emulation based on settings
            Interceptor.EmulateController = Program.Settings.EmulateController;
            emulatedToolStripStatusLabel.Visible = Program.Settings.EmulateController;

            // Enable watchdog based on settings
            if (!Program.Settings.AutoInject)
            {
                Interceptor.InjectionMode = InjectionMode.Compatibility;
            }

            // Inject if not bypassed
            if (!Program.Settings.BypassInjection)
            {
                // Attempt to inject into PS4 Remote Play
                try
                {
                    int pid = Interceptor.Inject();
                    m_RemotePlayProcess = Process.GetProcessById(pid);

                    // Set process
                    ForwardRemotePlayProcess();
                }
                // Injection failed
                catch (InterceptorException ex)
                {
                    // Only handle when PS4 Remote Play is in used by another injection
                    if (ex.InnerException != null && ex.InnerException.Message.Equals("STATUS_INTERNAL_ERROR: Unknown error in injected C++ completion routine. (Code: 15)"))
                    {
                        MessageBox.Show("The process has been injected by another executable. Restart PS4 Remote Play and try again.", "Injection Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Environment.Exit(-1);
                    }
                    else
                    {
                        // Handle exception if watchdog is disabled
                        if (!Program.Settings.AutoInject)
                        {
                            MessageBox.Show(string.Format("[{0}] - {1}", ex.GetType(), ex.Message), "Injection Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Environment.Exit(-1);
                        }
                    }
                }

                // Start watchdog to automatically inject when possible
                if (Program.Settings.AutoInject)
                {
                    Interceptor.Watchdog.Start();

                    // Watchdog callbacks
                    Interceptor.Watchdog.OnInjectionSuccess = () =>
                    {
                        ForwardRemotePlayProcess();
                    };
                    Interceptor.Watchdog.OnInjectionFailure = () =>
                    {

                    };
                }
            }
        }

        private void SetControlMode(ControlMode controlMode)
        {
            m_ControlMode = controlMode;

            if (m_ControlMode == ControlMode.Macro)
            {
                // Stop script and remove
                if (m_ScriptHost != null && m_ScriptHost.IsRunning)
                {
                    m_ScriptHost.Stop();
                    m_ScriptHost = null;
                }

                // Setup callback to interceptor
                Interceptor.Callback = new InterceptionDelegate(m_MacroPlayer.OnReceiveData);

                recordButton.Enabled = true;
                recordToolStripMenuItem.Enabled = true;
                loopCheckBox.Enabled = true;
                loopCheckBox.Checked = m_MacroPlayer.Loop;
                loopToolStripMenuItem.Enabled = true;
                recordOnTouchToolStripMenuItem.Enabled = true;
                scriptButton.Enabled = false;
                saveToolStripMenuItem.Enabled = true;
                saveAsToolStripMenuItem.Enabled = true;
                clearMacroToolStripMenuItem.Enabled = true;
                trimMacroToolStripMenuItem.Enabled = true;
            }
            else if (m_ControlMode == ControlMode.Script)
            {
                // Stop macro player
                if (m_MacroPlayer.IsRecording) m_MacroPlayer.Record();
                m_MacroPlayer.Stop();

                // Setup callback to interceptor
                Interceptor.Callback = new InterceptionDelegate(m_ScriptHost.OnReceiveData);

                recordButton.Enabled = false;
                recordToolStripMenuItem.Enabled = false;
                loopCheckBox.Enabled = false;
                loopCheckBox.Checked = false;
                loopToolStripMenuItem.Enabled = false;
                recordOnTouchToolStripMenuItem.Enabled = false;
                scriptButton.Enabled = true;
                saveToolStripMenuItem.Enabled = false;
                saveAsToolStripMenuItem.Enabled = false;
                clearMacroToolStripMenuItem.Enabled = false;
                trimMacroToolStripMenuItem.Enabled = false;
                currentTickToolStripStatusLabel.Text = CURRENT_TICK_DEFAULT_TEXT;
            }
            else if (m_ControlMode == ControlMode.Remapper)
            {
                // Stop macro player
                if (m_MacroPlayer.IsRecording) m_MacroPlayer.Record();
                m_MacroPlayer.Stop();

                // Stop script
                if (m_ScriptHost != null && m_ScriptHost.IsRunning) m_ScriptHost.Stop();

                // Setup callback to interceptor
                Interceptor.Callback = new InterceptionDelegate(m_Remapper.OnReceiveData);
            }
            else if (m_ControlMode == ControlMode.StatusChecker)
            {
                // Stop macro player
                if (m_MacroPlayer.IsRecording) m_MacroPlayer.Record();
                m_MacroPlayer.Stop();

                // Stop script
                if (m_ScriptHost != null && m_ScriptHost.IsRunning) m_ScriptHost.Stop();

                // Setup callback to interceptor
                Interceptor.Callback = new InterceptionDelegate(m_StatusChecker.OnReceiveData);
            }
        }

        private void TemporarilySetControlMode(ControlMode controlMode, Action action)
        {
            // Store current control mode and temporarily set it
            ControlMode oldControlMode = m_ControlMode;
            SetControlMode(controlMode);

            // Invoke action
            action?.Invoke();

            // Restore control mode
            SetControlMode(oldControlMode);
        }

        private void ForwardRemotePlayProcess()
        {
            m_Remapper.RemotePlayProcess = m_RemotePlayProcess;
            m_StatusChecker.RemotePlayProcess = m_RemotePlayProcess;
        }

        public void LoadMacro(string path)
        {
            SetControlMode(ControlMode.Macro);
            m_MacroPlayer.LoadFile(path);
        }

        public void LoadScript(string path)
        {
            var script = PS4MacroAPI.Internal.ScriptUtility.LoadScript(path);
            m_SelectedScript = script;

            m_ScriptHost = new ScriptHost(this, m_SelectedScript);
            m_ScriptHost.PropertyChanged += ScriptHost_PropertyChanged;

            SetControlMode(ControlMode.Script);
        }

        private void OnKeyPressed(object sender, GlobalKeyboardHookEventArgs e)
        {
            if (m_ControlMode == ControlMode.Remapper)
            {
                m_Remapper.OnKeyPressed(sender, e);
            }
            else if (m_ControlMode == ControlMode.StatusChecker)
            {
                m_StatusChecker.OnKeyPressed(sender, e);
            }
        }

        private void OnMouseEvent(object sender, GlobalMouseHookEventArgs e)
        {
            if (m_ControlMode == ControlMode.Remapper)
            {
                m_Remapper.OnMouseEvent(sender, e);
            }
            else if (m_ControlMode == ControlMode.StatusChecker)
            {
                m_StatusChecker.OnMouseEvent(sender, e);
            }
        }


        private void MainForm_Load(object sender, EventArgs e)
        {
            // Load startup file
            if (!string.IsNullOrWhiteSpace(Program.Settings.StartupFile))
                m_SaveLoadHelper.DirectLoad(Program.Settings.StartupFile);
        }

        /* Macro Player */
        #region MacroPlayer_PropertyChanged
        private void UpdateCurrentTick()
        {
            BeginInvoke((MethodInvoker)delegate
            {
                // Invalid sequence
                if (m_MacroPlayer.Sequence == null || m_MacroPlayer.Sequence.Count <= 0)
                {
                    currentTickToolStripStatusLabel.Text = CURRENT_TICK_DEFAULT_TEXT;
                }
                // Valid sequence
                else
                {
                    currentTickToolStripStatusLabel.Text = string.Format("{0}/{1}",
                        m_MacroPlayer.CurrentTick.ToString(),
                        m_MacroPlayer.Sequence.Count.ToString()
                    );
                }
            });
        }

        private void MacroPlayer_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "IsPlaying":
                    {
                        playButton.ForeColor = m_MacroPlayer.IsPlaying ? Color.Green : DefaultForeColor;
                        break;
                    }

                case "IsPaused":
                    {
                        playButton.ForeColor = m_MacroPlayer.IsPaused ? DefaultForeColor : playButton.ForeColor;
                        break;
                    }

                case "IsRecording":
                    {
                        recordButton.ForeColor = m_MacroPlayer.IsRecording ? Color.Red : DefaultForeColor;
                        currentTickToolStripStatusLabel.ForeColor = m_MacroPlayer.IsRecording ? Color.Red : DefaultForeColor;
                        break;
                    }

                case "CurrentTick":
                    {
                        UpdateCurrentTick();
                        break;
                    }

                case "Sequence":
                    {
                        UpdateCurrentTick();
                        break;
                    }

                case "Loop":
                    {
                        loopCheckBox.Checked = m_MacroPlayer.Loop;
                        loopToolStripMenuItem.Checked = m_MacroPlayer.Loop;
                        break;
                    }

                case "RecordShortcut":
                    {
                        recordOnTouchToolStripMenuItem.Checked = m_MacroPlayer.RecordShortcut;
                        break;
                    }
            }
        }
        #endregion

        /* Script Host */
        #region ScriptHost_PropertyChanged
        private void ScriptHost_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "IsRunning":
                    {
                        playButton.ForeColor = m_ScriptHost.IsRunning ? Color.Green : DefaultForeColor;
                        break;
                    }

                case "IsPaused":
                    {
                        if (m_ScriptHost.IsPaused && m_ScriptHost.IsRunning)
                        {
                            playButton.ForeColor = DefaultForeColor;
                        }
                        else if (!m_ScriptHost.IsPaused && m_ScriptHost.IsRunning)
                        {
                            playButton.ForeColor = Color.Green;
                        }
                        break;
                    }
            }
        }
        #endregion

        /* Save/Load Helper */
        #region SaveLoadHelper_PropertyChanged
        private void SaveLoadHelper_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "CurrentFile")
            {
                if (m_SaveLoadHelper.CurrentFile == null)
                {
                    fileNameToolStripStatusLabel.Text = SaveLoadHelper.DEFAULT_FILE_NAME;
                    currentTickToolStripStatusLabel.Text = CURRENT_TICK_DEFAULT_TEXT;
                }
                else
                {
                    fileNameToolStripStatusLabel.Text = System.IO.Path.GetFileName(m_SaveLoadHelper.CurrentFile);
                }
            }
        }
        #endregion

        /* Playback buttons methods */
        #region Playback Buttons

        private void playButton_Click(object sender, EventArgs e)
        {
            if (m_ControlMode == ControlMode.Macro)
            {
                m_MacroPlayer.Play();
            }
            else if (m_ControlMode == ControlMode.Script)
            {
                m_ScriptHost.Play();
            }
        }

        private void pauseButton_Click(object sender, EventArgs e)
        {
            if (m_ControlMode == ControlMode.Macro)
            {
                m_MacroPlayer.Pause();
            }
            else if (m_ControlMode == ControlMode.Script)
            {
                m_ScriptHost.Pause();
            }
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            if (m_ControlMode == ControlMode.Macro)
            {
                m_MacroPlayer.Stop();
            }
            else if (m_ControlMode == ControlMode.Script)
            {
                m_ScriptHost.Stop();
            }
        }

        private void recordButton_Click(object sender, EventArgs e)
        {
            if (m_ControlMode == ControlMode.Macro)
            {
                m_MacroPlayer.Record();
            }
        }

        private void loopCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (m_ControlMode == ControlMode.Macro)
            {
                m_MacroPlayer.Loop = loopCheckBox.Checked;
            }
        }
        #endregion

        /* Script buttons methods */
        #region Script Buttons
        private void scriptButton_Click(object sender, EventArgs e)
        {
            m_ScriptHost.ShowForm(this);
        }
        #endregion

        /* Menu strip methods */
        #region Menu Strip

        #region File
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetControlMode(ControlMode.Macro);
            m_MacroPlayer.Clear();
            m_SaveLoadHelper.ClearCurrentFile();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_SaveLoadHelper.Load();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_SaveLoadHelper.Save();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_SaveLoadHelper.SaveAs();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        #endregion

        #region Edit
        private void clearMacroToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_ControlMode == ControlMode.Macro)
            {
                m_MacroPlayer.Clear();
            }
        }

        private void trimMacroToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_MacroPlayer.Stop();

            var oldSequenceLength = m_MacroPlayer.Sequence.Count;
            m_MacroPlayer.Sequence = MacroUtility.TrimMacro(m_MacroPlayer.Sequence);

            // Show results
            var difference = oldSequenceLength - m_MacroPlayer.Sequence.Count;
            MessageBox.Show(
                $"{difference} frames removed" + "\n\n" + 
                $"Before: {oldSequenceLength} frames" + "\n" +
                $"After: {m_MacroPlayer.Sequence.Count} frames", "Trim Macro", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        #endregion

        #region Playback
        private void playToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_ControlMode == ControlMode.Macro)
            {
                m_MacroPlayer.Play();
            }
        }

        private void pauseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_ControlMode == ControlMode.Macro)
            {
                m_MacroPlayer.Pause();
            }
        }

        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_ControlMode == ControlMode.Macro)
            {
                m_MacroPlayer.Stop();
            }
        }

        private void recordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_ControlMode == ControlMode.Macro)
            {
                m_MacroPlayer.Record();
            }
        }

        private void loopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_ControlMode == ControlMode.Macro)
            {
                m_MacroPlayer.Loop = !loopToolStripMenuItem.Checked;
            }
        }

        private void recordOnTouchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_ControlMode == ControlMode.Macro)
            {
                m_MacroPlayer.RecordShortcut = !recordOnTouchToolStripMenuItem.Checked;
            }
        }
        #endregion

        #region Tools
        private void screenshotToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var backgroundMode = !(ModifierKeys == Keys.Shift);
            var frame = PS4MacroAPI.Internal.ScriptUtility.CaptureFrame(backgroundMode);
            var folder = "screenshots";

            // Create folder if not exist
            Directory.CreateDirectory(folder);

            if (frame != null)
            {
                var fileName = folder + @"\" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".png";
                frame.Save(fileName);

                Console.WriteLine($"{DateTime.Now.ToString()} - Screenshot saved to {Path.GetFullPath(fileName)}");
            }
            else
            {
                MessageBox.Show("Unable to capture screenshot!");
            }
        }

        private void imageHashToolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new ImageHashForm().Show(this);
        }

        private void resizeRemotePlayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new ResizeRemotePlayForm().ShowDialog(this);
        }

        private void macroCompressorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new MacroCompressorForm().ShowDialog(this);
        }

        private void remapperToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TemporarilySetControlMode(ControlMode.Remapper, () =>
            {
                new RemapperForm(m_Remapper).ShowDialog(this);
            });
        }
        #endregion

        #region Help
        private void statusCheckerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TemporarilySetControlMode(ControlMode.StatusChecker, () =>
            {
                m_StatusChecker.SetActive(true);
                new StatusCheckerForm(m_StatusChecker).ShowDialog(this);
                m_StatusChecker.SetActive(false);
            });
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var aboutForm = new AboutForm();
            aboutForm.ShowDialog(this);
        }
        #endregion

        #endregion

        /* Status strip methods */
        #region Status Strip

        #endregion
    }
}
