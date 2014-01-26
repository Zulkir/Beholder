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
using System.Linq;
using Beholder.Core;
using Beholder.Libraries.SharpDX11;
using Beholder.Libraries.SharpDX11.Core;
using Beholder.Libraries.SharpDX11.Platform;
using Beholder.Platform;
using SharpDX.DXGI;
using Beholder.Libraries.Windows.Forms;
using Device = SharpDX.Direct3D11.Device;
using SwapChainDescription = Beholder.Platform.SwapChainDescription;

namespace Beholder.Eyes.SharpDX11.Winforms
{
    class WinformsDevice : ICDevice, IDisposable
    {
        readonly Factory1 dxgiFactory;
        readonly Device d3dDevice;

        readonly WinformsEye eye;
        readonly CAdapter adapter;
        readonly DeviceInitializationFlags flags;
        readonly CSwapChain primarySwapChain;
        readonly CDeviceChildCreator creator;
        readonly CDeviceContext immediateContext;
        readonly CDeviceChildLoader loader;
        readonly List<CSwapChain> additionalSwapChains;

        public Device D3DDevice { get { return d3dDevice; } }
        public Factory1 DXGIFactory { get { return dxgiFactory; } }
        public CDeviceContext ImmediateContext { get { return immediateContext; } }

        public IEye Eye { get { return eye; } }
        public IAdapter Adapter { get { return adapter; } }
        public DeviceInitializationFlags Flags { get { return flags; } }
        public IPrimarySwapChain PrimarySwapChain { get { return primarySwapChain; } }
        public IDeviceChildCreator Create { get { return creator; } }
        public IDeviceChildLoader Load { get { return loader; } }
        IDeviceContext IDevice.ImmediateContext { get { return immediateContext; } }
        
        public WinformsDevice(Factory1 dxgiFactory, WinformsEye eye, CAdapter adapter,
            ControlWindow primaryWindow, SwapChainDescription primarySwapChainDesc,
            DeviceInitializationFlags flags, IFileSystem fileSystem)
        {
            this.flags = flags;
            this.eye = eye;
            this.dxgiFactory = dxgiFactory;
            this.adapter = adapter;

            d3dDevice = new Device(adapter.DXGIAdapter, CtSharpDX11.DeviceCreationFlags(flags));

            primarySwapChain = new CSwapChain(this, primaryWindow, ref primarySwapChainDesc, 
                pswc => { }, () => !primaryWindow.IsVisible && additionalSwapChains.All(aswc => !aswc.Window.IsVisible));

            creator = new CDeviceChildCreator(this);
            loader = new CDeviceChildLoader(this, fileSystem);
            immediateContext = new CDeviceContext(this, d3dDevice.ImmediateContext);

            additionalSwapChains = new List<CSwapChain>();
        }

        public void Dispose()
        {
            immediateContext.Dispose();
            loader.Dispose();
            creator.Dispose();
            foreach (var swc in additionalSwapChains)
                swc.DisposeFinal();
            primarySwapChain.DisposeFinal();
            d3dDevice.Dispose();
        }

        void OnAdditionalSwapChainDisposed(CSwapChain swc)
        {
            additionalSwapChains.Remove(swc);
        }

        public IAdditionalSwapChain CreateAdditionalSwapChain(IWindowHandle windowHandle, SwapChainDescription swapChainDesc)
        {
            var window = (ControlWindow) windowHandle;
            var aswc = new CSwapChain(this, window, ref swapChainDesc, OnAdditionalSwapChainDisposed, () => false);
            window.SetSwapChain(aswc);
            additionalSwapChains.Add(aswc);
            return aswc;
        }
    }
}
