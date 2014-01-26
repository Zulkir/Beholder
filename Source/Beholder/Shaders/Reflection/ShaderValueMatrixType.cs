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
using System.Linq;
using Beholder.Utility.Helpers;

namespace Beholder.Shaders.Reflection
{
    public class ShaderValueMatrixType : ShaderValueType, IEquatable<ShaderValueMatrixType>
    {
        readonly ShaderValueBaseType baseType;
        readonly ShaderValueMatrixDimension dimension;
        readonly int alignment;
        readonly int size;

        public ShaderValueBaseType BaseType { get { return baseType; } }
        public ShaderValueMatrixDimension Dimension { get { return dimension; } }
        public int Rows { get { return (int)dimension >> 4; } }
        public int Columns { get { return (int)dimension & 0xf; } }

        public override ShaderValueTypeClass Class { get { return ShaderValueTypeClass.Matrix; } }
        public override int Alignment { get { return alignment; } }
        public override int Size { get { return size; } }

        public ShaderValueMatrixType(ShaderValueBaseType baseType, ShaderValueMatrixDimension dimension)
        {
            this.baseType = baseType;
            this.dimension = dimension;

            int elementSize = baseType == ShaderValueBaseType.Double ? 8 : 4;

            alignment = DataHelper.Align(Rows == 3 ? elementSize * 4 : elementSize * Rows, 16);
            size = alignment * Columns;
        }

        public override bool TryGetBaseType(out ShaderValueBaseType baseTypeOut)
        {
            baseTypeOut = baseType;
            return true;
        }

        public bool Equals(ShaderValueMatrixType other)
        {
            if (ReferenceEquals(other, null))
                return false;
            if (ReferenceEquals(other, this))
                return true;
            return BaseType == other.BaseType &&
                   Dimension == other.Dimension;
        }

        public override bool Equals(ShaderValueType other)
        {
            return Equals(other as ShaderValueMatrixType);
        }

        protected override int DoGetHashCode()
        {
            return (int)BaseType | ((int)Dimension << 3);
        }

        public override string ToString()
        {
            return BaseType.ToStringFast() + Dimension.ToStringFast();
        }

        static readonly ShaderValueMatrixType[] FloatTypes =
            ((ShaderValueMatrixDimension[])Enum.GetValues(typeof(ShaderValueMatrixDimension)))
            .OrderBy(x => (int)x)
            .Select(x => 
                new ShaderValueMatrixType(ShaderValueBaseType.Float, x)).ToArray();
        public static ShaderValueMatrixType Float1x2 { get { return FloatTypes[0]; } }
        public static ShaderValueMatrixType Float1x3 { get { return FloatTypes[1]; } }
        public static ShaderValueMatrixType Float1x4 { get { return FloatTypes[2]; } }
        public static ShaderValueMatrixType Float2x2 { get { return FloatTypes[3]; } }
        public static ShaderValueMatrixType Float2x3 { get { return FloatTypes[4]; } }
        public static ShaderValueMatrixType Float2x4 { get { return FloatTypes[5]; } }
        public static ShaderValueMatrixType Float3x2 { get { return FloatTypes[6]; } }
        public static ShaderValueMatrixType Float3x3 { get { return FloatTypes[7]; } }
        public static ShaderValueMatrixType Float3x4 { get { return FloatTypes[8]; } }
        public static ShaderValueMatrixType Float4x2 { get { return FloatTypes[9]; } }
        public static ShaderValueMatrixType Float4x3 { get { return FloatTypes[10]; } }
        public static ShaderValueMatrixType Float4x4 { get { return FloatTypes[11]; } }

        static readonly ShaderValueMatrixType[] DoubleTypes =
            ((ShaderValueMatrixDimension[])Enum.GetValues(typeof(ShaderValueMatrixDimension))).Select(x =>
                new ShaderValueMatrixType(ShaderValueBaseType.Double, x)).ToArray();
        public static ShaderValueMatrixType Double1x2 { get { return DoubleTypes[0]; } }
        public static ShaderValueMatrixType Double1x3 { get { return DoubleTypes[1]; } }
        public static ShaderValueMatrixType Double1x4 { get { return DoubleTypes[2]; } }
        public static ShaderValueMatrixType Double2x2 { get { return DoubleTypes[3]; } }
        public static ShaderValueMatrixType Double2x3 { get { return DoubleTypes[4]; } }
        public static ShaderValueMatrixType Double2x4 { get { return DoubleTypes[5]; } }
        public static ShaderValueMatrixType Double3x2 { get { return DoubleTypes[6]; } }
        public static ShaderValueMatrixType Double3x3 { get { return DoubleTypes[7]; } }
        public static ShaderValueMatrixType Double3x4 { get { return DoubleTypes[8]; } }
        public static ShaderValueMatrixType Double4x2 { get { return DoubleTypes[9]; } }
        public static ShaderValueMatrixType Double4x3 { get { return DoubleTypes[10]; } }
        public static ShaderValueMatrixType Double4x4 { get { return DoubleTypes[11]; } }

        static readonly ShaderValueMatrixType[] IntTypes =
            ((ShaderValueMatrixDimension[])Enum.GetValues(typeof(ShaderValueMatrixDimension))).Select(x =>
                new ShaderValueMatrixType(ShaderValueBaseType.Int, x)).ToArray();
        public static ShaderValueMatrixType Int1x2 { get { return IntTypes[0]; } }
        public static ShaderValueMatrixType Int1x3 { get { return IntTypes[1]; } }
        public static ShaderValueMatrixType Int1x4 { get { return IntTypes[2]; } }
        public static ShaderValueMatrixType Int2x2 { get { return IntTypes[3]; } }
        public static ShaderValueMatrixType Int2x3 { get { return IntTypes[4]; } }
        public static ShaderValueMatrixType Int2x4 { get { return IntTypes[5]; } }
        public static ShaderValueMatrixType Int3x2 { get { return IntTypes[6]; } }
        public static ShaderValueMatrixType Int3x3 { get { return IntTypes[7]; } }
        public static ShaderValueMatrixType Int3x4 { get { return IntTypes[8]; } }
        public static ShaderValueMatrixType Int4x2 { get { return IntTypes[9]; } }
        public static ShaderValueMatrixType Int4x3 { get { return IntTypes[10]; } }
        public static ShaderValueMatrixType Int4x4 { get { return IntTypes[11]; } }

        static readonly ShaderValueMatrixType[] UIntTypes =
            ((ShaderValueMatrixDimension[])Enum.GetValues(typeof(ShaderValueMatrixDimension))).Select(x =>
                new ShaderValueMatrixType(ShaderValueBaseType.UInt, x)).ToArray();
        public static ShaderValueMatrixType UInt1x2 { get { return UIntTypes[0]; } }
        public static ShaderValueMatrixType UInt1x3 { get { return UIntTypes[1]; } }
        public static ShaderValueMatrixType UInt1x4 { get { return UIntTypes[2]; } }
        public static ShaderValueMatrixType UInt2x2 { get { return UIntTypes[3]; } }
        public static ShaderValueMatrixType UInt2x3 { get { return UIntTypes[4]; } }
        public static ShaderValueMatrixType UInt2x4 { get { return UIntTypes[5]; } }
        public static ShaderValueMatrixType UInt3x2 { get { return UIntTypes[6]; } }
        public static ShaderValueMatrixType UInt3x3 { get { return UIntTypes[7]; } }
        public static ShaderValueMatrixType UInt3x4 { get { return UIntTypes[8]; } }
        public static ShaderValueMatrixType UInt4x2 { get { return UIntTypes[9]; } }
        public static ShaderValueMatrixType UInt4x3 { get { return UIntTypes[10]; } }
        public static ShaderValueMatrixType UInt4x4 { get { return UIntTypes[11]; } }

        static readonly ShaderValueMatrixType[] SNormTypes =
            ((ShaderValueMatrixDimension[])Enum.GetValues(typeof(ShaderValueMatrixDimension))).Select(x =>
                new ShaderValueMatrixType(ShaderValueBaseType.SNorm, x)).ToArray();
        public static ShaderValueMatrixType SNorm1x2 { get { return SNormTypes[0]; } }
        public static ShaderValueMatrixType SNorm1x3 { get { return SNormTypes[1]; } }
        public static ShaderValueMatrixType SNorm1x4 { get { return SNormTypes[2]; } }
        public static ShaderValueMatrixType SNorm2x2 { get { return SNormTypes[3]; } }
        public static ShaderValueMatrixType SNorm2x3 { get { return SNormTypes[4]; } }
        public static ShaderValueMatrixType SNorm2x4 { get { return SNormTypes[5]; } }
        public static ShaderValueMatrixType SNorm3x2 { get { return SNormTypes[6]; } }
        public static ShaderValueMatrixType SNorm3x3 { get { return SNormTypes[7]; } }
        public static ShaderValueMatrixType SNorm3x4 { get { return SNormTypes[8]; } }
        public static ShaderValueMatrixType SNorm4x2 { get { return SNormTypes[9]; } }
        public static ShaderValueMatrixType SNorm4x3 { get { return SNormTypes[10]; } }
        public static ShaderValueMatrixType SNorm4x4 { get { return SNormTypes[11]; } }

        static readonly ShaderValueMatrixType[] UNormTypes =
            ((ShaderValueMatrixDimension[])Enum.GetValues(typeof(ShaderValueMatrixDimension))).Select(x =>
                new ShaderValueMatrixType(ShaderValueBaseType.UNorm, x)).ToArray();
        public static ShaderValueMatrixType UNorm1x2 { get { return UNormTypes[0]; } }
        public static ShaderValueMatrixType UNorm1x3 { get { return UNormTypes[1]; } }
        public static ShaderValueMatrixType UNorm1x4 { get { return UNormTypes[2]; } }
        public static ShaderValueMatrixType UNorm2x2 { get { return UNormTypes[3]; } }
        public static ShaderValueMatrixType UNorm2x3 { get { return UNormTypes[4]; } }
        public static ShaderValueMatrixType UNorm2x4 { get { return UNormTypes[5]; } }
        public static ShaderValueMatrixType UNorm3x2 { get { return UNormTypes[6]; } }
        public static ShaderValueMatrixType UNorm3x3 { get { return UNormTypes[7]; } }
        public static ShaderValueMatrixType UNorm3x4 { get { return UNormTypes[8]; } }
        public static ShaderValueMatrixType UNorm4x2 { get { return UNormTypes[9]; } }
        public static ShaderValueMatrixType UNorm4x3 { get { return UNormTypes[10]; } }
        public static ShaderValueMatrixType UNorm4x4 { get { return UNormTypes[11]; } }

        static readonly ShaderValueMatrixType[] BoolTypes =
            ((ShaderValueMatrixDimension[])Enum.GetValues(typeof(ShaderValueMatrixDimension))).Select(x =>
                new ShaderValueMatrixType(ShaderValueBaseType.Bool, x)).ToArray();
        public static ShaderValueMatrixType Bool1x2 { get { return BoolTypes[0]; } }
        public static ShaderValueMatrixType Bool1x3 { get { return BoolTypes[1]; } }
        public static ShaderValueMatrixType Bool1x4 { get { return BoolTypes[2]; } }
        public static ShaderValueMatrixType Bool2x2 { get { return BoolTypes[3]; } }
        public static ShaderValueMatrixType Bool2x3 { get { return BoolTypes[4]; } }
        public static ShaderValueMatrixType Bool2x4 { get { return BoolTypes[5]; } }
        public static ShaderValueMatrixType Bool3x2 { get { return BoolTypes[6]; } }
        public static ShaderValueMatrixType Bool3x3 { get { return BoolTypes[7]; } }
        public static ShaderValueMatrixType Bool3x4 { get { return BoolTypes[8]; } }
        public static ShaderValueMatrixType Bool4x2 { get { return BoolTypes[9]; } }
        public static ShaderValueMatrixType Bool4x3 { get { return BoolTypes[10]; } }
        public static ShaderValueMatrixType Bool4x4 { get { return BoolTypes[11]; } }
    }
}
