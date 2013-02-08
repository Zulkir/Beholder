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

namespace Beholder.Resources
{
    /// <summary>
    /// Represents an explicitly disposable resource stored in video memory.
    /// </summary>
    public interface IResource : IDeviceChild, IDisposableOnce
    {
        /// <summary>
        /// Get the dimension of the resource.
        /// </summary>
        ResourceDimension Dimension { get; }

        /// <summary>
        /// Identify how the resource is expected to be read from and written to. 
        /// Frequency of update is a key factor.
        /// </summary>
        Usage Usage { get; }

        /// <summary>
        /// Identify how the buffer will be bound to the pipeline. 
        /// Flags can be combined with a logical OR.
        /// </summary>
        BindFlags BindFlags { get; }

        /// <summary>
        /// Flags that identify other, less common resource options. 
        /// These flags can be combined with a logical OR.
        /// </summary>
        MiscFlags MiscFlags { get; }

        /// <summary>
        /// Extra flags that specify use-cases of the resource for less flexible APIs (i.e. D3D9).
        /// Flags can be combined with a logical OR.
        /// </summary>
        ExtraFlags ExtraFlags { get; }
    }
}
