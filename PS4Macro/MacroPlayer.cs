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
using System.Linq;
using System.Text;

namespace PS4Macro
{
    public class MacroPlayer
    {
        public bool IsPlaying { get; private set; }
        public bool IsRecording { get; private set; }
        public int CurrentTick { get; private set; }
        public List<DualshockState> Sequence { get; private set; }

        /* Constructor */
        public MacroPlayer()
        {
            IsPlaying = false;
            IsRecording = false;
            CurrentTick = 0;
            Sequence = new List<DualshockState>();
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
            Sequence = new List<DualshockState>();
        }

        public void OnReceiveData(ref DualshockState state)
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
