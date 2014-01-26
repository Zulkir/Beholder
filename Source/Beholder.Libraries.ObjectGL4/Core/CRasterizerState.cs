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
using Beholder.Utility.ForImplementations.Core;
using ObjectGL.GL42;
using OpenTK.Graphics.OpenGL;

namespace Beholder.Libraries.ObjectGL4.Core
{
    class CRasterizerState : RasterizerStateBase<ICDevice>
    {
        readonly PolygonMode polygonMode;
        readonly bool cullFaceEnable;
        readonly CullFaceMode cullFace;
        readonly FrontFaceDirection frontFace;
        readonly bool scissorEnable;
        readonly bool multisampleEnable;
        readonly bool lineSmoothEnable;

        public CRasterizerState(ICDevice device, ref RasterizerDescription desc)
            : base(device, ref desc)
        {
            polygonMode = CtObjectGL.PolygonMode(desc.FillMode);
            cullFaceEnable = CtObjectGL.CullEnable(desc.CullMode);
            cullFace = CtObjectGL.CullFace(desc.CullMode);
            frontFace = CtObjectGL.FrontFace(desc.FrontFaceWinding);
            
            if (desc.DepthBias != 0 || desc.DepthBiasClamp != 0.0f || desc.SlopeScaledDepthBias != 0.0f)
                throw new NotSupportedException("Depth bias is not supported by the OGL4 implementation of Behodler");

            scissorEnable = desc.ScissorEnable;
            multisampleEnable = desc.MultisampleEnable;
            lineSmoothEnable = desc.AntialiasedLineEnable;
        }

        public void Apply(Pipeline glPipeline)
        {
            var glRasterizer = glPipeline.Rasterizer;

            glRasterizer.PolygonModeFront = polygonMode;
            glRasterizer.PolygonModeBack = polygonMode;
            glRasterizer.CullFaceEnable = cullFaceEnable;
            glRasterizer.CullFace = cullFace;
            glRasterizer.FrontFace = frontFace;
            glRasterizer.ScissorEnable = scissorEnable;
            glRasterizer.MultisampleEnable = multisampleEnable;
            glRasterizer.LineSmoothEnable = lineSmoothEnable;
        }
    }
}
