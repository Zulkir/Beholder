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
using Beholder.Core;
using Beholder.Libraries.SharpDX9.Resources;
using Beholder.Libraries.SharpDX9.Shaders;
using Beholder.Math;
using Beholder.Resources;
using Beholder.Utility.ForImplementations.Core;
using SharpDX;
using SharpDX.Direct3D9;
using Color4 = Beholder.Math.Color4;
using Vector4 = Beholder.Math.Vector4;
using Viewport = Beholder.Core.Viewport;
using Beholder.Utility.Extensions;

namespace Beholder.Libraries.SharpDX9.Core
{
    public class CDeviceContext : DeviceContextBase<ICDevice>, IDisposable
    {
        Device d3dDevice;

        readonly ConstantVariablesSetter<float> vertexConstantSetterC; 
        readonly ConstantVariablesSetter<int> vertexConstantSetterI; 
        readonly ConstantVariablesSetter<bool> vertexConstantSetterB;

        readonly ConstantVariablesSetter<float> pixelConstantSetterC;
        readonly ConstantVariablesSetter<int> pixelConstantSetterI;
        readonly ConstantVariablesSetter<bool> pixelConstantSetterB;

        readonly CRasterizerState defaultRasterizerState;
        readonly CDepthStencilState defaultDepthStencilState;
        readonly CBlendState defaultBlendState;
        readonly CSamplerState defaultSamplerState;

        public override DeviceContextType ContextType { get { return DeviceContextType.Immediate; } }

        public CDeviceContext(ICDevice device) 
            : base(device)
        {
            d3dDevice = device.D3DDevice;

            vertexConstantSetterC = new ConstantVariablesSetter<float>(d3dDevice, 256, (d, r, a) => d.SetVertexShaderConstant(r, a));
            vertexConstantSetterI = new ConstantVariablesSetter<int>(d3dDevice, 16, (d, r, a) => d.SetVertexShaderConstant(r, a));
            vertexConstantSetterB = new ConstantVariablesSetter<bool>(d3dDevice, 16, (d, r, a) => d.SetVertexShaderConstant(r, a));

            pixelConstantSetterC = new ConstantVariablesSetter<float>(d3dDevice, 256, (d, r, a) => d.SetPixelShaderConstant(r, a));
            pixelConstantSetterI = new ConstantVariablesSetter<int>(d3dDevice, 16, (d, r, a) => d.SetPixelShaderConstant(r, a));
            pixelConstantSetterB = new ConstantVariablesSetter<bool>(d3dDevice, 16, (d, r, a) => d.SetPixelShaderConstant(r, a));

            defaultRasterizerState = (CRasterizerState)device.Create.RasterizerState(RasterizerDescription.Default);
            defaultDepthStencilState = (CDepthStencilState)device.Create.DepthStencilState(DepthStencilDescription.Default);
            defaultBlendState = (CBlendState)device.Create.BlendState(BlendDescription.Default);
            defaultSamplerState = (CSamplerState)device.Create.SamplerState(SamplerDescription.Default);
        }

        public void OnDeviceReset()
        {
            MarkAsDirty();
            vertexConstantSetterC.OnDeviceReset();
            vertexConstantSetterI.OnDeviceReset();
            vertexConstantSetterB.OnDeviceReset();
            pixelConstantSetterC.OnDeviceReset();
            pixelConstantSetterI.OnDeviceReset();
            pixelConstantSetterB.OnDeviceReset();
        }

        public void Dispose()
        {
            if (d3dDevice != null)
            {
                d3dDevice.Dispose(); 
                d3dDevice = null;
            }
        }

        public override void ClearRenderTargetView(IRenderTargetView renderTargetView, Color4 color)
        {
            var cView = (CRenderTargetView)renderTargetView;
            var sdxColor = new ColorBGRA(color.R, color.G, color.B, color.A);

            if (OutputMerger.RenderTargets.IsDirty || OutputMerger.RenderTargets.CurrentCount != 1 || OutputMerger.RenderTargets[0] != cView)
            {
                d3dDevice.SetRenderTarget(0, cView.D3DSurface);
                for (int i = 1; i < OutputMerger.RenderTargets.CurrentCount; i++)
                    d3dDevice.SetRenderTarget(i, null);
                d3dDevice.DepthStencilSurface = null;

                OutputMerger.RenderTargets.MarkAsDirty();
                OutputMerger.DepthStencil.MarkAsDirty();
            }

            var currentViewport = Rasterizer.Viewports[0];
            var requiredViewport = new Viewport(cView.Width, cView.Height);
            if (Rasterizer.Viewports.IsDirty || !Viewport.Equals(ref currentViewport, ref requiredViewport))
            {
                d3dDevice.Viewport = CtSharpDX9.Viewport(requiredViewport);
                Rasterizer.Viewports.MarkAsDirty();
            }

            d3dDevice.Clear(ClearFlags.Target, sdxColor, 1.0f, 0);
        }

        public override void ClearDepthStencilView(IDepthStencilView depthStencilView, ClearDepthStencilFlags flags, float depth, byte stencil)
        {
            var cView = (CDepthStencilView)depthStencilView;
            var d3dFlags = CtSharpDX9.ClearFlags(flags);

            if (OutputMerger.DepthStencil.IsDirty || OutputMerger.DepthStencil.Value != cView)
            {
                d3dDevice.DepthStencilSurface = cView.D3DSurface;
                OutputMerger.DepthStencil.MarkAsDirty();
            }

            var currentViewport = Rasterizer.Viewports[0];
            var requiredViewport = new Viewport(cView.Width, cView.Height);
            if (Rasterizer.Viewports.IsDirty || !Viewport.Equals(ref currentViewport, ref requiredViewport))
            {
                d3dDevice.Viewport = CtSharpDX9.Viewport(requiredViewport);
                Rasterizer.Viewports.MarkAsDirty();
            }

            d3dDevice.Clear(d3dFlags, new ColorBGRA(), depth, stencil);
        }

        public override void ClearUnorderedAccessView(IUnorderedAccessView unorderedAccessView, Vector4 value)
        {
            throw new NotSupportedException("UAVs are not supported by D3D9");
        }

        public override void ClearUnorderedAccessView(IUnorderedAccessView unorderedAccessView, IntVector4 value)
        {
            throw new NotSupportedException("UAVs are not supported by D3D9");
        }

        public override void Dispatch(int threadGroupCountX, int threadGroupCountY, int threadGroupCountZ)
        {
            throw new NotSupportedException("Dispatch pipeline is not supported by D3D9");
        }

        public override void DispatchIndirect(IBuffer bufferForArgs, int alignedByteOffsetForArgs)
        {
            throw new NotSupportedException("Dispatch pipeline is not supported by D3D9");
        }

        void PreDraw()
        {
            ConsumeInputAssembler();
            ConsumeRasterizer();
            ConsumeOutputMerger();
            ConsumeShaders();
        }

        void ConsumeShaders()
        {
            var cShaderCombination = (CShaderCombination)ShadersForDrawing.Value;
            var cVertexShader = cShaderCombination.VertexShader;
            var cPixelShader = cShaderCombination.PixelShader;

            if (ShadersForDrawing.IsDirty)
            {
                d3dDevice.VertexShader = cVertexShader.D3DShader;
                d3dDevice.PixelShader = cPixelShader.D3DShader;

                ConsumeShaderStage(cVertexShader, VertexStage, vertexConstantSetterC, vertexConstantSetterI, vertexConstantSetterB, 0x00100000, true);
                ConsumeShaderStage(cPixelShader, PixelStage, pixelConstantSetterC, pixelConstantSetterI, pixelConstantSetterB, 0, true);

                ShadersForDrawing.Clean();
            }
            else
            {
                ConsumeShaderStage(cVertexShader, VertexStage, vertexConstantSetterC, vertexConstantSetterI, vertexConstantSetterB, 0x00100000, false);
                ConsumeShaderStage(cPixelShader, PixelStage, pixelConstantSetterC, pixelConstantSetterI, pixelConstantSetterB, 0, false);
            }
        }

        void ConsumeShaderStage(CShader shader, DeviceContextBaseShaderStage stage,
            ConstantVariablesSetter<float> constantSetterC, 
            ConstantVariablesSetter<int> constantSetterI, 
            ConstantVariablesSetter<bool> constantSetterB, 
            int samplerFlag, bool force)
        {
            foreach (var variable in shader.UniformBufferRegistersC.GetActive())
                constantSetterC.SetBuffer((CBufferUniform<float>)stage.UniformBuffers[variable.BSlot], variable.Register, variable.RegisterCount);
            foreach (var variable in shader.UniformBufferRegistersI.GetActive())
                constantSetterI.SetBuffer((CBufferUniform<int>)stage.UniformBuffers[variable.BSlot], variable.Register, variable.RegisterCount);
            foreach (var variable in shader.UniformBufferRegistersB.GetActive())
                constantSetterB.SetBuffer((CBufferUniform<bool>)stage.UniformBuffers[variable.BSlot], variable.Register, variable.RegisterCount);
            stage.UniformBuffers.Clean();

            if (force)
            {
                foreach (var variable in shader.Textures)
                    d3dDevice.SetTexture(samplerFlag | variable.Slot, stage.ShaderResources[variable.Slot].NullOrFunc(t => ((CShaderResourceView)t).D3DBaseTexture));
                foreach (var kvp in shader.SamplerMap)
                    foreach (var register in kvp.Value)
                        ((CSamplerState)stage.Samplers[kvp.Key] ?? defaultSamplerState).Apply(register);

                stage.ShaderResources.Clean();
                stage.Samplers.Clean();
            }
            else
            {
                if (stage.ShaderResources.IsDirty)
                {
                    foreach (var dirtyIndex in stage.ShaderResources.DirtyIndices)
                        d3dDevice.SetTexture(samplerFlag | dirtyIndex, stage.ShaderResources[dirtyIndex].NullOrFunc(t => ((CShaderResourceView)t).D3DBaseTexture));
                    stage.ShaderResources.Clean();
                }

                if (stage.Samplers.IsDirty)
                {
                    foreach (var dirtyIndex in stage.Samplers.DirtyIndices)
                    {
                        int[] registers;
                        if (shader.SamplerMap.TryGetValue(dirtyIndex, out registers))
                            foreach (var register in registers)
                                ((CSamplerState)stage.Samplers[dirtyIndex] ?? defaultSamplerState).Apply(register);
                    }
                        
                    stage.Samplers.Clean();
                }
            }
        }

        void ConsumeInputAssembler()
        {
            if (InputAssembler.VertexSources.IsDirty)
            {
                foreach (var dirtyIndex in InputAssembler.VertexSources.DirtyIndices)
                {
                    var vertexSource = InputAssembler.VertexSources[dirtyIndex];
                    d3dDevice.SetStreamSource(dirtyIndex, vertexSource.Buffer.NullOrFunc(b => ((CBufferVertex)b).D3DBuffer), vertexSource.Offset, vertexSource.Stride);
                }
                InputAssembler.VertexSources.Clean();
            }

            if (InputAssembler.VertexLayout.IsDirty)
            {
                d3dDevice.VertexDeclaration = ((CVertexLayout)InputAssembler.VertexLayout.Value).D3DVertexDeclaration;
                InputAssembler.VertexLayout.Clean();
            }

            if (InputAssembler.IndexSource.IsDirty)
            {
                var indexSource = InputAssembler.IndexSource.Value;
                if (indexSource.Offset != 0)
                    throw new NotSupportedException("Index source offset is not supported by D3D9");
                if (indexSource.Buffer != null)
                {
                    if (indexSource.Buffer.ExtraFlags.HasFlag(ExtraFlags.SixteenBitIndices) && indexSource.Format != IndexFormat.SixteenBit)
                        throw new NotSupportedException("Only buffers with SixteenBitIndices extra flag set can be used as 16-bit indices by D3D9");
                    if (!indexSource.Buffer.ExtraFlags.HasFlag(ExtraFlags.SixteenBitIndices) && indexSource.Format != IndexFormat.ThirtyTwoBit)
                        throw new NotSupportedException("Only buffers without SixteenBitIndices extra flag set can be used as 32-bit indices by D3D9");
                }
                d3dDevice.Indices = indexSource.Buffer.NullOrFunc(b => ((CBufferIndex)b).D3DBuffer);
                InputAssembler.IndexSource.Clean();
            }
        }

        void ConsumeRasterizer()
        {
            if (Rasterizer.State.IsDirty)
            {
                ((CRasterizerState)Rasterizer.State.Value ?? defaultRasterizerState).Apply();
                Rasterizer.State.Clean();
            }

            if (Rasterizer.ScissorRectangles.IsDirty)
            {
                if (Rasterizer.ScissorRectangles.CurrentCount >= 1)
                    d3dDevice.ScissorRect = CtSharpDX9.Rectangle(Rasterizer.ScissorRectangles[0]);
                Rasterizer.ScissorRectangles.Clean();
            }

            if (Rasterizer.Viewports.IsDirty)
            {
                if (Rasterizer.Viewports.CurrentCount >= 1)
                    d3dDevice.Viewport = CtSharpDX9.Viewport(Rasterizer.Viewports[0]);
                Rasterizer.Viewports.Clean();
            }
        }

        void ConsumeOutputMerger()
        {
            if (OutputMerger.RenderTargets.IsDirty)
            {
                for (int i = 0; i < OutputMerger.RenderTargets.CurrentCount; i++)
                    d3dDevice.SetRenderTarget(i, ((CRenderTargetView)OutputMerger.RenderTargets[i]).D3DSurface);
                for (int i = OutputMerger.RenderTargets.CurrentCount; i < OutputMerger.RenderTargets.MaxCount; i++)
                    d3dDevice.SetRenderTarget(i, null);
                OutputMerger.RenderTargets.Clean();
            }

            if (OutputMerger.DepthStencil.IsDirty)
            {
                d3dDevice.DepthStencilSurface = OutputMerger.DepthStencil.Value.NullOrFunc(v => ((CDepthStencilView)v).D3DSurface);
                OutputMerger.DepthStencil.Clean();
            }

            if (OutputMerger.BlendState.IsDirty)
            {
                ((CBlendState)OutputMerger.BlendState.Value ?? defaultBlendState).Apply();
                OutputMerger.BlendState.Clean();
            }

            if (OutputMerger.BlendFactor.IsDirty)
            {
                d3dDevice.SetRenderState(RenderState.BlendFactor, OutputMerger.BlendFactor.Value.ToArgb());
                OutputMerger.BlendFactor.Clean();
            }

            if (OutputMerger.SampleMask.IsDirty)
            {
                if (OutputMerger.SampleMask.Value != uint.MaxValue)
                    throw new NotSupportedException("SampleMasks other than 0xffffffff are not supported by D3D9");
                OutputMerger.SampleMask.Clean();
            }

            if (OutputMerger.DepthStencilState.IsDirty)
            {
                ((CDepthStencilState)OutputMerger.DepthStencilState.Value ?? defaultDepthStencilState)
                    .Apply((Rasterizer.State.Value ?? defaultRasterizerState).FrontFaceWinding == Winding.CounterClockwise); 
                OutputMerger.DepthStencilState.Clean();
            }
        }

        public override void Draw(int vertexCount, int startVertexLocation)
        {
            PreDraw();
            var d3dPrimitiveType = CtSharpDX9.PrimitiveType(InputAssembler.PrimitiveTopology.Value);
            d3dDevice.DrawPrimitives(d3dPrimitiveType, startVertexLocation, CtSharpDX9.PrimitiveCount(d3dPrimitiveType, vertexCount));
        }

        public override void DrawAuto()
        {
            throw new NotSupportedException("DrawAuto is not supported by D3D9");
        }

        public override void DrawIndexed(int indexCount, int startIndexLocation, int baseVertexLocation)
        {
            PreDraw();
            var d3dPrimitiveType = CtSharpDX9.PrimitiveType(InputAssembler.PrimitiveTopology.Value);
            var cIndexBuffer = ((CBufferIndex)InputAssembler.IndexSource.Value.Buffer);
            int minVertexIndex = cIndexBuffer.GetMinVrtexIndex();
            int totalVertexCount = cIndexBuffer.GetTotalVertexCount();
            d3dDevice.DrawIndexedPrimitive(d3dPrimitiveType, baseVertexLocation, minVertexIndex, totalVertexCount, startIndexLocation, CtSharpDX9.PrimitiveCount(d3dPrimitiveType, indexCount));
        }

        public override void DrawIndexedInstanced(int indexCountPerInstance, int instanceCount, int startIndexLocation, int baseVertexLocation, int startInstanceLocation)
        {
            throw new NotImplementedException("DrawIndexedInstanced is not yet implemented by the D3D9 implementaion of Beholder");
        }

        public override void DrawIndexedInstancedIndirect(IBuffer bufferForArgs, int alignedByteOffsetForArgs)
        {
            throw new NotSupportedException("DrawIndexedInstancedIndirect is not supported by D3D9");
        }

        public override void DrawInstanced(int vertexCountPerInstance, int instanceCount, int startVertexLocation, int startInstanceLocation)
        {
            throw new NotImplementedException("DrawInstanced is not yet implemented by the D3D9 implementaion of Beholder");
        }

        public override void DrawInstancedIndirect(IBuffer bufferForArgs, int alignedByteOffsetForArgs)
        {
            throw new NotSupportedException("DrawInstancedIndirect is not supported by D3D9");
        }

        public override void GenerateMips(IShaderResourceView shaderResourceView)
        {
        }

        public override void SetSubresourceData(IResource resource, int subresourceIndex, SubresourceData data)
        {
            ((ICResource)resource).SetSubresourceData(subresourceIndex, data);
        }
    }
}
