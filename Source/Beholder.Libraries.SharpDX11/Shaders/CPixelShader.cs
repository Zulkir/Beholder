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
using System.IO;
using System.Text;
using Beholder.Shaders;
using Beholder.Utility.ForImplementations;
using SharpDX.D3DCompiler;
using SharpDX.Direct3D11;

namespace Beholder.Libraries.SharpDX11.Shaders
{
    sealed class CPixelShader : CShader, IPixelShader, IDisposableInternal
    {
        public override ShaderStage Stage { get { return ShaderStage.Pixel; } }
        public PixelShader D3DPixelShader { get; private set; }
        public PixelShaderProfile Profile { get; private set; }

        public CPixelShader(ICDevice device, CShaderReflection reflection) 
            : base(device, reflection)
        {
            Profile = ParseProfile(Reflection.Profile);
            var text = GenerateText<CPixelShader>(WriteIOAndCode);
            CompilationResult bytecode;
            try
            {
                bytecode = ShaderBytecode.Compile(text, "main", ProfileToString(Profile),
                    ShaderFlags.PackMatrixColumnMajor | ShaderFlags.OptimizationLevel3, EffectFlags.None, Name);
            }
            catch (Exception e)
            {
                throw new ArgumentException(string.Format("Failed to compile a pixel shader '{0}'\r\n--- Code ---\r\n{1}\r\n--- Errors ---\r\n{2}", Name, text, e.Message), e);
            }
            D3DPixelShader = new PixelShader(device.D3DDevice, bytecode);
        }

        public void DisposeInternal()
        {
            D3DPixelShader.Dispose();
        }

        static void WriteIOAndCode(StringBuilder builder, CPixelShader shader)
        {
            var reflection = shader.Reflection;
            WriteCodeLines(builder, reflection.CodeGlobalLines);
            builder.AppendLine();

            builder.AppendLine("#define INPUT(X) bs_input.##X");
            WriteIOStructure(builder, reflection.Input, "BS_Input");
            builder.AppendLine("#define OUTPUT(X) bs_output.##X");
            WriteIOStructure(builder, reflection.Output, "BS_Output");

            builder.AppendLine("BS_Output main(BS_Input bs_input)");
            builder.AppendLine("{");
            builder.AppendLine("\tBS_Output bs_output;");
            WriteCodeLines(builder, reflection.CodeMainLines);
            builder.AppendLine("\treturn bs_output;");
            builder.AppendLine("}");
        }

        public static PixelShaderProfile ParseProfile(string profileString)
        {
            switch (profileString)
            {
                case "ps_4_0": return PixelShaderProfile.ps_4_0;
                case "ps_4_1": return PixelShaderProfile.ps_4_1;
                case "ps_5_0": return PixelShaderProfile.ps_5_0;
                default: throw new InvalidDataException(string.Format("'{0}' is not a valid pixel shader profile.", profileString));
            }
        }

        public static string ProfileToString(PixelShaderProfile profile)
        {
            switch (profile)
            {
                case PixelShaderProfile.ps_4_0: return "ps_4_0";
                case PixelShaderProfile.ps_4_1: return "ps_4_1";
                case PixelShaderProfile.ps_5_0: return "ps_5_0";
                default: throw new ArgumentOutOfRangeException("profile");
            }
        }
    }
}
