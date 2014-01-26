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

namespace Beholder.Platform
{
    /// <summary>
    /// Which resources are supported for a given format and given adapter.
    /// </summary>
    [Flags]
    public enum FormatSupport
    {
        /// <summary>
        /// Buffer resources supported.
        /// </summary>
        Buffer = 0x1,

        ///// <summary>
        ///// Vertex buffers supported.
        ///// </summary>
        //IaVertexBuffer = 0x2,

        ///// <summary>
        ///// Index buffers supported.
        ///// </summary>
        //IaIndexBuffer = 0x4,

        /// <summary>
        /// Streaming output buffers supported.
        /// </summary>
        SoBuffer = 0x8,

        /// <summary>
        /// 1D texture resources supported.
        /// </summary>
        Texture1D = 0x10,

        /// <summary>
        /// 2D texture resources supported.
        /// </summary>
        Texture2D = 0x20,

        /// <summary>
        /// 3D texture resources supported.
        /// </summary>
        Texture3D = 0x40,

        /// <summary>
        /// Cube texture resources supported.
        /// </summary>
        TextureCube = 0x80,

        /// <summary>
        /// The intrinsic HLSL function load is supported.
        /// </summary>
        ShaderLoad = 0x100,

        /// <summary>
        /// The intrinsic HLSL functions sample supported.
        /// </summary>
        ShaderSample = 0x200,

        /// <summary>
        /// The intrinsic HLSL functions samplecmp and samplecmplevelzero are supported.
        /// </summary>
        ShaderSampleComparison = 0x400,

        ///// <summary>
        ///// Reserved.
        ///// </summary>
        //ShaderSampleMonoText = 0x800,

        /// <summary>
        /// Mipmaps are supported.
        /// </summary>
        Mip = 0x1000,

        /// <summary>
        /// Automatic generation of mipmaps is supported.
        /// </summary>
        MipAutogen = 0x2000,

        /// <summary>
        /// Rendertargets are supported.
        /// </summary>
        RenderTarget = 0x4000,

        /// <summary>
        /// Blend operations supported.
        /// </summary>
        Blendable = 0x8000,

        /// <summary>
        /// Depth stencils supported.
        /// </summary>
        DepthStencil = 0x10000,

        ///// <summary>
        ///// CPU locking supported.
        ///// </summary>
        //CpuLockable = 0x20000,

        /// <summary>
        /// Multisampling resolution supported. (DeviceContext::UpdateSubresource())
        /// </summary>
        MultisampleResolve = 0x40000,

        ///// <summary>
        ///// Format can be displayed on screen.
        ///// </summary>
        //Display = 0x80000,

        ///// <summary>
        ///// Format cannot be cast to another format. (Has or Is a typeless) 
        ///// </summary>
        //CastWithinBitLayout = 0x100000,

        /// <summary>
        /// Format can be used as a multisampled rendertarget.
        /// </summary>
        MultisampleRendertarget = 0x200000,

        /// <summary>
        /// Format can be used as a multisampled texture and read into a shader with the HLSL load function.
        /// </summary>
        MultisampleLoad = 0x400000,

        /// <summary>
        /// Format can be used with the HLSL gather function. This value is available in DirectX 10.1 or higher.
        /// </summary>
        ShaderGather = 0x800000,

        ///// <summary>
        ///// Format supports casting when used the resource is a back buffer.
        ///// </summary>
        //BackBufferCast = 0x1000000,

        /// <summary>
        /// Format can be used for an unordered access view.
        /// </summary>
        TypedUnorderedAccessView = 0x2000000,

        /// <summary>
        /// Format can be used with the HLSL gather with comparison function.
        /// </summary>
        ShaderGatherComparison = 0x4000000 
    }
}
