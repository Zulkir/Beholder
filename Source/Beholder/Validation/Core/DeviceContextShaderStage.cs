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

using Beholder.Core;
using Beholder.Resources;

namespace Beholder.Validation.Core
{
    class DeviceContextShaderStage<T> : Wrapper<T>, IDeviceContextShaderStage
        where T : class, IDeviceContextShaderStage
    {
        readonly DeviceContextShaderStageUniformBuffers uniformBuffers;
        readonly DeviceContextShaderStageShaderResources shaderResources;
        readonly DeviceContextShaderStageSamplers samplers;

        protected DeviceContextShaderStage(T real)
            : base(real)
        {
            uniformBuffers = new DeviceContextShaderStageUniformBuffers(Real.UniformBuffers);
            shaderResources = new DeviceContextShaderStageShaderResources(Real.ShaderResources);
            samplers = new DeviceContextShaderStageSamplers(Real.Samplers);
        }

        public IDeviceContextCumulativeArrayBinding<IBuffer> UniformBuffers
        {
            get { return uniformBuffers; }
        }

        public IDeviceContextCumulativeArrayBinding<IShaderResourceView> ShaderResources
        {
            get { return shaderResources; }
        }

        public IDeviceContextCumulativeArrayBinding<ISamplerState> Samplers
        {
            get { return samplers; }
        }
    }

    class DeviceContextShaderStage : DeviceContextShaderStage<IDeviceContextShaderStage>
    {
        public DeviceContextShaderStage(IDeviceContextShaderStage real) : base(real) {}
    }
}
