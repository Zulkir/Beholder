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
using System.Collections.Generic;
using Beholder.Core;
using Beholder.Libraries.ObjectGL4.Core;
using Beholder.Libraries.ObjectGL4.Resources;
using Beholder.Libraries.ObjectGL4.Shaders;
using Beholder.Resources;
using Beholder.Shaders;
using Beholder.Shaders.Reflection;
using Beholder.Utility.Collections;
using Beholder.Utility.ForImplementations;
using Beholder.Utility.ForImplementations.Resources;

namespace Beholder.Libraries.ObjectGL4
{
    public class CDeviceChildCreator : DeviceChildBase<ICDevice>, IDeviceChildCreator
    {
        readonly ConcurrentHashSet<CBlendState> blendStates = new ConcurrentHashSet<CBlendState>();
        readonly ConcurrentHashSet<CDepthStencilState> depthStencilStates = new ConcurrentHashSet<CDepthStencilState>();
        readonly ConcurrentHashSet<CRasterizerState> rasterizerStates = new ConcurrentHashSet<CRasterizerState>();
        readonly ConcurrentHashSet<CSamplerState> samplerStates = new ConcurrentHashSet<CSamplerState>();

        readonly ConcurrentHashSet<CBuffer> buffers = new ConcurrentHashSet<CBuffer>();
        readonly ConcurrentHashSet<CTexture1D> textures1D = new ConcurrentHashSet<CTexture1D>();
        readonly ConcurrentHashSet<CTexture1DArray> textures1DArray = new ConcurrentHashSet<CTexture1DArray>();
        readonly ConcurrentHashSet<CTexture1DRenderbuffer> textures1DRenderbuffer = new ConcurrentHashSet<CTexture1DRenderbuffer>();
        readonly ConcurrentHashSet<CTexture2D> textures2D = new ConcurrentHashSet<CTexture2D>();
        readonly ConcurrentHashSet<CTexture2DArray> textures2DArray = new ConcurrentHashSet<CTexture2DArray>();
        readonly ConcurrentHashSet<CTexture2DRenderbuffer> textures2DRenderbuffer = new ConcurrentHashSet<CTexture2DRenderbuffer>();
        readonly ConcurrentHashSet<CTexture2DMultisample> textures2DMultisample = new ConcurrentHashSet<CTexture2DMultisample>();
        readonly ConcurrentHashSet<CTexture2DMultisampleArray> textures2DMultisampleArray = new ConcurrentHashSet<CTexture2DMultisampleArray>();
        readonly ConcurrentHashSet<CTexture2DCubemap> textures2DCubemap = new ConcurrentHashSet<CTexture2DCubemap>();
        readonly ConcurrentHashSet<CTexture2DCubemapArray> textures2DCubemapArray = new ConcurrentHashSet<CTexture2DCubemapArray>();
        readonly ConcurrentHashSet<CTexture3D> textures3D = new ConcurrentHashSet<CTexture3D>(); 

        readonly ConcurrentHashSet<CVertexShader> vertexShaders = new ConcurrentHashSet<CVertexShader>();
        readonly ConcurrentHashSet<CHullShader> hullShaders = new ConcurrentHashSet<CHullShader>();
        readonly ConcurrentHashSet<CDomainShader> domainShaders = new ConcurrentHashSet<CDomainShader>();
        readonly ConcurrentHashSet<CGeometryShader> geometryShaders = new ConcurrentHashSet<CGeometryShader>();
        readonly ConcurrentHashSet<CPixelShader> pixelShaders = new ConcurrentHashSet<CPixelShader>();
        readonly ConcurrentHashSet<CComputeShader> computeShaders = new ConcurrentHashSet<CComputeShader>();

        readonly IEnumerable<IDisposableInternal>[] disposableResources;

        public CDeviceChildCreator(ICDevice device) : base(device)
        {
            disposableResources = new IEnumerable<IDisposableInternal>[]
            {
                buffers,
                textures1D,
                textures1DArray,
                textures1DRenderbuffer,
                textures2D,
                textures2DArray,
                textures2DRenderbuffer,
                textures2DMultisample,
                textures2DMultisampleArray,
                textures2DCubemap,
                textures2DCubemapArray,
                textures3D,

                vertexShaders,
                hullShaders,
                domainShaders,
                geometryShaders,
                pixelShaders,
                computeShaders
            };
        }

        public void Dispose()
        {
            foreach (var container in disposableResources)
                foreach (var resource in container)
                    resource.DisposeInternal();
        }

        #region States
        public IBlendState BlendState(BlendDescription description)
        {
            return CreatorHelper.StoreAndReturn(new CBlendState(device, ref description), blendStates);
        }

        public IDepthStencilState DepthStencilState(DepthStencilDescription description)
        {
            return CreatorHelper.StoreAndReturn(new CDepthStencilState(device, ref description), depthStencilStates);
        }

        public IRasterizerState RasterizerState(RasterizerDescription description)
        {
            return CreatorHelper.StoreAndReturn(new CRasterizerState(device, ref description), rasterizerStates);
        }

        public ISamplerState SamplerState(SamplerDescription description)
        {
            return CreatorHelper.StoreAndReturn(new CSamplerState(device, ref description), samplerStates);
        }
        #endregion

        #region Resources
        public IVertexLayout VertexLayout(IVertexShader vertexShader, VertexLayoutElement[] elements)
        {
            return new CVertexLayout(device, (CVertexShader)vertexShader, elements);
        }

        public IBuffer Buffer(BufferDescription description, SubresourceData? initialData = null)
        {
            SubresourceData data = initialData ?? new SubresourceData();
            return CreatorHelper.StoreAndReturn(new CBuffer(device, ref description, ref data, b => buffers.Remove(b)), buffers);
        }

        public ITexture1D Texture1D(Texture1DDescription description, SubresourceData[] initialData)
        {
            if ((description.BindFlags & (BindFlags.ShaderResource | BindFlags.UnorderedAccess)) == 0 &&
                description.ArraySize == 1 && description.MipLevels == 1 && description.Usage == Usage.Default && initialData == null)
                return CreatorHelper.StoreAndReturn(new CTexture1DRenderbuffer(device, ref description, t => textures1DRenderbuffer.Remove(t)), textures1DRenderbuffer);
            if (description.ArraySize == 1)
                return CreatorHelper.StoreAndReturn(new CTexture1D(device, ref description, initialData, t => textures1D.Remove(t)), textures1D);
            return CreatorHelper.StoreAndReturn(new CTexture1DArray(device, ref description, initialData, t => textures1DArray.Remove(t)), textures1DArray);
        }

        public ITexture2D Texture2D(Texture2DDescription description, SubresourceData[] initialData)
        {
            if ((description.BindFlags & (BindFlags.ShaderResource | BindFlags.UnorderedAccess)) == 0 &&
                description.ArraySize == 1 && description.MipLevels == 1 && description.Usage == Usage.Default && initialData == null)
                return CreatorHelper.StoreAndReturn(new CTexture2DRenderbuffer(device, ref description, t => textures2DRenderbuffer.Remove(t)), textures2DRenderbuffer);
            if (description.ArraySize == 1)
            {
                if (description.Sampling.Count == 1)
                    return CreatorHelper.StoreAndReturn(new CTexture2D(device, ref description, initialData, t => textures2D.Remove(t)), textures2D);
                if (initialData != null)
                    throw new NotSupportedException("Initial data is not supported for multisample textures by OpenGL");
                return CreatorHelper.StoreAndReturn(new CTexture2DMultisample(device, ref description, t => textures2DMultisample.Remove(t)), textures2DMultisample);
            }
            if (!description.MiscFlags.HasFlag(MiscFlags.TextureCube))
            {
                if (description.Sampling.Count == 1)
                    return CreatorHelper.StoreAndReturn(new CTexture2DArray(device, ref description, initialData, t => textures2DArray.Remove(t)), textures2DArray);
                if (initialData != null)
                    throw new NotSupportedException("Initial data is not supported for multisample textures by OpenGL");
                return CreatorHelper.StoreAndReturn(new CTexture2DMultisampleArray(device, ref description, t => textures2DMultisampleArray.Remove(t)), textures2DMultisampleArray);
            }
            if (description.Sampling.Count > 1)
                throw new NotSupportedException("Multisample cube textures are not supported by OpenGL");
            if (description.ArraySize == 6)
                return CreatorHelper.StoreAndReturn(new CTexture2DCubemap(device, ref description, initialData, t => textures2DCubemap.Remove(t)), textures2DCubemap);
            return CreatorHelper.StoreAndReturn(new CTexture2DCubemapArray(device, ref description, initialData, t => textures2DCubemapArray.Remove(t)), textures2DCubemapArray);
        }

        public ITexture3D Texture3D(Texture3DDescription description, SubresourceData[] initialData)
        {
            return CreatorHelper.StoreAndReturn(new CTexture3D(device, ref description, initialData, t => textures3D.Remove(t)), textures3D);
        }
        #endregion

        #region Shaders
        public IVertexShader VertexShader(IShaderReflection reflection)
        {
            var cReflection = new CShaderReflection(reflection);
            return CreatorHelper.StoreAndReturn(new CVertexShader(device, cReflection), vertexShaders);
        }

        public IHullShader HullShader(IShaderReflection reflection)
        {
            var cReflection = new CShaderReflection(reflection);
            return CreatorHelper.StoreAndReturn(new CHullShader(device, cReflection), hullShaders);
        }

        public IDomainShader DomainShader(IShaderReflection reflection)
        {
            var cReflection = new CShaderReflection(reflection);
            return CreatorHelper.StoreAndReturn(new CDomainShader(device, cReflection), domainShaders);
        }

        public IGeometryShader GeometryShader(IShaderReflection reflection)
        {
            var cReflection = new CShaderReflection(reflection);
            return CreatorHelper.StoreAndReturn(new CGeometryShader(device, cReflection), geometryShaders);
        }

        public IPixelShader PixelShader(IShaderReflection reflection)
        {
            var cReflection = new CShaderReflection(reflection);
            return CreatorHelper.StoreAndReturn(new CPixelShader(device, cReflection), pixelShaders);
        }

        public IComputeShader ComputeShader(IShaderReflection reflection)
        {
            var cReflection = new CShaderReflection(reflection);
            return CreatorHelper.StoreAndReturn(new CComputeShader(device, cReflection), computeShaders);
        }

        public IShaderCombination ShaderCombination(IVertexShader vertexShader, IHullShader hullShader,
            IDomainShader domainShader, IGeometryShader geometryShader, IPixelShader pixelShader)
        {
            return new CShaderCombination(device, (CVertexShader)vertexShader, (CHullShader)hullShader,
                (CDomainShader)domainShader, (CGeometryShader)geometryShader, (CPixelShader)pixelShader);
        }
        #endregion

        #region Atoms
        /*
        public IDrawAtomBasic DrawAtomBasic(PrimitiveTopology primitiveTopology, IVertexSource vertices,
            int vertexCount, int startVertexLocation,
            IShaderCombination shaders, params IUniform[] uniforms)
        {
            var subset = new DrawSubsetBasic(vertexCount, startVertexLocation);
            return new CDrawAtomBasic(device, (CVertexSource)vertices, primitiveTopology, ref subset, (CShaderCombination)shaders, uniforms);
        }

        public IDrawAtomAuto DrawAtomAuto(PrimitiveTopology primitiveTopology, IVertexSource vertices, IShaderCombination shaders, params IUniform[] uniforms)
        {
            return new CDrawAtomAuto(device, (CVertexSource)vertices, primitiveTopology, (CShaderCombination)shaders, uniforms);
        }

        public IDrawAtomIndexed DrawAtomIndexed(PrimitiveTopology primitiveTopology, IVertexSource vertices, IBuffer indices,
            int indexCount, int startIndexLocation, int baseVertexLocation,
            IShaderCombination shaders, params IUniform[] uniforms)
        {
            var subset = new DrawSubsetIndexed(indexCount, startIndexLocation, baseVertexLocation);
            return new CDrawAtomIndexed(device, (CVertexSource)vertices, (CBuffer)indices, primitiveTopology, ref subset, (CShaderCombination)shaders, uniforms);
        }

        public IDrawAtomIndexedInstanced DrawAtomIndexedInstanced(PrimitiveTopology primitiveTopology, IVertexSource vertices, IBuffer indices,
            int indexCountPerInstance, int instanceCount, int startIndexLocation, int baseVertexLocation, int startInstanceLocation,
            IShaderCombination shaders, params IUniform[] uniforms)
        {
            var subset = new DrawSubsetIndexedInstanced(indexCountPerInstance, instanceCount, startIndexLocation, baseVertexLocation, startInstanceLocation);
            return new CDrawAtomIndexedInstanced(device, (CVertexSource)vertices, (CBuffer)indices, primitiveTopology, ref subset, (CShaderCombination)shaders, uniforms);
        }

        public IDrawAtomIndexedInstancedIndirect DrawAtomIndexedInstancedIndirect(PrimitiveTopology primitiveTopology, IVertexSource vertices, IBuffer indices, IBuffer bufferForArgs, int alignedByteOffsetForArgs, IShaderCombination shaders, params IUniform[] uniforms)
        {
            throw new NotSupportedException("DrawIndexedInstancedIndirect method is not supported by OpenGL");
        }

        public IDrawAtomInstanced DrawAtomInstanced(PrimitiveTopology primitiveTopology, IVertexSource vertices,
            int vertexCountPerInstance, int instanceCount, int startVertexLocation, int startInstanceLocation,
            IShaderCombination shaders, params IUniform[] uniforms)
        {
            var subset = new DrawSubsetInstanced(vertexCountPerInstance, instanceCount, startVertexLocation, startInstanceLocation);
            return new CDrawAtomInstanced(device, (CVertexSource)vertices, primitiveTopology, ref subset, (CShaderCombination)shaders, uniforms);
        }

        public IDrawAtomInstancedIndirect DrawAtomInstancedIndirect(PrimitiveTopology primitiveTopology, IVertexSource vertices, IBuffer bufferForArgs, int alignedByteOffsetForArgs, IShaderCombination shaders, params IUniform[] uniforms)
        {
            throw new NotSupportedException("DrawInstancedIndirect method is not supported by OpenGL");
        }

        public IDispatchAtomBasic DispatchAtomBasic(IComputeShader shader, IntVector3 threadGroupsCount, params IUniform[] uniforms)
        {
            throw new NotImplementedException();
            //return new CDispatchAtomBasic(device, (CComputeShader)shader, ref threadGroupsCount, uniforms);
        }

        public IDispatchAtomIndirect DispatchAtomIndirect(IComputeShader shader, IBuffer bufferForArgs, int alignedByteOffsetForArgs, params IUniform[] uniforms)
        {
            throw new NotImplementedException();
            //return new CDispatchAtomIndirect(device, (CComputeShader)shader, (CBuffer)bufferForArgs, alignedByteOffsetForArgs, uniforms);
        }*/
        #endregion
    }
}
