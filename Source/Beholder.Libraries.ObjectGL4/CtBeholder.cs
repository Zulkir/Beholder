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
using Beholder.Core;
using Beholder.Platform;
using Beholder.Utility.ForImplementations.Platform;
using ObjectGL.GL42;
using OpenTK.Graphics.OpenGL;

namespace Beholder.Libraries.ObjectGL4
{
    static class CtBeholder
    {
        #region Format
        static FormatInfo ExplicitTexelFormatInfo(Format glFormat, ExplicitFormat bFormat)
        {
            int numColors;
            int colorBits;
            FormatElementType colorFormatType;
            int alphaBits;
            FormatElementType alphaFormatType;
            int totalBits;
            ColorAlphaFormatFlags flags;

            Utility.Helpers.FormatHelper.GetExplicitColorAlphaFormatInfo(bFormat,
                out numColors, out colorBits, out colorFormatType, out alphaBits, out alphaFormatType, out totalBits, out flags);

            return new FormatInfo
            {
                ID = (int)glFormat,
                Description = glFormat.ToString(),
                ExplicitFormat = bFormat,
                NumColors = numColors,
                ColorBits = colorBits,
                ColorFormatType = colorFormatType,
                AlphaBits = alphaBits,
                AlphaFormatType = alphaFormatType,
                TotalBits = totalBits,
                Flags = flags
            };
        }

        public static FormatInfo FormatInfo(Format glFormat)
        {
            switch (glFormat)
            {
                case Format.Rgba32f: return ExplicitTexelFormatInfo(glFormat, ExplicitFormat.R32G32B32A32_FLOAT);
                case Format.Rgba32ui: return ExplicitTexelFormatInfo(glFormat, ExplicitFormat.R32G32B32A32_UINT);
                case Format.Rgba32i: return ExplicitTexelFormatInfo(glFormat, ExplicitFormat.R32G32B32A32_SINT);

                case Format.Rgb32f: return ExplicitTexelFormatInfo(glFormat, ExplicitFormat.R32G32B32_FLOAT);
                case Format.Rgb32ui: return ExplicitTexelFormatInfo(glFormat, ExplicitFormat.R32G32B32_UINT);
                case Format.Rgb32i: return ExplicitTexelFormatInfo(glFormat, ExplicitFormat.R32G32B32_SINT);

                case Format.Rgba16f: return ExplicitTexelFormatInfo(glFormat, ExplicitFormat.R16G16B16A16_FLOAT);
                case Format.Rgba16: return ExplicitTexelFormatInfo(glFormat, ExplicitFormat.R16G16B16A16_UNORM);
                case Format.Rgba16ui: return ExplicitTexelFormatInfo(glFormat, ExplicitFormat.R16G16B16A16_UINT);
                case Format.Rgba16sn: return ExplicitTexelFormatInfo(glFormat, ExplicitFormat.R16G16B16A16_SNORM);
                case Format.Rgba16i: return ExplicitTexelFormatInfo(glFormat, ExplicitFormat.R16G16B16A16_SINT);

                case Format.Rg32f: return ExplicitTexelFormatInfo(glFormat, ExplicitFormat.R32G32_FLOAT);
                case Format.Rg32ui: return ExplicitTexelFormatInfo(glFormat, ExplicitFormat.R32G32_UINT);
                case Format.Rg32i: return ExplicitTexelFormatInfo(glFormat, ExplicitFormat.R32G32_SINT);

                case Format.Depth32fStencil8: return ExplicitTexelFormatInfo(glFormat, ExplicitFormat.D32_FLOAT_S8X24_UINT);

                case Format.Rgb10A2: return ExplicitTexelFormatInfo(glFormat, ExplicitFormat.R10G10B10A2_UNORM);
                case Format.Rgb10A2ui: return ExplicitTexelFormatInfo(glFormat, ExplicitFormat.R10G10B10A2_UINT);

                case Format.R11fG11fB10f: return ExplicitTexelFormatInfo(glFormat, ExplicitFormat.R11G11B10_FLOAT);

                case Format.Rgba8: return ExplicitTexelFormatInfo(glFormat, ExplicitFormat.R8G8B8A8_UNORM);
                case Format.Srgb8Alpha8: return ExplicitTexelFormatInfo(glFormat, ExplicitFormat.R8G8B8A8_UNORM_SRGB);
                case Format.Rgba8ui: return ExplicitTexelFormatInfo(glFormat, ExplicitFormat.R8G8B8A8_UINT);
                case Format.Rgba8sn: return ExplicitTexelFormatInfo(glFormat, ExplicitFormat.R8G8B8A8_SNORM);
                case Format.Rgba8i: return ExplicitTexelFormatInfo(glFormat, ExplicitFormat.R8G8B8A8_SINT);

                case Format.Rg16f: return ExplicitTexelFormatInfo(glFormat, ExplicitFormat.R16G16_FLOAT);
                case Format.Rg16: return ExplicitTexelFormatInfo(glFormat, ExplicitFormat.R16G16_UNORM);
                case Format.Rg16ui: return ExplicitTexelFormatInfo(glFormat, ExplicitFormat.R16G16_UINT);
                case Format.Rg16sn: return ExplicitTexelFormatInfo(glFormat, ExplicitFormat.R16G16_SNORM);
                case Format.Rg16i: return ExplicitTexelFormatInfo(glFormat, ExplicitFormat.R16G16_SINT);

                case Format.DepthComponent32f: return ExplicitTexelFormatInfo(glFormat, ExplicitFormat.D32_FLOAT);

                case Format.R32f: return ExplicitTexelFormatInfo(glFormat, ExplicitFormat.R32_FLOAT);
                case Format.R32ui: return ExplicitTexelFormatInfo(glFormat, ExplicitFormat.R32_UINT);
                case Format.R32i: return ExplicitTexelFormatInfo(glFormat, ExplicitFormat.R32_SINT);

                case Format.Depth24Stencil8: return ExplicitTexelFormatInfo(glFormat, ExplicitFormat.D24_UNORM_S8_UINT);

                case Format.Rg8: return ExplicitTexelFormatInfo(glFormat, ExplicitFormat.R8G8_UNORM);
                case Format.Rg8ui: return ExplicitTexelFormatInfo(glFormat, ExplicitFormat.R8G8_UINT);
                case Format.Rg8sn: return ExplicitTexelFormatInfo(glFormat, ExplicitFormat.R8G8_SNORM);
                case Format.Rg8i: return ExplicitTexelFormatInfo(glFormat, ExplicitFormat.R8G8_SINT);

                case Format.DepthComponent16: return ExplicitTexelFormatInfo(glFormat, ExplicitFormat.D16_UNORM);

                case Format.R16f: return ExplicitTexelFormatInfo(glFormat, ExplicitFormat.R16_FLOAT);
                case Format.R16: return ExplicitTexelFormatInfo(glFormat, ExplicitFormat.R16_UNORM);
                case Format.R16ui: return ExplicitTexelFormatInfo(glFormat, ExplicitFormat.R16_UINT);
                case Format.R16sn: return ExplicitTexelFormatInfo(glFormat, ExplicitFormat.R16_SNORM);
                case Format.R16i: return ExplicitTexelFormatInfo(glFormat, ExplicitFormat.R16_SINT);
                //case PixelInternalFormat.Alpha8: return ExplicitTexelFormatInfo(glFormat, ExplicitFormat.A8_UNORM);

                case Format.R8: return ExplicitTexelFormatInfo(glFormat, ExplicitFormat.R8_UNORM);
                case Format.R8ui: return ExplicitTexelFormatInfo(glFormat, ExplicitFormat.R8_UINT);
                case Format.R8sn: return ExplicitTexelFormatInfo(glFormat, ExplicitFormat.R8_SNORM);
                case Format.R8i: return ExplicitTexelFormatInfo(glFormat, ExplicitFormat.R8_SINT);

                case Format.Rgb9E5: return ExplicitTexelFormatInfo(glFormat, ExplicitFormat.R9G9B9E5_SHAREDEXP);

                // todo: r8g8_b8g8
                // todo: g8r8_g8b8

                case Format.CompressedRgbaS3tcDxt1Ext: return ExplicitTexelFormatInfo(glFormat, ExplicitFormat.BC1_UNORM);
                case Format.CompressedSrgbAlphaS3tcDxt1Ext: return ExplicitTexelFormatInfo(glFormat, ExplicitFormat.BC1_UNORM_SRGB);
                case Format.CompressedRgbaS3tcDxt3Ext: return ExplicitTexelFormatInfo(glFormat, ExplicitFormat.BC2_UNORM);
                case Format.CompressedSrgbAlphaS3tcDxt3Ext: return ExplicitTexelFormatInfo(glFormat, ExplicitFormat.BC2_UNORM_SRGB);
                case Format.CompressedRgbaS3tcDxt5Ext: return ExplicitTexelFormatInfo(glFormat, ExplicitFormat.BC3_UNORM);
                case Format.CompressedSrgbAlphaS3tcDxt5Ext: return ExplicitTexelFormatInfo(glFormat, ExplicitFormat.BC3_UNORM_SRGB);
                case Format.CompressedRedRgtc1: return ExplicitTexelFormatInfo(glFormat, ExplicitFormat.BC4_UNORM);
                case Format.CompressedSignedRedRgtc1: return ExplicitTexelFormatInfo(glFormat, ExplicitFormat.BC4_SNORM);
                case Format.CompressedRgRgtc2: return ExplicitTexelFormatInfo(glFormat, ExplicitFormat.BC5_UNORM);
                case Format.CompressedSignedRgRgtc2: return ExplicitTexelFormatInfo(glFormat, ExplicitFormat.BC5_SNORM);
                case Format.CompressedRgbaBptcUf: return ExplicitTexelFormatInfo(glFormat, ExplicitFormat.BC6H_UF16);
                case Format.CompressedRgbaBptcSf: return ExplicitTexelFormatInfo(glFormat, ExplicitFormat.BC6H_SF16);
                case Format.CompressedRgbaBptc: return ExplicitTexelFormatInfo(glFormat, ExplicitFormat.BC7_UNORM);
                case Format.CompressedSrgbAlphaBptc: return ExplicitTexelFormatInfo(glFormat, ExplicitFormat.BC7_UNORM_SRGB);
                    /*
                case Format.DepthComponent24: return new CFormatInfo
                {
                    
                };*/

                default: throw new ArgumentOutOfRangeException("glFormat");
            }
        }
        #endregion

        #region PrimitiveTopology
        public static PrimitiveTopology PrimitiveTopology(BeginMode glBeginMode, int patchVertexCount)
        {
            switch (glBeginMode)
            {
                case BeginMode.Points: return Beholder.Core.PrimitiveTopology.PointList;
                case BeginMode.Lines: return Beholder.Core.PrimitiveTopology.LineList;
                case BeginMode.LineStrip: return Beholder.Core.PrimitiveTopology.LineStrip;
                case BeginMode.Triangles: return Beholder.Core.PrimitiveTopology.TriangleList;
                case BeginMode.TriangleStrip: return Beholder.Core.PrimitiveTopology.TriangleStrip;
                case BeginMode.LinesAdjacency: return Beholder.Core.PrimitiveTopology.LineListWithAdjacency;
                case BeginMode.LineStripAdjacency: return Beholder.Core.PrimitiveTopology.LineStripWithAdjacency;
                case BeginMode.TrianglesAdjacency: return Beholder.Core.PrimitiveTopology.TriangleListWithAdjacency;
                case BeginMode.TriangleStripAdjacency: return Beholder.Core.PrimitiveTopology.TriangleStripWithAdjacency;
                case BeginMode.Patches: return Beholder.Core.PrimitiveTopology.PatchList1 + patchVertexCount - 1;
                default: throw new ArgumentOutOfRangeException("glBeginMode");
            }
        }
        #endregion

        #region ArrayIndex
        public static int ArrayIndex(CubemapFace cubemapFace)
        {
            switch (cubemapFace)
            {
                case CubemapFace.PositiveX: return 0;
                case CubemapFace.NegativeX: return 1;
                case CubemapFace.PositiveY: return 2;
                case CubemapFace.NegativeY: return 3;
                case CubemapFace.PositiveZ: return 4;
                case CubemapFace.NegativeZ: return 5;
                default: throw new ArgumentOutOfRangeException("cubemapFace");
            }
        }
        #endregion
    }
}
