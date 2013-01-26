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

namespace Beholder.Resources
{
    /// <summary>
    /// Identifies how to bind a resource to the pipeline.
    /// </summary>
    [Flags]
    public enum BindFlags : ushort
    {
        /// <summary>
        /// Resource will not be bound to any pipeline stage.
        /// </summary>
        None = 0,

        /// <summary>
        /// Bind a buffer as a vertex buffer to the input-assembler stage.
        /// </summary>
        VertexBuffer = 0x1,

        /// <summary>
        /// Bind a buffer as an index buffer to the input-assembler stage.
        /// </summary>
        IndexBuffer = 0x2,

        /// <summary>
        /// Bind a buffer as a uniform buffer to a shader stage; this flag may NOT be combined with any other bind flag.
        /// </summary>
        UniformBuffer = 0x4,

        /// <summary>
        /// Bind a buffer or texture to a shader stage.
        /// </summary>
        ShaderResource = 0x8,

        /// <summary>
        /// Bind an output buffer for the stream-output stage.
        /// </summary>
        StreamOutput = 0x10,

        /// <summary>
        /// Bind a texture as a render target for the output-merger stage.
        /// </summary>
        RenderTarget = 0x20,

        /// <summary>
        /// Bind a texture as a depth-stencil target for the output-merger stage.
        /// </summary>
        DepthStencil = 0x40,

        /// <summary>
        /// Bind an unordered access resource.
        /// </summary>
        UnorderedAccess = 0x80
    }
}
