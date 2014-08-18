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
using UnorderedAccessViewDescription = Beholder.Resources.UnorderedAccessViewDescription;
using UnorderedAccessViewDimension = Beholder.Resources.UnorderedAccessViewDimension;

namespace Beholder.Libraries.SharpDX11.Resources
{
    public class CUnorderedAccessView : UnorderedAccessViewBase<ICDevice>
    {
        readonly UnorderedAccessView d3dUav;
        readonly ICResource resource;

        public UnorderedAccessView D3DUnorderedAccessView { get { return d3dUav; } }
        public override IResource Resource { get { return resource; } }
        
        public CUnorderedAccessView(ICResource resource, ref UnorderedAccessViewDescription desc)
            : base(resource.Device, ref desc)
        {
            this.resource = resource;

            var d3dDescription = new SharpDX.Direct3D11.UnorderedAccessViewDescription {Format = (Format)desc.FormatID};
            switch (desc.Dimension)
            {
                case UnorderedAccessViewDimension.Unknown:
                    d3dDescription.Dimension = SharpDX.Direct3D11.UnorderedAccessViewDimension.Unknown;
                    break;
                case UnorderedAccessViewDimension.Buffer:
                    d3dDescription.Dimension = SharpDX.Direct3D11.UnorderedAccessViewDimension.Buffer;
                    d3dDescription.Buffer = new SharpDX.Direct3D11.UnorderedAccessViewDescription.BufferResource
                    {
                        FirstElement = desc.FirstElement,
                        ElementCount = desc.ElementCount,
                        Flags = CtSharpDX11.UnorderedAccessViewBufferFlags(desc.BufferFlags)
                    };
                    break;
                case UnorderedAccessViewDimension.Texture1D:
                    d3dDescription.Dimension = SharpDX.Direct3D11.UnorderedAccessViewDimension.Texture1D;
                    d3dDescription.Texture1D = new SharpDX.Direct3D11.UnorderedAccessViewDescription.Texture1DResource
                    {
                        MipSlice = desc.MipSlice
                    };
                    break;
                case UnorderedAccessViewDimension.Texture1DArray:
                    d3dDescription.Dimension = SharpDX.Direct3D11.UnorderedAccessViewDimension.Texture1DArray;
                    d3dDescription.Texture1DArray = new SharpDX.Direct3D11.UnorderedAccessViewDescription.Texture1DArrayResource
                    {
                        MipSlice = desc.MipSlice,
                        FirstArraySlice = desc.FirstArraySlice,
                        ArraySize = desc.ArraySize
                    };
                    break;
                case UnorderedAccessViewDimension.Texture2D:
                    d3dDescription.Dimension = SharpDX.Direct3D11.UnorderedAccessViewDimension.Texture2D;
                    d3dDescription.Texture2D = new SharpDX.Direct3D11.UnorderedAccessViewDescription.Texture2DResource
                    {
                        MipSlice = desc.MipSlice
                    };
                    break;
                case UnorderedAccessViewDimension.Texture2DArray:
                    d3dDescription.Dimension = SharpDX.Direct3D11.UnorderedAccessViewDimension.Texture2DArray;
                    d3dDescription.Texture2DArray = new SharpDX.Direct3D11.UnorderedAccessViewDescription.Texture2DArrayResource
                    {
                        MipSlice = desc.MipSlice,
                        FirstArraySlice = desc.FirstArraySlice,
                        ArraySize = desc.ArraySize
                    };
                    break;
                case UnorderedAccessViewDimension.Texture3D:
                    d3dDescription.Dimension = SharpDX.Direct3D11.UnorderedAccessViewDimension.Texture3D;
                    d3dDescription.Texture3D = new SharpDX.Direct3D11.UnorderedAccessViewDescription.Texture3DResource
                    {
                        MipSlice = desc.MipSlice,
                        FirstWSlice = desc.FirstDepthSlice,
                        WSize = desc.DepthSliceCount
                    };
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            d3dUav = new UnorderedAccessView(device.D3DDevice, resource.D3DResource, d3dDescription);
        }

        public void Dispose()
        {
            d3dUav.Dispose();
        }

        public bool DescEquals(ref UnorderedAccessViewDescription otherDesc)
        {
            return UnorderedAccessViewDescription.Equals(ref desc, ref otherDesc);
        }
    }
}
