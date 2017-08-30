// PS4Macro (File: PS4Macro.cs)
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

using PS4RemotePlayInterceptor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PS4Macro
{
    public partial class PS4Macro : Form
    {
        private MacroPlayer m_MacroPlayer;

        public PS4Macro()
        {
            InitializeComponent();

            m_MacroPlayer = new MacroPlayer();

            // Inject into PS4 Remote Play
            Interceptor.Callback = new InterceptionDelegate(m_MacroPlayer.OnReceiveData);
            Interceptor.Inject();
        }

        private void UpdateButtons()
        {
            playButton.ForeColor = m_MacroPlayer.IsPlaying ? Color.Green : DefaultForeColor;
            recordButton.ForeColor = m_MacroPlayer.IsRecording ? Color.Red : DefaultForeColor;
        }

        private void playButton_Click(object sender, EventArgs e)
        {
            m_MacroPlayer.Play();
            UpdateButtons();
        }

        private void pauseButton_Click(object sender, EventArgs e)
        {
            m_MacroPlayer.Pause();
            UpdateButtons();
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            m_MacroPlayer.Stop();
            UpdateButtons();
        }

        private void recordButton_Click(object sender, EventArgs e)
        {
            m_MacroPlayer.Record();
            UpdateButtons();
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            m_MacroPlayer.Clear();
            UpdateButtons();
        }
    }
}
