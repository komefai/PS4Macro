// PS4Macro (File: Classes/Settings.cs)
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

namespace PS4Macro.Classes
{
    public class Settings
    {
        public const string FILE_PATH = "settings.xml";

        public bool AutoInject { get; set; }
        public string StartupFile { get; set; }

        public Settings()
        {
            AutoInject = false;
            StartupFile = null;
        }

        public static Settings LoadOrCreate()
        {
            try
            {
                if (File.Exists(FILE_PATH))
                {
                    return Deserialize(FILE_PATH);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Settings Error: {0}", ex.Message);
            }

            return new Settings();
        }

        public static void Serialize(string path, Settings settings)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Settings));
            using (TextWriter writer = new StreamWriter(path))
            {
                serializer.Serialize(writer, settings);
            }
        }

        public static Settings Deserialize(string path)
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(Settings));
            using (TextReader reader = new StreamReader(path))
            {
                object obj = deserializer.Deserialize(reader);
                Settings settings = obj as Settings;
                return settings;
            }
        }
    }
}
