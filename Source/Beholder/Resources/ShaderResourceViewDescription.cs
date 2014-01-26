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
using System.Runtime.InteropServices;
using Beholder.Utility;

namespace Beholder.Resources
{
    /// <summary>
    /// Describes a shader resource view.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct ShaderResourceViewDescription
    {
        /// <summary>
        /// Type of the resource to be viewed.
        /// </summary>
        public ShaderResourceViewDimension Dimension;

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
        /// Only used for buffers. 
        /// </summary>
        public int ElementCount;

        /// <summary>
        /// Options for binding a raw buffer (see <see cref="ShaderResourceViewExtendedBufferFlags"/>).
        /// Only used for buffers.
        /// </summary>
        public ShaderResourceViewExtendedBufferFlags ExtendedBufferFlags;

        /// <summary>
        /// Index of the most detailed mipmap level to use. 
        /// This number is between 0 and MipLevels of the texture - 1.
        /// Not used for buffers and multisampled textures.
        /// </summary>
        public int MostDetailedMip;

        /// <summary>
        /// The maximum number of mipmap levels for the view of the texture.
        /// Set to -1 to indicate all the mipmap levels from MostDetailedMip on down to least detailed.
        /// Not used for buffers and multisampled textures.
        /// </summary>
        public int MipLevels;

        /// <summary>
        /// Index of the first texture to be used by the view in an array of textures.
        /// Only used for texture arrays.
        /// </summary>
        public int FirstArraySlice;

        /// <summary>
        /// Number of textures in the array to be used by the view.
        /// For cube textures, this value must be divisible by 6.
        /// Only used for texture arrays.
        /// </summary>
        public int ArraySize;
        
        public static bool Equals(ref ShaderResourceViewDescription desc1, ref ShaderResourceViewDescription desc2)
        {
            switch (desc1.Dimension)
            {
                case ShaderResourceViewDimension.Unknown:
                    return desc2.Dimension == ShaderResourceViewDimension.Unknown;
                case ShaderResourceViewDimension.Buffer:
                    return desc2.Dimension == ShaderResourceViewDimension.Buffer &&
                        desc1.FormatID == desc2.FormatID &&
                        desc1.FirstElement == desc2.FirstElement &&
                        desc1.ElementCount == desc2.ElementCount;
                case ShaderResourceViewDimension.Texture1D:
                    return desc2.Dimension == ShaderResourceViewDimension.Texture1D &&
                        desc1.FormatID == desc2.FormatID &&
                        desc1.MostDetailedMip == desc2.MostDetailedMip &&
                        desc1.MipLevels == desc2.MipLevels;
                case ShaderResourceViewDimension.Texture1DArray:
                    return desc2.Dimension == ShaderResourceViewDimension.Texture1DArray &&
                        desc1.FormatID == desc2.FormatID &&
                        desc1.MostDetailedMip == desc2.MostDetailedMip &&
                        desc1.MipLevels == desc2.MipLevels &&
                        desc1.FirstArraySlice == desc2.FirstArraySlice &&
                        desc1.ArraySize == desc2.ArraySize;
                case ShaderResourceViewDimension.Texture2D:
                    return desc2.Dimension == ShaderResourceViewDimension.Texture2D &&
                        desc1.FormatID == desc2.FormatID &&
                        desc1.MostDetailedMip == desc2.MostDetailedMip &&
                        desc1.MipLevels == desc2.MipLevels;
                case ShaderResourceViewDimension.Texture2DArray:
                    return desc2.Dimension == ShaderResourceViewDimension.Texture2DArray &&
                        desc1.FormatID == desc2.FormatID &&
                        desc1.MostDetailedMip == desc2.MostDetailedMip &&
                        desc1.MipLevels == desc2.MipLevels &&
                        desc1.FirstArraySlice == desc2.FirstArraySlice &&
                        desc1.ArraySize == desc2.ArraySize;
                case ShaderResourceViewDimension.Texture2DMultisampled:
                    return desc2.Dimension == ShaderResourceViewDimension.Texture2DMultisampled &&
                        desc1.FormatID == desc2.FormatID;
                case ShaderResourceViewDimension.Texture2DMultisampledArray:
                    return desc2.Dimension == ShaderResourceViewDimension.Texture2DMultisampledArray &&
                        desc1.FormatID == desc2.FormatID &&
                        desc1.FirstArraySlice == desc2.FirstArraySlice &&
                        desc1.ArraySize == desc2.ArraySize;
                case ShaderResourceViewDimension.Texture3D:
                    return desc2.Dimension == ShaderResourceViewDimension.Texture3D &&
                        desc1.FormatID == desc2.FormatID &&
                        desc1.MostDetailedMip == desc2.MostDetailedMip &&
                        desc1.MipLevels == desc2.MipLevels;
                case ShaderResourceViewDimension.TextureCube:
                    return desc2.Dimension == ShaderResourceViewDimension.TextureCube &&
                        desc1.FormatID == desc2.FormatID &&
                        desc1.MostDetailedMip == desc2.MostDetailedMip &&
                        desc1.MipLevels == desc2.MipLevels;
                case ShaderResourceViewDimension.TextureCubeArray:
                    return desc2.Dimension == ShaderResourceViewDimension.TextureCubeArray &&
                        desc1.FormatID == desc2.FormatID &&
                        desc1.MostDetailedMip == desc2.MostDetailedMip &&
                        desc1.MipLevels == desc2.MipLevels &&
                        desc1.FirstArraySlice == desc2.FirstArraySlice &&
                        desc1.ArraySize == desc2.ArraySize;
                default: throw new ShouldNeverHappenException();
            }
        }

        #region Buffer
        public static ShaderResourceViewDescription CreateFormBuffer(int formatID, int firstElement, int elementCount)
        {
            return new ShaderResourceViewDescription
            {
                Dimension = ShaderResourceViewDimension.Buffer,
                FormatID = formatID,
                FirstElement = firstElement,
                ElementCount = elementCount
            };
        }
        public static void CreateForBuffer(int formatID, int firstElement, int elementCount, out ShaderResourceViewDescription desc)
        {
            desc = new ShaderResourceViewDescription
            {
                Dimension = ShaderResourceViewDimension.Buffer,
                FormatID = formatID,
                FirstElement = firstElement,
                ElementCount = elementCount
            };
        }
        #endregion

        #region Texture1D
        public static ShaderResourceViewDescription CreateForTexture1D(int formatID, int mostDetailedMip, int mipLevels)
        {
            return new ShaderResourceViewDescription
            {
                Dimension = ShaderResourceViewDimension.Texture1D,
                FormatID = formatID,
                MostDetailedMip = mostDetailedMip,
                MipLevels = mipLevels
            };
        }
        public static void CreateForTexture1D(int formatID, int mostDetailedMip, int mipLevels, out ShaderResourceViewDescription desc)
        {
            desc = new ShaderResourceViewDescription
            {
                Dimension = ShaderResourceViewDimension.Texture1D,
                FormatID = formatID,
                MostDetailedMip = mostDetailedMip,
                MipLevels = mipLevels
            };
        }
        #endregion

        #region Texture1DArray
        public static ShaderResourceViewDescription CreateForTexture1DArray(int formatID, int mostDetailedMip, int mipLevels, int firstArraySlice, int arraySize)
        {
            return new ShaderResourceViewDescription
            {
                Dimension = ShaderResourceViewDimension.Texture1DArray,
                FormatID = formatID,
                MostDetailedMip = mostDetailedMip,
                MipLevels = mipLevels,
                FirstArraySlice = firstArraySlice,
                ArraySize = arraySize
            };
        }
        public static void CreateForTexture1DArray(int formatID, int mostDetailedMip, int mipLevels, int firstArraySlice, int arraySize, out ShaderResourceViewDescription desc)
        {
            desc = new ShaderResourceViewDescription
            {
                Dimension = ShaderResourceViewDimension.Texture1DArray,
                FormatID = formatID,
                MostDetailedMip = mostDetailedMip,
                MipLevels = mipLevels,
                FirstArraySlice = firstArraySlice,
                ArraySize = arraySize
            };
        }
        #endregion

        #region Texture2D
        public static ShaderResourceViewDescription CreateForTexture2D(int formatID, int mostDetailedMip, int mipLevels)
        {
            return new ShaderResourceViewDescription
            {
                Dimension = ShaderResourceViewDimension.Texture2D,
                FormatID = formatID,
                MostDetailedMip = mostDetailedMip,
                MipLevels = mipLevels
            };
        }
        public static void CreateForTexture2D(int formatID, int mostDetailedMip, int mipLevels, out ShaderResourceViewDescription desc)
        {
            desc = new ShaderResourceViewDescription
            {
                Dimension = ShaderResourceViewDimension.Texture2D,
                FormatID = formatID,
                MostDetailedMip = mostDetailedMip,
                MipLevels = mipLevels
            };
        }
        #endregion

        #region Texture2DArray
        public static ShaderResourceViewDescription CreateForTexture2DArray(int formatID, int mostDetailedMip, int mipLevels, int firstArraySlice, int arraySize)
        {
            return new ShaderResourceViewDescription
            {
                Dimension = ShaderResourceViewDimension.Texture2DArray,
                MostDetailedMip = mostDetailedMip,
                MipLevels = mipLevels,
                FirstArraySlice = firstArraySlice,
                ArraySize = arraySize
            };
        }
        public static void CreateForTexture2DArray(int formatID, int mostDetailedMip, int mipLevels, int firstArraySlice, int arraySize, out ShaderResourceViewDescription desc)
        {
            desc = new ShaderResourceViewDescription
            {
                Dimension = ShaderResourceViewDimension.Texture2DArray,
                MostDetailedMip = mostDetailedMip,
                MipLevels = mipLevels,
                FirstArraySlice = firstArraySlice,
                ArraySize = arraySize
            };
        }
        #endregion

        #region Texture2DMultisampled
        public static ShaderResourceViewDescription CreateForTexture2DMultisampled(int formatID)
        {
            return new ShaderResourceViewDescription
            {
                Dimension = ShaderResourceViewDimension.Texture2DMultisampled,
                FormatID = formatID
            };
        }
        public static void CreateForTexture2DMultisampled(int formatID, out ShaderResourceViewDescription desc)
        {
            desc = new ShaderResourceViewDescription
            {
                Dimension = ShaderResourceViewDimension.Texture2DMultisampled,
                FormatID = formatID
            };
        }
        #endregion

        #region Texture2DMultisampledArray
        public static ShaderResourceViewDescription CreateForTexture2DMultisampledArray(int formatID, int firstArraySlice, int arraySize)
        {
            return new ShaderResourceViewDescription
            {
                Dimension = ShaderResourceViewDimension.Texture2DMultisampledArray,
                FormatID = formatID,
                FirstArraySlice = firstArraySlice,
                ArraySize = arraySize
            };
        }
        public static void CreateForTexture2DMultisampledArray(int formatID, int firstArraySlice, int arraySize, out ShaderResourceViewDescription desc)
        {
            desc = new ShaderResourceViewDescription
            {
                Dimension = ShaderResourceViewDimension.Texture2DMultisampledArray,
                FormatID = formatID,
                FirstArraySlice = firstArraySlice,
                ArraySize = arraySize
            };
        }
        #endregion

        #region Texture3D
        public static ShaderResourceViewDescription CreateForTexture3D(int formatID, int mostDetailedMip, int mipLevels)
        {
            return new ShaderResourceViewDescription
            {
                Dimension = ShaderResourceViewDimension.Texture3D,
                FormatID = formatID,
                MostDetailedMip = mostDetailedMip,
                MipLevels = mipLevels
            };
        }
        public static void CreateForTexture3D(int formatID, int mostDetailedMip, int mipLevels, out ShaderResourceViewDescription desc)
        {
            desc = new ShaderResourceViewDescription
            {
                Dimension = ShaderResourceViewDimension.Texture3D,
                FormatID = formatID,
                MostDetailedMip = mostDetailedMip,
                MipLevels = mipLevels
            };
        }
        #endregion

        #region TextureCube
        public static ShaderResourceViewDescription CreateForTextureCube(int formatID, int mostDetailedMip, int mipLevels)
        {
            return new ShaderResourceViewDescription
            {
                Dimension = ShaderResourceViewDimension.TextureCube,
                FormatID = formatID,
                MostDetailedMip = mostDetailedMip,
                MipLevels = mipLevels
            };
        }
        public static void CreateForTextureCube(int formatID, int mostDetailedMip, int mipLevels, out ShaderResourceViewDescription desc)
        {
            desc = new ShaderResourceViewDescription
            {
                Dimension = ShaderResourceViewDimension.TextureCube,
                FormatID = formatID,
                MostDetailedMip = mostDetailedMip,
                MipLevels = mipLevels
            };
        }
        #endregion

        #region TextureCubeArray
        public static ShaderResourceViewDescription CreateForTextureCubeArray(int formatID, int mostDetailedMip, int mipLevels, int firstArraySlice, int arraySize)
        {
            return new ShaderResourceViewDescription
            {
                Dimension = ShaderResourceViewDimension.TextureCubeArray,
                FormatID = formatID,
                MostDetailedMip = mostDetailedMip,
                MipLevels = mipLevels,
                FirstArraySlice = firstArraySlice,
                ArraySize = arraySize
            };
        }
        public static void CreateForTextureCubeArray(int formatID, int mostDetailedMip, int mipLevels, int firstArraySlice, int arraySize, out ShaderResourceViewDescription desc)
        {
            desc = new ShaderResourceViewDescription
            {
                Dimension = ShaderResourceViewDimension.TextureCubeArray,
                FormatID = formatID,
                MostDetailedMip = mostDetailedMip,
                MipLevels = mipLevels,
                FirstArraySlice = firstArraySlice,
                ArraySize = arraySize
            };
        }
        #endregion

        #region BufferEx
        public static ShaderResourceViewDescription CreateForBufferEx(int formatID, int firstElement, int elementCount, ShaderResourceViewExtendedBufferFlags flags)
        {
            return new ShaderResourceViewDescription
            {
                Dimension = ShaderResourceViewDimension.ExtendedBuffer,
                FirstElement = firstElement,
                ElementCount = elementCount,
                ExtendedBufferFlags = flags
            };
        }
        public static void CreateForBufferEx(int formatID, int firstElement, int elementCount, ShaderResourceViewExtendedBufferFlags flags, out ShaderResourceViewDescription desc)
        {
            desc = new ShaderResourceViewDescription
            {
                Dimension = ShaderResourceViewDimension.ExtendedBuffer,
                FirstElement = firstElement,
                ElementCount = elementCount,
                ExtendedBufferFlags = flags
            };
        }
        #endregion
    }
}
