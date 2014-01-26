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
using System.Globalization;
using System.IO;
using System.Text;
using Beholder.Shaders;
using Beholder.Shaders.Reflection;
using Beholder.Utility.ForImplementations;
using SharpDX.D3DCompiler;
using SharpDX.Direct3D11;
using System.Linq;

namespace Beholder.Libraries.SharpDX11.Shaders
{
    sealed class CGeometryShader : CShader, IGeometryShader, IDisposableInternal
    {
        public override ShaderStage Stage { get { return ShaderStage.Geometry; } }
        public GeometryShader D3DGeometryShader { get; private set; }
        public GeometryShaderProfile Profile { get; private set; }
        public int MaxVertexCount { get; private set; }
        public GeometryShaderInputPrimitiveType InputPrimitiveType { get; private set; }
        public GeometryShaderOutputPrimitiveType OutputPrimitiveType { get; private set; }

        public CGeometryShader(ICDevice device, CShaderReflection reflection) 
            : base(device, reflection)
        {
            Profile = ParseProfile(reflection.Profile);
            MaxVertexCount = reflection.GetMaxVertexCount();
            InputPrimitiveType = reflection.GetGeometryInputPrimitiveType();
            OutputPrimitiveType = reflection.GetGeometryOutputPrimitiveType();

            var text = GenerateText<CGeometryShader>(WriteIOAndCode);
            CompilationResult bytecode;
            try
            {
                bytecode = ShaderBytecode.Compile(text, "main", ProfileToString(Profile),
                    ShaderFlags.PackMatrixColumnMajor | ShaderFlags.OptimizationLevel3, EffectFlags.None, Name);
            }
            catch (Exception e)
            {
                throw new ArgumentException(string.Format("Failed to compile a geometry shader '{0}'\r\n--- Code ---\r\n{1}\r\n--- Errors ---\r\n{2}", Name, text, e.Message), e);
            }
            D3DGeometryShader = new GeometryShader(device.D3DDevice, bytecode);
        }

        public void DisposeInternal()
        {
            D3DGeometryShader.Dispose();
        }

        static void WriteIOAndCode(StringBuilder builder, CGeometryShader shader)
        {
            var reflection = shader.Reflection;
            WriteCodeLines(builder, reflection.CodeGlobalLines);
            builder.AppendLine();

            builder.AppendLine("#define INPUT(I, N) bs_input[##I].##N");
            WriteIOStructure(builder, reflection.Input, "BS_Input");

            var hasInputExtra = reflection.InputExtra.Any(v => v.IsUsed);
            if (hasInputExtra)
            {
                builder.AppendLine("#define INPUT(X) bs_input_extra.##X");
                WriteIOStructure(builder, reflection.InputExtra, "BS_InputExtra");
            }

            var singleStream = !reflection.OutputStreams.Any();
            int numStreams = singleStream ? 1 : reflection.OutputStreams.Length;
            if (singleStream)
            {
                builder.AppendLine("#define OUTPUT(X) bs_output0.##X");
                
                WriteIOStructure(builder, reflection.Output, "BS_Output0");
                builder.AppendLine("#define EMIT bs_output_stream0.Append(bs_output0);");
                builder.AppendLine("#define RESTART bs_output_stream0.RestartStrip();");
                builder.AppendLine();
            }
            else
            {
                builder.AppendLine("#define OUTPUT(S, N) bs_output##S.##N");
                for (int i = 0; i < reflection.OutputStreams.Length; i++)
                    WriteIOStructure(builder, reflection.OutputStreams[i], "BS_Output" + i.ToString(CultureInfo.InvariantCulture));
                builder.AppendLine("#define EMIT(X) bs_output_stream##X.Append(bs_output##X);");
                builder.AppendLine("#define RESTART(X) bs_output_stream##X.RestartStrip();");
                builder.AppendLine();
            }

            builder.AppendLine(string.Format("[maxvertexcount({0})]", shader.MaxVertexCount));
            builder.AppendLine(string.Format("void main({0} BS_Input bs_input[{1}]", 
                GeometryShaderInputPrimitiveTypeToString(shader.InputPrimitiveType), GetInputVertexCount(shader.InputPrimitiveType)));
            if (hasInputExtra)
                builder.AppendLine("\t, BS_InputExtra bs_input_extra");
            var streamType = GetStreamTypeForOutputType(shader.OutputPrimitiveType);
            for (int i = 0; i < numStreams; i++)
                builder.AppendLine(string.Format("\t, inout {0}<BS_Output{1}> bs_output_stream{1}", streamType, i.ToString(CultureInfo.InvariantCulture)));
            builder.AppendLine("\t)");
            builder.AppendLine("{");
            for (int i = 0; i < numStreams; i++)
                builder.AppendLine(string.Format("\t BS_Output{0} bs_output{0};", i.ToString(CultureInfo.InvariantCulture)));
            WriteCodeLines(builder, reflection.CodeMainLines);
            builder.AppendLine("}");
        }

        public static GeometryShaderProfile ParseProfile(string profileString)
        {
            switch (profileString)
            {
                case "gs_4_0": return GeometryShaderProfile.gs_4_0;
                case "gs_4_1": return GeometryShaderProfile.gs_4_1;
                case "gs_5_0": return GeometryShaderProfile.gs_5_0;
                default: throw new InvalidDataException(string.Format("'{0}' is not a valid geometry shader profile.", profileString));
            }
        }

        public static string ProfileToString(GeometryShaderProfile profile)
        {
            switch (profile)
            {
                case GeometryShaderProfile.gs_4_0: return "gs_4_0";
                case GeometryShaderProfile.gs_4_1: return "gs_4_1";
                case GeometryShaderProfile.gs_5_0: return "gs_5_0";
                default: throw new ArgumentOutOfRangeException("profile");
            }
        }

        public static string GeometryShaderInputPrimitiveTypeToString(GeometryShaderInputPrimitiveType type)
        {
            switch (type)
            {
                case GeometryShaderInputPrimitiveType.Point: return "point";
                case GeometryShaderInputPrimitiveType.Line: return "line";
                case GeometryShaderInputPrimitiveType.Triangle: return "triangle";
                case GeometryShaderInputPrimitiveType.LineWithAdjacency: return "lineadj";
                case GeometryShaderInputPrimitiveType.TriangleWithAdjacency: return "triangleadj";
                default: throw new ArgumentOutOfRangeException("type");
            }
        }

        static int GetInputVertexCount(GeometryShaderInputPrimitiveType primitiveType)
        {
            switch (primitiveType)
            {
                case GeometryShaderInputPrimitiveType.Point: return 1;
                case GeometryShaderInputPrimitiveType.Line: return 2;
                case GeometryShaderInputPrimitiveType.Triangle: return 3;
                case GeometryShaderInputPrimitiveType.LineWithAdjacency: return 4;
                case GeometryShaderInputPrimitiveType.TriangleWithAdjacency: return 6;
                default: throw new ArgumentOutOfRangeException("primitiveType");
            }
        }

        public static string GetStreamTypeForOutputType(GeometryShaderOutputPrimitiveType type)
        {
            switch (type)
            {
                case GeometryShaderOutputPrimitiveType.Point: return "PointStream";
                case GeometryShaderOutputPrimitiveType.Line: return "LineStream";
                case GeometryShaderOutputPrimitiveType.Triangle: return "TriangleStream";
                default: throw new ArgumentOutOfRangeException("type");
            }
        }
    }
}
