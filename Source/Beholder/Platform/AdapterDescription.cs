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
using System.Runtime.InteropServices;

namespace Beholder.Platform
{
    /// <summary>
    /// Describes an adapter (or video card).
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct AdapterDescription
    {
        /// <summary>
        /// A string that contains the adapter description.
        /// </summary>
        public string Description;

        /// <summary>
        /// The PCI ID of the hardware vendor.
        /// </summary>
        public int VendorId;

        /// <summary>
        /// The PCI ID of the hardware device.
        /// </summary>
        public int DeviceId;

        /// <summary>
        /// The PCI ID of the sub system.
        /// </summary>
        public int SubSysId;

        /// <summary>
        /// The PCI ID of the revision number of the adapter.
        /// </summary>
        public int Revision;

        /// <summary>
        /// The number of bytes of dedicated video memory that are not shared with the CPU.
        /// Available only on DirectX 10 and higher.
        /// </summary>
        public long DedicatedVideoMemory;

        /// <summary>
        /// The number of bytes of dedicated system memory that are not shared with the GPU. This memory is allocated from available system memory at boot time.
        /// Available only on DirectX 10 and higher.
        /// </summary>
        public long DedicatedSystemMemory;

        /// <summary>
        /// The number of bytes of shared system memory. 
        /// This is the maximum value of system memory that may be consumed by the adapter during operation. 
        /// Any incidental memory consumed by the driver as it manages and uses video memory is additional.
        /// Available only on DirectX 10 and higher.
        /// </summary>
        public long SharedSystemMemory;

        /// <summary>
        /// High bits of the LUID - a unique value that identifies the adapter.
        /// Available only on DirectX 10 and higher.
        /// </summary>
        public int AdapterLuidHigh;

        /// <summary>
        /// Low bits of the LUID - a unique value that identifies the adapter.
        /// Available only on DirectX 10 and higher.
        /// </summary>
        public int AdapterLuidLow;

        /// <summary>
        /// A member of the <see cref="AdapterFlags"/> enumerated type that describes the adapter type. 
        /// The <see cref="AdapterFlags.Remote"/> flag specifies that the adapter is a remote adapter.
        /// Available only on DirectX 10 and higher.
        /// </summary>
        public AdapterFlags Flags;
    }
}
