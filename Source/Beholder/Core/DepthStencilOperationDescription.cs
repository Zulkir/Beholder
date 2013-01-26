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

namespace Beholder.Core
{
    /// <summary>
    /// Stencil operations that can be performed based on the results of stencil test.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct DepthStencilOperationDescription : IEquatable<DepthStencilOperationDescription>
    {
        /// <summary>
        /// The stencil operation to perform when stencil testing fails.
        /// </summary>
        public StencilOperation StencilFailOperation;

        /// <summary>
        /// The stencil operation to perform when stencil testing passes and depth testing fails.
        /// </summary>
        public StencilOperation StencilDepthFailOperation;

        /// <summary>
        /// The stencil operation to perform when stencil testing and depth testing both pass.
        /// </summary>
        public StencilOperation StencilPassOperation;

        /// <summary>
        /// A function that compares stencil data against existing stencil data.
        /// </summary>
        public Comparison StencilFunction;

        public bool Equals(DepthStencilOperationDescription other)
        {
            return
                StencilFailOperation == other.StencilFailOperation &&
                StencilDepthFailOperation == other.StencilDepthFailOperation &&
                StencilPassOperation == other.StencilPassOperation &&
                StencilFunction == other.StencilFunction;
        }

        public override bool Equals(object obj)
        {
            return obj is DepthStencilOperationDescription && Equals((DepthStencilOperationDescription)obj);
        }

        public override int GetHashCode()
        {
            return (int)StencilFailOperation | ((int)StencilDepthFailOperation << 4) | ((int)StencilPassOperation << 8) | ((int)StencilFunction << 12);
        }

        public override string ToString()
        {
            return string.Format("Fail:{0}; DepthFail:{1}, Pass:{2}; Func:{3}", 
                StencilFailOperation.ToString(), StencilDepthFailOperation.ToString(), StencilPassOperation.ToString(), StencilFunction.ToString());
        }

        public static bool operator ==(DepthStencilOperationDescription o1, DepthStencilOperationDescription o2)
        {
            return
                o1.StencilFailOperation == o2.StencilFailOperation &&
                o1.StencilDepthFailOperation == o2.StencilDepthFailOperation &&
                o1.StencilPassOperation == o2.StencilPassOperation &&
                o1.StencilFunction == o2.StencilFunction;
        }

        public static bool operator !=(DepthStencilOperationDescription o1, DepthStencilOperationDescription o2)
        {
            return
                o1.StencilFailOperation != o2.StencilFailOperation ||
                o1.StencilDepthFailOperation != o2.StencilDepthFailOperation ||
                o1.StencilPassOperation != o2.StencilPassOperation ||
                o1.StencilFunction != o2.StencilFunction;
        }

        public static DepthStencilOperationDescription Default
        {
            get
            {
                return new DepthStencilOperationDescription
                {
                    StencilFunction = Comparison.Never,
                    StencilDepthFailOperation = StencilOperation.Keep,
                    StencilPassOperation = StencilOperation.Keep,
                    StencilFailOperation = StencilOperation.Keep
                };
            }
        }
    }
}
