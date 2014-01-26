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

using System.Linq;
using Beholder.Core;
using Beholder.Libraries.ObjectGL4.Shaders;
using Beholder.Utility.ForImplementations.Core;

namespace Beholder.Libraries.ObjectGL4.Core
{
    class CVertexLayout : VertexLayoutBase<ICDevice>
    {
        readonly WVertrexLayoutElement[] wElements;

        public WVertrexLayoutElement[] WElements { get { return wElements; } }

        public CVertexLayout(ICDevice device, CVertexShader vertexShader, VertexLayoutElement[] elements) : base(device, vertexShader, elements)
        {
            wElements = elements.Select(e => new WVertrexLayoutElement(e)).ToArray();
        }
    }
}
