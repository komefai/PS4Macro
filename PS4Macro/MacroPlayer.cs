// PS4Macro (File: MacroPlayer.cs)
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
using System.Linq;
using System.Text;

namespace PS4Macro
{
    public class MacroPlayer : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        private bool m_IsPlaying = false;
        public bool IsPlaying
        {
            get { return m_IsPlaying; }
            private set
            {
                if (value != m_IsPlaying)
                {
                    m_IsPlaying = value;
                    NotifyPropertyChanged("IsPlaying");
                }
            }
        }

        private bool m_IsRecording = false;
        public bool IsRecording
        {
            get { return m_IsRecording; }
            private set
            {
                if (value != m_IsRecording)
                {
                    m_IsRecording = value;
                    NotifyPropertyChanged("IsRecording");
                }
            }
        }

        private int m_CurrentTick = 0;
        public int CurrentTick
        {
            get { return m_CurrentTick; }
            private set
            {
                if (value != m_CurrentTick)
                {
                    m_CurrentTick = value;
                    NotifyPropertyChanged("CurrentTick");
                }
            }
        }

        private List<DualShockState> m_Sequence = new List<DualShockState>();
        public List<DualShockState> Sequence
        {
            get { return m_Sequence; }
            private set
            {
                if (value != m_Sequence)
                {
                    m_Sequence = value;
                    NotifyPropertyChanged("Sequence");
                }
            }
        }

        /* Constructor */
        public MacroPlayer()
        {
            IsPlaying = false;
            IsRecording = false;
            CurrentTick = 0;
            Sequence = new List<DualShockState>();
        }


        public void Play()
        {
            IsPlaying = true;
        }

        public void Pause()
        {
            IsPlaying = false;
        }

        public void Stop()
        {
            IsPlaying = false;
            CurrentTick = 0;
        }

        public void Record()
        {
            IsRecording = !IsRecording;
        }

        public void Clear()
        {
            Sequence = new List<DualShockState>();
            CurrentTick = 0;
        }

        public void OnReceiveData(ref DualShockState state)
        {
            if (IsPlaying)
            {
                if (IsRecording)
                {
                    Sequence.Add(state);
                }
                else
                {
                    if (Sequence[CurrentTick] != null)
                        state = Sequence[CurrentTick];
                }

                // Increment tick
                CurrentTick++;

                // Reset tick if out of bounds
                if (CurrentTick >= Sequence.Count)
                    CurrentTick = 0;
            }
        }
    }
}
