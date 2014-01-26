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
using Beholder.Platform;
using Beholder.Resources;
using SharpDX;
using SharpDX.Direct3D9;
using Usage = SharpDX.Direct3D9.Usage;

namespace Beholder.Libraries.SharpDX9.Resources
{
    sealed class CTextureTextureCube : CTextureTexture<CubeTexture>
    {
        public CTextureTextureCube(ICDevice device, Texture1DDescription description, SubresourceData[] initialData, Action<CTextureTextureCube> onRelease)
            : base(device, description, initialData, t => onRelease((CTextureTextureCube)t))
        {
            if (!description.MiscFlags.HasFlag(MiscFlags.TextureCube))
                throw new InvalidOperationException("Trying to create a CTextureCube with a desciption that is missing the TextureCube misc flag");
            if (description.ArraySize != 6)
                throw new NotSupportedException("ArraySize of a D3D9 Cube Texture must be exactly 6");
            if (description.Width != 1)
                throw new NotSupportedException("Non-square cube textures are not supported by D3D9");
        }

        public CTextureTextureCube(ICDevice device, Texture2DDescription description, SubresourceData[] initialData, Action<CTextureTextureCube> onRelease)
            : base(device, description, initialData, t => onRelease((CTextureTextureCube)t))
        {
            if (!description.MiscFlags.HasFlag(MiscFlags.TextureCube))
                throw new InvalidOperationException("Trying to create a CTextureCube with a desciption that is missing the TextureCube misc flag");
            if (description.ArraySize != 6)
                throw new NotSupportedException("ArraySize of a D3D9 Cube Texture must be exactly 6");
            if (description.Width != description.Height)
                throw new NotSupportedException("Non-square cube textures are not supported by D3D9");
        }

        private CTextureTextureCube(ICDevice device, CubeTexture d3dTexture, bool isDefaultPool, Texture2DDescription description, ResourceDimension dimension, Action<CTextureTextureCube> onRelease)
            : base(device, d3dTexture, isDefaultPool, description, dimension, t => onRelease((CTextureTextureCube)t))
        {
            
        }

        protected override CubeTexture CreateD3DTexture(Device d3dDevice, int width, int height, int mipLevels, Usage usage, Format format, Pool pool)
        {
            return new CubeTexture(d3dDevice, width, mipLevels, usage, format, pool);
        }

        protected override Surface GetSurfaceLevel(CubeTexture d3dTexture, int arrayIndex, int mipLevel)
        {
            return d3dTexture.GetCubeMapSurface((CubeMapFace)arrayIndex, mipLevel);
        }

        protected override DataRectangle LockRectangle(CubeTexture d3dTexture, int subresourceIndex, LockFlags lockFlags)
        {
            return d3dTexture.LockRectangle((CubeMapFace)(subresourceIndex / desc.MipLevels), subresourceIndex % desc.MipLevels, lockFlags);
        }

        protected override void UnlockRectangle(CubeTexture d3dTexture, int subresourceIndex)
        {
            d3dTexture.UnlockRectangle((CubeMapFace)(subresourceIndex / desc.MipLevels), subresourceIndex % desc.MipLevels);
        }

        public static CTextureTextureCube FromFile(ICDevice device, IFileSystem fileSystem, string fileName, ResourceDimension dimension, Action<CTextureTexture<CubeTexture>> onRelease)
        {
            var d3dTexture = CubeTexture.FromMemory(device.D3DDevice, fileSystem.ReadBinary(fileName));
            var d3dDesc = d3dTexture.GetLevelDescription(0);
            var bDesc = new Texture2DDescription
            {
                Width = d3dDesc.Width,
                Height = d3dDesc.Height,
                MipLevels = d3dTexture.LevelCount,
                ArraySize = 6,
                FormatID = (int)d3dDesc.Format,
                Sampling = CtBeholder.Sampling(d3dDesc.MultiSampleType, d3dDesc.MultiSampleQuality),
                Usage = Beholder.Resources.Usage.Immutable,
                BindFlags = BindFlags.ShaderResource,
                MiscFlags = MiscFlags.TextureCube,
                ExtraFlags = ExtraFlags.None
            };
            return new CTextureTextureCube(device, d3dTexture, false, bDesc, dimension, onRelease);
        }
    }
}