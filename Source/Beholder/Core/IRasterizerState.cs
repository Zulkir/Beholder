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

using Beholder.Math;

namespace Beholder.Core
{
    /// <summary>
    /// Rasterizer state for the rasterizer stage.
    /// </summary>
    public interface IRasterizerState : IDeviceChild
    {
        /// <summary>
        /// <para>Depth value added to a given pixel.</para>
        /// <para>Beware that the depth bias behaves diferenty on different GPUs.</para>
        /// </summary>
        int DepthBias { get; }

        /// <summary>
        /// <para>Maximum depth bias of a pixel.</para>
        /// <para>Beware that the depth bias behaves diferenty on different GPUs.</para>
        /// </summary>
        float DepthBiasClamp { get; }

        /// <summary>
        /// <para>Scalar on a given pixel's slope.</para>
        /// <para>Beware that the depth bias behaves diferenty on different GPUs.</para>
        /// </summary>
        float SlopeScaledDepthBias { get; }

        /// <summary>
        /// Determines the fill mode to use when rendering.
        /// </summary>
        FillMode FillMode { get; }

        /// <summary>
        /// Indicates triangles facing the specified direction are not drawn.
        /// </summary>
        Cull CullMode { get; }

        /// <summary>
        /// Enable clipping based on distance.
        /// </summary>
        bool DepthClipEnable { get; }

        /// <summary>
        /// Enable scissor-rectangle culling. All pixels ouside an active scissor rectangle are culled.
        /// </summary>
        bool ScissorEnable { get; }

        /// <summary>
        /// Enable multisample antialiasing.
        /// </summary>
        bool MultisampleEnable { get; }

        /// <summary>
        /// Enable line antialiasing; only applies if doing line drawing and MultisampleEnable is false.
        /// </summary>
        bool AntialiasedLineEnable { get; }

        /// <summary>
        /// Determines which triangles are considered front-facing and which are back-facing.
        /// </summary>
        Winding FrontFaceWinding { get; }

        /// <summary>
        /// Gets a description of the rasterizer state.
        /// </summary>
        /// <param name="description">Variable to fill with the description data.</param>
        void GetDescription(out RasterizerDescription description);
    }
}
