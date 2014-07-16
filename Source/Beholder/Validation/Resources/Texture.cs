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
using System.Linq;
using Beholder.Platform;
using Beholder.Resources;
using Beholder.Utility.Helpers;

namespace Beholder.Validation.Resources
{
    abstract class Texture<T> : Texture, IWrapper<T> where T : ITexture
    {
        public new T Real { get; private set; }
        protected Texture(T real) : base(real) { Real = real; }
    }

    abstract class Texture : Resource<ITexture>, ITexture
    {
        protected Texture(ITexture real) : base(real) { }

        public int FormatID { get { CheckNotDisposed(); return Real.FormatID; } }
        public int MipLevels { get { CheckNotDisposed(); return Real.MipLevels; } }

        protected static void ValidateTextureDescription(IAdapter adapter, int width, int height, int depth, 
            int arraySize, int mipLevels, int formatID, FormatSupport requiredSupport, 
            Usage usage, BindFlags bindFlags, MiscFlags miscFlags, ExtraFlags extraFlags)
        {
            ValidateResourceDescription(usage, bindFlags);
            Check.Positive(width, "Width");
            Check.Positive(height, "Height");
            Check.Positive(depth, "Depth");
            Check.Positive(arraySize, "ArraySize");
            Check.Positive(mipLevels, "MipLevels");
            // todo: check max values also
            if (usage == Usage.Dynamic && mipLevels != 1)
                throw new ArgumentException("Dynamic texture must have exactly 1 mip level");
            if (mipLevels > TextureHelper.MipLevels(width, height, depth))
                throw new ArgumentException(string.Format(
                    "MipLevels value of {0} is greamter than the maximum for a {1}x{2}x{3} texture, which is is {4}",
                    mipLevels, width, height, depth, TextureHelper.MipLevels(width, height, depth)));
            if (!adapter.GetSupportedFormats(requiredSupport).Any(fi => fi.ID == formatID))
                throw new ArgumentException("Given format does not satisfy the required " + requiredSupport + " support");
            if (bindFlags.HasFlag(BindFlags.VertexBuffer))
                throw new ArgumentException("BindFlags.VertexBuffer is not supported by Textures");
            if (bindFlags.HasFlag(BindFlags.IndexBuffer))
                throw new ArgumentException("BindFlags.IndexBuffer is not supported by Textures");
            if (bindFlags.HasFlag(BindFlags.UniformBuffer))
                throw new ArgumentException("BindFlags.UniformBuffer is not supported by Textures");
            if (bindFlags.HasFlag(BindFlags.StreamOutput))
                throw new ArgumentException("BindFlags.StreamOutput is not supported by Textures");
            if (miscFlags.HasFlag(MiscFlags.BufferAllowRawViews))
                throw new ArgumentException("MiscFlags.BufferAllowRawViews is not supported by Textures");
            if (miscFlags.HasFlag(MiscFlags.BufferStructured))
                throw new ArgumentException("MiscFlags.BufferStructured is not supported by Textures");
            if (miscFlags.HasFlag(MiscFlags.DrawInderectArgs))
                throw new ArgumentException("MiscFlags.DrawInderectArgs is not supported by Textures");
            if (extraFlags.HasFlag(ExtraFlags.Points))
                throw new ArgumentException("ExtraFlags.Points is not supported by Textures");
            if (extraFlags.HasFlag(ExtraFlags.SixteenBitIndices))
                throw new ArgumentException("ExtraFlags.SixteenBitIndices is not supported by Textures");
        }
    }
}
