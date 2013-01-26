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
using System.Runtime.InteropServices;
using Beholder.Math;

namespace Beholder.Utility.DataProcessing
{
    public unsafe class ArrayWriter : IDataWriter
    {
        //readonly Array array;
        GCHandle handle;
        byte* start;

        byte* p;

        public int Offset
        {
            get { return (int)(p - start); }
            set { p = start + value; }
        }

        public void Pin(Array array)
        {
            if (array == null) throw new ArgumentNullException("array");
            if (handle.IsAllocated) throw new InvalidOperationException("An ArrayWriter must be unpinned before pinning to another array.");

            handle = GCHandle.Alloc(array, GCHandleType.Pinned);
            start = p = (byte*)handle.AddrOfPinnedObject();
        }

        public void Unpin()
        {
            handle.Free();
        }

        public void WriteBytes(byte[] sourceArray)
        {
            Marshal.Copy(sourceArray, 0, (IntPtr)p, sourceArray.Length);
            p += sourceArray.Length;
        }

        public void WriteBytes(byte[] sourceArray, int startSourceIndex, int count)
        {
            Marshal.Copy(sourceArray, startSourceIndex, (IntPtr)p, sourceArray.Length);
            p += sourceArray.Length;
        }

        public void SkipBytes(int count)
        {
            p += count;
        }

        public void WriteInt8(sbyte n)
        {
            *(sbyte*)p = n; p++;
        }

        public void WriteUint8(byte n)
        {
            *p = n; p++;
        }

        public void WriteInt16(short n)
        {
            *(short*)p = n; p += 2;
        }

        public void WriteUint16(ushort n)
        {
            *(ushort*)p = n; p += 2;
        }

        public void WriteInt32(int n)
        {
            *(int*)p = n; p += 4;
        }

        public void WriteUint32(uint n)
        {
            *(uint*)p = n; p += 4;
        }

        public void WriteInt64(long n)
        {
            *(long*)p = n; p += 8;
        }

        public void WriteUint64(ulong n)
        {
            *(ulong*)p = n; p += 8;
        }

        public void WriteSingle(float n)
        {
            *(float*)p = n; p += 4;
        }

        public void WriteVector(Vector2 v)
        {
            *(Vector2*)p = v; p += 8;
        }

        public void WriteVector(ref Vector2 v)
        {
            *(Vector2*)p = v; p += 8;
        }

        public void WriteVector(Vector3 v)
        {
            *(Vector3*)p = v; p += 12;
        }

        public void WriteVector(ref Vector3 v)
        {
            *(Vector3*)p = v; p += 12;
        }

        public void WriteVector(Vector4 v)
        {
            *(Vector4*)p = v; p += 16;
        }

        public void WriteVector(ref Vector4 v)
        {
            *(Vector4*)p = v; p += 16;
        }

        public void WriteMatrix(Matrix3x3 m)
        {
            *(Matrix3x3*)p = m; p += 36;
        }

        public void WriteMatrix(ref Matrix3x3 m)
        {
            *(Matrix3x3*)p = m; p += 36;
        }

        public void WriteMatrix(Matrix4x3 m)
        {
            *(Matrix4x3*)p = m; p += 48;
        }

        public void WriteMatrix(ref Matrix4x3 m)
        {
            *(Matrix4x3*)p = m; p += 48;
        }

        public void WriteMatrix(Matrix4x4 m)
        {
            *(Matrix4x4*)p = m; p += 64;
        }

        public void WriteMatrix(ref Matrix4x4 m)
        {
            *(Matrix4x4*)p = m; p += 64;
        }

        public void WriteQuaternion(Quaternion q)
        {
            *(Quaternion*)p = q; p += 16;
        }

        public void WriteQuaternion(ref Quaternion q)
        {
            *(Quaternion*)p = q; p += 16;
        }

        public void WriteDouble(double n)
        {
            *(double*)p = n; p += 8;
        }
    }
}
