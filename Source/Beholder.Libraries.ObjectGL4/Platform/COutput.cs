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
using System.Globalization;
using System.Linq;
using Beholder.Math;
using Beholder.Platform;
using OpenTK;

namespace Beholder.Libraries.ObjectGL4.Platform
{
    public class COutput : IOutput
    {
        readonly DisplayDevice glDisplayDevice;
        readonly OutputDescription outputDesc;

        public DisplayDevice GLDisplayDevice { get { return glDisplayDevice; } }
        public void GetDescription(out OutputDescription description) { description = outputDesc; }

        public COutput(DisplayDevice glDisplayDevice, int index)
        {
            this.glDisplayDevice = glDisplayDevice;

            outputDesc = new OutputDescription
            {
                DeviceName = string.Format("Display{0}", index.ToString(CultureInfo.InvariantCulture)),
                DesctopCoordinates = new Rectangle(),
                AttachedToDesctop = false,
                Rotation = ModeRotation.Unspecified,
                MonitorHandle = glDisplayDevice
            };
        }

        public IEnumerable<DisplayMode> GetSupportedDisplayModes()
        {
            return glDisplayDevice.AvailableResolutions.Select(r => new DisplayMode
            {
                Width = r.Width,
                Height = r.Height,
                FormatID = (int)CtObjectGL.ColorAlphaFormat(r.BitsPerPixel),
                RefreshRate = new Rational((int)(1000f * r.RefreshRate), 1000)
            });
        }
    }
}
