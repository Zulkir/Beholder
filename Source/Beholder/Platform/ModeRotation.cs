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

namespace Beholder.Platform
{
    /// <summary>
    /// Flags that indicate how the back buffers should be rotated to fit the physical rotation of a monitor.
    /// </summary>
    public enum ModeRotation
    {
        /// <summary>
        /// Unspecified rotation.
        /// </summary>
        Unspecified = 0,

        /// <summary>
        /// Specifies no rotation.
        /// </summary>
        Identity = 1,

        /// <summary>
        /// Specifies 90 degrees of rotation.
        /// </summary>
        Rotate90 = 2,

        /// <summary>
        /// Specifies 180 degrees of rotation.
        /// </summary>
        Rotate180 = 3,

        /// <summary>
        /// Specifies 270 degrees of rotation.
        /// </summary>
        Rotate270 = 4,
    }
}
