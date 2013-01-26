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
using Beholder.Resources;
using Beholder.Utility.Helpers;
using ObjectGL;
using ObjectGL.GL42;

namespace Beholder.Libraries.ObjectGL4.Resources
{
    sealed class CTexture1D : CTexture1DBase<Texture1D>
    {
        public CTexture1D(ICDevice device, ref Texture1DDescription desc, SubresourceData[] initialData, Action<CTexture1D> onRelease) 
            : base(device, ref desc, initialData, t => onRelease((CTexture1D)t))
        {
        }

        protected override void CreateNative()
        {
            glResource = new Texture1D(device.GetCurrentContext(), desc.Width, desc.MipLevels, (Format)desc.FormatID);
        }

        protected override void SetSubresourceData(Context glContext, int arrayIndex, int mipLevel, SubresourceData data)
        {
            var glFormat = (Format)desc.FormatID;
            if (!LocalHelpers.IsFormatCompressed(glFormat))
            {
                glResource.SetData(glContext, mipLevel, new Data(data.Pointer, data.UnpinPointer),
                    Texture.GetAppropriateFormatColor(glFormat), Texture.GetAppropriateFormatType(glFormat), 
                    CtObjectGL.ByteAlignment(data.RowByteAlignment));
            }
            else
            {
                int totalSize = LocalHelpers.CompressedImageSize(glFormat, TextureHelper.MipSize(mipLevel, desc.Width), 1, 1, data.RowByteAlignment);
                glResource.SetData(glContext, mipLevel, new Data(data.Pointer, data.UnpinPointer), totalSize);
            }
        }
    }
}
