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
using Beholder.Platform;

namespace Beholder.Resources
{
    /// <summary>
    /// Describes a 2D texture.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct Texture2DDescription
    {
        /// <summary>
        /// Texture width (in texels).
        /// </summary>
        public int Width;

        /// <summary>
        /// Texture height (in texels).
        /// </summary>
        public int Height;

        /// <summary>
        /// The maximum number of mipmap levels in the texture.
        /// Use 1 for a multisampled texture.
        /// </summary>
        public int MipLevels;

        /// <summary>
        /// Number of textures in the array.
        /// </summary>
        public int ArraySize;

        /// <summary>
        /// Texture format ID.
        /// </summary>
        public int FormatID;

        /// <summary>
        /// Structure that specifies multisampling parameters for the texture.
        /// </summary>
        public Sampling Sampling;

        /// <summary>
        /// Identify how the resource is expected to be read from and written to. 
        /// Frequency of update is a key factor.
        /// </summary>
        public Usage Usage;

        /// <summary>
        /// Identify how the buffer will be bound to the pipeline. 
        /// Flags can be combined with a logical OR.
        /// </summary>
        public BindFlags BindFlags;

        /// <summary>
        /// Flags that identify other, less common resource options. 
        /// These flags can be combined with a logical OR.
        /// </summary>
        public MiscFlags MiscFlags;

        /// <summary>
        /// Extra flags that specify use-cases of the resource for less flexible APIs (i.e. D3D9).
        /// Flags can be combined with a logical OR.
        /// </summary>
        public ExtraFlags ExtraFlags;

        public Texture2DDescription(
            int width,
            int height,
            int mipLevels,
            int arraySize,
            int formatID,
            Sampling sampling,
            Usage usage,
            BindFlags bindFlags,
            MiscFlags miscFlags,
            ExtraFlags extraFlags)
        {
            Width = width;
            Height = height;
            MipLevels = mipLevels;
            ArraySize = arraySize;
            FormatID = formatID;
            Sampling = sampling;
            Usage = usage;
            BindFlags = bindFlags;
            MiscFlags = miscFlags;
            ExtraFlags = extraFlags;
        }
    }
}
