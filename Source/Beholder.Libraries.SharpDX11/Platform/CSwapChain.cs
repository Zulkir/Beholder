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
using System.Threading;
using Beholder.Libraries.SharpDX11.Resources;
using Beholder.Libraries.Windows;
using Beholder.Math;
using Beholder.Platform;
using Beholder.Resources;
using Beholder.Utility.ForImplementations;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using BindFlags = SharpDX.Direct3D11.BindFlags;
using CpuAccessFlags = SharpDX.Direct3D11.CpuAccessFlags;
using Rational = SharpDX.DXGI.Rational;
using Resource = SharpDX.Direct3D11.Resource;
using SwapChainDescription = Beholder.Platform.SwapChainDescription;
using Texture2DDescription = SharpDX.Direct3D11.Texture2DDescription;
using Usage = SharpDX.DXGI.Usage;

namespace Beholder.Libraries.SharpDX11.Platform
{
    public class CSwapChain : DeviceChildBase<ICDevice>, IPrimarySwapChain, IAdditionalSwapChain
    {
        readonly ICWindow window;
        readonly SwapChain dxgiSwapChain;
        
        readonly Action<CSwapChain> onDisposed;
        readonly Func<bool> sleepInsteadOfPresent;

        CBackBuffer backBuffer;
        CAutoDepthStencil depthStencilBuffer;

        SwapChainDescription desc;
        DisplayMode fullscreenDisplayMode;
        SharpDX.DXGI.SwapChainDescription dxgiDesc;
        FullscreenState fullscreenState;
        IntSize lastSwapChainSize;

        public ICWindow Window { get { return window; } }
        IWindow ISwapChain.Window { get { return window; } }
        public IRenderTargetView GetCurrentColorBuffer() { return backBuffer.MainView; }
        public IDepthStencilView GetCurrentDepthStencilBuffer() { return depthStencilBuffer.MainView; }
        public FullscreenState FullscreenState { get { return fullscreenState; } }

        public int Width { get { return window.SwapChainWidth; } }
        public int Height { get { return window.SwapChainHeight; } }
        public int ColorBufferCount { get { return desc.ColorBufferCount; } }
        public int ColorBufferFormatID { get { return desc.ColorBufferFormatID; } }
        public bool EnableAutoDepthStencil { get { return desc.EnableAutoDepthStencil; } }
        public int DepthStencilFormatID { get { return desc.DepthStencilFormatID; } }
        public Sampling Sampling { get { return desc.Sampling; } }
        public bool VerticalSync { get { return desc.VerticalSync; } }
        public void GetDescription(out SwapChainDescription description) { description = desc; }
        public void GetFullscreenDisplayMode(out DisplayMode displayMode) { displayMode = fullscreenDisplayMode; }

        public CSwapChain(ICDevice device, ICWindow window,
            ref SwapChainDescription bSwapChainDesc,
            Action<CSwapChain> onDisposed, Func<bool> sleepInsteadOfPresent)
            : base(device)
        {
            this.window = window;
            this.onDisposed = onDisposed;
            this.sleepInsteadOfPresent = sleepInsteadOfPresent;

            desc = bSwapChainDesc;

            FillDXGIDesc();
            dxgiSwapChain = new SwapChain(device.DXGIFactory, device.D3DDevice, dxgiDesc);
            device.DXGIFactory.MakeWindowAssociation(window.Handle, WindowAssociationFlags.IgnoreAltEnter);
            FillBuffers();
        }

        void ReleaseBuffers()
        {
            if (depthStencilBuffer != null)
            {
                depthStencilBuffer.Dispose();
                depthStencilBuffer = null;
            }

            backBuffer.Dispose();
        }

        void FillDXGIDesc()
        {
            dxgiDesc = new SharpDX.DXGI.SwapChainDescription
            {
                ModeDescription = new ModeDescription
                {
                    Width = fullscreenState == FullscreenState.Fullscreen ? fullscreenDisplayMode.Width : window.SwapChainWidth,
                    Height = fullscreenState == FullscreenState.Fullscreen ? fullscreenDisplayMode.Height : window.SwapChainHeight,
                    Format = (Format)desc.ColorBufferFormatID,
                    RefreshRate = fullscreenState == FullscreenState.Fullscreen ? CtSharpDX11.Rational(fullscreenDisplayMode.RefreshRate) : new Rational(),
                    ScanlineOrdering = DisplayModeScanlineOrder.Unspecified,
                    Scaling = DisplayModeScaling.Unspecified
                },
                SampleDescription = new SampleDescription
                {
                    Count = desc.Sampling.Count,
                    Quality = desc.Sampling.Quality
                },
                Usage = Usage.RenderTargetOutput,
                BufferCount = desc.ColorBufferCount,
                OutputHandle = window.Handle,
                IsWindowed = true,
                SwapEffect = SwapEffect.Discard,
                Flags = SwapChainFlags.None
            };
        }

        void FillBuffers()
        {
            backBuffer = new CBackBuffer(device, Resource.FromSwapChain<Texture2D>(dxgiSwapChain, 0));

            if (!desc.EnableAutoDepthStencil) return;

            var d3dDepthStencilTexture = new Texture2D(device.D3DDevice, new Texture2DDescription
            {
                Width = dxgiDesc.ModeDescription.Width,
                Height = dxgiDesc.ModeDescription.Height,
                MipLevels = 1,
                ArraySize = 1,
                Format = (Format)desc.DepthStencilFormatID,
                SampleDescription = dxgiDesc.SampleDescription,
                Usage = ResourceUsage.Default,
                BindFlags = BindFlags.DepthStencil,
                CpuAccessFlags = CpuAccessFlags.None,
                OptionFlags = ResourceOptionFlags.None
            });

            depthStencilBuffer = new CAutoDepthStencil(device, d3dDepthStencilTexture);
        }

        void ResetProcedure()
        {
            ReleaseBuffers();
            FillDXGIDesc();
            dxgiSwapChain.ResizeBuffers(dxgiDesc.BufferCount, dxgiDesc.ModeDescription.Width, dxgiDesc.ModeDescription.Height,
                                        dxgiDesc.ModeDescription.Format, dxgiDesc.Flags);
            dxgiSwapChain.SetFullscreenState(fullscreenState == FullscreenState.Fullscreen, null);
            dxgiSwapChain.ResizeBuffers(dxgiDesc.BufferCount, dxgiDesc.ModeDescription.Width, dxgiDesc.ModeDescription.Height,
                                        dxgiDesc.ModeDescription.Format, dxgiDesc.Flags);
            FillBuffers();

            lastSwapChainSize = new IntSize(window.SwapChainWidth, window.SwapChainHeight);
        }

        public void Reset(ref SwapChainDescription description)
        {
            bool needsFixing = fullscreenState == FullscreenState.Fullscreen;

            desc = description;
            fullscreenState = FullscreenState.Windowed;

            window.OnWindowed();

            ResetProcedure();

            if (needsFixing) window.FixOutOfFullscreenBug11();
        }

        public void ResetToPseudoFullscreen(ref SwapChainDescription description)
        {
            Reset(ref description);

            fullscreenState = FullscreenState.PseudoFullscreen;

            window.OnPseudoFullscreen();

            ResetProcedure();
        }

        public void ResetToFullscreen(ref DisplayMode displayMode, ref SwapChainDescription description)
        {
            fullscreenDisplayMode = displayMode;
            desc = description;
            fullscreenState = FullscreenState.Fullscreen;

            window.OnFullscreen(displayMode.Width, displayMode.Height,
                () => { dxgiSwapChain.SetFullscreenState(false, null); window.OnFullscreenBroken11(); },
                () => { Reset(ref desc); ResetToFullscreen(ref fullscreenDisplayMode, ref desc); });

            ResetProcedure();
        }

        public bool BeginScene()
        {
            if (fullscreenState == FullscreenState.Fullscreen)
                if (!dxgiSwapChain.IsFullScreen)
                    window.OnFullscreenBroken11();

            if (lastSwapChainSize.Width != window.SwapChainWidth ||
                lastSwapChainSize.Height != window.SwapChainHeight)
            {
                ResetProcedure();
            }

            //todo decide
            //device.ImmediateContext.SetRenderTargets(backBuffer.MainView, depthStencilBuffer.MainView);
            return true;
        }

        public void EndScene()
        {
            
        }

        public void Present()
        {
            if (sleepInsteadOfPresent())
                Thread.Sleep(50);
            else
                dxgiSwapChain.Present(desc.VerticalSync ? 1 : 0, PresentFlags.None);
        }

        public void Dispose()
        {
            DisposeFinal();
            onDisposed(this);
        }

        public void DisposeFinal()
        {
            if (fullscreenState == FullscreenState.Fullscreen)
                Reset(ref desc);

            ReleaseBuffers();
            dxgiSwapChain.Dispose();
        }
    }
}
