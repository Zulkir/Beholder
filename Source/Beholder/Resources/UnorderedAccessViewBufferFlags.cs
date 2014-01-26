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

namespace Beholder.Resources
{
    /// <summary>
    /// Unordered-access-view buffer options.
    /// </summary>
    [Flags]
    public enum UnorderedAccessViewBufferFlags
    {
        /// <summary>
        /// No flags specified.
        /// </summary>
        None = 0,

        /// <summary>
        /// Resource contains raw, unstructured data. Requires the UAV format to be R32_TYPELESS.
        /// </summary>
        Raw = 0x1,

        /// <summary>
        /// Allow data to be appended to the end of the buffer. 
        /// This flag must also be used for any view that will be used as a AppendStructuredBuffer or a ConsumeStructuredBuffer. 
        /// Requires the UAV format to be R32_TYPELESS.
        /// </summary>
        Append = 0x2,

        /// <summary>
        /// Adds a counter to the unordered-access-view buffer. 
        /// This flag can only be used on a UAV that is a RWStructuredBuffer and it enables the functionality needed for the IncrementCounter and DecrementCounter methods in HLSL. 
        /// Requires the UAV format to be R32_TYPELESS.
        /// </summary>
        Counter = 0x4
    }
}
