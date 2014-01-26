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
using System.IO;
using System.Linq;
using System.Text;
using Beholder.Shaders;
using Beholder.Shaders.Reflection;
using Beholder.Utility.ForImplementations;
using Beholder.Utility.ForImplementations.Shaders;
using Beholder.Utility.Helpers;

namespace Beholder.Libraries.SharpDX9.Shaders
{
    abstract class CShader : DeviceChildBase<ICDevice>, IShader
    {
        readonly CShaderReflection reflection;
        readonly string name;

        readonly SlotTranslation<CShaderUniformBufferVariable> uniformBufferRegistersC; 
        readonly SlotTranslation<CShaderUniformBufferVariable> uniformBufferRegistersI; 
        readonly SlotTranslation<CShaderUniformBufferVariable> uniformBufferRegistersB;
        readonly CShaderObjectVariable[] textures;
        readonly Dictionary<int, int[]> samplerMap;

        public abstract ShaderStage Stage { get; }
        public CShaderReflection Reflection { get { return reflection; } }
        IShaderReflection IShader.Reflection { get { return reflection; } }
        public string Name { get { return name; } }

        public SlotTranslation<CShaderUniformBufferVariable> UniformBufferRegistersC { get { return uniformBufferRegistersC; } }
        public SlotTranslation<CShaderUniformBufferVariable> UniformBufferRegistersI { get { return uniformBufferRegistersI; } }
        public SlotTranslation<CShaderUniformBufferVariable> UniformBufferRegistersB { get { return uniformBufferRegistersB; } }
        public CShaderObjectVariable[] Textures { get { return textures; } }
        public Dictionary<int, int[]> SamplerMap { get { return samplerMap; } } 

        protected CShader(ICDevice device, CShaderReflection reflection)
            : base(device)
        {
            this.reflection = reflection;
            name = reflection.Name;

            var uniformBufferVariablesC = reflection.UniformBuffers.Where(v => v.RegisterType == ShaderRegisterType.C);
            CheckRegisterOverlap(uniformBufferVariablesC, 'C');
            uniformBufferRegistersC = new SlotTranslation<CShaderUniformBufferVariable>(uniformBufferVariablesC,
                h => h.BSlot, h => h.Register, "Uniform buffers", "D3D9 uniform buffer register");

            var uniformBufferVariablesI = reflection.UniformBuffers.Where(v => v.RegisterType == ShaderRegisterType.I);
            CheckRegisterOverlap(uniformBufferVariablesI, 'I');
            uniformBufferRegistersI = new SlotTranslation<CShaderUniformBufferVariable>(uniformBufferVariablesI,
                h => h.BSlot, h => h.Register, "Uniform buffers", "D3D9 uniform buffer register");

            var uniformBufferVariablesB = reflection.UniformBuffers.Where(v => v.RegisterType == ShaderRegisterType.B);
            CheckRegisterOverlap(uniformBufferVariablesB, 'B');
            uniformBufferRegistersB = new SlotTranslation<CShaderUniformBufferVariable>(uniformBufferVariablesB,
                h => h.BSlot, h => h.Register, "Uniform buffers", "D3D9 uniform buffer register");

            textures = reflection.Srvs;
            samplerMap = reflection.FixedSamplingPairs.GroupBy(p => p.BSamplerSlot).ToDictionary(
                g => g.Key,
                g => g.Select(p => p.Srv.Slot).ToArray());
        }
        
        static void CheckRegisterOverlap(IEnumerable<CShaderUniformBufferVariable> uniformBufferVariables, char registerLetter)
        {
            if (!uniformBufferVariables.Any())
                return;
            var orederedVariables = uniformBufferVariables.OrderBy(v => v.Register);
            var prev = orederedVariables.First();
            foreach (var variable in orederedVariables.Skip(1))
                if (variable.Register < prev.Register + prev.Size / 16)
                    throw new InvalidDataException(string.Format("Uniform buffers '{0}' and '{1}' have overlapping '{2}' D3D9 registers", prev.Name, variable.Name, registerLetter));
        }

        protected string GenerateText()
        {
            var builder = new StringBuilder();

            builder.AppendLine(@"
#define D3D
#define D3D9

#define BS_TEXTURE_DIMENSION(T) BS_TEXTURE_DIMENSION_##T
#define sample(T, C) BS_TEXTURE_SAMPLE(BS_TEXTURE_DIMENSION(T), T, C)
#define BS_TEXTURE_SAMPLE(D, T, C) tex##D(T, C) 
#define sampleBias(T, C, B) BS_TEXTURE_SAMPLE_BIAS(BS_TEXTURE_DIMENSION(T), T, C, B)
#define BS_TEXTURE_SAMPLE_BIAS(D, T, C, B) BS_TEXTURE_SAMPLE_BIAS_##D(T, C, B)
#define BS_TEXTURE_SAMPLE_BIAS_1D(T, C, B) tex1Dbias(T, float4(C, 0.0, 0.0, B))
#define BS_TEXTURE_SAMPLE_BIAS_2D(T, C, B) tex2Dbias(T, float4(C, 0.0, B))
#define BS_TEXTURE_SAMPLE_BIAS_3D(T, C, B) tex3Dbias(T, float4(C, B))
#define BS_TEXTURE_SAMPLE_BIAS_CUBE(T, C, B) texCUBEbias(T, float4(C, B))
#define sampleGrad(T, C, X, Y) BS_TEXTURE_SAMPLE_GRAD(BS_TEXTURE_DIMENSION(T), T, C, X, Y)
#define BS_TEXTURE_SAMPLE_GRAD(D, T, C, X, Y) BS_TEXTURE_SAMPLE_GRAD_##D(T, C, X, Y)
#define BS_TEXTURE_SAMPLE_GRAD_1D(T, C, X, Y) tex1Dgrad(T, C, X, Y)
#define BS_TEXTURE_SAMPLE_GRAD_2D(T, C, X, Y) tex2Dgrad(T, C, X, Y)
#define BS_TEXTURE_SAMPLE_GRAD_3D(T, C, X, Y) tex3Dgrad(T, C, X, Y)
#define BS_TEXTURE_SAMPLE_GRAD_CUBE(T, C, X, Y) texCUBEgrad(T, C, X, Y)
#define sampleLod(T, C, L) BS_TEXTURE_SAMPLE_LOD(BS_TEXTURE_DIMENSION(T), T, C, L)
#define BS_TEXTURE_SAMPLE_LOD(D, T, C, L) BS_TEXTURE_SAMPLE_LOD_##D(T, C, L)
#define BS_TEXTURE_SAMPLE_LOD_1D(T, C, L) tex1Dlod(T, float4(C, 0.0, 0.0, L))
#define BS_TEXTURE_SAMPLE_LOD_2D(T, C, L) tex2Dlod(T, float4(C, 0.0, L))
#define BS_TEXTURE_SAMPLE_LOD_3D(T, C, L) tex3Dlod(T, float4(C, L))
#define BS_TEXTURE_SAMPLE_LOD_CUBE(T, C, L) texCUBElod(T, float4(C, L))

#define INPUT(X) bs_input.##X
#define OUTPUT(X) bs_output.##X
");

            WriteStructures(builder);
            WriteUniformBuffers(builder);
            WriteTextures(builder);
            WriteIO(builder, "BS_Input", Reflection.Input);
            WriteIO(builder, "BS_Output", Reflection.Output);
            WriteCodeGlobal(builder);
            WriteCodeMain(builder);

            return builder.ToString();
        }

        void WriteStructures(StringBuilder builder)
        {
            if (Reflection.Structures == null)
                return;
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
                foreach (var variable in buffer.Elements)
                {
                    if (variable.RelativeOffset % 16 != 0)
                        throw new ArgumentException("Uniform variables that are not 16-byte aligned are not supported by D3D9");
                    builder.AppendLine(string.Format("\t{0} : register({1}{2});", ShaderHelper.ValueVariableDeclaration(variable.Type, variable.Name), buffer.RegisterType.ToStringFast(), buffer.Register + variable.RelativeOffset / 16));
                }
                builder.AppendLine();
            }
        }

        void WriteTextures(StringBuilder builder)
        {
            foreach (var variable in Reflection.Srvs)
            {
                var postfix = GetTextureTypePostfix(variable.Type);
                builder.AppendLine(string.Format("sampler{0} {1} : register(s{2});", postfix, variable.Name, variable.Slot));
                builder.AppendLine(string.Format("#define BS_TEXTURE_DIMENSION_{0} {1}", variable.Name, postfix));
            }
            
            builder.AppendLine();
        }

        static string GetTextureTypePostfix(ShaderObjectType type)
        {
            if (type.ElementType.Equals(ShaderValueVectorType.Float4))
            {
                switch (type.Class)
                {
                    case ShaderObjectTypeClass.Texture1D: return "1D";
                    case ShaderObjectTypeClass.Texture2D: return "2D";
                    case ShaderObjectTypeClass.Texture3D: return "3D";
                    case ShaderObjectTypeClass.TextureCube: return "CUBE";
                }
            }

            throw new NotSupportedException(string.Format("'{0}<{1}> SRV type is not supported by D3D9 implementation of Beholder",
                        ShaderHelper.ObjectTypeClassToString(type.Class), ShaderHelper.ValueTypeToString(type.ElementType)));
        }

        static void WriteIO(StringBuilder builder, string structureName, IEnumerable<CShaderIOVariable> variables)
        {
            builder.AppendLine(string.Format("struct {0}", structureName));
            builder.AppendLine("{");
            foreach (var variable in variables.Where(v => v.IsUsed))
                builder.AppendLine(string.Format("\t{0} : {1};", ShaderHelper.ValueVariableDeclaration(variable.Type, variable.Name), variable.SemanticString));
            builder.AppendLine("};");
            builder.AppendLine();
        }

        void WriteCodeGlobal(StringBuilder builder)
        {
            foreach (var line in Reflection.CodeGlobalLines)
                builder.AppendLine(line);
            builder.AppendLine();
        }

        void WriteCodeMain(StringBuilder builder)
        {
            builder.AppendLine("BS_Output main(BS_Input bs_input)");
            builder.AppendLine("{");
            builder.AppendLine("\tBS_Output bs_output;");
            foreach (var line in Reflection.CodeMainLines)
                builder.AppendLine(line);
            builder.AppendLine("\treturn bs_output;");
            builder.AppendLine("}");
            builder.AppendLine();
        }
    }
}
