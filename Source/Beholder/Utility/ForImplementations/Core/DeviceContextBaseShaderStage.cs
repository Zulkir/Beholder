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

using Beholder.Core;
using Beholder.Resources;

namespace Beholder.Utility.ForImplementations.Core
{
    public class DeviceContextBaseShaderStage : IDeviceContextShaderStage
    {
        readonly DeviceContextBaseCumulativeArrayBinding<IBuffer> uniformBuffers;
        readonly DeviceContextBaseCumulativeArrayBinding<ISamplerState> samplers;
        readonly DeviceContextBaseCumulativeArrayBinding<IShaderResourceView> shaderResources;

        public DeviceContextBaseShaderStage(int uniformBuffersCount, int samplersCount, int shaderResourcesCount)
        {
            uniformBuffers = new DeviceContextBaseCumulativeArrayBinding<IBuffer>(uniformBuffersCount, ReferenceEquals);
            samplers = new DeviceContextBaseCumulativeArrayBinding<ISamplerState>(samplersCount, ReferenceEquals);
            shaderResources = new DeviceContextBaseCumulativeArrayBinding<IShaderResourceView>(shaderResourcesCount, ReferenceEquals);
        }

        public DeviceContextBaseCumulativeArrayBinding<IBuffer> UniformBuffers { get { return uniformBuffers; } }
        public DeviceContextBaseCumulativeArrayBinding<ISamplerState> Samplers { get { return samplers; } }
        public DeviceContextBaseCumulativeArrayBinding<IShaderResourceView> ShaderResources { get { return shaderResources; } }

        IDeviceContextCumulativeArrayBinding<IBuffer> IDeviceContextShaderStage.UniformBuffers { get { return uniformBuffers; } }
        IDeviceContextCumulativeArrayBinding<ISamplerState> IDeviceContextShaderStage.Samplers { get { return samplers; } }
        IDeviceContextCumulativeArrayBinding<IShaderResourceView> IDeviceContextShaderStage.ShaderResources { get { return shaderResources; } }

        public void MarkAsDirty()
        {
            uniformBuffers.MarkAsDirty();
            samplers.MarkAsDirty();
            shaderResources.MarkAsDirty();
        }
    }
}
