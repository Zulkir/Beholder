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

using Beholder;
using Beholder.Core;
using Beholder.Input;
using Beholder.Platform;

namespace Launcher
{
    abstract class Scene
    {
        protected IEye Eye { get; private set; }
        protected IAdapter Adapter { get; set; }
        protected DisplayMode DesctopDisplayMode { get; set; }
        protected IDevice Device { get; private set; }
        protected IDeviceContext ImmediateContext { get; private set; }
        protected ISwapChain SwapChain { get; private set; }
        protected IWindow Window { get; set; }
        protected IKeyboardHandler Keyboard { get; set; }
        protected IMouseHandler Mouse { get; set; }

        protected Scene(IEye eye, DisplayMode desctopDisplayMode)
        {
            Eye = eye;
            Adapter = eye.Device.Adapter;
            Device = eye.Device;
            DesctopDisplayMode = desctopDisplayMode;
            ImmediateContext = Device.ImmediateContext;
            SwapChain = Device.PrimarySwapChain;
            Window = SwapChain.Window;
            Keyboard = Window.Input.Keyboard;
            Mouse = Window.Input.Mouse;
        }

        public abstract void NewFrame(IRealTime realTime);
    }
}
