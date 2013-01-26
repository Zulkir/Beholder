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
using Beholder.Utility.ForImplementations.Core;
using ObjectGL.GL42;
using OpenTK.Graphics.OpenGL;

namespace Beholder.Libraries.ObjectGL4.Core
{
    class CDepthStencilState : DepthStencilStateBase<ICDevice>
    {
        readonly StencilOp frontStencilFailOp;
        readonly StencilOp frontDepthFailOp;
        readonly StencilOp frontDepthPassOp;
        readonly StencilFunction frontStencilFunc;

        readonly StencilOp backStencilFailOp;
        readonly StencilOp backDepthFailOp;
        readonly StencilOp backDepthPassOp;
        readonly StencilFunction backStencilFunc;

        readonly int stencilFuncMask;
        readonly int stencilWriteMask;

        readonly bool depthTestEnable;
        readonly bool depthMask;
        readonly DepthFunction depthFunc;
        readonly bool stencilTestEnable;

        public CDepthStencilState(ICDevice device, ref DepthStencilDescription desc)
            : base(device, ref desc)
        {
            frontStencilFailOp = CtObjectGL.StencilOp(desc.FrontFaceStencilOp.StencilFailOperation);
            frontDepthFailOp = CtObjectGL.StencilOp(desc.FrontFaceStencilOp.StencilDepthFailOperation);
            frontDepthPassOp = CtObjectGL.StencilOp(desc.FrontFaceStencilOp.StencilPassOperation);
            frontStencilFunc = CtObjectGL.StencilFunction(desc.FrontFaceStencilOp.StencilFunction);

            backStencilFailOp = CtObjectGL.StencilOp(desc.BackFaceStencilOp.StencilFailOperation);
            backDepthFailOp = CtObjectGL.StencilOp(desc.BackFaceStencilOp.StencilDepthFailOperation);
            backDepthPassOp = CtObjectGL.StencilOp(desc.BackFaceStencilOp.StencilPassOperation);
            backStencilFunc = CtObjectGL.StencilFunction(desc.BackFaceStencilOp.StencilFunction);

            stencilFuncMask = desc.StencilReadMask;
            stencilWriteMask = desc.StencilWriteMask;

            depthTestEnable = desc.DepthEnable;
            depthMask = desc.DepthWriteEnable;
            depthFunc = CtObjectGL.DepthFunction(desc.DepthFunction);
            stencilTestEnable = desc.StencilEnable;
        }

        public void Apply(Pipeline glPipeline)
        {
            var glDepthStencil = glPipeline.DepthStencil;

            glDepthStencil.Front.StencilFailOp = frontStencilFailOp;
            glDepthStencil.Front.DepthFailOp = frontDepthFailOp;
            glDepthStencil.Front.DepthPassOp = frontDepthPassOp;
            glDepthStencil.Front.StencilFunc = frontStencilFunc;

            glDepthStencil.Front.StencilFuncMask = stencilFuncMask;
            glDepthStencil.Front.StencilWriteMask = stencilWriteMask;

            glDepthStencil.Back.StencilFailOp = backStencilFailOp;
            glDepthStencil.Back.DepthFailOp = backDepthFailOp;
            glDepthStencil.Back.DepthPassOp = backDepthPassOp;
            glDepthStencil.Back.StencilFunc = backStencilFunc;

            glDepthStencil.Back.StencilFuncMask = stencilFuncMask;
            glDepthStencil.Back.StencilWriteMask = stencilWriteMask;

            glDepthStencil.DepthTestEnable = depthTestEnable;
            glDepthStencil.DepthMask = depthMask;
            glDepthStencil.DepthFunc = depthFunc;
            glDepthStencil.StencilTestEnable = stencilTestEnable;
        }
    }
}
