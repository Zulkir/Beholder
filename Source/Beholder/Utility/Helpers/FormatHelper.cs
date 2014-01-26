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

namespace Beholder.Utility.Helpers
{
    public static class FormatHelper
    {
        public static void GetExplicitColorAlphaFormatInfo(ExplicitFormat format, 
            out int numColors, out int colorBits, out FormatElementType colorFormatType, 
            out int alphaBits, out FormatElementType alphaFormatType,
            out int totalBits, out ColorAlphaFormatFlags flags)
        {
            switch (format)
            {
                #region R32G32B32A32
                case ExplicitFormat.R32G32B32A32_TYPELESS:
                    {
                        numColors = 3;
                        colorBits = 96;
                        colorFormatType = FormatElementType.TYPELESS;
                        alphaBits = 32;
                        alphaFormatType = FormatElementType.TYPELESS;
                        totalBits = 128;
                        flags = ColorAlphaFormatFlags.None;
                        return;
                    }
                case ExplicitFormat.R32G32B32A32_FLOAT:
                    {
                        numColors = 3;
                        colorBits = 96;
                        colorFormatType = FormatElementType.FLOAT;
                        alphaBits = 32;
                        alphaFormatType = FormatElementType.FLOAT;
                        totalBits = 128;
                        flags = ColorAlphaFormatFlags.None;
                        return;
                    }
                case ExplicitFormat.R32G32B32A32_UINT:
                    {
                        numColors = 3;
                        colorBits = 96;
                        colorFormatType = FormatElementType.UINT;
                        alphaBits = 32;
                        alphaFormatType = FormatElementType.UINT;
                        totalBits = 128;
                        flags = ColorAlphaFormatFlags.None;
                        return;
                    }
                case ExplicitFormat.R32G32B32A32_SINT:
                    {
                        numColors = 3;
                        colorBits = 96;
                        colorFormatType = FormatElementType.SINT;
                        alphaBits = 32;
                        alphaFormatType = FormatElementType.SINT;
                        totalBits = 128;
                        flags = ColorAlphaFormatFlags.None;
                        return;
                    }
                #endregion
                #region R32G32B32
                case ExplicitFormat.R32G32B32_TYPELESS:
                    {
                        numColors = 3;
                        colorBits = 96;
                        colorFormatType = FormatElementType.TYPELESS;
                        alphaBits = 0;
                        alphaFormatType = FormatElementType.UNKNOWN;
                        totalBits = 96;
                        flags = ColorAlphaFormatFlags.None;
                        return;
                    }
                case ExplicitFormat.R32G32B32_FLOAT:
                    {
                        numColors = 3;
                        colorBits = 96;
                        colorFormatType = FormatElementType.FLOAT;
                        alphaBits = 0;
                        alphaFormatType = FormatElementType.UNKNOWN;
                        totalBits = 96;
                        flags = ColorAlphaFormatFlags.None;
                        return;
                    }
                case ExplicitFormat.R32G32B32_UINT:
                    {
                        numColors = 3;
                        colorBits = 96;
                        colorFormatType = FormatElementType.UINT;
                        alphaBits = 0;
                        alphaFormatType = FormatElementType.UNKNOWN;
                        totalBits = 96;
                        flags = ColorAlphaFormatFlags.None;
                        return;
                    }
                case ExplicitFormat.R32G32B32_SINT:
                    {
                        numColors = 3;
                        colorBits = 96;
                        colorFormatType = FormatElementType.SINT;
                        alphaBits = 0;
                        alphaFormatType = FormatElementType.UNKNOWN;
                        totalBits = 96;
                        flags = ColorAlphaFormatFlags.None;
                        return;
                    }
                #endregion
                #region R16G16B16A16
                case ExplicitFormat.R16G16B16A16_TYPELESS:
                    {
                        numColors = 3;
                        colorBits = 48;
                        colorFormatType = FormatElementType.TYPELESS;
                        alphaBits = 16;
                        alphaFormatType = FormatElementType.TYPELESS;
                        totalBits = 64;
                        flags = ColorAlphaFormatFlags.None;
                        return;
                    }
                case ExplicitFormat.R16G16B16A16_FLOAT:
                    {
                        numColors = 3;
                        colorBits = 48;
                        colorFormatType = FormatElementType.FLOAT;
                        alphaBits = 16;
                        alphaFormatType = FormatElementType.FLOAT;
                        totalBits = 64;
                        flags = ColorAlphaFormatFlags.None;
                        return;
                    }
                case ExplicitFormat.R16G16B16A16_UNORM:
                    {
                        numColors = 3;
                        colorBits = 48;
                        colorFormatType = FormatElementType.UNORM;
                        alphaBits = 16;
                        alphaFormatType = FormatElementType.UNORM;
                        totalBits = 64;
                        flags = ColorAlphaFormatFlags.None;
                        return;
                    }
                case ExplicitFormat.R16G16B16A16_UINT:
                    {
                        numColors = 3;
                        colorBits = 48;
                        colorFormatType = FormatElementType.UINT;
                        alphaBits = 16;
                        alphaFormatType = FormatElementType.UINT;
                        totalBits = 64;
                        flags = ColorAlphaFormatFlags.None;
                        return;
                    }
                case ExplicitFormat.R16G16B16A16_SNORM:
                    {
                        numColors = 3;
                        colorBits = 48;
                        colorFormatType = FormatElementType.SNORM;
                        alphaBits = 16;
                        alphaFormatType = FormatElementType.SNORM;
                        totalBits = 64;
                        flags = ColorAlphaFormatFlags.None;
                        return;
                    }
                case ExplicitFormat.R16G16B16A16_SINT:
                    {
                        numColors = 3;
                        colorBits = 48;
                        colorFormatType = FormatElementType.SINT;
                        alphaBits = 16;
                        alphaFormatType = FormatElementType.SINT;
                        totalBits = 64;
                        flags = ColorAlphaFormatFlags.None;
                        return;
                    }
                #endregion
                #region R32G32
                case ExplicitFormat.R32G32_TYPELESS:
                    {
                        numColors = 2;
                        colorBits = 64;
                        colorFormatType = FormatElementType.TYPELESS;
                        alphaBits = 0;
                        alphaFormatType = FormatElementType.UNKNOWN;
                        totalBits = 64;
                        flags = ColorAlphaFormatFlags.None;
                        return;
                    }
                case ExplicitFormat.R32G32_FLOAT:
                    {
                        numColors = 2;
                        colorBits = 64;
                        colorFormatType = FormatElementType.FLOAT;
                        alphaBits = 0;
                        alphaFormatType = FormatElementType.UNKNOWN;
                        totalBits = 64;
                        flags = ColorAlphaFormatFlags.None;
                        return;
                    }
                case ExplicitFormat.R32G32_UINT:
                    {
                        numColors = 2;
                        colorBits = 64;
                        colorFormatType = FormatElementType.UINT;
                        alphaBits = 0;
                        alphaFormatType = FormatElementType.UNKNOWN;
                        totalBits = 64;
                        flags = ColorAlphaFormatFlags.None;
                        return;
                    }
                case ExplicitFormat.R32G32_SINT:
                    {
                        numColors = 2;
                        colorBits = 64;
                        colorFormatType = FormatElementType.SINT;
                        alphaBits = 0;
                        alphaFormatType = FormatElementType.UNKNOWN;
                        totalBits = 64;
                        flags = ColorAlphaFormatFlags.None;
                        return;
                    }
                #endregion
                #region R32G8X24
                case ExplicitFormat.R32G8X24_TYPELESS:
                    {
                        numColors = 2;
                        colorBits = 40;
                        colorFormatType = FormatElementType.TYPELESS;
                        alphaBits = 0;
                        alphaFormatType = FormatElementType.UNKNOWN;
                        totalBits = 64;
                        flags = ColorAlphaFormatFlags.None;
                        return;
                    }
                case ExplicitFormat.D32_FLOAT_S8X24_UINT:
                    {
                        numColors = 1;
                        colorBits = 32;
                        colorFormatType = FormatElementType.FLOAT;
                        alphaBits = 8;
                        alphaFormatType = FormatElementType.UINT;
                        totalBits = 64;
                        flags = ColorAlphaFormatFlags.DepthStencil;
                        return;
                    }
                case ExplicitFormat.R32_FLOAT_X8X24_TYPELESS:
                    {
                        numColors = 1;
                        colorBits = 32;
                        colorFormatType = FormatElementType.FLOAT;
                        alphaBits = 0;
                        alphaFormatType = FormatElementType.UNKNOWN;
                        totalBits = 64;
                        flags = ColorAlphaFormatFlags.DepthStencil;
                        return;
                    }
                case ExplicitFormat.X32_TYPELESS_G8X24_UINT:
                    {
                        numColors = 1;
                        colorBits = 8;
                        colorFormatType = FormatElementType.UINT;
                        alphaBits = 0;
                        alphaFormatType = FormatElementType.UNKNOWN;
                        totalBits = 64;
                        flags = ColorAlphaFormatFlags.DepthStencil;
                        return;
                    }
                #endregion
                #region R10G10B10A2
                case ExplicitFormat.R10G10B10A2_TYPELESS:
                    {
                        numColors = 3;
                        colorBits = 30;
                        colorFormatType = FormatElementType.TYPELESS;
                        alphaBits = 2;
                        alphaFormatType = FormatElementType.TYPELESS;
                        totalBits = 32;
                        flags = ColorAlphaFormatFlags.None;
                        return;
                    }
                case ExplicitFormat.R10G10B10A2_UNORM:
                    {
                        numColors = 3;
                        colorBits = 30;
                        colorFormatType = FormatElementType.UNORM;
                        alphaBits = 2;
                        alphaFormatType = FormatElementType.UNORM;
                        totalBits = 32;
                        flags = ColorAlphaFormatFlags.None;
                        return;
                    }
                case ExplicitFormat.R10G10B10A2_UINT:
                    {
                        numColors = 3;
                        colorBits = 30;
                        colorFormatType = FormatElementType.UINT;
                        alphaBits = 2;
                        alphaFormatType = FormatElementType.UINT;
                        totalBits = 32;
                        flags = ColorAlphaFormatFlags.None;
                        return;
                    }
                #endregion
                #region R11G11B10
                case ExplicitFormat.R11G11B10_FLOAT:
                    {
                        numColors = 3;
                        colorBits = 32;
                        colorFormatType = FormatElementType.FLOAT;
                        alphaBits = 0;
                        alphaFormatType = FormatElementType.UNKNOWN;
                        totalBits = 32;
                        flags = ColorAlphaFormatFlags.None;
                        return;
                    }
                #endregion
                #region R8G8B8A8
                case ExplicitFormat.R8G8B8A8_TYPELESS:
                    {
                        numColors = 3;
                        colorBits = 24;
                        colorFormatType = FormatElementType.TYPELESS;
                        alphaBits = 8;
                        alphaFormatType = FormatElementType.TYPELESS;
                        totalBits = 32;
                        flags = ColorAlphaFormatFlags.None;
                        return;
                    }
                case ExplicitFormat.R8G8B8A8_UNORM:
                    {
                        numColors = 3;
                        colorBits = 24;
                        colorFormatType = FormatElementType.UNORM;
                        alphaBits = 8;
                        alphaFormatType = FormatElementType.UNORM;
                        totalBits = 32;
                        flags = ColorAlphaFormatFlags.None;
                        return;
                    }
                case ExplicitFormat.R8G8B8A8_UNORM_SRGB:
                    {
                        numColors = 3;
                        colorBits = 24;
                        colorFormatType = FormatElementType.UNORM_SRGB;
                        alphaBits = 8;
                        alphaFormatType = FormatElementType.UNORM;
                        totalBits = 32;
                        flags = ColorAlphaFormatFlags.None;
                        return;
                    }
                case ExplicitFormat.R8G8B8A8_UINT:
                    {
                        numColors = 3;
                        colorBits = 24;
                        colorFormatType = FormatElementType.UINT;
                        alphaBits = 8;
                        alphaFormatType = FormatElementType.UINT;
                        totalBits = 32;
                        flags = ColorAlphaFormatFlags.None;
                        return;
                    }
                case ExplicitFormat.R8G8B8A8_SNORM:
                    {
                        numColors = 3;
                        colorBits = 24;
                        colorFormatType = FormatElementType.SNORM;
                        alphaBits = 8;
                        alphaFormatType = FormatElementType.SNORM;
                        totalBits = 32;
                        flags = ColorAlphaFormatFlags.None;
                        return;
                    }
                case ExplicitFormat.R8G8B8A8_SINT:
                    {
                        numColors = 3;
                        colorBits = 24;
                        colorFormatType = FormatElementType.SINT;
                        alphaBits = 8;
                        alphaFormatType = FormatElementType.SINT;
                        totalBits = 32;
                        flags = ColorAlphaFormatFlags.None;
                        return;
                    }
                #endregion
                #region R16G16
                case ExplicitFormat.R16G16_TYPELESS:
                    {
                        numColors = 2;
                        colorBits = 32;
                        colorFormatType = FormatElementType.TYPELESS;
                        alphaBits = 0;
                        alphaFormatType = FormatElementType.UNKNOWN;
                        totalBits = 32;
                        flags = ColorAlphaFormatFlags.None;
                        return;
                    }
                case ExplicitFormat.R16G16_FLOAT:
                    {
                        numColors = 2;
                        colorBits = 32;
                        colorFormatType = FormatElementType.FLOAT;
                        alphaBits = 0;
                        alphaFormatType = FormatElementType.UNKNOWN;
                        totalBits = 32;
                        flags = ColorAlphaFormatFlags.None;
                        return;
                    }
                case ExplicitFormat.R16G16_UNORM:
                    {
                        numColors = 2;
                        colorBits = 32;
                        colorFormatType = FormatElementType.UNORM;
                        alphaBits = 0;
                        alphaFormatType = FormatElementType.UNKNOWN;
                        totalBits = 32;
                        flags = ColorAlphaFormatFlags.None;
                        return;
                    }
                case ExplicitFormat.R16G16_UINT:
                    {
                        numColors = 2;
                        colorBits = 32;
                        colorFormatType = FormatElementType.UINT;
                        alphaBits = 0;
                        alphaFormatType = FormatElementType.UNKNOWN;
                        totalBits = 32;
                        flags = ColorAlphaFormatFlags.None;
                        return;
                    }
                case ExplicitFormat.R16G16_SNORM:
                    {
                        numColors = 2;
                        colorBits = 32;
                        colorFormatType = FormatElementType.SNORM;
                        alphaBits = 0;
                        alphaFormatType = FormatElementType.UNKNOWN;
                        totalBits = 32;
                        flags = ColorAlphaFormatFlags.None;
                        return;
                    }
                case ExplicitFormat.R16G16_SINT:
                    {
                        numColors = 2;
                        colorBits = 32;
                        colorFormatType = FormatElementType.SINT;
                        alphaBits = 0;
                        alphaFormatType = FormatElementType.UNKNOWN;
                        totalBits = 32;
                        flags = ColorAlphaFormatFlags.None;
                        return;
                    }
                #endregion
                #region R32
                case ExplicitFormat.R32_TYPELESS:
                    {
                        numColors = 1;
                        colorBits = 32;
                        colorFormatType = FormatElementType.TYPELESS;
                        alphaBits = 0;
                        alphaFormatType = FormatElementType.UNKNOWN;
                        totalBits = 32;
                        flags = ColorAlphaFormatFlags.None;
                        return;
                    }
                case ExplicitFormat.D32_FLOAT:
                    {
                        numColors = 1;
                        colorBits = 32;
                        colorFormatType = FormatElementType.FLOAT;
                        alphaBits = 0;
                        alphaFormatType = FormatElementType.UNKNOWN;
                        totalBits = 32;
                        flags = ColorAlphaFormatFlags.DepthStencil;
                        return;
                    }
                case ExplicitFormat.R32_FLOAT:
                    {
                        numColors = 1;
                        colorBits = 32;
                        colorFormatType = FormatElementType.FLOAT;
                        alphaBits = 0;
                        alphaFormatType = FormatElementType.UNKNOWN;
                        totalBits = 32;
                        flags = ColorAlphaFormatFlags.None;
                        return;
                    }
                case ExplicitFormat.R32_UINT:
                    {
                        numColors = 1;
                        colorBits = 32;
                        colorFormatType = FormatElementType.UINT;
                        alphaBits = 0;
                        alphaFormatType = FormatElementType.UNKNOWN;
                        totalBits = 32;
                        flags = ColorAlphaFormatFlags.None;
                        return;
                    }
                case ExplicitFormat.R32_SINT:
                    {
                        numColors = 1;
                        colorBits = 32;
                        colorFormatType = FormatElementType.SINT;
                        alphaBits = 0;
                        alphaFormatType = FormatElementType.UNKNOWN;
                        totalBits = 32;
                        flags = ColorAlphaFormatFlags.None;
                        return;
                    }
                #endregion
                #region R24G8
                case ExplicitFormat.R24G8_TYPELESS:
                    {
                        numColors = 2;
                        colorBits = 32;
                        colorFormatType = FormatElementType.TYPELESS;
                        alphaBits = 0;
                        alphaFormatType = FormatElementType.UNKNOWN;
                        totalBits = 32;
                        flags = ColorAlphaFormatFlags.None;
                        return;
                    }
                case ExplicitFormat.D24_UNORM_S8_UINT:
                    {
                        numColors = 1;
                        colorBits = 24;
                        colorFormatType = FormatElementType.UNORM;
                        alphaBits = 8;
                        alphaFormatType = FormatElementType.UINT;
                        totalBits = 32;
                        flags = ColorAlphaFormatFlags.DepthStencil;
                        return;
                    }
                #endregion
                #region R24_X8
                case ExplicitFormat.R24_UNORM_X8_TYPELESS:
                    {
                        numColors = 1;
                        colorBits = 24;
                        colorFormatType = FormatElementType.UNORM;
                        alphaBits = 0;
                        alphaFormatType = FormatElementType.UNKNOWN;
                        totalBits = 32;
                        flags = ColorAlphaFormatFlags.DepthStencil;
                        return;
                    }
                #endregion
                #region X24_G8
                case ExplicitFormat.X24_TYPELESS_G8_UINT:
                    {
                        numColors = 1;
                        colorBits = 8;
                        colorFormatType = FormatElementType.UINT;
                        alphaBits = 0;
                        alphaFormatType = FormatElementType.UNKNOWN;
                        totalBits = 32;
                        flags = ColorAlphaFormatFlags.DepthStencil;
                        return;
                    }
                #endregion
                #region R8G8
                case ExplicitFormat.R8G8_TYPELESS:
                    {
                        numColors = 2;
                        colorBits = 16;
                        colorFormatType = FormatElementType.TYPELESS;
                        alphaBits = 0;
                        alphaFormatType = FormatElementType.UNKNOWN;
                        totalBits = 16;
                        flags = ColorAlphaFormatFlags.None;
                        return;
                    }
                case ExplicitFormat.R8G8_UNORM:
                    {
                        numColors = 2;
                        colorBits = 16;
                        colorFormatType = FormatElementType.UNORM;
                        alphaBits = 0;
                        alphaFormatType = FormatElementType.UNKNOWN;
                        totalBits = 16;
                        flags = ColorAlphaFormatFlags.None;
                        return;
                    }
                case ExplicitFormat.R8G8_UINT:
                    {
                        numColors = 2;
                        colorBits = 16;
                        colorFormatType = FormatElementType.UINT;
                        alphaBits = 0;
                        alphaFormatType = FormatElementType.UNKNOWN;
                        totalBits = 16;
                        flags = ColorAlphaFormatFlags.None;
                        return;
                    }
                case ExplicitFormat.R8G8_SNORM:
                    {
                        numColors = 2;
                        colorBits = 16;
                        colorFormatType = FormatElementType.SNORM;
                        alphaBits = 0;
                        alphaFormatType = FormatElementType.UNKNOWN;
                        totalBits = 16;
                        flags = ColorAlphaFormatFlags.None;
                        return;
                    }
                case ExplicitFormat.R8G8_SINT:
                    {
                        numColors = 2;
                        colorBits = 16;
                        colorFormatType = FormatElementType.SINT;
                        alphaBits = 0;
                        alphaFormatType = FormatElementType.UNKNOWN;
                        totalBits = 16;
                        flags = ColorAlphaFormatFlags.None;
                        return;
                    }
                #endregion
                #region R16
                case ExplicitFormat.R16_TYPELESS:
                    {
                        numColors = 1;
                        colorBits = 16;
                        colorFormatType = FormatElementType.TYPELESS;
                        alphaBits = 0;
                        alphaFormatType = FormatElementType.UNKNOWN;
                        totalBits = 16;
                        flags = ColorAlphaFormatFlags.None;
                        return;
                    }
                case ExplicitFormat.D16_UNORM:
                    {
                        numColors = 1;
                        colorBits = 16;
                        colorFormatType = FormatElementType.UNORM;
                        alphaBits = 0;
                        alphaFormatType = FormatElementType.UNKNOWN;
                        totalBits = 16;
                        flags = ColorAlphaFormatFlags.DepthStencil;
                        return;
                    }
                case ExplicitFormat.R16_FLOAT:
                    {
                        numColors = 1;
                        colorBits = 16;
                        colorFormatType = FormatElementType.FLOAT;
                        alphaBits = 0;
                        alphaFormatType = FormatElementType.UNKNOWN;
                        totalBits = 16;
                        flags = ColorAlphaFormatFlags.None;
                        return;
                    }
                case ExplicitFormat.R16_UNORM:
                    {
                        numColors = 1;
                        colorBits = 16;
                        colorFormatType = FormatElementType.UNORM;
                        alphaBits = 0;
                        alphaFormatType = FormatElementType.UNKNOWN;
                        totalBits = 16;
                        flags = ColorAlphaFormatFlags.None;
                        return;
                    }
                case ExplicitFormat.R16_UINT:
                    {
                        numColors = 1;
                        colorBits = 16;
                        colorFormatType = FormatElementType.UINT;
                        alphaBits = 0;
                        alphaFormatType = FormatElementType.UNKNOWN;
                        totalBits = 16;
                        flags = ColorAlphaFormatFlags.None;
                        return;
                    }
                case ExplicitFormat.R16_SNORM:
                    {
                        numColors = 1;
                        colorBits = 16;
                        colorFormatType = FormatElementType.SNORM;
                        alphaBits = 0;
                        alphaFormatType = FormatElementType.UNKNOWN;
                        totalBits = 16;
                        flags = ColorAlphaFormatFlags.None;
                        return;
                    }
                case ExplicitFormat.R16_SINT:
                    {
                        numColors = 1;
                        colorBits = 16;
                        colorFormatType = FormatElementType.SINT;
                        alphaBits = 0;
                        alphaFormatType = FormatElementType.UNKNOWN;
                        totalBits = 16;
                        flags = ColorAlphaFormatFlags.None;
                        return;
                    }
                #endregion
                #region R8
                case ExplicitFormat.R8_TYPELESS:
                    {
                        numColors = 1;
                        colorBits = 8;
                        colorFormatType = FormatElementType.TYPELESS;
                        alphaBits = 0;
                        alphaFormatType = FormatElementType.UNKNOWN;
                        totalBits = 8;
                        flags = ColorAlphaFormatFlags.None;
                        return;
                    }
                case ExplicitFormat.R8_UNORM:
                    {
                        numColors = 1;
                        colorBits = 8;
                        colorFormatType = FormatElementType.UNORM;
                        alphaBits = 0;
                        alphaFormatType = FormatElementType.UNKNOWN;
                        totalBits = 8;
                        flags = ColorAlphaFormatFlags.None;
                        return;
                    }
                case ExplicitFormat.R8_UINT:
                    {
                        numColors = 1;
                        colorBits = 8;
                        colorFormatType = FormatElementType.UINT;
                        alphaBits = 0;
                        alphaFormatType = FormatElementType.UNKNOWN;
                        totalBits = 8;
                        flags = ColorAlphaFormatFlags.None;
                        return;
                    }
                case ExplicitFormat.R8_SNORM:
                    {
                        numColors = 1;
                        colorBits = 8;
                        colorFormatType = FormatElementType.SNORM;
                        alphaBits = 0;
                        alphaFormatType = FormatElementType.UNKNOWN;
                        totalBits = 8;
                        flags = ColorAlphaFormatFlags.None;
                        return;
                    }
                case ExplicitFormat.R8_SINT:
                    {
                        numColors = 1;
                        colorBits = 8;
                        colorFormatType = FormatElementType.SINT;
                        alphaBits = 0;
                        alphaFormatType = FormatElementType.UNKNOWN;
                        totalBits = 8;
                        flags = ColorAlphaFormatFlags.None;
                        return;
                    }
                #endregion
                #region A8
                case ExplicitFormat.A8_UNORM:
                    {
                        numColors = 0;
                        colorBits = 0;
                        colorFormatType = FormatElementType.UNKNOWN;
                        alphaBits = 8;
                        alphaFormatType = FormatElementType.UNORM;
                        totalBits = 8;
                        flags = ColorAlphaFormatFlags.None;
                        return;
                    }
                #endregion
                #region R1
                case ExplicitFormat.R1_UNORM:
                    {
                        numColors = 1;
                        colorBits = 1;
                        colorFormatType = FormatElementType.UNORM;
                        alphaBits = 0;
                        alphaFormatType = FormatElementType.UNKNOWN;
                        totalBits = 1;
                        flags = ColorAlphaFormatFlags.None;
                        return;
                    }
                #endregion
                #region R9G9B9E5
                case ExplicitFormat.R9G9B9E5_SHAREDEXP:
                    {
                        numColors = 3;
                        colorBits = 32;
                        colorFormatType = FormatElementType.SHAREDEXP;
                        alphaBits = 0;
                        alphaFormatType = FormatElementType.UNKNOWN;
                        totalBits = 32;
                        flags = ColorAlphaFormatFlags.None;
                        return;
                    }
                #endregion
                #region R8G8_B8G8
                case ExplicitFormat.R8G8_B8G8_UNORM:
                    {
                        numColors = 3;
                        colorBits = 16;
                        colorFormatType = FormatElementType.UNORM_SUBSAMPLED;
                        alphaBits = 0;
                        alphaFormatType = FormatElementType.UNKNOWN;
                        totalBits = 16;
                        flags = ColorAlphaFormatFlags.None;
                        return;
                    }
                #endregion
                #region G8R8_G8B8
                case ExplicitFormat.G8R8_G8B8_UNORM:
                    {
                        numColors = 3;
                        colorBits = 16;
                        colorFormatType = FormatElementType.UNORM_SUBSAMPLED;
                        alphaBits = 0;
                        alphaFormatType = FormatElementType.UNKNOWN;
                        totalBits = 16;
                        flags = ColorAlphaFormatFlags.Bgra;
                        return;
                    }
                #endregion
                #region BC1
                case ExplicitFormat.BC1_TYPELESS:
                case ExplicitFormat.BC1_UNORM:
                case ExplicitFormat.BC1_UNORM_SRGB:
                    {
                        numColors = 0;
                        colorBits = 0;
                        colorFormatType = FormatElementType.UNKNOWN;
                        alphaBits = 0;
                        alphaFormatType = FormatElementType.UNKNOWN;
                        totalBits = 4;
                        flags = ColorAlphaFormatFlags.Compressed;
                        return;
                    }
                #endregion
                #region BC2
                case ExplicitFormat.BC2_TYPELESS:
                case ExplicitFormat.BC2_UNORM:
                case ExplicitFormat.BC2_UNORM_SRGB:
                    {
                        numColors = 0;
                        colorBits = 0;
                        colorFormatType = FormatElementType.UNKNOWN;
                        alphaBits = 0;
                        alphaFormatType = FormatElementType.UNKNOWN;
                        totalBits = 8;
                        flags = ColorAlphaFormatFlags.Compressed;
                        return;
                    }
                #endregion
                #region BC3
                case ExplicitFormat.BC3_TYPELESS:
                case ExplicitFormat.BC3_UNORM:
                case ExplicitFormat.BC3_UNORM_SRGB:
                    {
                        numColors = 0;
                        colorBits = 0;
                        colorFormatType = FormatElementType.UNKNOWN;
                        alphaBits = 0;
                        alphaFormatType = FormatElementType.UNKNOWN;
                        totalBits = 8;
                        flags = ColorAlphaFormatFlags.Compressed;
                        return;
                    }
                #endregion
                #region BC4
                case ExplicitFormat.BC4_TYPELESS:
                case ExplicitFormat.BC4_UNORM:
                case ExplicitFormat.BC4_SNORM:
                    {
                        numColors = 0;
                        colorBits = 0;
                        colorFormatType = FormatElementType.UNKNOWN;
                        alphaBits = 0;
                        alphaFormatType = FormatElementType.UNKNOWN;
                        totalBits = 4;
                        flags = ColorAlphaFormatFlags.Compressed;
                        return;
                    }
                #endregion
                #region BC5
                case ExplicitFormat.BC5_TYPELESS:
                case ExplicitFormat.BC5_UNORM:
                case ExplicitFormat.BC5_SNORM:
                    {
                        numColors = 0;
                        colorBits = 0;
                        colorFormatType = FormatElementType.UNKNOWN;
                        alphaBits = 0;
                        alphaFormatType = FormatElementType.UNKNOWN;
                        totalBits = 8;
                        flags = ColorAlphaFormatFlags.Compressed;
                        return;
                    }
                #endregion
                #region B5G6R5
                case ExplicitFormat.B5G6R5_UNORM:
                    {
                        numColors = 3;
                        colorBits = 16;
                        colorFormatType = FormatElementType.UNORM;
                        alphaBits = 0;
                        alphaFormatType = FormatElementType.UNKNOWN;
                        totalBits = 16;
                        flags = ColorAlphaFormatFlags.Bgra;
                        return;
                    }
                #endregion
                #region B5G5R5A1
                case ExplicitFormat.B5G5R5A1_UNORM:
                    {
                        numColors = 3;
                        colorBits = 16;
                        colorFormatType = FormatElementType.UNORM;
                        alphaBits = 1;
                        alphaFormatType = FormatElementType.UNORM;
                        totalBits = 16;
                        flags = ColorAlphaFormatFlags.Bgra;
                        return;
                    }
                #endregion
                #region B8G8R8A8
                case ExplicitFormat.B8G8R8A8_UNORM:
                    {
                        numColors = 3;
                        colorBits = 24;
                        colorFormatType = FormatElementType.UNORM;
                        alphaBits = 8;
                        alphaFormatType = FormatElementType.UNORM;
                        totalBits = 32;
                        flags = ColorAlphaFormatFlags.Bgra;
                        return;
                    }
                #endregion
                #region B8G8R8X8
                case ExplicitFormat.B8G8R8X8_UNORM:
                    {
                        numColors = 3;
                        colorBits = 24;
                        colorFormatType = FormatElementType.UNORM;
                        alphaBits = 0;
                        alphaFormatType = FormatElementType.UNKNOWN;
                        totalBits = 32;
                        flags = ColorAlphaFormatFlags.Bgra;
                        return;
                    }
                #endregion
                #region R10G10B10_A2
                case ExplicitFormat.R10G10B10_XR_BIAS_A2_UNORM:
                    {
                        numColors = 3;
                        colorBits = 30;
                        colorFormatType = FormatElementType.XR_BIAS;
                        alphaBits = 2;
                        alphaFormatType = FormatElementType.UNORM;
                        totalBits = 32;
                        flags = ColorAlphaFormatFlags.None;
                        return;
                    }
                #endregion
                #region B8G8R8A8 again
                case ExplicitFormat.B8G8R8A8_TYPELESS:
                    {
                        numColors = 3;
                        colorBits = 24;
                        colorFormatType = FormatElementType.TYPELESS;
                        alphaBits = 8;
                        alphaFormatType = FormatElementType.TYPELESS;
                        totalBits = 32;
                        flags = ColorAlphaFormatFlags.Bgra;
                        return;
                    }
                case ExplicitFormat.B8G8R8A8_UNORM_SRGB:
                    {
                        numColors = 3;
                        colorBits = 24;
                        colorFormatType = FormatElementType.UNORM_SRGB;
                        alphaBits = 8;
                        alphaFormatType = FormatElementType.UNORM;
                        totalBits = 32;
                        flags = ColorAlphaFormatFlags.Bgra;
                        return;
                    }
                #endregion
                #region B8G8R8X8 again
                case ExplicitFormat.B8G8R8X8_TYPELESS:
                    {
                        numColors = 3;
                        colorBits = 24;
                        colorFormatType = FormatElementType.TYPELESS;
                        alphaBits = 8;
                        alphaFormatType = FormatElementType.UNKNOWN;
                        totalBits = 32;
                        flags = ColorAlphaFormatFlags.Bgra;
                        return;
                    }
                case ExplicitFormat.B8G8R8X8_UNORM_SRGB:
                    {
                        numColors = 3;
                        colorBits = 24;
                        colorFormatType = FormatElementType.UNORM_SRGB;
                        alphaBits = 0;
                        alphaFormatType = FormatElementType.UNKNOWN;
                        totalBits = 32;
                        flags = ColorAlphaFormatFlags.Bgra;
                        return;
                    }
                #endregion
                #region BC6
                case ExplicitFormat.BC6H_TYPELESS:
                case ExplicitFormat.BC6H_UF16:
                case ExplicitFormat.BC6H_SF16:
                    {
                        numColors = 0;
                        colorBits = 0;
                        colorFormatType = FormatElementType.UNKNOWN;
                        alphaBits = 0;
                        alphaFormatType = FormatElementType.UNKNOWN;
                        totalBits = 8;
                        flags = ColorAlphaFormatFlags.Compressed;
                        return;
                    }
                #endregion
                #region BC7
                case ExplicitFormat.BC7_TYPELESS:
                case ExplicitFormat.BC7_UNORM:
                case ExplicitFormat.BC7_UNORM_SRGB:
                    {
                        numColors = 0;
                        colorBits = 0;
                        colorFormatType = FormatElementType.UNKNOWN;
                        alphaBits = 0;
                        alphaFormatType = FormatElementType.UNKNOWN;
                        totalBits = 8;
                        flags = ColorAlphaFormatFlags.Compressed;
                        return;
                    }
                #endregion
                default: throw new ArgumentException(string.Format("'{0}' is not a color-alpha format.", format));
            }
        }

        public static int GetExplicitFormatBits(ExplicitFormat format)
        {
            if (ExplicitFormat.R32G32B32A32_TYPELESS <= format && format <= ExplicitFormat.R32G32B32A32_SINT)
                return 128;
            if (ExplicitFormat.R32G32B32_TYPELESS <= format && format <= ExplicitFormat.R32G32B32_SINT)
                return 96;
            if (ExplicitFormat.R16G16B16A16_TYPELESS <= format && format <= ExplicitFormat.X32_TYPELESS_G8X24_UINT)
                return 64;
            if (ExplicitFormat.R10G10B10A2_TYPELESS <= format && format <= ExplicitFormat.X24_TYPELESS_G8_UINT)
                return 32;
            if (ExplicitFormat.R8G8_UNORM <= format && format <= ExplicitFormat.R16_SINT)
                return 16;
            if (ExplicitFormat.R8_TYPELESS <= format && format <= ExplicitFormat.A8_UNORM)
                return 8;
            if (ExplicitFormat.R9G9B9E5_SHAREDEXP == format)
                return 32;
            if (ExplicitFormat.R8G8_B8G8_UNORM <= format && format <= ExplicitFormat.G8R8_G8B8_UNORM)
                return 16;
            if (ExplicitFormat.BC1_TYPELESS <= format && format <= ExplicitFormat.BC1_UNORM_SRGB)
                return 4;
            if (ExplicitFormat.BC2_TYPELESS <= format && format <= ExplicitFormat.BC3_UNORM_SRGB)
                return 8;
            if (ExplicitFormat.BC4_TYPELESS <= format && format <= ExplicitFormat.BC4_SNORM)
                return 4;
            if (ExplicitFormat.BC5_TYPELESS <= format && format <= ExplicitFormat.BC5_SNORM)
                return 8;
            if (ExplicitFormat.B8G8R8A8_TYPELESS <= format && format <= ExplicitFormat.B8G8R8X8_UNORM_SRGB)
                return 32;
            if (ExplicitFormat.BC6H_TYPELESS <= format && format <= ExplicitFormat.BC7_UNORM_SRGB)
                return 8;

            throw new IndexOutOfRangeException("Trying to get bit-size of an unknown explicit format.");
        }
    }
}
