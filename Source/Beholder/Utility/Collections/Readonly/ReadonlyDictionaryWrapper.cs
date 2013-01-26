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

namespace Beholder.Utility.Collections.Readonly
{
    /// <summary>
    /// Represents a readonly wrapper around a dictionary.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
    public class ReadonlyDictionaryWrapper<TKey, TValue> : IReadonlyDictionary<TKey, TValue>
    {
        protected readonly IDictionary<TKey, TValue> dictionary;

        /// <summary>
        /// Gets the number of Key/Value pairs contained in the dictionary.
        /// </summary>
        public int Count
        {
            get { return dictionary.Count; }
        }

        /// <summary>
        /// Creates a new instace of ReadonlyDictionary wrapper around a given Dictionary.
        /// </summary>
        /// <param name="dictionary">Dictionary to wrap.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public ReadonlyDictionaryWrapper(IDictionary<TKey, TValue> dictionary)
        {
            if (dictionary == null)
            {
                throw new ArgumentNullException("dictionary");
            }
            this.dictionary = dictionary;
        }

        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key of the value to get.</param>
        /// <returns>
        /// The value assisiated with the provided key.
        /// </returns>
        public TValue this[TKey key]
        {
            get { return dictionary[key]; }
        }

        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key of the value to get.</param>
        /// <param name="value">
        ///     When this method returns, contains the value associated with the specified
        ///     key, if the key is found; otherwise, the default value for the type of the
        ///     value parameter. This parameter is passed uninitialized.
        /// </param>
        /// <returns>
        ///     true if the System.Collections.Generic.Dictionary&#9001;TKey,TValue&#9002; contains an
        ///     element with the specified key; otherwise, false.
        /// </returns>
        public bool TryGetValue(TKey key, out TValue value)
        {
            return dictionary.TryGetValue(key, out value);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return dictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return dictionary.GetEnumerator();
        }
    }

    public class ReadonlyDictionaryWrapper<TKey, TValue, TValueBase> : ReadonlyDictionaryWrapper<TKey, TValue>, IReadonlyDictionary<TKey, TValueBase> where TValue : TValueBase
    {
        public ReadonlyDictionaryWrapper(Dictionary<TKey, TValue> dictionary)
            : base(dictionary)
        { }

        TValueBase IReadonlyDictionary<TKey, TValueBase>.this[TKey key]
        {
            get { return dictionary[key]; }
        }

        bool IReadonlyDictionary<TKey, TValueBase>.TryGetValue(TKey key, out TValueBase value)
        {
            TValue concreteValue;

            if (dictionary.TryGetValue(key, out concreteValue))
            {
                value = concreteValue;
                return true;
            }

            value = default(TValueBase);
            return false;
        }

        IEnumerator<KeyValuePair<TKey, TValueBase>> IEnumerable<KeyValuePair<TKey, TValueBase>>.GetEnumerator()
        {
            return dictionary.Select(kvp => new KeyValuePair<TKey, TValueBase>(kvp.Key, kvp.Value)).GetEnumerator();
        }
    }

    public class ReadonlyDictionaryWrapper<TKey, TValue, TValueBase, TValueBaseBase> : ReadonlyDictionaryWrapper<TKey, TValue, TValueBase>, 
        IReadonlyDictionary<TKey, TValueBaseBase> 
        where TValue : TValueBase 
        where TValueBase : TValueBaseBase
    {
        public ReadonlyDictionaryWrapper(Dictionary<TKey, TValue> dictionary)
            : base(dictionary)
        { }

        TValueBaseBase IReadonlyDictionary<TKey, TValueBaseBase>.this[TKey key]
        {
            get { return dictionary[key]; }
        }

        bool IReadonlyDictionary<TKey, TValueBaseBase>.TryGetValue(TKey key, out TValueBaseBase value)
        {
            TValue concreteValue;

            if (dictionary.TryGetValue(key, out concreteValue))
            {
                value = concreteValue;
                return true;
            }

            value = default(TValueBase);
            return false;
        }

        IEnumerator<KeyValuePair<TKey, TValueBaseBase>> IEnumerable<KeyValuePair<TKey, TValueBaseBase>>.GetEnumerator()
        {
            return dictionary.Select(kvp => new KeyValuePair<TKey, TValueBaseBase>(kvp.Key, kvp.Value)).GetEnumerator();
        }
    }
}
