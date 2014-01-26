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
using Beholder.Resources;
using Beholder.Utility.ForImplementations.Resources;
using SharpDX.DXGI;
using SharpDX.Direct3D11;
using ShaderResourceViewDescription = Beholder.Resources.ShaderResourceViewDescription;

namespace Beholder.Libraries.SharpDX11.Resources
{
    class CShaderResourceView : ShaderResourceViewBase<ICDevice>
    {
        readonly ShaderResourceView d3dSrv;
        readonly ICResource resource;

        public ShaderResourceView D3DShaderResourceView { get { return d3dSrv; } }
        public override IResource Resource { get { return resource; } }
        
        public CShaderResourceView(ICResource resource, ref ShaderResourceViewDescription desc)
            : base(resource.Device, ref desc)
        {
            this.resource = resource;

            var d3dDescription = new SharpDX.Direct3D11.ShaderResourceViewDescription {Format = (Format)desc.FormatID};
            switch (desc.Dimension)
            {
                case ShaderResourceViewDimension.Unknown:
                    d3dDescription.Dimension = SharpDX.Direct3D.ShaderResourceViewDimension.Unknown;
                    break;
                case ShaderResourceViewDimension.Buffer:
                    d3dDescription.Dimension = SharpDX.Direct3D.ShaderResourceViewDimension.Buffer;
                    d3dDescription.Buffer = new SharpDX.Direct3D11.ShaderResourceViewDescription.BufferResource
                    {
                        ElementOffset = desc.FirstElement,
                        ElementWidth = desc.ElementCount
                    };
                    break;
                case ShaderResourceViewDimension.Texture1D:
                    d3dDescription.Dimension = SharpDX.Direct3D.ShaderResourceViewDimension.Texture1D;
                    d3dDescription.Texture1D = new SharpDX.Direct3D11.ShaderResourceViewDescription.Texture1DResource
                    {
                        MostDetailedMip = desc.MostDetailedMip,
                        MipLevels = desc.MipLevels
                    };
                    break;
                case ShaderResourceViewDimension.Texture1DArray:
                    d3dDescription.Dimension = SharpDX.Direct3D.ShaderResourceViewDimension.Texture1DArray;
                    d3dDescription.Texture1DArray = new SharpDX.Direct3D11.ShaderResourceViewDescription.Texture1DArrayResource
                    {
                        MostDetailedMip = desc.MostDetailedMip,
                        MipLevels = desc.MipLevels,
                        FirstArraySlice = desc.FirstArraySlice,
                        ArraySize = desc.ArraySize
                    };
                    break;
                case ShaderResourceViewDimension.Texture2D:
                    d3dDescription.Dimension = SharpDX.Direct3D.ShaderResourceViewDimension.Texture2D;
                    d3dDescription.Texture2D = new SharpDX.Direct3D11.ShaderResourceViewDescription.Texture2DResource
                    {
                        MostDetailedMip = desc.MostDetailedMip,
                        MipLevels = desc.MipLevels
                    };
                    break;
                case ShaderResourceViewDimension.Texture2DArray:
                    d3dDescription.Dimension = SharpDX.Direct3D.ShaderResourceViewDimension.Texture2DArray;
                    d3dDescription.Texture2DArray = new SharpDX.Direct3D11.ShaderResourceViewDescription.Texture2DArrayResource
                    {
                        MostDetailedMip = desc.MostDetailedMip,
                        MipLevels = desc.MipLevels,
                        FirstArraySlice = desc.FirstArraySlice,
                        ArraySize = desc.ArraySize
                    };
                    break;
                case ShaderResourceViewDimension.Texture2DMultisampled:
                    d3dDescription.Dimension = SharpDX.Direct3D.ShaderResourceViewDimension.Texture2DMultisampled;
                    d3dDescription.Texture2DMS = new SharpDX.Direct3D11.ShaderResourceViewDescription.Texture2DMultisampledResource();
                    break;
                case ShaderResourceViewDimension.Texture2DMultisampledArray:
                    d3dDescription.Dimension = SharpDX.Direct3D.ShaderResourceViewDimension.Texture2DMultisampledArray;
                    d3dDescription.Texture2DMSArray = new SharpDX.Direct3D11.ShaderResourceViewDescription.Texture2DMultisampledArrayResource
                    {
                        FirstArraySlice = desc.FirstArraySlice,
                        ArraySize = desc.ArraySize
                    };
                    break;
                case ShaderResourceViewDimension.Texture3D:
                    d3dDescription.Dimension = SharpDX.Direct3D.ShaderResourceViewDimension.Texture3D;
                    d3dDescription.Texture3D = new SharpDX.Direct3D11.ShaderResourceViewDescription.Texture3DResource
                    {
                        MostDetailedMip = desc.MostDetailedMip,
                        MipLevels = desc.MipLevels
                    };
                    break;
                case ShaderResourceViewDimension.TextureCube:
                    d3dDescription.Dimension = SharpDX.Direct3D.ShaderResourceViewDimension.TextureCube;
                    d3dDescription.TextureCube = new SharpDX.Direct3D11.ShaderResourceViewDescription.TextureCubeResource
                    {
                        MostDetailedMip = desc.MostDetailedMip,
                        MipLevels = desc.MipLevels
                    };
                    break;
                case ShaderResourceViewDimension.TextureCubeArray:
                    d3dDescription.Dimension = SharpDX.Direct3D.ShaderResourceViewDimension.TextureCubeArray;
                    d3dDescription.TextureCubeArray = new SharpDX.Direct3D11.ShaderResourceViewDescription.TextureCubeArrayResource
                    {
                        MostDetailedMip = desc.MostDetailedMip,
                        MipLevels = desc.MipLevels,
                        First2DArrayFace = desc.FirstArraySlice,
                        CubeCount = desc.ArraySize / 6
                    };
                    break;
                case ShaderResourceViewDimension.ExtendedBuffer:
                    d3dDescription.Dimension = SharpDX.Direct3D.ShaderResourceViewDimension.ExtendedBuffer;
                    d3dDescription.BufferEx = new SharpDX.Direct3D11.ShaderResourceViewDescription.ExtendedBufferResource
                    {
                        FirstElement = desc.FirstElement,
                        ElementCount = desc.ElementCount,
                        Flags = CtSharpDX11.ShaderResourceViewExtendedBufferFlags(desc.ExtendedBufferFlags)
                    };
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            d3dSrv = new ShaderResourceView(device.D3DDevice, resource.D3DResource, d3dDescription);
        }

        public void Dispose()
        {
            d3dSrv.Dispose();
        }

        public bool DescEquals(ref ShaderResourceViewDescription otherDesc)
        {
            return ShaderResourceViewDescription.Equals(ref desc, ref otherDesc);
        }
    }
}
