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

namespace Beholder.Resources
{
    /// <summary>
    /// Unordered-access view options.
    /// </summary>
    public enum UnorderedAccessViewDimension
    {
        /// <summary>
        /// The view type is unknown.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// View the resource as a buffer.
        /// </summary>
        Buffer = 1,

        /// <summary>
        /// View the resource as a 1D texture.
        /// </summary>
        Texture1D = 2,

        /// <summary>
        /// View the resource as a 1D texture array.
        /// </summary>
        Texture1DArray = 3,

        /// <summary>
        /// View the resource as a 2D texture.
        /// </summary>
        Texture2D = 4,

        /// <summary>
        /// View the resource as a 2D texture array.
        /// </summary>
        Texture2DArray  = 5,

        /// <summary>
        /// View the resource as a 3D texture array.
        /// </summary>
        Texture3D = 8
    }
}
