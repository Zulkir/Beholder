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
    public struct ThreeTuple<TFirst, TSecond, TThird> : IEquatable<ThreeTuple<TFirst, TSecond, TThird>>
    {
        public TFirst First;
        public TSecond Second;
        public TThird Third;

        public ThreeTuple(TFirst first, TSecond second, TThird third)
        {
            First = first;
            Second = second;
            Third = third;
        }

        public bool Equals(ThreeTuple<TFirst, TSecond, TThird> other)
        {
            return
                EqualityComparer<TFirst>.Default.Equals(First, other.First) &&
                EqualityComparer<TSecond>.Default.Equals(Second, other.Second) && 
                EqualityComparer<TThird>.Default.Equals(Third, other.Third);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is ThreeTuple<TFirst, TSecond, TThird>)) 
                return false;
            var other = (ThreeTuple<TFirst, TSecond, TThird>)obj;
            return Equals(ref this, ref other);
        }

        public override int GetHashCode()
        {
            return
                (EqualityComparer<TFirst>.Default.Equals(First, default(TFirst)) ? 0 : First.GetHashCode()) +
                (EqualityComparer<TSecond>.Default.Equals(Second, default(TSecond)) ? 0 : Second.GetHashCode() << 2) +
                (EqualityComparer<TThird>.Default.Equals(Third, default(TThird)) ? 0 : Third.GetHashCode() << 4);
        }

        public override string ToString()
        {
            return string.Format("[First: {0}; Second: {1}; Third: {2}]", First, Second, Third);
        }

        public static bool Equals(ref ThreeTuple<TFirst, TSecond, TThird> t1, ref ThreeTuple<TFirst, TSecond, TThird> t2)
        {
            return
                EqualityComparer<TFirst>.Default.Equals(t1.First, t2.First) &&
                EqualityComparer<TSecond>.Default.Equals(t1.Second, t2.Second) &&
                EqualityComparer<TThird>.Default.Equals(t1.Third, t2.Third);
        }
    }
}
