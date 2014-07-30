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
using Beholder.Shaders.Reflection;
using Beholder.Utility.ForImplementations;
using SharpDX.D3DCompiler;
using SharpDX.Direct3D11;

namespace Beholder.Libraries.SharpDX11.Shaders
{
    sealed class CComputeShader : CShader, IComputeShader, IDisposableInternal
    {
        public override ShaderStage Stage { get { return ShaderStage.Compute; } }
        public ComputeShader D3DComputeShader { get; private set; }
        public ComputeShaderProfile Profile { get; private set; }

        public int ThreadCountX { get; private set; }
        public int ThreadCountY { get; private set; }
        public int ThreadCountZ { get; private set; }

        public CComputeShader(ICDevice device, CShaderReflection reflection) 
            : base(device, reflection)
        {
            Profile = ParseProfile(reflection.Profile);
            ThreadCountX = reflection.GetThreadCountX();
            ThreadCountY = reflection.GetThreadCountY();
            ThreadCountZ = reflection.GetThreadCountZ();

            var text = GenerateText<CComputeShader>(WriteIOAndCode);
            CompilationResult bytecode;
            try
            {
                bytecode = ShaderBytecode.Compile(text, "main", ProfileToString(Profile),
                    ShaderFlags.PackMatrixColumnMajor | ShaderFlags.OptimizationLevel3, EffectFlags.None, Name);
            }
            catch (Exception e)
            {
                throw new ArgumentException(string.Format("Failed to compile a compute shader '{0}'\r\n--- Code ---\r\n{1}\r\n--- Errors ---\r\n{2}", Name, text, e.Message), e);
            }
            D3DComputeShader = new ComputeShader(device.D3DDevice, bytecode);
        }

        public void DisposeInternal()
        {
            D3DComputeShader.Dispose();
        }

        static void WriteIOAndCode(StringBuilder builder, CComputeShader shader)
        {
            var reflection = shader.Reflection;
            WriteCodeLines(builder, reflection.CodeGlobalLines);
            builder.AppendLine();

            builder.AppendLine("#define INPUT(X) bs_input.##X");
            WriteIOStructure(builder, reflection.Input, "BS_Input");

            builder.AppendLine(string.Format("[numthreads({0}, {1}, {2})]", shader.ThreadCountX, shader.ThreadCountY, shader.ThreadCountZ));
            builder.AppendLine("void main(BS_Input bs_input)");
            builder.AppendLine("{");
            WriteCodeLines(builder, reflection.CodeMainLines);
            builder.AppendLine("}");
        }

        public static ComputeShaderProfile ParseProfile(string profileString)
        {
            switch (profileString)
            {
                case "cs_4_0": return ComputeShaderProfile.cs_4_0;
                case "cs_4_1": return ComputeShaderProfile.cs_4_1;
                case "cs_5_0": return ComputeShaderProfile.cs_5_0;
                default: throw new InvalidDataException(string.Format("'{0}' is not a valid compute shader profile.", profileString));
            }
        }

        public static string ProfileToString(ComputeShaderProfile profile)
        {
            switch (profile)
            {
                case ComputeShaderProfile.cs_4_0: return "cs_4_0";
                case ComputeShaderProfile.cs_4_1: return "cs_4_1";
                case ComputeShaderProfile.cs_5_0: return "cs_5_0";
                default: throw new ArgumentOutOfRangeException("profile");
            }
        }
    }
}
