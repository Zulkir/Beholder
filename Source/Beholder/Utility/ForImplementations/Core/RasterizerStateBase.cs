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
using Beholder.Math;

namespace Beholder.Utility.ForImplementations.Core
{
    public abstract class RasterizerStateBase<TDevice> : DeviceChildBase<TDevice>, IRasterizerState where TDevice : IDevice
    {
        protected RasterizerDescription desc;

        public int DepthBias { get { return desc.DepthBias; } }
        public float DepthBiasClamp { get { return desc.DepthBiasClamp; } }
        public float SlopeScaledDepthBias { get { return desc.SlopeScaledDepthBias; } }
        public FillMode FillMode { get { return desc.FillMode; } }
        public Cull CullMode { get { return desc.CullMode; } }
        public bool DepthClipEnable { get { return desc.DepthClipEnable; } }
        public bool ScissorEnable { get { return desc.ScissorEnable; } }
        public bool MultisampleEnable { get { return desc.MultisampleEnable; } }
        public bool AntialiasedLineEnable { get { return desc.AntialiasedLineEnable; } }
        public Winding FrontFaceWinding { get { return desc.FrontFaceWinding; } }
        public void GetDescription(out RasterizerDescription description) { description = desc; }

        protected RasterizerStateBase(TDevice device, ref RasterizerDescription desc) : base(device)
        {
            this.desc = desc;
        }
    }
}
