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
using Beholder.Utility.ForImplementations.Core;
using ObjectGL.GL42;

namespace Beholder.Libraries.ObjectGL4.Core
{
    class CBlendState : BlendStateBase<ICDevice>
    {
        readonly bool blendEnable;
        readonly bool alphaToCoverageEnable;
        readonly bool independentBlendEnable;
        readonly RenderTargetBlend[] targetBlends;

        public CBlendState(ICDevice device, ref BlendDescription desc) : base(device, ref desc)
        {
            alphaToCoverageEnable = desc.AlphaToCoverageEnable;
            independentBlendEnable = desc.IndependentBlendEnable;

            if (!desc.IndependentBlendEnable)
            {
                blendEnable = desc.RenderTargetBlend0.BlendEnable;
                targetBlends = new RenderTargetBlend[1];
                targetBlends[0] = new RenderTargetBlend(desc.RenderTargetBlend0);
            }
            else
            {
                targetBlends = new RenderTargetBlend[System.Math.Min(
                    BlendDescription.MaxRenderTargetBlendCount, 
                    device.GetCurrentContext().Pipeline.Blend.Targets.Count)];

                for (int i = 0; i < targetBlends.Length; i++)
                {
                    var targetBlendDesc = BlendDescription.GetRenderTargetBlend(ref desc, i);
                    if (!blendEnable && targetBlendDesc.BlendEnable)
                        blendEnable = true;
                    targetBlends[i] = new RenderTargetBlend(targetBlendDesc);
                }
            }
        }

        public void Apply(Pipeline glPipeline)
        {
            var glBlend = glPipeline.Blend;
            glBlend.AlphaToCoverageEnable = alphaToCoverageEnable;
            glBlend.IndependentBlendEnable = independentBlendEnable;

            glBlend.BlendEnable = blendEnable;
            if (!blendEnable) 
                return;

            for (int i = 0; i < targetBlends.Length; i++)
            {
                var targetBlend = targetBlends[i];
                var glTargetBlend = glBlend.Targets[i];

                glTargetBlend.SeparateAlphaBlendEnable = targetBlend.Separate;

                glTargetBlend.Color.SrcFactor = targetBlend.SrcFactor;
                glTargetBlend.Color.DestFactor = targetBlend.DestFactor;
                glTargetBlend.Color.EquationMode = targetBlend.EquationMode;

                if (targetBlend.Separate)
                {
                    glTargetBlend.Alpha.SrcFactor = targetBlend.SrcFactorAlpha;
                    glTargetBlend.Alpha.DestFactor = targetBlend.DestFactorAlpha;
                    glTargetBlend.Alpha.EquationMode = targetBlend.EquationModeAlpha;
                }
            }
        }
    }
}
