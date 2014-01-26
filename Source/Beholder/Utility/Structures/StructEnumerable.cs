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
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace Beholder.Utility.Structures
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct StructEnumerable<T> where T : struct
    {
        readonly IEnumerable<T> many;
        readonly T single;
        readonly bool any;

        public bool Any() { return any; }
        
        public StructEnumerable(T single)
        {
            many = null;
            this.single = single;
            any = true;
        }

        public StructEnumerable(ref T single)
        {
            many = null;
            this.single = single;
            any = true;
        }

        public StructEnumerable(IEnumerable<T> many)
        {
            if (many == null) 
                throw new ArgumentNullException("many");
            this.many = many;
            single = default(T);
            any = true;
        }

        IEnumerable<T> SingleAsEnumerable()
        {
            if (any)
                yield return single;
        }

        public IEnumerable<T> AsEnumerable()
        {
            return many ?? SingleAsEnumerable();
        }

        public int Count()
        {
            return any 
                ? many == null 
                    ? 1 
                    : many.Count() 
                : 0;
        }

        public T FirstOrDefault()
        {
            return many == null ? single : many.First();
        }

        public void FirstOrDefault(out T first)
        {
            first = many == null ? single : many.First();
        }

        public static implicit operator StructEnumerable<T>(T single)
        {
            return new StructEnumerable<T>(ref single);
        }

        public static StructEnumerable<T> None { get { return new StructEnumerable<T>(); } }
    }
}
