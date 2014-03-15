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

using System.Linq;
using Beholder;
using Beholder.Basic;
using Beholder.Core;
using Beholder.Math;
using Beholder.Platform;
using Beholder.Resources;
using Beholder.Utility.Helpers;

namespace DemoRunner.Scenes.EarthlitNight
{
    public class EarthlitNightScene : IScene
    {
        private readonly IEye eye;
        private readonly IDevice device;
        private readonly IPrimarySwapChain swapChain;

        private const int StarCount = 2000;
        private const int SkyboxSize = 1024;

        private Stars stars;

        private IBuffer starsVertexBuffer;
        private IBuffer earthVertexBuffer;
        private IBuffer earthIndexBuffer;

        private ITexture2D starsProxyTexture;
        private ITexture2D skyboxTexture;
        private ITexture2D earthTexture;

        public EarthlitNightScene(IEye eye)
        {
            this.eye = eye;
            device = eye.Device;
            swapChain = device.PrimarySwapChain;

            stars = new Stars(device, StarCount);

            var meshFactory = new MeshFactory(device, Handedness.Right, Winding.Clockwise);
            var earthMesh = meshFactory.CreateSphere(false, 1.0f, 36);
            earthVertexBuffer = earthMesh.Vertices.Buffer;
            earthIndexBuffer = earthMesh.Indices.Buffer;

            var formatInfo = eye.Adapters[0].GetSupportedFormats(FormatSupport.RenderTarget | FormatSupport.Texture2D | FormatSupport.TextureCube)
                .First(x => x.ColorBits == 24 && x.AlphaBits == 8 && x.ColorFormatType == FormatElementType.UNORM);
            starsProxyTexture = device.Create.Texture2D(new Texture2DDescription
            {
                BindFlags = BindFlags.RenderTarget | BindFlags.ShaderResource,
                Usage = Usage.Default,
                Width = SkyboxSize,
                Height = SkyboxSize,
                ArraySize = 1,
                MipLevels = 1,
                FormatID = formatInfo.ID
            });

            skyboxTexture = device.Create.Texture2D(new Texture2DDescription
            {
                BindFlags = BindFlags.RenderTarget | BindFlags.ShaderResource,
                Usage = Usage.Default,
                Width = SkyboxSize,
                Height = SkyboxSize,
                ArraySize = 6,
                MipLevels = TextureHelper.MipLevels(SkyboxSize, SkyboxSize, 1),
                FormatID = formatInfo.ID,
                MiscFlags = MiscFlags.TextureCube | MiscFlags.GenerateMips
            });

            earthTexture = device.Load.Texture2D("../Textures/BasicTest.png");


        }

        public void OnNewFrame(IRealTime realTime)
        {
            var swapChain = eye.Device.PrimarySwapChain;
            if (swapChain.BeginScene())
            {
                var context = eye.Device.ImmediateContext;
                context.ClearRenderTargetView(swapChain.GetCurrentColorBuffer(), Color4.Black);

                context.Rasterizer.Viewports.Set(new Viewport(swapChain.Width, swapChain.Height));
                context.OutputMerger.RenderTargets.Set(swapChain.GetCurrentColorBuffer());

                stars.Draw(context, realTime);

                swapChain.EndScene();
                swapChain.Present();
            }
        }
    }
}