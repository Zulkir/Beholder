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
using Beholder.Libraries.SharpDX9.Resources;
using Beholder.Utility.ForImplementations.Core;
using SharpDX.Direct3D9;

namespace Beholder.Libraries.SharpDX9.Core
{
    class CSamplerState : SamplerStateBase<ICDevice>, IDefaultPoolResource, IDisposable
    {
        readonly StateBlock[] stateBlocks;

        public CSamplerState(ICDevice device, ref SamplerDescription desc) : base(device, ref desc)
        {
            stateBlocks = new StateBlock[16];
            if ((int)desc.Filter > 0x55) throw new NotSupportedException(string.Format(
                "'{0}' filter is not supported by the SharpDX9 implementation of Beholder.", desc.Filter));
        }

        public void OnDeviceLost()
        {
            for (int i = 0; i < stateBlocks.Length; i++)
            {
                if (stateBlocks[i] == null) 
                    continue;
                stateBlocks[i].Dispose();
                stateBlocks[i] = null;
            }
        }

        public void OnDeviceReset()
        {

        }

        StateBlock CreateStateBlock(int samplerIndex)
        {
            var d3dDevice = device.D3DDevice;

            d3dDevice.BeginStateBlock();
            d3dDevice.SetSamplerState(samplerIndex, SamplerState.AddressU, CtSharpDX9.TextureAddress(desc.AddressU));
            d3dDevice.SetSamplerState(samplerIndex, SamplerState.AddressV, CtSharpDX9.TextureAddress(desc.AddressV));
            d3dDevice.SetSamplerState(samplerIndex, SamplerState.AddressW, CtSharpDX9.TextureAddress(desc.AddressW));
            d3dDevice.SetSamplerState(samplerIndex, SamplerState.BorderColor, desc.BorderColor.ToArgb());
            switch (desc.Filter)
            {
                case Beholder.Core.Filter.MinMagMipPoint:
                    d3dDevice.SetSamplerState(samplerIndex, SamplerState.MagFilter, TextureFilter.Point);
                    d3dDevice.SetSamplerState(samplerIndex, SamplerState.MinFilter, TextureFilter.Point);
                    d3dDevice.SetSamplerState(samplerIndex, SamplerState.MipFilter, TextureFilter.Point);
                    break;
                case Beholder.Core.Filter.MinMagPointMipLinear:
                    d3dDevice.SetSamplerState(samplerIndex, SamplerState.MagFilter, TextureFilter.Point);
                    d3dDevice.SetSamplerState(samplerIndex, SamplerState.MinFilter, TextureFilter.Point);
                    d3dDevice.SetSamplerState(samplerIndex, SamplerState.MipFilter, TextureFilter.Linear);
                    break;
                case Beholder.Core.Filter.MinPointMagLinearMipPoint:
                    d3dDevice.SetSamplerState(samplerIndex, SamplerState.MagFilter, TextureFilter.Linear);
                    d3dDevice.SetSamplerState(samplerIndex, SamplerState.MinFilter, TextureFilter.Point);
                    d3dDevice.SetSamplerState(samplerIndex, SamplerState.MipFilter, TextureFilter.Point);
                    break;
                case Beholder.Core.Filter.MinPointMagMipLinear:
                    d3dDevice.SetSamplerState(samplerIndex, SamplerState.MagFilter, TextureFilter.Linear);
                    d3dDevice.SetSamplerState(samplerIndex, SamplerState.MinFilter, TextureFilter.Point);
                    d3dDevice.SetSamplerState(samplerIndex, SamplerState.MipFilter, TextureFilter.Linear);
                    break;
                case Beholder.Core.Filter.MinLinearMagMipPoint:
                    d3dDevice.SetSamplerState(samplerIndex, SamplerState.MagFilter, TextureFilter.Point);
                    d3dDevice.SetSamplerState(samplerIndex, SamplerState.MinFilter, TextureFilter.Linear);
                    d3dDevice.SetSamplerState(samplerIndex, SamplerState.MipFilter, TextureFilter.Point);
                    break;
                case Beholder.Core.Filter.MinLinearMagPointMipLinear:
                    d3dDevice.SetSamplerState(samplerIndex, SamplerState.MagFilter, TextureFilter.Point);
                    d3dDevice.SetSamplerState(samplerIndex, SamplerState.MinFilter, TextureFilter.Linear);
                    d3dDevice.SetSamplerState(samplerIndex, SamplerState.MipFilter, TextureFilter.Linear);
                    break;
                case Beholder.Core.Filter.MinMagLinearMipPoint:
                    d3dDevice.SetSamplerState(samplerIndex, SamplerState.MagFilter, TextureFilter.Linear);
                    d3dDevice.SetSamplerState(samplerIndex, SamplerState.MinFilter, TextureFilter.Linear);
                    d3dDevice.SetSamplerState(samplerIndex, SamplerState.MipFilter, TextureFilter.Point);
                    break;
                case Beholder.Core.Filter.MinMagMipLinear:
                    d3dDevice.SetSamplerState(samplerIndex, SamplerState.MagFilter, TextureFilter.Linear);
                    d3dDevice.SetSamplerState(samplerIndex, SamplerState.MinFilter, TextureFilter.Linear);
                    d3dDevice.SetSamplerState(samplerIndex, SamplerState.MipFilter, TextureFilter.Linear);
                    break;
                case Beholder.Core.Filter.Anisotropic:
                    d3dDevice.SetSamplerState(samplerIndex, SamplerState.MagFilter, TextureFilter.Anisotropic);
                    d3dDevice.SetSamplerState(samplerIndex, SamplerState.MinFilter, TextureFilter.Anisotropic);
                    d3dDevice.SetSamplerState(samplerIndex, SamplerState.MipFilter, TextureFilter.Linear);
                    break;
                default: throw new NotSupportedException(string.Format("Filter type '{0}' is not supported by the D3D9 implementation of Beholder", desc.Filter));
            }
            d3dDevice.SetSamplerState(samplerIndex, SamplerState.MipMapLodBias, (int)System.Math.Round(desc.MipLodBias));
            d3dDevice.SetSamplerState(samplerIndex, SamplerState.MaxMipLevel, (int)System.Math.Max(0.0, System.Math.Round(desc.MinimumLod)));
            d3dDevice.SetSamplerState(samplerIndex, SamplerState.MaxAnisotropy, desc.MaximumAnisotropy);
            d3dDevice.SetSamplerState(samplerIndex, SamplerState.SrgbTexture, 0);
            d3dDevice.SetSamplerState(samplerIndex, SamplerState.ElementIndex, 0);
            d3dDevice.SetSamplerState(samplerIndex, SamplerState.DisplacementMapOffset, 0);
            stateBlocks[samplerIndex] = d3dDevice.EndStateBlock();
            return stateBlocks[samplerIndex];
        }

        public void Apply(int samplerIndex)
        {
            var stateBlock = stateBlocks[samplerIndex] ?? CreateStateBlock(samplerIndex);
            stateBlock.Apply();
        }

        public void Dispose()
        {
            for (int i = 0; i < stateBlocks.Length; i++)
            {
                if (stateBlocks[i] == null) 
                    continue;
                stateBlocks[i].Dispose();
                stateBlocks[i] = null;
            }
        }
    }
}
