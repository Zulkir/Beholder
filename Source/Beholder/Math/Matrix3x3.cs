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
using System.Runtime.InteropServices;
using System.Text;

namespace Beholder.Math
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct Matrix3x3 : IEquatable<Matrix3x3>
    {
        /// <summary>
        /// First matrix column
        /// </summary>
        public Vector3 C1;

        /// <summary>
        /// Second matrix column
        /// </summary>
        public Vector3 C2;

        /// <summary>
        /// Third matrix column
        /// </summary>
        public Vector3 C3;

        #region Constructors
        public Matrix3x3(ref Vector3 c1, ref Vector3 c2, ref Vector3 c3)
        {
            C1 = c1;
            C2 = c2;
            C3 = c3;
        }
        public Matrix3x3(Vector3 c1, Vector3 c2, Vector3 c3) : this(ref c1, ref c2, ref c3) { }
        #endregion

        public float M11 { get { return C1.X; } set { C1.X = value; } }
        public float M21 { get { return C1.Y; } set { C1.Y = value; } }
        public float M31 { get { return C1.Z; } set { C1.Z = value; } }

        public float M12 { get { return C2.X; } set { C2.X = value; } }
        public float M22 { get { return C2.Y; } set { C2.Y = value; } }
        public float M32 { get { return C2.Z; } set { C2.Z = value; } }

        public float M13 { get { return C3.X; } set { C3.X = value; } }
        public float M23 { get { return C3.Y; } set { C3.Y = value; } }
        public float M33 { get { return C3.Z; } set { C3.Z = value; } }

        public float this[int rowMinusOne, int columnMinusOne]
        {
            get
            {
                switch (columnMinusOne)
                {
                    case 0:
                        switch (rowMinusOne)
                        {
                            case 0: return C1.X;
                            case 1: return C1.Y;
                            case 2: return C1.Z;
                            default: throw new IndexOutOfRangeException();
                        }
                    case 1:
                        switch (rowMinusOne)
                        {
                            case 0: return C2.X;
                            case 1: return C2.Y;
                            case 2: return C2.Z;
                            default: throw new IndexOutOfRangeException();
                        }
                    case 2:
                        switch (rowMinusOne)
                        {
                            case 0: return C3.X;
                            case 1: return C3.Y;
                            case 2: return C3.Z;
                            default: throw new IndexOutOfRangeException();
                        }
                    default: throw new IndexOutOfRangeException();
                }
            }
        }

        #region Object Overrides
        public bool Equals(Matrix3x3 other)
        {
            return
                C1.X == other.C1.X && C1.Y == other.C1.Y && C1.Z == other.C1.Z &&
                C2.X == other.C2.X && C2.Y == other.C2.Y && C2.Z == other.C2.Z &&
                C3.X == other.C3.X && C3.Y == other.C3.Y && C3.Z == other.C3.Z;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Matrix3x3)) return false;
            Matrix3x3 other = (Matrix3x3)obj;
            return Equals(ref this, ref other);
        }

        public override int GetHashCode()
        {
            return
                C1.X.GetHashCode() + C1.Y.GetHashCode() << 4 + C1.Z.GetHashCode() << 8 +
                C2.X.GetHashCode() << 12 + C2.Y.GetHashCode() << 16 + C2.Z.GetHashCode() << 20 +
                C3.X.GetHashCode() << 24 + C3.Y.GetHashCode() << 28 + C3.Z.GetHashCode() << 30;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder(128);
            builder.Append("{ C1:");
            builder.Append(C1.ToString());
            builder.Append(" C2:");
            builder.Append(C2.ToString());
            builder.Append(" C3:");
            builder.Append(C3.ToString());
            builder.Append(" }");

            return builder.ToString();
        }

        public static bool Equals(ref Matrix3x3 m1, ref Matrix3x3 m2)
        {
            return
                m1.C1.X == m2.C1.X && m1.C1.Y == m2.C1.Y && m1.C1.Z == m2.C1.Z &&
                m1.C2.X == m2.C2.X && m1.C2.Y == m2.C2.Y && m1.C2.Z == m2.C2.Z &&
                m1.C3.X == m2.C3.X && m1.C3.Y == m2.C3.Y && m1.C3.Z == m2.C3.Z;
        }
        #endregion

        #region Static Methods and Operators

        #region Determinant
        public static float Determinant(ref Matrix3x3 m)
        {
            return
                m.C1.X * (m.C2.Y * m.C3.Z - m.C3.Y * m.C2.Z) -
                m.C2.X * (m.C1.Y * m.C3.Z - m.C3.Y * m.C1.Z) +
                m.C3.X * (m.C1.Y * m.C2.Z - m.C2.Y * m.C1.Z);
        }
        public static float Determinant(Matrix3x3 m)
        {
            return Determinant(ref m);
        }
        #endregion

        #region Negate
        public static void Negate(ref Matrix3x3 m, out Matrix3x3 result)
        {
            result.C1.X = -m.C1.X;
            result.C1.Y = -m.C1.Y;
            result.C1.Z = -m.C1.Z;

            result.C2.X = -m.C2.X;
            result.C2.Y = -m.C2.Y;
            result.C2.Z = -m.C2.Z;

            result.C3.X = -m.C3.X;
            result.C3.Y = -m.C3.Y;
            result.C3.Z = -m.C3.Z;
        }
        public static void NegateInPlace(ref Matrix3x3 m)
        {
            Negate(ref m, out m);
        }
        public static Matrix3x3 Negate(Matrix3x3 m)
        {
            Negate(ref m, out m);
            return m;
        }
        public static Matrix3x3 operator -(Matrix3x3 m)
        {
            Negate(ref m, out m);
            return m;
        }
        #endregion

        #region Add
        public static void Add(ref Matrix3x3 left, ref Matrix3x3 right, out Matrix3x3 result)
        {
            result.C1.X = left.C1.X + right.C1.X;
            result.C1.Y = left.C1.Y + right.C1.Y;
            result.C1.Z = left.C1.Z + right.C1.Z;

            result.C2.X = left.C2.X + right.C2.X;
            result.C2.Y = left.C2.Y + right.C2.Y;
            result.C2.Z = left.C2.Z + right.C2.Z;

            result.C3.X = left.C3.X + right.C3.X;
            result.C3.Y = left.C3.Y + right.C3.Y;
            result.C3.Z = left.C3.Z + right.C3.Z;
        }
        public static void AddInPlace(ref Matrix3x3 leftAndResult, ref Matrix3x3 right)
        {
            Add(ref leftAndResult, ref right, out leftAndResult);
        }
        public static Matrix3x3 Add(Matrix3x3 left, Matrix3x3 right)
        {
            Add(ref left, ref right, out left);
            return left;
        }
        public static Matrix3x3 operator +(Matrix3x3 left, Matrix3x3 right)
        {
            Add(ref left, ref right, out left);
            return left;
        }
        #endregion

        #region Subtract
        public static void Subtract(ref Matrix3x3 left, ref Matrix3x3 right, out Matrix3x3 result)
        {
            result.C1.X = left.C1.X - right.C1.X;
            result.C1.Y = left.C1.Y - right.C1.Y;
            result.C1.Z = left.C1.Z - right.C1.Z;

            result.C2.X = left.C2.X - right.C2.X;
            result.C2.Y = left.C2.Y - right.C2.Y;
            result.C2.Z = left.C2.Z - right.C2.Z;

            result.C3.X = left.C3.X - right.C3.X;
            result.C3.Y = left.C3.Y - right.C3.Y;
            result.C3.Z = left.C3.Z - right.C3.Z;
        }
        public static void SubtractInPlace(ref Matrix3x3 leftAndResult, ref Matrix3x3 right)
        {
            Subtract(ref leftAndResult, ref right, out leftAndResult);
        }
        public static Matrix3x3 Subtract(Matrix3x3 left, Matrix3x3 right)
        {
            Subtract(ref left, ref right, out left);
            return left;
        }
        public static Matrix3x3 operator -(Matrix3x3 left, Matrix3x3 right)
        {
            Subtract(ref left, ref right, out left);
            return left;
        }
        #endregion

        #region Multiply (scalar)
        public static void Multiply(ref Matrix3x3 m, float scale, out Matrix3x3 result)
        {
            result.C1.X = m.C1.X * scale;
            result.C1.Y = m.C1.Y * scale;
            result.C1.Z = m.C1.Z * scale;

            result.C2.X = m.C2.X * scale;
            result.C2.Y = m.C2.Y * scale;
            result.C2.Z = m.C2.Z * scale;

            result.C3.X = m.C3.X * scale;
            result.C3.Y = m.C3.Y * scale;
            result.C3.Z = m.C3.Z * scale;
        }
        public static void MultiplyInPlace(ref Matrix3x3 m, float scale)
        {
            Multiply(ref m, scale, out m);
        }
        public static Matrix3x3 Multiply(Matrix3x3 m, float scale)
        {
            Multiply(ref m, scale, out m);
            return m;
        }
        public static Matrix3x3 operator *(float scale, Matrix3x3 m)
        {
            Multiply(ref m, scale, out m);
            return m;
        }
        public static Matrix3x3 operator *(Matrix3x3 m, float scale)
        {
            Multiply(ref m, scale, out m);
            return m;
        }
        #endregion

        #region Multiply (vector)
        public static void Multiply(ref Vector3 v, ref Matrix3x3 m, out Vector3 result)
        {
            result.X = v.X * m.C1.X + v.Y * m.C1.Y + v.Z * m.C1.Z;
            result.Y = v.X * m.C2.X + v.Y * m.C2.Y + v.Z * m.C2.Z;
            result.Z = v.X * m.C3.X + v.Y * m.C3.Y + v.Z * m.C3.Z;
        }
        public static void MultiplyInPlace(ref Vector3 v, ref Matrix3x3 m)
        {
            Vector3 result;
            Multiply(ref v, ref m, out result);
            v = result;
        }
        public static Vector3 Multiply(Vector3 v, Matrix3x3 m)
        {
            Vector3 result;
            Multiply(ref v, ref m, out result);
            return result;
        }
        public static Vector3 operator *(Vector3 v, Matrix3x3 m)
        {
            Vector3 result;
            Multiply(ref v, ref m, out result);
            return result;
        }
        #endregion

        #region Multiply (matrix)
        public static void Multiply(ref Matrix3x3 left, ref Matrix3x3 right, out Matrix3x3 result)
        {
            result.C1.X = (left.C1.X * right.C1.X) + (left.C2.X * right.C1.Y) + (left.C3.X * right.C1.Z);
            result.C1.Y = (left.C1.Y * right.C1.X) + (left.C2.Y * right.C1.Y) + (left.C3.Y * right.C1.Z);
            result.C1.Z = (left.C1.Z * right.C1.X) + (left.C2.Z * right.C1.Y) + (left.C3.Z * right.C1.Z);

            result.C2.X = (left.C1.X * right.C2.X) + (left.C2.X * right.C2.Y) + (left.C3.X * right.C2.Z);
            result.C2.Y = (left.C1.Y * right.C2.X) + (left.C2.Y * right.C2.Y) + (left.C3.Y * right.C2.Z);
            result.C2.Z = (left.C1.Z * right.C2.X) + (left.C2.Z * right.C2.Y) + (left.C3.Z * right.C2.Z);

            result.C3.X = (left.C1.X * right.C3.X) + (left.C2.X * right.C3.Y) + (left.C3.X * right.C3.Z);
            result.C3.Y = (left.C1.Y * right.C3.X) + (left.C2.Y * right.C3.Y) + (left.C3.Y * right.C3.Z);
            result.C3.Z = (left.C1.Z * right.C3.X) + (left.C2.Z * right.C3.Y) + (left.C3.Z * right.C3.Z);
        }
        public static void MultiplyInPlace(ref Matrix3x3 leftAndResult, ref Matrix3x3 right)
        {
            Matrix3x3 result;
            Multiply(ref leftAndResult, ref right, out result);
            leftAndResult = result;
        }
        public static Matrix3x3 Multiply(Matrix3x3 left, Matrix3x3 right)
        {
            Matrix3x3 result;
            Multiply(ref left, ref right, out result);
            return result;
        }
        public static Matrix3x3 operator *(Matrix3x3 left, Matrix3x3 right)
        {
            Matrix3x3 result;
            Multiply(ref left, ref right, out result);
            return result;
        }
        #endregion

        #region Transpose
        public static void Transpose(ref Matrix3x3 m, out Matrix3x3 result)
        {
            result.C1.X = m.C1.X;
            result.C1.Y = m.C2.X;
            result.C1.Z = m.C3.X;

            result.C2.X = m.C1.Y;
            result.C2.Y = m.C2.Y;
            result.C2.Z = m.C3.Y;

            result.C3.X = m.C1.Z;
            result.C3.Y = m.C2.Z;
            result.C3.Z = m.C3.Z;
        }
        public static void TransposeInPlace(ref Matrix3x3 m)
        {
            Matrix3x3 result;
            Transpose(ref m, out result);
            m = result;
        }
        public static Matrix3x3 Transpose(Matrix3x3 m)
        {
            Matrix3x3 result;
            Transpose(ref m, out result);
            return result;
        }
        #endregion

        #region Invert
        public static void Invert(ref Matrix3x3 m, out Matrix3x3 result)
        {
            float d11 = m.C2.Y * m.C3.Z - m.C3.Y * m.C2.Z;
            float d12 = -(m.C1.Y * m.C3.Z - m.C3.Y * m.C1.Z);
            float d13 = m.C1.Y * m.C2.Z - m.C2.Y * m.C1.Z;
            float invDet = 1f / (m.C1.X * d11 + m.C2.X * d12 + m.C3.X * d13);
            result.C1.X = d11 * invDet;
            result.C1.Y = d12 * invDet;
            result.C1.Z = d13 * invDet;
            result.C2.X = -(m.C2.X * m.C3.Z - m.C3.X * m.C2.Z) * invDet;
            result.C2.Y = (m.C1.X * m.C3.Z - m.C3.X * m.C1.Z) * invDet;
            result.C2.Z = -(m.C1.X * m.C2.Z - m.C2.X * m.C1.Z) * invDet;
            result.C3.X = (m.C2.X * m.C3.Y - m.C3.X * m.C2.Y) * invDet;
            result.C3.Y = -(m.C1.X * m.C3.Y - m.C3.X * m.C1.Y) * invDet;
            result.C3.Z = (m.C1.X * m.C2.Y - m.C2.X * m.C1.Y) * invDet;
        }
        public static void InvertInPlace(ref Matrix3x3 m)
        {
            Matrix3x3 result;
            Invert(ref m, out result);
            m = result;
        }
        public static Matrix3x3 Invert(Matrix3x3 m)
        {
            Matrix3x3 result;
            Invert(ref m, out result);
            return result;
        }
        #endregion

        #region Invert And Transpose
        public static void InvertAndTranspose(ref Matrix3x3 m, out Matrix3x3 result)
        {
            float d11 = m.C2.Y * m.C3.Z - m.C3.Y * m.C2.Z;
            float d12 = -(m.C1.Y * m.C3.Z - m.C3.Y * m.C1.Z);
            float d13 = m.C1.Y * m.C2.Z - m.C2.Y * m.C1.Z;
            float invDet = 1f / (m.C1.X * d11 + m.C2.X * d12 + m.C3.X * d13);
            result.C1.X = d11 * invDet;
            result.C2.X = d12 * invDet;
            result.C3.X = d13 * invDet;
            result.C1.Y = -(m.C2.X * m.C3.Z - m.C3.X * m.C2.Z) * invDet;
            result.C2.Y = (m.C1.X * m.C3.Z - m.C3.X * m.C1.Z) * invDet;
            result.C3.Y = -(m.C1.X * m.C2.Z - m.C2.X * m.C1.Z) * invDet;
            result.C1.Z = (m.C2.X * m.C3.Y - m.C3.X * m.C2.Y) * invDet;
            result.C2.Z = -(m.C1.X * m.C3.Y - m.C3.X * m.C1.Y) * invDet;
            result.C3.Z = (m.C1.X * m.C2.Y - m.C2.X * m.C1.Y) * invDet;
        }
        public static void InvertAndTransposeInPlace(ref Matrix3x3 m)
        {
            Matrix3x3 result;
            InvertAndTranspose(ref m, out result);
            m = result;
        }
        public static Matrix3x3 InvertAndTranspose(Matrix3x3 m)
        {
            Matrix3x3 result;
            InvertAndTranspose(ref m, out result);
            return result;
        }
        #endregion

        #endregion

        #region Constant Matrices
        public static Matrix3x3 Zero
        {
            get { return new Matrix3x3(); }
        }
        public static Matrix3x3 Identity
        {
            get
            {
                var result = new Matrix3x3();
                result.C1.X = result.C2.Y = result.C3.Z = 1.0f;
                return result;
            }
        }
        #endregion

        #region Typical transforms

        #region Identity
        public static void CreateIdentity(out Matrix3x3 result)
        {
            result = new Matrix3x3();
            result.C1.X = result.C2.Y = result.C3.Z = 1.0f;
        }
        #endregion

        #region Scaling
        public static void CreateScaling(float scale, out Matrix3x3 result)
        {
            result = new Matrix3x3();
            result.C1.X = result.C2.Y = result.C3.Z = scale;
        }
        public static Matrix3x3 CreateScaling(float scale)
        {
            Matrix3x3 result;
            CreateScaling(scale, out result);
            return result;
        }
        public static void CreateScaling(float sx, float sy, float sz, out Matrix3x3 result)
        {
            result = new Matrix3x3 { C1 = { X = sx }, C2 = { Y = sy }, C3 = { Z = sz } };
        }
        public static Matrix3x3 CreateScaling(float sx, float sy, float sz)
        {
            Matrix3x3 result;
            CreateScaling(sx, sy, sz, out result);
            return result;
        }
        public static void CreateScaling(ref Vector3 scale, out Matrix3x3 result)
        {
            result = new Matrix3x3 { C1 = { X = scale.X }, C2 = { Y = scale.Y }, C3 = { Z = scale.Z } };
        }
        public static Matrix3x3 CreateScaling(Vector3 scale)
        {
            Matrix3x3 result;
            CreateScaling(ref scale, out result);
            return result;
        }
        #endregion

        #region Rotation
        public static void CreateRotationX(float angle, out Matrix3x3 result)
        {
            float sina = MathHelper.Sin(angle);
            float cosa = MathHelper.Cos(angle);

            result = new Matrix3x3 
            {
                C1 = {X = 1.0f}, 
                C2 = {Y = cosa, Z = -sina}, 
                C3 = {Y = sina, Z = cosa}
            };
        }
        public static Matrix3x3 CreateRotationX(float angle)
        {
            Matrix3x3 result;
            CreateRotationX(angle, out result);
            return result;
        }

        public static void CreateRotationY(float angle, out Matrix3x3 result)
        {
            float sina = MathHelper.Sin(angle);
            float cosa = MathHelper.Cos(angle);

            result = new Matrix3x3 
            {
                C1 = {X = cosa, Z = -sina}, 
                C2 = {Y = 1.0f}, 
                C3 = {X = sina, Z = cosa}
            };
        }
        public static Matrix3x3 CreateRotationY(float angle)
        {
            Matrix3x3 result;
            CreateRotationY(angle, out result);
            return result;
        }

        public static void CreateRotationZ(float angle, out Matrix3x3 result)
        {
            float sina = MathHelper.Sin(angle);
            float cosa = MathHelper.Cos(angle);

            result = new Matrix3x3 
            {
                C1 = {X = cosa, Y = -sina}, 
                C2 = {X = sina, Y = cosa}, 
                C3 = {Z = 1.0f}
            };
        }
        public static Matrix3x3 CreateRotationZ(float angle)
        {
            Matrix3x3 result;
            CreateRotationZ(angle, out result);
            return result;
        }
        #endregion

        #endregion
    }
}
