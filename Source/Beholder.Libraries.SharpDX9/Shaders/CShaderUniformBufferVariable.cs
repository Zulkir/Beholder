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
using System.Linq;
using System.Text.RegularExpressions;
using Beholder.Shaders.Reflection;
using Beholder.Utility.Helpers;
using Beholder.Utility.Structures;

namespace Beholder.Libraries.SharpDX9.Shaders
{
    class CShaderUniformBufferVariable : ShaderUniformBufferVariable
    {
        readonly int bSlot;
        readonly ShaderRegisterType registerType;
        readonly int register;
        readonly int registerCount;

        public int BSlot { get { return bSlot; } }
        public ShaderRegisterType RegisterType { get { return registerType; } }
        public int Register { get { return register; } }
        public int RegisterCount { get { return registerCount; } }

        readonly static Regex RegisterEx = new Regex(@"^(\w)(\d+)$");

        public CShaderUniformBufferVariable(ShaderUniformBufferVariable bVariable)
            : base(bVariable.Name, bVariable.Elements.Select(e => new Pair<ShaderValueType, string>(e.Type, e.Name)), bVariable.SpecialParameters)
        {
            bSlot = bVariable.GetSlot();
            var slotString = bVariable.GetRequiredSpecialParameter("slotDX9");
            ParseSlot(slotString, out registerType, out register);
            registerCount = DataHelper.Align(bVariable.Size, 4);
        }

        static void ParseSlot(string slotString, out ShaderRegisterType registerType, out int register)
        {
            var match = RegisterEx.Match(slotString);
            if (!match.Success)
                throw new ArgumentException(string.Format("'{0}' is not a valid slotDX9 for uniform buffers", slotString));
            switch (match.Groups[1].Value)
            {
                case "c": registerType = ShaderRegisterType.C; break;
                case "i": registerType = ShaderRegisterType.I; break;
                case "b": registerType = ShaderRegisterType.B; break;
                default: throw new ArgumentException(string.Format("'{0}' is not a valid slotDX9 register letter for uniform buffers", match.Groups[1].Value));
            }
            register = int.Parse(match.Groups[2].Value);
        }
    }
}
