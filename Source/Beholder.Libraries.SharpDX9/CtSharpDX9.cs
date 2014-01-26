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
using Beholder.Math;
using Beholder.Utility;

namespace Beholder.Libraries.SharpDX9
{
    public static class CtSharpDX9
    {
        #region DeclarationType
        public static SharpDX.Direct3D9.DeclarationType DeclarationType(ExplicitFormat cFormat)
        {
            switch (cFormat)
            {
                case ExplicitFormat.R32_FLOAT: return SharpDX.Direct3D9.DeclarationType.Float1;
                case ExplicitFormat.R32G32_FLOAT: return SharpDX.Direct3D9.DeclarationType.Float2;
                case ExplicitFormat.R32G32B32_FLOAT: return SharpDX.Direct3D9.DeclarationType.Float3;
                case ExplicitFormat.R32G32B32A32_FLOAT: return SharpDX.Direct3D9.DeclarationType.Float4;
                case ExplicitFormat.R16G16_SINT: return SharpDX.Direct3D9.DeclarationType.Short2;
                case ExplicitFormat.R16G16_SNORM: return SharpDX.Direct3D9.DeclarationType.Short2N;
                case ExplicitFormat.R16G16B16A16_SINT: return SharpDX.Direct3D9.DeclarationType.Short4;
                case ExplicitFormat.R16G16B16A16_SNORM: return SharpDX.Direct3D9.DeclarationType.Short4N;
                case ExplicitFormat.R8G8B8A8_UINT: return SharpDX.Direct3D9.DeclarationType.Ubyte4;
                case ExplicitFormat.R8G8B8A8_UNORM: return SharpDX.Direct3D9.DeclarationType.UByte4N;
                case ExplicitFormat.R16G16_UNORM: return SharpDX.Direct3D9.DeclarationType.UShort2N;
                case ExplicitFormat.R16G16B16A16_UNORM: return SharpDX.Direct3D9.DeclarationType.UShort4N;
                default: throw new ShouldNeverHappenException(string.Format("Vertex element format '{0}' is not supported by SharpDX9Graphics.", cFormat.ToString()));
            }
        }
        #endregion

        #region PrimitiveType
        public static SharpDX.Direct3D9.PrimitiveType PrimitiveType(PrimitiveTopology primitiveTopology)
        {
            switch (primitiveTopology)
            {
                case PrimitiveTopology.PointList: return SharpDX.Direct3D9.PrimitiveType.PointList;
                case PrimitiveTopology.LineList: return SharpDX.Direct3D9.PrimitiveType.LineList;
                case PrimitiveTopology.LineStrip: return SharpDX.Direct3D9.PrimitiveType.LineStrip;
                case PrimitiveTopology.TriangleList: return SharpDX.Direct3D9.PrimitiveType.TriangleList;
                case PrimitiveTopology.TriangleStrip: return SharpDX.Direct3D9.PrimitiveType.TriangleStrip;
                default: throw new NotSupportedException(string.Format(
                    "Primitive topology '{0}' is not supported by the SharpDX9 implementation of Beholder.", primitiveTopology.ToString()));
            }
        }
        #endregion

        #region Image File Format
        public static SharpDX.Direct3D9.ImageFileFormat ImageFileFormat(string imageFormat)
        {
            switch (imageFormat)
            {
                case "BMP":
                    return SharpDX.Direct3D9.ImageFileFormat.Bmp;
                case "JPG":
                case "JPEG":
                    return SharpDX.Direct3D9.ImageFileFormat.Jpg;
                case "TGA":
                    return SharpDX.Direct3D9.ImageFileFormat.Tga;
                case "PNG":
                    return SharpDX.Direct3D9.ImageFileFormat.Png;
                case "DDS":
                    return SharpDX.Direct3D9.ImageFileFormat.Dds;
                case "DIB":
                    return SharpDX.Direct3D9.ImageFileFormat.Dib;
                case "HDR":
                    return SharpDX.Direct3D9.ImageFileFormat.Hdr;
                case "PFM":
                    return SharpDX.Direct3D9.ImageFileFormat.Pfm;
                case "PPM":
                    return SharpDX.Direct3D9.ImageFileFormat.Ppm;
                default:
                    throw new NotSupportedException(string.Format("Unkown image file format '{0}' found.", imageFormat));
            }
        }

        #endregion

        #region Viewport
        public static SharpDX.Direct3D9.Viewport Viewport(Viewport bViewport)
        {
            return new SharpDX.Direct3D9.Viewport
            {
                X = (int)System.Math.Round(bViewport.Left),
                Y = (int)System.Math.Round(bViewport.Top),
                Width = (int)System.Math.Round(bViewport.Width),
                Height = (int)System.Math.Round(bViewport.Height),
                MinZ = (int)System.Math.Round(bViewport.MinDepth),
                MaxZ = (int)System.Math.Round(bViewport.MaxDepth)
            };
        }
        #endregion

        #region PrimitiveCount
        public static int PrimitiveCount(SharpDX.Direct3D9.PrimitiveType d3dPrimitiveType, int indicesCount)
        {
            switch (d3dPrimitiveType)
            {
                case SharpDX.Direct3D9.PrimitiveType.PointList: return indicesCount;
                case SharpDX.Direct3D9.PrimitiveType.LineStrip: return indicesCount - 1;
                case SharpDX.Direct3D9.PrimitiveType.LineList: return indicesCount / 2;
                case SharpDX.Direct3D9.PrimitiveType.TriangleStrip: return indicesCount - 2;
                case SharpDX.Direct3D9.PrimitiveType.TriangleFan: return indicesCount - 2;
                case SharpDX.Direct3D9.PrimitiveType.TriangleList: return indicesCount / 3;
                default: throw new ArgumentOutOfRangeException("d3dPrimitiveType");
            }
        }
        #endregion

        #region MultisampleType
        public static SharpDX.Direct3D9.MultisampleType MultisampleType(int sampleCount)
        {
            if (sampleCount == 1) 
                sampleCount = 0;
            return (SharpDX.Direct3D9.MultisampleType)sampleCount;
        }
        #endregion

        #region TextureAddress
        public static SharpDX.Direct3D9.TextureAddress TextureAddress(TextureAddressMode bAddressMode)
        {
            switch (bAddressMode)
            {
                case TextureAddressMode.Wrap: return SharpDX.Direct3D9.TextureAddress.Wrap;
                case TextureAddressMode.Mirror: return SharpDX.Direct3D9.TextureAddress.Mirror;
                case TextureAddressMode.Clamp: return SharpDX.Direct3D9.TextureAddress.Clamp;
                case TextureAddressMode.Border: return SharpDX.Direct3D9.TextureAddress.Border;
                case TextureAddressMode.MirrorOnce: return SharpDX.Direct3D9.TextureAddress.MirrorOnce;
                default: throw new ShouldNeverHappenException();
            }
        }
        #endregion

        #region StreamSource
        public static SharpDX.Direct3D9.StreamSource StreamSource(VertexLayoutClassification bVertexLayoutClass)
        {
            return bVertexLayoutClass == VertexLayoutClassification.PerVertexData 
                ? SharpDX.Direct3D9.StreamSource.IndexedData 
                : SharpDX.Direct3D9.StreamSource.InstanceData;
        }
        #endregion

        #region ClearFlags
        public static SharpDX.Direct3D9.ClearFlags ClearFlags(ClearDepthStencilFlags bClearDepthStencilFlags)
        {
            SharpDX.Direct3D9.ClearFlags d3dClearFlags = 0;
            if ((bClearDepthStencilFlags & ClearDepthStencilFlags.DepthOnly) != 0) d3dClearFlags |= SharpDX.Direct3D9.ClearFlags.ZBuffer;
            if ((bClearDepthStencilFlags & ClearDepthStencilFlags.StencilOnly) != 0) d3dClearFlags |= SharpDX.Direct3D9.ClearFlags.Stencil;
            return d3dClearFlags;
        }
        #endregion

        #region Rectangle
        public static SharpDX.Rectangle Rectangle(IntRectangle bRectangle)
        {
            return new SharpDX.Rectangle(bRectangle.X, bRectangle.Y, bRectangle.Width + bRectangle.X, bRectangle.Height + bRectangle.Y);
        }
        #endregion
    }
}
