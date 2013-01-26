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
    /// Describes multi-sampling parameters for a resource.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    public struct Sampling : IEquatable<Sampling>
    {
        /// <summary>
        /// The number of samples per pixel.
        /// </summary>
        public ushort Count;

        /// <summary>
        /// The image quality level. 
        /// The higher the quality, the lower the performance. 
        /// The valid range is between zero and one less than the level returned by <see cref="IAdapter.GetMultisampleQualityLevelsCount"/>.
        /// </summary>
        public ushort Quality;

        /// <summary>
        /// Creates a new <see cref="Sampling"/> structure.
        /// </summary>
        /// <param name="count">The number of samples per pixel.</param>
        /// <param name="quality">The image quality level. 
        /// The higher the quality, the lower the performance. 
        /// The valid range is between zero and one less than the level returned by <see cref="IAdapter.GetMultisampleQualityLevelsCount"/>.</param>
        public Sampling(ushort count, ushort quality)
        {
            Count = count;
            Quality = quality;
        }

        public bool Equals(Sampling other)
        {
            return
                Count == other.Count &&
                Quality == other.Quality;
        }

        public override bool Equals(object obj)
        {
            return obj is Sampling && Equals((Sampling)obj);
        }

        public override int GetHashCode()
        {
            return Count | (Quality << 16);
        }

        public override string ToString()
        {
            return string.Format("{{Count: {0}; Quality: {1}}}", Count.ToString(), Quality.ToString());
        }

        public static bool operator ==(Sampling s1, Sampling s2)
        {
            return
                s1.Count == s2.Count &&
                s1.Quality == s2.Quality;
        }

        public static bool operator !=(Sampling s1, Sampling s2)
        {
            return
                s1.Count != s2.Count ||
                s1.Quality != s2.Quality;
        }

        public static Sampling NoMultisampling
        {
            get { return new Sampling(1, 0); }
        }

        public static Sampling MSAAx2
        {
            get { return new Sampling(2, 0); }
        }

        public static Sampling MSAAx4
        {
            get { return new Sampling(4, 0); }
        }

        public static Sampling MSAAx8
        {
            get { return new Sampling(8, 0); }
        }
    }
}
