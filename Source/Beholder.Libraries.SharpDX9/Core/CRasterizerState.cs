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
using Beholder.Libraries.SharpDX9.Resources;
using Beholder.Math;
using Beholder.Utility.ForImplementations;
using Beholder.Utility.ForImplementations.Core;
using SharpDX.Direct3D9;
using Cull = Beholder.Core.Cull;
using FillMode = SharpDX.Direct3D9.FillMode;

namespace Beholder.Libraries.SharpDX9.Core
{
    class CRasterizerState : RasterizerStateBase<ICDevice>, IDefaultPoolResource, IDisposableInternal
    {
        StateBlock stateBlock;

        public CRasterizerState(ICDevice device, ref RasterizerDescription desc) : base(device, ref desc)
        {
            OnDeviceReset();
        }

        public void Apply()
        {
            stateBlock.Apply();
        }

        public void DisposeInternal()
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
            d3dDevice.SetRenderState(RenderState.FillMode, (FillMode)desc.FillMode);
            switch (desc.CullMode)
            {
                case Cull.None:
                    d3dDevice.SetRenderState(RenderState.CullMode, SharpDX.Direct3D9.Cull.None);
                    break;
                case Cull.Backface:
                    d3dDevice.SetRenderState(RenderState.CullMode, desc.FrontFaceWinding == Winding.CounterClockwise ? SharpDX.Direct3D9.Cull.Clockwise : SharpDX.Direct3D9.Cull.Counterclockwise);
                    break;
                case Cull.Frontface:
                    d3dDevice.SetRenderState(RenderState.CullMode, desc.FrontFaceWinding == Winding.CounterClockwise ? SharpDX.Direct3D9.Cull.Counterclockwise : SharpDX.Direct3D9.Cull.Clockwise);
                    break;
            }
            d3dDevice.SetRenderState(RenderState.DepthBias, 4e-5f * desc.DepthBias);
            if (desc.DepthBiasClamp != 0.0f) 
                throw new NotSupportedException("DepthBiasClamp is not supported in Direct3D9");
            d3dDevice.SetRenderState(RenderState.SlopeScaleDepthBias, (int)(desc.SlopeScaledDepthBias + 0.5f));
            d3dDevice.SetRenderState(RenderState.Clipping, desc.DepthClipEnable);
            d3dDevice.SetRenderState(RenderState.ScissorTestEnable, desc.ScissorEnable);
            d3dDevice.SetRenderState(RenderState.MultisampleAntialias, desc.MultisampleEnable);
            d3dDevice.SetRenderState(RenderState.AntialiasedLineEnable, desc.AntialiasedLineEnable);
            stateBlock = d3dDevice.EndStateBlock();
        }
    }
}
