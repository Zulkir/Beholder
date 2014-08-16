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
using RenderTargetBlendDescription = Beholder.Core.RenderTargetBlendDescription;

namespace Beholder.Libraries.SharpDX11.Core
{
    class CBlendState : BlendStateBase<ICDevice>, IDisposableInternal
    {
        readonly BlendState d3dBlendState;

        internal BlendState D3DBlendState { get { return d3dBlendState; } }

        internal CBlendState(ICDevice device, ref BlendDescription desc) : base(device, ref desc)
        {
            var d3dDesc = new BlendStateDescription
            {
                AlphaToCoverageEnable = desc.AlphaToCoverageEnable,
                IndependentBlendEnable = desc.IndependentBlendEnable
            };

            int rtStatesCount = desc.IndependentBlendEnable ? 8 : 1;

            for (int i = 0; i < rtStatesCount; i++)
            {
                RenderTargetBlendDescription bRtDesc;
                BlendDescription.GetRenderTargetBlend(ref desc, i, out bRtDesc);

                d3dDesc.RenderTarget[i] = new SharpDX.Direct3D11.RenderTargetBlendDescription
                {
                    IsBlendEnabled = bRtDesc.BlendEnable,
                    SourceBlend = CtSharpDX11.BlendOption(bRtDesc.SourceBlend),
                    DestinationBlend = CtSharpDX11.BlendOption(bRtDesc.DestinationBlend),
                    BlendOperation = CtSharpDX11.BlendOperation(bRtDesc.BlendOperation),
                    SourceAlphaBlend = CtSharpDX11.BlendOption(bRtDesc.SourceBlendAlpha),
                    DestinationAlphaBlend = CtSharpDX11.BlendOption(bRtDesc.DestinationBlendAlpha),
                    AlphaBlendOperation = CtSharpDX11.BlendOperation(bRtDesc.BlendOperationAlpha),
                    RenderTargetWriteMask = CtSharpDX11.ColorWriteMaskFlags(bRtDesc.RenderTargetWriteMask)
                };
            }

            d3dBlendState = new BlendState(device.D3DDevice, d3dDesc);
        }

        public void DisposeInternal()
        {
            d3dBlendState.Dispose();
        }
    }
}
