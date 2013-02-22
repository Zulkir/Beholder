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
    /// Blend options. A blend option identifies the data source and an optional pre-blend operation.
    /// </summary>
    public enum Blend : byte
    {
        /// <summary>
        /// The data source is the color black (0, 0, 0, 0). 
        /// No pre-blend operation.
        /// </summary>
        Zero = 1,


        /// <summary>
        /// The data source is the color white (1, 1, 1, 1). 
        /// No pre-blend operation.
        /// </summary>
        One = 2,


        /// <summary>
        /// The data source is color data (RGB) from a pixel shader. 
        /// No pre-blend operation.
        /// </summary>
        SourceColor = 3,


        /// <summary>
        /// The data source is color data (RGB) from a pixel shader. 
        /// The pre-blend operation inverts the data, generating 1 - RGB.
        /// </summary>
        InverseSourceColor = 4,


        /// <summary>
        /// The data source is alpha data (A) from a pixel shader. 
        /// No pre-blend operation.
        /// </summary>
        SourceAlpha = 5,


        /// <summary>
        /// The data source is alpha data (A) from a pixel shader. 
        /// The pre-blend operation inverts the data, generating 1 - A.
        /// </summary>
        InverseSourceAlpha = 6,


        /// <summary>
        /// The data source is alpha data from a rendertarget. 
        /// No pre-blend operation.
        /// </summary>
        DestinationAlpha = 7,


        /// <summary>
        /// The data source is alpha data from a rendertarget. 
        /// The pre-blend operation inverts the data, generating 1 - A.
        /// </summary>
        InverseDestinationAlpha = 8,


        /// <summary>
        /// The data source is color data from a rendertarget. 
        /// No pre-blend operation.
        /// </summary>
        DestinationColor = 9,


        /// <summary>
        /// The data source is color data from a rendertarget. 
        /// The pre-blend operation inverts the data, generating 1 - RGB.
        /// </summary>
        InverseDestinationColor = 10,


        /// <summary>
        /// The data source is alpha data from a pixel shader. 
        /// The pre-blend operation clamps the data to 1 or less.
        /// </summary>
        SourceAlphaSaturation = 11,


        /// <summary>
        /// The data source is the blend factor set with IDeviceContext.OutputMerger.BlendFactor method. 
        /// No pre-blend operation.
        /// </summary>
        BlendFactor = 14,


        /// <summary>
        /// The data source is the blend factor set with IDeviceContext.OutputMerger.BlendFactor method. 
        /// The pre-blend operation inverts the blend factor, generating 1 - BlendFactor.
        /// </summary>
        InverseBlendFactor = 15,


        /// <summary>
        /// The data sources are both color data output by a pixel shader. 
        /// There is no pre-blend operation. 
        /// This options supports dual-source color blending.
        /// </summary>
        SourceOneColor = 16,


        /// <summary>
        /// The data sources are both color data output by a pixel shader. 
        /// The pre-blend operation inverts the data, generating 1 - RGB. 
        /// This options supports dual-source color blending.
        /// </summary>
        InverseSourceOneColor = 17,


        /// <summary>
        /// The data sources are alpha data output by a pixel shader. 
        /// There is no pre-blend operation. 
        /// This options supports dual-source color blending.
        /// </summary>
        SourceOneAlpha = 18,


        /// <summary>
        /// The data sources are alpha data output by a pixel shader. 
        /// The pre-blend operation inverts the data, generating 1 - A. 
        /// This options supports dual-source color blending.
        /// </summary>
        InverseSourceOneAlpha = 19,
    }
}
