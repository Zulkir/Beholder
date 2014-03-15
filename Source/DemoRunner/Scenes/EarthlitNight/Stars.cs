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
using System.Linq;
using System.Runtime.InteropServices;
using Beholder;
using Beholder.Core;
using Beholder.Math;
using Beholder.Platform;
using Beholder.Resources;
using Beholder.Shaders;

namespace DemoRunner.Scenes.EarthlitNight
{
    public class Stars
    {
        private readonly int starCount;
        private readonly IShaderCombination shaders;
        private readonly IBuffer timeUbuffer;
        private readonly IVertexLayout vertexLayout;
        private readonly IBuffer vertexBuffer;
        private readonly IBuffer indexBuffer;
        private readonly IRasterizerState rasterizerState;
        private readonly IDepthStencilState depthStencilState;
        private readonly IBlendState blendState;

        public Stars(IDevice device, int starCount)
        {
            this.starCount = starCount;
            shaders = CreateShaders(device);
            timeUbuffer = CreateTimeUbuffer(device);
            vertexLayout = CreateVertexLayout(device, shaders.VertexShader);
            vertexBuffer = CreateVertexBuffer(device, starCount);
            indexBuffer = CreateIndexBuffer(device, starCount);
            rasterizerState = CreateRasterizerState(device);
            depthStencilState = CreateDepthStencilState(device);
            blendState = CreateBlendState(device);
        }

        public unsafe void Draw(IDeviceContext context, IRealTime realTime)
        {
            context.ShadersForDrawing = shaders;

            float totalRealTime = realTime.TotalRealTime;
            context.SetSubresourceData(timeUbuffer, 0, new SubresourceData(new IntPtr(&totalRealTime)));
            context.VertexStage.UniformBuffers[0] = timeUbuffer;

            context.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;
            context.InputAssembler.VertexLayout = vertexLayout;
            context.InputAssembler.VertexSources[0] = new VertexSource(vertexBuffer, 0, Vertex.SizeInBytes);
            context.InputAssembler.IndexSource = new IndexSource(indexBuffer, 0, IndexFormat.SixteenBit);

            context.Rasterizer.State = rasterizerState;
            context.OutputMerger.DepthStencilState = depthStencilState;
            context.OutputMerger.BlendState = blendState;

            context.DrawIndexed(starCount * 6, 0, 0);
        }

        #region Common Helpers

        private static readonly Random Random = new Random();

        private static float RandomInRange(float min, float max, Func<float, float> distribution = null)
        {
            distribution = distribution ?? (x => x);
            return min + (max - min) *  distribution((float)Random.NextDouble());
        }

        #endregion

        #region Shaders

        const string VertexShaderText = @"
%meta
Name = StarVS
ProfileDX9 = vs_2_0
ProfileDX10 = vs_4_0
ProfileGL3 = 150

%ubuffers
ubuffer Time : slot = 0, slotGL3 = 0, slotDX9 = c0
	float CurrentTime

%input
float2 Position    : SDX9 = POSITION,  SDX10 = %name, SGL3 = %name
float2 TexCoord    : SDX9 = TEXCOORD,  SDX10 = %name, SGL3 = %name
float3 Color       : SDX9 = COLOR,     SDX10 = %name, SGL3 = %name
float  ShinePeriod : SDX9 = TEXCOORD2, SDX10 = %name, SGL3 = %name

%output
float4 Position : SDX9 = POSITION0, SDX10 = SV_Position, SGL3 = gl_Position
float2 TexCoord : SDX9 = TEXCOORD,  SDX10 = %name, SGL3 = %name
float3 Color    : SDX9 = COLOR,     SDX10 = %name, SGL3 = %name

%code_main
    OUTPUT(Position) = float4(INPUT(Position), 0.0, 1.0);
    OUTPUT(TexCoord) = INPUT(TexCoord);
    float twoPi = 6.28;
    float shinePhase = 0.5 * (1.0 + sin(twoPi * CurrentTime / INPUT(ShinePeriod)));
    float shininess = 0.35 + 0.65 * shinePhase;
	OUTPUT(Color) = shininess * INPUT(Color);
";

        const string PixelShaderText = @"
%meta
Name = StarPS
ProfileDX9 = ps_2_0
ProfileDX10 = ps_4_0
ProfileGL3 = 150

%input
float4 Position : SDX9 = %unused,   SDX10 = SV_Position, SGL3 = %unused
float2 TexCoord : SDX9 = TEXCOORD0, SDX10 = %name,	     SGL3 = %name
float3 Color    : SDX9 = COLOR,     SDX10 = %name,	     SGL3 = %name

%output
float4 Color    : SDX9 = COLOR, SDX10 = SV_Target, SGL3 = %name

%code_main
    float squareLength = dot(INPUT(TexCoord), INPUT(TexCoord));
    float intensity = saturate(1.0 - squareLength);
	OUTPUT(Color) = float4(INPUT(Color), 1.0) * intensity;
";

        private static IShaderCombination CreateShaders(IDevice device)
        {
            var vertexShader = device.Create.VertexShader(ShaderParser.Parse(VertexShaderText));
            var pixelShader = device.Create.PixelShader(ShaderParser.Parse(PixelShaderText));
            return device.Create.ShaderCombination(vertexShader, null, null, null, pixelShader);
        }

        #endregion

        #region Ubuffers

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        private struct TimeUbuffer
        {
            public float CurrentTime;

            public const int SizeInBytes = sizeof(float);
        }

        private IBuffer CreateTimeUbuffer(IDevice device)
        {
            return device.Create.Buffer(new BufferDescription
            {
                BindFlags = BindFlags.UniformBuffer,
                Usage = Usage.Dynamic,
                SizeInBytes = TimeUbuffer.SizeInBytes
            });
        }

        #endregion

        #region Vertices

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        private struct Vertex
        {
            public Vector2 Position;
            public Vector2 TexCoord;
            public Color3 Color;
            public float ShinePeriod;

            public const int SizeInBytes = 2 * 4 * sizeof(float);
        }

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        private struct VertexQuad
        {
            public Vertex TopLeft;
            public Vertex TopRight;
            public Vertex BottomRight;
            public Vertex BottomLeft;

            public const int SizeInBytes = 4 * Vertex.SizeInBytes;
        }

        private static IVertexLayout CreateVertexLayout(IDevice device, IVertexShader vertexShader)
        {
            return device.Create.VertexLayout(vertexShader, new[]
            {
                new VertexLayoutElement(ExplicitFormat.R32G32_FLOAT, 0, 0),
                new VertexLayoutElement(ExplicitFormat.R32G32_FLOAT, 0, 2 * sizeof(float)),
                new VertexLayoutElement(ExplicitFormat.R32G32B32_FLOAT, 0, 4 * sizeof(float)),
                new VertexLayoutElement(ExplicitFormat.R32_FLOAT, 0, 7 * sizeof(float)),
            });
        }

        private static IBuffer CreateVertexBuffer(IDevice device, int starCount)
        {
            var vertexData = Enumerable.Range(0, starCount).Select(i => CreateRandomVertexQuad()).ToArray();
            return device.Create.Buffer(new BufferDescription
            {
                BindFlags = BindFlags.VertexBuffer,
                Usage = Usage.Immutable,
                SizeInBytes = vertexData.Length * VertexQuad.SizeInBytes,
            }, new SubresourceData(vertexData));
        }

        private static VertexQuad CreateRandomVertexQuad()
        {
            var center = new Vector2(RandomInRange(-1f, 1f), RandomInRange(-1f, 1f));
            var radius = RandomInRange(0.001f, 0.005f, x => (float)Math.Pow(x, 6));
            var color = new Color3(RandomInRange(0.8f, 1.0f), 0.0f, RandomInRange(0.8f, 1.0f));
            color.G = Math.Min(color.R, color.B);
            var shinePeriod = RandomInRange(1f, 5f);
            return CreateVertexQuad(center, radius, color, shinePeriod);
        }

        private static VertexQuad CreateVertexQuad(Vector2 center, float radius, Color3 color, float shinePeriod)
        {
            return new VertexQuad
            {
                TopLeft = new Vertex
                {
                    Position = center + new Vector2(-radius, -radius),
                    TexCoord = new Vector2(-1f, -1f),
                    Color = color,
                    ShinePeriod = shinePeriod
                },
                TopRight = new Vertex
                {
                    Position = center + new Vector2(radius, -radius),
                    TexCoord = new Vector2(1f, -1f),
                    Color = color,
                    ShinePeriod = shinePeriod
                },
                BottomRight = new Vertex
                {
                    Position = center + new Vector2(radius, radius),
                    TexCoord = new Vector2(1f, 1f),
                    Color = color,
                    ShinePeriod = shinePeriod
                },
                BottomLeft = new Vertex
                {
                    Position = center + new Vector2(-radius, radius),
                    TexCoord = new Vector2(-1f, 1f),
                    Color = color,
                    ShinePeriod = shinePeriod
                }
            };
        }

        #endregion

        #region Indices

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        private struct IndexQuad
        {
            public ushort I0, I1, I2, I3, I4, I5;

            public const int SizeInBytes = 6 * sizeof(ushort);
        }

        private static IBuffer CreateIndexBuffer(IDevice device, int starCount)
        {
            var indexData = Enumerable.Range(0, starCount).Select(CreateIndexQuad).ToArray();
            return device.Create.Buffer(new BufferDescription
            {
                BindFlags = BindFlags.IndexBuffer,
                Usage = Usage.Immutable,
                ExtraFlags = ExtraFlags.SixteenBitIndices,
                SizeInBytes = indexData.Length * IndexQuad.SizeInBytes
            }, new SubresourceData(indexData));
        }

        private static IndexQuad CreateIndexQuad(int starIndex)
        {
            int offset = starIndex * 4;
            return new IndexQuad
            {
                I0 = (ushort)(offset),
                I1 = (ushort)(offset + 1),
                I2 = (ushort)(offset + 2),
                I3 = (ushort)(offset),
                I4 = (ushort)(offset + 2),
                I5 = (ushort)(offset + 3),
            };
        }

        #endregion

        #region States

        private IRasterizerState CreateRasterizerState(IDevice device)
        {
            return device.Create.RasterizerState(new RasterizerDescription
            {
                DepthBias = 0,
                DepthBiasClamp = 0,
                SlopeScaledDepthBias = 0,
                FillMode = FillMode.Solid,
                CullMode = Cull.None,
                DepthClipEnable = false,
                ScissorEnable = false,
                MultisampleEnable = false,
                AntialiasedLineEnable = false,
                FrontFaceWinding = Winding.Clockwise
            });
        }

        private IDepthStencilState CreateDepthStencilState(IDevice device)
        {
            return device.Create.DepthStencilState(new DepthStencilDescription
            {
                FrontFaceStencilOp = DepthStencilOperationDescription.Default,
                BackFaceStencilOp = DepthStencilOperationDescription.Default,
                DepthEnable = false,
                DepthWriteEnable = false,
                DepthFunction = Comparison.Never,
                StencilEnable = false,
                StencilReadMask = 0,
                StencilWriteMask = 0
            });
        }

        private IBlendState CreateBlendState(IDevice device)
        {
            return device.Create.BlendState(new BlendDescription
            {
                AlphaToCoverageEnable = false,
                IndependentBlendEnable = false,
                RenderTargetBlend0 = new RenderTargetBlendDescription
                {
                    BlendEnable = true,
                    SourceBlend = Blend.One,
                    DestinationBlend = Blend.One,
                    BlendOperation = BlendOperation.Add,
                    SourceBlendAlpha = Blend.One,
                    DestinationBlendAlpha = Blend.One,
                    BlendOperationAlpha = BlendOperation.Add,
                    RenderTargetWriteMask = ColorMask.All
                }
            });
        }

        #endregion
    }
}