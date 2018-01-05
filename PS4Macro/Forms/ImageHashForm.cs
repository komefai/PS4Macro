// PS4Macro (File: Forms/ImageHashForm.cs)
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

using PS4MacroAPI;
using PS4MacroAPI.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace PS4Macro.Forms
{
    public partial class ImageHashForm : Form
    {
        protected bool validData;
        string path;
        protected Image image;
        protected Thread getImageThread;

        public ImageHashForm()
        {
            InitializeComponent();
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
                        if ((ext == ".jpg") || (ext == ".png") || (ext == ".bmp"))
                        {
                            ret = true;
                        }
                    }
                }
            }
            return ret;
        }

        private void CompareImages()
        {
            if (imageAPictureBox.Image == null || imageBPictureBox.Image == null)
                return;

            var hashA = ImageHashing.AverageHash(imageAPictureBox.Image);
            var hashB = ImageHashing.AverageHash(imageBPictureBox.Image);

            //var similarity = ImageHashing.Similarity(imageAPictureBox.Image, imageBPictureBox.Image);
            var similarity = ImageHashing.Similarity(hashA, hashB);

            var message = String.Format("SIMILARITY: {0}%", similarity);
            //MessageBox.Show(message);
            compareButton.Text = message;
        }

        private void OnImageChanged(Image image, PictureBox pictureBox)
        {
            if (pictureBox == imageAPictureBox)
            {
                var hash = ImageHashing.AverageHash(imageAPictureBox.Image);
                imageAHashTextBox.Text = hash.ToString();
            }
            else if (pictureBox == imageBPictureBox)
            {
                var hash = ImageHashing.AverageHash(imageBPictureBox.Image);
                imageBHashTextBox.Text = hash.ToString();
            }
        }

        private void compareButton_Click(object sender, EventArgs e)
        {
            CompareImages();
        }

        private void ImageHashForm_DragEnter(object sender, DragEventArgs e)
        {
            string filename;
            validData = GetFilename(out filename, e);
            if (validData)
            {
                path = filename;
                getImageThread = new Thread(new ThreadStart(() =>
                {
                    using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
                    {
                        image = Image.FromStream(stream);
                    }
                }));

                getImageThread.Start();
                e.Effect = DragDropEffects.Copy;
            }
            else
                e.Effect = DragDropEffects.None;
        }

        private void ImageHashForm_DragDrop(object sender, DragEventArgs e)
        {
            if (validData)
            {
                while (getImageThread.IsAlive)
                {
                    Application.DoEvents();
                    Thread.Sleep(0);
                }

                PictureBox pb = null;

                if (this.PointToClient(new Point(e.X, e.Y)).X <= Size.Width / 2)
                    pb = imageAPictureBox;
                else
                    pb = imageBPictureBox;

                pb.Image = image;
                OnImageChanged(image, pb);
            }
        }
    }
}
