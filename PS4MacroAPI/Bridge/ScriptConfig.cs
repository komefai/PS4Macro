// PS4MacroAPI (File: Bridge/ScriptConfig.cs)
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

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace PS4MacroAPI
{
    public class ScriptConfig
    {
        public const int DEFAULT_SCREEN_WIDTH = 1024;
        public const int DEFAULT_SCREEN_HEIGHT = 768;

        public string Name { get; set; }
        public int LoopDelay { get; set; }
        public Size TargetSize { get; set; }
        public bool ThrowExceptions { get; set; }
        public List<Scene> Scenes { get; set; }

        public ScriptConfig()
        {
            Name = "Untitled Script";
            LoopDelay = 500;
            TargetSize = new Size(DEFAULT_SCREEN_WIDTH, DEFAULT_SCREEN_HEIGHT);
            ThrowExceptions = true;
            Scenes = null;
        }
    }
}
