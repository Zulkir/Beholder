﻿/*
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
using ObjectGL;
using ObjectGL.GL42;

namespace Beholder.Libraries.ObjectGL4.Resources
{
    class CTexture2DCubemap : CTexture2DBase<TextureCubemap>
    {
        public CTexture2DCubemap(ICDevice device, ref Texture2DDescription desc, SubresourceData[] initialData, Action<CTexture2DCubemap> onRelease)
            : base(device, ref desc, initialData, t => onRelease((CTexture2DCubemap)t))
        {
            if (desc.Width != desc.Height)
                throw new NotSupportedException("Non-square cubemaps are not supported by OpenGL");
            if (desc.ArraySize != 6)
                throw new InvalidOperationException("Array size of CTexture2DCubemap must be exactly 6");
        }

        protected override void CreateNative()
        {
            glResource = new TextureCubemap(device.GetCurrentContext(), desc.Width, desc.MipLevels, (Format)desc.FormatID);
        }

        protected override void SetSubresourceData(Context glContext, int arrayIndex, int mipLevel, SubresourceData data)
        {
            var glFormat = (Format)desc.FormatID;
            if (!LocalHelpers.IsFormatCompressed(glFormat))
            {
                glResource.SetData(glContext, CtObjectGL.CubemapFace(arrayIndex), mipLevel, new Data(data.Pointer, data.UnpinPointer), 
                    Texture.GetAppropriateFormatColor(glFormat), Texture.GetAppropriateFormatType(glFormat), CtObjectGL.ByteAlignment(data.RowByteAlignment));
            }
            else
            {
                int totalSize = LocalHelpers.CompressedImageSize(glFormat, TextureHelper.MipSize(mipLevel, desc.Width), TextureHelper.MipSize(mipLevel, desc.Height), 1, data.RowByteAlignment);
                glResource.SetData(glContext, CtObjectGL.CubemapFace(arrayIndex), mipLevel, new Data(data.Pointer, data.UnpinPointer), totalSize);
            }
        }
    }
}
