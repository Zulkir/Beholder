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
using Beholder.Utility.ForImplementations;
using Beholder.Utility.ForImplementations.Core;
using SharpDX.Direct3D11;

namespace Beholder.Libraries.SharpDX11.Core
{
    class CRasterizerState : RasterizerStateBase<ICDevice>, IDisposableInternal
    {
        readonly RasterizerState d3dRasterizerState;

        internal RasterizerState D3DRasterizerState { get { return d3dRasterizerState; } }

        internal CRasterizerState(ICDevice device, ref RasterizerDescription desc)
            : base(device, ref desc)
        {
            var d3dDesc = new RasterizerStateDescription
            {
                FillMode = CtSharpDX11.FillMode(desc.FillMode),
                CullMode = CtSharpDX11.CullMode(desc.CullMode),
                IsFrontCounterClockwise = desc.FrontFaceWinding == Winding.CounterClockwise,
                DepthBias = desc.DepthBias,
                DepthBiasClamp = desc.DepthBiasClamp,
                SlopeScaledDepthBias = desc.SlopeScaledDepthBias,
                IsDepthClipEnabled = desc.DepthClipEnable,
                IsScissorEnabled = desc.ScissorEnable,
                IsMultisampleEnabled = desc.MultisampleEnable,
                IsAntialiasedLineEnabled = desc.AntialiasedLineEnable
            };

            d3dRasterizerState = new RasterizerState(device.D3DDevice, ref d3dDesc);
        }

        public void DisposeInternal()
        {
            d3dRasterizerState.Dispose();
        }
    }
}
