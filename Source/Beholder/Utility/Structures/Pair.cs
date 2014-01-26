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
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Beholder.Utility.Structures
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct Pair<TFirst, TSecond> : IEquatable<Pair<TFirst, TSecond>>
    {
        public TFirst First;
        public TSecond Second;

        public Pair(TFirst first, TSecond second)
        {
            First = first;
            Second = second;
        }

        public bool Equals(Pair<TFirst, TSecond> other)
        {
            return
                EqualityComparer<TFirst>.Default.Equals(First, other.First) &&
                EqualityComparer<TSecond>.Default.Equals(Second, other.Second);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Pair<TFirst, TSecond>)) 
                return false;
            var other = (Pair<TFirst, TSecond>) obj;
            return Equals(ref this, ref other);
        }

        public override int GetHashCode()
        {
            return
                (EqualityComparer<TFirst>.Default.Equals(First, default(TFirst)) ? 0 : First.GetHashCode()) +
                (EqualityComparer<TSecond>.Default.Equals(Second, default(TSecond)) ? 0 : Second.GetHashCode() << 2);
        }

        public override string ToString()
        {
            return string.Format("[First: {0}; Second: {1}]", First, Second);
        }

        public static bool Equals(ref Pair<TFirst, TSecond> p1, ref Pair<TFirst, TSecond> p2)
        {
            return
                EqualityComparer<TFirst>.Default.Equals(p1.First, p2.First) &&
                EqualityComparer<TSecond>.Default.Equals(p1.Second, p2.Second);
        }
    }
}
