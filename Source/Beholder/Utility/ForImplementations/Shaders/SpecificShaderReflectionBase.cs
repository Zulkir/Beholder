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
using System.Linq;
using Beholder.Shaders.Reflection;
using Beholder.Utility.Collections;

namespace Beholder.Utility.ForImplementations.Shaders
{
    public abstract class SpecificShaderReflectionBase<TShaderUniformBufferVariable, TShaderSamplerVariable, TShaderObjectVariable, TShaderIOVariable, TShaderFixedSamplingPair> : IShaderReflection
        where TShaderUniformBufferVariable : ShaderUniformBufferVariable
        where TShaderSamplerVariable : ShaderSamplerVariable
        where TShaderObjectVariable : ShaderObjectVariable
        where TShaderIOVariable : ShaderIOVariable
        where TShaderFixedSamplingPair : ShaderFixedSamplingPair
    {
        readonly string name;
        readonly TightConcurrentDictionary<string, string> meta; 
        readonly ShaderValueStructureType[] structures;
        readonly TShaderUniformBufferVariable[] ubuffers;
        readonly TShaderSamplerVariable[] samplers;
        readonly TShaderObjectVariable[] srvs;
        readonly TShaderObjectVariable[] uavs;
        readonly TShaderIOVariable[] input;
        readonly TShaderIOVariable[] inputExtra;
        readonly TShaderIOVariable[] inputPatch;
        readonly TShaderIOVariable[] inputPatchExtra;
        readonly TShaderIOVariable[] output;
        readonly TShaderIOVariable[] outputPatch;
        readonly TShaderIOVariable[][] outputStreams;
        readonly TShaderFixedSamplingPair[] fixedSamplingPairs;
        readonly string[] codeGlobalLines;
        readonly string[] codeMainLines;
        readonly string[] codePatchLines;

        // todo: check these properties are needed
        public string Name { get { return name; } }
        public ShaderValueStructureType[] Structures { get { return structures; } }
        public TShaderUniformBufferVariable[] UniformBuffers { get { return ubuffers; } }
        public TShaderSamplerVariable[] Samplers { get { return samplers; } }
        public TShaderObjectVariable[] Srvs { get { return srvs; } }
        public TShaderObjectVariable[] Uavs { get { return uavs; } }
        public TShaderIOVariable[] Input { get { return input; } }
        public TShaderIOVariable[] InputExtra { get { return inputExtra; } }
        public TShaderIOVariable[] InputPatch { get { return inputPatch; } }
        public TShaderIOVariable[] InputPatchExtra { get { return inputPatchExtra; } }
        public TShaderIOVariable[] Output { get { return output; } }
        public TShaderIOVariable[] OutputPatch { get { return outputPatch; } }
        public TShaderIOVariable[][] OutputStreams { get { return outputStreams; } }
        public TShaderFixedSamplingPair[] FixedSamplingPairs { get { return fixedSamplingPairs; } }
        public string[] CodeGlobalLines { get { return codeGlobalLines; } }
        public string[] CodeMainLines { get { return codeMainLines; } }
        public string[] CodePatchLines { get { return codePatchLines; } }

        #region Implementation of IShaderReflection

        bool IShaderReflection.TryGetMeta(string metaName, out string metaValue)
        {
            return meta.TryGet(metaName, out metaValue);
        }

        public IEnumerable<KeyValuePair<string, string>> Meta { get { return meta; } }
        IReadOnlyList<ShaderValueStructureType> IShaderReflection.Structures { get { return structures; } }
        IReadOnlyList<ShaderUniformBufferVariable> IShaderReflection.UniformBuffers { get { return ubuffers; } }
        IReadOnlyList<ShaderSamplerVariable> IShaderReflection.Samplers { get { return samplers; } }
        IReadOnlyList<ShaderObjectVariable> IShaderReflection.Srvs { get { return srvs; } }
        IReadOnlyList<ShaderObjectVariable> IShaderReflection.Uavs { get { return uavs; } }
        IReadOnlyList<ShaderIOVariable> IShaderReflection.Input { get { return input; } }
        IReadOnlyList<ShaderIOVariable> IShaderReflection.InputExtra { get { return inputExtra; } }
        IReadOnlyList<ShaderIOVariable> IShaderReflection.InputPatch { get { return inputPatch; } }
        IReadOnlyList<ShaderIOVariable> IShaderReflection.InputPatchExtra { get { return inputPatchExtra; } }
        IReadOnlyList<ShaderIOVariable> IShaderReflection.Output { get { return output; } }
        IReadOnlyList<ShaderIOVariable> IShaderReflection.OutputPatch { get { return outputPatch; } }
        IReadOnlyList<IReadOnlyList<ShaderIOVariable>> IShaderReflection.OutputStreams { get { return outputStreams; } }
        IReadOnlyList<ShaderFixedSamplingPair> IShaderReflection.FixedSamplingPairs { get { return fixedSamplingPairs; } }
        IReadOnlyList<string> IShaderReflection.CodeGlobalLines { get { return codeGlobalLines; } }
        IReadOnlyList<string> IShaderReflection.CodeMainLines { get { return codeMainLines; } }
        IReadOnlyList<string> IShaderReflection.CodePatchLines { get { return codePatchLines; } }

        #endregion

        protected SpecificShaderReflectionBase(IShaderReflection bReflection, 
            Func<ShaderUniformBufferVariable, TShaderUniformBufferVariable> createUBuffer,
            Func<ShaderSamplerVariable, TShaderSamplerVariable> createSampler,
            Func<ShaderObjectVariable, TShaderObjectVariable> createObject,
            Func<ShaderIOVariable, TShaderIOVariable> createIO,
            Func<ShaderFixedSamplingPair, TShaderSamplerVariable[], TShaderObjectVariable[], TShaderFixedSamplingPair> createFixedSamplingPair)
        {
            meta = new TightConcurrentDictionary<string, string>(string.Equals, bReflection.Meta.Count());
            name = bReflection.GetRequiredMeta("Name");
            foreach (var kvp in bReflection.Meta)
                meta.GetOrAdd(kvp.Key, kvp.Value, (k, c) => c);
            structures = bReflection.Structures.ToArray();
            ubuffers = bReflection.UniformBuffers.Select(createUBuffer).ToArray();
            samplers = bReflection.Samplers.Select(createSampler).ToArray();
            srvs = bReflection.Srvs.Select(createObject).ToArray();
            uavs = bReflection.Uavs.Select(createObject).ToArray();
            input = bReflection.Input.Select(createIO).ToArray();
            inputExtra = bReflection.InputExtra.Select(createIO).ToArray();
            inputPatch = bReflection.InputPatch.Select(createIO).ToArray();
            inputPatchExtra = bReflection.InputPatchExtra.Select(createIO).ToArray();
            output = bReflection.Output.Select(createIO).ToArray();
            outputPatch = bReflection.OutputPatch.Select(createIO).ToArray();
            outputStreams = bReflection.OutputStreams.Select(s => s.Select(createIO).ToArray()).ToArray();
            fixedSamplingPairs = bReflection.FixedSamplingPairs.Select(p => createFixedSamplingPair(p, samplers, srvs)).ToArray();
            codeGlobalLines = bReflection.CodeGlobalLines.ToArray();
            codeMainLines = bReflection.CodeMainLines.ToArray();
            codePatchLines = bReflection.CodePatchLines.ToArray();
        }
    }
}
