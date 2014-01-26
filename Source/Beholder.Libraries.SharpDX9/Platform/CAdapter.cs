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
using System.Collections.Generic;
using System.Linq;
using Beholder.Core;
using Beholder.Platform;
using Beholder.Utility.ForImplementations.Platform;
using SharpDX.Direct3D9;
using DisplayMode = SharpDX.Direct3D9.DisplayMode;

namespace Beholder.Libraries.SharpDX9.Platform
{
    public class CAdapter : IAdapter, IOutput
    {
        readonly Direct3D direct3D;
        readonly AdapterInformation adapterInfo;
        readonly DisplayMode desctopDisplayMode;
        readonly Capabilities caps;
        readonly int index;
        readonly ApiVersion apiVersion;
        readonly AdapterRestrictions restrictions;
        readonly AdapterDescription adapterDesc;
        readonly OutputDescription outputDesc;

        readonly CAdapter[] outputs;
        readonly Dictionary<Format, FormatSupport> formatSupports;

        public int Index { get { return index; } }
        public IReadOnlyList<IOutput> Outputs { get { return outputs; } }
        void IAdapter.GetDescription(out AdapterDescription desc) { desc = adapterDesc; }
        void IOutput.GetDescription(out OutputDescription description) { description = outputDesc; }
        public AdapterInformation Info { get { return adapterInfo; } }
        public Api Api { get { return Api.Direct3D; } }
        public ApiVersion ApiVersion { get { return apiVersion; } }
        public IAdapterRestrictions Restrictions { get { return restrictions; } }

        public CAdapter(Direct3D direct3D, AdapterInformation adapterInfo)
        {
            this.direct3D = direct3D;
            this.adapterInfo = adapterInfo;
            desctopDisplayMode = adapterInfo.CurrentDisplayMode;
            caps = adapterInfo.GetCaps(DeviceType.Hardware);
            index = adapterInfo.Adapter;
            var shaderModel = System.Math.Min(caps.VertexShaderVersion.Major, caps.PixelShaderVersion.Major);
            apiVersion = new ApiVersion(9, (byte)shaderModel);
            var details = adapterInfo.Details;
            restrictions = new AdapterRestrictions
            {
                UniformBufferSlots = 256,
                SamplerSlots = caps.MaxSimultaneousTextures,
                ShaderResourceSlots = caps.MaxSimultaneousTextures,
                UnorderedAccessResourceSlots = 0,
                MaxVertexStreams = caps.MaxStreams,
                MaxVertexStreamElementCount = caps.MaxStreamStride / (4 * sizeof(float)),
                MaxStreamOutputTargets = 0,
                MaxViewports = 1,
                MaxRenderTargets = caps.SimultaneousRTCount,
                MaxThreadGroupsX = 0,
                MaxThreadGroupsY = 0,
                MaxThreadGroupsZ = 0,
                MaxThreadGroupsTotal = 0,
            };

            adapterDesc = new AdapterDescription
            {
                Description = details.Description,
                VendorId = details.VendorId,
                DeviceId = details.DeviceId,
                SubSysId = details.SubsystemId,
                Revision = details.Revision,
                DedicatedVideoMemory = 0,
                DedicatedSystemMemory = 0,
                SharedSystemMemory = 0,
                AdapterLuidHigh = 0,
                AdapterLuidLow = 0,
                Flags = AdapterFlags.None
            };

            outputDesc = new OutputDescription
            {
                DeviceName = details.DeviceName,
                DesctopCoordinates = new Math.Rectangle(),
                AttachedToDesctop = false,
                Rotation = ModeRotation.Unspecified,
                MonitorHandle = adapterInfo.Monitor
            };

            outputs = new[] { this };

            formatSupports = ((Format[])Enum.GetValues(typeof(Format))).ToDictionary(f => f, GetFormatSupport);
        }

        static readonly Format[] DisplayFormats = 
        { 
            #region Formats
            Format.A2R10G10B10, 
            Format.X8R8G8B8, 
            Format.X1R5G5B5,
            Format.R5G6B5 
            #endregion
        };
        static readonly IFormatInfo[] DisplayFormatInfos = DisplayFormats.Select(CtBeholder.DisplayPixelFormatInfo).ToArray();

        static readonly Format[] DepthStencilFormats = 
        {
            #region Formats
            //Format.D16Lockable,
            Format.D32,
            Format.D15S1,
            Format.D24S8,
            Format.D24X8,
            Format.D24X4S4,
            //Format.D32SingleLockable,
            Format.D24SingleS8,
            //Format.D32Lockable,
            //Format.S8Lockable,
            Format.D16
            #endregion
        };

        static readonly Format[] TexelFormats =
        {
            #region Formats
            Format.R8G8B8,
            Format.A8R8G8B8,
            Format.X8R8G8B8,
            Format.R5G6B5,
            Format.X1R5G5B5,
            Format.A1R5G5B5,
            Format.A4R4G4B4,
            Format.R3G3B2,
            Format.A8,
            Format.A8R3G3B2,
            Format.X4R4G4B4,
            Format.A2B10G10R10,
            Format.A8B8G8R8,
            Format.X8B8G8R8,
            Format.G16R16,
            Format.A2R10G10B10,
            Format.A16B16G16R16,

            Format.R32F,
            Format.G32R32F,
            Format.A32B32G32R32F,

            Format.R16F,
            Format.G16R16F,
            Format.A16B16G16R16F,

            Format.Dxt1,
            //Format.Dxt2,
            Format.Dxt3,
            //Format.Dxt4,
            Format.Dxt5,
            #endregion
        };
        static readonly IFormatInfo[] FormatInfos = TexelFormats.Concat(DepthStencilFormats).Select(CtBeholder.TexelFormatInfo).ToArray();

        static readonly DeclarationType[] VertexElementFormats =
        {
            #region Formats
            DeclarationType.Float1,
            DeclarationType.Float2,
            DeclarationType.Float3,
            DeclarationType.Float4,
            DeclarationType.Short2,
            DeclarationType.Short2N,
            DeclarationType.Short4,
            DeclarationType.Short4N,
            DeclarationType.Ubyte4,
            DeclarationType.UByte4N,
            DeclarationType.UShort2N,
            DeclarationType.UShort4N
            #endregion
        };
        static readonly ExplicitFormat[] VertexElementFormatInfos = VertexElementFormats.Select(CtBeholder.VertexElementFormat).ToArray();

        FormatSupport GetFormatSupport(Format d3dFormat)
        {
            FormatSupport support = 0;
            if (CheckFormat(Usage.None, ResourceType.Texture, d3dFormat))
                support |= (FormatSupport.Texture1D | FormatSupport.Texture2D);
            if (CheckFormat(Usage.None, ResourceType.VolumeTexture, d3dFormat))
                support |= FormatSupport.Texture3D;
            if (CheckFormat(Usage.None, ResourceType.CubeTexture, d3dFormat))
                support |= FormatSupport.TextureCube;
            if (CheckFormatTexture(Usage.None, d3dFormat))
                support |= FormatSupport.ShaderSample;
            // todo: FormatSupport.ShaderSampleComparison
            if (CheckFormatTexture(Usage.None, d3dFormat))
                support |= FormatSupport.Mip;
            if (CheckFormat(Usage.AutoGenerateMipMap, ResourceType.Texture, d3dFormat))
                support |= FormatSupport.MipAutogen;
            if (CheckFormat(Usage.RenderTarget, ResourceType.Surface, d3dFormat))
                support |= FormatSupport.RenderTarget;
            if (CheckFormat(Usage.None, ResourceType.Surface, d3dFormat)) // todo: make sure
                support |= FormatSupport.Blendable;
            if (DepthStencilFormats.Contains(d3dFormat) && CheckFormat(Usage.DepthStencil, ResourceType.Surface, d3dFormat))
                support |= FormatSupport.DepthStencil;
            // todo: FormatSupport.MultisampleResolve
            if (CheckFormat(Usage.RenderTarget, ResourceType.Surface, d3dFormat) && (
                direct3D.CheckDeviceMultisampleType(index, DeviceType.Hardware, d3dFormat, true, MultisampleType.TwoSamples) ||
                direct3D.CheckDeviceMultisampleType(index, DeviceType.Hardware, d3dFormat, true, MultisampleType.FourSamples) ||
                direct3D.CheckDeviceMultisampleType(index, DeviceType.Hardware, d3dFormat, true, MultisampleType.EightSamples) ||
                direct3D.CheckDeviceMultisampleType(index, DeviceType.Hardware, d3dFormat, true, MultisampleType.SixteenSamples)))
                support |= FormatSupport.MultisampleRendertarget;

            return support;
        }

        bool CheckFormatTexture(Usage usage, Format format)
        {
            return
                CheckFormat(usage, ResourceType.Texture, format) ||
                CheckFormat(usage, ResourceType.CubeTexture, format) ||
                CheckFormat(usage, ResourceType.VolumeTexture, format);
        }

        bool CheckFormat(Usage usage, ResourceType resourceType, Format format)
        {
            return direct3D.CheckDeviceFormat(index, DeviceType.Hardware, desctopDisplayMode.Format, usage, resourceType, format);
        }

        public IEnumerable<Beholder.Platform.DisplayMode> GetSupportedDisplayModes()
        {
            return DisplayFormats.SelectMany(d3dFormat =>
                adapterInfo.GetDisplayModes(d3dFormat).Select(d3dDiplayMode => new Beholder.Platform.DisplayMode
                {
                    Width = d3dDiplayMode.Width,
                    Height = d3dDiplayMode.Height,
                    RefreshRate = d3dDiplayMode.RefreshRate,
                    FormatID = (int)d3dFormat
                }));
        }

        public IFormatInfo GetFormatInfo(int formatID)
        {
            return FormatInfos.Single(fi => fi.ID == formatID);
        }

        public IEnumerable<IFormatInfo> GetSupportedWindowedDisplayFormats()
        {
            return DisplayFormatInfos.Where(formatInfo => formatInfo.ID == (int)desctopDisplayMode.Format);
        }

        public IEnumerable<IFormatInfo> GetSupportedFormats(FormatSupport requiredSupport)
        {
            return FormatInfos.Where(fi => (formatSupports[(Format)fi.ID] & requiredSupport) == requiredSupport);
        }

        public bool CheckViewFormatCompatability(int textureFormatID, int viewFormatID)
        {
            return textureFormatID == viewFormatID;
        }

        public IEnumerable<ExplicitFormat> GetSupportedVertexElementFormats()
        {
            return VertexElementFormatInfos;
        }

        public int GetMultisampleQualityLevelsCount(int formatID, int sampleCount)
        {
            if (sampleCount > 16)
                return 0;
            int qualityLevels;
            direct3D.CheckDeviceMultisampleType(adapterInfo.Adapter, DeviceType.Hardware, (Format)formatID, true, (MultisampleType)sampleCount, out qualityLevels);
            return qualityLevels;
        }
    }
}
