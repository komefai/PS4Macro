// PPS4Macro(File: Classes/Remapping/Remapper.cs)
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

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using PS4RemotePlayInterceptor;
using PS4Macro.Classes.GlobalHooks;
using System.IO;
using System.Drawing;

namespace PS4Macro.Classes.Remapping
{
    public class Remapper
    {
        public Process RemotePlayProcess { get; set; }

        public Dictionary<Keys, bool> PressedKeys { get; private set; }
        public Dictionary<Keys, BaseAction> KeysDict { get; private set; }
        public DualShockState CurrentState { get; private set; }

        public MouseStroke CurrentMouseStroke { get; private set; }
        public System.Timers.Timer MouseReleaseTimer { get; private set; }
        public bool IsCursorShowing { get; private set; }
        public int CursorOverflowX { get; private set; }
        public int CursorOverflowY { get; private set; }
        public bool LeftMouseDown { get; private set; }
        public bool RightMouseDown { get; private set; }
        public double MouseSpeedX { get; private set; }
        public double MouseSpeedY { get; private set; }

        // TODO: Expose value to UI
        public double MouseDecayRate { get; private set; }
        public double MouseDeadzone{ get; private set; }
        public int MouseReleaseDelay { get; private set; }

        public MacroPlayer MacroPlayer { get; private set; }
        public bool UsingMacroPlayer { get; private set; }

        public List<MappingAction> MappingsDataBinding { get; private set; }
        public List<MacroAction> MacrosDataBinding { get; private set; }

        public Remapper()
        {
            PressedKeys = new Dictionary<Keys, bool>();
            KeysDict = new Dictionary<Keys, BaseAction>();

            IsCursorShowing = true;
            MouseDecayRate = 10;
            MouseReleaseDelay = 50;

            MacroPlayer = new MacroPlayer();

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
            // Macro
            if (UsingMacroPlayer)
            {
                MacroPlayer.OnReceiveData(ref state);
            }
            // Mapping
            else
            {
                if (CurrentState != null)
                    state = CurrentState;

                if (!CheckFocusedWindow())
                    return;

                // TODO: Expose value to UI
                // Left mouse clicked
                if (LeftMouseDown)
                {
                    RemapperUtility.SetValue(state, "R2", 255);
                }
                else
                {
                    RemapperUtility.SetValue(state, "R2", 0);
                }

                // TODO: Expose value to UI
                // Right mouse clicked
                if (RightMouseDown)
                {
                    RemapperUtility.SetValue(state, "L2", 255);
                }
                else
                {
                    RemapperUtility.SetValue(state, "L2", 0);
                }

                // Mouse moved
                if (CurrentMouseStroke.DidMoved)
                {
                    MouseSpeedX = CurrentMouseStroke.VelocityX;
                    MouseSpeedY = CurrentMouseStroke.VelocityY;
                    CurrentMouseStroke.DidMoved = false;

                    // Stop release timer
                    if (MouseReleaseTimer != null)
                    {
                        MouseReleaseTimer.Stop();
                        MouseReleaseTimer = null;
                    }
                }
                // Mouse idle
                else
                {
                    // Start decay
                    MouseSpeedX /= MouseDecayRate;
                    MouseSpeedY /= MouseDecayRate;

                    // Stop joystick if within deadzone
                    if (Math.Abs(MouseSpeedX) < MouseDeadzone || Math.Abs(MouseSpeedY) < MouseDeadzone)
                    {
                        // Reset mouse speed
                        MouseSpeedX = 0;
                        MouseSpeedY = 0;

                        // Start release timer
                        if (MouseReleaseTimer == null)
                        {
                            MouseReleaseTimer = new System.Timers.Timer(MouseReleaseDelay);
                            MouseReleaseTimer.Start();
                            MouseReleaseTimer.Elapsed += (s, e) =>
                            {
                                // Recenter cursor
                                RemapperUtility.SetCursorPosition(500, 500);

                                // Reset cursor overflow
                                CursorOverflowX = 0;
                                CursorOverflowY = 0;

                                // Stop release timer
                                MouseReleaseTimer.Stop();
                                MouseReleaseTimer = null;
                            };

                        }
                    }
                }

                const double min = 0;
                const double max = 255;

                // Scale speed to joystick
                double rx = 128 + (MouseSpeedX * 127);
                double ry = 128 + (MouseSpeedY * 127);
                state.RX = (byte)((rx < min) ? min : (rx > max) ? max : rx);
                state.RY = (byte)((ry < min) ? min : (ry > max) ? max : ry);
            }
        }

        private bool CheckFocusedWindow()
        {
            return RemapperUtility.IsProcessInForeground(RemotePlayProcess);
        }

        public void OnKeyPressed(object sender, GlobalKeyboardHookEventArgs e)
        {
            // https://msdn.microsoft.com/en-us/library/windows/desktop/dd375731(v=vs.85).aspx

            if (!CheckFocusedWindow())
                return;

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

        public void OnMouseEvent(object sender, GlobalMouseHookEventArgs e)
        {
            bool focusedWindow = CheckFocusedWindow();

            // Focused
            if (focusedWindow)
            {
                // Hide cursor
                if (IsCursorShowing)
                {
                    RemapperUtility.ShowSystemCursor(false);
                    RemapperUtility.ShowStreamingToolBar(RemotePlayProcess, false);
                    IsCursorShowing = false;
                }
            }
            // Not focused
            else
            {
                // Show cursor
                if (!IsCursorShowing)
                {
                    RemapperUtility.ShowSystemCursor(true);
                    RemapperUtility.ShowStreamingToolBar(RemotePlayProcess, true);
                    IsCursorShowing = true;
                }

                // Ignore the rest if not focused
                return;
            }

            // Left mouse
            if (e.MouseState == GlobalMouseHook.MouseState.LeftButtonDown)
            {
                LeftMouseDown = true;
                e.Handled = focusedWindow;
            }
            else if (e.MouseState == GlobalMouseHook.MouseState.LeftButtonUp)
            {
                LeftMouseDown = false;
                e.Handled = focusedWindow;
            }
            // Right mouse
            else if (e.MouseState == GlobalMouseHook.MouseState.RightButtonDown)
            {
                RightMouseDown = true;
                e.Handled = focusedWindow;
            }
            else if (e.MouseState == GlobalMouseHook.MouseState.RightButtonUp)
            {
                RightMouseDown = false;
                e.Handled = focusedWindow;
            }
            // Mouse move
            else if (e.MouseState == GlobalMouseHook.MouseState.Move)
            {
                #region Store mouse stroke
                var newStroke = new MouseStroke()
                {
                    Timestamp = DateTime.Now,
                    RawData = e,
                    DidMoved = true,
                    X = e.MouseData.Point.X + CursorOverflowX,
                    Y = e.MouseData.Point.Y + CursorOverflowY
                };

                if (CurrentMouseStroke != null)
                {
                    double deltaTime = (newStroke.Timestamp - CurrentMouseStroke.Timestamp).TotalSeconds;
                    newStroke.VelocityX = (newStroke.X - CurrentMouseStroke.X) / deltaTime;
                    newStroke.VelocityY = (newStroke.Y - CurrentMouseStroke.Y) / deltaTime;
                }

                CurrentMouseStroke = newStroke;
                #endregion

                #region Adjust cursor position
                var x = e.MouseData.Point.X;
                var y = e.MouseData.Point.Y;

                var didSetPosition = false;
                var workingArea = Screen.PrimaryScreen.WorkingArea;

                if (x >= workingArea.Width)
                {
                    CursorOverflowX += workingArea.Width;
                    x = 0;
                    didSetPosition = true;
                }
                else if (x <= 0)
                {
                    CursorOverflowX -= workingArea.Width;
                    x = workingArea.Width;
                    didSetPosition = true;
                }

                if (y >= workingArea.Height)
                {
                    CursorOverflowY += workingArea.Height;
                    y = 0;
                    didSetPosition = true;
                }
                else if (y <= 0)
                {
                    CursorOverflowY -= workingArea.Height;
                    y = workingArea.Height;
                    didSetPosition = true;
                }

                if (didSetPosition)
                {
                    RemapperUtility.SetCursorPosition(x, y);
                    e.Handled = true;
                }
                #endregion
            }
        }

        public bool IsKeyDown()
        {
            return PressedKeys.Count > 0;
        }

        public bool IsKeyInUse(Keys key)
        {
            return KeysDict.ContainsKey(key);
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

            Action<BaseAction> addItem = item =>
            {
                try
                {
                    dict.Add(item.Key, item);
                }
                catch (ArgumentException ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            foreach (BaseAction item in MappingsDataBinding)
            {
                if (item.Key == Keys.None) continue;
                addItem(item);
            }
            foreach (BaseAction item in MacrosDataBinding)
            {
                if (item.Key == Keys.None) continue;
                addItem(item);
            }

            KeysDict = dict;
        }

        public void ExecuteActionsByKey(List<Keys> keys)
        {
            var state = new DualShockState();

            foreach (var key in keys)
            {
                if (key == Keys.None) continue;

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
                RemapperUtility.SetValue(state, action.Property, action.Value);
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
    }
}
