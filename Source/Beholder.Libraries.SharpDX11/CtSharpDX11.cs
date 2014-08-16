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

namespace Beholder.Libraries.SharpDX11
{
    public static class CtSharpDX11
    {
        #region BlendOption
        public static SharpDX.Direct3D11.BlendOption BlendOption(Beholder.Core.Blend bBlend)
        {
            return (SharpDX.Direct3D11.BlendOption)bBlend;
        }
        #endregion

        #region BlendOperation
        public static SharpDX.Direct3D11.BlendOperation BlendOperation(Beholder.Core.BlendOperation bBlendOp)
        {
            return (SharpDX.Direct3D11.BlendOperation)bBlendOp;
        }
        #endregion

        #region ColorWriteMaskFlags
        public static SharpDX.Direct3D11.ColorWriteMaskFlags ColorWriteMaskFlags(Beholder.Core.ColorMask bColorMask)
        {
            return (SharpDX.Direct3D11.ColorWriteMaskFlags)bColorMask;
        }
        #endregion

        #region Comparison
        public static SharpDX.Direct3D11.Comparison Comparison(Beholder.Core.Comparison bComparison)
        {
            return (SharpDX.Direct3D11.Comparison)bComparison;
        }
        #endregion

        #region CullMode
        public static SharpDX.Direct3D11.CullMode CullMode(Beholder.Core.Cull bCull)
        {
            return (SharpDX.Direct3D11.CullMode)bCull;
        }
        #endregion

        #region FillMode
        public static SharpDX.Direct3D11.FillMode FillMode(Beholder.Core.FillMode bFillMode)
        {
            return (SharpDX.Direct3D11.FillMode)bFillMode;
        }
        #endregion

        #region FormatSupport
        public static SharpDX.Direct3D11.FormatSupport FormatSupport(Beholder.Platform.FormatSupport bSupport)
        {
            return (SharpDX.Direct3D11.FormatSupport) bSupport;
        }
        #endregion

        #region StencilOperation
        public static SharpDX.Direct3D11.StencilOperation StencilOperation(Beholder.Core.StencilOperation bStencilOp)
        {
            return (SharpDX.Direct3D11.StencilOperation)bStencilOp;
        }
        #endregion

        #region TypelessFormat
        public static bool TryGetTypelessFormat(SharpDX.DXGI.Format dxgiFormat, out SharpDX.DXGI.Format typelessFormat)
        {
            switch (dxgiFormat)
            {
                case SharpDX.DXGI.Format.R32G32B32A32_Typeless:
                case SharpDX.DXGI.Format.R32G32B32A32_Float:
                case SharpDX.DXGI.Format.R32G32B32A32_UInt:
                case SharpDX.DXGI.Format.R32G32B32A32_SInt:
                    typelessFormat = SharpDX.DXGI.Format.R32G32B32A32_Typeless; return true;
                case SharpDX.DXGI.Format.R32G32B32_Typeless:
                case SharpDX.DXGI.Format.R32G32B32_Float:
                case SharpDX.DXGI.Format.R32G32B32_UInt:
                case SharpDX.DXGI.Format.R32G32B32_SInt:
                    typelessFormat = SharpDX.DXGI.Format.R32G32B32_Typeless; return true;
                case SharpDX.DXGI.Format.R16G16B16A16_Typeless:
                case SharpDX.DXGI.Format.R16G16B16A16_Float:
                case SharpDX.DXGI.Format.R16G16B16A16_UNorm:
                case SharpDX.DXGI.Format.R16G16B16A16_UInt:
                case SharpDX.DXGI.Format.R16G16B16A16_SNorm:
                case SharpDX.DXGI.Format.R16G16B16A16_SInt:
                    typelessFormat = SharpDX.DXGI.Format.R16G16B16A16_Typeless; return true;
                case SharpDX.DXGI.Format.R32G32_Typeless:
                case SharpDX.DXGI.Format.R32G32_Float:
                case SharpDX.DXGI.Format.R32G32_UInt:
                case SharpDX.DXGI.Format.R32G32_SInt:
                    typelessFormat = SharpDX.DXGI.Format.R32G32_Typeless; return true;
                case SharpDX.DXGI.Format.R32G8X24_Typeless:
                case SharpDX.DXGI.Format.D32_Float_S8X24_UInt:
                case SharpDX.DXGI.Format.R32_Float_X8X24_Typeless:
                case SharpDX.DXGI.Format.X32_Typeless_G8X24_UInt:
                    typelessFormat = SharpDX.DXGI.Format.R32G8X24_Typeless; return true;
                case SharpDX.DXGI.Format.R10G10B10A2_Typeless:
                case SharpDX.DXGI.Format.R10G10B10A2_UNorm:
                case SharpDX.DXGI.Format.R10G10B10A2_UInt:
                    typelessFormat = SharpDX.DXGI.Format.R10G10B10A2_Typeless; return true;
                //case SharpDX.DXGI.Format.R11G11B10_Float:
                case SharpDX.DXGI.Format.R8G8B8A8_Typeless:
                case SharpDX.DXGI.Format.R8G8B8A8_UNorm:
                case SharpDX.DXGI.Format.R8G8B8A8_UNorm_SRgb:
                case SharpDX.DXGI.Format.R8G8B8A8_UInt:
                case SharpDX.DXGI.Format.R8G8B8A8_SNorm:
                case SharpDX.DXGI.Format.R8G8B8A8_SInt:
                    typelessFormat = SharpDX.DXGI.Format.R8G8B8A8_Typeless; return true;
                case SharpDX.DXGI.Format.R16G16_Typeless:
                case SharpDX.DXGI.Format.R16G16_Float:
                case SharpDX.DXGI.Format.R16G16_UNorm:
                case SharpDX.DXGI.Format.R16G16_UInt:
                case SharpDX.DXGI.Format.R16G16_SNorm:
                case SharpDX.DXGI.Format.R16G16_SInt:
                    typelessFormat = SharpDX.DXGI.Format.R16G16_Typeless; return true;
                case SharpDX.DXGI.Format.R32_Typeless:
                case SharpDX.DXGI.Format.D32_Float:
                case SharpDX.DXGI.Format.R32_Float:
                case SharpDX.DXGI.Format.R32_UInt:
                case SharpDX.DXGI.Format.R32_SInt:
                    typelessFormat = SharpDX.DXGI.Format.R32_Typeless; return true;
                case SharpDX.DXGI.Format.R24G8_Typeless:
                case SharpDX.DXGI.Format.D24_UNorm_S8_UInt:
                case SharpDX.DXGI.Format.R24_UNorm_X8_Typeless:
                case SharpDX.DXGI.Format.X24_Typeless_G8_UInt:
                    typelessFormat = SharpDX.DXGI.Format.R24G8_Typeless; return true;
                case SharpDX.DXGI.Format.R8G8_Typeless:
                case SharpDX.DXGI.Format.R8G8_UNorm:
                case SharpDX.DXGI.Format.R8G8_UInt:
                case SharpDX.DXGI.Format.R8G8_SNorm:
                case SharpDX.DXGI.Format.R8G8_SInt:
                    typelessFormat = SharpDX.DXGI.Format.R8G8_Typeless; return true;
                case SharpDX.DXGI.Format.R16_Typeless:
                case SharpDX.DXGI.Format.R16_Float:
                case SharpDX.DXGI.Format.D16_UNorm:
                case SharpDX.DXGI.Format.R16_UNorm:
                case SharpDX.DXGI.Format.R16_UInt:
                case SharpDX.DXGI.Format.R16_SNorm:
                case SharpDX.DXGI.Format.R16_SInt:
                    typelessFormat = SharpDX.DXGI.Format.R16_Typeless; return true;
                case SharpDX.DXGI.Format.R8_Typeless:
                case SharpDX.DXGI.Format.R8_UNorm:
                case SharpDX.DXGI.Format.R8_UInt:
                case SharpDX.DXGI.Format.R8_SNorm:
                case SharpDX.DXGI.Format.R8_SInt:
                case SharpDX.DXGI.Format.A8_UNorm:
                    typelessFormat = SharpDX.DXGI.Format.R8_Typeless; return true;
                //case SharpDX.DXGI.Format.R1_UNorm:
                //case SharpDX.DXGI.Format.R9G9B9E5_SharedExp:
                //case SharpDX.DXGI.Format.R8G8_B8G8_UNorm:
                //case SharpDX.DXGI.Format.G8R8_G8B8_UNorm:
                case SharpDX.DXGI.Format.BC1_Typeless:
                case SharpDX.DXGI.Format.BC1_UNorm:
                case SharpDX.DXGI.Format.BC1_UNorm_SRgb:
                    typelessFormat = SharpDX.DXGI.Format.BC1_Typeless; return true;
                case SharpDX.DXGI.Format.BC2_Typeless:
                case SharpDX.DXGI.Format.BC2_UNorm:
                case SharpDX.DXGI.Format.BC2_UNorm_SRgb:
                    typelessFormat = SharpDX.DXGI.Format.BC2_Typeless; return true;
                case SharpDX.DXGI.Format.BC3_Typeless:
                case SharpDX.DXGI.Format.BC3_UNorm:
                case SharpDX.DXGI.Format.BC3_UNorm_SRgb:
                    typelessFormat = SharpDX.DXGI.Format.BC3_Typeless; return true;
                case SharpDX.DXGI.Format.BC4_Typeless:
                case SharpDX.DXGI.Format.BC4_UNorm:
                case SharpDX.DXGI.Format.BC4_SNorm:
                    typelessFormat = SharpDX.DXGI.Format.BC4_Typeless; return true;
                case SharpDX.DXGI.Format.BC5_Typeless:
                case SharpDX.DXGI.Format.BC5_UNorm:
                case SharpDX.DXGI.Format.BC5_SNorm:
                    typelessFormat = SharpDX.DXGI.Format.BC5_Typeless; return true;
                //case SharpDX.DXGI.Format.B5G6R5_UNorm,
                //case SharpDX.DXGI.Format.B5G5R5A1_UNorm,
                case SharpDX.DXGI.Format.B8G8R8A8_Typeless:
                case SharpDX.DXGI.Format.B8G8R8A8_UNorm:
                case SharpDX.DXGI.Format.B8G8R8A8_UNorm_SRgb:
                    typelessFormat = SharpDX.DXGI.Format.B8G8R8A8_Typeless; return true;
                case SharpDX.DXGI.Format.B8G8R8X8_Typeless:
                case SharpDX.DXGI.Format.B8G8R8X8_UNorm:
                case SharpDX.DXGI.Format.B8G8R8X8_UNorm_SRgb:
                    typelessFormat = SharpDX.DXGI.Format.B8G8R8X8_Typeless; return true;
                //case SharpDX.DXGI.Format.R10G10B10_XR_Bias_A2_UNorm,
                case SharpDX.DXGI.Format.BC6H_Typeless:
                case SharpDX.DXGI.Format.BC6H_Uf16:
                case SharpDX.DXGI.Format.BC6H_Sf16:
                    typelessFormat = SharpDX.DXGI.Format.BC6H_Typeless; return true;
                case SharpDX.DXGI.Format.BC7_Typeless:
                case SharpDX.DXGI.Format.BC7_UNorm:
                case SharpDX.DXGI.Format.BC7_UNorm_SRgb:
                    typelessFormat = SharpDX.DXGI.Format.BC7_Typeless; return true;
                //default: throw new ArgumentException(string.Format("Format '{0}' does not have a typeless equivalent.", dxgiFormat.ToString()));
            }
            typelessFormat = SharpDX.DXGI.Format.Unknown;
            return false;
        }
        #endregion

        #region PrimitiveTopology
        public static SharpDX.Direct3D.PrimitiveTopology PrimitiveTopology(Beholder.Core.PrimitiveTopology bPrimitiveTopology)
        {
            return (SharpDX.Direct3D.PrimitiveTopology)bPrimitiveTopology;
        }
        #endregion

        #region Viewport
        public static void Viewport(ref Beholder.Core.Viewport bViewport, out SharpDX.ViewportF d3dViewport)
        {
            d3dViewport = new SharpDX.ViewportF
            {
                X = bViewport.Left,
                Y = bViewport.Top,
                Width = bViewport.Width,
                Height = bViewport.Height,
                MinDepth = bViewport.MinDepth,
                MaxDepth = bViewport.MaxDepth
            };
        }
        #endregion

        #region DeviceCreationFlags
        public static SharpDX.Direct3D11.DeviceCreationFlags DeviceCreationFlags(Beholder.Platform.DeviceInitializationFlags bFlags)
        {
            return (SharpDX.Direct3D11.DeviceCreationFlags)bFlags;
        }
        #endregion

        #region RenderTargetViewDimension
        public static SharpDX.Direct3D11.RenderTargetViewDimension RenderTargetViewDimension(Beholder.Resources.RenderTargetViewDimension bRTVDim)
        {
            return (SharpDX.Direct3D11.RenderTargetViewDimension)bRTVDim;
        }
        #endregion

        #region DepthStencilViewDimension
        public static SharpDX.Direct3D11.DepthStencilViewDimension DepthStencilViewDimension(Beholder.Resources.DepthStencilViewDimension bDSVDim)
        {
            return (SharpDX.Direct3D11.DepthStencilViewDimension)bDSVDim;
        }
        #endregion

        #region DepthStencilViewFlags
        public static SharpDX.Direct3D11.DepthStencilViewFlags DepthStencilViewFlags(Beholder.Resources.DepthStencilViewFlags bFlags)
        {
            return (SharpDX.Direct3D11.DepthStencilViewFlags)bFlags;
        }
        #endregion

        #region ShaderResourceViewDimension
        public static SharpDX.Direct3D.ShaderResourceViewDimension ShaderResourceViewDimension(Beholder.Resources.ShaderResourceViewDimension bSRVDim)
        {
            return (SharpDX.Direct3D.ShaderResourceViewDimension)bSRVDim;
        }
        #endregion

        #region ShaderResourceViewExtendedBufferFlags
        public static SharpDX.Direct3D11.ShaderResourceViewExtendedBufferFlags ShaderResourceViewExtendedBufferFlags(Beholder.Resources.ShaderResourceViewExtendedBufferFlags bFlags)
        {
            return (SharpDX.Direct3D11.ShaderResourceViewExtendedBufferFlags)bFlags;
        }
        #endregion

        #region UnorderedAccessViewDimension
        public static SharpDX.Direct3D11.UnorderedAccessViewDimension UnorderedAccessViewDimension(Beholder.Resources.UnorderedAccessViewDimension bUAVDim)
        {
            return (SharpDX.Direct3D11.UnorderedAccessViewDimension)bUAVDim;
        }
        #endregion

        #region UnorderedAccessViewBufferFlags
        public static SharpDX.Direct3D11.UnorderedAccessViewBufferFlags UnorderedAccessViewBufferFlags(Beholder.Resources.UnorderedAccessViewBufferFlags bFlags)
        {
            return (SharpDX.Direct3D11.UnorderedAccessViewBufferFlags)bFlags;
        }
        #endregion

        #region SampleDescription
        public static SharpDX.DXGI.SampleDescription SampleDescription(Beholder.Platform.Sampling bSampling)
        {
            return new SharpDX.DXGI.SampleDescription { Count = bSampling.Count, Quality = bSampling.Quality };
        }
        #endregion

        #region ResourceUsage
        public static SharpDX.Direct3D11.ResourceUsage ResourceUsage(Beholder.Resources.Usage bUsage)
        {
            switch (bUsage)
            {
                case Beholder.Resources.Usage.Immutable: return SharpDX.Direct3D11.ResourceUsage.Immutable;
                case Beholder.Resources.Usage.Default: return SharpDX.Direct3D11.ResourceUsage.Default;
                case Beholder.Resources.Usage.Dynamic: return SharpDX.Direct3D11.ResourceUsage.Dynamic;
                case Beholder.Resources.Usage.Staging: return SharpDX.Direct3D11.ResourceUsage.Staging;
                default: throw new ArgumentOutOfRangeException("bUsage");
            }
        }
        #endregion

        #region BindFlags
        public static SharpDX.Direct3D11.BindFlags BindFlags(Beholder.Resources.BindFlags bFlags)
        {
            return (SharpDX.Direct3D11.BindFlags)bFlags;
        }
        #endregion

        #region CpuAccessFlags
        public static SharpDX.Direct3D11.CpuAccessFlags CpuAccessFlags(Beholder.Resources.CpuAccessFlags bFlags)
        {
            return (SharpDX.Direct3D11.CpuAccessFlags)bFlags;
        }

        public static SharpDX.Direct3D11.CpuAccessFlags CpuAccessFlags(Beholder.Resources.Usage bUsage)
        {
            switch (bUsage)
            {
                case Beholder.Resources.Usage.Immutable:
                case Beholder.Resources.Usage.Default:
                    return SharpDX.Direct3D11.CpuAccessFlags.None;
                case Beholder.Resources.Usage.Dynamic:
                    return SharpDX.Direct3D11.CpuAccessFlags.Write;
                case Beholder.Resources.Usage.Staging:
                    return SharpDX.Direct3D11.CpuAccessFlags.Read;
                default: throw new ArgumentOutOfRangeException("bUsage");
            }
        }
        #endregion

        #region ResourceOptionFlags
        public static SharpDX.Direct3D11.ResourceOptionFlags ResourceOptionFlags(Beholder.Resources.MiscFlags bFlags)
        {
            return (SharpDX.Direct3D11.ResourceOptionFlags)bFlags;
        }
        #endregion

        #region DepthStencilClearFlags
        public static SharpDX.Direct3D11.DepthStencilClearFlags DepthStencilClearFlags(Beholder.Core.ClearDepthStencilFlags bFlags)
        {
            return (SharpDX.Direct3D11.DepthStencilClearFlags)bFlags;
        }
        #endregion

        #region Filter
        public static SharpDX.Direct3D11.Filter Filter(Beholder.Core.Filter bFilter)
        {
            return (SharpDX.Direct3D11.Filter)bFilter;
        }
        #endregion

        #region TextureAddressMode
        public static SharpDX.Direct3D11.TextureAddressMode TextureAddressMode(Beholder.Core.TextureAddressMode bTextureAddressMode)
        {
            return (SharpDX.Direct3D11.TextureAddressMode)bTextureAddressMode;
        }
        #endregion

        #region VertexLayoutClassification
        public static SharpDX.Direct3D11.InputClassification InputClassification(Beholder.Core.VertexLayoutClassification bVertexLayoutClassification)
        {
            return (SharpDX.Direct3D11.InputClassification)bVertexLayoutClassification;
        }
        #endregion

        #region Rational
        public static SharpDX.DXGI.Rational Rational(Math.Rational bRational)
        {
            return new SharpDX.DXGI.Rational { Numerator = bRational.Numerator, Denominator = bRational.Denominator };
        }
        #endregion

        #region Format
        public static SharpDX.DXGI.Format Format(Beholder.Core.ExplicitFormat bExplicitFormat)
        {
            return (SharpDX.DXGI.Format)bExplicitFormat;
        }

        public static SharpDX.DXGI.Format Format(Beholder.Core.IndexFormat bIndexFormat)
        {
            switch (bIndexFormat)
            {
                case Beholder.Core.IndexFormat.Undefined: return SharpDX.DXGI.Format.Unknown;
                case Beholder.Core.IndexFormat.SixteenBit: return SharpDX.DXGI.Format.R16_UInt;
                case Beholder.Core.IndexFormat.ThirtyTwoBit: return SharpDX.DXGI.Format.R32_UInt;
                default: throw new ArgumentOutOfRangeException("bIndexFormat");
            }
        }
        #endregion

        #region ResourceRegion
        public static SharpDX.Direct3D11.ResourceRegion ResourceRegion(Beholder.Resources.Box bBox)
        {
            return new SharpDX.Direct3D11.ResourceRegion(bBox.Left, bBox.Top, bBox.Front, bBox.Right, bBox.Bottom, bBox.Back);
        }
        #endregion

        #region MapMode
        public static SharpDX.Direct3D11.MapMode MapMode(Beholder.Resources.MapType bMapType)
        {
            return (SharpDX.Direct3D11.MapMode)bMapType;
        }
        #endregion

        #region MapFlags
        public static SharpDX.Direct3D11.MapFlags MapFlags(Beholder.Resources.MapFlags bMapFlags)
        {
            return (SharpDX.Direct3D11.MapFlags)bMapFlags;
        }
        #endregion
    }
}
