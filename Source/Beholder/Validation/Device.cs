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

using Beholder.Core;
using Beholder.Platform;
using Beholder.Validation.Platform;

namespace Beholder.Validation
{
    class Device : Wrapper<IDevice>, IDevice
    {
        public Device(IDevice real) : base(real) { }

        public IEye Eye { get { return Wrappers.Get(Real.Eye); } }
        public IAdapter Adapter { get { return Wrappers.Get(Real.Adapter); } }
        public DeviceInitializationFlags Flags { get { return Real.Flags; } }
        public IDeviceChildCreator Create { get { return Wrappers.Get(Real.Create); } }
        public IDeviceChildLoader Load { get { return Wrappers.Get(Real.Load); } }
        public IPrimarySwapChain PrimarySwapChain { get { return Wrappers.Get(Real.PrimarySwapChain); } }
        public IDeviceContext ImmediateContext { get { return Wrappers.Get(Real.ImmediateContext); } }

        public IAdditionalSwapChain CreateAdditionalSwapChain(IWindowHandle windowHandle, SwapChainDescription swapChainDesc)
        {
            Check.ExistingInternal(windowHandle, "windowHandle");
            SwapChain.ValidateDescriptionWindowed(ref swapChainDesc, Adapter);
            return Wrappers.Get(Real.CreateAdditionalSwapChain(windowHandle.GetReal(), swapChainDesc));
        }
    }
}
