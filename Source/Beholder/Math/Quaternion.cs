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
using System.Globalization;
using System.Runtime.InteropServices;

namespace Beholder.Math
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct Quaternion : IEquatable<Quaternion>
    {
        public float X, Y, Z, W;

        #region Constructors
        public Quaternion(float x, float y, float z, float w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        public Quaternion(Vector3 xyz, float w)
        {
            X = xyz.X;
            Y = xyz.Y;
            Z = xyz.Z;
            W = w;
        }

        public Quaternion(ref Vector3 xyz, float w)
        {
            X = xyz.X;
            Y = xyz.Y;
            Z = xyz.Z;
            W = w;
        }

        public Quaternion(Vector4 data)
        {
            X = data.X;
            Y = data.Y;
            Z = data.Z;
            W = data.W;
        }

        public Quaternion(ref Vector4 data)
        {
            X = data.X;
            Y = data.Y;
            Z = data.Z;
            W = data.W;
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

        public bool Equals(Quaternion v)
        {
            return X == v.X && Y == v.Y && Z == v.Z && W == v.W;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Quaternion))
            {
                return false;
            }

            Quaternion other = (Quaternion)obj;
            return Equals(ref this, ref other);
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() + Y.GetHashCode() << 2 + Z.GetHashCode() << 4 + W.GetHashCode() << 6;
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

        public static bool Equals(ref Quaternion v1, ref Quaternion v2)
        {
            return
                v1.X == v2.X &&
                v1.Y == v2.Y &&
                v1.Z == v2.Z &&
                v1.W == v2.W;
        }

        #endregion

        #region Properties

        public Quaternion Conjugate()
        {
            return new Quaternion { X = -X, Y = -Y, Z = -Z, W = W };
        }

        public float Norm()
        {
            return MathHelper.Sqrt(X * X + Y * Y + Z * Z + W * W);
        }

        public float NormSquared()
        {
            return X * X + Y * Y + Z * Z + W * W;
        }

        public Quaternion Normalize()
        {
            float invLength = 1.0f / MathHelper.Sqrt(X * X + Y * Y + Z * Z + W * W);
            return new Quaternion { X = X * invLength, Y = Y * invLength, Z = Z * invLength, W = W * invLength };
        }

        #endregion

        #region Convertions
        public Matrix3x3 ToMatrix3x3()
        {
            Matrix3x3 result;
            ToMatrix3x3(ref this, out result);
            return result;
        }

        public Matrix4x3 ToMatrix4x3()
        {
            Matrix4x3 result;
            ToMatrix4x3(ref this, out result);
            return result;
        }

        public Matrix4x4 ToMatrix4x4()
        {
            Matrix4x4 result;
            ToMatrix4x4(ref this, out result);
            return result;
        }
        #endregion

        #region Static Methods and Operators

        #region Norm
        public static float Norm(ref Quaternion q)
        {
            return MathHelper.Sqrt(q.X * q.X + q.Y * q.Y + q.Z * q.Z + q.W * q.W);
        }
        public static float Norm(Quaternion q)
        {
            return MathHelper.Sqrt(q.X * q.X + q.Y * q.Y + q.Z * q.Z + q.W * q.W);
        }
        #endregion

        #region Norm Squared
        public static float NormSquared(ref Quaternion q)
        {
            return q.X * q.X + q.Y * q.Y + q.Z * q.Z + q.W * q.W;
        }
        public static float NormSquared(Quaternion q)
        {
            return q.X * q.X + q.Y * q.Y + q.Z * q.Z + q.W * q.W;
        }
        #endregion

        #region Normalize
        public static void Normalize(ref Quaternion q, out Quaternion result)
        {
            float intLength = 1.0f / MathHelper.Sqrt(q.X * q.X + q.Y * q.Y + q.Z * q.Z + q.W * q.W);
            result.X = q.X * intLength;
            result.Y = q.Y * intLength;
            result.Z = q.Z * intLength;
            result.W = q.W * intLength;
        }
        public static void NormalizeInPlace(ref Quaternion q)
        {
            Normalize(ref q, out q);
        }
        public static Quaternion Normalize(Quaternion q)
        {
            Normalize(ref q, out q);
            return q;
        }
        #endregion

        #region Negate
        public static void Negate(ref Quaternion q, out Quaternion result)
        {
            result.X = -q.X;
            result.Y = -q.Y;
            result.Z = -q.Z;
            result.W = -q.W;
        }
        public static void NegateInPlace(ref Quaternion q)
        {
            Negate(ref q, out q);
        }
        public static Quaternion Negate(Quaternion q)
        {
            Negate(ref q, out q);
            return q;
        }
        public static Quaternion operator -(Quaternion q)
        {
            Negate(ref q, out q);
            return q;
        }
        #endregion

        #region Conjugate
        public static void Conjugate(ref Quaternion q, out Quaternion result)
        {
            result.X = -q.X;
            result.Y = -q.Y;
            result.Z = -q.Z;
            result.W = q.W;
        }
        public static void ConjugateInPlace(ref Quaternion q)
        {
            Conjugate(ref q, out q);
        }
        public static Quaternion Conjugate(Quaternion q)
        {
            Conjugate(ref q, out q);
            return q;
        }
        #endregion

        #region Add
        public static void Add(ref Quaternion left, ref Quaternion right, out Quaternion result)
        {
            result.X = left.X + right.X;
            result.Y = left.Y + right.Y;
            result.Z = left.Z + right.Z;
            result.W = left.W + right.W;
        }
        public static void AddInPlace(ref Quaternion leftAndResult, ref Quaternion right)
        {
            Add(ref leftAndResult, ref right, out leftAndResult);
        }
        public static Quaternion Add(Quaternion left, Quaternion right)
        {
            Add(ref left, ref right, out left);
            return left;
        }
        public static Quaternion operator +(Quaternion left, Quaternion right)
        {
            Add(ref left, ref right, out left);
            return left;
        }
        #endregion

        #region Subtract
        public static void Subtract(ref Quaternion left, ref Quaternion right, out Quaternion result)
        {
            result.X = left.X - right.X;
            result.Y = left.Y - right.Y;
            result.Z = left.Z - right.Z;
            result.W = left.W - right.W;
        }
        public static void SubtractInPlace(ref Quaternion leftAndResult, ref Quaternion right)
        {
            Subtract(ref leftAndResult, ref right, out leftAndResult);
        }
        public static Quaternion Subtract(Quaternion left, Quaternion right)
        {
            Subtract(ref left, ref right, out left);
            return left;
        }
        public static Quaternion operator -(Quaternion left, Quaternion right)
        {
            Subtract(ref left, ref right, out left);
            return left;
        }
        #endregion

        #region Multiply (scalar)
        public static void Multiply(ref Quaternion q, float scale, out Quaternion result)
        {
            result.X = q.X * scale;
            result.Y = q.Y * scale;
            result.Z = q.Z * scale;
            result.W = q.W * scale;
        }
        public static void MultiplyInPlace(ref Quaternion q, float scale)
        {
            Multiply(ref q, scale, out q);
        }
        public static Quaternion Multiply(Quaternion q, float scale)
        {
            Multiply(ref q, scale, out q);
            return q;
        }
        public static Quaternion operator *(float scale, Quaternion q)
        {
            Multiply(ref q, scale, out q);
            return q;
        }
        public static Quaternion operator *(Quaternion q, float scale)
        {
            Multiply(ref q, scale, out q);
            return q;
        }
        #endregion

        #region Multiply (quaternion)
        public static void Multiply(ref Quaternion left, ref Quaternion right, out Quaternion result)
        {
            float a = (left.W + left.X) * (right.W + right.X);
            float b = (left.Z - left.Y) * (right.Y - right.Z);
            float c = (left.X - left.W) * (right.Y + right.Z);
            float d = (left.Y + left.Z) * (right.X - right.W);
            float e = (left.X + left.Z) * (right.X + right.Y);
            float f = (left.X - left.Z) * (right.X - right.Y);
            float g = (left.W + left.Y) * (right.W - right.Z);
            float h = (left.W - left.Y) * (right.W + right.Z);

            result.X = a - (e + f + g + h) * 0.5f;
            result.Y = -c + (e - f + g - h) * 0.5f;
            result.Z = -d + (e - f - g + h) * 0.5f;
            result.W = b + (-e - f + g + h) * 0.5f;
        }
        public static void MultiplyInPlace(ref Quaternion leftAndResult, ref Quaternion right)
        {
            Multiply(ref leftAndResult, ref right, out leftAndResult);
        }
        public static Quaternion MultiplyInPlace(Quaternion left, Quaternion right)
        {
            Multiply(ref left, ref right, out left);
            return left;
        }
        public static Quaternion operator *(Quaternion left, Quaternion right)
        {
            Multiply(ref left, ref right, out left);
            return left;
        }
        #endregion

        #region ApplyRotation
        public static void ApplyRotation(ref Vector3 v, ref Quaternion q, out Vector3 result)
        {
            Vector3 a, b;

            a.X = q.Y * v.Z - q.Z * v.Y + q.W * v.X;
            a.Y = q.Z * v.X - q.X * v.Z + q.W * v.Y;
            a.Z = q.X * v.Y - q.Y * v.X + q.W * v.Z;

            b.X = q.Y * a.Z - q.Z * a.Y;
            b.Y = q.Z * a.X - q.X * a.Z;
            b.Z = q.X * a.Y - q.Y * a.X;

            result.X = v.X + 2.0f * b.X;
            result.Y = v.Y + 2.0f * b.Y;
            result.Z = v.Z + 2.0f * b.Z;
        }
        public static void ApplyRotationInPlace(ref Vector3 v, ref Quaternion q)
        {
            ApplyRotation(ref v, ref q, out v);
        }
        public static Vector3 ApplyRotation(Vector3 v, Quaternion q)
        {
            ApplyRotation(ref v, ref q, out v);
            return v;
        }
        #endregion

        #region Dot
        public static float Dot(ref Quaternion left, ref Quaternion right)
        {
            return left.X * right.X + left.Y * right.Y + left.Z * right.Z + left.W * right.W;
        }
        public static float Dot(Quaternion left, Quaternion right)
        {
            return left.X * right.X + left.Y * right.Y + left.Z * right.Z + left.W * right.W;
        }
        #endregion

        #region Lerp
        public static void Lerp(ref Quaternion left, ref Quaternion right, float amount, out Quaternion result)
        {
            result.X = left.X - amount * (left.X - right.X);
            result.Y = left.Y - amount * (left.Y - right.Y);
            result.Z = left.Z - amount * (left.Z - right.Z);
            result.W = left.W - amount * (left.W - right.W);
        }
        public static void LerpInPlace(ref Quaternion leftAndResult, ref Quaternion right, float amount)
        {
            Lerp(ref leftAndResult, ref right, amount, out leftAndResult);
        }
        public static Quaternion Lerp(Quaternion left, Quaternion right, float amount)
        {
            Lerp(ref left, ref right, amount, out left);
            return left;
        }
        #endregion

        #region Nlerp
        public static void Nlerp(ref Quaternion left, ref Quaternion right, float amount, out Quaternion result)
        {
            result.X = left.X - amount * (left.X - right.X);
            result.Y = left.Y - amount * (left.Y - right.Y);
            result.Z = left.Z - amount * (left.Z - right.Z);
            result.W = left.W - amount * (left.W - right.W);
            float invLength = 1.0f / MathHelper.Sqrt(result.X * result.X + result.Y * result.Y + result.Z * result.Z + result.W * result.W);
            result.X *= invLength;
            result.Y *= invLength;
            result.Z *= invLength;
            result.W *= invLength;
        }
        public static void NlerpInPlace(ref Quaternion leftAndResult, ref Quaternion right, float amount)
        {
            Nlerp(ref leftAndResult, ref right, amount, out leftAndResult);
        }
        public static Quaternion Nlerp(Quaternion left, Quaternion right, float amount)
        {
            Nlerp(ref left, ref right, amount, out left);
            return left;
        }
        #endregion

        #region SLerp
        public static void Slerp(ref Quaternion left, ref Quaternion right, float amount, out Quaternion result)
        {
            float cosTheta = Dot(ref left, ref right);

            if (cosTheta < -1f) cosTheta = -1f;
            else if (cosTheta > 1f) cosTheta = 1f;

            float theta = MathHelper.Acos(cosTheta);

            float sinTheta = MathHelper.Sin(theta);
            float invSinTheta = 1.0f / sinTheta;

            float wLeft = MathHelper.Sin((1.0f - amount) * theta) * invSinTheta;
            float wRight = MathHelper.Sin(amount * theta) * invSinTheta;

            if (cosTheta < 0f) wRight = -wRight;

            result.X = wLeft * left.X + wRight * right.X;
            result.Y = wLeft * left.Y + wRight * right.Y;
            result.Z = wLeft * left.Z + wRight * right.Z;
            result.W = wLeft * left.W + wRight * right.W;
        }
        public static void SlerpInPlace(ref Quaternion leftAndResult, ref Quaternion right, float amount)
        {
            Slerp(ref leftAndResult, ref right, amount, out leftAndResult);
        }
        public static Quaternion Slerp(Quaternion left, Quaternion right, float amount)
        {
            Slerp(ref left, ref right, amount, out left);
            return left;
        }
        #endregion

        #endregion

        #region Static Convertions
        public static void ToMatrix3x3(ref Quaternion q, out Matrix3x3 result)
        {
            float x2 = q.X + q.X;
            float y2 = q.Y + q.Y;
            float z2 = q.Z + q.Z;

            {
                float xx2 = q.X * x2;
                float yy2 = q.Y * y2;
                float zz2 = q.Z * z2;
                result.C1.X = 1.0f - yy2 - zz2;
                result.C2.Y = 1.0f - xx2 - zz2;
                result.C3.Z = 1.0f - xx2 - yy2;
            }
            {
                float yz2 = q.Y * z2;
                float wx2 = q.W * x2;
                result.C2.Z = yz2 - wx2;
                result.C3.Y = yz2 + wx2;
            }
            {
                float xy2 = q.X * y2;
                float wz2 = q.W * z2;
                result.C1.Y = xy2 - wz2;
                result.C2.X = xy2 + wz2;
            }
            {
                float xz2 = q.X * z2;
                float wy2 = q.W * y2;
                result.C3.X = xz2 - wy2;
                result.C1.Z = xz2 + wy2;
            }
        }
        public static Matrix3x3 ToMatrix3x3(Quaternion q)
        {
            Matrix3x3 result;
            ToMatrix3x3(ref q, out result);
            return result;
        }

        public static void ToMatrix4x3(ref Quaternion q, out Matrix4x3 result)
        {
            float x2 = q.X + q.X;
            float y2 = q.Y + q.Y;
            float z2 = q.Z + q.Z;

            {
                float xx2 = q.X * x2;
                float yy2 = q.Y * y2;
                float zz2 = q.Z * z2;
                result.C1.X = 1.0f - yy2 - zz2;
                result.C2.Y = 1.0f - xx2 - zz2;
                result.C3.Z = 1.0f - xx2 - yy2;
            }
            {
                float yz2 = q.Y * z2;
                float wx2 = q.W * x2;
                result.C2.Z = yz2 - wx2;
                result.C3.Y = yz2 + wx2;
            }
            {
                float xy2 = q.X * y2;
                float wz2 = q.W * z2;
                result.C1.Y = xy2 - wz2;
                result.C2.X = xy2 + wz2;
            }
            {
                float xz2 = q.X * z2;
                float wy2 = q.W * y2;
                result.C3.X = xz2 - wy2;
                result.C1.Z = xz2 + wy2;
            }

            result.C1.W = result.C2.W = result.C3.W = 0.0f;
        }
        public static Matrix4x3 ToMatrix4x3(Quaternion q)
        {
            Matrix4x3 result;
            ToMatrix4x3(ref q, out result);
            return result;
        }

        public static void ToMatrix4x4(ref Quaternion q, out Matrix4x4 result)
        {
            float x2 = q.X + q.X;
            float y2 = q.Y + q.Y;
            float z2 = q.Z + q.Z;

            {
                float xx2 = q.X * x2;
                float yy2 = q.Y * y2;
                float zz2 = q.Z * z2;
                result.C1.X = 1.0f - yy2 - zz2;
                result.C2.Y = 1.0f - xx2 - zz2;
                result.C3.Z = 1.0f - xx2 - yy2;
            }
            {
                float yz2 = q.Y * z2;
                float wx2 = q.W * x2;
                result.C2.Z = yz2 - wx2;
                result.C3.Y = yz2 + wx2;
            }
            {
                float xy2 = q.X * y2;
                float wz2 = q.W * z2;
                result.C1.Y = xy2 - wz2;
                result.C2.X = xy2 + wz2;
            }
            {
                float xz2 = q.X * z2;
                float wy2 = q.W * y2;
                result.C3.X = xz2 - wy2;
                result.C1.Z = xz2 + wy2;
            }

            result.C1.W = result.C2.W = result.C3.W = 0.0f;
            result.C4 = new Vector4 { W = 1.0f };
        }
        public static Matrix4x4 ToMatrix4x4(Quaternion q)
        {
            Matrix4x4 result;
            ToMatrix4x4(ref q, out result);
            return result;
        }
        #endregion

        #region Typical Rotations

        public static void CreateIdentity(out Quaternion result)
        {
            result = new Quaternion { W = 1.0f };
        }

        public static void CreateRotationX(float angle, out Quaternion result)
        {
            angle *= 0.5f;
            float sina = MathHelper.Sin(angle);
            float cosa = MathHelper.Cos(angle);

            result = new Quaternion { X = sina, W = cosa };
        }
        public static Quaternion CreateRotationX(float angle)
        {
            Quaternion result;
            CreateRotationX(angle, out result);
            return result;
        }

        public static void CreateRotationY(float angle, out Quaternion result)
        {
            angle *= 0.5f;
            float sina = MathHelper.Sin(angle);
            float cosa = MathHelper.Cos(angle);

            result = new Quaternion {Y = sina, W = cosa };
        }
        public static Quaternion CreateRotationY(float angle)
        {
            Quaternion result;
            CreateRotationY(angle, out result);
            return result;
        }

        public static void CreateRotationZ(float angle, out Quaternion result)
        {
            angle *= 0.5f;
            float sina = MathHelper.Sin(angle);
            float cosa = MathHelper.Cos(angle);

            result = new Quaternion { Z = sina, W = cosa };
        }
        public static Quaternion CreateRotationZ(float angle)
        {
            Quaternion result;
            CreateRotationZ(angle, out result);
            return result;
        }

        public static void CreateRotationAxis(Vector3 axis, float angle, out Quaternion result)
        {
            Vector3.NormalizeInPlace(ref axis);
            angle *= 0.5f;
            float sina = MathHelper.Sin(angle);
            float cosa = MathHelper.Cos(angle);

            result = new Quaternion { X = axis.X * sina, Y = axis.Y * sina, Z = axis.Z * sina, W = cosa };
        }
        public static Quaternion CreateRotationAxis(Vector3 axis, float angle)
        {
            Quaternion result;
            CreateRotationAxis(axis, angle, out result);
            return result;
        }

        #endregion

        #region Constant Quaternions
        public static Quaternion Zero
        {
            get { return new Quaternion(); }
        }

        public static Quaternion Identity 
        {
            get { return new Quaternion { W = 1.0f }; } 
        }
        #endregion
    }
}
