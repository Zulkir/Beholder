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
using Beholder.Utility.Collections;

namespace Beholder.Shaders.Reflection
{
    public class ShaderReflection : IShaderReflection
    {
        readonly TightConcurrentDictionary<string, string> meta;
        readonly ShaderValueStructureType[] structures; 
        readonly ShaderUniformBufferVariable[] buffers;
        readonly ShaderSamplerVariable[] samplers;
        readonly ShaderObjectVariable[] srvs;
        readonly ShaderObjectVariable[] uavs;
        readonly ShaderIOVariable[] input;
        readonly ShaderIOVariable[] inputExtra;
        readonly ShaderIOVariable[] inputPatch;
        readonly ShaderIOVariable[] inputPatchExtra;
        readonly ShaderIOVariable[] output;
        readonly ShaderIOVariable[] outputPatch;
        readonly ShaderIOVariable[][] outputStreams;
        readonly ShaderFixedSamplingPair[] fixedSamplingPairs;
        readonly string[] codeGlobalLines;
        readonly string[] codeMainLines;
        readonly string[] codePatchLines;

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

            this.structures = (structures ?? Enumerable.Empty<ShaderValueStructureType>()).ToArray();
            buffers = (uniformBuffers ?? Enumerable.Empty<ShaderUniformBufferVariable>()).ToArray();
            this.samplers = (samplers ?? Enumerable.Empty<ShaderSamplerVariable>()).ToArray();
            this.srvs = (srvs ?? Enumerable.Empty<ShaderObjectVariable>()).ToArray();
            this.uavs = (uavs ?? Enumerable.Empty<ShaderObjectVariable>()).ToArray();
            this.input = (input ?? Enumerable.Empty<ShaderIOVariable>()).ToArray();
            this.inputExtra = (inputExtra ?? Enumerable.Empty<ShaderIOVariable>()).ToArray();
            this.inputPatch = (inputPatch ?? Enumerable.Empty<ShaderIOVariable>()).ToArray();
            this.inputPatchExtra = (inputPatchExtra ?? Enumerable.Empty<ShaderIOVariable>()).ToArray();
            this.output = (output ?? Enumerable.Empty<ShaderIOVariable>()).ToArray();
            this.outputPatch = (outputPatch ?? Enumerable.Empty<ShaderIOVariable>()).ToArray();
            this.outputStreams = (outputStreams ?? Enumerable.Empty<IEnumerable<ShaderIOVariable>>()).Select(x => x.ToArray()).ToArray();
            this.fixedSamplingPairs = (fixedSamplingPairs ?? Enumerable.Empty<ShaderFixedSamplingPair>()).ToArray();
            this.codeGlobalLines = (codeGlobalLines ?? Enumerable.Empty<string>()).ToArray();
            this.codeMainLines = (codeMainLines ?? Enumerable.Empty<string>()).ToArray();
            this.codePatchLines = (codePatchLines ?? Enumerable.Empty<string>()).ToArray();
        }

        public bool TryGetMeta(string parameterName, out string parameterValue)
        {
            return meta.TryGet(parameterName, out parameterValue);
        }

        public IEnumerable<KeyValuePair<string, string>> Meta { get { return meta; } } 
        public IReadOnlyList<ShaderValueStructureType> Structures { get { return structures; } } 
        public IReadOnlyList<ShaderUniformBufferVariable> UniformBuffers { get { return buffers; } }
        public IReadOnlyList<ShaderSamplerVariable> Samplers { get { return samplers; } }
        public IReadOnlyList<ShaderObjectVariable> Srvs { get { return srvs; } }
        public IReadOnlyList<ShaderObjectVariable> Uavs { get { return uavs; } }
        public IReadOnlyList<ShaderIOVariable> Input { get { return input; } }
        public IReadOnlyList<ShaderIOVariable> InputExtra { get { return inputExtra; } }
        public IReadOnlyList<ShaderIOVariable> InputPatch { get { return inputPatch; } }
        public IReadOnlyList<ShaderIOVariable> InputPatchExtra { get { return inputPatchExtra; } }
        public IReadOnlyList<ShaderIOVariable> Output { get { return output; } }
        public IReadOnlyList<ShaderIOVariable> OutputPatch { get { return outputPatch; } }
        public IReadOnlyList<IReadOnlyList<ShaderIOVariable>> OutputStreams { get { return outputStreams; } }
        public IReadOnlyList<ShaderFixedSamplingPair> FixedSamplingPairs { get { return fixedSamplingPairs; } } 
        public IReadOnlyList<string> CodeGlobalLines { get { return codeGlobalLines; } }
        public IReadOnlyList<string> CodeMainLines { get { return codeMainLines; } }
        public IReadOnlyList<string> CodePatchLines { get { return codePatchLines; } }

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
