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
using System.IO;
using Beholder.Shaders;
using Beholder.Utility.ForImplementations;
using SharpDX.Direct3D9;

namespace Beholder.Libraries.SharpDX9.Shaders
{
    class CPixelShader : CShader, IPixelShader, IDisposableInternal
    {
        readonly PixelShader d3dShader;
        readonly PixelShaderProfile profile;

        public override ShaderStage Stage { get { return ShaderStage.Pixel; } }
        public PixelShader D3DShader { get { return d3dShader; } }
        public PixelShaderProfile Profile { get { return profile; } }

        public CPixelShader(ICDevice device, CShaderReflection reflection) 
            : base(device, reflection)
        {
            profile = ParseProfile(reflection.Profile);

            var text = GenerateText();
            try
            {
                var bytecode = ShaderBytecode.Compile(text, "main", ProfileToString(profile), ShaderFlags.PackMatrixColumnMajor | ShaderFlags.OptimizationLevel3);
                d3dShader = new PixelShader(device.D3DDevice, bytecode);
                bytecode.Dispose();
            }
            catch (Exception e)
            {
                throw new ArgumentException(string.Format("Failed to compile a pixel shader '{0}'\r\n--- Code ---\r\n{1}\r\n--- Errors ---\r\n{2}", Name, text, e.Message), e);
            }
        }

        public void DisposeInternal()
        {
            d3dShader.Dispose();
        }

        public static PixelShaderProfile ParseProfile(string profileString)
        {
            switch (profileString)
            {
                case "ps_2_0": return PixelShaderProfile.ps_2_0;
                case "ps_3_0": return PixelShaderProfile.ps_3_0;
                default: throw new InvalidDataException(string.Format("'{0}' is not a valid pixel shader profile.", profileString));
            }
        }

        public static string ProfileToString(PixelShaderProfile profile)
        {
            switch (profile)
            {
                case PixelShaderProfile.ps_2_0: return "ps_2_0";
                case PixelShaderProfile.ps_3_0: return "ps_3_0";
                default: throw new ArgumentOutOfRangeException("profile");
            }
        }
    }
}
