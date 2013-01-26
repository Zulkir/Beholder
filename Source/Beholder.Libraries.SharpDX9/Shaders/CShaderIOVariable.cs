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
using System.Text.RegularExpressions;
using Beholder.Shaders.Reflection;
using SharpDX.Direct3D9;

namespace Beholder.Libraries.SharpDX9.Shaders
{
    class CShaderIOVariable : ShaderIOVariable
    {
        readonly string semanticString;
        readonly DeclarationUsage usage;
        readonly byte usageIndex;
        readonly bool isUsed;

        public string SemanticString { get { return semanticString; } }
        public DeclarationUsage Usage { get { return usage; } }
        public byte UsageIndex { get { return usageIndex; } }
        public bool IsUsed { get { return isUsed; } }

        public CShaderIOVariable(ShaderIOVariable bVariable)
            : base(bVariable.Interpolation, bVariable.Type, bVariable.Name, bVariable.SpecialParameters)
        {
            if (bVariable.Interpolation != ShaderIOVariableInterpolation.Default)
                throw new ArgumentException("Interpolation modifiers are not supported by D3D9");

            semanticString = bVariable.GetRequiredSpecialParameter("SDX9");
            if (semanticString == "%name")
                semanticString = Name;
            isUsed = semanticString != "%unused";
            if (isUsed)
                ParseSemantic(semanticString, out usage, out usageIndex);
        }

        static readonly Regex SemanticEx = new Regex(@"^([a-zA-Z]+)(\d+)?$");

        static void ParseSemantic(string semanticString, out DeclarationUsage declarationUsage, out byte index)
        {
            var match = SemanticEx.Match(semanticString);
            if (!match.Success)
                throw new InvalidDataException(string.Format("'{0}' is not a correct D3D9 semantic", semanticString));
            declarationUsage = ParseDeclarationUsage(match.Groups[1].Value);
            index = string.IsNullOrEmpty(match.Groups[2].Value) ? (byte)0 : byte.Parse(match.Groups[2].Value);
        }

        static DeclarationUsage ParseDeclarationUsage(string semantic)
        {
            switch (semantic.ToUpper())
            {
                case "POSITION": return DeclarationUsage.Position;
                case "BLENDWEIGHT": return DeclarationUsage.BlendWeight;
                case "BLENDINDICES": return DeclarationUsage.BlendIndices;
                case "NORMAL": return DeclarationUsage.Normal;
                case "PSIZE": return DeclarationUsage.PointSize;
                case "TEXCOORD": return DeclarationUsage.TextureCoordinate;
                case "TANGENT": return DeclarationUsage.Tangent;
                case "BINORMAL": return DeclarationUsage.Binormal;
                case "TESSFACTOR": return DeclarationUsage.TessellateFactor;
                case "POSITIONT": return DeclarationUsage.PositionTransformed;
                case "COLOR": return DeclarationUsage.Color;
                case "FOG": return DeclarationUsage.Fog;
                case "DEPTH": return DeclarationUsage.Depth;
                case "SAMPLE": return DeclarationUsage.Sample;
                default: throw new InvalidDataException(string.Format("'{0}' is not a correct D3D9 semantic", semantic));
            }
        }

        public static bool AreCompatible(IEnumerable<CShaderIOVariable> output, IEnumerable<CShaderIOVariable> input)
        {
            return input.Where(i => i.IsUsed).All(i => output.Any(o => o.Usage == i.Usage && o.UsageIndex == i.UsageIndex && o.Type.Equals(i.Type)));
        }
    }
}
