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
    public struct Quadruple<TFirst, TSecond, TThird, TFourth> :  
        IEquatable<Quadruple<TFirst, TSecond, TThird, TFourth>>
    {
        public TFirst First;
        public TSecond Second;
        public TThird Third;
        public TFourth Fourth;

        public Quadruple(TFirst first, TSecond second, TThird third, TFourth fourth)
        {
            First = first;
            Second = second;
            Third = third;
            Fourth = fourth;
        }

        public bool Equals(Quadruple<TFirst, TSecond, TThird, TFourth> other)
        {
            return
                EqualityComparer<TFirst>.Default.Equals(First, other.First) &&
                EqualityComparer<TSecond>.Default.Equals(Second, other.Second) && 
                EqualityComparer<TThird>.Default.Equals(Third, other.Third) &&
                EqualityComparer<TFourth>.Default.Equals(Fourth, other.Fourth);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Quadruple<TFirst, TSecond, TThird, TFourth>)) 
                return false;
            var other = (Quadruple<TFirst, TSecond, TThird, TFourth>)obj;
            return Equals(ref this, ref other);
        }

        public override int GetHashCode()
        {
            return
                (EqualityComparer<TFirst>.Default.Equals(First, default(TFirst)) ? 0 : First.GetHashCode()) +
                (EqualityComparer<TSecond>.Default.Equals(Second, default(TSecond)) ? 0 : Second.GetHashCode() << 2) +
                (EqualityComparer<TThird>.Default.Equals(Third, default(TThird)) ? 0 : Third.GetHashCode() << 4) +
                (EqualityComparer<TFourth>.Default.Equals(Fourth, default(TFourth)) ? 0 : Fourth.GetHashCode() << 6);
        }

        public override string ToString()
        {
            return string.Format("[First: {0}; Second: {1}; Third: {2}; Fourth: {3}]", First, Second, Third, Fourth);
        }

        public static bool Equals(ref Quadruple<TFirst, TSecond, TThird, TFourth> q1, ref Quadruple<TFirst, TSecond, TThird, TFourth> q2)
        {
            return
                EqualityComparer<TFirst>.Default.Equals(q1.First, q2.First) &&
                EqualityComparer<TSecond>.Default.Equals(q1.Second, q2.Second) &&
                EqualityComparer<TThird>.Default.Equals(q1.Third, q2.Third) &&
                EqualityComparer<TFourth>.Default.Equals(q1.Fourth, q2.Fourth);
        }
    }
}
