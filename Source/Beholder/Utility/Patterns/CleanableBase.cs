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

namespace Beholder.Utility.Patterns
{
    public abstract class CleanableBase<T> : ICleanable
    {
        readonly protected ICleanableObserver Observer;

        protected T CurrentValue;
        protected T NewValue;

        // currentValue is unknown
        protected bool Invalid;
        // newValue != currentValue or invalid
        protected bool Dirty;
        // NotifyDirty() was called
        protected bool Notified;
        
        public int Tag { get; private set; }

        protected CleanableBase(ICleanableObserver observer, int tag)
        {
            Observer = observer;
            Tag = tag;
            Invalid = Dirty = true;
        }

        protected abstract bool NewEqualsCurrent();
        protected abstract void ApplyNewValue();

        public T Get()
        {
            return NewValue;
        }

        public void Set(T value)
        {
            NewValue = value;

            Dirty = (Invalid || !NewEqualsCurrent());
            if (Dirty && !Notified) 
                Observer.NotifyDirty(this);
        }

        public void Invalidate(bool notify)
        {
            Invalid = Dirty = true;

            if (notify && !Notified)
            {
                Notified = true;
                Observer.NotifyDirty(this);
            }
        }

        public void OnExternalChange(T currentValueSet)
        {
            CurrentValue = currentValueSet;

            Invalid = false;
            Dirty = !NewEqualsCurrent();
            if (Dirty && !Notified) 
                Observer.NotifyDirty(this);
        }

        protected void Apply()
        {
            CurrentValue = NewValue;
            Invalid = Dirty = false;
            ApplyNewValue();
        }

        public void Force(T value)
        {
            NewValue = value;
            Apply();
        }

        public void ForceCurrent()
        {
            Apply();
        }

        public void Clean()
        {
            Notified = false;
            if (Dirty) 
                Apply();
        }
    }
}
