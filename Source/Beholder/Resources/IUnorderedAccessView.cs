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

namespace Beholder.Resources
{
    /// <summary>
    /// A view that represents a resource that can be accessed by the shader in an unordered way.
    /// </summary>
    public interface IUnorderedAccessView : IResourceView
    {
        /// <summary>
        /// Type of the resource to be viewed.
        /// </summary>
        UnorderedAccessViewDimension Dimension { get; }

        /// <summary>
        /// Data format ID.
        /// </summary>
        int FormatID { get; }

        /// <summary>
        /// Index of the first buffer element to be used by the view.
        /// Only used for buffers.
        /// </summary>
        int FirstElement { get; }

        /// <summary>
        /// Number of buffer elements in the resource.
        /// For structured buffers, this is the number of structures in the buffer.
        /// Only used for buffers.
        /// </summary>
        int ElementCount { get; }

        /// <summary>
        /// View options for the buffer resource.
        /// Only used for buffers (obviously).
        /// </summary>
        UnorderedAccessViewBufferFlags BufferFlags { get; }

        /// <summary>
        /// Index of the mip level to be used by the view.
        /// Not used for buffers.
        /// </summary>
        int MipSlice { get; }

        /// <summary>
        /// Index of the first texture to be used by the view in an array of textures.
        /// Only used for texture arrays.
        /// </summary>
        int FirstArraySlice { get; }

        /// <summary>
        /// Number of textures in the array to be used by the view.
        /// Only used for texture arrays.
        /// </summary>
        int ArraySize { get; }

        /// <summary>
        /// First depth level to be used by the view.
        /// Only used by 3D textures.
        /// </summary>
        int FirstDepthSlice { get; }

        /// <summary>
        /// Number of depth levels to be used by the view, starting from FirstDepthSlice.
        /// Only used by 3D textures.
        /// </summary>
        int DepthSliceCount { get; }

        /// <summary>
        /// Gets a description of the view.
        /// </summary>
        /// <param name="description">Variable to fill with the description data.</param>
        void GetDesc(out UnorderedAccessViewDescription description);
    }
}
