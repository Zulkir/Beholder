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
using Beholder.Utility.Helpers;

namespace Beholder.Validation.Core
{
    class DeviceContextRasterizer : Wrapper<IDeviceContextRasterizer>, IDeviceContextRasterizer
    {
        readonly DeviceContextRasterizerScissorRectangles scissorRectangles;
        readonly DeviceContextRasterizerViewports viewports;

        public DeviceContextRasterizer(IDeviceContextRasterizer real)
            : base(real)
        {
            scissorRectangles = new DeviceContextRasterizerScissorRectangles(Real.ScissorRectangles);
            viewports = new DeviceContextRasterizerViewports(Real.Viewports);
        }

        public IRasterizerState State
        {
            get { return Wrappers.Get(Real.State); } 
            set
            {
                Check.NullOrInternal(value, "value");
                Real.State = GeneralHelper.NullOrChild(value, s => ((RasterizerState)s).Real);
            }
        }

        public IDeviceContextResettingArrayBinding<IntRectangle> ScissorRectangles { get { return scissorRectangles; } }
        public IDeviceContextResettingArrayBinding<Viewport> Viewports { get { return viewports; } }
    }
}
