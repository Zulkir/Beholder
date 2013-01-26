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
using Beholder.Libraries.ObjectGL4.Core;
using Beholder.Math;
using Beholder.Platform;
using ObjectGL.GL42;
using OpenTK.Graphics.OpenGL;

namespace Beholder.Libraries.ObjectGL4
{
    public static class CtObjectGL
    {
        #region ColorAlphaFormat
        public static Format ColorAlphaFormat(int bpp)
        {
            switch (bpp)
            {
                case 128: return Format.Rgba32f;
                case 64: return Format.Rgba16f;
                case 32: return Format.Rgba8;
                case 16: return Format.R16;
                case 8: return Format.R8;
                default: throw new ArgumentOutOfRangeException("bpp");
            }
        }
        #endregion

        #region DepthStencilFormat
        public static OpenTK.Graphics.OpenGL.PixelInternalFormat DepthStencilFormat(ExplicitFormat bFormat)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region ColorFormat
        public static OpenTK.Graphics.ColorFormat ColorFormat(ObjectGL.GL42.Format glPixelInternalFormat)
        {
            switch (glPixelInternalFormat)
            {
                case ObjectGL.GL42.Format.Rgba32f: return new OpenTK.Graphics.ColorFormat(32, 32, 32, 32);
                case ObjectGL.GL42.Format.Rgb32f: return new OpenTK.Graphics.ColorFormat(32, 32, 32, 0);
                case ObjectGL.GL42.Format.Rgba16: return new OpenTK.Graphics.ColorFormat(16, 16, 16, 16);
                case ObjectGL.GL42.Format.Rg32f: return new OpenTK.Graphics.ColorFormat(32, 32, 0, 0);
                case ObjectGL.GL42.Format.Rgb10A2: return new OpenTK.Graphics.ColorFormat(10, 10, 10, 2);
                case ObjectGL.GL42.Format.R11fG11fB10f: return new OpenTK.Graphics.ColorFormat(11, 11, 10, 0);
                case ObjectGL.GL42.Format.Rgba8: return new OpenTK.Graphics.ColorFormat(8, 8, 8, 8);
                case ObjectGL.GL42.Format.Rg16: return new OpenTK.Graphics.ColorFormat(16, 16, 0, 0);
                case ObjectGL.GL42.Format.R32f: return new OpenTK.Graphics.ColorFormat(32, 0, 0, 0);
                case ObjectGL.GL42.Format.Rg8: return new OpenTK.Graphics.ColorFormat(8, 8, 0, 0);
                case ObjectGL.GL42.Format.R16: return new OpenTK.Graphics.ColorFormat(16, 0, 0, 0);
                case ObjectGL.GL42.Format.R8: return new OpenTK.Graphics.ColorFormat(8, 0, 0, 0);
                default: throw new ArgumentOutOfRangeException("glPixelInternalFormat");
            }
        }
        #endregion

        #region DepthStencilBits
        public static void DepthStencilBits(OpenTK.Graphics.OpenGL.PixelInternalFormat glPixelInternalFormat, out int depthBits, out int stencilBits)
        {
            switch (glPixelInternalFormat)
            {
                case OpenTK.Graphics.OpenGL.PixelInternalFormat.Depth32fStencil8:
                    depthBits = 32;
                    stencilBits = 8;
                    return;
                case OpenTK.Graphics.OpenGL.PixelInternalFormat.DepthComponent32f:
                    depthBits = 32;
                    stencilBits = 0;
                    return;
                case OpenTK.Graphics.OpenGL.PixelInternalFormat.Depth24Stencil8:
                    depthBits = 24;
                    stencilBits = 8;
                    return;
                case OpenTK.Graphics.OpenGL.PixelInternalFormat.DepthComponent16:
                    depthBits = 16;
                    stencilBits = 0;
                    return;
                default: throw new ArgumentOutOfRangeException("glPixelInternalFormat");
            }
        }
        #endregion

        #region GraphicsContextFlags
        public static OpenTK.Graphics.GraphicsContextFlags GraphicsContextFlags(DeviceInitializationFlags bFlags)
        {
            var glFlags = OpenTK.Graphics.GraphicsContextFlags.ForwardCompatible;
            if ((bFlags & DeviceInitializationFlags.Debug) != 0) glFlags |= OpenTK.Graphics.GraphicsContextFlags.Debug;
            return glFlags;
        }
        #endregion

        #region ClearBuffer
        public static OpenTK.Graphics.OpenGL.ClearBuffer ClearBuffer(ClearDepthStencilFlags bFlags)
        {
            switch (bFlags)
            {
                case ClearDepthStencilFlags.DepthOnly: return OpenTK.Graphics.OpenGL.ClearBuffer.Depth;
                case ClearDepthStencilFlags.StencilOnly: return OpenTK.Graphics.OpenGL.ClearBuffer.Stencil;
                case ClearDepthStencilFlags.All: return OpenTK.Graphics.OpenGL.ClearBuffer.DepthStencil;
                default: throw new ArgumentOutOfRangeException("bFlags");
            }
        }
        #endregion

        #region DepthStencil
        public static DepthStencil DepthStencil(ClearDepthStencilFlags bFlags)
        {
            switch (bFlags)
            {
                case 0: return ObjectGL.GL42.DepthStencil.None;
                case ClearDepthStencilFlags.DepthOnly: return ObjectGL.GL42.DepthStencil.Depth;
                case ClearDepthStencilFlags.StencilOnly: return ObjectGL.GL42.DepthStencil.Stencil;
                case ClearDepthStencilFlags.All: return ObjectGL.GL42.DepthStencil.Both;
                default: throw new ArgumentOutOfRangeException("bFlags");
            }
        }
        #endregion

        #region CompareFunc
        public static ObjectGL.GL42.CompareFunc CompareFunc(Beholder.Core.Comparison bComparison)
        {
            switch (bComparison)
            {
                case Comparison.Never: return ObjectGL.GL42.CompareFunc.Never;
                case Comparison.Less: return ObjectGL.GL42.CompareFunc.Less;
                case Comparison.Equal: return ObjectGL.GL42.CompareFunc.Equal;
                case Comparison.LessEqual: return ObjectGL.GL42.CompareFunc.Lequal;
                case Comparison.Greater: return ObjectGL.GL42.CompareFunc.Greater;
                case Comparison.NotEqual: return ObjectGL.GL42.CompareFunc.Notequal;
                case Comparison.GreaterEqual: return ObjectGL.GL42.CompareFunc.Gequal;
                case Comparison.Always: return ObjectGL.GL42.CompareFunc.Always;
                default: throw new ArgumentOutOfRangeException("bComparison");
            }
        }
        #endregion

        #region TextureWrapMode
        public static TextureWrapMode TextureWrapMode(Beholder.Core.TextureAddressMode bMode)
        {
            switch (bMode)
            {
                case TextureAddressMode.Wrap: return OpenTK.Graphics.OpenGL.TextureWrapMode.Repeat;
                case TextureAddressMode.Mirror: return OpenTK.Graphics.OpenGL.TextureWrapMode.MirroredRepeat;
                case TextureAddressMode.Clamp: return OpenTK.Graphics.OpenGL.TextureWrapMode.ClampToEdge;
                case TextureAddressMode.Border: return OpenTK.Graphics.OpenGL.TextureWrapMode.ClampToBorder;
                case TextureAddressMode.MirrorOnce: throw new NotSupportedException("MirrorOnce teture address mode is not supported by OGL");
                default: throw new ArgumentOutOfRangeException("bMode");
            }
        }
        #endregion

        #region VertexElementFormat
        internal static void VertexElementFormat(ExplicitFormat bFormat, out int vertexAttribPointerType, 
            out VertexAttributeDimension dimension, out WVertexLayoutElementType type, out bool normalized)
        {
            switch (bFormat)
            {
                case ExplicitFormat.R32G32B32A32_FLOAT:
                    vertexAttribPointerType = (int)VertexAttribPointerType.Float;
                    dimension = VertexAttributeDimension.Four;
                    type = WVertexLayoutElementType.F;
                    normalized = false;
                    break;
                case ExplicitFormat.R32G32B32A32_UINT:
                    vertexAttribPointerType = (int)VertexAttribIPointerType.UnsignedInt;
                    dimension = VertexAttributeDimension.Four;
                    type = WVertexLayoutElementType.I;
                    normalized = false;
                    break;
                case ExplicitFormat.R32G32B32A32_SINT:
                    vertexAttribPointerType = (int)VertexAttribIPointerType.Int;
                    dimension = VertexAttributeDimension.Four;
                    type = WVertexLayoutElementType.I;
                    normalized = false;
                    break;
                case ExplicitFormat.R32G32B32_FLOAT:
                    vertexAttribPointerType = (int)VertexAttribPointerType.Float;
                    dimension = VertexAttributeDimension.Three;
                    type = WVertexLayoutElementType.F;
                    normalized = false;
                    break;
                case ExplicitFormat.R32G32B32_UINT:
                    vertexAttribPointerType = (int)VertexAttribIPointerType.UnsignedInt;
                    dimension = VertexAttributeDimension.Three;
                    type = WVertexLayoutElementType.I;
                    normalized = false;
                    break;
                case ExplicitFormat.R32G32B32_SINT:
                    vertexAttribPointerType = (int)VertexAttribIPointerType.Int;
                    dimension = VertexAttributeDimension.Three;
                    type = WVertexLayoutElementType.I;
                    normalized = false;
                    break;
                case ExplicitFormat.R16G16B16A16_FLOAT:
                    vertexAttribPointerType = (int)VertexAttribPointerType.HalfFloat;
                    dimension = VertexAttributeDimension.Four;
                    type = WVertexLayoutElementType.F;
                    normalized = false;
                    break;
                case ExplicitFormat.R16G16B16A16_UNORM:
                    vertexAttribPointerType = (int)VertexAttribPointerType.UnsignedShort;
                    dimension = VertexAttributeDimension.Four;
                    type = WVertexLayoutElementType.F;
                    normalized = true;
                    break;
                case ExplicitFormat.R16G16B16A16_UINT:
                    vertexAttribPointerType = (int)VertexAttribIPointerType.UnsignedShort;
                    dimension = VertexAttributeDimension.Four;
                    type = WVertexLayoutElementType.I;
                    normalized = false;
                    break;
                case ExplicitFormat.R16G16B16A16_SNORM:
                    vertexAttribPointerType = (int)VertexAttribPointerType.Short;
                    dimension = VertexAttributeDimension.Four;
                    type = WVertexLayoutElementType.F;
                    normalized = true;
                    break;
                case ExplicitFormat.R16G16B16A16_SINT:
                    vertexAttribPointerType = (int)VertexAttribIPointerType.Short;
                    dimension = VertexAttributeDimension.Four;
                    type = WVertexLayoutElementType.I;
                    normalized = false;
                    break;
                case ExplicitFormat.R32G32_FLOAT:
                    vertexAttribPointerType = (int)VertexAttribPointerType.Float;
                    dimension = VertexAttributeDimension.Two;
                    type = WVertexLayoutElementType.F;
                    normalized = false;
                    break;
                case ExplicitFormat.R32G32_UINT:
                    vertexAttribPointerType = (int)VertexAttribIPointerType.UnsignedInt;
                    dimension = VertexAttributeDimension.Two;
                    type = WVertexLayoutElementType.I;
                    normalized = false;
                    break;
                case ExplicitFormat.R32G32_SINT:
                    vertexAttribPointerType = (int)VertexAttribIPointerType.Int;
                    dimension = VertexAttributeDimension.Two;
                    type = WVertexLayoutElementType.I;
                    normalized = false;
                    break;
                case ExplicitFormat.R10G10B10A2_UNORM:
                    vertexAttribPointerType = (int)VertexAttribPointerType.UnsignedInt2101010Rev;
                    dimension = VertexAttributeDimension.Four;
                    type = WVertexLayoutElementType.F;
                    normalized = true;
                    break;
                case ExplicitFormat.R8G8B8A8_UNORM:
                    vertexAttribPointerType = (int)VertexAttribPointerType.UnsignedByte;
                    dimension = VertexAttributeDimension.Four;
                    type = WVertexLayoutElementType.F;
                    normalized = true;
                    break;
                case ExplicitFormat.R8G8B8A8_UINT:
                    vertexAttribPointerType = (int)VertexAttribIPointerType.UnsignedByte;
                    dimension = VertexAttributeDimension.Four;
                    type = WVertexLayoutElementType.I;
                    normalized = false;
                    break;
                case ExplicitFormat.R8G8B8A8_SNORM:
                    vertexAttribPointerType = (int)VertexAttribPointerType.Byte;
                    dimension = VertexAttributeDimension.Four;
                    type = WVertexLayoutElementType.F;
                    normalized = true;
                    break;
                case ExplicitFormat.R8G8B8A8_SINT:
                    vertexAttribPointerType = (int)VertexAttribIPointerType.Byte;
                    dimension = VertexAttributeDimension.Four;
                    type = WVertexLayoutElementType.I;
                    normalized = false;
                    break;
                case ExplicitFormat.R16G16_FLOAT:
                    vertexAttribPointerType = (int)VertexAttribPointerType.HalfFloat;
                    dimension = VertexAttributeDimension.Two;
                    type = WVertexLayoutElementType.F;
                    normalized = false;
                    break;
                case ExplicitFormat.R16G16_UNORM:
                    vertexAttribPointerType = (int)VertexAttribPointerType.UnsignedShort;
                    dimension = VertexAttributeDimension.Two;
                    type = WVertexLayoutElementType.F;
                    normalized = true;
                    break;
                case ExplicitFormat.R16G16_UINT:
                    vertexAttribPointerType = (int)VertexAttribIPointerType.UnsignedShort;
                    dimension = VertexAttributeDimension.Two;
                    type = WVertexLayoutElementType.I;
                    normalized = false;
                    break;
                case ExplicitFormat.R16G16_SNORM:
                    vertexAttribPointerType = (int)VertexAttribPointerType.Short;
                    dimension = VertexAttributeDimension.Two;
                    type = WVertexLayoutElementType.F;
                    normalized = true;
                    break;
                case ExplicitFormat.R16G16_SINT:
                    vertexAttribPointerType = (int)VertexAttribIPointerType.Short;
                    dimension = VertexAttributeDimension.Two;
                    type = WVertexLayoutElementType.I;
                    normalized = false;
                    break;
                case ExplicitFormat.R32_FLOAT:
                    vertexAttribPointerType = (int)VertexAttribPointerType.Float;
                    dimension = VertexAttributeDimension.One;
                    type = WVertexLayoutElementType.F;
                    normalized = false;
                    break;
                case ExplicitFormat.R32_UINT:
                    vertexAttribPointerType = (int)VertexAttribIPointerType.UnsignedInt;
                    dimension = VertexAttributeDimension.One;
                    type = WVertexLayoutElementType.I;
                    normalized = false;
                    break;
                case ExplicitFormat.R32_SINT:
                    vertexAttribPointerType = (int)VertexAttribIPointerType.Int;
                    dimension = VertexAttributeDimension.One;
                    type = WVertexLayoutElementType.I;
                    normalized = false;
                    break;
                case ExplicitFormat.R8G8_UNORM:
                    vertexAttribPointerType = (int)VertexAttribPointerType.UnsignedByte;
                    dimension = VertexAttributeDimension.Two;
                    type = WVertexLayoutElementType.F;
                    normalized = true;
                    break;
                case ExplicitFormat.R8G8_UINT:
                    vertexAttribPointerType = (int)VertexAttribIPointerType.UnsignedByte;
                    dimension = VertexAttributeDimension.Two;
                    type = WVertexLayoutElementType.I;
                    normalized = false;
                    break;
                case ExplicitFormat.R8G8_SNORM:
                    vertexAttribPointerType = (int)VertexAttribPointerType.Byte;
                    dimension = VertexAttributeDimension.Two;
                    type = WVertexLayoutElementType.F;
                    normalized = true;
                    break;
                case ExplicitFormat.R8G8_SINT:
                    vertexAttribPointerType = (int)VertexAttribIPointerType.Byte;
                    dimension = VertexAttributeDimension.Two;
                    type = WVertexLayoutElementType.I;
                    normalized = false;
                    break;
                case ExplicitFormat.R16_FLOAT:
                    vertexAttribPointerType = (int)VertexAttribPointerType.HalfFloat;
                    dimension = VertexAttributeDimension.One;
                    type = WVertexLayoutElementType.F;
                    normalized = false;
                    break;
                case ExplicitFormat.R16_UNORM:
                    vertexAttribPointerType = (int)VertexAttribPointerType.UnsignedShort;
                    dimension = VertexAttributeDimension.One;
                    type = WVertexLayoutElementType.F;
                    normalized = true;
                    break;
                case ExplicitFormat.R16_UINT:
                    vertexAttribPointerType = (int)VertexAttribIPointerType.UnsignedShort;
                    dimension = VertexAttributeDimension.One;
                    type = WVertexLayoutElementType.I;
                    normalized = false;
                    break;
                case ExplicitFormat.R16_SNORM:
                    vertexAttribPointerType = (int)VertexAttribPointerType.Short;
                    dimension = VertexAttributeDimension.One;
                    type = WVertexLayoutElementType.F;
                    normalized = true;
                    break;
                case ExplicitFormat.R16_SINT:
                    vertexAttribPointerType = (int)VertexAttribIPointerType.Short;
                    dimension = VertexAttributeDimension.One;
                    type = WVertexLayoutElementType.I;
                    normalized = false;
                    break;
                case ExplicitFormat.R8_UNORM:
                    vertexAttribPointerType = (int)VertexAttribPointerType.UnsignedByte;
                    dimension = VertexAttributeDimension.One;
                    type = WVertexLayoutElementType.F;
                    normalized = true;
                    break;
                case ExplicitFormat.R8_UINT:
                    vertexAttribPointerType = (int)VertexAttribIPointerType.UnsignedByte;
                    dimension = VertexAttributeDimension.One;
                    type = WVertexLayoutElementType.I;
                    normalized = false;
                    break;
                case ExplicitFormat.R8_SNORM:
                    vertexAttribPointerType = (int)VertexAttribPointerType.Byte;
                    dimension = VertexAttributeDimension.One;
                    type = WVertexLayoutElementType.F;
                    normalized = true;
                    break;
                case ExplicitFormat.R8_SINT:
                    vertexAttribPointerType = (int)VertexAttribIPointerType.Byte;
                    dimension = VertexAttributeDimension.One;
                    type = WVertexLayoutElementType.I;
                    normalized = false;
                    break;
                default:
                    throw new NotSupportedException(string.Format("'{0}' format is not supported as vertex format", bFormat));
            }
        }
        #endregion

        #region BeginMode
        public static void BeginMode(PrimitiveTopology bTopology, out BeginMode mode, out int patchVertexCount)
        {
            switch (bTopology)
            {
                case PrimitiveTopology.PointList: mode = OpenTK.Graphics.OpenGL.BeginMode.Points; patchVertexCount = 0; return;
                case PrimitiveTopology.LineList: mode = OpenTK.Graphics.OpenGL.BeginMode.Lines; patchVertexCount = 0; return;
                case PrimitiveTopology.LineStrip: mode = OpenTK.Graphics.OpenGL.BeginMode.LineStrip; patchVertexCount = 0; return;
                case PrimitiveTopology.TriangleList: mode = OpenTK.Graphics.OpenGL.BeginMode.Triangles; patchVertexCount = 0; return;
                case PrimitiveTopology.TriangleStrip: mode = OpenTK.Graphics.OpenGL.BeginMode.TriangleStrip; patchVertexCount = 0; return;
                case PrimitiveTopology.LineListWithAdjacency: mode = OpenTK.Graphics.OpenGL.BeginMode.LinesAdjacency; patchVertexCount = 0; return;
                case PrimitiveTopology.LineStripWithAdjacency: mode = OpenTK.Graphics.OpenGL.BeginMode.LineStripAdjacency; patchVertexCount = 0; return;
                case PrimitiveTopology.TriangleListWithAdjacency: mode = OpenTK.Graphics.OpenGL.BeginMode.TrianglesAdjacency; patchVertexCount = 0; return;
                case PrimitiveTopology.TriangleStripWithAdjacency: mode = OpenTK.Graphics.OpenGL.BeginMode.TriangleStripAdjacency; patchVertexCount = 0; return;
                case PrimitiveTopology.PatchList1: mode = OpenTK.Graphics.OpenGL.BeginMode.Patches; patchVertexCount = 1; return;
                case PrimitiveTopology.PatchList2: mode = OpenTK.Graphics.OpenGL.BeginMode.Patches; patchVertexCount = 2; return;
                case PrimitiveTopology.PatchList3: mode = OpenTK.Graphics.OpenGL.BeginMode.Patches; patchVertexCount = 3; return;
                case PrimitiveTopology.PatchList4: mode = OpenTK.Graphics.OpenGL.BeginMode.Patches; patchVertexCount = 4; return;
                case PrimitiveTopology.PatchList5: mode = OpenTK.Graphics.OpenGL.BeginMode.Patches; patchVertexCount = 5; return;
                case PrimitiveTopology.PatchList6: mode = OpenTK.Graphics.OpenGL.BeginMode.Patches; patchVertexCount = 6; return;
                case PrimitiveTopology.PatchList7: mode = OpenTK.Graphics.OpenGL.BeginMode.Patches; patchVertexCount = 7; return;
                case PrimitiveTopology.PatchList8: mode = OpenTK.Graphics.OpenGL.BeginMode.Patches; patchVertexCount = 8; return;
                case PrimitiveTopology.PatchList9: mode = OpenTK.Graphics.OpenGL.BeginMode.Patches; patchVertexCount = 9; return;
                case PrimitiveTopology.PatchList10: mode = OpenTK.Graphics.OpenGL.BeginMode.Patches; patchVertexCount = 10; return;
                case PrimitiveTopology.PatchList11: mode = OpenTK.Graphics.OpenGL.BeginMode.Patches; patchVertexCount = 11; return;
                case PrimitiveTopology.PatchList12: mode = OpenTK.Graphics.OpenGL.BeginMode.Patches; patchVertexCount = 12; return;
                case PrimitiveTopology.PatchList13: mode = OpenTK.Graphics.OpenGL.BeginMode.Patches; patchVertexCount = 13; return;
                case PrimitiveTopology.PatchList14: mode = OpenTK.Graphics.OpenGL.BeginMode.Patches; patchVertexCount = 14; return;
                case PrimitiveTopology.PatchList15: mode = OpenTK.Graphics.OpenGL.BeginMode.Patches; patchVertexCount = 15; return;
                case PrimitiveTopology.PatchList16: mode = OpenTK.Graphics.OpenGL.BeginMode.Patches; patchVertexCount = 16; return;
                case PrimitiveTopology.PatchList17: mode = OpenTK.Graphics.OpenGL.BeginMode.Patches; patchVertexCount = 17; return;
                case PrimitiveTopology.PatchList18: mode = OpenTK.Graphics.OpenGL.BeginMode.Patches; patchVertexCount = 18; return;
                case PrimitiveTopology.PatchList19: mode = OpenTK.Graphics.OpenGL.BeginMode.Patches; patchVertexCount = 19; return;
                case PrimitiveTopology.PatchList20: mode = OpenTK.Graphics.OpenGL.BeginMode.Patches; patchVertexCount = 20; return;
                case PrimitiveTopology.PatchList21: mode = OpenTK.Graphics.OpenGL.BeginMode.Patches; patchVertexCount = 21; return;
                case PrimitiveTopology.PatchList22: mode = OpenTK.Graphics.OpenGL.BeginMode.Patches; patchVertexCount = 22; return;
                case PrimitiveTopology.PatchList23: mode = OpenTK.Graphics.OpenGL.BeginMode.Patches; patchVertexCount = 23; return;
                case PrimitiveTopology.PatchList24: mode = OpenTK.Graphics.OpenGL.BeginMode.Patches; patchVertexCount = 24; return;
                case PrimitiveTopology.PatchList25: mode = OpenTK.Graphics.OpenGL.BeginMode.Patches; patchVertexCount = 25; return;
                case PrimitiveTopology.PatchList26: mode = OpenTK.Graphics.OpenGL.BeginMode.Patches; patchVertexCount = 26; return;
                case PrimitiveTopology.PatchList27: mode = OpenTK.Graphics.OpenGL.BeginMode.Patches; patchVertexCount = 27; return;
                case PrimitiveTopology.PatchList28: mode = OpenTK.Graphics.OpenGL.BeginMode.Patches; patchVertexCount = 28; return;
                case PrimitiveTopology.PatchList29: mode = OpenTK.Graphics.OpenGL.BeginMode.Patches; patchVertexCount = 29; return;
                case PrimitiveTopology.PatchList30: mode = OpenTK.Graphics.OpenGL.BeginMode.Patches; patchVertexCount = 30; return;
                case PrimitiveTopology.PatchList31: mode = OpenTK.Graphics.OpenGL.BeginMode.Patches; patchVertexCount = 31; return;
                case PrimitiveTopology.PatchList32: mode = OpenTK.Graphics.OpenGL.BeginMode.Patches; patchVertexCount = 32; return;
                default: throw new ArgumentOutOfRangeException("bTopology");
            }
        }
        #endregion

        #region PolygonMode
        public static PolygonMode PolygonMode(FillMode bFillMode)
        {
            switch (bFillMode)
            {
                case FillMode.Wireframe: return OpenTK.Graphics.OpenGL.PolygonMode.Line;
                case FillMode.Solid: return OpenTK.Graphics.OpenGL.PolygonMode.Fill;
                default: throw new ArgumentOutOfRangeException("bFillMode");
            }
        }
        #endregion

        #region CullEnable
        public static bool CullEnable(Cull bCull)
        {
            switch (bCull)
            {
                case Cull.None: return false;
                case Cull.Frontface:
                case Cull.Backface: return true;
                default: throw new ArgumentOutOfRangeException("bCull");
            }
        }
        #endregion

        #region CullFace
        public static CullFaceMode CullFace(Cull bCull)
        {
            switch (bCull)
            {
                // Swap is intentional: we render everything upside-down in the FBO
                case Cull.None: return CullFaceMode.FrontAndBack;
                case Cull.Frontface: return CullFaceMode.Back;
                case Cull.Backface: return CullFaceMode.Front;
                default: throw new ArgumentOutOfRangeException("bCull");
            }
        }
        #endregion

        #region FrontFace
        public static FrontFaceDirection FrontFace(Winding bWinding)
        {
            switch (bWinding)
            {
                case Winding.Clockwise: return FrontFaceDirection.Cw;
                case Winding.CounterClockwise: return FrontFaceDirection.Ccw;
                default: throw new ArgumentOutOfRangeException("bWinding");
            }
        }
        #endregion

        #region DepthFunction
        public static DepthFunction DepthFunction(Comparison bComparison)
        {
            switch (bComparison)
            {
                case Comparison.Never: return OpenTK.Graphics.OpenGL.DepthFunction.Never;
                case Comparison.Less: return OpenTK.Graphics.OpenGL.DepthFunction.Less;
                case Comparison.Equal: return OpenTK.Graphics.OpenGL.DepthFunction.Equal;
                case Comparison.LessEqual: return OpenTK.Graphics.OpenGL.DepthFunction.Lequal;
                case Comparison.Greater: return OpenTK.Graphics.OpenGL.DepthFunction.Greater;
                case Comparison.NotEqual: return OpenTK.Graphics.OpenGL.DepthFunction.Notequal;
                case Comparison.GreaterEqual: return OpenTK.Graphics.OpenGL.DepthFunction.Gequal;
                case Comparison.Always: return OpenTK.Graphics.OpenGL.DepthFunction.Always;
                default: throw new ArgumentOutOfRangeException("bComparison");
            }
        }
        #endregion

        #region StencilOp
        public static StencilOp StencilOp(StencilOperation bStencilOperation)
        {
            switch (bStencilOperation)
            {
                case StencilOperation.Keep: return OpenTK.Graphics.OpenGL.StencilOp.Keep;
                case StencilOperation.Zero: return OpenTK.Graphics.OpenGL.StencilOp.Zero;
                case StencilOperation.Replace: return OpenTK.Graphics.OpenGL.StencilOp.Replace;
                case StencilOperation.IncrementSaturate: return OpenTK.Graphics.OpenGL.StencilOp.Incr;
                case StencilOperation.DecrementSaturate: return OpenTK.Graphics.OpenGL.StencilOp.Decr;
                case StencilOperation.Invert: return OpenTK.Graphics.OpenGL.StencilOp.Invert;
                case StencilOperation.Increment: return OpenTK.Graphics.OpenGL.StencilOp.IncrWrap;
                case StencilOperation.Decrement: return OpenTK.Graphics.OpenGL.StencilOp.DecrWrap;
                default: throw new ArgumentOutOfRangeException("bStencilOperation");
            }
        }
        #endregion

        #region StencilFunc
        public static StencilFunction StencilFunction(Comparison bComparison)
        {
            switch (bComparison)
            {
                case Comparison.Never: return OpenTK.Graphics.OpenGL.StencilFunction.Never;
                case Comparison.Less: return OpenTK.Graphics.OpenGL.StencilFunction.Less;
                case Comparison.Equal: return OpenTK.Graphics.OpenGL.StencilFunction.Equal;
                case Comparison.LessEqual: return OpenTK.Graphics.OpenGL.StencilFunction.Lequal;
                case Comparison.Greater: return OpenTK.Graphics.OpenGL.StencilFunction.Greater;
                case Comparison.NotEqual: return OpenTK.Graphics.OpenGL.StencilFunction.Notequal;
                case Comparison.GreaterEqual: return OpenTK.Graphics.OpenGL.StencilFunction.Gequal;
                case Comparison.Always: return OpenTK.Graphics.OpenGL.StencilFunction.Always;
                default: throw new ArgumentOutOfRangeException("bComparison");
            }
        }
        #endregion

        #region BlendFactor
        public static BlendFactor BlendFactor(Blend bBlend)
        {
            switch (bBlend)
            {
                case Blend.Zero: return ObjectGL.GL42.BlendFactor.Zero;
                case Blend.One: return ObjectGL.GL42.BlendFactor.One;
                case Blend.SourceColor: return ObjectGL.GL42.BlendFactor.SrcColor;
                case Blend.InverseSourceColor: return ObjectGL.GL42.BlendFactor.OneMinusSrcColor;
                case Blend.SourceAlpha: return ObjectGL.GL42.BlendFactor.SrcAlpha;
                case Blend.InverseSourceAlpha: return ObjectGL.GL42.BlendFactor.OneMinusSrcAlpha;
                case Blend.DestinationAlpha: return ObjectGL.GL42.BlendFactor.DstAlpha;
                case Blend.InverseDestinationAlpha: return ObjectGL.GL42.BlendFactor.OneMinusDstAlpha;
                case Blend.DestinationColor: return ObjectGL.GL42.BlendFactor.DstColor;
                case Blend.InverseDestinationColor: return ObjectGL.GL42.BlendFactor.OneMinusDstColor;
                case Blend.SourceAlphaSaturation: return ObjectGL.GL42.BlendFactor.SrcAlphaSaturate; // todo: make sure. It may differ when Src Alpha is greater than 1 - Dest Alpha
                case Blend.BlendFactor: return ObjectGL.GL42.BlendFactor.ConstantColor;
                case Blend.InverseBlendFactor: return ObjectGL.GL42.BlendFactor.OneMinusConstantColor;
                case Blend.SourceOneColor: return ObjectGL.GL42.BlendFactor.Src1Color;
                case Blend.InverseSourceOneColor: return ObjectGL.GL42.BlendFactor.OneMinusSrc1Color;
                case Blend.SourceOneAlpha: return ObjectGL.GL42.BlendFactor.Src1Alpha;
                case Blend.InverseSourceOneAlpha: return ObjectGL.GL42.BlendFactor.OneMinusSrc1Alpha;
                default: throw new ArgumentOutOfRangeException("bBlend");
            }
        }
        #endregion

        #region BlendFunc
        public static BlendFunc BlendFunc(BlendOperation blendOperation)
        {
            switch (blendOperation)
            {
                case BlendOperation.Add: return ObjectGL.GL42.BlendFunc.Add;
                case BlendOperation.Subtract: return ObjectGL.GL42.BlendFunc.Subtract;
                case BlendOperation.ReverseSubtract: return ObjectGL.GL42.BlendFunc.ReverseSubract;
                case BlendOperation.Minimum: return ObjectGL.GL42.BlendFunc.Min;
                case BlendOperation.Maximum: return ObjectGL.GL42.BlendFunc.Max;
                default: throw new ArgumentOutOfRangeException("blendOperation");
            }
        }
        #endregion

        #region ByteAlignment
        public static ByteAlignment ByteAlignment(int byteAlignment)
        {
            switch (byteAlignment)
            {
                case 1: return ObjectGL.GL42.ByteAlignment.One;
                case 2: return ObjectGL.GL42.ByteAlignment.Two;
                case 4: return ObjectGL.GL42.ByteAlignment.Four;
                case 8: return ObjectGL.GL42.ByteAlignment.Eight;
                default: throw new NotSupportedException(string.Format("Byte alignment '{0}' is not supported by OpenGL", byteAlignment));
            }
        }
        #endregion

        #region SampleCount
        public static int SampleCount(Sampling bSampling)
        {
            return bSampling.Count > 1 ? bSampling.Count : 0;
        }
        #endregion

        #region CubemapFace
        public static CubemapFace CubemapFace(int faceIndex)
        {
            switch (faceIndex)
            {
                case 0: return ObjectGL.GL42.CubemapFace.PositiveX;
                case 1: return ObjectGL.GL42.CubemapFace.NegativeX;
                case 2: return ObjectGL.GL42.CubemapFace.PositiveY;
                case 3: return ObjectGL.GL42.CubemapFace.NegativeY;
                case 4: return ObjectGL.GL42.CubemapFace.PositiveZ;
                case 5: return ObjectGL.GL42.CubemapFace.NegativeZ;
                default: throw new ArgumentOutOfRangeException("faceIndex");
            }
        }
        #endregion

        #region DrawElementsType
        public static DrawElementsType DrawElementsType(IndexFormat bIndexFormat)
        {
            switch (bIndexFormat)
            {
                case IndexFormat.SixteenBit: return OpenTK.Graphics.OpenGL.DrawElementsType.UnsignedShort;
                case IndexFormat.ThirtyTwoBit: return OpenTK.Graphics.OpenGL.DrawElementsType.UnsignedInt;
                default: throw new ArgumentOutOfRangeException("bIndexFormat");
            }
        }
        #endregion
    }
}
