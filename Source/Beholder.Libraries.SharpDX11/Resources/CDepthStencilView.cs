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
using Beholder.Utility.ForImplementations.Resources;
using SharpDX.DXGI;
using SharpDX.Direct3D11;
using DepthStencilViewDescription = Beholder.Resources.DepthStencilViewDescription;
using DepthStencilViewDimension = Beholder.Resources.DepthStencilViewDimension;

namespace Beholder.Libraries.SharpDX11.Resources
{
    class CDepthStencilView : DepthStencilViewBase<ICDevice, ICResource>
    {
        readonly DepthStencilView d3dDsv;

        public DepthStencilView D3DDepthStencilView { get { return d3dDsv; } }

        public CDepthStencilView(ICResource resource, ref DepthStencilViewDescription desc)
            : base(resource.Device, resource, ref desc)
        {
            var d3dDesc = new SharpDX.Direct3D11.DepthStencilViewDescription {Format = (Format)desc.FormatID};
            switch (desc.Dimension)
            {
                case DepthStencilViewDimension.Unknown:
                    d3dDesc.Dimension = SharpDX.Direct3D11.DepthStencilViewDimension.Unknown;
                    break;
                case DepthStencilViewDimension.Texture1D:
                    d3dDesc.Dimension = SharpDX.Direct3D11.DepthStencilViewDimension.Texture1D;
                    d3dDesc.Texture1D = new SharpDX.Direct3D11.DepthStencilViewDescription.Texture1DResource
                    {
                        MipSlice = desc.MipSlice
                    };
                    break;
                case DepthStencilViewDimension.Texture1DArray:
                    d3dDesc.Dimension = SharpDX.Direct3D11.DepthStencilViewDimension.Texture1DArray;
                    d3dDesc.Texture1DArray = new SharpDX.Direct3D11.DepthStencilViewDescription.Texture1DArrayResource
                    {
                        MipSlice = desc.MipSlice,
                        FirstArraySlice = desc.FirstArraySlice,
                        ArraySize = desc.ArraySize
                    };
                    break;
                case DepthStencilViewDimension.Texture2D:
                    d3dDesc.Dimension = SharpDX.Direct3D11.DepthStencilViewDimension.Texture2D;
                    d3dDesc.Texture2D = new SharpDX.Direct3D11.DepthStencilViewDescription.Texture2DResource
                    {
                        MipSlice = desc.MipSlice
                    };
                    break;
                case DepthStencilViewDimension.Texture2DArray:
                    d3dDesc.Dimension = SharpDX.Direct3D11.DepthStencilViewDimension.Texture2DArray;
                    d3dDesc.Texture2DArray = new SharpDX.Direct3D11.DepthStencilViewDescription.Texture2DArrayResource
                    {
                        MipSlice = desc.MipSlice,
                        FirstArraySlice = desc.FirstArraySlice,
                        ArraySize = desc.ArraySize
                    };
                    break;
                case DepthStencilViewDimension.Texture2DMultisampled:
                    d3dDesc.Dimension = SharpDX.Direct3D11.DepthStencilViewDimension.Texture2DMultisampled;
                    d3dDesc.Texture2DMS = new SharpDX.Direct3D11.DepthStencilViewDescription.Texture2DMultisampledResource();
                    break;
                case DepthStencilViewDimension.Texture2DMultisampledArray:
                    d3dDesc.Dimension = SharpDX.Direct3D11.DepthStencilViewDimension.Texture2DMultisampledArray;
                    d3dDesc.Texture2DMSArray = new SharpDX.Direct3D11.DepthStencilViewDescription.Texture2DMultisampledArrayResource
                    {
                        FirstArraySlice = desc.FirstArraySlice,
                        ArraySize = desc.ArraySize
                    };
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            d3dDsv = new DepthStencilView(device.D3DDevice, resource.D3DResource, d3dDesc);
        }

        public void Dispose()
        {
            d3dDsv.Dispose();
        }

        public bool DescEquals(ref DepthStencilViewDescription otherDesc)
        {
            return DepthStencilViewDescription.Equals(ref desc, ref otherDesc);
        }
    }
}
