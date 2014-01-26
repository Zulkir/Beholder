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
using SharpDX.Direct3D9;

namespace Beholder.Libraries.SharpDX9
{
    static class LocalHelpers
    {
        public static void GetTextureDataSettingParameters(Format format, int width, int height, int depth, int byteAlignment, out int rowPitch, out int slicePitch, out int totalSize)
        {
            switch (format)
            {
                case Format.A32B32G32R32F:
                    TextureHelper.GetInfoForSetData(128, width, height, depth, byteAlignment, out rowPitch, out slicePitch, out totalSize);
                    break;
                case Format.A16B16G16R16:
                case Format.Q16W16V16U16:
                case Format.G32R32F:
                case Format.A16B16G16R16F:
                    TextureHelper.GetInfoForSetData(64, width, height, depth, byteAlignment, out rowPitch, out slicePitch, out totalSize);
                    break;
                case Format.G16R16:
                case Format.X8B8G8R8:
                case Format.A8B8G8R8:
                case Format.A2B10G10R10:
                case Format.V16U16:
                case Format.Q8W8V8U8:
                case Format.A2W10V10U10:
                case Format.X8L8V8U8:
                case Format.R32F:
                case Format.G16R16F:
                case Format.D32Lockable:
                case Format.D24SingleS8:
                case Format.D32SingleLockable:
                case Format.D24X4S4:
                case Format.D24X8:
                case Format.D24S8:
                case Format.D32:
                case Format.X8R8G8B8:
                case Format.A8R8G8B8:
                case Format.A2R10G10B10:
                    TextureHelper.GetInfoForSetData(32, width, height, depth, byteAlignment, out rowPitch, out slicePitch, out totalSize);
                    break;
                case Format.R8G8B8:
                    TextureHelper.GetInfoForSetData(24, width, height, depth, byteAlignment, out rowPitch, out slicePitch, out totalSize);
                    break;
                case Format.A8L8:
                case Format.L16:
                case Format.A8P8:
                case Format.X4R4G4B4:
                case Format.A8R3G3B2:
                case Format.A4R4G4B4:
                case Format.V8U8:
                case Format.L6V5U5:
                case Format.R8G8_B8G8:
                case Format.G8R8_G8B8:
                case Format.R16F:
                case Format.D16:
                case Format.D15S1:
                case Format.D16Lockable:
                case Format.R5G6B5:
                case Format.X1R5G5B5:
                case Format.A1R5G5B5:
                    TextureHelper.GetInfoForSetData(16, width, height, depth, byteAlignment, out rowPitch, out slicePitch, out totalSize);
                    break;
                case Format.A4L4:
                case Format.L8:
                case Format.P8:
                case Format.A8:
                case Format.R3G3B2:
                case Format.S8Lockable:
                    TextureHelper.GetInfoForSetData(8, width, height, depth, byteAlignment, out rowPitch, out slicePitch, out totalSize);
                    break;
                case Format.A1:
                    TextureHelper.GetInfoForSetData(1, width, height, depth, byteAlignment, out rowPitch, out slicePitch, out totalSize);
                    break;
                case Format.Dxt1:
                    TextureHelper.GetInfoForSetDataCompressed(4, width, height, depth, byteAlignment, out rowPitch, out slicePitch, out totalSize);
                    break;
                case Format.Dxt3:
                case Format.Dxt5:
                    TextureHelper.GetInfoForSetDataCompressed(8, width, height, depth, byteAlignment, out rowPitch, out slicePitch, out totalSize);
                    break;
                //case Format.Yuy2:
                //case Format.Uyvy:
                //case Format.Multi2Argb8:
                //case Format.Dxt4:
                //case Format.Dxt2:
                //case Format.Index32:
                //case Format.Index16:
                //case Format.VertexData:
                default:
                    throw new ArgumentException(string.Format("Size of '{0}' format is unknown to Beholder", format));
            }
        }
    }
}
