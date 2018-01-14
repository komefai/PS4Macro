// PS4Macro (File: Program.cs)
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

using Mono.Options;
using PS4Macro.Classes;
using System;
using System.Threading;
using System.Windows.Forms;

namespace PS4Macro
{
    static class Program
    {
        private static Settings m_Settings = Settings.LoadDefaultOrCreate();
        public static Settings Settings => m_Settings;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Get command line arguments
            string[] args = Environment.GetCommandLineArgs();

            // Parse command line arguments
            try
            {
                var p = new OptionSet()
                    .Add("AutoInject:", "Automatically poll for PS4 Remote Play and inject whenever possible", v => m_Settings.AutoInject = v == null ? true : Convert.ToBoolean(v))
                    .Add("BypassInjection:", "Bypass the injection for debugging purposes", v => m_Settings.BypassInjection = v == null ? true : Convert.ToBoolean(v))
                    .Add("EmulateController:", "Run with controller emulation (use without a controller)", v => m_Settings.EmulateController = v == null ? true : Convert.ToBoolean(v))
                    .Add("ShowConsole:", "Open debugging console on launch", v => m_Settings.ShowConsole = v == null ? true : Convert.ToBoolean(v))
                    .Add("StartupFile=", "Absolute or relative path to the file to load on launch (can be xml or dll)", v => m_Settings.StartupFile = v);

                p.Add("Settings=", "Absolute or relative path to the settings file (will take priority)", v => m_Settings = Settings.Load(v));
                p.Add("h|?|help", "Displays this help message", v => {
                    p.WriteOptionDescriptions(Console.Out);
                    Environment.Exit(0);
                });

                var extras = p.Parse(args);
            }
            catch (OptionException ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Try `PS4Macro --help` for usage.");
            }

            // Display console for debugging if enabled
            if (Settings.ShowConsole)
            {
                ConsoleHelper.AllocConsole();
                ConsoleHelper.SetOut();
            }

            // Add the event handler for handling UI thread exceptions to the event.
            Application.ThreadException += new ThreadExceptionEventHandler(OnThreadException);

            // Set the unhandled exception mode to force all Windows Forms errors
            // to go through the handler.
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

            // Add the event handler for handling non-UI thread exceptions to the event. 
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(OnUnhandledException);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Forms.MainForm());
        }

        static void OnThreadException(object sender, ThreadExceptionEventArgs e)
        {
            MessageBox.Show(e.Exception.ToString(), "Unhandled Thread Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.ExceptionObject.ToString(), "Unhandled Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
