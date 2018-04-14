// PS4Macro(File: Classes/Remapping/Remapper.cs)
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
using System.ComponentModel;

namespace PS4Macro.Classes.Remapping
{
    public enum AnalogStick
    {
        Left,
        Right
    }

    public class Remapper
    {
        //private const int MOUSE_CENTER_X = 500;
        //private const int MOUSE_CENTER_Y = 500;
        //private const int MOUSE_RELEASE_TIME = 50;
        private const int MOUSE_SENSITIVITY_DIVISOR = 100000;

        // Delegates
        public delegate void OnMouseAxisChangedDelegate(byte x, byte y);
        public OnMouseAxisChangedDelegate OnMouseAxisChanged { get; set; }

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
        public bool MiddleMouseDown { get; private set; }
        public double MouseSpeedX { get; private set; }
        public double MouseSpeedY { get; private set; }

        public bool EnableMouseInput { get; set; }
        public bool DebugCursor { get; set; }
        public double MouseSensitivity { get; set; }
        public double MouseDecayRate { get; set; }
        public double MouseDecayThreshold { get; set; }
        public double MouseAnalogDeadzone { get; set; }
        public double MouseMakeupSpeed { get; set; }
        public AnalogStick MouseMovementAnalog { get; set; }
        public bool MouseInvertXAxis { get; set; }
        public bool MouseInvertYAxis { get; set; }
        public int LeftMouseMapping { get; set; }
        public int RightMouseMapping { get; set; }
        public int MiddleMouseMapping { get; set; }

        public MacroPlayer MacroPlayer { get; private set; }
        public bool UsingMacroPlayer { get; private set; }

        public List<MappingAction> MappingsDataBinding { get; private set; }
        public List<MacroAction> MacrosDataBinding { get; private set; }

        public Remapper()
        {
            PressedKeys = new Dictionary<Keys, bool>();
            KeysDict = new Dictionary<Keys, BaseAction>();

            IsCursorShowing = true;

            EnableMouseInput = false;
            DebugCursor = false;
            MouseSensitivity = 1;
            MouseDecayRate = 1.2;
            MouseDecayThreshold = 0.1;
            MouseAnalogDeadzone = 14.25;
            MouseMakeupSpeed = 500;
            MouseMovementAnalog = AnalogStick.Right;
            MouseInvertXAxis = false;
            MouseInvertYAxis = false;
            LeftMouseMapping = 11; // R2
            RightMouseMapping = 10; // L2
            MiddleMouseMapping = 9; // L1

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
                if (!CheckFocusedWindow())
                    return;

                // Create the default state to modify
                if (CurrentState == null)
                {
                    CurrentState = new DualShockState() { Battery = 255 };
                }

                // Mouse Input
                if (EnableMouseInput)
                {
                    var checkState = new DualShockState();

                    // Left mouse
                    var leftMap = MappingsDataBinding.ElementAtOrDefault(LeftMouseMapping);
                    if (leftMap != null)
                    {
                        if (LeftMouseDown)
                        {
                            RemapperUtility.SetValue(CurrentState, leftMap.Property, leftMap.Value);
                        }
                        else
                        {
                            var defaultValue = RemapperUtility.GetValue(checkState, leftMap.Property);
                            RemapperUtility.SetValue(CurrentState, leftMap.Property, defaultValue);
                        }
                    }


                    // Right mouse
                    var rightMap = MappingsDataBinding.ElementAtOrDefault(RightMouseMapping);
                    if (rightMap != null)
                    {
                        if (RightMouseDown)
                        {
                            RemapperUtility.SetValue(CurrentState, rightMap.Property, rightMap.Value);
                        }
                        else
                        {
                            var defaultValue = RemapperUtility.GetValue(checkState, rightMap.Property);
                            RemapperUtility.SetValue(CurrentState, rightMap.Property, defaultValue);
                        }
                    }

                    // Middle mouse
                    var middleMap = MappingsDataBinding.ElementAtOrDefault(MiddleMouseMapping);
                    if (middleMap != null)
                    {
                        if (MiddleMouseDown)
                        {
                            RemapperUtility.SetValue(CurrentState, middleMap.Property, middleMap.Value);
                        }
                        else
                        {
                            var defaultValue = RemapperUtility.GetValue(checkState, middleMap.Property);
                            RemapperUtility.SetValue(CurrentState, middleMap.Property, defaultValue);
                        }
                    }

                    // Mouse moved
                    if (CurrentMouseStroke != null && CurrentMouseStroke.DidMoved)
                    {
                        MouseSpeedX = (CurrentMouseStroke.VelocityX * MouseSensitivity) / MOUSE_SENSITIVITY_DIVISOR;
                        if (MouseInvertXAxis) MouseSpeedX *= -1;

                        MouseSpeedY = (CurrentMouseStroke.VelocityY * MouseSensitivity) / MOUSE_SENSITIVITY_DIVISOR;
                        if (MouseInvertYAxis) MouseSpeedY *= -1;

                        CurrentMouseStroke.DidMoved = false;

                        // Stop release timer
                        //if (MouseReleaseTimer != null)
                        //{
                        //    MouseReleaseTimer.Stop();
                        //    MouseReleaseTimer = null;
                        //}
                    }
                    // Mouse idle
                    else
                    {
                        // Start decay
                        MouseSpeedX /= MouseDecayRate;
                        MouseSpeedY /= MouseDecayRate;

                        // Stop decaying joystick if below threshold
                        //if (Math.Abs(MouseSpeedX) < MouseDecayThreshold || Math.Abs(MouseSpeedY) < MouseDecayThreshold)
                        //{
                        //    // Reset mouse speed
                        //    if (Math.Abs(MouseSpeedX) < MouseDecayThreshold) MouseSpeedX = 0;
                        //    if (Math.Abs(MouseSpeedY) < MouseDecayThreshold) MouseSpeedY = 0;

                        //    // Start release timer
                        //    if (MouseReleaseTimer == null)
                        //    {
                        //        MouseReleaseTimer = new System.Timers.Timer(MOUSE_RELEASE_TIME);
                        //        MouseReleaseTimer.Start();
                        //        MouseReleaseTimer.Elapsed += (s, e) =>
                        //        {
                        //            // Recenter cursor
                        //            RemapperUtility.SetCursorPosition(MOUSE_CENTER_X, MOUSE_CENTER_Y);

                        //            // Reset cursor overflow
                        //            CursorOverflowX = 0;
                        //            CursorOverflowY = 0;

                        //            // Stop release timer
                        //            MouseReleaseTimer.Stop();
                        //            MouseReleaseTimer = null;
                        //        };

                        //    }
                        //}
                    }

                    const double min = 0;
                    const double max = 255;
                    string analogProperty = MouseMovementAnalog == AnalogStick.Left ? "L" : "R";

                    // Minimum speed
                    double positiveSpeed = 128 + MouseAnalogDeadzone;
                    double negativeSpeed = 128 - MouseAnalogDeadzone;

                    // Base speed
                    double baseX = ((MouseSpeedX > 0) ? positiveSpeed : ((MouseSpeedX < 0) ? negativeSpeed : 128));
                    double baseY = ((MouseSpeedY > 0) ? positiveSpeed : ((MouseSpeedY < 0) ? negativeSpeed : 128));

                    // Makeup speed
                    double makeupX = Math.Sign(MouseSpeedX) * MouseMakeupSpeed;
                    double makeupY = Math.Sign(MouseSpeedY) * MouseMakeupSpeed;

                    // Scale speed to analog values
                    double rx = baseX + (makeupX * MouseSpeedX * MouseSpeedX * 127);
                    double ry = baseY + (makeupY * MouseSpeedY * MouseSpeedY * 127);

                    byte scaledX = (byte)((rx < min) ? min : (rx > max) ? max : rx);
                    byte scaledY = (byte)((ry < min) ? min : (ry > max) ? max : ry);

                    // Set the analog values
                    RemapperUtility.SetValue(CurrentState, analogProperty + "X", scaledX);
                    RemapperUtility.SetValue(CurrentState, analogProperty + "Y", scaledY);

                    // Invoke callback
                    OnMouseAxisChanged?.Invoke(scaledX, scaledY);
                }

                // Assign the state
                state = CurrentState;
            }
        }

        public void RefreshProcess()
        {
            try
            {
                if (RemotePlayProcess != null && !RemotePlayProcess.HasExited)
                {
                    RemotePlayProcess.Refresh();
                }
            }
            catch (Exception) { }
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
                if (EnableMouseInput)
                {
                    // Hide cursor
                    if (!DebugCursor && IsCursorShowing)
                    {
                        ShowCursorAndToolbar(false);
                    }
                }
            }
            // Not focused
            else
            {
                // Show cursor
                if (!DebugCursor && !IsCursorShowing)
                {
                    ShowCursorAndToolbar(true);
                }

                // Ignore the rest if not focused
                return;
            }

            // Ignore if disabled
            if (!EnableMouseInput)
                return;

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
            // Middle mouse
            else if (e.MouseState == GlobalMouseHook.MouseState.MiddleButtonDown)
            {
                MiddleMouseDown = true;
                e.Handled = focusedWindow;
            }
            else if (e.MouseState == GlobalMouseHook.MouseState.MiddleButtonUp)
            {
                MiddleMouseDown = false;
                e.Handled = focusedWindow;
            }
            // Mouse move
            else if (e.MouseState == GlobalMouseHook.MouseState.Move)
            {
                var rawX = e.MouseData.Point.X;
                var rawY = e.MouseData.Point.Y;

                // Ignore if at center
                //if (rawX == MOUSE_CENTER_X && rawY == MOUSE_CENTER_Y)
                //    return;

                #region Store mouse stroke
                var newStroke = new MouseStroke()
                {
                    Timestamp = DateTime.Now,
                    RawData = e,
                    DidMoved = true,
                    X = rawX + CursorOverflowX,
                    Y = rawY + CursorOverflowY
                };

                if (CurrentMouseStroke != null)
                {
                    double deltaTime = (newStroke.Timestamp - CurrentMouseStroke.Timestamp).TotalSeconds;
                    newStroke.VelocityX = (newStroke.X - CurrentMouseStroke.X) / deltaTime;
                    newStroke.VelocityY = (newStroke.Y - CurrentMouseStroke.Y) / deltaTime;
                }

                CurrentMouseStroke = newStroke;
                #endregion

                #region Adjust cursor position;
                var didSetPosition = false;
                var screen = Screen.FromHandle(RemotePlayProcess.MainWindowHandle);
                var workingArea = screen.WorkingArea;
                var tmpX = rawX - workingArea.X;
                var tmpY = rawY - workingArea.Y;

                if (tmpX >= workingArea.Width)
                {
                    CursorOverflowX += workingArea.Width;
                    //tmpX = 0;
                    didSetPosition = true;
                }
                else if (tmpX <= 0)
                {
                    CursorOverflowX -= workingArea.Width;
                    //tmpX = workingArea.Width;
                    didSetPosition = true;
                }

                if (tmpY >= workingArea.Height)
                {
                    CursorOverflowY += workingArea.Height;
                    //tmpY = 0;
                    didSetPosition = true;
                }
                else if (tmpY <= 0)
                {
                    CursorOverflowY -= workingArea.Height;
                    //tmpY = workingArea.Height;
                    didSetPosition = true;
                }

                // Block cursor
                if (didSetPosition)
                {
                    //RemapperUtility.SetCursorPosition(tmpX, tmpY);
                    e.Handled = true;
                }
                #endregion
            }
        }

        private void ShowCursorAndToolbar(bool value)
        {
            if (value)
            {
                RemapperUtility.ShowSystemCursor(true);
                RemapperUtility.ShowStreamingToolBar(RemotePlayProcess, true);
                IsCursorShowing = true;
            }
            else
            {
                RemapperUtility.ShowSystemCursor(false);
                RemapperUtility.ShowStreamingToolBar(RemotePlayProcess, false);
                IsCursorShowing = false;
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

        public string GetBindingsFilePath()
        {
            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\" + "bindings.xml";
        }

        public void SaveBindings()
        {
            var container = new BindingsContainer
            {
                Mappings = MappingsDataBinding,
                Macros = MacrosDataBinding,
                EnableMouseInput = EnableMouseInput,
                MouseSensitivity = MouseSensitivity,
                MouseDecayRate = MouseDecayRate,
                MouseDecayThreshold = MouseDecayThreshold,
                MouseAnalogDeadzone = MouseAnalogDeadzone,
                MouseMakeupSpeed = MouseMakeupSpeed,
                MouseMovementAnalog = MouseMovementAnalog,
                MouseInvertXAxis = MouseInvertXAxis,
                MouseInvertYAxis = MouseInvertYAxis,
                LeftMouseMapping = LeftMouseMapping,
                RightMouseMapping = RightMouseMapping
            };


            BindingsContainer.Serialize(GetBindingsFilePath(), container);
        }

        public void LoadBindings()
        {
            var container = BindingsContainer.Deserialize(GetBindingsFilePath());

            MappingsDataBinding = container.Mappings;
            MacrosDataBinding = container.Macros;

            EnableMouseInput = container.EnableMouseInput;
            MouseSensitivity = container.MouseSensitivity;
            MouseDecayRate = container.MouseDecayRate;
            MouseDecayThreshold = container.MouseDecayThreshold;
            MouseAnalogDeadzone = container.MouseAnalogDeadzone;
            MouseMakeupSpeed = container.MouseMakeupSpeed;
            MouseMovementAnalog = container.MouseMovementAnalog;
            MouseInvertXAxis = container.MouseInvertXAxis;
            MouseInvertYAxis = container.MouseInvertYAxis;
            LeftMouseMapping = container.LeftMouseMapping;
            RightMouseMapping = container.RightMouseMapping;
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
