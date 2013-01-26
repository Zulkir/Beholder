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
using Beholder.Utility.Helpers;

namespace Beholder.Shaders.Reflection
{
    public class ShaderValueArrayType : ShaderValueType, IEquatable<ShaderValueArrayType>
    {
        readonly ShaderValueType elementType;
        readonly int length;
        readonly int alignment;
        readonly int size;

        public override ShaderValueTypeClass Class { get { return ShaderValueTypeClass.Array; } }
        public override int Alignment { get { return alignment; } }
        public override int Size { get { return size; } }
        public ShaderValueType ElementType { get { return elementType; } }
        public int Length { get { return length; } }

        public ShaderValueArrayType(ShaderValueType elementType, int length)
        {
            if (elementType == null)
                throw new ArgumentNullException("elementType");

            this.elementType = elementType;
            this.length = length;

            switch (elementType.Class)
            {
                case ShaderValueTypeClass.Scalar:
                case ShaderValueTypeClass.Vector:
                case ShaderValueTypeClass.Structure:
                    alignment = DataHelper.Align(elementType.Alignment, 16);
                    size = alignment * Length;
                    break;
                case ShaderValueTypeClass.Matrix:
                    var cElementType = (ShaderValueMatrixType)elementType;
                    alignment = DataHelper.Align(cElementType.Alignment / cElementType.Columns, 16);
                    size = alignment * cElementType.Columns * Length;
                    break;
                case ShaderValueTypeClass.Array:
                    throw new NotSupportedException("Arrays of arrays are not supported by std140 layout");
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override bool TryGetBaseType(out ShaderValueBaseType baseTypeOut)
        {
            return elementType.TryGetBaseType(out baseTypeOut);
        }

        public bool Equals(ShaderValueArrayType other)
        {
            if (ReferenceEquals(other, null))
                return false;
            if (ReferenceEquals(other, this))
                return true;
            return ElementType.Equals(other.ElementType) &&
                   Length == other.Length;
        }

        public override bool Equals(ShaderValueType other)
        {
            return Equals(other as ShaderValueArrayType);
        }

        protected override int DoGetHashCode()
        {
            return ElementType.GetHashCode() + (Size << 18);
        }

        public override string ToString()
        {
            return string.Format("{0}[{1}]", ElementType, Size.ToString(CultureInfo.InvariantCulture));
        }
    }
}
