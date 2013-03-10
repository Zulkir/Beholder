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
using System.Linq;
using Beholder.Shaders;
using Beholder.Utility.ForImplementations;
using ObjectGL.GL42;

namespace Beholder.Libraries.ObjectGL4.Shaders
{
    class CShaderCombination : DeviceChildBase<ICDevice>, IShaderCombination
    {
        readonly ShaderProgram glProgram;
        public ShaderProgram GLProgram { get { return glProgram; } }

        public CVertexShader VertexShader { get; private set; }
        public CHullShader HullShader { get; private set; }
        public CDomainShader DomainShader { get; private set; }
        public CGeometryShader GeometryShader { get; private set; }
        public CPixelShader PixelShader { get; private set; }

        IVertexShader IShaderCombination.VertexShader { get { return VertexShader; } }
        IHullShader IShaderCombination.HullShader { get { return HullShader; } }
        IDomainShader IShaderCombination.DomainShader { get { return DomainShader; } }
        IGeometryShader IShaderCombination.GeometryShader { get { return GeometryShader; } }
        IPixelShader IShaderCombination.PixelShader { get { return PixelShader; } }

        public VertexShader GLVertexShader { get; private set; }
        public TesselationControlShader GLTessControlShader { get; private set; }
        public TesselationEvaluationShader GLTessEvalShader { get; private set; }
        public GeometryShader GLGeometryShader { get; private set; }
        public FragmentShader GLFragmentShader { get; private set; }

        public int MaxValueBufferSlotPlusOne { get; private set; }
        public int MaxTextureSlotPlusOne { get; private set; }

        public CShaderCombination(ICDevice device, CVertexShader vertexShader, CHullShader hullShader, CDomainShader domainShader, CGeometryShader geometryShader, CPixelShader pixelShader)
            : base(device)
        {
            // UBO stuff
            var orderedShaders = EnumerateShaders(vertexShader, hullShader, domainShader, geometryShader, pixelShader)
                .Where(s => s.UniformBufferSlots.ApiRangeLength > 0)
                .OrderBy(s => s.UniformBufferSlots.ApiStartSlot);

            if (orderedShaders.Any())
            {
                var prev = orderedShaders.First();
                foreach (var shader in orderedShaders.Skip(1))
                {
                    if (shader.UniformBufferSlots.ApiStartSlot < prev.UniformBufferSlots.ApiStartSlot + prev.UniformBufferSlots.ApiRangeLength) throw new InvalidOperationException(string.Format(
                        "{0} shader '{1}' and {2} shader '{3}' cannot be combined since they have overlapping OpenGL uniform buffer slots.", prev.Stage, prev.Reflection.Name, shader.Stage, shader.Reflection.Name));
                    prev = shader;
                }
                MaxValueBufferSlotPlusOne = prev.UniformBufferSlots.ApiStartSlot + prev.UniformBufferSlots.ApiRangeLength;
            }
            var uboNames = new string[MaxValueBufferSlotPlusOne];
            foreach (var shader in orderedShaders)
                foreach (var ubufferWithSlots in shader.UniformBufferSlots.GetActive())
                    uboNames[ubufferWithSlots.GLSlot] = ubufferWithSlots.Name;

            // Texture stuff
            orderedShaders = EnumerateShaders(vertexShader, hullShader, domainShader, geometryShader, pixelShader)
                .Where(s => s.TextureSlots.ApiRangeLength > 0)
                .OrderBy(s => s.TextureSlots.ApiStartSlot);

            if (orderedShaders.Any())
            {
                var prev = orderedShaders.First();
                foreach (var shader in orderedShaders.Skip(1))
                {
                    if (shader.TextureSlots.ApiStartSlot < prev.TextureSlots.ApiStartSlot + prev.TextureSlots.ApiRangeLength) throw new InvalidOperationException(string.Format(
                        "{0} shader '{1}' and {2} shader '{3}' cannot be combined since they have overlapping texture slots.", prev.Stage, prev.Reflection.Name, shader.Stage, shader.Reflection.Name));
                    prev = shader;
                }
                MaxTextureSlotPlusOne = prev.TextureSlots.ApiStartSlot + prev.TextureSlots.ApiRangeLength;
            }
            var samplerNames = new string[MaxTextureSlotPlusOne];
            foreach (var shader in orderedShaders)
                foreach (var textureWithSlots in shader.TextureSlots.GetActive())
                    samplerNames[textureWithSlots.GLSlot] = textureWithSlots.Name;

            // todo: check shader IO compatability

            VertexShader = vertexShader;
            HullShader = hullShader;
            DomainShader = domainShader;
            GeometryShader = geometryShader;
            PixelShader = pixelShader;

            // todo: tesselation

            GLVertexShader =
                hullShader != null ? vertexShader.GetGLShaderToHull() :
                geometryShader != null ? vertexShader.GetGLShaderToGeometry() :
                vertexShader.GetGLShaderToPixel();

            if (hullShader != null || domainShader != null)
            {
                GLTessControlShader = hullShader.GetGLShader();
                var layout = hullShader.EncodeLayout();
                GLTessEvalShader = 
                    geometryShader != null ? domainShader.GetGLShaderToGeometry(layout) 
                    : domainShader.GetGLShaderToPixel(layout);
            }
            if (geometryShader != null)
                GLGeometryShader = geometryShader.GetGLShaderToPixel();

            if (pixelShader != null)
                GLFragmentShader = pixelShader.GetGLShaderToOutputMerger();

            // todo: tesselation, transform feedback

            string errors;
            if (!ShaderProgram.TryLink(device.GetCurrentContext(), new ShaderProgramDescription
                {
                    VertexShaders = GLVertexShader,
                    TesselationControlShaders = GLTessControlShader,
                    TesselationEvaluationShaders = GLTessEvalShader,
                    GeometryShaders = GLGeometryShader,
                    FragmentShaders = GLFragmentShader,
                    VertexAttributeNames = vertexShader.AttributeNames,
                    UniformBufferNames = uboNames,
                    SamplerNames = samplerNames
                }, 
                out glProgram, out errors))
                throw new InvalidOperationException("Failed to link a program from multiple shaders.\n\r\n\rErrors:" + errors);
        }

        static IEnumerable<CShader> EnumerateShaders(CVertexShader vertexShader, CHullShader hullShader, CDomainShader domainShader, CGeometryShader geometryShader, CPixelShader pixelShader)
        {
            if (vertexShader != null) yield return vertexShader;
            if (hullShader != null) yield return hullShader;
            if (domainShader != null) yield return domainShader;
            if (geometryShader != null) yield return geometryShader;
            if (pixelShader != null) yield return pixelShader;
        }
    }
}
