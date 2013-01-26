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
using System.Globalization;
using System.Text;
using Beholder.Shaders;
using Beholder.Shaders.Reflection;
using ObjectGL.GL42;
using System.Linq;

namespace Beholder.Libraries.ObjectGL4.Shaders
{
    class CGeometryShader : CShader, IGeometryShader, IDisposable
    {
        GeometryShader glShaderFromVertex;
        GeometryShader glShaderFromDomain;
        readonly int maxVertexCount;
        readonly GeometryShaderInputPrimitiveType inputPrimitiveType;
        readonly GeometryShaderOutputPrimitiveType outputPrimitiveType;

        public override ShaderStage Stage { get { return ShaderStage.Geometry; } }

        public int MaxVertexCount { get { return maxVertexCount; } }
        public GeometryShaderInputPrimitiveType InputPrimitiveType { get { return inputPrimitiveType; } }
        public GeometryShaderOutputPrimitiveType OutputPrimitiveType { get { return outputPrimitiveType; } }

        public CGeometryShader(ICDevice device, CShaderReflection reflection) : base(device, reflection)
        {
            maxVertexCount = Reflection.GetMaxVertexCount();
            inputPrimitiveType = Reflection.GetGeometryInputPrimitiveType();
            outputPrimitiveType = Reflection.GetGeometryOutputPrimitiveType();
        }

        public void Dispose()
        {
            if (glShaderFromVertex != null) glShaderFromVertex.Dispose();
            if (glShaderFromDomain != null) glShaderFromDomain.Dispose();
        }

        public GeometryShader GetGLShaderFromVertex() { return glShaderFromVertex ?? (glShaderFromVertex = CreateNative("bs_vertex_")); }
        public GeometryShader GetGLShaderFromDomain() { return glShaderFromDomain ?? (glShaderFromDomain = CreateNative("bs_domain_")); }

        GeometryShader CreateNative(string inputPrefix)
        {
            var text = GenerateText<CGeometryShader, string>(inputPrefix, WriteLayout, WriteIOAndCode);
            GeometryShader glShader;
            string errors;
            if (!GeometryShader.TryCompile(text, out glShader, out errors))
                throw new Exception("Failed to compile a shader.\r\n\r\nShader Text:\r\n\r\n" + text + "\r\n\r\nErrors:\r\n\r\n" + errors);
            return glShader;
        }

        static void WriteLayout(StringBuilder builder, CGeometryShader shader, string inputPrefix)
        {
            builder.AppendLine(string.Format("layout ( {0} ) in;",
                GeometryShaderInputPrimitiveTypeToString(shader.InputPrimitiveType)));
            builder.AppendLine(string.Format("layout ( {0}, max_vertices = {1} ) out;",
                GeometryShaderOutputPrimitiveTypeToString(shader.OutputPrimitiveType), shader.MaxVertexCount.ToString(CultureInfo.InvariantCulture)));
            builder.AppendLine();
        }

        static void WriteIOAndCode(StringBuilder builder, CGeometryShader shader, string inputPrefix)
        {
            var reflection = shader.Reflection;
            WriteCodeLines(builder, reflection.CodeGlobalLines);
            builder.AppendLine();

            WriteInputArrayBlock(builder, reflection.Input, inputPrefix);
            WriteInputExtraBlock(builder, reflection.InputExtra, "INPUT_EXTRA", "InputExtra", "bs_input_extra_");

            if (reflection.OutputStreams.Any()) 
                WriteOutputStreamsBlock(builder, reflection.OutputStreams);
            else 
                WriteOutputBlock(builder, reflection.Output);

            WriteFunction(builder, "main", null, reflection.CodeMainLines, null);
        }

        static void WriteOutputBlock(StringBuilder builder, IEnumerable<CShaderIOVariable> variables)
        {
            WriteSimpleIOBlock(builder, variables, "OUTPUT", "out", "bs_geometry_");

            builder.AppendLine("#define EMIT EmitVertex();");
            builder.AppendLine("#define RESTART EndPrimitive();");
            builder.AppendLine();
        }

        static void WriteOutputStreamsBlock(StringBuilder builder, CShaderIOVariable[][] streams)
        {
            builder.AppendLine("#define OUTPUT(X, Y) OUTPUT_ARRAY_##X(Y)");
            for (int i = 0; i < streams.Length; i++)
            {
                var stream = streams[i];
                builder.AppendLine(string.Format("layout ( stream = {0} ) out;", i.ToString(CultureInfo.InvariantCulture)));
                builder.AppendLine();

                var iString = i.ToString(CultureInfo.InvariantCulture);
                builder.AppendLine(string.Format("#define OUTPUT_ARRAY_{0}(Y) bs_geometry_{1}##Y", iString, i == 0 ? "" : iString));
                builder.AppendLine();
                foreach (var variable in stream.Where(v => v.IsUsed))
                {
                    if (variable.IsSystem)
                        builder.AppendLine(string.Format("#define bs_geometry_{0}{1} {2}", i == 0 ? "" : iString, variable.Name, variable.Semantic));
                    else
                        WriteIOVariable(builder, variable, "out", string.Format("bs_geometry_{0}", i == 0 ? "" : iString), "");
                }
                builder.AppendLine();
            }

            builder.AppendLine("#define EMIT(X) EmitStreamVertex(##X);");
            builder.AppendLine("#define RESTART(X) EndStreamPrimitive(##X);");
            builder.AppendLine();
        }

        public static string GeometryShaderInputPrimitiveTypeToString(GeometryShaderInputPrimitiveType type)
        {
            switch (type)
            {
                case GeometryShaderInputPrimitiveType.Point: return "points";
                case GeometryShaderInputPrimitiveType.Line: return "lines";
                case GeometryShaderInputPrimitiveType.Triangle: return "triangles";
                case GeometryShaderInputPrimitiveType.LineWithAdjacency: return "lines_adjacency";
                case GeometryShaderInputPrimitiveType.TriangleWithAdjacency: return "triangles_adjacency";
                default: throw new ArgumentOutOfRangeException("type");
            }
        }

        public static string GeometryShaderOutputPrimitiveTypeToString(GeometryShaderOutputPrimitiveType type)
        {
            switch (type)
            {
                case GeometryShaderOutputPrimitiveType.Point: return "points";
                case GeometryShaderOutputPrimitiveType.Line: return "line_strip";
                case GeometryShaderOutputPrimitiveType.Triangle: return "triangle_strip";
                default: throw new ArgumentOutOfRangeException("type");
            }
        }
    }
}
