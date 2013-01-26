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

using Beholder.Utility;

namespace Beholder.Resources
{
    /// <summary>
    /// Describes a depth-stencil view.
    /// </summary>
    public struct DepthStencilViewDescription
    {
        /// <summary>
        /// Type of the resource to be viewed.
        /// </summary>
        public DepthStencilViewDimension Dimension;

        /// <summary>
        /// Data format ID.
        /// </summary>
        public int FormatID;

        /// <summary>
        /// Describes whether the texture is read only. 
        /// Pass <see cref="DepthStencilViewFlags.None"/> to specify that it is not read only.
        /// </summary>
        public DepthStencilViewFlags Flags;

        /// <summary>
        /// Index of the mip level to be used by the view.
        /// Not used for multisampled textures.
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

        public static bool Equals(ref DepthStencilViewDescription desc1, ref DepthStencilViewDescription desc2)
        {
            switch (desc1.Dimension)
            {
                case DepthStencilViewDimension.Unknown: 
                    return desc2.Dimension == DepthStencilViewDimension.Unknown;
                case DepthStencilViewDimension.Texture1D:
                    return desc2.Dimension == DepthStencilViewDimension.Texture1D &&
                        desc1.FormatID == desc2.FormatID &&
                        desc1.Flags == desc2.Flags &&
                        desc1.MipSlice == desc2.MipSlice;
                case DepthStencilViewDimension.Texture1DArray:
                    return desc2.Dimension == DepthStencilViewDimension.Texture1DArray &&
                        desc1.FormatID == desc2.FormatID &&
                        desc1.Flags == desc2.Flags &&
                        desc1.MipSlice == desc2.MipSlice &&
                        desc1.FirstArraySlice == desc2.FirstArraySlice &&
                        desc1.ArraySize == desc2.ArraySize;
                case DepthStencilViewDimension.Texture2D:
                    return desc2.Dimension == DepthStencilViewDimension.Texture2D &&
                        desc1.FormatID == desc2.FormatID &&
                        desc1.Flags == desc2.Flags &&
                        desc1.MipSlice == desc2.MipSlice;
                case DepthStencilViewDimension.Texture2DArray:
                    return desc2.Dimension == DepthStencilViewDimension.Texture2DArray &&
                        desc1.FormatID == desc2.FormatID &&
                        desc1.Flags == desc2.Flags &&
                        desc1.MipSlice == desc2.MipSlice &&
                        desc1.FirstArraySlice == desc2.FirstArraySlice &&
                        desc1.ArraySize == desc2.ArraySize;
                case DepthStencilViewDimension.Texture2DMultisampled:
                    return desc2.Dimension == DepthStencilViewDimension.Texture2DMultisampled &&
                        desc1.FormatID == desc2.FormatID &&
                        desc1.Flags == desc2.Flags;
                case DepthStencilViewDimension.Texture2DMultisampledArray:
                    return desc2.Dimension == DepthStencilViewDimension.Texture2DMultisampledArray &&
                        desc1.FormatID == desc2.FormatID &&
                        desc1.Flags == desc2.Flags &&
                        desc1.FirstArraySlice == desc2.FirstArraySlice &&
                        desc1.ArraySize == desc2.ArraySize;
                default: throw new ShouldNeverHappenException();
            }
        }

        #region Texture1D
        public static DepthStencilViewDescription CreateForTexture1D(int formatID, DepthStencilViewFlags flags, int mipSlice)
        {
            return new DepthStencilViewDescription
            {
                Dimension = DepthStencilViewDimension.Texture1D,
                FormatID = formatID,
                Flags = flags,
                MipSlice = mipSlice
            };
        }
        public static void CreateForTexture1D(int formatID, DepthStencilViewFlags flags, int mipSlice, out DepthStencilViewDescription desc)
        {
            desc = new DepthStencilViewDescription
            {
                Dimension = DepthStencilViewDimension.Texture1D,
                FormatID = formatID,
                Flags = flags,
                MipSlice = mipSlice
            };
        }
        #endregion

        #region Texture1DArray
        public static DepthStencilViewDescription CreateForTexture1DArray(int formatID, DepthStencilViewFlags flags, int mipSlice, int firstArraySlice, int arraySize)
        {
            return new DepthStencilViewDescription
            {
                Dimension = DepthStencilViewDimension.Texture1DArray,
                FormatID = formatID,
                Flags = flags,
                MipSlice = mipSlice,
                FirstArraySlice = firstArraySlice,
                ArraySize = arraySize
            };
        }
        public static void CreateForTexture1DArray(int formatID, DepthStencilViewFlags flags, int mipSlice, int firstArraySlice, int arraySize, out DepthStencilViewDescription desc)
        {
            desc = new DepthStencilViewDescription
            {
                Dimension = DepthStencilViewDimension.Texture1DArray,
                FormatID = formatID,
                Flags = flags,
                MipSlice = mipSlice,
                FirstArraySlice = firstArraySlice,
                ArraySize = arraySize
            };
        }
        #endregion

        #region Texture2D
        public static DepthStencilViewDescription CreateForTexture2D(int formatID, DepthStencilViewFlags flags, int mipSlice)
        {
            return new DepthStencilViewDescription
            {
                Dimension = DepthStencilViewDimension.Texture2D,
                FormatID = formatID,
                Flags = flags,
                MipSlice = mipSlice
            };
        }
        public static void CreateForTexture2D(int formatID, DepthStencilViewFlags flags, int mipSlice, out DepthStencilViewDescription desc)
        {
            desc = new DepthStencilViewDescription
            {
                Dimension = DepthStencilViewDimension.Texture2D,
                FormatID = formatID,
                Flags = flags,
                MipSlice = mipSlice
            };
        }
        #endregion

        #region Texture2DArray
        public static DepthStencilViewDescription CreateForTexture2DArray(int formatID, DepthStencilViewFlags flags, int mipSlice, int firstArraySlice, int arraySize)
        {
            return new DepthStencilViewDescription
            {
                Dimension = DepthStencilViewDimension.Texture2DArray,
                FormatID = formatID,
                Flags = flags,
                MipSlice = mipSlice,
                FirstArraySlice = firstArraySlice,
                ArraySize = arraySize
            };
        }
        public static void CreateForTexture2DArray(int formatID, DepthStencilViewFlags flags, int mipSlice, int firstArraySlice, int arraySize, out DepthStencilViewDescription desc)
        {
            desc = new DepthStencilViewDescription
            {
                Dimension = DepthStencilViewDimension.Texture2DArray,
                FormatID = formatID,
                Flags = flags,
                MipSlice = mipSlice,
                FirstArraySlice = firstArraySlice,
                ArraySize = arraySize
            };
        }
        #endregion

        #region Texture2DMultisampled
        public static DepthStencilViewDescription CreateForTexture2DMultisampled(int formatID, DepthStencilViewFlags flags)
        {
            return new DepthStencilViewDescription
            {
                Dimension = DepthStencilViewDimension.Texture2DMultisampled,
                FormatID = formatID,
                Flags = flags,
            };
        }
        public static void CreateForTexture2DMultisampled(int formatID, DepthStencilViewFlags flags, out DepthStencilViewDescription desc)
        {
            desc = new DepthStencilViewDescription
            {
                Dimension = DepthStencilViewDimension.Texture2DMultisampled,
                FormatID = formatID,
                Flags = flags,
            };
        }
        #endregion

        #region Texture2DMultisampledArray
        public static DepthStencilViewDescription CreateForTexture2DMultisampledArray(int formatID, DepthStencilViewFlags flags, int firstArraySlice, int arraySize)
        {
            return new DepthStencilViewDescription
            {
                Dimension = DepthStencilViewDimension.Texture2DMultisampledArray,
                FormatID = formatID,
                Flags = flags,
                FirstArraySlice = firstArraySlice,
                ArraySize = arraySize
            };
        }
        public static void CreateForTexture2DMultisampledArray(int formatID, DepthStencilViewFlags flags, int firstArraySlice, int arraySize, out DepthStencilViewDescription desc)
        {
            desc = new DepthStencilViewDescription
            {
                Dimension = DepthStencilViewDimension.Texture2DMultisampledArray,
                FormatID = formatID,
                Flags = flags,
                FirstArraySlice = firstArraySlice,
                ArraySize = arraySize
            };
        }
        #endregion
    }
}
