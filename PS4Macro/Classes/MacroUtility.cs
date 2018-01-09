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
        public static List<DualShockState> TrimMacro(List<DualShockState> sequence)
        {
            var newSequence = sequence.Select(item => item == null ? null : item.Clone()).ToList();

            // Edit sequence
            TrimMacroInPlace(newSequence);

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
                    var isDefaultState = DualShockState.IsDefaultState(sequence.ElementAt(i));
                    if (!isDefaultState)
                    {
                        if (i == 0) return;
                        var offsetVal = i - offset;
                        if (offsetVal == 0) return;
                        sequence.RemoveRange(0, offsetVal);
                        return;
                    }
                }
            }
            else
            {
                int offset = 5;
                for (var i = sequence.Count - 1; i >= 0; i--)
                {
                    var isDefaultState = DualShockState.IsDefaultState(sequence.ElementAt(i));
                    if (!isDefaultState)
                    {
                        if (i == sequence.Count - 1) return;
                        var count = i + offset;
                        var offsetVal = sequence.Count - count;
                        if (count + offsetVal > sequence.Count) return;
                        sequence.RemoveRange(count, offsetVal);
                        return;
                    }
                }
            }
        }
    }
}
