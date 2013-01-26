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
using Beholder.Core;
using Beholder.Utility.ForImplementations.Core;
using ObjectGL.GL42;
using OpenTK.Graphics.OpenGL;

namespace Beholder.Libraries.ObjectGL4.Core
{
    class CSamplerState : SamplerStateBase<ICDevice>
    {
        Sampler glSampler;
        Sampler glSamplerMipless;

        public CSamplerState(ICDevice device, ref SamplerDescription desc) : base(device, ref desc)
        {
            
        }

        public Sampler GLSampler { get { return glSampler ?? (glSampler = CreateGLSampler(false)); } }
        public Sampler GLSamplerMipless { get { return glSamplerMipless ?? (glSamplerMipless = CreateGLSampler(true)); } }

        unsafe Sampler CreateGLSampler(bool mipless)
        {
            var result = new Sampler();

            switch ((Filter)((int)desc.Filter & 0x33))
            {
                case Filter.MinMagMipPoint:
                    result.SetMinFilter(mipless ? TextureMinFilter.Nearest : TextureMinFilter.NearestMipmapNearest);
                    break;
                case Filter.MinMagPointMipLinear:
                    result.SetMinFilter(mipless ? TextureMinFilter.Nearest : TextureMinFilter.NearestMipmapLinear);
                    break;
                case Filter.MinLinearMagMipPoint:
                    result.SetMinFilter(mipless ? TextureMinFilter.Linear : TextureMinFilter.LinearMipmapNearest);
                    break;
                case Filter.MinLinearMagPointMipLinear:
                    result.SetMinFilter(mipless ? TextureMinFilter.Linear : TextureMinFilter.LinearMipmapLinear);
                    break;
                default: throw new ArgumentOutOfRangeException("desc.Filter");
            }

            switch ((Filter)((int)desc.Filter & 0x0c))
            {
                case Filter.MinMagMipPoint:
                    result.SetMagFilter(TextureMagFilter.Nearest);
                    break;
                case Filter.MinPointMagLinearMipPoint:
                    result.SetMagFilter(TextureMagFilter.Linear);
                    break;
                default: throw new ArgumentOutOfRangeException("desc.Filter");
            }

            if (((int)desc.Filter & 0x55) == 0x55)
            {
                result.SetMaxAnisotropy(desc.MaximumAnisotropy);
            }

            if (((int)desc.Filter & 0x80) != 0)
            {
                result.SetCompareMode(TextureCompareMode.CompareRefToTexture);
                result.SetCompareFunc(CtObjectGL.CompareFunc(desc.ComparisonFunction));
            }

            result.SetWrapS(CtObjectGL.TextureWrapMode(desc.AddressU));
            result.SetWrapT(CtObjectGL.TextureWrapMode(desc.AddressV));
            result.SetWrapR(CtObjectGL.TextureWrapMode(desc.AddressW));

            if (desc.MipLodBias != 0.0f)
                throw new NotSupportedException("MipLodBias is not supported by the OGL implementation of Beholder");

            var borderColor = desc.BorderColor;
            result.SetBorderColor((float*)&borderColor);
            result.SetMaxLod(desc.MaximumLod);
            result.SetMinLod(desc.MinimumLod);

            return result;
        }
    }
}
