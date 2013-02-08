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
using Beholder.Core;
using Beholder.Shaders;
using Beholder.Utility.ForImplementations;
using Beholder.Utility.Structures;
using SharpDX.D3DCompiler;
using SharpDX.Direct3D11;
using System.Linq;

namespace Beholder.Libraries.SharpDX11.Shaders
{
    sealed class CVertexShader : CShader, IVertexShader, IDisposableInternal
    {
        public override ShaderStage Stage { get { return ShaderStage.Vertex; } }
        public VertexShader D3DVertexShader { get; private set; }
        public VertexShaderProfile Profile { get; private set; }

        CompilationResult Bytecode { get; set; }
        List<Pair<InputElement, int>> D3DInputElementsDraft { get; set; } 

        public CVertexShader(ICDevice device, CShaderReflection reflection) 
            : base(device, reflection)
        {
            Profile = ParseProfile(Reflection.Profile);
            var text = GenerateText<CVertexShader>(WriteIOAndCode);
            try
            {
                Bytecode = ShaderBytecode.Compile(text, "main", ProfileToString(Profile),
                    ShaderFlags.PackMatrixColumnMajor | ShaderFlags.OptimizationLevel3, EffectFlags.None, Name);
            }
            catch (Exception e)
            {
                throw new ArgumentException(string.Format("Failed to compile a vertex shader '{0}'\r\n--- Code ---\r\n{1}\r\n--- Errors ---\r\n{2}", Name, text, e.Message), e);
            }
            D3DVertexShader = new VertexShader(device.D3DDevice, Bytecode);
            D3DInputElementsDraft = CreateVertexElementsDraft(reflection);
        }

        public void DisposeInternal()
        {
            D3DVertexShader.Dispose();
        }

        static List<Pair<InputElement, int>> CreateVertexElementsDraft(CShaderReflection reflection)
        {
            var vertexElementsDraft = new List<Pair<InputElement, int>>();
            int bElementIndex = 0;
            foreach (var variable in reflection.Input)
            {
                if (variable.IsUsed)
                {
                    for (int i = 0; i < variable.LayoutElementsRequired; i++)
                    {
                        vertexElementsDraft.Add(new Pair<InputElement, int>(new InputElement
                        {
                            SemanticName = variable.Semantic,
                            SemanticIndex = i
                        }, bElementIndex));
                    }
                }
                bElementIndex++;
            }
            return vertexElementsDraft;
        }

        static void WriteIOAndCode(StringBuilder builder, CVertexShader shader)
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

        public static VertexShaderProfile ParseProfile(string profileString)
        {
            switch (profileString)
            {
                case "vs_4_0": return VertexShaderProfile.vs_4_0;
                case "vs_4_1": return VertexShaderProfile.vs_4_1;
                case "vs_5_0": return VertexShaderProfile.vs_5_0;
                default: throw new InvalidDataException(string.Format("'{0}' is not a valid vertex shader profile.", profileString));
            }
        }

        public static string ProfileToString(VertexShaderProfile profile)
        {
            switch (profile)
            {
                case VertexShaderProfile.vs_4_0: return "vs_4_0";
                case VertexShaderProfile.vs_4_1: return "vs_4_1";
                case VertexShaderProfile.vs_5_0: return "vs_5_0";
                default: throw new ArgumentOutOfRangeException("profile");
            }
        }

        public InputLayout GenerateD3DInputLayout(VertexLayoutElement[] bElements)
        {
            var d3dElements =  D3DInputElementsDraft.Select(draftPair =>
            {
                var d3dElement = draftPair.First;
                var bElement = bElements[draftPair.Second];

                d3dElement.Format = CtSharpDX11.Format(bElement.Format);
                d3dElement.Slot = bElement.InputSlot;
                d3dElement.AlignedByteOffset = bElement.Offset;
                d3dElement.Classification = CtSharpDX11.InputClassification(bElement.VertexLayoutClassification);
                d3dElement.InstanceDataStepRate = bElement.InstanceDataStepRate;
                return d3dElement;
            }).ToArray();
            return new InputLayout(device.D3DDevice, Bytecode, d3dElements);
        }
    }
}
