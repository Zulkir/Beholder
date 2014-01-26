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
using System.Runtime.InteropServices;

namespace Beholder.Platform
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ApiVersion : IEquatable<ApiVersion>, IComparable<ApiVersion>
    {
        public byte Minor;
        public byte Major;

        public ApiVersion(byte major, byte minor)
        {
            Major = major;
            Minor = minor;
        }

        public override int GetHashCode()
        {
            return Minor | (Major << 8);
        }

        public override string ToString()
        {
            return string.Format("{0}.{1}", Major, Minor);
        }

        public static bool operator ==(ApiVersion v1, ApiVersion v2)
        {
            return v1.GetHashCode() == v2.GetHashCode();
        }

        public static bool operator !=(ApiVersion v1, ApiVersion v2)
        {
            return v1.GetHashCode() != v2.GetHashCode();
        }

        public bool Equals(ApiVersion other)
        {
            return GetHashCode().Equals(other.GetHashCode());
        }

        public override bool Equals(object obj)
        {
            return obj != null && obj is ApiVersion && Equals((ApiVersion)obj);
        }

        public int CompareTo(ApiVersion other)
        {
            return GetHashCode().CompareTo(other.GetHashCode());
        }

        public static bool operator <(ApiVersion left, ApiVersion right)
        {
            return left.GetHashCode() < right.GetHashCode();
        }

        public static bool operator <=(ApiVersion left, ApiVersion right)
        {
            return left.GetHashCode() <= right.GetHashCode();
        }

        public static bool operator >(ApiVersion left, ApiVersion right)
        {
            return left.GetHashCode() > right.GetHashCode();
        }

        public static bool operator >=(ApiVersion left, ApiVersion right)
        {
            return left.GetHashCode() >= right.GetHashCode();
        }
    }
}
