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

using Beholder;
using Beholder.Core;
using Beholder.Math;
using Beholder.Platform;
using Beholder.Resources;
using Beholder.Shaders;

namespace Launcher
{
    class TriangleScene : Scene
    {
        const string VertexShaderText = @"
%meta
Name = PrimitiveVS
ProfileDX9 = vs_2_0
ProfileDX10 = vs_4_0
ProfileGL3 = 150

%input
float4 Position : SDX9 = POSITION, SDX10 = %name, SGL3 = %name
float4 Color    : SDX9 = COLOR,    SDX10 = %name, SGL3 = %name

%output
float4 Position : SDX9 = POSITION0, SDX10 = SV_Position, SGL3 = gl_Position
float4 Color    : SDX9 = COLOR,     SDX10 = %name,       SGL3 = %name

%code_main
    OUTPUT(Position) = INPUT(Position);
    OUTPUT(Color) = INPUT(Color);
";

        const string PixelShaderText = @"
%meta
Name = PrimitivePS
ProfileDX9 = ps_2_0
ProfileDX10 = ps_4_0
ProfileGL3 = 150

%input
float4 Position : SDX9 = %unused, SDX10 = SV_Position, SGL3 = %unused
float4 Color    : SDX9 = COLOR,   SDX10 = %name,       SGL3 = %name

%output
float4 Color : SDX10 = SV_Target, SGL3 = %name, SDX9 = COLOR

%code_main
    float inverseGamma = 1.0 / 2.2;
    OUTPUT(Color) = pow(INPUT(Color), float4(inverseGamma, inverseGamma, inverseGamma, 1.0f));
";

        struct Vertex
        {
            public Vector4 Position;
            public Color4 Color;

            public Vertex(float x, float y, Color4 color)
            {
                Position = new Vector4(x, y, 0f, 1f);
                Color = color;
            }

            public const int SizeInBytes = 8 * sizeof(float);
        }

        readonly IShaderCombination shaderCombination;
        readonly IVertexLayout vertexLayout;
        readonly IBuffer vertexBuffer;

        public TriangleScene(IEye eye, DisplayMode desctopDisplayMode)
            : base(eye, desctopDisplayMode)
        {
            var vertexShader = Device.Create.VertexShader(ShaderParser.Parse(VertexShaderText));
            var pixelShader = Device.Create.PixelShader(ShaderParser.Parse(PixelShaderText));
            shaderCombination = Device.Create.ShaderCombination(vertexShader, null, null, null, pixelShader);

            var vertexData = new[]
                {
                    new Vertex(-0.7f, -0.7f, Color4.Red), 
                    new Vertex(0.0f, 0.7f, Color4.Yellow),
                    new Vertex(0.7f, -0.7f, Color4.Green)
                };

            vertexBuffer = Device.Create.Buffer(new BufferDescription
            {
                SizeInBytes = vertexData.Length * Vertex.SizeInBytes,
                Usage = Usage.Immutable,
                BindFlags = BindFlags.VertexBuffer
            }, new SubresourceData(vertexData));

            vertexLayout = Device.Create.VertexLayout(vertexShader, new[]
                {
                    new VertexLayoutElement(ExplicitFormat.R32G32B32A32_FLOAT, 0, 0),
                    new VertexLayoutElement(ExplicitFormat.R32G32B32A32_FLOAT, 0, 16)
                });
        }

        public override void NewFrame(IRealTime realTime)
        {
            if (SwapChain.BeginScene())
            {
                ImmediateContext.ClearRenderTargetView(SwapChain.GetCurrentColorBuffer(), Color4.CornflowerBlue);
                ImmediateContext.OutputMerger.RenderTargets.Set(SwapChain.GetCurrentColorBuffer());
                ImmediateContext.Rasterizer.Viewports.Set(new Viewport(SwapChain.Width, SwapChain.Height));

                ImmediateContext.ShadersForDrawing = shaderCombination;

                ImmediateContext.InputAssembler.VertexLayout = vertexLayout;
                ImmediateContext.InputAssembler.VertexSources[0] = new VertexSource(vertexBuffer, 0, Vertex.SizeInBytes);
                ImmediateContext.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;

                ImmediateContext.Draw(3, 0);

                SwapChain.EndScene();
                SwapChain.Present();
            }
        }
    }
}
