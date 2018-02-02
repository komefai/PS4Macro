// PS4Macro(File: Controls/AxisDisplay.cs)
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
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PS4Macro.Controls
{
    class AxisDisplay : UserControl
    {
        private Color m_OuterColor;
        public Color OuterColor
        {
            get { return m_OuterColor; }
            set
            {
                m_OuterBrush = new SolidBrush(value);
                m_OuterColor = value;
            }
        }
        private Brush m_OuterBrush = null;

        private Color m_InnerColor;
        public Color InnerColor
        {
            get { return m_InnerColor; }
            set
            {
                m_InnerBrush = new SolidBrush(value);
                m_InnerColor = value;
            }
        }
        private Brush m_InnerBrush = null;

        public int InnerSize { get; set; }

        public PointF Value { get; set; }

        public AxisDisplay()
        {
            OuterColor = Color.DodgerBlue;
            InnerColor = Color.GhostWhite;
            InnerSize = 12;
            Value = new PointF(0f, 0f);

            SetStyle(ControlStyles.UserPaint, true);
            DoubleBuffered = true;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            // Draw with smoothing.
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
            e.Graphics.InterpolationMode = InterpolationMode.High;

            Rectangle rect = e.ClipRectangle;
            rect.Width -= 2;
            rect.Height -= 2;

            // Outer
            e.Graphics.FillEllipse(m_OuterBrush, rect);

            // Inner
            var halfSize = InnerSize / 2;
            var innerRect = new Rectangle(((rect.Width / 2) - halfSize), ((rect.Height / 2) - halfSize), InnerSize, InnerSize);
            innerRect.X += (int)(((rect.Width / 2) - halfSize) * Value.X);
            innerRect.Y -= (int)(((rect.Height / 2) - halfSize) * Value.Y);
            e.Graphics.FillEllipse(m_InnerBrush, innerRect);

            //e.Graphics.FillEllipse(m_InnerBrush, new Rectangle(((rect.Width / 2) - halfSize) * Value.X, ((rect.Height / 2) - halfSize) * Value.Y, InnerSize, InnerSize));
        }
    }
}
