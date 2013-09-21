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
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Beholder.Libraries.SharpDX9.Platform;
using Beholder.Libraries.Windows.Forms;
using Beholder.Platform;
using SharpDX.Direct3D9;

namespace Beholder.Eyes.SharpDX9.Winforms
{
    public class WinformsEye : IEye
    {
        readonly Direct3D direct3D;
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
            direct3D = new Direct3D();
            adapters = direct3D.Adapters.Select(a => new CAdapter(direct3D, a)).ToArray();
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
                ClientSize = new Size(clientWidth, clientHeight),
                Text = title,
                FormBorderStyle = isResizable ? FormBorderStyle.Sizable : FormBorderStyle.FixedSingle
            });
        }

        public IWindowHandle PrepareExistingWindow(object platformWindow)
        {
            if (!(platformWindow is Control)) 
                throw new ArgumentException("windowObject argument has to be of type Windows.Forms.Control for the SharpDX9 Winforms eye.");
            return new ControlWindow((Control)platformWindow);
        }

        public void Initialize(
            IAdapter adapter, 
            IWindowHandle windowHandle, 
            SwapChainDescription primarySwapChainDescription,
            DeviceInitializationFlags initializationFlags,
            IFileSystem fileSystem)
        {
            var cAdapter = (CAdapter)adapter;

            var primaryWindow = (ControlWindow)windowHandle;
            device = new WinformsDevice(this, direct3D, cAdapter, primaryWindow, primarySwapChainDescription, initializationFlags, fileSystem);
            primaryWindow.SetSwapChain(device);
            primaryWindow.Control.Show();

            windows.Add(primaryWindow);

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
                gameLoop.Exit();
        }

        public void RunLoop(IWindow mainWindow)
        {
            if (mainWindow != null)
            {
                if (!(mainWindow is ControlWindow)) 
                    throw new ArgumentException("mainWindow argument has to be of type WinformsControlWindow for the SharpDX9 Winforms eye.");
                ((ControlWindow)mainWindow).FormClosing += OnMainFormClosing;
            }

            gameLoop = new GameLoop(null, OnNewFrame);
            gameLoop.Run();
        }

        public void TerminateLoop()
        {
            if (!PreventExit())
                gameLoop.Exit();
        }

        public void Dispose()
        {
            if (IsDisposed) throw new InvalidOperationException("WinformsEye was already disposed");

            if (device != null) device.Dispose();
            if (direct3D != null) direct3D.Dispose();

            IsDisposed = true;
        }
    }
}
