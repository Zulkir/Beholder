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
using Beholder.Platform;
using Beholder.Resources;
using Beholder.Utility.ForImplementations;
using SharpDX.Direct3D9;
using Usage = Beholder.Resources.Usage;

namespace Beholder.Libraries.SharpDX9.Resources
{
    public class CAutoDepthStencil : DeviceChildBase<ICDevice>, ICResource, ITexture2D
    {
        Surface d3dSurface;
        readonly CDepthStencilView mainView;
        readonly Texture2DDescription desc;

        [Obsolete("This method is only to implement IResource interface. Dispose() should be called instead.")]
        void IReleasable.Release() { throw new InvalidOperationException("Swap chain's auto depth-stencil buffer cannot be explicitly released."); }
        bool IReleasable.IsReleased { get { return d3dSurface == null; } }

        public void ReleaseIfExists()
        {
            if (d3dSurface == null) return;
            d3dSurface.Dispose();
            d3dSurface = null;
        }

        public CDepthStencilView MainView { get { return mainView; } }
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
        public void GetDescription(out Texture2DDescription tex2DDesc) { tex2DDesc = desc; }

        public CAutoDepthStencil(ICDevice device, int width, int height, int formatID, Sampling sampling)
            : base(device)
        {
            desc = new Texture2DDescription
            {
                Width = width,
                Height = height,
                MipLevels = 1,
                ArraySize = 1,
                FormatID = formatID,
                Sampling = sampling,
                Usage = Usage.Default,
                BindFlags = BindFlags.DepthStencil,
                MiscFlags = MiscFlags.None,
                ExtraFlags = ExtraFlags.None
            };

            DepthStencilViewDescription viewDesc;
            DepthStencilViewDescription.CreateForTexture2D(formatID, DepthStencilViewFlags.None, 0, out viewDesc);
            mainView = new CDepthStencilView(this, d3dSurface, ref viewDesc);
        }

        public void UpdateSurface(Surface newD3DSurface)
        {
            d3dSurface = newD3DSurface;
            mainView.D3DSurface = newD3DSurface;
        }

        public void SetSubresourceData(int subresourceIndex, SubresourceData newData) { throw new InvalidOperationException("Trying to explicitly set the newData of the swap chain auto depth-stencil buffer."); }

        public IRenderTargetView ViewAsRenderTarget(int formatID, int mipSlice) { throw new ResourceBindingException(); }
        public IRenderTargetView ViewAsRenderTargetArray(int formatID, int mipSlice, int firstArraySlice, int arraySize) { throw new ResourceBindingException(); }
        public IRenderTargetView ViewAsRenderTargetMultisampled(int formatID) { throw new ResourceBindingException(); }
        public IRenderTargetView ViewAsRenderTargetMultisampledArray(int formatID, int firstArraySlice, int arraySize) { throw new ResourceBindingException(); }

        public IDepthStencilView ViewAsDepthStencil(int formatID, DepthStencilViewFlags flags, int mipSlice) { throw new InvalidOperationException("Additional views cannot be created upon a swap chain's auto depth-stencil buffer."); }
        public IDepthStencilView ViewAsDepthStencilArray(int formatID, DepthStencilViewFlags flags, int mipSlice, int firstArraySlice, int arraySize) { throw new InvalidOperationException("Additional views cannot be created upon a swap chain's auto depth-stencil buffer."); }
        public IDepthStencilView ViewAsDepthStencilMultisampled(int formatID, DepthStencilViewFlags flags) { throw new InvalidOperationException("Additional views cannot be created upon a swap chain's auto depth-stencil buffer."); }
        public IDepthStencilView ViewAsDepthStencilMultisampledArray(int formatID, DepthStencilViewFlags flags, int firstArraySlice, int arraySize) { throw new InvalidOperationException("Additional views cannot be created upon a swap chain's auto depth-stencil buffer."); }

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