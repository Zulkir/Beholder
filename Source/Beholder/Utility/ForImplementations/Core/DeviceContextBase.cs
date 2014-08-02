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

using Beholder.Core;
using Beholder.Math;
using Beholder.Resources;
using Beholder.Shaders;
using Beholder.Utility.Patterns;

namespace Beholder.Utility.ForImplementations.Core
{
    public abstract class DeviceContextBase<TDevice> : DeviceChildBase<TDevice>, IDeviceContext where TDevice : IDevice
    {
        protected DeviceContextBase(TDevice device) : base(device)
        {
            var restrictions = device.Adapter.Restrictions;
            shaderForDispatching = new DirtyProperty<IComputeShader>();
            computeStage = new DeviceContextBaseComputeShaderStage(restrictions.UniformBufferSlots, restrictions.SamplerSlots, restrictions.ShaderResourceSlots, restrictions.UnorderedAccessResourceSlots);
            shadersForDrawing = new DirtyProperty<IShaderCombination>();
            inputAssembler = new DeviceContextBaseInputAssembler(restrictions.MaxVertexStreams);
            vertexStage = new DeviceContextBaseShaderStage(restrictions.UniformBufferSlots, restrictions.SamplerSlots, restrictions.ShaderResourceSlots);
            hullStage = new DeviceContextBaseShaderStage(restrictions.UniformBufferSlots, restrictions.SamplerSlots, restrictions.ShaderResourceSlots);
            domainStage = new DeviceContextBaseShaderStage(restrictions.UniformBufferSlots, restrictions.SamplerSlots, restrictions.ShaderResourceSlots);
            geometryStage = new DeviceContextBaseShaderStage(restrictions.UniformBufferSlots, restrictions.SamplerSlots, restrictions.ShaderResourceSlots);
            streamOutput = new DeviceContextBaseStreamOutput(restrictions.MaxStreamOutputTargets);
            rasterizer = new DeviceContextBaseRasterizer(restrictions.MaxViewports);
            pixelStage = new DeviceContextBaseShaderStage(restrictions.UniformBufferSlots, restrictions.SamplerSlots, restrictions.ShaderResourceSlots);
            outputMerger = new DeviceContextBaseOutputMerger(restrictions.MaxRenderTargets, restrictions.MaxRenderTargets); // todo: make rtvs and uavs competetive
        }

        public abstract DeviceContextType ContextType { get; }

        public abstract void ClearRenderTargetView(IRenderTargetView renderTargetView, Color4 color);
        public abstract void ClearDepthStencilView(IDepthStencilView depthStencilView, ClearDepthStencilFlags flags, float depth, byte stencil);
        public abstract void ClearUnorderedAccessView(IUnorderedAccessView unorderedAccessView, Vector4 value);
        public abstract void ClearUnorderedAccessView(IUnorderedAccessView unorderedAccessView, IntVector4 value);
        public abstract void CopyResource(IResource dstResource, IResource srcResource);
        public abstract void CopySubresourceRegion(IResource dstResource, int dstSubresource, int dstX, int dstY, int dstZ, IResource srcResource, int srcSubresource, Box? srcBox);
        public abstract void GenerateMips(IShaderResourceView shaderResourceView);
        public abstract MappedSubresource Map(IResource resource, int subresource, MapType mapType, MapFlags mapFlags);
        public abstract void SetSubresourceData(IResource resource, int subresourceIndex, SubresourceData data);
        public abstract void Unmap(IResource resource, int subresource);
        
        readonly DirtyProperty<IComputeShader> shaderForDispatching;
        readonly DeviceContextBaseComputeShaderStage computeStage;
        readonly DirtyProperty<IShaderCombination> shadersForDrawing;
        readonly DeviceContextBaseInputAssembler inputAssembler;
        readonly DeviceContextBaseShaderStage vertexStage;
        readonly DeviceContextBaseShaderStage hullStage;
        readonly DeviceContextBaseShaderStage domainStage;
        readonly DeviceContextBaseShaderStage geometryStage;
        readonly DeviceContextBaseStreamOutput streamOutput;
        readonly DeviceContextBaseRasterizer rasterizer;
        readonly DeviceContextBaseShaderStage pixelStage;
        readonly DeviceContextBaseOutputMerger outputMerger;

        protected DirtyProperty<IComputeShader> ShaderForDispatching { get { return shaderForDispatching; } }
        protected DeviceContextBaseComputeShaderStage ComputeStage { get { return computeStage; } }
        protected DirtyProperty<IShaderCombination> ShadersForDrawing { get { return shadersForDrawing; } } 
        protected DeviceContextBaseInputAssembler InputAssembler { get { return inputAssembler; } }
        protected DeviceContextBaseShaderStage VertexStage { get { return vertexStage; } }
        protected DeviceContextBaseShaderStage HullStage { get { return hullStage; } }
        protected DeviceContextBaseShaderStage DomainStage { get { return domainStage; } }
        protected DeviceContextBaseShaderStage GeometryStage { get { return geometryStage; } }
        protected DeviceContextBaseStreamOutput StreamOutput { get { return streamOutput; } }
        protected DeviceContextBaseRasterizer Rasterizer { get { return rasterizer; } }
        protected DeviceContextBaseShaderStage PixelStage { get { return pixelStage; } }
        protected DeviceContextBaseOutputMerger OutputMerger { get { return outputMerger; } }

        IComputeShader IDeviceContext.ShaderForDispatching { get { return shaderForDispatching.Value; } set { shaderForDispatching.Value = value; } }
        IDeviceContextComputeShaderStage IDeviceContext.ComputeStage { get { return computeStage; } }
        IShaderCombination IDeviceContext.ShadersForDrawing { get { return shadersForDrawing.Value; } set { shadersForDrawing.Value = value; } }
        IDeviceContextInputAssembler IDeviceContext.InputAssembler { get { return inputAssembler; } }
        IDeviceContextShaderStage IDeviceContext.VertexStage { get { return vertexStage; } }
        IDeviceContextShaderStage IDeviceContext.HullStage { get { return hullStage; } }
        IDeviceContextShaderStage IDeviceContext.DomainStage { get { return domainStage; } }
        IDeviceContextShaderStage IDeviceContext.GeometryStage { get { return geometryStage; } }
        //IDeviceContextStreamOutput IDeviceContext.StreamOutput { get { return streamOutput; } }
        IDeviceContextRasterizer IDeviceContext.Rasterizer { get { return rasterizer; } }
        IDeviceContextShaderStage IDeviceContext.PixelStage { get { return pixelStage; } }
        IDeviceContextOutputMerger IDeviceContext.OutputMerger { get { return outputMerger; } }

        public abstract void ConsumeDispatchPipeline();
        public abstract void Dispatch(int threadGroupCountX, int threadGroupCountY, int threadGroupCountZ);
        public abstract void DispatchIndirect(IBuffer bufferForArgs, int alignedByteOffsetForArgs);
        
        public abstract void ConsumeDrawPipeline();
        public abstract void Draw(int vertexCount, int startVertexLocation);
        public abstract void DrawAuto();
        public abstract void DrawIndexed(int indexCount, int startIndexLocation, int baseVertexLocation);
        public abstract void DrawIndexedInstanced(int indexCountPerInstance, int instanceCount, int startIndexLocation, int baseVertexLocation, int startInstanceLocation);
        public abstract void DrawIndexedInstancedIndirect(IBuffer bufferForArgs, int alignedByteOffsetForArgs);
        public abstract void DrawInstanced(int vertexCountPerInstance, int instanceCount, int startVertexLocation, int startInstanceLocation);
        public abstract void DrawInstancedIndirect(IBuffer bufferForArgs, int alignedByteOffsetForArgs);

        protected void MarkAsDirty()
        {
            shaderForDispatching.MarkAsDirty();
            computeStage.MarkAsDirty();
            shadersForDrawing.MarkAsDirty();
            inputAssembler.MarkAsDirty();
            vertexStage.MarkAsDirty();
            hullStage.MarkAsDirty();
            domainStage.MarkAsDirty();
            geometryStage.MarkAsDirty();
            streamOutput.MarkAsDirty();
            rasterizer.MarkAsDirty();
            pixelStage.MarkAsDirty();
            outputMerger.MarkAsDirty();
        }
    }
}
