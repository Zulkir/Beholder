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

using Beholder.Utility.Delegates;

namespace Beholder.Utility.Patterns
{
    public class CleanableRef<T> : CleanableBase<T>
        where T : struct
    {
        readonly FuncRef<T, T, bool> equalityFunc;
        readonly ActionRef<T> applyAction;

        public CleanableRef(ICleanableObserver observer, int tag, FuncRef<T, T, bool> equalityFunc, ActionRef<T> applyAction) 
            : base(observer, tag)
        {
            this.equalityFunc = equalityFunc;
            this.applyAction = applyAction;
        }

        protected override bool NewEqualsCurrent()
        {
            return equalityFunc(ref NewValue, ref CurrentValue);
        }

        protected override void ApplyNewValue()
        {
            applyAction(ref NewValue);
        }

        public void Get(out T value) { value = NewValue; }

        public void Set(ref T value)
        {
            NewValue = value;

            Dirty = (Invalid || !NewEqualsCurrent());
            if (Dirty && !Notified) 
                Observer.NotifyDirty(this);
        }

        public void OnExternalChange(ref T currentValueSet)
        {
            CurrentValue = currentValueSet;

            Invalid = false;
            Dirty = !NewEqualsCurrent();
            if (Dirty && !Notified) 
                Observer.NotifyDirty(this);
        }

        public void Force(ref T value)
        {
            NewValue = value;
            Apply();
        }
    }
}
