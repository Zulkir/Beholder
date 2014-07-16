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
using Beholder.Libraries.SharpDX11.Core;
using Beholder.Resources;
using Beholder.Utility.ForImplementations.Resources;
using SharpDX;
using SharpDX.Direct3D11;
using Buffer = SharpDX.Direct3D11.Buffer;
using BufferDescription = Beholder.Resources.BufferDescription;
using MapFlags = SharpDX.Direct3D11.MapFlags;
using RenderTargetViewDescription = Beholder.Resources.RenderTargetViewDescription;
using ShaderResourceViewDescription = Beholder.Resources.ShaderResourceViewDescription;
using UnorderedAccessViewDescription = Beholder.Resources.UnorderedAccessViewDescription;

namespace Beholder.Libraries.SharpDX11.Resources
{
    class CBuffer : BufferBase<ICDevice>, ICResource
    {
        readonly Buffer d3dBuffer;

        public Buffer D3DBuffer { get { return d3dBuffer; } }
        public Resource D3DResource { get { return d3dBuffer; } }

        public CBuffer(ICDevice device, ref BufferDescription desc, ref SubresourceData initialData, Action<CBuffer> onDispose)
            : base(device, ref desc, b => onDispose((CBuffer)b))
        {
            var d3dDesc = new SharpDX.Direct3D11.BufferDescription
            {
                SizeInBytes = desc.SizeInBytes,
                Usage = CtSharpDX11.ResourceUsage(desc.Usage),
                BindFlags = CtSharpDX11.BindFlags(desc.BindFlags),
                CpuAccessFlags = CtSharpDX11.CpuAccessFlags(desc.Usage),
                OptionFlags = CtSharpDX11.ResourceOptionFlags(desc.MiscFlags),
                StructureByteStride = desc.StructureByteStride
            };

            if (initialData.Pointer == IntPtr.Zero)
            {
                d3dBuffer = new Buffer(device.D3DDevice, d3dDesc);
            }
            else
            {
                d3dBuffer = new Buffer(device.D3DDevice, initialData.Pointer, d3dDesc);
                initialData.UnpinPointer();
            }
        }

        protected override void DisposeOfNative()
        {
            d3dBuffer.Dispose();
        }

        public void SetSubresourceData(CDeviceContext deviceContext, int subresourceIndex, SubresourceData data)
        {
            switch (desc.Usage)
            {
                case Usage.Default:
                    {
                        deviceContext.UpdateSubresourceInternal(d3dBuffer, 0, null, data.Pointer, 0, 0);
                    }
                    break;
                case Usage.Dynamic:
                    {
                        var box = deviceContext.MapSubresourceInternal(d3dBuffer, 0, MapMode.WriteDiscard, MapFlags.None);
                        Utilities.CopyMemory(box.DataPointer, data.Pointer, desc.SizeInBytes);
                        deviceContext.UnmapSubresourceInternal(d3dBuffer, 0);
                    }
                    break;
                default:
                    throw new InvalidOperationException("Can only set data of Default and Dynamic resources.");
            }
            data.UnpinPointer();
        }

        #region For BufferBase
        protected override void DisposeRtv(IRenderTargetView view) { ((CRenderTargetView)view).Dispose(); }
        protected override void DisposeSrv(IShaderResourceView view) { ((CShaderResourceView)view).Dispose(); }
        protected override void DisposeUav(IUnorderedAccessView view) { ((CUnorderedAccessView)view).Dispose(); }

        protected override bool DescEqualsRtv(IRenderTargetView view, ref RenderTargetViewDescription viewDescription) { return ((CRenderTargetView)view).DescEquals(ref viewDescription); }
        protected override bool DescEqualsSrv(IShaderResourceView view, ref ShaderResourceViewDescription viewDescription) { return ((CShaderResourceView)view).DescEquals(ref viewDescription); }
        protected override bool DescEqualsUav(IUnorderedAccessView view, ref UnorderedAccessViewDescription viewDescription) { return ((CUnorderedAccessView)view).DescEquals(ref viewDescription); }

        protected override IRenderTargetView CreateRtv(ref RenderTargetViewDescription viewDescription) { return new CRenderTargetView(this, ref viewDescription); }
        protected override IShaderResourceView CreateSrv(ref ShaderResourceViewDescription viewDescription) { return new CShaderResourceView(this, ref viewDescription); }
        protected override IUnorderedAccessView CreateUav(ref UnorderedAccessViewDescription viewDescription) { return new CUnorderedAccessView(this, ref viewDescription); }
        #endregion
    }
}
