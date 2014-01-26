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
using System.Text;
using System.Runtime.InteropServices;

namespace Beholder.Math
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct Matrix4x4 : IEquatable<Matrix4x4>
    {
        /// <summary>
        /// First matrix column
        /// </summary>
        public Vector4 C1;

        /// <summary>
        /// Second matrix column
        /// </summary>
        public Vector4 C2;

        /// <summary>
        /// Third matrix column
        /// </summary>
        public Vector4 C3;

        /// <summary>
        /// Fourth matrix column
        /// </summary>
        public Vector4 C4;

        #region Constructors
        public Matrix4x4(ref Vector4 c1, ref Vector4 c2, ref Vector4 c3, ref Vector4 c4)
        {
            C1 = c1;
            C2 = c2;
            C3 = c3;
            C4 = c4;
        }
        public Matrix4x4(Vector4 c1, Vector4 c2, Vector4 c3, Vector4 c4) : this(ref c1, ref c2, ref c3, ref c4) { }

        public Matrix4x4(ref Matrix4x3 matrix4x3)
        {
            FromMatrix4x3(ref matrix4x3, out this);
        }
        public Matrix4x4(Matrix4x3 matrix4x3) : this(ref matrix4x3) { }

        public Matrix4x4(ref Matrix3x3 matrix3x3)
        {
            FromMatrix3x3(ref matrix3x3, out this);
        }
        public Matrix4x4(Matrix3x3 matrix3x3) : this(ref matrix3x3) { }
        #endregion

        public float M11 { get { return C1.X; } set { C1.X = value; } }
        public float M21 { get { return C1.Y; } set { C1.Y = value; } }
        public float M31 { get { return C1.Z; } set { C1.Z = value; } }
        public float M41 { get { return C1.W; } set { C1.W = value; } }

        public float M12 { get { return C2.X; } set { C2.X = value; } }
        public float M22 { get { return C2.Y; } set { C2.Y = value; } }
        public float M32 { get { return C2.Z; } set { C2.Z = value; } }
        public float M42 { get { return C2.W; } set { C2.W = value; } }

        public float M13 { get { return C3.X; } set { C3.X = value; } }
        public float M23 { get { return C3.Y; } set { C3.Y = value; } }
        public float M33 { get { return C3.Z; } set { C3.Z = value; } }
        public float M43 { get { return C3.W; } set { C3.W = value; } }

        public float M14 { get { return C4.X; } set { C4.X = value; } }
        public float M24 { get { return C4.Y; } set { C4.Y = value; } }
        public float M34 { get { return C4.Z; } set { C4.Z = value; } }
        public float M44 { get { return C4.W; } set { C4.W = value; } }

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
                            case 3: return C1.W;
                            default: throw new IndexOutOfRangeException();
                        }
                    case 1:
                        switch (rowMinusOne)
                        {
                            case 0: return C2.X;
                            case 1: return C2.Y;
                            case 2: return C2.Z;
                            case 3: return C2.W;
                            default: throw new IndexOutOfRangeException();
                        }
                    case 2:
                        switch (rowMinusOne)
                        {
                            case 0: return C3.X;
                            case 1: return C3.Y;
                            case 2: return C3.Z;
                            case 3: return C3.W;
                            default: throw new IndexOutOfRangeException();
                        }
                    case 3:
                        switch (rowMinusOne)
                        {
                            case 0: return C4.X;
                            case 1: return C4.Y;
                            case 2: return C4.Z;
                            case 3: return C4.W;
                            default: throw new IndexOutOfRangeException();
                        }
                    default: throw new IndexOutOfRangeException();
                }
            }
        }

        #region Object Overrides
        public bool Equals(Matrix4x4 other)
        {
            return
                C1.X == other.C1.X && C1.Y == other.C1.Y && C1.Z == other.C1.Z && C1.W == other.C1.W &&
                C2.X == other.C2.X && C2.Y == other.C2.Y && C2.Z == other.C2.Z && C2.W == other.C2.W &&
                C3.X == other.C3.X && C3.Y == other.C3.Y && C3.Z == other.C3.Z && C3.W == other.C3.W &&
                C4.X == other.C4.X && C4.Y == other.C4.Y && C4.Z == other.C4.Z && C4.W == other.C4.W;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Matrix4x4)) return false;
            Matrix4x4 other = (Matrix4x4)obj;
            return Equals(ref this, ref other);
        }

        public override int GetHashCode()
        {
            return
                C1.X.GetHashCode()          + C1.Y.GetHashCode() << 2   + C1.Z.GetHashCode() << 4   + C1.W.GetHashCode() << 6   +
                C2.X.GetHashCode() << 8     + C2.Y.GetHashCode() << 10  + C2.Z.GetHashCode() << 12  + C2.W.GetHashCode() << 14  +
                C3.X.GetHashCode() << 16    + C3.Y.GetHashCode() << 18  + C3.Z.GetHashCode() << 20  + C3.W.GetHashCode() << 22  +
                C4.X.GetHashCode() << 24    + C4.Y.GetHashCode() << 26  + C4.Z.GetHashCode() << 28  + C4.W.GetHashCode() << 30;
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
            builder.Append(" C4:");
            builder.Append(C4.ToString());
            builder.Append(" }");

            return builder.ToString();
        }

        public static bool Equals(ref Matrix4x4 m1, ref Matrix4x4 m2)
        {
            return
                m1.C1.X == m2.C1.X && m1.C1.Y == m2.C1.Y && m1.C1.Z == m2.C1.Z && m1.C1.W == m2.C1.W &&
                m1.C2.X == m2.C2.X && m1.C2.Y == m2.C2.Y && m1.C2.Z == m2.C2.Z && m1.C2.W == m2.C2.W &&
                m1.C3.X == m2.C3.X && m1.C3.Y == m2.C3.Y && m1.C3.Z == m2.C3.Z && m1.C3.W == m2.C3.W &&
                m1.C4.X == m2.C4.X && m1.C4.Y == m2.C4.Y && m1.C4.Z == m2.C4.Z && m1.C4.W == m2.C4.W;
        }
        #endregion

        #region Static Methods and Operators

        #region Determinant
        public static float Determinant(ref Matrix4x4 m)
        {
            float d3344 = m.C3.Z * m.C4.W - m.C4.Z * m.C3.W;
            float d3244 = m.C2.Z * m.C4.W - m.C4.Z * m.C2.W;
            float d3243 = m.C2.Z * m.C3.W - m.C3.Z * m.C2.W;
            float d3144 = m.C1.Z * m.C4.W - m.C4.Z * m.C1.W;
            float d3143 = m.C1.Z * m.C3.W - m.C3.Z * m.C1.W;
            float d3142 = m.C1.Z * m.C2.W - m.C2.Z * m.C1.W;

            return
                m.C1.X * (m.C2.Y * d3344 - m.C3.Y * d3244 + m.C4.Y * d3243) -
                m.C2.X * (m.C1.Y * d3344 - m.C3.Y * d3144 + m.C4.Y * d3143) +
                m.C3.X * (m.C1.Y * d3244 - m.C2.Y * d3144 + m.C4.Y * d3142) -
                m.C4.X * (m.C1.Y * d3243 - m.C2.Y * d3143 + m.C3.Y * d3142);
        }
        public static float Determinant(Matrix4x4 m)
        {
            return Determinant(ref m);
        }
        #endregion

        #region Negate
        public static void Negate(ref Matrix4x4 m, out Matrix4x4 result)
        {
            result.C1.X = -m.C1.X;
            result.C1.Y = -m.C1.Y;
            result.C1.Z = -m.C1.Z;
            result.C1.W = -m.C1.W;

            result.C2.X = -m.C2.X;
            result.C2.Y = -m.C2.Y;
            result.C2.Z = -m.C2.Z;
            result.C2.W = -m.C2.W;

            result.C3.X = -m.C3.X;
            result.C3.Y = -m.C3.Y;
            result.C3.Z = -m.C3.Z;
            result.C3.W = -m.C3.W;

            result.C4.X = -m.C4.X;
            result.C4.Y = -m.C4.Y;
            result.C4.Z = -m.C4.Z;
            result.C4.W = -m.C4.W;
        }
        public static void NegateInPlace(ref Matrix4x4 m)
        {
            Negate(ref m, out m);
        }
        public static Matrix4x4 Negate(Matrix4x4 m)
        {
            Negate(ref m, out m);
            return m;
        }
        public static Matrix4x4 operator -(Matrix4x4 m)
        {
            Negate(ref m, out m);
            return m;
        }
        #endregion

        #region Add
        public static void Add(ref Matrix4x4 left, ref Matrix4x4 right, out Matrix4x4 result)
        {
            result.C1.X = left.C1.X + right.C1.X;
            result.C1.Y = left.C1.Y + right.C1.Y;
            result.C1.Z = left.C1.Z + right.C1.Z;
            result.C1.W = left.C1.W + right.C1.W;

            result.C2.X = left.C2.X + right.C2.X;
            result.C2.Y = left.C2.Y + right.C2.Y;
            result.C2.Z = left.C2.Z + right.C2.Z;
            result.C2.W = left.C2.W + right.C2.W;

            result.C3.X = left.C3.X + right.C3.X;
            result.C3.Y = left.C3.Y + right.C3.Y;
            result.C3.Z = left.C3.Z + right.C3.Z;
            result.C3.W = left.C3.W + right.C3.W;

            result.C4.X = left.C4.X + right.C4.X;
            result.C4.Y = left.C4.Y + right.C4.Y;
            result.C4.Z = left.C4.Z + right.C4.Z;
            result.C4.W = left.C4.W + right.C4.W;
        }
        public static void AddInPlace(ref Matrix4x4 leftAndResult, ref Matrix4x4 right)
        {
            Add(ref leftAndResult, ref right, out leftAndResult);
        }
        public static Matrix4x4 Add(Matrix4x4 left, Matrix4x4 right)
        {
            Add(ref left, ref right, out left);
            return left;
        }
        public static Matrix4x4 operator +(Matrix4x4 left, Matrix4x4 right)
        {
            Add(ref left, ref right, out left);
            return left;
        }
        #endregion

        #region Subtract
        public static void Subtract(ref Matrix4x4 left, ref Matrix4x4 right, out Matrix4x4 result)
        {
            result.C1.X = left.C1.X - right.C1.X;
            result.C1.Y = left.C1.Y - right.C1.Y;
            result.C1.Z = left.C1.Z - right.C1.Z;
            result.C1.W = left.C1.W - right.C1.W;

            result.C2.X = left.C2.X - right.C2.X;
            result.C2.Y = left.C2.Y - right.C2.Y;
            result.C2.Z = left.C2.Z - right.C2.Z;
            result.C2.W = left.C2.W - right.C2.W;

            result.C3.X = left.C3.X - right.C3.X;
            result.C3.Y = left.C3.Y - right.C3.Y;
            result.C3.Z = left.C3.Z - right.C3.Z;
            result.C3.W = left.C3.W - right.C3.W;

            result.C4.X = left.C4.X - right.C4.X;
            result.C4.Y = left.C4.Y - right.C4.Y;
            result.C4.Z = left.C4.Z - right.C4.Z;
            result.C4.W = left.C4.W - right.C4.W;
        }
        public static void SubtractInPlace(ref Matrix4x4 leftAndResult, ref Matrix4x4 right)
        {
            Subtract(ref leftAndResult, ref right, out leftAndResult);
        }
        public static Matrix4x4 Subtract(Matrix4x4 left, Matrix4x4 right)
        {
            Subtract(ref left, ref right, out left);
            return left;
        }
        public static Matrix4x4 operator -(Matrix4x4 left, Matrix4x4 right)
        {
            Subtract(ref left, ref right, out left);
            return left;
        }
        #endregion

        #region Multiply (scalar)
        public static void Multiply(ref Matrix4x4 m, float scale, out Matrix4x4 result)
        {
            result.C1.X = m.C1.X * scale;
            result.C1.Y = m.C1.Y * scale;
            result.C1.Z = m.C1.Z * scale;
            result.C1.W = m.C1.W * scale;

            result.C2.X = m.C2.X * scale;
            result.C2.Y = m.C2.Y * scale;
            result.C2.Z = m.C2.Z * scale;
            result.C2.W = m.C2.W * scale;

            result.C3.X = m.C3.X * scale;
            result.C3.Y = m.C3.Y * scale;
            result.C3.Z = m.C3.Z * scale;
            result.C3.W = m.C3.W * scale;

            result.C4.X = m.C4.X * scale;
            result.C4.Y = m.C4.Y * scale;
            result.C4.Z = m.C4.Z * scale;
            result.C4.W = m.C4.W * scale;
        }
        public static void MultiplyInPlace(ref Matrix4x4 m, float scale)
        {
            Multiply(ref m, scale, out m);
        }
        public static Matrix4x4 Multiply(Matrix4x4 m, float scale)
        {
            Multiply(ref m, scale, out m);
            return m;
        }
        public static Matrix4x4 operator *(float scale, Matrix4x4 m)
        {
            Multiply(ref m, scale, out m);
            return m;
        }
        public static Matrix4x4 operator *(Matrix4x4 m, float scale)
        {
            Multiply(ref m, scale, out m);
            return m;
        }
        #endregion

        #region Multiply (vector)
        public static void Multiply(ref Vector4 v, ref Matrix4x4 m, out Vector4 result)
        {
            result.X = v.X * m.C1.X + v.Y * m.C1.Y + v.Z * m.C1.Z + v.W * m.C1.W;
            result.Y = v.X * m.C2.X + v.Y * m.C2.Y + v.Z * m.C2.Z + v.W * m.C2.W;
            result.Z = v.X * m.C3.X + v.Y * m.C3.Y + v.Z * m.C3.Z + v.W * m.C3.W;
            result.W = v.X * m.C4.X + v.Y * m.C4.Y + v.Z * m.C4.Z + v.W * m.C4.W;
        }
        public static void MultiplyInPlace(ref Vector4 v, ref Matrix4x4 m)
        {
            Vector4 result;
            Multiply(ref v, ref m, out result);
            v = result;
        }
        public static Vector4 Multiply(Vector4 v, Matrix4x4 m)
        {
            Vector4 result;
            Multiply(ref v, ref m, out result);
            return result;
        }
        public static Vector4 operator *(Vector4 v, Matrix4x4 m)
        {
            Vector4 result;
            Multiply(ref v, ref m, out result);
            return result;
        }
        #endregion

        #region Multiply (matrix)
        public static void Multiply(ref Matrix4x4 left, ref Matrix4x4 right, out Matrix4x4 result)
        {
            result.C1.X = (left.C1.X * right.C1.X) + (left.C2.X * right.C1.Y) + (left.C3.X * right.C1.Z) + (left.C4.X * right.C1.W);
            result.C1.Y = (left.C1.Y * right.C1.X) + (left.C2.Y * right.C1.Y) + (left.C3.Y * right.C1.Z) + (left.C4.Y * right.C1.W);
            result.C1.Z = (left.C1.Z * right.C1.X) + (left.C2.Z * right.C1.Y) + (left.C3.Z * right.C1.Z) + (left.C4.Z * right.C1.W);
            result.C1.W = (left.C1.W * right.C1.X) + (left.C2.W * right.C1.Y) + (left.C3.W * right.C1.Z) + (left.C4.W * right.C1.W);

            result.C2.X = (left.C1.X * right.C2.X) + (left.C2.X * right.C2.Y) + (left.C3.X * right.C2.Z) + (left.C4.X * right.C2.W);
            result.C2.Y = (left.C1.Y * right.C2.X) + (left.C2.Y * right.C2.Y) + (left.C3.Y * right.C2.Z) + (left.C4.Y * right.C2.W);
            result.C2.Z = (left.C1.Z * right.C2.X) + (left.C2.Z * right.C2.Y) + (left.C3.Z * right.C2.Z) + (left.C4.Z * right.C2.W);
            result.C2.W = (left.C1.W * right.C2.X) + (left.C2.W * right.C2.Y) + (left.C3.W * right.C2.Z) + (left.C4.W * right.C2.W);

            result.C3.X = (left.C1.X * right.C3.X) + (left.C2.X * right.C3.Y) + (left.C3.X * right.C3.Z) + (left.C4.X * right.C3.W);
            result.C3.Y = (left.C1.Y * right.C3.X) + (left.C2.Y * right.C3.Y) + (left.C3.Y * right.C3.Z) + (left.C4.Y * right.C3.W);
            result.C3.Z = (left.C1.Z * right.C3.X) + (left.C2.Z * right.C3.Y) + (left.C3.Z * right.C3.Z) + (left.C4.Z * right.C3.W);
            result.C3.W = (left.C1.W * right.C3.X) + (left.C2.W * right.C3.Y) + (left.C3.W * right.C3.Z) + (left.C4.W * right.C3.W);

            result.C4.X = (left.C1.X * right.C4.X) + (left.C2.X * right.C4.Y) + (left.C3.X * right.C4.Z) + (left.C4.X * right.C4.W);
            result.C4.Y = (left.C1.Y * right.C4.X) + (left.C2.Y * right.C4.Y) + (left.C3.Y * right.C4.Z) + (left.C4.Y * right.C4.W);
            result.C4.Z = (left.C1.Z * right.C4.X) + (left.C2.Z * right.C4.Y) + (left.C3.Z * right.C4.Z) + (left.C4.Z * right.C4.W);
            result.C4.W = (left.C1.W * right.C4.X) + (left.C2.W * right.C4.Y) + (left.C3.W * right.C4.Z) + (left.C4.W * right.C4.W);
        }
        public static void MultiplyInPlace(ref Matrix4x4 leftAndResult, ref Matrix4x4 right)
        {
            Matrix4x4 result;
            Multiply(ref leftAndResult, ref right, out result);
            leftAndResult = result;
        }
        public static Matrix4x4 Multiply(Matrix4x4 left, Matrix4x4 right)
        {
            Matrix4x4 result;
            Multiply(ref left, ref right, out result);
            return result;
        }
        public static Matrix4x4 operator *(Matrix4x4 left, Matrix4x4 right)
        {
            Matrix4x4 result;
            Multiply(ref left, ref right, out result);
            return result;
        }
        #endregion

        #region Transpose
        public static void Transpose(ref Matrix4x4 m, out Matrix4x4 result)
        {
            result.C1.X = m.C1.X;
            result.C1.Y = m.C2.X;
            result.C1.Z = m.C3.X;
            result.C1.W = m.C4.X;

            result.C2.X = m.C1.Y;
            result.C2.Y = m.C2.Y;
            result.C2.Z = m.C3.Y;
            result.C2.W = m.C4.Y;

            result.C3.X = m.C1.Z;
            result.C3.Y = m.C2.Z;
            result.C3.Z = m.C3.Z;
            result.C3.W = m.C4.Z;

            result.C4.X = m.C1.W;
            result.C4.Y = m.C2.W;
            result.C4.Z = m.C3.W;
            result.C4.W = m.C4.W;
        }
        public static void TransposeInPlace(ref Matrix4x4 m)
        {
            Matrix4x4 result;
            Transpose(ref m, out result);
            m = result;
        }
        public static Matrix4x4 Transpose(Matrix4x4 m)
        {
            Matrix4x4 result;
            Transpose(ref m, out result);
            return result;
        }
        #endregion

        #region Invert
        public static void Invert(ref Matrix4x4 m, out Matrix4x4 result)
        {
            float d3344 = m.C3.Z * m.C4.W - m.C4.Z * m.C3.W;
            float d3244 = m.C2.Z * m.C4.W - m.C4.Z * m.C2.W;
            float d3243 = m.C2.Z * m.C3.W - m.C3.Z * m.C2.W;
            float d3144 = m.C1.Z * m.C4.W - m.C4.Z * m.C1.W;
            float d3143 = m.C1.Z * m.C3.W - m.C3.Z * m.C1.W;
            float d3142 = m.C1.Z * m.C2.W - m.C2.Z * m.C1.W;
            float d11 = m.C2.Y * d3344 - m.C3.Y * d3244 + m.C4.Y * d3243;
            float d12 = -(m.C1.Y * d3344 - m.C3.Y * d3144 + m.C4.Y * d3143);
            float d13 = m.C1.Y * d3244 - m.C2.Y * d3144 + m.C4.Y * d3142;
            float d14 = -(m.C1.Y * d3243 - m.C2.Y * d3143 + m.C3.Y * d3142);
            float invDet = 1f / (m.C1.X * d11 + m.C2.X * d12 + m.C3.X * d13 + m.C4.X * d14);
            result.C1.X = d11 * invDet;
            result.C1.Y = d12 * invDet;
            result.C1.Z = d13 * invDet;
            result.C1.W = d14 * invDet;
            result.C2.X = -(m.C2.X * d3344 - m.C3.X * d3244 + m.C4.X * d3243) * invDet;
            result.C2.Y = (m.C1.X * d3344 - m.C3.X * d3144 + m.C4.X * d3143) * invDet;
            result.C2.Z = -(m.C1.X * d3244 - m.C2.X * d3144 + m.C4.X * d3142) * invDet;
            result.C2.W = (m.C1.X * d3243 - m.C2.X * d3143 + m.C3.X * d3142) * invDet;
            float d2344 = m.C3.Y * m.C4.W - m.C4.Y * m.C3.W;
            float d2244 = m.C2.Y * m.C4.W - m.C4.Y * m.C2.W;
            float d2243 = m.C2.Y * m.C3.W - m.C3.Y * m.C2.W;
            float d2144 = m.C1.Y * m.C4.W - m.C4.Y * m.C1.W;
            float d2143 = m.C1.Y * m.C3.W - m.C3.Y * m.C1.W;
            float d2142 = m.C1.Y * m.C2.W - m.C2.Y * m.C1.W;
            result.C3.X = (m.C2.X * d2344 - m.C3.X * d2244 + m.C4.X * d2243) * invDet;
            result.C3.Y = -(m.C1.X * d2344 - m.C3.X * d2144 + m.C4.X * d2143) * invDet;
            result.C3.Z = (m.C1.X * d2244 - m.C2.X * d2144 + m.C4.X * d2142) * invDet;
            result.C3.W = -(m.C1.X * d2243 - m.C2.X * d2143 + m.C3.X * d2142) * invDet;
            float d2334 = m.C3.Y * m.C4.Z - m.C4.Y * m.C3.Z;
            float d2234 = m.C2.Y * m.C4.Z - m.C4.Y * m.C2.Z;
            float d2233 = m.C2.Y * m.C3.Z - m.C3.Y * m.C2.Z;
            float d2134 = m.C1.Y * m.C4.Z - m.C4.Y * m.C1.Z;
            float d2133 = m.C1.Y * m.C3.Z - m.C3.Y * m.C1.Z;
            float d2132 = m.C1.Y * m.C2.Z - m.C2.Y * m.C1.Z;
            result.C4.X = -(m.C2.X * d2334 - m.C3.X * d2234 + m.C4.X * d2233) * invDet;
            result.C4.Y = (m.C1.X * d2334 - m.C3.X * d2134 + m.C4.X * d2133) * invDet;
            result.C4.Z = -(m.C1.X * d2234 - m.C2.X * d2134 + m.C4.X * d2132) * invDet;
            result.C4.W = (m.C1.X * d2233 - m.C2.X * d2133 + m.C3.X * d2132) * invDet;
        }
        public static void InvertInPlace(ref Matrix4x4 m)
        {
            Matrix4x4 result;
            Invert(ref m, out result);
            m = result;
        }
        public static Matrix4x4 Invert(Matrix4x4 m)
        {
            Matrix4x4 result;
            Invert(ref m, out result);
            return result;
        }
        #endregion

        #region Invert And Transpose
        public static void InvertAndTranspose(ref Matrix4x4 m, out Matrix4x4 result)
        {
            float d3344 = m.C3.Z * m.C4.W - m.C4.Z * m.C3.W;
            float d3244 = m.C2.Z * m.C4.W - m.C4.Z * m.C2.W;
            float d3243 = m.C2.Z * m.C3.W - m.C3.Z * m.C2.W;
            float d3144 = m.C1.Z * m.C4.W - m.C4.Z * m.C1.W;
            float d3143 = m.C1.Z * m.C3.W - m.C3.Z * m.C1.W;
            float d3142 = m.C1.Z * m.C2.W - m.C2.Z * m.C1.W;
            float d11 = m.C2.Y * d3344 - m.C3.Y * d3244 + m.C4.Y * d3243;
            float d12 = -(m.C1.Y * d3344 - m.C3.Y * d3144 + m.C4.Y * d3143);
            float d13 = m.C1.Y * d3244 - m.C2.Y * d3144 + m.C4.Y * d3142;
            float d14 = -(m.C1.Y * d3243 - m.C2.Y * d3143 + m.C3.Y * d3142);
            float invDet = 1f / (m.C1.X * d11 + m.C2.X * d12 + m.C3.X * d13 + m.C4.X * d14);
            result.C1.X = d11 * invDet;
            result.C2.X = d12 * invDet;
            result.C3.X = d13 * invDet;
            result.C4.X = d14 * invDet;
            result.C1.Y = -(m.C2.X * d3344 - m.C3.X * d3244 + m.C4.X * d3243) * invDet;
            result.C2.Y = (m.C1.X * d3344 - m.C3.X * d3144 + m.C4.X * d3143) * invDet;
            result.C3.Y = -(m.C1.X * d3244 - m.C2.X * d3144 + m.C4.X * d3142) * invDet;
            result.C4.Y = (m.C1.X * d3243 - m.C2.X * d3143 + m.C3.X * d3142) * invDet;
            float d2344 = m.C3.Y * m.C4.W - m.C4.Y * m.C3.W;
            float d2244 = m.C2.Y * m.C4.W - m.C4.Y * m.C2.W;
            float d2243 = m.C2.Y * m.C3.W - m.C3.Y * m.C2.W;
            float d2144 = m.C1.Y * m.C4.W - m.C4.Y * m.C1.W;
            float d2143 = m.C1.Y * m.C3.W - m.C3.Y * m.C1.W;
            float d2142 = m.C1.Y * m.C2.W - m.C2.Y * m.C1.W;
            result.C1.Z = (m.C2.X * d2344 - m.C3.X * d2244 + m.C4.X * d2243) * invDet;
            result.C2.Z = -(m.C1.X * d2344 - m.C3.X * d2144 + m.C4.X * d2143) * invDet;
            result.C3.Z = (m.C1.X * d2244 - m.C2.X * d2144 + m.C4.X * d2142) * invDet;
            result.C4.Z = -(m.C1.X * d2243 - m.C2.X * d2143 + m.C3.X * d2142) * invDet;
            float d2334 = m.C3.Y * m.C4.Z - m.C4.Y * m.C3.Z;
            float d2234 = m.C2.Y * m.C4.Z - m.C4.Y * m.C2.Z;
            float d2233 = m.C2.Y * m.C3.Z - m.C3.Y * m.C2.Z;
            float d2134 = m.C1.Y * m.C4.Z - m.C4.Y * m.C1.Z;
            float d2133 = m.C1.Y * m.C3.Z - m.C3.Y * m.C1.Z;
            float d2132 = m.C1.Y * m.C2.Z - m.C2.Y * m.C1.Z;
            result.C1.W = -(m.C2.X * d2334 - m.C3.X * d2234 + m.C4.X * d2233) * invDet;
            result.C2.W = (m.C1.X * d2334 - m.C3.X * d2134 + m.C4.X * d2133) * invDet;
            result.C3.W = -(m.C1.X * d2234 - m.C2.X * d2134 + m.C4.X * d2132) * invDet;
            result.C4.W = (m.C1.X * d2233 - m.C2.X * d2133 + m.C3.X * d2132) * invDet;
        }
        public static void InvertAndTransposeInPlace(ref Matrix4x4 m)
        {
            Matrix4x4 result;
            InvertAndTranspose(ref m, out result);
            m = result;
        }
        public static Matrix4x4 InvertAndTranspose(Matrix4x4 m)
        {
            Matrix4x4 result;
            InvertAndTranspose(ref m, out result);
            return result;
        }
        #endregion

        #endregion

        #region Constant Matrices
        public static Matrix4x4 Zero
        {
            get { return new Matrix4x4(); }
        }
        public static Matrix4x4 Identity
        {
            get
            {
                var result = new Matrix4x4();
                result.C1.X = result.C2.Y = result.C3.Z = result.C4.W = 1.0f;
                return result;
            }
        }
        #endregion

        #region Static Convertions
        public static void FromMatrix3x3(ref Matrix3x3 m, out Matrix4x4 result)
        {
            result.C1 = new Vector4 { X = m.C1.X, Y = m.C1.Y, Z = m.C1.Z, W = 0.0f };
            result.C2 = new Vector4 { X = m.C2.X, Y = m.C2.Y, Z = m.C2.Z, W = 0.0f };
            result.C3 = new Vector4 { X = m.C3.X, Y = m.C3.Y, Z = m.C3.Z, W = 0.0f };
            result.C4 = new Vector4 { W = 1.0f };
        }
        public static Matrix4x4 FromMatrix3x3(Matrix3x3 m)
        {
            Matrix4x4 result;
            FromMatrix3x3(ref m, out result);
            return result;
        }

        public static void FromMatrix4x3(ref Matrix4x3 m, out Matrix4x4 result)
        {
            result.C1 = m.C1;
            result.C2 = m.C2;
            result.C3 = m.C3;
            result.C4 = new Vector4 { W = 1.0f };
        }
        public static Matrix4x4 FromMatrix4x3(Matrix4x3 m)
        {
            Matrix4x4 result;
            FromMatrix4x3(ref m, out result);
            return result;
        }
        #endregion

        #region Typical transforms

        #region Identity
        public static void CreateIdentity(out Matrix4x4 result)
        {
            result = new Matrix4x4();
            result.C1.X = result.C2.Y = result.C3.Z = result.C4.W = 1.0f;
        }
        #endregion

        #region Scaling
        public static void CreateScaling(float scale, out Matrix4x4 result)
        {
            result = new Matrix4x4();
            result.C1.X = result.C2.Y = result.C3.Z = scale;
            result.C4.W = 1.0f;
        }
        public static Matrix4x4 CreateScaling(float scale)
        {
            Matrix4x4 result;
            CreateScaling(scale, out result);
            return result;
        }
        public static void CreateScaling(float sx, float sy, float sz, out Matrix4x4 result)
        {
            result = new Matrix4x4 { C1 = { X = sx }, C2 = { Y = sy }, C3 = { Z = sz }, C4 = { W = 1.0f } };
        }
        public static Matrix4x4 CreateScaling(float sx, float sy, float sz)
        {
            Matrix4x4 result;
            CreateScaling(sx, sy, sz, out result);
            return result;
        }
        public static void CreateScaling(ref Vector3 scale, out Matrix4x4 result)
        {
            result = new Matrix4x4 { C1 = { X = scale.X }, C2 = { Y = scale.Y }, C3 = { Z = scale.Z }, C4 = { W = 1.0f } };
        }
        public static Matrix4x4 CreateScaling(Vector3 scale)
        {
            Matrix4x4 result;
            CreateScaling(ref scale, out result);
            return result;
        }
        #endregion

        #region Rotation
        public static void CreateRotationX(float angle, out Matrix4x4 result)
        {
            float sina = MathHelper.Sin(angle);
            float cosa = MathHelper.Cos(angle);

            result = new Matrix4x4
            {
                C1 = {X = 1.0f},
                C2 = {Y = cosa, Z = -sina},
                C3 = {Y = sina, Z = cosa},
                C4 = {W = 1.0f}
            };
        }
        public static Matrix4x4 CreateRotationX(float angle)
        {
            Matrix4x4 result;
            CreateRotationX(angle, out result);
            return result;
        }

        public static void CreateRotationY(float angle, out Matrix4x4 result)
        {
            float sina = MathHelper.Sin(angle);
            float cosa = MathHelper.Cos(angle);

            result = new Matrix4x4
            {
                C1 = {X = cosa, Z = -sina},
                C2 = {Y = 1.0f},
                C3 = {X = sina, Z = cosa},
                C4 = {W = 1.0f}
            };
        }
        public static Matrix4x4 CreateRotationY(float angle)
        {
            Matrix4x4 result;
            CreateRotationY(angle, out result);
            return result;
        }

        public static void CreateRotationZ(float angle, out Matrix4x4 result)
        {
            float sina = MathHelper.Sin(angle);
            float cosa = MathHelper.Cos(angle);

            result = new Matrix4x4
            {
                C1 = {X = cosa, Y = -sina},
                C2 = {X = sina, Y = cosa},
                C3 = {Z = 1.0f},
                C4 = {W = 1.0f}
            };
        }
        public static Matrix4x4 CreateRotationZ(float angle)
        {
            Matrix4x4 result;
            CreateRotationZ(angle, out result);
            return result;
        }
        #endregion

        #region Translation
        public static void CreateTranslation(float tx, float ty, float tz, out Matrix4x4 result)
        {
            result = new Matrix4x4();
            result.C1.X = result.C2.Y = result.C3.Z = result.C4.W = 1.0f;
            result.C1.W = tx;
            result.C2.W = ty;
            result.C3.W = tz;
        }
        public static Matrix4x4 CreateTranslation(float tx, float ty, float tz)
        {
            Matrix4x4 result;
            CreateTranslation(tx, ty, tz, out result);
            return result;
        }

        public static void CreateTranslation(ref Vector3 translation, out Matrix4x4 result)
        {
            result = new Matrix4x4();
            result.C1.X = result.C2.Y = result.C3.Z = result.C4.W = 1.0f;
            result.C1.W = translation.X;
            result.C2.W = translation.Y;
            result.C3.W = translation.Z;
        }
        public static Matrix4x4 CreateTranslation(Vector3 translation)
        {
            Matrix4x4 result;
            CreateTranslation(ref translation, out result);
            return result;
        }
        #endregion

        #region LookAtRH
        public static void CreateLookAtRH(ref Vector3 cameraPosition, ref Vector3 cameraTarget, ref Vector3 cameraUpVector, out Matrix4x4 result)
        {
            Vector3 toEye;// = (cameraPosition - cameraTarget).Normalize();
            Vector3.Subtract(ref cameraPosition, ref cameraTarget, out toEye);
            Vector3.NormalizeInPlace(ref toEye);

            Vector3 left;// = Vector3.Cross(cameraUpVector, toEye).Normalize();
            Vector3.Cross(ref cameraUpVector, ref toEye, out left);
            Vector3.NormalizeInPlace(ref left);

            Vector3 exactUp;// = Vector3.Cross(toEye, left);
            Vector3.Cross(ref toEye, ref left, out exactUp);

            result = new Matrix4x4
            {
                C1 = new Vector4
                {
                    X = left.X,
                    Y = left.Y,
                    Z = left.Z,
                    W = -Vector3.Dot(ref left, ref cameraPosition)
                },
                C2 = new Vector4
                {
                    X = exactUp.X,
                    Y = exactUp.Y,
                    Z = exactUp.Z,
                    W = -Vector3.Dot(ref exactUp, ref cameraPosition)
                },
                C3 = new Vector4
                {
                    X = toEye.X,
                    Y = toEye.Y,
                    Z = toEye.Z,
                    W = -Vector3.Dot(ref toEye, ref cameraPosition)
                },
                C4 = new Vector4
                {
                    X = 0.0f,
                    Y = 0.0f,
                    Z = 0.0f,
                    W = 1.0f
                }
            };
        }
        public static Matrix4x4 CreateLookAtRH(Vector3 cameraPosition, Vector3 cameraTarget, Vector3 cameraUpVector)
        {
            Matrix4x4 result;
            CreateLookAtRH(ref cameraPosition, ref cameraTarget, ref cameraUpVector, out result);
            return result;
        }
        #endregion

        #region LookAtLH
        public static void CreateLookAtLH(ref Vector3 cameraPosition, ref Vector3 cameraTarget, ref Vector3 cameraUpVector, out Matrix4x4 result)
        {
            Vector3 toEye;// = (cameraPosition - cameraTarget).Normalize();
            Vector3.Subtract(ref cameraPosition, ref cameraTarget, out toEye);
            Vector3.NormalizeInPlace(ref toEye);


            Vector3 left;// = Vector3.Cross(cameraUpVector, toEye).Normalize();
            Vector3.Cross(ref cameraUpVector, ref toEye, out left);
            Vector3.NormalizeInPlace(ref left);

            Vector3 exactUp;// = Vector3.Cross(toEye, left);
            Vector3.Cross(ref toEye, ref left, out exactUp);

            result = new Matrix4x4
            {
                C1 = new Vector4
                {
                    X = left.X,
                    Y = left.Y,
                    Z = -left.Z,
                    W = -Vector3.Dot(ref left, ref cameraPosition)
                },
                C2 = new Vector4
                {
                    X = exactUp.X,
                    Y = exactUp.Y,
                    Z = -exactUp.Z,
                    W = -Vector3.Dot(ref exactUp, ref cameraPosition)
                },
                C3 = new Vector4
                {
                    X = toEye.X,
                    Y = toEye.Y,
                    Z = -toEye.Z,
                    W = -Vector3.Dot(ref toEye, ref cameraPosition)
                },
                C4 = new Vector4
                {
                    X = 0.0f,
                    Y = 0.0f,
                    Z = 0.0f,
                    W = 1.0f
                }
            };
        }
        public static Matrix4x4 CreateLookAtLH(Vector3 cameraPosition, Vector3 cameraTarget, Vector3 cameraUpVector)
        {
            Matrix4x4 result;
            CreateLookAtLH(ref cameraPosition, ref cameraTarget, ref cameraUpVector, out result);
            return result;
        }
        #endregion

        #region Orthorgraphic
        public static void CreateOrthorgraphic(float width, float height, float zNear, float zFar, out Matrix4x4 result)
        {
            result = new Matrix4x4
            {
                C1 = new Vector4
                {
                    X = 2f / width,
                    Y = 0.0f,
                    Z = 0.0f,
                    W = 0.0f
                },
                C2 = new Vector4
                {
                    X = 0.0f,
                    Y = 2f / height,
                    Z = 0.0f,
                    W = 0.0f
                },
                C3 = new Vector4
                {
                    X = 0.0f,
                    Y = 0.0f,
                    Z = 1.0f / (zNear - zFar),
                    W = zNear / (zNear - zFar),
                },
                C4 = new Vector4
                {
                    X = 0.0f,
                    Y = 0.0f,
                    Z = 0.0f,
                    W = 1.0f
                }
            };
        }
        public static Matrix4x4 CreateOrthorgraphic(float width, float height, float zNear, float zFar)
        {
            Matrix4x4 result;
            CreateOrthorgraphic(width, height, zNear, zFar, out result);
            return result;
        }
        #endregion

        #region PerspectiveFieldOfView
        public static void CreatePerspectiveFieldOfView(float fieldOfView, float aspectRatio, float zNear, float zFar, out Matrix4x4 result)
        {
            if (fieldOfView <= 0f || fieldOfView >= 3.14159274f) throw new ArgumentException("fieldOfView must be more than 0 and less than pi.");
            if (zNear <= 0f) throw new ArgumentException("zNear must be greater that zero.");
            if (zFar <= zNear) throw new ArgumentException("zFar must greater than zNear.");

            float a = 1f / MathHelper.Tan(fieldOfView * 0.5f);
            float b = a / aspectRatio;

            result = new Matrix4x4
            {
                C1 = new Vector4
                {
                    X = b,
                    Y = 0.0f,
                    Z = 0.0f,
                    W = 0.0f
                },
                C2 = new Vector4
                {
                    X = 0.0f,
                    Y = a,
                    Z = 0.0f,
                    W = 0.0f
                },
                C3 = new Vector4
                {
                    X = 0.0f,
                    Y = 0.0f,
                    Z = zFar / (zNear - zFar),
                    W = zNear * zFar / (zNear - zFar)
                },
                C4 = new Vector4
                {
                    X = 0.0f,
                    Y = 0.0f,
                    Z = -1.0f,
                    W = 0.0f
                }
            };
        }
        public static Matrix4x4 CreatePerspectiveFieldOfView(float fieldOfView, float aspectRatio, float zNear, float zFar)
        {
            Matrix4x4 result;
            CreatePerspectiveFieldOfView(fieldOfView, aspectRatio, zNear, zFar, out result);
            return result;
        }
        #endregion

        #endregion
    }
}
