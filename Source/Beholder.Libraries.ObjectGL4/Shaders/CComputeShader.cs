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
using System.Text;
using Beholder.Shaders;
using Beholder.Utility.ForImplementations;

namespace Beholder.Libraries.ObjectGL4.Shaders
{
    class CComputeShader : CShader, IComputeShader, IDisposableInternal
    {
        public override ShaderStage Stage { get { return ShaderStage.Compute; } }

        public CComputeShader(ICDevice device, CShaderReflection reflection) : base(device, reflection)
        {
            
        }

        public void DisposeInternal()
        {
            throw new NotImplementedException();
        }

        static void WriteLayout(StringBuilder builder)
        {
            throw new NotImplementedException();
        }

        static void WriteIOAndCode(StringBuilder builder, string inputPrefix)
        {
            throw new NotImplementedException();
        }
    }
}
