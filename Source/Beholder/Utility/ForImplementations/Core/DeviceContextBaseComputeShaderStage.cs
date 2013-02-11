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

using System.Collections.Generic;
using Beholder.Core;
using Beholder.Resources;
using Beholder.Utility.Collections;
using Beholder.Utility.Structures;

namespace Beholder.Utility.ForImplementations.Core
{
    public class DeviceContextBaseComputeShaderStage : DeviceContextBaseShaderStage, IDeviceContextComputeShaderStage
    {
        readonly DeviceContextBaseCumulativeArrayBinding<IUnorderedAccessView> unorderedAccessResources;
        readonly int[] initialCounts;
        readonly SortedIntSet initialCountChangedIndices;

        public DeviceContextBaseComputeShaderStage(int uniformBuffersCount, int samplersCount, int shaderResourcesCount, int unorderedAccessResourcesCount)
            : base(uniformBuffersCount, samplersCount, shaderResourcesCount)
        {
            unorderedAccessResources = new DeviceContextBaseCumulativeArrayBinding<IUnorderedAccessView>(unorderedAccessResourcesCount, ReferenceEquals);
            initialCounts = new int[uniformBuffersCount];
            initialCountChangedIndices = new SortedIntSet();
        }

        public DeviceContextBaseCumulativeArrayBinding<IUnorderedAccessView> UnorderedAccessResources { get { return unorderedAccessResources; } }
        public int[] InitialCounts { get { return initialCounts; } }
        public SortedIntSet InitialCountChangedIndices { get { return initialCountChangedIndices; } }

        IDeviceContextCumulativeArrayBinding<IUnorderedAccessView> IDeviceContextComputeShaderStage.UnorderedAccessResources { get { return unorderedAccessResources; } }

        void IDeviceContextComputeShaderStage.SetUavInitialCount(int index, int initialCount)
        {
            initialCounts[index] = initialCount;
            initialCountChangedIndices.Add(index);
        }

        public IEnumerable<ThreeTuple<int, IUnorderedAccessView, int>> GetChangedIndexViewCountTuples()
        {
            using (var eUavs = unorderedAccessResources.DirtyIndices.GetEnumerator())
            using (var eCounts = initialCountChangedIndices.GetEnumerator())
            {
                bool haveNextUav = eUavs.MoveNext();
                bool haveNextCount = eCounts.MoveNext();

                while (haveNextUav || haveNextCount)
                {
                    if (!haveNextUav)
                    {
                        while (haveNextCount)
                        {
                            var index = eCounts.Current;
                            yield return new ThreeTuple<int, IUnorderedAccessView, int>(index, unorderedAccessResources[index], initialCounts[index]);
                            haveNextCount = eCounts.MoveNext();
                        }
                    }
                    else if (!haveNextCount)
                    {
                        while (haveNextUav)
                        {
                            var index = eUavs.Current;
                            yield return new ThreeTuple<int, IUnorderedAccessView, int>(index, unorderedAccessResources[index], -1);
                            haveNextUav = eUavs.MoveNext();
                        }
                    }
                    else
                    {
                        int index = System.Math.Min(eUavs.Current, eCounts.Current);
                        int count = -1;

                        if (eUavs.Current == index)
                        {
                            haveNextUav = eUavs.MoveNext();
                        }
                        if (eCounts.Current == index)
                        {
                            haveNextCount = eCounts.MoveNext();
                            count = initialCounts[index];
                        }

                        yield return new ThreeTuple<int, IUnorderedAccessView, int>(index, unorderedAccessResources[index], count);
                    }
                }
            }
        }
    }
}
