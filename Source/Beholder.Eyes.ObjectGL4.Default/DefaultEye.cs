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
using System.Collections.Generic;
using Beholder.Libraries.ObjectGL4.Platform;
using Beholder.Platform;
using Beholder.Utility.ForImplementations.Platform;

namespace Beholder.Eyes.ObjectGL4.Default
{
    public class DefaultEye : IEye
    {
        readonly CAdapter[] adapters = new[] { new CAdapter() };
        DefaultDevice device;
        DefaultWindow loopWindow;
        LoopTimer loopTimer;
        bool initialized;
        bool isDisposed;

        public IReadOnlyList<IAdapter> Adapters { get { return adapters; } }

        public IWindowHandle PrepareExistingWindow(object platformWindow)
        {
            throw new NotImplementedException();
        }

        public IWindowHandle CreateNewWindow(int clientWidth, int clientHeight, string title, bool isResizable)
        {
            return new WindowHandle(clientWidth, clientHeight, title, isResizable);
        }

        public IDevice Device { get { return device; } }
        public bool IsInitialized { get { return initialized; } }
        public bool IsDisposed { get { return isDisposed; } }

        public void Initialize(IAdapter adapter,
            IWindowHandle windowHandle, 
            SwapChainDescription primarySwapChainDesc,
            DeviceInitializationFlags initializationFlags,
            IFileSystem fileSystem)
        {
            if (adapter != adapters[0])
                throw new ArgumentException("Provided adapter is not on of this eye's adapters");

            device = new DefaultDevice(this, (CAdapter)adapter, initializationFlags, (WindowHandle)windowHandle, ref primarySwapChainDesc, fileSystem);

            initialized = true;
        }

        public void RunLoop(IWindow mainWindow)
        {
            if (!initialized)
                throw new NotSupportedException("Starting a loop without creating a device and primary swap chain is not supported by ObjectGl4 DefaultEye");
            if (!(mainWindow is DefaultWindow))
                throw new NotSupportedException("For ObjectGL4 DefaultEye, 'loopWindow' must be of type " + typeof(DefaultWindow).FullName);

            loopTimer = new LoopTimer();

            loopWindow = (DefaultWindow)mainWindow;
            loopWindow.RenderFrame += (sender, args) =>
            {
                loopTimer.UpdateTime();
                if (NewFrame != null) NewFrame(loopTimer.RealTime);
            };
            loopWindow.Closing += (sender, args) =>
            {
                var e = new PreventEventArgs();
                if (LoopTerminating != null) LoopTerminating(e);
                if (e.Prevent)
                    args.Cancel = true;
            };
            loopWindow.Run();
        }

        public void TerminateLoop()
        {
            device.DefaultWindow.Close();
        }

        public event Action<IRealTime> NewFrame;
        public event Action<PreventEventArgs> LoopTerminating;

        public void Dispose()
        {
            if (device != null) 
                device.Dispose();
            isDisposed = true;
        }
    }
}
