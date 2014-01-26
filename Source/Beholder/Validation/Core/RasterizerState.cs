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

namespace Beholder.Validation.Core
{
    class RasterizerState : DeviceChild<IRasterizerState>, IRasterizerState
    {
        public RasterizerState(IRasterizerState real) 
            : base(real)
        {
            
        }

        public int DepthBias { get { return Real.DepthBias; } }
        public float DepthBiasClamp { get { return Real.DepthBiasClamp; } }
        public float SlopeScaledDepthBias { get { return Real.SlopeScaledDepthBias; } }
        public FillMode FillMode { get { return Real.FillMode; } }
        public Cull CullMode { get { return Real.CullMode; } }
        public bool DepthClipEnable { get { return Real.DepthClipEnable; } }
        public bool ScissorEnable { get { return Real.ScissorEnable; } }
        public bool MultisampleEnable { get { return Real.MultisampleEnable; } }
        public bool AntialiasedLineEnable { get { return Real.AntialiasedLineEnable; } }
        public Winding FrontFaceWinding { get { return Real.FrontFaceWinding; } }
        public void GetDescription(out RasterizerDescription description) { Real.GetDescription(out description); }

        public static void ValidateDescription(RasterizerDescription description)
        {
            Check.Enumeration(description.FillMode, "description.FillMode");
            Check.Enumeration(description.CullMode, "description.CullMode");
            Check.Enumeration(description.FrontFaceWinding, "description.FrontFaceWinding");
        }
    }
}
