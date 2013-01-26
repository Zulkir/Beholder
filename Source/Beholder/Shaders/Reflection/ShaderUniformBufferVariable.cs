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

using System.Collections.Generic;
using System.Linq;
using Beholder.Utility.Helpers;
using Beholder.Utility.Structures;

namespace Beholder.Shaders.Reflection
{
    // todo: rename
    public class ShaderUniformBufferVariable : ShaderSpecialParametersHolder
    {
        readonly ShaderStructureElementCollection elements;
        readonly int size;

        public ShaderStructureElementCollection Elements { get { return elements; } }
        public int Size { get { return size; } }

        public ShaderUniformBufferVariable(string name, IEnumerable<Pair<ShaderValueType, string>> elementsTypeNamePairs, IEnumerable<KeyValuePair<string, string>> specialParameters) 
            : base(name, specialParameters)
        {
            elements = new ShaderStructureElementCollection(elementsTypeNamePairs);

            var alignment = DataHelper.Align(elements.Max(e => e.Type.Alignment), 16);
            size = DataHelper.Align(elements.Last().Type.Size, alignment);
        }
    }
}
