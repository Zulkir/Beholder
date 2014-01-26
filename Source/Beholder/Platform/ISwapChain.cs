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

using Beholder.Resources;

namespace Beholder.Platform
{
    /// <summary>
    /// Implements one or more surfaces for storing rendered data before presenting it to an output.
    /// </summary>
    public interface ISwapChain : IDeviceChild
    {
        /// <summary>
        /// Gets a window that holds the swap chain.
        /// </summary>
        IWindow Window { get; }

        /// <summary>
        /// Gets the width of the swap chain.
        /// </summary>
        int Width { get; }

        /// <summary>
        /// Gets the height of the swap chain.
        /// </summary>
        int Height { get; }

        /// <summary>
        /// Number of the swap chain's color buffers.
        /// </summary>
        int ColorBufferCount { get; }

        /// <summary>
        /// Format ID of the swap chain's color buffer.
        /// </summary>
        int ColorBufferFormatID { get; }

        /// <summary>
        /// Specifies whether the swap chain should use its own (auto) depth-stencil buffer.
        /// </summary>
        bool EnableAutoDepthStencil { get; }

        /// <summary>
        /// Format ID of the swap chain's auto depth-stencil buffer. If <see cref="EnableAutoDepthStencil"/> is set to false, this field is ignored.
        /// </summary>
        int DepthStencilFormatID { get; }

        /// <summary>
        /// A <see cref="Sampling"/> structure describing multi-sampling parameters.
        /// </summary>
        Sampling Sampling { get; }

        /// <summary>
        /// Specifies whether the swap chain should wait for a monitor vertical refresh to present a new frame.
        /// </summary>
        /// <remarks>Using this option will remove the "tearing" effect and reduce the chance of the GPU overheat, 
        /// but may reduce FPS if its non-synchronized value is below monitor's refresh rate.
        /// </remarks>
        bool VerticalSync { get; }

        /// <summary>
        /// Gets the description of the swap chain.
        /// </summary>
        /// <param name="description">Variable to fill with the description data.</param>
        void GetDescription(out SwapChainDescription description);

        /// <summary>
        /// Gets a <see cref="IRenderTargetView"/> of the color buffer to which rendering should be done this frame.
        /// </summary>
        /// <returns><see cref="IRenderTargetView"/> of the color buffer.</returns>
        IRenderTargetView GetCurrentColorBuffer();

        /// <summary>
        /// Gets a <see cref="IDepthStencilView"/> of the depth-stencil buffer which should be used this frame.
        /// </summary>
        /// <returns><see cref="IDepthStencilView"/> of the depth-stencil buffer.</returns>
        IDepthStencilView GetCurrentDepthStencilBuffer();

        /// <summary>
        /// Checks whether the rendering is currently possible into the swap chain and prepares it to accepting drawing commands.
        /// </summary>
        /// <returns>Boolean value indicating whether it is currenlt possible to draw into the swap chain this frame.</returns>
        bool BeginScene();

        /// <summary>
        /// Prepares the swap chain to present the buffer.
        /// This method should always be called after the succeded Begin call and before the Present call.
        /// </summary>
        void EndScene();

        /// <summary>
        /// Presents the back buffer data to the screen.
        /// </summary>
        void Present();

        /// <summary>
        /// Resets the swap chain and switches it to windowed mode.
        /// </summary>
        /// <param name="description">New settings for the swap chains to have after the reset.</param>
        void Reset(ref SwapChainDescription description);
    }
}
