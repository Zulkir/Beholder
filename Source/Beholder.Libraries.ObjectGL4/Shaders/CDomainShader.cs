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
using System.Text;
using Beholder.Shaders;
using Beholder.Shaders.Reflection;
using Beholder.Utility.Collections;
using Beholder.Utility.ForImplementations;
using Beholder.Utility.Structures;
using ObjectGL.GL42;

namespace Beholder.Libraries.ObjectGL4.Shaders
{
    class CDomainShader : CShader, IDomainShader, IDisposableInternal
    {
        readonly TightConcurrentDictionary<TesselationLayout, TesselationEvaluationShader> glShadersToGeometry;
        readonly TightConcurrentDictionary<TesselationLayout, TesselationEvaluationShader> glShadersToPixel;
        readonly TesselationDomain domain;

        public override ShaderStage Stage { get { return ShaderStage.Domain; } }
        public TesselationDomain Domain { get { return domain; } }

        public CDomainShader(ICDevice device, CShaderReflection reflection) : base(device, reflection)
        {
            glShadersToGeometry = new TightConcurrentDictionary<TesselationLayout, TesselationEvaluationShader>(TesselationLayout.Equal);
            glShadersToPixel = new TightConcurrentDictionary<TesselationLayout, TesselationEvaluationShader>(TesselationLayout.Equal);
            domain = Reflection.GetTesselationDomain();
        }

        public void DisposeInternal()
        {
            foreach (var kvp in glShadersToGeometry)
                kvp.Value.Dispose();
            foreach (var kvp in glShadersToPixel)
                kvp.Value.Dispose();
        }

        public TesselationEvaluationShader GetGLShaderToGeometry(TesselationLayout layout)
        {
            return glShadersToGeometry.GetOrAdd(layout, this, (l, c) => c.CreateNative(l, ShaderStage.Geometry));
        }

        public TesselationEvaluationShader GetGLShaderToPixel(TesselationLayout layout)
        {
            return glShadersToGeometry.GetOrAdd(layout, this, (l, c) => c.CreateNative(l, ShaderStage.Pixel));
        }

        TesselationEvaluationShader CreateNative(TesselationLayout layout, ShaderStage outputStage)
        {
            var text = GenerateText<CDomainShader, Pair<TesselationLayout, ShaderStage>>(new Pair<TesselationLayout, ShaderStage>(layout, outputStage), WriteLayout, WriteIOAndCode);
            TesselationEvaluationShader glShader;
            string errors;
            if (!TesselationEvaluationShader.TryCompile(text, out glShader, out errors))
                throw new Exception("Failed to compile a shader.\r\n\r\nShader Text:\r\n\r\n" + text + "\r\n\r\nErrors:\r\n\r\n" + errors);
            return glShader;
        }

        static void WriteLayout(StringBuilder builder, CDomainShader shader, Pair<TesselationLayout, ShaderStage> layoutAndOutputStage)
        {
            var layout = layoutAndOutputStage.First;

            TesselationDomain hullDomain;
            TesselationPartitioning partitioning;
            TesselationTopology topology;
            layout.Decode(out hullDomain, out partitioning, out topology);
            
            if (hullDomain != shader.Domain)
                throw new InvalidOperationException("Hull and Domain shaders have cannot have different domains");
            builder.AppendLine(string.Format("layout ( {0}, {1}, {2} ) in;",
                TesselationDomainToString(hullDomain), TesselationPartitioningToString(partitioning), TesselationTopologyToString(topology)));
        }

        static void WriteIOAndCode(StringBuilder builder, CDomainShader shader, Pair<TesselationLayout, ShaderStage> layoutAndOutputStage)
        {
            var outputStage = layoutAndOutputStage.Second;

            var reflection = shader.Reflection;
            WriteCodeLines(builder, reflection.CodeGlobalLines);
            builder.AppendLine();
            WritePatchBlock(builder, reflection.InputPatch, "INPUT_PATCH", "bs_patch_", "patch in");
            WriteInputArrayBlock(builder, reflection.Input, OutputPrefixForStage(ShaderStage.Domain));
            WriteInputExtraBlock(builder, reflection.InputExtra, "INPUT_EXTRA", "InputExtra", "bs_input_extra_");
            WriteSimpleIOBlock(builder, reflection.Output, "OUTPUT", "out", OutputPrefixForStage(outputStage));
            WriteFunction(builder, "main", null, reflection.CodeMainLines, outputStage == ShaderStage.Pixel ? PositionAdjustment : null);
        }

        static string TesselationDomainToString(TesselationDomain domain)
        {
            switch (domain)
            {
                case TesselationDomain.Isoline: return "isolines";
                case TesselationDomain.Triangle: return "triangles";
                case TesselationDomain.Quad: return "quads";
                default: throw new ArgumentOutOfRangeException("domain");
            }
        }

        static string TesselationPartitioningToString(TesselationPartitioning partitioning)
        {
            switch (partitioning)
            {
                case TesselationPartitioning.Integer: return "equal_spacing";
                case TesselationPartitioning.FractionalEven: return "fractional_even_spacing";
                case TesselationPartitioning.FractionalOdd: return "fractional_odd_spacing";
                default: throw new ArgumentOutOfRangeException("partitioning");
            }
        }

        static string TesselationTopologyToString(TesselationTopology topology)
        {
            switch (topology)
            {
                case TesselationTopology.Line: return "cw";
                case TesselationTopology.TriangleCW: return "cw";
                case TesselationTopology.TriangleCCW: return "ccw";
                default: throw new ArgumentOutOfRangeException("topology");
            }
        }
    }
}
