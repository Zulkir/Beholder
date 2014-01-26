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
    public struct Vector3 : IEquatable<Vector3>
    {
        public float X;
        public float Y;
        public float Z;

        #region Constructors
        public Vector3(float value)
        {
            X = Y = Z = value;
        }

        public Vector3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Vector3(Vector2 xy, float z)
        {
            X = xy.X;
            Y = xy.Y;
            Z = z;
        }

        public Vector3(ref Vector2 xy, float z)
        {
            X = xy.X;
            Y = xy.Y;
            Z = z;
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
                    case 2: return Z;
                    default: throw new IndexOutOfRangeException();
                }
            }
        }

        public Vector2 XY
        {
            get { return new Vector2 { X = X, Y = Y }; }
            set { X = value.X; Y = value.Y; }
        }

        #region Object Overrides

        public bool Equals(Vector3 v)
        {
            return X == v.X && Y == v.Y && Z == v.Z;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Vector3))
            {
                return false;
            }

            Vector3 other = (Vector3)obj;
            return Equals(ref this, ref other);
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() + Y.GetHashCode() << 10 + Z.GetHashCode() << 20;
        }

        public override string ToString()
        {
            return
                "{" +
                X.ToString(CultureInfo.InvariantCulture) + ", " +
                Y.ToString(CultureInfo.InvariantCulture) + ", " +
                Z.ToString(CultureInfo.InvariantCulture) +
                "}";
        }

        public static bool Equals(ref Vector3 v1, ref Vector3 v2)
        {
            return
                v1.X == v2.X &&
                v1.Y == v2.Y &&
                v1.Z == v2.Z;
        }

        #endregion

        #region Properties

        public float LengthSquared()
        {
            return X * X + Y * Y + Z * Z;
        }

        public float Length()
        {
            return MathHelper.Sqrt(X * X + Y * Y + Z * Z);
        }

        public Vector3 Normalize()
        {
            float invLength = 1.0f / MathHelper.Sqrt(X * X + Y * Y + Z * Z);
            return new Vector3 { X = X * invLength, Y = Y * invLength, Z = Z * invLength };
        }

        #endregion

        #region Static Methods and Operators

        #region Length
        public static float Length(ref Vector3 v)
        {
            return MathHelper.Sqrt(v.X * v.X + v.Y * v.Y + v.Z * v.Z);
        }
        public static float Length(Vector3 v)
        {
            return MathHelper.Sqrt(v.X * v.X + v.Y * v.Y + v.Z * v.Z);
        }
        #endregion

        #region Distance
        public static float Distance(ref Vector3 left, ref Vector3 right)
        {
            Vector3 difference;
            Subtract(ref left, ref right, out difference);
            return Length(ref difference);
        }
        public static float Distance(Vector3 left, Vector3 right)
        {
            SubtractInPlace(ref left, ref right);
            return Length(ref left);
        }
        #endregion

        #region Length Squared
        public static float LengthSquared(ref Vector3 v)
        {
            return v.X * v.X + v.Y * v.Y + v.Z * v.Z;
        }
        public static float LengthSquared(Vector3 v)
        {
            return v.X * v.X + v.Y * v.Y + v.Z * v.Z;
        }
        #endregion

        #region Distance Squared
        public static float DistanceSquared(ref Vector3 left, ref Vector3 right)
        {
            Vector3 difference;
            Subtract(ref left, ref right, out difference);
            return LengthSquared(ref difference);
        }
        public static float DistanceSquared(Vector3 left, Vector3 right)
        {
            SubtractInPlace(ref left, ref right);
            return LengthSquared(ref left);
        }
        #endregion

        #region Normalize
        public static void Normalize(ref Vector3 v, out Vector3 result)
        {
            float invLength = 1.0f / MathHelper.Sqrt(v.X * v.X + v.Y * v.Y + v.Z * v.Z);
            result.X = v.X * invLength;
            result.Y = v.Y * invLength;
            result.Z = v.Z * invLength;
        }
        public static void NormalizeInPlace(ref Vector3 v)
        {
            Normalize(ref v, out v);
        }
        public static Vector3 Normalize(Vector3 v)
        {
            Normalize(ref v, out v);
            return v;
        }
        #endregion

        #region Negate
        public static void Negate(ref Vector3 v, out Vector3 result)
        {
            result.X = -v.X;
            result.Y = -v.Y;
            result.Z = -v.Z;
        }
        public static void NegateInPlace(ref Vector3 v)
        {
            Negate(ref v, out v);
        }
        public static Vector3 Negate(Vector3 v)
        {
            Negate(ref v, out v);
            return v;
        }
        public static Vector3 operator -(Vector3 v)
        {
            Negate(ref v, out v);
            return v;
        }
        #endregion

        #region Add
        public static void Add(ref Vector3 left, ref Vector3 right, out Vector3 result)
        {
            result.X = left.X + right.X;
            result.Y = left.Y + right.Y;
            result.Z = left.Z + right.Z;
        }
        public static void AddInPlace(ref Vector3 leftAndResult, ref Vector3 right)
        {
            Add(ref leftAndResult, ref right, out leftAndResult);
        }
        public static Vector3 Add(Vector3 left, Vector3 right)
        {
            Add(ref left, ref right, out left);
            return left;
        }
        public static Vector3 operator +(Vector3 left, Vector3 right)
        {
            Add(ref left, ref right, out left);
            return left;
        }
        #endregion

        #region Subtract
        public static void Subtract(ref Vector3 left, ref Vector3 right, out Vector3 result)
        {
            result.X = left.X - right.X;
            result.Y = left.Y - right.Y;
            result.Z = left.Z - right.Z;
        }
        public static void SubtractInPlace(ref Vector3 leftAndResult, ref Vector3 right)
        {
            Subtract(ref leftAndResult, ref right, out leftAndResult);
        }
        public static Vector3 Subtract(Vector3 left, Vector3 right)
        {
            Subtract(ref left, ref right, out left);
            return left;
        }
        public static Vector3 operator -(Vector3 left, Vector3 right)
        {
            Subtract(ref left, ref right, out left);
            return left;
        }
        #endregion

        #region Multiply (scalar)
        public static void Multiply(ref Vector3 v, float scale, out Vector3 result)
        {
            result.X = v.X * scale;
            result.Y = v.Y * scale;
            result.Z = v.Z * scale;
        }
        public static void MultiplyInPlace(ref Vector3 v, float scale)
        {
            Multiply(ref v, scale, out v);
        }
        public static Vector3 Multiply(Vector3 v, float scale)
        {
            Multiply(ref v, scale, out v);
            return v;
        }
        public static Vector3 operator *(float scale, Vector3 v)
        {
            Multiply(ref v, scale, out v);
            return v;
        }
        public static Vector3 operator *(Vector3 v, float scale)
        {
            Multiply(ref v, scale, out v);
            return v;
        }
        #endregion

        #region Dot
        public static float Dot(ref Vector3 left, ref Vector3 right)
        {
            return left.X * right.X + left.Y * right.Y + left.Z * right.Z;
        }
        public static float Dot(Vector3 left, Vector3 right)
        {
            return left.X * right.X + left.Y * right.Y + left.Z * right.Z;
        }/*
        public static float operator *(Vector3 left, Vector3 right)
        {
            return left.X * right.X + left.Y * right.Y + left.Z * right.Z;
        }*/
        #endregion

        #region Cross
        public static void Cross(ref Vector3 left, ref Vector3 right, out Vector3 result)
        {
            result.X = left.Y * right.Z - left.Z * right.Y;
            result.Y = left.Z * right.X - left.X * right.Z;
            result.Z = left.X * right.Y - left.Y * right.X;
        }
        public static void CrossInPlace(ref Vector3 leftAndResult, ref Vector3 right)
        {
            Vector3 result;
            Cross(ref leftAndResult, ref right, out result);
            leftAndResult = result;
        }
        public static Vector3 Cross(Vector3 left, Vector3 right)
        {
            Vector3 result;
            Cross(ref left, ref right, out result);
            return result;
        }/*
        public static Vector3 operator %(Vector3 left, Vector3 right)
        {
            Vector3 result;
            Cross(ref left, ref right, out result);
            return result;
        }*/
        #endregion

        #region Lerp
        public static void Lerp(ref Vector3 left, ref Vector3 right, float amount, out Vector3 result)
        {
            result.X = left.X - amount * (left.X - right.X);
            result.Y = left.Y - amount * (left.Y - right.Y);
            result.Z = left.Z - amount * (left.Z - right.Z);
        }
        public static void LerpInPlace(ref Vector3 leftAndResult, ref Vector3 right, float amount)
        {
            Lerp(ref leftAndResult, ref right, amount, out leftAndResult);
        }
        public static Vector3 Lerp(Vector3 left, Vector3 right, float amount)
        {
            Lerp(ref left, ref right, amount, out left);
            return left;
        }
        #endregion

        #region NLerp
        public static void NLerp(ref Vector3 left, ref Vector3 right, float amount, out Vector3 result)
        {
            result.X = left.X - amount * (left.X - right.X);
            result.Y = left.Y - amount * (left.Y - right.Y);
            result.Z = left.Z - amount * (left.Z - right.Z);
            float invLength = 1.0f / MathHelper.Sqrt(result.X * result.X + result.Y * result.Y + result.Z * result.Z);
            result.X *= invLength;
            result.Y *= invLength;
            result.Z *= invLength;
        }
        public static void NLerpInPlace(ref Vector3 leftAndResult, ref Vector3 right, float amount)
        {
            NLerp(ref leftAndResult, ref right, amount, out leftAndResult);
        }
        public static Vector3 NLerp(Vector3 left, Vector3 right, float amount)
        {
            NLerp(ref left, ref right, amount, out left);
            return left;
        }
        #endregion

        #endregion

        #region Constant Vectors

        /// <summary>
        /// Returns a new vector with all coordinates equal to zero
        /// </summary>
        public static Vector3 Zero
        {
            get { return new Vector3(); }
        }

        /// <summary>
        /// Returns a {1, 0, 0} vector
        /// </summary>
        public static Vector3 UnitX
        {
            get { return new Vector3 { X = 1f }; }
        }

        /// <summary>
        /// Returns a {0, 1, 0} vector
        /// </summary>
        public static Vector3 UnitY
        {
            get { return new Vector3 { Y = 1f }; }
        }

        /// <summary>
        /// Returns a {0, 0, 1} vector
        /// </summary>
        public static Vector3 UnitZ
        {
            get { return new Vector3 { Z = 1f }; }
        }

        #endregion
    }
}
