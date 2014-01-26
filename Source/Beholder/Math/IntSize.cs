/*
Copyright (c) 2010-2014 Beholder Project - Daniil Rodin

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

namespace Beholder.Math
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct IntSize : IEquatable<IntSize>
    {
        public int Width;
        public int Height;

        public IntSize(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public bool Equals(IntSize other)
        {
            return Width == other.Width && Height == other.Height;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is IntSize))
                return false;
            var other = (IntSize)obj;
            return Equals(ref this, ref other);
        }

        public override int GetHashCode()
        {
            return Width + Height << 16;
        }

        public override string ToString()
        {
            return
                "{ Width: " +
                Width.ToString() + "; Height: " +
                Height.ToString() +
                "; }";
        }

        public static bool Equals(ref IntSize size1, ref IntSize size2)
        {
            return
                size1.Width == size2.Width &&
                size1.Height == size2.Height;
        }
    }
}
