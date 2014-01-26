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
using Beholder.Libraries.SharpDX9.Resources;
using Beholder.Libraries.Windows;
using Beholder.Math;
using Beholder.Platform;
using Beholder.Resources;
using Beholder.Utility.ForImplementations;
using SharpDX.Direct3D9;

namespace Beholder.Libraries.SharpDX9.Platform
{
    public class CAdditionalSwapChain : DeviceChildBase<ICDevice>, IAdditionalSwapChain
    {
        readonly ICWindow window;
        readonly Action<CAdditionalSwapChain> onDispose;
        CBackBuffer backBuffer;
        CAutoDepthStencil depthStencilBuffer;

        SwapChain d3dSwapChain;
        SwapChainDescription desc;
        IntSize lastSwapChainSize;
        bool playDead;

        public int Width { get { return window.SwapChainWidth; } }
        public int Height { get { return window.SwapChainHeight; } }
        public int ColorBufferCount { get { return desc.ColorBufferCount; } }
        public int ColorBufferFormatID { get { return desc.ColorBufferFormatID; } }
        public bool EnableAutoDepthStencil { get { return desc.EnableAutoDepthStencil; } }
        public int DepthStencilFormatID { get { return desc.DepthStencilFormatID; } }
        public Sampling Sampling { get { return desc.Sampling; } }
        public bool VerticalSync { get { return desc.VerticalSync; } }

        public void GetDescription(out SwapChainDescription description) { description = desc; }
        public ICWindow Window { get { return window; } }
        IWindow ISwapChain.Window { get { return window; } }
        public IRenderTargetView GetCurrentColorBuffer() { return backBuffer.MainView; }
        public IDepthStencilView GetCurrentDepthStencilBuffer()
        {
            if (!desc.EnableAutoDepthStencil) 
                throw new InvalidOperationException("Trying to access an unexisting auto depth-stencil buffer.");
            return depthStencilBuffer.MainView;
        }

        public CAdditionalSwapChain(ICDevice device, ICWindow window, 
            SwapChainDescription swapChainDesc, Action<CAdditionalSwapChain> onDispose)
            : base(device)
        {
            this.window = window;
            this.onDispose = onDispose;

            desc = swapChainDesc;

            CreateAll();

            lastSwapChainSize = new IntSize(window.SwapChainWidth, window.SwapChainHeight);
        }

        void CreateAll()
        {
            var presentParams = new PresentParameters
            {
                BackBufferWidth = window.SwapChainWidth,
                BackBufferHeight = window.SwapChainHeight,
                BackBufferFormat = (Format)desc.ColorBufferFormatID,
                FullScreenRefreshRateInHz = 0,
                BackBufferCount = desc.ColorBufferCount - 1,
                MultiSampleType = CtSharpDX9.MultisampleType(desc.Sampling.Count),
                MultiSampleQuality = desc.Sampling.Quality,
                SwapEffect = SwapEffect.Discard,
                DeviceWindowHandle = window.Handle,
                Windowed = true,
                PresentationInterval = desc.VerticalSync ? PresentInterval.Default : PresentInterval.Immediate
            };

            d3dSwapChain = new SwapChain(device.D3DDevice, presentParams);
            lastSwapChainSize = new IntSize(window.SwapChainWidth, window.SwapChainHeight);

            backBuffer = new CBackBuffer(device, window.SwapChainWidth, window.SwapChainHeight, desc.ColorBufferFormatID, desc.Sampling);
            backBuffer.UpdateSurface(d3dSwapChain.GetBackBuffer(0));

            depthStencilBuffer = new CAutoDepthStencil(device, window.SwapChainWidth, window.SwapChainHeight, desc.DepthStencilFormatID, desc.Sampling);

            if (!desc.EnableAutoDepthStencil) return;

            var d3dDepthStencilSurface = Surface.CreateDepthStencil(
                device.D3DDevice, 
                presentParams.BackBufferWidth, presentParams.BackBufferHeight,
                (Format)desc.DepthStencilFormatID,
                CtSharpDX9.MultisampleType(desc.Sampling.Count),
                desc.Sampling.Quality,
                true);

            depthStencilBuffer.UpdateSurface(d3dDepthStencilSurface);
        }

        public void OnDeviceLost()
        {
            if (playDead) return;

            depthStencilBuffer.ReleaseIfExists();
            backBuffer.ReleaseIfExists();

            d3dSwapChain.Dispose();
        }

        public void OnDeviceReset(bool primaryFullScreen)
        {
            playDead = primaryFullScreen;

            if (playDead) return;

            CreateAll();
        }

        void ResetProcedure()
        {
            OnDeviceLost();
            OnDeviceReset(playDead);
        }

        public void Reset(ref SwapChainDescription description)
        {
            this.desc = description;

            ResetProcedure();
        }

        public bool BeginScene()
        {
            if (playDead) return false;

            var coopLevel = device.D3DDevice.TestCooperativeLevel();

            if (coopLevel.Success)
            {
                if (lastSwapChainSize.Width != window.SwapChainWidth ||
                    lastSwapChainSize.Height != window.SwapChainHeight)
                {
                    ResetProcedure();
                }
                device.D3DDevice.BeginScene();
                //todo decide
                //device.ImmediateContext.SetRenderTargets(backBuffer.MainView, depthStencilBuffer.MainView);
                return true;
            }

            return false;
        }

        public void EndScene()
        {
            device.D3DDevice.EndScene();
        }

        public void Present() 
        {
            backBuffer.ReleaseIfExists();

            // Exception may be thrown here for uncontrolled reasons like UAC.
            // ReSharper disable EmptyGeneralCatchClause
            try { d3dSwapChain.Present(SharpDX.Direct3D9.Present.None); } catch {}
            // ReSharper restore EmptyGeneralCatchClause

            var d3dBackBufferSurface = d3dSwapChain.GetBackBuffer(0);
            backBuffer.UpdateSurface(d3dBackBufferSurface);
        }

        public void Dispose()
        {
            onDispose(this);
            DisposeFinal();
        }

        public void DisposeFinal()
        {
            depthStencilBuffer.ReleaseIfExists();
            backBuffer.ReleaseIfExists();

            d3dSwapChain.Dispose();
        }
    }
}
