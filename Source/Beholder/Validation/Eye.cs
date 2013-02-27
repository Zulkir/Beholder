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
using Beholder.Utility.Collections.Readonly;
using Beholder.Validation.Platform;

namespace Beholder.Validation
{
    public class Eye : Wrapper<IEye>, IEye
    {
        readonly ReadonlyArrayWrapper<Adapter> roAdapters;
        bool loopIsRunning;

        public Eye(IEye real) : base (real)
        {
            roAdapters = new ReadonlyArrayWrapper<Adapter>(Real.Adapters.Select(Wrappers.Get).Cast<Adapter>().ToArray());
            Wrappers.InitializeEye(this);
        }

        public IReadonlyList<IAdapter> Adapters { get { return roAdapters; } }
        public bool IsInitialized { get { return Real.IsInitialized; } }
        public bool IsDisposed { get { return Real.IsDisposed; } }

        public IWindowHandle PrepareExistingWindow(object windowObject)
        {
            Check.NotNull(windowObject, "windowObject");
            return Wrappers.Get(Real.PrepareExistingWindow(windowObject));
        }

        public IWindowHandle CreateNewWindow(int clientWidth, int clientHeight, string title, bool isResizable)
        {
            Check.Positive(clientWidth, "clientWidth");
            Check.Positive(clientHeight, "clientHeight");
            Check.NotNull(title, "title");
            return Wrappers.Get(Real.CreateNewWindow(clientWidth, clientHeight, title, isResizable));
        }

        public void Initialize(
            IAdapter adapter, 
            IWindowHandle windowHandle, 
            SwapChainDescription primarySwapChainDesc, 
            DeviceInitializationFlags initializationFlags,
            IFileSystem fileSystem)
        {
            Check.ExistingInternal(adapter, "adapter");
            Check.ExistingInternal(windowHandle, "windowHandle");
            SwapChain.ValidateDescriptionWindowed(ref primarySwapChainDesc, Wrappers.Get(adapter));
            Real.Initialize(adapter.GetReal(), ((WindowHandle)windowHandle).Real, primarySwapChainDesc, initializationFlags, fileSystem);
        }

        public IDevice Device
        {
            get
            {
                if (!IsInitialized)
                    throw new InvalidOperationException("Trying to get a device while the eye is not yet initialized");
                return Wrappers.Get(Real.Device);
            }
        }

        public void RunLoop(IWindow mainWindow)
        {
            Check.NullOrInternal(mainWindow, "mainWindow");
            Real.RunLoop(mainWindow.GetReal());
            loopIsRunning = true;
        }

        public void TerminateLoop()
        {
            if (!loopIsRunning)
                throw new InvalidOperationException("Trying to stop the loop while it is not running");
            Real.TerminateLoop();
        }

        public event Action<IRealTime> NewFrame
        {
            add { Real.NewFrame += value; }
            remove { Real.NewFrame -= value; }
        }

        public event Action<PreventEventArgs> LoopTerminating
        {
            add { Real.LoopTerminating += value; }
            remove { Real.LoopTerminating -= value; }
        }

        public void Dispose()
        {
            if (IsDisposed) 
                throw new InvalidOperationException("Trying to dispose of an IEye that has already been disposed of.");
            Real.Dispose();
        }
    }
}
