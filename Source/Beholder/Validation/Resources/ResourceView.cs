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
using Beholder.Resources;

namespace Beholder.Validation.Resources
{
    abstract class ResourceView<T> : ResourceView, IWrapper<T> where T : IResourceView
    {
        public new T Real { get; private set; }
        protected ResourceView(T real) : base(real) { Real = real; }
    }

    abstract class ResourceView : DeviceChild<IResourceView>, IResourceView
    {
        protected ResourceView(IResourceView real) : base(real) { }

        public IResource Resource { get { return Wrappers.Get(Real.Resource); } }

        protected void CheckResourceNotReleased()
        {
            if (Real.Resource.IsReleased)
                throw new InvalidOperationException("Trying to use a view of an already released resource");
        }
    }
}
