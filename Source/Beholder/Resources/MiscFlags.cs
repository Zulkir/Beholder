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
using Beholder.Core;

namespace Beholder.Resources
{
    /// <summary>
    /// Identifies other, less common options for resources.
    /// </summary>
    [Flags]
    public enum MiscFlags
    {
        /// <summary>
        /// Specifies no flags.
        /// </summary>
        None = 0x0,

        /// <summary>
        /// Enables mipmap generation using <see cref="IDeviceContext.GenerateMips"/> on a texture resource. 
        /// The resource must be created with the bind flags that specify that the resource is a render target and a shader resource.
        /// </summary>
        GenerateMips = 0x1,
        
        /// <summary>
        /// Enables a resource to be a cube texture created from a Texture2DArray that contains 6 textures.
        /// </summary>
        TextureCube = 0x4,

        /// <summary>
        /// Enables instancing of GPU-generated content.
        /// </summary>
        DrawInderectArgs = 0x10,

        /// <summary>
        /// Enables a resource as a byte address buffer.
        /// </summary>
        BufferAllowRawViews = 0x20,

        /// <summary>
        /// Enables a resource as a structured buffer.
        /// </summary>
        BufferStructured = 0x40,

        //TODO: Clamp
        //Clamp = 0x80,

        //SharedKyyMutex = 0x100,
        //GdiCompatible = 0x200,
    }
}
