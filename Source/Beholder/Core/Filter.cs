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

namespace Beholder.Core
{
    /// <summary>
    /// Filtering options during texture sampling.
    /// </summary>
    public enum Filter : byte
    {
        /// <summary>
        /// Use point sampling for minification, magnification, and mip-level sampling.
        /// </summary>
        MinMagMipPoint = 0,
        
        /// <summary>
        /// Use point sampling for minification and magnification; use linear interpolation for mip-level sampling.
        /// </summary>
        MinMagPointMipLinear = 0x1,
        
        /// <summary>
        /// Use point sampling for minification; use linear interpolation for magnification; use point sampling for mip-level sampling.
        /// </summary>
        MinPointMagLinearMipPoint = 0x4,
        
        /// <summary>
        /// Use point sampling for minification; use linear interpolation for magnification and mip-level sampling.
        /// </summary>
        MinPointMagMipLinear = 0x5,
        
        /// <summary>
        /// Use linear interpolation for minification; use point sampling for magnification and mip-level sampling.
        /// </summary>
        MinLinearMagMipPoint = 0x10,

        /// <summary>
        /// Use linear interpolation for minification; use point sampling for magnification; use linear interpolation for mip-level sampling.
        /// </summary>
        MinLinearMagPointMipLinear = 0x11,
        
        /// <summary>
        /// Use linear interpolation for minification and magnification; use point sampling for mip-level sampling.
        /// </summary>
        MinMagLinearMipPoint = 0x14,

        /// <summary>
        /// Use linear interpolation for minification, magnification, and mip-level sampling.
        /// </summary>
        MinMagMipLinear = 0x15,
        
        /// <summary>
        /// Use anisotropic interpolation for minification, magnification, and mip-level sampling.
        /// </summary>
        Anisotropic = 0x55,

        /// <summary>
        /// Use point sampling for minification, magnification, and mip-level sampling. Compare the result to the comparison value.
        /// </summary>
        ComparisonMinMagMipPoint = 0x80,

        /// <summary>
        /// Use point sampling for minification and magnification; use linear interpolation for mip-level sampling. Compare the result to the comparison value.
        /// </summary>
        ComparisonMinMagPointMipLinear = 0x81,

        /// <summary>
        /// Use point sampling for minification; use linear interpolation for magnification; use point sampling for mip-level sampling. Compare the result to the comparison value.
        /// </summary>
        ComparisonMinPointMagLinearMipPoint = 0x84,

        /// <summary>
        /// Use point sampling for minification; use linear interpolation for magnification and mip-level sampling. Compare the result to the comparison value.
        /// </summary>
        ComparisonMinPointMagMipLinear = 0x85,

        /// <summary>
        /// Use linear interpolation for minification; use point sampling for magnification and mip-level sampling. Compare the result to the comparison value.
        /// </summary>
        ComparisonMinLinearMagMipPoint = 0x90,

        /// <summary>
        /// Use linear interpolation for minification; use point sampling for magnification; use linear interpolation for mip-level sampling. Compare the result to the comparison value.
        /// </summary>
        ComparisonMinLinearMagPointMipLinear = 0x91,

        /// <summary>
        /// Use linear interpolation for minification and magnification; use point sampling for mip-level sampling. Compare the result to the comparison value.
        /// </summary>
        ComparisonMinMagLinearMipPoint = 0x94,
        
        /// <summary>
        /// Use linear interpolation for minification, magnification, and mip-level sampling. Compare the result to the comparison value.
        /// </summary>
        ComparisonMinMagMipLinear = 0x95,

        /// <summary>
        /// Use anisotropic interpolation for minification, magnification, and mip-level sampling. Compare the result to the comparison value.
        /// </summary>
        ComparisonAnisotropic = 0xd5
    }
}
