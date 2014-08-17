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
using Beholder.Utility.Helpers;
using SharpDX;
using SharpDX.DXGI;
using SharpDX.Direct3D11;
using DepthStencilViewDescription = Beholder.Resources.DepthStencilViewDescription;
using MapFlags = SharpDX.Direct3D11.MapFlags;
using RenderTargetViewDescription = Beholder.Resources.RenderTargetViewDescription;
using Resource = SharpDX.Direct3D11.Resource;
using ShaderResourceViewDescription = Beholder.Resources.ShaderResourceViewDescription;
using Texture2DDescription = Beholder.Resources.Texture2DDescription;
using UnorderedAccessViewDescription = Beholder.Resources.UnorderedAccessViewDescription;
using Usage = Beholder.Resources.Usage;

namespace Beholder.Libraries.SharpDX11.Resources
{
    public class CTexture2D : Texture2DBase<ICDevice>, ICResource
    {
        readonly Texture2D d3dTexture2D;

        public Texture2D D3DTexture2D { get { return d3dTexture2D; } }
        public Resource D3DResource { get { return d3dTexture2D; } }

        public CTexture2D(ICDevice device, ref Texture2DDescription desc, SubresourceData[] initialData, Action<CTexture2D> onRelease)
            : base(device, ref desc, t => onRelease((CTexture2D)t))
        {
            var d3dDesc = new SharpDX.Direct3D11.Texture2DDescription
            {
                Width = desc.Width,
                Height = desc.Height,
                MipLevels = desc.MipLevels,
                ArraySize = desc.ArraySize,
                Format = (Format)desc.FormatID,
                SampleDescription = CtSharpDX11.SampleDescription(desc.Sampling),
                Usage = CtSharpDX11.ResourceUsage(desc.Usage),
                BindFlags = CtSharpDX11.BindFlags(desc.BindFlags),
                CpuAccessFlags = CtSharpDX11.CpuAccessFlags(desc.Usage),
                OptionFlags = CtSharpDX11.ResourceOptionFlags(desc.MiscFlags)
            };

            if (initialData == null)
            {
                d3dTexture2D = new Texture2D(device.D3DDevice, d3dDesc);
            }
            else
            {
                int subresourceCount = desc.ArraySize * desc.MipLevels;

                var boxes = new DataBox[subresourceCount];
                for (int i = 0; i < boxes.Length; i++)
                {
                    int rowPitch, slicePitch, totalSize;
                    LocalHelpers.GetTextureDataSettingParameters((Format)desc.FormatID, 
                        TextureHelper.MipSize(i, desc.Width), TextureHelper.MipSize(i, desc.Height), 1,
                        initialData[0].RowByteAlignment, out rowPitch, out slicePitch, out totalSize);
                    boxes[i] = new DataBox(initialData[i].Pointer, rowPitch, 0);
                }

                d3dTexture2D = new Texture2D(device.D3DDevice, d3dDesc, boxes);

                for (int i = 0; i < subresourceCount; i++)
                    initialData[i].UnpinPointer();
            }
        }

        public CTexture2D(ICDevice device, Texture2D d3dTexture2D, ref Texture2DDescription desc, Action<CTexture2D> onRelease)
            : base(device, ref desc, t => onRelease((CTexture2D)t))
        {
            this.d3dTexture2D = d3dTexture2D;
        }

        protected override void DisposeOfNative()
        {
            d3dTexture2D.Dispose();
        }

        public void SetSubresourceData(CDeviceContext deviceContext, int subresourceIndex, SubresourceData data)
        {
            // todo: testtesttest
            int mipSlice = subresourceIndex % desc.MipLevels;
            int mipWidth = TextureHelper.MipSize(mipSlice, desc.Width);
            int mipHeight = TextureHelper.MipSize(mipSlice, desc.Height);

            int rowPitch, slicePitch, totalSize;
            LocalHelpers.GetTextureDataSettingParameters((Format)desc.FormatID, mipWidth, mipHeight, 1,
                data.RowByteAlignment, out rowPitch, out slicePitch, out totalSize);

            switch (desc.Usage)
            {
                case Usage.Default:
                    {
                        deviceContext.UpdateSubresourceInternal(d3dTexture2D, subresourceIndex, null, data.Pointer, rowPitch, 0);
                    }
                    break;
                case Usage.Dynamic:
                    {
                        var box = deviceContext.MapSubresourceInternal(d3dTexture2D, subresourceIndex, MapMode.WriteDiscard, MapFlags.None);
                        var destinationStartPointer = box.DataPointer;

                        if (box.RowPitch == rowPitch)
                        {
                            Utilities.CopyMemory(destinationStartPointer, data.Pointer, totalSize);
                        }
                        else
                        {
                            int sizeToWrite = System.Math.Min(box.RowPitch, rowPitch);
                            for (int y = 0; y < slicePitch / rowPitch; y++)
                            {
                                var writePointer = destinationStartPointer + box.RowPitch * y;
                                Utilities.CopyMemory(writePointer, data.Pointer + rowPitch * y, sizeToWrite);
                            }
                        }
                        deviceContext.UnmapSubresourceInternal(d3dTexture2D, subresourceIndex);
                    }
                    break;
                default:
                    throw new InvalidOperationException("To set the resource data, its usage must be either Default or Dynamic.");
            }

            data.UnpinPointer();
        }

        #region For TextureXDBase
        protected override void DisposeRtv(IRenderTargetView view) { ((CRenderTargetView)view).Dispose(); }
        protected override void DisposeDsv(IDepthStencilView view) { ((CDepthStencilView)view).Dispose(); }
        protected override void DisposeSrv(IShaderResourceView view) { ((CShaderResourceView)view).Dispose(); }
        protected override void DisposeUav(IUnorderedAccessView view) { ((CUnorderedAccessView)view).Dispose(); }

        protected override bool DescEqualsRtv(IRenderTargetView view, ref RenderTargetViewDescription viewDescription) { return ((CRenderTargetView)view).DescEquals(ref viewDescription); }
        protected override bool DescEqualsDsv(IDepthStencilView view, ref DepthStencilViewDescription viewDescription) { return ((CDepthStencilView)view).DescEquals(ref viewDescription); }
        protected override bool DescEqualsSrv(IShaderResourceView view, ref ShaderResourceViewDescription viewDescription) { return ((CShaderResourceView)view).DescEquals(ref viewDescription); }
        protected override bool DescEqualsUav(IUnorderedAccessView view, ref UnorderedAccessViewDescription viewDescription) { return ((CUnorderedAccessView)view).DescEquals(ref viewDescription); }

        protected override IRenderTargetView CreateRtv(ref RenderTargetViewDescription viewDescription) { return new CRenderTargetView(this, ref viewDescription); }
        protected override IDepthStencilView CreateDsv(ref DepthStencilViewDescription viewDescription) { return new CDepthStencilView(this, ref viewDescription); }
        protected override IShaderResourceView CreateSrv(ref ShaderResourceViewDescription viewDescription) { return new CShaderResourceView(this, ref viewDescription); }
        protected override IUnorderedAccessView CreateUav(ref UnorderedAccessViewDescription viewDescription) { return new CUnorderedAccessView(this, ref viewDescription); }
        #endregion
    }
}


