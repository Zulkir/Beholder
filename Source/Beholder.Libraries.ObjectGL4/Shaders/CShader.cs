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
using System.IO;
using System.Text;
using Beholder.Shaders;
using Beholder.Shaders.Reflection;
using Beholder.Utility.ForImplementations;
using Beholder.Utility.ForImplementations.Shaders;
using Beholder.Utility.Helpers;
using System.Linq;

namespace Beholder.Libraries.ObjectGL4.Shaders
{
    abstract class CShader : DeviceChildBase<ICDevice>, IShader
    {
        readonly CShaderReflection reflection;
        readonly SlotTranslation<CShaderUniformBufferVariable> uniformBufferSlots;
        readonly SlotTranslation<CShaderObjectVariable> textureSlots;
        readonly Dictionary<int, SamplerSettingData[]> samplerMap;

        public abstract ShaderStage Stage { get; }
        public CShaderReflection Reflection { get { return reflection; } }
        IShaderReflection IShader.Reflection { get { return reflection; } }
        
        public SlotTranslation<CShaderUniformBufferVariable> UniformBufferSlots { get { return uniformBufferSlots; } }
        public SlotTranslation<CShaderObjectVariable> TextureSlots { get { return textureSlots; } }
        public Dictionary<int, SamplerSettingData[]> SamplerMap { get { return samplerMap; } }

        protected CShader(ICDevice device, CShaderReflection reflection)
            : base(device)
        {
            this.reflection = reflection;

            uniformBufferSlots = new SlotTranslation<CShaderUniformBufferVariable>(reflection.UniformBuffers, h => h.BSlot, h => h.GLSlot, "Uniform buffers", "OpenGL uniform buffer slot");
            textureSlots = new SlotTranslation<CShaderObjectVariable>(reflection.Srvs, h => h.BSlot, h => h.GLSlot, "SRV variables", "OpenGL sampler slot");
            samplerMap = reflection.FixedSamplingPairs.GroupBy(p => p.BSamplerSlot).ToDictionary(
                g => g.Key,
                g => g.Select(p => new SamplerSettingData(p.Srv.GLSlot, SupportsMips(TextureTypeToString(p.Srv.Type)))).ToArray());
        }

        protected string GenerateText<TShader, TParam>(TParam param, Action<StringBuilder, TShader, TParam> writeLayout, Action<StringBuilder, TShader, TParam> writeIOAndCode)
            where TShader : CShader
        {
            var builder = new StringBuilder();

            builder.AppendLine(string.Format(@"#version {0}", Reflection.GLVersion));

            builder.AppendLine(ShaderMacros.GetText(Stage));

            writeLayout(builder, (TShader)this, param);
            WriteStructures(builder);
            WriteUniformBuffers(builder);
            WriteTextures(builder);
            writeIOAndCode(builder, (TShader)this, param);

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
                    WriteStructureElement(builder, element);
                builder.AppendLine("};");
                builder.AppendLine();
            }
        }

        void WriteUniformBuffers(StringBuilder builder)
        {
            foreach (var buffer in Reflection.UniformBuffers)
            {
                builder.AppendLine(string.Format("layout(std140) uniform {0}", buffer.Name));
                builder.AppendLine("{");
                foreach (var element in buffer.Elements)
                    WriteStructureElement(builder, element);
                builder.AppendLine("};");
                builder.AppendLine();
            }
        }

        static void WriteStructureElement(StringBuilder builder, ShaderValueStructureElement element)
        {
            builder.AppendLine(string.Format("\t{0};", ShaderHelper.ValueVariableDeclaration(element.Type, element.Name)));
        }

        void WriteTextures(StringBuilder builder)
        {
            foreach (var variable in Reflection.Srvs)
            {
                builder.AppendLine(string.Format("uniform {0} {1};", TextureTypeToString(variable.Type), variable.Name));
                
                string dimension;
                builder.AppendLine(TryGetTextureLoadDimension(variable.Type, out dimension)
                    ? string.Format("#define BS_TEXTURE_LOAD_DIMENSION_{0} {1}", variable.Name, dimension)
                    : string.Format("#define BS_TEXTURE_LOAD_DIMENSION_{0} error_loadXXX_functions_are_not_supported_for_{0}_texture", variable.Name));
                builder.AppendLine(TryGetTextureSampleCmpDimension(variable.Type, out dimension)
                    ? string.Format("#define BS_TEXTURE_SAMPLE_CMP_OFFSET_{0} {1}", variable.Name, dimension)
                    : string.Format("#define BS_TEXTURE_SAMPLE_CMP_OFFSET_{0} error_sampleCmpXXX_functions_are_not_supported_for_{0}_texture", variable.Name));
            }
        }

        static string TextureTypeToString(ShaderObjectType type)
        {
            switch (type.Class)
            {
                #region Big Switch
                case ShaderObjectTypeClass.Buffer:
                    if (type.ElementType.Equals(ShaderValueVectorType.Float4))
                        return "samplerBuffer";
                    if (type.ElementType.Equals(ShaderValueVectorType.Int4))
                        return "isamplerBuffer";
                    if (type.ElementType.Equals(ShaderValueVectorType.UInt4))
                        return "usamplerBuffer";
                    break;
                case ShaderObjectTypeClass.Texture1D:
                    if (type.ElementType.Equals(ShaderValueVectorType.Float4))
                        return "sampler1D";
                    if (type.ElementType.Equals(ShaderValueVectorType.Int4))
                        return "isampler1D";
                    if (type.ElementType.Equals(ShaderValueVectorType.UInt4))
                        return "usampler1D";
                    if (type.ElementType.Equals(ShaderValueScalarType.Float))
                        return "sampler1DShadow";
                    break;
                case ShaderObjectTypeClass.Texture1DArray:
                    if (type.ElementType.Equals(ShaderValueVectorType.Float4))
                        return "sampler1DArray";
                    if (type.ElementType.Equals(ShaderValueVectorType.Int4))
                        return "isampler1DArray";
                    if (type.ElementType.Equals(ShaderValueVectorType.UInt4))
                        return "usampler1DArray";
                    if (type.ElementType.Equals(ShaderValueScalarType.Float))
                        return "sampler1DArrayShadow";
                    break;
                case ShaderObjectTypeClass.Texture2D:
                    if (type.ElementType.Equals(ShaderValueVectorType.Float4))
                        return "sampler2D";
                    if (type.ElementType.Equals(ShaderValueVectorType.Int4))
                        return "isampler2D";
                    if (type.ElementType.Equals(ShaderValueVectorType.UInt4))
                        return "usampler2D";
                    if (type.ElementType.Equals(ShaderValueScalarType.Float))
                        return "sampler2DShadow";
                    break;
                case ShaderObjectTypeClass.Texture2DArray:
                    if (type.ElementType.Equals(ShaderValueVectorType.Float4))
                        return "sampler2DArray";
                    if (type.ElementType.Equals(ShaderValueVectorType.Int4))
                        return "isampler2DArray";
                    if (type.ElementType.Equals(ShaderValueVectorType.UInt4))
                        return "usampler2DArray";
                    if (type.ElementType.Equals(ShaderValueScalarType.Float))
                        return "sampler2DArrayShadow";
                    break;
                case ShaderObjectTypeClass.Texture2DMS:
                    if (type.ElementType.Equals(ShaderValueVectorType.Float4))
                        return "sampler2DMS";
                    if (type.ElementType.Equals(ShaderValueVectorType.Int4))
                        return "isampler2DMS";
                    if (type.ElementType.Equals(ShaderValueVectorType.UInt4))
                        return "usampler2DMS";
                    break;
                case ShaderObjectTypeClass.Texture2DMSArray:
                    if (type.ElementType.Equals(ShaderValueVectorType.Float4))
                        return "sampler2DMSArray";
                    if (type.ElementType.Equals(ShaderValueVectorType.Int4))
                        return "isampler2DMSArray";
                    if (type.ElementType.Equals(ShaderValueVectorType.UInt4))
                        return "usampler2DMSArray";
                    break;
                case ShaderObjectTypeClass.TextureCube:
                    if (type.ElementType.Equals(ShaderValueVectorType.Float4))
                        return "samplerCube";
                    if (type.ElementType.Equals(ShaderValueVectorType.Int4))
                        return "isamplerCube";
                    if (type.ElementType.Equals(ShaderValueVectorType.UInt4))
                        return "usamplerCube";
                    if (type.ElementType.Equals(ShaderValueScalarType.Float))
                        return "samplerCubeShadow";
                    break;
                case ShaderObjectTypeClass.TextureCubeArray:
                    if (type.ElementType.Equals(ShaderValueVectorType.Float4))
                        return "samplerCubeArray";
                    if (type.ElementType.Equals(ShaderValueVectorType.Int4))
                        return "isamplerCubeArray";
                    if (type.ElementType.Equals(ShaderValueVectorType.UInt4))
                        return "usamplerCubeArray";
                    if (type.ElementType.Equals(ShaderValueScalarType.Float))
                        return "samplerCubeArrayShadow";
                    break;
                case ShaderObjectTypeClass.Texture3D:
                    if (type.ElementType.Equals(ShaderValueVectorType.Float4))
                        return "sampler3D";
                    if (type.ElementType.Equals(ShaderValueVectorType.Int4))
                        return "isampler3D";
                    if (type.ElementType.Equals(ShaderValueVectorType.UInt4))
                        return "usampler3D";
                    break;/*
                case ShaderObjectTypeClass.RWBuffer:
                case ShaderObjectTypeClass.ByteAddressBuffer:
                case ShaderObjectTypeClass.RWByteAddressBuffer:
                case ShaderObjectTypeClass.AppendStructuredBuffer:
                case ShaderObjectTypeClass.ConsumeStructuredBuffer:
                case ShaderObjectTypeClass.RWStructuredBuffer:
                case ShaderObjectTypeClass.StructuredBuffer:
                case ShaderObjectTypeClass.RWTexture1D:
                case ShaderObjectTypeClass.RWTexture1DArray:
                case ShaderObjectTypeClass.RWTexture2D:
                case ShaderObjectTypeClass.RWTexture2DArray:
                case ShaderObjectTypeClass.RWTexture3D:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();*/
                #endregion
            }

            throw new NotSupportedException(string.Format("'{0}<{1}> SRV type is not supported by OGL implementation of Beholder", 
                        ShaderHelper.ObjectTypeClassToString(type.Class), ShaderHelper.ValueTypeToString(type.ElementType)));
        }

        static bool TryGetTextureLoadDimension(ShaderObjectType type, out string dimension)
        {
            switch (type.Class)
            {
                //case ShaderObjectTypeClass.Buffer:
                case ShaderObjectTypeClass.Texture1D:
                //case ShaderObjectTypeClass.Texture2DMS:
                    dimension = "2";
                    return true;
                case ShaderObjectTypeClass.Texture1DArray:
                case ShaderObjectTypeClass.Texture2D:
                //case ShaderObjectTypeClass.Texture2DMSArray:
                    dimension = "3";
                    return true;
                case ShaderObjectTypeClass.Texture2DArray:
                case ShaderObjectTypeClass.Texture3D:
                    dimension = "4";
                    return true;
            }
            dimension = null;
            return false;
        }

        static bool TryGetTextureSampleCmpDimension(ShaderObjectType type, out string dimension)
        {
            switch (type.Class)
            {
                case ShaderObjectTypeClass.Texture1D:
                    dimension = "1";
                    return true;
                case ShaderObjectTypeClass.Texture1DArray:
                case ShaderObjectTypeClass.Texture2D:
                    dimension = "2";
                    return true;
                case ShaderObjectTypeClass.Texture2DArray:
                //case ShaderObjectTypeClass.TextureCube:
                    dimension = "3";
                    return true;
            }
            dimension = null;
            return false;
        }

        static bool SupportsMips(string glSamplerType)
        {
            switch (glSamplerType)
            {
                case "sampler1D":
                case "sampler2D":
                case "sampler3D":
                case "samplerCube":
                case "sampler1DArray":
                case "sampler2DArray":
                case "samplerCubeArray":
                case "isampler1D":
                case "isampler2D":
                case "isampler3D":
                case "isamplerCube":
                case "isampler1DArray":
                case "isampler2DArray":
                case "isamplerCubeArray":
                case "usampler1D":
                case "usampler2D":
                case "usampler3D":
                case "usamplerCube":
                case "usampler1DArray":
                case "usampler2DArray":
                case "usamplerCubeArray":
                    return true;
                default:
                    return false;
            }
        }

        protected static void WriteFunction(StringBuilder builder, string name, string pre, IEnumerable<string> lines, string post)
        {
            builder.AppendLine(string.Format("void {0}()", name));
            builder.AppendLine("{");
            if (pre != null)
                builder.AppendLine("\t" + pre);
            WriteCodeLines(builder, lines);
            if (post != null)
                builder.AppendLine("\t" + post);
            builder.AppendLine("}");
            builder.AppendLine();
        }

        protected static void WriteCodeLines(StringBuilder builder, IEnumerable<string> lines)
        {
            foreach (var line in lines)
                builder.AppendLine(line);
        }

        protected static void WriteSimpleIOBlock(StringBuilder builder, IEnumerable<CShaderIOVariable> variables, string macroName, string direction, string prefix)
        {
            builder.AppendLine(string.Format("#define {0}(x) {1}##x", macroName, prefix));
            foreach (var variable in variables.Where(v => v.IsUsed))
                if (variable.IsSystem)
                    builder.AppendLine(string.Format("#define {0}{1} {2}", prefix, variable.Name, variable.Semantic));
                else
                    WriteIOVariable(builder, variable, direction, prefix, "");
            builder.AppendLine();
        }

        protected static void WriteInputArrayBlock(StringBuilder builder, IEnumerable<CShaderIOVariable> variables, string inputPrefix)
        {
            builder.AppendLine("#define INPUT(I, N) INPUT_ARRAY_##N(I)");
            foreach (var variable in variables.Where(v => v.IsUsed))
            {
                if (variable.IsSystem)
                {
                    builder.AppendLine(string.Format("#define INPUT_ARRAY_{0}(X) gl_in[##X].{1}", variable.Name, variable.Semantic));
                }
                else
                {
                    builder.AppendLine(string.Format("#define INPUT_ARRAY_{0}(X) {1}{0}[##X]", variable.Name, inputPrefix));
                    WriteIOVariable(builder, variable, "in", inputPrefix, "[]");
                }
                builder.AppendLine();
            }
        }

        protected static void WriteInputExtraBlock(StringBuilder builder, IEnumerable<CShaderIOVariable> variables, string macroName, string blockName, string prefix)
        {
            builder.AppendLine(string.Format("#define {0}(X) {1}##X", macroName, prefix));
            foreach (var variable in variables.Where(v => v.IsUsed))
            {
                if (!variable.IsSystem)
                    throw new InvalidDataException(string.Format("{0} cannot have non gl_*** variables when useing OGL implementation of Behodler", blockName));
                builder.AppendLine(string.Format("#define {0}{1} {2}", prefix, variable.Name, variable.Semantic));
            }
            builder.AppendLine();
        }

        protected static void WriteIOVariable(StringBuilder builder, CShaderIOVariable variable, string direction, string prefix, string postfix)
        {
            builder.AppendLine(string.Format("{0}{1} {2}{3};", InterpolationToString(variable.Interpolation),
                direction, ShaderHelper.ValueVariableDeclaration(variable.Type, prefix + variable.Name), postfix));
        }

        protected static void WritePatchBlock(StringBuilder builder, IEnumerable<CShaderIOVariable> variables, string macroName, string prefix, string direction)
        {
            builder.AppendLine(string.Format("#define {0}(X) {1}##X", macroName, prefix));
            foreach (var variable in variables.Where(v => v.IsUsed))
            {
                if (variable.IsSystem)
                    builder.AppendLine(string.Format("#define {0}{1} {2}", prefix, variable.Name, variable.Semantic));
                else
                    WriteIOVariable(builder, variable, direction, prefix, "");
            }
            builder.AppendLine();
        }

        static string InterpolationToString(ShaderIOVariableInterpolation interpolation)
        {
            return string.Format("{0}{1}{2}{3}",
                interpolation.HasFlag(ShaderIOVariableInterpolation.NoInterpolation) ? "flat " : "",
                interpolation.HasFlag(ShaderIOVariableInterpolation.NoPerspective) ? "noperspective " : "",
                interpolation.HasFlag(ShaderIOVariableInterpolation.Centroid) ? "centroid " : "",
                interpolation.HasFlag(ShaderIOVariableInterpolation.Sample) ? "sample " : ""
                );
        }
    }
}
