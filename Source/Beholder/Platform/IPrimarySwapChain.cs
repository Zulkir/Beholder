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
    /// Represents a primary swap chain of the device.
    /// This swap chain is capable of switching to the fullscreen mode.
    /// </summary>
    public interface IPrimarySwapChain : ISwapChain
    {
        /// <summary>
        /// Indicates whether the swap chain is in the fullscreen mode at the moment.
        /// </summary>
        FullscreenState FullscreenState { get; }

        void GetFullscreenDisplayMode(out DisplayMode displayMode);

        void ResetToPseudoFullscreen(ref SwapChainDescription description);

        /// <summary>
        /// Resets a swap chain and makes it switch to the fullscreen mode.
        /// </summary>
        /// <param name="displayMode">Display mode to use while in the fullscreen mode.</param>
        /// <param name="description">
        /// New settings for the swap chains to have after the reset.
        /// ColorBufferFormatID must be supported as a fullscreen swap chain color buffer format by the adapter.
        /// </param>
        void ResetToFullscreen(ref DisplayMode displayMode, ref SwapChainDescription description);
    }
}
