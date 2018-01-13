// PS4Macro (File: Classes/MacroPlayer.cs)
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

using PS4RemotePlayInterceptor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace PS4Macro.Classes
{
    public delegate void MacroLapEnterHandler(object sender);

    public class MacroPlayer : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region Properties
        private bool m_Loop = true;
        public bool Loop
        {
            get { return m_Loop; }
            set
            {
                if (value != m_Loop)
                {
                    m_Loop = value;
                    NotifyPropertyChanged("Loop");
                }
            }
        }

        private bool m_RecordShortcut = true;
        public bool RecordShortcut
        {
            get { return m_RecordShortcut; }
            set
            {
                if (value != m_RecordShortcut)
                {
                    m_RecordShortcut = value;
                    NotifyPropertyChanged("RecordShortcut");
                }
            }
        }

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
            set
            {
                if (value != m_Sequence)
                {
                    m_Sequence = value;
                    NotifyPropertyChanged("Sequence");
                }
            }
        }
        #endregion

        #region Events
        public event MacroLapEnterHandler LapEnter;
        #endregion

        private bool m_RecordShortcutDown = false;

        /* Constructor */
        public MacroPlayer()
        {
            Loop = true;
            RecordShortcut = true;
            IsPlaying = false;
            IsPaused = false;
            IsRecording = false;
            CurrentTick = 0;
            Sequence = new List<DualShockState>();
        }


        public void Play()
        {
            IsPlaying = true;
            IsPaused = false;
        }

        public void Pause()
        {
            IsPlaying = true;
            IsPaused = true;
        }

        public void Stop()
        {
            IsPlaying = false;
            IsPaused = false;
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

        public void LoadFile(string path)
        {
            Sequence = DualShockState.Deserialize(path);
        }

        public void SaveFile(string path)
        {
            DualShockState.Serialize(path, Sequence);
        }

        public void OnReceiveData(ref DualShockState state)
        {
            // Record shortcut trigger
            if (RecordShortcut)
            {
                // Down
                if (state.TouchButton)
                {
                    if (!m_RecordShortcutDown)
                    {
                        m_RecordShortcutDown = true;
                    }
                }
                // Up
                else
                {
                    if (m_RecordShortcutDown)
                    {
                        m_RecordShortcutDown = false;

                        // Auto play
                        if (!IsPlaying || IsPaused)
                            Play();

                        // Record
                        Record();
                    }
                }

                // Override real value
                state.TouchButton = false;
            }

            // Playback
            if (IsPlaying && !IsPaused)
            {
                // Recording
                if (IsRecording)
                {
                    Sequence.Add(state);
                }
                // Playing
                else
                {
                    DualShockState newState = Sequence[CurrentTick];
                    DualShockState oldState = state;

                    if (newState != null)
                    {
                        // Update the state
                        state = newState;
                        // Replace battery status
                        state.Battery = oldState.Battery;
                        state.IsCharging = oldState.IsCharging;
                    }
                }

                // Increment tick
                CurrentTick++;

                // Reset tick if out of bounds
                if (CurrentTick >= Sequence.Count)
                {
                    CurrentTick = 0;

                    // Raise LapEnter event
                    LapEnter?.Invoke(this);

                    // Stop if looping is disabled
                    if (!Loop && !IsRecording)
                    {
                        Stop();
                    }
                }

                // Emulation delay compensation
                if (Program.Settings.EmulateController)
                {
                    System.Threading.Thread.Sleep(1);
                }
            }
        }
    }
}
