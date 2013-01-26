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
using System.Globalization;

namespace Beholder.Shaders.Reflection
{
    public class ShaderObjectType : IEquatable<ShaderObjectType>
    {
        public ShaderObjectTypeClass Class { get; private set; }
        public ShaderValueType ElementType { get; private set; }
        public int? Samples { get; private set; }

        public ShaderObjectType(ShaderObjectTypeClass typeClass, ShaderValueType elementType = null, int? samples = null)
        {
            Class = typeClass;
            ElementType = elementType;
            Samples = samples;
        }

        public bool Equals(ShaderObjectType other)
        {
            if (ReferenceEquals(other, null))
                return false;
            if (ReferenceEquals(other, this))
                return true;
            return Class == other.Class && ElementType.Equals(other.ElementType) && Samples == other.Samples;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ShaderObjectType);
        }

        public override int GetHashCode()
        {
            return ElementType.GetHashCode() | ((int)Class << 18) | ((Samples ?? -1) << 23);
        }

        public override string ToString()
        {
            return Samples.HasValue
                ? string.Format("{0}<{1},{2}>", Class.ToStringFast(), ElementType, Samples.Value.ToString(CultureInfo.InvariantCulture))
                : string.Format("{0}<{1}>", Class.ToStringFast(), ElementType);
        }
    }
}