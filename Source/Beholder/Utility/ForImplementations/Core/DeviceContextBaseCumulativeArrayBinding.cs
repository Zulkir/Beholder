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
using Beholder.Core;
using Beholder.Utility.Collections;

namespace Beholder.Utility.ForImplementations.Core
{
    public class DeviceContextBaseCumulativeArrayBinding<T> : IDeviceContextCumulativeArrayBinding<T>
    {
        readonly T[] elements;
        readonly SortedIntSet dirtyIndices;
        readonly Func<T, T, bool> areEqual;

        public DeviceContextBaseCumulativeArrayBinding(int count, Func<T, T, bool> areEqual)
        {
            elements = new T[count];
            dirtyIndices = new SortedIntSet();
            this.areEqual = areEqual;
            MarkAsDirty();
        }

        public IEnumerable<int> DirtyIndices { get { return dirtyIndices; } }
        public bool IsDirty { get { return dirtyIndices.Count > 0; } }
        public int Count { get { return elements.Length; } }

        public void Clean()
        {
            dirtyIndices.Clear();
        }

        public void MarkAsDirty()
        {
            dirtyIndices.FillWithRange(elements.Length);
        }

        public T this[int index] 
        {
            get { return elements[index]; } 
            set
            {
                if (areEqual(elements[index], value)) 
                    return;
                elements[index] = value;
                dirtyIndices.Add(index);
            } 
        }

        public IEnumerator<T> GetEnumerator()
        {
            return ((IEnumerable<T>)elements).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
