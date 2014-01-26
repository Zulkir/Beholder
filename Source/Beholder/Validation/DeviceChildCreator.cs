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
using Beholder.Resources;
using Beholder.Shaders;
using Beholder.Shaders.Reflection;

namespace Beholder.Validation
{
    class DeviceChildCreator : Wrapper<IDeviceChildCreator>, IDeviceChildCreator
    {
        public DeviceChildCreator(IDeviceChildCreator real)
            : base(real)
        {
            
        }

        public IDevice Device
        {
            get { return Wrappers.Get(Real.Device); }
        }
        
        public IBuffer Buffer(BufferDescription description, SubresourceData? initialData = new SubresourceData?())
        {
            Resources.Buffer.ValidateDescription(Real.Device.Adapter, description);
            if (initialData.HasValue)
            {
                if (initialData.Value.Pointer == IntPtr.Zero)
                    throw new ArgumentException("initialData.Pointer cannot be NULL");
                //if (initialData.Value.RowByteAlignment != 0)
                //    throw new ArgumentException("initialData.RowByteAlignment must be 0 when creating a Buffer");
                Check.NotNull(initialData.Value.UnpinPointer, "initialData.UnpinPointer");
            }
            return Wrappers.Get(Real.Buffer(description, initialData));
        }

        public ITexture1D Texture1D(Texture1DDescription description, SubresourceData[] initialData = null)
        {
            Resources.Texture1D.ValidateDescription(Real.Device.Adapter, description);
            if (initialData != null)
            {
                if (initialData.Length != description.ArraySize * description.MipLevels)
                    throw new ArgumentException("initialData.Length must be equal to the number of subresources (ArraySize * MipLevels in case of a Texture1D)");
                for (int i = 0; i < initialData.Length; i++)
                {
                    if (initialData[i].Pointer == IntPtr.Zero)
                        throw new ArgumentException(string.Format("initialData[{0}].Pointer cannot be NULL", i));
                    //if (initialData[i].RowByteAlignment != 0)
                    //    throw new ArgumentException(string.Format("initialData[{0}].RowByteAlignment must be 0 when creating a Texture1D", i));
                    Check.NotNull(initialData[i].UnpinPointer, string.Format("initialData[{0}].UnpinPointer", i));
                }
            }
            return Wrappers.Get(Real.Texture1D(description, initialData));
        }

        public ITexture2D Texture2D(Texture2DDescription description, SubresourceData[] initialData = null)
        {
            Resources.Texture2D.ValidateDescription(Real.Device.Adapter, description);
            if (initialData != null)
            {
                if (initialData.Length != description.ArraySize * description.MipLevels)
                    throw new ArgumentException("initialData.Length must be equal to the number of subresources (ArraySize * MipLevels in case of a Texture2D)");
                for (int i = 0; i < initialData.Length; i++)
                {
                    if (initialData[i].Pointer == IntPtr.Zero)
                        throw new ArgumentException(string.Format("initialData[{0}].Pointer cannot be NULL", i));
                    if (initialData[i].RowByteAlignment != 1 && initialData[i].RowByteAlignment != 2 && initialData[i].RowByteAlignment != 4 && initialData[i].RowByteAlignment != 8)
                        throw new ArgumentException(string.Format("initialData[{0}].RowByteAlignment must be either 1, 2, 4, or 8 when creating a Texture2D", i));
                    Check.NotNull(initialData[i].UnpinPointer, string.Format("initialData[{0}].UnpinPointer", i));
                }
            }
            return Wrappers.Get(Real.Texture2D(description, initialData));
        }

        public ITexture3D Texture3D(Texture3DDescription description, SubresourceData[] initialData = null)
        {
            Resources.Texture3D.ValidateDescription(Real.Device.Adapter, description);
            if (initialData != null)
            {
                if (initialData.Length != description.MipLevels)
                    throw new ArgumentException("initialData.Length must be equal to the number of subresources (MipLevels in case of a Texture3D)");
                for (int i = 0; i < initialData.Length; i++)
                {
                    if (initialData[i].Pointer == IntPtr.Zero)
                        throw new ArgumentException(string.Format("initialData[{0}].Pointer cannot be NULL", i));
                    if (initialData[i].RowByteAlignment != 1 && initialData[i].RowByteAlignment != 2 && initialData[i].RowByteAlignment != 4 && initialData[i].RowByteAlignment != 8)
                        throw new ArgumentException(string.Format("initialData[{0}].RowByteAlignment must be either 1, 2, 4, or 8 when creating a Texture3D", i));
                    Check.NotNull(initialData[i].UnpinPointer, string.Format("initialData[{0}].UnpinPointer", i));
                }
            }
            return Wrappers.Get(Real.Texture3D(description, initialData));
        }

        public IVertexShader VertexShader(IShaderReflection reflection)
        {
            Shaders.VertexShader.ValidateReflection(reflection);
            return Wrappers.Get(Real.VertexShader(reflection));
        }

        public IHullShader HullShader(IShaderReflection reflection)
        {
            Shaders.HullShader.ValidateReflection(reflection);
            return Wrappers.Get(Real.HullShader(reflection));
        }

        public IDomainShader DomainShader(IShaderReflection reflection)
        {
            Shaders.DomainShader.ValidateReflection(reflection);
            return Wrappers.Get(Real.DomainShader(reflection));
        }

        public IGeometryShader GeometryShader(IShaderReflection reflection)
        {
            Shaders.GeometryShader.ValidateReflection(reflection);
            return Wrappers.Get(Real.GeometryShader(reflection));
        }

        public IPixelShader PixelShader(IShaderReflection reflection)
        {
            Shaders.PixelShader.ValidateReflection(reflection);
            return Wrappers.Get(Real.PixelShader(reflection));
        }

        public IComputeShader ComputeShader(IShaderReflection reflection)
        {
            Shaders.ComputeShader.ValidateReflection(reflection);
            return Wrappers.Get(Real.ComputeShader(reflection));
        }

        public IVertexLayout VertexLayout(IVertexShader vertexShader, VertexLayoutElement[] elements)
        {
            Check.ExistingInternal(vertexShader, "vertexShader");
            Check.NotNullNotEmpty(elements, "elements");
            Core.VertexLayout.ValidateElements(vertexShader, elements);
            return Wrappers.Get(Real.VertexLayout(vertexShader.GetReal(), elements));
        }

        public IBlendState BlendState(BlendDescription description)
        {
            Core.BlendState.ValidateDescription(description);
            return Wrappers.Get(Real.BlendState(description));
        }

        public IDepthStencilState DepthStencilState(DepthStencilDescription description)
        {
            Core.DepthStencilState.ValidateDscription(description);
            return Wrappers.Get(Real.DepthStencilState(description));
        }

        public IRasterizerState RasterizerState(RasterizerDescription description)
        {
            Core.RasterizerState.ValidateDescription(description);
            return Wrappers.Get(Real.RasterizerState(description));
        }

        public ISamplerState SamplerState(SamplerDescription description)
        {
            Core.SamplerState.ValidateDescription(description);
            return Wrappers.Get(Real.SamplerState(description));
        }

        public IShaderCombination ShaderCombination(IVertexShader vertexShader, IHullShader hullShader, IDomainShader domainShader, IGeometryShader geometryShader, IPixelShader pixelShader)
        {
            Check.NullOrInternal(vertexShader, "vertexShader");
            Check.NullOrInternal(hullShader, "hullShader");
            Check.NullOrInternal(domainShader, "domainShader");
            Check.NullOrInternal(geometryShader, "geometryShader");
            Check.NullOrInternal(pixelShader, "pixelShader");
            
            if (vertexShader == null)
                throw new ArgumentException("Vertex shader must always be present");
            if (hullShader == null ^ domainShader == null)
                throw new ArgumentException("Hull and Domain shaders must be present or absent together");
            return Wrappers.Get(Real.ShaderCombination(vertexShader.GetReal(), hullShader.GetReal(), domainShader.GetReal(), geometryShader.GetReal(), pixelShader.GetReal()));
        }
    }
}
