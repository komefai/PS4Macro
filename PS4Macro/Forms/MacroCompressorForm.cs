// PS4Macro (File: Forms/MacroCompressorForm.cs)
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

using PS4Macro.Classes;
using PS4RemotePlayInterceptor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace PS4Macro.Forms
{
    public partial class MacroCompressorForm : Form
    {
        private string CurrentPath { get; set; }
        protected bool IsDataValid { get; set; }
        protected List<DualShockState> Sequence { get; set; }
        protected Thread GetDataThread { get; set; }

        public MacroCompressorForm()
        {
            InitializeComponent();

            // Default properties
            ResetDefaultProperties();
        }

        private void UpdateAllProperties(bool value)
        {
            for (var i = 0; i < propertiesCheckedListBox.Items.Count; i++)
            {
                propertiesCheckedListBox.SetItemChecked(i, value);
            }

            UpdateExportButton();
        }

        private void ResetDefaultProperties()
        {
            var defaultChecked = new List<string>()
            {
                "LX",
                "LY",
                "RX",
                "RY",
                "L2",
                "R2",
                "Triangle",
                "Circle",
                "Cross",
                "Square",
                "DPad_Up",
                "DPad_Down",
                "DPad_Left",
                "DPad_Right",
                "L1",
                "R3",
                "Share",
                "Options",
                "R1",
                "L3",
                "PS",
                "TouchButton"
            };

            for (var i = 0; i < propertiesCheckedListBox.Items.Count; i++)
            {
                var shouldCheck = defaultChecked.Contains(propertiesCheckedListBox.Items[i]);
                propertiesCheckedListBox.SetItemChecked(i, shouldCheck);
            }
        }

        private void LoadSequence(string sequencePath)
        {
            CurrentPath = sequencePath;

            using (FileStream stream = new FileStream(sequencePath, FileMode.Open, FileAccess.Read))
            {
                using (TextReader reader = new StreamReader(stream))
                {
                    XmlSerializer deserializer = new XmlSerializer(typeof(List<DualShockState>));
                    object obj = deserializer.Deserialize(reader);
                    Sequence = obj as List<DualShockState>;
                }
            }
        }

        private void UpdateUI()
        {
            fileNameLabel.Text = Path.GetFileName(CurrentPath);
            framesCountLabel.Text = $"{Sequence.Count} frames";

            UpdateExportButton();
        }

        private void UpdateExportButton()
        {
            exportButton.Enabled = !string.IsNullOrWhiteSpace(CurrentPath) && propertiesCheckedListBox.CheckedItems.Count > 0;
        }

        private bool GetFilename(out string filename, DragEventArgs e)
        {
            bool ret = false;
            filename = String.Empty;
            if ((e.AllowedEffect & DragDropEffects.Copy) == DragDropEffects.Copy)
            {
                Array data = e.Data.GetData("FileDrop") as Array;
                if (data != null)
                {
                    if ((data.Length == 1) && (data.GetValue(0) is string))
                    {
                        filename = ((string[])data)[0];
                        string ext = Path.GetExtension(filename).ToLower();
                        if (ext == ".xml")
                        {
                            ret = true;
                        }
                    }
                }
            }
            return ret;
        }

        private void MacroCompressorForm_DragEnter(object sender, DragEventArgs e)
        {
            string inputPath;
            IsDataValid = GetFilename(out inputPath, e);
            if (IsDataValid)
            {
                GetDataThread = new Thread(new ThreadStart(() =>
                {
                    try
                    {
                        LoadSequence(inputPath);
                    }
                    catch
                    {
                        IsDataValid = false;
                        CurrentPath = null;
                    }
                }));

                GetDataThread.Start();
                e.Effect = DragDropEffects.Copy;
            }
            else
                e.Effect = DragDropEffects.None;
        }

        private void MacroCompressorForm_DragDrop(object sender, DragEventArgs e)
        {
            if (IsDataValid)
            {
                while (GetDataThread.IsAlive)
                {
                    Application.DoEvents();
                    Thread.Sleep(0);
                }

                UpdateUI();
            }
        }

        private void fileNameLabel_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.InitialDirectory = Assembly.GetExecutingAssembly().Location;
            openFileDialog.Filter = SaveLoadHelper.XML_FILTER;
            openFileDialog.FilterIndex = 0;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                LoadSequence(openFileDialog.FileName);
                UpdateUI();
            }
        }

        private void selectAllButton_Click(object sender, EventArgs e)
        {
            UpdateAllProperties(true);
        }

        private void deselectAllButton_Click(object sender, EventArgs e)
        {
            UpdateAllProperties(false);
        }

        private void exportButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.InitialDirectory = Path.GetDirectoryName(CurrentPath);
            saveFileDialog.Filter = SaveLoadHelper.XML_FILTER;
            saveFileDialog.FilterIndex = 0;
            saveFileDialog.RestoreDirectory = true;
            saveFileDialog.FileName = Path.GetFileName(CurrentPath).Replace(".xml", ".min.xml");

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                List<string> excludeProperties = new List<string>();

                var checkedItems = propertiesCheckedListBox.CheckedItems;
                foreach (var item in propertiesCheckedListBox.Items)
                {
                    if (!checkedItems.Contains(item)) excludeProperties.Add(item.ToString());
                }

                // Compress macro
                //DualShockState.SerializeCompressed(saveFileDialog.FileName, sequence, excludeProperties);
            }
        }

        private void propertiesCheckedListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (propertiesCheckedListBox.IsHandleCreated)
                BeginInvoke((MethodInvoker)(() => UpdateExportButton()));
        }
    }
}
