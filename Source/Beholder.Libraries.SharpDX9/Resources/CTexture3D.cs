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
using Beholder.Utility;
using Beholder.Utility.ForImplementations.Resources;
using Beholder.Utility.Helpers;
using SharpDX;
using SharpDX.Direct3D9;
using Usage = SharpDX.Direct3D9.Usage;

namespace Beholder.Libraries.SharpDX9.Resources
{
    sealed class CTexture3D : Texture3DBase<ICDevice>, ICResource, IDefaultPoolResource
    {
        VolumeTexture d3dTexture;
        readonly byte[][] dynamicData;
        readonly int[] dynamicRowByteAlignments;

        public CTexture3D(ICDevice device, Texture3DDescription desc, SubresourceData[] initialData, Action<CTexture3D> onRelease)
            : base(device, ref desc, t => onRelease((CTexture3D)t))
        {
            if (desc.BindFlags != BindFlags.ShaderResource)
                throw new NotSupportedException("Texture3D bind flags other than ShaderResoruce are not supported by D3D9");
            if (desc.Usage == Beholder.Resources.Usage.Dynamic)
            {
                if (!device.D3DDevice.Capabilities.Caps2.HasFlag(Caps2.DynamicTextures))
                    throw new NotSupportedException("Dynamic textures are not supported by your graphics adapter in D3D9");
                dynamicData = new byte[desc.MipLevels][];
                dynamicRowByteAlignments = new int[desc.MipLevels];
            }

            CreateNative();

            if (initialData != null)
            {
                if (desc.BindFlags.HasFlag(BindFlags.RenderTarget) || desc.BindFlags.HasFlag(BindFlags.DepthStencil))
                    throw new NotSupportedException("Initial newData is not supported for depth-stencil resources in D3D9");
                for (int i = 0; i < initialData.Length; i++)
                    SetSubresourceData(i, initialData[i]);
            }
        }

        private CTexture3D(ICDevice device, VolumeTexture d3dTexture, ref Texture3DDescription desc, Action<CTexture3D> onDispose)
            : base(device, ref desc, t => onDispose((CTexture3D)t))
        {
            this.d3dTexture = d3dTexture;
        }

        #region Lifecycle
        void CreateNative()
        {
            Usage usage;
            Pool pool;
            switch (Usage)
            {
                case Beholder.Resources.Usage.Immutable:
                case Beholder.Resources.Usage.Default:
                    usage = SharpDX.Direct3D9.Usage.None;
                    pool = Pool.Managed;
                    break;
                case Beholder.Resources.Usage.Dynamic:
                    usage = SharpDX.Direct3D9.Usage.Dynamic;
                    pool = Pool.Default;
                    break;
                case Beholder.Resources.Usage.Staging:
                    usage = SharpDX.Direct3D9.Usage.None;
                    pool = Pool.SystemMemory;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            d3dTexture = new VolumeTexture(device.D3DDevice, desc.Width, desc.Height, desc.Depth, desc.MipLevels, usage, (Format)desc.FormatID, pool);
        }

        protected override void DisposeOfNative()
        {
            d3dTexture.Dispose();
        }

        public void OnDeviceLost()
        {
            if (desc.Usage != Beholder.Resources.Usage.Dynamic) return;
            Dispose();
        }

        public void OnDeviceReset()
        {
            if (desc.Usage != Beholder.Resources.Usage.Dynamic) return;

            CreateNative();

            if (desc.Usage == Beholder.Resources.Usage.Dynamic)
            {
                for (int i = 0; i < desc.MipLevels; i++)
                {
                    var data = new SubresourceData(dynamicData[i], 0, dynamicRowByteAlignments[i]);
                    SetSubresourceData(i, data);
                }
            }    
        }
        #endregion

        #region Set Subresource Data
        public void SetSubresourceData(int level, SubresourceData newData)
        {
            int mipWidth = TextureHelper.MipSize(level, desc.Width);
            int mipHeight = TextureHelper.MipSize(level, desc.Height);
            int mipDepth = TextureHelper.MipSize(level, desc.Depth);

            int rowPitch, slicePitch, totalSize;
            LocalHelpers.GetTextureDataSettingParameters((Format)desc.FormatID, mipWidth, mipHeight, mipDepth,
                newData.RowByteAlignment, out rowPitch, out slicePitch, out totalSize);

            var dataBox = d3dTexture.LockBox(level, desc.Usage == Beholder.Resources.Usage.Dynamic ? LockFlags.Discard : LockFlags.None);
            //var stream = dataBox.Data;
            if (dataBox.RowPitch == rowPitch && dataBox.SlicePitch == slicePitch)
            {
                Utilities.CopyMemory(dataBox.DataPointer, newData.Pointer, totalSize);
            }
            else if (dataBox.RowPitch == rowPitch)
            {
                for (int z = 0; z < mipDepth; z++)
                {
                    var srcPointer = newData.Pointer + z * slicePitch;
                    var dstPointer = dataBox.DataPointer + z * dataBox.SlicePitch;
                    Utilities.CopyMemory(dstPointer, srcPointer, slicePitch);
                }
            }
            else
            {
                for (int z = 0; z < mipDepth; z++)
                {
                    for (int y = 0; y < slicePitch / rowPitch; y++) // Differs from mipHeight for compressed formats
                    {
                        var srcPointer = newData.Pointer + z * slicePitch + y * rowPitch;
                        var dstPointer = dataBox.DataPointer + z * dataBox.SlicePitch + y * dataBox.RowPitch;
                        Utilities.CopyMemory(dstPointer, srcPointer, rowPitch);
                    }
                }
            }
            d3dTexture.UnlockBox(level);

            if (desc.Usage == Beholder.Resources.Usage.Dynamic)
            {
                if (dynamicData[level] == null || dynamicData[level].Length < totalSize)
                    dynamicData[level] = new byte[totalSize];
                Marshal.Copy(newData.Pointer, dynamicData[level], 0, totalSize);
                dynamicRowByteAlignments[level] = newData.RowByteAlignment;
            }

            newData.UnpinPointer();
        }
        #endregion

        #region Eye-specific properties
        public bool IsDefaultPool { get { return desc.Usage == Beholder.Resources.Usage.Dynamic; } }
        #endregion

        #region For Texture3DBase
        protected override void DisposeRtv(IRenderTargetView view) { }
        protected override void DisposeSrv(IShaderResourceView view) { }
        protected override void DisposeUav(IUnorderedAccessView view) { }

        protected override bool DescEqualsRtv(IRenderTargetView view, ref RenderTargetViewDescription viewDescription) { return ((CRenderTargetView) view).DescEquals(ref viewDescription); }
        protected override bool DescEqualsSrv(IShaderResourceView view, ref ShaderResourceViewDescription viewDescription) { return ((CShaderResourceView) view).DescEquals(ref viewDescription); }
        protected override bool DescEqualsUav(IUnorderedAccessView view, ref UnorderedAccessViewDescription viewDescription) { throw new ShouldNeverHappenException(); }

        protected override IRenderTargetView CreateRtv(ref RenderTargetViewDescription viewDescription) { throw new ResourceBindingException(); }
        protected override IShaderResourceView CreateSrv(ref ShaderResourceViewDescription viewDescription) { return new CShaderResourceView(this, d3dTexture, ref viewDescription); }
        protected override IUnorderedAccessView CreateUav(ref UnorderedAccessViewDescription viewDescription) { throw new ResourceBindingException(); }
        #endregion

        public static CTexture3D FromFile(ICDevice device, IFileSystem fileSystem, string fileName, Action<CTexture3D> onRelease)
        {
            var d3dTexture = VolumeTexture.FromMemory(device.D3DDevice, fileSystem.ReadBinary(fileName));
            var d3dDesc = d3dTexture.GetLevelDescription(0);
            var bDesc = new Texture3DDescription
            {
                Width = d3dDesc.Width,
                Height = d3dDesc.Height,
                Depth = d3dDesc.Depth,
                MipLevels = d3dTexture.LevelCount,
                FormatID = (int)d3dDesc.Format,
                Usage = Beholder.Resources.Usage.Immutable,
                BindFlags =  BindFlags.ShaderResource,
                MiscFlags = MiscFlags.None,
                ExtraFlags = ExtraFlags.None
            };
            return new CTexture3D(device, d3dTexture, ref bDesc, onRelease);
        }
    }
}