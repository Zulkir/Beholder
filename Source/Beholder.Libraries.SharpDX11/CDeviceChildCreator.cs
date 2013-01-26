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
using System.Collections.Generic;
using Beholder.Core;
using Beholder.Libraries.SharpDX11.Core;
using Beholder.Libraries.SharpDX11.Resources;
using Beholder.Libraries.SharpDX11.Shaders;
using Beholder.Resources;
using Beholder.Shaders;
using Beholder.Shaders.Reflection;
using Beholder.Utility.Collections;
using Beholder.Utility.ForImplementations;
using Beholder.Utility.ForImplementations.Resources;
using BSamplerDescription = Beholder.Core.SamplerDescription;
using BBufferDescription = Beholder.Resources.BufferDescription;
using BTexture1DDescription = Beholder.Resources.Texture1DDescription;
using BTexture2DDescription = Beholder.Resources.Texture2DDescription;
using BTexture3DDescription = Beholder.Resources.Texture3DDescription;

namespace Beholder.Libraries.SharpDX11
{
    public class CDeviceChildCreator : DeviceChildBase<ICDevice>, IDeviceChildCreator
    {
        readonly ConcurrentHashSet<CBlendState> blendStates;
        readonly ConcurrentHashSet<CDepthStencilState> depthStencilStates;
        readonly ConcurrentHashSet<CRasterizerState> rasterizerStates;
        readonly ConcurrentHashSet<CSamplerState> samplerStates;

        readonly ConcurrentHashSet<CVertexLayout> vertexLayouts; 
        readonly ConcurrentHashSet<CBuffer> buffers;
        readonly ConcurrentHashSet<CTexture1D> textures1D;
        readonly ConcurrentHashSet<CTexture2D> textures2D;
        readonly ConcurrentHashSet<CTexture3D> textures3D;

        readonly ConcurrentHashSet<CVertexShader> vertexShaders;
        readonly ConcurrentHashSet<CPixelShader> pixelShaders;
        readonly ConcurrentHashSet<CGeometryShader> geometryShaders;
        readonly ConcurrentHashSet<CHullShader> hullShaders;
        readonly ConcurrentHashSet<CDomainShader> domainShaders;
        readonly ConcurrentHashSet<CComputeShader> computeShaders; 

        readonly IEnumerable<IDisposable>[] disposableResources;

        public CDeviceChildCreator(ICDevice device) : base(device)
        {
            blendStates = new ConcurrentHashSet<CBlendState>();
            depthStencilStates = new ConcurrentHashSet<CDepthStencilState>();
            rasterizerStates = new ConcurrentHashSet<CRasterizerState>();
            samplerStates = new ConcurrentHashSet<CSamplerState>();

            vertexLayouts = new ConcurrentHashSet<CVertexLayout>();
            buffers = new ConcurrentHashSet<CBuffer>();
            textures1D = new ConcurrentHashSet<CTexture1D>();
            textures2D = new ConcurrentHashSet<CTexture2D>();
            textures3D = new ConcurrentHashSet<CTexture3D>();

            vertexShaders = new ConcurrentHashSet<CVertexShader>();
            pixelShaders = new ConcurrentHashSet<CPixelShader>();
            geometryShaders = new ConcurrentHashSet<CGeometryShader>();
            hullShaders = new ConcurrentHashSet<CHullShader>();
            domainShaders = new ConcurrentHashSet<CDomainShader>();
            computeShaders = new ConcurrentHashSet<CComputeShader>();

            disposableResources = new IEnumerable<IDisposable>[]
            {
                buffers,
                textures1D,
                textures2D,
                textures3D,

                vertexLayouts,
                blendStates,
                depthStencilStates,
                rasterizerStates,
                samplerStates,

                vertexShaders,
                pixelShaders,
                geometryShaders,
                hullShaders,
                domainShaders,
                computeShaders
            };
        }

        public void Dispose()
        {
            foreach (var container in disposableResources)
                foreach (var resource in container)
                    resource.Dispose();
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

        public ISamplerState SamplerState(BSamplerDescription description)
        {
            return CreatorHelper.StoreAndReturn(new CSamplerState(device, ref description), samplerStates);
        }
        #endregion

        #region Resources
        public IVertexLayout VertexLayout(IVertexShader vertexShader, VertexLayoutElement[] elements)
        {
            return CreatorHelper.StoreAndReturn(new CVertexLayout(device, (CVertexShader)vertexShader, elements), vertexLayouts);
        }

        public IBuffer Buffer(BBufferDescription description, SubresourceData? initialData = null)
        {
            SubresourceData data = initialData ?? new SubresourceData();
            return CreatorHelper.StoreAndReturn(new CBuffer(device, ref description, ref data, b => buffers.Remove(b)), buffers);
        }

        public ITexture1D Texture1D(BTexture1DDescription description, SubresourceData[] initialData)
        {
            return CreatorHelper.StoreAndReturn(new CTexture1D(device, ref description, initialData, t => textures1D.Remove(t)), textures1D);
        }

        public ITexture2D Texture2D(BTexture2DDescription description, SubresourceData[] initialData)
        {
            return CreatorHelper.StoreAndReturn(new CTexture2D(device, ref description, initialData, t => textures2D.Remove(t)), textures2D);
        }

        public ITexture3D Texture3D(BTexture3DDescription description, SubresourceData[] initialData)
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
    }
}
