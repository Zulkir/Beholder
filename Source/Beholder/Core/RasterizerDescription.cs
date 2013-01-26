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
using System.Runtime.InteropServices;
using Beholder.Math;

namespace Beholder.Core
{
    /// <summary>
    /// Describes a rasterizer state.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct RasterizerDescription : IEquatable<RasterizerDescription>
    {
        /// <summary>
        /// <para>Depth value added to a given pixel.</para>
        /// <para>Beware that the depth bias behaves differently on different GPUs.</para>
        /// </summary>
        public int DepthBias;

        /// <summary>
        /// <para>Maximum depth bias of a pixel.</para>
        /// <para>Beware that the depth bias behaves differently on different GPUs.</para>
        /// </summary>
        public float DepthBiasClamp;

        /// <summary>
        /// <para>Scalar on a given pixel's slope.</para>
        /// <para>Beware that the depth bias behaves differently on different GPUs.</para>
        /// </summary>
        public float SlopeScaledDepthBias;

        /// <summary>
        /// Determines the fill mode to use when rendering.
        /// </summary>
        public FillMode FillMode;

        /// <summary>
        /// Indicates triangles facing the specified direction are not drawn.
        /// </summary>
        public Cull CullMode;

        /// <summary>
        /// Enable clipping based on distance.
        /// </summary>
        public bool DepthClipEnable;

        /// <summary>
        /// Enable scissor-rectangle culling. All pixels ouside an active scissor rectangle are culled.
        /// </summary>
        public bool ScissorEnable;

        /// <summary>
        /// Enable multisample antialiasing.
        /// </summary>
        public bool MultisampleEnable;

        /// <summary>
        /// Enable line antialiasing; only applies if doing line drawing and MultisampleEnable is false.
        /// </summary>
        public bool AntialiasedLineEnable;

        /// <summary>
        /// Determines which triangles are considered front-facing and which are back-facing.
        /// </summary>
        public Winding FrontFaceWinding;

        /// <summary>
        /// Creates a new <see cref="RasterizerDescription"/> structure.
        /// </summary>
        /// <param name="fillMode">Determines the fill mode to use when rendering.</param>
        /// <param name="cullMode">Indicates triangles facing the specified direction are not drawn.</param>
        /// <param name="depthBias">
        /// <para>Depth value added to a given pixel.</para>
        /// <para>Beware that the depth bias behaves differently on different GPUs.</para></param>
        /// <param name="depthBiasClamp">
        /// <para>Maximum depth bias of a pixel.</para>
        /// <para>Beware that the depth bias behaves differently on different GPUs.</para></param>
        /// <param name="slopeScaledDepthBias">
        /// <para>Scalar on a given pixel's slope.</para>
        /// <para>Beware that the depth bias behaves differently on different GPUs.</para></param>
        /// <param name="depthClipEnable">Enable clipping based on distance.</param>
        /// <param name="scissorEnable">Enable scissor-rectangle culling. All pixels ouside an active scissor rectangle are culled.</param>
        /// <param name="multisampleEnable">Enable multisample antialiasing.</param>
        /// <param name="antialiasedLineEnable">Enable line antialiasing; only applies if doing line drawing and MultisampleEnable is false.</param>
        /// <param name="frontFaceWinding">Determines which triangles are considered front-facing and which are back-facing.</param>
        public RasterizerDescription(FillMode fillMode, Cull cullMode, int depthBias, float depthBiasClamp, float slopeScaledDepthBias, 
            bool depthClipEnable, bool scissorEnable, bool multisampleEnable, bool antialiasedLineEnable,
            Winding frontFaceWinding)
        {
            FillMode = fillMode;
            CullMode = cullMode;
            DepthBias = depthBias;
            DepthBiasClamp = depthBiasClamp;
            SlopeScaledDepthBias = slopeScaledDepthBias;
            DepthClipEnable = depthClipEnable;
            ScissorEnable = scissorEnable;
            MultisampleEnable = multisampleEnable;
            AntialiasedLineEnable = antialiasedLineEnable;
            FrontFaceWinding = frontFaceWinding;
        }

        public bool Equals(RasterizerDescription other)
        {
            return
                FillMode == other.FillMode &&
                CullMode == other.CullMode &&
                DepthBias == other.DepthBias &&
                DepthBiasClamp == other.DepthBiasClamp &&
                SlopeScaledDepthBias == other.SlopeScaledDepthBias &&
                DepthClipEnable == other.DepthClipEnable &&
                ScissorEnable == other.ScissorEnable &&
                MultisampleEnable == other.MultisampleEnable &&
                AntialiasedLineEnable == other.AntialiasedLineEnable &&
                FrontFaceWinding == other.FrontFaceWinding;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is RasterizerDescription)) return false;
            var other = (RasterizerDescription)obj;
            return
                FillMode == other.FillMode &&
                CullMode == other.CullMode &&
                DepthBias == other.DepthBias &&
                DepthBiasClamp == other.DepthBiasClamp &&
                SlopeScaledDepthBias == other.SlopeScaledDepthBias &&
                DepthClipEnable == other.DepthClipEnable &&
                ScissorEnable == other.ScissorEnable &&
                MultisampleEnable == other.MultisampleEnable &&
                AntialiasedLineEnable == other.AntialiasedLineEnable &&
                FrontFaceWinding == other.FrontFaceWinding;
        }

        public override int GetHashCode()
        {
            return
                (DepthClipEnable ? 1 : 0) |
                (ScissorEnable ? 2 : 0) |
                (MultisampleEnable ? 4 : 0) |
                (AntialiasedLineEnable ? 8 : 0) |
                ((int)(FillMode - 2) << 4) |
                ((int)CullMode << 5) |
                ((int)FrontFaceWinding << 7) |
                ((DepthBias ^ DepthBiasClamp.GetHashCode() ^ SlopeScaledDepthBias.GetHashCode()) << 8);

        }

        public static bool Equals(ref RasterizerDescription desc1, ref RasterizerDescription desc2)
        {
            return
                desc1.FillMode == desc2.FillMode &&
                desc1.CullMode == desc2.CullMode &&
                desc1.DepthBias == desc2.DepthBias &&
                desc1.DepthBiasClamp == desc2.DepthBiasClamp &&
                desc1.SlopeScaledDepthBias == desc2.SlopeScaledDepthBias &&
                desc1.DepthClipEnable == desc2.DepthClipEnable &&
                desc1.ScissorEnable == desc2.ScissorEnable &&
                desc1.MultisampleEnable == desc2.MultisampleEnable &&
                desc1.AntialiasedLineEnable == desc2.AntialiasedLineEnable &&
                desc1.FrontFaceWinding == desc2.FrontFaceWinding;
        }

        public static RasterizerDescription Default
        {
            get
            {
                return new RasterizerDescription
                {
                    FillMode = FillMode.Solid,
                    CullMode = Cull.None, // Differs from D3D11
                    FrontFaceWinding = Winding.Clockwise,
                    DepthBias = 0,
                    SlopeScaledDepthBias = 0.0f,
                    DepthBiasClamp = 0.0f,
                    DepthClipEnable = true,
                    ScissorEnable = false,
                    MultisampleEnable = true, // Differs from D3D11
                    AntialiasedLineEnable = false
                };
            }
        }

        public static RasterizerDescription Wireframe
        {
            get
            {
                return new RasterizerDescription
                {
                    FillMode = FillMode.Wireframe,
                    CullMode = Cull.None, // Differs from D3D11
                    FrontFaceWinding = Winding.Clockwise,
                    DepthBias = 0,
                    SlopeScaledDepthBias = 0.0f,
                    DepthBiasClamp = 0.0f,
                    DepthClipEnable = true,
                    ScissorEnable = false,
                    MultisampleEnable = true, // Differs from D3D11
                    AntialiasedLineEnable = false
                };
            }
        }
    }
}
