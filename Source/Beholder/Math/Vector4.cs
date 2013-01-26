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
using System.Globalization;

namespace Beholder.Math
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct Vector4 : IEquatable<Vector4>
    {
        public float X;
        public float Y;
        public float Z;
        public float W;

        #region Constructors
        public Vector4(float x, float y, float z, float w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        public Vector4(Vector3 xyz, float w)
        {
            X = xyz.X;
            Y = xyz.Y;
            Z = xyz.Z;
            W = w;
        }

        public Vector4(ref Vector3 xyz, float w)
        {
            X = xyz.X;
            Y = xyz.Y;
            Z = xyz.Z;
            W = w;
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
                    case 3: return W;
                    default: throw new IndexOutOfRangeException();
                }
            }
        }

        public Vector3 XYZ
        {
            get { return new Vector3 { X = X, Y = Y, Z = Z }; }
            set { X = value.X; Y = value.Y; Z = value.Z; }
        }

        #region Object Overrides

        public bool Equals(Vector4 v)
        {
            return X == v.X && Y == v.Y && Z == v.Z && W == v.W;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Vector4))
                return false;
            var other = (Vector4) obj;
            return Equals(ref this, ref other);
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() + Y.GetHashCode() << 8 + Z.GetHashCode() << 16 + W.GetHashCode() << 24;
        }

        public override string ToString()
        {
            return
                "{" +
                X.ToString(CultureInfo.InvariantCulture) + ", " +
                Y.ToString(CultureInfo.InvariantCulture) + ", " +
                Z.ToString(CultureInfo.InvariantCulture) + ", " +
                W.ToString(CultureInfo.InvariantCulture) +
                "}";
        }

        public static bool Equals(ref Vector4 v1, ref Vector4 v2)
        {
            return
                v1.X == v2.X &&
                v1.Y == v2.Y &&
                v1.Z == v2.Z &&
                v1.W == v2.W;
        }

        #endregion

        #region Properties

        public float Length()
        {
            return MathHelper.Sqrt(X * X + Y * Y + Z * Z + W * W);
        }

        public float LengthSquared()
        {
            return X * X + Y * Y + Z * Z + W * W;
        }

        #endregion

        #region Static Methods and Operators

        #region Length
        public static float Length(ref Vector4 v)
        {
            return MathHelper.Sqrt(v.X * v.X + v.Y * v.Y + v.Z * v.Z + v.W * v.W);
        }
        public static float Length(Vector4 v)
        {
            return MathHelper.Sqrt(v.X * v.X + v.Y * v.Y + v.Z * v.Z + v.W * v.W);
        }
        #endregion

        #region Distance
        public static float Distance(ref Vector4 left, ref Vector4 right)
        {
            Vector4 difference;
            Subtract(ref left, ref right, out difference);
            return Length(ref difference);
        }
        public static float Distance(Vector4 left, Vector4 right)
        {
            SubtractInPlace(ref left, ref right);
            return Length(ref left);
        }
        #endregion

        #region Length Squared
        public static float LengthSquared(ref Vector4 v)
        {
            return v.X * v.X + v.Y * v.Y + v.Z * v.Z + v.W * v.W;
        }
        public static float LengthSquared(Vector4 v)
        {
            return v.X * v.X + v.Y * v.Y + v.Z * v.Z + v.W * v.W;
        }
        #endregion

        #region Distance Squared
        public static float DistanceSquared(ref Vector4 left, ref Vector4 right)
        {
            Vector4 difference;
            Subtract(ref left, ref right, out difference);
            return LengthSquared(ref difference);
        }
        public static float DistanceSquared(Vector4 left, Vector4 right)
        {
            SubtractInPlace(ref left, ref right);
            return LengthSquared(ref left);
        }
        #endregion

        #region Negate
        public static void Negate(ref Vector4 v, out Vector4 result)
        {
            result.X = -v.X;
            result.Y = -v.Y;
            result.Z = -v.Z;
            result.W = -v.W;
        }
        public static void NegateInPlace(ref Vector4 v)
        {
            Negate(ref v, out v);
        }
        public static Vector4 Negate(Vector4 v)
        {
            Negate(ref v, out v);
            return v;
        }
        public static Vector4 operator -(Vector4 v)
        {
            Negate(ref v, out v);
            return v;
        }
        #endregion

        #region Add
        public static void Add(ref Vector4 left, ref Vector4 right, out Vector4 result)
        {
            result.X = left.X + right.X;
            result.Y = left.Y + right.Y;
            result.Z = left.Z + right.Z;
            result.W = left.W + right.W;
        }
        public static void AddInPlace(ref Vector4 leftAndResult, ref Vector4 right)
        {
            Add(ref leftAndResult, ref right, out leftAndResult);
        }
        public static Vector4 Add(Vector4 left, Vector4 right)
        {
            Add(ref left, ref right, out left);
            return left;
        }
        public static Vector4 operator +(Vector4 left, Vector4 right)
        {
            Add(ref left, ref right, out left);
            return left;
        }
        #endregion

        #region Subtract
        public static void Subtract(ref Vector4 left, ref Vector4 right, out Vector4 result)
        {
            result.X = left.X - right.X;
            result.Y = left.Y - right.Y;
            result.Z = left.Z - right.Z;
            result.W = left.W - right.W;
        }
        public static void SubtractInPlace(ref Vector4 leftAndResult, ref Vector4 right)
        {
            Subtract(ref leftAndResult, ref right, out leftAndResult);
        }
        public static Vector4 Subtract(Vector4 left, Vector4 right)
        {
            Subtract(ref left, ref right, out left);
            return left;
        }
        public static Vector4 operator -(Vector4 left, Vector4 right)
        {
            Subtract(ref left, ref right, out left);
            return left;
        }
        #endregion

        #region Multiply (scalar)
        public static void Multiply(ref Vector4 v, float scale, out Vector4 result)
        {
            result.X = v.X * scale;
            result.Y = v.Y * scale;
            result.Z = v.Z * scale;
            result.W = v.W * scale;
        }
        public static void MultiplyInPlace(ref Vector4 v, float scale)
        {
            Multiply(ref v, scale, out v);
        }
        public static Vector4 Multiply(Vector4 v, float scale)
        {
            Multiply(ref v, scale, out v);
            return v;
        }
        public static Vector4 operator *(float scale, Vector4 v)
        {
            Multiply(ref v, scale, out v);
            return v;
        }
        public static Vector4 operator *(Vector4 v, float scale)
        {
            Multiply(ref v, scale, out v);
            return v;
        }
        #endregion

        #region Dot
        public static float Dot(ref Vector4 left, ref Vector4 right)
        {
            return left.X * right.X + left.Y * right.Y + left.Z * right.Z + left.W * right.W;
        }
        public static float Dot(Vector4 left, Vector4 right)
        {
            return left.X * right.X + left.Y * right.Y + left.Z * right.Z + left.W * right.W;
        }/*
        public static float operator *(Vector4 left, Vector4 right)
        {
            return left.X * right.X + left.Y * right.Y + left.Z * right.Z + left.W * right.W;
        }*/
        #endregion

        #region Lerp
        public static void Lerp(ref Vector4 left, ref Vector4 right, float amount, out Vector4 result)
        {
            result.X = left.X - amount * (left.X - right.X);
            result.Y = left.Y - amount * (left.Y - right.Y);
            result.Z = left.Z - amount * (left.Z - right.Z);
            result.W = left.W - amount * (left.W - right.W);
        }
        public static void LerpInPlace(ref Vector4 leftAndResult, ref Vector4 right, float amount)
        {
            Lerp(ref leftAndResult, ref right, amount, out leftAndResult);
        }
        public static Vector4 Lerp(Vector4 left, Vector4 right, float amount)
        {
            Lerp(ref left, ref right, amount, out left);
            return left;
        }
        #endregion

        #endregion

        #region Constant Vectors

        /// <summary>
        /// Returns a new vector with all coordinates equal to zero
        /// </summary>
        public static Vector4 Zero
        {
            get { return new Vector4(); }
        }

        /// <summary>
        /// Returns a {1, 0, 0, 0} vector
        /// </summary>
        public static Vector4 UnitX
        {
            get { return new Vector4 { X = 1f }; }
        }

        /// <summary>
        /// Returns a {0, 1, 0, 0} vector
        /// </summary>
        public static Vector4 UnitY
        {
            get { return new Vector4 { Y = 1f }; }
        }

        /// <summary>
        /// Returns a {0, 0, 1, 0} vector
        /// </summary>
        public static Vector4 UnitZ
        {
            get { return new Vector4 { Z = 1f }; }
        }

        /// <summary>
        /// Returns a {0, 0, 0, 1} vector
        /// </summary>
        public static Vector4 UnitW
        {
            get { return new Vector4 { W = 1f }; }
        }

        #endregion
    }
}
