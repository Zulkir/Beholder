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

using Beholder.Resources;

namespace Beholder.Utility.ForImplementations.Resources
{
    public abstract class UnorderedAccessViewBase<TDevice> : DeviceChildBase<TDevice>, IUnorderedAccessView where TDevice : IDevice
    {
        protected UnorderedAccessViewDescription desc;

        public abstract IResource Resource { get; }

        public UnorderedAccessViewDimension Dimension { get { return desc.Dimension; } }
        public int FormatID { get { return desc.FormatID; } }
        public int FirstElement { get { return desc.FirstElement; } }
        public int ElementCount { get { return desc.ElementCount; } }
        public UnorderedAccessViewBufferFlags BufferFlags { get { return desc.BufferFlags; } }
        public int MipSlice { get { return desc.MipSlice; } }
        public int FirstArraySlice { get { return desc.FirstArraySlice; } }
        public int ArraySize { get { return desc.ArraySize; } }
        public int FirstDepthSlice { get { return desc.FirstDepthSlice; } }
        public int DepthSliceCount { get { return desc.DepthSliceCount; } }
        public void GetDesc(out UnorderedAccessViewDescription description) { description = desc; }

        protected UnorderedAccessViewBase(TDevice device, ref UnorderedAccessViewDescription desc)
            : base(device)
        {
            this.desc = desc;
        }
    }
}
