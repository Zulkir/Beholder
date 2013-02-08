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
using System.Linq;
using System.Text;
using Beholder.Shaders;
using Beholder.Shaders.Reflection;
using Beholder.Utility.ForImplementations;
using ObjectGL.GL42;

namespace Beholder.Libraries.ObjectGL4.Shaders
{
    class CHullShader : CShader, IHullShader, IDisposableInternal
    {
        TesselationControlShader glShaderFromVertex;

        readonly TesselationDomain domain;
        readonly TesselationPartitioning partitioning;
        readonly TesselationTopology outputTopology;
        readonly int outputControlPoints;
        readonly float maxTesselationFactor;

        public override ShaderStage Stage { get { return ShaderStage.Hull; } }

        public TesselationDomain Domain { get { return domain; } }
        public TesselationPartitioning Partitioning { get { return partitioning; } }
        public TesselationTopology OutputTopology { get { return outputTopology; } }
        public int OutputControlPoints { get { return outputControlPoints; } }
        public float MaxTesselationFactor { get { return maxTesselationFactor; } }
        
        public CHullShader(ICDevice device, CShaderReflection reflection) : base(device, reflection)
        {
            domain = reflection.GetTesselationDomain();
            partitioning = reflection.GetTesselationPartitioning();
            outputTopology = reflection.GetTesselationOutputTopology();
            outputControlPoints = reflection.GetOutputControlPoints();
            maxTesselationFactor = reflection.GetMaxTesselationFactor();
        }

        public void DisposeInternal()
        {
            if (glShaderFromVertex != null) glShaderFromVertex.Dispose();
        }

        public TesselationControlShader GetGLShader() { return glShaderFromVertex ?? (glShaderFromVertex = CreateNative()); }

        internal TesselationLayout EncodeLayout()
        {
            return new TesselationLayout(domain, partitioning, outputTopology);
        }

        TesselationControlShader CreateNative()
        {
            var text = GenerateText<CHullShader, object>(null, WriteLayout, WriteIOAndCode);
            TesselationControlShader glShader;
            string errors;
            if (!TesselationControlShader.TryCompile(text, out glShader, out errors))
                throw new Exception("Failed to compile a shader.\r\n\r\nShader Text:\r\n\r\n" + text + "\r\n\r\nErrors:\r\n\r\n" + errors);
            return glShader;
        }

        static void WriteLayout(StringBuilder builder, CHullShader shader, object param)
        {
            builder.AppendLine(string.Format("layout ( vertices = {0} ) out;", shader.OutputControlPoints.ToString(CultureInfo.InvariantCulture)));
            builder.AppendLine();
        }

        static void WriteIOAndCode(StringBuilder builder, CHullShader shader, object param)
        {
            var reflection = shader.Reflection;

            WriteCodeLines(builder, reflection.CodeGlobalLines);
            builder.AppendLine();
            WriteInputArrayBlock(builder, reflection.Input, "bs_vertex_");
            WriteInputExtraBlock(builder, reflection.InputPatchExtra, "INPUT_PATCH_EXTRA", "InputPatchExtra", "bs_input_patch_extra_");
            WritePatchBlock(builder, reflection.OutputPatch, "OUTPUT_PATCH", "bs_output_patch_", "patch out");
            WriteFunction(builder, "BS_Patch_Function", null, reflection.CodePatchLines, null);
            WriteInputExtraBlock(builder, reflection.InputExtra, "INPUT_EXTRA", "InputExtra", "bs_input_extra_");
            WriteOutputBlock(builder, reflection.Output);
            WriteFunction(builder, "main", "BS_Patch_Function();", reflection.CodeMainLines, null);
        }

        static void WriteOutputBlock(StringBuilder builder, IEnumerable<CShaderIOVariable> variables)
        {
            if (variables == null)
                return;
            builder.AppendLine("#define OUTPUT(X) OUTPUT_ARRAY_##X");
            foreach (var variable in variables.Where(v => v.IsUsed))
            {
                if (variable.IsSystem)
                    builder.AppendLine(string.Format("#define OUTPUT_ARRAY_{0} gl_out[gl_InvocationID].{1}", variable.Name, variable.Semantic));
                else
                {
                    builder.AppendLine(string.Format("#define OUTPUT_ARRAY_{0} bs_hull_{0}[gl_InvocationID]", variable.Name));
                    WriteIOVariable(builder, variable, "out", "bs_hull_", "[]");
                }
            }
            builder.AppendLine();
        }
    }
}
