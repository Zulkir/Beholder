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
using Beholder.Resources;
using Beholder.Utility.ForImplementations.Resources;
using ObjectGL;
using ObjectGL.GL42;
using OpenTK.Graphics.OpenGL;
using Buffer = ObjectGL.GL42.Buffer;
using IResource = ObjectGL.GL42.IResource;

namespace Beholder.Libraries.ObjectGL4.Resources
{
    sealed class CBuffer : BufferBase<ICDevice>, ICResource
    {
        readonly Buffer glBuffer;

        public CBuffer(ICDevice device, ref BufferDescription desc, ref SubresourceData initialData, Action<CBuffer> onRelease)
            : base(device, ref desc, b => onRelease((CBuffer)b))
        {
            BufferTarget bufferTarget;
            if (desc.BindFlags.HasFlag(BindFlags.UniformBuffer))
                bufferTarget = BufferTarget.UniformBuffer;
            else if (desc.BindFlags.HasFlag(BindFlags.StreamOutput))
                bufferTarget = BufferTarget.TransformFeedbackBuffer;
            else if (desc.BindFlags.HasFlag(BindFlags.IndexBuffer))
                bufferTarget = BufferTarget.ElementArrayBuffer;
            else if (desc.BindFlags.HasFlag(BindFlags.VertexBuffer))
                bufferTarget = BufferTarget.ArrayBuffer;
            else 
                bufferTarget = BufferTarget.TextureBuffer;

            BufferUsageHint bufferUsageHint;
            switch (desc.Usage)
            {
                case Usage.Immutable:
                case Usage.Default: bufferUsageHint = BufferUsageHint.StaticDraw; break;
                case Usage.Dynamic: bufferUsageHint = BufferUsageHint.DynamicDraw; break;
                case Usage.Staging: bufferUsageHint = BufferUsageHint.StreamRead; break;
                default: throw new ArgumentOutOfRangeException();
            }

            glBuffer = initialData.Pointer == IntPtr.Zero
                ? new Buffer(device.GetCurrentContext(), bufferTarget, desc.SizeInBytes, bufferUsageHint)
                : new Buffer(device.GetCurrentContext(), bufferTarget, desc.SizeInBytes, bufferUsageHint, new Data(initialData.Pointer, initialData.UnpinPointer));
        }

        public IResource GLResource { get { return glBuffer; } }
        public Buffer GLBuffer { get { return glBuffer; } }

        protected override void DisposeOfNative()
        {
            glBuffer.Dispose();
        }

        public void SetSubresourceData(CDeviceContext deviceContext, int subresourceIndex, SubresourceData data)
        {
            glBuffer.SetData(deviceContext.GLContext, glBuffer.CreationTarget, new Data(data.Pointer, data.UnpinPointer));
        }

        #region For BufferBase
        protected override void DisposeRtv(IRenderTargetView view) { throw new NotSupportedException("Render-to-Buffer techniques are not supported by the core OpenGL"); }
        protected override void DisposeSrv(IShaderResourceView view) { ((CShaderResourceView)view).GLTexture.Dispose(); }
        protected override void DisposeUav(IUnorderedAccessView view) { throw new NotImplementedException(); }

        protected override bool DescEqualsRtv(IRenderTargetView view, ref RenderTargetViewDescription viewDescription) { throw new NotSupportedException("Render-to-Buffer techniques are not supported by the core OpenGL"); }
        protected override bool DescEqualsSrv(IShaderResourceView view, ref ShaderResourceViewDescription viewDescription) { return ((CShaderResourceView)view).DescEquals(ref viewDescription); }
        protected override bool DescEqualsUav(IUnorderedAccessView view, ref UnorderedAccessViewDescription viewDescription) { throw new NotImplementedException(); }

        protected override IRenderTargetView CreateRtv(ref RenderTargetViewDescription viewDescription) { throw new NotSupportedException("Render-to-Buffer techniques are not supported by the core OpenGL"); }

        protected override IShaderResourceView CreateSrv(ref ShaderResourceViewDescription viewDescription)
        {
            // todo: check format for being SizedInternalFormat
            return new CShaderResourceView(this, new TextureBuffer(device.GetCurrentContext(), glBuffer, (SizedInternalFormat)viewDescription.FormatID), ref viewDescription);
        }
        
        protected override IUnorderedAccessView CreateUav(ref UnorderedAccessViewDescription viewDescription) { throw new NotImplementedException(); }
        #endregion
    }
}
