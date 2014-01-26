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
using Beholder.Math;

namespace Beholder.Validation.Core
{
    class SamplerState : DeviceChild<ISamplerState>, ISamplerState
    {
        public SamplerState(ISamplerState real)
            : base(real)
        {
            
        }

        public Filter Filter { get { return Real.Filter; } }
        public TextureAddressMode AddressU { get { return Real.AddressU; } }
        public TextureAddressMode AddressV { get { return Real.AddressV; } }
        public TextureAddressMode AddressW { get { return Real.AddressW; } }
        public float MipLodBias { get { return Real.MipLodBias; } }
        public int MaximumAnisotropy { get { return Real.MaximumAnisotropy; } }
        public Comparison ComparisonFunction { get { return Real.ComparisonFunction; } }
        public Color4 BorderColor { get { return Real.BorderColor; } }
        public float MinimumLod { get { return Real.MinimumLod; } }
        public float MaximumLod { get { return Real.MaximumLod; } }
        public void GetDescription(out SamplerDescription description) { Real.GetDescription(out description); }

        public static void ValidateDescription(SamplerDescription description)
        {
            Check.Enumeration(description.Filter, "description.Filter");
            Check.Enumeration(description.AddressU, "description.AddressU");
            Check.Enumeration(description.AddressV, "description.AddressV");
            Check.Enumeration(description.AddressW, "description.AddressW");
            if (description.MaximumAnisotropy < 1 || description.MaximumAnisotropy > 16)
                throw new ArgumentException("description.MaximumAnisotropy must be in the range from 1 to 16");
            Check.Enumeration(description.ComparisonFunction, "description.ComparisonFunction");
            if (description.MinimumLod > description.MaximumLod)
                throw new ArgumentException("description.MinimumLod cannot be greater than description.MaximumLod");
        }
    }
}
