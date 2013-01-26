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
using System.Linq;
using Beholder.Shaders.Reflection;

namespace Beholder.Validation.Shaders
{
    static class ShaderReflectionExtensions
    {
        public static void ValidateCommon(this IShaderReflection reflection)
        {
            string metaValue;
            if (!reflection.TryGetMeta("Name", out metaValue))
                throw new ArgumentException("Shader reflection must contain a 'Name' meta element");
            if (reflection.FixedSamplingPairs != null && reflection.FixedSamplingPairs.Any())
            {
                if (reflection.Srvs == null || !reflection.Srvs.Any())
                    throw new ArgumentException("reflection.FixedSampling is present, but reflection.Srvs are not");
                if (reflection.Samplers == null || !reflection.Samplers.Any())
                    throw new ArgumentException("reflection.FixedSampling is present, but reflection.Samplers are not");
                foreach (var fixedSamplingPair in reflection.FixedSamplingPairs)
                {
                    if (!reflection.Srvs.Any(v => v.Name == fixedSamplingPair.SrvName))
                        throw new ArgumentException(string.Format("SRV '{0}' specified in reflection.FixedSampling is missing from the reflection.Srvs", fixedSamplingPair.SrvName));
                    if (!reflection.Samplers.Any(v => v.Name == fixedSamplingPair.SamplerName))
                        throw new ArgumentException(string.Format("Sampler '{0}' specified in reflection.FixedSampling is missing from the reflection.Samplers", fixedSamplingPair.SamplerName));
                }
            }
        }
    }
}
