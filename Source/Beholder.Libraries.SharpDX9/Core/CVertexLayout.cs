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

using Beholder.Core;
using Beholder.Libraries.SharpDX9.Resources;
using Beholder.Libraries.SharpDX9.Shaders;
using Beholder.Utility.ForImplementations;
using Beholder.Utility.ForImplementations.Core;
using SharpDX.Direct3D9;

namespace Beholder.Libraries.SharpDX9.Core
{
    class CVertexLayout : VertexLayoutBase<ICDevice>, IDefaultPoolResource, IDisposableInternal
    {
        readonly VertexElement[] d3dElements;
        VertexDeclaration d3dVertexDeclaration;

        public VertexDeclaration D3DVertexDeclaration { get { return d3dVertexDeclaration; } }

        public CVertexLayout(ICDevice device, CVertexShader vertexShader, VertexLayoutElement[] elements) 
            : base(device, vertexShader, elements)
        {
            d3dElements = vertexShader.GenerateD3DVertexDeclarationElements(elements);
            CreateNative();
        }

        public void DisposeInternal()
        {
            DisposeOfNative();
        }

        public void OnDeviceLost()
        {
            d3dVertexDeclaration.Dispose();
        }

        public void OnDeviceReset()
        {
            CreateNative();
        }

        void CreateNative()
        {
            d3dVertexDeclaration = new VertexDeclaration(device.D3DDevice, d3dElements);
        }

        void DisposeOfNative()
        {
            d3dVertexDeclaration.Dispose();
        }
    }
}
