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
using Beholder.Platform;
using Beholder.Resources;
using SharpDX.Direct3D9;
using Usage = Beholder.Resources.Usage;

namespace Beholder.Libraries.SharpDX9.Resources
{
    class CTextureSurface : CTexture
    {
        Surface d3dSurface;

        private CTextureSurface(ICDevice device, ResourceDimension dimension, Texture2DDescription description, SubresourceData[] initialData, Action<CTextureSurface> onRelease)
            : base(device, dimension, description, t => onRelease((CTextureSurface)t))
        {
            CreateNative();

            if (initialData != null) 
                throw new NotSupportedException("Initial newData is not supported for render target and depth-stencil resources by D3D9 implementation of Beholder");
        }

        public CTextureSurface(ICDevice device, Texture1DDescription description, SubresourceData[] initialData, Action<CTextureSurface> onRelease)
            : this(device, ResourceDimension.Texture1D, new Texture2DDescription
            {
                Width = description.Width,
                Height = 1,
                MipLevels = description.MipLevels,
                ArraySize = description.ArraySize,
                FormatID = description.FormatID,
                Sampling = Sampling.NoMultisampling,
                Usage = description.Usage,
                BindFlags = description.BindFlags,
                ExtraFlags = description.ExtraFlags,
                MiscFlags = description.MiscFlags                 
            }, initialData, onRelease)
        { }

        public CTextureSurface(ICDevice device, Texture2DDescription description, SubresourceData[] initialData, Action<CTextureSurface> onRelease)
            : this(device, ResourceDimension.Texture2D, description, initialData, onRelease)
        { }

        void CreateNative()
        {
            switch (desc.BindFlags)
            {
                case BindFlags.RenderTarget:
                    d3dSurface = Surface.CreateRenderTarget(device.D3DDevice, desc.Width, desc.Height, (Format)desc.FormatID,
                        CtSharpDX9.MultisampleType(desc.Sampling.Count), desc.Sampling.Quality,
                        desc.Usage == Usage.Dynamic || desc.Usage == Usage.Staging);
                    break;
                case BindFlags.DepthStencil:
                    d3dSurface = Surface.CreateDepthStencil(device.D3DDevice, desc.Width, desc.Height, (Format)desc.FormatID,
                        CtSharpDX9.MultisampleType(desc.Sampling.Count), desc.Sampling.Quality, true);
                    break;
            }
        }

        public override void DisposeConcrete()
        {
            if (d3dSurface != null) { d3dSurface.Dispose(); d3dSurface = null; }
        }

        public override void OnDeviceLost()
        {
            Dispose();
        }

        public override void OnDeviceReset()
        {
            CreateNative();

            if (rtvs != null)
                foreach (var rtv in rtvs)
                    rtv.D3DSurface = d3dSurface;
            if (dsvs != null)
                foreach (var dsv in dsvs)
                    dsv.D3DSurface = d3dSurface;
        }

        public override void SetSubresourceData(int level, SubresourceData newData)
        {
            throw new NotSupportedException("Setting newData of render target and depth-stencil resources is not supported by the D3D9 implementation of Behodler");
        }

        protected override Surface GetSurfaceLevel(int arrayIndex, int mipLevel)
        {
            if (arrayIndex != 0)
                throw new ArgumentOutOfRangeException("arrayIndex");
            if (mipLevel != 0)
                throw new ArgumentOutOfRangeException("mipLevel");
            return d3dSurface;
        }

        protected override BaseTexture GetTexture()
        {
            throw new InvalidOperationException("Trying to get a d3dTexture from a resource that does not have one");
        }
    }
}
