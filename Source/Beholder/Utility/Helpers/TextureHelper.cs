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

namespace Beholder.Utility.Helpers
{
    public static class TextureHelper
    {
        public static int MipSize(int mipSlice, int baseSliceSize)
        {
            return System.Math.Max(1, baseSliceSize >> mipSlice);
        }

        public static int MipLevels(int width, int height, int depth)
        {
            int maxDimension = System.Math.Max(System.Math.Max(width, height), depth);
            int result = 1;
            while (maxDimension > 1)
            {
                maxDimension >>= 1;
                result++;
            }
            return result;
        }

        public static int EncodeSubresourceIndex(int mipCount, int arrayIndex, int mipLevel)
        {
            return mipLevel + (arrayIndex * mipCount);
        }

        public static void DecodeSubresourceIndex(int mipCount, int subresourceIndex, out int arrayIndex, out int mipLevel)
        {
            arrayIndex = subresourceIndex / mipCount;
            mipLevel = subresourceIndex % mipCount;
        }

        public static void GetInfoForSetData(int bitsPerPixel, int width, int height, int depth, int byteAlignment, out int rowPitch, out int slicePitch, out int totalSize)
        {
            int packedRowBytes = DataHelper.Align(width * bitsPerPixel, 8) / 8;
            rowPitch = DataHelper.Align(packedRowBytes, byteAlignment);
            slicePitch = rowPitch * height;
            totalSize = slicePitch * depth;
        }

        public static void GetInfoForSetDataCompressed(int bitsPerPixel, int width, int height, int depth, int byteAlignment, out int rowPitch, out int slicePitch, out int totalSize)
        {
            int storedWidth = DataHelper.Align(width, 4);
            int storedHeight = DataHelper.Align(height, 4);
            int packedBlockRowBytes = (storedWidth * bitsPerPixel) / 8;
            int heightInBlocks = storedHeight / 4;
            rowPitch = DataHelper.Align(packedBlockRowBytes, byteAlignment);
            slicePitch = rowPitch * heightInBlocks;
            totalSize = slicePitch * depth;
        }
    }
}
