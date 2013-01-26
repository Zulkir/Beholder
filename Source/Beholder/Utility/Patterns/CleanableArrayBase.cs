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
using Beholder.Utility.Delegates;

namespace Beholder.Utility.Patterns
{
    public abstract class CleanableArrayBase<T> : ICleanable
    {
        protected readonly ICleanableObserver Observer;

        protected T[] Data;
        protected bool Notified;
        protected bool Invalid;

        public int Tag { get; private set; }

        protected CleanableArrayBase(ICleanableObserver observer, int tag, int initialLength)
        {
            Observer = observer;
            Tag = tag;
            Data = new T[initialLength];
            Invalid = true;
        }

        protected CleanableArrayBase(ICleanableObserver observer, int tag, T[] array)
        {
            Observer = observer;
            Tag = tag;
            Data = array;
        }

        protected abstract int CurrentCount { get; }
        protected abstract void SetCountProc(int newCount);
        protected abstract void Apply();
        protected abstract void ForceCount(int count);

        public T Get(int index)
        {
            return Data[index];
        }

        public void Get(int index, out T item)
        {
            item = Data[index];
        }

        public void Get(T[] destinationArray, int destinationIndex)
        {
            Array.Copy(Data, 0, destinationArray, destinationIndex, CurrentCount);
        }

        public void SetUnchecked(T[] sourceArray, int sourceArrayIndex, int count)
        {
            SetCountProc(count);

            Array.Copy(sourceArray, sourceArrayIndex, Data, 0, count);

            if (!Notified)
            {
                Notified = true;
                Observer.NotifyDirty(this);
            }
        }

        public void SetCheckedByVal(T[] sourceArray, int sourceArrayIndex, int count, Func<T, T, bool> equality)
        {
            if (Invalid)
            {
                SetUnchecked(sourceArray, sourceArrayIndex, count);
                return;
            }

            SetCountProc(count);

            bool somethingDifferent = false;

            for (int i = 0; i < count; i++)
            {
                if (equality(Data[i], sourceArray[sourceArrayIndex + i])) 
                    continue;
                somethingDifferent = true;
                break;
            }

            Array.Copy(sourceArray, sourceArrayIndex, Data, 0, count);

            if (somethingDifferent && !Notified)
            {
                Notified = true;
                Observer.NotifyDirty(this);
            }
        }

        public void SetCheckedByRef(T[] sourceArray, int sourceArrayIndex, int count, FuncRef<T, T, bool> equality)
        {
            if (Invalid)
            {
                SetUnchecked(sourceArray, sourceArrayIndex, count);
                return;
            }

            SetCountProc(count);

            bool somethingDifferent = false;

            for (int i = 0; i < count; i++)
            {
                if (equality(ref Data[i], ref sourceArray[sourceArrayIndex + i])) 
                    continue;
                somethingDifferent = true;
                break;
            }

            Array.Copy(sourceArray, sourceArrayIndex, Data, 0, count);

            if (somethingDifferent && !Notified)
            {
                Notified = true;
                Observer.NotifyDirty(this);
            }
        }

        public void SetUnchecked(IEnumerable<T> source, int count)
        {
            SetCountProc(count);

            using (var enumerator = source.GetEnumerator())
            {
                for (int i = 0; i < count; i++)
                {
                    enumerator.MoveNext();
                    Data[i] = enumerator.Current;
                }
            }

            if (!Notified)
            {
                Notified = true;
                Observer.NotifyDirty(this);
            }
        }

        public void SetCheckedByVal(IEnumerable<T> source, int count, Func<T, T, bool> equality)
        {
            if (Invalid)
            {
                SetUnchecked(source, count);
                return;
            }

            SetCountProc(count);

            bool somethingDifferent = false;

            using (var enumerator = source.GetEnumerator())
            {
                for (int i = 0; i < count; i++)
                {
                    enumerator.MoveNext();

                    if (somethingDifferent)
                        Data[i] = enumerator.Current;
                    else
                    {
                        T item = enumerator.Current;
                        if (!equality(Data[i], item))
                        {
                            somethingDifferent = true;
                            Data[i] = item;
                        }
                    }
                }
            }

            if (somethingDifferent && !Notified)
            {
                Notified = true;
                Observer.NotifyDirty(this);
            }
        }

        public void SetCheckedByRef(IEnumerable<T> source, int count, FuncRef<T, T, bool> equality)
        {
            if (Invalid)
            {
                SetUnchecked(source, count);
                return;
            }

            SetCountProc(count);

            bool somethingDifferent = false;

            using (var enumerator = source.GetEnumerator())
            {
                for (int i = 0; i < count; i++)
                {
                    enumerator.MoveNext();

                    if (somethingDifferent)
                        Data[i] = enumerator.Current;
                    else
                    {
                        var item = enumerator.Current;
                        if (!equality(ref Data[i], ref item))
                        {
                            somethingDifferent = true;
                            Data[i] = item;
                        }
                    }
                }
            }

            if (somethingDifferent && !Notified)
            {
                Notified = true;
                Observer.NotifyDirty(this);
            }
        }

        public void SetUnchecked(T item)
        {
            SetCountProc(1);
            SetSingleUnchecked(0, item);
        }

        public void SetCheckedByVal(T item, Func<T, T, bool> equality)
        {
            SetCountProc(1);
            SetSingleCheckedByVal(0, item, equality);
        }

        public void SetCheckedByRef(ref T item, FuncRef<T, T, bool> equality)
        {
            SetCountProc(1);
            SetSingleCheckedByRef(0, ref item, equality);
        }

        public void SetSingleUnchecked(int index, T item)
        {
            Data[index] = item;

            if (!Notified)
            {
                Notified = true;
                Observer.NotifyDirty(this);
            }
        }

        public void SetSingleCheckedByVal(int index, T item, Func<T, T, bool> equality)
        {
            if (Invalid)
            {
                SetSingleUnchecked(index, item);
                return;
            }

            bool somethingDifferent = !equality(Data[index], item);

            Data[index] = item;

            if (somethingDifferent && !Notified)
            {
                Notified = true;
                Observer.NotifyDirty(this);
            }
        }

        public void SetSingleCheckedByRef(int index, ref T item, FuncRef<T, T, bool> equality)
        {
            if (Invalid)
            {
                SetSingleUnchecked(index, item);
                return;
            }

            bool somethingDifferent = !equality(ref Data[index], ref item);

            Data[index] = item;

            if (somethingDifferent && !Notified)
            {
                Notified = true;
                Observer.NotifyDirty(this);
            }
        }

        public void Invalidate(bool notify)
        {
            Invalid = true;

            if (notify && !Notified)
            {
                Notified = true;
                Observer.NotifyDirty(this);
            }
        }

        public void Force(T[] sourceArray, int sourceArrayIndex, int count)
        {
            ForceCount(count);

            Array.Copy(sourceArray, sourceArrayIndex, Data, 0, count);

            Apply();
        }

        public void ForceCurrent()
        {
            Apply();
        }

        public void Clean()
        {
            if (!(Notified || Invalid)) 
                return;
            Apply();
        }
    }
}
