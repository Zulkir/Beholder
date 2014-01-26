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
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Beholder.Libraries.ObjectGL4.Resources;
using Beholder.Platform;
using Beholder.Resources;
using Beholder.Shaders;
using Beholder.Utility.Collections;
using Beholder.Utility.ForImplementations;
using Beholder.Utility.ForImplementations.Platform;
using Beholder.Utility.ForImplementations.Resources;
using Beholder.Utility.Helpers;
using ObjectGL;
using ObjectGL.GL42;

namespace Beholder.Libraries.ObjectGL4
{
    public class CDeviceChildLoader : DeviceChildBase<ICDevice>, IDeviceChildLoader, IDisposable
    {
        readonly IFileSystem fileSystem;
        readonly ConcurrentHashSet<CTexture2D> textures2DLoaded = new ConcurrentHashSet<CTexture2D>();
        readonly IEnumerable<IDisposable>[] disposableResources;

        public CDeviceChildLoader(ICDevice device, IFileSystem fileSystem)
            : base(device)
        {
            this.fileSystem = fileSystem ?? new StraightforwardFileSystem();

            disposableResources = new IEnumerable<IDisposable>[]
            {
                textures2DLoaded
            };
        }

        public void Dispose()
        {
            foreach (var container in disposableResources)
                foreach (var resource in container)
                    resource.Dispose();
        }

        public ITexture1D Texture1D(string fileName)
        {
            throw new NotImplementedException();
        }

        public unsafe ITexture2D Texture2D(string fileName)
        {
            var data = fileSystem.ReadBinary(fileName);
            if (IsDDS(data))
                throw new NotImplementedException();

            Bitmap bitmap;
            using (var stream = new MemoryStream(data))
            {
                bitmap = new Bitmap(stream);
            }

            var desc = new Texture2DDescription
            {
                Width = bitmap.Width,
                Height = bitmap.Height,
                MipLevels = TextureHelper.MipLevels(bitmap.Width, bitmap.Height, 1),
                ArraySize = 1,
                FormatID = (int)Format.Rgba8,
                Sampling = Sampling.NoMultisampling,
                Usage = Usage.Immutable,
                BindFlags = BindFlags.ShaderResource,
                MiscFlags = MiscFlags.None,
                ExtraFlags = ExtraFlags.None
            };
            var tex = new CTexture2D(device, ref desc, null, t => textures2DLoaded.Remove(t));
            var glTex = (Texture2D)tex.GLResource;
            var rawdData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            glTex.SetData(device.GetCurrentContext(), 0, new Data(rawdData.Scan0), FormatColor.Bgr, FormatType.UnsignedByte);
            bitmap.UnlockBits(rawdData);
            glTex.GenerateMipmap(device.GetCurrentContext());
            return CreatorHelper.StoreAndReturn(tex, textures2DLoaded);
        }

        public ITexture3D Texture3D(string fileName)
        {
            throw new NotImplementedException();
        }

        static bool IsDDS(byte[] data)
        {
            return data[0] == 0x44 && data[1] == 0x44 && data[2] == 0x53 && data[3] == 0x20;
        }

        public IVertexShader VertexShader(string fileName)
        {
            var lines = fileSystem.ReadText(fileName);
            var shaderReflection = ShaderParser.ParseLines(lines);
            return device.Create.VertexShader(shaderReflection);
        }

        public IHullShader HullShader(string fileName)
        {
            var lines = fileSystem.ReadText(fileName);
            var shaderReflection = ShaderParser.ParseLines(lines);
            return device.Create.HullShader(shaderReflection);
        }

        public IDomainShader DomainShader(string fileName)
        {
            var lines = fileSystem.ReadText(fileName);
            var shaderReflection = ShaderParser.ParseLines(lines);
            return device.Create.DomainShader(shaderReflection);
        }

        public IGeometryShader GeometryShader(string fileName)
        {
            var lines = fileSystem.ReadText(fileName);
            var shaderReflection = ShaderParser.ParseLines(lines);
            return device.Create.GeometryShader(shaderReflection);
        }

        public IPixelShader PixelShader(string fileName)
        {
            var lines = fileSystem.ReadText(fileName);
            var shaderReflection = ShaderParser.ParseLines(lines);
            return device.Create.PixelShader(shaderReflection);
        }

        public IComputeShader ComputeShader(string fileName)
        {
            var lines = fileSystem.ReadText(fileName);
            var shaderReflection = ShaderParser.ParseLines(lines);
            return device.Create.ComputeShader(shaderReflection);
        }
    }
}
