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
using System.Text;

namespace Beholder.Math
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct Matrix4x3 : IEquatable<Matrix4x3>
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

        #region Constructors
        public Matrix4x3(ref Vector4 c1, ref Vector4 c2, ref Vector4 c3)
        {
            C1 = c1;
            C2 = c2;
            C3 = c3;
        }
        public Matrix4x3(Vector4 c1, Vector4 c2, Vector4 c3) : this(ref c1, ref c2, ref c3) { }

        public Matrix4x3(ref Matrix3x3 matrix3x3)
        {
            FromMatrix3x3(ref matrix3x3, out this);
        }
        public Matrix4x3(Matrix3x3 matrix3x3) : this(ref matrix3x3) { }
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
                            case 0: return 0.0f;
                            case 1: return 0.0f;
                            case 2: return 0.0f;
                            case 3: return 1.0f;
                            default: throw new IndexOutOfRangeException();
                        }
                    default: throw new IndexOutOfRangeException();
                }
            }
        }

        #region Object Overrides
        public bool Equals(Matrix4x3 other)
        {
            return
                C1.X == other.C1.X && C1.Y == other.C1.Y && C1.Z == other.C1.Z && C1.W == other.C1.W &&
                C2.X == other.C2.X && C2.Y == other.C2.Y && C2.Z == other.C2.Z && C2.W == other.C2.W &&
                C3.X == other.C3.X && C3.Y == other.C3.Y && C3.Z == other.C3.Z && C3.W == other.C3.W; ;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Matrix4x3)) return false;
            Matrix4x3 other = (Matrix4x3)obj;
            return Equals(ref this, ref other);
        }

        public override int GetHashCode()
        {
            return
                C1.X.GetHashCode() + C1.Y.GetHashCode() << 3 + C1.Z.GetHashCode() << 6 + C1.W.GetHashCode() << 8 +
                C2.X.GetHashCode() << 11 + C2.Y.GetHashCode() << 14 + C2.Z.GetHashCode() << 16 + C2.W.GetHashCode() << 19 +
                C3.X.GetHashCode() << 22 + C3.Y.GetHashCode() << 24 + C3.Z.GetHashCode() << 27 + C3.W.GetHashCode() << 30;
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

        public static bool Equals(ref Matrix4x3 m1, ref Matrix4x3 m2)
        {
            return
                m1.C1.X == m2.C1.X && m1.C1.Y == m2.C1.Y && m1.C1.Z == m2.C1.Z && m1.C1.W == m2.C1.W &&
                m1.C2.X == m2.C2.X && m1.C2.Y == m2.C2.Y && m1.C2.Z == m2.C2.Z && m1.C2.W == m2.C2.W &&
                m1.C3.X == m2.C3.X && m1.C3.Y == m2.C3.Y && m1.C3.Z == m2.C3.Z && m1.C3.W == m2.C3.W;
        }
        #endregion

        #region Static Methods and Operators

        #region Determinant
        public static float Determinant(ref Matrix4x3 m)
        {
            return
                m.C1.X * (m.C2.Y * m.C3.Z - m.C3.Y * m.C2.Z) -
                m.C2.X * (m.C1.Y * m.C3.Z - m.C3.Y * m.C1.Z) +
                m.C3.X * (m.C1.Y * m.C2.Z - m.C2.Y * m.C1.Z);
        }
        public static float Determinant(Matrix4x3 m)
        {
            return Determinant(ref m);
        }
        #endregion

        #region Negate
        public static void Negate(ref Matrix4x3 m, out Matrix4x3 result)
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
        }
        public static void NegateInPlace(ref Matrix4x3 m)
        {
            Negate(ref m, out m);
        }
        public static Matrix4x3 Negate(Matrix4x3 m)
        {
            Negate(ref m, out m);
            return m;
        }
        public static Matrix4x3 operator -(Matrix4x3 m)
        {
            Negate(ref m, out m);
            return m;
        }
        #endregion

        #region Add
        public static void Add(ref Matrix4x3 left, ref Matrix4x3 right, out Matrix4x3 result)
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
        }
        public static void AddInPlace(ref Matrix4x3 leftAndResult, ref Matrix4x3 right)
        {
            Add(ref leftAndResult, ref right, out leftAndResult);
        }
        public static Matrix4x3 Add(Matrix4x3 left, Matrix4x3 right)
        {
            Add(ref left, ref right, out left);
            return left;
        }
        public static Matrix4x3 operator +(Matrix4x3 left, Matrix4x3 right)
        {
            Add(ref left, ref right, out left);
            return left;
        }
        #endregion

        #region Subtract
        public static void Subtract(ref Matrix4x3 left, ref Matrix4x3 right, out Matrix4x3 result)
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
        }
        public static void SubtractInPlace(ref Matrix4x3 leftAndResult, ref Matrix4x3 right)
        {
            Subtract(ref leftAndResult, ref right, out leftAndResult);
        }
        public static Matrix4x3 Subtract(Matrix4x3 left, Matrix4x3 right)
        {
            Subtract(ref left, ref right, out left);
            return left;
        }
        public static Matrix4x3 operator -(Matrix4x3 left, Matrix4x3 right)
        {
            Subtract(ref left, ref right, out left);
            return left;
        }
        #endregion

        #region Multiply (scalar)
        public static void Multiply(ref Matrix4x3 m, float scale, out Matrix4x3 result)
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
        }
        public static void MultiplyInPlace(ref Matrix4x3 m, float scale)
        {
            Multiply(ref m, scale, out m);
        }
        public static Matrix4x3 Multiply(Matrix4x3 m, float scale)
        {
            Multiply(ref m, scale, out m);
            return m;
        }
        public static Matrix4x3 operator *(float scale, Matrix4x3 m)
        {
            Multiply(ref m, scale, out m);
            return m;
        }
        public static Matrix4x3 operator *(Matrix4x3 m, float scale)
        {
            Multiply(ref m, scale, out m);
            return m;
        }
        #endregion

        #region Multiply (vector)
        public static void Multiply(ref Vector4 v, ref Matrix4x3 m, out Vector4 result)
        {
            result.X = v.X * m.C1.X + v.Y * m.C1.Y + v.Z * m.C1.Z + v.W * m.C1.W;
            result.Y = v.X * m.C2.X + v.Y * m.C2.Y + v.Z * m.C2.Z + v.W * m.C2.W;
            result.Z = v.X * m.C3.X + v.Y * m.C3.Y + v.Z * m.C3.Z + v.W * m.C3.W;
            result.W = v.W;
        }
        public static void MultiplyInPlace(ref Vector4 v, ref Matrix4x3 m)
        {
            Vector4 result;
            Multiply(ref v, ref m, out result);
            v = result;
        }
        public static Vector4 Multiply(Vector4 v, Matrix4x3 m)
        {
            Vector4 result;
            Multiply(ref v, ref m, out result);
            return result;
        }
        public static Vector4 operator *(Vector4 v, Matrix4x3 m)
        {
            Vector4 result;
            Multiply(ref v, ref m, out result);
            return result;
        }
        #endregion

        #region Multiply (matrix)
        public static void Multiply(ref Matrix4x3 left, ref Matrix4x3 right, out Matrix4x3 result)
        {
            result.C1.X = (left.C1.X * right.C1.X) + (left.C2.X * right.C1.Y) + (left.C3.X * right.C1.Z);
            result.C1.Y = (left.C1.Y * right.C1.X) + (left.C2.Y * right.C1.Y) + (left.C3.Y * right.C1.Z);
            result.C1.Z = (left.C1.Z * right.C1.X) + (left.C2.Z * right.C1.Y) + (left.C3.Z * right.C1.Z);
            result.C1.W = (left.C1.W * right.C1.X) + (left.C2.W * right.C1.Y) + (left.C3.W * right.C1.Z) + right.C1.W;

            result.C2.X = (left.C1.X * right.C2.X) + (left.C2.X * right.C2.Y) + (left.C3.X * right.C2.Z);
            result.C2.Y = (left.C1.Y * right.C2.X) + (left.C2.Y * right.C2.Y) + (left.C3.Y * right.C2.Z);
            result.C2.Z = (left.C1.Z * right.C2.X) + (left.C2.Z * right.C2.Y) + (left.C3.Z * right.C2.Z);
            result.C2.W = (left.C1.W * right.C2.X) + (left.C2.W * right.C2.Y) + (left.C3.W * right.C2.Z) + right.C2.W;

            result.C3.X = (left.C1.X * right.C3.X) + (left.C2.X * right.C3.Y) + (left.C3.X * right.C3.Z);
            result.C3.Y = (left.C1.Y * right.C3.X) + (left.C2.Y * right.C3.Y) + (left.C3.Y * right.C3.Z);
            result.C3.Z = (left.C1.Z * right.C3.X) + (left.C2.Z * right.C3.Y) + (left.C3.Z * right.C3.Z);
            result.C3.W = (left.C1.W * right.C3.X) + (left.C2.W * right.C3.Y) + (left.C3.W * right.C3.Z) + right.C3.W;
        }
        public static void MultiplyInPlace(ref Matrix4x3 leftAndResult, ref Matrix4x3 right)
        {
            Matrix4x3 result;
            Multiply(ref leftAndResult, ref right, out result);
            leftAndResult = result;
        }
        public static Matrix4x3 Multiply(Matrix4x3 left, Matrix4x3 right)
        {
            Matrix4x3 result;
            Multiply(ref left, ref right, out result);
            return result;
        }
        public static Matrix4x3 operator *(Matrix4x3 left, Matrix4x3 right)
        {
            Matrix4x3 result;
            Multiply(ref left, ref right, out result);
            return result;
        }
        #endregion

        #region Invert
        public static void Invert(ref Matrix4x3 m, out Matrix4x3 result)
        {
            float d3344 = m.C3.Z;
            float d3244 = m.C2.Z;
            float d3243 = m.C2.Z * m.C3.W - m.C3.Z * m.C2.W;
            float d3144 = m.C1.Z;
            float d3143 = m.C1.Z * m.C3.W - m.C3.Z * m.C1.W;
            float d3142 = m.C1.Z * m.C2.W - m.C2.Z * m.C1.W;
            float d11 = m.C2.Y * d3344 - m.C3.Y * d3244;
            float d12 = -(m.C1.Y * d3344 - m.C3.Y * d3144);
            float d13 = m.C1.Y * d3244 - m.C2.Y * d3144;
            float d14 = -(m.C1.Y * d3243 - m.C2.Y * d3143 + m.C3.Y * d3142);
            float invDet = 1f / (m.C1.X * d11 + m.C2.X * d12 + m.C3.X * d13);
            result.C1.X = d11 * invDet;
            result.C1.Y = d12 * invDet;
            result.C1.Z = d13 * invDet;
            result.C1.W = d14 * invDet;
            result.C2.X = -(m.C2.X * d3344 - m.C3.X * d3244) * invDet;
            result.C2.Y = (m.C1.X * d3344 - m.C3.X * d3144) * invDet;
            result.C2.Z = -(m.C1.X * d3244 - m.C2.X * d3144) * invDet;
            result.C2.W = (m.C1.X * d3243 - m.C2.X * d3143 + m.C3.X * d3142) * invDet;
            float d2344 = m.C3.Y;
            float d2244 = m.C2.Y;
            float d2243 = m.C2.Y * m.C3.W - m.C3.Y * m.C2.W;
            float d2144 = m.C1.Y;
            float d2143 = m.C1.Y * m.C3.W - m.C3.Y * m.C1.W;
            float d2142 = m.C1.Y * m.C2.W - m.C2.Y * m.C1.W;
            result.C3.X = (m.C2.X * d2344 - m.C3.X * d2244) * invDet;
            result.C3.Y = -(m.C1.X * d2344 - m.C3.X * d2144) * invDet;
            result.C3.Z = (m.C1.X * d2244 - m.C2.X * d2144) * invDet;
            result.C3.W = -(m.C1.X * d2243 - m.C2.X * d2143 + m.C3.X * d2142) * invDet;
        }
        public static void InvertInPlace(ref Matrix4x3 m)
        {
            Matrix4x3 result;
            Invert(ref m, out result);
            m = result;
        }
        public static Matrix4x3 Invert(Matrix4x3 m)
        {
            Matrix4x3 result;
            Invert(ref m, out result);
            return result;
        }
        #endregion

        #region Invert And Transpose
        public static void InvertAndTranspose(ref Matrix4x3 m, out Matrix4x3 result)
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
            result.C1.W = 0f;
            result.C2.W = 0f;
            result.C3.W = 0f;
        }
        public static void InvertAndTransposeInPlace(ref Matrix4x3 m)
        {
            Matrix4x3 result;
            InvertAndTranspose(ref m, out result);
            m = result;
        }
        public static Matrix4x3 InvertAndTranspose(Matrix4x3 m)
        {
            Matrix4x3 result;
            InvertAndTranspose(ref m, out result);
            return result;
        }
        #endregion

        #endregion

        #region Convertions
        public static void FromMatrix3x3(ref Matrix3x3 m, out Matrix4x3 result)
        {
            result.C1 = new Vector4 { X = m.C1.X, Y = m.C1.Y, Z = m.C1.Z, W = 0.0f };
            result.C2 = new Vector4 { X = m.C2.X, Y = m.C2.Y, Z = m.C2.Z, W = 0.0f };
            result.C3 = new Vector4 { X = m.C3.X, Y = m.C3.Y, Z = m.C3.Z, W = 0.0f };
        }
        public static Matrix4x3 FromMatrix3x3(Matrix3x3 m)
        {
            Matrix4x3 result;
            FromMatrix3x3(ref m, out result);
            return result;
        }
        #endregion

        #region Constant Matrices
        public static Matrix4x3 Zero
        {
            get { return new Matrix4x3(); }
        }
        public static Matrix4x3 Identity
        {
            get
            {
                var result = new Matrix4x3();
                result.C1.X = result.C2.Y = result.C3.Z = 1.0f;
                return result;
            }
        }
        #endregion

        #region Typical transforms

        #region Identity
        public static void CreateIdentity(out Matrix4x3 result)
        {
            result = new Matrix4x3();
            result.C1.X = result.C2.Y = result.C3.Z = 1.0f;
        }
        #endregion

        #region Scaling
        public static void CreateScaling(float scale, out Matrix4x3 result)
        {
            result = new Matrix4x3();
            result.C1.X = result.C2.Y = result.C3.Z = scale;
        }
        public static Matrix4x3 CreateScaling(float scale)
        {
            Matrix4x3 result;
            CreateScaling(scale, out result);
            return result;
        }
        public static void CreateScaling(float sx, float sy, float sz, out Matrix4x3 result)
        {
            result = new Matrix4x3 { C1 = { X = sx }, C2 = { Y = sy }, C3 = { Z = sz } };
        }
        public static Matrix4x3 CreateScaling(float sx, float sy, float sz)
        {
            Matrix4x3 result;
            CreateScaling(sx, sy, sz, out result);
            return result;
        }
        public static void CreateScaling(ref Vector3 scale, out Matrix4x3 result)
        {
            result = new Matrix4x3 { C1 = { X = scale.X }, C2 = { Y = scale.Y }, C3 = { Z = scale.Z } };
        }
        public static Matrix4x3 CreateScaling(Vector3 scale)
        {
            Matrix4x3 result;
            CreateScaling(ref scale, out result);
            return result;
        }
        #endregion

        #region Rotation
        public static void CreateRotationX(float angle, out Matrix4x3 result)
        {
            float sina = MathHelper.Sin(angle);
            float cosa = MathHelper.Cos(angle);

            result = new Matrix4x3 
            {
                C1 = {X = 1.0f}, 
                C2 = {Y = cosa, Z = -sina}, 
                C3 = {Y = sina, Z = cosa}
            };
        }
        public static Matrix4x3 CreateRotationX(float angle)
        {
            Matrix4x3 result;
            CreateRotationX(angle, out result);
            return result;
        }

        public static void CreateRotationY(float angle, out Matrix4x3 result)
        {
            float sina = MathHelper.Sin(angle);
            float cosa = MathHelper.Cos(angle);

            result = new Matrix4x3 
            {
                C1 = {X = cosa, Z = -sina}, 
                C2 = {Y = 1.0f}, 
                C3 = {X = sina, Z = cosa}
            };
        }
        public static Matrix4x3 CreateRotationY(float angle)
        {
            Matrix4x3 result;
            CreateRotationY(angle, out result);
            return result;
        }

        public static void CreateRotationZ(float angle, out Matrix4x3 result)
        {
            float sina = MathHelper.Sin(angle);
            float cosa = MathHelper.Cos(angle);

            result = new Matrix4x3 
            {
                C1 = {X = cosa, Y = -sina}, 
                C2 = {X = sina, Y = cosa}, 
                C3 = {Z = 1.0f}
            };
        }
        public static Matrix4x3 CreateRotationZ(float angle)
        {
            Matrix4x3 result;
            CreateRotationZ(angle, out result);
            return result;
        }
        #endregion

        #region Translation
        public static void CreateTranslation(float tx, float ty, float tz, out Matrix4x3 result)
        {
            result = new Matrix4x3();
            result.C1.X = result.C2.Y = result.C3.Z = 1.0f;
            result.C1.W = tx;
            result.C2.W = ty;
            result.C3.W = tz;
        }
        public static Matrix4x3 CreateTranslation(float tx, float ty, float tz)
        {
            Matrix4x3 result;
            CreateTranslation(tx, ty, tz, out result);
            return result;
        }

        public static void CreateTranslation(ref Vector3 translation, out Matrix4x3 result)
        {
            result = new Matrix4x3();
            result.C1.X = result.C2.Y = result.C3.Z = 1.0f;
            result.C1.W = translation.X;
            result.C2.W = translation.Y;
            result.C3.W = translation.Z;
        }
        public static Matrix4x3 CreateTranslation(Vector3 translation)
        {
            Matrix4x3 result;
            CreateTranslation(ref translation, out result);
            return result;
        }
        #endregion

        #endregion
    }
}
