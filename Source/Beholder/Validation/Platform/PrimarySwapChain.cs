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
using Beholder.Platform;

namespace Beholder.Validation.Platform
{
    class PrimarySwapChain : SwapChain<IPrimarySwapChain>, IPrimarySwapChain
    {
        public PrimarySwapChain(IPrimarySwapChain real) : base(real) { }

        public FullscreenState FullscreenState { get { return Real.FullscreenState; } }
        public void GetFullscreenDisplayMode(out DisplayMode displayMode) { Real.GetFullscreenDisplayMode(out displayMode); }

        public void ResetToPseudoFullscreen(ref SwapChainDescription description)
        {
            if (InsideBeginEnd)
                throw new InvalidOperationException("Trying to call SwapChain.Reset() while inside a Begin-End pair.");

            ValidateDescriptionWindowed(ref description, Device.Adapter);

            Real.ResetToPseudoFullscreen(ref description);
        }

        public void ResetToFullscreen(ref DisplayMode displayMode, ref SwapChainDescription description)
        {
            if (InsideBeginEnd)
                throw new InvalidOperationException("Trying to call SwapChain.Reset() while inside a Begin-End pair.");

            ValidateDescriptionFullscreen(ref displayMode, ref description, Device.Adapter);

            Real.ResetToFullscreen(ref displayMode, ref description);
        }
    }
}
