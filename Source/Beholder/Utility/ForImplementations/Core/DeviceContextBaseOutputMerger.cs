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

using Beholder.Core;
using Beholder.Math;
using Beholder.Resources;
using Beholder.Utility.Collections;
using Beholder.Utility.Patterns;

namespace Beholder.Utility.ForImplementations.Core
{
    public class DeviceContextBaseOutputMerger : IDeviceContextOutputMerger
    {
        readonly DeviceContextBaseResettingArrayBinding<IRenderTargetView> renderTargets;
        readonly DirtyProperty<IDepthStencilView> depthStencil;
        readonly DirtyProperty<int> unorderedAccessResourcesOffset;
        readonly DeviceContextBaseResettingArrayBinding<IUnorderedAccessView> unorderedAccessResources;
        readonly int[] initialCounts;
        readonly SortedIntSet initialCountChangedIndices;

        readonly DirtyProperty<IBlendState> blendState;
        readonly DirtyProperty<Color4> blendFactor;
        readonly DirtyProperty<uint> sampleMask;

        readonly DirtyProperty<IDepthStencilState> depthStencilState;
        readonly DirtyProperty<int> stencilReference;

        public DeviceContextBaseOutputMerger(int maxRenderTargets, int maxUnorderedAccessResources)
        {
            renderTargets = new DeviceContextBaseResettingArrayBinding<IRenderTargetView>(maxRenderTargets, ReferenceEquals);
            depthStencil = new DirtyProperty<IDepthStencilView>();
            unorderedAccessResourcesOffset = new DirtyProperty<int>();
            unorderedAccessResources = new DeviceContextBaseResettingArrayBinding<IUnorderedAccessView>(maxUnorderedAccessResources, ReferenceEquals);
            initialCounts = new int[maxUnorderedAccessResources];
            initialCountChangedIndices = new SortedIntSet();

            blendState = new DirtyProperty<IBlendState>();
            blendFactor = new DirtyProperty<Color4>();
            sampleMask = new DirtyProperty<uint>(uint.MaxValue);

            depthStencilState = new DirtyProperty<IDepthStencilState>();
            stencilReference = new DirtyProperty<int>();
        }

        public DeviceContextBaseResettingArrayBinding<IRenderTargetView> RenderTargets { get { return renderTargets; } }
        public DirtyProperty<IDepthStencilView> DepthStencil { get { return depthStencil; } }
        public DirtyProperty<int> UnorderedAccessResourcesOffset { get { return unorderedAccessResourcesOffset; } }
        public DeviceContextBaseResettingArrayBinding<IUnorderedAccessView> UnorderedAccessResources { get { return unorderedAccessResources; } }
        public int[] InitialCounts { get { return initialCounts; } }
        public SortedIntSet InitialCountsChangedIndices { get { return initialCountChangedIndices; } }

        public DirtyProperty<IBlendState> BlendState { get { return blendState; } }
        public DirtyProperty<Color4> BlendFactor { get { return blendFactor; } }
        public DirtyProperty<uint> SampleMask { get { return sampleMask; } }

        public DirtyProperty<IDepthStencilState> DepthStencilState { get { return depthStencilState; } }
        public DirtyProperty<int> StencilReference { get { return stencilReference; } }

        IDeviceContextResettingArrayBinding<IRenderTargetView> IDeviceContextOutputMerger.RenderTargets { get { return renderTargets; } }
        IDepthStencilView IDeviceContextOutputMerger.DepthStencil { get { return DepthStencil.Value; } set { DepthStencil.Value = value; } }
        int IDeviceContextOutputMerger.UnorderedAccessResourcesOffset { get { return unorderedAccessResourcesOffset.Value; } set { unorderedAccessResourcesOffset.Value = value; } }
        IDeviceContextResettingArrayBinding<IUnorderedAccessView> IDeviceContextOutputMerger.UnorderedAccessResources { get { return UnorderedAccessResources; } }

        public void SetUavInitialCount(int index, int initialCount)
        {
            initialCounts[index] = initialCount;
            initialCountChangedIndices.Add(index);
        }

        IBlendState IDeviceContextOutputMerger.BlendState { get { return blendState.Value; } set { blendState.Value = value; } }
        Color4 IDeviceContextOutputMerger.BlendFactor { get { return blendFactor.Value; } set { blendFactor.Value = value; } }
        uint IDeviceContextOutputMerger.SampleMask { get { return sampleMask.Value; } set { sampleMask.Value = value; } }

        IDepthStencilState IDeviceContextOutputMerger.DepthStencilState { get { return depthStencilState.Value; } set { depthStencilState.Value = value; } }
        int IDeviceContextOutputMerger.StencilReference { get { return stencilReference.Value; } set { stencilReference.Value = value; } }

        public void MarkAsDirty()
        {
            renderTargets.MarkAsDirty();
            depthStencil.MarkAsDirty();
            unorderedAccessResources.MarkAsDirty();

            blendState.MarkAsDirty();
            blendFactor.MarkAsDirty();
            sampleMask.MarkAsDirty();

            depthStencilState.MarkAsDirty();
            stencilReference.MarkAsDirty();
        }
    }
}
