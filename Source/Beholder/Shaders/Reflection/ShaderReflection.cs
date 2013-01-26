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
using Beholder.Utility.Collections;
using Beholder.Utility.Collections.Readonly;

namespace Beholder.Shaders.Reflection
{
    public class ShaderReflection : IShaderReflection
    {
        readonly TightConcurrentDictionary<string, string> meta;
        readonly ReadonlyArrayWrapper<ShaderValueStructureType> roStructures; 
        readonly ReadonlyArrayWrapper<ShaderUniformBufferVariable> roBuffers;
        readonly ReadonlyArrayWrapper<ShaderSamplerVariable> roSamplers;
        readonly ReadonlyArrayWrapper<ShaderObjectVariable> roSrvs;
        readonly ReadonlyArrayWrapper<ShaderObjectVariable> roUavs;
        readonly ReadonlyArrayWrapper<ShaderIOVariable> roInput;
        readonly ReadonlyArrayWrapper<ShaderIOVariable> roInputExtra;
        readonly ReadonlyArrayWrapper<ShaderIOVariable> roInputPatch;
        readonly ReadonlyArrayWrapper<ShaderIOVariable> roInputPatchExtra;
        readonly ReadonlyArrayWrapper<ShaderIOVariable> roOutput;
        readonly ReadonlyArrayWrapper<ShaderIOVariable> roOutputPatch;
        readonly ReadonlyArrayWrapper<ReadonlyArrayWrapper<ShaderIOVariable>> roOutputStreams;
        readonly ReadonlyArrayWrapper<ShaderFixedSamplingPair> roFixedSamplingPairs;
        readonly ReadonlyArrayWrapper<string> roCodeGlobalLines;
        readonly ReadonlyArrayWrapper<string> roCodeMainLines;
        readonly ReadonlyArrayWrapper<string> roCodePatchLines;

        public ShaderReflection(IEnumerable<KeyValuePair<string, string>> meta, 
            IEnumerable<ShaderValueStructureType> structures, IEnumerable<ShaderUniformBufferVariable> uniformBuffers = null,
            IEnumerable<ShaderSamplerVariable> samplers = null, IEnumerable<ShaderObjectVariable> srvs = null, IEnumerable<ShaderObjectVariable> uavs = null, 
            IEnumerable<ShaderIOVariable> input = null,
            IEnumerable<ShaderIOVariable> inputExtra = null,
            IEnumerable<ShaderIOVariable> inputPatch = null,
            IEnumerable<ShaderIOVariable> inputPatchExtra = null,
            IEnumerable<ShaderIOVariable> output = null,
            IEnumerable<ShaderIOVariable> outputPatch = null,
            IEnumerable<IEnumerable<ShaderIOVariable>> outputStreams = null, 
            IEnumerable<ShaderFixedSamplingPair> fixedSamplingPairs = null, 
            IEnumerable<string> codeGlobalLines = null,
            IEnumerable<string> codeMainLines = null,
            IEnumerable<string> codePatchLines = null)
        {
            meta = meta ?? new KeyValuePair<string, string>[0];
            this.meta = new TightConcurrentDictionary<string, string>(string.Equals, meta.Count());
            foreach (var specialParameter in meta)
                this.meta.GetOrAdd(specialParameter.Key, specialParameter.Value, (k, v) => v);

            roStructures = structures != null ? new ReadonlyArrayWrapper<ShaderValueStructureType>(structures.ToArray()) : ReadonlyArrayWrapper<ShaderValueStructureType>.Empty;
            roBuffers = uniformBuffers != null ? new ReadonlyArrayWrapper<ShaderUniformBufferVariable>(uniformBuffers.ToArray()) : ReadonlyArrayWrapper<ShaderUniformBufferVariable>.Empty;
            roSamplers = samplers != null ? new ReadonlyArrayWrapper<ShaderSamplerVariable>(samplers.ToArray()) : ReadonlyArrayWrapper<ShaderSamplerVariable>.Empty;
            roSrvs = srvs != null ? new ReadonlyArrayWrapper<ShaderObjectVariable>(srvs.ToArray()) : ReadonlyArrayWrapper<ShaderObjectVariable>.Empty;
            roUavs = uavs != null ? new ReadonlyArrayWrapper<ShaderObjectVariable>(uavs.ToArray()) : ReadonlyArrayWrapper<ShaderObjectVariable>.Empty;
            roInput = input != null ? new ReadonlyArrayWrapper<ShaderIOVariable>(input.ToArray()) : ReadonlyArrayWrapper<ShaderIOVariable>.Empty;
            roInputExtra = inputExtra != null ? new ReadonlyArrayWrapper<ShaderIOVariable>(inputExtra.ToArray()) : ReadonlyArrayWrapper<ShaderIOVariable>.Empty;
            roInputPatch = inputPatch != null ? new ReadonlyArrayWrapper<ShaderIOVariable>(inputPatch.ToArray()) : ReadonlyArrayWrapper<ShaderIOVariable>.Empty;
            roInputPatchExtra = inputPatchExtra != null ? new ReadonlyArrayWrapper<ShaderIOVariable>(inputPatchExtra.ToArray()) : ReadonlyArrayWrapper<ShaderIOVariable>.Empty;
            roOutput = output != null ? new ReadonlyArrayWrapper<ShaderIOVariable>(output.ToArray()) : ReadonlyArrayWrapper<ShaderIOVariable>.Empty;
            roOutputPatch = outputPatch != null ? new ReadonlyArrayWrapper<ShaderIOVariable>(outputPatch.ToArray()) : ReadonlyArrayWrapper<ShaderIOVariable>.Empty;
            roOutputStreams = outputStreams != null 
                ? new ReadonlyArrayWrapper<ReadonlyArrayWrapper<ShaderIOVariable>>(outputStreams.Select(o => 
                    new ReadonlyArrayWrapper<ShaderIOVariable>(o.ToArray())).ToArray())
                : ReadonlyArrayWrapper<ReadonlyArrayWrapper<ShaderIOVariable>>.Empty;
            roFixedSamplingPairs = fixedSamplingPairs != null ? new ReadonlyArrayWrapper<ShaderFixedSamplingPair>(fixedSamplingPairs.ToArray()) : ReadonlyArrayWrapper<ShaderFixedSamplingPair>.Empty;
            roCodeGlobalLines = codeGlobalLines != null ? new ReadonlyArrayWrapper<string>(codeGlobalLines.ToArray()) : ReadonlyArrayWrapper<string>.Empty;
            roCodeMainLines = codeMainLines != null ? new ReadonlyArrayWrapper<string>(codeMainLines.ToArray()) : ReadonlyArrayWrapper<string>.Empty;
            roCodePatchLines = codePatchLines != null ? new ReadonlyArrayWrapper<string>(codePatchLines.ToArray()) : ReadonlyArrayWrapper<string>.Empty;
        }

        public bool TryGetMeta(string parameterName, out string parameterValue)
        {
            return meta.TryGet(parameterName, out parameterValue);
        }

        public IEnumerable<KeyValuePair<string, string>> Meta { get { return meta; } } 
        public IReadonlyList<ShaderValueStructureType> Structures { get { return roStructures; } } 
        public IReadonlyList<ShaderUniformBufferVariable> UniformBuffers { get { return roBuffers; } }
        public IReadonlyList<ShaderSamplerVariable> Samplers { get { return roSamplers; } }
        public IReadonlyList<ShaderObjectVariable> Srvs { get { return roSrvs; } }
        public IReadonlyList<ShaderObjectVariable> Uavs { get { return roUavs; } }
        public IReadonlyList<ShaderIOVariable> Input { get { return roInput; } }
        public IReadonlyList<ShaderIOVariable> InputExtra { get { return roInputExtra; } }
        public IReadonlyList<ShaderIOVariable> InputPatch { get { return roInputPatch; } }
        public IReadonlyList<ShaderIOVariable> InputPatchExtra { get { return roInputPatchExtra; } }
        public IReadonlyList<ShaderIOVariable> Output { get { return roOutput; } }
        public IReadonlyList<ShaderIOVariable> OutputPatch { get { return roOutputPatch; } }
        public IReadonlyList<IReadonlyList<ShaderIOVariable>> OutputStreams { get { return roOutputStreams; } }
        public IReadonlyList<ShaderFixedSamplingPair> FixedSamplingPairs { get { return roFixedSamplingPairs; } } 
        public IReadonlyList<string> CodeGlobalLines { get { return roCodeGlobalLines; } }
        public IReadonlyList<string> CodeMainLines { get { return roCodeMainLines; } }
        public IReadonlyList<string> CodePatchLines { get { return roCodePatchLines; } }

        public static ShaderStage ParseShaderStage(string stageString)
        {
            switch (stageString)
            {
                case "Vertex": return ShaderStage.Vertex;
                case "Hull": return ShaderStage.Hull;
                case "Domain": return ShaderStage.Domain;
                case "Geometry": return ShaderStage.Geometry;
                case "Pixel": return ShaderStage.Pixel;
                case "Compute": return ShaderStage.Compute;
                default: throw new ArgumentOutOfRangeException("stageString", string.Format("Unknown shader stage '{0}'", stageString));
            }
        }
    }
}
