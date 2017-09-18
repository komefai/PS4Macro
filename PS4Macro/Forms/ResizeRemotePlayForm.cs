// PS4Macro (File: Forms/ResizeRemotePlayForm.cs)
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

using PS4MacroAPI;
using PS4MacroAPI.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PS4Macro.Forms
{
    public partial class ResizeRemotePlayForm : Form
    {
        public ResizeRemotePlayForm()
        {
            InitializeComponent();
        }

        private void ResizeRemotePlayForm_Load(object sender, EventArgs e)
        {
            try
            {
                var size = ScriptUtility.GetWindowSize();
                widthNumericUpDown.Value = size.Width;
                heightNumericUpDown.Value = size.Height;
            }
            catch {}
        }

        private void resizeButton_Click(object sender, EventArgs e)
        {
            try
            {
                Size size = new Size((int)widthNumericUpDown.Value, (int)heightNumericUpDown.Value);
                ScriptUtility.ResizeWindow(size);
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Resize Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
