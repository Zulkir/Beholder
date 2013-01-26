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

namespace Beholder.Utility.Patterns
{
    public class CleanableArrayFixed<T> : CleanableArrayBase<T>
    {
        readonly Action<T[]> applyAction;
        
        readonly int length;
        public int Length { get { return length; } }

        public CleanableArrayFixed(ICleanableObserver observer, int tag, int arrayLength, Action<T[]> applyAction)
            : base(observer, tag, arrayLength)
        {
            this.applyAction = applyAction;
            length = arrayLength;
        }

        protected CleanableArrayFixed(ICleanableObserver observer, int tag, T[] array, Action<T[]> applyAction)
            : base (observer, tag, array)
        {
            this.applyAction = applyAction;
            length = array.Length;
        }

        protected override int CurrentCount
        {
            get { return length; }
        }

        protected override void SetCountProc(int newCount)
        {
            
        }

        protected override void ForceCount(int count)
        {
            
        }

        protected override void Apply()
        {
            applyAction(Data);
            Notified = Invalid = false;
        }
    }
}
