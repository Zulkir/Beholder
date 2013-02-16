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
using Beholder.Basic;
using Beholder.Math;
using Beholder.Shaders;
using Beholder.Platform;
using Beholder.Resources;

namespace Launcher
{
    class CubeScene : Scene
    {
        const string VertexShaderText = @"
%meta
Name = DiffuseSpecularVS
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
float3 WorldPosition : SDX9 = TEXCOORD0, SDX10 = %name, 	  SGL3 = %name
float3 WorldNormal   : SDX9 = TEXCOORD1, SDX10 = %name, 	  SGL3 = %name
float2 TexCoord 	 : SDX9 = TEXCOORD2, SDX10 = %name, 	  SGL3 = %name

%code_main
    float4 worldPosition4 = mul(float4(INPUT(Position), 1.0), World);
    OUTPUT(Position) = mul(worldPosition4, ViewProjection);
    OUTPUT(WorldPosition) = worldPosition4.xyz;
    OUTPUT(WorldNormal) = normalize(mul(float4(INPUT(Normal), 0.0), WorldInverseTranspose).xyz);
    OUTPUT(TexCoord) = INPUT(TexCoord);
";

        const string PixelShaderText = @"
%meta
Name = DiffuseSpecularPS
ProfileDX9 = ps_2_0
ProfileDX10 = ps_4_0
ProfileGL3 = 150

%ubuffers
ubuffer CameraPixel : slot = 0, slotGL3 = 2, slotDX9 = c0
	float3 CameraPosition
ubuffer Light : slot = 1, slotGL3 = 3, slotDX9 = c1
	float3 AmbientColor
	float3 LightPosition
	float3 LightColor

%samplers
sampler TextureMapSampler : slot = 0

%srvs
Texture2D<float4> DiffuseTexture  : slot = 0, slotGL3 = 0 
Texture2D<float4> SpecularTexture : slot = 1, slotGL3 = 1 

%fixed_sampling
DiffuseTexture  : TextureMapSampler
SpecularTexture : TextureMapSampler

%input
float4 Position      : SDX9 = %unused,	 SDX10 = SV_Position, SGL3 = %unused
float3 WorldPosition : SDX9 = TEXCOORD0, SDX10 = %name,       SGL3 = %name
float3 WorldNormal   : SDX9 = TEXCOORD1, SDX10 = %name,       SGL3 = %name
float2 TexCoord      : SDX9 = TEXCOORD2, SDX10 = %name,       SGL3 = %name

%output
float4 Color : SDX9 = COLOR, SDX10 = SV_Target, SGL3 = %name

%code_main
    float3 toEye = normalize(CameraPosition - INPUT(WorldPosition));
    float3 toLight = normalize(LightPosition - INPUT(WorldPosition));
    float3 normal = normalize(INPUT(WorldNormal));
    
    float diffuseFactor = clamp(dot(toLight, normal), 0.0, 1.0);
    float specularFactor = pow(saturate(dot(toEye, reflect(-toLight, normal))), 24.0);
    
    float3 color3 = 
        sample(DiffuseTexture, INPUT(TexCoord)).xyz * saturate(diffuseFactor * LightColor + AmbientColor) + 
    	sample(SpecularTexture, INPUT(TexCoord)).xyz * specularFactor;

    OUTPUT(Color) = float4(color3, 1.0f);
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

        struct CameraPixel
        {
            public Vector3 CameraPosition;
            float padding0;
            public const int SizeInBytes = 4 * sizeof(float);
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

        IShaderCombination shaderCombination;
        Mesh cubeMesh;
        IVertexLayout vertexLayout;

        IBuffer transformBuffer;
        IBuffer cameraVertexBuffer;
        IBuffer cameraPixelBuffer;
        IBuffer lightBuffer;

        IShaderResourceView diffuseView;
        IShaderResourceView specualrView;
        ISamplerState samplerState;

        IDepthStencilState depthStencilState;

        public CubeScene(IEye eye, int displatFormatID, ISwapChain swapChain)
            : base(eye, displatFormatID, swapChain)
        {

        }

        protected override void Initialize()
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
                new BufferDescription {SizeInBytes = Transform.SizeInBytes, BindFlags = BindFlags.UniformBuffer, Usage = Usage.Dynamic});
            cameraVertexBuffer = Device.Create.Buffer(
                new BufferDescription { SizeInBytes = CameraVertex.SizeInBytes, BindFlags = BindFlags.UniformBuffer, Usage = Usage.Dynamic });
            cameraPixelBuffer = Device.Create.Buffer(
                new BufferDescription { SizeInBytes = CameraPixel.SizeInBytes, BindFlags = BindFlags.UniformBuffer, Usage = Usage.Dynamic });
            lightBuffer = Device.Create.Buffer(
                new BufferDescription { SizeInBytes = Light.SizeInBytes, BindFlags = BindFlags.UniformBuffer, Usage = Usage.Dynamic });

            var diffuseTexture = Device.Load.Texture2D("../Textures/DiffuseTEST.png");
            diffuseView = diffuseTexture.ViewAsShaderResource(diffuseTexture.FormatID, 0, diffuseTexture.MipLevels);

            var specularTexture = Device.Load.Texture2D("../Textures/SpecularTEST.png");
            specualrView = specularTexture.ViewAsShaderResource(specularTexture.FormatID, 0, diffuseTexture.MipLevels);

            samplerState = Device.Create.SamplerState(SamplerDescription.Default);
            depthStencilState = Device.Create.DepthStencilState(DepthStencilDescription.Enabled);
        }

        protected unsafe override void NewFrame(IRealTime realTime)
        {
            base.NewFrame(realTime);

            float angle = realTime.TotalRealTime * 0.25f;
            var cameraPosition = new Vector3(5, 3, 0);
            var world = Matrix4x4.CreateRotationX(angle) * Matrix4x4.CreateRotationY(2 * angle) * Matrix4x4.CreateRotationZ(3 * angle);
            var transform = new Transform
            {
                World = world,
                WorldInverseTranspose = Matrix4x4.Transpose(Matrix4x4.Invert(world))
            };

            var cameraVertex = new CameraVertex 
            { 
                ViewProjection = 
                Matrix4x4.CreateLookAtRH(cameraPosition, Vector3.Zero, Vector3.UnitZ) * 
                Matrix4x4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, SwapChain.GetAspectRatio(), 0.1f, 1000.0f)
            };
            var cameraPixel = new CameraPixel
            {
                CameraPosition = cameraPosition
            };
            var light = new Light
            {
                AmbientColor = new Color3(0.2f, 0.2f, 0.2f),
                LightPosition = new Vector3(10f, -7f, 2f),
                LightColor = Color3.White
            };

            if (SwapChain.BeginScene())
            {
                ImmediateContext.OutputMerger.RenderTargets.Set(SwapChain.GetCurrentColorBuffer());
                ImmediateContext.OutputMerger.DepthStencil = SwapChain.GetCurrentDepthStencilBuffer();
                ImmediateContext.Rasterizer.Viewports.Set(new Viewport(SwapChain.Width, SwapChain.Height));

                ImmediateContext.ClearRenderTargetView(SwapChain.GetCurrentColorBuffer(), Color4.Black);
                ImmediateContext.ClearDepthStencilView(SwapChain.GetCurrentDepthStencilBuffer(), ClearDepthStencilFlags.All, 1.0f, 0);

                ImmediateContext.ShadersForDrawing = shaderCombination;

                ImmediateContext.InputAssembler.VertexLayout = vertexLayout;
                ImmediateContext.InputAssembler.VertexSources[0] = cubeMesh.Vertices;
                ImmediateContext.InputAssembler.IndexSource = cubeMesh.Indices;
                ImmediateContext.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;

                ImmediateContext.SetSubresourceData(transformBuffer, 0, new SubresourceData((IntPtr)(&transform)));
                ImmediateContext.SetSubresourceData(cameraVertexBuffer, 0, new SubresourceData((IntPtr)(&cameraVertex)));
                ImmediateContext.SetSubresourceData(cameraPixelBuffer, 0, new SubresourceData((IntPtr)(&cameraPixel)));
                ImmediateContext.SetSubresourceData(lightBuffer, 0, new SubresourceData((IntPtr)(&light)));

                ImmediateContext.VertexStage.UniformBuffers[0] = transformBuffer;
                ImmediateContext.VertexStage.UniformBuffers[1] = cameraVertexBuffer;
                ImmediateContext.PixelStage.UniformBuffers[0] = cameraPixelBuffer;
                ImmediateContext.PixelStage.UniformBuffers[1] = lightBuffer;
                ImmediateContext.PixelStage.Samplers[0] = samplerState;
                ImmediateContext.PixelStage.ShaderResources[0] = diffuseView;
                ImmediateContext.PixelStage.ShaderResources[1] = specualrView;

                ImmediateContext.OutputMerger.DepthStencilState = depthStencilState;

                var meshSubset = cubeMesh.Subsets[0];
                ImmediateContext.DrawIndexed(meshSubset.IndexCount, meshSubset.StartIndexLocation, meshSubset.BaseVertexLocation);

                SwapChain.EndScene();
                SwapChain.Present();
            }
        }
    }
}