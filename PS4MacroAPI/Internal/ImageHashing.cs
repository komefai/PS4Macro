// PS4MacroAPI (File: Internal/ImageHashing.cs)
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
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;

namespace PS4MacroAPI.Internal
{
    // References
    // https://github.com/jforshee/ImageHashing

    /// <summary>
    /// Contains a variety of methods useful in generating image hashes for image comparison
    /// and recognition.
    /// 
    /// Credit for the AverageHash implementation to David Oftedal of the University of Oslo.
    /// </summary>
    public class ImageHashing
    {
        #region Private constants and utility methods
        /// <summary>
        /// Bitcounts array used for BitCount method (used in Similarity comparisons).
        /// Don't try to read this or understand it, I certainly don't. Credit goes to
        /// David Oftedal of the University of Oslo, Norway for this. 
        /// http://folk.uio.no/davidjo/computing.php
        /// </summary>
        private static byte[] bitCounts = {
            0,1,1,2,1,2,2,3,1,2,2,3,2,3,3,4,1,2,2,3,2,3,3,4,2,3,3,4,3,4,4,5,1,2,2,3,2,3,3,4,
            2,3,3,4,3,4,4,5,2,3,3,4,3,4,4,5,3,4,4,5,4,5,5,6,1,2,2,3,2,3,3,4,2,3,3,4,3,4,4,5,
            2,3,3,4,3,4,4,5,3,4,4,5,4,5,5,6,2,3,3,4,3,4,4,5,3,4,4,5,4,5,5,6,3,4,4,5,4,5,5,6,
            4,5,5,6,5,6,6,7,1,2,2,3,2,3,3,4,2,3,3,4,3,4,4,5,2,3,3,4,3,4,4,5,3,4,4,5,4,5,5,6,
            2,3,3,4,3,4,4,5,3,4,4,5,4,5,5,6,3,4,4,5,4,5,5,6,4,5,5,6,5,6,6,7,2,3,3,4,3,4,4,5,
            3,4,4,5,4,5,5,6,3,4,4,5,4,5,5,6,4,5,5,6,5,6,6,7,3,4,4,5,4,5,5,6,4,5,5,6,5,6,6,7,
            4,5,5,6,5,6,6,7,5,6,6,7,6,7,7,8
        };

        /// <summary>
        /// Counts bits (duh). Utility function for similarity.
        /// I wouldn't try to understand this. I just copy-pasta'd it
        /// from Oftedal's implementation. It works.
        /// </summary>
        /// <param name="num">The hash we are counting.</param>
        /// <returns>The total bit count.</returns>
        private static uint BitCount(ulong num)
        {
            uint count = 0;
            for (; num > 0; num >>= 8)
                count += bitCounts[(num & 0xff)];
            return count;
        }
        #endregion

        #region Public interface methods
        /// <summary>
        /// Computes the average hash of an image according to the algorithm given by Dr. Neal Krawetz
        /// on his blog: http://www.hackerfactor.com/blog/index.php?/archives/432-Looks-Like-It.html.
        /// </summary>
        /// <param name="image">The image to hash.</param>
        /// <returns>The hash of the image.</returns>
        public static ulong AverageHash(Image image)
        {
            // Squeeze the image into an 8x8 canvas
            Bitmap squeezed = new Bitmap(8, 8, PixelFormat.Format32bppRgb);
            Graphics canvas = Graphics.FromImage(squeezed);
            canvas.CompositingQuality = CompositingQuality.HighQuality;
            canvas.InterpolationMode = InterpolationMode.HighQualityBilinear;
            canvas.SmoothingMode = SmoothingMode.HighQuality;
            canvas.DrawImage(image, 0, 0, 8, 8);

            // Reduce colors to 6-bit grayscale and calculate average color value
            byte[] grayscale = new byte[64];
            uint averageValue = 0;
            for (int y = 0; y < 8; y++)
                for (int x = 0; x < 8; x++)
                {
                    uint pixel = (uint)squeezed.GetPixel(x, y).ToArgb();
                    uint gray = (pixel & 0x00ff0000) >> 16;
                    gray += (pixel & 0x0000ff00) >> 8;
                    gray += (pixel & 0x000000ff);
                    gray /= 12;

                    grayscale[x + (y * 8)] = (byte)gray;
                    averageValue += gray;
                }
            averageValue /= 64;

            // Compute the hash: each bit is a pixel
            // 1 = higher than average, 0 = lower than average
            ulong hash = 0;
            for (int i = 0; i < 64; i++)
                if (grayscale[i] >= averageValue)
                    hash |= (1UL << (63 - i));

            return hash;
        }

        /// <summary>
        /// Computes the average hash of the image content in the given file.
        /// </summary>
        /// <param name="path">Path to the input file.</param>
        /// <returns>The hash of the input file's image content.</returns>
        public static ulong AverageHash(String path)
        {
            Bitmap bmp = new Bitmap(path);
            return AverageHash(bmp);
        }

        /// <summary>
        /// Returns a percentage-based similarity value between the two given hashes. The higher
        /// the percentage, the closer the hashes are to being identical.
        /// </summary>
        /// <param name="hash1">The first hash.</param>
        /// <param name="hash2">The second hash.</param>
        /// <returns>The similarity percentage.</returns>
        public static double Similarity(ulong hash1, ulong hash2)
        {
            return ((64 - BitCount(hash1 ^ hash2)) * 100) / 64.0;
        }

        /// <summary>
        /// Returns a percentage-based similarity value between the two given images. The higher
        /// the percentage, the closer the images are to being identical.
        /// </summary>
        /// <param name="image1">The first image.</param>
        /// <param name="image2">The second image.</param>
        /// <returns>The similarity percentage.</returns>
        public static double Similarity(Image image1, Image image2)
        {
            ulong hash1 = AverageHash(image1);
            ulong hash2 = AverageHash(image2);
            return Similarity(hash1, hash2);
        }

        /// <summary>
        /// Returns a percentage-based similarity value between the image content of the two given
        /// files. The higher the percentage, the closer the image contents are to being identical.
        /// </summary>
        /// <param name="path1">The first image file.</param>
        /// <param name="path2">The second image file.</param>
        /// <returns>The similarity percentage.</returns>
        public static double Similarity(String path1, String path2)
        {
            ulong hash1 = AverageHash(path1);
            ulong hash2 = AverageHash(path2);
            return Similarity(hash1, hash2);
        }
        #endregion
    }
}
