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
using Beholder.Resources;

namespace Beholder.Core
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct VertexSource
    {
        /// <summary>
        /// Vertex buffer from which vertex data is streamed.
        /// </summary>
        public IBuffer Buffer;

        /// <summary>
        /// Byte offset into the buffer to begin streaming the data.
        /// </summary>
        public int Offset;

        /// <summary>
        /// Byte stride of one vertex in the buffer.
        /// </summary>
        public int Stride;

        /// <summary>
        /// Creates a new <see cref="VertexSource"/> structure.
        /// </summary>
        /// <param name="buffer">Vertex buffer from which vertex data is streamed.</param>
        /// <param name="offset">Byte offset into the buffer to begin streaming the data.</param>
        /// <param name="stride">Byte stride of one vertex in the buffer.</param>
        public VertexSource(IBuffer buffer, int offset, int stride)
        {
            Buffer = buffer;
            Offset = offset;
            Stride = stride;
        }

        public static bool Equals(VertexSource vs1, VertexSource vs2)
        {
            return ReferenceEquals(vs1.Buffer, vs2.Buffer) &&
                   vs1.Offset == vs2.Offset &&
                   vs1.Stride == vs2.Stride;
        }
    }
}
