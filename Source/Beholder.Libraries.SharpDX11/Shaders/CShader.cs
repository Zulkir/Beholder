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
using System.Globalization;
using System.Text;
using Beholder.Shaders;
using Beholder.Shaders.Reflection;
using Beholder.Utility.ForImplementations;
using Beholder.Utility.Helpers;
using System.Linq;

namespace Beholder.Libraries.SharpDX11.Shaders
{
    abstract class CShader : DeviceChildBase<ICDevice>, IShader
    {
        readonly CShaderReflection reflection;
        readonly string name;

        public CShaderReflection Reflection { get { return reflection; } }
        IShaderReflection IShader.Reflection { get { return reflection; } }
        public string Name { get { return name; } }
        public abstract ShaderStage Stage { get; }

        protected CShader(ICDevice device, CShaderReflection reflection)
            : base(device)
        {
            this.reflection = reflection;
            name = reflection.Name;
        }

        protected string GenerateText<T>(Action<StringBuilder, T> writeIOAndCode)
            where T : CShader
        {
            var builder = new StringBuilder();

            builder.AppendLine(@"
#define D3D
#define D3D10
#define D3D11

#define BS_SAMPLER(T) BS_SAMPLER_FOR_##T
#define calculateLevelOfDetail(T, C) T.CalculateLevelOfDetail(BS_SAMPLER(T), C)
#define gather(T, C) T.Gather(BS_SAMPLER(T), C)
#define gatherOffset(T, C, O) T.Gather(BS_SAMPLER(T), C, O)
#define gatherCmp(T, C, V) T.GatherCmpRed(BS_SAMPLER(T), C, V, 0)
#define gatherCmpOffset(T, C, V, O) T.GatherCmpRed(BS_SAMPLER(T), C, V, O)
#define load(T, C) T.Load(C)
#define loadOffset(T, C, O) T.Load(C, O)
#define loadSample(T, C, S) T.Load(C, 0, S)
#define sample(T, C) T.Sample(BS_SAMPLER(T), C)
#define sampleOffset(T, C, O) T.Sample(BS_SAMPLER(T), C, O)
#define sampleBias(T, C, B) T.SampleBias(BS_SAMPLER(T), C, B)
#define sampleBiasOffset(T, C, B, O) T.SampleBias(BS_SAMPLER(T), C, B, O)
#define sampleCmp(T, C, V) T.SampleCmp(BS_SAMPLER(T), C, V)
#define sampleCmpOffset(T, C, V, O) T.SampleCmp(BS_SAMPLER(T), C, V, O)
#define sampleCmpLevelZero(T, C, V) T.SampleCmpLevelZero(BS_SAMPLER(T), C, V)
#define sampleCmpLevelZeroOffset(T, C, V, O) T.SampleCmpLevelZero(BS_SAMPLER(T), C, V, O)
#define sampleGrad(T, C, X, Y) T.SampleGrad(BS_SAMPLER(T), C, X, Y)
#define sampleGradOffset(T, C, X, Y, O) T.SampleGrad(BS_SAMPLER(T), C, X, Y, O)
#define sampleLevel(T, C, L) T.Sample(BS_SAMPLER(T), C, L)
#define sampleLevelOffset(T, C, L, O) T.Sample(BS_SAMPLER(T), C, L, O)
");

            WriteStructures(builder);
            WriteUniformBuffers(builder);
            WriteSamplers(builder);
            WriteSrvsAndUavs(builder);
            WriteFixedSampling(builder);
            writeIOAndCode(builder, (T)this);

            return builder.ToString();
        }

        protected static void WriteCodeLines(StringBuilder builder, IEnumerable<string> lines)
        {
            foreach (var line in lines)
                builder.AppendLine(line);
        }

        void WriteStructures(StringBuilder builder)
        {
            foreach (var structure in Reflection.Structures)
            {
                builder.AppendLine(string.Format("struct {0}", structure.Name));
                builder.AppendLine("{");
                foreach (var element in structure.Elements)
                    builder.AppendLine(string.Format("\t{0};", ShaderHelper.ValueVariableDeclaration(element.Type, element.Name)));
                builder.AppendLine("};");
                builder.AppendLine();
            }
        }

        void WriteUniformBuffers(StringBuilder builder)
        {
            foreach (var buffer in Reflection.UniformBuffers)
            {
                builder.AppendLine(string.Format("cbuffer {0} : register(b{1})", buffer.Name, buffer.GetSlot()));
                builder.AppendLine("{");
                foreach (var element in buffer.Elements)
                    WriteStructureElementWithPackOffset(builder, element);
                builder.AppendLine("};");
                builder.AppendLine();
            }
        }

        static void WriteStructureElementWithPackOffset(StringBuilder builder, ShaderValueStructureElement element)
        {
            builder.AppendLine(string.Format("\t{0} : packoffset({1});", ShaderHelper.ValueVariableDeclaration(element.Type, element.Name), GetPackOffset(element.RelativeOffset)));
        }

        static string GetPackOffset(int byteOffset)
        {
            int register = byteOffset / 16;
            char letter;
            switch (byteOffset % 16)
            {
                case 0: letter = 'x'; break;
                case 4: letter = 'y'; break;
                case 8: letter = 'z'; break;
                case 12: letter = 'w'; break;
                default: throw new ArgumentException(string.Format("There is no pack offset for a byte offset '{0}'", byteOffset));
            }
            return string.Format("c{0}.{1}", register.ToString(CultureInfo.InvariantCulture), letter.ToString(CultureInfo.InvariantCulture));
        }

        void WriteSamplers(StringBuilder builder)
        {
            foreach (var sampler in Reflection.Samplers)
                builder.AppendLine(string.Format("sampler {0} : register(s{1});", sampler.Name, sampler.GetSlot()));
            builder.AppendLine();
        }

        void WriteSrvsAndUavs(StringBuilder builder)
        {
            foreach (var srv in Reflection.Srvs)
                WriteObjectVariableDeclaration(builder, srv, 't');
            builder.AppendLine();
            foreach (var uav in Reflection.Uavs)
                WriteObjectVariableDeclaration(builder, uav, 'u');
            builder.AppendLine();
        }

        static void WriteObjectVariableDeclaration(StringBuilder builder, ShaderObjectVariable variable, char registerLetter)
        {
            builder.AppendLine(string.Format("{0} <{1}> {2} : register({3}{4});",
                    ShaderHelper.ObjectTypeClassToString(variable.Type.Class),
                    ShaderHelper.ValueTypeToString(variable.Type.ElementType),
                    variable.Name,
                    registerLetter,
                    variable.GetSlot()));
        }

        void WriteFixedSampling(StringBuilder builder)
        {
            foreach (var pair in Reflection.FixedSamplingPairs)
                builder.AppendLine(string.Format("#define BS_SAMPLER_FOR_{0} {1}", pair.SrvName, pair.SamplerName));
            builder.AppendLine();
        }

        protected static void WriteIOStructure(StringBuilder builder, IEnumerable<CShaderIOVariable> variables, string structureName)
        {
            builder.AppendLine(string.Format("struct {0}", structureName));
            builder.AppendLine("{");
            foreach (var element in variables.Where(v => v.IsUsed))
                builder.AppendLine(string.Format("\t{0} : {1};", ShaderHelper.ValueVariableDeclaration(element.Type, element.Name), element.Semantic));
            builder.AppendLine("};");
            builder.AppendLine();
        }
    }
}
