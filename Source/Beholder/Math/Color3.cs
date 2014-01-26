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
    public struct Color3 : IEquatable<Color3>
    {
        public Vector3 Data;

        #region Constructors
        /// <summary>
        /// Creates a new instance of a <see cref="Color3"/>
        /// </summary>
        /// <param name="red">Red component of a color (float in the range from 0.0f to 1.0f)</param>
        /// <param name="green">Green component of a color (float in the range from 0.0f to 1.0f)</param>
        /// <param name="blue">Blue component of a color (float in the range from 0.0f to 1.0f)</param>
        public Color3(float red, float green, float blue)
        {
            Data.X = red;
            Data.Y = green;
            Data.Z = blue;
        }

        /// <summary>
        /// Creates a new instance of a <see cref="Color3"/> class.
        /// </summary>
        /// <param name="red">Red component of a color (integer in the range from 0 to 255)</param>
        /// <param name="green">Green component of a color (integer in the range from 0 to 255)</param>
        /// <param name="blue">Blue component of a color (integer in the range from 0 to 255)</param>
        public Color3(int red, int green, int blue)
        {
            Data.X = red / 255.0f;
            Data.Y = green / 255.0f;
            Data.Z = blue / 255.0f;
        }
        #endregion

        public float R { get { return Data.X; } }
        public float G { get { return Data.Y; } }
        public float B { get { return Data.Z; } }

        public float this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: return Data.X;
                    case 1: return Data.Y;
                    case 2: return Data.Z;
                    default: throw new IndexOutOfRangeException();
                }
            }
        }

        #region Object Overrides

        public bool Equals(Color3 other)
        {
            return
                Data.X == other.Data.X &&
                Data.Y == other.Data.Z &&
                Data.Z == other.Data.Z;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Color3))
            {
                return false;
            }
            var other = (Color3)obj;
            return Equals(ref this, ref other);
        }

        public override int GetHashCode()
        {
            return Data.X.GetHashCode() + (Data.Y.GetHashCode() << 2) + (Data.Z.GetHashCode() << 4);
        }

        public override string ToString()
        {
            return
                "{" +
                Data.X.ToString(CultureInfo.InvariantCulture) + ", " +
                Data.Y.ToString(CultureInfo.InvariantCulture) + ", " +
                Data.Z.ToString(CultureInfo.InvariantCulture) +
                "}";
        }

        public static bool Equals(ref Color3 c1, ref Color3 c2)
        {
            return
                c1.Data.X == c2.Data.X &&
                c1.Data.Y == c2.Data.Y &&
                c1.Data.Z == c2.Data.Z;
        }

        #endregion

        #region Convertions
        public int ToArgb()
        {
            return (0xff << 24) | ((int)(Data.X * 255.99f) << 16) | ((int)(Data.Y * 255.99f) << 8) | (int)(Data.Z * 255.99f);
        }

        public Color4 ToColor4()
        {
            return new Color4 { Data = new Vector4 { X = Data.X, Y = Data.Y, Z = Data.Z, W = 1f } };
        }
        #endregion

        #region Static Methods and Operators

        #region Multiply (scalar)
        public static void Multiply(ref Color3 c, float scale, out Color3 result)
        {
            result.Data.X = c.Data.X * scale;
            result.Data.Y = c.Data.Y * scale;
            result.Data.Z = c.Data.Z * scale;
        }
        public static void MultiplyInPlace(ref Color3 c, float scale)
        {
            Multiply(ref c, scale, out c);
        }
        public static Color3 Multiply(Color3 c, float scale)
        {
            Multiply(ref c, scale, out c);
            return c;
        }
        public static Color3 operator *(float scale, Color3 c)
        {
            Multiply(ref c, scale, out c);
            return c;
        }
        public static Color3 operator *(Color3 c, float scale)
        {
            Multiply(ref c, scale, out c);
            return c;
        }
        #endregion


        #region Lerp
        public static void Lerp(ref Color3 left, ref Color3 right, float amount, out Color3 result)
        {
            result.Data.X = left.Data.X - amount * (left.Data.X - right.Data.X);
            result.Data.Y = left.Data.Y - amount * (left.Data.Y - right.Data.Y);
            result.Data.Z = left.Data.Z - amount * (left.Data.Z - right.Data.Z);
        }
        public static void LerpInPlace(ref Color3 leftAndResult, ref Color3 right, float amount)
        {
            Lerp(ref leftAndResult, ref right, amount, out leftAndResult);
        }
        public static Color3 Lerp(Color3 left, Color3 right, float amount)
        {
            Lerp(ref left, ref right, amount, out left);
            return left;
        }
        #endregion

        #endregion

        #region Static Convertions
        public static int ToArgb(ref Color3 color)
        {
            return (0xff << 24) | ((int)(color.Data.X * 255.99f) << 16) | ((int)(color.Data.Y * 255.99f) << 8) | (int)(color.Data.X * 255.99f);
        }
        public static explicit operator Vector3(Color3 c)
        {
            return c.Data;
        }
        public static explicit operator Color3(Vector3 v)
        {
            return new Color3 { Data = v };
        }

        public static void ToColor4(ref Color3 c, out Color4 result)
        {
            result.Data = new Vector4 { X = c.Data.X, Y = c.Data.Y, Z = c.Data.Z, W = 1.0f };
        }
        
        #endregion

        #region Constant Colors

        public static Color3 Black { get { return new Color3(0, 0, 0); } }
        public static Color3 White { get { return new Color3(255, 255, 255); } }
        public static Color3 Red { get { return new Color3(255, 0, 0); } }
        public static Color3 Green { get { return new Color3(0, 255, 0); } }
        public static Color3 Blue { get { return new Color3(0, 0, 255); } }
        public static Color3 Yellow { get { return new Color3(255, 255, 0); } }
        public static Color3 Magneta { get { return new Color3(255, 0, 255); } }
        public static Color3 Cyan { get { return new Color3(0, 255, 255); } }

        public static Color3 CornflowerBlue { get { return new Color3(100, 149, 237); } }
        public static Color3 Violet { get { return new Color3(143, 0, 255); } }

        #endregion
    }
}
