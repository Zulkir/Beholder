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
using System.Runtime.InteropServices;
using Beholder.Resources;
using Beholder.Utility;
using SharpDX.Direct3D9;
using SharpDX;
using Usage = SharpDX.Direct3D9.Usage;

namespace Beholder.Libraries.SharpDX9.Resources
{
    abstract class CBufferGpu<TSelf, TD3DResource> : CBuffer, IDefaultPoolResource
        where TSelf : CBufferGpu<TSelf, TD3DResource>
        where TD3DResource : Resource
    {
        TD3DResource d3dBuffer;
        readonly byte[] mediumData;
        readonly bool needsMedium;

        public TD3DResource D3DBuffer { get { return d3dBuffer; } }

        protected CBufferGpu(ICDevice device, BufferDescription description, SubresourceData? initialData, Action<TSelf> onRelease)
            : base(device, ref description, b => onRelease((TSelf)b))
        {
            needsMedium = desc.Usage == Beholder.Resources.Usage.Dynamic || desc.BindFlags == BindFlags.IndexBuffer;
            if (needsMedium)
                mediumData = new byte[desc.SizeInBytes];

            if (desc.BindFlags != BindFlags.VertexBuffer && desc.BindFlags != BindFlags.IndexBuffer)
                throw new NotSupportedException(string.Format("BindFlags '{0}' are not supported for D3D9 Buffer resource", desc.BindFlags));
            if (desc.MiscFlags != MiscFlags.None)
                throw new NotSupportedException("MiscFlags are not supported by D3D9 Buffers");
            if (desc.StructureByteStride != 0)
                throw new NotSupportedException("Structured buffer stride is not supported by D3D9 Buffers");

            CreateNative();

            if (initialData.HasValue)
                SetSubresourceData(0, initialData.Value);
        }

        protected abstract TD3DResource CreateBuffer(Device d3dDevice, int sizeInBytes, Usage usage, Pool pool, ExtraFlags extraFlags);
        protected abstract DataStream LockBuffer(TD3DResource d3dBuffer, int offset, int size, LockFlags lockFlags);
        protected abstract void UnlockBuffer(TD3DResource d3dBuffer);
        protected unsafe virtual void OnDataChanging(byte* newData) { }

        #region Lifecycle
        void CreateNative()
        {
            Usage usage;
            Pool pool;

            switch (desc.Usage)
            {
                case Beholder.Resources.Usage.Immutable:
                case Beholder.Resources.Usage.Default:
                    usage = SharpDX.Direct3D9.Usage.WriteOnly;
                    pool = Pool.Managed;
                    break;
                case Beholder.Resources.Usage.Dynamic:
                    usage = SharpDX.Direct3D9.Usage.Dynamic | SharpDX.Direct3D9.Usage.WriteOnly;
                    pool = Pool.Default;
                    break;
                case Beholder.Resources.Usage.Staging:
                    usage = SharpDX.Direct3D9.Usage.None;
                    pool = Pool.SystemMemory;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (desc.ExtraFlags.HasFlag(ExtraFlags.Points))
                usage |= SharpDX.Direct3D9.Usage.Points;

            d3dBuffer = CreateBuffer(device.D3DDevice, desc.SizeInBytes, usage, pool, desc.ExtraFlags);
        }

        protected override void DisposeOfNative()
        {
            d3dBuffer.Dispose();
        }

        public void OnDeviceLost()
        {
            if (desc.Usage != Beholder.Resources.Usage.Dynamic) return;
            DisposeInternal();
        }

        public void OnDeviceReset()
        {
            if (desc.Usage != Beholder.Resources.Usage.Dynamic) return;

            CreateNative();

            if (desc.Usage == Beholder.Resources.Usage.Dynamic)
            {
                var subresourceData = new SubresourceData(mediumData);
                SetSubresourceData(0, subresourceData);
            }

        }
        #endregion

        #region Set Subresource Data
        public override unsafe void SetSubresourceData(int subresourceIndex, SubresourceData newData)
        {
            var stream = LockBuffer(d3dBuffer, 0, desc.SizeInBytes, desc.Usage == Beholder.Resources.Usage.Dynamic ? LockFlags.Discard : LockFlags.None);
            stream.WriteRange(newData.Pointer, desc.SizeInBytes);
            UnlockBuffer(d3dBuffer);

            if (needsMedium)
                Marshal.Copy(newData.Pointer, mediumData, 0, desc.SizeInBytes);

            OnDataChanging((byte*)newData.Pointer);

            newData.UnpinPointer();
        }
        #endregion

        protected override void DisposeRtv(IRenderTargetView view) { }
        protected override void DisposeSrv(IShaderResourceView view) { }
        protected override void DisposeUav(IUnorderedAccessView view) { }
        protected override bool DescEqualsRtv(IRenderTargetView view, ref RenderTargetViewDescription viewDescription) { throw new ShouldNeverHappenException(); }
        protected override bool DescEqualsSrv(IShaderResourceView view, ref ShaderResourceViewDescription viewDescription) { throw new ShouldNeverHappenException(); }
        protected override bool DescEqualsUav(IUnorderedAccessView view, ref UnorderedAccessViewDescription viewDescription) { throw new ShouldNeverHappenException(); }
        protected override IRenderTargetView CreateRtv(ref RenderTargetViewDescription viewDescription) { throw new ResourceBindingException(); }
        protected override IShaderResourceView CreateSrv(ref ShaderResourceViewDescription viewDescription) { throw new ResourceBindingException(); }
        protected override IUnorderedAccessView CreateUav(ref UnorderedAccessViewDescription viewDescription) { throw new ResourceBindingException(); }
    }
}
