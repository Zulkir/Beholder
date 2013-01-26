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

namespace Beholder.Math
{
    public static class MathHelper
    {
        public const float Pi = 3.1415926535897932384626433832795f;
        public const float TwoPi = 6.283185307179586476925286766559f;
        public const float PiOver2 = 1.5707963267948966192313216916398f;
        public const float PiOver4 = 0.78539816339744830961566084581988f;

        /// <summary>
        /// Returns a sine of the specified angle
        /// </summary>
        /// <param name="a">Angle</param>
        /// <returns></returns>
        public static float Sin(float a)
        {
            return (float)System.Math.Sin(a);
        }

        /// <summary>
        /// Returns a cosine of the specified angle
        /// </summary>
        /// <param name="a">Angle</param>
        /// <returns></returns>
        public static float Cos(float a)
        {
            return (float)System.Math.Cos(a);
        }

        public static float Tan(float a)
        {
            return (float)System.Math.Tan(a);
        }

        /// <summary>
        /// Returns the angle whose sine is the specified number.
        /// </summary>
        /// <param name="x">A number representing a sine, where -1 ≤ x ≤ 1.</param>
        /// <returns></returns>
        public static float Asin(float x)
        {
            return (float)System.Math.Asin(x);
        }

        /// <summary>
        /// Returns the angle whose cosine is the specified number.
        /// </summary>
        /// <param name="x">A number representing a cosine, where -1 ≤ x ≤ 1.</param>
        public static float Acos(float x)
        {
            return (float)System.Math.Acos(x);
        }

        /// <summary>
        /// Returns a specified number raised to the specified power.
        /// </summary>
        /// <param name="b">Number to be raised to a power.</param>
        /// <param name="e">Number that specifies a power.</param>
        public static float Pow(float b, float e)
        {
            return (float)System.Math.Pow(b, e);
        }

        /// <summary>
        /// Returns a square root of a specified number
        /// </summary>
        public static float Sqrt(float x)
        {
            return (float)System.Math.Sqrt(x);
        }
    }
}
