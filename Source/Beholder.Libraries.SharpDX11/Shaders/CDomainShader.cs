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
    sealed class CDomainShader : CShader, IDomainShader, IDisposableInternal
    {
        public override ShaderStage Stage { get { return ShaderStage.Domain; } }
        public DomainShader D3DDomainShader { get; private set; }
        public DomainShaderProfile Profile { get; private set; }
        public TesselationDomain Domain { get; private set; }
        public int InputControlPoints { get; private set; }

        public CDomainShader(ICDevice device, CShaderReflection reflection) 
            : base(device, reflection)
        {
            Profile = ParseProfile(reflection.Profile);
            Domain = reflection.GetTesselationDomain();
            InputControlPoints = reflection.GetInputControlPoints();

            var text = GenerateText<CDomainShader>(WriteIOAndCode);
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
            D3DDomainShader = new DomainShader(device.D3DDevice, bytecode);
        }

        public void DisposeInternal()
        {
            D3DDomainShader.Dispose();
        }

        static void WriteIOAndCode(StringBuilder builder, CDomainShader shader)
        {
            var reflection = shader.Reflection;
            WriteCodeLines(builder, reflection.CodeGlobalLines);
            builder.AppendLine();

            builder.AppendLine("#define INPUT(I, N) bs_input[##I].##N");
            WriteIOStructure(builder, reflection.Input, "BS_Input");

            var hasInputExtra = reflection.InputExtra.Any(v => v.IsUsed);
            if (hasInputExtra)
            {
                builder.AppendLine("#define INPUT_EXTRA(X) bs_input_extra.##X");
                WriteIOStructure(builder, reflection.InputExtra, "BS_InputExtra");
            }

            var hasInputPatch = reflection.InputPatch.Any(v => v.IsUsed);
            if (hasInputPatch)
            {
                builder.AppendLine("#define INPUT_PATCH(X) bs_input_patch.##X");
                WriteIOStructure(builder, reflection.InputPatch.Select(v =>
                {
                    switch (v.Semantic.ToLower())
                    {
                        case "sv_tessfactor": return new CShaderIOVariable(new ShaderIOVariable(v.Interpolation, new ShaderValueArrayType(v.Type, GetTessFactorLength(shader.Domain)), v.Name, v.SpecialParameters));
                        case "sv_insidetessfactor": return new CShaderIOVariable(new ShaderIOVariable(v.Interpolation, new ShaderValueArrayType(v.Type, GetInsideTessFactorLength(shader.Domain)), v.Name, v.SpecialParameters));
                        default: return v;
                    }
                }), "BS_InputPatch");
            }
            
            builder.AppendLine("#define OUTPUT(X) bs_output.##X");
            WriteIOStructure(builder, reflection.Output, "BS_Output");

            builder.AppendLine(string.Format("[domain(\"{0}\")]", DomainToString(shader.Domain)));
            builder.AppendLine(string.Format("BS_Output main(const OutputPatch<BS_Input, {0}> bs_input", shader.InputControlPoints.ToString(CultureInfo.InvariantCulture)));
            if (hasInputExtra)
                builder.AppendLine(", BS_InputExtra bs_input_extra");
            if (hasInputPatch) 
                builder.AppendLine(", BS_InputPatch bs_input_patch");
            builder.AppendLine(")");
            builder.AppendLine("{");
            builder.AppendLine("\tBS_Output bs_output;");
            WriteCodeLines(builder, reflection.CodeMainLines);
            builder.AppendLine("\treturn bs_output;");
            builder.AppendLine("}");
        }

        public static DomainShaderProfile ParseProfile(string profileString)
        {
            switch (profileString)
            {
                case "ds_5_0": return DomainShaderProfile.ds_5_0;
                default: throw new InvalidDataException(string.Format("'{0}' is not a valid domain shader profile.", profileString));
            }
        }

        public static string ProfileToString(DomainShaderProfile profile)
        {
            switch (profile)
            {
                case DomainShaderProfile.ds_5_0: return "ds_5_0";
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
