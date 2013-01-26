/*
Copyright (c) 2010-2013 Beholder Project - Daniil Rodin

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/

using System;
using System.Runtime.InteropServices;
using Beholder.Math;

namespace Beholder.Platform
{
    /// <summary>
    /// Describes a display mode.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct DisplayMode : IEquatable<DisplayMode>
    {
        /// <summary>
        /// Width of the display.
        /// </summary>
        public int Width;

        /// <summary>
        /// Height of the display.
        /// </summary>
        public int Height;

        /// <summary>
        /// Refresh rate of the display.
        /// </summary>
        public Rational RefreshRate;

        /// <summary>
        /// Format ID of the display.
        /// </summary>
        public int FormatID;

        /// <summary>
        /// Creates a new <see cref="DisplayMode"/> structure.
        /// </summary>
        /// <param name="width">Width of the display.</param>
        /// <param name="height">Height of the display.</param>
        /// <param name="refreshRate">Refresh rate of the display.</param>
        /// <param name="formatID">Format ID of the display.</param>
        public DisplayMode(
            int width,
            int height,
            Rational refreshRate,
            int formatID
            )
        {
            Width = width;
            Height = height;
            RefreshRate = refreshRate;
            FormatID = formatID;
        }

        public static bool Equals(ref DisplayMode dm1, ref DisplayMode dm2)
        {
            return
                dm1.Width == dm2.Width &&
                dm1.Height == dm2.Height &&
                dm1.RefreshRate == dm2.RefreshRate &&
                dm1.FormatID == dm2.FormatID;
        }

        public bool Equals(DisplayMode other)
        {
            return Equals(ref this, ref other);
        }

        public override bool Equals(object obj)
        {
            return obj != null && obj is DisplayMode && Equals((DisplayMode)obj);
        }

        public override int GetHashCode()
        {
            return Width + (Height << 6) + (RefreshRate.GetHashCode() << 12) + (FormatID << 20);
        }

        public override string ToString()
        {
            return string.Format("{{Width: {0}; Height: {1}; FormatID: {2}; RefreshRate: {3}}}", Width, Height, FormatID, RefreshRate);
        }
    }
}
