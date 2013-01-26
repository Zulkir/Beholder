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

namespace Beholder.Shaders.Reflection
{
    public static class ShaderObjectTypeClassExtensions
    {
         public static string ToStringFast(this ShaderObjectTypeClass typeClass)
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