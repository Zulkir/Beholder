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

using Beholder.Shaders;
using Beholder.Utility.ForImplementations;
using Beholder.Utility.Helpers;
using SharpDX.Direct3D11;

namespace Beholder.Libraries.SharpDX11.Shaders
{
    class CShaderCombination : DeviceChildBase<ICDevice>, IShaderCombination
    {
        readonly CVertexShader vertexShader;
        readonly CHullShader hullShader;
        readonly CDomainShader domainShader;
        readonly CGeometryShader geometryShader;
        readonly CPixelShader pixelShader;

        readonly VertexShader d3dVertexShader;
        readonly HullShader d3dHullShader;
        readonly DomainShader d3dDomainShader;
        readonly GeometryShader d3dGeometryShader;
        readonly PixelShader d3dPixelShader;

        IVertexShader IShaderCombination.VertexShader { get { return vertexShader; } }
        IHullShader IShaderCombination.HullShader { get { return hullShader; } }
        IDomainShader IShaderCombination.DomainShader { get { return domainShader; } }
        IGeometryShader IShaderCombination.GeometryShader { get { return geometryShader; } }
        IPixelShader IShaderCombination.PixelShader { get { return pixelShader; } }

        public VertexShader D3DVertexShader { get { return d3dVertexShader; } }
        public HullShader D3DHullShader { get { return d3dHullShader; } }
        public DomainShader D3DDomainShader { get { return d3dDomainShader; } }
        public GeometryShader D3DGeometryShader { get { return d3dGeometryShader; } }
        public PixelShader D3DPixelShader { get { return d3dPixelShader; } }

        public CShaderCombination(ICDevice device, 
            CVertexShader vertexShader, CHullShader hullShader, CDomainShader domainShader, 
            CGeometryShader geometryShader, CPixelShader pixelShader) 
            : base(device)
        {
            this.vertexShader = vertexShader;
            this.hullShader = hullShader;
            this.domainShader = domainShader;
            this.geometryShader = geometryShader;
            this.pixelShader = pixelShader;

            d3dVertexShader = GeneralHelper.NullOrChild(vertexShader, s => s.D3DVertexShader);
            d3dHullShader = GeneralHelper.NullOrChild(hullShader, s => s.D3DHullShader);
            d3dDomainShader = GeneralHelper.NullOrChild(domainShader, s => s.D3DDomainShader);
            d3dGeometryShader = GeneralHelper.NullOrChild(geometryShader, s => s.D3DGeometryShader);
            d3dPixelShader = GeneralHelper.NullOrChild(pixelShader, s => s.D3DPixelShader);

            // todo: check io compatability
        }
    }
}
