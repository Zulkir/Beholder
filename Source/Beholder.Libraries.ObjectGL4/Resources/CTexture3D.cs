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
using Beholder.Utility.ForImplementations.Resources;
using Beholder.Utility.Helpers;
using ObjectGL;
using ObjectGL.GL42;
using IResource = ObjectGL.GL42.IResource;

namespace Beholder.Libraries.ObjectGL4.Resources
{
    sealed class CTexture3D : Texture3DBase<ICDevice>, ICResource
    {
        readonly Texture3D glResource;

        public IResource GLResource { get { return glResource; } }

        public CTexture3D(ICDevice device, ref Texture3DDescription desc, SubresourceData[] initialData, Action<CTexture3D> onRelease)
            : base(device, ref desc, t => onRelease((CTexture3D)t))
        {
            glResource = new Texture3D(device.GetCurrentContext(), desc.Width, desc.Height, desc.Depth, desc.MipLevels, (Format)desc.FormatID);
            if (initialData != null)
                for (int mipLevel = 0; mipLevel < desc.MipLevels; mipLevel++)
                    SetSubresourceData(device.GetCurrentContext(), mipLevel, initialData[mipLevel]);
        }

        void SetSubresourceData(Context glContext, int mipLevel, SubresourceData data)
        {
            var glFormat = (Format)desc.FormatID;
            if (!LocalHelpers.IsFormatCompressed(glFormat))
            {
                glResource.SetData(device.GetCurrentContext(), mipLevel, new Data(data.Pointer, data.UnpinPointer),
                    Texture.GetAppropriateFormatColor(glFormat), Texture.GetAppropriateFormatType(glFormat),
                    CtObjectGL.ByteAlignment(data.RowByteAlignment));
            }
            else
            {
                int totalSize = LocalHelpers.CompressedImageSize(glFormat, TextureHelper.MipSize(mipLevel, desc.Width), TextureHelper.MipSize(mipLevel, desc.Height), 
                    TextureHelper.MipSize(mipLevel, desc.Depth), data.RowByteAlignment);
                glResource.SetData(device.GetCurrentContext(), mipLevel, new Data(data.Pointer, data.UnpinPointer), totalSize);
            }
        }

        public void SetSubresourceData(CDeviceContext deviceContext, int subresourceIndex, SubresourceData data)
        {
            SetSubresourceData(deviceContext.GLContext, subresourceIndex, data);
        }

        #region Common TextureXDBase Implementation
        protected override void DisposeOfNative() { glResource.Dispose(); }
        protected override void DisposeRtv(IRenderTargetView view) { }
        protected override void DisposeSrv(IShaderResourceView view) { }
        protected override void DisposeUav(IUnorderedAccessView view) { }

        protected override bool DescEqualsRtv(IRenderTargetView view, ref RenderTargetViewDescription viewDescription) { return ((CRenderTargetView)view).DescEquals(ref viewDescription); }
        protected override IRenderTargetView CreateRtv(ref RenderTargetViewDescription viewDescription)
        {
            return glResource.ResourceType == ResourceType.Renderbuffer ?
                new CRenderTargetView(this, (Renderbuffer)(IResource)glResource, ref viewDescription) :
                new CRenderTargetView(this, (Texture)(IResource)glResource, ref viewDescription);
        }

        protected override bool DescEqualsSrv(IShaderResourceView view, ref ShaderResourceViewDescription viewDescription) { return ((CShaderResourceView)view).DescEquals(ref viewDescription); }
        protected override IShaderResourceView CreateSrv(ref ShaderResourceViewDescription viewDescription) { return new CShaderResourceView(this, (Texture)(IResource)glResource, ref viewDescription); }

        protected override bool DescEqualsUav(IUnorderedAccessView view, ref UnorderedAccessViewDescription viewDescription) { throw new NotImplementedException(); }
        protected override IUnorderedAccessView CreateUav(ref UnorderedAccessViewDescription viewDescription) { throw new NotImplementedException(); }
        #endregion
    }
}
