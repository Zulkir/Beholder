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

using Beholder.Core;

namespace Beholder.Utility.ForImplementations.Core
{
    public class DepthStencilStateBase<TDevice> : DeviceChildBase<TDevice>, IDepthStencilState where TDevice : IDevice
    {
        protected DepthStencilDescription desc;

        public DepthStencilOperationDescription FrontFaceStencilOp { get { return desc.FrontFaceStencilOp; } }
        public DepthStencilOperationDescription BackFaceStencilOp { get { return desc.BackFaceStencilOp; } }
        public bool DepthEnable { get { return desc.DepthEnable; } }
        public bool DepthWriteEnable { get { return desc.DepthWriteEnable; } }
        public Comparison DepthFunction { get { return desc.DepthFunction; } }
        public bool StencilEnable { get { return desc.StencilEnable; } }
        public byte StencilReadMask { get { return desc.StencilReadMask; } }
        public byte StencilWriteMask { get { return desc.StencilWriteMask; } }
        public void GetDescription(out DepthStencilDescription description) { description = desc; }

        public DepthStencilStateBase(TDevice device, ref DepthStencilDescription desc) : base(device)
        {
            this.desc = desc;
        }
    }
}
