﻿/*
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

namespace Beholder.Utility.ForImplementations.Core
{
    public class BlendStateBase<TDevice> : DeviceChildBase<TDevice>, IBlendState where TDevice : IDevice
    {
        protected BlendDescription desc;

        public RenderTargetBlendDescription GetDescriptionForTarget(int renderTargetIndex)
        {
            RenderTargetBlendDescription rtDesc;
            BlendDescription.GetRenderTargetBlend(ref desc, renderTargetIndex, out rtDesc);
            return rtDesc;
        }
        public bool AlphaToCoverageEnable { get { return desc.AlphaToCoverageEnable; } }
        public bool IndependentBlendEnable { get { return desc.IndependentBlendEnable; } }
        public void GetDescription(out BlendDescription description) { description = desc; }

        public BlendStateBase(TDevice device, ref BlendDescription desc) : base(device)
        {
            this.desc = desc;
        }
    }
}
