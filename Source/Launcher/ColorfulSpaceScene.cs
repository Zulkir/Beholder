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
    class ColorfulSpaceScene : Scene
    {
        const string VertexShaderText = @"
%meta
Name = LinearMovementVS
ProfileDX9 = vs_2_0
ProfileDX10 = vs_4_0
ProfileGL3 = 150

%ubuffers
ubuffer Time : slot = 0, slotGL3 = 0, slotDX9 = c0
	float CurrentTime

%input
float3 Position : SDX9 = POSITION, SDX10 = %name, SGL3 = %name
float3 Color    : SDX9 = COLOR,    SDX10 = %name, SGL3 = %name

%output
float4 Position : SDX9 = POSITION0, SDX10 = SV_Position, SGL3 = gl_Position
float3 Color    : SDX9 = COLOR,     SDX10 = %name,       SGL3 = %name

%code_main
	float normalizedOffset = frac(INPUT(Position).z + CurrentTime);
	OUTPUT(Position) = float4(INPUT(Position).x, INPUT(Position).y, 1.0 - normalizedOffset, 1.0);
	OUTPUT(Color) = INPUT(Color);
";

        const string GeometryShaderText = @"
%meta
Name = PointToQuadGS
ProfileDX10 = gs_4_0
ProfileGL3 = 150
MaxVertexCount = 4
InputPrimitiveType = Point
OutputPrimitiveType = Triangle

%ubuffers
ubuffer Camera : slot = 0, slotGL3 = 1
	float AspectRatio

%input
float4 Position : SDX10 = SV_Position, SGL3 = gl_Position
float3 Color    : SDX10 = %name,       SGL3 = %name

%output
float4 Position : SDX10 = SV_Position, SGL3 = gl_Position
float3 Color    : SDX10 = %name,       SGL3 = %name
float2 TexCoord : SDX10 = %name,       SGL3 = %name

%code_global
	static const float MaxHalfSize = 0.01;

%code_main
    float2 xy = INPUT(0, Position).xy;
	float z = INPUT(0, Position).z;
	float4 position = float4(xy / z, z, 1.0);
	float halfWidth = MaxHalfSize * (1.0 - z);
	float halfHeight = halfWidth * AspectRatio;
	
	OUTPUT(Position) = position + float4(-halfWidth, -halfHeight, 0.0, 0.0);
	OUTPUT(Color) = INPUT(0, Color);
	OUTPUT(TexCoord) = float2(-1.0, -1.0);
	EMIT
	
	OUTPUT(Position) = position + float4(halfWidth, -halfHeight, 0.0, 0.0);
	OUTPUT(Color) = INPUT(0, Color);
	OUTPUT(TexCoord) = float2(1.0, -1.0);
	EMIT
	
	OUTPUT(Position) = position + float4(-halfWidth, halfHeight, 0.0, 0.0);
	OUTPUT(Color) = INPUT(0, Color);
	OUTPUT(TexCoord) = float2(-1.0, 1.0);
	EMIT
	
	OUTPUT(Position) = position + float4(halfWidth, halfHeight, 0.0, 0.0);
	OUTPUT(Color) = INPUT(0, Color);
	OUTPUT(TexCoord) = float2(1.0, 1.0);
	EMIT
	
	RESTART
";

        const string PixelShaderText = @"
%meta
Name = StarEmulationPS
ProfileDX9 = ps_2_0
ProfileDX10 = ps_4_0
ProfileGL3 = 150

%input
float4 Position : SDX9 = %unused,   SDX10 = SV_Position, SGL3 = %unused
float3 Color    : SDX9 = COLOR,     SDX10 = %name,	     SGL3 = %name
float2 TexCoord : SDX9 = TEXCOORD0, SDX10 = %name,	     SGL3 = %name

%output
float4 Color : SDX9 = COLOR, SDX10 = SV_Target, SGL3 = %name

%code_main
	float fancyLength = abs(INPUT(TexCoord).x * INPUT(TexCoord).y);
	OUTPUT(Color) = float4(INPUT(Color), 1.0) * max(0.0, 1.0 - 10.0 * fancyLength);
";

        struct Vertex
        {
            public const int SizeInBytes = 6 * sizeof(float);

            public Vector3 Position;
            public Vector3 Color;

            public Vertex(float offset)
            {
                Position = new Vector3((float)Rand.NextDouble() - 0.5f, (float)Rand.NextDouble() - 0.5f, offset);

                // random saturated color
                switch (Rand.Next(1, 6))
                {
                    case 1: Color = new Vector3(1, 0, (float)Rand.NextDouble()); break;
                    case 2: Color = new Vector3(0, 1, (float)Rand.NextDouble()); break;
                    case 3: Color = new Vector3(1, (float)Rand.NextDouble(), 0); break;
                    case 4: Color = new Vector3(0, (float)Rand.NextDouble(), 1); break;
                    case 5: Color = new Vector3((float)Rand.NextDouble(), 1, 0); break;
                    case 6: Color = new Vector3((float)Rand.NextDouble(), 0, 1); break;
                    default: Color = new Vector3(1, 1, 1); break;
                }
            }

            static readonly Random Rand = new Random();
        }

        struct Time
        {
            public float CurrentTime;
            private Vector3 padding;
            public const int SizeInBytes = 4 * sizeof(float);
        }

        struct Camera
        {
            public float AspectRatio;
            private Vector3 padding;
            public const int SizeInBytes = 4 * sizeof(float);
        }

        const int ParticleCount = 512;
        const float ParticleSpeed = 0.75f;

        readonly IShaderCombination shaderCombination;
        readonly IBuffer vertexBuffer;
        readonly IVertexLayout vertexLayout;

        readonly IBuffer timeBuffer;
        readonly IBuffer cameraBuffer;

        readonly IBlendState blendState;

        public ColorfulSpaceScene(IEye eye, DisplayMode desctopDisplayMode)
            : base(eye, desctopDisplayMode)
        {
            var vertexShader = Device.Create.VertexShader(ShaderParser.Parse(VertexShaderText));
            var geometryShader = Device.Create.GeometryShader(ShaderParser.Parse(GeometryShaderText));
            var pixelShader = Device.Create.PixelShader(ShaderParser.Parse(PixelShaderText));
            shaderCombination = Device.Create.ShaderCombination(vertexShader, null, null, geometryShader, pixelShader);

            var vertexData = new Vertex[ParticleCount];
            for (int i = 0; i < vertexData.Length; i++)
                vertexData[i] = new Vertex((float)i / ParticleCount);

            vertexBuffer = Device.Create.Buffer(new BufferDescription
            {
                Usage = Usage.Immutable,
                SizeInBytes = ParticleCount * Vertex.SizeInBytes,
                BindFlags = BindFlags.VertexBuffer,
                ExtraFlags = ExtraFlags.Points
            }, new SubresourceData(vertexData));

            vertexLayout = Device.Create.VertexLayout(vertexShader, new[]
            {
                new VertexLayoutElement(ExplicitFormat.R32G32B32_FLOAT, 0, 0),
                new VertexLayoutElement(ExplicitFormat.R32G32B32_FLOAT, 0, 12)
            });

            timeBuffer = Device.Create.Buffer(new BufferDescription
            {
                Usage = Usage.Dynamic,
                SizeInBytes = Time.SizeInBytes,
                BindFlags = BindFlags.UniformBuffer
            });

            cameraBuffer = Device.Create.Buffer(new BufferDescription
            {
                Usage = Usage.Dynamic,
                SizeInBytes = Camera.SizeInBytes,
                BindFlags = BindFlags.UniformBuffer
            });

            blendState = Device.Create.BlendState(BlendDescription.Additive);
        }

        public unsafe override void NewFrame(IRealTime realTime)
        {
            if (SwapChain.BeginScene())
            {
                var time = new Time { CurrentTime = realTime.TotalRealTime * ParticleSpeed };
                var camera = new Camera { AspectRatio = SwapChain.GetAspectRatio() };
                ImmediateContext.SetSubresourceData(timeBuffer, 0, new SubresourceData(new IntPtr(&time)));
                ImmediateContext.SetSubresourceData(cameraBuffer, 0, new SubresourceData(new IntPtr(&camera)));

                ImmediateContext.OutputMerger.RenderTargets.Set(SwapChain.GetCurrentColorBuffer());
                ImmediateContext.Rasterizer.Viewports.Set(new Viewport(SwapChain.Width, SwapChain.Height));
                ImmediateContext.ClearRenderTargetView(SwapChain.GetCurrentColorBuffer(), Color4.Black);

                ImmediateContext.OutputMerger.BlendState = blendState;

                ImmediateContext.ShadersForDrawing = shaderCombination;
                ImmediateContext.InputAssembler.VertexLayout = vertexLayout;
                ImmediateContext.InputAssembler.PrimitiveTopology = PrimitiveTopology.PointList;
                ImmediateContext.InputAssembler.VertexSources[0] = new VertexSource(vertexBuffer, 0, Vertex.SizeInBytes);
                ImmediateContext.VertexStage.UniformBuffers[0] = timeBuffer;
                ImmediateContext.GeometryStage.UniformBuffers[0] = cameraBuffer;

                ImmediateContext.Draw(ParticleCount, 0);

                SwapChain.EndScene();
                SwapChain.Present();
            }
        }
    }
}
