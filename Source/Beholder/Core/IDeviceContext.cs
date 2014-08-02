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

using Beholder.Math;
using Beholder.Resources;
using Beholder.Shaders;

namespace Beholder.Core
{
    /// <summary>
    /// Represents a device context which generates rendering commands.
    /// </summary>
    public interface IDeviceContext : IDeviceChild
    {
        DeviceContextType ContextType { get; }

        IComputeShader ShaderForDispatching { get; set; }
        IDeviceContextComputeShaderStage ComputeStage { get; }

        IShaderCombination ShadersForDrawing { get; set; }
        IDeviceContextInputAssembler InputAssembler { get; }
        IDeviceContextShaderStage VertexStage { get; }
        IDeviceContextShaderStage HullStage { get; }
        IDeviceContextShaderStage DomainStage { get; }
        IDeviceContextShaderStage GeometryStage { get; }
        //IDeviceContextStreamOutput StreamOutput { get; }
        IDeviceContextRasterizer Rasterizer { get; }
        IDeviceContextShaderStage PixelStage { get; }
        IDeviceContextOutputMerger OutputMerger { get; }

        void ClearRenderTargetView(IRenderTargetView renderTargetView, Color4 color);
        void ClearDepthStencilView(IDepthStencilView depthStencilView, ClearDepthStencilFlags flags, float depth, byte stencil);
        void ClearUnorderedAccessView(IUnorderedAccessView unorderedAccessView, Vector4 value);
        void ClearUnorderedAccessView(IUnorderedAccessView unorderedAccessView, IntVector4 value);

        void CopyResource(IResource dstResource, IResource srcResource);
        void CopySubresourceRegion(IResource dstResource, int dstSubresource, int dstX, int dstY, int dstZ, IResource srcResource, int srcSubresource, Box? srcBox);
        void GenerateMips(IShaderResourceView shaderResourceView);
        MappedSubresource Map(IResource resource, int subresource, MapType mapType, MapFlags mapFlags);
        void SetSubresourceData(IResource resource, int subresourceIndex, SubresourceData data);
        void Unmap(IResource resource, int subresource);

        void ConsumeDispatchPipeline();
        void Dispatch(int threadGroupCountX, int threadGroupCountY, int threadGroupCountZ);
        void DispatchIndirect(IBuffer bufferForArgs, int alignedByteOffsetForArgs);

        void ConsumeDrawPipeline();
        void Draw(int vertexCount, int startVertexLocation);
        void DrawAuto();
        void DrawIndexed(int indexCount, int startIndexLocation, int baseVertexLocation);
        void DrawIndexedInstanced(int indexCountPerInstance, int instanceCount, int startIndexLocation, int baseVertexLocation, int startInstanceLocation);
        void DrawIndexedInstancedIndirect(IBuffer bufferForArgs, int alignedByteOffsetForArgs);
        void DrawInstanced(int vertexCountPerInstance, int instanceCount, int startVertexLocation, int startInstanceLocation);
        void DrawInstancedIndirect(IBuffer bufferForArgs, int alignedByteOffsetForArgs);
    }
}
