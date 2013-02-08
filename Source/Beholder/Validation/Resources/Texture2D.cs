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

namespace Beholder.Validation.Resources
{
    class Texture2D : Texture<ITexture2D>, ITexture2D
    {
        public Texture2D(ITexture2D real) : base(real) { }

        public int Width { get { CheckNotDisposed(); return Real.Width; } }
        public int Height { get { CheckNotDisposed(); return Real.Height; } }
        public int ArraySize { get { CheckNotDisposed(); return Real.ArraySize; } }
        public Sampling Sampling { get { CheckNotDisposed(); return Real.Sampling; } }
        public void GetDescription(out Texture2DDescription texture2DDesc){ CheckNotDisposed(); Real.GetDescription(out texture2DDesc); }

        #region View As
        public IRenderTargetView ViewAsRenderTarget(int formatID, int mipSlice)
        {
            CheckNotDisposed();
            CheckRenderTargetBinding();
            RenderTargetView.ValidateTexture2D(this, formatID, mipSlice);
            return Wrappers.Get(Real.ViewAsRenderTarget(formatID, mipSlice));
        }

        public IRenderTargetView ViewAsRenderTargetArray(int formatID, int mipSlice, int firstArraySlice, int arraySize)
        {
            CheckNotDisposed();
            CheckRenderTargetBinding();
            RenderTargetView.ValidateTexture2DArray(this, formatID, mipSlice, firstArraySlice, arraySize);
            return Wrappers.Get(Real.ViewAsRenderTargetArray(formatID, mipSlice, firstArraySlice, arraySize));
        }

        public IRenderTargetView ViewAsRenderTargetMultisampled(int formatID)
        {
            CheckNotDisposed();
            CheckRenderTargetBinding();
            RenderTargetView.ValidateTexture2DMultisampled(this, formatID);
            return Wrappers.Get(Real.ViewAsRenderTargetMultisampled(formatID));
        }

        public IRenderTargetView ViewAsRenderTargetMultisampledArray(int formatID, int firstArraySlice, int arraySize)
        {
            CheckNotDisposed();
            CheckRenderTargetBinding();
            RenderTargetView.ValidateTexture2DMultisampledArray(this, formatID, firstArraySlice, arraySize);
            return Wrappers.Get(Real.ViewAsRenderTargetMultisampledArray(formatID, firstArraySlice, arraySize));
        }

        public IDepthStencilView ViewAsDepthStencil(int formatID, DepthStencilViewFlags flags, int mipSlice)
        {
            CheckNotDisposed();
            CheckDepthStencilBinding();
            DepthStencilView.ValidateTexture2D(this, formatID, flags, mipSlice);
            return Wrappers.Get(Real.ViewAsDepthStencil(formatID, flags, mipSlice));
        }

        public IDepthStencilView ViewAsDepthStencilArray(int formatID, DepthStencilViewFlags flags, int mipSlice, int firstArraySlice, int arraySize)
        {
            CheckNotDisposed();
            CheckDepthStencilBinding();
            DepthStencilView.ValidateTexture2DArray(this, formatID, flags, mipSlice, firstArraySlice, arraySize);
            return Wrappers.Get(Real.ViewAsDepthStencilArray(formatID, flags, mipSlice, firstArraySlice, arraySize));
        }

        public IDepthStencilView ViewAsDepthStencilMultisampled(int formatID, DepthStencilViewFlags flags)
        {
            CheckNotDisposed();
            CheckDepthStencilBinding();
            DepthStencilView.ValidateTexture2DMultisampled(this, formatID, flags);
            return Wrappers.Get(Real.ViewAsDepthStencilMultisampled(formatID, flags));
        }

        public IDepthStencilView ViewAsDepthStencilMultisampledArray(int formatID, DepthStencilViewFlags flags, int firstArraySlice, int arraySize)
        {
            CheckNotDisposed();
            CheckDepthStencilBinding();
            DepthStencilView.ValidateTexture2DMultisampledArray(this, formatID, flags, firstArraySlice, arraySize);
            return Wrappers.Get(Real.ViewAsDepthStencilMultisampledArray(formatID, flags, firstArraySlice, arraySize));
        }

        public IShaderResourceView ViewAsShaderResource(int formatID, int mostDetailedMip, int mipLevels)
        {
            CheckNotDisposed();
            CheckShaderResourceBinding();
            ShaderResourceView.ValidateTexture2D(this, formatID, mostDetailedMip, mipLevels);
            return Wrappers.Get(Real.ViewAsShaderResource(formatID, mostDetailedMip, mipLevels));
        }

        public IShaderResourceView ViewAsShaderResourceArray(int formatID, int mostDetailedMip, int mipLevels, int firstArraySlice, int arraySize)
        {
            CheckNotDisposed();
            CheckShaderResourceBinding();
            ShaderResourceView.ValidateTexture2DArray(this, formatID, mostDetailedMip, mipLevels, firstArraySlice, arraySize);
            return Wrappers.Get(Real.ViewAsShaderResourceArray(formatID, mostDetailedMip, mipLevels, firstArraySlice, arraySize));
        }

        public IShaderResourceView ViewAsShaderResourceMultisampled(int formatID)
        {
            CheckNotDisposed();
            CheckShaderResourceBinding();
            ShaderResourceView.ValidateTexture2DMultisampled(this, formatID);
            return Wrappers.Get(Real.ViewAsShaderResourceMultisampled(formatID));
        }

        public IShaderResourceView ViewAsShaderResourceMultisampledArray(int formatID, int firstArraySlice, int arraySize)
        {
            CheckNotDisposed();
            CheckShaderResourceBinding();
            ShaderResourceView.ValidateTexture2DMultisampledArray(this, formatID, firstArraySlice, arraySize);
            return Wrappers.Get(Real.ViewAsShaderResourceMultisampledArray(formatID, firstArraySlice, arraySize));
        }

        public IShaderResourceView ViewAsShaderResourceCube(int formatID, int mostDetailedMip, int mipLevels)
        {
            CheckNotDisposed();
            CheckShaderResourceBinding();
            ShaderResourceView.ValidateTextureCube(this, formatID, mostDetailedMip, mipLevels);
            return Wrappers.Get(Real.ViewAsShaderResourceCube(formatID, mostDetailedMip, mipLevels));
        }

        public IShaderResourceView ViewAsShaderResourceCubeArray(int formatID, int mostDetailedMip, int mipLevels, int first2DArrayFace, int cubeCount)
        {
            CheckNotDisposed();
            CheckShaderResourceBinding();
            ShaderResourceView.ValidateTextureCubeArray(this, formatID, mostDetailedMip, mipLevels, first2DArrayFace, cubeCount);
            return Wrappers.Get(Real.ViewAsShaderResourceCubeArray(formatID, mostDetailedMip, mipLevels, first2DArrayFace, cubeCount));
        }

        public IUnorderedAccessView ViewAsUnorderedAccessResource(int formatID, int mipSlice)
        {
            CheckNotDisposed();
            CheckUnorderedAccessBinding();
            UnorderedAccessView.ValidateTexture2D(this, formatID, mipSlice);
            return Wrappers.Get(Real.ViewAsUnorderedAccessResource(formatID, mipSlice));
        }

        public IUnorderedAccessView ViewAsUnorderedAccessResourceArray(int formatID, int mipSlice, int firstArraySlice, int arraySize)
        {
            CheckNotDisposed();
            CheckUnorderedAccessBinding();
            UnorderedAccessView.ValidateTexture2DArray(this, formatID, mipSlice, firstArraySlice, arraySize);
            return Wrappers.Get(Real.ViewAsUnorderedAccessResourceArray(formatID, mipSlice, firstArraySlice, arraySize));
        }
        #endregion

        public static void ValidateDescription(IAdapter adapter, Texture2DDescription description)
        {
            var requiredSupport = FormatSupport.Texture2D;
            if (description.MipLevels > 0)
                requiredSupport |= FormatSupport.Mip;
            if (description.MiscFlags.HasFlag(MiscFlags.GenerateMips))
                requiredSupport |= FormatSupport.MipAutogen;
            if (description.MiscFlags.HasFlag(MiscFlags.TextureCube))
                requiredSupport |= FormatSupport.TextureCube;
            if (description.Sampling.Count != 1 && description.Sampling.Quality >= adapter.GetMultisampleQualityLevelsCount(description.FormatID, description.Sampling.Count))
                throw new ArgumentException(string.Format(
                    "Quality '{0}' for {1}-sample multisampling is not supported by the given format", 
                    description.Sampling.Quality, description.Sampling.Count));
            ValidateTextureDescription(adapter, description.Width, description.Height, 1, description.ArraySize, description.MipLevels,
                description.FormatID, requiredSupport, description.Usage, description.BindFlags, description.MiscFlags, description.ExtraFlags);
        }
    }
}
