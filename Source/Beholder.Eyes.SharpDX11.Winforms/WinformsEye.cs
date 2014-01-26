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
using System.Linq;
using System.Windows.Forms;
using Beholder.Libraries.SharpDX11.Platform;
using Beholder.Platform;
using SharpDX.DXGI;
using Beholder.Libraries.Windows.Forms;
using BSwapChainDescription = Beholder.Platform.SwapChainDescription;

namespace Beholder.Eyes.SharpDX11.Winforms
{
    public class WinformsEye : IEye
    {
        readonly Factory1 factory;
        readonly CAdapter[] adapters;
        readonly List<ControlWindow> windows;

        WinformsDevice device;
        GameLoop gameLoop;

        public event Action<IRealTime> NewFrame;
        public event Action<PreventEventArgs> LoopTerminating;

        public IReadOnlyList<IAdapter> Adapters { get { return adapters; } }
        public bool IsInitialized { get; private set; }
        public bool IsDisposed { get; private set; }
        public IDevice Device { get { return device; } }

        public WinformsEye()
        {
            factory = new Factory1();
            adapters = Enumerable.Range(0, factory.GetAdapterCount1()).Select(i => new CAdapter(factory.GetAdapter1(i), i)).ToArray();
            windows = new List<ControlWindow>();
        }

        void OnNewFrame(RealTime realTime)
        {
            foreach (var window in windows)
                window.OnNewFrame();

            if (NewFrame != null)
                NewFrame(realTime);
        }

        public IWindowHandle CreateNewWindow(int clientWidth, int clientHeight, string title, bool isResizable)
        {
            return new ControlWindow(new GameForm
            {
                ClientSize = new System.Drawing.Size(clientWidth, clientHeight),
                Text = title,
                FormBorderStyle = isResizable ? FormBorderStyle.Sizable : FormBorderStyle.FixedSingle
            });
        }

        public IWindowHandle PrepareExistingWindow(object platformWindow)
        {
            if (!(platformWindow is Control))
                throw new ArgumentException("windowObject argument has to be of type Windows.Forms.Control for the SlimDX9 Winforms eye.");
            return new ControlWindow((Control)platformWindow);
        }

        public void Initialize(
            IAdapter adapter,
            IWindowHandle windowHandle, 
            BSwapChainDescription primarySwapChainDesc,
            DeviceInitializationFlags initializationFlags,
            IFileSystem fileSystem)
        {
            var cAdapter = (CAdapter)adapter;
            var window = (ControlWindow)windowHandle;
            device = new WinformsDevice(factory, this, cAdapter, window, primarySwapChainDesc, initializationFlags, fileSystem);
            window.SetSwapChain(device.PrimarySwapChain);
            windows.Add(window);

            window.Control.Show();

            IsInitialized = true;
        }

        // Returns whether the exit was prevented.
        bool PreventExit()
        {
            if (LoopTerminating == null) return false;

            var preventArgs = new PreventEventArgs();
            LoopTerminating(preventArgs);

            return preventArgs.Prevent;
        }

        void OnMainFormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = PreventExit();

            if (!e.Cancel)
            {
                gameLoop.Exit();
            }
        }

        public void RunLoop(IWindow mainWindow)
        {
            if (mainWindow != null)
            {
                if (!(mainWindow is ControlWindow)) throw new ArgumentException("mainWindow argument has to be of type WinformsControlWindow for the SlimDX9 Winforms eye.");
                ((ControlWindow)mainWindow).FormClosing += OnMainFormClosing;
            }

            gameLoop = new GameLoop(null, OnNewFrame);
            gameLoop.Run();
        }

        public void TerminateLoop()
        {
            if (!PreventExit())
            {
                gameLoop.Exit();
            }
        }

        public void Dispose()
        {
            if (IsDisposed) throw new InvalidOperationException("WinformsEye was already disposed");

            if (device != null) device.Dispose();
            if (factory != null) factory.Dispose();

            IsDisposed = true;
        }
    }
}
