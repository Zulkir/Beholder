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
    class UnorderedAccessView : ResourceView<IUnorderedAccessView>, IUnorderedAccessView
    {
        public UnorderedAccessView(IUnorderedAccessView real) : base(real) { }

        public UnorderedAccessViewDimension Dimension { get { CheckResourceNotDisposed(); return Real.Dimension; } }
        public int FormatID { get { CheckResourceNotDisposed(); return Real.FormatID; } }
        public int FirstElement { get { CheckResourceNotDisposed(); return Real.FirstElement; } }
        public int ElementCount { get { CheckResourceNotDisposed(); return Real.ElementCount; } }
        public UnorderedAccessViewBufferFlags BufferFlags { get { CheckResourceNotDisposed(); return Real.BufferFlags; } }
        public int MipSlice { get { CheckResourceNotDisposed(); return Real.MipSlice; } }
        public int FirstArraySlice { get { CheckResourceNotDisposed(); return Real.FirstArraySlice; } }
        public int ArraySize { get { CheckResourceNotDisposed(); return Real.ArraySize; } }
        public int FirstDepthSlice { get { CheckResourceNotDisposed(); return Real.FirstDepthSlice; } }
        public int DepthSliceCount { get { CheckResourceNotDisposed(); return Real.DepthSliceCount; } }
        public void GetDesc(out UnorderedAccessViewDescription description) { CheckResourceNotDisposed(); Real.GetDesc(out description); }

        public static void ValidateBuffer(IBuffer buffer, int formatID, int firstElement, int elementCount, UnorderedAccessViewBufferFlags flags)
        {
            ValidateFormat(buffer, formatID, FormatSupport.Buffer, "buffer unordered access resource");
            ValidateBufferPart(buffer, formatID, firstElement, elementCount, flags);
        }

        public static void ValidateTexture1D(ITexture1D texture1D, int formatID, int mipSlice)
        {
            ValidateFormat(texture1D, formatID, FormatSupport.Texture1D, "texture1D unordered access resource");
            ValidateMipSlice(texture1D, mipSlice);
        }

        public static void ValidateTexture1DArray(ITexture1D texture1D, int formatID, int mipSlice, int firstArraySlice, int arraySize)
        {
            ValidateFormat(texture1D, formatID, FormatSupport.Texture1D, "texture1D unordered access resource");
            ValidateMipSlice(texture1D, mipSlice);
            ValidateTextureArrayPart(texture1D.ArraySize, firstArraySlice, arraySize);
        }

        public static void ValidateTexture2D(ITexture2D texture2D, int formatID, int mipSlice)
        {
            ValidateFormat(texture2D, formatID, FormatSupport.Texture2D, "texture2D unordered access resource");
            ValidateMipSlice(texture2D, mipSlice);
        }

        public static void ValidateTexture2DArray(ITexture2D texture2D, int formatID, int mipSlice, int firstArraySlice, int arraySize)
        {
            ValidateFormat(texture2D, formatID, FormatSupport.Texture2D, "texture2D unordered access resource");
            ValidateMipSlice(texture2D, mipSlice);
            ValidateTextureArrayPart(texture2D.ArraySize, firstArraySlice, arraySize);
        }

        public static void ValidateTexture3D(ITexture3D texture3D, int formatID, int mipSlice, int firstDepthSlice, int depthSliceCount)
        {
            ValidateFormat(texture3D, formatID, FormatSupport.Texture3D, "texture3D unordered access resource");
            ValidateMipSlice(texture3D, mipSlice);
            ValidateTexture3DPart(texture3D, firstDepthSlice, depthSliceCount);
        }

        static void ValidateFormat(IResource resource, int formatID, FormatSupport support, string usageDescription)
        {
            var adapter = resource.Device.Adapter;
            if (!adapter.GetSupportedFormats(support).Any(fi => fi.ID == formatID))
                throw new ArgumentException("Given format is cannot be used as a " + usageDescription + " format");
            if (resource is ITexture && !adapter.CheckViewFormatCompatability(((ITexture)resource).FormatID, formatID))
                throw new ArgumentException("Given format is not compatible with the resource texture format");
        }

        static void ValidateBufferPart(IBuffer buffer, int formatID, int firstElement, int elementCount, UnorderedAccessViewBufferFlags flags)
        {
            Check.NotNegative(firstElement, "firstElement");
            Check.Positive(elementCount, "elementCount");

            int elementSizeInBytes = buffer.Device.Adapter.GetFormatInfo(formatID).TotalBits / 8;
            int bufferSizeInElements = buffer.SizeInBytes / elementSizeInBytes;
            if (firstElement + elementCount > bufferSizeInElements)
                throw new ArgumentException(string.Format(
                    "firstElement + elementCount = {0}, but the buffer is only {1} elements long", firstElement + elementCount, bufferSizeInElements));
            if (flags != UnorderedAccessViewBufferFlags.None)
            {
                // todo: check format supports raw apeend or counters
            }
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

        static void ValidateTexture3DPart(ITexture3D texture3D, int firstDepthSlice, int depthSliceCount)
        {
            Check.NotNegative(firstDepthSlice, "firstDepthSlice");
            Check.Positive(depthSliceCount, "depthSliceCount");
            if (firstDepthSlice + depthSliceCount > texture3D.Depth)
                throw new ArgumentException(string.Format(
                    "firstDepthSlice + depthSliceCount = {0}, but the texture depth is only {1}", firstDepthSlice + depthSliceCount, texture3D.Depth));
        }
    }
}
