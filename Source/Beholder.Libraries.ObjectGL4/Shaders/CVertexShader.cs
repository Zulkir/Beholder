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
using System.Linq;
using System.Text;
using Beholder.Shaders;
using Beholder.Utility.ForImplementations;
using ObjectGL.GL42;

namespace Beholder.Libraries.ObjectGL4.Shaders
{
    class CVertexShader : CShader, IVertexShader, IDisposableInternal
    {
        VertexShader glShaderToHull;
        VertexShader glShaderToGeometry;
        VertexShader glShaderToPixel;

        private readonly string[] attributeNames;

        public override ShaderStage Stage { get { return ShaderStage.Vertex; } }
        public string[] AttributeNames { get { return attributeNames; } }

        public CVertexShader(ICDevice device, CShaderReflection reflection)
            : base(device, reflection)
        {
            attributeNames = Reflection.Input.Where(v => v.IsUsed && !v.IsSystem).Select(v => OutputPrefixForStage(ShaderStage.Vertex) + v.Semantic).ToArray();
        }

        public void DisposeInternal()
        {
            if (glShaderToHull != null) glShaderToHull.Dispose();
            if (glShaderToGeometry != null) glShaderToGeometry.Dispose();
            if (glShaderToPixel != null) glShaderToPixel.Dispose();
        }

        public VertexShader GetGLShaderToHull() { return glShaderToHull ?? (glShaderToHull = CreateNative(ShaderStage.Hull)); }
        public VertexShader GetGLShaderToGeometry() { return glShaderToGeometry ?? (glShaderToGeometry = CreateNative(ShaderStage.Geometry)); }
        public VertexShader GetGLShaderToPixel() { return glShaderToPixel ?? (glShaderToPixel = CreateNative(ShaderStage.Pixel)); }

        VertexShader CreateNative(ShaderStage outputStage)
        {
            var text = GenerateText<CVertexShader, ShaderStage>(outputStage, WriteLayout, WriteIOAndCode);
            VertexShader glShader;
            string errors;
            if (!VertexShader.TryCompile(text, out glShader, out errors))
                throw new Exception("Failed to compile a shader.\r\n\r\nShader Text:\r\n\r\n" + text + "\r\n\r\nErrors:\r\n\r\n" + errors);
            return glShader;
        }

        static void WriteLayout(StringBuilder builder, CVertexShader shader, ShaderStage outputStage)
        {
        }

        static void WriteIOAndCode(StringBuilder builder, CVertexShader shader, ShaderStage outputStage)
        {
            var reflection = shader.Reflection;

            WriteCodeLines(builder, reflection.CodeGlobalLines);
            builder.AppendLine();

            WriteSimpleIOBlock(builder, reflection.Input, "INPUT", "in", OutputPrefixForStage(ShaderStage.Vertex));
            WriteSimpleIOBlock(builder, reflection.Output, "OUTPUT", "out", OutputPrefixForStage(outputStage));
            WriteFunction(builder, "main", null, reflection.CodeMainLines, outputStage == ShaderStage.Pixel ? PositionAdjustment : null);
        }
    }
}
