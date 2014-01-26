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
using System.Runtime.InteropServices;
using Beholder.Core;
using ObjectGL.GL42;

namespace Beholder.Libraries.ObjectGL4.Core
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    struct WVertrexLayoutElement
    {
        // todo: make sure about types
        public int StreamSourceIndex;
        public int GLVertexAttribPointerType;
        public int Offset;
        public int Divisor;
        public VertexAttributeDimension Dimension;
        public WVertexLayoutElementType Type;
        public bool Normalized;

        public WVertrexLayoutElement(VertexLayoutElement vertexLayoutElement)
        {
            CtObjectGL.VertexElementFormat(vertexLayoutElement.Format, out GLVertexAttribPointerType, out Dimension, out Type, out Normalized);
            StreamSourceIndex = vertexLayoutElement.InputSlot;
            Offset = vertexLayoutElement.Offset;
            Divisor = vertexLayoutElement.VertexLayoutClassification == VertexLayoutClassification.PerVertexData 
                ? 0 : vertexLayoutElement.InstanceDataStepRate;
        }
    }
}
