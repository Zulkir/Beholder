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
using Beholder.Platform;
using SharpDX.DXGI;
using OutputDescription = Beholder.Platform.OutputDescription;

namespace Beholder.Libraries.SharpDX11.Platform
{
    public class COutput : IOutput
    {
        readonly Output dxgiOutput;
        readonly OutputDescription desc;

        public void GetDescription(out OutputDescription description) { description = desc; }

        internal COutput(Output dxgiOutput)
        {
            this.dxgiOutput = dxgiOutput;

            var dxgiDesc = dxgiOutput.Description;
            var bounds = dxgiDesc.DesktopBounds;

            desc = new OutputDescription
            {
                DeviceName = dxgiDesc.DeviceName,
                DesctopCoordinates = new Math.Rectangle(bounds.Left, bounds.Top, bounds.Width, bounds.Height),
                AttachedToDesctop = dxgiDesc.IsAttachedToDesktop,
                Rotation = CtBeholder.ModeRotation(dxgiDesc.Rotation),
                MonitorHandle = dxgiDesc.MonitorHandle
            };
        }

        public IEnumerable<DisplayMode> GetSupportedDisplayModes()
        {
            var allFormats = (Format[]) Enum.GetValues(typeof (Format));
            return from format in allFormats
                   let modes = dxgiOutput.GetDisplayModeList(format, 0)
                   where modes != null
                   from mode in modes
                   select new DisplayMode
                    {
                        Width = mode.Width,
                        Height = mode.Height,
                        RefreshRate = CtBeholder.Rational(mode.RefreshRate),
                        FormatID = (int) format
                    };
        }
    }
}
