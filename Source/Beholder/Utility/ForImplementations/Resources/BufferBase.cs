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
    public abstract class BufferBase<TDevice> : DeviceChildBase<TDevice>, IBuffer, IDisposable where TDevice : IDevice
    {
        protected BufferDescription desc;

        readonly List<IRenderTargetView> rtvs;
        readonly List<IShaderResourceView> srvs;
        readonly List<IUnorderedAccessView> uavs;
        readonly Action<IBuffer> onRelease;
        bool isReleased;

        public ResourceDimension Dimension { get { return ResourceDimension.Buffer; } }
        public int SizeInBytes { get { return desc.SizeInBytes; } }
        public Usage Usage { get { return desc.Usage; } }
        public BindFlags BindFlags { get { return desc.BindFlags; } }
        public MiscFlags MiscFlags { get { return desc.MiscFlags; } }
        public int StructureByteStride { get { return desc.StructureByteStride; } }
        public ExtraFlags ExtraFlags { get { return desc.ExtraFlags; } }
        public void GetDescription(out BufferDescription description) { description = desc; }
        public bool IsReleased { get { return isReleased; } }

        protected BufferBase(TDevice device, ref BufferDescription desc, Action<IBuffer> onRelease)
            : base(device)
        {
            this.desc = desc;
            this.onRelease = onRelease;
            rtvs = new List<IRenderTargetView>();
            srvs = new List<IShaderResourceView>();
            uavs = new List<IUnorderedAccessView>();
        }

        protected abstract void DisposeOfNative();
        protected abstract void DisposeRtv(IRenderTargetView view);
        protected abstract void DisposeSrv(IShaderResourceView view);
        protected abstract void DisposeUav(IUnorderedAccessView view);

        public void Dispose()
        {
            foreach (var v in rtvs) { DisposeRtv(v); }
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

        public IRenderTargetView ViewAsRenderTarget(int formatID, int firstElement, int elementCount)
        {
            RenderTargetViewDescription viewDesc;
            RenderTargetViewDescription.CreateForBuffer(formatID, firstElement, elementCount, out viewDesc);
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

        public IShaderResourceView ViewAsShaderResource(int formatID, int firstElement, int elementCount)
        {
            ShaderResourceViewDescription viewDesc;
            ShaderResourceViewDescription.CreateForBuffer(formatID, firstElement, elementCount, out viewDesc);
            return GetSrv(ref viewDesc);
        }

        public IShaderResourceView ViewAsShaderResourceExtended(int formatID, int firstElement, int elementCount, ShaderResourceViewExtendedBufferFlags flags)
        {
            ShaderResourceViewDescription viewDesc;
            ShaderResourceViewDescription.CreateForBufferEx(formatID, firstElement, elementCount, flags, out viewDesc);
            return GetSrv(ref viewDesc);
        }
        #endregion

        #region View as Unordered Access Resource
        protected abstract bool DescEqualsUav(IUnorderedAccessView view, ref UnorderedAccessViewDescription viewDescription);
        protected abstract IUnorderedAccessView CreateUav(ref UnorderedAccessViewDescription viewDescription);

        IUnorderedAccessView GetUAV(ref UnorderedAccessViewDescription viewDesc)
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

        public IUnorderedAccessView ViewAsUnorderedAccessResource(int formatID, int firstElement, int elementCount, UnorderedAccessViewBufferFlags flags)
        {
            UnorderedAccessViewDescription viewDesc;
            UnorderedAccessViewDescription.CreateForBuffer(formatID, firstElement, elementCount, flags, out viewDesc);
            return GetUAV(ref viewDesc);
        }
        #endregion
    }
}
