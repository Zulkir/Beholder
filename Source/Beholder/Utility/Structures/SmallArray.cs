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
using System.Runtime.InteropServices;

namespace Beholder.Utility.Structures
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct SmallArray<T> : IEnumerable<T>
    {
        readonly int count;
        readonly T single;
        readonly T[] array;

        public int Count { get { return count; } }

        public SmallArray(T single)
        {
            count = 1;
            this.single = single;
            array = null;
        }

        public SmallArray(ref T single)
        {
            count = 1;
            this.single = single;
            array = null;
        }

        public SmallArray(T[] array)
        {
            if (array == null || array.Length == 0)
            {
                count = 0;
                single = default(T);
                this.array = null;
            }
            else if (array.Length == 1)
            {
                count = 1;
                single = array[0];
                this.array = null;
            }
            else
            {
                count = array.Length;
                single = default(T);
                this.array = array;
            }
        }


        public IEnumerator<T> GetEnumerator()
        {
            if (count == 0) 
                yield break;
            if (count == 1 && array == null)
                yield return single;
            else
                for (int i = 0; i < count; i++)
                    yield return array[i];
        }

        IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }
    }
}
