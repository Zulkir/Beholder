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
using Beholder.Resources;
using SharpDX;
using SharpDX.Direct3D9;
using Usage = SharpDX.Direct3D9.Usage;

namespace Beholder.Libraries.SharpDX9.Resources
{
    sealed class CBufferIndex : CBufferGpu<CBufferIndex, IndexBuffer>
    {
        int minVertexIndex;
        int totalVertexCount;
        public int GetMinVrtexIndex() { return minVertexIndex; }
        public int GetTotalVertexCount() { return totalVertexCount; }

        public CBufferIndex(ICDevice device, BufferDescription description, SubresourceData? initialData, Action<CBufferIndex> onRelease)
            : base(device, description, initialData, onRelease) { }

        protected override IndexBuffer CreateBuffer(Device d3dDevice, int sizeInBytes, Usage usage, Pool pool, ExtraFlags extraFlags)
        {
            return new IndexBuffer(d3dDevice, sizeInBytes, usage, pool, extraFlags.HasFlag(ExtraFlags.SixteenBitIndices));
        }

        protected override DataStream LockBuffer(IndexBuffer d3dBuffer, int offset, int size, LockFlags lockFlags)
        {
            return d3dBuffer.Lock(offset, size, lockFlags);
        }

        protected override void UnlockBuffer(IndexBuffer d3dBuffer)
        {
            d3dBuffer.Unlock();
        }

        protected override unsafe void OnDataChanging(byte* data)
        {
            if (desc.ExtraFlags.HasFlag(ExtraFlags.SixteenBitIndices))
            {
                ushort min, max;
                UnsafeMinMax16((ushort*)data, desc.SizeInBytes / sizeof(ushort), out min, out max);
                minVertexIndex = min;
                totalVertexCount = max - min + 1;
            }
            else
            {
                int min, max;
                UnsafeMinMax32((int*)data, desc.SizeInBytes / sizeof(int), out min, out max);
                minVertexIndex = min;
                totalVertexCount = max - min + 1;
            }
        }

        static unsafe void UnsafeMinMax16(ushort* indices, int sizeInUshorts, out ushort min, out ushort max)
        {
            ushort lMin = ushort.MaxValue;
            ushort lMax = ushort.MinValue;

            if (sizeInUshorts == 1)
                lMax = lMin = *indices;
            else
                for (ushort* p = indices; p < indices + sizeInUshorts; p++)
                {
                    if (*p < lMin) lMin = *p;
                    else if (*p > lMax) lMax = *p;
                }

            min = lMin;
            max = lMax;
        }

        static unsafe void UnsafeMinMax32(int* indices, int sizeInInts, out int min, out int max)
        {
            int lMin = int.MaxValue;
            int lMax = int.MinValue;

            if (sizeInInts == 1)
                lMax = lMin = *indices;
            else
                for (int* p = indices; p < indices + sizeInInts; p++)
                {
                    if (*p < lMin) lMin = *p;
                    else if (*p > lMax) lMax = *p;
                }

            min = lMin;
            max = lMax;
        }
    }
}
