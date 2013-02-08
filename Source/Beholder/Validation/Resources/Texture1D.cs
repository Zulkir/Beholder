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
    class Texture1D : Texture<ITexture1D>, ITexture1D
    {
        public Texture1D(ITexture1D real) : base(real) { }

        public int Width { get { CheckNotDisposed(); return Real.Width; } }
        public int ArraySize { get { CheckNotDisposed(); return Real.ArraySize; } }
        public void GetDescription(out Texture1DDescription description) { CheckNotDisposed(); Real.GetDescription(out description); }

        #region View As
        public IRenderTargetView ViewAsRenderTarget(int formatID, int mipSlice)
        {
            CheckNotDisposed();
            CheckRenderTargetBinding();
            RenderTargetView.ValidateTexture1D(this, formatID, mipSlice);
            return Wrappers.Get(Real.ViewAsRenderTarget(formatID, mipSlice));
        }

        public IRenderTargetView ViewAsRenderTargetArray(int formatID, int mipSlice, int firstArraySlice, int arraySize)
        {
            CheckNotDisposed();
            RenderTargetView.ValidateTexture1DArray(this, formatID, mipSlice, firstArraySlice, arraySize);
            return Wrappers.Get(Real.ViewAsRenderTargetArray(formatID, mipSlice, firstArraySlice, arraySize));
        }

        public IDepthStencilView ViewAsDepthStencil(int formatID, DepthStencilViewFlags flags, int mipSlice)
        {
            CheckNotDisposed();
            CheckRenderTargetBinding();
            DepthStencilView.ValidateTexture1D(this, formatID, flags, mipSlice);
            return Wrappers.Get(Real.ViewAsDepthStencil(formatID, flags, mipSlice));
        }

        public IDepthStencilView ViewAsDepthStencilArray(int formatID, DepthStencilViewFlags flags, int mipSlice, int firstArraySlice, int arraySize)
        {
            CheckNotDisposed();
            CheckDepthStencilBinding();
            DepthStencilView.ValidateTexture1DArray(this, formatID, flags, mipSlice, firstArraySlice, arraySize);
            return Wrappers.Get(Real.ViewAsDepthStencilArray(formatID, flags, mipSlice, firstArraySlice, arraySize));
        }

        public IShaderResourceView ViewAsShaderResource(int formatID, int mostDetailedMip, int mipLevels)
        {
            CheckNotDisposed();
            CheckShaderResourceBinding();
            ShaderResourceView.ValidateTexture1D(this, formatID, mostDetailedMip, mipLevels);
            return Wrappers.Get(Real.ViewAsShaderResource(formatID, mostDetailedMip, mipLevels));
        }

        public IShaderResourceView ViewAsShaderResourceArray(int formatID, int mostDetailedMip, int mipLevels, int firstArraySlice, int arraySize)
        {
            CheckNotDisposed();
            CheckShaderResourceBinding();
            ShaderResourceView.ValidateTexture1DArray(this, formatID, mostDetailedMip, mipLevels, firstArraySlice, arraySize);
            return Wrappers.Get(Real.ViewAsShaderResourceArray(formatID, mostDetailedMip, mipLevels, firstArraySlice, arraySize));
        }

        public IUnorderedAccessView ViewAsUnorderedAccessResource(int formatID, int mipSlice)
        {
            CheckNotDisposed();
            CheckUnorderedAccessBinding();
            UnorderedAccessView.ValidateTexture1D(this, formatID, mipSlice);
            return Wrappers.Get(Real.ViewAsUnorderedAccessResource(formatID, mipSlice));
        }

        public IUnorderedAccessView ViewAsUnorderedAccessResourceArray(int formatID, int mipSlice, int firstArraySlice, int arraySize)
        {
            CheckNotDisposed();
            CheckUnorderedAccessBinding();
            UnorderedAccessView.ValidateTexture1DArray(this, formatID, mipSlice, firstArraySlice, arraySize);
            return Wrappers.Get(Real.ViewAsUnorderedAccessResourceArray(formatID, mipSlice, firstArraySlice, arraySize));
        }
        #endregion

        public static void ValidateDescription(IAdapter adapter, Texture1DDescription description)
        {
            var requiredSupport = FormatSupport.Texture1D;
            if (description.MipLevels > 0)
                requiredSupport |= FormatSupport.Mip;
            if (description.MiscFlags.HasFlag(MiscFlags.GenerateMips))
                requiredSupport |= FormatSupport.MipAutogen;
            if (description.MiscFlags.HasFlag(MiscFlags.TextureCube))
                throw new ArgumentException("MiscFlags.TextureCube is not supported by 1D textures");
            ValidateTextureDescription(adapter, description.Width, 1, 1, description.ArraySize, description.MipLevels,
                description.FormatID, requiredSupport, description.Usage, description.BindFlags, description.MiscFlags, description.ExtraFlags);
        }
    }
}
