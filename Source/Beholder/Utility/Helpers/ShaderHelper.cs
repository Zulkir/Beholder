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
using System.Globalization;
using Beholder.Shaders.Reflection;

namespace Beholder.Utility.Helpers
{
    public static class ShaderHelper
    {
        public static string BaseTypeToString(ShaderValueBaseType baseType)
        {
            switch (baseType)
            {
                case ShaderValueBaseType.Float: return "float";
                case ShaderValueBaseType.Double: return "double";
                case ShaderValueBaseType.Int: return "int";
                case ShaderValueBaseType.UInt: return "uint";
                case ShaderValueBaseType.SNorm: return "snorm";
                case ShaderValueBaseType.UNorm: return "unorm";
                case ShaderValueBaseType.Bool: return "bool";
                default: throw new ArgumentOutOfRangeException();
            }
        }

        static string ScalarTypeToString(ShaderValueScalarType type)
        {
            return BaseTypeToString(type.BaseType);
        }

        static string VectorTypeToString(ShaderValueVectorType type)
        {
            return BaseTypeToString(type.BaseType) + ((int)type.Dimension).ToString(CultureInfo.InvariantCulture);
        }

        static string MatrixTypeToString(ShaderValueMatrixType type)
        {
            return string.Format("{0}{1}x{2}",
                BaseTypeToString(type.BaseType),
                type.Rows.ToString(CultureInfo.InvariantCulture),
                type.Columns.ToString(CultureInfo.InvariantCulture));
        }

        public static string ValueTypeToString(ShaderValueType type)
        {
            switch (type.Class)
            {
                case ShaderValueTypeClass.Scalar:
                    return ScalarTypeToString((ShaderValueScalarType)type);
                case ShaderValueTypeClass.Vector:
                    return VectorTypeToString((ShaderValueVectorType)type);
                case ShaderValueTypeClass.Matrix:
                    return MatrixTypeToString((ShaderValueMatrixType)type);
                case ShaderValueTypeClass.Array:
                    var arrayType = (ShaderValueArrayType)type;
                    return string.Format("{0}[{1}]", ValueTypeToString(arrayType.ElementType), arrayType.Length);
                case ShaderValueTypeClass.Structure:
                    return ((ShaderValueStructureType)type).Name;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static string ValueVariableDeclaration(ShaderValueType type, string name)
        {
            if (type.Class != ShaderValueTypeClass.Array)
                return string.Format("{0} {1}", ValueTypeToString(type), name);
            var arrayType = (ShaderValueArrayType)type;
            return string.Format("{0} {1}[{2}]", ValueTypeToString(arrayType.ElementType), name, arrayType.Length.ToString(CultureInfo.InvariantCulture));
        }

        public static string ObjectTypeClassToString(ShaderObjectTypeClass typeClass)
        {
            switch (typeClass)
            {
                case ShaderObjectTypeClass.Buffer: return "Buffer";
                case ShaderObjectTypeClass.RWBuffer: return "RWBuffer";
                case ShaderObjectTypeClass.ByteAddressBuffer: return "ByteAddressBuffer";
                case ShaderObjectTypeClass.RWByteAddressBuffer: return "RWByteAddressBuffer";
                case ShaderObjectTypeClass.AppendStructuredBuffer: return "AppendStructuredBuffer";
                case ShaderObjectTypeClass.ConsumeStructuredBuffer: return "ConsumeStructuredBuffer";
                case ShaderObjectTypeClass.RWStructuredBuffer: return "RWStructuredBuffer";
                case ShaderObjectTypeClass.StructuredBuffer: return "StructuredBuffer";
                case ShaderObjectTypeClass.Texture1D: return "Texture1D";
                case ShaderObjectTypeClass.RWTexture1D: return "RWTexture1D";
                case ShaderObjectTypeClass.Texture1DArray: return "Texture1DArray";
                case ShaderObjectTypeClass.RWTexture1DArray: return "RWTexture1DArray";
                case ShaderObjectTypeClass.Texture2D: return "Texture2D";
                case ShaderObjectTypeClass.RWTexture2D: return "RWTexture2D";
                case ShaderObjectTypeClass.Texture2DArray: return "Texture2DArray";
                case ShaderObjectTypeClass.RWTexture2DArray: return "RWTexture2DArray";
                case ShaderObjectTypeClass.Texture2DMS: return "Texture2DMS";
                case ShaderObjectTypeClass.Texture2DMSArray: return "Texture2DMSArray";
                case ShaderObjectTypeClass.TextureCube: return "TextureCube";
                case ShaderObjectTypeClass.TextureCubeArray: return "TextureCubeArray";
                case ShaderObjectTypeClass.Texture3D: return "Texture3D";
                case ShaderObjectTypeClass.RWTexture3D: return "RWTexture3D";
                default: throw new ArgumentOutOfRangeException("typeClass");
            }
        }
    }
}
