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
using System.Drawing;
using System.Drawing.Imaging;
using Beholder;
using Beholder.Core;
using Beholder.Platform;
using Beholder.Resources;
using System.Linq;

namespace DemoRunner
{
    public class TextureLoader
    {
        private readonly IDevice device;
        private readonly int formatId;
        private readonly bool bgra;

        public TextureLoader(IDevice device)
        {
            this.device = device;
            var formatRgba = device.Adapter.GetSupportedFormats(FormatSupport.Texture2D).FirstOrDefault(x => x.ExplicitFormat == ExplicitFormat.R8G8B8A8_UNORM);
            var formatBgra = device.Adapter.GetSupportedFormats(FormatSupport.Texture2D).FirstOrDefault(x => x.ExplicitFormat == ExplicitFormat.B8G8R8A8_UNORM);
            if (formatRgba != null)
            {
                formatId = formatRgba.ID;
                bgra = false;
            }
            else if (formatBgra != null)
            {
                formatId = formatBgra.ID;
                bgra = true;
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        public unsafe ITexture2D Load(string fileName)
        {
            ITexture2D result;
            var bitmap = new Bitmap(fileName);

            var data = new byte[bitmap.Width * bitmap.Height * 4 * 2];
            var mipPointers = new IntPtr[GetMipCount(bitmap.Width, bitmap.Height)];

            fixed (byte* pData = data)
            {
                mipPointers[0] = (IntPtr)pData;

                var bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
                {
                    var dst = pData;
                    var src = (byte*)bitmapData.Scan0;

                    if (bgra)
                    {
                        for (int y = 0; y < bitmap.Height; y++)
                        {
                            for (int x = 0; x < bitmap.Width; x++)
                            {
                                dst[0] = src[0];
                                dst[1] = src[1];
                                dst[2] = src[2];
                                dst[3] = 255;

                                src += 3;
                                dst += 4;
                            }
                        }
                    }
                    else
                    {
                        for (int y = 0; y < bitmap.Height; y++)
                        {
                            for (int x = 0; x < bitmap.Width; x++)
                            {
                                dst[0] = src[2];
                                dst[1] = src[1];
                                dst[2] = src[0];
                                dst[3] = 255;

                                src += 3;
                                dst += 4;
                            }
                        }
                    }


                    int mipWidth = bitmap.Width;
                    int mipHeight = bitmap.Height;

                    for (int level = 1; level < mipPointers.Length; level++)
                    {
                        mipPointers[level] = (IntPtr)dst;
                        src = (byte*)mipPointers[level - 1];

                        int srcRowSpan = mipWidth * 4;

                        mipWidth = Math.Max(mipWidth / 2, 1);
                        mipHeight = Math.Max(mipHeight / 2, 1);

                        for (int y = 0; y < mipHeight; y++)
                        {
                            for (int x = 0; x < mipWidth; x++)
                            {
                                for (int i = 0; i < 3; i++)
                                {
                                    float tl = SrgbToLinear(src[i] / 255f);
                                    float tr = SrgbToLinear(src[i + 4] / 255f);
                                    float bl = SrgbToLinear(src[i + srcRowSpan] / 255f);
                                    float br = SrgbToLinear(src[i + 4 + srcRowSpan] / 255f);

                                    dst[i] = (byte)(LinearToSrgb((tl + tr + bl + br) / 4f) * 255.9999f);
                                }

                                dst[3] = 255;

                                src += 8;
                                dst += 4;
                            }

                            src += srcRowSpan;
                        }
                    }
                }
                bitmap.UnlockBits(bitmapData);

                var desc = new Texture2DDescription
                {
                    Width = bitmap.Width,
                    Height = bitmap.Height,
                    ArraySize = 1,
                    MipLevels = mipPointers.Length,
                    FormatID = formatId,
                    Sampling = Sampling.NoMultisampling,
                    BindFlags = BindFlags.ShaderResource,
                    Usage = Usage.Immutable
                };

                result = device.Create.Texture2D(desc, mipPointers.Select(x => new SubresourceData(x)).ToArray());
            }
            return result;
        }

        private static int GetMipCount(int width, int height)
        {
            int maxDimension = Math.Max(width, height);

            int mipCount = 1;
            while (maxDimension > 1)
            {
                mipCount++;
                maxDimension /= 2;
            }

            return mipCount;
        }

        private static float SrgbToLinear(float c)
        {
            if (c <= 0.04045f)
                return c / 12.92f;

            return (float)Math.Pow((c + 0.055f) / (1f + 0.055f), 2.4f);
        }

        private static float LinearToSrgb(float c)
        {
            if (c <= 0.0031308f)
                return c * 12.92f;

            return (1f + 0.055f) * (float)Math.Pow(c, 1f / 2.4f) - 0.055f;
        }
    }
}