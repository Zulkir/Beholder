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
using System.Linq;
using Beholder.Utility.Helpers;
using Beholder.Utility.Structures;

namespace Beholder.Shaders.Reflection
{
    public class ShaderValueStructureType : ShaderValueType, IEquatable<ShaderValueStructureType>
    {
        readonly string name;
        readonly ShaderStructureElementCollection elements;
        readonly int alignment;
        readonly int size;

        public override ShaderValueTypeClass Class { get { return ShaderValueTypeClass.Structure; } }
        public override int Alignment { get { return alignment; } }
        public override int Size { get { return size; } }
        public string Name { get { return name; } }
        public ShaderStructureElementCollection Elements { get { return elements; } }

        public ShaderValueStructureType(string name, IEnumerable<Pair<ShaderValueType, string>> elementsTypeNamePairs)
        {
            if (name == null)
                throw new ArgumentNullException("name");
            if (elementsTypeNamePairs == null)
                throw new ArgumentNullException("elementsTypeNamePairs");

            this.name = name;
            this.elements = new ShaderStructureElementCollection(elementsTypeNamePairs);

            alignment = DataHelper.Align(elements.Max(e => e.Type.Alignment), 16);
            size = DataHelper.Align(elements.Last().Type.Size, alignment);
        }

        public override bool TryGetBaseType(out ShaderValueBaseType baseTypeOut)
        {
            ShaderValueBaseType firstBaseType;
            if (elements.Count == 0 || !elements[0].Type.TryGetBaseType(out firstBaseType))
            {
                baseTypeOut = 0;
                return false;
            }

            baseTypeOut = firstBaseType;

            foreach (var element in elements.Skip(1))
            {
                ShaderValueBaseType elementBaseType;
                if (!element.Type.TryGetBaseType(out elementBaseType) || elementBaseType != firstBaseType)
                    return false;
            }

            return true;
        }

        public bool Equals(ShaderValueStructureType other)
        {
            if (ReferenceEquals(other, null))
                return false;
            if (ReferenceEquals(other, this))
                return true;
            return name == other.name && elements.Equals(other.elements);
        }

        public override bool Equals(ShaderValueType other)
        {
            return Equals(other as ShaderValueStructureType);
        }

        protected override int DoGetHashCode()
        {
            return (Name.GetHashCode() & (0x3ffff)) | 0x40000;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
