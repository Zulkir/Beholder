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

using Beholder.Libraries.ObjectGL4.Resources;
using Beholder.Platform;
using Beholder.Resources;
using ObjectGL.GL42;

namespace Beholder.Libraries.ObjectGL4.Platform
{
    public class SwapChainSurfaces
    {
        readonly ICDevice device;
        CBackBuffer backBuffer;
        CAutoDepthStencil autoDepthStencil;

        Texture2DDescription texture2DDescription;

        internal CBackBuffer BackBuffer { get { return backBuffer; } }

        public SwapChainSurfaces(ICDevice device, int width, int height, ref SwapChainDescription implicitSwapChainDescription)
        {
            this.device = device;

            var glContext = device.GetCurrentContext();

            texture2DDescription = new Texture2DDescription
            {
                ArraySize = 1,
                ExtraFlags = ExtraFlags.None,
                Usage = Usage.Default,
                MipLevels = 1,
                MiscFlags = MiscFlags.None
            };

            OnReset(glContext, width, height, ref implicitSwapChainDescription);
        }

        public void Dispose()
        {
            if (backBuffer != null) { backBuffer.Dispose(); backBuffer = null; }
            if (autoDepthStencil != null) { autoDepthStencil.Dispose(); autoDepthStencil = null; }
        }

        public IRenderTargetView GetCurrentColorBuffer()
        {
            return backBuffer.MainView;
        }

        public IDepthStencilView GetCurrentDepthStencilBuffer()
        {
            return autoDepthStencil.MainView;
        }

        public void OnReset(Context glContext, int width, int height, ref SwapChainDescription implicitSwapChainDescription)
        {
            if (backBuffer != null) { backBuffer.Dispose(); backBuffer = null; }
            if (autoDepthStencil != null) { autoDepthStencil.Dispose(); autoDepthStencil = null; }

            texture2DDescription.Width = width;
            texture2DDescription.Height = height;
            texture2DDescription.Sampling = implicitSwapChainDescription.Sampling;

            texture2DDescription.FormatID = implicitSwapChainDescription.ColorBufferFormatID;
            texture2DDescription.BindFlags = BindFlags.RenderTarget;

            backBuffer = new CBackBuffer(device, ref texture2DDescription);

            if (implicitSwapChainDescription.EnableAutoDepthStencil)
            {
                texture2DDescription.FormatID = implicitSwapChainDescription.DepthStencilFormatID;
                texture2DDescription.BindFlags = BindFlags.DepthStencil;
                autoDepthStencil = new CAutoDepthStencil(device, ref texture2DDescription);
            }
        }
    }
}
