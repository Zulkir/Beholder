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
using Beholder.Input;
using Beholder.Libraries.ObjectGL4.Platform;
using Beholder.Platform;
using Beholder.Resources;
using ObjectGL.GL42;
using OpenTK;
using OpenTK.Graphics;

namespace Beholder.Eyes.ObjectGL4.Default
{
    public class DefaultWindow : GameWindow, IPrimarySwapChain, IWindow, IInputHandler
    {
        readonly Context glContext;

        public Context GLContext { get { return glContext; } }

        #region IInputHandler Members
        readonly Keyboard keyboard;
        readonly Mouse mouse;

        IKeyboardHandler IInputHandler.Keyboard { get { return keyboard; } }
        IMouseHandler IInputHandler.Mouse { get { return mouse; } }
        #endregion

        #region IWindow Members
        bool closed;

        ISwapChain IWindow.SwapChain { get { return this; } }
        IInputHandler IWindow.Input { get { return this; } }
        bool IWindow.Closed { get { return closed; } }
        bool IWindow.ContainsFocus { get { return Focused; } }
        #endregion

        #region IDeviceChild Members
        IDevice IDeviceChild.Device { get { return device; } }
        #endregion

        #region ISwapChain Members
        readonly DefaultDevice device;
        readonly SwapChainSurfaces swapChainSurfaces;
        SwapChainDescription implicitSwapChainDesc;

        IWindow ISwapChain.Window { get { return this; } }
        int ISwapChain.ColorBufferCount { get { return implicitSwapChainDesc.ColorBufferCount; } }
        int ISwapChain.ColorBufferFormatID { get { return implicitSwapChainDesc.ColorBufferFormatID; } }
        bool ISwapChain.EnableAutoDepthStencil { get { return implicitSwapChainDesc.EnableAutoDepthStencil; } }
        int ISwapChain.DepthStencilFormatID { get { return implicitSwapChainDesc.DepthStencilFormatID; } }
        Sampling ISwapChain.Sampling { get { return implicitSwapChainDesc.Sampling; } }
        bool ISwapChain.VerticalSync { get { return implicitSwapChainDesc.VerticalSync; } }
        void ISwapChain.GetDescription(out SwapChainDescription description) { description = implicitSwapChainDesc; }

        IRenderTargetView ISwapChain.GetCurrentColorBuffer()
        {
            return swapChainSurfaces.GetCurrentColorBuffer();
        }

        IDepthStencilView ISwapChain.GetCurrentDepthStencilBuffer()
        {
            if (!implicitSwapChainDesc.EnableAutoDepthStencil)
                throw new InvalidOperationException("Trying to get depth-stencil buffer of a swap chain that has AutoDepthStencil disabled");

            return swapChainSurfaces.GetCurrentDepthStencilBuffer();
        }

        bool ISwapChain.BeginScene()
        {
            // todo: bind glContext to this window
            //((IDeviceContext)device.ImmediateContext).OutputMerger.RenderTargets.Set(swapChainSurfaces.GetCurrentColorBuffer());
            //((IDeviceContext)device.ImmediateContext).OutputMerger.DepthStencil = swapChainSurfaces.GetCurrentDepthStencilBuffer();
            return WindowState != WindowState.Minimized;
        }

        void ISwapChain.EndScene()
        {
            device.ImmediateContext.OnEndScene(swapChainSurfaces);
        }

        void ISwapChain.Present()
        {
            SwapBuffers();
        }

        void ISwapChain.Reset(ref SwapChainDescription description)
        {
            implicitSwapChainDesc = description;

            if (fullscreenState == FullscreenState.Fullscreen)
                DisplayDevice.Default.RestoreResolution();

            fullscreenState = FullscreenState.Windowed;
            WindowState = WindowState.Normal;
            ResetProcedure();
        }

        #endregion

        #region IPrimarySwapChain Members
        FullscreenState fullscreenState;
        DisplayMode fullscreenDisplayMode;

        FullscreenState IPrimarySwapChain.FullscreenState
        {
            get { return fullscreenState; }
        }

        void IPrimarySwapChain.GetFullscreenDisplayMode(out DisplayMode displayMode)
        {
            displayMode = fullscreenDisplayMode;
        }

        void IPrimarySwapChain.ResetToPseudoFullscreen(ref SwapChainDescription description)
        {
            implicitSwapChainDesc = description;

            var defaultDisplay = DisplayDevice.Default;

            if (fullscreenState == FullscreenState.Fullscreen)
                defaultDisplay.RestoreResolution();

            fullscreenState = FullscreenState.PseudoFullscreen;
            WindowState = WindowState.Fullscreen;
            ResetProcedure();
        }

        void IPrimarySwapChain.ResetToFullscreen(ref DisplayMode displayMode, ref SwapChainDescription description)
        {
            fullscreenDisplayMode = displayMode;
            implicitSwapChainDesc = description;

            fullscreenState = FullscreenState.Fullscreen;
            DisplayDevice.Default.ChangeResolution(displayMode.Width, displayMode.Height, device.Adapter.GetFormatInfo(displayMode.FormatID).TotalBits, displayMode.RefreshRate.ToSingle());
            WindowState = WindowState.Fullscreen;
            ResetProcedure();
        }
        #endregion

        void ResetProcedure()
        {
            swapChainSurfaces.OnReset(glContext, ClientSize.Width, ClientSize.Height, ref implicitSwapChainDesc);
        }

        public DefaultWindow(DefaultDevice device, int width, int height, ref SwapChainDescription implicitSwapChainDescription, 
            string title, GameWindowFlags gameWindowFlags, DisplayDevice displayDevice, ref Context glContext)
            : base(width, height, GraphicsMode.Default, title, gameWindowFlags, displayDevice)
        {
            glContext = glContext ?? new Context(Context);
            this.glContext = glContext;

            this.device = device;
            implicitSwapChainDesc = implicitSwapChainDescription;
            swapChainSurfaces = new SwapChainSurfaces(device, width, height, ref implicitSwapChainDescription);

            keyboard = new Keyboard(this);
            mouse = new Mouse(this);
        }

        protected override void OnResize(EventArgs e)
        {
            swapChainSurfaces.OnReset(glContext, ClientSize.Width, ClientSize.Height, ref implicitSwapChainDesc);

            base.OnResize(e);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            keyboard.OnNewFrame();
            mouse.OnNewFrame((float)e.Time);

            base.OnRenderFrame(e);
        }

        protected override void OnClosed(EventArgs e)
        {
            closed = true;
            
            base.OnClosed(e);
        }

        public override void Dispose()
        {
            swapChainSurfaces.Dispose();
            base.Dispose();
        }
    }
}