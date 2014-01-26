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
using System.Linq;
using System.Text;
using Beholder.Shaders;
using Beholder.Shaders.Reflection;
using Beholder.Utility.ForImplementations;
using SharpDX.D3DCompiler;
using SharpDX.Direct3D11;

namespace Beholder.Libraries.SharpDX11.Shaders
{
    sealed class CHullShader : CShader, IHullShader, IDisposableInternal
    {
        public override ShaderStage Stage { get { return ShaderStage.Hull; } }
        public HullShader D3DHullShader { get; private set; }
        public HullShaderProfile Profile { get; private set; }

        public TesselationDomain Domain { get; private set; }
        public TesselationPartitioning Partitioning { get; private set; }
        public TesselationTopology OutputTopology { get; private set; }
        public int InputControlPoints { get; private set; }
        public int OutputControlPoints { get; private set; }
        public float MaxTesselationFactor { get; private set; }

        public CHullShader(ICDevice device, CShaderReflection reflection) 
            : base(device, reflection)
        {
            Profile = ParseProfile(reflection.Profile);
            Domain = reflection.GetTesselationDomain();
            Partitioning = reflection.GetTesselationPartitioning();
            OutputTopology = reflection.GetTesselationOutputTopology();
            InputControlPoints = reflection.GetInputControlPoints();
            OutputControlPoints = reflection.GetOutputControlPoints();
            MaxTesselationFactor = reflection.GetMaxTesselationFactor();

            var text = GenerateText<CHullShader>(WriteIOAndCode);
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
            D3DHullShader = new HullShader(device.D3DDevice, bytecode);
        }

        public void DisposeInternal()
        {
            D3DHullShader.Dispose();
        }

        static void WriteIOAndCode(StringBuilder builder, CHullShader shader)
        {
            var reflection = shader.Reflection;
            WriteCodeLines(builder, reflection.CodeGlobalLines);
            builder.AppendLine();

            builder.AppendLine("#define INPUT(I, N) bs_input[##I].##N");
            WriteIOStructure(builder, reflection.Input, "BS_Input");

            bool hasInputPatchExtra = reflection.InputPatchExtra.Any(v => v.IsUsed);
            if (hasInputPatchExtra)
            {
                builder.AppendLine("#define INPUT_PATCH_EXTRA(X) bs_input_patch_extra.##X");
                WriteIOStructure(builder, reflection.InputPatchExtra, "BS_InputPatchExtra");
            }

            builder.AppendLine("#define OUTPUT_PATCH(X) bs_output_patch.##X");
            //builder.AppendLine("#define PATCH(N, I) bs_output_patch.##N[##I]");
            WriteIOStructure(builder, reflection.OutputPatch.Select(v =>
            {
                switch (v.Semantic.ToLower())
                {
                    case "sv_tessfactor": return new CShaderIOVariable(new ShaderIOVariable(v.Interpolation, new ShaderValueArrayType(v.Type, GetTessFactorLength(shader.Domain)), v.Name, v.SpecialParameters));
                    case "sv_insidetessfactor": return new CShaderIOVariable(new ShaderIOVariable(v.Interpolation, new ShaderValueArrayType(v.Type, GetInsideTessFactorLength(shader.Domain)), v.Name, v.SpecialParameters));
                    default: return v;
                }
            }), "BS_OutputPatch");

            builder.AppendLine(string.Format("BS_OutputPatch BS_ConstantFunction(InputPatch<BS_Input, {0}> bs_input", shader.InputControlPoints.ToString(CultureInfo.InvariantCulture)));
            builder.AppendLine(hasInputPatchExtra ? ", BS_InputPatchExtra bs_input_patch_extra)" : ")");
            builder.AppendLine("{");
            builder.AppendLine("\tBS_OutputPatch bs_output_patch;");
            WriteCodeLines(builder, reflection.CodePatchLines);
            builder.AppendLine("\treturn bs_output_patch;");
            builder.AppendLine("}");
            builder.AppendLine();

            bool hasInputExtra = reflection.InputExtra.Any(v => v.IsUsed);
            if (hasInputExtra)
            {
                builder.AppendLine("#define INPUT_EXTRA(X) bs_input_extra.##X");
                WriteIOStructure(builder, reflection.InputExtra, "BS_InputExtra");
            }

            builder.AppendLine("#define OUTPUT(X) bs_output.##X");
            WriteIOStructure(builder, reflection.Output, "BS_Output");

            builder.AppendLine(string.Format("[domain(\"{0}\")]", DomainToString(shader.Domain)));
            builder.AppendLine(string.Format("[partitioning(\"{0}\")]", PartitioningToString(shader.Partitioning)));
            builder.AppendLine(string.Format("[outputtopology(\"{0}\")]", TopologyToString(shader.OutputTopology)));
            builder.AppendLine(string.Format("[outputcontrolpoints({0})]", shader.OutputControlPoints.ToString(CultureInfo.InvariantCulture)));
            builder.AppendLine("[patchconstantfunc(\"BS_ConstantFunction\")]");
            builder.AppendLine(string.Format(CultureInfo.InvariantCulture, "[maxtessfactor({0:0.0000}f)]", shader.MaxTesselationFactor));
            builder.AppendLine(string.Format("BS_Output main(InputPatch<BS_Input, {0}> bs_input", shader.InputControlPoints.ToString(CultureInfo.InvariantCulture)));
            builder.AppendLine(hasInputExtra ? ", BS_InputExtra bs_input_extra)" : ")");
            builder.AppendLine("{");
            builder.AppendLine("\tBS_Output bs_output;");
            WriteCodeLines(builder, reflection.CodeMainLines);
            builder.AppendLine("\treturn bs_output;");
            builder.AppendLine("}");
        }

        public static HullShaderProfile ParseProfile(string profileString)
        {
            switch (profileString)
            {
                case "hs_5_0": return HullShaderProfile.hs_5_0;
                default: throw new InvalidDataException(string.Format("'{0}' is not a valid hull shader profile.", profileString));
            }
        }

        public static string ProfileToString(HullShaderProfile profile)
        {
            switch (profile)
            {
                case HullShaderProfile.hs_5_0: return "hs_5_0";
                default: throw new ArgumentOutOfRangeException("profile");
            }
        }

        public static string DomainToString(TesselationDomain domain)
        {
            switch (domain)
            {
                case TesselationDomain.Isoline: return "isoline";
                case TesselationDomain.Triangle: return "tri";
                case TesselationDomain.Quad: return "quad";
                default: throw new ArgumentOutOfRangeException("domain");
            }
        }

        public static string PartitioningToString(TesselationPartitioning partitioning)
        {
            switch (partitioning)
            {
                case TesselationPartitioning.Integer: return "integer";
                case TesselationPartitioning.FractionalEven: return "fractional_even";
                case TesselationPartitioning.FractionalOdd: return "fractional_odd";
                default: throw new ArgumentOutOfRangeException("partitioning");
            }
        }

        public static string TopologyToString(TesselationTopology topology)
        {
            switch (topology)
            {
                case TesselationTopology.Line: return "line";
                case TesselationTopology.TriangleCW: return "triangle_cw";
                case TesselationTopology.TriangleCCW: return "triangle_ccw";
                default: throw new ArgumentOutOfRangeException("topology");
            }
        }

        static int GetTessFactorLength(TesselationDomain domain)
        {
            switch (domain)
            {
                case TesselationDomain.Isoline: return 2;
                case TesselationDomain.Triangle: return 3;
                case TesselationDomain.Quad: return 4;
                default: throw new ArgumentOutOfRangeException("domain");
            }
        }

        static int GetInsideTessFactorLength(TesselationDomain domain)
        {
            switch (domain)
            {
                case TesselationDomain.Isoline: return 0;
                case TesselationDomain.Triangle: return 1;
                case TesselationDomain.Quad: return 2;
                default: throw new ArgumentOutOfRangeException("domain");
            }
        }
    }
}
