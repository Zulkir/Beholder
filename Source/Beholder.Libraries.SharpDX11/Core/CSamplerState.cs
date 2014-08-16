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
using Beholder.Utility.ForImplementations;
using Beholder.Utility.ForImplementations.Core;
using SharpDX.Direct3D11;

namespace Beholder.Libraries.SharpDX11.Core
{
    class CSamplerState : SamplerStateBase<ICDevice>, IDisposableInternal
    {
        readonly SamplerState d3dSamplerState;

        internal SamplerState D3DSamplerState { get { return d3dSamplerState; } }

        internal CSamplerState(ICDevice device, ref SamplerDescription desc) : base(device, ref desc)
        {
            var d3dDesc = new SamplerStateDescription
            {
                Filter = CtSharpDX11.Filter(desc.Filter),
                AddressU = CtSharpDX11.TextureAddressMode(desc.AddressU),
                AddressV = CtSharpDX11.TextureAddressMode(desc.AddressV),
                AddressW = CtSharpDX11.TextureAddressMode(desc.AddressW),
                MipLodBias = desc.MipLodBias,
                MaximumAnisotropy = desc.MaximumAnisotropy,
                ComparisonFunction = CtSharpDX11.Comparison(desc.ComparisonFunction),
                BorderColor = new SharpDX.Color4 { Red = desc.BorderColor.Data.X, Green = desc.BorderColor.Data.Y, Blue = desc.BorderColor.Data.Z, Alpha = desc.BorderColor.Data.W },
                MinimumLod = desc.MinimumLod,
                MaximumLod = desc.MaximumLod
            };

            d3dSamplerState = new SamplerState(device.D3DDevice, d3dDesc);
        }

        public void DisposeInternal()
        {
            d3dSamplerState.Dispose();
        }
    }
}
