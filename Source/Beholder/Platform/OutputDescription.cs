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
using System.Runtime.InteropServices;
using Beholder.Math;

namespace Beholder.Platform
{
    /// <summary>
    /// Describes an output or physical connection between the adapter (video card) and a device.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct OutputDescription
    {
        /// <summary>
        /// A string that contains the name of the output device.
        /// </summary>
        public string DeviceName;

        /// <summary>
        /// A <see cref="Rectangle"/> structure containing the bounds of the output in desktop coordinates.
        /// Available only on DirectX 10 and higher.
        /// </summary>
        public Rectangle DesctopCoordinates;

        /// <summary>
        /// True if the output is attached to the desktop; otherwise, false.
        /// Available only on DirectX 10 and higher.
        /// </summary>
        public bool AttachedToDesctop;

        /// <summary>
        /// A member of the <see cref="ModeRotation"/> enumerated type describing on how an image is rotated by the output.
        /// Available only on DirectX 10 and higher.
        /// </summary>
        public ModeRotation Rotation;

        /// <summary>
        /// An handle that represents the display monitor.
        /// </summary>
        public object MonitorHandle;
    }
}
