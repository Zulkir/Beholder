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
using Beholder.Shaders.Reflection;

namespace Beholder.Libraries.ObjectGL4.Shaders
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    struct TesselationLayout : IEquatable<TesselationLayout>
    {
        readonly int code;

        public TesselationLayout(TesselationDomain domain, TesselationPartitioning partitioning, TesselationTopology topology)
        {
            code = (int)domain | ((int)partitioning << 8) | ((int)topology << 16);
        }

        public void Decode(out TesselationDomain domain, out TesselationPartitioning partitioning, out TesselationTopology topology)
        {
            domain = (TesselationDomain)(code & 0xff);
            partitioning = (TesselationPartitioning)((code >> 8) & 0xff);
            topology = (TesselationTopology)((code >> 16) & 0xff);
        }

        public static bool Equal(TesselationLayout tl1, TesselationLayout tl2)
        {
            return tl1.code == tl2.code;
        }

        public bool Equals(TesselationLayout other)
        {
            return code == other.code;
        }

        public override bool Equals(object obj)
        {
            return obj is TesselationLayout && Equals((TesselationLayout)obj);
        }

        public override int GetHashCode()
        {
            return code;
        }

        public override string ToString()
        {
            TesselationDomain domain;
            TesselationPartitioning partitioning;
            TesselationTopology topology;
            Decode(out domain, out partitioning, out topology);
            return string.Format("{0}, {1}, {2}", domain, partitioning, topology);
        }
    }
}
