// PS4Macro (File: Classes/StatusChecker.cs)
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

using PS4Macro.Classes.GlobalHooks;
using PS4Macro.Classes.Remapping;
using PS4RemotePlayInterceptor;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml.Serialization;

namespace PS4Macro.Classes
{
    public class StatusCheckData
    {
        public TimeSpan TimeoutInterval { get; private set; }

        public bool DidWorkOnce { get; set; }
        public bool IsWorking { get; set; }
        public DateTime LastWorkingTimestamp { get; set; }

        public StatusCheckData(TimeSpan timeoutInterval)
        {
            TimeoutInterval = timeoutInterval;
        }

        public StatusCheckData()
        {
            TimeoutInterval = TimeSpan.MinValue;
        }
    }

    public class StatusChecker
    {
        private const int TIMER_DUE_TIME = 0;
        private const int TIMER_PERIOD = 1000;

        // Delegates
        public delegate void OnStatusChangedDelegate();
        public OnStatusChangedDelegate OnStatusChanged { get; set; }

        public Process RemotePlayProcess { get; set; }
        public IntPtr ForegroundWindowHandle { get; set; }
        public IntPtr MainWindowHandle { get; set; }

        public List<StatusCheckData> StatusList { get; private set; }

        public StatusCheckData InterceptorStatus { get; private set; }
        public StatusCheckData KeyboardStatus { get; private set; }
        public StatusCheckData MouseStatus { get; private set; }

        public bool IsActive { get; private set; }

        private Timer m_Timer = null;
        private AutoResetEvent m_AutoEvent = null;

        public StatusChecker()
        {
            InterceptorStatus = new StatusCheckData(TimeSpan.FromSeconds(1));
            KeyboardStatus = new StatusCheckData();
            MouseStatus = new StatusCheckData();

            StatusList = new List<StatusCheckData>()
            {
                InterceptorStatus, KeyboardStatus, MouseStatus
            };

            m_AutoEvent = new AutoResetEvent(false);
            m_Timer = new Timer(CheckStatus, m_AutoEvent, Timeout.Infinite, Timeout.Infinite);
        }

        public string GetSettingsText()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Settings));
            using (StringWriter textWriter = new StringWriter())
            {
                serializer.Serialize(textWriter, Program.Settings);
                return textWriter.ToString();
            }
        }

        private void CheckStatus(object state)
        {
            // Process
            if (RemotePlayProcess != null)
            {
                var mainWindowHandle = RemotePlayProcess.MainWindowHandle;
                if (mainWindowHandle != MainWindowHandle)
                {
                    MainWindowHandle = mainWindowHandle;
                    OnStatusChanged?.Invoke();
                }
            }

            var foregroundWindowHandle = RemapperUtility.GetForegroundWindow();
            if (foregroundWindowHandle != ForegroundWindowHandle)
            {
                ForegroundWindowHandle = foregroundWindowHandle;
                OnStatusChanged?.Invoke();
            }

            // Status
            foreach (var statusData in StatusList)
            {
                // Ignore infinite timeouts
                if (statusData.TimeoutInterval == TimeSpan.MinValue)
                    continue;

                // Reset IsWorking flag if timeout
                var now = DateTime.Now;
                if (now - statusData.LastWorkingTimestamp >= statusData.TimeoutInterval)
                {
                    statusData.IsWorking = false;
                }
            }
        }

        public void RefreshProcess()
        {
            try
            {
                if (RemotePlayProcess != null && !RemotePlayProcess.HasExited)
                {
                    RemotePlayProcess.Refresh();
                }
            }
            catch (Exception) { }
        }

        public void SetActive(bool active)
        {
            if (active == IsActive) return;

            if (active)
            {
                m_Timer.Change(TIMER_DUE_TIME, TIMER_PERIOD);
            }
            else
            {
                m_Timer.Change(Timeout.Infinite, Timeout.Infinite);
            }

            IsActive = active;
        }

        private void UpdateStatus(StatusCheckData status)
        {
            status.DidWorkOnce = true;
            status.IsWorking = true;
            status.LastWorkingTimestamp = DateTime.Now;
            OnStatusChanged?.Invoke();
        }

        public void OnReceiveData(ref DualShockState state)
        {
            UpdateStatus(InterceptorStatus);
        }

        public void OnKeyPressed(object sender, GlobalKeyboardHookEventArgs e)
        {
            UpdateStatus(KeyboardStatus);
        }

        public void OnMouseEvent(object sender, GlobalMouseHookEventArgs e)
        {
            UpdateStatus(MouseStatus);
        }
    }
}
