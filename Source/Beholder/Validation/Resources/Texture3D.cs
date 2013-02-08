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
    class Texture3D : Texture<ITexture3D>, ITexture3D
    {
        public Texture3D(ITexture3D real) : base(real) { }

        public int Width { get { CheckNotDisposed(); return Real.Width; } }
        public int Height { get { CheckNotDisposed(); return Real.Height; } }
        public int Depth { get { CheckNotDisposed(); return Real.Depth; } }
        public void GetDescription(out Texture3DDescription description) { CheckNotDisposed(); Real.GetDescription(out description); }

        #region View As
        public IRenderTargetView ViewAsRenderTarget(int formatID, int mipSlice, int firstDepthSlice, int depthSliceCount)
        {
            CheckNotDisposed();
            RenderTargetView.ValidateTexture3D(this, formatID, mipSlice, firstDepthSlice, depthSliceCount);
            return Wrappers.Get(Real.ViewAsRenderTarget(formatID, mipSlice, firstDepthSlice, depthSliceCount));
        }

        public IShaderResourceView ViewAsShaderResource(int formatID, int mostDetailedMip, int mipLevels)
        {
            CheckNotDisposed();
            ShaderResourceView.ValidateTexture3D(this, formatID, mostDetailedMip, mipLevels);
            return Wrappers.Get(Real.ViewAsShaderResource(formatID, mostDetailedMip, mipLevels));
        }

        public IUnorderedAccessView ViewAsUnorderedAccessResource(int formatID, int mipSlice, int firstDepthSlice, int depthSliceCount)
        {
            CheckNotDisposed();
            UnorderedAccessView.ValidateTexture3D(this, formatID, mipSlice, firstDepthSlice, depthSliceCount);
            return Wrappers.Get(Real.ViewAsUnorderedAccessResource(formatID, mipSlice, firstDepthSlice, depthSliceCount));
        }
        #endregion

        public static void ValidateDescription(IAdapter adapter, Texture3DDescription description)
        {
            var requiredSupport = FormatSupport.Texture3D;
            if (description.MipLevels > 0)
                requiredSupport |= FormatSupport.Mip;
            if (description.MiscFlags.HasFlag(MiscFlags.GenerateMips))
                requiredSupport |= FormatSupport.MipAutogen;
            if (description.MiscFlags.HasFlag(MiscFlags.TextureCube))
                throw new ArgumentException("MiscFlags.TextureCube is not supported by 1D textures");
            ValidateTextureDescription(adapter, description.Width, description.Height, description.Depth, 1, description.MipLevels,
                description.FormatID, requiredSupport, description.Usage, description.BindFlags, description.MiscFlags, description.ExtraFlags);
        }
    }
}
