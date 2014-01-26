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
using Beholder;
using Beholder.Basic;
using Beholder.Core;
using Beholder.Math;
using Beholder.Shaders;
using Beholder.Platform;
using Beholder.Resources;
using Beholder.Utility.Helpers;

namespace Launcher
{
    class RenderToTextureScene : Scene
    {
        const string VertexShaderText = @"
%meta
Name = DiffuseVS
ProfileDX9 = vs_2_0
ProfileDX10 = vs_4_0
ProfileGL3 = 150

%ubuffers 
ubuffer Transform : slot = 0, slotGL3 = 0, slotDX9 = c0
	float4x4 World
	float4x4 WorldInverseTranspose

ubuffer CameraVertex : slot = 1, slotGL3 = 1, slotDX9 = c8
	float4x4 ViewProjection

%input
float3 Position : SDX9 = POSITION, SDX10 = %name, SGL3 = %name
float3 Normal   : SDX9 = NORMAL,   SDX10 = %name, SGL3 = %name
float2 TexCoord : SDX9 = TEXCOORD, SDX10 = %name, SGL3 = %name

%output
float4 Position  	 : SDX9 = POSITION0, SDX10 = SV_Position, SGL3 = gl_Position
float3 WorldPosition : SDX9 = TEXCOORD0, SDX10 = %name,		  SGL3 = %name
float3 WorldNormal   : SDX9 = TEXCOORD1, SDX10 = %name,		  SGL3 = %name
float2 TexCoord 	 : SDX9 = TEXCOORD2, SDX10 = %name,		  SGL3 = %name

%code_main
    float4 worldPosition4 = mul(float4(INPUT(Position), 1.0), World);
    OUTPUT(Position) = mul(worldPosition4, ViewProjection);
    OUTPUT(WorldPosition) = worldPosition4.xyz;
    OUTPUT(WorldNormal) = normalize(mul(float4(INPUT(Normal), 0.0), WorldInverseTranspose).xyz);
    OUTPUT(TexCoord) = INPUT(TexCoord);
";

        const string PixelShaderText = @"
%meta
Name = DiffusePS
ProfileDX9 = ps_2_0
ProfileDX10 = ps_4_0
ProfileGL3 = 150

%ubuffers
ubuffer Light : slot = 0, slotGL3 = 2, slotDX9 = c0
	float3 AmbientColor
	float3 LightPosition
	float3 LightColor

%samplers
sampler TextureMapSampler : slot = 0

%srvs
Texture2D <float4> DiffuseTexture : slot = 0, slotGL3 = 0

%fixed_sampling
DiffuseTexture : TextureMapSampler

%input
float4 Position  	 : SDX9 = %unused,   SDX10 = SV_Position, SGL3 = %unused
float3 WorldPosition : SDX9 = TEXCOORD0, SDX10 = %name, 	  SGL3 = %name
float3 WorldNormal   : SDX9 = TEXCOORD1, SDX10 = %name, 	  SGL3 = %name
float2 TexCoord 	 : SDX9 = TEXCOORD2, SDX10 = %name, 	  SGL3 = %name

%output
float4 Color : SDX9 = COLOR, SDX10 = SV_Target, SGL3 = %name

%code_main
    float3 toLight = normalize(LightPosition - INPUT(WorldPosition));
    float3 normal = normalize(INPUT(WorldNormal));
    
    float diffuseFactor = clamp(dot(toLight, normal), 0.0f, 1.0f);
    
    OUTPUT(Color) = float4(
    	sample(DiffuseTexture, INPUT(TexCoord)).xyz * saturate(diffuseFactor * LightColor + AmbientColor),
    	1.0f);
";

        struct Transform
        {
            public Matrix4x4 World;
            public Matrix4x4 WorldInverseTranspose;
            public const int SizeInBytes = 32 * sizeof(float);
        }

        struct CameraVertex
        {
            public Matrix4x4 ViewProjection;
            public const int SizeInBytes = 16 * sizeof(float);
        }

        struct Light
        {
            public Color3 AmbientColor;
            private float padding0;
            public Vector3 LightPosition;
            private float padding1;
            public Color3 LightColor;
            private float padding2;
            public const int SizeInBytes = 12 * sizeof(float);
        }

        readonly IShaderCombination shaderCombination;
        readonly Mesh cubeMesh;
        readonly IVertexLayout vertexLayout;

        readonly IBuffer transformBuffer;
        readonly IBuffer cameraVertexBuffer;
        readonly IBuffer lightBuffer;

        const int TargetSize = 1 << 10;
        readonly IRenderTargetView targetRtv;
        readonly IShaderResourceView targetSrv;
        readonly IDepthStencilView targetDsv;

        readonly IShaderResourceView diffuseView;
        readonly ISamplerState samplerState;

        readonly IDepthStencilState depthStencilState;

        public RenderToTextureScene(IEye eye, DisplayMode desctopDisplayMode)
            : base(eye, desctopDisplayMode)
        {
            var vertexShader = Device.Create.VertexShader(ShaderParser.Parse(VertexShaderText));
            var pixelShader = Device.Create.PixelShader(ShaderParser.Parse(PixelShaderText));
            shaderCombination = Device.Create.ShaderCombination(vertexShader, null, null, null, pixelShader);

            var meshFactory = new MeshFactory(Device, Handedness.Right, Winding.Clockwise);
            cubeMesh = meshFactory.CreateCube(2.0f);

            vertexLayout = Device.Create.VertexLayout(vertexShader, new[]
            {
                new VertexLayoutElement(ExplicitFormat.R32G32B32_FLOAT, 0, 0),
                new VertexLayoutElement(ExplicitFormat.R32G32B32_FLOAT, 0, 12),
                new VertexLayoutElement(ExplicitFormat.R32G32_FLOAT, 0, 24)
            });

            transformBuffer = Device.Create.Buffer(
                new BufferDescription { SizeInBytes = Transform.SizeInBytes, BindFlags = BindFlags.UniformBuffer, Usage = Usage.Dynamic });
            cameraVertexBuffer = Device.Create.Buffer(
                new BufferDescription { SizeInBytes = CameraVertex.SizeInBytes, BindFlags = BindFlags.UniformBuffer, Usage = Usage.Dynamic });
            lightBuffer = Device.Create.Buffer(
                new BufferDescription { SizeInBytes = Light.SizeInBytes, BindFlags = BindFlags.UniformBuffer, Usage = Usage.Dynamic });

            var renderTargetFormats = Eye.Adapters[0]
                .GetSupportedFormats(FormatSupport.Texture2D | FormatSupport.RenderTarget | FormatSupport.MipAutogen)
                .Where(fi => fi.ColorBits <= 24);
            if (!renderTargetFormats.Any()) throw new NotSupportedException("Render target textures are not supported.");
            var renderTargetFormatInfo = renderTargetFormats
                .OrderByDescending(fi => fi.ColorBits)
                .ThenBy(fi => fi.TotalBits)
                .First();

            var renderTargetTexture = Device.Create.Texture2D(new Texture2DDescription
            {
                Width = TargetSize,
                Height = TargetSize,
                MipLevels = TextureHelper.MipLevels(TargetSize, TargetSize, 1),
                ArraySize = 1,
                FormatID = renderTargetFormatInfo.ID,
                Sampling = Sampling.NoMultisampling,
                Usage = Usage.Default,
                BindFlags = BindFlags.RenderTarget | BindFlags.ShaderResource,
                MiscFlags = MiscFlags.GenerateMips,
                ExtraFlags = ExtraFlags.None
            });
            targetRtv = renderTargetTexture.ViewAsRenderTarget(renderTargetFormatInfo.ID, 0);
            targetSrv = renderTargetTexture.ViewAsShaderResource(renderTargetFormatInfo.ID, 0, renderTargetTexture.MipLevels);

            var dsFormats = Eye.Adapters[0].GetSupportedFormats(FormatSupport.Texture2D | FormatSupport.DepthStencil);
            var dsFormatInfo = dsFormats
                .OrderBy(fi => (fi.ColorBits == 24 && fi.AlphaBits == 8) ? 0 : 1)
                .ThenByDescending(fi => fi.ColorBits)
                .ThenBy(fi => fi.TotalBits)
                .First();

            var depthStencilTexture = Device.Create.Texture2D(new Texture2DDescription
            {
                Width = TargetSize,
                Height = TargetSize,
                MipLevels = 1,
                ArraySize = 1,
                FormatID = dsFormatInfo.ID,
                Sampling = Sampling.NoMultisampling,
                Usage = Usage.Default,
                BindFlags = BindFlags.DepthStencil,
                MiscFlags = MiscFlags.None,
                ExtraFlags = ExtraFlags.None
            });
            targetDsv = depthStencilTexture.ViewAsDepthStencil(dsFormatInfo.ID, DepthStencilViewFlags.None, 0);

            var diffuseTexture = Device.Load.Texture2D("../Textures/DiffuseTest.png");
            diffuseView = diffuseTexture.ViewAsShaderResource(diffuseTexture.FormatID, 0, diffuseTexture.MipLevels);

            samplerState = Device.Create.SamplerState(SamplerDescription.Anisotropic);
            depthStencilState = Device.Create.DepthStencilState(DepthStencilDescription.Enabled);
        }


        public unsafe override void NewFrame(IRealTime realTime)
        {
            float angle = realTime.TotalRealTime * 0.125f;
            var cameraPosition = new Vector3(5, 3, 0);
            var world = Matrix4x4.CreateRotationX(angle) * Matrix4x4.CreateRotationY(2 * angle) * Matrix4x4.CreateRotationZ(3 * angle);
            var transform = new Transform
            {
                World = world,
                WorldInverseTranspose = Matrix4x4.Transpose(Matrix4x4.Invert(world))
            };
            var light = new Light
            {
                AmbientColor = new Color3(0.2f, 0.2f, 0.2f),
                LightPosition = new Vector3(10f, -7f, 2f),
                LightColor = Color3.White
            };

            if (SwapChain.BeginScene())
            {
                ImmediateContext.ShadersForDrawing = shaderCombination;

                ImmediateContext.InputAssembler.VertexLayout = vertexLayout;
                ImmediateContext.InputAssembler.VertexSources[0] = cubeMesh.Vertices;
                ImmediateContext.InputAssembler.IndexSource = cubeMesh.Indices;
                ImmediateContext.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;

                ImmediateContext.SetSubresourceData(transformBuffer, 0, new SubresourceData((IntPtr)(&transform)));
                ImmediateContext.SetSubresourceData(lightBuffer, 0, new SubresourceData((IntPtr)(&light)));

                ImmediateContext.VertexStage.UniformBuffers[0] = transformBuffer;
                ImmediateContext.VertexStage.UniformBuffers[1] = cameraVertexBuffer;
                ImmediateContext.PixelStage.UniformBuffers[0] = lightBuffer;
                ImmediateContext.PixelStage.Samplers[0] = samplerState;

                ImmediateContext.OutputMerger.DepthStencilState = depthStencilState;

                var meshSubset = cubeMesh.Subsets[0];

                // Render Target
                ImmediateContext.OutputMerger.RenderTargets.Set(targetRtv);
                ImmediateContext.OutputMerger.DepthStencil = targetDsv;
                ImmediateContext.Rasterizer.Viewports.Set(new Viewport(TargetSize, TargetSize));

                ImmediateContext.ClearRenderTargetView(targetRtv, Color4.CornflowerBlue);
                ImmediateContext.ClearDepthStencilView(targetDsv, ClearDepthStencilFlags.All, 1.0f, 0);

                var cameraVertex = new CameraVertex
                {
                    ViewProjection =
                    Matrix4x4.CreateLookAtRH(cameraPosition, Vector3.Zero, Vector3.UnitZ) *
                    Matrix4x4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, 1.0f, 0.1f, 1000.0f)
                };
                ImmediateContext.SetSubresourceData(cameraVertexBuffer, 0, new SubresourceData((IntPtr)(&cameraVertex)));

                ImmediateContext.PixelStage.ShaderResources[0] = diffuseView;

                ImmediateContext.DrawIndexed(meshSubset.IndexCount, meshSubset.StartIndexLocation, meshSubset.BaseVertexLocation);
                ImmediateContext.GenerateMips(targetSrv);

                // Swap Chain
                ImmediateContext.OutputMerger.RenderTargets.Set(SwapChain.GetCurrentColorBuffer());
                ImmediateContext.OutputMerger.DepthStencil = SwapChain.GetCurrentDepthStencilBuffer();
                ImmediateContext.Rasterizer.Viewports.Set(new Viewport(SwapChain.Width, SwapChain.Height));

                ImmediateContext.ClearRenderTargetView(SwapChain.GetCurrentColorBuffer(), Color4.Blue);
                ImmediateContext.ClearDepthStencilView(SwapChain.GetCurrentDepthStencilBuffer(), ClearDepthStencilFlags.All, 1.0f, 0);

                cameraVertex = new CameraVertex
                {
                    ViewProjection =
                    Matrix4x4.CreateLookAtRH(cameraPosition, Vector3.Zero, Vector3.UnitZ) *
                    Matrix4x4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, SwapChain.GetAspectRatio(), 0.1f, 1000.0f)
                };
                ImmediateContext.SetSubresourceData(cameraVertexBuffer, 0, new SubresourceData((IntPtr)(&cameraVertex)));

                ImmediateContext.PixelStage.ShaderResources[0] = targetSrv;

                ImmediateContext.DrawIndexed(meshSubset.IndexCount, meshSubset.StartIndexLocation, meshSubset.BaseVertexLocation);

                SwapChain.EndScene();
                SwapChain.Present();
            }
        }
    }
}
