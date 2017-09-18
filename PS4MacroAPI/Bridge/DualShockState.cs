// PS4MacroAPI (File: Bridge/DualShockState.cs)
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
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace PS4MacroAPI
{
    public class Touch
    {
        public byte TouchID { get; set; }
        public bool IsTouched { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        /* Constructors */
        public Touch() { }
        public Touch(byte touchID, bool isTouched, int x, int y)
        {
            TouchID = touchID;
            IsTouched = isTouched;
            X = x;
            Y = y;
        }
        public Touch(Touch touch)
        {
            TouchID = touch.TouchID;
            IsTouched = touch.IsTouched;
            X = touch.X;
            Y = touch.Y;
        }

        public Touch Clone()
        {
            return new Touch(this);
        }
    }

    public class DualShockState
    {
        public const int TOUCHPAD_DATA_OFFSET = 35;

        private enum VK : byte
        {
            L2 = 1 << 2,
            R2 = 1 << 3,
            Triangle = 1 << 7,
            Circle = 1 << 6,
            Cross = 1 << 5,
            Square = 1 << 4,
            DPad_Up = 1 << 3,
            DPad_Down = 1 << 2,
            DPad_Left = 1 << 1,
            DPad_Right = 1 << 0,
            L1 = 1 << 0,
            R1 = 1 << 1,
            Share = 1 << 4,
            Options = 1 << 5,
            L3 = 1 << 6,
            R3 = 1 << 7,
            PS = 1 << 0,
            TouchButton = 1 << 2 - 1
        }

        public DateTime ReportTimeStamp { get; set; }
        public byte LX { get; set; }
        public byte LY { get; set; }
        public byte RX { get; set; }
        public byte RY { get; set; }
        public byte L2 { get; set; }
        public byte R2 { get; set; }
        public bool Triangle { get; set; }
        public bool Circle { get; set; }
        public bool Cross { get; set; }
        public bool Square { get; set; }
        public bool DPad_Up { get; set; }
        public bool DPad_Down { get; set; }
        public bool DPad_Left { get; set; }
        public bool DPad_Right { get; set; }
        public bool L1 { get; set; }
        public bool R1 { get; set; }
        public bool Share { get; set; }
        public bool Options { get; set; }
        public bool L3 { get; set; }
        public bool R3 { get; set; }
        public bool PS { get; set; }

        public Touch Touch1 { get; set; }
        public Touch Touch2 { get; set; }
        public bool TouchButton { get; set; }
        public byte TouchPacketCounter { get; set; }

        public byte FrameCounter { get; set; }
        public byte Battery { get; set; }
        public bool IsCharging { get; set; }

        public short AccelX { get; set; }
        public short AccelY { get; set; }
        public short AccelZ { get; set; }
        public short GyroX { get; set; }
        public short GyroY { get; set; }
        public short GyroZ { get; set; }

        /* Constructor */
        public DualShockState()
        {
            LX = 0x80;
            LY = 0x80;
            RX = 0x80;
            RY = 0x80;
            FrameCounter = 255; // null
            TouchPacketCounter = 255; // null
        }

        public DualShockState(DualShockState state)
        {
            ReportTimeStamp = state.ReportTimeStamp;
            LX = state.LX;
            LY = state.LY;
            RX = state.RX;
            RY = state.RY;
            L2 = state.L2;
            R2 = state.R2;
            Triangle = state.Triangle;
            Circle = state.Circle;
            Cross = state.Cross;
            Square = state.Square;
            DPad_Up = state.DPad_Up;
            DPad_Down = state.DPad_Down;
            DPad_Left = state.DPad_Left;
            DPad_Right = state.DPad_Right;
            L1 = state.L1;
            R3 = state.R3;
            Share = state.Share;
            Options = state.Options;
            R1 = state.R1;
            L3 = state.L3;
            PS = state.PS;
            Touch1 = state.Touch1.Clone();
            Touch2 = state.Touch2.Clone();
            TouchButton = state.TouchButton;
            TouchPacketCounter = state.TouchPacketCounter;
            FrameCounter = state.FrameCounter;
            Battery = state.Battery;
            IsCharging = state.IsCharging;
            AccelX = state.AccelX;
            AccelY = state.AccelY;
            AccelZ = state.AccelZ;
            GyroX = state.GyroX;
            GyroY = state.GyroY;
            GyroZ = state.GyroZ;
        }

        public void CopyTo(DualShockState state)
        {
            state.ReportTimeStamp = ReportTimeStamp;
            state.LX = LX;
            state.LY = LY;
            state.RX = RX;
            state.RY = RY;
            state.L2 = L2;
            state.R2 = R2;
            state.Triangle = Triangle;
            state.Circle = Circle;
            state.Cross = Cross;
            state.Square = Square;
            state.DPad_Up = DPad_Up;
            state.DPad_Down = DPad_Down;
            state.DPad_Left = DPad_Left;
            state.DPad_Right = DPad_Right;
            state.L1 = L1;
            state.R3 = R3;
            state.Share = Share;
            state.Options = Options;
            state.R1 = R1;
            state.L3 = L3;
            state.PS = PS;
            state.Touch1 = Touch1.Clone();
            state.Touch2 = Touch2.Clone();
            state.TouchButton = TouchButton;
            state.TouchPacketCounter = TouchPacketCounter;
            state.FrameCounter = FrameCounter;
            state.Battery = Battery;
            state.IsCharging = IsCharging;
            state.AccelX = AccelX;
            state.AccelY = AccelY;
            state.AccelZ = AccelZ;
            state.GyroX = GyroX;
            state.GyroY = GyroY;
            state.GyroZ = GyroZ;
        }

        public DualShockState Clone()
        {
            return new DualShockState(this);
        }

        /// <summary>
        /// Serialize a list of DualShockState to xml file
        /// </summary>
        public static void Serialize(string path, List<DualShockState> list)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<DualShockState>));
            using (TextWriter writer = new StreamWriter(path))
            {
                serializer.Serialize(writer, list);
            }
        }

        /// <summary>
        /// Deserialize a list of DualShockState from xml file
        /// </summary>
        public static List<DualShockState> Deserialize(string path)
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(List<DualShockState>));
            using (TextReader reader = new StreamReader(path))
            {
                object obj = deserializer.Deserialize(reader);
                List<DualShockState> list = obj as List<DualShockState>;
                return list;
            }
        }
    }
}
