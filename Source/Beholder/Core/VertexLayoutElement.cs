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

using System;
using System.Runtime.InteropServices;

namespace Beholder.Core
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct VertexLayoutElement : IEquatable<VertexLayoutElement>
    {
        public ExplicitFormat Format;
        public int InputSlot;
        public int Offset;
        public VertexLayoutClassification VertexLayoutClassification;
        public int InstanceDataStepRate;

        public VertexLayoutElement(ExplicitFormat format, int inputSlot, int offset, VertexLayoutClassification vertexLayoutClassification = VertexLayoutClassification.PerVertexData, int instanceDataStepRate = 0)
        {
            Format = format;
            InputSlot = inputSlot;
            Offset = offset;
            VertexLayoutClassification = vertexLayoutClassification;
            InstanceDataStepRate = instanceDataStepRate;
        }

        public bool Equals(VertexLayoutElement other)
        {
            return
                Format == other.Format &&
                InputSlot == other.InputSlot &&
                Offset == other.Offset &&
                VertexLayoutClassification == other.VertexLayoutClassification &&
                InstanceDataStepRate == other.InstanceDataStepRate;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is VertexLayoutElement)) return false;
            var other = (VertexLayoutElement)obj;
            return Equals(ref this, ref other);
        }

        public override int GetHashCode()
        {
            return InputSlot | (Offset << 8) | ((int)Format << 16);
        }

        public override string ToString()
        {
            return string.Format("[ {0} {1} {2} {3} {4} ]", Format, InputSlot, Offset, VertexLayoutClassification, InstanceDataStepRate);
        }

        public static bool Equals(ref VertexLayoutElement ie1, ref VertexLayoutElement ie2)
        {
            return
                ie1.Format == ie2.Format &&
                ie1.InputSlot == ie2.InputSlot &&
                ie1.Offset == ie2.Offset &&
                ie1.VertexLayoutClassification == ie2.VertexLayoutClassification &&
                ie1.InstanceDataStepRate == ie2.InstanceDataStepRate;
        }
    }
}
