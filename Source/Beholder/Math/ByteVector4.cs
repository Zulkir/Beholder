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
    public struct ByteVector4 : IEquatable<ByteVector4>
    {
        public byte X;
        public byte Y;
        public byte Z;
        public byte W;

        public ByteVector4(byte x, byte y, byte z, byte w)
        {
            X = x; 
            Y = y;
            Z = z;
            W = w;
        }

        public int ToInt() { return X | (Y << 8) | (Z << 16) | (W << 24); }

        public bool Equals(ByteVector4 other)
        {
            return ToInt() == other.ToInt();
        }

        public override bool Equals(object obj)
        {
            return obj is ByteVector4 && obj.GetHashCode() == ToInt();
        }

        public override int GetHashCode() { return ToInt(); }

        public override string ToString()
        {
            return
                "{" +
                X.ToString() + ", " +
                Y.ToString() + ", " +
                Z.ToString() + ", " +
                W.ToString() +
                "}";
        }

        public static bool Equals(ByteVector4 v1, ByteVector4 v2)
        {
            return
                v1.X == v2.X &&
                v1.Y == v2.Y &&
                v1.Z == v2.Z &&
                v1.W == v2.W;
        }
    }
}
