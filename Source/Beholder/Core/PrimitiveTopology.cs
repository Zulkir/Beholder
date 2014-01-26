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

namespace Beholder.Core
{
    /// <summary>
    /// How the pipeline interprets vertex data that is bound to the input-assembler stage. 
    /// These primitive topology values determine how the vertex data is rendered on screen.
    /// </summary>
    public enum PrimitiveTopology
    {
        /// <summary>
        /// The IA stage has not been initialized with a primitive topology. 
        /// The IA stage will not function properly unless a primitive topology is defined.
        /// </summary>
        Undefined = 0,

        /// <summary>
        /// Interpret the vertex data as a list of points.
        /// </summary>
        PointList = 1,

        /// <summary>
        /// Interpret the vertex data as a list of lines.
        /// </summary>
        LineList = 2,

        /// <summary>
        /// Interpret the vertex data as a line strip.
        /// </summary>
        LineStrip = 3,

        /// <summary>
        /// Interpret the vertex data as a list of triangles.
        /// </summary>
        TriangleList = 4,

        /// <summary>
        /// Interpret the vertex data as a triangle strip.
        /// </summary>
        TriangleStrip = 5,

        /// <summary>
        /// Interpret the vertex data as list of lines with adjacency data.
        /// </summary>
        LineListWithAdjacency = 10,

        /// <summary>
        /// Interpret the vertex data as line strip with adjacency data.
        /// </summary>
        LineStripWithAdjacency = 11,

        /// <summary>
        /// Interpret the vertex data as list of triangles with adjacency data.
        /// </summary>
        TriangleListWithAdjacency = 12,

        /// <summary>
        /// Interpret the vertex data as triangle strip with adjacency data.
        /// </summary>
        TriangleStripWithAdjacency = 13,

        /// <summary>
        /// Interpret the vertex data as a patch list with 1 control point.
        /// </summary>
        PatchList1 = 33,

        /// <summary>
        /// Interpret the vertex data as a patch list with 2 control points.
        /// </summary>
        PatchList2 = 34,

        /// <summary>
        /// Interpret the vertex data as a patch list with 3 control points.
        /// </summary>
        PatchList3 = 35,

        /// <summary>
        /// Interpret the vertex data as a patch list with 4 control points.
        /// </summary>
        PatchList4 = 36,

        /// <summary>
        /// Interpret the vertex data as a patch list with 5 control points.
        /// </summary>
        PatchList5 = 37,

        /// <summary>
        /// Interpret the vertex data as a patch list with 6 control points.
        /// </summary>
        PatchList6 = 38,

        /// <summary>
        /// Interpret the vertex data as a patch list with 7 control points.
        /// </summary>
        PatchList7 = 39,

        /// <summary>
        /// Interpret the vertex data as a patch list with 8 control points.
        /// </summary>
        PatchList8 = 40,

        /// <summary>
        /// Interpret the vertex data as a patch list with 9 control points.
        /// </summary>
        PatchList9 = 41,

        /// <summary>
        /// Interpret the vertex data as a patch list with 10 control points.
        /// </summary>
        PatchList10 = 42,

        /// <summary>
        /// Interpret the vertex data as a patch list with 11 control points.
        /// </summary>
        PatchList11 = 43,

        /// <summary>
        /// Interpret the vertex data as a patch list with 12 control points.
        /// </summary>
        PatchList12 = 44,

        /// <summary>
        /// Interpret the vertex data as a patch list with 13 control points.
        /// </summary>
        PatchList13 = 45,

        /// <summary>
        /// Interpret the vertex data as a patch list with 14 control points.
        /// </summary>
        PatchList14 = 46,

        /// <summary>
        /// Interpret the vertex data as a patch list with 15 control points.
        /// </summary>
        PatchList15 = 47,

        /// <summary>
        /// Interpret the vertex data as a patch list with 16 control points.
        /// </summary>
        PatchList16 = 48,

        /// <summary>
        /// Interpret the vertex data as a patch list with 17 control points.
        /// </summary>
        PatchList17 = 49,

        /// <summary>
        /// Interpret the vertex data as a patch list with 18 control points.
        /// </summary>
        PatchList18 = 50,

        /// <summary>
        /// Interpret the vertex data as a patch list with 19 control points.
        /// </summary>
        PatchList19 = 51,

        /// <summary>
        /// Interpret the vertex data as a patch list with 20 control points.
        /// </summary>
        PatchList20 = 52,

        /// <summary>
        /// Interpret the vertex data as a patch list with 21 control points.
        /// </summary>
        PatchList21 = 53,

        /// <summary>
        /// Interpret the vertex data as a patch list with 22 control points.
        /// </summary>
        PatchList22 = 54,

        /// <summary>
        /// Interpret the vertex data as a patch list with 23 control points.
        /// </summary>
        PatchList23 = 55,

        /// <summary>
        /// Interpret the vertex data as a patch list with 24 control points.
        /// </summary>
        PatchList24 = 56,

        /// <summary>
        /// Interpret the vertex data as a patch list with 25 control points.
        /// </summary>
        PatchList25 = 57,

        /// <summary>
        /// Interpret the vertex data as a patch list with 26 control points.
        /// </summary>
        PatchList26 = 58,

        /// <summary>
        /// Interpret the vertex data as a patch list with 27 control points.
        /// </summary>
        PatchList27 = 59,

        /// <summary>
        /// Interpret the vertex data as a patch list with 28 control points.
        /// </summary>
        PatchList28 = 60,

        /// <summary>
        /// Interpret the vertex data as a patch list with 29 control points.
        /// </summary>
        PatchList29 = 61,

        /// <summary>
        /// Interpret the vertex data as a patch list with 30 control points.
        /// </summary>
        PatchList30 = 62,

        /// <summary>
        /// Interpret the vertex data as a patch list with 31 control points.
        /// </summary>
        PatchList31 = 63,

        /// <summary>
        /// Interpret the vertex data as a patch list with 32 control points.
        /// </summary>
        PatchList32 = 64,
    }
}
