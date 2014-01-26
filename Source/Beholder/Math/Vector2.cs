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
    public struct Vector2 : IEquatable<Vector2>
    {
        public float X;
        public float Y;

        #region Constructors
        public Vector2(float value)
        {
            X = Y = value;
        }

        public Vector2(float x, float y)
        {
            X = x;
            Y = y;
        }
        #endregion

        public float this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: return X;
                    case 1: return Y;
                    default: throw new IndexOutOfRangeException();
                }
            }
        }

        #region Object Overrides

        public bool Equals(Vector2 v)
        {
            return X == v.X && Y == v.Y;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Vector2))
            {
                return false;
            }

            Vector2 other = (Vector2)obj;
            return Equals(ref this, ref other);
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() + Y.GetHashCode() << 16;
        }

        public override string ToString()
        {
            return
                "{" +
                X.ToString(CultureInfo.InvariantCulture) + ", " +
                Y.ToString(CultureInfo.InvariantCulture) +
                "}";
        }

        public static bool Equals(ref Vector2 v1, ref Vector2 v2)
        {
            return
                v1.X == v2.X &&
                v1.Y == v2.Y;
        }

        #endregion

        #region Properties

        public float LengthSquared()
        {
            return X * X + Y * Y;
        }

        public float Length()
        {
            return MathHelper.Sqrt(X * X + Y * Y);
        }

        public Vector2 Normalize()
        {
            float invLength = 1.0f / MathHelper.Sqrt(X * X + Y * Y);
            return new Vector2 { X = X * invLength, Y = Y * invLength };
        }

        #endregion

        #region Static Methods and Operators

        #region Length
        public static float Length(ref Vector2 v)
        {
            return MathHelper.Sqrt(v.X * v.X + v.Y * v.Y);
        }
        public static float Length(Vector2 v)
        {
            return MathHelper.Sqrt(v.X * v.X + v.Y * v.Y);
        }
        #endregion

        #region Distance
        public static float Distance(ref Vector2 left, ref Vector2 right)
        {
            Vector2 difference;
            Subtract(ref left, ref right, out difference);
            return Length(ref difference);
        }
        public static float Distance(Vector2 left, Vector2 right)
        {
            SubtractInPlace(ref left, ref right);
            return Length(ref left);
        }
        #endregion

        #region Length Squared
        public static float LengthSquared(ref Vector2 v)
        {
            return v.X * v.X + v.Y * v.Y;
        }
        public static float LengthSquared(Vector2 v)
        {
            return v.X * v.X + v.Y * v.Y;
        }
        #endregion

        #region Distance Squared
        public static float DistanceSquared(ref Vector2 left, ref Vector2 right)
        {
            Vector2 difference;
            Subtract(ref left, ref right, out difference);
            return LengthSquared(ref difference);
        }
        public static float DistanceSquared(Vector2 left, Vector2 right)
        {
            SubtractInPlace(ref left, ref right);
            return LengthSquared(ref left);
        }
        #endregion

        #region Normalize
        public static void Normalize(ref Vector2 v, out Vector2 result)
        {
            float invLength = 1.0f / MathHelper.Sqrt(v.X * v.X + v.Y * v.Y);
            result.X = v.X * invLength;
            result.Y = v.Y * invLength;
        }
        public static void NormalizeInPlace(ref Vector2 v)
        {
            Normalize(ref v, out v);
        }
        public static Vector2 Normalize(Vector2 v)
        {
            Normalize(ref v, out v);
            return v;
        }
        #endregion

        #region Negate
        public static void Negate(ref Vector2 v, out Vector2 result)
        {
            result.X = -v.X;
            result.Y = -v.Y;
        }
        public static void NegateInPlace(ref Vector2 v)
        {
            Negate(ref v, out v);
        }
        public static Vector2 Negate(Vector2 v)
        {
            Negate(ref v, out v);
            return v;
        }
        public static Vector2 operator -(Vector2 v)
        {
            Negate(ref v, out v);
            return v;
        }
        #endregion

        #region Add
        public static void Add(ref Vector2 left, ref Vector2 right, out Vector2 result)
        {
            result.X = left.X + right.X;
            result.Y = left.Y + right.Y;
        }
        public static void AddInPlace(ref Vector2 leftAndResult, ref Vector2 right)
        {
            Add(ref leftAndResult, ref right, out leftAndResult);
        }
        public static Vector2 Add(Vector2 left, Vector2 right)
        {
            Add(ref left, ref right, out left);
            return left;
        }
        public static Vector2 operator +(Vector2 left, Vector2 right)
        {
            Add(ref left, ref right, out left);
            return left;
        }
        #endregion

        #region Subtract
        public static void Subtract(ref Vector2 left, ref Vector2 right, out Vector2 result)
        {
            result.X = left.X - right.X;
            result.Y = left.Y - right.Y;
        }
        public static void SubtractInPlace(ref Vector2 leftAndResult, ref Vector2 right)
        {
            Subtract(ref leftAndResult, ref right, out leftAndResult);
        }
        public static Vector2 Subtract(Vector2 left, Vector2 right)
        {
            Subtract(ref left, ref right, out left);
            return left;
        }
        public static Vector2 operator -(Vector2 left, Vector2 right)
        {
            Subtract(ref left, ref right, out left);
            return left;
        }
        #endregion

        #region Multiply (scalar)
        public static void Multiply(ref Vector2 v, float scale, out Vector2 result)
        {
            result.X = v.X * scale;
            result.Y = v.Y * scale;
        }
        public static void MultiplyInPlace(ref Vector2 v, float scale)
        {
            Multiply(ref v, scale, out v);
        }
        public static Vector2 Multiply(Vector2 v, float scale)
        {
            Multiply(ref v, scale, out v);
            return v;
        }
        public static Vector2 operator *(float scale, Vector2 v)
        {
            Multiply(ref v, scale, out v);
            return v;
        }
        public static Vector2 operator *(Vector2 v, float scale)
        {
            Multiply(ref v, scale, out v);
            return v;
        }
        #endregion

        #region Dot
        public static float Dot(ref Vector2 left, ref Vector2 right)
        {
            return left.X * right.X + left.Y * right.Y;
        }
        public static float Dot(Vector2 left, Vector2 right)
        {
            return left.X * right.X + left.Y * right.Y;
        }
        public static float operator *(Vector2 left, Vector2 right)
        {
            return left.X * right.X + left.Y * right.Y;
        }
        #endregion

        #region Cross
        public static float Cross(ref Vector2 left, ref Vector2 right)
        {
            return left.X * right.Y - left.Y * right.X;
        }
        public static float Cross(Vector2 left, Vector2 right)
        {
            return left.X * right.Y - left.Y * right.X;
        }
        public static float operator %(Vector2 left, Vector2 right)
        {
            return left.X * right.Y - left.Y * right.X;
        }
        #endregion

        #region Lerp
        public static void Lerp(ref Vector2 left, ref Vector2 right, float amount, out Vector2 result)
        {
            result.X = left.X - amount * (left.X - right.X);
            result.Y = left.Y - amount * (left.Y - right.Y);
        }
        public static void LerpInPlace(ref Vector2 leftAndResult, ref Vector2 right, float amount)
        {
            Lerp(ref leftAndResult, ref right, amount, out leftAndResult);
        }
        public static Vector2 Lerp(Vector2 left, Vector2 right, float amount)
        {
            Lerp(ref left, ref right, amount, out left);
            return left;
        }
        #endregion

        #region NLerp
        public static void NLerp(ref Vector2 left, ref Vector2 right, float amount, out Vector2 result)
        {
            result.X = left.X - amount * (left.X - right.X);
            result.Y = left.Y - amount * (left.Y - right.Y);
            float invLength = 1.0f / MathHelper.Sqrt(result.X * result.X + result.Y * result.Y);
            result.X *= invLength;
            result.Y *= invLength;
        }
        public static void NLerpInPlace(ref Vector2 leftAndResult, ref Vector2 right, float amount)
        {
            NLerp(ref leftAndResult, ref right, amount, out leftAndResult);
        }
        public static Vector2 NLerp(Vector2 left, Vector2 right, float amount)
        {
            NLerp(ref left, ref right, amount, out left);
            return left;
        }
        #endregion

        #endregion

        #region Constant Vectors

        /// <summary>
        /// Returns a new vector with all coordinates equal to zero.
        /// </summary>
        public static Vector2 Zero
        {
            get { return new Vector2(); }
        }

        /// <summary>
        /// Returns a {1, 0} vector.
        /// </summary>
        public static Vector2 UnitX
        {
            get { return new Vector2 { X = 1f }; }
        }

        /// <summary>
        /// Returns a {0, 1} vector.
        /// </summary>
        public static Vector2 UnitY
        {
            get { return new Vector2 { Y = 1f }; }
        }

        #endregion
    }
}
