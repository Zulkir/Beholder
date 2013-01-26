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
using System.Collections.Generic;
using Beholder.Resources;

namespace Beholder.Utility.ForImplementations.Resources
{
    public abstract class Texture1DBase<TDevice> : DeviceChildBase<TDevice>, ITexture1D, IDisposable where TDevice : IDevice
    {
        protected Texture1DDescription desc;

        readonly List<IRenderTargetView> rtvs;
        readonly List<IDepthStencilView> dsvs;
        readonly List<IShaderResourceView> srvs;
        readonly List<IUnorderedAccessView> uavs;
        readonly Action<ITexture1D> onRelease;
        bool isReleased;

        public ResourceDimension Dimension { get { return ResourceDimension.Texture1D; } }
        public int Width { get { return desc.Width; } }
        public int MipLevels { get { return desc.MipLevels; } }
        public int ArraySize { get { return desc.ArraySize; } }
        public int FormatID { get { return desc.FormatID; } }
        public Usage Usage { get { return desc.Usage; } }
        public BindFlags BindFlags { get { return desc.BindFlags; } }
        public MiscFlags MiscFlags { get { return desc.MiscFlags; } }
        public ExtraFlags ExtraFlags { get { return desc.ExtraFlags; } }
        public void GetDescription(out Texture1DDescription description) { description = desc; }
        public bool IsReleased { get { return isReleased; } }

        protected Texture1DBase(TDevice device, ref Texture1DDescription desc, Action<ITexture1D> onRelease)
            : base(device)
        {
            this.desc = desc;
            this.onRelease = onRelease;
            rtvs = new List<IRenderTargetView>();
            dsvs = new List<IDepthStencilView>();
            srvs = new List<IShaderResourceView>();
            uavs = new List<IUnorderedAccessView>();
        }

        protected abstract void DisposeOfNative();
        protected abstract void DisposeRtv(IRenderTargetView view);
        protected abstract void DisposeDsv(IDepthStencilView view);
        protected abstract void DisposeSrv(IShaderResourceView view);
        protected abstract void DisposeUav(IUnorderedAccessView view);

        public void Dispose()
        {
            foreach (var v in rtvs) { DisposeRtv(v); }
            foreach (var v in dsvs) { DisposeDsv(v); }
            foreach (var v in srvs) { DisposeSrv(v); }
            foreach (var v in uavs) { DisposeUav(v); }
            DisposeOfNative();
            isReleased = true;
        }

        public void Release()
        {
            onRelease(this);
            Dispose();
        }

        #region View as Render Target
        protected abstract bool DescEqualsRtv(IRenderTargetView view, ref RenderTargetViewDescription viewDescription);
        protected abstract IRenderTargetView CreateRtv(ref RenderTargetViewDescription viewDescription);

        IRenderTargetView GetRtv(ref RenderTargetViewDescription viewDesc)
        {
            IRenderTargetView view = null;
            foreach (var v in rtvs) { if (DescEqualsRtv(v, ref viewDesc)) { view = v; break; } }
            if (view == null)
            {
                view = CreateRtv(ref viewDesc);
                rtvs.Add(view);
            }
            return view;
        }

        public IRenderTargetView ViewAsRenderTarget(int formatID, int mipSlice)
        {
            RenderTargetViewDescription viewDesc;
            RenderTargetViewDescription.CreateForTexture1D(formatID, mipSlice, out viewDesc);
            return GetRtv(ref viewDesc);
        }
        public IRenderTargetView ViewAsRenderTargetArray(int formatID, int mipSlice, int firstArraySlice, int arraySize)
        {
            RenderTargetViewDescription viewDesc;
            RenderTargetViewDescription.CreateForTexture1DArray(formatID, mipSlice, firstArraySlice, arraySize, out viewDesc);
            return GetRtv(ref viewDesc);
        }
        #endregion

        #region View as Depth Stencil
        protected abstract bool DescEqualsDsv(IDepthStencilView view, ref DepthStencilViewDescription viewDescription);
        protected abstract IDepthStencilView CreateDsv(ref DepthStencilViewDescription viewDescription);

        IDepthStencilView GetDsv(ref DepthStencilViewDescription viewDesc)
        {
            IDepthStencilView view = null;
            foreach (var v in dsvs) { if (DescEqualsDsv(v, ref viewDesc)) { view = v; break; } }
            if (view == null)
            {
                view = CreateDsv(ref viewDesc);
                dsvs.Add(view);
            }
            return view;
        }

        public IDepthStencilView ViewAsDepthStencil(int formatID, DepthStencilViewFlags flags, int mipSlice)
        {
            DepthStencilViewDescription viewDesc;
            DepthStencilViewDescription.CreateForTexture1D(formatID, flags, mipSlice, out viewDesc);
            return GetDsv(ref viewDesc);
        }
        public IDepthStencilView ViewAsDepthStencilArray(int formatID, DepthStencilViewFlags flags, int mipSlice, int firstArraySlice, int arraySize)
        {
            DepthStencilViewDescription viewDesc;
            DepthStencilViewDescription.CreateForTexture1DArray(formatID, flags, mipSlice, firstArraySlice, arraySize, out viewDesc);
            return GetDsv(ref viewDesc);
        }
        #endregion

        #region View as Shader Resource
        protected abstract bool DescEqualsSrv(IShaderResourceView view, ref ShaderResourceViewDescription viewDescription);
        protected abstract IShaderResourceView CreateSrv(ref ShaderResourceViewDescription viewDescription);

        IShaderResourceView GetSrv(ref ShaderResourceViewDescription viewDesc)
        {
            IShaderResourceView view = null;
            foreach (var v in srvs) { if (DescEqualsSrv(v, ref viewDesc)) { view = v; break; } }
            if (view == null)
            {
                view = CreateSrv(ref viewDesc);
                srvs.Add(view);
            }
            return view;
        }

        public IShaderResourceView ViewAsShaderResource(int formatID, int mostDetailedMip, int mipLevels)
        {
            ShaderResourceViewDescription viewDesc;
            ShaderResourceViewDescription.CreateForTexture1D(formatID, mostDetailedMip, mipLevels, out viewDesc);
            return GetSrv(ref viewDesc);
        }
        public IShaderResourceView ViewAsShaderResourceArray(int formatID, int mostDetailedMip, int mipLevels, int firstArraySlice, int arraySize)
        {
            ShaderResourceViewDescription viewDesc;
            ShaderResourceViewDescription.CreateForTexture1DArray(formatID, mostDetailedMip, mipLevels, firstArraySlice, arraySize, out viewDesc);
            return GetSrv(ref viewDesc);
        }
        #endregion

        #region View as Unordered Access Resource
        protected abstract bool DescEqualsUav(IUnorderedAccessView view, ref UnorderedAccessViewDescription viewDescription);
        protected abstract IUnorderedAccessView CreateUav(ref UnorderedAccessViewDescription viewDescription);

        IUnorderedAccessView GetUav(ref UnorderedAccessViewDescription viewDesc)
        {
            IUnorderedAccessView view = null;
            foreach (var v in uavs) { if (DescEqualsUav(v, ref viewDesc)) { view = v; break; } }
            if (view == null)
            {
                view = CreateUav(ref viewDesc);
                uavs.Add(view);
            }
            return view;
        }

        public IUnorderedAccessView ViewAsUnorderedAccessResource(int formatID, int mipSlice)
        {
            UnorderedAccessViewDescription viewDesc;
            UnorderedAccessViewDescription.CreateForTexture1D(formatID, mipSlice, out viewDesc);
            return GetUav(ref viewDesc);
        }
        public IUnorderedAccessView ViewAsUnorderedAccessResourceArray(int formatID, int mipSlice, int firstArraySlice, int arraySize)
        {
            UnorderedAccessViewDescription viewDesc;
            UnorderedAccessViewDescription.CreateForTexture1DArray(formatID, mipSlice, firstArraySlice, arraySize, out viewDesc);
            return GetUav(ref viewDesc);
        }
        #endregion
    }
}
