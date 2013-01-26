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
using Beholder.Resources;

namespace Beholder.Validation.Resources
{
    abstract class Resource : DeviceChild<IResource>, IResource
    {
        protected Resource(IResource real) : base(real) { releasable = new Releasable(Real); }

        readonly Releasable releasable;
        public bool IsReleased { get { return releasable.IsReleased; } }
        public void Release() { releasable.Release(); Wrappers.Remove<IResource>(this); }

        public ResourceDimension Dimension { get { CheckNotReleased(); return Real.Dimension; } }
        public Usage Usage { get { CheckNotReleased(); return Real.Usage; } }
        public BindFlags BindFlags { get { CheckNotReleased(); return Real.BindFlags; } }
        public MiscFlags MiscFlags { get { CheckNotReleased(); return Real.MiscFlags; } }
        public ExtraFlags ExtraFlags { get { CheckNotReleased(); return Real.ExtraFlags; } }

        protected void CheckNotReleased()
        {
            if (IsReleased)
                throw new InvalidOperationException("Trying to use an already released resource");
        }

        protected void CheckRenderTargetBinding()
        {
            if (!BindFlags.HasFlag(BindFlags.RenderTarget))
                throw new InvalidOperationException("Resource must have a RenderTarget bind flag to be viewed as a render target");
        }

        protected void CheckDepthStencilBinding()
        {
            if (!BindFlags.HasFlag(BindFlags.DepthStencil))
                throw new InvalidOperationException("Resource must have a DepthStencil bind flag to be viewed as a depth-stencil");
        }

        protected void CheckShaderResourceBinding()
        {
            if (!BindFlags.HasFlag(BindFlags.ShaderResource))
                throw new InvalidOperationException("Resource must have a ShaderResource bind flag to be viewed as a shader resource");
        }

        protected void CheckUnorderedAccessBinding()
        {
            if (!BindFlags.HasFlag(BindFlags.UnorderedAccess))
                throw new InvalidOperationException("Resource must have a UnorderedAccess bind flag to be viewed as an unordered access resource");
        }

        protected static void ValidateResourceDescription(Usage usage, BindFlags bindFlags)
        {
            switch (usage)
            {
                case Usage.Default:
                    break;
                case Usage.Immutable:
                    if (bindFlags.HasFlag(BindFlags.StreamOutput))
                        throw new ArgumentException("Immutable resource cannot be used as Stream Output since the GPU cannot write into it");
                    if (bindFlags.HasFlag(BindFlags.RenderTarget))
                        throw new ArgumentException("Immutable resource cannot be used as Render Target since the GPU cannot write into it");
                    if (bindFlags.HasFlag(BindFlags.DepthStencil))
                        throw new ArgumentException("Immutable resource cannot be used as Depth-Stencil since the GPU cannot write into it");
                    if (bindFlags.HasFlag(BindFlags.UnorderedAccess))
                        throw new ArgumentException("Immutable resource cannot be used as Unordered Access Resource since the GPU cannot write into it");
                    break;
                case Usage.Dynamic:
                    if (bindFlags.HasFlag(BindFlags.StreamOutput))
                        throw new ArgumentException("Dynamic resource cannot be used as Stream Output since the GPU cannot write into it");
                    if (bindFlags.HasFlag(BindFlags.RenderTarget))
                        throw new ArgumentException("Dynamic resource cannot be used as Render Target since the GPU cannot write into it");
                    if (bindFlags.HasFlag(BindFlags.DepthStencil))
                        throw new ArgumentException("Dynamic resource cannot be used as Depth-Stencil since the GPU cannot write into it");
                    if (bindFlags.HasFlag(BindFlags.UnorderedAccess))
                        throw new ArgumentException("Dynamic resource cannot be used as Unordered Access Resource since the GPU cannot write into it");
                    break;
                case Usage.Staging:
                    if (bindFlags != BindFlags.None)
                        throw new ArgumentException("Staging resource cannot be bound to the graphics pipeline and thus cannot have Bind Flags");
                    break;
                default:
                    throw new ArgumentOutOfRangeException("Usage");
            }

            if (bindFlags.HasFlag(BindFlags.UniformBuffer) && bindFlags != BindFlags.UniformBuffer)
                throw new ArgumentException("BindFlags.UniformBuffer cannot be combined with other bind flags");
        }
    }

    abstract class Resource<T> : Resource, IWrapper<T> where T : IResource
    {
        public new T Real { get; private set; }
        protected Resource(T real) : base(real) { Real = real; }
    }
}
