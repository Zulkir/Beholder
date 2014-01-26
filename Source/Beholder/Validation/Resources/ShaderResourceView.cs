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
using System.Linq;
using Beholder.Platform;
using Beholder.Resources;

namespace Beholder.Validation.Resources
{
    class ShaderResourceView : ResourceView<IShaderResourceView>, IShaderResourceView
    {
        public ShaderResourceView(IShaderResourceView real) : base(real) { }

        public ShaderResourceViewDimension Dimension { get { CheckResourceNotDisposed(); return Real.Dimension; } }
        public int FormatID { get { CheckResourceNotDisposed(); return Real.FormatID; } }
        public int FirstElement { get { CheckResourceNotDisposed(); return Real.FirstElement; } }
        public int ElementCount { get { CheckResourceNotDisposed(); return Real.ElementCount; } }
        public ShaderResourceViewExtendedBufferFlags ExtendedBufferFlags { get { CheckResourceNotDisposed(); return Real.ExtendedBufferFlags; } }
        public int MostDetailedMip { get { CheckResourceNotDisposed(); return Real.MostDetailedMip; } }
        public int MipLevels { get { CheckResourceNotDisposed(); return Real.MipLevels; } }
        public int FirstArraySlice { get { CheckResourceNotDisposed(); return Real.FirstArraySlice; } }
        public int ArraySize { get { CheckResourceNotDisposed(); return Real.ArraySize; } }
        public void GetDescription(out ShaderResourceViewDescription description) { CheckResourceNotDisposed(); Real.GetDescription(out description); }

        public static void ValidateBuffer(IBuffer buffer, int formatID, int firstElement, int elementCount)
        {
            ValidateFormat(buffer, formatID, FormatSupport.Buffer, "buffer shader resource");
            ValidateBufferPart(buffer, formatID, firstElement, elementCount, ShaderResourceViewExtendedBufferFlags.None);
        }

        public static void ValidateTexture1D(ITexture1D texture1D, int formatID, int mostDetailedMip, int mipLevels)
        {
            ValidateFormat(texture1D, formatID, FormatSupport.Texture1D, "texture1D shader resource");
            ValidateMip(texture1D, mostDetailedMip, mipLevels);
        }

        public static void ValidateTexture1DArray(ITexture1D texture1D, int formatID, int mostDetailedMip, int mipLevels, int firstArraySlice, int arraySize)
        {
            ValidateFormat(texture1D, formatID, FormatSupport.Texture1D, "texture1D shader resource");
            ValidateMip(texture1D, mostDetailedMip, mipLevels);
            ValidateTextureArrayPart(texture1D.ArraySize, firstArraySlice, arraySize);
        }

        public static void ValidateTexture2D(ITexture2D texture2D, int formatID, int mostDetailedMip, int mipLevels)
        {
            ValidateFormat(texture2D, formatID, FormatSupport.Texture2D, "texture2D shader resource");
            ValidateMip(texture2D, mostDetailedMip, mipLevels);
        }

        public static void ValidateTexture2DArray(ITexture2D texture2D, int formatID, int mostDetailedMip, int mipLevels, int firstArraySlice, int arraySize)
        {
            ValidateFormat(texture2D, formatID, FormatSupport.Texture2D, "texture2D shader resource");
            ValidateMip(texture2D, mostDetailedMip, mipLevels);
            ValidateTextureArrayPart(texture2D.ArraySize, firstArraySlice, arraySize);
        }

        public static void ValidateTexture2DMultisampled(ITexture2D texture2D, int formatID)
        {
            ValidateFormat(texture2D, formatID, FormatSupport.Texture2D, "texture2D shader resource");
        }

        public static void ValidateTexture2DMultisampledArray(ITexture2D texture2D, int formatID, int firstArraySlice, int arraySize)
        {
            ValidateFormat(texture2D, formatID, FormatSupport.Texture2D, "texture2D shader resource");
            ValidateTextureArrayPart(texture2D.ArraySize, firstArraySlice, arraySize);
        }

        public static void ValidateTexture3D(ITexture3D texture3D, int formatID, int mostDetailedMip, int mipLevels)
        {
            ValidateFormat(texture3D, formatID, FormatSupport.Texture3D, "texture3D shader resource");
            ValidateMip(texture3D, mostDetailedMip, mipLevels);
        }

        public static  void ValidateTextureCube(ITexture2D texture2D, int formatID, int mostDetailedMip, int mipLevels)
        {
            ValidateFormat(texture2D, formatID, FormatSupport.Texture2D | FormatSupport.TextureCube, "cube texture shader resource");
            ValidateMip(texture2D, mostDetailedMip, mipLevels);
        }

        public static void ValidateTextureCubeArray(ITexture2D texture2D, int formatID, int mostDetailedMip, int mipLevels, int firstArraySlice, int arraySize)
        {
            ValidateFormat(texture2D, formatID, FormatSupport.Texture2D | FormatSupport.TextureCube, "cube texture shader resource");
            ValidateMip(texture2D, mostDetailedMip, mipLevels);
            ValidateTextureArrayPart(texture2D.ArraySize, firstArraySlice, arraySize);
            if (arraySize % 6 != 0)
                throw new ArgumentException("array size must divisible by 6 for the cube texture view");
        }

        public static void ValidateBufferEx(IBuffer buffer, int formatID, int firstElement, int elementCount, ShaderResourceViewExtendedBufferFlags flags)
        {
            ValidateFormat(buffer, formatID, 0, "buffer shader resource");
            ValidateBufferPart(buffer, formatID, firstElement, elementCount, flags);
        }

        static void ValidateFormat(IResource resource, int formatID, FormatSupport support, string usageDescription)
        {
            var adapter = resource.Device.Adapter;
            if (!adapter.GetSupportedFormats(support).Any(fi => fi.ID == formatID))
                throw new ArgumentException("Given format is cannot be used as a " + usageDescription + " format");
            if (resource is ITexture && !adapter.CheckViewFormatCompatability(((ITexture)resource).FormatID, formatID))
                throw new ArgumentException("Given format is not compatible with the resource texture format");
        }

        static void ValidateBufferPart(IBuffer buffer, int formatID, int firstElement, int elementCount, ShaderResourceViewExtendedBufferFlags extendedBufferFlags)
        {
            Check.NotNegative(firstElement, "firstElement");
            Check.Positive(elementCount, "elementCount");

            int elementSizeInBytes = buffer.Device.Adapter.GetFormatInfo(formatID).TotalBits / 8;
            int bufferSizeInElements = buffer.SizeInBytes / elementSizeInBytes;
            if (firstElement + elementCount > bufferSizeInElements)
                throw new ArgumentException(string.Format(
                    "firstElement + elementCount = {0}, but the buffer is only {1} elements long", firstElement + elementCount, bufferSizeInElements));
            if (extendedBufferFlags.HasFlag(ShaderResourceViewExtendedBufferFlags.Raw) && !buffer.MiscFlags.HasFlag(MiscFlags.BufferAllowRawViews))
                throw new ArgumentException("Only buffers with MiscFlags.BufferAllowRawViews flag set can be viewed as raw buffers");
        }

        static void ValidateMip(ITexture texture, int mostDetailedMip, int mipLevels)
        {
            Check.NotNegative(mostDetailedMip, "mostDetailedMip");
            Check.Positive(mipLevels, "mipLevels");

            if (mostDetailedMip + mipLevels > texture.MipLevels)
                throw new ArgumentException(string.Format(
                    "mostDetailedMip + mipLevels = {0}, but the texture has only {1} mip levels", mostDetailedMip + mipLevels, texture.MipLevels));
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
