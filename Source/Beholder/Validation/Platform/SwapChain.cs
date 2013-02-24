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
using System.Linq;
using Beholder.Platform;
using Beholder.Resources;

namespace Beholder.Validation.Platform
{
    abstract class SwapChain<T> : SwapChain, IWrapper<T> where T : ISwapChain
    {
        public new T Real { get; private set; }
        protected SwapChain(T real) : base(real) { Real = real; }
    }

    abstract class SwapChain : Wrapper<ISwapChain>, ISwapChain
    {
        protected bool InsideBeginEnd { get; private set; }

        protected SwapChain(ISwapChain real) : base(real)
        {
        }

        public int Width { get { return Real.Width; } }
        public int Height { get { return Real.Height; } }
        public int ColorBufferCount { get { return Real.ColorBufferCount; } }
        public int ColorBufferFormatID { get { return Real.ColorBufferFormatID; } }
        public bool EnableAutoDepthStencil { get { return Real.EnableAutoDepthStencil; } }
        public int DepthStencilFormatID { get { return Real.DepthStencilFormatID; } }
        public Sampling Sampling { get { return Real.Sampling; } }
        public bool VerticalSync { get { return Real.VerticalSync; } }
        public IWindow Window { get { return Wrappers.Get(Real.Window); } }
        public IDevice Device { get { return Wrappers.Get(Real.Device); } }
        public void GetDescription(out SwapChainDescription description) { Real.GetDescription(out description); }

        public bool BeginScene()
        {
            if (InsideBeginEnd)
                throw new InvalidOperationException("Trying to call SwapChain.BeginScene() while already inside a Begin-End pair.");
            InsideBeginEnd = Real.BeginScene();
            return InsideBeginEnd;
        }

        public void EndScene()
        {
            if (!InsideBeginEnd)
                throw new InvalidOperationException("Trying to call SwapChain.EndScene() while not inside a Begin-End pair.");
            InsideBeginEnd = false;
            Real.EndScene();
        }

        public void Present()
        {
            if (InsideBeginEnd)
                throw new InvalidOperationException("Trying to call SwapChain.Present() while inside a Begin-End pair.");
            Real.Present();
        }

        public void Reset(ref SwapChainDescription description)
        {
            if (InsideBeginEnd)
                throw new InvalidOperationException("Trying to call SwapChain.Reset() while inside a Begin-End pair.");

            ValidateDescriptionWindowed(ref description, Device.Adapter);

            Real.Reset(ref description);
        }

        public IRenderTargetView GetCurrentColorBuffer()
        {
            return Wrappers.Get(Real.GetCurrentColorBuffer());
        }

        public IDepthStencilView GetCurrentDepthStencilBuffer()
        {
            if (!Real.EnableAutoDepthStencil)
                throw new InvalidOperationException("Trying to get an auto depth-stencil buffer of a swap chain that does not have one.");
            return Wrappers.Get(Real.GetCurrentDepthStencilBuffer());
        }

        static void ValidateDescriptionCommon(ref SwapChainDescription description, IAdapter adapter)
        {
            Check.Positive(description.ColorBufferCount, "description.ColorBufferCount");

            int depthStencilFormatID = description.DepthStencilFormatID;
            if (description.EnableAutoDepthStencil &&
                adapter.GetSupportedFormats(FormatSupport.DepthStencil).All(fi => fi.ID != depthStencilFormatID))
                throw new ArgumentException(
                    "Given swap chain's depth-stencil format is not supported by the adapter");

            if (description.Sampling != Sampling.NoMultisampling && description.Sampling.Quality >=
                adapter.GetMultisampleQualityLevelsCount(description.ColorBufferFormatID, description.Sampling.Count))
                throw new ArgumentException(
                    "Given swap chain's multisampling is not supported by the given adapter for the given color format");
        }

        public static void ValidateDescriptionWindowed(ref SwapChainDescription description, IAdapter adapter)
        {
            ValidateDescriptionCommon(ref description, adapter);

            var colorFormatID = description.ColorBufferFormatID;
            if (adapter.GetSupportedWindowedDisplayFormats().All(fi => fi.ID != colorFormatID))
                throw new ArgumentException("Given swap chain's color format is not supported by the adapter in windowed mode");
        }

        protected static void ValidateDescriptionFullscreen(ref DisplayMode displayMode, ref SwapChainDescription description, IAdapter adapter)
        {
            // todo: correctly validate multimonitor system

            ValidateDescriptionCommon(ref description, adapter);

            var displayModeLoc = displayMode;
            if (adapter.Outputs.SelectMany(o => o.GetSupportedDisplayModes()).All(dm => !DisplayMode.Equals(ref dm, ref displayModeLoc)))
                throw new ArgumentException("Given display mode is not supported by any of the adapter outputs");
            if (displayMode.FormatID != description.ColorBufferFormatID)
                throw new ArgumentException("Display mode format ID and swap chain descriptopn format ID must be the same when in fullscreen mode");
        }
    }
}