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

namespace Beholder.Validation.Core
{
    class DepthStencilState : DeviceChild<IDepthStencilState>, IDepthStencilState
    {
        public DepthStencilState(IDepthStencilState real)
            : base(real)
        {
            
        }

        public DepthStencilOperationDescription FrontFaceStencilOp { get { return Real.FrontFaceStencilOp; } }
        public DepthStencilOperationDescription BackFaceStencilOp { get { return Real.BackFaceStencilOp; } }
        public bool DepthEnable { get { return Real.DepthEnable; } }
        public bool DepthWriteEnable { get { return Real.DepthWriteEnable; } }
        public Comparison DepthFunction { get { return Real.DepthFunction; } }
        public bool StencilEnable { get { return Real.StencilEnable; } }
        public byte StencilReadMask { get { return Real.StencilReadMask; } }
        public byte StencilWriteMask { get { return Real.StencilWriteMask; } }
        public void GetDescription(out DepthStencilDescription description) { Real.GetDescription(out description); }

        public static void ValidateDscription(DepthStencilDescription description)
        {
            if (!description.DepthEnable) 
                return;

            Check.Enumeration(description.FrontFaceStencilOp.StencilFailOperation, "description.FrontFaceStencilOp.StencilFailOperation");
            Check.Enumeration(description.FrontFaceStencilOp.StencilDepthFailOperation, "description.FrontFaceStencilOp.StencilDepthFailOperation");
            Check.Enumeration(description.FrontFaceStencilOp.StencilPassOperation, "description.FrontFaceStencilOp.StencilPassOperation");
            Check.Enumeration(description.FrontFaceStencilOp.StencilFunction, "description.FrontFaceStencilOp.StencilFunction");

            Check.Enumeration(description.BackFaceStencilOp.StencilFailOperation, "description.BackFaceStencilOp.StencilFailOperation");
            Check.Enumeration(description.BackFaceStencilOp.StencilDepthFailOperation, "description.BackFaceStencilOp.StencilDepthFailOperation");
            Check.Enumeration(description.BackFaceStencilOp.StencilPassOperation, "description.BackFaceStencilOp.StencilPassOperation");
            Check.Enumeration(description.BackFaceStencilOp.StencilFunction, "description.BackFaceStencilOp.StencilFunction");

            Check.Enumeration(description.DepthFunction, "description.DepthFunction");
        }
    }
}
