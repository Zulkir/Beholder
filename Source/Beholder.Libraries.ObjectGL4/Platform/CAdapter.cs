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

using System.Collections.Generic;
using System.Linq;
using Beholder.Core;
using Beholder.Platform;
using Beholder.Utility.Collections.Readonly;
using Beholder.Utility.ForImplementations.Platform;
using ObjectGL.GL42;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Beholder.Libraries.ObjectGL4.Platform
{
    public class CAdapter : IAdapter
    {
        readonly AdapterDescription adapterDesc;
        readonly ReadonlyArrayWrapper<COutput> roOutputs;

        readonly ApiVersion apiVersion;
        readonly AdapterRestrictions restrictions;
        
        public int Index { get { return 0; } }
        public Api Api { get { return Api.OpenGL; } }
        public ApiVersion ApiVersion { get { return apiVersion; } }
        public IAdapterRestrictions Restrictions { get { return restrictions; } }
        public IReadonlyList<IOutput> Outputs { get { return roOutputs; } }
        public void GetDescription(out AdapterDescription desc) { desc = adapterDesc; }
        
        public CAdapter()
        {
            var tempWindow = new GameWindow();
            GraphicsMode bestMode = tempWindow.Context.GraphicsMode;
            var tempContext = new Context(tempWindow.Context);
            var implementation = tempContext.Implementation;
            apiVersion = new ApiVersion((byte)implementation.MajorVersion, (byte)implementation.MinorVersion);
            restrictions = new AdapterRestrictions
            {
                UniformBufferSlots = tempContext.Implementation.MaxUniformBufferBindings,
                SamplerSlots = tempContext.Implementation.MaxCombinedTextureImageUnits, // todo Make those separate
                ShaderResourceSlots = tempContext.Implementation.MaxCombinedTextureImageUnits,
                UnorderedAccessResourceSlots = 0, // todo Set it
                MaxVertexStreams = tempContext.Implementation.MaxVertexAttributes,
                MaxVertexStreamElementCount = 16, // todo: set it
                MaxStreamOutputTargets = tempContext.Implementation.MaxTransformFeedbackSeparateComponents, // todo: make sure
                MaxViewports = tempContext.Implementation.MaxViewports,
                MaxRenderTargets = tempContext.Implementation.MaxColorAttachments,
                MaxThreadGroupsX = 0, // todo: set it,
                MaxThreadGroupsY = 0, // todo: set it,
                MaxThreadGroupsZ = 0,  // todo: set it
                MaxThreadGroupsTotal = 0
            };

            adapterDesc = new AdapterDescription
            {
                Description = GL.GetString(StringName.Renderer),
                VendorId = 0,
                DeviceId = 0,
                SubSysId = 0,
                Revision = 0,
                DedicatedVideoMemory = 0,
                DedicatedSystemMemory = 0,
                SharedSystemMemory = 0,
                AdapterLuidHigh = 0,
                AdapterLuidLow = 0,
                Flags = AdapterFlags.None
            };

            tempWindow.Dispose();

            roOutputs = new ReadonlyArrayWrapper<COutput>(GetAvailableDisplays().Select((d, i) => new COutput(d, i)).ToArray());

            //foreach (var format in DisplayColorFormats)
            //{
            //    var expectedColorFormat = CtObjectGL.ColorFormat(format);
            //
            //    tempWindow = new GameWindow(1, 1, new GraphicsMode(expectedColorFormat, bestMode.Depth, bestMode.Stencil));
            //    var actualColorFormat = tempWindow.Context.GraphicsMode.ColorFormat;
            //    tempWindow.Dispose();
            //
            //    if (expectedColorFormat.Red == actualColorFormat.Red &&
            //        expectedColorFormat.Green == actualColorFormat.Green &&
            //        expectedColorFormat.Blue == actualColorFormat.Blue &&
            //        expectedColorFormat.Alpha == actualColorFormat.Alpha)
            //    {
            //        supportedDisplayFormats.Add(format);
            //    }
            //}
        }

        static IEnumerable<DisplayDevice> GetAvailableDisplays()
        {
            for (DisplayIndex i = DisplayIndex.First; i <= DisplayIndex.Sixth; i++)
            {
                var display = DisplayDevice.GetDisplay(i);
                if (display != null)
                    yield return display;
                else
                    yield break;
            }
        }

        static readonly Format[] RenderbufferAndTextureFormats =
        {
            #region RenderbufferAndTextureFormats
            Format.Rgba32f,
            Format.Rgba32ui,
            Format.Rgba32i,

            Format.Rgba16f,
            Format.Rgba16,
            Format.Rgba16ui,
            Format.Rgba16i,

            Format.Rg32f,
            Format.Rg32ui,
            Format.Rg32i,

            //PixelInternalFormat.Depth32fStencil8,

            Format.Rgb10A2,

            Format.R11fG11fB10f,

            Format.Rgba8,
            Format.Srgb8Alpha8,
            Format.Rgba8ui,
            Format.Rgba8i,

            Format.Rg16f,
            Format.Rg16,
            Format.Rg16ui,
            Format.Rg16i,

            //PixelInternalFormat.DepthComponent32f,

            Format.R32f,
            Format.R32ui,
            Format.R32i,

            //PixelInternalFormat.Depth24Stencil8,

            Format.Rg8,
            Format.Rg8ui,
            Format.Rg8i,

            //PixelInternalFormat.DepthComponent16,

            Format.R16f,
            Format.R16,
            Format.R16ui,
            Format.R16i,

            Format.R8,
            Format.R8ui,
            Format.R8i
            #endregion
        };

        static readonly Format[] TextureOnlyFormats =
        {
            #region TextureOnlyFormats
            Format.Rgb32f,
            Format.Rgb32ui,
            Format.Rgb32i,

            Format.Rgba16sn,
            /*
            Format.Rgb16f,
            Format.Rgb16,
            Format.Rgb16ui,
            Format.Rgb16i,
            */
            
            Format.Rgba8sn,
            Format.Rg16sn, 
            
            /*
            Format.Rgb8,
            Format.Srgb8,
            Format.Rgb8ui,
            Format.Rgb8i,*/

            Format.R16sn, 
            Format.R8sn, 
            Format.Rgb9E5,
            #endregion
        };

        static readonly Format[] CompressedFormats = 
        {
            #region CompressedFormats
            Format.CompressedRgbaS3tcDxt1Ext,
            Format.CompressedSrgbAlphaS3tcDxt1Ext,

            Format.CompressedRgbaS3tcDxt3Ext,
            Format.CompressedSrgbAlphaS3tcDxt3Ext,

            Format.CompressedRgbaS3tcDxt5Ext,
            Format.CompressedSrgbAlphaS3tcDxt5Ext,

            Format.CompressedRedRgtc1,
            Format.CompressedSignedRedRgtc1,

            Format.CompressedRgRgtc2,
            Format.CompressedSignedRgRgtc2,

            Format.CompressedRgbaBptcUf,
            Format.CompressedRgbaBptcSf,

            Format.CompressedRgbaBptc,
            Format.CompressedSrgbAlphaBptc
            #endregion
        };

        static readonly Format[] DisplayColorFormats = 
        {
            #region DisplayColorFormats
            //Format.Rgba32f,
            //Format.Rgb32f,
            //Format.Rgba16,
            //Format.Rg32f,
            //Format.Rgb10A2,
            //Format.R11fG11fB10f,
            Format.Rgba8,
            //Format.Rg16,
            //Format.R32f,
            //Format.Rg8,
            //Format.R16,
            //Format.R8
            #endregion
        };

        static readonly Format[] MipAutogenFormats = 
        {
            #region MipAutogenFormats
            Format.Rgba32f,
            Format.Rgba16f,
            Format.Rgba16,
            Format.Rgba16sn,
            Format.Rg32f,
            Format.Rgb10A2,
            Format.R11fG11fB10f,
            Format.Rgba8,
            Format.Srgb8Alpha8,
            Format.Rgba8sn, 
            Format.Rg16f,
            Format.Rg16,
            Format.Rg16sn,
            Format.R32f,
            Format.Rg8,
            Format.Rg8sn,
            Format.R16f,
            Format.R16,
            Format.R16sn,
            Format.R8,
            Format.R8sn
            #endregion
        };
        
        static readonly Format[] DepthStencilFormats =
        {
            #region DepthStencilFormats
            Format.Depth32fStencil8,
            Format.DepthComponent32f,
            Format.Depth24Stencil8,
            Format.DepthComponent16
            #endregion
        };

        static readonly ExplicitFormat[] VertexElementFormats = 
        {
            #region VertexElementFormats
            ExplicitFormat.R32G32B32A32_FLOAT,
            ExplicitFormat.R32G32B32A32_UINT,
            ExplicitFormat.R32G32B32A32_SINT,

            ExplicitFormat.R32G32B32_FLOAT,
            ExplicitFormat.R32G32B32_UINT,
            ExplicitFormat.R32G32B32_SINT,

            ExplicitFormat.R16G16B16A16_FLOAT,
            ExplicitFormat.R16G16B16A16_UNORM,
            ExplicitFormat.R16G16B16A16_UINT,
            ExplicitFormat.R16G16B16A16_SNORM,
            ExplicitFormat.R16G16B16A16_SINT,

            ExplicitFormat.R32G32_FLOAT,
            ExplicitFormat.R32G32_UINT,
            ExplicitFormat.R32G32_SINT,

            ExplicitFormat.R10G10B10A2_UNORM,
            //ExplicitFormat.R10G10B10A2_UINT,

            //ExplicitFormat.R11G11B10_FLOAT,

            ExplicitFormat.R8G8B8A8_UNORM,
            ExplicitFormat.R8G8B8A8_UINT,
            ExplicitFormat.R8G8B8A8_SNORM,
            ExplicitFormat.R8G8B8A8_SINT,

            ExplicitFormat.R16G16_FLOAT,
            ExplicitFormat.R16G16_UNORM,
            ExplicitFormat.R16G16_UINT,
            ExplicitFormat.R16G16_SNORM,
            ExplicitFormat.R16G16_SINT,

            ExplicitFormat.R32_FLOAT,
            ExplicitFormat.R32_UINT,
            ExplicitFormat.R32_SINT,

            ExplicitFormat.R8G8_UNORM,
            ExplicitFormat.R8G8_UINT,
            ExplicitFormat.R8G8_SNORM,
            ExplicitFormat.R8G8_SINT,

            ExplicitFormat.R16_FLOAT,
            ExplicitFormat.R16_UNORM,
            ExplicitFormat.R16_UINT,
            ExplicitFormat.R16_SNORM,
            ExplicitFormat.R16_SINT,

            ExplicitFormat.R8_UNORM,
            ExplicitFormat.R8_UINT,
            ExplicitFormat.R8_SNORM,
            ExplicitFormat.R8_SINT,
            #endregion
        };

        static readonly int[] MultisampleCountsToTry = { 2, 4, 8, 16, 32, 64 };

        static readonly FormatInfo[] RenderbufferAndTextureFormatInfos =
            RenderbufferAndTextureFormats
            .Select(CtBeholder.FormatInfo)
            .ToArray();

        static readonly FormatInfo[] TextureOnlyFormatInfos =
            TextureOnlyFormats
            .Select(CtBeholder.FormatInfo)
            .ToArray();

        static readonly FormatInfo[] CompressedFormatInfos =
            CompressedFormats
            .Select(CtBeholder.FormatInfo)
            .ToArray();

        static readonly FormatInfo[] DepthStencilFormatInfos =
            DepthStencilFormats
            .Select(CtBeholder.FormatInfo)
            .ToArray();

        static readonly IEnumerable<FormatInfo> AllColorFormatInfos =
            RenderbufferAndTextureFormatInfos
            .Concat(TextureOnlyFormatInfos)
            .Concat(CompressedFormatInfos)
            .Concat(DepthStencilFormatInfos);

        static readonly IEnumerable<FormatInfo> MultisampleColorFormatInfos =
            RenderbufferAndTextureFormatInfos
            .Concat(TextureOnlyFormatInfos)
            .Concat(DepthStencilFormatInfos);

        static readonly IEnumerable<FormatInfo> Texture1DFormatInfos =
            RenderbufferAndTextureFormatInfos
            .Concat(TextureOnlyFormatInfos)
            .Concat(DepthStencilFormatInfos);

        static readonly IEnumerable<FormatInfo> Texture2DFormatInfos = AllColorFormatInfos;

        static readonly IEnumerable<FormatInfo> Texture3DFormatInfos =
            RenderbufferAndTextureFormatInfos
            .Concat(TextureOnlyFormatInfos)
            .Concat(CompressedFormatInfos);

        static readonly IEnumerable<FormatInfo> TextureCubeFormatInfos = AllColorFormatInfos;

        static readonly IEnumerable<FormatInfo> MipFormatInfos = AllColorFormatInfos;

        static readonly FormatInfo[] MipAutogenFormatInfos =
            AllColorFormatInfos.Where(fi => MipAutogenFormats.Contains((Format)fi.ID))
            .ToArray();

        static readonly FormatInfo[] DisplayColorFormatInfos =
            DisplayColorFormats
            .Select(CtBeholder.FormatInfo)
            .ToArray();
        /*
        static readonly CDepthStencilFormatInfo[] DepthStencilFormatInfos =
            DepthStencilFormats
            .Select(CtBeholder.DepthStencilFormatInfo)
            .ToArray();*/

        public IFormatInfo GetFormatInfo(int formatID)
        {
            return AllColorFormatInfos.Single(fi => fi.ID == formatID);
        }
        /*
        public IDepthStencilFormatInfo GetDepthStencilFormatInfo(int formatID)
        {
            return DepthStencilFormatInfos.Single(fi => fi.ID == formatID);
        }*/

        public IEnumerable<IFormatInfo> GetSupportedWindowedDisplayFormats()
        {
            return DisplayColorFormatInfos;
        }
        /*
        public IEnumerable<IDepthStencilFormatInfo> GetSupportedDepthStencilFormats()
        {
            return DepthStencilFormatInfos;
        }*/

        public IEnumerable<IFormatInfo> GetSupportedFormats(FormatSupport requiredSupport)
        {
            // todo: rewrite

            var resultingFormatInfos = AllColorFormatInfos;

            if ((requiredSupport & FormatSupport.Texture1D) != 0) resultingFormatInfos = resultingFormatInfos.Intersect(Texture1DFormatInfos);
            if ((requiredSupport & FormatSupport.Texture2D) != 0) resultingFormatInfos = resultingFormatInfos.Intersect(Texture2DFormatInfos);
            if ((requiredSupport & FormatSupport.Texture3D) != 0) resultingFormatInfos = resultingFormatInfos.Intersect(Texture3DFormatInfos);
            if ((requiredSupport & FormatSupport.TextureCube) != 0) resultingFormatInfos = resultingFormatInfos.Intersect(TextureCubeFormatInfos);
            if ((requiredSupport & FormatSupport.Mip) != 0) resultingFormatInfos = resultingFormatInfos.Intersect(MipFormatInfos);
            if ((requiredSupport & FormatSupport.MipAutogen) != 0) resultingFormatInfos = resultingFormatInfos.Intersect(MipAutogenFormatInfos);
            if ((requiredSupport & FormatSupport.RenderTarget) != 0) resultingFormatInfos = resultingFormatInfos.Intersect(RenderbufferAndTextureFormatInfos);
            if (requiredSupport.HasFlag(FormatSupport.DepthStencil)) resultingFormatInfos = resultingFormatInfos.Intersect(DepthStencilFormatInfos);

            return resultingFormatInfos;
        }

        public bool CheckViewFormatCompatability(int textureFormatID, int viewFormatID)
        {
            return textureFormatID == viewFormatID;
        }

        public IEnumerable<ExplicitFormat> GetSupportedVertexElementFormats()
        {
            return VertexElementFormats;
        }

        public int GetMultisampleQualityLevelsCount(int formatID, int sampleCount)
        {
            return MultisampleColorFormatInfos.Any(fi => fi.ID == formatID) &&
                (sampleCount == 2 || sampleCount == 4) // todo: replace with ARB_internalformat_query (OpenGL 4.1)
                ? 1 : 0;
        }
    }
}
