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
using Beholder.Math;
using Beholder.Platform;
using Beholder.Resources;
using Beholder.Shaders;

namespace Launcher
{
    class CurveTesselationScene : Scene
    {
        const string VertexShaderText = @"
%meta
Name = PassthroughVS
ProfileDX9 = vs_2_0
ProfileDX10 = vs_4_0
ProfileGL3 = 150

%input
float4 Position : SDX9 = POSITION, SDX10 = %name, SGL3 = %name

%output
float4 Position : SDX9 = POSITION0, SDX10 = SV_Position, SGL3 = gl_Position

%code_main
	OUTPUT(Position) = INPUT(Position);
";

        const string HullShaderText = @"
%meta
Name = IsolineHS
ProfileDX10 = hs_5_0
ProfileGL3 = 400
Domain = Isoline
Partitioning = Integer
OutputTopology = Line
InputControlPoints = 4
OutputControlPoints = 4
MaxTessFactor = 64.0

%ubuffers
ubuffer TessFactor : slot = 0, slotGL3 = 0
	int NumSegments

%input
float4 Position : SDX10 = SV_Position, SGL3 = gl_Position

%input_extra
int ControlPointID : SDX10 = SV_OutputControlPointID, SGL3 = gl_InvocationID
	
%output
float4 Position : SDX10 = %name, SGL3 = %name

%output_patch
float LineTess : SDX10 = SV_TessFactor, SGL3 = gl_TessLevelOuter

%code_patch
	OUTPUT_PATCH(LineTess)[1] = float(NumSegments);
	OUTPUT_PATCH(LineTess)[0] = 1.0;
	
%code_main
	OUTPUT(Position) = INPUT(INPUT_EXTRA(ControlPointID), Position);
";

        const string DomainShaderTet = @"
%meta
Name = IsolineDS
ProfileDX10 = ds_5_0
ProfileGL3 = 400
InputControlPoints = 4
Domain = Isoline

%input
float4 Position : SDX10 = %name, SGL3 = %name
	
%input_extra
float2 TessCoord : SDX10 = SV_DomainLocation, SGL3 = gl_TessCoord
	
%input_patch
float LineTess : SDX10 = SV_TessFactor, SGL3 = gl_TessLevelOuter
	
%output
float4 Position : SDX10 = SV_Position, SGL3 = gl_Position
	
%code_main
	float u = INPUT_EXTRA(TessCoord).x;
    
    float3 p0 = INPUT(0, Position).xyz;
    float3 p1 = INPUT(1, Position).xyz;
    float3 p2 = INPUT(2, Position).xyz;
    float3 p3 = INPUT(3, Position).xyz;
    
    float u1 = 1.0 - u;
    float u2 = u * u;

    float b3 = u2 * u;
    float b2 = 3.0 * u2 * u1;
    float b1 = 3.0 * u * u1 * u1;
    float b0 = u1 * u1 * u1;

    OUTPUT(Position) = float4(p0 * b0 + p1 * b1 + p2 * b2 + p3 * b3, 1.0);
";

        const string PixelShaderText = @"
%meta
Name = YellowPS
ProfileDX9 = ps_2_0
ProfileDX10 = ps_4_0
ProfileGL3 = 150

%input
float4 Position : SDX10 = SV_Position, SGL3 = %unused, SDX9 = %unused
	
%output
float4 Color : SDX10 = SV_Target, SGL3 = %name, SDX9 = COLOR
	
%code_main
	OUTPUT(Color) = float4(1.0, 1.0, 0.0, 1.0);
";

        struct Vertex
        {
            public const int SizeInBytes = 2 * sizeof(float);

            public Vector2 Position;

            public Vertex(float x, float y)
            {
                Position = new Vector2(x, y);
            }
        }

        struct TessFactor
        {
            public int NumSegments;
            private IntVector3 padding;
            public const int SizeInBytes = 4 * sizeof(float);
        }

        readonly IShaderCombination shaderCombination;
        readonly IBuffer vertexBuffer;
        readonly IVertexLayout vertexLayout;
        readonly IBuffer tessFactorBuffer;

        public CurveTesselationScene(IEye eye, DisplayMode desctopDisplayMode)
            : base(eye, desctopDisplayMode)
        {
            var vertexShader = Device.Create.VertexShader(ShaderParser.Parse(VertexShaderText));
            var hullShader = Device.Create.HullShader(ShaderParser.Parse(HullShaderText));
            var domainShader = Device.Create.DomainShader(ShaderParser.Parse(DomainShaderTet));
            var pixelShader = Device.Create.PixelShader(ShaderParser.Parse(PixelShaderText));
            shaderCombination = Device.Create.ShaderCombination(vertexShader, hullShader, domainShader, null, pixelShader);

            vertexBuffer = Device.Create.Buffer(new BufferDescription
            {
                SizeInBytes = 12 * Vertex.SizeInBytes,
                Usage = Usage.Immutable,
                BindFlags = BindFlags.VertexBuffer,
            }, new SubresourceData(new[]            
            {
                new Vertex(-1f, 1/3f), new Vertex(4f, 1f), new Vertex(-4f, 1f), new Vertex(1f, 1/3f),    
                new Vertex(-1f, -1/3f), new Vertex(4f, 1/3f), new Vertex(-4f, 1/3f), new Vertex(1f, -1/3f),    
                new Vertex(-1f, -1f), new Vertex(4f, -1/3f), new Vertex(-4f, -1/3f), new Vertex(1f, -1f)    
            }));

            vertexLayout = Device.Create.VertexLayout(vertexShader, new[]
            {
                new VertexLayoutElement(ExplicitFormat.R32G32_FLOAT, 0, 0)
            });

            tessFactorBuffer = Device.Create.Buffer(new BufferDescription
            {
                SizeInBytes = TessFactor.SizeInBytes,
                Usage = Usage.Dynamic,
                BindFlags = BindFlags.UniformBuffer
            });
        }

        public unsafe override void NewFrame(IRealTime realTime)
        {
            if (SwapChain.BeginScene())
            {
                ImmediateContext.OutputMerger.RenderTargets.Set(SwapChain.GetCurrentColorBuffer());
                ImmediateContext.Rasterizer.Viewports.Set(new Viewport(SwapChain.Width, SwapChain.Height));
                ImmediateContext.ClearRenderTargetView(SwapChain.GetCurrentColorBuffer(), Color4.Violet);

                ImmediateContext.ShadersForDrawing = shaderCombination;
                ImmediateContext.InputAssembler.VertexLayout = vertexLayout;
                ImmediateContext.InputAssembler.PrimitiveTopology = PrimitiveTopology.PatchList4;
                ImmediateContext.InputAssembler.VertexSources[0] = new VertexSource(vertexBuffer, 0, Vertex.SizeInBytes);
                ImmediateContext.HullStage.UniformBuffers[0] = tessFactorBuffer;
                var tessFactor = new TessFactor();

                tessFactor.NumSegments = 4;
                ImmediateContext.SetSubresourceData(tessFactorBuffer, 0, new SubresourceData(new IntPtr(&tessFactor)));
                ImmediateContext.Draw(4, 0);

                tessFactor.NumSegments = 64;
                ImmediateContext.SetSubresourceData(tessFactorBuffer, 0, new SubresourceData(new IntPtr(&tessFactor)));
                ImmediateContext.Draw(4, 4);

                var amount = Math.Sin(realTime.TotalRealTime - Math.PI) * 0.5 + 0.5;
                tessFactor.NumSegments = (int)(32.0 * amount * amount) + 1;
                ImmediateContext.SetSubresourceData(tessFactorBuffer, 0, new SubresourceData(new IntPtr(&tessFactor)));
                ImmediateContext.Draw(4, 8);

                SwapChain.EndScene();
                SwapChain.Present();
            }
        }
    }
}
