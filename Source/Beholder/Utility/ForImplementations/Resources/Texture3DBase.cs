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
using Beholder.Resources;

namespace Beholder.Utility.ForImplementations.Resources
{
    public abstract class Texture3DBase<TDevice> : DeviceChildBase<TDevice>, ITexture3D, IDisposableInternal where TDevice : IDevice
    {
        protected Texture3DDescription desc;

        readonly List<IRenderTargetView> rtvs;
        readonly List<IShaderResourceView> srvs;
        readonly List<IUnorderedAccessView> uavs;
        readonly Action<ITexture3D> onDispose;
        bool isDisposed;

        public ResourceDimension Dimension { get { return ResourceDimension.Texture3D; } }
        public int Width { get { return desc.Width; } }
        public int Height { get { return desc.Height; } }
        public int Depth { get { return desc.Depth; } }
        public int MipLevels { get { return desc.MipLevels; } }
        public int FormatID { get { return desc.FormatID; } }
        public Usage Usage { get { return desc.Usage; } }
        public BindFlags BindFlags { get { return desc.BindFlags; } }
        public MiscFlags MiscFlags { get { return desc.MiscFlags; } }
        public ExtraFlags ExtraFlags { get { return desc.ExtraFlags; } }
        public void GetDescription(out Texture3DDescription description) { description = desc; }
        public bool IsDisposed { get { return isDisposed; } }

        protected Texture3DBase(TDevice device, ref Texture3DDescription desc, Action<ITexture3D> onDispose)
            : base(device)
        {
            this.desc = desc;
            this.onDispose = onDispose;
            rtvs = new List<IRenderTargetView>();
            srvs = new List<IShaderResourceView>();
            uavs = new List<IUnorderedAccessView>();
        }

        protected abstract void DisposeOfNative();
        protected abstract void DisposeRtv(IRenderTargetView view);
        protected abstract void DisposeSrv(IShaderResourceView view);
        protected abstract void DisposeUav(IUnorderedAccessView view);

        public void DisposeInternal()
        {
            foreach (var v in rtvs) { DisposeRtv(v); }
            foreach (var v in srvs) { DisposeSrv(v); }
            foreach (var v in uavs) { DisposeUav(v); }
            DisposeOfNative();
            isDisposed = true;
        }

        public void Dispose()
        {
            onDispose(this);
            DisposeInternal();
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

        public IRenderTargetView ViewAsRenderTarget(int formatID, int mipSlice, int firstDepthSlice, int depthSliceCount)
        {
            RenderTargetViewDescription viewDesc;
            RenderTargetViewDescription.CreateForTexture3D(formatID, mipSlice, firstDepthSlice, depthSliceCount, out viewDesc);
            return GetRtv(ref viewDesc);
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
            ShaderResourceViewDescription.CreateForTexture3D(formatID, mostDetailedMip, mipLevels, out viewDesc);
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

        public IUnorderedAccessView ViewAsUnorderedAccessResource(int formatID, int mipSlice, int firstDepthSlice, int depthSliceCount)
        {
            UnorderedAccessViewDescription viewDesc;
            UnorderedAccessViewDescription.CreateForTexture3D(formatID, mipSlice, firstDepthSlice, depthSliceCount, out viewDesc);
            return GetUav(ref viewDesc);
        }
        #endregion
    }
}
