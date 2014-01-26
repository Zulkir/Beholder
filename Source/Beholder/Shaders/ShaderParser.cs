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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Beholder.Shaders.Reflection;
using Beholder.Utility.Structures;

namespace Beholder.Shaders
{
    public static class ShaderParser
    {
        enum ParsingState
        {
            Starting,
            Meta,
            Structures,
            UniformBuffers,
            Samplers,
            Srvs,
            Uavs,
            Input,
            InputExtra,
            InputPatch,
            InputPatchExtra,
            Output,
            OutputPatch,
            OutputStreams,
            FixedSampling,
            CodeGlobal,
            CodeMain,
            CodePatch
        }

        public static ShaderReflection Parse(string text)
        {
            return ParseLines(text.Split('\n'));
        }

        public static ShaderReflection ParseLines(IEnumerable<string> lines)
        {
            var state = ParsingState.Starting;
            var structures = new Dictionary<string, ShaderValueStructureType>();
            List<KeyValuePair<string, string>> meta = null;
            string structureName = null;
            List<Pair<ShaderValueType, string>> structurePairs = null;
            List<ShaderUniformBufferVariable> uniformBuffers = null;
            string ubufferName = null;
            List<Pair<ShaderValueType, string>> ubufferPairs = null;
            KeyValuePair<string, string>[] ubufferSpecialParameters = null;
            List<ShaderSamplerVariable> samplers = null;
            List<ShaderObjectVariable> srvs = null;
            List<ShaderObjectVariable> uavs = null;
            List<ShaderIOVariable> input = null;
            List<ShaderIOVariable> inputExtra = null;
            List<ShaderIOVariable> inputPatch = null;
            List<ShaderIOVariable> inputPatchExtra = null;
            List<ShaderIOVariable> output = null;
            List<ShaderIOVariable> outputPatch = null;
            List<List<ShaderIOVariable>> outputStreams = null;
            List<ShaderFixedSamplingPair> fixedSamplingPairs = null;
            List<string> codeGlobalLines = null;
            List<string> codeMainLines = null;
            List<string> codePatchLines = null;

            foreach (var line in lines)
            {
                var trimmedLine = line.Trim();

                if (state != ParsingState.CodeGlobal && state != ParsingState.CodeMain && 
                    state != ParsingState.CodePatch && trimmedLine == "")
                    continue;

                switch (state)
                {
                    case ParsingState.Starting:
                        state = ParseParsingState(trimmedLine);
                        break;
                    case ParsingState.Meta: SimpleProcessing(trimmedLine, ref state, ref meta, ParseMeta); break;
                    case ParsingState.Structures:
                        structurePairs = structurePairs ?? new List<Pair<ShaderValueType, string>>();

                        if (trimmedLine.StartsWith("%") || trimmedLine.StartsWith("struct"))
                        {
                            if (structureName != null)
                            {
                                if (structurePairs.Count == 0)
                                    throw new InvalidDataException("Structures cannot be empty");
                                structures.Add(structureName, new ShaderValueStructureType(structureName, structurePairs));
                                structurePairs.Clear();
                                structureName = null;
                            }

                            if (trimmedLine.StartsWith("%")) { state = ParseParsingState(trimmedLine); continue; }

                            if (trimmedLine.StartsWith("struct"))
                                structureName = ParseStrcutureDeclaration(trimmedLine);
                        }
                        else if (structureName == null)
                            throw new InvalidDataException(string.Format("struct declaration expected, but '{0}' found", trimmedLine));
                        else
                            structurePairs.Add(ParseStructureElementTypeNamePair(trimmedLine, structures));
                        break;
                    case ParsingState.UniformBuffers:
                        uniformBuffers = uniformBuffers ?? new List<ShaderUniformBufferVariable>();
                        ubufferPairs = ubufferPairs ?? new List<Pair<ShaderValueType, string>>();

                        if (trimmedLine.StartsWith("%") || trimmedLine.StartsWith("ubuffer"))
                        {
                            if (ubufferName != null)
                            {
                                if (ubufferPairs.Count == 0)
                                    throw new InvalidDataException("Uniform Buffers cannot be empty");
                                uniformBuffers.Add(new ShaderUniformBufferVariable(ubufferName, ubufferPairs, ubufferSpecialParameters));
                                ubufferPairs.Clear();
                                ubufferName = null;
                            }

                            if (trimmedLine.StartsWith("%")) { state = ParseParsingState(trimmedLine); continue; }

                            if (trimmedLine.StartsWith("ubuffer"))
                                ParseUniformBufferHeader(trimmedLine, out ubufferName, out ubufferSpecialParameters);
                        }
                        else if (ubufferName == null)
                            throw new InvalidDataException(string.Format("ubuffer declaration expected, but '{0}' found", trimmedLine));
                        else
                            ubufferPairs.Add(ParseStructureElementTypeNamePair(trimmedLine, structures));
                        break;
                    case ParsingState.Samplers: SimpleProcessing(trimmedLine, ref state, ref samplers, ParseSampler); break;
                    case ParsingState.Srvs: SimpleProcessing(trimmedLine, ref state, ref srvs, structures, ParseObjectVariable); break;
                    case ParsingState.Uavs: SimpleProcessing(trimmedLine, ref state, ref uavs, structures, ParseObjectVariable); break;
                    case ParsingState.Input: SimpleProcessing(trimmedLine, ref state, ref input, structures, ParseIOVariable); break;
                    case ParsingState.InputExtra: SimpleProcessing(trimmedLine, ref state, ref inputExtra, structures, ParseIOVariable); break;
                    case ParsingState.InputPatch: SimpleProcessing(trimmedLine, ref state, ref inputPatch, structures, ParseIOVariable); break;
                    case ParsingState.InputPatchExtra: SimpleProcessing(trimmedLine, ref state, ref inputPatchExtra, structures, ParseIOVariable); break;
                    case ParsingState.Output: SimpleProcessing(trimmedLine, ref state, ref output, structures, ParseIOVariable); break;
                    case ParsingState.OutputPatch: SimpleProcessing(trimmedLine, ref state, ref outputPatch, structures, ParseIOVariable); break;
                    case ParsingState.OutputStreams:
                        if (trimmedLine.StartsWith("%")) { state = ParseParsingState(trimmedLine); continue; }
                        outputStreams = outputStreams ?? new List<List<ShaderIOVariable>>();
                        if (trimmedLine.StartsWith("stream"))
                            outputStreams.Add(new List<ShaderIOVariable>());
                        else if (outputStreams.Count == 0)
                            throw new InvalidDataException(string.Format("stream declaration expected, but '{0}' found", trimmedLine));
                        else
                            outputStreams[outputStreams.Count - 1].Add(ParseIOVariable(trimmedLine, structures));
                        break;
                    case ParsingState.FixedSampling: SimpleProcessing(trimmedLine, ref state, ref fixedSamplingPairs, ParseFixedSamplingPair); break;
                    case ParsingState.CodeGlobal: CodeProcessing(line, trimmedLine, ref state, ref codeGlobalLines); break;
                    case ParsingState.CodeMain: CodeProcessing(line, trimmedLine, ref state, ref codeMainLines); break;
                    case ParsingState.CodePatch: CodeProcessing(line, trimmedLine, ref state, ref codePatchLines); break;
                }
            }

            if (structureName != null)
            {
                if (structurePairs.Count == 0)
                    throw new InvalidDataException("Structures cannot be empty");
                structures.Add(structureName, new ShaderValueStructureType(structureName, structurePairs));
                structurePairs.Clear();
            }

            if (ubufferName != null)
            {
                if (ubufferPairs.Count == 0)
                    throw new InvalidDataException("Uniform Buffers cannot be empty");
                uniformBuffers.Add(new ShaderUniformBufferVariable(ubufferName, ubufferPairs, ubufferSpecialParameters));
                ubufferPairs.Clear();
            }

            return new ShaderReflection(meta, structures.Values, uniformBuffers, samplers, srvs, uavs, 
                input, inputExtra, inputPatch, inputPatchExtra, output, outputPatch, outputStreams, fixedSamplingPairs, 
                codeGlobalLines, codeMainLines, codePatchLines);
        }

        static void SimpleProcessing<T>(string trimmedLine, ref ParsingState state, ref List<T> list, 
            IDictionary<string, ShaderValueStructureType> structures, Func<string, IDictionary<string, ShaderValueStructureType>, T> parse)
        {
            if (trimmedLine.StartsWith("%")) { state = ParseParsingState(trimmedLine); return; }
            list = list ?? new List<T>();
            list.Add(parse(trimmedLine, structures));
        }

        static void SimpleProcessing<T>(string trimmedLine, ref ParsingState state, ref List<T> list, Func<string, T> parse)
        {
            SimpleProcessing(trimmedLine, ref state, ref list, null, (l, s) => parse(l));
        }

        static void CodeProcessing(string line, string trimmedLine, ref ParsingState state, ref List<string> list)
        {
            if (trimmedLine.StartsWith("%")) { state = ParseParsingState(trimmedLine); return; }
            list = list ?? new List<string>();
            list.Add(line.TrimEnd());
        }

        static ParsingState ParseParsingState(string trimmedLine)
        {
            switch (trimmedLine)
            {
                case "%meta": return ParsingState.Meta;
                case "%structures": return ParsingState.Structures;
                case "%ubuffers": return ParsingState.UniformBuffers;
                case "%samplers": return ParsingState.Samplers;
                case "%srvs": return ParsingState.Srvs;
                case "%uavs": return ParsingState.Uavs;
                case "%input": return ParsingState.Input;
                case "%input_extra": return ParsingState.InputExtra;
                case "%input_patch": return ParsingState.InputPatch;
                case "%input_patch_extra": return ParsingState.InputPatchExtra;
                case "%output": return ParsingState.Output;
                case "%output_patch": return ParsingState.OutputPatch;
                case "%output_streams": return ParsingState.OutputStreams;
                case "%fixed_sampling": return ParsingState.FixedSampling;
                case "%code_global": return ParsingState.CodeGlobal;
                case "%code_main": return ParsingState.CodeMain;
                case "%code_patch": return ParsingState.CodePatch;
                default: throw new ArgumentOutOfRangeException("trimmedLine", string.Format("Unexpected region '{0}'", trimmedLine));
            }
        }

        readonly static Regex AssignmentEx = new Regex(@"^([_a-zA-Z]\w*)\s*=\s*(.+)$");

        static KeyValuePair<string, string> ParseMeta(string trimmedLine)
        {
            var match = AssignmentEx.Match(trimmedLine);
            if (!match.Success)
                throw new InvalidDataException(string.Format("'{0}' is not a valid meta line", trimmedLine));
            return new KeyValuePair<string, string>(match.Groups[1].Value, match.Groups[2].Value);
        }

        readonly static Regex SomethingWithSpecialParametersEx = new Regex(@"^([\w\s\[\]\<\>,]*)(\s*:\s*([\w\s\=,%\.]*))?$");
        readonly static char[] SpecialParametersSeparators = new[] { ',' };

        static void ParseSomethingWithSpecialParameters(string trimmedLine, string somethingType, out string trimmedDeclaration, out KeyValuePair<string, string>[] specialParameters)
        {
            var match = SomethingWithSpecialParametersEx.Match(trimmedLine);
            if (!match.Success)
                throw new InvalidDataException(string.Format("'{0}' is not a valid {1} declaration", trimmedLine, somethingType));
            trimmedDeclaration = match.Groups[1].Value.Trim();
            specialParameters = string.IsNullOrEmpty(match.Groups[2].Value) 
                ? new KeyValuePair<string, string>[0] 
                : ParseSpecialParameters(match.Groups[3].Value);
        }

        static KeyValuePair<string, string>[] ParseSpecialParameters(string paramsLine)
        {
            var pairStrings = paramsLine.Split(SpecialParametersSeparators);
            var result = new KeyValuePair<string, string>[pairStrings.Length];
            for (int i = 0; i < pairStrings.Length; i++)
            {
                var match = AssignmentEx.Match(pairStrings[i].Trim());
                if (!match.Success)
                    throw new InvalidDataException(string.Format("'{0}' is not a valid special parameter assignment", pairStrings[i].Trim()));
                result[i] = new KeyValuePair<string, string>(match.Groups[1].Value, match.Groups[2].Value);
            }
            return result;
        }

        static readonly Regex VariableArrayEx = new Regex(@"^([\w\s\<\>\,]*)\s*\[\s*(\d+)\s*]$");
        static readonly Regex VariableNameEx = new Regex(@"^([\w\s\<\>\,]*)\s+([_a-zA-Z]\w*)$");
        static readonly Regex VariableTypeEx = new Regex(@"^([\w\s]+\s+)?([_a-zA-Z]\w*(\s*<[\w\s,]+>)?)$");

        static void ParseVariableDeclaration(string trimmedLine, out string modifyers, out string type, out string name, out int? arraySize)
        {
            string lineWithoutArray;
            var variableArrayMatch = VariableArrayEx.Match(trimmedLine);
            if (variableArrayMatch.Success)
            {
                arraySize = int.Parse(variableArrayMatch.Groups[2].Value);
                lineWithoutArray = variableArrayMatch.Groups[1].Value.Trim();
            }
            else
            {
                arraySize = null;
                lineWithoutArray = trimmedLine;
            }

            var variableNameMatch = VariableNameEx.Match(lineWithoutArray);
            if (!variableNameMatch.Success)
                throw new InvalidDataException(string.Format("'{0}' is not a valid variable declaration", lineWithoutArray));

            var typeString = variableNameMatch.Groups[1].Value.Trim();
            name = variableNameMatch.Groups[2].Value;

            var variableTypeMatch = VariableTypeEx.Match(typeString);
            if (!variableTypeMatch.Success)
                throw new InvalidDataException(string.Format("'{0}' is not a valid variable type declaration", typeString));

            modifyers = variableTypeMatch.Groups[1].Value.Trim();
            type = variableTypeMatch.Groups[2].Value.Trim();
        }

        static ShaderValueType ParseValueType(string trimmedLine, IDictionary<string, ShaderValueStructureType> structures)
        {
            switch (trimmedLine)
            {
                #region Giant Switch
                case "float": return ShaderValueScalarType.Float;
                case "double": return ShaderValueScalarType.Double;
                case "int": return ShaderValueScalarType.Int;
                case "uint": return ShaderValueScalarType.UInt;
                case "snorm": return ShaderValueScalarType.SNorm;
                case "unorm": return ShaderValueScalarType.UNorm;
                case "bool": return ShaderValueScalarType.Bool;

                case "float2": return ShaderValueVectorType.Float2;
                case "float3": return ShaderValueVectorType.Float3;
                case "float4": return ShaderValueVectorType.Float4;
                case "double2": return ShaderValueVectorType.Double2;
                case "double3": return ShaderValueVectorType.Double3;
                case "double4": return ShaderValueVectorType.Double4;
                case "int2": return ShaderValueVectorType.Int2;
                case "int3": return ShaderValueVectorType.Int3;
                case "int4": return ShaderValueVectorType.Int4;
                case "uint2": return ShaderValueVectorType.UInt2;
                case "uint3": return ShaderValueVectorType.UInt3;
                case "uint4": return ShaderValueVectorType.UInt4;
                case "snorm2": return ShaderValueVectorType.SNorm2;
                case "snorm3": return ShaderValueVectorType.SNorm3;
                case "snorm4": return ShaderValueVectorType.SNorm4;
                case "unorm2": return ShaderValueVectorType.UNorm2;
                case "unorm3": return ShaderValueVectorType.UNorm3;
                case "unorm4": return ShaderValueVectorType.UNorm4;
                case "bool2": return ShaderValueVectorType.Bool2;
                case "bool3": return ShaderValueVectorType.Bool3;
                case "bool4": return ShaderValueVectorType.Bool4;

                case "float1x2": return ShaderValueMatrixType.Float1x2;
                case "float2x2": return ShaderValueMatrixType.Float2x2;
                case "float3x2": return ShaderValueMatrixType.Float3x2;
                case "float4x2": return ShaderValueMatrixType.Float4x2;
                case "float1x3": return ShaderValueMatrixType.Float1x3;
                case "float2x3": return ShaderValueMatrixType.Float2x3;
                case "float3x3": return ShaderValueMatrixType.Float3x3;
                case "float4x3": return ShaderValueMatrixType.Float4x3;
                case "float1x4": return ShaderValueMatrixType.Float1x4;
                case "float2x4": return ShaderValueMatrixType.Float2x4;
                case "float3x4": return ShaderValueMatrixType.Float3x4;
                case "float4x4": return ShaderValueMatrixType.Float4x4;

                case "double1x2": return ShaderValueMatrixType.Double1x2;
                case "double2x2": return ShaderValueMatrixType.Double2x2;
                case "double3x2": return ShaderValueMatrixType.Double3x2;
                case "double4x2": return ShaderValueMatrixType.Double4x2;
                case "double1x3": return ShaderValueMatrixType.Double1x3;
                case "double2x3": return ShaderValueMatrixType.Double2x3;
                case "double3x3": return ShaderValueMatrixType.Double3x3;
                case "double4x3": return ShaderValueMatrixType.Double4x3;
                case "double1x4": return ShaderValueMatrixType.Double1x4;
                case "double2x4": return ShaderValueMatrixType.Double2x4;
                case "double3x4": return ShaderValueMatrixType.Double3x4;
                case "double4x4": return ShaderValueMatrixType.Double4x4;

                case "int1x2": return ShaderValueMatrixType.Int1x2;
                case "int2x2": return ShaderValueMatrixType.Int2x2;
                case "int3x2": return ShaderValueMatrixType.Int3x2;
                case "int4x2": return ShaderValueMatrixType.Int4x2;
                case "int1x3": return ShaderValueMatrixType.Int1x3;
                case "int2x3": return ShaderValueMatrixType.Int2x3;
                case "int3x3": return ShaderValueMatrixType.Int3x3;
                case "int4x3": return ShaderValueMatrixType.Int4x3;
                case "int1x4": return ShaderValueMatrixType.Int1x4;
                case "int2x4": return ShaderValueMatrixType.Int2x4;
                case "int3x4": return ShaderValueMatrixType.Int3x4;
                case "int4x4": return ShaderValueMatrixType.Int4x4;

                case "uint1x2": return ShaderValueMatrixType.UInt1x2;
                case "uint2x2": return ShaderValueMatrixType.UInt2x2;
                case "uint3x2": return ShaderValueMatrixType.UInt3x2;
                case "uint4x2": return ShaderValueMatrixType.UInt4x2;
                case "uint1x3": return ShaderValueMatrixType.UInt1x3;
                case "uint2x3": return ShaderValueMatrixType.UInt2x3;
                case "uint3x3": return ShaderValueMatrixType.UInt3x3;
                case "uint4x3": return ShaderValueMatrixType.UInt4x3;
                case "uint1x4": return ShaderValueMatrixType.UInt1x4;
                case "uint2x4": return ShaderValueMatrixType.UInt2x4;
                case "uint3x4": return ShaderValueMatrixType.UInt3x4;
                case "uint4x4": return ShaderValueMatrixType.UInt4x4;

                case "snorm1x2": return ShaderValueMatrixType.SNorm1x2;
                case "snorm2x2": return ShaderValueMatrixType.SNorm2x2;
                case "snorm3x2": return ShaderValueMatrixType.SNorm3x2;
                case "snorm4x2": return ShaderValueMatrixType.SNorm4x2;
                case "snorm1x3": return ShaderValueMatrixType.SNorm1x3;
                case "snorm2x3": return ShaderValueMatrixType.SNorm2x3;
                case "snorm3x3": return ShaderValueMatrixType.SNorm3x3;
                case "snorm4x3": return ShaderValueMatrixType.SNorm4x3;
                case "snorm1x4": return ShaderValueMatrixType.SNorm1x4;
                case "snorm2x4": return ShaderValueMatrixType.SNorm2x4;
                case "snorm3x4": return ShaderValueMatrixType.SNorm3x4;
                case "snorm4x4": return ShaderValueMatrixType.SNorm4x4;

                case "unorm1x2": return ShaderValueMatrixType.UNorm1x2;
                case "unorm2x2": return ShaderValueMatrixType.UNorm2x2;
                case "unorm3x2": return ShaderValueMatrixType.UNorm3x2;
                case "unorm4x2": return ShaderValueMatrixType.UNorm4x2;
                case "unorm1x3": return ShaderValueMatrixType.UNorm1x3;
                case "unorm2x3": return ShaderValueMatrixType.UNorm2x3;
                case "unorm3x3": return ShaderValueMatrixType.UNorm3x3;
                case "unorm4x3": return ShaderValueMatrixType.UNorm4x3;
                case "unorm1x4": return ShaderValueMatrixType.UNorm1x4;
                case "unorm2x4": return ShaderValueMatrixType.UNorm2x4;
                case "unorm3x4": return ShaderValueMatrixType.UNorm3x4;
                case "unorm4x4": return ShaderValueMatrixType.UNorm4x4;

                case "bool1x2": return ShaderValueMatrixType.Bool1x2;
                case "bool2x2": return ShaderValueMatrixType.Bool2x2;
                case "bool3x2": return ShaderValueMatrixType.Bool3x2;
                case "bool4x2": return ShaderValueMatrixType.Bool4x2;
                case "bool1x3": return ShaderValueMatrixType.Bool1x3;
                case "bool2x3": return ShaderValueMatrixType.Bool2x3;
                case "bool3x3": return ShaderValueMatrixType.Bool3x3;
                case "bool4x3": return ShaderValueMatrixType.Bool4x3;
                case "bool1x4": return ShaderValueMatrixType.Bool1x4;
                case "bool2x4": return ShaderValueMatrixType.Bool2x4;
                case "bool3x4": return ShaderValueMatrixType.Bool3x4;
                case "bool4x4": return ShaderValueMatrixType.Bool4x4;
                #endregion
            }

            ShaderValueStructureType structureType;
            if (!structures.TryGetValue(trimmedLine, out structureType))
                throw new InvalidDataException(string.Format("Unknown type '{0}'", trimmedLine));
            return structureType;
        }

        static readonly Regex StructureDeclarationEx = new Regex(@"^struct\s+([_a-zA-Z]\w*)$");

        static string ParseStrcutureDeclaration(string trimmedLine)
        {
            var match = StructureDeclarationEx.Match(trimmedLine);
            if (!match.Success)
                throw new InvalidDataException(string.Format("'{0}' is not a valid structure declaration", trimmedLine));
            return match.Groups[1].Value;
        }

        static Pair<ShaderValueType, string> ParseStructureElementTypeNamePair(string trimmedLine, IDictionary<string, ShaderValueStructureType> structures)
        {
            string modifyersString, typeString, name;
            int? arraySize;
            ParseVariableDeclaration(trimmedLine, out modifyersString, out typeString, out name, out arraySize);

            if (!string.IsNullOrEmpty(modifyersString))
                throw new InvalidDataException("Modifyers are not supported for structure elements");

            var nonArrayType = ParseValueType(typeString, structures);
            var type = arraySize.HasValue ? new ShaderValueArrayType(nonArrayType, arraySize.Value) : nonArrayType;
            return new Pair<ShaderValueType, string>(type, name);
        }

        static readonly Regex UniformBufferDeclarationEx = new Regex(@"^ubuffer\s+([_a-zA-Z]\w*)$");

        static void ParseUniformBufferHeader(string trimmedLine, out string name, out KeyValuePair<string, string>[] specialParameters)
        {
            string declarationString;
            ParseSomethingWithSpecialParameters(trimmedLine, "ubuffer", out declarationString, out specialParameters);
            var match = UniformBufferDeclarationEx.Match(declarationString);
            if (!match.Success)
                throw new InvalidDataException(string.Format("'{0}' is not a valid ubuffer declaration", declarationString));
            name = match.Groups[1].Value;
        }

        static ShaderObjectTypeClass ParseObjectTypeClass(string trimmedLine)
        {
            switch (trimmedLine)
            {
                case "Buffer": return ShaderObjectTypeClass.Buffer;
                case "RWBuffer": return ShaderObjectTypeClass.RWBuffer;
                case "ByteAddressBuffer": return ShaderObjectTypeClass.ByteAddressBuffer;
                case "RWByteAddressBuffer": return ShaderObjectTypeClass.RWByteAddressBuffer;
                case "AppendStructuredBuffer": return ShaderObjectTypeClass.AppendStructuredBuffer;
                case "ConsumeStructuredBuffer": return ShaderObjectTypeClass.ConsumeStructuredBuffer;
                case "RWStructuredBuffer": return ShaderObjectTypeClass.RWStructuredBuffer;
                case "StructuredBuffer": return ShaderObjectTypeClass.StructuredBuffer;
                case "Texture1D": return ShaderObjectTypeClass.Texture1D;
                case "RWTexture1D": return ShaderObjectTypeClass.RWTexture1D;
                case "Texture1DArray": return ShaderObjectTypeClass.Texture1DArray;
                case "RWTexture1DArray": return ShaderObjectTypeClass.RWTexture1DArray;
                case "Texture2D": return ShaderObjectTypeClass.Texture2D;
                case "RWTexture2D": return ShaderObjectTypeClass.RWTexture2D;
                case "Texture2DArray": return ShaderObjectTypeClass.Texture2DArray;
                case "RWTexture2DArray": return ShaderObjectTypeClass.RWTexture2DArray;
                case "Texture2DMS": return ShaderObjectTypeClass.Texture2DMS;
                case "Texture2DMSArray": return ShaderObjectTypeClass.Texture2DMSArray;
                case "TextureCube": return ShaderObjectTypeClass.TextureCube;
                case "TextureCubeArray": return ShaderObjectTypeClass.TextureCubeArray;
                case "Texture3D": return ShaderObjectTypeClass.Texture3D;
                case "RWTexture3D": return ShaderObjectTypeClass.RWTexture3D;
                default: throw new ArgumentOutOfRangeException("trimmedLine", string.Format("Unknown shader object type '{0}'", trimmedLine));
            }
        }

        readonly static Regex SamplerEx = new Regex(@"^sampler\s+([_a-zA-Z]\w*)$");

        static ShaderSamplerVariable ParseSampler(string trimmedLine)
        {
            string declarationString;
            KeyValuePair<string, string>[] specialParameters;
            ParseSomethingWithSpecialParameters(trimmedLine, "sampler", out declarationString, out specialParameters);
            var match = SamplerEx.Match(declarationString);
            if (!match.Success)
                throw new InvalidDataException(string.Format("'{0}' is not a valid sampler declaration", declarationString));
            return new ShaderSamplerVariable(match.Groups[1].Value, specialParameters);
        }

        readonly static Regex ObjectTypeEx = new Regex(@"^([_a-zA-Z]\w*)(\s*<\s*([_a-zA-Z]\w*)\s*(,\s*(\d+))?\s*>)?$");

        static ShaderObjectType ParseObjectType(string trimmedLine, IDictionary<string, ShaderValueStructureType> structures)
        {
            var match = ObjectTypeEx.Match(trimmedLine);
            if (!match.Success)
                throw new InvalidDataException(string.Format("'{0}' is not a valid shader object type declaration", trimmedLine));
            if (string.IsNullOrEmpty(match.Groups[2].Value))
                return new ShaderObjectType(ParseObjectTypeClass(match.Groups[1].Value));
            if (string.IsNullOrEmpty(match.Groups[4].Value))
                return new ShaderObjectType(ParseObjectTypeClass(match.Groups[1].Value), ParseValueType(match.Groups[3].Value, structures));
            return new ShaderObjectType(ParseObjectTypeClass(match.Groups[1].Value), ParseValueType(match.Groups[3].Value, structures), int.Parse(match.Groups[5].Value));
        }

        static ShaderObjectVariable ParseObjectVariable(string trimmedLine, IDictionary<string, ShaderValueStructureType> structures)
        {
            string declarationString;
            KeyValuePair<string, string>[] specialParameters;
            ParseSomethingWithSpecialParameters(trimmedLine, "shader object", out declarationString, out specialParameters);

            string modifyersString, typeString, name;
            int? arraySize;
            ParseVariableDeclaration(declarationString, out modifyersString, out typeString, out name, out arraySize);

            if (!string.IsNullOrEmpty(modifyersString))
                throw new InvalidDataException("Modifyers are not supported for object variables");
            if (arraySize.HasValue)
                throw new InvalidDataException("Object variable arrays are not supported");

            return new ShaderObjectVariable(ParseObjectType(typeString, structures), name, specialParameters);
        }

        static ShaderIOVariableInterpolation ParseInterpolation(string interpolationString)
        {
            switch (interpolationString)
            {
                case "default": return ShaderIOVariableInterpolation.Default;
                case "nointerpolation": return ShaderIOVariableInterpolation.NoInterpolation;
                case "noperspective": return ShaderIOVariableInterpolation.NoPerspective;
                case "centroid": return ShaderIOVariableInterpolation.Centroid;
                case "sample": return ShaderIOVariableInterpolation.Sample;
                default: throw new ArgumentOutOfRangeException("interpolationString", string.Format("'{0}' is not a valid shader IO interpolation modifyer", interpolationString));
            }
        }

        static readonly char[] InterpolationSeparators = new[] { ' ', '\t', '\r', '\n' };

        static ShaderIOVariable ParseIOVariable(string trimmedLine, IDictionary<string, ShaderValueStructureType> structures)
        {
            string declarationString;
            KeyValuePair<string, string>[] specialParameters;
            ParseSomethingWithSpecialParameters(trimmedLine, "shader IO variable", out declarationString, out specialParameters);

            string modifyersString, typeString, name;
            int? arraySize;
            ParseVariableDeclaration(declarationString, out modifyersString, out typeString, out name, out arraySize);

            if (arraySize.HasValue)
                throw new InvalidDataException("IO variable arrays are not supported");

            var interpolationStrings = modifyersString.Split(InterpolationSeparators, StringSplitOptions.RemoveEmptyEntries);
            var interpolation = interpolationStrings.Aggregate(ShaderIOVariableInterpolation.Default, (current, interpolationString) => current | ParseInterpolation(interpolationString));

            var type = ParseValueType(typeString, structures);

            return new ShaderIOVariable(interpolation, type, name, specialParameters);
        }

        static readonly Regex FixedSamplingPairEx = new Regex(@"^([_a-zA-Z]\w*)\s*:\s*([_a-zA-Z]\w*)$");

        static ShaderFixedSamplingPair ParseFixedSamplingPair(string trimmedLine)
        {
            var match = FixedSamplingPairEx.Match(trimmedLine);
            if (!match.Success)
                throw new InvalidDataException(string.Format("'{0}' is not a valid fixed sampling declaration", trimmedLine));

            return new ShaderFixedSamplingPair(match.Groups[1].Value, match.Groups[2].Value);
        }
    }
}
