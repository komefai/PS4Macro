// PS4Macro (File: Forms/StatusCheckerForm.cs)
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
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace PS4Macro.Forms
{
    public partial class StatusCheckerForm : Form
    {
        #region Native API
        [DllImport("user32.dll")]
        static extern bool HideCaret(IntPtr hWnd);
        #endregion

        public StatusChecker StatusChecker { get; set; }

        public StatusCheckerForm(StatusChecker statusChecker)
        {
            InitializeComponent();

            StatusChecker = statusChecker;
        }

        private void SetStatusText(StatusCheckData status, Label label, string textOverride = null)
        {
            label.Text = status.IsWorking ? "Working" : "Not Working";
            label.ForeColor = status.IsWorking ? Color.Green : Color.Red;

            if (textOverride != null && !status.IsWorking)
            {
                label.Text = textOverride;
            }
        }

        private void OnStatusChanged()
        {
            BeginInvoke(new Action(() =>
            {
                // Update process
                remotePlayProcessLabel.Text = StatusChecker.RemotePlayProcess == null ? "-" : "PID=" + StatusChecker.RemotePlayProcess.Id;
                remotePlayHandleLabel.Text = StatusChecker.MainWindowHandle == IntPtr.Zero ? "-" : StatusChecker.MainWindowHandle.ToString();
                foregroundWindowLabel.Text = StatusChecker.ForegroundWindowHandle == IntPtr.Zero ? "-" : StatusChecker.ForegroundWindowHandle.ToString();

                // Update status labels
                SetStatusText(StatusChecker.InterceptorStatus, interceptorStatusLabel);
                SetStatusText(StatusChecker.KeyboardStatus, keyboardStatusLabel, "Press Any Key");
                SetStatusText(StatusChecker.MouseStatus, mouseStatusLabel, "Move/Click Mouse");
            }));
        }

        private void StatusCheckerForm_Load(object sender, EventArgs e)
        {
            // Trigger on load
            OnStatusChanged();

            // Refresh process
            StatusChecker.RefreshProcess();

            // Get settings
            settingsRichTextBox.Text = StatusChecker.GetSettingsText();

            StatusChecker.OnStatusChanged += OnStatusChanged;
        }

        private void StatusCheckerForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            StatusChecker.OnStatusChanged -= OnStatusChanged;
        }
    }
}
