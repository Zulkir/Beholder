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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Beholder.Core;
using Beholder.Resources;
using Beholder.Validation.Resources;

namespace Beholder.Validation.Core
{
    class DeviceContextOutputMergerRenderTargets : Wrapper<IDeviceContextResettingArrayBinding<IRenderTargetView>>, IDeviceContextResettingArrayBinding<IRenderTargetView>
    {
        public DeviceContextOutputMergerRenderTargets(IDeviceContextResettingArrayBinding<IRenderTargetView> real)
            : base(real)
        {
            
        }

        public IEnumerator<IRenderTargetView> GetEnumerator()
        {
            return Real.Select(Wrappers.Get).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int MaxCount { get { return Real.MaxCount; } }

        public int CurrentCount { get { return Real.CurrentCount; } }

        public IRenderTargetView this[int index] { get { return Wrappers.Get(Real[index]); } }

        public void Set(IRenderTargetView singleValue)
        {
            Check.ExistingInternal(singleValue, "singleValue");
            Real.Set(((RenderTargetView)singleValue).Real);
        }

        public void Set(IEnumerable<IRenderTargetView> values)
        {
            Check.NotNullNotEmpty(values, "values");
            var first = values.First();
            int width = first.Width;
            int height = first.Height;
            int formatID = first.FormatID;
            if (values.Any(v => v.Width != width || v.Height != height || v.FormatID != formatID))
                throw new ArgumentException("All simultaneous render target views must have the same width, height annd formatID");
            Real.Set(values.Select(v => ((RenderTargetView)v).Real));
        }
    }
}
