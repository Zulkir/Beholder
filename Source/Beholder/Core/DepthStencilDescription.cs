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

namespace Beholder.Core
{
    /// <summary>
    /// Describes a depth-stencil state.
    /// </summary>
    /// [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct DepthStencilDescription : IEquatable<DepthStencilDescription>
    {
        /// <summary>
        /// Identifies how to use the results of the depth test and the stencil test for pixels whose surface normal is facing towards the camera.
        /// </summary>
        public DepthStencilOperationDescription FrontFaceStencilOp;
        
        /// <summary>
        /// Identifies how to use the results of the depth test and the stencil test for pixels whose surface normal is facing away from the camera.
        /// </summary>
        public DepthStencilOperationDescription BackFaceStencilOp;

        /// <summary>
        /// Enable depth testing.
        /// </summary>
        public bool DepthEnable;

        /// <summary>
        /// Enable writing to the depth buffer.
        /// </summary>
        public bool DepthWriteEnable;

        /// <summary>
        /// A function that compares depth data against existing depth data.
        /// </summary>
        public Comparison DepthFunction;
        
        /// <summary>
        /// Enable stencil testing.
        /// </summary>
        public bool StencilEnable;

        /// <summary>
        /// Identifies a portion of the depth-stencil buffer for reading stencil data.
        /// </summary>
        public byte StencilReadMask;

        /// <summary>
        /// Identifies a portion of the depth-stencil buffer for writing stencil data.
        /// </summary>
        public byte StencilWriteMask;
        
        /// <summary>
        /// Creates a new <see cref="DepthStencilDescription"/> structure.
        /// </summary>
        /// <param name="depthEnable">Enable depth testing.</param>
        /// <param name="depthWriteEnable">Enable writing to the depth buffer.</param>
        /// <param name="depthFunction">A function that compares depth data against existing depth data.</param>
        /// <param name="stencilEnable">Enable stencil testing.</param>
        /// <param name="stencilReadMask">Identifies a portion of the depth-stencil buffer for reading stencil data.</param>
        /// <param name="stencilWriteMask">Identifies a portion of the depth-stencil buffer for writing stencil data.</param>
        /// <param name="frontFace">Identifies how to use the results of the depth test and the stencil test for pixels whose surface normal is facing towards the camera.</param>
        /// <param name="backFace">Identifies how to use the results of the depth test and the stencil test for pixels whose surface normal is facing away from the camera.</param>
        public DepthStencilDescription(bool depthEnable, bool depthWriteEnable, Comparison depthFunction,
            bool stencilEnable, byte stencilReadMask, byte stencilWriteMask,
            DepthStencilOperationDescription frontFace, DepthStencilOperationDescription backFace)
        {
            DepthEnable = depthEnable;
            DepthWriteEnable = depthWriteEnable;
            DepthFunction = depthFunction;
            StencilEnable = stencilEnable;
            StencilReadMask = stencilReadMask;
            StencilWriteMask = stencilWriteMask;
            FrontFaceStencilOp = frontFace;
            BackFaceStencilOp = backFace;
        }

        public bool Equals(DepthStencilDescription other)
        {
            return
                DepthEnable == other.DepthEnable &&
                DepthWriteEnable == other.DepthWriteEnable &&
                DepthFunction == other.DepthFunction &&
                StencilEnable == other.StencilEnable &&
                StencilReadMask == other.StencilReadMask &&
                StencilWriteMask == other.StencilWriteMask &&
                FrontFaceStencilOp.Equals(other.FrontFaceStencilOp) &&
                BackFaceStencilOp.Equals(other.BackFaceStencilOp);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is DepthStencilDescription)) return false;
            var other = (DepthStencilDescription)obj;
            return
                DepthEnable == other.DepthEnable &&
                DepthWriteEnable == other.DepthWriteEnable &&
                DepthFunction == other.DepthFunction &&
                StencilEnable == other.StencilEnable &&
                StencilReadMask == other.StencilReadMask &&
                StencilWriteMask == other.StencilWriteMask &&
                FrontFaceStencilOp == other.FrontFaceStencilOp &&
                BackFaceStencilOp == other.BackFaceStencilOp;
        }

        public override int GetHashCode()
        {
            return
                (
                (DepthEnable ? 1 : 0) |
                (DepthWriteEnable ? 2 : 0) |
                (StencilEnable ? 4 : 0) |
                ((int)DepthFunction << 3) |
                (StencilReadMask << 16) |
                (StencilWriteMask << 24))
                +
                (FrontFaceStencilOp.GetHashCode() ^ BackFaceStencilOp.GetHashCode());

        }

        public static bool Equals(ref DepthStencilDescription desc1, ref DepthStencilDescription desc2)
        {
            return
                desc1.DepthEnable == desc2.DepthEnable &&
                desc1.DepthWriteEnable == desc2.DepthWriteEnable &&
                desc1.DepthFunction == desc2.DepthFunction &&
                desc1.StencilEnable == desc2.StencilEnable &&
                desc1.StencilReadMask == desc2.StencilReadMask &&
                desc1.StencilWriteMask == desc2.StencilWriteMask &&
                desc1.FrontFaceStencilOp.Equals(desc2.FrontFaceStencilOp) &&
                desc1.BackFaceStencilOp.Equals(desc2.BackFaceStencilOp);
        }

        public static DepthStencilDescription Default
        {
            get
            {
                return new DepthStencilDescription
                {
                    DepthEnable = false, // Differs from D3D11
                    DepthWriteEnable = false, // Differs from D3D11
                    DepthFunction = Comparison.Less,
                    StencilEnable = false,
                    StencilReadMask = 0xff,
                    StencilWriteMask = 0xff,
                    FrontFaceStencilOp = DepthStencilOperationDescription.Default,
                    BackFaceStencilOp = DepthStencilOperationDescription.Default
                };
            }
        }

        public static DepthStencilDescription Enabled
        {
            get
            {
                return new DepthStencilDescription
                {
                    DepthEnable = true, // Differs from D3D11
                    DepthWriteEnable = true, // Differs from D3D11
                    DepthFunction = Comparison.Less,
                    StencilEnable = false,
                    StencilReadMask = 0xff,
                    StencilWriteMask = 0xff,
                    FrontFaceStencilOp = DepthStencilOperationDescription.Default,
                    BackFaceStencilOp = DepthStencilOperationDescription.Default
                };
            }
        }
    }
}
