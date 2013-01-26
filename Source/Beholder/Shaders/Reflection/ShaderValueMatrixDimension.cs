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

namespace Beholder.Shaders.Reflection
{
    public enum ShaderValueMatrixDimension
    {
        Matrix1x2 = 0x12,
        Matrix2x2 = 0x22,
        Matrix3x2 = 0x32,
        Matrix4x2 = 0x42,
        Matrix1x3 = 0x13,
        Matrix2x3 = 0x23,
        Matrix3x3 = 0x33,
        Matrix4x3 = 0x43,
        Matrix1x4 = 0x14,
        Matrix2x4 = 0x24,
        Matrix3x4 = 0x34,
        Matrix4x4 = 0x44,
    }
}
