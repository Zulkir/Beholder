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
using System.Collections.Generic;

namespace Beholder.Utility.Collections
{
    // todo: make ICollection, disallow adding null

    /// <summary>
    /// List that does not change the indices of added elements.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class LazyList<T> : IEnumerable<IndexValuePair<T>> where T : class
    {
        readonly Stack<int> holes;

        T[] array;
        int count;
        int arrayCount;
        int capacity;

        /// <summary>
        /// Gets the number of elements actually contained in the LazyList.
        /// </summary>
        public int Count { get { return count; } }

        /// <summary>
        /// Gets the total number of elements the internal data structure can hold without resizing.
        /// </summary>
        public int Capacity { get { return capacity; } }

        /// <summary>
        /// Indicates whether there are empty slots in the list between 0 and Count - 1 indices.
        /// </summary>
        public bool HasHoles { get { return holes.Count != 0; } }

        /// <summary>
        /// Creates a new instance of a LazyList class.
        /// </summary>
        public LazyList() : this(16) { }

        /// <summary>
        /// Creates a new instance of a LazyList class specifying the inital capacity of the internal structure.
        /// </summary>
        /// <param name="initialCapacity">Inital capacity of the internal structure</param>
        public LazyList(int initialCapacity)
        {
            capacity = initialCapacity;
            array = new T[capacity];
            holes = new Stack<int>();
            arrayCount = 0;
            count = 0;
        }

        /// <summary>
        /// Gets the element at a specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element to get.</param>
        /// <returns>Element at a specified index</returns>
        public T this[int index] { get { return array[index]; } }

        /// <summary>
        /// Gets the element at a specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element to get.</param>
        /// <returns>Element at a specified index</returns>
        public T GetValue(int index) { return array[index]; }

        /// <summary>
        /// Adds an object to the list taking its index into account.
        /// </summary>
        /// <param name="constructor">Constructor of the object that uses an element's index in the list.</param>
        /// <returns>Object that was added.</returns>
        public int Add(Func<int, T> constructor)
        {
            int pos;
            if (holes.Count == 0)
            {
                if (count == capacity)
                {
                    capacity *= 2; 
                    var newarray = new T[capacity];
                    Array.Copy(array, newarray, count);
                    array = newarray;
                }

                array[count] = constructor(count);
                arrayCount++;
                pos = count;
            }
            else
            {
                pos = holes.Pop();
            }
            array[pos] = constructor(pos);
            count++;
            return pos;
        }

        /// <summary>
        /// Adds an object to the list.
        /// </summary>
        /// <param name="item">Object to add.</param>
        /// <returns>Index of the added object.</returns>
        public int Add(T item)
        {
            return Add(i => item);
        }

        /// <summary>
        /// Removes all elements from the list.
        /// </summary>
        public void Clear()
        {
            count = 0;
            arrayCount = 0;
            holes.Clear();

            for (int i = 0; i < arrayCount; i++)
                array[i] = null;
        }

        /// <summary>
        /// Determines whether the element is in the list.
        /// </summary>
        /// <param name="item">The object to locate in the list.</param>
        /// <returns>True if the element is in the list. False otherwise.</returns>
        public bool Contains(T item)
        {
            for (int i = 0; i < arrayCount; i++)
            {
                var t = array[i];
                if (t != null && t.Equals(item))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Removes a first occurence of a specific object from the list.
        /// </summary>
        /// <param name="item">The object to remove from the list.</param>
        /// <returns>True if the object was deleted. False otherwise.</returns>
        public bool Remove(T item)
        {
            for (int i = 0; i < arrayCount; i++)
            {
                if (array[i] == null || !array[i].Equals(item)) continue;
                RemoveAt(i);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Removes the element at the specified index of the list.
        /// </summary>
        /// <param name="index">The zero-based index of the element to remove.</param>
        public void RemoveAt(int index)
        {
            if (count == 1)
            {
                arrayCount = 0;
                holes.Clear();
            }
            else if (index == arrayCount - 1)
                arrayCount--;
            else
                holes.Push(index);
            array[index] = null;
            count--;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the index-value pairs of the elements in the list.
        /// </summary>
        public IEnumerator<IndexValuePair<T>> GetEnumerator()
        {
            for (int i = 0; i < arrayCount; i++)
                if (array[i] != null)
                    yield return new IndexValuePair<T> {Index = i, Value = array[i]};
        }

        /// <summary>
        /// Gets a collection containing values in the list.
        /// </summary>
        public IEnumerable<T> Values
        {
            get
            {
                for (int i = 0; i < arrayCount; i++)
                    if (array[i] != null)
                        yield return array[i];
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
