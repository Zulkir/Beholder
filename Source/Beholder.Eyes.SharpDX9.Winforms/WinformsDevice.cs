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
using System.Threading;
using Beholder.Core;
using Beholder.Libraries.SharpDX9;
using Beholder.Libraries.SharpDX9.Core;
using Beholder.Libraries.SharpDX9.Platform;
using Beholder.Libraries.SharpDX9.Resources;
using Beholder.Libraries.Windows;
using Beholder.Libraries.Windows.Forms;
using Beholder.Math;
using Beholder.Platform;
using Beholder.Resources;
using SharpDX.Direct3D9;
using D3DFormat = SharpDX.Direct3D9.Format;
using BDisplayMode = Beholder.Platform.DisplayMode;

namespace Beholder.Eyes.SharpDX9.Winforms
{
    class WinformsDevice : ICDevice, IPrimarySwapChain, IDisposable
    {
        readonly WinformsEye eye;
        readonly CAdapter adapter;
        readonly ICWindow primaryWindow;

        readonly Device d3dDevice;
        readonly DeviceInitializationFlags flags;
        readonly Capabilities caps;
        PresentParameters devicePresentParams;
        
        readonly List<CAdditionalSwapChain> additionalSwapChains;
        readonly CDeviceChildCreator creator;
        readonly CDeviceContext immediateContext;

        CBackBuffer implicitBackBuffer;
        CAutoDepthStencil implicitDepthStencilBuffer;

        SwapChainDescription implicitSwapChainDesc;
        IntSize lastSwapChainSize;
        FullscreenState fullscreenState;
        BDisplayMode fullscreenDisplayMode;

        public Device D3DDevice { get { return d3dDevice; } }
        public CDeviceContext ImmediateContext { get { return immediateContext; } }

        public IEye Eye { get { return eye; } }
        public IAdapter Adapter { get { return adapter; } }
        public DeviceInitializationFlags Flags { get { return flags; } }
        public IPrimarySwapChain PrimarySwapChain { get { return this; } }
        public IDeviceChildCreator Create { get { return creator; } }
        IDeviceContext IDevice.ImmediateContext { get { return immediateContext; } }

        public int Width { get { return primaryWindow.SwapChainWidth; } }
        public int Height { get { return primaryWindow.SwapChainHeight; } }
        public int ColorBufferCount { get { return implicitSwapChainDesc.ColorBufferCount; } }
        public int ColorBufferFormatID { get { return implicitSwapChainDesc.ColorBufferFormatID; } }
        public bool EnableAutoDepthStencil { get { return implicitSwapChainDesc.EnableAutoDepthStencil; } }
        public int DepthStencilFormatID { get { return implicitSwapChainDesc.DepthStencilFormatID; } }
        public Sampling Sampling { get { return implicitSwapChainDesc.Sampling; } }
        public bool VerticalSync { get { return implicitSwapChainDesc.VerticalSync; } }
        public IWindow Window { get { return primaryWindow; } }
        public IDevice Device { get { return this; } }
        public IRenderTargetView GetCurrentColorBuffer() { return implicitBackBuffer.MainView; }
        public IDepthStencilView GetCurrentDepthStencilBuffer()
        {
            if (!implicitSwapChainDesc.EnableAutoDepthStencil) throw new InvalidOperationException("Trying to access an unexisting auto depth-stencil buffer.");
            return implicitDepthStencilBuffer.MainView;
        }
        public void GetDescription(out SwapChainDescription description) { description = implicitSwapChainDesc; }

        public FullscreenState FullscreenState { get { return fullscreenState; } }
        public void GetFullscreenDisplayMode(out BDisplayMode displayMode) { displayMode = fullscreenDisplayMode; }

        public WinformsDevice(WinformsEye eye, Direct3D direct3D, CAdapter adapter, 
            ControlWindow primaryWindow, SwapChainDescription implicitSwapChainDesc, 
            DeviceInitializationFlags flags, IFileSystem fileSystem)
        {
            this.eye = eye;
            this.adapter = adapter;
            this.primaryWindow = primaryWindow;
            this.implicitSwapChainDesc = implicitSwapChainDesc;
            this.flags = flags;

            caps = adapter.Info.GetCaps(DeviceType.Hardware);
            devicePresentParams = new PresentParameters();

            FillDevicePresentParams();
            d3dDevice = new Device(direct3D, adapter.Index, DeviceType.Hardware, primaryWindow.Handle, CreateFlags.FpuPreserve | CreateFlags.HardwareVertexProcessing, devicePresentParams);

            CreateBackBufferAndDepthStencil();

            additionalSwapChains = new List<CAdditionalSwapChain>();
            creator = new CDeviceChildCreator(this);
            immediateContext = new CDeviceContext(this);

            lastSwapChainSize = new IntSize(primaryWindow.SwapChainWidth, primaryWindow.SwapChainHeight);
            fullscreenState = FullscreenState.Windowed;
            fullscreenDisplayMode = adapter.GetSupportedDisplayModes().First();
        }

        void CreateBackBufferAndDepthStencil()
        {
            implicitBackBuffer = new CBackBuffer(this, primaryWindow.SwapChainWidth, primaryWindow.SwapChainHeight, 
                implicitSwapChainDesc.ColorBufferFormatID, implicitSwapChainDesc.Sampling);
            implicitBackBuffer.UpdateSurface(d3dDevice.GetBackBuffer(0, 0));

            implicitDepthStencilBuffer = new CAutoDepthStencil(this, primaryWindow.SwapChainWidth, primaryWindow.SwapChainHeight, 
                implicitSwapChainDesc.DepthStencilFormatID, implicitSwapChainDesc.Sampling);

            if (implicitSwapChainDesc.EnableAutoDepthStencil)
                implicitDepthStencilBuffer.UpdateSurface(d3dDevice.DepthStencilSurface);
        }


        void FillDevicePresentParams()
        {
            devicePresentParams.BackBufferWidth = primaryWindow.SwapChainWidth;
            devicePresentParams.BackBufferHeight = primaryWindow.SwapChainHeight;
            devicePresentParams.BackBufferFormat = (Format)implicitSwapChainDesc.ColorBufferFormatID;
            devicePresentParams.BackBufferCount = implicitSwapChainDesc.ColorBufferCount - 1;
            devicePresentParams.MultiSampleType = CtSharpDX9.MultisampleType(implicitSwapChainDesc.Sampling.Count);
            devicePresentParams.MultiSampleQuality = implicitSwapChainDesc.Sampling.Quality;
            devicePresentParams.SwapEffect = SwapEffect.Discard;
            devicePresentParams.DeviceWindowHandle = primaryWindow.Handle;
            devicePresentParams.Windowed = fullscreenState == FullscreenState.Windowed || fullscreenState == FullscreenState.PseudoFullscreen;
            devicePresentParams.PresentationInterval = implicitSwapChainDesc.VerticalSync ? PresentInterval.One : PresentInterval.Immediate;
            devicePresentParams.FullScreenRefreshRateInHz = fullscreenState == FullscreenState.Fullscreen ? fullscreenDisplayMode.RefreshRate.Round() : 0;

            if (implicitSwapChainDesc.EnableAutoDepthStencil)
            {
                devicePresentParams.EnableAutoDepthStencil = true;
                devicePresentParams.AutoDepthStencilFormat = (Format) implicitSwapChainDesc.DepthStencilFormatID;
                devicePresentParams.PresentFlags = PresentFlags.DiscardDepthStencil;
            }
            else
            {
                devicePresentParams.EnableAutoDepthStencil = false;
                devicePresentParams.AutoDepthStencilFormat = D3DFormat.Unknown;
                devicePresentParams.PresentFlags = PresentFlags.None;
            }
        }

        /// <summary>
        /// Check device cooperative level before calling!
        /// </summary>
        void ResetProcedure()
        {
            creator.OnDeviceLost();
            implicitDepthStencilBuffer.ReleaseIfExists();
            implicitBackBuffer.ReleaseIfExists();
            foreach (CAdditionalSwapChain t in additionalSwapChains)
                t.OnDeviceLost();

            FillDevicePresentParams();
            d3dDevice.Reset(devicePresentParams);
            lastSwapChainSize = new IntSize(primaryWindow.SwapChainWidth, primaryWindow.SwapChainHeight);

            CreateBackBufferAndDepthStencil();
            foreach (CAdditionalSwapChain t in additionalSwapChains)
                t.OnDeviceReset(fullscreenState == FullscreenState.Fullscreen);
            creator.OnDeviceReset();
            immediateContext.OnDeviceReset();
        }

        public void Reset(ref SwapChainDescription description)
        {
            fullscreenState = FullscreenState.Windowed;
            implicitSwapChainDesc = description;

            primaryWindow.OnWindowed();

            var coopLevel = d3dDevice.TestCooperativeLevel();
            if (coopLevel.Success || coopLevel == ResultCode.DeviceNotReset)
                ResetProcedure();
        }

        public void ResetToPseudoFullscreen(ref SwapChainDescription description)
        {
            fullscreenState = FullscreenState.PseudoFullscreen;
            implicitSwapChainDesc = description;

            primaryWindow.OnPseudoFullscreen();

            var coopLevel = d3dDevice.TestCooperativeLevel();
            if (coopLevel.Success || coopLevel == ResultCode.DeviceNotReset)
                ResetProcedure();
        }

        public void ResetToFullscreen(ref BDisplayMode displayMode, ref SwapChainDescription description)
        {
            fullscreenDisplayMode = displayMode;

            fullscreenState = FullscreenState.Fullscreen;
            implicitSwapChainDesc = description;

            primaryWindow.OnFullscreen(displayMode.Width, displayMode.Height, () => { }, () => { }); //todo: remove unneeded args

            var coopLevel = d3dDevice.TestCooperativeLevel();
            if (coopLevel.Success || coopLevel == ResultCode.DeviceNotReset)
                ResetProcedure();
        }

        bool ISwapChain.BeginScene()
        {
            var coopLevel = d3dDevice.TestCooperativeLevel();
            if (coopLevel.Success)
            {
                if (lastSwapChainSize.Width != primaryWindow.SwapChainWidth ||
                    lastSwapChainSize.Height != primaryWindow.SwapChainHeight)
                {
                    ResetProcedure();
                }
                d3dDevice.BeginScene();
                //todo: decide
                //immediateContext.SetRenderTargets(implicitBackBuffer.MainView, implicitDepthStencilBuffer.MainView);
                return true;
            }

            if (coopLevel == ResultCode.DeviceNotReset)
            {
                ResetProcedure();
                d3dDevice.BeginScene();
                //todo: decide
                //immediateContext.SetRenderTargets(implicitBackBuffer.MainView, implicitDepthStencilBuffer.MainView);
                return true;
            }

            Thread.Sleep(50);
            return false;
        }

        void ISwapChain.EndScene() 
        { 
            d3dDevice.EndScene(); 
        }

        void ISwapChain.Present()
        {
            implicitBackBuffer.ReleaseIfExists();

            if (!primaryWindow.IsVisible)
            {
                if (additionalSwapChains.All(aswc => !aswc.Window.IsVisible))
                    Thread.Sleep(50);
            }
            else
            {
                // Exception may be thrown here for uncontrolled reasons like UAC.
                // ReSharper disable EmptyGeneralCatchClause
                try { d3dDevice.Present(); }
                catch { }
                // ReSharper restore EmptyGeneralCatchClause
            }
                
            implicitBackBuffer.UpdateSurface(d3dDevice.GetBackBuffer(0, 0));
        }

        public IAdditionalSwapChain CreateAdditionalSwapChain(IWindowHandle windowHandle, SwapChainDescription swapChainDesc)
        {
            var window = (ControlWindow)windowHandle;
            var aswc = new CAdditionalSwapChain(this, window, swapChainDesc, asc => additionalSwapChains.Remove(asc));
            window.SetSwapChain(aswc);
            additionalSwapChains.Add(aswc);
            return aswc;
        }

        public void Dispose()
        {
            creator.Dispose();
            implicitDepthStencilBuffer.ReleaseIfExists();
            implicitBackBuffer.ReleaseIfExists();
            foreach (var swc in additionalSwapChains)
                swc.DisposeFinal();
            d3dDevice.Dispose();
        }
    }
}
