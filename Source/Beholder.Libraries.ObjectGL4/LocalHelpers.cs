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
using Beholder.Utility.Helpers;
using ObjectGL.GL42;

namespace Beholder.Libraries.ObjectGL4
{
    class LocalHelpers
    {
        public static bool IsFormatCompressed(Format internalFormat)
        {
            switch (internalFormat)
            {
                case Format.CompressedRgbaS3tcDxt1Ext:
                case Format.CompressedSrgbAlphaS3tcDxt1Ext:
                case Format.CompressedRgbaS3tcDxt3Ext:
                case Format.CompressedSrgbAlphaS3tcDxt3Ext:
                case Format.CompressedRgbaS3tcDxt5Ext:
                case Format.CompressedSrgbAlphaS3tcDxt5Ext:
                case Format.CompressedRedRgtc1:
                case Format.CompressedSignedRedRgtc1:
                case Format.CompressedRgRgtc2:
                case Format.CompressedSignedRgRgtc2:
                case Format.CompressedRgbaBptcUf:
                case Format.CompressedRgbaBptcSf:
                case Format.CompressedRgbaBptc:
                case Format.CompressedSrgbAlphaBptc:
                    return true;
                default:
                    return false;
            }
        }

        public static int CompressedImageSize(Format format, int width, int height, int depth, int byteAlignment)
        {
            int rowPitch, slicePitch, totalSize;
            switch (format)
            {
                case Format.CompressedRgbaS3tcDxt1Ext:
                case Format.CompressedSrgbAlphaS3tcDxt1Ext:
                case Format.CompressedRedRgtc1: // bc4
                case Format.CompressedSignedRedRgtc1:
                    TextureHelper.GetInfoForSetDataCompressed(4, width, height, depth, byteAlignment, out rowPitch, out slicePitch, out totalSize);
                    return totalSize;
                case Format.CompressedRgbaS3tcDxt3Ext:
                case Format.CompressedSrgbAlphaS3tcDxt3Ext:
                case Format.CompressedRgbaS3tcDxt5Ext:
                case Format.CompressedSrgbAlphaS3tcDxt5Ext:
                case Format.CompressedRgRgtc2:
                case Format.CompressedSignedRgRgtc2:
                case Format.CompressedRgbaBptcUf:
                case Format.CompressedRgbaBptcSf:
                case Format.CompressedRgbaBptc:
                case Format.CompressedSrgbAlphaBptc:
                    TextureHelper.GetInfoForSetDataCompressed(8, width, height, depth, byteAlignment, out rowPitch, out slicePitch, out totalSize);
                    return totalSize;
                default:
                    throw new InvalidOperationException(string.Format("Format '{0}' is not a known compressed format", format));
            }
        }
    }
}
