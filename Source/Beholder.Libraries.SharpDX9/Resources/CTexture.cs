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
using System.Collections.Generic;
using Beholder.Platform;
using Beholder.Resources;
using Beholder.Utility.ForImplementations;
using SharpDX.Direct3D9;
using Usage = Beholder.Resources.Usage;

namespace Beholder.Libraries.SharpDX9.Resources
{
    abstract class CTexture : ITexture1D, ITexture2D, ICResource, IDefaultPoolResource, IDisposableInternal
    {
        readonly protected ICDevice device;
        readonly protected ResourceDimension dimension;
        readonly protected List<CRenderTargetView> rtvs;
        readonly protected List<CDepthStencilView> dsvs;
        readonly protected List<CShaderResourceView> srvs;
        protected Texture2DDescription desc;

        readonly Action<CTexture> onDispose;
        bool isDisposed;

        protected CTexture(ICDevice device, ResourceDimension dimension, Texture2DDescription description, Action<CTexture> onDispose)
        {
            this.device = device;
            this.dimension = dimension;
            this.desc = description;
            this.onDispose = onDispose;

            if (desc.BindFlags.HasFlag(BindFlags.RenderTarget))
                rtvs = new List<CRenderTargetView>();
            if (desc.BindFlags.HasFlag(BindFlags.DepthStencil))
                dsvs = new List<CDepthStencilView>();
            if (desc.BindFlags.HasFlag(BindFlags.ShaderResource))
                srvs = new List<CShaderResourceView>();
        }

        public void Dispose()
        {
            onDispose(this);
            DisposeInternal();
        }

        public void DisposeInternal()
        {
            DisposeConcrete();
            isDisposed = true;
        }

        #region Abstract
        public abstract void DisposeConcrete();
        public abstract void OnDeviceLost();
        public abstract void OnDeviceReset();
        public abstract void SetSubresourceData(int level, SubresourceData newData);
        protected abstract Surface GetSurfaceLevel(int arrayIndex, int mipLevel);
        protected abstract BaseTexture GetTexture();
        #endregion

        #region ICResource, IDeviceChild, and IReleasable
        public ICDevice Device { get { return device; } }
        IDevice IDeviceChild.Device { get { return device; } }
        public bool IsDisposed { get { return isDisposed; } }
        #endregion

        #region Resource Description
        ResourceDimension IResource.Dimension { get { return dimension; } }
        Usage IResource.Usage { get { return desc.Usage; } }
        BindFlags IResource.BindFlags { get { return desc.BindFlags; } }
        MiscFlags IResource.MiscFlags { get { return desc.MiscFlags; } }
        ExtraFlags IResource.ExtraFlags { get { return desc.ExtraFlags; } }
        #endregion

        #region Texture Description
        int ITexture.FormatID { get { return desc.FormatID; } }
        int ITexture.MipLevels { get { return desc.MipLevels; } }
        #endregion

        #region Texture1D Description
        int ITexture1D.Width { get { return desc.Width; } }
        int ITexture1D.ArraySize { get { return desc.ArraySize; } }
        void ITexture1D.GetDescription(out Texture1DDescription description)
        {
            description = new Texture1DDescription
            {
                Width = desc.Width,
                MipLevels = desc.MipLevels,
                ArraySize = desc.ArraySize,
                FormatID = desc.FormatID,
                Usage = desc.Usage,
                BindFlags = desc.BindFlags,
                MiscFlags = desc.MiscFlags,
                ExtraFlags = desc.ExtraFlags
            };
        }
        #endregion

        #region Texture2D Description
        int ITexture2D.Width { get { return desc.Width; } }
        int ITexture2D.Height { get { return desc.Height; } }
        int ITexture2D.ArraySize { get { return desc.ArraySize; } }
        Sampling ITexture2D.Sampling { get { return desc.Sampling; } }
        void ITexture2D.GetDescription(out Texture2DDescription texture2DDesc) { texture2DDesc = desc; }
        #endregion

        #region View as Render Target
        IRenderTargetView GetRtv(ref RenderTargetViewDescription viewDesc)
        {
            CRenderTargetView view = null;
            foreach (var v in rtvs) { if (v.DescEquals(ref viewDesc)) { view = v; break; } }
            if (view == null)
            {
                view = new CRenderTargetView(this, GetSurfaceLevel(viewDesc.FirstArraySlice, viewDesc.MipSlice), ref viewDesc);
                rtvs.Add(view);
            }
            return view;
        }

        public IRenderTargetView ViewAsRenderTarget(int formatID, int mipSlice)
        {
            RenderTargetViewDescription viewDesc;
            RenderTargetViewDescription.CreateForTexture2D(formatID, mipSlice, out viewDesc);
            return GetRtv(ref viewDesc);
        }

        public IRenderTargetView ViewAsRenderTargetArray(int formatID, int mipSlice, int firstArraySlice, int arraySize)
        {
            RenderTargetViewDescription viewDesc;
            RenderTargetViewDescription.CreateForTexture2DArray(formatID, mipSlice, firstArraySlice, arraySize, out viewDesc);
            return GetRtv(ref viewDesc);
        }

        public IRenderTargetView ViewAsRenderTargetMultisampled(int formatID)
        {
            RenderTargetViewDescription viewDesc;
            RenderTargetViewDescription.CreateForTexture2DMultisampled(formatID, out viewDesc);
            return GetRtv(ref viewDesc);
        }
        public IRenderTargetView ViewAsRenderTargetMultisampledArray(int formatID, int firstArraySlice, int arraySize)
        {
            RenderTargetViewDescription viewDesc;
            RenderTargetViewDescription.CreateForTexture2DMultisampledArray(formatID, firstArraySlice, arraySize, out viewDesc);
            return GetRtv(ref viewDesc);
        }

        IRenderTargetView ITexture1D.ViewAsRenderTarget(int formatID, int mipSlice)
        {
            return ViewAsRenderTarget(formatID, mipSlice);
        }

        IRenderTargetView ITexture1D.ViewAsRenderTargetArray(int formatID, int mipSlice, int firstArraySlice, int arraySize)
        {
            return ViewAsRenderTargetArray(formatID, mipSlice, firstArraySlice, arraySize);
        }
        #endregion

        #region View as Depth Stencil
        IDepthStencilView GetDsv(ref DepthStencilViewDescription viewDesc)
        {
            CDepthStencilView view = null;
            foreach (var v in dsvs) { if (v.DescEquals(ref viewDesc)) { view = v; break; } }
            if (view == null)
            {
                view = new CDepthStencilView(this, GetSurfaceLevel(viewDesc.FirstArraySlice, viewDesc.MipSlice), ref viewDesc);
                dsvs.Add(view);
            }
            return view;
        }

        public IDepthStencilView ViewAsDepthStencil(int formatID, DepthStencilViewFlags flags, int mipSlice)
        {
            DepthStencilViewDescription viewDesc;
            DepthStencilViewDescription.CreateForTexture2D(formatID, flags, mipSlice, out viewDesc);
            return GetDsv(ref viewDesc);
        }

        public IDepthStencilView ViewAsDepthStencilArray(int formatID, DepthStencilViewFlags flags, int mipSlice, int firstArraySlice, int arraySize)
        {
            DepthStencilViewDescription viewDesc;
            DepthStencilViewDescription.CreateForTexture2DArray(formatID, flags, mipSlice, firstArraySlice, arraySize, out viewDesc);
            return GetDsv(ref viewDesc);
        }

        public IDepthStencilView ViewAsDepthStencilMultisampled(int formatID, DepthStencilViewFlags flags)
        {
            DepthStencilViewDescription viewDesc;
            DepthStencilViewDescription.CreateForTexture2DMultisampled(formatID, flags, out viewDesc);
            return GetDsv(ref viewDesc);
        }

        public IDepthStencilView ViewAsDepthStencilMultisampledArray(int formatID, DepthStencilViewFlags flags, int firstArraySlice, int arraySize)
        {
            DepthStencilViewDescription viewDesc;
            DepthStencilViewDescription.CreateForTexture2DMultisampledArray(formatID, flags, firstArraySlice, arraySize, out viewDesc);
            return GetDsv(ref viewDesc);
        }

        IDepthStencilView ITexture1D.ViewAsDepthStencil(int formatID, DepthStencilViewFlags flags, int mipSlice)
        {
            return ViewAsDepthStencil(formatID, flags, mipSlice);
        }

        IDepthStencilView ITexture1D.ViewAsDepthStencilArray(int formatID, DepthStencilViewFlags flags, int mipSlice, int firstArraySlice, int arraySize)
        {
            return ViewAsDepthStencilArray(formatID, flags, mipSlice, firstArraySlice, arraySize);
        }
        #endregion

        #region View as Shader Resource
        IShaderResourceView GetSrv(ref ShaderResourceViewDescription viewDesc)
        {
            CShaderResourceView view = null;
            foreach (var v in srvs) { if (v.DescEquals(ref viewDesc)) { view = v; break; } }
            if (view == null)
            {
                view = new CShaderResourceView(this, GetTexture(), ref viewDesc);
                srvs.Add(view);
            }
            return view;
        }

        public IShaderResourceView ViewAsShaderResource(int formatID, int mostDetailedMip, int mipLevels)
        {
            ShaderResourceViewDescription viewDesc;
            ShaderResourceViewDescription.CreateForTexture2D(formatID, mostDetailedMip, mipLevels, out viewDesc);
            return GetSrv(ref viewDesc);
        }

        public IShaderResourceView ViewAsShaderResourceArray(int formatID, int mostDetailedMip, int mipLevels, int firstArraySlice, int arraySize)
        {
            ShaderResourceViewDescription viewDesc;
            ShaderResourceViewDescription.CreateForTexture2DArray(formatID, mostDetailedMip, mipLevels, firstArraySlice, arraySize, out viewDesc);
            return GetSrv(ref viewDesc);
        }

        public IShaderResourceView ViewAsShaderResourceMultisampled(int formatID)
        {
            ShaderResourceViewDescription viewDesc;
            ShaderResourceViewDescription.CreateForTexture2DMultisampled(formatID, out viewDesc);
            return GetSrv(ref viewDesc);
        }

        public IShaderResourceView ViewAsShaderResourceMultisampledArray(int formatID, int firstArraySlice, int arraySize)
        {
            ShaderResourceViewDescription viewDesc;
            ShaderResourceViewDescription.CreateForTexture2DMultisampledArray(formatID, firstArraySlice, arraySize, out viewDesc);
            return GetSrv(ref viewDesc);
        }

        public IShaderResourceView ViewAsShaderResourceCube(int formatID, int mostDetailedMip, int mipLevels)
        {
            ShaderResourceViewDescription viewDesc;
            ShaderResourceViewDescription.CreateForTextureCube(formatID, mostDetailedMip, mipLevels, out viewDesc);
            return GetSrv(ref viewDesc);
        }

        public IShaderResourceView ViewAsShaderResourceCubeArray(int formatID, int mostDetailedMip, int mipLevels, int first2DArrayFace, int cubeCount)
        {
            ShaderResourceViewDescription viewDesc;
            ShaderResourceViewDescription.CreateForTextureCubeArray(formatID, mostDetailedMip, mipLevels, first2DArrayFace, cubeCount, out viewDesc);
            return GetSrv(ref viewDesc);
        }

        IShaderResourceView ITexture1D.ViewAsShaderResource(int formatID, int mostDetailedMip, int mipLevels)
        {
            return ViewAsShaderResource(formatID, mostDetailedMip, mipLevels);
        }

        IShaderResourceView ITexture1D.ViewAsShaderResourceArray(int formatID, int mostDetailedMip, int mipLevels, int firstArraySlice, int arraySize)
        {
            return ViewAsShaderResourceArray(formatID, mostDetailedMip, mipLevels, firstArraySlice, arraySize);
        }
        #endregion

        #region View as Unordered Access Resource
        IUnorderedAccessView ITexture1D.ViewAsUnorderedAccessResource(int formatID, int mipSlice) { throw new ResourceBindingException(); }
        IUnorderedAccessView ITexture2D.ViewAsUnorderedAccessResource(int formatID, int mipSlice) { throw new ResourceBindingException(); }
        IUnorderedAccessView ITexture1D.ViewAsUnorderedAccessResourceArray(int formatID, int mipSlice, int firstArraySlice, int arraySize) { throw new ResourceBindingException(); }
        IUnorderedAccessView ITexture2D.ViewAsUnorderedAccessResourceArray(int formatID, int mipSlice, int firstArraySlice, int arraySize) { throw new ResourceBindingException(); }
        #endregion
    }
}
