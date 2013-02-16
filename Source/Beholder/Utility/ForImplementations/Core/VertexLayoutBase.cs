﻿/*
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
using Beholder.Shaders;
using Beholder.Utility.Collections.Readonly;

namespace Beholder.Utility.ForImplementations.Core
{
    public abstract class VertexLayoutBase<T> : DeviceChildBase<T>, IVertexLayout where T : IDevice
    {
        readonly IVertexShader vertexShader;
        readonly ReadonlyArrayWrapper<VertexLayoutElement> roElements;

        public IVertexShader VertexShader { get { return vertexShader; } }
        public IReadonlyList<VertexLayoutElement> Elements { get { return roElements; } }

        protected VertexLayoutBase(T device, IVertexShader vertexShader, VertexLayoutElement[] elements) : base(device)
        {
            this.vertexShader = vertexShader;
            roElements = new ReadonlyArrayWrapper<VertexLayoutElement>(elements);
        }
    }
}