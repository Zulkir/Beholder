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
using System.Text;
using Beholder.Shaders;
using Beholder.Utility;

namespace Beholder.Libraries.ObjectGL4.Shaders
{
    static class ShaderMacros
    {
        [Flags]
        enum T
        {
            Float = 0x1,
            Double = 0x2,
            Int = 0x4,
            Uint = 0x8,
            Bool = 0x10,
            All = Float | Double | Int | Uint | Bool
        }

        public static readonly string VertexText;
        public static readonly string HullText;
        public static readonly string DomainText;
        public static readonly string GeometryText;
        public static readonly string PixelText;
        public static readonly string ComputeText;

        public static string GetText(ShaderStage stage)
        {
            switch (stage)
            {
                case ShaderStage.Vertex: return VertexText;
                case ShaderStage.Pixel: return PixelText;
                case ShaderStage.Geometry: return GeometryText;
                case ShaderStage.Hull: return HullText;
                case ShaderStage.Domain: return DomainText;
                case ShaderStage.Compute: return ComputeText;
                default: throw new ArgumentOutOfRangeException("stage");
            }
        }
        
        static ShaderMacros()
        {
            VertexText = GenerateMacros(ShaderStage.Vertex);
            HullText = GenerateMacros(ShaderStage.Hull);
            DomainText = GenerateMacros(ShaderStage.Domain);
            GeometryText = GenerateMacros(ShaderStage.Geometry);
            PixelText = GenerateMacros(ShaderStage.Pixel);
            ComputeText = GenerateMacros(ShaderStage.Compute);
        }

        static string GenerateMacros(ShaderStage stage)
        {
            var builder = new StringBuilder(1 << 14);

            #region General
            builder.AppendLine(@"
#define OGL
#define OGL3
#define OGL4

#define static");
            #endregion

            #region Types
            Vectors(T.Float, builder, "#define float{1} {0}");
            Vectors(T.Double, builder, "#define double{1} {0}");
            Vectors(T.Int, builder, "#define int{1} {0}");
            Vectors(T.Uint, builder, "#define uint{1} {0}");
            Vectors(T.Bool, builder, "#define bool{1} {0}");

            Matrices(T.Float, builder, "#define float{1}x{2} {0}");
            Matrices(T.Double, builder, "#define double{1}x{2} {0}");
            #endregion

            #region all
            Scalars(T.Float | /*T.Double |*/ T.Int | T.Uint, builder, type => 
                string.Format("bool all({0} arg) {{ return arg != {0}(0); }}", type));

            AggregateVectors(T.Float | /*T.Double |*/ T.Int | T.Uint, builder,
                type => string.Format("bool all({0} arg) {{ return", type),
                type => string.Format(" arg.x != {0}(0)", GetBaseType(type)),
                (type, i) => string.Format(" && arg.{0} != {1}(0)", VectorElement(i), GetBaseType(type)),
                type => "; }");
            /*
            AggregateMatrices(T.Float | T.Double, builder,
                type => string.Format("bool all({0} arg) {{ return", type),
                type => " arg[0][0] != 0.0",
                (type, r, c) => string.Format(" && arg[{0}][{1}] != 0.0", r, c),
                type => "; }");*/
            #endregion

            #region any
            Scalars(T.Float | /*T.Double |*/ T.Int | T.Uint, builder, type =>
                string.Format("bool any({0} arg) {{ return arg != {0}(0); }}", type));

            AggregateVectors(T.Float | /*T.Double |*/ T.Int | T.Uint, builder,
                type => string.Format("bool any({0} arg) {{ return", type),
                type => string.Format(" arg.x != {0}(0)", GetBaseType(type)),
                (type, i) => string.Format(" || arg.{0} != {1}(0)", VectorElement(i), GetBaseType(type)),
                type => "; }");
            /*
            AggregateMatrices(T.Float | T.Double, builder,
                type => string.Format("bool any({0} arg) {{ return", type),
                type => " arg[0][0] != 0.0",
                (type, r, c) => string.Format(" || arg[{0}][{1}] != 0.0", r, c),
                type => "; }");*/
            #endregion

            #region atan2
            builder.AppendLine("#define atan2(Y, X) atan(Y, X)");
            #endregion

            #region clip
            if (stage == ShaderStage.Pixel)
            {
                builder.AppendLine("void clip(float arg) { if (arg < 0.0) { discard; } }");

                AggregateVectors(T.Float, builder,
                    type => string.Format("void clip({0} arg) {{ if (", type),
                    type => "arg.x < 0.0",
                    (type, i) => string.Format(" || arg.{0} < 0.0", VectorElement(i)),
                    type => ") { discard; } }");
                /*
                AggregateMatrices(T.Float, builder,
                    type => string.Format("void clip({0} arg) {{ if (", type),
                    type => "arg[0][0] < 0.0",
                    (type, r, c) => string.Format(" || arg[{0}][{1}] < 0.0", r, c),
                    type => ") { discard; } }");*/
            }
            #endregion

            #region ddx ddy
            if (stage == ShaderStage.Pixel)
            {
                builder.AppendLine("#define ddx dFdx");
                builder.AppendLine("#define ddy dFdy");
            }
            #endregion

            #region dot
            AggregateVectors(T.Int, builder,
                type => string.Format("int dot({0} arg1, {0} arg2) {{ return ", type),
                type => "arg1.x * arg2.x",
                (type, i) => string.Format(" + arg1.{0} * arg2.{0}", VectorElement(i)),
                type => "; }");
            #endregion

            #region fmod
            builder.AppendLine("#define fmod(X, Y) (X - Y * trunc(X / Y))");
            #endregion

            #region frac
            builder.AppendLine("#define frac(X) fract(X)");
            #endregion

            #region frexp
            builder.AppendLine("float frexp(float x, out float e) { float absx = abs(x); e = ceil(log2(absx)); return absx * exp2(-e); }");
            Vectors(T.Float, builder, "{0} frexp({0} x, out {0} e) {{ {0} absx = abs(x); e = ceil(log2(absx)); return absx * exp2(-e); }}");
            //Matrices(T.Float, builder, "{0} frexp({0} x, out {0} e) {{ {0} absx = abs(x); e = ceil(log2(absx)); return absx * exp2(-e); }}");
            #endregion

            #region GetRenderTargetSampleCount
            if (stage == ShaderStage.Pixel)
            {
                builder.AppendLine("#define GetRenderTargetSampleCount() gl_NumSamples");
                //builder.AppendLine("uint GetRenderTargetSampleCount() { return gl_NumSamples; }");
            }
            #endregion

            #region GetRenderTargetSamplePosition
            if (stage == ShaderStage.Pixel)
            {
                builder.AppendLine("#define GetRenderTargetSamplePosition() gl_SamplePosition ");
                //builder.AppendLine("uint GetRenderTargetSampleCount() { return gl_NumSamples; }");
            }
            #endregion

            #region isfinite
            builder.AppendLine("#define isfinite(X) (!isinf(X))");
            #endregion

            #region ldexp
            builder.AppendLine("float ldexp(float x, float e) { return x * exp2(e); } ");
            Vectors(T.Float, builder, "{0} ldexp({0} x, {0} e) {{ return x * exp2(e); }}");
            //Matrices(T.Float, builder, "{0} ldexp({0} x, {0} e) {{ return x * exp2(e); }}");
            #endregion

            #region lerp
            builder.AppendLine("#define lerp mix");
            #endregion

            #region lit
            builder.AppendLine("float4 lit(float n_dot_l, float n_dot_h, float m) { float diffuse = max(0.0, n_dot_l); float specular; if (diffuse < 0.0) specular = 0.0; else specular = pow(n_dot_h, m); return float4(1.0, diffuse, specular, 1.0); } ");
            #endregion

            #region log10
            builder.AppendLine("#define log10(X) (log2(X) / log2(10.0))");
            #endregion

            #region modf
            builder.AppendLine("float modf(float x, out int ip) { float ipf; float result = modf(x, ipf); ip = int(ipf); return result; }");
            Vectors(T.Float, builder, (type, size) => string.Format("{0} modf({0} x, out int{1} ip) {{ {0} ipf; {0} result = modf(x, ipf); ip = int{1}(ipf); return result; }}", type, size));
            #endregion

            #region mul
            builder.AppendLine("#define mul(A, B) (A * B)");
            #endregion

            #region noise
            //builder.AppendLine("#define noise noise1");
            #endregion

            #region rsqrt
            builder.AppendLine("#define rsqrt inversesqrt");
            #endregion

            #region sqrt
            builder.AppendLine("#define saturate(x) clamp(x, 0.0, 1.0)");
            #endregion

            #region sincos
            builder.AppendLine("void sincos(float x, out float s, out float c) { s = sin(x); c = cos(x); }");
            Vectors(T.Float, builder, "void sincos({0} x, out {0} s, out {0} c) {{ s = sin(x); c = cos(x); }}");
            #endregion

            #region Textures
            builder.AppendLine(@"
#define calculateLevelOfDetail(T, C) textureQueryLod(T, C).x
#define gather(T, C) textureGather(T, C)
#define gatherOffset(T, C, O) textureGatherOffset(T, C, O)
#define gatherCmp(T, C, V) textureGather(T, C, V)
#define gatherCmpOffset(T, C, V, O) textureGatherOffset(T, C, V, O)
#define getSize(T) textureSize(T)
#define BS_TEXTURE_LOAD_DIMENSION(T) BS_TEXTURE_LOAD_DIMENSION_##T
#define load(T, C) BS_TEXTURE_LOAD(BS_TEXTURE_LOAD_DIMENSION(T), T, C)
#define BS_TEXTURE_LOAD(D, T, C) BS_TEXTURE_LOAD_##D(T, C)
#define BS_TEXTURE_LOAD_2(T, C) texelFetch(T, C.x, C.y)
#define BS_TEXTURE_LOAD_3(T, C) texelFetch(T, C.xy, C.z)
#define BS_TEXTURE_LOAD_4(T, C) texelFetch(T, C.xyz, C.w)
#define loadOffset(T, C, O) BS_TEXTURE_LOAD_OFFSET(BS_TEXTURE_LOAD_DIMENSION(T), T, C, O)
#define BS_TEXTURE_LOAD_OFFSET(D, T, C, O) BS_TEXTURE_LOAD_OFFSET_##D(T, C, O) 
#define BS_TEXTURE_LOAD_OFFSET_2(T, C, O) texelFetchOffset(T, C.x, C.y, O)
#define BS_TEXTURE_LOAD_OFFSET_3(T, C, O) texelFetchOffset(T, C.xy, C.z, O)
#define BS_TEXTURE_LOAD_OFFSET_4(T, C, O) texelFetchOffset(T, C.xyz, C.w, O)
#define loadSample(T, C, S) texelFetch(T, C, S)
#define sample(T, C) texture(T, C)
#define sampleOffset(T, C, O) textureOffset(T, C, O)
#define sampleBias(T, C, B) texture(T, C, B)
#define sampleBiasOffset(T, C, B, O) textureOffset(T, C, O, B)
#define BS_TEXTURE_SAMPLE_CMP_DIMENSION(T) BS_TEXTURE_SAMPLE_CMP_DIMENSION_##T
#define sampleCmp(T, C, V) BS_TEXTURE_SAMPLE_CMP(BS_TEXTURE_SAMPLE_CMP_DIMENSION(T), T, C, V)
#define BS_TEXTURE_SAMPLE_CMP(D, T, C, V) BS_TEXTURE_SAMPLE_CMP_##D(T, C, V)
#define BS_TEXTURE_SAMPLE_CMP_1(T, C, V) uint(texture(t, float3(l, 0.0, v)))
#define BS_TEXTURE_SAMPLE_CMP_2(T, C, V) uint(texture(t, float3(l, v)))
#define BS_TEXTURE_SAMPLE_CMP_3(T, C, V) uint(texture(t, float4(l, v)))
#define BS_TEXTURE_SAMPLE_CMP_4(T, C, V) uint(texture(t, l, v))
#define sampleCmpOffset(T, C, V, O) BS_TEXTURE_SAMPLE_CMP_OFFSET(BS_TEXTURE_SAMPLE_CMP_DIMENSION(T), T, C, V, O)
#define BS_TEXTURE_SAMPLE_CMP_OFFSET(D, T, C, V, O) BS_TEXTURE_SAMPLE_CMP_OFFSET_##D(T, C, V, O)
#define BS_TEXTURE_SAMPLE_CMP_OFFSET_1(T, C, V, O) uint(textureOffset(T, float3(C, 0.0, V), O)))
#define BS_TEXTURE_SAMPLE_CMP_OFFSET_2(T, C, V, O) uint(textureOffset(T, float3(C, V), O))
#define BS_TEXTURE_SAMPLE_CMP_OFFSET_3(T, C, V, O) uint(textureOffset(T, float4(C, V), O))
#define sampleCmpLevelZero(T, C, V) BS_TEXTURE_SAMPLE_CMP_LEVEL_ZERO(BS_TEXTURE_SAMPLE_CMP_DIMENSION(T), T, C, V)
#define BS_TEXTURE_SAMPLE_CMP_LEVEL_ZERO(D, T, C, V) BS_TEXTURE_SAMPLE_CMP_LEVEL_ZERO_##D(T, C, V)
#define BS_TEXTURE_SAMPLE_CMP_LEVEL_ZERO_1(T, C, V) uint(texture(t, float3(l, 0.0, v)))
#define BS_TEXTURE_SAMPLE_CMP_LEVEL_ZERO_2(T, C, V) uint(texture(t, float3(l, v)))
#define BS_TEXTURE_SAMPLE_CMP_LEVEL_ZERO_3(T, C, V) uint(texture(t, float4(l, v)))
#define BS_TEXTURE_SAMPLE_CMP_LEVEL_ZERO_4(T, C, V) uint(texture(t, l, v))
#define sampleCmpLevelZeroOffset(T, C, V, O) BS_TEXTURE_SAMPLE_CMP_LEVEL_ZERO_OFFSET(BS_TEXTURE_SAMPLE_CMP_DIMENSION(T), T, C, V, O)
#define BS_TEXTURE_SAMPLE_CMP_LEVEL_ZERO_OFFSET(D, T, C, V, O) BS_TEXTURE_SAMPLE_CMP_LEVEL_ZERO_OFFSET_##D(T, C, V, O)
#define BS_TEXTURE_SAMPLE_CMP_LEVEL_ZERO_OFFSET_1(T, C, V, O) uint(textureLodOffset(T, float3(C, 0.0, V), 0.0, O))
#define BS_TEXTURE_SAMPLE_CMP_LEVEL_ZERO_OFFSET_2(T, C, V, O) uint(textureLodOffset(T, float3(C, V), 0.0, O))
#define sampleGrad(T, C, X, Y) textureGrad(T, C, X, Y)
#define sampleGradOffset(T, C, X, Y, O) textureGradOffset(T, C, X, Y, O)
#define sampleLod(T, C, L) textureLod(T, C, L)
#define sampleLodOffset(T, C, L, O) textureLodOffset(T, C, L, O)
");
            #endregion

            return builder.ToString();
        }

        static string GetBaseType(string type)
        {
            switch (type)
            {
                case "float": return "float";
                case "double": return "double";
                case "int": return "int";
                case "uint": return "uint";
                case "bool": return "bool";
            }
            if (type.StartsWith("vec") || type.StartsWith("mat"))
                return "float";
            if (type.StartsWith("dvec") || type.StartsWith("dmat"))
                return "double";
            if (type.StartsWith("ivec"))
                return "int";
            if (type.StartsWith("uvec"))
                return "uint";
            if (type.StartsWith("bvec"))
                return "bool";

            throw new ArgumentOutOfRangeException("type");
        }

        static string VectorElement(int index)
        {
            switch (index)
            {
                case 0: return "x";
                case 1: return "y";
                case 2: return "z";
                case 3: return "w";
                default: throw new ShouldNeverHappenException();
            }
        }

        static void Scalars(T types, StringBuilder builder, Func<string, string> fromType)
        {
            if (types.HasFlag(T.Float)) builder.AppendLine(fromType("float"));
            if (types.HasFlag(T.Double)) builder.AppendLine(fromType("double"));
            if (types.HasFlag(T.Int)) builder.AppendLine(fromType("int"));
            if (types.HasFlag(T.Uint)) builder.AppendLine(fromType("uint"));
            if (types.HasFlag(T.Bool)) builder.AppendLine(fromType("bool"));
        }

        static void Scalars(T types, StringBuilder builder, string template)
        {
            Scalars(types, builder, t => string.Format(template, t));
        }

        static void Vectors(T types, StringBuilder builder, Func<string, int, string> fromTypeDimension)
        {
            for (int i = 2; i <= 4; i++)
            {
                if (types.HasFlag(T.Float)) builder.AppendLine(fromTypeDimension("vec" + i, i));
                if (types.HasFlag(T.Double)) builder.AppendLine(fromTypeDimension("dvec" + i, i));
                if (types.HasFlag(T.Int)) builder.AppendLine(fromTypeDimension("ivec" + i, i));
                if (types.HasFlag(T.Uint)) builder.AppendLine(fromTypeDimension("uvec" + i, i));
                if (types.HasFlag(T.Bool)) builder.AppendLine(fromTypeDimension("bvec" + i, i));
            }
        }

        static void Vectors(T types, StringBuilder builder, string template)
        {
            Vectors(types, builder, (t, d) => string.Format(template, t, d));
        }

        static void Matrices(T types, StringBuilder builder, Func<string, int, int, string> fromTypeDimension)
        {
            for (int r = 2; r <= 4; r++ )
                for (int c = 2; c <= 4; c++)
                {
                    if (types.HasFlag(T.Float)) builder.AppendLine(fromTypeDimension("mat" + r + "x" + c, r, c));
                    if (types.HasFlag(T.Double)) builder.AppendLine(fromTypeDimension("dmat" + r + "x" + c, r, c));
                }
        }

        static void Matrices(T types, StringBuilder builder, string template)
        {
            Matrices(types, builder, (t, r, c) => string.Format(template, t, r, c));
        }

        static void AggregateVectors(T types, StringBuilder builder, 
            Func<string, string> start, Func<string, string> first, 
            Func<string, int, string> additional, Func<string, string> finish)
        {
            Vectors(types, builder, (type, size) =>
            {
                var localBuilder = new StringBuilder();
                localBuilder.Append(start(type));
                localBuilder.Append(first(type));
                for (int i = 1; i < size; i++)
                    localBuilder.Append(additional(type, i));
                localBuilder.Append(finish(type));
                return localBuilder.ToString();
            });
        }

        static void AggregateMatrices(T types, StringBuilder builder, 
            Func<string, string> start, Func<string, string> first, 
            Func<string, int, int, string> additional, Func<string, string> finish)
        {
            Matrices(types, builder, (type, rSize, cSize) =>
            {
                var localBuilder = new StringBuilder();
                localBuilder.Append(start(type));
                localBuilder.Append(first(type));
                for (int r = 0; r < rSize; r++)
                    for (int c = r == 0 ? 1 : 0; c < cSize; c++)
                        localBuilder.Append(additional(type, r, c));
                localBuilder.Append(finish(type));
                return localBuilder.ToString();
            });
        }

        static void GSampler(StringBuilder builder, string template, string glslType)
        {
            builder.AppendLine(string.Format(template, string.Format("{0}", glslType), "vec4"));
            builder.AppendLine(string.Format(template, string.Format("i{0}", glslType), "ivec4"));
            builder.AppendLine(string.Format(template, string.Format("u{0}", glslType), "uvec4"));
        }
    }
}
