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

namespace Beholder.Core
{
    /// <summary>
    /// Describes the blend state for a render target.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct RenderTargetBlendDescription : IEquatable<RenderTargetBlendDescription>
    {
        /// <summary>
        /// Enable (or disable) blending.
        /// </summary>
        public bool BlendEnable;

        /// <summary>
        /// Specifies the first RGB data source and includes an optional pre-blend operation.
        /// </summary>
        public Blend SourceBlend;
        
        /// <summary>
        /// Specifies the second RGB data source and includes an optional pre-blend operation.
        /// </summary>
        public Blend DestinationBlend;
        
        /// <summary>
        /// Defines how to combine the RGB data sources.
        /// </summary>
        public BlendOperation BlendOperation;

        /// <summary>
        /// Specifies the first alpha data source and includes an optional pre-blend operation. Blend options that end in '...Color' are not allowed.
        /// </summary>
        public Blend SourceBlendAlpha;
        
        /// <summary>
        /// Specifies the second alpha data source and includes an optional pre-blend operation. Blend options that end in '...Color' are not allowed.
        /// </summary>
        public Blend DestinationBlendAlpha;
        
        /// <summary>
        /// Defines how to combine the alpha data sources.
        /// </summary>
        public BlendOperation BlendOperationAlpha;

        /// <summary>
        /// A write mask.
        /// </summary>
        public ColorMask RenderTargetWriteMask;

        /// <summary>
        /// Creates a new <see cref="RenderTargetBlendDescription"/> structure.
        /// </summary>
        /// <param name="blendEnable">Enable (or disable) blending.</param>
        /// <param name="sourceBlend">Specifies the first RGB data source and includes an optional pre-blend operation.</param>
        /// <param name="destinationBlend">Specifies the second RGB data source and includes an optional pre-blend operation.</param>
        /// <param name="blendOperation">Defines how to combine the RGB data sources.</param>
        /// <param name="sourceBlendAlpha">Specifies the first alpha data source and includes an optional pre-blend operation. Blend options that end in '...Color' are not allowed.</param>
        /// <param name="destinationBlendAlpha">Specifies the second alpha data source and includes an optional pre-blend operation. Blend options that end in '...Color' are not allowed.</param>
        /// <param name="blendOperationAlpha">Defines how to combine the alpha data sources.</param>
        /// <param name="renderTargetrWriteMask">A write mask.</param>
        public RenderTargetBlendDescription(
            bool blendEnable,
            Blend sourceBlend,
            Blend destinationBlend,
            BlendOperation blendOperation,
            Blend sourceBlendAlpha,
            Blend destinationBlendAlpha,
            BlendOperation blendOperationAlpha,
            ColorMask renderTargetrWriteMask)
        {
            BlendEnable = blendEnable;
            SourceBlend = sourceBlend;
            DestinationBlend = destinationBlend;
            BlendOperation = blendOperation;
            SourceBlendAlpha = sourceBlendAlpha;
            DestinationBlendAlpha = destinationBlendAlpha;
            BlendOperationAlpha = blendOperationAlpha;
            RenderTargetWriteMask = renderTargetrWriteMask;
        }

        public bool Equals(RenderTargetBlendDescription other)
        {
            return
                BlendEnable == other.BlendEnable &&
                SourceBlend == other.SourceBlend &&
                DestinationBlend == other.DestinationBlend &&
                BlendOperation == other.BlendOperation &&
                SourceBlendAlpha == other.SourceBlendAlpha &&
                DestinationBlendAlpha == other.DestinationBlendAlpha &&
                BlendOperationAlpha == other.BlendOperationAlpha &&
                RenderTargetWriteMask == other.RenderTargetWriteMask;
        }

        public override bool Equals(object obj)
        {
            return obj is BlendDescription && Equals((BlendDescription)obj);
        }

        public override int GetHashCode()
        {
            return (int)SourceBlend | ((int)DestinationBlend << 4) | ((int)BlendOperation << 8) |
                ((int)SourceBlendAlpha << 11) | ((int)DestinationBlendAlpha << 15) | ((int)BlendOperationAlpha << 19) |
                ((int)RenderTargetWriteMask << 22);
        }

        public static bool Equals(ref RenderTargetBlendDescription desc1, ref RenderTargetBlendDescription desc2)
        {
            return
                desc1.BlendEnable == desc2.BlendEnable &&
                desc1.SourceBlend == desc2.SourceBlend &&
                desc1.DestinationBlend == desc2.DestinationBlend &&
                desc1.BlendOperation == desc2.BlendOperation &&
                desc1.SourceBlendAlpha == desc2.SourceBlendAlpha &&
                desc1.DestinationBlendAlpha == desc2.DestinationBlendAlpha &&
                desc1.BlendOperationAlpha == desc2.BlendOperationAlpha &&
                desc1.RenderTargetWriteMask == desc2.RenderTargetWriteMask;
        }

        public static RenderTargetBlendDescription Default
        {
            get { return None; }
        }

        public static RenderTargetBlendDescription None
        {
            get
            {
                return new RenderTargetBlendDescription
                {
                    BlendEnable = false,
                    SourceBlend = Blend.One,
                    DestinationBlend = Blend.Zero,
                    BlendOperation = BlendOperation.Add,
                    SourceBlendAlpha = Blend.One,
                    DestinationBlendAlpha = Blend.Zero,
                    BlendOperationAlpha = BlendOperation.Add,
                    RenderTargetWriteMask = ColorMask.All
                };
            }
        }

        public static RenderTargetBlendDescription AlphaBlend
        {
            get
            {
                return new RenderTargetBlendDescription
                {
                    BlendEnable = true,
                    SourceBlend = Blend.SourceAlpha,
                    DestinationBlend = Blend.InverseSourceAlpha,
                    BlendOperation = BlendOperation.Add,
                    SourceBlendAlpha = Blend.SourceAlpha,
                    DestinationBlendAlpha = Blend.InverseSourceAlpha,
                    BlendOperationAlpha = BlendOperation.Add,
                    RenderTargetWriteMask = ColorMask.All
                };
            }
        }

        public static RenderTargetBlendDescription Additive
        {
            get
            {
                return new RenderTargetBlendDescription
                {
                    BlendEnable = true,
                    SourceBlend = Blend.One,
                    DestinationBlend = Blend.One,
                    BlendOperation = BlendOperation.Add,
                    SourceBlendAlpha = Blend.One,
                    DestinationBlendAlpha = Blend.One,
                    BlendOperationAlpha = BlendOperation.Add,
                    RenderTargetWriteMask = ColorMask.All
                };
            }
        }
    }
}
