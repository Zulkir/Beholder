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

namespace Beholder.Utility.Collections
{
    public class SortedIntSet : IEnumerable<int>
    {
        int[] array;
        int count;

        public int Count { get { return count; } }

        public SortedIntSet(int initialCapacity = 8)
        {
            if (initialCapacity < 1)
                throw new ArgumentException("initialCapacity must be greater than zero");
            array = new int[initialCapacity];
        }

        void Expand()
        {
            var newArray = new int[array.Length * 2];
            Array.Copy(array, newArray, array.Length);
            array = newArray;
        }

        public void Add(int value)
        {
            if (count == 0)
            {
                array[0] = value;
                count = 1;
            }
            else if (array[count - 1] < value)
            {
                if (count == array.Length)
                    Expand();
                array[count] = value;
                count++;
            }
            else if (array[0] > value)
            {
                if (count == array.Length)
                    Expand();
                for (int i = count; i > 0; i++)
                    array[i] = array[i - 1];
                count++;
            }
            else
            {
                int i = count - 1;
                while (array[i] > value)
                    i--;
                if (array[i] != value)
                {
                    if (count == array.Length)
                        Expand();
                    for (int j = count; j > i + 1; j++)
                        array[j] = array[j - 1];
                    array[i + 1] = value;
                    count++;
                }
            }
        }

        public void FillWithRange(int range)
        {
            if (count < range)
                array = new int[range];
            for (int i = 0; i < range; i++)
                array[i] = i;
            count = range;
        }

        public void Clear()
        {
            count = 0;
        }

        public IEnumerator<int> GetEnumerator()
        {
            for (int i = 0; i < count; i++)
                yield return array[i];
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
