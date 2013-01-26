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

using System;
using Beholder.Core;
using Beholder.Libraries.SharpDX9.Resources;
using Beholder.Utility.ForImplementations.Core;
using SharpDX.Direct3D9;
using Blend = SharpDX.Direct3D9.Blend;
using BlendOperation = SharpDX.Direct3D9.BlendOperation;

namespace Beholder.Libraries.SharpDX9.Core
{
    class CBlendState : BlendStateBase<ICDevice>, IDefaultPoolResource, IDisposable
    {
        StateBlock stateBlock;

        public CBlendState(ICDevice device, ref BlendDescription desc) : base(device, ref desc)
        {
            if (desc.AlphaToCoverageEnable || desc.IndependentBlendEnable)
                throw new NotSupportedException("Alpha-to-coverage and independent blend are not yet supported by the SharpDX9 implementation of Beholder.");
            OnDeviceReset();
        }
        
        public void Apply()
        {
            stateBlock.Apply();
        }

        public void Dispose()
        {
            stateBlock.Dispose();
        }

        public void OnDeviceLost()
        {
            stateBlock.Dispose();
        }

        public void OnDeviceReset()
        {
            var d3dDevice = device.D3DDevice;
            d3dDevice.BeginStateBlock();
            if (desc.RenderTargetBlend0.SourceBlend != Beholder.Core.Blend.One || desc.RenderTargetBlend0.DestinationBlend != Beholder.Core.Blend.Zero || desc.RenderTargetBlend0.BlendOperation != Beholder.Core.BlendOperation.Add ||
                desc.RenderTargetBlend0.SourceBlendAlpha != Beholder.Core.Blend.One || desc.RenderTargetBlend0.DestinationBlendAlpha != Beholder.Core.Blend.Zero || desc.RenderTargetBlend0.BlendOperationAlpha != Beholder.Core.BlendOperation.Add)
            {
                d3dDevice.SetRenderState(RenderState.AlphaBlendEnable, true);

                d3dDevice.SetRenderState(RenderState.SourceBlend, (Blend)desc.RenderTargetBlend0.SourceBlend);
                d3dDevice.SetRenderState(RenderState.DestinationBlend, (Blend)desc.RenderTargetBlend0.DestinationBlend);
                d3dDevice.SetRenderState(RenderState.BlendOperation, (BlendOperation)desc.RenderTargetBlend0.BlendOperation);

                if (desc.RenderTargetBlend0.SourceBlendAlpha != desc.RenderTargetBlend0.SourceBlend ||
                    desc.RenderTargetBlend0.DestinationBlendAlpha != desc.RenderTargetBlend0.DestinationBlend)
                {
                    d3dDevice.SetRenderState(RenderState.SeparateAlphaBlendEnable, true);
                    d3dDevice.SetRenderState(RenderState.SourceBlendAlpha, (Blend)desc.RenderTargetBlend0.SourceBlendAlpha);
                    d3dDevice.SetRenderState(RenderState.DestinationBlendAlpha, (Blend)desc.RenderTargetBlend0.DestinationBlendAlpha);
                    d3dDevice.SetRenderState(RenderState.BlendOperationAlpha, (Blend)desc.RenderTargetBlend0.BlendOperationAlpha);
                }
                else
                {
                    d3dDevice.SetRenderState(RenderState.SeparateAlphaBlendEnable, false);
                }
            }
            else
            {
                d3dDevice.SetRenderState(RenderState.AlphaBlendEnable, false);
            }

            d3dDevice.SetRenderState(RenderState.ColorWriteEnable, (ColorWriteEnable)desc.RenderTargetBlend0.RenderTargetWriteMask);
            stateBlock = d3dDevice.EndStateBlock();
        }
    }
}
