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

namespace Beholder.Resources
{
    /// <summary>
    /// Represents a 3D texture resource that contains texel data, which is structured memory.
    /// </summary>
    public interface ITexture3D : ITexture
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
        /// Texture depth (in texels).
        /// </summary>
        int Depth { get; }
        
        /// <summary>
        /// Gets a description of the texture.
        /// </summary>
        /// <param name="description">Variable to fill with the description data.</param>
        void GetDescription(out Texture3DDescription description);
        
        IRenderTargetView ViewAsRenderTarget(int formatID, int mipSlice, int firstDepthSlice, int depthSliceCount);
        IShaderResourceView ViewAsShaderResource(int formatID, int mostDetailedMip, int mipLevels);
        IUnorderedAccessView ViewAsUnorderedAccessResource(int formatID, int mipSlice, int firstDepthSlice, int depthSliceCount);
    }
}
