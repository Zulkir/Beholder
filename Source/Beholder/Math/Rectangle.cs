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
using System.Globalization;
using System.Runtime.InteropServices;

namespace Beholder.Math
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct Rectangle : IEquatable<Rectangle>
    {
        public float X;
        public float Y;
        public float Width;
        public float Height;

        public Rectangle(float x, float y, float width, float height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public bool ContainsPoint(float x, float y)
        {
            return
                (x >= X) && (x <= X + Width) &&
                (y >= Y) && (y <= Y + Height);
        }

        public bool Equals(Rectangle other)
        {
            return
                X == other.X &&
                Y == other.Y &&
                Width == other.Width &&
                Height == other.Height;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Rectangle))
                return false;
            var other = (Rectangle)obj;
            return Equals(ref this, ref other);
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() + Y.GetHashCode() << 8 + Width.GetHashCode() << 16 + Height.GetHashCode() << 24;
        }

        public override string ToString()
        {
            return
                "{ X: " +
                X.ToString(CultureInfo.InvariantCulture) + "; Y: " +
                Y.ToString(CultureInfo.InvariantCulture) + "; Width: " +
                Width.ToString(CultureInfo.InvariantCulture) + "; Height: " +
                Height.ToString(CultureInfo.InvariantCulture) +
                "; }";
        }

        public static bool Equals(ref Rectangle r1, ref Rectangle r2)
        {
            return
                r1.X == r2.X &&
                r1.Y == r2.Y &&
                r1.Width == r2.Width &&
                r1.Height == r2.Height;
        }
    }
}
