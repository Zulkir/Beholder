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
using System.Runtime.InteropServices;
using Beholder.Platform;
using Beholder.Resources;
using Beholder.Utility.Helpers;
using SharpDX;
using SharpDX.Direct3D9;
using Usage = SharpDX.Direct3D9.Usage;

namespace Beholder.Libraries.SharpDX9.Resources
{
    abstract class CTextureTexture<TD3DTexture> : CTexture
        where TD3DTexture : BaseTexture
    {
        readonly Caps2 d3dCaps2;
        TD3DTexture d3dTexture;
        readonly byte[][] dynamicData;
        readonly int[] dynamicRowByteAlignments;
        bool isDefaultPool;

        public bool IsDefaultPool { get { return isDefaultPool; } }

        private CTextureTexture(ICDevice device, ResourceDimension dimension, Texture2DDescription description, SubresourceData[] initialData, Action<CTexture> onDispose)
            : base(device, dimension, description, onDispose)
        {
            this.d3dCaps2 = device.D3DDevice.Capabilities.Caps2;

            if (desc.Usage == Beholder.Resources.Usage.Dynamic)
            {
                dynamicData = new byte[desc.ArraySize * desc.MipLevels][];
                dynamicRowByteAlignments = new int[desc.ArraySize * desc.MipLevels];
            }
                
            CreateNative();

            if (initialData != null)
            {
                if (desc.BindFlags.HasFlag(BindFlags.RenderTarget) || desc.BindFlags.HasFlag(BindFlags.DepthStencil))
                    throw new NotSupportedException("Initial newData is not supported for render target and depth-stencil resources in D3D9");
                for (int i = 0; i < initialData.Length; i++)
                    SetSubresourceData(i, initialData[i]);
            }
        }

        protected CTextureTexture(ICDevice device, Texture1DDescription description, SubresourceData[] initialData, Action<CTexture> onRelease)
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

        protected CTextureTexture(ICDevice device, Texture2DDescription description, SubresourceData[] initialData, Action<CTexture> onRelease)
            : this(device, ResourceDimension.Texture2D, description, initialData, onRelease)
        { }

        protected CTextureTexture(ICDevice device, TD3DTexture d3dTexture, bool isDefaultPool, Texture2DDescription description, ResourceDimension dimension, Action<CTexture> onDispose)
            : base(device, dimension, description, onDispose)
        {
            this.d3dCaps2 = device.D3DDevice.Capabilities.Caps2;
            this.d3dTexture = d3dTexture;
            this.isDefaultPool = isDefaultPool;

            if (dimension == ResourceDimension.Texture1D && desc.Height != 1)
                throw new InvalidOperationException("One-dimensional textures must have a height of 1");
        }

        #region Abstract
        protected abstract TD3DTexture CreateD3DTexture(Device d3dDevice, int width, int height, int mipLevels, Usage usage, Format format, Pool pool);
        protected abstract Surface GetSurfaceLevel(TD3DTexture d3dTexture, int arrayIndex, int mipLevel);
        protected abstract DataRectangle LockRectangle(TD3DTexture d3dTexture, int subresourceIndex, LockFlags lockFlags);
        protected abstract void UnlockRectangle(TD3DTexture d3dTexture, int subresourceIndex);
        #endregion

        void CreateNative()
        {
            Usage usage; 
            Pool pool;
            GetUsageAndPool(out usage, out pool);
            isDefaultPool = pool == Pool.Default;
            int mipLevels;
            if (usage.HasFlag(Usage.AutoGenerateMipMap))
            {
                if (desc.MipLevels == 1)
                    mipLevels = 1;
                else if (desc.MipLevels == TextureHelper.MipLevels(desc.Width, desc.Height, 1))
                    mipLevels = 0;
                else
                    throw new NotSupportedException("Only textures with mip level count of maximum or 1 can have autogenerated mip levels in D3D9");
            }
            else
            {
                mipLevels = desc.MipLevels;
            }
            d3dTexture = CreateD3DTexture(device.D3DDevice, desc.Width, desc.Height, mipLevels, usage, (Format)desc.FormatID, pool);
        }

        void GetUsageAndPool(out Usage usage, out Pool pool)
        {
            switch (desc.Usage)
            {
                case Beholder.Resources.Usage.Immutable:
                case Beholder.Resources.Usage.Default:
                    pool = Pool.Managed;
                    break;
                case Beholder.Resources.Usage.Dynamic:
                    pool = Pool.Default;
                    break;
                case Beholder.Resources.Usage.Staging:
                    pool = Pool.SystemMemory;
                    break;
                default: throw new ArgumentOutOfRangeException("usage");
            }

            usage = Usage.None;
            if (desc.MiscFlags.HasFlag(MiscFlags.GenerateMips))
            {
                if (desc.BindFlags.HasFlag(BindFlags.DepthStencil))
                    throw new NotSupportedException("Autogeneration of mipmaps is not supported for depth-stencil textures in D3D9");
                if (desc.Usage == Beholder.Resources.Usage.Staging)
                    throw new NotSupportedException("Autogeneration of mipmaps is not supported for system-memory resources in D3D9");
                usage |= Usage.AutoGenerateMipMap;
            }
            if (desc.Usage == Beholder.Resources.Usage.Dynamic)
            {
                if (!d3dCaps2.HasFlag(Caps2.DynamicTextures))
                    throw new NotSupportedException("Dynamic textures are not supported by your graphics adapter in D3D9");
                usage |= Usage.Dynamic;
            }
            else if (desc.BindFlags.HasFlag(BindFlags.DepthStencil))
            {
                if (desc.Usage == Beholder.Resources.Usage.Staging)
                    throw new NotSupportedException("Depth-stencil textures cannot lie in system memory");
                pool = Pool.Default;
                usage |= Usage.DepthStencil;
            }
            else if (desc.BindFlags.HasFlag(BindFlags.RenderTarget))
            {
                if (desc.Usage == Beholder.Resources.Usage.Staging)
                    throw new NotSupportedException("Render target textures cannot lie in system memory");
                pool = Pool.Default;
                usage |= Usage.RenderTarget;
            }
            // todo: make sure about this
            //if (desc.Usage != BUsage.Staging)
            //{
            //    usage |= D3DUsage.WriteOnly;
            //}
        }

        public override void DisposeConcrete()
        {
            if (rtvs != null)
                foreach (var rtv in rtvs)
                    rtv.D3DSurface.Dispose();
            if (dsvs != null)
                foreach (var dsv in dsvs)
                    dsv.D3DSurface.Dispose();

            if (d3dTexture != null) { d3dTexture.Dispose(); d3dTexture = null; }
        }

        public override void OnDeviceLost()
        {
            if (!IsDefaultPool) 
                return;
            DisposeInternal();
        }

        public override void OnDeviceReset()
        {
            if (!IsDefaultPool) 
                return;

            CreateNative();

            if (rtvs != null)
                foreach (var rtv in rtvs)
                    rtv.D3DSurface = GetSurfaceLevel(d3dTexture, rtv.FirstArraySlice, rtv.MipSlice);
            if (dsvs != null)
                foreach (var dsv in dsvs)
                    dsv.D3DSurface = GetSurfaceLevel(d3dTexture, dsv.FirstArraySlice, dsv.MipSlice);
            if (srvs != null)
                foreach (var srv in srvs)
                    srv.D3DBaseTexture = d3dTexture;

            if (desc.Usage == Beholder.Resources.Usage.Dynamic)
            {
                for (int i = 0; i < dynamicData.Length; i++)
                {
                    var data = new SubresourceData(dynamicData[i], 0, dynamicRowByteAlignments[i]);
                    SetSubresourceData(i, data);
                }
            }
        }

        public override void SetSubresourceData(int level, SubresourceData newData)
        {
            if (desc.BindFlags != BindFlags.ShaderResource)
                throw new NotSupportedException("Setting the newData of the render target or depth-stencil resource is not supported by the D3D9 implementation of Beholder");

            int mipWidth = TextureHelper.MipSize(level, desc.Width);
            int mipHeight = TextureHelper.MipSize(level, desc.Height);

            int rowPitch, slicePitch, totalSize;
            LocalHelpers.GetTextureDataSettingParameters((Format)desc.FormatID, mipWidth, mipHeight, 1,
                newData.RowByteAlignment, out rowPitch, out slicePitch, out totalSize);

            var dataRectangle = LockRectangle(d3dTexture, level, desc.Usage == Beholder.Resources.Usage.Dynamic ? LockFlags.Discard : LockFlags.None);

            if (dataRectangle.Pitch == rowPitch)
            {
                Utilities.CopyMemory(dataRectangle.DataPointer, newData.Pointer, totalSize);
            }
            else
            {
                for (int y = 0; y < slicePitch / rowPitch; y++) // Differs from mipHeight for compressed formats
                {
                    var dstPointer = dataRectangle.DataPointer + y * dataRectangle.Pitch;
                    var srcPointer = newData.Pointer + y * rowPitch;
                    Utilities.CopyMemory(dstPointer, srcPointer, rowPitch);
                }
            }
            UnlockRectangle(d3dTexture, level);

            if (desc.Usage == Beholder.Resources.Usage.Dynamic)
            {
                if (dynamicData[level] == null || dynamicData[level].Length < totalSize)
                    dynamicData[level] = new byte[totalSize];
                Marshal.Copy(newData.Pointer, dynamicData[level], 0, totalSize);
                dynamicRowByteAlignments[level] = newData.RowByteAlignment;
            }

            newData.UnpinPointer();
        }

        protected override Surface GetSurfaceLevel(int arrayIndex, int mipLevel)
        {
            return GetSurfaceLevel(d3dTexture, arrayIndex, mipLevel);
        }

        protected override BaseTexture GetTexture()
        {
            return d3dTexture;
        }
    }
}