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
using RenderTargetViewDescription = Beholder.Resources.RenderTargetViewDescription;
using RenderTargetViewDimension = Beholder.Resources.RenderTargetViewDimension;

namespace Beholder.Libraries.SharpDX11.Resources
{
    class CRenderTargetView : RenderTargetViewBase<ICDevice, ICResource>
    {
        readonly RenderTargetView d3dRtv;

        public RenderTargetView D3DRenderTargetView { get { return d3dRtv; } }

        public CRenderTargetView(ICResource resource, ref RenderTargetViewDescription desc)
            : base(resource.Device, resource, ref desc)
        {
            var d3dDescription = new SharpDX.Direct3D11.RenderTargetViewDescription {Format = (Format)desc.FirstArraySlice};
            switch (desc.Dimension)
            {
                case RenderTargetViewDimension.Unknown:
                    d3dDescription.Dimension = SharpDX.Direct3D11.RenderTargetViewDimension.Unknown;
                    break;
                case RenderTargetViewDimension.Buffer:
                    d3dDescription.Dimension = SharpDX.Direct3D11.RenderTargetViewDimension.Buffer;
                    d3dDescription.Buffer = new SharpDX.Direct3D11.RenderTargetViewDescription.BufferResource
                    {
                        ElementOffset = desc.ElementOffset,
                        ElementWidth = desc.ElementWidth
                    };
                    break;
                case RenderTargetViewDimension.Texture1D:
                    d3dDescription.Dimension = SharpDX.Direct3D11.RenderTargetViewDimension.Texture1D;
                    d3dDescription.Texture1D = new SharpDX.Direct3D11.RenderTargetViewDescription.Texture1DResource
                    {
                        MipSlice = desc.MipSlice
                    };
                    break;
                case RenderTargetViewDimension.Texture1DArray:
                    d3dDescription.Dimension = SharpDX.Direct3D11.RenderTargetViewDimension.Texture1DArray;
                    d3dDescription.Texture1DArray = new SharpDX.Direct3D11.RenderTargetViewDescription.Texture1DArrayResource
                    {
                        MipSlice = desc.MipSlice,
                        FirstArraySlice = desc.FirstArraySlice,
                        ArraySize = desc.ArraySize
                    };
                    break;
                case RenderTargetViewDimension.Texture2D:
                    d3dDescription.Dimension = SharpDX.Direct3D11.RenderTargetViewDimension.Texture2D;
                    d3dDescription.Texture2D = new SharpDX.Direct3D11.RenderTargetViewDescription.Texture2DResource
                    {
                        MipSlice = desc.MipSlice
                    };
                    break;
                case RenderTargetViewDimension.Texture2DArray:
                    d3dDescription.Dimension = SharpDX.Direct3D11.RenderTargetViewDimension.Texture2DArray;
                    d3dDescription.Texture2DArray = new SharpDX.Direct3D11.RenderTargetViewDescription.Texture2DArrayResource
                    {
                        MipSlice = desc.MipSlice,
                        FirstArraySlice = desc.FirstArraySlice,
                        ArraySize = desc.ArraySize
                    };
                    break;
                case RenderTargetViewDimension.Texture2DMultisampled:
                    d3dDescription.Dimension = SharpDX.Direct3D11.RenderTargetViewDimension.Texture2DMultisampled;
                    d3dDescription.Texture2DMS = new SharpDX.Direct3D11.RenderTargetViewDescription.Texture2DMultisampledResource();
                    break;
                case RenderTargetViewDimension.Texture2DMultisampledArray:
                    d3dDescription.Dimension = SharpDX.Direct3D11.RenderTargetViewDimension.Texture2DMultisampledArray;
                    d3dDescription.Texture2DMSArray = new SharpDX.Direct3D11.RenderTargetViewDescription.Texture2DMultisampledArrayResource
                    {
                        FirstArraySlice = desc.FirstArraySlice,
                        ArraySize = desc.ArraySize
                    };
                    break;
                case RenderTargetViewDimension.Texture3D:
                    d3dDescription.Dimension = SharpDX.Direct3D11.RenderTargetViewDimension.Texture3D;
                    d3dDescription.Texture3D = new SharpDX.Direct3D11.RenderTargetViewDescription.Texture3DResource
                    {
                        MipSlice = desc.MipSlice,
                        FirstDepthSlice = desc.FirstDepthSlice,
                        DepthSliceCount = desc.DepthSliceCount
                    };
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            d3dRtv = new RenderTargetView(device.D3DDevice, resource.D3DResource, d3dDescription);
        }

        public void Dispose()
        {
            d3dRtv.Dispose();
        }

        public bool DescEquals(ref RenderTargetViewDescription otherDesc)
        {
            return RenderTargetViewDescription.Equals(ref desc, ref otherDesc);
        }
    }
}
