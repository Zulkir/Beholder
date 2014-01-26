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
using System.Globalization;
using System.IO;

namespace Beholder.Shaders.Reflection
{
    public static class ShaderReflectionExtensions
    {
        public static string GetRequiredMeta(this IShaderReflection reflection, string metaName)
        {
            string metaValue;
            if (!reflection.TryGetMeta(metaName, out metaValue))
                throw new InvalidDataException(string.Format("Required meta '{0}' was not found", metaName));
            return metaValue;
        }

        public static int GetRequiredMetaInt(this IShaderReflection reflection, string metaName)
        {
            var resultString = GetRequiredMeta(reflection, metaName);
            int result;
            if (!int.TryParse(resultString, out result))
                throw new InvalidDataException(string.Format("Integer expected, but '{1}' found as meta '{0}'", metaName, resultString));
            return result;
        }

        public static float GetRequiredMetaFloat(this IShaderReflection reflection, string metaName)
        {
            var resultString = GetRequiredMeta(reflection, metaName);
            float result;
            if (!float.TryParse(resultString, NumberStyles.Float, CultureInfo.InvariantCulture, out result))
                throw new InvalidDataException(string.Format("Float expected, but '{1}' found as meta '{0}'", metaName, resultString));
            return result;
        }

        public static int GetMaxVertexCount(this IShaderReflection reflection)
        {
            return GetRequiredMetaInt(reflection, "MaxVertexCount");
        }

        public static GeometryShaderInputPrimitiveType GetGeometryInputPrimitiveType(this IShaderReflection reflection)
        {
            var resultString = GetRequiredMeta(reflection, "InputPrimitiveType");
            switch (resultString)
            {
                case "Point": return GeometryShaderInputPrimitiveType.Point;
                case "Line": return GeometryShaderInputPrimitiveType.Line;
                case "Triangle": return GeometryShaderInputPrimitiveType.Triangle;
                case "LineWithAdjacency": return GeometryShaderInputPrimitiveType.LineWithAdjacency;
                case "TriangleWithAdjacency": return GeometryShaderInputPrimitiveType.TriangleWithAdjacency;
                default: throw new ArgumentException(string.Format("'{0}' is not a valid geometry shader input primitive type", resultString));
            }
        }

        public static GeometryShaderOutputPrimitiveType GetGeometryOutputPrimitiveType(this IShaderReflection reflection)
        {
            var resultString = GetRequiredMeta(reflection, "OutputPrimitiveType");
            switch (resultString)
            {
                case "Point": return GeometryShaderOutputPrimitiveType.Point;
                case "Line": return GeometryShaderOutputPrimitiveType.Line;
                case "Triangle": return GeometryShaderOutputPrimitiveType.Triangle;
                default: throw new ArgumentException(string.Format("'{0}' is not a valid geometry shader output primitive type", resultString));
            }
        }

        public static TesselationDomain GetTesselationDomain(this IShaderReflection reflection)
        {
            var resultString = GetRequiredMeta(reflection, "Domain");
            switch (resultString)
            {
                case "Isoline": return TesselationDomain.Isoline;
                case "Triangle": return TesselationDomain.Triangle;
                case "Quad": return TesselationDomain.Quad;
                default: throw new ArgumentException(string.Format("'{0}' is not a valid tesselation domain", resultString));
            }
        }

        public static TesselationPartitioning GetTesselationPartitioning(this IShaderReflection reflection)
        {
            var resultString = GetRequiredMeta(reflection, "Partitioning");
            switch (resultString)
            {
                case "Integer": return TesselationPartitioning.Integer;
                case "FractionalEven": return TesselationPartitioning.FractionalEven;
                case "FractionalOdd": return TesselationPartitioning.FractionalOdd;
                default: throw new ArgumentException(string.Format("'{0}' is not a valid tesselation partitioning", resultString));
            }
        }

        public static TesselationTopology GetTesselationOutputTopology(this IShaderReflection reflection)
        {
            var resultString = GetRequiredMeta(reflection, "OutputTopology");
            switch (resultString)
            {
                case "Line": return TesselationTopology.Line;
                case "TriangleCW": return TesselationTopology.TriangleCW;
                case "TriangleCCW": return TesselationTopology.TriangleCCW;
                default: throw new ArgumentException(string.Format("'{0}' is not a valid tesselation topology", resultString));
            }
        }

        public static int GetOutputControlPoints(this IShaderReflection reflection)
        {
            return GetRequiredMetaInt(reflection, "OutputControlPoints");
        }

        public static int GetInputControlPoints(this IShaderReflection reflection)
        {
            return GetRequiredMetaInt(reflection, "InputControlPoints");
        }

        public static float GetMaxTesselationFactor(this IShaderReflection reflection)
        {
            return GetRequiredMetaFloat(reflection, "MaxTessFactor");
        }
    }
}
