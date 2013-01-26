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
using System.Collections.Generic;
using System.Linq;
using Beholder.Shaders.Reflection;
using Beholder.Utility.Collections;
using Beholder.Utility.Collections.Readonly;

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
        readonly TightConcurrentDictionary<string, string> meta;
        readonly ReadonlyArrayWrapper<ShaderValueStructureType> roStructures;
        readonly ReadonlyArrayWrapper<TShaderUniformBufferVariable> roBuffers;
        readonly ReadonlyArrayWrapper<TShaderSamplerVariable> roSamplers;
        readonly ReadonlyArrayWrapper<TShaderObjectVariable> roSrvs;
        readonly ReadonlyArrayWrapper<TShaderObjectVariable> roUavs;
        readonly ReadonlyArrayWrapper<TShaderIOVariable> roInput;
        readonly ReadonlyArrayWrapper<TShaderIOVariable> roInputExtra;
        readonly ReadonlyArrayWrapper<TShaderIOVariable> roInputPatch;
        readonly ReadonlyArrayWrapper<TShaderIOVariable> roInputPatchExtra;
        readonly ReadonlyArrayWrapper<TShaderIOVariable> roOutput;
        readonly ReadonlyArrayWrapper<TShaderIOVariable> roOutputPatch;
        readonly ReadonlyArrayWrapper<ReadonlyArrayWrapper<TShaderIOVariable>> roOutputStreams;
        readonly ReadonlyArrayWrapper<TShaderFixedSamplingPair> roFixedSamplingPairs;
        readonly ReadonlyArrayWrapper<string> roCodeGlobalLines;
        readonly ReadonlyArrayWrapper<string> roCodeMainLines;
        readonly ReadonlyArrayWrapper<string> roCodePatchLines;
        #endregion

        #region Implementation of IShaderReflection

        bool IShaderReflection.TryGetMeta(string metaName, out string metaValue)
        {
            return meta.TryGet(metaName, out metaValue);
        }

        public IEnumerable<KeyValuePair<string, string>> Meta { get { return meta; } }
        IReadonlyList<ShaderValueStructureType> IShaderReflection.Structures { get { return roStructures; } }
        IReadonlyList<ShaderUniformBufferVariable> IShaderReflection.UniformBuffers { get { return roBuffers; } }
        IReadonlyList<ShaderSamplerVariable> IShaderReflection.Samplers { get { return roSamplers; } }
        IReadonlyList<ShaderObjectVariable> IShaderReflection.Srvs { get { return roSrvs; } }
        IReadonlyList<ShaderObjectVariable> IShaderReflection.Uavs { get { return roUavs; } }
        IReadonlyList<ShaderIOVariable> IShaderReflection.Input { get { return roInput; } }
        IReadonlyList<ShaderIOVariable> IShaderReflection.InputExtra { get { return roInputExtra; } }
        IReadonlyList<ShaderIOVariable> IShaderReflection.InputPatch { get { return roInputPatch; } }
        IReadonlyList<ShaderIOVariable> IShaderReflection.InputPatchExtra { get { return roInputPatchExtra; } }
        IReadonlyList<ShaderIOVariable> IShaderReflection.Output { get { return roOutput; } }
        IReadonlyList<ShaderIOVariable> IShaderReflection.OutputPatch { get { return roOutputPatch; } }
        IReadonlyList<IReadonlyList<ShaderIOVariable>> IShaderReflection.OutputStreams { get { return roOutputStreams; } }
        IReadonlyList<ShaderFixedSamplingPair> IShaderReflection.FixedSamplingPairs { get { return roFixedSamplingPairs; } }
        IReadonlyList<string> IShaderReflection.CodeGlobalLines { get { return roCodeGlobalLines; } }
        IReadonlyList<string> IShaderReflection.CodeMainLines { get { return roCodeMainLines; } }
        IReadonlyList<string> IShaderReflection.CodePatchLines { get { return roCodePatchLines; } }

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
            roStructures = new ReadonlyArrayWrapper<ShaderValueStructureType>(structures);

            ubuffers = bReflection.UniformBuffers.Select(createUBuffer).ToArray();
            roBuffers = new ReadonlyArrayWrapper<TShaderUniformBufferVariable>(ubuffers);

            samplers = bReflection.Samplers.Select(createSampler).ToArray();
            roSamplers = new ReadonlyArrayWrapper<TShaderSamplerVariable>(samplers);

            srvs = bReflection.Srvs.Select(createObject).ToArray();
            roSrvs = new ReadonlyArrayWrapper<TShaderObjectVariable>(srvs);

            uavs = bReflection.Uavs.Select(createObject).ToArray();
            roUavs = new ReadonlyArrayWrapper<TShaderObjectVariable>(uavs);

            input = bReflection.Input.Select(createIO).ToArray();
            roInput = new ReadonlyArrayWrapper<TShaderIOVariable>(input);

            inputExtra = bReflection.InputExtra.Select(createIO).ToArray();
            roInputExtra = new ReadonlyArrayWrapper<TShaderIOVariable>(inputExtra);

            inputPatch = bReflection.InputPatch.Select(createIO).ToArray();
            roInputPatch = new ReadonlyArrayWrapper<TShaderIOVariable>(inputPatch);

            inputPatchExtra = bReflection.InputPatchExtra.Select(createIO).ToArray();
            roInputPatchExtra = new ReadonlyArrayWrapper<TShaderIOVariable>(inputPatchExtra);

            output = bReflection.Output.Select(createIO).ToArray();
            roOutput = new ReadonlyArrayWrapper<TShaderIOVariable>(output);

            outputPatch = bReflection.OutputPatch.Select(createIO).ToArray();
            roOutputPatch = new ReadonlyArrayWrapper<TShaderIOVariable>(outputPatch);

            outputStreams = bReflection.OutputStreams.Select(s => s.Select(createIO).ToArray()).ToArray();
            roOutputStreams = new ReadonlyArrayWrapper<ReadonlyArrayWrapper<TShaderIOVariable>>(outputStreams.Select(s => new ReadonlyArrayWrapper<TShaderIOVariable>(s)).ToArray());

            fixedSamplingPairs = bReflection.FixedSamplingPairs.Select(p => createFixedSamplingPair(p, samplers, srvs)).ToArray();
            roFixedSamplingPairs = new ReadonlyArrayWrapper<TShaderFixedSamplingPair>(fixedSamplingPairs);

            codeGlobalLines = bReflection.CodeGlobalLines.ToArray();
            roCodeGlobalLines = new ReadonlyArrayWrapper<string>(codeGlobalLines);

            codeMainLines = bReflection.CodeMainLines.ToArray();
            roCodeMainLines = new ReadonlyArrayWrapper<string>(codeMainLines);

            codePatchLines = bReflection.CodePatchLines.ToArray();
            roCodePatchLines = new ReadonlyArrayWrapper<string>(codePatchLines);
        }
    }
}
