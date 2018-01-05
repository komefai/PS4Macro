// PS4Macro (File: Classes/ScriptHostUtility.cs)
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

namespace PS4Macro.Classes
{
    class ScriptHostUtility
    {
        public static PS4RemotePlayInterceptor.DualShockState ConvertAPIToInterceptorState(PS4MacroAPI.DualShockState state)
        {
            var serialized = SerializeAPIState(state);
            return DeserializeInterceptorState(serialized);
        }

        public static PS4MacroAPI.DualShockState ConvertInterceptorToAPIState(PS4RemotePlayInterceptor.DualShockState state)
        {
            var serialized = SerializeInterceptorState(state);
            return DeserializeAPIState(serialized);
        }

        public static string SerializeAPIState(PS4MacroAPI.DualShockState state)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(PS4MacroAPI.DualShockState));
            var sb = new StringBuilder();

            using (StringWriter writer = new StringWriter(sb))
            {
                serializer.Serialize(writer, state);
                return sb.ToString();
            }
        }

        public static PS4MacroAPI.DualShockState DeserializeAPIState(string data)
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(PS4MacroAPI.DualShockState));
            using (StringReader reader = new StringReader(data))
            {
                object obj = deserializer.Deserialize(reader);
                return obj as PS4MacroAPI.DualShockState;
            }
        }

        public static string SerializeInterceptorState(PS4RemotePlayInterceptor.DualShockState state)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(PS4RemotePlayInterceptor.DualShockState));
            var sb = new StringBuilder();

            using (StringWriter writer = new StringWriter(sb))
            {
                serializer.Serialize(writer, state);
                return sb.ToString();
            }
        }

        public static PS4RemotePlayInterceptor.DualShockState DeserializeInterceptorState(string data)
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(PS4RemotePlayInterceptor.DualShockState));
            using (StringReader reader = new StringReader(data))
            {
                object obj = deserializer.Deserialize(reader);
                return obj as PS4RemotePlayInterceptor.DualShockState;
            }
        }

        #region Sequence
        public static List<PS4RemotePlayInterceptor.DualShockState> ConvertAPIToInterceptorSequence(List<PS4MacroAPI.DualShockState> sequence)
        {
            var serialized = SerializeAPISequence(sequence);
            return DeserializeInterceptorSequence(serialized);
        }

        public static List<PS4MacroAPI.DualShockState> ConvertInterceptorToAPISequence(List<PS4RemotePlayInterceptor.DualShockState> sequence)
        {
            var serialized = SerializeInterceptorSequence(sequence);
            return DeserializeAPISequence(serialized);
        }

        public static string SerializeAPISequence(List<PS4MacroAPI.DualShockState> sequence)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<PS4MacroAPI.DualShockState>));
            var sb = new StringBuilder();

            using (StringWriter writer = new StringWriter(sb))
            {
                serializer.Serialize(writer, sequence);
                return sb.ToString();
            }
        }

        public static List<PS4MacroAPI.DualShockState> DeserializeAPISequence(string data)
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(List<PS4MacroAPI.DualShockState>));
            using (StringReader reader = new StringReader(data))
            {
                object obj = deserializer.Deserialize(reader);
                return obj as List<PS4MacroAPI.DualShockState>;
            }
        }

        public static string SerializeInterceptorSequence(List<PS4RemotePlayInterceptor.DualShockState> sequence)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<PS4RemotePlayInterceptor.DualShockState>));
            var sb = new StringBuilder();

            using (StringWriter writer = new StringWriter(sb))
            {
                serializer.Serialize(writer, sequence);
                return sb.ToString();
            }
        }

        public static List<PS4RemotePlayInterceptor.DualShockState> DeserializeInterceptorSequence(string data)
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(List<PS4RemotePlayInterceptor.DualShockState>));
            using (StringReader reader = new StringReader(data))
            {
                object obj = deserializer.Deserialize(reader);
                return obj as List<PS4RemotePlayInterceptor.DualShockState>;
            }
        }
        #endregion
    }
}
