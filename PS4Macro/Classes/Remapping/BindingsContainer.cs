// PS4Macro (File: Classes/Remapping/BindingsContainer.cs)
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
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace PS4Macro.Classes.Remapping
{
    public class BindingsContainer
    {
        public List<MappingAction> Mappings { get; set; }
        public List<MacroAction> Macros { get; set; }

        public bool EnableMouseInput { get; set; }
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

        public static void Serialize(string path, BindingsContainer container)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(BindingsContainer));
            using (TextWriter writer = new StreamWriter(path))
            {
                serializer.Serialize(writer, container);
            }
        }

        public static BindingsContainer Deserialize(string path)
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(BindingsContainer));
            using (TextReader reader = new StreamReader(path))
            {
                object obj = deserializer.Deserialize(reader);
                BindingsContainer container = obj as BindingsContainer;
                return container;
            }
        }
    }
}
