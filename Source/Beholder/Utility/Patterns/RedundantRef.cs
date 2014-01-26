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
using Beholder.Utility.Delegates;

namespace Beholder.Utility.Patterns
{
    public class RedundantRef<T> : RedundantBase<T>
        where T : struct
    {
        readonly FuncRef<T, T, bool> equality;
        readonly ActionRef<T> applyAction;

        public RedundantRef(FuncRef<T, T, bool> equality, ActionRef<T> applyAction)
        {
            this.equality = equality;
            this.applyAction = applyAction;
        }

        public void Get(out T value)
        {
            if (invalid) throw new InvalidOperationException("Current value is unknown.");
            value = currentValue; 
        }

        public void Set(ref T newValue)
        {
            if (invalid || !equality(ref currentValue, ref newValue))
                Force(ref newValue);
        }

        public void Force(ref T newValue)
        {
            currentValue = newValue;
            invalid = false;
            applyAction(ref newValue);
        }

        public void ForceCurrent()
        {
            if (invalid) 
                throw new InvalidOperationException("Current value is unknown.");
            applyAction(ref currentValue);
        }
    }
}
