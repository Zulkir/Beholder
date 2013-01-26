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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Beholder.Validation
{
    static class Check
    {
        static readonly ConcurrentDictionary<Type, int[]> ValidEnums = new ConcurrentDictionary<Type, int[]>();

        public static void NotNull<T>(T arg, string argName) where T : class
        {
            if (ReferenceEquals(null, arg)) throw new ArgumentNullException(argName);
        }

        static void Internal<T>(T arg, string argName) where T : class
        {
            if (!(arg is IWrapper<T>)) throw new ArgumentException(string.Format("{0} is not created by this eye", argName), argName);
        }

        public static void ExistingInternal<T>(T arg, string argName) where T : class
        {
            NotNull(arg, argName);
            Internal(arg, argName);
        }

        public static void NullOrInternal<T>(T arg, string argName) where T : class
        {
            if (!ReferenceEquals(null, arg))
                Internal(arg, argName);
        }

        public static void NotNegative(int arg, string argName)
        {
            if (arg < 0) throw new ArgumentException(string.Format("{0} cannot be negative", argName), argName);
        }

        public static void NotNegative(float arg, string argName)
        {
            if (arg < 0) throw new ArgumentException(string.Format("{0} cannot be negative", argName), argName);
        }

        public static void Positive(int arg, string argName)
        {
            if (arg <= 0) throw new ArgumentException(string.Format("{0} must be greater than zero", argName), argName);
        }

        public static void NotNullNotEmpty(string arg, string argName)
        {
            if (string.IsNullOrEmpty(arg)) throw new ArgumentException(string.Format("'{0}' can be neither null nor empty", argName), argName);
        }

        public static void NotNullNotEmpty<T>(IEnumerable<T> arg, string argName)
        {
            if (ReferenceEquals(null, arg))
                throw new ArgumentNullException(argName);
            if (!arg.Any())
                throw new ArgumentException(string.Format("{0} can not be empty", argName), argName);
        }

        public static void NullOrEmpty<T>(IEnumerable<T> arg, string argName) where T : class
        {
            if (!ReferenceEquals(null, arg) && arg.Any())
                throw new ArgumentException(string.Format("{0} cannot contain anyhing", argName), argName);
        }

        public static void Enumeration<T>(T arg, string argName) where T : IConvertible, IComparable
        {
            var array = ValidEnums.GetOrAdd(typeof(T), t => ((T[])Enum.GetValues(typeof(T))).Select(x => x.ToInt32(CultureInfo.InvariantCulture)).ToArray());
            if (!array.Contains(arg.ToInt32(CultureInfo.InvariantCulture)))
                throw new ArgumentException(string.Format("{0} ({1}) is not a valid {2}", argName, arg, typeof(T).Name));
        }

        static readonly ConcurrentDictionary<Type, int> Masks = new ConcurrentDictionary<Type, int>();

        public static void Flags<T>(T arg, string argName) where T : IConvertible, IComparable
        {
            var mask = Masks.GetOrAdd(typeof(T), t => ((T[])Enum.GetValues(typeof(T))).Select(x => x.ToInt32(CultureInfo.InvariantCulture)).Aggregate((x, y) => x | y));
            if ((mask | arg.ToInt32(CultureInfo.InvariantCulture)) != mask)
                throw new ArgumentException(string.Format("{0} ({1}) is not a valid {2}", argName, arg, typeof(T).Name));
        }
    }
}
