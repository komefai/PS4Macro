// PS4MacroAPI (File: Bridge/Scene.cs)
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
using System.Linq;
using System.Text;

namespace PS4MacroAPI
{
    public abstract class Scene
    {
        abstract public string Name { get; }

        abstract public bool Match(ScriptBase script);
        abstract public void OnMatched(ScriptBase script);

        // Sort by search priority (frequent scenes first)
        public static Scene Search(ScriptBase script)
        {
            if (script.Config == null || script.Config.Scenes == null)
                return null;

            foreach (var scene in script.Config.Scenes)
            {
                if (scene.Match(script))
                    return scene;
            }

            return null;
        }

        public static string CreateID(Scene instance, string name)
        {
            return $"{instance.Name}_{name}";
        }
    }
}
