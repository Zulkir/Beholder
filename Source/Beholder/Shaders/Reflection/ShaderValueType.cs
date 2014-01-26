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

namespace Beholder.Shaders.Reflection
{
    public abstract class ShaderValueType : IEquatable<ShaderValueType>
    {
        public abstract ShaderValueTypeClass Class { get; }
        public abstract int Alignment { get; }
        public abstract int Size { get; }
        public abstract bool Equals(ShaderValueType other);
        protected abstract int DoGetHashCode();
        public abstract bool TryGetBaseType(out ShaderValueBaseType baseType);

        public override bool Equals(object obj)
        {
            return Equals(obj as ShaderValueType);
        }

        public override int GetHashCode()
        {
            return DoGetHashCode();
        }

        public static bool AreEqual(ShaderValueType t1, ShaderValueType t2)
        {
            return ReferenceEquals(t1, null) ? ReferenceEquals(t2, null) : t1.Equals(t2);
        }

        public static bool operator == (ShaderValueType t1, ShaderValueType t2)
        {
            return AreEqual(t1, t2);
        }

        public static bool operator !=(ShaderValueType t1, ShaderValueType t2)
        {
            return !AreEqual(t1, t2);
        }
    }
}
