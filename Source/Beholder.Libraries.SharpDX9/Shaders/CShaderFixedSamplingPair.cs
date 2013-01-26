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
using Beholder.Shaders.Reflection;
using System.Linq;

namespace Beholder.Libraries.SharpDX9.Shaders
{
    class CShaderFixedSamplingPair : ShaderFixedSamplingPair
    {
        readonly int bSamplerSlot;
        readonly CShaderObjectVariable srv;

        public int BSamplerSlot { get { return bSamplerSlot; } }
        public CShaderObjectVariable Srv { get { return srv; } }

        public CShaderFixedSamplingPair(ShaderFixedSamplingPair bPair, IEnumerable<ShaderSamplerVariable> samplers, IEnumerable<CShaderObjectVariable> srvs) 
            : base(bPair.SrvName, bPair.SamplerName)
        {
            bSamplerSlot = samplers.Single(s => s.Name == SamplerName).GetSlot();
            srv = srvs.Single(s => s.Name == SrvName);
        }
    }
}