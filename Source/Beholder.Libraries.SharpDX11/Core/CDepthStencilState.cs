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
using Beholder.Utility.ForImplementations;
using Beholder.Utility.ForImplementations.Core;
using SharpDX.Direct3D11;
using DepthStencilOperationDescription = SharpDX.Direct3D11.DepthStencilOperationDescription;

namespace Beholder.Libraries.SharpDX11.Core
{
    class CDepthStencilState : DepthStencilStateBase<ICDevice>, IDisposableInternal
    {
        readonly DepthStencilState d3dDepthStencilState;

        internal DepthStencilState D3DDepthStencilState { get { return d3dDepthStencilState; } }

        internal CDepthStencilState(ICDevice device, ref DepthStencilDescription desc)
            : base(device, ref desc)
        {
            var d3dDesc = new DepthStencilStateDescription
            {
                IsDepthEnabled = desc.DepthEnable,
                DepthWriteMask = desc.DepthWriteEnable ? DepthWriteMask.All : DepthWriteMask.Zero,
                DepthComparison = CtSharpDX11.Comparison(desc.DepthFunction),
                IsStencilEnabled = desc.StencilEnable,
                StencilReadMask = desc.StencilReadMask,
                StencilWriteMask = desc.StencilWriteMask,
                FrontFace = new DepthStencilOperationDescription
                {
                    FailOperation = CtSharpDX11.StencilOperation(desc.FrontFaceStencilOp.StencilFailOperation),
                    DepthFailOperation = CtSharpDX11.StencilOperation(desc.FrontFaceStencilOp.StencilDepthFailOperation),
                    PassOperation = CtSharpDX11.StencilOperation(desc.FrontFaceStencilOp.StencilPassOperation),
                    Comparison = CtSharpDX11.Comparison(desc.FrontFaceStencilOp.StencilFunction)
                },
                BackFace = new DepthStencilOperationDescription
                {
                    FailOperation = CtSharpDX11.StencilOperation(desc.BackFaceStencilOp.StencilFailOperation),
                    DepthFailOperation = CtSharpDX11.StencilOperation(desc.BackFaceStencilOp.StencilDepthFailOperation),
                    PassOperation = CtSharpDX11.StencilOperation(desc.BackFaceStencilOp.StencilPassOperation),
                    Comparison = CtSharpDX11.Comparison(desc.BackFaceStencilOp.StencilFunction)
                }
            };

            d3dDepthStencilState = new DepthStencilState(device.D3DDevice, ref d3dDesc);
        }

        public void DisposeInternal()
        {
            d3dDepthStencilState.Dispose();
        }
    }
}
