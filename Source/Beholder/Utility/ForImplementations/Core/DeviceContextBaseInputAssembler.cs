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

using Beholder.Core;
using Beholder.Utility.Patterns;

namespace Beholder.Utility.ForImplementations.Core
{
    public class DeviceContextBaseInputAssembler : IDeviceContextInputAssembler
    {
        readonly DeviceContextBaseCumulativeArrayBinding<VertexSource> vertexSources;
        readonly DirtyProperty<PrimitiveTopology> primitiveTopology;
        readonly DirtyProperty<IVertexLayout> vertexLayout;
        readonly DirtyProperty<IndexSource> indexSource;

        public DeviceContextBaseInputAssembler(int vertexSourceCount)
        {
            vertexSources = new DeviceContextBaseCumulativeArrayBinding<VertexSource>(vertexSourceCount, VertexSource.Equals);
            primitiveTopology = new DirtyProperty<PrimitiveTopology>();
            vertexLayout = new DirtyProperty<IVertexLayout>();
            indexSource = new DirtyProperty<IndexSource>();
        }

        public DirtyProperty<PrimitiveTopology> PrimitiveTopology { get { return primitiveTopology; } }
        public DirtyProperty<IVertexLayout> VertexLayout { get { return vertexLayout; } }
        public DeviceContextBaseCumulativeArrayBinding<VertexSource> VertexSources { get { return vertexSources; } }
        IDeviceContextCumulativeArrayBinding<VertexSource> IDeviceContextInputAssembler.VertexSources { get { return vertexSources; } }
        public DirtyProperty<IndexSource> IndexSource { get { return indexSource; } }

        PrimitiveTopology IDeviceContextInputAssembler.PrimitiveTopology { get { return primitiveTopology.Value; } set { primitiveTopology.Value = value; } }
        IVertexLayout IDeviceContextInputAssembler.VertexLayout { get { return vertexLayout.Value; } set { vertexLayout.Value = value; } }
        IndexSource IDeviceContextInputAssembler.IndexSource { get { return indexSource.Value; } set { indexSource.Value = value; } }

        public void MarkAsDirty()
        {
            vertexSources.MarkAsDirty();
            primitiveTopology.MarkAsDirty();
            vertexLayout.MarkAsDirty();
            indexSource.MarkAsDirty();
        }
    }
}
