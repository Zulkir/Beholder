﻿/*
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

using Beholder.Shaders.Reflection;
using Beholder.Utility.Structures;
using System.Linq;

namespace Beholder.Libraries.ObjectGL4.Shaders
{
    class CShaderUniformBufferVariable : ShaderUniformBufferVariable
    {
        readonly int bSlot;
        readonly int glSlot;

        public int BSlot { get { return bSlot; } }
        public int GLSlot { get { return glSlot; } }

        public CShaderUniformBufferVariable(ShaderUniformBufferVariable bVariable)
            : base(bVariable.Name, bVariable.Elements.Select(e => new Pair<ShaderValueType, string>(e.Type, e.Name)), bVariable.SpecialParameters)
        {
            bSlot = bVariable.GetSlot();
            glSlot = bVariable.GetSlotGL();
        }
    }
}
