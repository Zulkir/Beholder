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
using Beholder.Platform;
using Beholder.Resources;

namespace Beholder.Validation.Resources
{
    class Buffer : Resource<IBuffer>, IBuffer
    {
        public Buffer(IBuffer real) : base(real) { }

        public int SizeInBytes { get { CheckNotDisposed(); return Real.SizeInBytes; } }
        public int StructureByteStride { get { CheckNotDisposed(); return Real.StructureByteStride; } }
        public void GetDescription(out BufferDescription description) { CheckNotDisposed(); Real.GetDescription(out description); }

        #region View As
        public IRenderTargetView ViewAsRenderTarget(int formatID, int firstElement, int elementCount)
        {
            CheckNotDisposed();
            CheckRenderTargetBinding();
            RenderTargetView.ValidateBuffer(Real, formatID, firstElement, elementCount);
            return Wrappers.Get(Real.ViewAsRenderTarget(formatID, firstElement, elementCount));
        }

        public IShaderResourceView ViewAsShaderResource(int formatID, int firstElement, int elementCount)
        {
            CheckNotDisposed();
            CheckShaderResourceBinding();
            ShaderResourceView.ValidateBuffer(Real, formatID, firstElement, elementCount);
            return Wrappers.Get(Real.ViewAsShaderResource(formatID, firstElement, elementCount));
        }

        public IShaderResourceView ViewAsShaderResourceExtended(int formatID, int firstElement, int elementCount, ShaderResourceViewExtendedBufferFlags flags)
        {
            CheckNotDisposed();
            CheckShaderResourceBinding();
            ShaderResourceView.ValidateBufferEx(Real, formatID, firstElement, elementCount, flags);
            return Wrappers.Get(Real.ViewAsShaderResourceExtended(formatID, firstElement, elementCount, flags));
        }

        public IUnorderedAccessView ViewAsUnorderedAccessResource(int formatID, int firstElement, int elementCount, UnorderedAccessViewBufferFlags flags)
        {
            CheckNotDisposed();
            CheckUnorderedAccessBinding();
            UnorderedAccessView.ValidateBuffer(Real, formatID, firstElement, elementCount, flags);
            return Wrappers.Get(Real.ViewAsUnorderedAccessResource(formatID, firstElement, elementCount, flags));
        }
        #endregion

        public static void ValidateDescription(IAdapter adapter, BufferDescription description)
        {
            Check.Positive(description.SizeInBytes, "SizeInBytes");
            if (description.BindFlags.HasFlag(BindFlags.UniformBuffer) && description.SizeInBytes % 16 != 0)
                throw new ArgumentException("Uniform buffer size must be a multiple of 16");
            // todo: Check max buffer size

            ValidateResourceDescription(description.Usage, description.BindFlags);

            if (description.MiscFlags.HasFlag(MiscFlags.GenerateMips))
                throw new ArgumentException("MiscFlags.GenerateMips flag is invalid for a Buffer resource");
            if (description.MiscFlags.HasFlag(MiscFlags.TextureCube))
                throw new ArgumentException("MiscFlags.TextureCube flag is invalid for a Buffer resource");
            if (description.MiscFlags.HasFlag(MiscFlags.DrawInderectArgs) && description.Usage != Usage.Default)
                throw new ArgumentException("MiscFlags.DrawInderectArgs flag is only available for the Buffers with Default usage");
            if (description.MiscFlags.HasFlag(MiscFlags.BufferAllowRawViews) && description.Usage != Usage.Default)
                throw new ArgumentException("MiscFlags.BufferAllowRawViews flag is only available for the Buffers with Default usage");
            if (description.MiscFlags.HasFlag(MiscFlags.BufferStructured) && description.Usage != Usage.Default)
                throw new ArgumentException("MiscFlags.BufferStructured flag is only available for the Buffers with Default usage");

            if (description.ExtraFlags.HasFlag(ExtraFlags.Points) && !description.BindFlags.HasFlag(BindFlags.VertexBuffer))
                throw new ArgumentException("ExtraFlags.Points flag is only available for the Buffers with BindFlags.VertexBuffer flag set");
            if (description.ExtraFlags.HasFlag(ExtraFlags.SixteenBitIndices) && !description.BindFlags.HasFlag(BindFlags.IndexBuffer))
                throw new ArgumentException("ExtraFlags.SixteenBitIndices flag is only available for the Buffers with BindFlags.IndexBuffer flag set");
            if (description.ExtraFlags.HasFlag(ExtraFlags.UniformInt) && description.ExtraFlags.HasFlag(ExtraFlags.UniformBool))
                throw new ArgumentException("ExtraFlags.UniformInt and ExtraFlags.UniformBool cannot be set together");
            if (description.ExtraFlags.HasFlag(ExtraFlags.UniformInt) && !description.BindFlags.HasFlag(BindFlags.UniformBuffer))
                throw new ArgumentException("ExtraFlags.UniformInt flag is only available for the Buffers with BindFlags.UniformBuffer flag set");
            if (description.ExtraFlags.HasFlag(ExtraFlags.UniformBool) && !description.BindFlags.HasFlag(BindFlags.UniformBuffer))
                throw new ArgumentException("ExtraFlags.UniformBool flag is only available for the Buffers with BindFlags.UniformBuffer flag set");
            Check.NotNegative(description.StructureByteStride, "StructureByteStride");
            // todo: Check max structure size
        }
    }
}
