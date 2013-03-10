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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Beholder.Utility.Structures;

namespace Beholder.Utility.Collections
{
    public class TightConcurrentDictionary<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
    {
        readonly object writeLock = new object();
        readonly Func<TKey, TKey, bool> areEqual;
        Pair<TKey, TValue>[] pairs;
        int count;

        public int Count { get { return count; } }

        public TightConcurrentDictionary(Func<TKey, TKey, bool> areEqual, int initialCapacity = 1)
        {
            if (areEqual == null)
                throw new ArgumentNullException("areEqual");
            this.areEqual = areEqual;
            pairs = new Pair<TKey, TValue>[initialCapacity];
        }

        public bool TryGet(TKey key, out TValue value)
        {
            var pairsLoc = pairs;
            int countLoc = count;
            for (int i = 0; i < countLoc; i++)
                if (areEqual(pairsLoc[i].First, key))
                {
                    value = pairsLoc[i].Second;
                    return true;
                }
            value = default(TValue);
            return false;
        }

        public TValue GetOrAdd<TClosure>(TKey key, TClosure closure, Func<TKey, TClosure, TValue> constructor)
        {
            var pairsLoc = pairs;
            int countLoc = count;
            for (int i = 0; i < countLoc; i++)
                if (areEqual(pairsLoc[i].First, key))
                    return pairsLoc[i].Second;

            Pair<TKey, TValue> pair;
            lock (writeLock)
            {
                pairsLoc = pairs;
                countLoc = count;
                for (int i = 0; i < countLoc; i++)
                    if (areEqual(pairsLoc[i].First, key))
                        return pairsLoc[i].Second;

                if (count == pairs.Length)
                {
                    var newPairs = new Pair<TKey, TValue>[count * 2];
                    Array.Copy(pairs, newPairs, pairs.Length);
                    pairs = newPairs;
                }
                    
                pair = new Pair<TKey, TValue> {First = key, Second = constructor(key, closure)};
                pairs[count] = pair;
                count++;
            }
            return pair.Second;
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            for (int i = 0; i < count; i++)
                yield return new KeyValuePair<TKey, TValue>(pairs[i].First, pairs[i].Second);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
