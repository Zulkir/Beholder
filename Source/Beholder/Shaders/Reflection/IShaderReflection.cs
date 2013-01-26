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
using Beholder.Utility.Collections.Readonly;

namespace Beholder.Shaders.Reflection
{
    public interface IShaderReflection
    {
        IEnumerable<KeyValuePair<string, string>> Meta { get; } 
        bool TryGetMeta(string metaName, out string metaValue);
        IReadonlyList<ShaderValueStructureType> Structures { get; } 
        IReadonlyList<ShaderUniformBufferVariable> UniformBuffers { get; }
        IReadonlyList<ShaderSamplerVariable> Samplers { get; } 
        IReadonlyList<ShaderObjectVariable> Srvs { get; } 
        IReadonlyList<ShaderObjectVariable> Uavs { get; } 
        IReadonlyList<ShaderIOVariable> Input { get; } 
        IReadonlyList<ShaderIOVariable> InputExtra { get; } 
        IReadonlyList<ShaderIOVariable> InputPatch { get; } 
        IReadonlyList<ShaderIOVariable> InputPatchExtra { get; } 
        IReadonlyList<ShaderIOVariable> Output { get; }
        IReadonlyList<ShaderIOVariable> OutputPatch { get; }
        IReadonlyList<IReadonlyList<ShaderIOVariable>> OutputStreams { get; }
        IReadonlyList<ShaderFixedSamplingPair> FixedSamplingPairs { get; } 
        IReadonlyList<string> CodeGlobalLines { get; } 
        IReadonlyList<string> CodeMainLines { get; } 
        IReadonlyList<string> CodePatchLines { get; } 
    }
}
