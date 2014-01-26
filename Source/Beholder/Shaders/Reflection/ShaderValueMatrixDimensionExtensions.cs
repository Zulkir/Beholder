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
    public static class ShaderValueMatrixDimensionExtensions
    {
         public static string ToStringFast(this ShaderValueMatrixDimension dimension)
         {
             switch (dimension)
             {
                 case ShaderValueMatrixDimension.Matrix1x2: return "1x2";
                 case ShaderValueMatrixDimension.Matrix2x2: return "2x2";
                 case ShaderValueMatrixDimension.Matrix3x2: return "3x2";
                 case ShaderValueMatrixDimension.Matrix4x2: return "4x2";
                 case ShaderValueMatrixDimension.Matrix1x3: return "1x3";
                 case ShaderValueMatrixDimension.Matrix2x3: return "2x3";
                 case ShaderValueMatrixDimension.Matrix3x3: return "3x3";
                 case ShaderValueMatrixDimension.Matrix4x3: return "4x3";
                 case ShaderValueMatrixDimension.Matrix1x4: return "1x4";
                 case ShaderValueMatrixDimension.Matrix2x4: return "2x4";
                 case ShaderValueMatrixDimension.Matrix3x4: return "3x4";
                 case ShaderValueMatrixDimension.Matrix4x4: return "4x4";
                 default: throw new ArgumentOutOfRangeException("dimension");
             }
         }
    }
}