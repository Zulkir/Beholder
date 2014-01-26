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
using System.Collections.Generic;

namespace Beholder.Shaders.Reflection
{
    public class ShaderIOVariable : ShaderSpecialParametersHolder
    {
        readonly ShaderIOVariableInterpolation interpolation;
        readonly ShaderValueType type;
        readonly ShaderValueBaseType baseType;

        public ShaderIOVariable(ShaderIOVariableInterpolation interpolation, ShaderValueType type, string name, IEnumerable<KeyValuePair<string, string>> specialParameters)
            : base(name, specialParameters)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            this.interpolation = interpolation;
            this.type = type;
            if (!type.TryGetBaseType(out baseType))
                throw new ArgumentException("Type of a shader IO variable must have a base type");
        }

        public ShaderIOVariableInterpolation Interpolation { get { return interpolation; } }
        public ShaderValueType Type { get { return type; } }
        public int LayoutElementsRequired { get { return type.Class == ShaderValueTypeClass.Array ? ((ShaderValueArrayType)type).Length : 1; } }
        public ShaderValueBaseType BaseType { get { return baseType; } }
    }
}
