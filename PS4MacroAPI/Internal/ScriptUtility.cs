// PS4MacroAPI (File: Internal/ScriptUtility.cs)
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

using PS4MacroAPI.Internal;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace PS4MacroAPI.Internal
{
    public class ScriptUtility
    {
        public static Process FindProcess()
        {
            Process[] processes = Process.GetProcessesByName("PS4Macro");
            foreach (var process in processes)
            {
                return process;
            }
            return null;
        }

        public static Process FindRemotePlayProcess()
        {
            Process[] processes = Process.GetProcessesByName("RemotePlay");
            foreach (var process in processes)
            {
                return process;
            }
            return null;
        }

        public static IntPtr FindStreamingPanel(Process remotePlayProcess)
        {
            // Find panel in process
            var childHandles = WindowControl.GetAllChildHandles(remotePlayProcess.MainWindowHandle);
            var panelHandle = childHandles.Find(ptr => {
                var sb = new StringBuilder(50);
                WindowControl.GetClassName(ptr, sb, 50);
                return sb.ToString() == "WindowsForms10.Window.8.app.0.141b42a_r9_ad1";
            });

            return panelHandle;
        }

        public static List<ScriptBase> LoadScripts(string path)
        {
            string[] dllFileNames = null;
            if (Directory.Exists(path))
            {
                dllFileNames = Directory.GetFiles(path, "*.dll");
            }

            ICollection<Assembly> assemblies = new List<Assembly>(dllFileNames.Length);
            foreach (string dllFile in dllFileNames)
            {
                AssemblyName an = AssemblyName.GetAssemblyName(dllFile);

                // Ignore self
                if (an.Name == "PS4MacroAPI")
                    continue;

                Assembly assembly = Assembly.Load(an);
                assemblies.Add(assembly);
            }

            Type scriptType = typeof(ScriptBase);
            ICollection<Type> scriptTypes = new List<Type>();
            foreach (Assembly assembly in assemblies)
            {
                if (assembly != null)
                {
                    Type[] types = assembly.GetTypes();
                    foreach (Type type in types)
                    {
                        if (type.IsInterface || type.IsAbstract)
                        {
                            continue;
                        }
                        else
                        {
                            if (type.IsSubclassOf(scriptType))
                            {
                                scriptTypes.Add(type);
                            }
                        }
                    }
                }
            }

            List<ScriptBase> scripts = new List<ScriptBase>(scriptTypes.Count);
            foreach (Type type in scriptTypes)
            {
                ScriptBase script = (ScriptBase)Activator.CreateInstance(type);
                scripts.Add(script);
            }

            return scripts;
        }

        public static ScriptBase LoadScript(string dllFile)
        {
            AssemblyName an = AssemblyName.GetAssemblyName(dllFile);
            Assembly assembly = Assembly.Load(an);

            Type scriptType = typeof(ScriptBase);

            if (assembly != null)
            {
                Type[] types = assembly.GetTypes();
                foreach (Type type in types)
                {
                    if (type.IsInterface || type.IsAbstract)
                    {
                        continue;
                    }
                    else
                    {
                        if (type.IsSubclassOf(scriptType))
                        {
                            ScriptBase script = (ScriptBase)Activator.CreateInstance(type);
                            return script;
                        }
                    }
                }
            }

            return null;
        }

        private static WindowControl CreateTempWindowControl()
        {
            var process = FindRemotePlayProcess();
            var panel = FindStreamingPanel(process);
            return new WindowControl(process.MainWindowHandle, panel);
        }

        public static Bitmap CaptureFrame()
        {
            var windowControl = CreateTempWindowControl();
            return windowControl.CaptureFrame();
        }

        public static Size GetWindowSize()
        {
            var windowControl = CreateTempWindowControl();
            return windowControl.GetWindowSize();
        }

        public static void ResizeWindow(Size size, bool fixedSize = false)
        {
            var windowControl = CreateTempWindowControl();
            windowControl.ResizeWindow(size, fixedSize);
        }
    }
}
