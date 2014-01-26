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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Beholder.Core;

namespace Beholder.Validation.Core
{
    class DeviceContextRasterizerViewports : Wrapper<IDeviceContextResettingArrayBinding<Viewport>>, IDeviceContextResettingArrayBinding<Viewport>
    {
        public DeviceContextRasterizerViewports(IDeviceContextResettingArrayBinding<Viewport> real)
            : base(real)
        {
            
        }

        public IEnumerator<Viewport> GetEnumerator() { return Real.GetEnumerator(); }
        IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }
        public int MaxCount { get { return Real.MaxCount; } }
        public int CurrentCount { get { return Real.CurrentCount; } }
        public Viewport this[int index] { get { return Real[index]; } }

        public void Set(Viewport singleValue)
        {
            ValidateViewport(singleValue);
            Real.Set(singleValue);
        }

        public void Set(IEnumerable<Viewport> values)
        {
            if (values == null)
                throw new ArgumentNullException("values");
            if (values.Count() > MaxCount)
                throw new ArgumentException(string.Format("Provided enumeration has {0} elements, while maximum is {1}", values.Count(), MaxCount));
            foreach (var viewport in values)
                ValidateViewport(viewport);
            Real.Set(values);
        }

        public void SetEmpty()
        {
            Real.SetEmpty();
        }

        static void ValidateViewport(Viewport viewport)
        {
            Check.NotNegative(viewport.Left, "viewport.Left");
            Check.NotNegative(viewport.Top, "viewport.Top");
            Check.NotNegative(viewport.Width, "viewport.Width");
            Check.NotNegative(viewport.Height, "viewport.Height");
            Check.NotNegative(viewport.MinDepth, "viewport.MinDepth");
            if (viewport.MaxDepth > 1.0f)
                throw new ArgumentException("viewport.MinDepth and viewport.MaxDepth must be in the range from 0.0 to 1.0");
            if (viewport.MinDepth > viewport.MaxDepth)
                throw new ArgumentException("viewport.MinDepth cannot be greater than viewport.MaxDepth");
        }
    }
}
