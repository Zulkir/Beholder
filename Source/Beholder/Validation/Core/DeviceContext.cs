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
using Beholder.Math;
using Beholder.Resources;
using Beholder.Shaders;
using Beholder.Validation.Resources;

namespace Beholder.Validation.Core
{
    class DeviceContext : DeviceChild<IDeviceContext>, IDeviceContext
    {
        readonly DeviceContextComputeShaderStage computeStage;
        readonly DeviceContextInputAssembler inputAssembler;
        readonly DeviceContextRasterizer rasterizer;
        readonly DeviceContextOutputMerger outputMerger;
        readonly DeviceContextShaderStage vertexStage;
        readonly DeviceContextShaderStage hullStage;
        readonly DeviceContextShaderStage domainStage;
        readonly DeviceContextShaderStage geometryStage;
        readonly DeviceContextShaderStage pixelStage;

        public DeviceContext(IDeviceContext real) 
            : base(real)
        {
            computeStage = new DeviceContextComputeShaderStage(Real.ComputeStage);
            inputAssembler = new DeviceContextInputAssembler(Real.InputAssembler);
            rasterizer = new DeviceContextRasterizer(Real.Rasterizer);
            outputMerger = new DeviceContextOutputMerger(Real.OutputMerger);
            vertexStage = new DeviceContextShaderStage(Real.VertexStage);
            hullStage = new DeviceContextShaderStage(Real.HullStage);
            domainStage = new DeviceContextShaderStage(Real.DomainStage);
            geometryStage = new DeviceContextShaderStage(Real.GeometryStage);
            pixelStage = new DeviceContextShaderStage(Real.PixelStage);
        }

        #region Implementation of IDeviceContext

        public DeviceContextType ContextType { get { return Real.ContextType; } }

        public void ClearRenderTargetView(IRenderTargetView renderTargetView, Color4 color)
        {
            Check.ExistingInternal(renderTargetView, "renderTargetView");
            Real.ClearRenderTargetView(renderTargetView.GetReal(), color);
        }

        public void ClearDepthStencilView(IDepthStencilView depthStencilView, ClearDepthStencilFlags flags, float depth, byte stencil)
        {
            Check.ExistingInternal(depthStencilView, "depthStencilView");
            Check.Flags(flags, "flags");
            if (depth < 0.0f || depth > 1.0f)
                throw new ArgumentException("Depth must be in the range from 0.0 to 1.0");
            Real.ClearDepthStencilView(((DepthStencilView)depthStencilView).Real, flags, depth, stencil);
        }

        public void ClearUnorderedAccessView(IUnorderedAccessView unorderedAccessView, Vector4 value)
        {
            Check.ExistingInternal(unorderedAccessView, "unorderedAccessView");
            Real.ClearUnorderedAccessView(unorderedAccessView.GetReal(), value);
        }

        public void ClearUnorderedAccessView(IUnorderedAccessView unorderedAccessView, IntVector4 value)
        {
            Check.ExistingInternal(unorderedAccessView, "unorderedAccessView");
            Real.ClearUnorderedAccessView(unorderedAccessView.GetReal(), value);
        }

        public IComputeShader ShaderForDispatching
        {
            get { return Real.ShaderForDispatching; }
            set
            {
                Check.NullOrInternal(value, "value"); 
                Real.ShaderForDispatching = value.GetReal();
            }
        }

        public IDeviceContextComputeShaderStage ComputeStage
        {
            get { return computeStage; }
        }

        public void Dispatch(int threadGroupCountX, int threadGroupCountY, int threadGroupCountZ)
        {
            Check.Positive(threadGroupCountX, "threadGroupCountX");
            if (threadGroupCountX > Real.Device.Adapter.Restrictions.MaxThreadGroupsX)
                throw new ArgumentException(string.Format(
                    "threadGroupCountX exceeds the maximum value ({0}) supported by the adapter with this implementation of Beholder", 
                    Device.Adapter.Restrictions.MaxThreadGroupsX));
            
            Check.Positive(threadGroupCountY, "threadGroupCountY");
            if (threadGroupCountY > Real.Device.Adapter.Restrictions.MaxThreadGroupsY)
                throw new ArgumentException(string.Format(
                    "threadGroupCountY exceeds the maximum value ({0}) supported by the adapter with this implementation of Beholder",
                    Device.Adapter.Restrictions.MaxThreadGroupsY));

            Check.Positive(threadGroupCountZ, "threadGroupCountZ");
            if (threadGroupCountZ > Real.Device.Adapter.Restrictions.MaxThreadGroupsZ)
                throw new ArgumentException(string.Format(
                    "threadGroupCountZ exceeds the maximum value ({0}) supported by the adapter with this implementation of Beholder",
                    Device.Adapter.Restrictions.MaxThreadGroupsZ));

            if (threadGroupCountX * threadGroupCountY * threadGroupCountZ > Real.Device.Adapter.Restrictions.MaxThreadGroupsTotal)
                throw new ArgumentException(string.Format(
                    "Total number of thread groups exceeds the maximum value ({0}) supported by the adapter with this implementation of Beholder",
                    Device.Adapter.Restrictions.MaxThreadGroupsTotal));

            Real.Dispatch(threadGroupCountX, threadGroupCountY, threadGroupCountZ);
        }

        public void DispatchIndirect(IBuffer bufferForArgs, int alignedByteOffsetForArgs)
        {
            Check.ExistingInternal(bufferForArgs, "bufferForArgs");
            Check.NotNegative(alignedByteOffsetForArgs, "alignedByteOffsetForArgs");
            Real.DispatchIndirect(bufferForArgs.GetReal(), alignedByteOffsetForArgs);
        }

        public IShaderCombination ShadersForDrawing
        {
            get { return Wrappers.Get(Real.ShadersForDrawing); }
            set { Check.NullOrInternal(value, "value"); Real.ShadersForDrawing = value.GetReal(); }
        }

        public IDeviceContextInputAssembler InputAssembler
        {
            get { return inputAssembler; }
        }

        public IDeviceContextShaderStage VertexStage
        {
            get { return vertexStage; }
        }

        public IDeviceContextShaderStage HullStage
        {
            get { return hullStage; }
        }

        public IDeviceContextShaderStage DomainStage
        {
            get { return domainStage; }
        }

        public IDeviceContextShaderStage GeometryStage
        {
            get { return geometryStage; }
        }

        public IDeviceContextStreamOutput StreamOutput
        {
            get { throw new NotImplementedException(); }
        }

        public IDeviceContextRasterizer Rasterizer
        {
            get { return rasterizer; }
        }

        public IDeviceContextShaderStage PixelStage
        {
            get { return pixelStage; }
        }

        public IDeviceContextOutputMerger OutputMerger
        {
            get { return outputMerger; }
        }

        void ValidateDraw(bool indexed)
        {
            ValidateDrawInputAssembler(indexed);
            ValidateDrawShaderStage(ShadersForDrawing.VertexShader, VertexStage, "vertex");
            if (ShadersForDrawing.HullShader != null)
                ValidateDrawTesselation();
            if (ShadersForDrawing.GeometryShader != null)
                ValidateDrawShaderStage(ShadersForDrawing.GeometryShader, GeometryStage, "geometry");
            ValidateDrawRasterizer();
            if (ShadersForDrawing.PixelShader != null)
                ValidateDrawShaderStage(ShadersForDrawing.PixelShader, PixelStage, "pixel");
            ValidateDrawOutputMerger();
        }

        void ValidateDrawInputAssembler(bool indexed)
        {
            if (ShadersForDrawing == null)
                throw new InvalidOperationException("Draw Error: ShadersForDrawing is null");
            if (InputAssembler.PrimitiveTopology == PrimitiveTopology.Undefined)
                throw new InvalidOperationException("Draw Error: InputAssembler.PrimitiveTopology is Undefined");
            if (InputAssembler.VertexLayout == null)
                throw new InvalidOperationException("Draw Error: InputAssembler.VertexLayout is null");
            if (InputAssembler.VertexLayout.VertexShader != ShadersForDrawing.VertexShader)
                throw new InvalidOperationException("Draw Error: InputAssembler.VertexLayout.Shader does not match the ShadersForDrawing.VertexShader");
            for (int i = 0; i < InputAssembler.VertexLayout.Elements.Count; i++)
                if (InputAssembler.VertexSources[InputAssembler.VertexLayout.Elements[i].InputSlot].Buffer == null)
                    throw new InvalidOperationException(string.Format(
                        "Draw Error: InputAssembler.VertexLayout.Elements[i].InputSlot is {0}, but InputAssembler.VertexSources[{0}].Buffer is null", i));
            if (indexed && InputAssembler.IndexSource.Buffer == null)
                throw new InvalidOperationException("Draw Error: Draw***Indexed***() is called, but InputAssembler.IndexSource.Buffer is null");
        }

        void ValidateDrawTesselation()
        {
            if (InputAssembler.PrimitiveTopology < PrimitiveTopology.PatchList1 || InputAssembler.PrimitiveTopology > PrimitiveTopology.PatchList32)
                throw new InvalidOperationException("Draw Error: InputAssembler.PrimitiveTopology must be a PatchList when using tesselation");
            ValidateDrawShaderStage(ShadersForDrawing.HullShader, HullStage, "hull");
            ValidateDrawShaderStage(ShadersForDrawing.DomainShader, DomainStage, "domain");
        }

        void ValidateDrawShaderStage(IShader shader, IDeviceContextShaderStage stage, string stageName)
        {
            foreach (var variable in shader.Reflection.UniformBuffers)
                if (stage.UniformBuffers[variable.GetSlot()] == null)
                    throw new InvalidOperationException(string.Format(
                        "Draw Error: Current {0} shader uses a uniform buffer bound to slot {1}, but it is null", 
                        stageName, variable.GetSlot()));
            foreach (var variable in shader.Reflection.Samplers)
                if (stage.Samplers[variable.GetSlot()] == null)
                    throw new InvalidOperationException(string.Format(
                        "Draw Error: Current {0} shader uses a sampler bound to slot {1}, but it is null",
                        stageName, variable.GetSlot()));
            foreach (var variable in shader.Reflection.Srvs)
                if (stage.ShaderResources[variable.GetSlot()] == null)
                    throw new InvalidOperationException(string.Format(
                        "Draw Error: Current {0} shader uses a shader resource bound to slot {1}, but it is null",
                        stageName, variable.GetSlot()));
        }

        static void ValidateDrawRasterizer()
        {
            // nothing to validate
        }

        void ValidateDrawOutputMerger()
        {
            if (OutputMerger.RenderTargets.CurrentCount > 0 && OutputMerger.DepthStencil != null)
            {
                var rt = OutputMerger.RenderTargets[0];
                var ds = OutputMerger.DepthStencil;

                if (rt.Width != ds.Width || rt.Height != ds.Height)
                    throw new InvalidOperationException("Render targets and depth-tencil must have the same width and height");
            }
        }

        public void Draw(int vertexCount, int startVertexLocation)
        {
            ValidateDraw(false);
            Real.Draw(vertexCount, startVertexLocation);
        }

        public void DrawAuto()
        {
            ValidateDraw(false);
            Real.DrawAuto();
        }

        public void DrawIndexed(int indexCount, int startIndexLocation, int baseVertexLocation)
        {
            ValidateDraw(true);
            Real.DrawIndexed(indexCount, startIndexLocation, baseVertexLocation);
        }

        public void DrawIndexedInstanced(int indexCountPerInstance, int instanceCount, int startIndexLocation, int baseVertexLocation, int startInstanceLocation)
        {
            ValidateDraw(true);
            Real.DrawIndexedInstanced(indexCountPerInstance, instanceCount, startIndexLocation, baseVertexLocation, startIndexLocation);
        }

        public void DrawIndexedInstancedIndirect(IBuffer bufferForArgs, int alignedByteOffsetForArgs)
        {
            Check.ExistingInternal(bufferForArgs, "bufferForArgs");
            Check.NotNegative(alignedByteOffsetForArgs, "alignedByteOffsetForArgs");
            ValidateDraw(true);
            Real.DrawIndexedInstancedIndirect(bufferForArgs.GetReal(), alignedByteOffsetForArgs);
        }

        public void DrawInstanced(int vertexCountPerInstance, int instanceCount, int startVertexLocation, int startInstanceLocation)
        {
            ValidateDraw(false);
            Real.DrawInstanced(vertexCountPerInstance, instanceCount, startVertexLocation, startInstanceLocation);
        }

        public void DrawInstancedIndirect(IBuffer bufferForArgs, int alignedByteOffsetForArgs)
        {
            Check.ExistingInternal(bufferForArgs, "bufferForArgs");
            Check.NotNegative(alignedByteOffsetForArgs, "alignedByteOffsetForArgs");
            ValidateDraw(false);
            Real.DrawInstancedIndirect(bufferForArgs.GetReal(), alignedByteOffsetForArgs);
        }

        public void GenerateMips(IShaderResourceView shaderResourceView)
        {
            Check.ExistingInternal(shaderResourceView, "shaderResourceView");
            Real.GenerateMips(shaderResourceView.GetReal());
        }

        public void SetSubresourceData(IResource resource, int subresourceIndex, SubresourceData data)
        {
            Check.ExistingInternal(resource, "resource");
            switch (resource.Dimension)
            {
                case ResourceDimension.Buffer:
                    if (subresourceIndex < 0)
                        throw new ArgumentOutOfRangeException("subresourceIndex");
                    break;
                case ResourceDimension.Texture1D:
                    var tex1D = (ITexture1D)resource;
                    if (subresourceIndex >= tex1D.ArraySize * tex1D.MipLevels)
                        throw new ArgumentOutOfRangeException("subresourceIndex");
                    break;
                case ResourceDimension.Texture2D:
                    var tex2D = (ITexture2D)resource;
                    if (subresourceIndex >= tex2D.ArraySize * tex2D.MipLevels)
                        throw new ArgumentOutOfRangeException("subresourceIndex");
                    break;
                case ResourceDimension.Texture3D:
                    var tex3D = (ITexture3D)resource;
                    if (subresourceIndex >= tex3D.MipLevels)
                        throw new ArgumentOutOfRangeException("subresourceIndex");
                    break;
            }
            if (data.Pointer == IntPtr.Zero)
                throw new ArgumentException("data.Pointer cannot be 0");
            Real.SetSubresourceData(resource.GetReal(), subresourceIndex, data);
        }

        #endregion
    }
}
