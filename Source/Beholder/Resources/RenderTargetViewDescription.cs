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
    /// Describes a render target view.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct RenderTargetViewDescription
    {
        /// <summary>
        /// Type of the resource to be viewed.
        /// </summary>
        public RenderTargetViewDimension Dimension;

        /// <summary>
        /// Data format ID.
        /// </summary>
        public int FormatID;

        /// <summary>
        /// Number of bytes between the beginning of the buffer and the first element to access.
        /// </summary>
        public int ElementOffset;

        /// <summary>
        /// The width of each element (in bytes). 
        /// This can be determined from the format stored in the render-target-view description.
        /// </summary>
        public int ElementWidth;

        /// <summary>
        /// Index of the mip level to be used by the view.
        /// Not used for buffers and multisampled textures.
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
        /// A value of -1 indicates all of the slices along the w axis, starting from FirstDepthSlice.
        /// Only used by 3D textures.
        /// </summary>
        public int DepthSliceCount;

        public static bool Equals(ref RenderTargetViewDescription desc1, ref RenderTargetViewDescription desc2)
        {
            switch (desc1.Dimension)
            {
                case RenderTargetViewDimension.Unknown:
                    return desc2.Dimension == RenderTargetViewDimension.Unknown;
                case RenderTargetViewDimension.Buffer:
                    return desc2.Dimension == RenderTargetViewDimension.Buffer &&
                        desc1.FormatID == desc2.FormatID &&
                        desc1.ElementOffset == desc2.ElementOffset &&
                        desc1.ElementWidth == desc2.ElementWidth;
                case RenderTargetViewDimension.Texture1D:
                    return desc2.Dimension == RenderTargetViewDimension.Texture1D &&
                        desc1.FormatID == desc2.FormatID &&
                        desc1.MipSlice == desc2.MipSlice;
                case RenderTargetViewDimension.Texture1DArray:
                    return desc2.Dimension == RenderTargetViewDimension.Texture1DArray &&
                        desc1.FormatID == desc2.FormatID &&
                        desc1.MipSlice == desc2.MipSlice &&
                        desc1.FirstArraySlice == desc2.FirstArraySlice &&
                        desc1.ArraySize == desc2.ArraySize;
                case RenderTargetViewDimension.Texture2D:
                    return desc2.Dimension == RenderTargetViewDimension.Texture2D &&
                        desc1.FormatID == desc2.FormatID &&
                        desc1.MipSlice == desc2.MipSlice;
                case RenderTargetViewDimension.Texture2DArray:
                    return desc2.Dimension == RenderTargetViewDimension.Texture2DArray &&
                        desc1.FormatID == desc2.FormatID &&
                        desc1.MipSlice == desc2.MipSlice &&
                        desc1.FirstArraySlice == desc2.FirstArraySlice &&
                        desc1.ArraySize == desc2.ArraySize;
                case RenderTargetViewDimension.Texture2DMultisampled:
                    return desc2.Dimension == RenderTargetViewDimension.Texture2DMultisampled &&
                        desc1.FormatID == desc2.FormatID;
                case RenderTargetViewDimension.Texture2DMultisampledArray:
                    return desc2.Dimension == RenderTargetViewDimension.Texture2DMultisampledArray &&
                        desc1.FormatID == desc2.FormatID &&
                        desc1.FirstArraySlice == desc2.FirstArraySlice &&
                        desc1.ArraySize == desc2.ArraySize;
                case RenderTargetViewDimension.Texture3D:
                    return desc2.Dimension == RenderTargetViewDimension.Texture3D &&
                        desc1.FormatID == desc2.FormatID &&
                        desc1.MipSlice == desc2.MipSlice &&
                        desc1.FirstDepthSlice == desc2.FirstDepthSlice &&
                        desc1.DepthSliceCount == desc2.DepthSliceCount;
                default: throw new ShouldNeverHappenException();
            }
        }

        #region Buffer
        public static RenderTargetViewDescription CreateForBuffer(int formatID, int elementOffset, int elementWidth)
        {
            return new RenderTargetViewDescription
            {
                Dimension = RenderTargetViewDimension.Buffer,
                FormatID = formatID,
                ElementOffset = elementOffset,
                ElementWidth = elementWidth
            };
        }
        public static void CreateForBuffer(int formatID, int elementOffset, int elementWidth, out RenderTargetViewDescription desc)
        {
            desc = new RenderTargetViewDescription
            {
                Dimension = RenderTargetViewDimension.Buffer,
                FormatID = formatID,
                ElementOffset = elementOffset,
                ElementWidth = elementWidth
            };
        }
        #endregion

        #region Texture1D
        public static RenderTargetViewDescription CreateForTexture1D(int formatID, int mipSlice)
        {
            return new RenderTargetViewDescription
            {
                Dimension = RenderTargetViewDimension.Texture1D,
                FormatID = formatID,
                MipSlice = mipSlice
            };
        }
        public static void CreateForTexture1D(int formatID, int mipSlice, out RenderTargetViewDescription desc)
        {
            desc = new RenderTargetViewDescription
            {
                Dimension = RenderTargetViewDimension.Texture1D,
                FormatID = formatID,
                MipSlice = mipSlice
            };
        }
        #endregion

        #region Texture1DArray
        public static RenderTargetViewDescription CreateForTexture1DArray(int formatID, int mipSlice, int firstArraySlice, int arraySize)
        {
            return new RenderTargetViewDescription
            {
                Dimension = RenderTargetViewDimension.Texture1DArray,
                FormatID = formatID,
                MipSlice = mipSlice,
                FirstArraySlice = firstArraySlice,
                ArraySize = arraySize
            };
        }
        public static void CreateForTexture1DArray(int formatID, int mipSlice, int firstArraySlice, int arraySize, out RenderTargetViewDescription desc)
        {
            desc = new RenderTargetViewDescription
            {
                Dimension = RenderTargetViewDimension.Texture1DArray,
                FormatID = formatID,
                MipSlice = mipSlice,
                FirstArraySlice = firstArraySlice,
                ArraySize = arraySize
            };
        }
        #endregion

        #region Texture2D
        public static RenderTargetViewDescription CreateForTexture2D(int formatID, int mipSlice)
        {
            return new RenderTargetViewDescription
            {
                Dimension = RenderTargetViewDimension.Texture2D,
                FormatID = formatID,
                MipSlice = mipSlice
            };
        }
        public static void CreateForTexture2D(int formatID, int mipSlice, out RenderTargetViewDescription desc)
        {
            desc = new RenderTargetViewDescription
            {
                Dimension = RenderTargetViewDimension.Texture2D,
                FormatID = formatID,
                MipSlice = mipSlice
            };
        }
        #endregion

        #region Texture2DArray
        public static RenderTargetViewDescription CreateForTexture2DArray(int formatID, int mipSlice, int firstArraySlice, int arraySize)
        {
            return new RenderTargetViewDescription
            {
                Dimension = RenderTargetViewDimension.Texture2DArray,
                FormatID = formatID,
                MipSlice = mipSlice,
                FirstArraySlice = firstArraySlice,
                ArraySize = arraySize
            };
        }
        public static void CreateForTexture2DArray(int formatID, int mipSlice, int firstArraySlice, int arraySize, out RenderTargetViewDescription desc)
        {
            desc = new RenderTargetViewDescription
            {
                Dimension = RenderTargetViewDimension.Texture2DArray,
                FormatID = formatID,
                MipSlice = mipSlice,
                FirstArraySlice = firstArraySlice,
                ArraySize = arraySize
            };
        }
        #endregion

        #region Texture2DMultisampled
        public static RenderTargetViewDescription CreateForTexture2DMultisampled(int formatID)
        {
            return new RenderTargetViewDescription
            {
                Dimension = RenderTargetViewDimension.Texture2DMultisampled,
                FormatID = formatID
            };
        }
        public static void CreateForTexture2DMultisampled(int formatID, out RenderTargetViewDescription desc)
        {
            desc = new RenderTargetViewDescription
            {
                Dimension = RenderTargetViewDimension.Texture2DMultisampled,
                FormatID = formatID
            };
        }
        #endregion

        #region Texture2DMultisampledArray
        public static RenderTargetViewDescription CreateForTexture2DMultisampledArray(int formatID, int firstArraySlice, int arraySize)
        {
            return new RenderTargetViewDescription
            {
                Dimension = RenderTargetViewDimension.Texture2DMultisampledArray,
                FormatID = formatID,
                FirstArraySlice = firstArraySlice,
                ArraySize = arraySize
            };
        }
        public static void CreateForTexture2DMultisampledArray(int formatID, int firstArraySlice, int arraySize, out RenderTargetViewDescription desc)
        {
            desc = new RenderTargetViewDescription
            {
                Dimension = RenderTargetViewDimension.Texture2DMultisampledArray,
                FormatID = formatID,
                FirstArraySlice = firstArraySlice,
                ArraySize = arraySize
            };
        }
        #endregion

        #region Texture3D
        public static RenderTargetViewDescription CreateForTexture3D(int formatID, int mipSlice, int firstDepthSlice, int depthSliceCount)
        {
            return new RenderTargetViewDescription
            {
                Dimension = RenderTargetViewDimension.Texture3D,
                FormatID = formatID,
                MipSlice = mipSlice,
                FirstDepthSlice = firstDepthSlice,
                DepthSliceCount = depthSliceCount
            };
        }
        public static void CreateForTexture3D(int formatID, int mipSlice, int firstDepthSlice, int depthSliceCount, out RenderTargetViewDescription desc)
        {
            desc = new RenderTargetViewDescription
            {
                Dimension = RenderTargetViewDimension.Texture3D,
                FormatID = formatID,
                MipSlice = mipSlice,
                FirstDepthSlice = firstDepthSlice,
                DepthSliceCount = depthSliceCount
            };
        }
        #endregion
    }
}
