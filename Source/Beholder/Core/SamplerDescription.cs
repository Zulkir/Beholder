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
    /// Describes a sampler state.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct SamplerDescription : IEquatable<SamplerDescription>
    {
        /// <summary>
        /// Filtering method to use when sampling a texture.
        /// </summary>
        public Filter Filter;

        /// <summary>
        /// Method to use for resolving a U texture coordinate that is outside the 0 to 1 range.
        /// </summary>
        public TextureAddressMode AddressU;
        
        /// <summary>
        /// Method to use for resolving a V texture coordinate that is outside the 0 to 1 range.
        /// </summary>
        public TextureAddressMode AddressV;
        
        /// <summary>
        /// Method to use for resolving a W texture coordinate that is outside the 0 to 1 range.
        /// </summary>
        public TextureAddressMode AddressW;

        /// <summary>
        /// Offset from the calculated mipmap level.
        /// </summary>
        public float MipLodBias;
        
        /// <summary>
        /// Clamping value used if Filter.Anisotropic or Filter.ComparisonAnisotropic is specified in Filter. Valid values are between 1 and 16.
        /// </summary>
        public int MaximumAnisotropy;

        /// <summary>
        /// A function that compares sampled data against existing sampled data.
        /// </summary>
        public Comparison ComparisonFunction;

        /// <summary>
        /// Border color to use if TextureAddressMode.Border is specified for AddressU, AddressV, or AddressW. Range must be between 0.0 and 1.0 inclusive.
        /// </summary>
        public Color4 BorderColor;

        /// <summary>
        /// Lower end of the mipmap range to clamp access to, where 0 is the largest and most detailed mipmap level and any level higher than that is less detailed.
        /// </summary>
        public float MinimumLod;

        /// <summary>
        /// Upper end of the mipmap range to clamp access to, where 0 is the largest and most detailed mipmap level and any level higher than that is less detailed. 
        /// This value must be greater than or equal to MinimumLod. To have no upper limit on LOD set this to a large value such as System.Single.MaxValue.
        /// </summary>
        public float MaximumLod;
        
        /// <summary>
        /// Creates a new <see cref="SamplerDescription"/> structure.
        /// </summary>
        /// <param name="filter">Filtering method to use when sampling a texture.</param>
        /// <param name="addressU">Method to use for resolving a U texture coordinate that is outside the 0 to 1 range.</param>
        /// <param name="addressV">Method to use for resolving a V texture coordinate that is outside the 0 to 1 range.</param>
        /// <param name="addressW">Method to use for resolving a W texture coordinate that is outside the 0 to 1 range.</param>
        /// <param name="mipLodBias">Offset from the calculated mipmap level.</param>
        /// <param name="maximumAnisotropy">Clamping value used if Filter.Anisotropic or Filter.ComparisonAnisotropic is specified in Filter. Valid values are between 1 and 16.</param>
        /// <param name="comparisonFunction">A function that compares sampled data against existing sampled data.</param>
        /// <param name="borderColor">Border color to use if TextureAddressMode.Border is specified for AddressU, AddressV, or AddressW. Range must be between 0.0 and 1.0 inclusive.</param>
        /// <param name="minimumLod">Lower end of the mipmap range to clamp access to, where 0 is the largest and most detailed mipmap level and any level higher than that is less detailed.</param>
        /// <param name="maximumLod">Upper end of the mipmap range to clamp access to, where 0 is the largest and most detailed mipmap level and any level higher than that is less detailed. 
        /// This value must be greater than or equal to MinimumLod. To have no upper limit on LOD set this to a large value such as System.Single.MaxValue.</param>
        public SamplerDescription(
            Filter filter,
            TextureAddressMode addressU,
            TextureAddressMode addressV,
            TextureAddressMode addressW,
            float mipLodBias,
            int maximumAnisotropy,
            Comparison comparisonFunction,
            Color4 borderColor,
            float minimumLod,
            float maximumLod)
        {
            Filter = filter;
            AddressU = addressU;
            AddressV = addressV;
            AddressW = addressW;
            MipLodBias = mipLodBias;
            MaximumAnisotropy = maximumAnisotropy;
            ComparisonFunction = comparisonFunction;
            BorderColor = borderColor;
            MinimumLod = minimumLod;
            MaximumLod = maximumLod;
        }

        public bool Equals(SamplerDescription other)
        {
            return
                Filter == other.Filter &&
                AddressU == other.AddressU &&
                AddressV == other.AddressV &&
                AddressW == other.AddressW &&
                MipLodBias == other.MipLodBias &&
                MaximumAnisotropy == other.MaximumAnisotropy &&
                ComparisonFunction == other.ComparisonFunction &&
                Color4.Equals(ref BorderColor, ref other.BorderColor) &&
                MinimumLod == other.MinimumLod &&
                MaximumLod == other.MaximumLod;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is SamplerDescription)) return false;
            SamplerDescription other = (SamplerDescription)obj;
            return
                Filter == other.Filter &&
                AddressU == other.AddressU &&
                AddressV == other.AddressV &&
                AddressW == other.AddressW &&
                MipLodBias == other.MipLodBias &&
                MaximumAnisotropy == other.MaximumAnisotropy &&
                ComparisonFunction == other.ComparisonFunction &&
                Color4.Equals(ref BorderColor, ref other.BorderColor) &&
                MinimumLod == other.MinimumLod &&
                MaximumLod == other.MaximumLod;
        }

        public override int GetHashCode()
        {
            return (int)Filter | ((int)AddressU << 16) | ((int)AddressV << 19) | ((int)AddressW << 21) |
                ((int)ComparisonFunction << 24) | ((MaximumAnisotropy - 1) << 24) + 
                (MipLodBias.GetHashCode() ^ Color4.ToArgb(ref BorderColor) ^ MinimumLod.GetHashCode() ^ MaximumLod.GetHashCode());
        }

        public static bool Equals(ref SamplerDescription desc1, ref SamplerDescription desc2)
        {
            return
                desc1.Filter == desc2.Filter &&
                desc1.AddressU == desc2.AddressU &&
                desc1.AddressV == desc2.AddressV &&
                desc1.AddressW == desc2.AddressW &&
                desc1.MipLodBias == desc2.MipLodBias &&
                desc1.MaximumAnisotropy == desc2.MaximumAnisotropy &&
                desc1.ComparisonFunction == desc2.ComparisonFunction &&
                Color4.Equals(ref desc1.BorderColor, ref desc2.BorderColor) &&
                desc1.MinimumLod == desc2.MinimumLod &&
                desc1.MaximumLod == desc2.MaximumLod;
        }

        public static SamplerDescription Default
        {
            get
            {
                return new SamplerDescription
                {
                    Filter = Filter.MinMagMipLinear,
                    AddressU = TextureAddressMode.Clamp,
                    AddressV = TextureAddressMode.Clamp,
                    AddressW = TextureAddressMode.Clamp,
                    MipLodBias = 0.0f,
                    MaximumAnisotropy = 16,
                    ComparisonFunction = Comparison.Never,
                    BorderColor = new Color4(),
                    MaximumLod = 3.402823466e+38f,
                    MinimumLod = -3.402823466e+38f
                };
            }
        }

        public static SamplerDescription Anisotropic
        {
            get
            {
                return new SamplerDescription
                {
                    Filter = Filter.Anisotropic,
                    AddressU = TextureAddressMode.Clamp,
                    AddressV = TextureAddressMode.Clamp,
                    AddressW = TextureAddressMode.Clamp,
                    MipLodBias = 0.0f,
                    MaximumAnisotropy = 16,
                    ComparisonFunction = Comparison.Never,
                    BorderColor = new Color4(),
                    MaximumLod = 3.402823466e+38f,
                    MinimumLod = -3.402823466e+38f
                    
                };
            }
        }

        public static SamplerDescription MinMagPointMipLinear
        {
            get
            {
                return new SamplerDescription
                {
                    Filter = Filter.MinMagPointMipLinear,
                    AddressU = TextureAddressMode.Clamp,
                    AddressV = TextureAddressMode.Clamp,
                    AddressW = TextureAddressMode.Clamp,
                    MipLodBias = 0.0f,
                    MaximumAnisotropy = 16,
                    ComparisonFunction = Comparison.Never,
                    BorderColor = new Color4(),
                    MaximumLod = 3.402823466e+38f,
                    MinimumLod = -3.402823466e+38f
                };
            }
        }
    }
}
