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

using System.Collections.Generic;
using Beholder.Core;
using Beholder.Utility.Collections.Readonly;

namespace Beholder.Platform
{
    /// <summary>
    /// Represents a display sub-system (including one or more GPUs, DACs and video memory) that will be used by the <see cref="IDevice"/>.
    /// </summary>
    public interface IAdapter
    {
        /// <summary>
        /// Index of this adapter in the <see cref="IEye.Adapters"/> list.
        /// </summary>
        int Index { get; }

        /// <summary>
        /// Outputs (monitors) that are bound to this adapter.
        /// </summary>
        IReadonlyList<IOutput> Outputs { get; }

        /// <summary>
        /// Gets the description of the adapter.
        /// </summary>
        /// <param name="adapterDesc">Variable to fill with the description data.</param>
        void GetDescription(out AdapterDescription adapterDesc);

        Api Api { get; }
        ApiVersion ApiVersion { get; }
        IAdapterRestrictions Restrictions { get; }

        IFormatInfo GetFormatInfo(int formatID);

        /// <summary>
        /// Formats that can be used as a swap chain format in a windowed mode.
        /// </summary>
        IEnumerable<IFormatInfo> GetSupportedWindowedDisplayFormats();

        /// <summary>
        /// Texel formats that can be used for textures of a specified usage.
        /// </summary>
        /// <param name="requiredSupport">Flags specifying how the texture of the desired format will be used.</param>
        IEnumerable<IFormatInfo> GetSupportedFormats(FormatSupport requiredSupport);

        /// <summary>
        /// Checks whether the given texture format and view format are compatible.
        /// </summary>
        /// <param name="textureFormatID">Format of the texture</param>
        /// <param name="viewFormatID">Format of the view</param>
        bool CheckViewFormatCompatability(int textureFormatID, int viewFormatID);

        /// <summary>
        /// Supported vertex formats.
        /// </summary>
        IEnumerable<ExplicitFormat> GetSupportedVertexElementFormats();

        /// <summary>
        /// Gets the number of multisampling quality levels available for a given sample count of a texture with a specified format.
        /// </summary>
        /// <param name="formatID">Format ID of a surface being sampled.</param>
        /// <param name="sampleCount">Number of samples.</param>
        int GetMultisampleQualityLevelsCount(int formatID, int sampleCount);
    }
}
