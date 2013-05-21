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
using Beholder;
using Beholder.Core;
using Beholder.Input;
using Beholder.Platform;

namespace Launcher
{
    class MetaScene : IDisposable
    {
        readonly IEye eye;
        readonly IWindow window;
        readonly DisplayMode desctopDisplayMode;
        Scene currentScene;

        public MetaScene(IEye eye, DisplayMode desctopDisplayMode)
        {
            this.eye = eye;
            this.desctopDisplayMode = desctopDisplayMode;
            var swapChain = eye.Device.PrimarySwapChain;
            window = swapChain.Window;
            eye.NewFrame += NewFrame;
        }

        public void Run()
        {
            currentScene = CreateAndInitializeScene(0);
            eye.RunLoop(window);
        }

        public void Dispose()
        {
            eye.TerminateLoop();

        }

        protected virtual void NewFrame(IRealTime realTime)
        {
            if (window.Input.Keyboard.KeyReleased(Keys.F))
            {
                SwapChainDescription sd;
                eye.Device.PrimarySwapChain.GetDescription(out sd);
                var dm = desctopDisplayMode;
                sd.ColorBufferFormatID = dm.FormatID;
                eye.Device.PrimarySwapChain.ResetToFullscreen(ref dm, ref sd);
            }
            else if (window.Input.Keyboard.KeyReleased(Keys.W))
            {
                SwapChainDescription sd;
                eye.Device.PrimarySwapChain.GetDescription(out sd);
                eye.Device.PrimarySwapChain.Reset(ref sd);
            }
            else if (window.Input.Keyboard.KeyReleased(Keys.P))
            {
                SwapChainDescription sd;
                eye.Device.PrimarySwapChain.GetDescription(out sd);
                eye.Device.PrimarySwapChain.ResetToPseudoFullscreen(ref sd);
            }

            for (int i = 0; i <= 6; i++)
                if (window.Input.Keyboard.KeyPressed(Keys.D0 + i))
                    currentScene = CreateAndInitializeScene(i);

            currentScene.NewFrame(realTime);
        }

        Scene CreateAndInitializeScene(int index)
        {
            var device = eye.Device;
            var deviceContext = eye.Device.ImmediateContext;
            var swapChain = eye.Device.PrimarySwapChain;

            deviceContext.Rasterizer.State = device.Create.RasterizerState(RasterizerDescription.Default);
            deviceContext.Rasterizer.Viewports.Set(new Viewport(swapChain.Width, swapChain.Height));
            deviceContext.OutputMerger.RenderTargets.SetEmpty();
            deviceContext.OutputMerger.DepthStencil = null;
            deviceContext.OutputMerger.BlendState = device.Create.BlendState(BlendDescription.Default);
            deviceContext.OutputMerger.DepthStencilState = device.Create.DepthStencilState(DepthStencilDescription.Default);

            switch (index)
            {
                case 0: return new ClearScreenScene(eye, desctopDisplayMode);
                case 1: return new TriangleScene(eye, desctopDisplayMode);
                case 2: return new FullscreenQuadScene(eye, desctopDisplayMode);
                case 3: return new CubeScene(eye, desctopDisplayMode);
                case 4: return new RenderToTextureScene(eye, desctopDisplayMode);
                case 5: return new ColorfulSpaceScene(eye, desctopDisplayMode);
                case 6: return new CurveTesselationScene(eye, desctopDisplayMode);
                default: throw new ArgumentOutOfRangeException();
            }
        }
    }
}
