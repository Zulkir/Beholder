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

namespace Beholder.Shaders.Reflection
{
    public class ShaderValueScalarType : ShaderValueType, IEquatable<ShaderValueScalarType>
    {
        readonly ShaderValueBaseType baseType;
        readonly int alignment;
        readonly int size;
        
        public ShaderValueBaseType BaseType { get { return baseType; } }

        public override ShaderValueTypeClass Class { get { return ShaderValueTypeClass.Scalar; } }
        public override int Alignment { get { return alignment; } }
        public override int Size { get { return size; } }

        public ShaderValueScalarType(ShaderValueBaseType baseType)
        {
            this.baseType = baseType;
            alignment = size = baseType == ShaderValueBaseType.Double ? 8 : 4;
        }

        public override bool TryGetBaseType(out ShaderValueBaseType baseTypeOut)
        {
            baseTypeOut = baseType;
            return true;
        }

        public bool Equals(ShaderValueScalarType other)
        {
            if (ReferenceEquals(other, null))
                return false;
            if (ReferenceEquals(other, this))
                return true;
            return BaseType == other.BaseType;
        }

        public override bool Equals(ShaderValueType other)
        {
            return Equals(other as ShaderValueScalarType);
        }

        protected override int DoGetHashCode()
        {
            return (int)BaseType;
        }

        public override string ToString()
        {
            return BaseType.ToStringFast();
        }

        public static ShaderValueScalarType FromBaseType(ShaderValueBaseType baseType)
        {
            switch (baseType)
            {
                case ShaderValueBaseType.Float: return Float;
                case ShaderValueBaseType.Double: return Double;
                case ShaderValueBaseType.Int: return Int;
                case ShaderValueBaseType.UInt: return UInt;
                case ShaderValueBaseType.SNorm: return SNorm;
                case ShaderValueBaseType.UNorm: return UNorm;
                case ShaderValueBaseType.Bool: return Bool;
                default: throw new ArgumentOutOfRangeException("baseType");
            }
        }

        static readonly ShaderValueScalarType FloatType = new ShaderValueScalarType(ShaderValueBaseType.Float);
        public static ShaderValueScalarType Float { get { return FloatType; } }

        static readonly ShaderValueScalarType DoubleType = new ShaderValueScalarType(ShaderValueBaseType.Double);
        public static ShaderValueScalarType Double { get { return DoubleType; } }

        static readonly ShaderValueScalarType IntType = new ShaderValueScalarType(ShaderValueBaseType.Int);
        public static ShaderValueScalarType Int { get { return IntType; } }

        static readonly ShaderValueScalarType UIntType = new ShaderValueScalarType(ShaderValueBaseType.UInt);
        public static ShaderValueScalarType UInt { get { return UIntType; } }

        static readonly ShaderValueScalarType SNormType = new ShaderValueScalarType(ShaderValueBaseType.SNorm);
        public static ShaderValueScalarType SNorm { get { return SNormType; } }

        static readonly ShaderValueScalarType UNormType = new ShaderValueScalarType(ShaderValueBaseType.UNorm);
        public static ShaderValueScalarType UNorm { get { return UNormType; } }

        static readonly ShaderValueScalarType BoolType = new ShaderValueScalarType(ShaderValueBaseType.Bool);
        public static ShaderValueScalarType Bool { get { return BoolType; } }
    }
}
