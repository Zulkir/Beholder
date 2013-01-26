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

using System;
using System.IO;
using Beholder.Resources;
using Beholder.Shaders;

namespace Beholder.Validation
{
    class DeviceChildLoader : IDeviceChildLoader
    {
        readonly IDeviceChildLoader realLoader;

        public IDevice Device { get { return Wrappers.Get(realLoader.Device); } }

        public DeviceChildLoader(IDeviceChildLoader realLoader)
        {
            this.realLoader = realLoader;
        }

        public ITexture1D Texture1D(string fileName)
        {
            if (fileName == null) throw new ArgumentNullException("fileName");
            if (!File.Exists(fileName)) throw new FileNotFoundException("File was not found", fileName);

            return Wrappers.Get(realLoader.Texture1D(fileName));
        }

        public ITexture2D Texture2D(string fileName)
        {
            if (fileName == null) throw new ArgumentNullException("fileName");
            if (!File.Exists(fileName)) throw new FileNotFoundException("File was not found", fileName);

            return Wrappers.Get(realLoader.Texture2D(fileName));
        }

        public ITexture3D Texture3D(string fileName)
        {
            if (fileName == null) throw new ArgumentNullException("fileName");
            if (!File.Exists(fileName)) throw new FileNotFoundException("File was not found", fileName);

            return Wrappers.Get(realLoader.Texture3D(fileName));
        }

        public IVertexShader VertexShader(string fileName)
        {
            if (fileName == null) throw new ArgumentNullException("fileName");
            if (!File.Exists(fileName)) throw new FileNotFoundException("File was not found", fileName);

            return Wrappers.Get(realLoader.VertexShader(fileName));
        }

        public IHullShader HullShader(string fileName)
        {
            if (fileName == null) throw new ArgumentNullException("fileName");
            if (!File.Exists(fileName)) throw new FileNotFoundException("File was not found", fileName);

            return Wrappers.Get(realLoader.HullShader(fileName));
        }

        public IDomainShader DomainShader(string fileName)
        {
            if (fileName == null) throw new ArgumentNullException("fileName");
            if (!File.Exists(fileName)) throw new FileNotFoundException("File was not found", fileName);

            return Wrappers.Get(realLoader.DomainShader(fileName));
        }

        public IGeometryShader GeometryShader(string fileName)
        {
            if (fileName == null) throw new ArgumentNullException("fileName");
            if (!File.Exists(fileName)) throw new FileNotFoundException("File was not found", fileName);

            return Wrappers.Get(realLoader.GeometryShader(fileName));
        }

        public IPixelShader PixelShader(string fileName)
        {
            if (fileName == null) throw new ArgumentNullException("fileName");
            if (!File.Exists(fileName)) throw new FileNotFoundException("File was not found", fileName);

            return Wrappers.Get(realLoader.PixelShader(fileName));
        }

        public IComputeShader ComputeShader(string fileName)
        {
            if (fileName == null) throw new ArgumentNullException("fileName");
            if (!File.Exists(fileName)) throw new FileNotFoundException("File was not found", fileName);

            return Wrappers.Get(realLoader.ComputeShader(fileName));
        }
    }
}
