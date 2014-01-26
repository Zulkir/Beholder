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
    /// These flags identify the type of resource that will be viewed as a shader resource.
    /// </summary>
    public enum ShaderResourceViewDimension
    {
        /// <summary>
        /// The type is unknown.
        /// </summary>
        Unknown,

        /// <summary>
        /// The resource is a buffer.
        /// </summary>
        Buffer,

        /// <summary>
        /// The resource is a 1D texture.
        /// </summary>
        Texture1D,

        /// <summary>
        /// The resource is an array of 1D textures.
        /// </summary>
        Texture1DArray,

        /// <summary>
        /// The resource is a 2D texture.
        /// </summary>
        Texture2D,

        /// <summary>
        /// The resource is an array of 2D textures.
        /// </summary>
        Texture2DArray,

        /// <summary>
        /// The resource is a multisampling 2D texture.
        /// </summary>
        Texture2DMultisampled,

        /// <summary>
        /// The resource is an array of multisampling 2D textures.
        /// </summary>
        Texture2DMultisampledArray,

        /// <summary>
        /// The resource is a 3D texture.
        /// </summary>
        Texture3D,

        /// <summary>
        /// The resource is a cube texture.
        /// </summary>
        TextureCube,

        /// <summary>
        /// The resource is an array of cube textures.
        /// </summary>
        TextureCubeArray,

        /// <summary>
        /// The resource is an extended buffer.
        /// </summary>
        ExtendedBuffer,
    }
}
