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
using System.Runtime.InteropServices;

namespace Beholder.Utility.Collections
{
    /// <summary>
    /// Pair of a value and its index. Used for iteration through a LazyList.
    /// </summary>
    /// <typeparam name="T">Type of the value to hold.</typeparam>
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct IndexValuePair<T>
    {
        /// <summary>
        /// Index of the value.
        /// </summary>
        public int Index;

        /// <summary>
        /// Value being held.
        /// </summary>
        public T Value;

        /// <summary>
        /// Creates a new IndexValuePair structure.
        /// </summary>
        /// <param name="index">Index of the value.</param>
        /// <param name="value">Value to be held.</param>
        public IndexValuePair(int index, T value)
        {
            Index = index;
            Value = value;
        }
    }
}
