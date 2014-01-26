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

namespace Beholder.Validation.Core
{
    class DeviceContextInputAssembler : Wrapper<IDeviceContextInputAssembler>, IDeviceContextInputAssembler
    {
        readonly DeviceContextInputAssemblerVertexSources vertexSources;

        public DeviceContextInputAssembler(IDeviceContextInputAssembler real) 
            : base(real)
        {
            vertexSources = new DeviceContextInputAssemblerVertexSources(Real.VertexSources);
        }

        public PrimitiveTopology PrimitiveTopology
        {
            get { return Real.PrimitiveTopology; } 
            set
            {
                Check.Enumeration(value, "value");
                Real.PrimitiveTopology = value;
            }
        }

        public IVertexLayout VertexLayout
        {
            get { return Wrappers.Get(Real.VertexLayout); }
            set
            {
                Check.NullOrInternal(value, "value"); 
                Real.VertexLayout = value.GetReal();
            }
        }

        public IDeviceContextCumulativeArrayBinding<VertexSource> VertexSources { get { return vertexSources; } }

        public IndexSource IndexSource 
        { 
            get { return Real.IndexSource; } 
            set 
            {
                Check.NullOrInternal(value.Buffer, "value.Buffer");
                if (value.Buffer != null)
                {
                    value.Buffer = value.Buffer.GetReal();
                    Check.NotNegative(value.Offset, "value.Offset");
                    Check.Enumeration(value.Format, "value.Format");
                    if (value.Format == IndexFormat.Undefined)
                        throw new ArgumentException("When index buffer is provided, its format cannot be undefined");
                }
                Real.IndexSource = value;
            } 
        }
    }
}
