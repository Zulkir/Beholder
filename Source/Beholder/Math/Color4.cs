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
    public struct Color4 : IEquatable<Color4>
    {
        public Vector4 Data;

        #region Constructors
        /// <summary>
        /// Creates a new <see cref="Color4"/> structure with alpha = 1.0f.
        /// </summary>
        /// <param name="red">Red component of a color (float in the range from 0.0f to 1.0f).</param>
        /// <param name="green">Green component of a color (float in the range from 0.0f to 1.0f).</param>
        /// <param name="blue">Blue component of a color (float in the range from 0.0f to 1.0f).</param>
        public Color4(float red, float green, float blue)
        {
            Data.X = red;
            Data.Y = green;
            Data.Z = blue;
            Data.W = 1.0f;
        }

        /// <summary>
        /// Creates a new instance of a <see cref="Color4"/>.
        /// </summary>
        /// <param name="red">Red component of a color (float in the range from 0.0f to 1.0f).</param>
        /// <param name="green">Green component of a color (float in the range from 0.0f to 1.0f).</param>
        /// <param name="blue">Blue component of a color (float in the range from 0.0f to 1.0f).</param>
        /// <param name="alpha">Alpha component of a color (float in the range from 0.0f to 1.0f).</param>
        public Color4(float red, float green, float blue, float alpha)
        {
            Data.X = red;
            Data.Y = green;
            Data.Z = blue;
            Data.W = alpha;
        }


        /// <summary>
        /// Creates a new instance of a <see cref="Color4"/> with alpha = 255 (1.0f).
        /// </summary>
        /// <param name="red">Red component of a color (integer in the range from 0 to 255).</param>
        /// <param name="green">Green component of a color (integer in the range from 0 to 255).</param>
        /// <param name="blue">Blue component of a color (integer in the range from 0 to 255).</param>
        public Color4(int red, int green, int blue)
        {
            Data.X = red / 255.0f;
            Data.Y = green / 255.0f;
            Data.Z = blue / 255.0f;
            Data.W = 1.0f;
        }

        /// <summary>
        /// Creates a new instance of a <see cref="Color4"/>.
        /// </summary>
        /// <param name="red">Red component of a color (integer in the range from 0 to 255).</param>
        /// <param name="green">Green component of a color (integer in the range from 0 to 255).</param>
        /// <param name="blue">Blue component of a color (integer in the range from 0 to 255).</param>
        /// <param name="alpha">Alpha component of a color (integer in the range from 0 to 255).</param>
        public Color4(int red, int green, int blue, int alpha)
        {
            Data.X = red / 255.0f;
            Data.Y = green / 255.0f;
            Data.Z = blue / 255.0f;
            Data.W = alpha / 255.0f;
        }

        public Color4(Color3 color3, float alpha)
        {
            Data.X = color3.Data.X;
            Data.Y = color3.Data.Y;
            Data.Z = color3.Data.Z;
            Data.W = alpha;
        }

        public Color4(ref Color3 color3, float alpha)
        {
            Data.X = color3.Data.X;
            Data.Y = color3.Data.Y;
            Data.Z = color3.Data.Z;
            Data.W = alpha;
        }

        public Color4(Color3 color3, int alpha)
        {
            Data.X = color3.Data.X;
            Data.Y = color3.Data.Y;
            Data.Z = color3.Data.Z;
            Data.W = alpha / 255.0f;
        }

        public Color4(ref Color3 color3, int alpha)
        {
            Data.X = color3.Data.X;
            Data.Y = color3.Data.Y;
            Data.Z = color3.Data.Z;
            Data.W = alpha / 255.0f;
        }
        #endregion

        public float R { get { return Data.X; } set { Data.X = value; } }
        public float G { get { return Data.Y; } set { Data.Y = value; } }
        public float B { get { return Data.Z; } set { Data.Z = value; } }
        public float A { get { return Data.W; } set { Data.W = value; } }

        public float this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: return Data.X;
                    case 1: return Data.Y;
                    case 2: return Data.Z;
                    case 3: return Data.W;
                    default: throw new IndexOutOfRangeException();
                }
            }
        }

        public Color3 RGB
        {
            get { return new Color3 { Data = new Vector3 { X = Data.X, Y = Data.Y, Z = Data.Z } }; }
            set { Data.X = value.Data.X; Data.Y = value.Data.Y; Data.Z = value.Data.Z; }
        }

        #region Object Overrides

        public bool Equals(Color4 other)
        {
            return
                Data.X == other.Data.X &&
                Data.Y == other.Data.Y &&
                Data.Z == other.Data.Z &&
                Data.W == other.Data.W;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Color4))
                return false;
            var other = (Color4)obj;
            return Equals(ref this, ref other);
        }

        public override int GetHashCode()
        {
            return Data.X.GetHashCode() + (Data.Y.GetHashCode() << 2) + (Data.Z.GetHashCode() << 4) + (Data.W.GetHashCode() << 6);
        }

        public override string ToString()
        {
            return
                "{" +
                Data.X.ToString(CultureInfo.InvariantCulture) + ", " +
                Data.Y.ToString(CultureInfo.InvariantCulture) + ", " +
                Data.Z.ToString(CultureInfo.InvariantCulture) + ", " +
                Data.W.ToString(CultureInfo.InvariantCulture) +
                "}";
        }

        public static bool Equals(ref Color4 c1, ref Color4 c2)
        {
            return
                c1.Data.X == c2.Data.X &&
                c1.Data.Y == c2.Data.Y &&
                c1.Data.Z == c2.Data.Z &&
                c1.Data.W == c2.Data.W;
        }

        #endregion

        #region Convertions
        public int ToArgb()
        {
            return ((int)(Data.W * 255.99f) << 24) + ((int)(Data.X * 255.99f) << 16) + ((int)(Data.Y * 255.99f) << 8) + (int)(Data.Z * 255.99f);
        }
        #endregion

        #region Static Methods and Operators

        #region Multiply (scalar)
        public static void Multiply(ref Color4 c, float scale, out Color4 result)
        {
            result.Data.X = c.Data.X * scale;
            result.Data.Y = c.Data.Y * scale;
            result.Data.Z = c.Data.Z * scale;
            result.Data.W = c.Data.W * scale;
        }
        public static void MultiplyInPlace(ref Color4 c, float scale)
        {
            Multiply(ref c, scale, out c);
        }
        public static Color4 Multiply(Color4 c, float scale)
        {
            Multiply(ref c, scale, out c);
            return c;
        }
        public static Color4 operator *(float scale, Color4 c)
        {
            Multiply(ref c, scale, out c);
            return c;
        }
        public static Color4 operator *(Color4 c, float scale)
        {
            Multiply(ref c, scale, out c);
            return c;
        }
        #endregion

        #region Lerp
        public static void Lerp(ref Color4 left, ref Color4 right, float amount, out Color4 result)
        {
            result.Data.X = left.Data.X - amount * (left.Data.X - right.Data.X);
            result.Data.Y = left.Data.Y - amount * (left.Data.Y - right.Data.Y);
            result.Data.Z = left.Data.Z - amount * (left.Data.Z - right.Data.Z);
            result.Data.W = left.Data.W - amount * (left.Data.W - right.Data.W);
        }
        public static void LerpInPlace(ref Color4 leftAndResult, ref Color4 right, float amount)
        {
            Lerp(ref leftAndResult, ref right, amount, out leftAndResult);
        }
        public static Color4 Lerp(Color4 left, Color4 right, float amount)
        {
            Lerp(ref left, ref right, amount, out left);
            return left;
        }
        #endregion

        #endregion

        #region Static Convertions
        public static int ToArgb(ref Color4 color)
        {
            return ((int)(color.Data.W * 255.99f) << 24) + ((int)(color.Data.X * 255.99f) << 16) + ((int)(color.Data.Y * 255.99f) << 8) + (int)(color.Data.Z * 255.99f);
        }
        public static explicit operator Vector4(Color4 c)
        {
            return c.Data;
        }
        public static explicit operator Color4(Vector4 v)
        {
            return new Color4 { Data = v };
        }
        #endregion

        #region Constant Colors

        public static Color4 Black { get { return new Color4(0, 0, 0); } }
        public static Color4 White { get { return new Color4(255, 255, 255); } }
        public static Color4 Red { get { return new Color4(255, 0, 0); } }
        public static Color4 Green { get { return new Color4(0, 255, 0); } }
        public static Color4 Blue { get { return new Color4(0, 0, 255); } }
        public static Color4 Yellow { get { return new Color4(255, 255, 0); } }
        public static Color4 Magneta { get { return new Color4(255, 0, 255); } }
        public static Color4 Cyan { get { return new Color4(0, 255, 255); } }

        public static Color4 CornflowerBlue { get { return new Color4(100, 149, 237); } }
        public static Color4 Violet { get { return new Color4(143, 0, 255); } }

        #endregion
    }
}
