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

namespace Beholder.Validation.Core
{
    class BlendState : DeviceChild<IBlendState>, IBlendState
    {
        public BlendState(IBlendState real) : base(real) {}

        public RenderTargetBlendDescription GetDescriptionForTarget(int renderTargetIndex) { return Real.GetDescriptionForTarget(renderTargetIndex); }
        public bool AlphaToCoverageEnable { get { return Real.AlphaToCoverageEnable; } }
        public bool IndependentBlendEnable { get { return Real.IndependentBlendEnable; } }
        public void GetDescription(out BlendDescription description) { Real.GetDescription(out description); }

        public static void ValidateDescription(BlendDescription description)
        {
            for (int i = 0; i < BlendDescription.MaxRenderTargetBlendCount; i++)
                ValidateRenderTargetBlendDescription(description.GetRenderTargetBlend(0));
        }

        static void ValidateRenderTargetBlendDescription(RenderTargetBlendDescription description)
        {
            if (!description.BlendEnable)
                return;
            Check.Enumeration(description.SourceBlend, "description.SourceBlend");
            Check.Enumeration(description.DestinationBlend, "description.DestinationBlend");
            Check.Enumeration(description.BlendOperation, "description.BlendOperation");
            Check.Enumeration(description.SourceBlendAlpha, "description.SourceBlendAlpha");
            Check.Enumeration(description.DestinationBlendAlpha, "description.DestinationBlendAlpha");
            Check.Enumeration(description.BlendOperationAlpha, "description.BlendOperationAlpha");
            Check.Flags(description.RenderTargetWriteMask, "description.RenderTargetWriteMask");
        }
    }
}
