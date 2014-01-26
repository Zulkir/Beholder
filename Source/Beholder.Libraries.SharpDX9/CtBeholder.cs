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
using Beholder.Core;
using Beholder.Platform;
using Beholder.Utility.ForImplementations.Platform;
using SharpDX.Direct3D9;

namespace Beholder.Libraries.SharpDX9
{
    public static class CtBeholder
    {
        #region Format
        static FormatInfo ExplicitFormatInfo(Format d3dFormat, ExplicitFormat bFormat)
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
                ID = (int)d3dFormat,
                Description = d3dFormat.ToString(),
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

        public static IFormatInfo DisplayPixelFormatInfo(Format d3dFormat)
        {
            var formatInfo = new FormatInfo
                                 {
                ID = (int)d3dFormat,
                NumColors = 3,
                ColorFormatType = FormatElementType.UNORM,
                Description = d3dFormat.ToString(),
                Flags = ColorAlphaFormatFlags.None
            };

            switch (d3dFormat)
            {
                case Format.A2R10G10B10:
                    {
                        formatInfo.ColorBits = 30;
                        formatInfo.AlphaBits = 2;
                        formatInfo.AlphaFormatType = FormatElementType.UNORM;
                        formatInfo.TotalBits = 32;
                    }
                    break;
                case Format.X8R8G8B8:
                    {
                        formatInfo.ColorBits = 24;
                        formatInfo.AlphaBits = 0;
                        formatInfo.AlphaFormatType = FormatElementType.UNKNOWN;
                        formatInfo.TotalBits = 32;
                    }
                    break;
                case Format.R5G6B5:
                    {
                        formatInfo.ColorBits = 16;
                        formatInfo.AlphaBits = 0;
                        formatInfo.AlphaFormatType = FormatElementType.UNKNOWN;
                        formatInfo.TotalBits = 16;
                        formatInfo.ExplicitFormat = ExplicitFormat.B5G6R5_UNORM;
                    }
                    break;
                case Format.X1R5G5B5:
                    {
                        formatInfo.ColorBits = 15;
                        formatInfo.AlphaBits = 0;
                        formatInfo.AlphaFormatType = FormatElementType.UNKNOWN;
                        formatInfo.TotalBits = 16;
                    }
                    break;
                default: throw new ArgumentOutOfRangeException("d3dFormat");
            }

            return formatInfo;
        }

        public static IFormatInfo TexelFormatInfo(Format d3dFormat)
        {
            switch (d3dFormat)
            {
                case Format.A32B32G32R32F: return ExplicitFormatInfo(d3dFormat, ExplicitFormat.R32G32B32A32_FLOAT);
                case Format.A16B16G16R16F: return ExplicitFormatInfo(d3dFormat, ExplicitFormat.R16G16B16A16_FLOAT);
                case Format.A16B16G16R16: return ExplicitFormatInfo(d3dFormat, ExplicitFormat.R16G16B16A16_UNORM);
                case Format.G32R32F: return ExplicitFormatInfo(d3dFormat, ExplicitFormat.R32G32_FLOAT);
                case Format.A2B10G10R10: return ExplicitFormatInfo(d3dFormat, ExplicitFormat.R10G10B10A2_UNORM);
                case Format.A8B8G8R8: return ExplicitFormatInfo(d3dFormat, ExplicitFormat.R8G8B8A8_UNORM);
                case Format.G16R16F: return ExplicitFormatInfo(d3dFormat, ExplicitFormat.R16G16_FLOAT);
                case Format.G16R16: return ExplicitFormatInfo(d3dFormat, ExplicitFormat.R16G16_UNORM);
                case Format.D32SingleLockable: return ExplicitFormatInfo(d3dFormat, ExplicitFormat.D32_FLOAT);
                case Format.R32F: return ExplicitFormatInfo(d3dFormat, ExplicitFormat.R32_FLOAT);
                case Format.D24S8: return ExplicitFormatInfo(d3dFormat, ExplicitFormat.D24_UNORM_S8_UINT);
                case Format.D16: return ExplicitFormatInfo(d3dFormat, ExplicitFormat.D16_UNORM);
                case Format.R16F: return ExplicitFormatInfo(d3dFormat, ExplicitFormat.R16_FLOAT);
                case Format.A8: return ExplicitFormatInfo(d3dFormat, ExplicitFormat.A8_UNORM);
                case Format.G8R8_G8B8: return ExplicitFormatInfo(d3dFormat, ExplicitFormat.R8G8_B8G8_UNORM);
                case Format.R8G8_B8G8: return ExplicitFormatInfo(d3dFormat, ExplicitFormat.G8R8_G8B8_UNORM);
                case Format.Dxt1: return ExplicitFormatInfo(d3dFormat, ExplicitFormat.BC1_UNORM);
                case Format.Dxt3: return ExplicitFormatInfo(d3dFormat, ExplicitFormat.BC2_UNORM);
                case Format.Dxt5: return ExplicitFormatInfo(d3dFormat, ExplicitFormat.BC3_UNORM);
                case Format.R5G6B5: return ExplicitFormatInfo(d3dFormat, ExplicitFormat.B5G6R5_UNORM);
                case Format.A1R5G5B5: return ExplicitFormatInfo(d3dFormat, ExplicitFormat.B5G5R5A1_UNORM);
                case Format.A8R8G8B8: return ExplicitFormatInfo(d3dFormat, ExplicitFormat.B8G8R8A8_UNORM);
                case Format.X8R8G8B8: return ExplicitFormatInfo(d3dFormat, ExplicitFormat.B8G8R8X8_UNORM);
            }

            var formatInfo = new FormatInfo
            {
                ID = (int)d3dFormat,
                Description = d3dFormat.ToString()
            };

            switch (d3dFormat)
            {
                case Format.R8G8B8:
                    {
                        formatInfo.NumColors = 3;
                        formatInfo.ColorBits = 24;
                        formatInfo.ColorFormatType = FormatElementType.UNORM;
                        formatInfo.AlphaBits = 0;
                        formatInfo.AlphaFormatType = FormatElementType.UNKNOWN;
                        formatInfo.TotalBits = 24;
                        formatInfo.Flags = ColorAlphaFormatFlags.Bgra;
                    }
                    break;
                case Format.X1R5G5B5:
                    {
                        formatInfo.NumColors = 3;
                        formatInfo.ColorBits = 15;
                        formatInfo.ColorFormatType = FormatElementType.UNORM;
                        formatInfo.AlphaBits = 0;
                        formatInfo.AlphaFormatType = FormatElementType.UNKNOWN;
                        formatInfo.TotalBits = 16;
                        formatInfo.Flags = ColorAlphaFormatFlags.Bgra;
                    }
                    break;
                case Format.A4R4G4B4:
                    {
                        formatInfo.NumColors = 3;
                        formatInfo.ColorBits = 12;
                        formatInfo.ColorFormatType = FormatElementType.UNORM;
                        formatInfo.AlphaBits = 4;
                        formatInfo.AlphaFormatType = FormatElementType.UNORM;
                        formatInfo.TotalBits = 16;
                        formatInfo.Flags = ColorAlphaFormatFlags.Bgra;
                    }
                    break;
                case Format.R3G3B2:
                    {
                        formatInfo.NumColors = 3;
                        formatInfo.ColorBits = 8;
                        formatInfo.ColorFormatType = FormatElementType.UNORM;
                        formatInfo.AlphaBits = 0;
                        formatInfo.AlphaFormatType = FormatElementType.UNKNOWN;
                        formatInfo.TotalBits = 8;
                        formatInfo.Flags = ColorAlphaFormatFlags.Bgra;
                    }
                    break;
                case Format.A8R3G3B2:
                    {
                        formatInfo.NumColors = 3;
                        formatInfo.ColorBits = 8;
                        formatInfo.ColorFormatType = FormatElementType.UNORM;
                        formatInfo.AlphaBits = 8;
                        formatInfo.AlphaFormatType = FormatElementType.UNORM;
                        formatInfo.TotalBits = 8;
                        formatInfo.Flags = ColorAlphaFormatFlags.Bgra;
                    }
                    break;
                case Format.X4R4G4B4:
                    {
                        formatInfo.NumColors = 3;
                        formatInfo.ColorBits = 12;
                        formatInfo.ColorFormatType = FormatElementType.UNORM;
                        formatInfo.AlphaBits = 0;
                        formatInfo.AlphaFormatType = FormatElementType.UNKNOWN;
                        formatInfo.TotalBits = 16;
                        formatInfo.Flags = ColorAlphaFormatFlags.Bgra;
                    }
                    break;
                case Format.X8B8G8R8:
                    {
                        formatInfo.NumColors = 3;
                        formatInfo.ColorBits = 24;
                        formatInfo.ColorFormatType = FormatElementType.UNORM;
                        formatInfo.AlphaBits = 0;
                        formatInfo.AlphaFormatType = FormatElementType.UNKNOWN;
                        formatInfo.TotalBits = 32;
                    }
                    break;
                case Format.A2R10G10B10:
                    {
                        formatInfo.NumColors = 3;
                        formatInfo.ColorBits = 30;
                        formatInfo.ColorFormatType = FormatElementType.UNORM;
                        formatInfo.AlphaBits = 2;
                        formatInfo.AlphaFormatType = FormatElementType.UNORM;
                        formatInfo.TotalBits = 32;
                        formatInfo.Flags = ColorAlphaFormatFlags.Bgra;
                    }
                    break;
                case Format.D16Lockable:
                    {
                        formatInfo.NumColors = 1;
                        formatInfo.ColorBits = 16;
                        formatInfo.ColorFormatType = FormatElementType.UNORM;
                        formatInfo.AlphaBits = 0;
                        formatInfo.AlphaFormatType = FormatElementType.UNKNOWN;
                        formatInfo.TotalBits = 16;
                        formatInfo.Flags = ColorAlphaFormatFlags.DepthStencil;
                    }
                    break;
                case Format.D32:
                    {
                        formatInfo.NumColors = 1;
                        formatInfo.ColorBits = 32;
                        formatInfo.ColorFormatType = FormatElementType.UNORM;
                        formatInfo.AlphaBits = 0;
                        formatInfo.AlphaFormatType = FormatElementType.UNKNOWN;
                        formatInfo.TotalBits = 32;
                    }
                    break;
                case Format.D15S1:
                    {
                        formatInfo.NumColors = 1;
                        formatInfo.ColorBits = 15;
                        formatInfo.ColorFormatType = FormatElementType.UNORM;
                        formatInfo.AlphaBits = 1;
                        formatInfo.AlphaFormatType = FormatElementType.UINT;
                        formatInfo.TotalBits = 16;
                    }
                    break;
                case Format.D24X8:
                    {
                        formatInfo.NumColors = 1;
                        formatInfo.ColorBits = 24;
                        formatInfo.ColorFormatType = FormatElementType.UNORM;
                        formatInfo.AlphaBits = 0;
                        formatInfo.AlphaFormatType = FormatElementType.UNKNOWN;
                        formatInfo.TotalBits = 32;
                    }
                    break;
                case Format.D24X4S4:
                    {
                        formatInfo.NumColors = 1;
                        formatInfo.ColorBits = 24;
                        formatInfo.ColorFormatType = FormatElementType.UNORM;
                        formatInfo.AlphaBits = 4;
                        formatInfo.AlphaFormatType = FormatElementType.UINT;
                        formatInfo.TotalBits = 32;
                    }
                    break;
                case Format.D24SingleS8:
                    {
                        formatInfo.NumColors = 1;
                        formatInfo.ColorBits = 24;
                        formatInfo.ColorFormatType = FormatElementType.FLOAT;
                        formatInfo.AlphaBits = 8;
                        formatInfo.AlphaFormatType = FormatElementType.UINT;
                        formatInfo.TotalBits = 32;
                    }
                    break;
                case Format.D32Lockable:
                    {
                        formatInfo.NumColors = 1;
                        formatInfo.ColorBits = 32;
                        formatInfo.ColorFormatType = FormatElementType.UNORM;
                        formatInfo.AlphaBits = 0;
                        formatInfo.AlphaFormatType = FormatElementType.UNKNOWN;
                        formatInfo.TotalBits = 32;
                    }
                    break;
                case Format.S8Lockable:
                    {
                        formatInfo.NumColors = 0;
                        formatInfo.ColorBits = 0;
                        formatInfo.ColorFormatType = FormatElementType.UNKNOWN;
                        formatInfo.AlphaBits = 8;
                        formatInfo.AlphaFormatType = FormatElementType.UINT;
                        formatInfo.TotalBits = 8;
                    }
                    break;
                default: throw new ArgumentOutOfRangeException("d3dFormat");
            }

            return formatInfo;
        }
        #endregion

        #region Sampling
        public static Sampling Sampling(MultisampleType multisampleType, int multisampleQuality)
        {
            return new Sampling
            {
                Count = multisampleType == MultisampleType.None ? (ushort)1 : (ushort)multisampleType,
                Quality = (ushort)multisampleQuality
            };
        }
        #endregion

        #region VertexElementFormat
        public static ExplicitFormat VertexElementFormat(DeclarationType d3dDeclType)
        {
            switch (d3dDeclType)
            {
                case DeclarationType.Float1: return ExplicitFormat.R32_FLOAT;
                case DeclarationType.Float2: return ExplicitFormat.R32G32_FLOAT;
                case DeclarationType.Float3: return ExplicitFormat.R32G32B32_FLOAT;
                case DeclarationType.Float4: return ExplicitFormat.R32G32B32A32_FLOAT;
                case DeclarationType.Short2: return ExplicitFormat.R16G16_SINT;
                case DeclarationType.Short2N: return ExplicitFormat.R16G16_SNORM;
                case DeclarationType.Short4: return ExplicitFormat.R16G16B16A16_SINT;
                case DeclarationType.Short4N: return ExplicitFormat.R16G16B16A16_SNORM;
                case DeclarationType.Ubyte4: return ExplicitFormat.R8G8B8A8_UINT;
                case DeclarationType.UByte4N: return ExplicitFormat.R8G8B8A8_UNORM;
                case DeclarationType.UShort2N: return ExplicitFormat.R16G16_UNORM;
                case DeclarationType.UShort4N: return ExplicitFormat.R16G16B16A16_UNORM;
                default: throw new ArgumentOutOfRangeException("d3dDeclType");
            }
        }
        #endregion

        #region PrimitiveTopology
        public static PrimitiveTopology PrimitiveTopology(PrimitiveType d3dPrimitiveType)
        {
            switch (d3dPrimitiveType)
            {
                case PrimitiveType.TriangleStrip: return Beholder.Core.PrimitiveTopology.TriangleStrip;
                case PrimitiveType.TriangleList: return Beholder.Core.PrimitiveTopology.TriangleList;
                case PrimitiveType.LineStrip: return Beholder.Core.PrimitiveTopology.LineStrip;
                case PrimitiveType.LineList: return Beholder.Core.PrimitiveTopology.LineList;
                case PrimitiveType.PointList: return Beholder.Core.PrimitiveTopology.PointList;
                default: throw new ArgumentOutOfRangeException("d3dPrimitiveType");
            }
        }
        #endregion
    }
}
