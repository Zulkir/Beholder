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

namespace Beholder.Shaders.Reflection
{
    public class ShaderValueVectorType : ShaderValueType, IEquatable<ShaderValueVectorType>
    {
        readonly ShaderValueBaseType baseType;
        readonly ShaderValueVectorDimension dimension;
        readonly int alignment;
        readonly int size;

        public ShaderValueBaseType BaseType { get { return baseType; } }
        public ShaderValueVectorDimension Dimension { get { return dimension; } }

        public override ShaderValueTypeClass Class { get { return ShaderValueTypeClass.Vector; } }
        public override int Alignment { get { return alignment; } }
        public override int Size { get { return size; } }

        public ShaderValueVectorType(ShaderValueBaseType baseType, ShaderValueVectorDimension dimension)
        {
            this.baseType = baseType;
            this.dimension = dimension;

            int elementSize = baseType == ShaderValueBaseType.Double ? 8 : 4;

            alignment = dimension == ShaderValueVectorDimension.Vector2 ? 2 * elementSize : 4 * elementSize;
            size = 4 * (int)dimension;
        }

        public override bool TryGetBaseType(out ShaderValueBaseType baseTypeOut)
        {
            baseTypeOut = baseType;
            return true;
        }

        public bool Equals(ShaderValueVectorType other)
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
            return Equals(other as ShaderValueVectorType);
        }

        protected override int DoGetHashCode()
        {
            return (int)BaseType | (((int)Dimension - 1) << 3);
        }

        public override string ToString()
        {
            return BaseType.ToStringFast() + Dimension.ToStringFast();
        }

        static readonly ShaderValueVectorType[] FloatTypes = Enumerable.Range(2, 4).Select(x => 
            new ShaderValueVectorType(ShaderValueBaseType.Float, (ShaderValueVectorDimension)x)).ToArray();
        public static ShaderValueVectorType Float2 { get { return FloatTypes[0]; } }
        public static ShaderValueVectorType Float3 { get { return FloatTypes[1]; } }
        public static ShaderValueVectorType Float4 { get { return FloatTypes[2]; } }

        static readonly ShaderValueVectorType[] DoubleTypes = Enumerable.Range(2, 4).Select(x =>
            new ShaderValueVectorType(ShaderValueBaseType.Double, (ShaderValueVectorDimension)x)).ToArray();
        public static ShaderValueVectorType Double2 { get { return DoubleTypes[0]; } }
        public static ShaderValueVectorType Double3 { get { return DoubleTypes[1]; } }
        public static ShaderValueVectorType Double4 { get { return DoubleTypes[2]; } }

        static readonly ShaderValueVectorType[] IntTypes = Enumerable.Range(2, 4).Select(x =>
            new ShaderValueVectorType(ShaderValueBaseType.Int, (ShaderValueVectorDimension)x)).ToArray();
        public static ShaderValueVectorType Int2 { get { return IntTypes[0]; } }
        public static ShaderValueVectorType Int3 { get { return IntTypes[1]; } }
        public static ShaderValueVectorType Int4 { get { return IntTypes[2]; } }

        static readonly ShaderValueVectorType[] UIntTypes = Enumerable.Range(2, 4).Select(x =>
            new ShaderValueVectorType(ShaderValueBaseType.UInt, (ShaderValueVectorDimension)x)).ToArray();
        public static ShaderValueVectorType UInt2 { get { return UIntTypes[0]; } }
        public static ShaderValueVectorType UInt3 { get { return UIntTypes[1]; } }
        public static ShaderValueVectorType UInt4 { get { return UIntTypes[2]; } }

        static readonly ShaderValueVectorType[] SNormTypes = Enumerable.Range(2, 4).Select(x =>
            new ShaderValueVectorType(ShaderValueBaseType.SNorm, (ShaderValueVectorDimension)x)).ToArray();
        public static ShaderValueVectorType SNorm2 { get { return SNormTypes[0]; } }
        public static ShaderValueVectorType SNorm3 { get { return SNormTypes[1]; } }
        public static ShaderValueVectorType SNorm4 { get { return SNormTypes[2]; } }

        static readonly ShaderValueVectorType[] UNormTypes = Enumerable.Range(2, 4).Select(x =>
            new ShaderValueVectorType(ShaderValueBaseType.UNorm, (ShaderValueVectorDimension)x)).ToArray();
        public static ShaderValueVectorType UNorm2 { get { return UNormTypes[0]; } }
        public static ShaderValueVectorType UNorm3 { get { return UNormTypes[1]; } }
        public static ShaderValueVectorType UNorm4 { get { return UNormTypes[2]; } }

        static readonly ShaderValueVectorType[] BoolTypes = Enumerable.Range(2, 4).Select(x =>
            new ShaderValueVectorType(ShaderValueBaseType.Bool, (ShaderValueVectorDimension)x)).ToArray();
        public static ShaderValueVectorType Bool2 { get { return BoolTypes[0]; } }
        public static ShaderValueVectorType Bool3 { get { return BoolTypes[1]; } }
        public static ShaderValueVectorType Bool4 { get { return BoolTypes[2]; } }
    }
}
