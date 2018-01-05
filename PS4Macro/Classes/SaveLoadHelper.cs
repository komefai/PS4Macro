// PS4Macro (File: Classes/SaveLoadHelper.cs)
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

using PS4Macro.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace PS4Macro.Classes
{
    public class SaveLoadHelper : INotifyPropertyChanged
    {
        public const string DEFAULT_FILE_NAME = "untitled.xml";
        private const string SAVE_FILTER = "XML Files (*.xml)|*.xml|All files (*.*)|*.*";
        private const string LOAD_FILTER = "Supported Files (*.xml, *.dll)|*.xml;*.dll|All files (*.*)|*.*";

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region Properties
        private string m_CurrentFile = null;
        public string CurrentFile
        {
            get { return m_CurrentFile; }
            private set
            {
                if (value != m_CurrentFile)
                {
                    m_CurrentFile = value;
                    NotifyPropertyChanged("CurrentFile");
                }
            }
        }
        #endregion

        private MainForm m_MainForm;
        private MacroPlayer m_MacroPlayer;

        /* Constructor */
        public SaveLoadHelper(MainForm mainForm, MacroPlayer macroPlayer)
        {
            m_MainForm = mainForm;
            m_MacroPlayer = macroPlayer;
        }

        public void ClearCurrentFile()
        {
            CurrentFile = null;
        }

        public void DirectLoad(string filename)
        {
            // Load
            CurrentFile = filename;

            try
            {
                // Macro
                if (CurrentFile.EndsWith("xml"))
                {
                    m_MainForm.LoadMacro(CurrentFile);
                }
                // Script
                else if (CurrentFile.EndsWith("dll"))
                {
                    m_MainForm.LoadScript(CurrentFile);
                }
                // Invalid
                else
                {
                    ClearCurrentFile();
                }
            }
            catch (Exception ex)
            {
                ClearCurrentFile();
                MessageBox.Show(ex.Message, "Open Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void Load()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.InitialDirectory = Assembly.GetExecutingAssembly().Location;
            openFileDialog.Filter = LOAD_FILTER;
            openFileDialog.FilterIndex = 0;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Load
                DirectLoad(openFileDialog.FileName);
            }
        }

        public void Save()
        {
            // Currently has a file loaded
            if (!string.IsNullOrWhiteSpace(CurrentFile))
            {
                if (File.Exists(CurrentFile))
                {
                    // Save
                    m_MacroPlayer.SaveFile(CurrentFile);
                }
                else
                {
                    // Save as
                    SaveAs(DEFAULT_FILE_NAME);
                }
            }
            // No file loaded
            else
            {
                // Save as
                SaveAs(DEFAULT_FILE_NAME);
            }
        }

        public void SaveAs(string fileName = null)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.InitialDirectory = Assembly.GetExecutingAssembly().Location;
            saveFileDialog.Filter = SAVE_FILTER;
            saveFileDialog.FilterIndex = 0;
            saveFileDialog.RestoreDirectory = true;
            saveFileDialog.FileName = fileName == null ? Path.GetFileName(CurrentFile) : fileName;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Save
                CurrentFile = saveFileDialog.FileName;
                m_MacroPlayer.SaveFile(CurrentFile);
            }
        }
    }
}
