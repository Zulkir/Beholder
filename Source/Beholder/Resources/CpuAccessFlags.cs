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
    /// This enumeration is currently unused. Specifies the types of CPU access allowed for a resource.
    /// </summary>
    [Flags]
    public enum CpuAccessFlags
    {
        /// <summary>
        /// No flags specified.
        /// </summary>
        None = 0x0,

        /// <summary>
        /// The resource is to be mappable so that the CPU can change its contents. 
        /// Resources created with this flag cannot be set as outputs of the pipeline and must be created with either dynamic or staging usage (see <see cref="Usage"/>).
        /// </summary>
        Write = 0x10000,

        /// <summary>
        /// The resource is to be mappable so that the CPU can read its contents. 
        /// Resources created with this flag cannot be set as either inputs or outputs to the pipeline and must be created with staging usage (see <see cref="Usage"/>).
        /// </summary>
        Read = 0x20000
    }
}
