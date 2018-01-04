// PPS4Macro(File: Classes/Remapping/Remapper.cs)
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
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using PS4RemotePlayInterceptor;
using System.IO;

namespace PS4Macro.Classes.Remapping
{
    public class Remapper
    {
        public Process RemotePlayProcess { get; set; }

        public Dictionary<Keys, bool> PressedKeys { get; private set; }
        public Dictionary<Keys, BaseAction> KeysDict { get; private set; }
        public DualShockState CurrentState { get; private set; }

        public MacroPlayer MacroPlayer { get; private set; }
        public bool UsingMacroPlayer { get; private set; }

        public List<MappingAction> MappingsDataBinding { get; private set; }
        public List<MacroAction> MacrosDataBinding { get; private set; }

        public Remapper()
        {
            PressedKeys = new Dictionary<Keys, bool>();
            KeysDict = new Dictionary<Keys, BaseAction>();

            MacroPlayer = new MacroPlayer();
            MacroPlayer.Loop = false;

            MappingsDataBinding = new List<MappingAction>()
            {
                new MappingAction("L Left", Keys.A, "LX", 0),
                new MappingAction("L Right", Keys.D, "LX", 255),
                new MappingAction("L Up", Keys.W, "LY", 0),
                new MappingAction("L Down", Keys.S, "LY", 255),

                new MappingAction("R Left", Keys.J, "RX", 0),
                new MappingAction("R Right", Keys.L, "RX", 255),
                new MappingAction("R Up", Keys.I, "RY", 0),
                new MappingAction("R Down", Keys.K, "RY", 255),

                new MappingAction("R1", Keys.E, "R1", true),
                new MappingAction("L1", Keys.Q, "L1", true),
                new MappingAction("L2", Keys.U, "L2", 255),
                new MappingAction("R2", Keys.O, "R2", 255),

                new MappingAction("Triangle", Keys.C, "Triangle", true),
                new MappingAction("Circle", Keys.Escape, "Circle", true),
                new MappingAction("Cross", Keys.Enter, "Cross", true),
                new MappingAction("Square", Keys.V, "Square", true),

                new MappingAction("DPad Up", Keys.Up, "DPad_Up", true),
                new MappingAction("DPad Down", Keys.Down, "DPad_Down", true),
                new MappingAction("DPad Left", Keys.Left, "DPad_Left", true),
                new MappingAction("DPad Right", Keys.Right, "DPad_Right", true),

                new MappingAction("L3", Keys.N, "L3", true),
                new MappingAction("R3", Keys.M, "R3", true),

                new MappingAction("Share", Keys.LControlKey, "Share", true),
                new MappingAction("Options", Keys.Z, "Options", true),
                new MappingAction("PS", Keys.LShiftKey, "PS", true),

                new MappingAction("Touch Button", Keys.T, "TouchButton", true)
            };

            MacrosDataBinding = new List<MacroAction>();

            // Load bindings if file exist
            if (File.Exists(GetBindingsFilePath()))
            {
                LoadBindings();
            }

            CreateActions();
        }

        public void OnReceiveData(ref DualShockState state)
        {
            if (UsingMacroPlayer)
            {
                MacroPlayer.OnReceiveData(ref state);
            }
            else
            {
                if (CurrentState != null)
                    state = CurrentState;
            }
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        public void OnKeyPressed(object sender, GlobalKeyboardHookEventArgs e)
        {
            // https://msdn.microsoft.com/en-us/library/windows/desktop/dd375731(v=vs.85).aspx

            if (RemotePlayProcess == null)
                return;

            // Check for focused window
            var activeWindow = GetForegroundWindow();
            if (activeWindow != IntPtr.Zero)
            {
                if (activeWindow != RemotePlayProcess.MainWindowHandle)
                {
                    return;
                }
            }

            int vk = e.KeyboardData.VirtualCode;
            Keys key = (Keys)vk;

            // Key down
            if (e.KeyboardState == GlobalKeyboardHook.KeyboardState.KeyDown)
            {
                if (!PressedKeys.ContainsKey(key))
                {
                    PressedKeys.Add(key, true);
                    ExecuteActionsByKey(PressedKeys.Keys.ToList());
                }

                e.Handled = true;
            }
            // Key up
            else if (e.KeyboardState == GlobalKeyboardHook.KeyboardState.KeyUp)
            {
                if (PressedKeys.ContainsKey(key))
                {
                    PressedKeys.Remove(key);
                    ExecuteActionsByKey(PressedKeys.Keys.ToList());
                }

                e.Handled = true;
            }

            // Reset state
            if (!IsKeyDown())
            {
                CurrentState = null;
            }
        }

        public bool IsKeyDown()
        {
            return PressedKeys.Count > 0;
        }

        private string GetBindingsFilePath()
        {
            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\" + "bindings.xml"; 
        }

        public void SaveBindings()
        {
            var container = new BindingsContainer();
            container.Mappings = MappingsDataBinding;
            container.Macros = MacrosDataBinding;

            BindingsContainer.Serialize(GetBindingsFilePath(), container);
        }

        public void LoadBindings()
        {
            var container = BindingsContainer.Deserialize(GetBindingsFilePath());
            MappingsDataBinding = container.Mappings;
            MacrosDataBinding = container.Macros;
        }

        public void CreateActions()
        {
            var dict = new Dictionary<Keys, BaseAction>();

            foreach (var item in MappingsDataBinding)
            {
                if (item.Key == Keys.None) continue;
                dict.Add(item.Key, item);
            }
            foreach (var item in MacrosDataBinding)
            {
                if (item.Key == Keys.None) continue;
                dict.Add(item.Key, item);
            }

            KeysDict = dict;
        }

        public void ExecuteActionsByKey(List<Keys> keys)
        {
            var state = new DualShockState();

            foreach (var key in keys)
            {
                if (key < 0) continue;

                try
                {
                    BaseAction action = KeysDict[key];
                    ExecuteAction(action, state);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.StackTrace);
                }
            }
        }

        private void ExecuteAction(BaseAction action, DualShockState state = null)
        {
            if (action == null)
                return;

            // Test remap action
            {
                MappingAction cast = action as MappingAction;
                if (cast != null)
                {
                    ExecuteRemapAction(cast, state);
                    return;
                }
            }
            // Test macro action
            {
                MacroAction cast = action as MacroAction;
                if (cast != null)
                {
                    ExecuteMacroAction(cast);
                    return;
                }
            }
        }

        private void ExecuteRemapAction(MappingAction action, DualShockState state)
        {
            if (state == null)
                state = new DualShockState();

            // Try to set property using Reflection
            bool didSetProperty = false;
            try
            {
                SetValue(state, action.Property, action.Value);
                didSetProperty = true;
            }
            catch (Exception ex) { Debug.WriteLine(ex.StackTrace); }

            if (didSetProperty)
            {
                MacroPlayer.Stop();
                UsingMacroPlayer = false;

                state.Battery = 255;
                state.IsCharging = true;
                CurrentState = state;
            }
        }

        private void ExecuteMacroAction(MacroAction action)
        {
            //// TODO: Load sequence from cache
            ////List<DualShockState> sequence = new List<DualShockState>();

            UsingMacroPlayer = true;

            MacroPlayer.Stop();
            MacroPlayer.LoadFile(action.Path);
            MacroPlayer.Play();
        }

        // https://stackoverflow.com/questions/13270183/type-conversion-issue-when-setting-property-through-reflection
        private static void SetValue(object inputObject, string propertyName, object propertyVal)
        {
            //find out the type
            Type type = inputObject.GetType();

            //get the property information based on the type
            System.Reflection.PropertyInfo propertyInfo = type.GetProperty(propertyName);

            //find the property type
            Type propertyType = propertyInfo.PropertyType;

            //Convert.ChangeType does not handle conversion to nullable types
            //if the property type is nullable, we need to get the underlying type of the property
            var targetType = IsNullableType(propertyInfo.PropertyType) ? Nullable.GetUnderlyingType(propertyInfo.PropertyType) : propertyInfo.PropertyType;

            //Returns an System.Object with the specified System.Type and whose value is
            //equivalent to the specified object.
            propertyVal = Convert.ChangeType(propertyVal, targetType);

            //Set the value of the property
            propertyInfo.SetValue(inputObject, propertyVal, null);

        }
        private static bool IsNullableType(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>));
        }
    }
}
