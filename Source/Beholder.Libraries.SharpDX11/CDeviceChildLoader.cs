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
using System.Collections.Generic;
using Beholder.Libraries.SharpDX11.Resources;
using Beholder.Platform;
using Beholder.Resources;
using Beholder.Shaders;
using Beholder.Utility.Collections;
using Beholder.Utility.ForImplementations;
using Beholder.Utility.ForImplementations.Platform;
using Beholder.Utility.ForImplementations.Resources;

namespace Beholder.Libraries.SharpDX11
{
    public class CDeviceChildLoader : DeviceChildBase<ICDevice>, IDeviceChildLoader
    {
        readonly IFileSystem fileSystem;
        readonly ConcurrentHashSet<CTexture1D> textures1DLoaded = new ConcurrentHashSet<CTexture1D>();
        readonly ConcurrentHashSet<CTexture2D> textures2DLoaded = new ConcurrentHashSet<CTexture2D>();
        readonly ConcurrentHashSet<CTexture3D> textures3DLoaded = new ConcurrentHashSet<CTexture3D>();
        readonly IEnumerable<IDisposable>[] disposableResources;

        public CDeviceChildLoader(ICDevice device, IFileSystem fileSystem)
            : base(device)
        {
            this.fileSystem = fileSystem ?? new StraightforwardFileSystem();

            disposableResources = new IEnumerable<IDisposable>[]
            {
                textures1DLoaded,
                textures2DLoaded,
                textures3DLoaded,
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
            return CreatorHelper.StoreAndReturn(CTexture1D.FromFile(device, fileSystem, fileName, t => textures1DLoaded.Remove(t)), textures1DLoaded);
        }

        public ITexture2D Texture2D(string fileName)
        {
            return CreatorHelper.StoreAndReturn(CTexture2D.FromFile(device, fileSystem, fileName, t => textures2DLoaded.Remove(t)), textures2DLoaded);
        }

        public ITexture3D Texture3D(string fileName)
        {
            return CreatorHelper.StoreAndReturn(CTexture3D.FromFile(device, fileSystem, fileName, t => textures3DLoaded.Remove(t)), textures3DLoaded);
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