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

namespace Beholder.Shaders.Reflection
{
    public interface IShaderReflection
    {
        IEnumerable<KeyValuePair<string, string>> Meta { get; } 
        bool TryGetMeta(string metaName, out string metaValue);
        IReadOnlyList<ShaderValueStructureType> Structures { get; } 
        IReadOnlyList<ShaderUniformBufferVariable> UniformBuffers { get; }
        IReadOnlyList<ShaderSamplerVariable> Samplers { get; } 
        IReadOnlyList<ShaderObjectVariable> Srvs { get; } 
        IReadOnlyList<ShaderObjectVariable> Uavs { get; } 
        IReadOnlyList<ShaderIOVariable> Input { get; } 
        IReadOnlyList<ShaderIOVariable> InputExtra { get; } 
        IReadOnlyList<ShaderIOVariable> InputPatch { get; } 
        IReadOnlyList<ShaderIOVariable> InputPatchExtra { get; } 
        IReadOnlyList<ShaderIOVariable> Output { get; }
        IReadOnlyList<ShaderIOVariable> OutputPatch { get; }
        IReadOnlyList<IReadOnlyList<ShaderIOVariable>> OutputStreams { get; }
        IReadOnlyList<ShaderFixedSamplingPair> FixedSamplingPairs { get; } 
        IReadOnlyList<string> CodeGlobalLines { get; } 
        IReadOnlyList<string> CodeMainLines { get; } 
        IReadOnlyList<string> CodePatchLines { get; } 
    }
}
