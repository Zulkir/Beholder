﻿/*
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
using System.Collections.Generic;
using Beholder.Core;
using Beholder.Libraries.SharpDX11.Resources;
using Beholder.Libraries.SharpDX11.Shaders;
using Beholder.Math;
using Beholder.Resources;
using Beholder.Utility.ForImplementations.Core;
using Beholder.Utility.Helpers;
using SharpDX;
using SharpDX.Direct3D11;
using Beholder.Utility.ForImplementations.Resources;
using Color4 = Beholder.Math.Color4;
using DeviceContextType = Beholder.Core.DeviceContextType;
using Rectangle = SharpDX.Rectangle;
using Vector4 = Beholder.Math.Vector4;
using Viewport = SharpDX.Direct3D11.Viewport;

namespace Beholder.Libraries.SharpDX11.Core
{
    public class CDeviceContext : DeviceContextBase<ICDevice>
    {
        readonly DeviceContext d3dDeviceContext;
        readonly DeviceContextType deviceContextType;

        readonly CRasterizerState defaultRasterizerState;
        readonly CDepthStencilState defaultDepthStencilState;
        readonly CBlendState defaultBlendState;
        readonly CSamplerState defaultSamplerState;

        readonly Dictionary<int, Rectangle[]> scissorRectangleArrays;
        readonly Dictionary<int, Viewport[]> viewportArrays;
        readonly Dictionary<int, StreamOutputBufferBinding[]> streamOutputBufferArrays;
        readonly Dictionary<int, RenderTargetView[]> renderTargetArrays;
        readonly Dictionary<int, UnorderedAccessView[]> unorderedAccessViewArrays;
        readonly Dictionary<int, int[]> initialCountArrays; 

        public CDeviceContext(ICDevice device, DeviceContext d3dDeviceContext) : base(device)
        {
            this.d3dDeviceContext = d3dDeviceContext;
            deviceContextType = CtBeholder.DeviceContextType(d3dDeviceContext.TypeInfo);

            defaultRasterizerState = (CRasterizerState)device.Create.RasterizerState(RasterizerDescription.Default);
            defaultDepthStencilState = (CDepthStencilState)device.Create.DepthStencilState(DepthStencilDescription.Default);
            defaultBlendState = (CBlendState)device.Create.BlendState(BlendDescription.Default);
            defaultSamplerState = (CSamplerState)device.Create.SamplerState(SamplerDescription.Default);

            scissorRectangleArrays = new Dictionary<int, Rectangle[]>();
            viewportArrays = new Dictionary<int, Viewport[]>();
            streamOutputBufferArrays = new Dictionary<int, StreamOutputBufferBinding[]>();
            renderTargetArrays = new Dictionary<int, RenderTargetView[]>();
            unorderedAccessViewArrays = new Dictionary<int, UnorderedAccessView[]>();
            initialCountArrays = new Dictionary<int, int[]>();
        }

        public void Dispose()
        {
            d3dDeviceContext.Dispose();    
        }

        public override DeviceContextType ContextType { get { return deviceContextType; } }

        public unsafe override void ClearRenderTargetView(IRenderTargetView renderTargetView, Color4 color)
        {
            d3dDeviceContext.ClearRenderTargetView(((CRenderTargetView)renderTargetView).D3DRenderTargetView, *(SharpDX.Color4*)&color);
        }

        public override void ClearDepthStencilView(IDepthStencilView depthStencilView, ClearDepthStencilFlags flags, float depth, byte stencil)
        {
            d3dDeviceContext.ClearDepthStencilView(((CDepthStencilView)depthStencilView).D3DDepthStencilView, CtSharpDX11.DepthStencilClearFlags(flags), depth, stencil);
        }

        public unsafe override void ClearUnorderedAccessView(IUnorderedAccessView unorderedAccessView, Vector4 value)
        {
            d3dDeviceContext.ClearUnorderedAccessView(((CUnorderedAccessView)unorderedAccessView).D3DUnorderedAccessView, *(SharpDX.Vector4*)&value);
        }

        public unsafe override void ClearUnorderedAccessView(IUnorderedAccessView unorderedAccessView, IntVector4 value)
        {
            d3dDeviceContext.ClearUnorderedAccessView(((CUnorderedAccessView)unorderedAccessView).D3DUnorderedAccessView, *(Int4*)&value);
        }

        void PreDispatch()
        {
            if (ShaderForDispatching.IsDirty)
            {
                d3dDeviceContext.ComputeShader.Set(((CComputeShader)ShaderForDispatching.Value).D3DComputeShader);
                ShaderForDispatching.Clean();
            }

            ConsumeShaderStage(d3dDeviceContext.ComputeShader, ComputeStage);
            if (ComputeStage.UnorderedAccessResources.IsDirty || ComputeStage.InitialCountChangedIndices.Count != 0)
            {
                foreach (var indexViewCount in ComputeStage.GetChangedIndexViewCountTuples())
                {
                    d3dDeviceContext.ComputeShader.SetUnorderedAccessView(
                        indexViewCount.First, 
                        GeneralHelper.NullOrChild(indexViewCount.Second, v => ((CUnorderedAccessView)v).D3DUnorderedAccessView), 
                        indexViewCount.Third);
                }
                ComputeStage.UnorderedAccessResources.Clean();
                ComputeStage.InitialCountChangedIndices.Clear();
            }
        }

        public override void Dispatch(int threadGroupCountX, int threadGroupCountY, int threadGroupCountZ)
        {
            PreDispatch();
            d3dDeviceContext.Dispatch(threadGroupCountX, threadGroupCountY, threadGroupCountZ);
        }

        public override void DispatchIndirect(IBuffer bufferForArgs, int alignedByteOffsetForArgs)
        {
            PreDispatch();
            d3dDeviceContext.DispatchIndirect(((CBuffer)bufferForArgs).D3DBuffer, alignedByteOffsetForArgs);
        }

        void PreDraw()
        {
            ConsumeInputAssembler();
            ConsumeStreamOutput();
            ConsumeRasterizer();
            ConsumeOutputMerger();
            ConsumeShadersForDrawing();
        }

        void ConsumeShadersForDrawing()
        {
            if (ShadersForDrawing.IsDirty)
            {
                var shaders = ((CShaderCombination)ShadersForDrawing.Value);
                d3dDeviceContext.VertexShader.Set(shaders.D3DVertexShader);
                d3dDeviceContext.HullShader.Set(shaders.D3DHullShader);
                d3dDeviceContext.DomainShader.Set(shaders.D3DDomainShader);
                d3dDeviceContext.GeometryShader.Set(shaders.D3DGeometryShader);
                d3dDeviceContext.PixelShader.Set(shaders.D3DPixelShader);
                ShadersForDrawing.Clean();
            }

            ConsumeShaderStage(d3dDeviceContext.VertexShader, VertexStage);
            ConsumeShaderStage(d3dDeviceContext.HullShader, HullStage);
            ConsumeShaderStage(d3dDeviceContext.DomainShader, DomainStage);
            ConsumeShaderStage(d3dDeviceContext.GeometryShader, GeometryStage);
            ConsumeShaderStage(d3dDeviceContext.PixelShader, PixelStage);
        }

        void ConsumeShaderStage(CommonShaderStage d3dStage, DeviceContextBaseShaderStage bStage)
        {
            foreach (var dirtyIndex in bStage.UniformBuffers.DirtyIndices)
                d3dStage.SetConstantBuffer(dirtyIndex, GeneralHelper.NullOrChild(bStage.UniformBuffers[dirtyIndex], b => ((CBuffer)b).D3DBuffer));
            bStage.UniformBuffers.Clean();

            foreach (var dirtyIndex in bStage.Samplers.DirtyIndices)
                d3dStage.SetSampler(dirtyIndex, ((CSamplerState)bStage.Samplers[dirtyIndex] ?? defaultSamplerState).D3DSamplerState);
            bStage.Samplers.Clean();

            foreach (var dirtyIndex in bStage.ShaderResources.DirtyIndices)
                d3dStage.SetShaderResource(dirtyIndex, GeneralHelper.NullOrChild(bStage.ShaderResources[dirtyIndex], r => ((CShaderResourceView)r).D3DShaderResourceView));
            bStage.ShaderResources.Clean();
        }

        void ConsumeInputAssembler()
        {
            foreach (var dirtyIndex in InputAssembler.VertexSources.DirtyIndices)
            {
                var vertexSource = InputAssembler.VertexSources[dirtyIndex];
                d3dDeviceContext.InputAssembler.SetVertexBuffers(dirtyIndex, 
                    new VertexBufferBinding(GeneralHelper.NullOrChild(vertexSource.Buffer, b => ((CBuffer)b).D3DBuffer), 
                        vertexSource.Stride, vertexSource.Offset));
            }
            InputAssembler.VertexSources.Clean();

            if (InputAssembler.PrimitiveTopology.IsDirty)
            {
                d3dDeviceContext.InputAssembler.PrimitiveTopology = CtSharpDX11.PrimitiveTopology(InputAssembler.PrimitiveTopology.Value);
                InputAssembler.PrimitiveTopology.Clean();
            }

            if (InputAssembler.VertexLayout.IsDirty)
            {
                d3dDeviceContext.InputAssembler.InputLayout = ((CVertexLayout)InputAssembler.VertexLayout.Value).D3DInputLayout;
                InputAssembler.VertexLayout.Clean();
            }

            if (InputAssembler.IndexSource.IsDirty)
            {
                var indexSource = InputAssembler.IndexSource.Value;
                d3dDeviceContext.InputAssembler.SetIndexBuffer(
                    GeneralHelper.NullOrChild(indexSource.Buffer, b => ((CBuffer)b).D3DBuffer), 
                    CtSharpDX11.Format(indexSource.Format), indexSource.Offset);
                InputAssembler.IndexSource.Clean();
            }
        }

        void ConsumeStreamOutput()
        {
            if (StreamOutput.Targets.IsDirty)
            {
                var streamOutputBufferArray = streamOutputBufferArrays.GetOrAdd(StreamOutput.Targets.CurrentCount, c => new StreamOutputBufferBinding[c]);
                for (int i = 0; i < streamOutputBufferArray.Length; i++)
                {
                    var bTarget = StreamOutput.Targets[i];
                    streamOutputBufferArray[i] = new StreamOutputBufferBinding(((CBuffer)bTarget.Buffer).D3DBuffer, bTarget.Offset); // todo: null check
                }
                d3dDeviceContext.StreamOutput.SetTargets(streamOutputBufferArray);
                StreamOutput.Targets.Clean();
            }
        }

        void ConsumeRasterizer()
        {
            if (Rasterizer.State.IsDirty)
            {
                d3dDeviceContext.Rasterizer.State = ((CRasterizerState)Rasterizer.State.Value ?? defaultRasterizerState).D3DRasterizerState;
                Rasterizer.State.Clean();
            }

            if (Rasterizer.ScissorRectangles.IsDirty)
            {
                var scissorRectanglesArray = scissorRectangleArrays.GetOrAdd(Rasterizer.ScissorRectangles.CurrentCount, c => new Rectangle[c]);
                for (int i = 0; i < scissorRectanglesArray.Length; i++)
                {
                    var bRectangle = Rasterizer.ScissorRectangles[i];
                    scissorRectanglesArray[i] = new Rectangle(bRectangle.X, bRectangle.Y, bRectangle.X + bRectangle.Width, bRectangle.Y + bRectangle.Height);
                }
                d3dDeviceContext.Rasterizer.SetScissorRectangles(scissorRectanglesArray);
                Rasterizer.ScissorRectangles.Clean();
            }

            if (Rasterizer.Viewports.IsDirty)
            {
                var viewportsArray = viewportArrays.GetOrAdd(Rasterizer.Viewports.CurrentCount, c => new Viewport[c]);
                for (int i = 0; i < viewportsArray.Length; i++)
                {
                    var bViewport = Rasterizer.Viewports[i];
                    viewportsArray[i] = new Viewport(bViewport.Left, bViewport.Top, bViewport.Width, bViewport.Height, bViewport.MinDepth, bViewport.MaxDepth);
                }
                d3dDeviceContext.Rasterizer.SetViewports(viewportsArray);
                Rasterizer.Viewports.Clean();
            }
        }

        unsafe void ConsumeOutputMerger()
        {
            if (OutputMerger.RenderTargets.IsDirty || OutputMerger.DepthStencil.IsDirty || OutputMerger.UnorderedAccessResourcesOffset.IsDirty ||
                OutputMerger.UnorderedAccessResources.IsDirty || OutputMerger.InitialCountsChangedIndices.Count != 0)
            {
                var renderTargetArray = renderTargetArrays.GetOrAdd(OutputMerger.RenderTargets.CurrentCount, c => new RenderTargetView[c]);
                var depthStencil = GeneralHelper.NullOrChild(OutputMerger.DepthStencil.Value, v => ((CDepthStencilView)v).D3DDepthStencilView);
                var unorderedResourceArray = unorderedAccessViewArrays.GetOrAdd(OutputMerger.UnorderedAccessResources.CurrentCount, c => new UnorderedAccessView[c]);
                var initialCountArray = initialCountArrays.GetOrAdd(OutputMerger.UnorderedAccessResources.CurrentCount, c => new int[c]);

                for (int i = 0; i < renderTargetArray.Length; i++)
                    renderTargetArray[i] = ((CRenderTargetView)OutputMerger.RenderTargets[i]).D3DRenderTargetView;
                for (int i = 0; i < unorderedResourceArray.Length; i++)
                {
                    unorderedResourceArray[i] = ((CUnorderedAccessView)OutputMerger.UnorderedAccessResources[i]).D3DUnorderedAccessView;
                    initialCountArray[i] = -1;
                }
                foreach (int dirtyIndex in OutputMerger.InitialCountsChangedIndices)
                    initialCountArray[dirtyIndex] = OutputMerger.InitialCounts[dirtyIndex];

                d3dDeviceContext.OutputMerger.SetTargets(depthStencil, OutputMerger.UnorderedAccessResourcesOffset.Value, unorderedResourceArray, initialCountArray, renderTargetArray);
                
                OutputMerger.RenderTargets.Clean();
                OutputMerger.DepthStencil.Clean();
                OutputMerger.UnorderedAccessResourcesOffset.Clean();
                OutputMerger.UnorderedAccessResources.Clean();
                OutputMerger.InitialCountsChangedIndices.Clear();
            }

            if (OutputMerger.BlendState.IsDirty || OutputMerger.BlendFactor.IsDirty || OutputMerger.SampleMask.IsDirty)
            {
                var blendFactor = OutputMerger.BlendFactor.Value;
                d3dDeviceContext.OutputMerger.SetBlendState(((CBlendState)OutputMerger.BlendState.Value ?? defaultBlendState).D3DBlendState, *(SharpDX.Color4*)&blendFactor, OutputMerger.SampleMask.Value);
                OutputMerger.BlendState.Clean();
                OutputMerger.BlendFactor.Clean();
                OutputMerger.SampleMask.Clean();
            }

            if (OutputMerger.DepthStencilState.IsDirty)
            {
                d3dDeviceContext.OutputMerger.SetDepthStencilState(((CDepthStencilState)OutputMerger.DepthStencilState.Value ?? defaultDepthStencilState).D3DDepthStencilState, OutputMerger.StencilReference.Value);
                OutputMerger.DepthStencilState.Clean();
            }
        }

        public override void Draw(int vertexCount, int startVertexLocation)
        {
            PreDraw();
            d3dDeviceContext.Draw(vertexCount, startVertexLocation);
        }

        public override void DrawAuto()
        {
            PreDraw();
            d3dDeviceContext.DrawAuto();
        }

        public override void DrawIndexed(int indexCount, int startIndexLocation, int baseVertexLocation)
        {
            PreDraw();
            d3dDeviceContext.DrawIndexed(indexCount, startIndexLocation, baseVertexLocation);
        }

        public override void DrawIndexedInstanced(int indexCountPerInstance, int instanceCount, int startIndexLocation, int baseVertexLocation, int startInstanceLocation)
        {
            PreDraw();
            d3dDeviceContext.DrawIndexedInstanced(indexCountPerInstance, instanceCount, startIndexLocation, baseVertexLocation, startInstanceLocation);
        }

        public override void DrawIndexedInstancedIndirect(IBuffer bufferForArgs, int alignedByteOffsetForArgs)
        {
            PreDraw();
            d3dDeviceContext.DrawIndexedInstancedIndirect(((CBuffer)bufferForArgs).D3DBuffer, alignedByteOffsetForArgs);
        }

        public override void DrawInstanced(int vertexCountPerInstance, int instanceCount, int startVertexLocation, int startInstanceLocation)
        {
            PreDraw();
            d3dDeviceContext.DrawInstanced(vertexCountPerInstance, instanceCount, startVertexLocation, startInstanceLocation);
        }

        public override void DrawInstancedIndirect(IBuffer bufferForArgs, int alignedByteOffsetForArgs)
        {
            PreDraw();
            d3dDeviceContext.DrawInstancedIndirect(((CBuffer)bufferForArgs).D3DBuffer, alignedByteOffsetForArgs);
        }

        public override void GenerateMips(IShaderResourceView shaderResourceView)
        {
            d3dDeviceContext.GenerateMips(((CShaderResourceView)shaderResourceView).D3DShaderResourceView);
        }

        public override void SetSubresourceData(IResource resource, int subresourceIndex, SubresourceData data)
        {
            ((ICResource)resource).SetSubresourceData(this, subresourceIndex, data);
        }

        internal void UpdateSubresource(Resource d3dResource, int subresourceIndex, ResourceRegion? subresourceRegion, IntPtr data, int rowPitch, int depthPitch)
        {
            d3dDeviceContext.UpdateSubresource(d3dResource, subresourceIndex, subresourceRegion, data, rowPitch, depthPitch);
        }

        internal DataBox MapSubresource(Resource d3dResource, int subresourceIndex, MapMode mapMode, MapFlags mapFlags)
        {
            return d3dDeviceContext.MapSubresource(d3dResource, subresourceIndex, mapMode, mapFlags);
        }

        internal void UnmapSubresource(Resource d3dResource, int subresourceIndex)
        {
            d3dDeviceContext.UnmapSubresource(d3dResource, subresourceIndex);
        }
    }
}