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
    public struct Bool32Vector2 : IEquatable<Bool32Vector2>
    {
        public Bool32 X;
        public Bool32 Y;

        public Bool32Vector2(Bool32 x, Bool32 y)
        {
            X = x; Y = y;
        }

        public Bool32Vector2(bool x, bool y)
        {
            X = (Bool32)x; Y = (Bool32)y;
        }

        public bool Equals(Bool32Vector2 other)
        {
            return
                X.IsTrue == other.X.IsTrue &&
                Y.IsTrue == other.Y.IsTrue;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Bool32Vector2))
                return false;
            var other = (Bool32Vector2)obj;
            return Equals(ref this, ref other);
        }

        public override int GetHashCode()
        {
            return X.IntValue + Y.IntValue << 16;
        }

        public override string ToString()
        {
            return
                "{" +
                X.ToString() + ", " +
                Y.ToString() +
                "}";
        }

        public static bool Equals(ref Bool32Vector2 v1, ref Bool32Vector2 v2)
        {
            return
                v1.X == v2.X &&
                v1.Y == v2.Y;
        }
    }
}
