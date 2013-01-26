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
using Beholder.Core;
using Beholder.Libraries.ObjectGL4.Resources;
using Beholder.Math;
using Beholder.Platform;
using ObjectGL;
using ObjectGL.GL42;
using OpenTK.Graphics.OpenGL;
using Buffer = ObjectGL.GL42.Buffer;

namespace Beholder.Libraries.ObjectGL4.Core
{
    class Presenter : IDisposable
    {
        struct Vertex
        {
            public Vector2 Position;

            public Vertex(float px, float py)
            {
                Position.X = px;
                Position.Y = py;
            }
        }

        const string VertexShaderText =
@"#version 150

in vec4 in_position;

void main()
{
    gl_Position = vec4(in_position.x, in_position.y, 0.0f, 1.0f);
}
";

        const string FragmentShaderText =
@"#version 150

uniform sampler2D Image;

layout(pixel_center_integer) in vec4 gl_FragCoord;

out vec4 out_color;

void main()
{
    ivec2 screenpos = ivec2(gl_FragCoord.x, textureSize(Image, 0).y - gl_FragCoord.y - 1);
    out_color = texelFetch(Image, screenpos, 0);
}
";

        readonly Context glContext;
        readonly Pipeline glPipeline;
        readonly Framebuffer sourceFramebuffer;
        readonly Framebuffer destinationFramebuffer;
        readonly Sampler sampler;
        readonly VertexShader vertexShader;
        readonly FragmentShader fragmentShader;
        readonly ShaderProgram program;
        readonly VertexArray vertexArray;
        readonly Buffer vertices;
        readonly Buffer indices;
        Viewport[] tempViewports;

        public Presenter(Context glContext)
        {
            this.glContext = glContext;
            glPipeline = glContext.Pipeline;

            sourceFramebuffer = new Framebuffer(glContext);
            destinationFramebuffer = new Framebuffer(glContext);

            vertices = new Buffer(glContext, BufferTarget.ArrayBuffer, 4 * 8 * sizeof(float), BufferUsageHint.StaticDraw, new Data(new[]
            {
                new Vertex(-1f, -1f),
                new Vertex(-1f, 1f),
                new Vertex(1f, 1f),
                new Vertex(1f, -1f)
            }));

            indices = new Buffer(glContext, BufferTarget.ElementArrayBuffer, 6 * sizeof(ushort), BufferUsageHint.StaticDraw, new Data(new ushort[] 
            { 
                0, 1, 2, 0, 2, 3
            }));

            sampler = new Sampler();
            sampler.SetMagFilter(TextureMagFilter.Nearest);
            sampler.SetMinFilter(TextureMinFilter.Nearest);

            string shaderErrors;
            if (!VertexShader.TryCompile(VertexShaderText, out vertexShader, out shaderErrors) ||
                !FragmentShader.TryCompile(FragmentShaderText, out fragmentShader, out shaderErrors) ||
                !ShaderProgram.TryLink(glContext, new ShaderProgramDescription
                    {
                        VertexShaders = vertexShader,
                        FragmentShaders = fragmentShader,
                        VertexAttributeNames = new[] { "in_position" },
                        SamplerNames = new[] { "Image" }
                    }, 
                    out program, out shaderErrors))
                throw new ArgumentException("Program errors:\n\n" + shaderErrors);

            vertexArray = new VertexArray(glContext);
            vertexArray.SetElementArrayBuffer(glContext, indices);
            vertexArray.SetVertexAttributeF(glContext, 0, vertices, VertexAttributeDimension.Two, VertexAttribPointerType.Float, false, 8, 0);
        }

        public void Dispose()
        {
            vertexArray.Dispose();
            program.Dispose();
            fragmentShader.Dispose();
            vertexShader.Dispose();
            sampler.Dispose();
            indices.Dispose();
            vertices.Dispose();
            destinationFramebuffer.Dispose();
            sourceFramebuffer.Dispose();
        }

        public void OnEndScene(Context glImmediateContext, CBackBuffer backBuffer)
        {
            var boundProgram = glPipeline.Program;
            var boundVertexArray = glPipeline.VertexArray;
            var boundTexture = glPipeline.Textures[0];
            var boundSampler = glPipeline.Samplers[0];
            var boundFramebuffer = glPipeline.Framebuffer;

            var boundViewportCount = glPipeline.Viewports.EnabledViewportCount;
            if (tempViewports == null || tempViewports.Length < boundViewportCount)
                tempViewports = new Viewport[boundViewportCount];
            for (int i = 0; i < boundViewportCount; i++)
            {
                var glViewport = glPipeline.Viewports[i];
                tempViewports[i] = new Viewport
                {
                    Left = glViewport.X,
                    Top = glViewport.Y,
                    Width =  glViewport.Width,
                    Height = glViewport.Height,
                    MinDepth = glViewport.Near,
                    MaxDepth = glViewport.Far
                };
            }

            var boundCullFaceEnable = glPipeline.Rasterizer.CullFaceEnable;
            var boundPolyginModeFront = glPipeline.Rasterizer.PolygonModeFront;
            var boundPolygonModeBack = glPipeline.Rasterizer.PolygonModeBack;
            var boundScissorEnable = glPipeline.Rasterizer.ScissorEnable;
            var boundMultisampleEnable = glPipeline.Rasterizer.MultisampleEnable;
            var boundLineSmoothEnable = glPipeline.Rasterizer.LineSmoothEnable;

            var boundDepthTestEnable = glPipeline.DepthStencil.DepthTestEnable;
            var boundDepthMask = glPipeline.DepthStencil.DepthMask;
            var boundStencilTestEnable = glPipeline.DepthStencil.StencilTestEnable;

            var boundBlendEnable = glPipeline.Blend.BlendEnable;
            var boundAlphaToCoverageEnable = glPipeline.Blend.AlphaToCoverageEnable;
            var boundSampleMask = glPipeline.Blend.SampleMask;

            glPipeline.Program = program;
            glPipeline.VertexArray = vertexArray;
            
            if (backBuffer.Sampling != Sampling.NoMultisampling)
            {
                sourceFramebuffer.AttachTextureImage(glContext, FramebufferAttachmentPoint.Color0, (Texture2DMultisample)backBuffer.GLTexture);
                destinationFramebuffer.AttachTextureImage(glContext, FramebufferAttachmentPoint.Color0, backBuffer.GLResolveTexture, 0);
               glContext.BlitFramebuffer(
                   sourceFramebuffer, 0, 0, backBuffer.Width, backBuffer.Height,
                   destinationFramebuffer, 0, 0, backBuffer.Width, backBuffer.Height,
                   ClearBufferMask.ColorBufferBit, BlitFramebufferFilter.Nearest);
                glPipeline.Textures[0] = backBuffer.GLResolveTexture;
            }
            else
            {
                glPipeline.Textures[0] = backBuffer.GLTexture;
            }
            
            glPipeline.Samplers[0] = sampler;
            glPipeline.Framebuffer = null;
            glPipeline.Viewports.EnabledViewportCount = 1;
            glPipeline.Viewports[0].Set(backBuffer.Width, backBuffer.Height);

            glPipeline.Rasterizer.CullFaceEnable = false;
            glPipeline.Rasterizer.PolygonModeFront = PolygonMode.Fill;
            glPipeline.Rasterizer.PolygonModeBack = PolygonMode.Fill;
            glPipeline.Rasterizer.ScissorEnable = false;
            glPipeline.Rasterizer.MultisampleEnable = false;
            glPipeline.Rasterizer.LineSmoothEnable = false;

            glPipeline.DepthStencil.DepthTestEnable = false;
            glPipeline.DepthStencil.DepthMask = false;
            glPipeline.DepthStencil.StencilTestEnable = false;

            glPipeline.Blend.BlendEnable = false;
            glPipeline.Blend.AlphaToCoverageEnable = false;
            glPipeline.Blend.SampleMask = 0xffffffff;

            glImmediateContext.DrawElements(BeginMode.Triangles, 6, DrawElementsType.UnsignedShort, 0);

            glPipeline.Program = boundProgram;
            glPipeline.VertexArray = boundVertexArray;
            glPipeline.Textures[0] = boundTexture;
            glPipeline.Samplers[0] = boundSampler;
            glPipeline.Framebuffer = boundFramebuffer;

            for (int i = 0; i < boundViewportCount; i++)
            {
                glPipeline.Viewports[i].Set(
                    tempViewports[i].Left, tempViewports[i].Top, 
                    tempViewports[i].Width, tempViewports[i].Height, 
                    tempViewports[i].MinDepth, tempViewports[i].MaxDepth);
            }
            glPipeline.Viewports.EnabledViewportCount = boundViewportCount;

            glPipeline.Rasterizer.CullFaceEnable = boundCullFaceEnable;
            glPipeline.Rasterizer.PolygonModeFront = boundPolyginModeFront;
            glPipeline.Rasterizer.PolygonModeBack = boundPolygonModeBack;
            glPipeline.Rasterizer.ScissorEnable = boundScissorEnable;
            glPipeline.Rasterizer.MultisampleEnable = boundMultisampleEnable;
            glPipeline.Rasterizer.LineSmoothEnable = boundLineSmoothEnable;

            glPipeline.DepthStencil.DepthTestEnable = boundDepthTestEnable;
            glPipeline.DepthStencil.DepthMask = boundDepthMask;
            glPipeline.DepthStencil.StencilTestEnable = boundStencilTestEnable;

            glPipeline.Blend.BlendEnable = boundBlendEnable;
            glPipeline.Blend.AlphaToCoverageEnable = boundAlphaToCoverageEnable;
            glPipeline.Blend.SampleMask = boundSampleMask;
        }
    }
}
