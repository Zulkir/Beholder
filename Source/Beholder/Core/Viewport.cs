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
using System.Runtime.InteropServices;
using Beholder.Platform;

namespace Beholder.Core
{
    /// <summary>
    /// Defines the dimensions of a viewport.
    /// </summary>
    /// <remarks>
    /// <para>
    /// In all cases, Width and Height must be >= 0 and TopLeftX + Width and TopLeftY + Height must be &lt;= VIEWPORT_BOUNDS_MAX.
    /// </para>
    /// <para>
    /// The range for the minimum and maximum viewport size is dependent on the feature level defined by <see cref="FeatureLevel"/>.
    /// For Direct3D11 the range is (-32768, 32767);
    /// For lower feature levels the range is (-16384, 16383).
    /// </para>
    /// </remarks>
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct Viewport
    {
        /// <summary>
        /// X position of the left hand side of the viewport.
        /// Ranges between VIEWPORT_BOUNDS_MIN and VIEWPORT_BOUNDS_MAX (see remarks).
        /// </summary>
        public float Left;

        /// <summary>
        /// X position of the left hand side of the viewport.
        /// Ranges between VIEWPORT_BOUNDS_MIN and VIEWPORT_BOUNDS_MAX (see remarks).
        /// </summary>
        public float Top;

        /// <summary>
        /// Width of the viewport.
        /// </summary>
        public float Width;

        /// <summary>
        /// Height of the viewport.
        /// </summary>
        public float Height;

        /// <summary>
        /// Minimum depth of the viewport. Ranges between 0 and 1.
        /// </summary>
        public float MinDepth;

        /// <summary>
        /// Maximum depth of the viewport. Ranges between 0 and 1.
        /// </summary>
        public float MaxDepth;

        /// <summary>
        /// Creates a new <see cref="Viewport"/> structure.
        /// </summary>
        /// <param name="width">Width of the viewport.</param>
        /// <param name="height">Height of the viewport.</param>
        public Viewport(float width, float height)
        {
            Left = 0;
            Top = 0;
            Width = width;
            Height = height;
            MinDepth = 0.0f;
            MaxDepth = 1.0f;
        }

        /// <summary>
        /// Creates a new <see cref="Viewport"/> structure.
        /// </summary>
        /// <param name="left">
        /// X position of the left hand side of the viewport.
        /// Ranges between VIEWPORT_BOUNDS_MIN and VIEWPORT_BOUNDS_MAX (see remarks).
        /// </param>
        /// <param name="top">
        /// X position of the left hand side of the viewport.
        /// Ranges between VIEWPORT_BOUNDS_MIN and VIEWPORT_BOUNDS_MAX (see remarks).</param>
        /// <param name="width">Width of the viewport.</param>
        /// <param name="height">Height of the viewport.</param>
        public Viewport(float left, float top, float width, float height)
        {
            Left = left;
            Top = top;
            Width = width;
            Height = height;
            MinDepth = 0.0f;
            MaxDepth = 1.0f;
        }

        /// <summary>
        /// Creates a new <see cref="Viewport"/> structure.
        /// </summary>
        /// <param name="left">
        /// X position of the left hand side of the viewport.
        /// Ranges between VIEWPORT_BOUNDS_MIN and VIEWPORT_BOUNDS_MAX (see remarks).
        /// </param>
        /// <param name="top">
        /// X position of the left hand side of the viewport.
        /// Ranges between VIEWPORT_BOUNDS_MIN and VIEWPORT_BOUNDS_MAX (see remarks).</param>
        /// <param name="width">Width of the viewport.</param>
        /// <param name="height">Height of the viewport.</param>
        /// <param name="minDepth">Minimum depth of the viewport. Ranges between 0 and 1.</param>
        /// <param name="maxDepth">Maximum depth of the viewport. Ranges between 0 and 1.</param>
        public Viewport(float left, float top, float width, float height, float minDepth, float maxDepth)
        {
            Left = left;
            Top = top;
            Width = width;
            Height = height;
            MinDepth = minDepth;
            MaxDepth = maxDepth;
        }

        public static bool Equals(ref Viewport v1, ref Viewport v2)
        {
            return
                v1.Left == v2.Left &&
                v1.Top == v2.Top &&
                v1.Width == v2.Width &&
                v1.Height == v2.Height &&
                v1.MinDepth == v2.MinDepth &&
                v1.MaxDepth == v2.MaxDepth;
        }

        public static bool Equals(Viewport v1, Viewport v2)
        {
            return
                v1.Left == v2.Left &&
                v1.Top == v2.Top &&
                v1.Width == v2.Width &&
                v1.Height == v2.Height &&
                v1.MinDepth == v2.MinDepth &&
                v1.MaxDepth == v2.MaxDepth;
        }
    }
}
