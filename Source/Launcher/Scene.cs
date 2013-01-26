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
using Beholder;
using Beholder.Core;
using Beholder.Input;
using Beholder.Platform;

namespace Launcher
{
    abstract class Scene : IDisposable
    {
        //readonly IEye eye;

        //protected IGraphicsMachine Machine { get; private set; }
        protected IEye Eye { get; private set; }
        protected IAdapter Adapter { get; private set; }
        protected IDevice Device { get; private set; }
        protected DisplayMode DesctopDisplayMode { get; private set; }
        protected int DisplayFormatID { get; private set; }
        protected IDeviceContext ImmediateContext { get; private set; }
        protected IKeyboardHandler Keyboard { get; private set; }
        protected IMouseHandler Mouse { get; private set; }
        protected ISwapChain SwapChain { get; private set; }
        protected IWindow Window { get; private set; }

        protected Scene(IEye eye, int displayFormatID, ISwapChain swapChain)
        {
            Eye = eye;
            Adapter = eye.Device.Adapter;
            Device = eye.Device;
            DisplayFormatID = displayFormatID;

            DesctopDisplayMode = Adapter.Outputs[0].GetSupportedDisplayModes()
                .OrderByDescending(m => m.Width * m.Height)
                .ThenByDescending(m => m.RefreshRate == 60 ? int.MaxValue : m.RefreshRate.Round())
                .ThenBy(m => { var fi = Adapter.GetFormatInfo(m.FormatID); return fi.ColorBits == 24 && fi.TotalBits == 32 ? 0 : 1; })
                .ThenBy(m => Program.FormatTypePriority(Adapter.GetFormatInfo(m.FormatID).ColorFormatType))
                .ThenByDescending(m => Adapter.GetFormatInfo(m.FormatID).ColorBits)
                .First();
            ImmediateContext = Device.ImmediateContext;
            SwapChain = swapChain;
            Window = swapChain.Window;
            Keyboard = Window.Input.Keyboard;
            Mouse = Window.Input.Mouse;

            eye.NewFrame += NewFrame;
        }

        public void Run()
        {
            Initialize();
            Eye.RunLoop(Window);
        }

        public void Dispose()
        {
            Eye.TerminateLoop();
            
        }

        protected abstract void Initialize();

        protected virtual void NewFrame(IRealTime realTime)
        {
            if (Window.Input.Keyboard.KeyReleased(Keys.F))
            {
                SwapChainDescription sd;
                Eye.Device.PrimarySwapChain.GetDescription(out sd);
                DisplayMode dm = DesctopDisplayMode;
                sd.ColorBufferFormatID = dm.FormatID;
                Eye.Device.PrimarySwapChain.ResetToFullscreen(ref dm, ref sd);
            }
            else if (Window.Input.Keyboard.KeyReleased(Keys.W))
            {
                SwapChainDescription sd;
                Eye.Device.PrimarySwapChain.GetDescription(out sd);
                Eye.Device.PrimarySwapChain.Reset(ref sd);
            }
            else if (Window.Input.Keyboard.KeyReleased(Keys.P))
            {
                SwapChainDescription sd;
                Eye.Device.PrimarySwapChain.GetDescription(out sd);
                Eye.Device.PrimarySwapChain.ResetToPseudoFullscreen(ref sd);
            }
        }
    }
}
