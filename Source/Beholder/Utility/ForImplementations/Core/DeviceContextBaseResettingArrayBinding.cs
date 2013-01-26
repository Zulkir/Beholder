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
using Beholder.Core;

namespace Beholder.Utility.ForImplementations.Core
{
    public class DeviceContextBaseResettingArrayBinding<T> : IDeviceContextResettingArrayBinding<T>
    {
        readonly T[] elements;
        readonly int maxCount;
        readonly Func<T, T, bool> areEqual;
        int currentCount;
        bool isDirty;

        public DeviceContextBaseResettingArrayBinding(int maxCount, Func<T, T, bool> areEqual)
        {
            elements = new T[maxCount];
            this.maxCount = maxCount;
            this.areEqual = areEqual;
            isDirty = true;
        }

        public T this[int index] { get { return elements[index]; } }

        public int MaxCount { get { return maxCount; } }
        public int CurrentCount { get { return currentCount; } }
        public bool IsDirty { get { return isDirty; } }

        public void Clean()
        {
            isDirty = false;
        }

        public void MarkAsDirty()
        {
            isDirty = true;
        }

        public void Set(T singleValue)
        {
            if (currentCount == 1 && areEqual(elements[0], singleValue))
                return;
            elements[0] = singleValue;
            currentCount = 1;
            isDirty = true;
        }

        public void Set(IEnumerable<T> values)
        {
            if (values == null)
            {
                isDirty = currentCount != 0;
                currentCount = 0;
            }
            else
            {
                int i = 0;
                foreach (var value in values)
                {
                    if (!isDirty && !areEqual(elements[i], value))
                        isDirty = true;
                    elements[i] = value;
                    i++;
                }
                if (!isDirty && currentCount != i)
                    isDirty = true;
                currentCount = i;
            }
        }

        #region Implementation of IEnumerable

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < currentCount; i++)
                yield return elements[i];
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}
