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
    public abstract class ShaderResourceViewBase<TDevice> : DeviceChildBase<TDevice>, IShaderResourceView where TDevice : IDevice
    {
        protected ShaderResourceViewDescription desc;

        public abstract IResource Resource { get; }

        public ShaderResourceViewDimension Dimension { get { return desc.Dimension; } }
        public int FormatID { get { return desc.FormatID; } }
        public int FirstElement { get { return desc.FirstElement; } }
        public int ElementCount { get { return desc.ElementCount; } }
        public int MostDetailedMip { get { return desc.MostDetailedMip; } }
        public int MipLevels { get { return desc.MipLevels; } }
        public int FirstArraySlice { get { return desc.FirstArraySlice; } }
        public int ArraySize { get { return desc.ArraySize; } }
        public ShaderResourceViewExtendedBufferFlags ExtendedBufferFlags { get { return desc.ExtendedBufferFlags; } }
        public void GetDescription(out ShaderResourceViewDescription description) { description = desc; }

        protected ShaderResourceViewBase(TDevice device, ref ShaderResourceViewDescription desc)
            : base(device)
        {
            this.desc = desc;
        }
    }
}
