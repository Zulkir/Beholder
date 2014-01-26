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

using Beholder.Shaders;
using Beholder.Utility.ForImplementations;

namespace Beholder.Libraries.SharpDX9.Shaders
{
    class CShaderCombination : DeviceChildBase<ICDevice>, IShaderCombination
    {
        readonly CVertexShader vertexShader;
        readonly CPixelShader pixelShader;

        public CVertexShader VertexShader { get { return vertexShader; } }
        public CPixelShader PixelShader { get { return pixelShader; } }

        IVertexShader IShaderCombination.VertexShader { get { return vertexShader; } }
        IPixelShader IShaderCombination.PixelShader { get { return pixelShader; } }
        IHullShader IShaderCombination.HullShader { get { return null; } }
        IDomainShader IShaderCombination.DomainShader { get { return null; } }
        IGeometryShader IShaderCombination.GeometryShader { get { return null; } }

        public CShaderCombination(ICDevice device, CVertexShader vertexShader, CPixelShader pixelShader) 
            : base(device)
        {
            this.vertexShader = vertexShader;
            this.pixelShader = pixelShader;
        }
    }
}
