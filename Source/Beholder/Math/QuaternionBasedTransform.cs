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

namespace Beholder.Math
{
    /// <summary>
    /// Represents an interpolatable transformantion with the order Translation -> Scaling -> Rotation.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct QuaternionBasedTransform : IEquatable<QuaternionBasedTransform>
    {
        /// <summary>
        /// Rotation matrix.
        /// </summary>
        public Vector3 Translation;

        /// <summary>
        /// Uniform scaling.
        /// </summary>
        public float Scaling;

        /// <summary>
        /// Unit quaternion that represents a rotation.
        /// </summary>
        public Quaternion Rotation;

        public QuaternionBasedTransform(float scaling, Quaternion rotation, Vector3 translation)
        {
            Translation = translation;
            Scaling = scaling;
            Rotation = rotation;
        }

        public QuaternionBasedTransform(float scaling, ref Quaternion rotation, ref Vector3 translation)
        {
            Scaling = scaling;
            Translation = translation;
            Rotation = rotation;
        }

        #region Object Overrides
        public bool Equals(QuaternionBasedTransform other)
        {
            return
                Vector3.Equals(ref Translation, ref other.Translation) &&
                Scaling == other.Scaling &&
                Quaternion.Equals(ref Rotation, ref other.Rotation);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is QuaternionBasedTransform))
            {
                return false;
            }
            QuaternionBasedTransform other = (QuaternionBasedTransform) obj;
            return Equals(ref this, ref other);
        }

        public override int GetHashCode()
        {
            return Translation.GetHashCode() + Scaling.GetHashCode() << 10 + Rotation.GetHashCode() << 20;
        }

        public override string ToString()
        {
            return "{ Translation: " + Translation.ToString() +
                "; Scaling: " + Scaling.ToString(CultureInfo.InvariantCulture) +
                "; Rotation: " + Rotation.ToString() + "; }";
        }

        public static bool Equals(ref QuaternionBasedTransform t1, ref QuaternionBasedTransform t2)
        {
            return
                Vector3.Equals(ref t1.Translation, ref t2.Translation) &&
                t1.Scaling == t2.Scaling &&
                Quaternion.Equals(ref t1.Rotation, ref t2.Rotation);
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
        public static void Combine(ref QuaternionBasedTransform left, ref QuaternionBasedTransform right, out QuaternionBasedTransform result)
        {
            result.Scaling = left.Scaling * right.Scaling;
            Quaternion.Multiply(ref left.Rotation, ref right.Rotation, out result.Rotation);
            Quaternion.ApplyRotation(ref left.Translation, ref right.Rotation, out result.Translation);
            Vector3.MultiplyInPlace(ref result.Translation, right.Scaling);
            Vector3.AddInPlace(ref result.Translation, ref right.Translation);
        }
        public static void CombineInPlace(ref QuaternionBasedTransform leftAndResult, ref QuaternionBasedTransform right)
        {
            Combine(ref leftAndResult, ref right, out leftAndResult);
        }
        public static QuaternionBasedTransform Combine(QuaternionBasedTransform left, QuaternionBasedTransform right)
        {
            Combine(ref left, ref right, out left);
            return left;
        }
        public static QuaternionBasedTransform operator *(QuaternionBasedTransform left, QuaternionBasedTransform right)
        {
            Combine(ref left, ref right, out left);
            return left;
        }
        #endregion

        #region Invert
        public static void Invert(ref QuaternionBasedTransform t, out QuaternionBasedTransform result)
        {
            result.Scaling = 1.0f / t.Scaling;
            Quaternion.Conjugate(ref t.Rotation, out result.Rotation);
            Vector3.Multiply(ref t.Translation, result.Scaling, out result.Translation);
            Quaternion.ApplyRotationInPlace(ref result.Translation, ref result.Rotation);
        }
        public static void InvertInPlace(ref QuaternionBasedTransform t)
        {
            Invert(ref t, out t);
        }
        public static QuaternionBasedTransform Invert(QuaternionBasedTransform t)
        {
            Invert(ref t, out t);
            return t;
        }
        #endregion

        #region Nlerp
        public static void Nlerp(ref QuaternionBasedTransform left, ref QuaternionBasedTransform right, float amount, out QuaternionBasedTransform result)
        {
            Vector3.Lerp(ref left.Translation, ref right.Translation, amount, out result.Translation);
            result.Scaling = left.Scaling - amount * (left.Scaling - right.Scaling);
            Quaternion.Nlerp(ref left.Rotation, ref right.Rotation, amount, out result.Rotation);
        }
        public static void NlerpInPlace(ref QuaternionBasedTransform leftAndResult, ref QuaternionBasedTransform right, float amount)
        {
            Nlerp(ref leftAndResult, ref right, amount, out leftAndResult);
        }
        public static QuaternionBasedTransform Nlerp(QuaternionBasedTransform left, QuaternionBasedTransform right, float amount)
        {
            Nlerp(ref left, ref right, amount, out left);
            return left;
        }
        #endregion

        #region Slerp
        public static void Slerp(ref QuaternionBasedTransform left, ref QuaternionBasedTransform right, float amount, out QuaternionBasedTransform result)
        {
            Vector3.Lerp(ref left.Translation, ref right.Translation, amount, out result.Translation);
            result.Scaling = left.Scaling - amount * (left.Scaling - right.Scaling);
            Quaternion.Slerp(ref left.Rotation, ref right.Rotation, amount, out result.Rotation);
        }
        public static void SlerpInPlace(ref QuaternionBasedTransform leftAndResult, ref QuaternionBasedTransform right, float amount)
        {
            Nlerp(ref leftAndResult, ref right, amount, out leftAndResult);
        }
        public static QuaternionBasedTransform Slerp(QuaternionBasedTransform left, QuaternionBasedTransform right, float amount)
        {
            Nlerp(ref left, ref right, amount, out left);
            return left;
        }
        #endregion
        
        #endregion

        #region Static Convertions
        public static void ToMatrix4x3(ref QuaternionBasedTransform t, out Matrix4x3 m)
        {
            Matrix3x3 scalingAndRotation;
            Quaternion.ToMatrix3x3(ref t.Rotation, out scalingAndRotation);
            Matrix3x3.MultiplyInPlace(ref scalingAndRotation, t.Scaling);
            Matrix4x3.FromMatrix3x3(ref scalingAndRotation, out m);
            m.C1.W = t.Translation.X;
            m.C2.W = t.Translation.Y;
            m.C3.W = t.Translation.Z;
        }
        public static Matrix4x3 ToMatrix4x3(QuaternionBasedTransform t)
        {
            Matrix4x3 result;
            ToMatrix4x3(ref t, out result);
            return result;
        }

        public static void ToMatrix4x4(ref QuaternionBasedTransform t, out Matrix4x4 m)
        {
            Matrix3x3 scalingAndRotation;
            Quaternion.ToMatrix3x3(ref t.Rotation, out scalingAndRotation);
            Matrix3x3.MultiplyInPlace(ref scalingAndRotation, t.Scaling);
            Matrix4x4.FromMatrix3x3(ref scalingAndRotation, out m);
            m.C1.W = t.Translation.X;
            m.C2.W = t.Translation.Y;
            m.C3.W = t.Translation.Z;
        }
        public static Matrix4x4 ToMatrix4x4(QuaternionBasedTransform t)
        {
            Matrix4x4 result;
            ToMatrix4x4(ref t, out result);
            return result;
        }
        #endregion

        #region Constant Transforms
        public static QuaternionBasedTransform Identity
        {
            get { return new QuaternionBasedTransform { Scaling = 1.0f, Rotation = { W = 1.0f } }; }
        }
        #endregion
    }
}
