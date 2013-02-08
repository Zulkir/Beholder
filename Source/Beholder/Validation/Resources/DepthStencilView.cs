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
using System.Linq;
using Beholder.Platform;
using Beholder.Resources;

namespace Beholder.Validation.Resources
{
    class DepthStencilView : ResourceView<IDepthStencilView>, IDepthStencilView
    {
        public DepthStencilView(IDepthStencilView real) : base(real) { }

        public int Width { get { return Real.Width; } }
        public int Height { get { return Real.Height; } }
        public DepthStencilViewDimension Dimension { get { CheckResourceNotDisposed(); return Real.Dimension; } }
        public int FormatID { get { CheckResourceNotDisposed(); return Real.FormatID; } }
        public DepthStencilViewFlags Flags { get { CheckResourceNotDisposed(); return Real.Flags; } }
        public int MipSlice { get { CheckResourceNotDisposed(); return Real.MipSlice; } }
        public int FirstArraySlice { get { CheckResourceNotDisposed(); return Real.FirstArraySlice; } }
        public int ArraySize { get { CheckResourceNotDisposed(); return Real.ArraySize; } }
        public void GetDescription(out DepthStencilViewDescription description) { CheckResourceNotDisposed(); Real.GetDescription(out description); }

        public static void ValidateTexture1D(ITexture1D texture1D, int formatID, DepthStencilViewFlags flags, int mipSlice)
        {
            ValidateFormatAndFlags(texture1D, flags, formatID, FormatSupport.DepthStencil | FormatSupport.Texture1D, "texture1D depth-stencil");
            ValidateMipSlice(texture1D, mipSlice);
        }

        public static void ValidateTexture1DArray(ITexture1D texture1D, int formatID, DepthStencilViewFlags flags, int mipSlice, int firstArraySlice, int arraySize)
        {
            ValidateFormatAndFlags(texture1D, flags, formatID, FormatSupport.DepthStencil | FormatSupport.Texture1D, "texture1D depth-stencil");
            ValidateMipSlice(texture1D, mipSlice);
            ValidateTextureArrayPart(texture1D.ArraySize, firstArraySlice, arraySize);
        }

        public static void ValidateTexture2D(ITexture2D texture2D, int formatID, DepthStencilViewFlags flags, int mipSlice)
        {
            ValidateFormatAndFlags(texture2D, flags, formatID, FormatSupport.DepthStencil | FormatSupport.Texture2D, "texture2D depth-stencil");
            ValidateMipSlice(texture2D, mipSlice);
        }

        public static void ValidateTexture2DArray(ITexture2D texture2D, int formatID, DepthStencilViewFlags flags, int mipSlice, int firstArraySlice, int arraySize)
        {
            ValidateFormatAndFlags(texture2D, flags, formatID, FormatSupport.DepthStencil | FormatSupport.Texture2D, "texture2D depth-stencil");
            ValidateMipSlice(texture2D, mipSlice);
            ValidateTextureArrayPart(texture2D.ArraySize, firstArraySlice, arraySize);
        }

        public static void ValidateTexture2DMultisampled(ITexture2D texture2D, int formatID, DepthStencilViewFlags flags)
        {
            ValidateFormatAndFlags(texture2D, flags, formatID, FormatSupport.DepthStencil | FormatSupport.Texture2D | FormatSupport.MultisampleRendertarget, "texture2D multisample depth-stencil");
        }

        public static void ValidateTexture2DMultisampledArray(ITexture2D texture2D, int formatID, DepthStencilViewFlags flags, int firstArraySlice, int arraySize)
        {
            ValidateFormatAndFlags(texture2D, flags, formatID, FormatSupport.DepthStencil | FormatSupport.Texture2D | FormatSupport.MultisampleRendertarget, "texture2D multisample depth-stencil");
            ValidateTextureArrayPart(texture2D.ArraySize, firstArraySlice, arraySize);
        }

        static void ValidateFormatAndFlags(ITexture texture, DepthStencilViewFlags flags, int formatID, FormatSupport support, string usageDescription)
        {
            var adapter = texture.Device.Adapter;
            if (!adapter.GetSupportedFormats(support).Any(fi => fi.ID == formatID))
                throw new ArgumentException("Given format is cannot be used as a " + usageDescription + " format");
            if (!adapter.CheckViewFormatCompatability(texture.FormatID, formatID))
                throw new ArgumentException("Given format is not compatible with the resource texture format");
            if ((int)flags < 0 || (int)flags > 2)
                throw new ArgumentException(string.Format("Unexpected flags value '{0}'", flags));
        }

        static void ValidateMipSlice(ITexture texture, int mipSlice)
        {
            Check.NotNegative(mipSlice, "mipSlice");
            if (mipSlice >= texture.MipLevels)
                throw new ArgumentException(string.Format(
                    "mipSlice = {0}, but the texture has only {1} mip levels", mipSlice, texture.MipLevels));
        }

        static void ValidateTextureArrayPart(int textureArraySize, int firstArraySlice, int arraySize)
        {
            Check.NotNegative(firstArraySlice, "firstArraySlice");
            Check.Positive(arraySize, "arraySize");
            if (firstArraySlice + arraySize > textureArraySize)
                throw new ArgumentException(string.Format(
                    "firstArraySlice + arraySize = {0}, but the texture array has only {1} elements", firstArraySlice + arraySize, textureArraySize));
        }
    }
}
