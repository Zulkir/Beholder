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
using System.Collections.Generic;
using ObjectGL.GL42;
using Beholder.Utility.ForImplementations.Resources;

namespace Beholder.Libraries.ObjectGL4.Core
{
    class DummyColorRenderbufferProvider : IDisposable
    {
        readonly Context glContext;
        readonly Dictionary<int, Renderbuffer> renderbuffers;
        int currentWidth;
        int currentHeight;

        public DummyColorRenderbufferProvider(Context glContext)
        {
            this.glContext = glContext;
            renderbuffers = new Dictionary<int, Renderbuffer>();
        }

        public Renderbuffer GetRenderbuffer(int width, int height, int samples)
        {
            if (currentWidth < width || currentHeight < height)
            {
                currentWidth = width;
                currentHeight = height;
                Dispose();
            }

            return renderbuffers.GetOrAdd(samples, s => new Renderbuffer(glContext, currentWidth, currentHeight, Format.Rgba8, s));
        }

        public void Dispose()
        {
            foreach (var renderbuffer in renderbuffers.Values)
                renderbuffer.Dispose();
            renderbuffers.Clear();
        }
    }
}
