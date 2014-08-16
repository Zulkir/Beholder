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
using Beholder.Core;
using Beholder.Libraries.ObjectGL4;
using Beholder.Libraries.ObjectGL4.Platform;
using Beholder.Platform;
using Beholder.Utility.ForImplementations.Platform;
using ObjectGL.GL42;
using OpenTK;

namespace Beholder.Eyes.ObjectGL4.Default
{
    class DefaultDevice : ICDevice, IDisposable
    {
        readonly DefaultEye eye;
        readonly CAdapter adapter;
        readonly DeviceInitializationFlags flags;

        readonly Context glContext;
        readonly DefaultWindow primaryWindow;
        readonly CDeviceChildCreator creator;
        readonly CDeviceContext immediateContext;

        public Context GetCurrentContext() { return glContext; }

        public IEye Eye { get { return eye; } }
        public IAdapter Adapter { get { return adapter; } }
        public DeviceInitializationFlags Flags { get { return flags; } }
        public IPrimarySwapChain PrimarySwapChain { get { return primaryWindow; } }
        public DefaultWindow DefaultWindow { get { return primaryWindow; } }
        public IDeviceChildCreator Create { get { return creator; } }
        public CDeviceContext ImmediateContext { get { return immediateContext; } }
        IDeviceContext IDevice.ImmediateContext { get { return immediateContext; } }

        public DefaultDevice(DefaultEye eye, CAdapter adapter, DeviceInitializationFlags flags,
            WindowHandle windowHandle, ref SwapChainDescription implicitSwapChainDescription,
            IFileSystem fileSystem)
        {
            this.eye = eye;
            this.adapter = adapter;
            this.flags = flags;

            primaryWindow = new DefaultWindow(this, windowHandle.ClientWidth, windowHandle.ClientHeight,
                ref implicitSwapChainDescription, windowHandle.Title, GameWindowFlags.Default,
                ((COutput)adapter.Outputs[0]).GLDisplayDevice, ref glContext);

            creator = new CDeviceChildCreator(this);
            immediateContext = new CDeviceContext(this, glContext);
        }

        public IAdditionalSwapChain CreateAdditionalSwapChain(IWindowHandle windowHandle, SwapChainDescription swapChainDesc)
        {
            throw new NotSupportedException("Additional swap chains are not yet supported by the ObjectGL.Default eye");
        }

        public void Dispose()
        {
            immediateContext.Dispose();
            creator.Dispose();
            primaryWindow.Close();
            primaryWindow.Dispose();
        }
    }
}
