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
using Beholder.Resources;
using SharpDX;
using SharpDX.Direct3D9;
using Usage = SharpDX.Direct3D9.Usage;

namespace Beholder.Libraries.SharpDX9.Resources
{
    sealed class CBufferVertex : CBufferGpu<CBufferVertex, VertexBuffer>
    {
        public CBufferVertex(ICDevice device, BufferDescription description, SubresourceData? initialData, Action<CBufferVertex> onRelease)
            : base(device, description, initialData, onRelease) { }

        protected override VertexBuffer CreateBuffer(Device d3dDevice, int sizeInBytes, Usage usage, Pool pool, ExtraFlags extraFlags)
        {
            return new VertexBuffer(d3dDevice, sizeInBytes, usage, VertexFormat.None, pool);
        }

        protected override DataStream LockBuffer(VertexBuffer d3dBuffer, int offset, int size, LockFlags lockFlags)
        {
            return d3dBuffer.Lock(offset, size, lockFlags);
        }

        protected override void UnlockBuffer(VertexBuffer d3dBuffer)
        {
            d3dBuffer.Unlock();
        }
    }
}
