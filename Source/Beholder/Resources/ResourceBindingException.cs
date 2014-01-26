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
    /// Represents errors that are caused by trying to create a resource view upon a resource that was not created with the appropriate bind flag.
    /// </summary>
    public class ResourceBindingException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceBindingException"/> class with the specified error message.
        /// </summary>
        public ResourceBindingException()
            : base("Trying to create a view on the resource that was not created with the appropriate binding flags.")
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceBindingException"/> class with the specified error message
        /// and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="innerException"></param>
        public ResourceBindingException(Exception innerException)
            : base("Trying to create a view on the resource that was not created with the appropriate binding flags.", innerException)
        { }
    }
}
