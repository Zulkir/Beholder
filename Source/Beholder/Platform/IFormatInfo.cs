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

using Beholder.Core;

namespace Beholder.Platform
{
    /// <summary>
    /// Describes a color-alpha format.
    /// </summary>
    public interface IFormatInfo
    {
        /// <summary>
        /// ID of the format.
        /// </summary>
        int ID { get; }

        /// <summary>
        /// Number of color channels (not including alpha) suppoted by the format.
        /// </summary>
        int NumColors { get; }

        /// <summary>
        /// Number of bits that contain color data.
        /// </summary>
        int ColorBits { get; }

        /// <summary>
        /// Number of bits that contain alpha data.
        /// </summary>
        int AlphaBits { get; }

        /// <summary>
        /// Total bits allocated for one pixel or texel of this format (including padding).
        /// </summary>
        int TotalBits { get; }

        /// <summary>
        /// Type of the color data.
        /// </summary>
        FormatElementType ColorFormatType { get; }

        /// <summary>
        /// Type of the alpha data.
        /// </summary>
        FormatElementType AlphaFormatType { get; }

        /// <summary>
        /// Corresponding explicit format if one exists. 
        /// </summary>
        ExplicitFormat ExplicitFormat { get; }

        /// <summary>
        /// A string that contains a format description.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Flags specifying additional format features and drawbacks.
        /// </summary>
        ColorAlphaFormatFlags Flags { get; }
    }
}
