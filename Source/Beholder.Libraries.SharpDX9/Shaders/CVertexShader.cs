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
using System.Linq;
using Beholder.Core;
using Beholder.Shaders;
using Beholder.Utility.ForImplementations;
using Beholder.Utility.Structures;
using SharpDX.Direct3D9;

namespace Beholder.Libraries.SharpDX9.Shaders
{
    class CVertexShader : CShader, IVertexShader, IDisposableInternal
    {
        readonly VertexShader d3dShader;
        readonly VertexShaderProfile profile;
        readonly List<Pair<VertexElement, int>> d3dVertexElementsDraft;

        public override ShaderStage Stage { get { return ShaderStage.Vertex; } }
        public VertexShader D3DShader { get { return d3dShader; } }
        public VertexShaderProfile Profile { get { return profile; } }

        public CVertexShader(ICDevice device, CShaderReflection reflection) 
            : base(device, reflection)
        {
            profile = ParseProfile(reflection.Profile);

            var text = GenerateText();
            try
            {
                var bytecode = ShaderBytecode.Compile(text, "main", ProfileToString(profile), ShaderFlags.PackMatrixColumnMajor | ShaderFlags.OptimizationLevel3);
                d3dShader = new VertexShader(device.D3DDevice, bytecode);
                bytecode.Dispose();
            }
            catch (Exception e)
            {
                throw new ArgumentException(string.Format("Failed to compile a vertex shader '{0}'\r\n--- Code ---\r\n{1}\r\n--- Errors ---\r\n{2}", Name, text, e.Message), e);
            }

            d3dVertexElementsDraft = CreateVertexElementsDraft(reflection);
        }

        public void DisposeInternal()
        {
            d3dShader.Dispose();
        }

        static List<Pair<VertexElement, int>> CreateVertexElementsDraft(CShaderReflection reflection)
        {
            var vertexElementsDraft = new List<Pair<VertexElement, int>>();
            int bElementIndex = 0;
            foreach (var variable in reflection.Input)
            {
                if (variable.IsUsed)
                {
                    for (int i = 0; i < variable.LayoutElementsRequired; i++)
                    {
                        vertexElementsDraft.Add(new Pair<VertexElement, int>(new VertexElement
                        {
                            Method = 0,
                            Usage = variable.Usage,
                            UsageIndex = variable.UsageIndex
                        }, bElementIndex));
                    }
                }
                bElementIndex++;
            }
            return vertexElementsDraft;
        }

        public static VertexShaderProfile ParseProfile(string profileString)
        {
            switch (profileString)
            {
                case "vs_2_0": return VertexShaderProfile.vs_2_0;
                case "vs_3_0": return VertexShaderProfile.vs_3_0;
                default: throw new InvalidDataException(string.Format("'{0}' is not a valid vertex shader profile.", profileString));
            }
        }

        public static string ProfileToString(VertexShaderProfile profile)
        {
            switch (profile)
            {
                case VertexShaderProfile.vs_2_0: return "vs_2_0";
                case VertexShaderProfile.vs_3_0: return "vs_3_0";
                default: throw new ArgumentOutOfRangeException("profile");
            }
        }

        static readonly VertexElement[] D3DElementsEdning = { VertexElement.VertexDeclarationEnd }; 

        public VertexElement[] GenerateD3DVertexDeclarationElements(VertexLayoutElement[] bElements)
        {
            return d3dVertexElementsDraft.Select(draftPair =>
            {
                var d3dElement = draftPair.First;
                var bElement = bElements[draftPair.Second];

                d3dElement.Stream = (short)bElement.InputSlot;
                d3dElement.Offset = (short)bElement.Offset;
                d3dElement.Type = CtSharpDX9.DeclarationType(bElement.Format);

                return d3dElement;
            }).Concat(D3DElementsEdning).ToArray();
        }
    }
}
