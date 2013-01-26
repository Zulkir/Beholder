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
using Beholder.Utility.ForImplementations;
using SharpDX.Direct3D11;
using BindFlags = Beholder.Resources.BindFlags;
using DepthStencilViewDescription = Beholder.Resources.DepthStencilViewDescription;
using DepthStencilViewFlags = Beholder.Resources.DepthStencilViewFlags;
using ResourceDimension = Beholder.Resources.ResourceDimension;
using Texture2DDescription = Beholder.Resources.Texture2DDescription;

namespace Beholder.Libraries.SharpDX11.Resources
{
    class CAutoDepthStencil : DeviceChildBase<ICDevice>, ICResource, ITexture2D
    {
        readonly Texture2D d3dTexture2D;
        readonly CDepthStencilView depthStencilView;
        Texture2DDescription desc;
        bool isReleased;

        public Texture2D D3DTexture2D { get { return d3dTexture2D; } }
        public Resource D3DResource { get { return d3dTexture2D; } }
        public CDepthStencilView MainView { get { return depthStencilView; } }

        public ResourceDimension Dimension { get { return ResourceDimension.Texture2D; } }
        public int Width { get { return desc.Width; } }
        public int Height { get { return desc.Height; } }
        public int MipLevels { get { return desc.MipLevels; } }
        public int ArraySize { get { return desc.ArraySize; } }
        public int FormatID { get { return desc.FormatID; } }
        public Sampling Sampling { get { return desc.Sampling; } }
        public Usage Usage { get { return desc.Usage; } }
        public BindFlags BindFlags { get { return desc.BindFlags; } }
        public MiscFlags MiscFlags { get { return desc.MiscFlags; } }
        public ExtraFlags ExtraFlags { get { return desc.ExtraFlags; } }
        public void GetDescription(out Texture2DDescription texture2DDesc) { texture2DDesc = desc; }

        public void Dispose()
        {
            depthStencilView.Dispose();
            d3dTexture2D.Dispose();
            isReleased = true;
        }

        [Obsolete("This method is only to implement IResource interface. Dispose() should be called instead.")]
        void IReleasable.Release() { throw new InvalidOperationException("Swap chain's auto depth-stencil buffer cannot be explicitly released."); }
        bool IReleasable.IsReleased { get { return isReleased; } }

        public CAutoDepthStencil(ICDevice device, Texture2D d3dTexture2D)
            : base(device)
        {
            this.d3dTexture2D = d3dTexture2D;

            SharpDX.Direct3D11.Texture2DDescription d3dDesc = d3dTexture2D.Description;

            desc = new Texture2DDescription
            {
                Width = d3dDesc.Width,
                Height = d3dDesc.Height,
                MipLevels = d3dDesc.MipLevels,
                ArraySize = d3dDesc.ArraySize,
                FormatID = (int)d3dDesc.Format,
                Sampling = CtBeholder.Sampling(d3dDesc.SampleDescription),
                Usage = CtBeholder.Usage(d3dDesc.Usage),
                BindFlags = CtBeholder.BindFlags(d3dDesc.BindFlags),
                MiscFlags = CtBeholder.MiscFlags(d3dDesc.OptionFlags),
                ExtraFlags = ExtraFlags.None
            };

            DepthStencilViewDescription dsvDesc;
            if (desc.Sampling == Sampling.NoMultisampling)
            {
                DepthStencilViewDescription.CreateForTexture2D(desc.FormatID, DepthStencilViewFlags.None, 0, out dsvDesc);
            }
            else
            {
                DepthStencilViewDescription.CreateForTexture2DMultisampled(desc.FormatID, DepthStencilViewFlags.None, out dsvDesc);
            }

            depthStencilView = new CDepthStencilView(this, ref dsvDesc);
        }


        public void SetSubresourceData(CDeviceContext deviceContext, int subresourceIndex, SubresourceData data)
        { throw new InvalidOperationException("Trying to explicitly set the data of the swap chain auto depth-stencil buffer."); }

        public IRenderTargetView ViewAsRenderTarget(int formatID, int mipSlice) { throw new ResourceBindingException(); }
        public IRenderTargetView ViewAsRenderTargetArray(int formatID, int mipSlice, int firstArraySlice, int arraySize) { throw new ResourceBindingException(); }
        public IRenderTargetView ViewAsRenderTargetMultisampled(int formatID) { throw new ResourceBindingException(); }
        public IRenderTargetView ViewAsRenderTargetMultisampledArray(int formatID, int firstArraySlice, int arraySize) { throw new ResourceBindingException(); }

        public IDepthStencilView ViewAsDepthStencil(int formatID, DepthStencilViewFlags flags, int mipSlice)
        { throw new InvalidOperationException("Additional views cannot be created upon a swap chain's auto depth-stencil buffer."); }
        public IDepthStencilView ViewAsDepthStencilArray(int formatID, DepthStencilViewFlags flags, int mipSlice, int firstArraySlice, int arraySize)
        { throw new InvalidOperationException("Additional views cannot be created upon a swap chain's auto depth-stencil buffer."); }
        public IDepthStencilView ViewAsDepthStencilMultisampled(int formatID, DepthStencilViewFlags flags)
        { throw new InvalidOperationException("Additional views cannot be created upon a swap chain's auto depth-stencil buffer."); }
        public IDepthStencilView ViewAsDepthStencilMultisampledArray(int formatID, DepthStencilViewFlags flags, int firstArraySlice, int arraySize)
        { throw new InvalidOperationException("Additional views cannot be created upon a swap chain's auto depth-stencil buffer."); }

        public IShaderResourceView ViewAsShaderResource(int formatID, int mostDetailedMip, int mipLevels) { throw new ResourceBindingException(); }
        public IShaderResourceView ViewAsShaderResourceArray(int formatID, int mostDetailedMip, int mipLevels, int firstArraySlice, int arraySize) { throw new ResourceBindingException(); }
        public IShaderResourceView ViewAsShaderResourceMultisampled(int formatID) { throw new ResourceBindingException(); }
        public IShaderResourceView ViewAsShaderResourceMultisampledArray(int formatID, int firstArraySlice, int arraySize) { throw new ResourceBindingException(); }
        public IShaderResourceView ViewAsShaderResourceCube(int formatID, int mostDetailedMip, int mipLevels) { throw new ResourceBindingException(); }
        public IShaderResourceView ViewAsShaderResourceCubeArray(int formatID, int mostDetailedMip, int mipLevels, int first2DArrayFace, int cubeCount) { throw new ResourceBindingException(); }

        public IUnorderedAccessView ViewAsUnorderedAccessResource(int formatID, int mipSlice) { throw new ResourceBindingException(); }
        public IUnorderedAccessView ViewAsUnorderedAccessResourceArray(int formatID, int mipSlice, int firstArraySlice, int arraySize) { throw new ResourceBindingException(); }
    }
}
