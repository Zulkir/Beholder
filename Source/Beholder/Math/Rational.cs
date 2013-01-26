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
    public struct Rational : IEquatable<Rational>
    {
        public int Numerator;
        public int Denominator;

        public Rational(int numerator, int denominator)
        {
            Numerator = numerator;
            Denominator = denominator;
        }

        public float ToSingle() 
        {
            if (Denominator == 0) 
                throw new DivideByZeroException();
            return (float)((double)Numerator / Denominator);
        }

        public double ToDouble()
        {
            if (Denominator == 0) 
                throw new DivideByZeroException();
            return (double)Numerator / Denominator;
        }

        public int Round()
        {
            if (Denominator == 0) 
                throw new DivideByZeroException();
            return (int)System.Math.Round((double)Numerator / Denominator);
        }

        public bool IsWhole { get { return Numerator % Denominator == 0; } }

        public bool Equals(Rational other)
        {
            if (other.Numerator > Numerator)
                return (other.Numerator % Numerator == 0) && (Denominator * (other.Numerator / Numerator) == other.Denominator);
            if (other.Numerator == Numerator)
                return other.Denominator == Denominator;
            return (Numerator % other.Numerator == 0) && (other.Denominator * (Numerator / other.Numerator) == Denominator);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Rational)) 
                return false;
            var other = (Rational)obj;
            return Equals(ref this, ref other);
        }

        public static bool Equals(ref Rational r1, ref Rational r2)
        {
            if (r1.Numerator > r2.Numerator)
                return (r1.Numerator % r2.Numerator == 0) && (r2.Denominator * (r1.Numerator / r2.Numerator) == r1.Denominator);
            if (r1.Numerator == r2.Numerator)
                return r1.Denominator == r2.Denominator;
            return (r2.Numerator % r1.Numerator == 0) && (r1.Denominator * (r2.Numerator / r1.Numerator) == r2.Denominator);
        }

        public override int GetHashCode()
        {
            return ToSingle().GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("{0}/{1}", Numerator.ToString(), Denominator.ToString());
        }

        public static implicit operator Rational(int n)
        {
            return new Rational(n, 1);
        }

        public static bool operator ==(Rational r1, Rational r2)
        {
            return r1.ToDouble() == r2.ToDouble();
        }

        public static bool operator !=(Rational r1, Rational r2)
        {
            return r1.ToDouble() != r2.ToDouble();
        }
    }
}
