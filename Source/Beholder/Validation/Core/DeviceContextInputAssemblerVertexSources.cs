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

using System.Collections;
using System.Collections.Generic;
using Beholder.Core;

namespace Beholder.Validation.Core
{
    class DeviceContextInputAssemblerVertexSources : Wrapper<IDeviceContextCumulativeArrayBinding<VertexSource>>, IDeviceContextCumulativeArrayBinding<VertexSource>
    {
        public DeviceContextInputAssemblerVertexSources(IDeviceContextCumulativeArrayBinding<VertexSource> real)
            : base(real)
        {
            
        }

        public IEnumerator<VertexSource> GetEnumerator()
        {
            return Real.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count { get { return Real.Count; } }
        
        public VertexSource this[int index]
        {
            get { return Real[index]; } 
            set
            {
                Check.NullOrInternal(value.Buffer, "value.Buffer");
                if (value.Buffer != null)
                {
                    value.Buffer = value.Buffer.GetReal();
                    Check.NotNegative(value.Offset, "value.Offset");
                    Check.NotNegative(value.Stride, "value.Stride");
                }
                Real[index] = value;
            }
        }
    }
}
