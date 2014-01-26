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
    class FullscreenQuadScene : Scene
    {
        struct Vertex
        {
            public Vector2 Position;
            public Vector4 Data;

            public Vertex(float x, float y, float r, float g, float b, float a)
            {
                Position.X = x;
                Position.Y = y;
                Data.X = r;
                Data.Y = g;
                Data.Z = b;
                Data.W = a;
            }

            public const int SizeInBytes = 6 * sizeof(float);
        }

        const string VertexShaderText = @"
%meta
Name = QuadSceneVS
Stage = Vertex
ProfileDX9 = vs_2_0
ProfileDX10 = vs_4_0
ProfileGL3 = 150

%input
float2 Position : SDX9 = POSITION, SDX10 = %name, SGL3 = %name
float4 Data     : SDX9 = TEXCOORD, SDX10 = %name, SGL3 = %name

%output
float4 Position : SDX9 = POSITION, SDX10 = SV_Position, SGL3 = gl_Position
float4 Data     : SDX9 = TEXCOORD, SDX10 = %name,       SGL3 = %name

%code_main
    OUTPUT(Position) = float4(INPUT(Position), 0.0, 1.0);
    OUTPUT(Data) = INPUT(Data);
";

        const string PixelShaderText = @"
%meta
Name = QuadScenePS
Stage = Pixel
ProfileDX9 = ps_2_0
ProfileDX10 = ps_4_0
ProfileGL3 = 150

%input
float4 Position : SDX9 = %unused,  SDX10 = SV_Position, SGL3 = %unused
float4 Data     : SDX9 = TEXCOORD, SDX10 = %name,       SGL3 = %name

%output
float4 Color : SDX9 = COLOR, SDX10 = SV_Target, SGL3 = %name

%code_main
    float4x4 mat = float4x4(
        float4(-1.0, 0.0, 0.0, 0.0), 
        float4(0.0, -1.0, 0.0, 0.0), 
        float4(0.0, 0.0, -1.0, 0.0), 
        float4(0.0, 0.0, 0.0, -1.0)
    );
    
    float4 red = float4(1.0, 0.0, 0.0, 1.0);
    float4 green = float4(0.0, 1.0, 0.0, 1.0);
    
    float tenX = INPUT(Data).x * 10.0;
    float tenY = INPUT(Data).y * 10.0;
    
    OUTPUT(Color) = float4(trunc(3 * INPUT(Data).x), trunc(3 * INPUT(Data).y), 0.0, 1.0);
    
    //float s;
    //float c;
    //sincos(length(INPUT(Data).xy), s, c);
    //OUTPUT(Color) = float4(s, c, 0.0, 1.0);
    
    //OUTPUT(Color) = float4(sign(INPUT(Data).xy), 0.0, 1.0);
    
    //float p = noise(float2(tenX, tenY));
    //OUTPUT(Color) = float4(3.0 * noise(100.0*tenX), 3.0 * noise(100.0*tenY), 0.0, 1.0);
    
    //int wholeX;
    //int wholeY;
    //float resX = modf(tenX, wholeX);
    //float resY = modf(tenY, wholeY);
    //OUTPUT(Color) = float4(resX, resY, wholeX / 10.0, 1.0);
    
    //float2 v = float2(1.0 / ldexp(tenX, 1.2), 1.0 / ldexp(tenY, 2));
    //OUTPUT(Color) = float4(v, 0.0, 1.0);
    
    //float3 v1 = float3(0.0, 0.0, 0.5);
    //float3 v2 = float3(0.5, 0.5, 0.0);
    //OUTPUT(Color) = float4(cross(v1, v2), 1.0);
    
    //float expX;
    //float expY;
    //float mantX = frexp(tenX, expX);
    //float mantY = frexp(tenY, expY);
    //OUTPUT(Color) = float4(expX, expY, float(gl_NumSamples)/*GetRenderTargetSampleCount()*/, 1.0);
    
    //OUTPUT(Color) = float4(frac(tenX), frac(tenY), 0.0, 1.0); 
    
    //OUTPUT(Color) = float4(fmod(tenX, tenY), fmod(tenY, tenX), 0.0, 1.0);
    
    //clip(INPUT(Data).xy);
    //OUTPUT(Color) = green; 
    
    //if (all(clamp(INPUT(Data), 0.0, 1.0).xy)) OUTPUT(Color) = red; else OUTPUT(Color) = green;
    
    //OUTPUT(Color) = abs(INPUT(Data));
    //float4x4 absmat = abs(mat);
    //OUTPUT(Color) = float4(absmat[0][0], 0.0, absmat[2][2], absmat[3][3]);
    //OUTPUT(Color) = float4(1.0, 0.0, 0.0, 1.0);
";

        readonly IShaderCombination shaders;
        readonly IVertexLayout vertexLayout;
        readonly IBuffer vertexBuffer;
        readonly IBuffer indexBuffer;

        public FullscreenQuadScene(IEye eye, DisplayMode desctopDisplayMode)
            : base(eye, desctopDisplayMode)
        {
            var vs = Device.Create.VertexShader(ShaderParser.Parse(VertexShaderText));
            var ps = Device.Create.PixelShader(ShaderParser.Parse(PixelShaderText));
            shaders = Device.Create.ShaderCombination(vs, null, null, null, ps);

            vertexLayout = Device.Create.VertexLayout(vs, new[]
            {
                new VertexLayoutElement(ExplicitFormat.R32G32_FLOAT, 0, 0),
                new VertexLayoutElement(ExplicitFormat.R32G32B32A32_FLOAT, 0, 8)
            });

            vertexBuffer = Device.Create.Buffer(new BufferDescription
            {
                BindFlags = BindFlags.VertexBuffer,
                Usage = Usage.Immutable,
                SizeInBytes = 4 * Vertex.SizeInBytes
            }, new SubresourceData(new[]
            {
                new Vertex(-1, -1, -1, -1,  1,  1),
                new Vertex( 1, -1,  1, -1,  1,  1),
                new Vertex( 1,  1,  1,  1,  0,  0),
                new Vertex(-1,  1, -1,  1,  0,  0)
            }));

            indexBuffer = Device.Create.Buffer(new BufferDescription
            {
                BindFlags = BindFlags.IndexBuffer,
                Usage = Usage.Immutable,
                ExtraFlags = ExtraFlags.SixteenBitIndices,
                SizeInBytes = 6 * sizeof(ushort)
            }, new SubresourceData(new ushort[]
            {
                0, 1, 2,
                0, 2, 3
            }));
        }

        public override void NewFrame(IRealTime realTime)
        {
            if (SwapChain.BeginScene())
            {
                ImmediateContext.OutputMerger.RenderTargets.Set(SwapChain.GetCurrentColorBuffer());
                ImmediateContext.Rasterizer.Viewports.Set(new Viewport(SwapChain.Width, SwapChain.Height));
                ImmediateContext.ClearRenderTargetView(SwapChain.GetCurrentColorBuffer(), Color4.CornflowerBlue);

                ImmediateContext.ShadersForDrawing = shaders;
                ImmediateContext.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;
                ImmediateContext.InputAssembler.VertexLayout = vertexLayout;
                ImmediateContext.InputAssembler.VertexSources[0] = new VertexSource(vertexBuffer, 0, Vertex.SizeInBytes);
                ImmediateContext.InputAssembler.IndexSource = new IndexSource(indexBuffer, 0, IndexFormat.SixteenBit);

                ImmediateContext.DrawIndexed(6, 0, 0);

                SwapChain.EndScene();
                SwapChain.Present();
            }
        }
    }
}
