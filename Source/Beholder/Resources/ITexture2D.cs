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

using Beholder.Platform;

namespace Beholder.Resources
{
    /// <summary>
    /// Represents a 2D texture resource that contains texel data, which is structured memory.
    /// </summary>
    public interface ITexture2D : ITexture
    {
        /// <summary>
        /// Texture width (in texels).
        /// </summary>
        int Width { get; }

        /// <summary>
        /// Texture height (in texels).
        /// </summary>
        int Height { get; }

        /// <summary>
        /// Number of textures in the array.
        /// </summary>
        int ArraySize { get; }

        /// <summary>
        /// Structure that specifies multisampling parameters for the texture. See <see cref="Sampling"/>.
        /// </summary>
        Sampling Sampling { get; }

        /// <summary>
        /// Gets a description of the texture.
        /// </summary>
        /// <param name="texture2DDesc">Variable to fill with the description data.</param>
        void GetDescription(out Texture2DDescription texture2DDesc);

        IRenderTargetView ViewAsRenderTarget(int formatID, int mipSlice);
        IRenderTargetView ViewAsRenderTargetArray(int formatID, int mipSlice, int firstArraySlice, int arraySize);
        IRenderTargetView ViewAsRenderTargetMultisampled(int formatID);
        IRenderTargetView ViewAsRenderTargetMultisampledArray(int formatID, int firstArraySlice, int arraySize);
        
        IDepthStencilView ViewAsDepthStencil(int formatID, DepthStencilViewFlags flags, int mipSlice);
        IDepthStencilView ViewAsDepthStencilArray(int formatID, DepthStencilViewFlags flags, int mipSlice, int firstArraySlice, int arraySize);
        IDepthStencilView ViewAsDepthStencilMultisampled(int formatID, DepthStencilViewFlags flags);
        IDepthStencilView ViewAsDepthStencilMultisampledArray(int formatID, DepthStencilViewFlags flags, int firstArraySlice, int arraySize);
        
        IShaderResourceView ViewAsShaderResource(int formatID, int mostDetailedMip, int mipLevels);
        IShaderResourceView ViewAsShaderResourceArray(int formatID, int mostDetailedMip, int mipLevels, int firstArraySlice, int arraySize);
        IShaderResourceView ViewAsShaderResourceMultisampled(int formatID);
        IShaderResourceView ViewAsShaderResourceMultisampledArray(int formatID, int firstArraySlice, int arraySize);
        IShaderResourceView ViewAsShaderResourceCube(int formatID, int mostDetailedMip, int mipLevels);
        IShaderResourceView ViewAsShaderResourceCubeArray(int formatID, int mostDetailedMip, int mipLevels, int first2DArrayFace, int cubeCount);

        IUnorderedAccessView ViewAsUnorderedAccessResource(int formatID, int mipSlice);
        IUnorderedAccessView ViewAsUnorderedAccessResourceArray(int formatID, int mipSlice, int firstArraySlice, int arraySize);
    }
}
