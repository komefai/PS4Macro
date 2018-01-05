// PS4Macro (File: Classes/MacroUtility.cs)
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
using System.Linq;
using System.Text;

namespace PS4Macro.Classes
{
    public class MacroUtility
    {
        private static DualShockState _defaultState = new DualShockState();
        private const byte AnalogDeadZoneMin = 0x78;
        private const byte AnalogDeadZoneCenter = 0x80;
        private const byte AnalogDeadZoneMax = 0x88;

        public static List<DualShockState> TrimMacro(List<DualShockState> sequence)
        {
            var newSequence = sequence.Select(item => item.Clone()).ToList();

            // Edit sequence
            TrimSequence(newSequence, false); // Trim start
            TrimSequence(newSequence, true); // Trim end

            return newSequence;
        }

        public static void TrimMacroInPlace(List<DualShockState> sequence)
        {
            // Edit sequence
            TrimSequence(sequence, false); // Trim start
            TrimSequence(sequence, true); // Trim end
        }

        public static void TrimSequence(List<DualShockState> sequence, bool fromEnd)
        {
            if (!fromEnd)
            {
                int offset = 5;
                for (var i = 0; i < sequence.Count; i++)
                {
                    var isDefaultState = IsDefaultState(sequence.ElementAt(i));
                    if (!isDefaultState)
                    {
                        if (i == 0) return;
                        sequence.RemoveRange(0, i - offset);
                        return;
                    }
                }
            }
            else
            {
                int offset = 10;
                for (var i = sequence.Count - 1; i >= 0; i--)
                {
                    var isDefaultState = IsDefaultState(sequence.ElementAt(i));
                    if (!isDefaultState)
                    {
                        if (i == sequence.Count - 1) return;
                        var count = i + offset;
                        sequence.RemoveRange(count, sequence.Count - count);
                        return;
                    }
                }
            }
        }

        private static bool IsDefaultState(DualShockState state)
        {
            return (state.LX == _defaultState.LX ||
                        (state.LX >= AnalogDeadZoneMin && state.LX <= AnalogDeadZoneMax)) &&
                    (state.LY == _defaultState.LY ||
                        (state.LY >= AnalogDeadZoneMin && state.LY <= AnalogDeadZoneMax)) &&
                    (state.RX == _defaultState.RX ||
                        (state.RX >= AnalogDeadZoneMin && state.RX <= AnalogDeadZoneMax)) &&
                    (state.RY == _defaultState.RY ||
                        (state.RY >= AnalogDeadZoneMin && state.RY <= AnalogDeadZoneMax)) &&
                    state.L2 == _defaultState.L2 &&
                    state.R2 == _defaultState.R2 &&
                    state.Triangle == _defaultState.Triangle &&
                    state.Circle == _defaultState.Circle &&
                    state.Cross == _defaultState.Cross &&
                    state.Square == _defaultState.Square &&
                    state.DPad_Up == _defaultState.DPad_Up &&
                    state.DPad_Down == _defaultState.DPad_Down &&
                    state.DPad_Left == _defaultState.DPad_Left &&
                    state.DPad_Right == _defaultState.DPad_Right &&
                    state.L1 == _defaultState.L1 &&
                    state.R1 == _defaultState.R1 &&
                    state.Share == _defaultState.Share &&
                    state.Options == _defaultState.Options &&
                    state.L3 == _defaultState.L3 &&
                    state.R3 == _defaultState.R3 &&
                    state.PS == _defaultState.PS;
        }
    }
}
