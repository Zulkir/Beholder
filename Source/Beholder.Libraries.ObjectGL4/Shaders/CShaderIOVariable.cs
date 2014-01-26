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

using System.Collections.Generic;
using System.Linq;
using Beholder.Shaders.Reflection;

namespace Beholder.Libraries.ObjectGL4.Shaders
{
    class CShaderIOVariable : ShaderIOVariable
    {
        readonly string semantic;
        readonly bool isSystem;
        readonly bool isUsed;

        public string Semantic { get { return semantic; } }
        public bool IsSystem { get { return isSystem; } }
        public bool IsUsed { get { return isUsed; } }

        public CShaderIOVariable(ShaderIOVariable bVariable) 
            : base(bVariable.Interpolation, bVariable.Type, bVariable.Name, bVariable.SpecialParameters)
        {
            semantic = bVariable.GetSemanticGL();
            if (semantic == "%name")
                semantic = Name;
            isSystem = semantic.StartsWith("gl_");
            isUsed = semantic != "%unused";
        }

        public static bool AreCompatible(IEnumerable<CShaderIOVariable> output, IEnumerable<CShaderIOVariable> input)
        {
            return input.Where(i => i.IsUsed && !i.IsSystem).All(i => output.Any(o => o.Semantic == i.Semantic && o.Type.Equals(i.Type)));
        }
    }
}