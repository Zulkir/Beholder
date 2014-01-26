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

namespace Beholder.Platform
{
    /// <summary>
    /// Describes a swap chain.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct SwapChainDescription
    {
        /// <summary>
        /// Number of the swap chain's color buffers.
        /// Must have at least one, but two are prefered. (Double buffering)
        /// </summary>
        public int ColorBufferCount;

        /// <summary>
        /// Format ID of the swap chain's color buffer.
        /// </summary>
        /// <remarks>
        /// You can check format support using the <see cref="IAdapter"/> methods.
        /// </remarks>
        public int ColorBufferFormatID;

        /// <summary>
        /// Specifies whether the swap chain should use its own (auto) depth-stencil buffer.
        /// </summary>
        public bool EnableAutoDepthStencil;

        /// <summary>
        /// Format ID of the swap chain's auto depth-stencil buffer. If <see cref="EnableAutoDepthStencil"/> is set to false, this field is ignored.
        /// </summary>
        /// <remarks>
        /// You can check format support using the <see cref="IAdapter"/> methods.
        /// </remarks>
        public int DepthStencilFormatID;

        /// <summary>
        /// A <see cref="Sampling"/> structure describing multi-sampling parameters.
        /// </summary>
        /// <remarks>
        /// You can check different sampling modes support using the <see cref="IAdapter"/> methods.
        /// </remarks>
        public Sampling Sampling;

        /// <summary>
        /// Specifies whether the swap chain should wait for a monitor vertical refresh to present a new frame.
        /// </summary>
        /// <remarks>Using this option will remove the "tearing" effect and reduce the chance of the GPU overheat, 
        /// but may reduce FPS if its non-synchronized value is below monitor's refresh rate.
        /// </remarks>
        public bool VerticalSync;

        /// <summary>
        /// Creates a new <see cref="SwapChainDescription"/> structure.
        /// </summary>
        /// <param name="colorBufferCount">Number of the swap chain's color buffers.</param>
        /// <param name="colorBufferFormatID">Format of the swap chain's color buffer.</param>
        /// <param name="enableAutoDepthStencil">Specifies whether the swap chain should use its own (auto) depth-stencil buffer.</param>
        /// <param name="depthStencilFormatID">Format of the swap chain's auto depth-stencil buffer. If <see cref="EnableAutoDepthStencil"/> is set to false, this field is ignored.</param>
        /// <param name="sampling">A <see cref="Sampling"/> structure describing multi-sampling parameters.</param>
        /// <param name="verticalSync">Specifies whether the swap chain should wait for a monitor vertical refresh to present a new frame.
            /// <remarks>Using this option will remove the "tearing" effect and reduce the chance of the GPU overheat, 
            /// but may reduce FPS if its non-synchronized value is below monitor's refresh rate.
            /// </remarks>
        /// </param>
        public SwapChainDescription(
            int colorBufferCount,
            int colorBufferFormatID,
            bool enableAutoDepthStencil,
            int depthStencilFormatID,
            Sampling sampling,
            bool verticalSync)
        {
            ColorBufferCount = colorBufferCount;
            ColorBufferFormatID = colorBufferFormatID;
            EnableAutoDepthStencil = enableAutoDepthStencil;
            DepthStencilFormatID = depthStencilFormatID;
            Sampling = sampling;
            VerticalSync = verticalSync;
        }
    }
}
