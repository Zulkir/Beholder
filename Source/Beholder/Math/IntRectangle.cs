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

namespace Beholder.Math
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct IntRectangle : IEquatable<IntRectangle>
    {
        public int X;
        public int Y;
        public int Width;
        public int Height;

        public IntRectangle(int x, int y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public bool ContainsPoint(int x, int y)
        {
            return
                (x >= X) && (x <= X + Width) &&
                (y >= Y) && (y <= Y + Height);
        }

        public bool Equals(IntRectangle other)
        {
            return
                X == other.X &&
                Y == other.Y &&
                Width == other.Width &&
                Height == other.Height;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is IntRectangle))
            {
                return false;
            }

            IntRectangle other = (IntRectangle)obj;
            return Equals(ref this, ref other);
        }

        public override int GetHashCode()
        {
            return X + Y << 8 + Width << 16 + Height << 24;
        }

        public override string ToString()
        {
            return
                "{ X: " +
                X.ToString() + "; Y: " +
                Y.ToString() + "; Width: " +
                Width.ToString() + "; Height: " +
                Height.ToString() +
                "; }";
        }

        public static bool Equals(IntRectangle r1, IntRectangle r2)
        {
            return
                r1.X == r2.X &&
                r1.Y == r2.Y &&
                r1.Width == r2.Width &&
                r1.Height == r2.Height;
        }

        public static bool Equals(ref IntRectangle r1, ref IntRectangle r2)
        {
            return
                r1.X == r2.X &&
                r1.Y == r2.Y &&
                r1.Width == r2.Width &&
                r1.Height == r2.Height;
        }
    }
}
