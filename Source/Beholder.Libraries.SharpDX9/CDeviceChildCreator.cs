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
using Beholder.Libraries.SharpDX9.Core;
using Beholder.Libraries.SharpDX9.Resources;
using Beholder.Libraries.SharpDX9.Shaders;
using Beholder.Resources;
using Beholder.Shaders;
using Beholder.Shaders.Reflection;
using Beholder.Utility.ForImplementations;
using Beholder.Utility.ForImplementations.Resources;
using Usage = Beholder.Resources.Usage;

namespace Beholder.Libraries.SharpDX9
{
    public class CDeviceChildCreator : DeviceChildBase<ICDevice>, IDeviceChildCreator
    {
        readonly HashSet<CVertexLayout> vertexLayouts;
        readonly HashSet<CBufferVertex> staticVertexBuffers;
        readonly HashSet<CBufferIndex> staticIndexBuffers;
        readonly HashSet<CBufferVertex> dynamicVertexBuffers;
        readonly HashSet<CBufferIndex> dynamicIndexBuffers;
        readonly HashSet<CBufferUniform<float>> uniformBuffersFloat;
        readonly HashSet<CBufferUniform<int>> uniformBuffersInt;
        readonly HashSet<CBufferUniform<bool>> uniformBuffersBool;

        readonly HashSet<CTextureSurface> textureSurfaces; 
        readonly HashSet<CTextureTextureSingle> staticTexturesSingle;
        readonly HashSet<CTextureTextureCube> staticTexturesCube;
        readonly HashSet<CTexture3D> staticTextures3D;
        readonly HashSet<CTextureTextureSingle> dynamicTexturesSingle;
        readonly HashSet<CTextureTextureCube> dynamicTexturesCube;
        readonly HashSet<CTexture3D> dynamicTextures3D;

        readonly HashSet<CBlendState> blendStates;
        readonly HashSet<CDepthStencilState> depthStencilStates;
        readonly HashSet<CRasterizerState> rasterizerStates;
        readonly HashSet<CSamplerState> samplerStates;

        readonly HashSet<CVertexShader> vertexShaders;
        readonly HashSet<CPixelShader> pixelShaders;

        readonly IEnumerable<IDisposableInternal>[] disposableResourceContainers;
        readonly IEnumerable<IDefaultPoolResource>[] defaultPoolResourcesContainers;

        public CDeviceChildCreator(ICDevice device) : base(device)
        {
            vertexLayouts = new HashSet<CVertexLayout>();
            staticVertexBuffers = new HashSet<CBufferVertex>();
            staticIndexBuffers = new HashSet<CBufferIndex>();
            dynamicVertexBuffers = new HashSet<CBufferVertex>();
            dynamicIndexBuffers = new HashSet<CBufferIndex>();
            uniformBuffersFloat = new HashSet<CBufferUniform<float>>();
            uniformBuffersInt = new HashSet<CBufferUniform<int>>();
            uniformBuffersBool = new HashSet<CBufferUniform<bool>>();

            textureSurfaces = new HashSet<CTextureSurface>();
            staticTexturesSingle = new HashSet<CTextureTextureSingle>();
            staticTexturesCube = new HashSet<CTextureTextureCube>();
            staticTextures3D = new HashSet<CTexture3D>();
            dynamicTexturesSingle = new HashSet<CTextureTextureSingle>();
            dynamicTexturesCube = new HashSet<CTextureTextureCube>();
            dynamicTextures3D = new HashSet<CTexture3D>();

            blendStates = new HashSet<CBlendState>();
            depthStencilStates = new HashSet<CDepthStencilState>();
            rasterizerStates = new HashSet<CRasterizerState>();
            samplerStates = new HashSet<CSamplerState>();

            vertexShaders = new HashSet<CVertexShader>();
            pixelShaders = new HashSet<CPixelShader>();

            disposableResourceContainers = new IEnumerable<IDisposableInternal>[]
            {
                vertexLayouts,
                staticVertexBuffers,
                staticIndexBuffers,
                dynamicVertexBuffers,
                dynamicIndexBuffers,
                uniformBuffersFloat,
                uniformBuffersInt,
                uniformBuffersBool,

                textureSurfaces,
                staticTexturesSingle,
                staticTexturesCube,
                staticTextures3D,
                dynamicTexturesSingle,
                dynamicTexturesCube,
                dynamicTextures3D,

                blendStates,
                depthStencilStates,
                rasterizerStates,
                samplerStates,

                vertexShaders,
                pixelShaders
            };

            defaultPoolResourcesContainers = new IEnumerable<IDefaultPoolResource>[]
            {
                vertexLayouts,
                dynamicVertexBuffers,
                dynamicIndexBuffers,

                textureSurfaces,
                dynamicTexturesSingle,
                dynamicTexturesCube,
                dynamicTextures3D,
                
                blendStates,
                depthStencilStates,
                rasterizerStates,
                samplerStates
            };
        }

        public void OnDeviceLost()
        {
            foreach (var container in defaultPoolResourcesContainers)
                foreach (var resource in container)
                    resource.OnDeviceLost();
        }

        public void OnDeviceReset()
        {
            foreach (var container in defaultPoolResourcesContainers)
                foreach (var resource in container)
                    resource.OnDeviceReset();
        }

        public void Dispose()
        {
            foreach (var container in disposableResourceContainers)
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
            return CreatorHelper.StoreAndReturn(new CVertexLayout(device, (CVertexShader)vertexShader, elements), vertexLayouts);
        }

        public IBuffer Buffer(BufferDescription description, SubresourceData? initialData = null)
        {
            switch (description.BindFlags)
            {
                case BindFlags.None:
                    throw new NotImplementedException();
                case BindFlags.VertexBuffer:
                    return description.Usage == Usage.Dynamic 
                        ? CreatorHelper.StoreAndReturn(new CBufferVertex(device, description, initialData, b => dynamicVertexBuffers.Remove(b)), dynamicVertexBuffers) 
                        : CreatorHelper.StoreAndReturn(new CBufferVertex(device, description, initialData, b => staticVertexBuffers.Remove(b)), staticVertexBuffers);
                case BindFlags.IndexBuffer:
                    return description.Usage == Usage.Dynamic
                        ? CreatorHelper.StoreAndReturn(new CBufferIndex(device, description, initialData, b => dynamicIndexBuffers.Remove(b)), dynamicIndexBuffers)
                        : CreatorHelper.StoreAndReturn(new CBufferIndex(device, description, initialData, b => staticIndexBuffers.Remove(b)), staticIndexBuffers);
                case BindFlags.UniformBuffer:
                    if (description.ExtraFlags.HasFlag(ExtraFlags.UniformInt))
                        return CreatorHelper.StoreAndReturn(new CBufferUniform<int>(device, description, initialData, b => uniformBuffersInt.Remove(b)), uniformBuffersInt);
                    if (description.ExtraFlags.HasFlag(ExtraFlags.UniformBool))
                        return CreatorHelper.StoreAndReturn(new CBufferUniform<bool>(device, description, initialData, b => uniformBuffersBool.Remove(b)), uniformBuffersBool);
                    return CreatorHelper.StoreAndReturn(new CBufferUniform<float>(device, description, initialData, b => uniformBuffersFloat.Remove(b)), uniformBuffersFloat);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        HashSet<CTextureTextureSingle> GetHashSetTextureSingle(bool dynamic)
        {
            return dynamic ? dynamicTexturesSingle : staticTexturesSingle;
        }
        HashSet<CTextureTextureCube> GetHashSetTextureCube(bool dynamic)
        {
            return dynamic ? dynamicTexturesCube : staticTexturesCube;
        }
        HashSet<CTexture3D> GetHashSetTexture3D(bool dynamic)
        {
            return dynamic ? dynamicTextures3D : staticTextures3D;
        }

        public ITexture1D Texture1D(Texture1DDescription description, SubresourceData[] initialData)
        {
            if (!description.MiscFlags.HasFlag(MiscFlags.TextureCube))
            {
                if (description.MipLevels == 1 && (description.BindFlags == BindFlags.RenderTarget || description.BindFlags == BindFlags.DepthStencil))
                {
                    var tex = new CTextureSurface(device, description, initialData, t => textureSurfaces.Remove(t));
                    return CreatorHelper.StoreAndReturn(tex, textureSurfaces);
                }
                else
                {
                    var tex = new CTextureTextureSingle(device, description, initialData, t => GetHashSetTextureSingle(t.IsDefaultPool).Remove(t));
                    return CreatorHelper.StoreAndReturn(tex, GetHashSetTextureSingle(tex.IsDefaultPool));
                }
            }
            else
            {
                var tex = new CTextureTextureCube(device, description, initialData, t => GetHashSetTextureCube(t.IsDefaultPool).Remove(t));
                return CreatorHelper.StoreAndReturn(tex, GetHashSetTextureCube(tex.IsDefaultPool));
            }
        }

        public ITexture2D Texture2D(Texture2DDescription description, SubresourceData[] initialData)
        {
            if (!description.MiscFlags.HasFlag(MiscFlags.TextureCube))
            {
                if (description.MipLevels == 1 && (description.BindFlags == BindFlags.RenderTarget || description.BindFlags == BindFlags.DepthStencil))
                {
                    var tex = new CTextureSurface(device, description, initialData, t => textureSurfaces.Remove(t));
                    return CreatorHelper.StoreAndReturn(tex, textureSurfaces);
                }
                else
                {
                    var tex = new CTextureTextureSingle(device, description, initialData, t => GetHashSetTextureSingle(t.IsDefaultPool).Remove(t));
                    return CreatorHelper.StoreAndReturn(tex, GetHashSetTextureSingle(tex.IsDefaultPool));
                }
            }
            else
            {
                var tex = new CTextureTextureCube(device, description, initialData, t => GetHashSetTextureCube(t.IsDefaultPool).Remove(t));
                return CreatorHelper.StoreAndReturn(tex, GetHashSetTextureCube(tex.IsDefaultPool));
            }
        }

        public ITexture3D Texture3D(Texture3DDescription description, SubresourceData[] initialData)
        {
            var tex = new CTexture3D(device, description, initialData, t => GetHashSetTexture3D(t.IsDefaultPool));
            return CreatorHelper.StoreAndReturn(tex, GetHashSetTexture3D(tex.IsDefaultPool));
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
            throw new NotSupportedException("Hull shaders are not supported by D3D9");
        }

        public IDomainShader DomainShader(IShaderReflection reflection)
        {
            throw new NotSupportedException("Domain shaders are not supported by D3D9");
        }

        public IGeometryShader GeometryShader(IShaderReflection reflection)
        {
            throw new NotSupportedException("Geometry shaders are not supported by D3D9");
        }

        public IPixelShader PixelShader(IShaderReflection reflection)
        {
            var cReflection = new CShaderReflection(reflection);
            return CreatorHelper.StoreAndReturn(new CPixelShader(device, cReflection), pixelShaders);
        }

        public IComputeShader ComputeShader(IShaderReflection reflection)
        {
            throw new NotSupportedException("Compute shaders are not supported by D3D9");
        }

        public IShaderCombination ShaderCombination(IVertexShader vertexShader, IHullShader hullShader,
            IDomainShader domainShader, IGeometryShader geometryShader, IPixelShader pixelShader)
        {
            return new CShaderCombination(device, (CVertexShader)vertexShader, (CPixelShader)pixelShader);
        }
        #endregion
    }
}
