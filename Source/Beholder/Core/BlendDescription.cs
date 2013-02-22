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
    /// Describes a blend state.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct BlendDescription : IEquatable<BlendDescription>
    {
        /// <summary>
        /// Maximum number of independent render target blend options.
        /// </summary>
        public static int MaxRenderTargetBlendCount { get { return 8; } }

        /// <summary>
        /// Blend description for the render target slot 0 or for all the render target slots,
        /// if <see cref="IndependentBlendEnable"/> property is set to False.
        /// </summary>
        public RenderTargetBlendDescription RenderTargetBlend0;

        /// <summary>
        /// Blend description for the render target slot 1.
        /// If <see cref="IndependentBlendEnable"/> property is set to False, this field is ignored.
        /// </summary>
        public RenderTargetBlendDescription RenderTargetBlend1;

        /// <summary>
        /// Blend description for the render target slot 2.
        /// If <see cref="IndependentBlendEnable"/> property is set to False, this field is ignored.
        /// </summary>
        public RenderTargetBlendDescription RenderTargetBlend2;

        /// <summary>
        /// Blend description for the render target slot 3.
        /// If <see cref="IndependentBlendEnable"/> property is set to False, this field is ignored.
        /// </summary>
        public RenderTargetBlendDescription RenderTargetBlend3;

        /// <summary>
        /// Blend description for the render target slot 4.
        /// If <see cref="IndependentBlendEnable"/> property is set to False, this field is ignored.
        /// </summary>
        public RenderTargetBlendDescription RenderTargetBlend4;

        /// <summary>
        /// Blend description for the render target slot 5.
        /// If <see cref="IndependentBlendEnable"/> property is set to False, this field is ignored.
        /// </summary>
        public RenderTargetBlendDescription RenderTargetBlend5;

        /// <summary>
        /// Blend description for the render target slot 6.
        /// If <see cref="IndependentBlendEnable"/> property is set to False, this field is ignored.
        /// </summary>
        public RenderTargetBlendDescription RenderTargetBlend6;

        /// <summary>
        /// Blend description for the render target slot 7.
        /// If <see cref="IndependentBlendEnable"/> property is set to False, this field is ignored.
        /// </summary>
        public RenderTargetBlendDescription RenderTargetBlend7;

        /// <summary>
        /// Determines whether or not to use alpha-to-coverage as a multisampling technique when setting a pixel to a rendertarget.
        /// </summary>
        public bool AlphaToCoverageEnable;

        /// <summary>
        /// Set to True to enable independent blending in simultaneous render targets. 
        /// If set to False, only the RenderTargetBlend0 members are used. RenderTargetBlend[1..7] are ignored.
        /// </summary>
        public bool IndependentBlendEnable;

        private readonly byte numEnabledTargets;

        public static RenderTargetBlendDescription GetRenderTargetBlend(ref BlendDescription desc, int renderTargetSlot)
        {
            RenderTargetBlendDescription targetBlend;
            GetRenderTargetBlend(ref desc, renderTargetSlot, out targetBlend);
            return targetBlend;
        }

        public static void GetRenderTargetBlend(ref BlendDescription desc, int renderTargetSlot, out RenderTargetBlendDescription renderTargetBlend)
        {
            switch (renderTargetSlot)
            {
                case 0: renderTargetBlend = desc.RenderTargetBlend0; break;
                case 1: renderTargetBlend = desc.RenderTargetBlend1; break;
                case 2: renderTargetBlend = desc.RenderTargetBlend2; break;
                case 3: renderTargetBlend = desc.RenderTargetBlend3; break;
                case 4: renderTargetBlend = desc.RenderTargetBlend4; break;
                case 5: renderTargetBlend = desc.RenderTargetBlend5; break;
                case 6: renderTargetBlend = desc.RenderTargetBlend6; break;
                case 7: renderTargetBlend = desc.RenderTargetBlend7; break;
                default: throw new ArgumentOutOfRangeException("renderTargetSlot", "RenderTargetBlendDescription in BlendDescription has to be in range from 0 to 7"); 
            }
        }

        /// <summary>
        /// Retrieves a blend description for a specified render target slot.
        /// </summary>
        /// <param name="renderTargetSlot">Render target slot.</param>
        public RenderTargetBlendDescription GetRenderTargetBlend(int renderTargetSlot)
        {
            RenderTargetBlendDescription result;
            GetRenderTargetBlend(ref this, renderTargetSlot, out result);
            return result;
        }

        /// <summary>
        /// Sets a blend description for a specified render target slot.
        /// </summary>
        /// <param name="desc">Description to set the render target blend for.</param>
        /// <param name="renderTargetSlot">Render target slot.</param>
        /// <param name="renderTargetBlend">Blend description for the render target.</param>
        public static void SetRenderTargetBlend(ref BlendDescription desc, int renderTargetSlot, ref RenderTargetBlendDescription renderTargetBlend)
        {
            switch (renderTargetSlot)
            {
                case 0: desc.RenderTargetBlend0 = renderTargetBlend; break;
                case 1: desc.RenderTargetBlend1 = renderTargetBlend; break;
                case 2: desc.RenderTargetBlend2 = renderTargetBlend; break;
                case 3: desc.RenderTargetBlend3 = renderTargetBlend; break;
                case 4: desc.RenderTargetBlend4 = renderTargetBlend; break;
                case 5: desc.RenderTargetBlend5 = renderTargetBlend; break;
                case 6: desc.RenderTargetBlend6 = renderTargetBlend; break;
                case 7: desc.RenderTargetBlend7 = renderTargetBlend; break;
                default: throw new ArgumentOutOfRangeException("renderTargetSlot", "RenderTargetBlendDescription in BlendDescription has to be in range from 0 to 7");
            }
        }

        /// <summary>
        /// Sets a blend description for a specified render target slot.
        /// </summary>
        /// <param name="renderTargetSlot">Render target slot.</param>
        /// <param name="renderTargetBlend">Blend description for the render target.</param>
        public void SetRenderTargetBlend(int renderTargetSlot, RenderTargetBlendDescription renderTargetBlend)
        {
            SetRenderTargetBlend(ref this, renderTargetSlot, ref renderTargetBlend);
        }
        
        #region Constructiors
        
        private BlendDescription(byte numEnabledTargets)
            : this()
        {
            this.numEnabledTargets = numEnabledTargets;
        }
        
        /// <summary>
        /// Creates a new <see cref="BlendDescription"/> structure with independent render target blend disabled.
        /// </summary>
        /// <param name="alphaToCoverageEnable">Determines whether or not to use alpha-to-coverage as a multisampling technique when setting a pixel to a rendertarget.</param>
        /// <param name="allRenderTargetBlend">Blend description for all the render target slots.</param>
        public BlendDescription(
            bool alphaToCoverageEnable, 
            //bool independentBlendEnbale,
            RenderTargetBlendDescription allRenderTargetBlend) : this()
        {
            AlphaToCoverageEnable = alphaToCoverageEnable;
            IndependentBlendEnable = false;
            RenderTargetBlend0 = allRenderTargetBlend;
        }

        /// <summary>
        /// Creates a new <see cref="BlendDescription"/> structure with independent render target blend enabled.
        /// </summary>
        /// <param name="alphaToCoverageEnable">Determines whether or not to use alpha-to-coverage as a multisampling technique when setting a pixel to a rendertarget.</param>
        /// <param name="renderTargetBlend0">Blend description for the render target slot 0.</param>
        /// <param name="renderTargetBlend1">Blend description for the render target slot 1.</param>
        public BlendDescription(
            bool alphaToCoverageEnable,
            //bool independentBlendEnbale,
            RenderTargetBlendDescription renderTargetBlend0,
            RenderTargetBlendDescription renderTargetBlend1)
            : this(2)
        {
            AlphaToCoverageEnable = alphaToCoverageEnable;
            IndependentBlendEnable = true;
            RenderTargetBlend0 = renderTargetBlend0;
            RenderTargetBlend1 = renderTargetBlend1;
        }

        /// <summary>
        /// Creates a new <see cref="BlendDescription"/> structure with independent render target blend enabled.
        /// </summary>
        /// <param name="alphaToCoverageEnable">Determines whether or not to use alpha-to-coverage as a multisampling technique when setting a pixel to a rendertarget.</param>
        /// <param name="renderTargetBlend0">Blend description for the render target slot 0.</param>
        /// <param name="renderTargetBlend1">Blend description for the render target slot 1.</param>
        /// <param name="renderTargetBlend2">Blend description for the render target slot 2.</param>
        public BlendDescription(
            bool alphaToCoverageEnable,
            //bool independentBlendEnbale,
            RenderTargetBlendDescription renderTargetBlend0,
            RenderTargetBlendDescription renderTargetBlend1,
            RenderTargetBlendDescription renderTargetBlend2)
            : this(3)
        {
            AlphaToCoverageEnable = alphaToCoverageEnable;
            IndependentBlendEnable = true;
            RenderTargetBlend0 = renderTargetBlend0;
            RenderTargetBlend1 = renderTargetBlend1;
            RenderTargetBlend2 = renderTargetBlend2;
        }

        /// <summary>
        /// Creates a new <see cref="BlendDescription"/> structure with independent render target blend enabled.
        /// </summary>
        /// <param name="alphaToCoverageEnable">Determines whether or not to use alpha-to-coverage as a multisampling technique when setting a pixel to a rendertarget.</param>
        /// <param name="renderTargetBlend0">Blend description for the render target slot 0.</param>
        /// <param name="renderTargetBlend1">Blend description for the render target slot 1.</param>
        /// <param name="renderTargetBlend2">Blend description for the render target slot 2.</param>
        /// <param name="renderTargetBlend3">Blend description for the render target slot 3.</param>
        public BlendDescription(
            bool alphaToCoverageEnable,
            //bool independentBlendEnbale,
            RenderTargetBlendDescription renderTargetBlend0,
            RenderTargetBlendDescription renderTargetBlend1,
            RenderTargetBlendDescription renderTargetBlend2,
            RenderTargetBlendDescription renderTargetBlend3)
            : this(4)
        {
            AlphaToCoverageEnable = alphaToCoverageEnable;
            IndependentBlendEnable = true;
            RenderTargetBlend0 = renderTargetBlend0;
            RenderTargetBlend1 = renderTargetBlend1;
            RenderTargetBlend2 = renderTargetBlend2;
            RenderTargetBlend3 = renderTargetBlend3;
        }

        /// <summary>
        /// Creates a new <see cref="BlendDescription"/> structure with independent render target blend enabled.
        /// </summary>
        /// <param name="alphaToCoverageEnable">Determines whether or not to use alpha-to-coverage as a multisampling technique when setting a pixel to a rendertarget.</param>
        /// <param name="renderTargetBlend0">Blend description for the render target slot 0.</param>
        /// <param name="renderTargetBlend1">Blend description for the render target slot 1.</param>
        /// <param name="renderTargetBlend2">Blend description for the render target slot 2.</param>
        /// <param name="renderTargetBlend3">Blend description for the render target slot 3.</param>
        /// <param name="renderTargetBlend4">Blend description for the render target slot 4.</param>
        public BlendDescription(
            bool alphaToCoverageEnable,
            //bool independentBlendEnbale,
            RenderTargetBlendDescription renderTargetBlend0,
            RenderTargetBlendDescription renderTargetBlend1,
            RenderTargetBlendDescription renderTargetBlend2,
            RenderTargetBlendDescription renderTargetBlend3,
            RenderTargetBlendDescription renderTargetBlend4)
            : this(5)
        {
            AlphaToCoverageEnable = alphaToCoverageEnable;
            IndependentBlendEnable = true;
            RenderTargetBlend0 = renderTargetBlend0;
            RenderTargetBlend1 = renderTargetBlend1;
            RenderTargetBlend2 = renderTargetBlend2;
            RenderTargetBlend3 = renderTargetBlend3;
            RenderTargetBlend4 = renderTargetBlend4;
        }
        
        /// <summary>
        /// Creates a new <see cref="BlendDescription"/> structure with independent render target blend enabled.
        /// </summary>
        /// <param name="alphaToCoverageEnable">Determines whether or not to use alpha-to-coverage as a multisampling technique when setting a pixel to a rendertarget.</param>
        /// <param name="renderTargetBlend0">Blend description for the render target slot 0.</param>
        /// <param name="renderTargetBlend1">Blend description for the render target slot 1.</param>
        /// <param name="renderTargetBlend2">Blend description for the render target slot 2.</param>
        /// <param name="renderTargetBlend3">Blend description for the render target slot 3.</param>
        /// <param name="renderTargetBlend4">Blend description for the render target slot 4.</param>
        /// <param name="renderTargetBlend5">Blend description for the render target slot 5.</param>
        public BlendDescription(
            bool alphaToCoverageEnable,
            //bool independentBlendEnbale,
            RenderTargetBlendDescription renderTargetBlend0,
            RenderTargetBlendDescription renderTargetBlend1,
            RenderTargetBlendDescription renderTargetBlend2,
            RenderTargetBlendDescription renderTargetBlend3,
            RenderTargetBlendDescription renderTargetBlend4,
            RenderTargetBlendDescription renderTargetBlend5)
            : this(6)
        {
            AlphaToCoverageEnable = alphaToCoverageEnable;
            IndependentBlendEnable = true;
            RenderTargetBlend0 = renderTargetBlend0;
            RenderTargetBlend1 = renderTargetBlend1;
            RenderTargetBlend2 = renderTargetBlend2;
            RenderTargetBlend3 = renderTargetBlend3;
            RenderTargetBlend4 = renderTargetBlend4;
            RenderTargetBlend5 = renderTargetBlend5;
        }

        /// <summary>
        /// Creates a new <see cref="BlendDescription"/> structure with independent render target blend enabled.
        /// </summary>
        /// <param name="alphaToCoverageEnable">Determines whether or not to use alpha-to-coverage as a multisampling technique when setting a pixel to a rendertarget.</param>
        /// <param name="renderTargetBlend0">Blend description for the render target slot 0.</param>
        /// <param name="renderTargetBlend1">Blend description for the render target slot 1.</param>
        /// <param name="renderTargetBlend2">Blend description for the render target slot 2.</param>
        /// <param name="renderTargetBlend3">Blend description for the render target slot 3.</param>
        /// <param name="renderTargetBlend4">Blend description for the render target slot 4.</param>
        /// <param name="renderTargetBlend5">Blend description for the render target slot 5.</param>
        /// <param name="renderTargetBlend6">Blend description for the render target slot 6.</param>
        public BlendDescription(
            bool alphaToCoverageEnable,
            //bool independentBlendEnbale,
            RenderTargetBlendDescription renderTargetBlend0,
            RenderTargetBlendDescription renderTargetBlend1,
            RenderTargetBlendDescription renderTargetBlend2,
            RenderTargetBlendDescription renderTargetBlend3,
            RenderTargetBlendDescription renderTargetBlend4,
            RenderTargetBlendDescription renderTargetBlend5,
            RenderTargetBlendDescription renderTargetBlend6)
            : this(7)
        {
            AlphaToCoverageEnable = alphaToCoverageEnable;
            IndependentBlendEnable = true;
            RenderTargetBlend0 = renderTargetBlend0;
            RenderTargetBlend1 = renderTargetBlend1;
            RenderTargetBlend2 = renderTargetBlend2;
            RenderTargetBlend3 = renderTargetBlend3;
            RenderTargetBlend4 = renderTargetBlend4;
            RenderTargetBlend5 = renderTargetBlend5;
            RenderTargetBlend6 = renderTargetBlend6;
        }
        
        /// <summary>
        /// Creates a new <see cref="BlendDescription"/> structure with independent render target blend enabled.
        /// </summary>
        /// <param name="alphaToCoverageEnable">Determines whether or not to use alpha-to-coverage as a multisampling technique when setting a pixel to a rendertarget.</param>
        /// <param name="renderTargetBlend0">Blend description for the render target slot 0.</param>
        /// <param name="renderTargetBlend1">Blend description for the render target slot 1.</param>
        /// <param name="renderTargetBlend2">Blend description for the render target slot 2.</param>
        /// <param name="renderTargetBlend3">Blend description for the render target slot 3.</param>
        /// <param name="renderTargetBlend4">Blend description for the render target slot 4.</param>
        /// <param name="renderTargetBlend5">Blend description for the render target slot 5.</param>
        /// <param name="renderTargetBlend6">Blend description for the render target slot 6.</param>
        /// <param name="renderTargetBlend7">Blend description for the render target slot 7.</param>
        public BlendDescription(
            bool alphaToCoverageEnable,
            //bool independentBlendEnbale,
            RenderTargetBlendDescription renderTargetBlend0,
            RenderTargetBlendDescription renderTargetBlend1,
            RenderTargetBlendDescription renderTargetBlend2,
            RenderTargetBlendDescription renderTargetBlend3,
            RenderTargetBlendDescription renderTargetBlend4,
            RenderTargetBlendDescription renderTargetBlend5,
            RenderTargetBlendDescription renderTargetBlend6,
            RenderTargetBlendDescription renderTargetBlend7)
            : this(8)
        {
            AlphaToCoverageEnable = alphaToCoverageEnable;
            IndependentBlendEnable = true;
            RenderTargetBlend0 = renderTargetBlend0;
            RenderTargetBlend1 = renderTargetBlend1;
            RenderTargetBlend2 = renderTargetBlend2;
            RenderTargetBlend3 = renderTargetBlend3;
            RenderTargetBlend4 = renderTargetBlend4;
            RenderTargetBlend5 = renderTargetBlend5;
            RenderTargetBlend6 = renderTargetBlend6;
            RenderTargetBlend7 = renderTargetBlend7;
        }
        #endregion

        /// <summary>
        /// Returs a value indicating whether this BlendDescription is equal to another BlendDescription.
        /// </summary>
        /// <param name="other">A BlendDescription to compare to this instance.</param>
        /// <returns>True if two descriptions are equals and False otherwise.</returns>
        public bool Equals(BlendDescription other)
        {
            if (IndependentBlendEnable)
            {
                return other.IndependentBlendEnable &&
                    AlphaToCoverageEnable == other.AlphaToCoverageEnable &&
                    RenderTargetBlendDescription.Equals(ref RenderTargetBlend0, ref other.RenderTargetBlend0) &&
                    RenderTargetBlendDescription.Equals(ref RenderTargetBlend1, ref other.RenderTargetBlend1) &&
                    RenderTargetBlendDescription.Equals(ref RenderTargetBlend2, ref other.RenderTargetBlend2) &&
                    RenderTargetBlendDescription.Equals(ref RenderTargetBlend3, ref other.RenderTargetBlend3) &&
                    RenderTargetBlendDescription.Equals(ref RenderTargetBlend4, ref other.RenderTargetBlend4) &&
                    RenderTargetBlendDescription.Equals(ref RenderTargetBlend5, ref other.RenderTargetBlend5) &&
                    RenderTargetBlendDescription.Equals(ref RenderTargetBlend6, ref other.RenderTargetBlend6) &&
                    RenderTargetBlendDescription.Equals(ref RenderTargetBlend7, ref other.RenderTargetBlend7);
            }

            return !other.IndependentBlendEnable &&
                    AlphaToCoverageEnable == other.AlphaToCoverageEnable &&
                    RenderTargetBlendDescription.Equals(ref RenderTargetBlend0, ref other.RenderTargetBlend0);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is BlendDescription)) return false;
            BlendDescription other = (BlendDescription)obj;
            if (IndependentBlendEnable)
            {
                return other.IndependentBlendEnable &&
                    AlphaToCoverageEnable == other.AlphaToCoverageEnable &&
                    RenderTargetBlendDescription.Equals(ref RenderTargetBlend0, ref other.RenderTargetBlend0) &&
                    RenderTargetBlendDescription.Equals(ref RenderTargetBlend1, ref other.RenderTargetBlend1) &&
                    RenderTargetBlendDescription.Equals(ref RenderTargetBlend2, ref other.RenderTargetBlend2) &&
                    RenderTargetBlendDescription.Equals(ref RenderTargetBlend3, ref other.RenderTargetBlend3) &&
                    RenderTargetBlendDescription.Equals(ref RenderTargetBlend4, ref other.RenderTargetBlend4) &&
                    RenderTargetBlendDescription.Equals(ref RenderTargetBlend5, ref other.RenderTargetBlend5) &&
                    RenderTargetBlendDescription.Equals(ref RenderTargetBlend6, ref other.RenderTargetBlend6) &&
                    RenderTargetBlendDescription.Equals(ref RenderTargetBlend7, ref other.RenderTargetBlend7);
            }

            return !other.IndependentBlendEnable &&
                    AlphaToCoverageEnable == other.AlphaToCoverageEnable &&
                    RenderTargetBlendDescription.Equals(ref RenderTargetBlend0, ref other.RenderTargetBlend0);
        }

        public override int GetHashCode()
        {
            return
                (AlphaToCoverageEnable ? 1 : 0) |
                (IndependentBlendEnable ? 2 : 0) |
                (RenderTargetBlend0.GetHashCode() << 2) |
                numEnabledTargets << 28;
        }


        /// <summary>
        /// Returns a value indicating whether the specified blend descriptions are equal.
        /// </summary>
        /// <param name="desc1">First description to compare.</param>
        /// <param name="desc2">Sencond description to compare.</param>
        /// <returns>True if two descriptions are equals and False otherwise.</returns>
        public static bool Equals(ref BlendDescription desc1, ref BlendDescription desc2)
        {
            if (desc1.IndependentBlendEnable)
            {
                return desc2.IndependentBlendEnable &&
                    desc1.AlphaToCoverageEnable == desc2.AlphaToCoverageEnable &&
                    RenderTargetBlendDescription.Equals(ref desc1.RenderTargetBlend0, ref desc2.RenderTargetBlend0) &&
                    RenderTargetBlendDescription.Equals(ref desc1.RenderTargetBlend1, ref desc2.RenderTargetBlend1) &&
                    RenderTargetBlendDescription.Equals(ref desc1.RenderTargetBlend2, ref desc2.RenderTargetBlend2) &&
                    RenderTargetBlendDescription.Equals(ref desc1.RenderTargetBlend3, ref desc2.RenderTargetBlend3) &&
                    RenderTargetBlendDescription.Equals(ref desc1.RenderTargetBlend4, ref desc2.RenderTargetBlend4) &&
                    RenderTargetBlendDescription.Equals(ref desc1.RenderTargetBlend5, ref desc2.RenderTargetBlend5) &&
                    RenderTargetBlendDescription.Equals(ref desc1.RenderTargetBlend6, ref desc2.RenderTargetBlend6) &&
                    RenderTargetBlendDescription.Equals(ref desc1.RenderTargetBlend7, ref desc2.RenderTargetBlend7);
            }

            return !desc2.IndependentBlendEnable &&
                    desc1.AlphaToCoverageEnable == desc2.AlphaToCoverageEnable &&
                    RenderTargetBlendDescription.Equals(ref desc1.RenderTargetBlend0, ref desc2.RenderTargetBlend0);
        }

        public static BlendDescription Default
        {
            get
            {
                return new BlendDescription
                {
                    AlphaToCoverageEnable = false,
                    IndependentBlendEnable = false,
                    RenderTargetBlend0 = RenderTargetBlendDescription.Default
                };
            }
        }

        public static BlendDescription Opaque
        {
            get
            {
                return new BlendDescription
                {
                    AlphaToCoverageEnable = false,
                    IndependentBlendEnable = false,
                    RenderTargetBlend0 = RenderTargetBlendDescription.None
                };
            }
        }


        public static BlendDescription AlphaBlend
        {
            get
            {
                return new BlendDescription
                {
                    AlphaToCoverageEnable = false,
                    IndependentBlendEnable = false,
                    RenderTargetBlend0 = RenderTargetBlendDescription.AlphaBlend
                };
            }
        }


        public static BlendDescription Additive
        {
            get
            {
                return new BlendDescription
                {
                    AlphaToCoverageEnable = false,
                    IndependentBlendEnable = false,
                    RenderTargetBlend0 = RenderTargetBlendDescription.Additive
                };
            }
        }
    }
}
