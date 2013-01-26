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

using Beholder.Core;
using Beholder.Resources;
using Beholder.Shaders;
using Beholder.Shaders.Reflection;

namespace Beholder
{
    /// <summary>
    /// A <see cref="IDevice"/> 'submenu' responsible for creating new <see cref="IDeviceChild"/>s.
    /// </summary>
    public interface IDeviceChildCreator : IDeviceChild
    {
        IBuffer Buffer(BufferDescription description, SubresourceData? initialData = null);
        ITexture1D Texture1D(Texture1DDescription description, SubresourceData[] initialData = null);
        ITexture2D Texture2D(Texture2DDescription description, SubresourceData[] initialData = null);
        ITexture3D Texture3D(Texture3DDescription description, SubresourceData[] initialData = null);

        IVertexShader VertexShader(IShaderReflection reflection);
        IHullShader HullShader(IShaderReflection reflection);
        IDomainShader DomainShader(IShaderReflection reflection);
        IGeometryShader GeometryShader(IShaderReflection reflection);
        IPixelShader PixelShader(IShaderReflection reflection);
        IComputeShader ComputeShader(IShaderReflection reflection);

        IShaderCombination ShaderCombination(IVertexShader vertexShader, IHullShader hullShader,
            IDomainShader domainShader, IGeometryShader geometryShader, IPixelShader pixelShader);

        IVertexLayout VertexLayout(IVertexShader vertexShader, VertexLayoutElement[] elements);

        IBlendState BlendState(BlendDescription description);
        IDepthStencilState DepthStencilState(DepthStencilDescription description);
        IRasterizerState RasterizerState(RasterizerDescription description);
        ISamplerState SamplerState(SamplerDescription description);
    }
}
