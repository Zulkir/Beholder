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
    public unsafe class ArrayReader
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

        public byte[] ReadBytes(int count)
        {
            var bytes = new byte[count];
            Marshal.Copy((IntPtr)p, bytes, 0, count);
            p += count;

            return bytes;
        }

        public void ReadBytes(byte[] destArray, int destOffset, int count)
        {
            var bytes = new byte[count];
            Marshal.Copy((IntPtr)p, bytes, destOffset, count);
            p += count;
        }

        public byte[] PeekBytes(int count)
        {
            var bytes = new byte[count];
            Marshal.Copy((IntPtr)p, bytes, 0, count);

            return bytes;
        }

        public void PeekBytes(byte[] destArray, int destOffset, int count)
        {
            var bytes = new byte[count];
            Marshal.Copy((IntPtr)p, bytes, destOffset, count);
        }

        public void SkipBytes(int count)
        {
            p += count;
        }

        public sbyte ReadInt8()
        {
            sbyte n = *(sbyte*)p; p++;
            return n;
        }

        public sbyte PeekInt8()
        {
            return *(sbyte*)p;
        }

        public byte ReadUint8()
        {
            byte n = *p; p++;
            return n;
        }

        public byte PeekUint8()
        {
            return *p;
        }

        public short ReadInt16()
        {
            var n = *(short*)p; p += 2;
            return n;
        }

        public short PeekInt16()
        {
            return *(short*)p;
        }

        public ushort ReadUint16()
        {
            var n = *(ushort*)p; p += 2;
            return n;
        }

        public ushort PeekUint16()
        {
            return *(ushort*)p;
        }

        public int ReadInt32()
        {
            var n = *(int*)p; p += 4;
            return n;
        }

        public int PeekInt32()
        {
            return *(int*)p;
        }

        public uint ReadUint32()
        {
            var n = *(uint*)p; p += 4;
            return n;
        }

        public uint PeekUint32()
        {
            return *(uint*)p;
        }

        public long ReadInt64()
        {
            var n = *(long*)p; p += 8;
            return n;
        }

        public long PeekInt64()
        {
            return *(long*)p;
        }

        public ulong ReadUint64()
        {
            var n = *(ulong*)p; p += 8;
            return n;
        }

        public ulong PeekUint64()
        {
            return *(ulong*)p;
        }

        public float ReadSingle()
        {
            var n = *(float*)p; p += 4;
            return n;
        }

        public float PeekSingle()
        {
            return *(float*)p;
        }

        public Vector2 ReadVector2()
        {
            var n = *(Vector2*)p; p += 8;
            return n;
        }

        public void ReadVector2(out Vector2 v)
        {
            v = *(Vector2*)p; p += 8;
        }

        public Vector2 PeekVector2()
        {
            return *(Vector2*)p;
        }

        public void PeekVector2(out Vector2 v)
        {
            v = *(Vector2*)p;
        }

        public Vector3 ReadVector3()
        {
            var n = *(Vector3*)p; p += 12;
            return n;
        }

        public void ReadVector3(out Vector3 v)
        {
            v = *(Vector3*)p; p += 12;
        }

        public Vector3 PeekVector3()
        {
            return *(Vector3*)p;
        }

        public void PeekVector3(out Vector3 v)
        {
            v = *(Vector3*)p;
        }

        public Vector4 ReadVector4()
        {
            var n = *(Vector4*)p; p += 16;
            return n;
        }

        public void ReadVector4(out Vector4 v)
        {
            v = *(Vector4*)p; p += 16;
        }

        public Vector4 PeekVector4()
        {
            return *(Vector4*)p;
        }

        public void PeekVector4(out Vector4 v)
        {
            v = *(Vector4*)p;
        }

        public Quaternion ReadQuaternion()
        {
            var n = *(Quaternion*)p; p += 16;
            return n;
        }

        public void ReadQuaternion(out Quaternion q)
        {
            q = *(Quaternion*)p; p += 16;
        }

        public Quaternion PeekQuaternion()
        {
            return *(Quaternion*)p;
        }

        public void PeekQuaternion(out Quaternion q)
        {
            q = *(Quaternion*)p;
        }

        public double ReadDouble()
        {
            var n = *(double*)p; p += 8;
            return n;
        }

        public double PeekDouble()
        {
            return *(double*)p;
        }

        public string ReadAscii(int numChars)
        {
            char[] chars = new char[numChars];

            for (int i = 0; i < numChars; i++, p++)
            {
                chars[i] = *(char*)p;
            }

            return new string(chars);
        }

        public string PeekAscii(int numChars)
        {
            var chars = new char[numChars];

            byte* pIter = p;
            for (int i = 0; i < numChars; i++, pIter++)
            {
                chars[i] = *(char*)pIter;
            }

            return new string(chars);
        }
    }
}
