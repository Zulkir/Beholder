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
using ObjectGL.GL42;
using IResource = ObjectGL.GL42.IResource;

namespace Beholder.Libraries.ObjectGL4.Resources
{
    abstract class CTexture2DBase<T> : Texture2DBase<ICDevice>, ICResource where T : IResource
    {
        protected T glResource;

        public IResource GLResource { get { return glResource; } }

        protected CTexture2DBase(ICDevice device, ref Texture2DDescription desc, SubresourceData[] initialData, Action<ITexture2D> onDispose)
            : base(device, ref desc, onDispose)
        {
            CreateNative();
            if (initialData != null)
                for (int arrayIndex = 0; arrayIndex < desc.ArraySize; arrayIndex++)
                    for (int mipLevel = 0; mipLevel < desc.MipLevels; mipLevel++)
                        SetSubresourceData(device.GetCurrentContext(), arrayIndex, mipLevel, 
                            initialData[TextureHelper.EncodeSubresourceIndex(desc.MipLevels, arrayIndex, mipLevel)]);
        }

        protected abstract void CreateNative();
        protected abstract void SetSubresourceData(Context glContext, int arrayIndex, int mipLevel, SubresourceData data);

        public void SetSubresourceData(CDeviceContext deviceContext, int subresourceIndex, SubresourceData data)
        {
            int arrayIndex, mipLevel;
            TextureHelper.DecodeSubresourceIndex(desc.MipLevels, subresourceIndex, out arrayIndex, out mipLevel);
            SetSubresourceData(deviceContext.GLContext, arrayIndex, mipLevel,data);
        }

        #region Common TextureXDBase Implementation
        protected override void DisposeOfNative() { glResource.Dispose(); }
        protected override void DisposeRtv(IRenderTargetView view) { }
        protected override void DisposeDsv(IDepthStencilView view) { }
        protected override void DisposeSrv(IShaderResourceView view) { }
        protected override void DisposeUav(IUnorderedAccessView view) { }

        protected override bool DescEqualsRtv(IRenderTargetView view, ref RenderTargetViewDescription viewDescription) { return ((CRenderTargetView)view).DescEquals(ref viewDescription); }
        protected override IRenderTargetView CreateRtv(ref RenderTargetViewDescription viewDescription)
        {
            return glResource.ResourceType == ResourceType.Renderbuffer ?
                new CRenderTargetView(this, (Renderbuffer)(IResource)glResource, ref viewDescription) :
                new CRenderTargetView(this, (Texture)(IResource)glResource, ref viewDescription);
        }

        protected override bool DescEqualsDsv(IDepthStencilView view, ref DepthStencilViewDescription viewDescription) { return ((CDepthStencilView)view).DescEquals(ref viewDescription); }
        protected override IDepthStencilView CreateDsv(ref DepthStencilViewDescription viewDescription)
        {
            return glResource.ResourceType == ResourceType.Renderbuffer ?
                new CDepthStencilView(this, (Renderbuffer)(IResource)glResource, ref viewDescription) :
                new CDepthStencilView(this, (Texture)(IResource)glResource, ref viewDescription);
        }

        protected override bool DescEqualsSrv(IShaderResourceView view, ref ShaderResourceViewDescription viewDescription) { return ((CShaderResourceView)view).DescEquals(ref viewDescription); }
        protected override IShaderResourceView CreateSrv(ref ShaderResourceViewDescription viewDescription) { return new CShaderResourceView(this, (Texture)(IResource)glResource, ref viewDescription); }

        protected override bool DescEqualsUav(IUnorderedAccessView view, ref UnorderedAccessViewDescription viewDescription) { throw new NotImplementedException(); }
        protected override IUnorderedAccessView CreateUav(ref UnorderedAccessViewDescription viewDescription) { throw new NotImplementedException(); }
        #endregion
    }
}
