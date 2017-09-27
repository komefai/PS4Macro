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
    /// <summary>
    /// Utility class for scripts
    /// </summary>
    public class ScriptUtility
    {
        /// <summary>
        /// Find PS4 Macro process
        /// </summary>
        /// <returns></returns>
        public static Process FindProcess()
        {
            Process[] processes = Process.GetProcessesByName("PS4Macro");
            foreach (var process in processes)
            {
                return process;
            }
            return null;
        }

        /// <summary>
        /// Find PS4 Remote Play process
        /// </summary>
        /// <returns></returns>
        public static Process FindRemotePlayProcess()
        {
            Process[] processes = Process.GetProcessesByName("RemotePlay");
            foreach (var process in processes)
            {
                return process;
            }
            return null;
        }

        /// <summary>
        /// Find the streaming panel of PS4 Remote Play
        /// </summary>
        /// <param name="remotePlayProcess"></param>
        /// <returns></returns>
        public static IntPtr FindStreamingPanel(Process remotePlayProcess)
        {
            // Find panel in process
            var childHandles = WindowControl.GetAllChildHandles(remotePlayProcess.MainWindowHandle);
            var panelHandle = childHandles.Find(ptr =>
            {
                var sb = new StringBuilder(50);
                WindowControl.GetClassName(ptr, sb, 50);
                var str = sb.ToString();
                return str == "WindowsForms10.Window.8.app.0.141b42a_r9_ad1" || str == "WindowsForms10.Window.8.app.0.141b42a_r10_ad1";
            });

            // Try to find the best one possible instead
            if (panelHandle == IntPtr.Zero)
            {
                IntPtr biggestPanel = IntPtr.Zero;
                Rect biggestSize = new Rect();
                foreach (var ptr in childHandles)
                {
                    Rect rect = new Rect();
                    WindowControl.GetWindowRect(ptr, ref rect);

                    if (rect.Bottom - rect.Top >= biggestSize.Bottom - biggestSize.Top)
                    {
                        biggestPanel = ptr;
                        biggestSize = rect;
                    }
                }

                panelHandle = biggestPanel;
            }

            return panelHandle;
        }

        /// <summary>
        /// Load scripts from path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Load a script from path
        /// </summary>
        /// <param name="dllFile"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Create a window control temporarily
        /// </summary>
        /// <returns></returns>
        private static WindowControl CreateTempWindowControl()
        {
            var process = FindRemotePlayProcess();
            var panel = FindStreamingPanel(process);
            return new WindowControl(process.MainWindowHandle, panel);
        }

        /// <summary>
        /// One shot method for CaptureFrame
        /// </summary>
        /// <returns></returns>
        public static Bitmap CaptureFrame()
        {
            var windowControl = CreateTempWindowControl();
            return windowControl.CaptureFrame();
        }

        /// <summary>
        /// One shot method for GetWindowSize
        /// </summary>
        /// <returns></returns>
        public static Size GetWindowSize()
        {
            var windowControl = CreateTempWindowControl();
            return windowControl.GetWindowSize();
        }

        /// <summary>
        /// One shot method for ResizeWindow
        /// </summary>
        /// <param name="size"></param>
        /// <param name="fixedSize"></param>
        public static void ResizeWindow(Size size, bool fixedSize = false)
        {
            var windowControl = CreateTempWindowControl();
            windowControl.ResizeWindow(size, fixedSize);
        }
    }
}
