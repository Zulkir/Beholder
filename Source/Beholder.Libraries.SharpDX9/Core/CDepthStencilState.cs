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
using Beholder.Libraries.SharpDX9.Resources;
using Beholder.Utility.ForImplementations;
using Beholder.Utility.ForImplementations.Core;
using SharpDX.Direct3D9;
using StencilOperation = SharpDX.Direct3D9.StencilOperation;

namespace Beholder.Libraries.SharpDX9.Core
{
    class CDepthStencilState : DepthStencilStateBase<ICDevice>, IDefaultPoolResource, IDisposableInternal
    {
        StateBlock stateBlockCw;
        StateBlock stateBlockCcw;

        public CDepthStencilState(ICDevice device, ref DepthStencilDescription desc)
            : base (device, ref desc)
        {
            OnDeviceReset();
        }

        public void Apply(bool frontCounterClockwise)
        {
            if (frontCounterClockwise)
            {
                stateBlockCcw.Apply();
            }
            else
            {
                stateBlockCw.Apply();
            }
        }

        public void OnDeviceLost()
        {
            stateBlockCcw.Dispose();
            stateBlockCw.Dispose();
        }

        public void OnDeviceReset()
        {
            stateBlockCw = CreateStateBlock(false);
            stateBlockCcw = CreateStateBlock(true);
        }

        public void DisposeInternal()
        {
            stateBlockCcw.Dispose();
            stateBlockCw.Dispose();
        }

        StateBlock CreateStateBlock(bool frontCounterClockwise)
        {
            var d3dDevice = device.D3DDevice;

            d3dDevice.BeginStateBlock();
            if (desc.DepthEnable)
            {
                d3dDevice.SetRenderState(RenderState.ZEnable, ZBufferType.UseZBuffer);
                d3dDevice.SetRenderState(RenderState.ZWriteEnable, desc.DepthWriteEnable);
                d3dDevice.SetRenderState(RenderState.ZFunc, (Compare)desc.DepthFunction);
            }
            else
            {
                d3dDevice.SetRenderState(RenderState.ZEnable, ZBufferType.DontUseZBuffer);
            }

            if (desc.StencilEnable)
            {
                d3dDevice.SetRenderState(RenderState.StencilEnable, true);
                d3dDevice.SetRenderState(RenderState.StencilMask, (int)desc.StencilReadMask);
                d3dDevice.SetRenderState(RenderState.StencilWriteMask, (int)desc.StencilWriteMask);

                if (desc.FrontFaceStencilOp.Equals(desc.BackFaceStencilOp))
                {
                    d3dDevice.SetRenderState(RenderState.TwoSidedStencilMode, false);
                    d3dDevice.SetRenderState(RenderState.StencilFail, (StencilOperation)desc.FrontFaceStencilOp.StencilFailOperation);
                    d3dDevice.SetRenderState(RenderState.StencilZFail, (StencilOperation)desc.FrontFaceStencilOp.StencilDepthFailOperation);
                    d3dDevice.SetRenderState(RenderState.StencilPass, (StencilOperation)desc.FrontFaceStencilOp.StencilPassOperation);
                    d3dDevice.SetRenderState(RenderState.StencilFunc, (Compare)desc.FrontFaceStencilOp.StencilFunction);
                }
                else if (frontCounterClockwise)
                {
                    d3dDevice.SetRenderState(RenderState.TwoSidedStencilMode, true);

                    d3dDevice.SetRenderState(RenderState.StencilFail, (StencilOperation)desc.BackFaceStencilOp.StencilFailOperation);
                    d3dDevice.SetRenderState(RenderState.StencilZFail, (StencilOperation)desc.BackFaceStencilOp.StencilDepthFailOperation);
                    d3dDevice.SetRenderState(RenderState.StencilPass, (StencilOperation)desc.BackFaceStencilOp.StencilPassOperation);
                    d3dDevice.SetRenderState(RenderState.StencilFunc, (Compare)desc.BackFaceStencilOp.StencilFunction);

                    d3dDevice.SetRenderState(RenderState.CcwStencilFail, (StencilOperation)desc.FrontFaceStencilOp.StencilFailOperation);
                    d3dDevice.SetRenderState(RenderState.CcwStencilZFail, (StencilOperation)desc.FrontFaceStencilOp.StencilDepthFailOperation);
                    d3dDevice.SetRenderState(RenderState.CcwStencilPass, (StencilOperation)desc.FrontFaceStencilOp.StencilPassOperation);
                    d3dDevice.SetRenderState(RenderState.CcwStencilFunc, (Compare)desc.FrontFaceStencilOp.StencilFunction);
                }
                else
                {
                    d3dDevice.SetRenderState(RenderState.TwoSidedStencilMode, true);

                    d3dDevice.SetRenderState(RenderState.StencilFail, (StencilOperation)desc.FrontFaceStencilOp.StencilFailOperation);
                    d3dDevice.SetRenderState(RenderState.StencilZFail, (StencilOperation)desc.FrontFaceStencilOp.StencilDepthFailOperation);
                    d3dDevice.SetRenderState(RenderState.StencilPass, (StencilOperation)desc.FrontFaceStencilOp.StencilPassOperation);
                    d3dDevice.SetRenderState(RenderState.StencilFunc, (Compare)desc.FrontFaceStencilOp.StencilFunction);

                    d3dDevice.SetRenderState(RenderState.CcwStencilFail, (StencilOperation)desc.BackFaceStencilOp.StencilFailOperation);
                    d3dDevice.SetRenderState(RenderState.CcwStencilZFail, (StencilOperation)desc.BackFaceStencilOp.StencilDepthFailOperation);
                    d3dDevice.SetRenderState(RenderState.CcwStencilPass, (StencilOperation)desc.BackFaceStencilOp.StencilPassOperation);
                    d3dDevice.SetRenderState(RenderState.CcwStencilFunc, (Compare)desc.BackFaceStencilOp.StencilFunction);
                }
            }
            else
            {
                d3dDevice.SetRenderState(RenderState.StencilEnable, false);
            }
            return d3dDevice.EndStateBlock();
        }
    }
}
