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

using Beholder.Shaders;
using Beholder.Shaders.Reflection;

namespace Beholder.Validation.Shaders
{
    class PixelShader : Shader<IPixelShader>, IPixelShader
    {
        public PixelShader(IPixelShader real) : base(real) { }

        public static void ValidateReflection(IShaderReflection reflection)
        {
            reflection.ValidateCommon();

            Check.NotNullNotEmpty(reflection.Input, "reflection.Input");
            Check.NotNullNotEmpty(reflection.Output, "reflection.Output");
            Check.NotNullNotEmpty(reflection.CodeMainLines, "reflection.CodeMainLines");

            Check.NullOrEmpty(reflection.InputExtra, "reflection.InputExtra");
            Check.NullOrEmpty(reflection.InputPatch, "reflection.InputPatch");
            Check.NullOrEmpty(reflection.InputPatchExtra, "reflection.InputPatchExtra");
            Check.NullOrEmpty(reflection.OutputPatch, "reflection.OutputPatch");
            Check.NullOrEmpty(reflection.OutputStreams, "reflection.OutputStreams");
            Check.NullOrEmpty(reflection.CodePatchLines, "reflection.CodePatchLines");
        }
    }
}
