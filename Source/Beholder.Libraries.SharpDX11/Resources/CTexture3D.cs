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
using Beholder.Libraries.SharpDX11.Core;
using Beholder.Platform;
using Beholder.Resources;
using Beholder.Utility.ForImplementations.Resources;
using Beholder.Utility.Helpers;
using SharpDX;
using SharpDX.DXGI;
using SharpDX.Direct3D11;
using CpuAccessFlags = SharpDX.Direct3D11.CpuAccessFlags;
using MapFlags = SharpDX.Direct3D11.MapFlags;
using RenderTargetViewDescription = Beholder.Resources.RenderTargetViewDescription;
using Resource = SharpDX.Direct3D11.Resource;
using ShaderResourceViewDescription = Beholder.Resources.ShaderResourceViewDescription;
using Texture3DDescription = Beholder.Resources.Texture3DDescription;
using UnorderedAccessViewDescription = Beholder.Resources.UnorderedAccessViewDescription;
using Usage = Beholder.Resources.Usage;

namespace Beholder.Libraries.SharpDX11.Resources
{
    class CTexture3D : Texture3DBase<ICDevice>, ICResource
    {
        readonly Texture3D d3dTexture3D;

        public Texture3D D3DTexture3D { get { return d3dTexture3D; } }
        public Resource D3DResource { get { return d3dTexture3D; } }

        public unsafe CTexture3D(ICDevice device, ref Texture3DDescription desc, SubresourceData[] initialData, Action<CTexture3D> onRelease)
            : base(device, ref desc, t => onRelease((CTexture3D)t))
        {
            var d3dDesc = new SharpDX.Direct3D11.Texture3DDescription
            {
                Width = desc.Width,
                Height = desc.Height,
                Depth = desc.Depth,
                MipLevels = desc.MipLevels,
                Format = (Format)desc.FormatID,
                Usage = CtSharpDX11.ResourceUsage(desc.Usage),
                BindFlags = CtSharpDX11.BindFlags(desc.BindFlags),
                CpuAccessFlags = (desc.Usage & Usage.Dynamic) != 0 ? CpuAccessFlags.Write : CpuAccessFlags.None,
                OptionFlags = CtSharpDX11.ResourceOptionFlags(desc.MiscFlags)
            };

            if (initialData == null)
            {
                d3dTexture3D = new Texture3D(device.D3DDevice, d3dDesc);
            }
            else
            {
                int subresourceCount = desc.MipLevels;

                var boxes = new DataBox[subresourceCount];
                for (int i = 0; i < boxes.Length; i++)
                {
                    int rowPitch, slicePitch, totalSize;
                    LocalHelpers.GetTextureDataSettingParameters((Format)desc.FormatID, desc.Width, desc.Height, desc.Depth,
                        initialData[0].RowByteAlignment, out rowPitch, out slicePitch, out totalSize);
                    boxes[i] = new DataBox(initialData[i].Pointer, rowPitch, slicePitch);
                }

                d3dTexture3D = new Texture3D(device.D3DDevice, d3dDesc, boxes);

                for (int i = 0; i < subresourceCount; i++)
                    initialData[i].UnpinPointer();
            }
        }

        private CTexture3D(ICDevice device, Texture3D d3dTexture3D, ref Texture3DDescription desc, Action<CTexture3D> onRelease)
            : base(device, ref desc, t => onRelease((CTexture3D)t))
        {
            this.d3dTexture3D = d3dTexture3D;
        }

        protected override void DisposeOfNative()
        {
            d3dTexture3D.Dispose();
        }

        public unsafe void SetSubresourceData(CDeviceContext deviceContext, int subresourceIndex, SubresourceData data)
        {
            // todo: testtesttest
            int mipSlice = subresourceIndex % desc.MipLevels;
            int mipWidth = TextureHelper.MipSize(mipSlice, desc.Width);
            int mipHeight = TextureHelper.MipSize(mipSlice, desc.Height);
            int mipDepth = TextureHelper.MipSize(mipSlice, desc.Depth);

            int rowPitch, slicePitch, totalSize;
            LocalHelpers.GetTextureDataSettingParameters((Format)desc.FormatID, mipWidth, mipHeight, mipDepth,
                data.RowByteAlignment, out rowPitch, out slicePitch, out totalSize);

            switch (desc.Usage)
            {
                case Usage.Default:
                    {
                        deviceContext.UpdateSubresource(d3dTexture3D, subresourceIndex, null, data.Pointer, rowPitch, slicePitch);
                    }
                    break;
                case Usage.Dynamic:
                    {
                        var box = deviceContext.MapSubresource(d3dTexture3D, subresourceIndex, MapMode.WriteDiscard, MapFlags.None);
                        var destinationStartPointer = box.DataPointer;

                        if (box.SlicePitch == slicePitch && box.RowPitch == rowPitch)
                        {
                            Utilities.CopyMemory(destinationStartPointer, data.Pointer, totalSize);
                        }
                        else
                        {
                            for (int z = 0; z < mipDepth; z++)
                            {
                                if (box.RowPitch == rowPitch)
                                {
                                    var writePointer = destinationStartPointer + box.SlicePitch * z;
                                    Utilities.CopyMemory(writePointer, data.Pointer + slicePitch * z, System.Math.Min(box.SlicePitch, slicePitch));
                                }
                                else
                                {
                                    for (int y = 0; y < slicePitch / rowPitch; y++)
                                    {
                                        var writePointer = destinationStartPointer + box.SlicePitch * z + box.RowPitch * y;
                                        Utilities.CopyMemory(writePointer, data.Pointer + slicePitch * z + rowPitch * y, System.Math.Min(box.RowPitch, rowPitch));
                                    }
                                }
                            }
                        }
                        deviceContext.UnmapSubresource(d3dTexture3D, subresourceIndex);
                    }
                    break;
                default:
                    throw new InvalidOperationException("To set the resource data, its usage must be either Default or Dynamic.");
            }

            data.UnpinPointer();
        }

        #region For TextureXDBase
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

        public static CTexture3D FromFile(ICDevice device, IFileSystem fileSystem, string fileName, Action<CTexture3D> onRelease)
        {
            var d3dTexture3D = Resource.FromMemory<Texture3D>(device.D3DDevice, fileSystem.ReadBinary(fileName));
            var d3dDesc = d3dTexture3D.Description;
            var bDesc = new Texture3DDescription
            {
                Width = d3dDesc.Width,
                Height = d3dDesc.Height,
                Depth = d3dDesc.Depth,
                MipLevels = d3dDesc.MipLevels,
                FormatID = (int)d3dDesc.Format,
                Usage = CtBeholder.Usage(d3dDesc.Usage),
                BindFlags = CtBeholder.BindFlags(d3dDesc.BindFlags),
                MiscFlags = CtBeholder.MiscFlags(d3dDesc.OptionFlags),
                ExtraFlags = ExtraFlags.None
            };
            return new CTexture3D(device, d3dTexture3D, ref bDesc, onRelease);
        }
    }
}
