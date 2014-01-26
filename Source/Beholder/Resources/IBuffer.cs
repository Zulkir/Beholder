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
    /// Represents a buffer resource, which is unstructured memory. Buffers typically store vertex or index data.
    /// </summary>
    public interface IBuffer : IResource
    {
        /// <summary>
        /// Size of the buffer in bytes.
        /// </summary>
        int SizeInBytes { get; }

        /// <summary>
        /// The size of the structure (in bytes) when it represents a structured buffer.
        /// </summary>
        int StructureByteStride { get; }

        /// <summary>
        /// Gets a description of the buffer.
        /// </summary>
        /// <param name="description">Variable to fill with the description data.</param>
        void GetDescription(out BufferDescription description);

        IRenderTargetView ViewAsRenderTarget(int formatID, int firstElement, int elementCount);
        IShaderResourceView ViewAsShaderResource(int formatID, int firstElement, int elementCount);
        IShaderResourceView ViewAsShaderResourceExtended(int formatID, int firstElement, int elementCount, ShaderResourceViewExtendedBufferFlags flags);
        IUnorderedAccessView ViewAsUnorderedAccessResource(int formatID, int firstElement, int elementCount, UnorderedAccessViewBufferFlags flags);
    }
}
