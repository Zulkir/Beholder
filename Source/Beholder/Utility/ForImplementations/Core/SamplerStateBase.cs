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
using Beholder.Math;

namespace Beholder.Utility.ForImplementations.Core
{
    public abstract class SamplerStateBase<TDevice> : DeviceChildBase<TDevice>, ISamplerState where TDevice : IDevice
    {
        protected SamplerDescription desc;

        public Filter Filter { get { return desc.Filter; } }
        public TextureAddressMode AddressU { get { return desc.AddressU; } }
        public TextureAddressMode AddressV { get { return desc.AddressV; } }
        public TextureAddressMode AddressW { get { return desc.AddressW; } }
        public float MipLodBias { get { return desc.MipLodBias; } }
        public int MaximumAnisotropy { get { return desc.MaximumAnisotropy; } }
        public Comparison ComparisonFunction { get { return desc.ComparisonFunction; } }
        public Color4 BorderColor { get { return desc.BorderColor; } }
        public float MinimumLod { get { return desc.MinimumLod; } }
        public float MaximumLod { get { return desc.MaximumLod; } }
        public void GetDescription(out SamplerDescription description) { description = desc; }

        protected SamplerStateBase(TDevice device, ref SamplerDescription desc) : base(device)
        {
            this.desc = desc;
        }
    }
}
