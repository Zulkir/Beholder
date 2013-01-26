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
using System.Runtime.InteropServices;

namespace Beholder.Resources
{
    /// <summary>
    /// Specifies data for initializing or updating a subresource.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct SubresourceData
    {
        public IntPtr Pointer;
        public Action UnpinPointer;

        /// <summary>
        /// Must be 0 for Buffers and 1D Textures. Must be 1, 2, 4, or 8 for 2D and 3D Textures
        /// </summary>
        public int RowByteAlignment;

        public SubresourceData(IntPtr pointer, int rowByteAlignment = 4)
        {
            Pointer = pointer;
            UnpinPointer = () => { };
            RowByteAlignment = rowByteAlignment;
        }

        public unsafe SubresourceData(Array array, int offsetInBytes = 0, int rowByteAlignment = 4)
        {
            GCHandle gcHandle = GCHandle.Alloc(array, GCHandleType.Pinned);
            Pointer = (IntPtr)((byte*)gcHandle.AddrOfPinnedObject() + offsetInBytes);
            UnpinPointer = gcHandle.Free;
            RowByteAlignment = rowByteAlignment;
        }
    }
}
