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

namespace Beholder.Resources
{
    /// <summary>
    /// Specifies how to access a resource used in a depth-stencil view.
    /// </summary>
    public enum DepthStencilViewDimension
    {
        /// <summary>
        /// Not a valid value and is not used.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// The resource will be accessed as a 1D texture.
        /// </summary>
        Texture1D = 1,

        /// <summary>
        /// The resource will be accessed as an array of 1D textures.
        /// </summary>
        Texture1DArray = 2,

        /// <summary>
        /// The resource will be accessed as a 2D texture.
        /// </summary>
        Texture2D = 3,

        /// <summary>
        /// The resource will be accessed as an array of 2D textures.
        /// </summary>
        Texture2DArray = 4,

        /// <summary>
        /// The resource will be accessed as a 2D texture with multisampling.
        /// </summary>
        Texture2DMultisampled = 5,

        /// <summary>
        /// The resource will be accessed as an array of 2D textures with multisampling.
        /// </summary>
        Texture2DMultisampledArray = 6,
    }
}
