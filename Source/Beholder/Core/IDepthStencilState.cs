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

namespace Beholder.Core
{
    /// <summary>
    /// Depth-stencil state which sets up the depth-stencil test for the output-merger stage.
    /// </summary>
    public interface IDepthStencilState : IDeviceChild
    {
        /// <summary>
        /// Identifies how to use the results of the depth test and the stencil test for pixels whose surface normal is facing towards the camera.
        /// </summary>
        DepthStencilOperationDescription FrontFaceStencilOp { get; }

        /// <summary>
        /// Identifies how to use the results of the depth test and the stencil test for pixels whose surface normal is facing away from the camera.
        /// </summary>
        DepthStencilOperationDescription BackFaceStencilOp { get; }

        /// <summary>
        /// Enable depth testing.
        /// </summary>
        bool DepthEnable { get; }

        /// <summary>
        /// Enable writing to the depth buffer.
        /// </summary>
        bool DepthWriteEnable { get; }

        /// <summary>
        /// A function that compares depth data against existing depth data.
        /// </summary>
        Comparison DepthFunction { get; }

        /// <summary>
        /// Enable stencil testing.
        /// </summary>
        bool StencilEnable { get; }

        /// <summary>
        /// Identifies a portion of the depth-stencil buffer for reading stencil data.
        /// </summary>
        byte StencilReadMask { get; }

        /// <summary>
        /// Identifies a portion of the depth-stencil buffer for writing stencil data.
        /// </summary>
        byte StencilWriteMask { get; }

        /// <summary>
        /// Gets a description of the depth-stencil state.
        /// </summary>
        /// <param name="description">Variable to fill with the description data.</param>
        void GetDescription(out DepthStencilDescription description);
    }
}
