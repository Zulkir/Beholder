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
using Beholder.Resources;
using ObjectGL.GL42;

namespace Beholder.Libraries.ObjectGL4.Resources
{
    class CTexture1DRenderbuffer : CTexture1DBase<Renderbuffer>
    {
        public CTexture1DRenderbuffer(ICDevice device, ref Texture1DDescription desc, Action<CTexture1DRenderbuffer> onRelease)
            : base(device, ref desc, null, t => onRelease((CTexture1DRenderbuffer)t))
        {
        }

        protected override void CreateNative()
        {
            glResource = new Renderbuffer(device.GetCurrentContext(), desc.Width, 1, (Format)desc.FormatID);
        }

        protected override void SetSubresourceData(Context glContext, int arrayIndex, int mipLevel, SubresourceData data)
        {
            throw new NotSupportedException("Renderbuffers do not support setting data");
        }
    }
}
