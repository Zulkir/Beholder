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

using Beholder.Math;

namespace Beholder.Utility.DataProcessing
{
    public interface IDataWriter
    {
        void SkipBytes(int numBytes);
        void WriteInt8(sbyte b);
        void WriteUint8(byte b);
        void WriteInt16(short s);
        void WriteUint16(ushort s);
        void WriteInt32(int i);
        void WriteUint32(uint i);
        void WriteInt64(long l);
        void WriteUint64(ulong l);
        void WriteSingle(float f);
        void WriteVector(Vector2 v);
        void WriteVector(Vector3 v);
        void WriteVector(Vector4 v);
        void WriteVector(ref Vector2 v);
        void WriteVector(ref Vector3 v);
        void WriteVector(ref Vector4 v);
        void WriteMatrix(Matrix3x3 m);
        void WriteMatrix(Matrix4x3 m);
        void WriteMatrix(Matrix4x4 m);
        void WriteMatrix(ref Matrix3x3 m);
        void WriteMatrix(ref Matrix4x3 m);
        void WriteMatrix(ref Matrix4x4 m);
        void WriteQuaternion(Quaternion q);
        void WriteQuaternion(ref Quaternion q);
        void WriteDouble(double d);
    }
}
