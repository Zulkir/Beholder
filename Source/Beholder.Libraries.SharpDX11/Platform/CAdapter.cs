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
using System.Collections.Generic;
using System.Linq;
using Beholder.Core;
using Beholder.Platform;
using Beholder.Utility.Collections.Readonly;
using Beholder.Utility.ForImplementations.Platform;
using SharpDX.DXGI;
using SharpDX.Direct3D11;
using AdapterDescription = Beholder.Platform.AdapterDescription;
using Device = SharpDX.Direct3D11.Device;
using FeatureLevel = SharpDX.Direct3D.FeatureLevel;
using FormatSupport = SharpDX.Direct3D11.FormatSupport;

namespace Beholder.Libraries.SharpDX11.Platform
{
    public class CAdapter : IAdapter
    {
        const FormatSupport DisplaySupportMask = FormatSupport.RenderTarget | FormatSupport.Display | FormatSupport.MultisampleRendertarget;
        const FormatSupport DepthStencilSupportMask = FormatSupport.DepthStencil;
        const FormatSupport VertexElementSupportMask = FormatSupport.InputAssemblyVertexBuffer;

        static readonly int[] MultisampleCountsToTry = { 2, 4, 8, 16, 32, 64 };

        readonly Adapter1 dxgiAdapter;
        readonly AdapterDescription desc;
        readonly int index;
        readonly ApiVersion apiVersion;
        readonly AdapterRestrictions restrictions;
        readonly COutput[] outputs;
        readonly ReadonlyArrayWrapper<COutput> roOutputs;

        
        readonly FormatSupport[] formatsSupport;
        readonly IFormatInfo[] formatInfos;
        readonly ExplicitFormat[] vertexElementFormats;

        readonly int[,] multisampleQualityLevels;

        public Adapter1 DXGIAdapter { get { return dxgiAdapter; } }
        public int Index { get { return index; } }
        public Api Api { get { return Api.Direct3D; } }
        public ApiVersion ApiVersion { get { return apiVersion; } }
        public IAdapterRestrictions Restrictions { get { return restrictions; } }
        public IReadonlyList<IOutput> Outputs { get { return roOutputs; } }
        public void GetDescription(out AdapterDescription bAdapterDesc) { bAdapterDesc = desc; }

        public CAdapter(Adapter1 dxgiAdapter, int index)
        {
            this.dxgiAdapter = dxgiAdapter;
            this.index = index;

            AdapterDescription1 dxgiDesc = dxgiAdapter.Description1;
            desc = new AdapterDescription
            {
                Description = dxgiDesc.Description,
                VendorId = dxgiDesc.VendorId,
                DeviceId = dxgiDesc.DeviceId,
                SubSysId = dxgiDesc.SubsystemId,
                Revision = dxgiDesc.Revision,
                DedicatedVideoMemory = dxgiDesc.DedicatedVideoMemory,
                DedicatedSystemMemory = dxgiDesc.DedicatedSystemMemory,
                SharedSystemMemory = dxgiDesc.SharedSystemMemory,
                AdapterLuidHigh = (int)(dxgiDesc.Luid >> 32),
                AdapterLuidLow = (int)(dxgiDesc.Luid & 0x00000000FFFFFFFF),
                Flags = CtBeholder.AdapterFlags(dxgiDesc.Flags)
            };

            outputs = dxgiAdapter.Outputs.Select(o => new COutput(o)).ToArray();
            roOutputs = new ReadonlyArrayWrapper<COutput>(outputs);

            using (var d3dDevice = new Device(dxgiAdapter))
            {
                switch (d3dDevice.FeatureLevel)
                {
                    case FeatureLevel.Level_9_1: apiVersion = new ApiVersion(9, 1); break;
                    case FeatureLevel.Level_9_3: apiVersion = new ApiVersion(9, 3); break;
                    case FeatureLevel.Level_9_2: apiVersion = new ApiVersion(9, 2); break;
                    case FeatureLevel.Level_10_0: apiVersion = new ApiVersion(10, 0); break;
                    case FeatureLevel.Level_10_1: apiVersion = new ApiVersion(10, 1); break;
                    case FeatureLevel.Level_11_0: apiVersion = new ApiVersion(11, 0); break;
                    default: throw new ArgumentOutOfRangeException("d3dDevice.FeatureLevel");
                }

                restrictions = new AdapterRestrictions
                {
                    UniformBufferSlots = CommonShaderStage.ConstantBufferApiSlotCount,
                    SamplerSlots = CommonShaderStage.SamplerSlotCount,
                    ShaderResourceSlots = CommonShaderStage.InputResourceSlotCount,
                    UnorderedAccessResourceSlots = ComputeShaderStage.UnorderedAccessViewSlotCount,
                    MaxVertexStreams = InputAssemblerStage.VertexInputResourceSlotCount,
                    MaxVertexStreamElementCount = InputAssemblerStage.VertexInputStructureElementCount,
                    MaxStreamOutputTargets = 4,
                    MaxViewports = 16,
                    MaxRenderTargets = 8,
                    MaxThreadGroupsX = d3dDevice.FeatureLevel == FeatureLevel.Level_11_0 ? ComputeShaderStage.ThreadGroupMaximumX : 768,
                    MaxThreadGroupsY = d3dDevice.FeatureLevel == FeatureLevel.Level_11_0 ? ComputeShaderStage.ThreadGroupMaximumY : 768,
                    MaxThreadGroupsZ = d3dDevice.FeatureLevel == FeatureLevel.Level_11_0 ? ComputeShaderStage.ThreadGroupMaximumZ : 1,
                    MaxThreadGroupsTotal = d3dDevice.FeatureLevel == FeatureLevel.Level_11_0 ? ComputeShaderStage.ThreadGroupMaximumThreadsPerGroup : 768
                };

                var allFormats = (Format[])Enum.GetValues(typeof(Format));

                formatsSupport = allFormats.Select(d3dDevice.CheckFormatSupport).ToArray();
                formatInfos = allFormats.Where(f => f != 0).Select(CtBeholder.FormatInfo).ToArray();
                //depthStencilFormatInfos = allFormats.Where(f => (formatsSupport[(int)f] & DepthStencilSupportMask) != 0).Select(CtBeholder.DepthStencilFormatInfo).ToArray();
                vertexElementFormats = allFormats.Where(f => (formatsSupport[(int)f] & VertexElementSupportMask) != 0).Select(f => (ExplicitFormat)f).ToArray();

                multisampleQualityLevels = new int[allFormats.Length, MultisampleCountsToTry.Length];

                for (int i = 0; i < allFormats.Length; i++)
                    for (int j = 0; j < MultisampleCountsToTry.Length; j++)
                        multisampleQualityLevels[i, j] = d3dDevice.CheckMultisampleQualityLevels((Format)i, MultisampleCountsToTry[j]);
            }
            /*
            using (var writer = new System.IO.StreamWriter("Formats.txt"))
            {
                for (int i = 0; i < formatsSupport.Length; i++)
                {
                    writer.WriteLine(string.Format("{0, -30}:{1}", (Format)i, formatsSupport[i].ToString().Replace(", ", " | ")));
                }
            }*/
        }

        public IFormatInfo GetFormatInfo(int formatID)
        {
            return formatInfos.Single(fi => fi.ID == formatID);
        }

        public IEnumerable<IFormatInfo> GetSupportedWindowedDisplayFormats()
        {
            return formatInfos.Where(f => (formatsSupport[f.ID] & DisplaySupportMask) == DisplaySupportMask);
        }

        public IEnumerable<IFormatInfo> GetSupportedFormats(Beholder.Platform.FormatSupport requiredSupport)
        {
            var d3dSupport = CtSharpDX11.FormatSupport(requiredSupport);
            return formatInfos.Where(f => (formatsSupport[f.ID] & d3dSupport) == d3dSupport);
        }

        public bool CheckViewFormatCompatability(int textureFormatID, int viewFormatID)
        {
            Format typelessFormat;
            return textureFormatID == viewFormatID || 
                (CtSharpDX11.TryGetTypelessFormat((Format)viewFormatID, out typelessFormat) && 
                textureFormatID == (int)typelessFormat);
        }

        public IEnumerable<ExplicitFormat> GetSupportedVertexElementFormats()
        {
            return vertexElementFormats;
        }

        public int GetMultisampleQualityLevelsCount(int formatID, int sampleCount)
        {
            int indexOfSampleCount = -1;
            for (int i = 0; i < MultisampleCountsToTry.Length; i++)
            {
                if (MultisampleCountsToTry[i] != sampleCount) 
                    continue;
                indexOfSampleCount = i;
                break;
            }
            return indexOfSampleCount > -1 ? multisampleQualityLevels[formatID, indexOfSampleCount] : 0;
        }
    }
}
