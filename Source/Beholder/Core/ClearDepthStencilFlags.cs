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

namespace Beholder.Core
{
    /// <summary>
    /// <para>
    /// Specifies the parts of the depth stencil to clear.
    /// </para>
    /// <para>
    /// Try to avoid using values other than <see cref="ClearDepthStencilFlags.All"/> (see remarks).
    /// </para>
    /// </summary>
    /// <remarks>
    /// Even though option to clear only a part of a depth-stencil texture is available, Both Depth and Stencil parts have to be cleared every frame. 
    /// Otherwise, multi-GPU performance may drop significantly.
    /// </remarks>
    [Flags]
    public enum ClearDepthStencilFlags
    {
        /// <summary>
        /// Clear the depth buffer.
        /// </summary>
        DepthOnly = 0x1,


        /// <summary>
        /// Clear the stencil buffer.
        /// </summary>
        StencilOnly = 0x2,


        /// <summary>
        /// Clear the whole buffer.
        /// </summary>
        All = 0x3
    }
}
