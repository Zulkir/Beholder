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
using Beholder.Utility.Helpers;

namespace Beholder.Utility.ForImplementations.Resources
{
    public abstract class RenderTargetViewBase<TDevice, TResource> : DeviceChildBase<TDevice>, IRenderTargetView 
        where TDevice : IDevice
        where TResource : IResource
    {
        protected RenderTargetViewDescription desc;
        protected readonly TResource resource;
        protected readonly int width;
        protected readonly int height;

        public IResource Resource { get { return resource; } }
        public int Width { get { return width; } }
        public int Height { get { return height; } }

        public RenderTargetViewDimension Dimension { get { return desc.Dimension; } }
        public int FormatID { get { return desc.FormatID; } }
        public int ElementOffset { get { return desc.ElementOffset; } }
        public int ElementWidth { get { return desc.ElementWidth; } }
        public int MipSlice { get { return desc.MipSlice; } }
        public int FirstArraySlice { get { return desc.FirstArraySlice; } }
        public int ArraySize { get { return desc.ArraySize; } }
        public int FirstDepthSlice { get { return desc.FirstDepthSlice; } }
        public int DepthSliceCount { get { return desc.DepthSliceCount; } }
        public void GetDescription(out RenderTargetViewDescription description) { description = desc; }

        protected RenderTargetViewBase(TDevice device, TResource resource, ref RenderTargetViewDescription desc)
            : base (device)
        {
            this.desc = desc;
            this.resource = resource;

            switch (desc.Dimension)
            {
                case RenderTargetViewDimension.Buffer:
                    var buffer = (IBuffer)resource;
                    width = (buffer.SizeInBytes - desc.ElementOffset) / desc.ElementWidth;
                    height = 1;
                    break;
                case RenderTargetViewDimension.Texture1D:
                case RenderTargetViewDimension.Texture1DArray:
                    var tex1D = (ITexture1D)resource;
                    width = TextureHelper.MipSize(desc.MipSlice, tex1D.Width);
                    height = 1;
                    break;
                case RenderTargetViewDimension.Texture2D:
                case RenderTargetViewDimension.Texture2DArray:
                    var tex2D = (ITexture2D)resource;
                    width = TextureHelper.MipSize(desc.MipSlice, tex2D.Width);
                    height = TextureHelper.MipSize(desc.MipSlice, tex2D.Height);
                    break;
                case RenderTargetViewDimension.Texture2DMultisampled:
                case RenderTargetViewDimension.Texture2DMultisampledArray:
                    var tex2DMS = (ITexture2D)resource;
                    width = tex2DMS.Width;
                    height = tex2DMS.Height;
                    break;
                case RenderTargetViewDimension.Texture3D:
                    var tex3D = (ITexture3D)resource;
                    width = TextureHelper.MipSize(desc.MipSlice, tex3D.Width);
                    height = TextureHelper.MipSize(desc.MipSlice, tex3D.Height);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("desc.Dimension");
            }
        }
    }
}
