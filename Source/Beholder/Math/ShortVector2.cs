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
    public struct ShortVector2 : IEquatable<ShortVector2>
    {
        public short X;
        public short Y;

        public ShortVector2(short x, short y)
        {
            X = x;
            Y = y;
        }

        public bool Equals(ShortVector2 other)
        {
            return
                X == other.X &&
                Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is ShortVector2))
                return false;
            var other = (ShortVector2)obj;
            return Equals(ref this, ref other);
        }

        public override int GetHashCode()
        {
            return X + Y << 16;
        }

        public override string ToString()
        {
            return
                "{" +
                X.ToString() + ", " +
                Y.ToString() +
                "}";
        }

        public static bool Equals(ref ShortVector2 v1, ref ShortVector2 v2)
        {
            return
                v1.X == v2.X &&
                v1.Y == v2.Y;
        }
    }
}
