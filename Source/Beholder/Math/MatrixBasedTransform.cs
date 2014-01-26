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
using System.Globalization;
using System.Text;

namespace Beholder.Math
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct MatrixBasedTransform : IEquatable<MatrixBasedTransform>
    {
        /// <summary>
        /// Three-dimentional translation.
        /// </summary>
        public Vector3 Translation;

        /// <summary>
        /// Uniform scaling.
        /// </summary>
        public float Scaling;

        /// <summary>
        /// Rotation matrix.
        /// </summary>
        public Matrix3x3 Rotation;
        
        public MatrixBasedTransform(float scaling, Matrix3x3 rotation, Vector3 translation)
        {
            Scaling = scaling;
            Rotation = rotation;
            Translation = translation;
        }

        public MatrixBasedTransform(float scaling, ref Matrix3x3 rotation, ref Vector3 translation)
        {
            Scaling = scaling;
            Rotation = rotation;
            Translation = translation;
        }

        #region Object Overrides
        public bool Equals(MatrixBasedTransform other)
        {
            return
                Scaling == other.Scaling &&
                Matrix3x3.Equals(ref Rotation, ref other.Rotation) &&
                Vector3.Equals(ref Translation, ref other.Translation);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is MatrixBasedTransform))
            {
                return false;
            }
            
            MatrixBasedTransform other = (MatrixBasedTransform) obj;
            return Equals(ref this, ref other);
        }

        public override int GetHashCode()
        {
            return
                Scaling.GetHashCode() +
                Rotation.C1.X.GetHashCode() << 3 + Rotation.C1.Y.GetHashCode() << 5 + Rotation.C1.Z.GetHashCode() << 8 +
                Rotation.C2.X.GetHashCode() << 9 + Rotation.C2.Y.GetHashCode() << 11 + Rotation.C2.Z.GetHashCode() << 14 +
                Rotation.C3.X.GetHashCode() << 17 + Rotation.C3.Y.GetHashCode() << 19 + Rotation.C3.Z.GetHashCode() << 22 +
                Translation.X.GetHashCode() << 25 + Translation.Y.GetHashCode() << 27 + Translation.Z.GetHashCode() << 30;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder(128);
            builder.Append("{ Scaling:");
            builder.Append(Scaling.ToString(CultureInfo.InvariantCulture));
            builder.Append("; Rotation:");
            builder.Append(Rotation.ToString());
            builder.Append("; Translation:");
            builder.Append(Translation.ToString());
            builder.Append("; }");

            return builder.ToString();
        }

        public static bool Equals(ref MatrixBasedTransform t1, ref MatrixBasedTransform t2)
        {
            return
                t1.Scaling == t2.Scaling &&
                Matrix3x3.Equals(ref t1.Rotation, ref t2.Rotation) &&
                Vector3.Equals(ref t1.Translation, ref t2.Translation);
        }
        #endregion

        #region Convertions
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

        #region Static Methods

        #region Combine
        public static void Combine(ref MatrixBasedTransform left, ref MatrixBasedTransform right, out MatrixBasedTransform result)
        {
            result.Scaling = left.Scaling * right.Scaling;
            Matrix3x3.Multiply(ref left.Rotation, ref right.Rotation, out result.Rotation);
            Matrix3x3.Multiply(ref left.Translation, ref right.Rotation, out result.Translation);
            Vector3.MultiplyInPlace(ref result.Translation, right.Scaling);
            Vector3.AddInPlace(ref result.Translation, ref right.Translation);
        }
        public static void CombineInPlace(ref MatrixBasedTransform leftAndResult, ref MatrixBasedTransform right)
        {
            Combine(ref leftAndResult, ref right, out leftAndResult);
        }
        public static MatrixBasedTransform Combine(MatrixBasedTransform left, MatrixBasedTransform right)
        {
            Combine(ref left, ref right, out left);
            return left;
        }
        public static MatrixBasedTransform operator *(MatrixBasedTransform left, MatrixBasedTransform right)
        {
            Combine(ref left, ref right, out left);
            return left;
        }
        #endregion

        #region Invert
        public static void Invert(ref MatrixBasedTransform t, out MatrixBasedTransform result)
        {
            result.Scaling = 1.0f / t.Scaling;
            Matrix3x3.Transpose(ref t.Rotation, out result.Rotation);
            Vector3.Multiply(ref t.Translation, result.Scaling, out result.Translation);
            Matrix3x3.MultiplyInPlace(ref result.Translation, ref result.Rotation);
        }
        public static void InvertInPlace(ref MatrixBasedTransform t)
        {
            Invert(ref t, out t);
        }
        public static MatrixBasedTransform Invert(MatrixBasedTransform t)
        {
            Invert(ref t, out t);
            return t;
        }
        #endregion

        #endregion

        #region Static Convertions
        public static void ToMatrix4x3(ref MatrixBasedTransform t, out Matrix4x3 m)
        {
            Matrix3x3 scalingAndRotation;
            Matrix3x3.Multiply(ref t.Rotation, t.Scaling, out scalingAndRotation);
            Matrix4x3.FromMatrix3x3(ref scalingAndRotation, out m);
            m.C1.W = t.Translation.X;
            m.C2.W = t.Translation.Y;
            m.C3.W = t.Translation.Z;
        }
        public static Matrix4x3 ToMatrix4x3(MatrixBasedTransform t)
        {
            Matrix4x3 result;
            ToMatrix4x3(ref t, out result);
            return result;
        }

        public static void ToMatrix4x4(ref MatrixBasedTransform t, out Matrix4x4 m)
        {
            Matrix3x3 scalingAndRotation;
            Matrix3x3.Multiply(ref t.Rotation, t.Scaling, out scalingAndRotation);
            Matrix4x4.FromMatrix3x3(ref scalingAndRotation, out m);
            m.C1.W = t.Translation.X;
            m.C2.W = t.Translation.Y;
            m.C3.W = t.Translation.Z;
        }
        public static Matrix4x4 ToMatrix4x4(MatrixBasedTransform t)
        {
            Matrix4x4 result;
            ToMatrix4x4(ref t, out result);
            return result;
        }
        #endregion

        #region Constant Transforms

        public static MatrixBasedTransform Identity
        {
            get
            {
                var result = new MatrixBasedTransform { Scaling = 1.0f };
                Matrix3x3.CreateIdentity(out result.Rotation);
                return result;
            }
        }

        #endregion
    }
}
