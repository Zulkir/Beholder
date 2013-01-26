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
using Beholder.Shaders;
using Beholder.Shaders.Reflection;

namespace Beholder.Validation.Shaders
{
    class GeometryShader : Shader<IGeometryShader>, IGeometryShader
    {
        public GeometryShader(IGeometryShader real) : base(real) { }

        public static void ValidateReflection(IShaderReflection reflection)
        {
            reflection.ValidateCommon();

            string metaString;

            int maxVertexCount;
            if (!reflection.TryGetMeta("MaxVertexCount", out metaString))
                throw new ArgumentException("Geometry shader reflection must have a 'MaxVertexCount' element");
            if (!int.TryParse(metaString, out maxVertexCount))
                throw new ArgumentException("MaxVertexCount must be an integer");
            // todo: MVC range

            if (!reflection.TryGetMeta("InputPrimitiveType", out metaString))
                throw new ArgumentException("Geometry shader reflection must have an 'InputPrimitiveType' meta element");
            reflection.GetGeometryInputPrimitiveType();

            if (!reflection.TryGetMeta("OutputPrimitiveType", out metaString))
                throw new ArgumentException("Geometry shader reflection must have an 'OutputPrimitiveType' meta element");
            reflection.GetGeometryOutputPrimitiveType();

            bool hasOutput = reflection.Output.Count > 0;
            bool hasOutputStreams = reflection.OutputStreams.Count > 0;
            if (!hasOutput && !hasOutputStreams)
                throw new ArgumentException("Geometry shader reflection must either have Output or OutputStreams");
            if (hasOutput && hasOutputStreams)
                throw new ArgumentException("Geometry shader reflection cannot have both Output and OutputStreams at the same time");

            Check.NotNullNotEmpty(reflection.Input, "reflection.Input");
            Check.NotNullNotEmpty(reflection.CodeMainLines, "reflection.CodeMainLines");

            Check.NullOrEmpty(reflection.InputPatch, "reflection.InputPatch");
            Check.NullOrEmpty(reflection.OutputPatch, "reflection.OutputPatch");
            Check.NullOrEmpty(reflection.CodePatchLines, "reflection.CodePatchLines");
        }
    }
}
