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

using Beholder.Core;
using Beholder.Platform;
using Beholder.Resources;
using Beholder.Utility;
using Beholder.Utility.ForImplementations.Platform;

namespace Beholder.Libraries.SharpDX11
{
    static class CtBeholder
    {
        #region Rational
        public static Math.Rational Rational(SharpDX.DXGI.Rational sdxRational)
        {
            return new Math.Rational
            {
                Numerator = sdxRational.Numerator,
                Denominator = sdxRational.Denominator
            };
        }
        #endregion

        #region FormatInfo
        public static IFormatInfo FormatInfo(SharpDX.DXGI.Format dxgiFormat)
        {
            int numColors;
            int colorBits;
            FormatElementType colorFormatType;
            int alphaBits;
            FormatElementType alphaFormatType;
            int totalBits;
            ColorAlphaFormatFlags flags;

            Utility.Helpers.FormatHelper.GetExplicitColorAlphaFormatInfo((ExplicitFormat)dxgiFormat,
                out numColors, out colorBits, out colorFormatType, out alphaBits, out alphaFormatType, out totalBits, out flags);

            return new FormatInfo
            {
                ID = (int)dxgiFormat,
                Description = dxgiFormat.ToString(),
                ExplicitFormat = (ExplicitFormat)dxgiFormat,
                NumColors = numColors,
                ColorBits = colorBits,
                ColorFormatType = colorFormatType,
                AlphaBits = alphaBits,
                AlphaFormatType = alphaFormatType,
                TotalBits = totalBits,
                Flags = flags
            };
        }
        #endregion
        /*
        #region DepthStencilFormatInfo
        public static Platform.IDepthStencilFormatInfo DepthStencilFormatInfo(SharpDX.DXGI.Format dxgiFormat)
        {
            int depthBits;
            Platform.FormatElementType depthFormatType;
            int stencilBits;
            int totalBits;

            Utility.Helpers.FormatHelper.GetExplicitDepthStencilFormatInfo((Platform.ExplicitFormat)dxgiFormat, 
                out depthBits, out depthFormatType, out stencilBits, out totalBits);

            return new CDepthStencilFormatInfo
            {
                ID = (int)dxgiFormat,
                Description = dxgiFormat.ToString(),
                ExplicitFormat = (Platform.ExplicitFormat)dxgiFormat,
                DepthBits = depthBits,
                DepthFormatType = depthFormatType,
                StencilBits = stencilBits,
                TotalBits = totalBits
            };
        }
        #endregion*/

        #region ModeRoataion
        public static ModeRotation ModeRotation(SharpDX.DXGI.DisplayModeRotation dxgiModeRotation)
        {
            switch (dxgiModeRotation)
            {
                case SharpDX.DXGI.DisplayModeRotation.Unspecified: return Beholder.Platform.ModeRotation.Unspecified;
                case SharpDX.DXGI.DisplayModeRotation.Identity: return Beholder.Platform.ModeRotation.Identity;
                case SharpDX.DXGI.DisplayModeRotation.Rotate90: return Beholder.Platform.ModeRotation.Rotate90;
                case SharpDX.DXGI.DisplayModeRotation.Rotate180: return Beholder.Platform.ModeRotation.Rotate180;
                case SharpDX.DXGI.DisplayModeRotation.Rotate270: return Beholder.Platform.ModeRotation.Rotate270;
                default: throw new ShouldNeverHappenException("Unknown SharpDX.DXGI.DisplayModeRotation found.");
            }
        }
        #endregion

        #region AdapterFlags
        public static AdapterFlags AdapterFlags(SharpDX.DXGI.AdapterFlags dxgiAdapterFlags)
        {
            return (AdapterFlags)dxgiAdapterFlags;
        }
        #endregion

        #region Sampling
        public static Sampling Sampling(SharpDX.DXGI.SampleDescription dxgiSampleDesc)
        {
            return new Sampling { Count = (ushort)dxgiSampleDesc.Count, Quality = (ushort)dxgiSampleDesc.Quality };
        }
        #endregion

        #region Usage
        public static Usage Usage(SharpDX.Direct3D11.ResourceUsage d3dResourceUsage)
        {
            return (Usage)d3dResourceUsage;
        }
        #endregion

        #region BindFlags
        public static BindFlags BindFlags(SharpDX.Direct3D11.BindFlags d3dFlags)
        {
            return (BindFlags)d3dFlags;
        }
        #endregion

        #region MiscFlags
        public static MiscFlags MiscFlags(SharpDX.Direct3D11.ResourceOptionFlags d3dFlags)
        {
            return (MiscFlags)d3dFlags;
        }
        #endregion

        #region PrimitiveTopology
        public static PrimitiveTopology PrimitiveTopology(SharpDX.Direct3D.PrimitiveTopology d3dPrimitiveTopology)
        {
            return (PrimitiveTopology)d3dPrimitiveTopology;
        }
        #endregion

        #region DeviceContextType
        public static DeviceContextType DeviceContextType(SharpDX.Direct3D11.DeviceContextType d3dDeviceContextType)
        {
            return (DeviceContextType)d3dDeviceContextType;
        }
        #endregion
    }
}
