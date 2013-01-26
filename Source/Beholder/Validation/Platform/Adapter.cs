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
using System.Linq;
using Beholder.Core;
using Beholder.Platform;
using Beholder.Utility.Collections.Readonly;

namespace Beholder.Validation.Platform
{
    class Adapter : Wrapper<IAdapter>, IAdapter
    {
        readonly ReadonlyArrayWrapper<Output> roOutputs;

        public Adapter(IAdapter real) : base (real)
        {
            roOutputs = new ReadonlyArrayWrapper<Output>(real.Outputs.Select(Wrappers.Get).ToArray());
        }

        public int Index { get { return Real.Index; } }
        public IReadonlyList<IOutput> Outputs { get { return roOutputs; } }
        public void GetDescription(out AdapterDescription adapterDesc) { Real.GetDescription(out adapterDesc); }
        public Api Api { get { return Real.Api; } }
        public ApiVersion ApiVersion { get { return Real.ApiVersion; } }
        public IAdapterRestrictions Restrictions { get { return Real.Restrictions; } }
        public IFormatInfo GetFormatInfo(int formatID) { return Real.GetFormatInfo(formatID); }
        public IEnumerable<IFormatInfo> GetSupportedWindowedDisplayFormats() { return Real.GetSupportedWindowedDisplayFormats(); }
        public IEnumerable<IFormatInfo> GetSupportedFormats(FormatSupport requiredSupport) { return Real.GetSupportedFormats(requiredSupport); }
        public bool CheckViewFormatCompatability(int textureFormatID, int viewFormatID) { return Real.CheckViewFormatCompatability(textureFormatID, viewFormatID); }
        public IEnumerable<ExplicitFormat> GetSupportedVertexElementFormats() { return Real.GetSupportedVertexElementFormats(); }
        public int GetMultisampleQualityLevelsCount(int formatID, int sampleCount) { return Real.GetMultisampleQualityLevelsCount(formatID, sampleCount); }
    }
}
