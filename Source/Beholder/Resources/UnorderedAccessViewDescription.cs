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
using Beholder.Utility;

namespace Beholder.Resources
{
    /// <summary>
    /// Describes an unordered access view.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct UnorderedAccessViewDescription
    {
        /// <summary>
        /// Type of the resource to be viewed.
        /// </summary>
        public UnorderedAccessViewDimension Dimension;

        /// <summary>
        /// Data format ID.
        /// </summary>
        public int FormatID;

        /// <summary>
        /// Index of the first buffer element to be used by the view.
        /// Only used for buffers.
        /// </summary>
        public int FirstElement;

        /// <summary>
        /// Number of buffer elements in the resource.
        /// For structured buffers, this is the number of structures in the buffer.
        /// Only used for buffers.
        /// </summary>
        public int ElementCount;

        /// <summary>
        /// View options for the buffer resource.
        /// Only used for buffers (obviously).
        /// </summary>
        public UnorderedAccessViewBufferFlags BufferFlags;

        /// <summary>
        /// Index of the mip level to be used by the view.
        /// Not used for buffers.
        /// </summary>
        public int MipSlice;

        /// <summary>
        /// Index of the first texture to be used by the view in an array of textures.
        /// Only used for texture arrays.
        /// </summary>
        public int FirstArraySlice;

        /// <summary>
        /// Number of textures in the array to be used by the view.
        /// Only used for texture arrays.
        /// </summary>
        public int ArraySize;

        /// <summary>
        /// First depth level to be used by the view.
        /// Only used by 3D textures.
        /// </summary>
        public int FirstDepthSlice;

        /// <summary>
        /// Number of depth levels to be used by the view, starting from FirstDepthSlice.
        /// Only used by 3D textures.
        /// </summary>
        public int DepthSliceCount;

        public static bool Equals(ref UnorderedAccessViewDescription desc1, ref UnorderedAccessViewDescription desc2)
        {
            switch (desc1.Dimension)
            {
                case UnorderedAccessViewDimension.Unknown:
                    return desc2.Dimension == UnorderedAccessViewDimension.Unknown;
                case UnorderedAccessViewDimension.Buffer:
                    return desc2.Dimension == UnorderedAccessViewDimension.Buffer &&
                        desc1.FormatID == desc2.FormatID &&
                        desc1.FirstElement == desc2.FirstElement &&
                        desc1.ElementCount == desc2.ElementCount &&
                        desc1.BufferFlags == desc2.BufferFlags;
                case UnorderedAccessViewDimension.Texture1D:
                    return desc2.Dimension == UnorderedAccessViewDimension.Texture1D &&
                        desc1.FormatID == desc2.FormatID &&
                        desc1.MipSlice == desc2.MipSlice;
                case UnorderedAccessViewDimension.Texture1DArray:
                    return desc2.Dimension == UnorderedAccessViewDimension.Texture1DArray &&
                        desc1.FormatID == desc2.FormatID &&
                        desc1.MipSlice == desc2.MipSlice &&
                        desc1.FirstArraySlice == desc2.FirstArraySlice &&
                        desc1.ArraySize == desc2.ArraySize;
                case UnorderedAccessViewDimension.Texture2D:
                    return desc2.Dimension == UnorderedAccessViewDimension.Texture2D &&
                        desc1.FormatID == desc2.FormatID &&
                        desc1.MipSlice == desc2.MipSlice;
                case UnorderedAccessViewDimension.Texture2DArray:
                    return desc2.Dimension == UnorderedAccessViewDimension.Texture2DArray &&
                        desc1.FormatID == desc2.FormatID &&
                        desc1.MipSlice == desc2.MipSlice &&
                        desc1.FirstArraySlice == desc2.FirstArraySlice &&
                        desc1.ArraySize == desc2.ArraySize;
                case UnorderedAccessViewDimension.Texture3D:
                    return desc2.Dimension == UnorderedAccessViewDimension.Texture3D &&
                        desc1.FormatID == desc2.FormatID &&
                        desc1.MipSlice == desc2.MipSlice &&
                        desc1.FirstDepthSlice == desc2.FirstDepthSlice &&
                        desc1.DepthSliceCount == desc2.DepthSliceCount;
                default: throw new ShouldNeverHappenException();
            }
        }

        #region Buffer
        public static UnorderedAccessViewDescription CreateForBuffer(int formatID, int firstElement, int elementCount, UnorderedAccessViewBufferFlags flags)
        {
            return new UnorderedAccessViewDescription
            {
                Dimension = UnorderedAccessViewDimension.Buffer,
                FormatID = formatID,
                FirstElement = firstElement,
                ElementCount = elementCount,
                BufferFlags = flags
            };
        }
        public static void CreateForBuffer(int formatID, int firstElement, int elementCount, UnorderedAccessViewBufferFlags flags, out UnorderedAccessViewDescription desc)
        {
            desc = new UnorderedAccessViewDescription
            {
                Dimension = UnorderedAccessViewDimension.Buffer,
                FormatID = formatID,
                FirstElement = firstElement,
                ElementCount = elementCount,
                BufferFlags = flags
            };
        }
        #endregion

        #region Texture1D
        public static UnorderedAccessViewDescription CreateForTexture1D(int formatID, int mipSlice)
        {
            return new UnorderedAccessViewDescription
            {
                Dimension = UnorderedAccessViewDimension.Texture1D,
                FormatID = formatID,
                MipSlice = mipSlice
            };
        }
        public static void CreateForTexture1D(int formatID, int mipSlice, out UnorderedAccessViewDescription desc)
        {
            desc = new UnorderedAccessViewDescription
            {
                Dimension = UnorderedAccessViewDimension.Texture1D,
                FormatID = formatID,
                MipSlice = mipSlice
            };
        }
        #endregion

        #region Texture1DArray
        public static UnorderedAccessViewDescription CreateForTexture1DArray(int formatID, int mipSlice, int firstArraySlice, int arraySize)
        {
            return new UnorderedAccessViewDescription
            {
                Dimension = UnorderedAccessViewDimension.Texture1DArray,
                FormatID = formatID,
                MipSlice = mipSlice,
                FirstArraySlice = firstArraySlice,
                ArraySize = arraySize
            };
        }
        public static void CreateForTexture1DArray(int formatID, int mipSlice, int firstArraySlice, int arraySize, out UnorderedAccessViewDescription desc)
        {
            desc = new UnorderedAccessViewDescription
            {
                Dimension = UnorderedAccessViewDimension.Texture1DArray,
                FormatID = formatID,
                MipSlice = mipSlice,
                FirstArraySlice = firstArraySlice,
                ArraySize = arraySize
            };
        }
        #endregion

        #region Texture2D
        public static UnorderedAccessViewDescription CreateForTexture2D(int formatID, int mipSlice)
        {
            return new UnorderedAccessViewDescription
            {
                Dimension = UnorderedAccessViewDimension.Texture2D,
                FormatID = formatID,
                MipSlice = mipSlice
            };
        }
        public static void CreateForTexture2D(int formatID, int mipSlice, out UnorderedAccessViewDescription desc)
        {
            desc = new UnorderedAccessViewDescription
            {
                Dimension = UnorderedAccessViewDimension.Texture2D,
                FormatID = formatID,
                MipSlice = mipSlice
            };
        }
        #endregion

        #region Texture2DArray
        public static UnorderedAccessViewDescription CreateForTexture2DArray(int formatID, int mipSlice, int firstArraySlice, int arraySize)
        {
            return new UnorderedAccessViewDescription
            {
                Dimension = UnorderedAccessViewDimension.Texture2DArray,
                FormatID = formatID,
                MipSlice = mipSlice,
                FirstArraySlice = firstArraySlice,
                ArraySize = arraySize
            };
        }
        public static void CreateForTexture2DArray(int formatID, int mipSlice, int firstArraySlice, int arraySize, out UnorderedAccessViewDescription desc)
        {
            desc = new UnorderedAccessViewDescription
            {
                Dimension = UnorderedAccessViewDimension.Texture2DArray,
                FormatID = formatID,
                MipSlice = mipSlice,
                FirstArraySlice = firstArraySlice,
                ArraySize = arraySize
            };
        }
        #endregion

        #region Texture3D
        public static UnorderedAccessViewDescription CreateForTexture3D(int formatID, int mipSlice, int firstDepthSlice, int depthSliceCount)
        {
            return new UnorderedAccessViewDescription
            {
                Dimension = UnorderedAccessViewDimension.Texture3D,
                FormatID = formatID,
                MipSlice = mipSlice,
                FirstDepthSlice = firstDepthSlice,
                DepthSliceCount = depthSliceCount
            };
        }
        public static void CreateForTexture3D(int formatID, int mipSlice, int firstDepthSlice, int depthSliceCount, out UnorderedAccessViewDescription desc)
        {
            desc = new UnorderedAccessViewDescription
            {
                Dimension = UnorderedAccessViewDimension.Texture3D,
                FormatID = formatID,
                MipSlice = mipSlice,
                FirstDepthSlice = firstDepthSlice,
                DepthSliceCount = depthSliceCount
            };
        }
        #endregion
    }
}
