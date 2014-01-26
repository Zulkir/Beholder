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

namespace Beholder.Utility.Patterns
{
    public class CleanableArray<T> : CleanableArrayBase<T>
    {
        readonly Action<T[], int> applyAction;
        readonly bool applyOnZeroCount;

        int currentCount;
        int newCount;
        
        public CleanableArray(ICleanableObserver observer, int tag, int initialLength, Action<T[], int> applyAction, bool applyOnZeroCount)
            : base(observer, tag, initialLength)
        {
            this.applyAction = applyAction;
            this.applyOnZeroCount = applyOnZeroCount;
            currentCount = newCount = initialLength;
        }

        protected override int CurrentCount
        {
            get { return currentCount; }
        }

        protected override void SetCountProc(int value)
        {
            if (!Invalid && value == currentCount) return;

            newCount = value;

            if (!Notified)
            {
                Notified = true;
                Observer.NotifyDirty(this);
            }

            if (value > Data.Length)
            {
                var newData = new T[value];
                Array.Copy(Data, newData, Data.Length);
                Data = newData;
            }
        }

        protected override void ForceCount(int count)
        {
            newCount = count;

            if (count > Data.Length)
                Data = new T[count];
        }

        protected override void Apply()
        {
            currentCount = newCount;

            if (newCount > 0 || applyOnZeroCount)
                applyAction(Data, newCount);

            Notified = Invalid = false;
        }

        public int GetCount() { return newCount; }

        public void SetCount(int value)
        {
            SetCountProc(value);
        }
    }
}
