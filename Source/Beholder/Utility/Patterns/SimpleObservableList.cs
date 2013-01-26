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

namespace Beholder.Utility.Patterns
{
    public class SimpleObservableList<T> : IList<T>, ISimpleObserver, ISimpleObservable
        where T : ISimpleObservable
    {
        readonly List<T> internalList;
        readonly List<ISimpleObserver> observers = new List<ISimpleObserver>();

        public SimpleObservableList()
        {
            internalList = new List<T>();
        }

        public Action<ISimpleObserver> Subsribe(ISimpleObserver observer)
        {
            if (!observers.Contains(observer)) 
                observers.Add(observer);
            return o => observers.Remove(o);
        }

        public void Notify()
        {
            foreach (var t in observers)
                t.Notify();
        }


        public T this[int index]
        {
            get { return internalList[index]; } 
            set { internalList[index] = value; Notify(); }
        }
        

        public void Add(T item)
        {
            internalList.Add(item);
            Notify();
        }

        public void Clear()
        {
            internalList.Clear();
            Notify();
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            internalList.CopyTo(array, arrayIndex);
        }

        public int Count { get { return internalList.Count; } }

        public bool IsReadOnly { get { return ((ICollection<T>)internalList).IsReadOnly; } }

        public bool Remove(T item)
        {
            bool result = internalList.Remove(item);
            Notify();
            return result;
        }

        public bool Contains(T item)
        {
            return internalList.Contains(item);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return internalList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int IndexOf(T item)
        {
            return internalList.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            internalList.Insert(index, item);
            Notify();
        }

        public void RemoveAt(int index)
        {
            internalList.RemoveAt(index);
            Notify();
        }
    }
}
