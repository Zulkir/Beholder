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
    public struct Bool32 : IEquatable<Bool32>
    {
        public int IntValue;
        public bool IsTrue { get { return IntValue != 0; } set { IntValue = value ? 1 : 0; } }

        public Bool32(bool b) { IntValue = b ? 1 : 0; }

        public bool Equals(Bool32 other) { return IntValue == other.IntValue; }
        public override int GetHashCode() { return IntValue; }
        public override string ToString() { return IntValue == 0 ? "False" : "True"; }

        public override bool Equals(object obj)
        {
            if (obj is Bool32)
                return IntValue == obj.GetHashCode();
            if (obj is bool)
                return ((bool)obj) == (IntValue != 0);
            return false;
        }

        public static bool Equals(Bool32 b1, Bool32 b2)
        {
            return
                (b1.IntValue == 0 && b2.IntValue == 0) ||
                (b1.IntValue != 0 && b2.IntValue != 0);
        }

        public static bool operator ==(Bool32 b1, Bool32 b2)
        {
            return
                (b1.IntValue == 0 && b2.IntValue == 0) ||
                (b1.IntValue != 0 && b2.IntValue != 0);
        }

        public static bool operator !=(Bool32 b1, Bool32 b2)
        {
            return
                (b1.IntValue != 0 && b2.IntValue == 0) ||
                (b1.IntValue == 0 && b2.IntValue != 0);
        }


        public static explicit operator bool(Bool32 bool32)
        {
            return bool32.IntValue != 0;
        }

        public static explicit operator Bool32(bool b)
        {
            return new Bool32 { IntValue = b ? 1 : 0 };
        }

        public static explicit operator int(Bool32 bool32)
        {
            return bool32.IntValue;
        }

        public static explicit operator Bool32(int i)
        {
            return new Bool32 { IntValue = i };
        }
    }
}
