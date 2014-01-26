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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Beholder.Utility.Helpers;
using Beholder.Utility.Structures;

namespace Beholder.Shaders.Reflection
{
    public class ShaderStructureElementCollection : IReadOnlyList<ShaderValueStructureElement>, IEquatable<ShaderStructureElementCollection>
    {
        readonly ShaderValueStructureElement[] elements;

        public ShaderStructureElementCollection(IEnumerable<Pair<ShaderValueType, string>> elementsTypeNamePairs)
        {
            if (elementsTypeNamePairs == null)
                throw new ArgumentNullException("elementsTypeNamePairs");

            elements = new ShaderValueStructureElement[elementsTypeNamePairs.Count()];
            int relativeOffset = 0;
            int i = 0;

            foreach (var pair in elementsTypeNamePairs)
            {
                var type = pair.First;
                int alignment = type.Alignment;
                relativeOffset = DataHelper.Align(relativeOffset, alignment);
                elements[i] = new ShaderValueStructureElement(type, pair.Second, relativeOffset);
                relativeOffset += type.Size;
                i++;
            }
        }

        #region Implementation of IReadOnlyList<out ShaderValueStructureElement>

        public ShaderValueStructureElement this[int index]
        {
            get { return elements[index]; }
        }

        public ShaderValueStructureElement Get(int index)
        {
            return elements[index];
        }

        public int Count { get { return elements.Length; } }

        #endregion

        #region Implementation of IEquatable<ShaderStructureElementCollection>

        public bool Equals(ShaderStructureElementCollection other)
        {
            if (elements.Length != other.elements.Length)
                return false;
            for (int i = 0; i < elements.Length; i++)
                if (!elements[i].Equals(other.elements[i]))
                    return false;
            return true;
        }

        #endregion

        #region Implementation of IEnumerable

        public IEnumerator<ShaderValueStructureElement> GetEnumerator()
        {
            return ((IEnumerable<ShaderValueStructureElement>)elements).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}
