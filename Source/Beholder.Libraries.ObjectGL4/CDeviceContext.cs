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
using Beholder.Core;
using Beholder.Libraries.ObjectGL4.Core;
using Beholder.Libraries.ObjectGL4.Platform;
using Beholder.Libraries.ObjectGL4.Resources;
using Beholder.Libraries.ObjectGL4.Shaders;
using Beholder.Math;
using Beholder.Resources;
using Beholder.Utility.ForImplementations.Core;
using ObjectGL.GL42;
using OpenTK.Graphics.OpenGL;
using IResource = Beholder.Resources.IResource;

namespace Beholder.Libraries.ObjectGL4
{
    public class CDeviceContext : DeviceContextBase<ICDevice>
    {
        readonly Context glContext;
        readonly Pipeline glPipeline;
        readonly Framebuffer glFramebufferForDrawing;
        readonly Framebuffer glFramebufferForClearingColor;
        readonly Framebuffer glFramebufferForClearingDepthStencil;
        readonly VertexArray glVertexArray;
        readonly Presenter presenter;
        readonly DummyColorRenderbufferProvider dummyColorRenderbufferProvider;

        readonly CRasterizerState defaultRasterizerState;
        readonly CDepthStencilState defaultDepthStencilState;
        readonly CBlendState defaultBlendState;
        readonly CSamplerState defaultSamplerState;

        public CDeviceContext(ICDevice device, Context glContext) : base(device)
        {
            this.glContext = glContext;
            glPipeline = glContext.Pipeline;
            glFramebufferForDrawing = new Framebuffer(glContext);
            glFramebufferForClearingColor = new Framebuffer(glContext);
            glFramebufferForClearingDepthStencil = new Framebuffer(glContext);
            glVertexArray = new VertexArray(glContext);
            presenter = new Presenter(glContext);
            dummyColorRenderbufferProvider = new DummyColorRenderbufferProvider(glContext);

            defaultRasterizerState = (CRasterizerState)device.Create.RasterizerState(RasterizerDescription.Default);
            defaultDepthStencilState = (CDepthStencilState)device.Create.DepthStencilState(DepthStencilDescription.Default);
            defaultBlendState = (CBlendState)device.Create.BlendState(BlendDescription.Default);
            defaultSamplerState = (CSamplerState)device.Create.SamplerState(SamplerDescription.Default);
        }

        public void Dispose()
        {
            dummyColorRenderbufferProvider.Dispose();
            presenter.Dispose();
            glFramebufferForClearingDepthStencil.Dispose();
            glFramebufferForClearingColor.Dispose();
            glFramebufferForDrawing.Dispose();
            glVertexArray.Dispose();
        }

        internal Context GLContext { get { return glContext; } }
        public override DeviceContextType ContextType { get { return DeviceContextType.Immediate; } }

        public void OnEndScene(SwapChainSurfaces swapChainSurfaces)
        {
            presenter.OnEndScene(glContext, swapChainSurfaces.BackBuffer);
        }

        public unsafe override void ClearRenderTargetView(IRenderTargetView renderTargetView, Color4 color)
        {
            ((CRenderTargetView)renderTargetView).SetSelf(glContext, glFramebufferForClearingColor, 0);
            glFramebufferForClearingColor.ClearColor(glContext, 0, *(OpenTK.Graphics.Color4*)&color);
        }

        public override void ClearDepthStencilView(IDepthStencilView depthStencilView, ClearDepthStencilFlags flags, float depth, byte stencil)
        {
            var cDepthStencilView = ((CDepthStencilView)depthStencilView);

            var dummyBuffer = dummyColorRenderbufferProvider.GetRenderbuffer(cDepthStencilView.Width, cDepthStencilView.Height, cDepthStencilView.Samples);
            glFramebufferForClearingDepthStencil.AttachRenderbuffer(glContext, FramebufferAttachmentPoint.Color0, dummyBuffer);

            cDepthStencilView.SetSelf(glContext, glFramebufferForClearingDepthStencil);
            glFramebufferForClearingDepthStencil.ClearDepthStencil(glContext, CtObjectGL.DepthStencil(flags), depth, stencil);

            //GL.CheckFramebufferStatus(FramebufferTarget.DrawFramebuffer);
        }

        public override void ClearUnorderedAccessView(IUnorderedAccessView unorderedAccessView, Vector4 value)
        {
            throw new NotImplementedException();
        }

        public override void ClearUnorderedAccessView(IUnorderedAccessView unorderedAccessView, IntVector4 value)
        {
            throw new NotImplementedException();
        }

        public override void CopyResource(IResource dstResource, IResource srcResource)
        {
            throw new NotImplementedException();
        }

        public override void CopySubresourceRegion(IResource dstResource, int dstSubresource, int dstX, int dstY, int dstZ, IResource srcResource, int srcSubresource, Box? srcBox)
        {
            throw new NotImplementedException();
        }

        public override void GenerateMips(IShaderResourceView shaderResourceView)
        {
            ((CShaderResourceView)shaderResourceView).GLTexture.GenerateMipmap(glContext);
        }

        public override MappedSubresource Map(IResource resource, int subresource, MapType mapType, MapFlags mapFlags)
        {
            throw new NotImplementedException();
        }

        public override void SetSubresourceData(IResource resource, int subresourceIndex, SubresourceData data)
        {
            ((ICResource)resource).SetSubresourceData(this, subresourceIndex, data);
        }

        public override void Unmap(IResource resource, int subresource)
        {
            throw new NotImplementedException();
        }

        public override void Dispatch(int threadGroupCountX, int threadGroupCountY, int threadGroupCountZ)
        {
            throw new NotImplementedException();
        }

        public override void DispatchIndirect(IBuffer bufferForArgs, int alignedByteOffsetForArgs)
        {
            throw new NotImplementedException();
        }

        void PreDraw()
        {
            glPipeline.VertexArray = glVertexArray;
            glPipeline.Framebuffer = glFramebufferForDrawing;
            ConsumeShadersForDrawing();
            ConsumeInputAssembler();
            ConsumeStreamOutput();
            ConsumeRasterizer();
            ConsumeOutputMerger();
        }

        void ConsumeShadersForDrawing()
        {
            var bShadersForDrawing = ShadersForDrawing;
            var shaders = (CShaderCombination)bShadersForDrawing.Value;
            bool shaderCombinationIsDirty = bShadersForDrawing.IsDirty;

            if (shaderCombinationIsDirty)
            {
                glPipeline.Program = shaders.GLProgram;
                bShadersForDrawing.Clean();
            }

            ConsumeShaderStage(shaders.VertexShader, VertexStage, shaderCombinationIsDirty);
            ConsumeShaderStage(shaders.HullShader, HullStage, shaderCombinationIsDirty);
            ConsumeShaderStage(shaders.DomainShader, DomainStage, shaderCombinationIsDirty);
            ConsumeShaderStage(shaders.GeometryShader, GeometryStage, shaderCombinationIsDirty);
            ConsumeShaderStage(shaders.PixelShader, PixelStage, shaderCombinationIsDirty);

            glPipeline.UniformBuffers.UnsetAllStartingFrom(shaders.MaxValueBufferSlotPlusOne);
            glPipeline.Textures.UnsetAllStartingFrom(shaders.MaxTextureSlotPlusOne);
        }

        void ConsumeShaderStage(CShader shader, DeviceContextBaseShaderStage bStage, bool force)
        {
            if (force)
            {
                if (shader != null)
                {
                    foreach (var bufferWithSlots in shader.UniformBufferSlots.GetActive())
                        glPipeline.UniformBuffers[bufferWithSlots.GLSlot] = ((CBuffer)bStage.UniformBuffers[bufferWithSlots.BSlot]).GLBuffer;
                    
                    foreach (var textureWithSlots in shader.TextureSlots.GetActive())
                        glPipeline.Textures[textureWithSlots.GLSlot] = ((CShaderResourceView)bStage.ShaderResources[textureWithSlots.BSlot]).GLTexture;
                    
                    foreach (var kvp in shader.SamplerMap)
                        foreach (var samplerSettingData in kvp.Value)
                            glPipeline.Samplers[samplerSettingData.GLSlot] = samplerSettingData.SupportsMips
                                ? ((CSamplerState)bStage.Samplers[kvp.Key] ?? defaultSamplerState).GLSampler
                                : ((CSamplerState)bStage.Samplers[kvp.Key] ?? defaultSamplerState).GLSamplerMipless;
                }

                bStage.UniformBuffers.Clean();
                bStage.ShaderResources.Clean();
                bStage.Samplers.Clean();
            }
            else
            {
                var bUniformBuffers = bStage.UniformBuffers;
                if (bUniformBuffers.IsDirty)
                {
                    if (shader != null)
                    {
                        foreach (var bSlot in bUniformBuffers.DirtyIndices)
                        {
                            CShaderUniformBufferVariable variable;
                            if (shader.UniformBufferSlots.TryGetVariable(bSlot, out variable))
                                glPipeline.UniformBuffers[variable.GLSlot] = bUniformBuffers[bSlot] != null ? ((CBuffer)bUniformBuffers[bSlot]).GLBuffer : null;
                        }
                    }
                    bUniformBuffers.Clean();
                }

                var bSrvs = bStage.ShaderResources;
                if (bSrvs.IsDirty)
                {
                    if (shader != null)
                    {
                        foreach (var bSlot in bSrvs.DirtyIndices)
                        {
                            CShaderObjectVariable variable;
                            if (shader.TextureSlots.TryGetVariable(bSlot, out variable))
                                glPipeline.Textures[variable.GLSlot] = bSrvs[bSlot] != null ? ((CShaderResourceView)bSrvs[bSlot]).GLTexture : null;
                        }
                    }
                    bSrvs.Clean();
                }

                var bSamplers = bStage.Samplers;
                if (bSamplers.IsDirty)
                {
                    if (shader != null)
                        foreach (var dirtyIndex in bSamplers.DirtyIndices)
                        {
                            SamplerSettingData[] samplerSettingDataArray;
                            if (shader.SamplerMap.TryGetValue(dirtyIndex, out samplerSettingDataArray))
                                foreach (var samplerSettingData in samplerSettingDataArray)
                                    glPipeline.Samplers[samplerSettingData.GLSlot] = samplerSettingData.SupportsMips
                                        ? ((CSamplerState)bStage.Samplers[dirtyIndex] ?? defaultSamplerState).GLSampler
                                        : ((CSamplerState)bStage.Samplers[dirtyIndex] ?? defaultSamplerState).GLSamplerMipless;
                        }
                    bSamplers.Clean();
                }
            }
        }

        void ConsumeInputAssembler()
        {
            var bPrimitiveTopology = InputAssembler.PrimitiveTopology;
            if (bPrimitiveTopology.IsDirty)
            {
                BeginMode mode;
                int patchVertexCount;
                CtObjectGL.BeginMode(bPrimitiveTopology.Value, out mode, out patchVertexCount);
                glPipeline.PatchVertexCount = patchVertexCount;
                bPrimitiveTopology.Clean();
            }

            var bVertexLayout = InputAssembler.VertexLayout;
            var bVertexSources = InputAssembler.VertexSources;
            if (bVertexLayout.IsDirty || bVertexSources.IsDirty)
            {
                var layoutElements = ((CVertexLayout)bVertexLayout.Value).WElements;
                for (int i = 0; i < layoutElements.Length; i++)
                {
                    var layoutElement = layoutElements[i];
                    var streamSource = bVertexSources[layoutElement.StreamSourceIndex];
                    var glBuffer = ((CBuffer)streamSource.Buffer).GLBuffer;

                    switch (layoutElement.Type)
                    {
                        case WVertexLayoutElementType.F:
                            glVertexArray.SetVertexAttributeF(glContext, i, glBuffer, layoutElement.Dimension, 
                                (VertexAttribPointerType)layoutElement.GLVertexAttribPointerType, 
                                layoutElement.Normalized, 
                                streamSource.Stride, streamSource.Offset + layoutElement.Offset, layoutElement.Divisor);
                            break;
                        case WVertexLayoutElementType.I:
                            glVertexArray.SetVertexAttributeI(glContext, i, glBuffer, layoutElement.Dimension, 
                                (VertexAttribIPointerType)layoutElement.GLVertexAttribPointerType,
                                streamSource.Stride, streamSource.Offset + layoutElement.Offset, layoutElement.Divisor);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
                glVertexArray.DisableVertexAttributesStartingFrom(glContext, layoutElements.Length);
                bVertexLayout.Clean();
                bVertexSources.Clean();
            }

            var bIndexSource = InputAssembler.IndexSource;
            if (bIndexSource.IsDirty)
            {
                if (bIndexSource.Value.Offset != 0)
                    throw new NotSupportedException("IndexSource.Offset is not supported by OpenGL");
                glVertexArray.SetElementArrayBuffer(glContext, bIndexSource.Value.Buffer != null ? ((CBuffer)bIndexSource.Value.Buffer).GLBuffer : null);
                bIndexSource.Clean();
            }
        }

        void ConsumeStreamOutput()
        {
            var bTargets = StreamOutput.Targets;
            if (bTargets.IsDirty)
            {
                // TODO
                //throw new NotImplementedException("Stream Output is not yet supported by OGL implementation of Beholder");
                bTargets.Clean();
            }
        }

        void ConsumeRasterizer()
        {
            var bState = Rasterizer.State;
            if (bState.IsDirty)
            {
                ((CRasterizerState)bState.Value ?? defaultRasterizerState).Apply(glPipeline);
                bState.Clean();
            }

            int renderTargetHeight = OutputMerger.RenderTargets.CurrentCount > 0 ? ((CRenderTargetView)OutputMerger.RenderTargets[0]).Height
                    : OutputMerger.DepthStencil.Value != null ? ((CDepthStencilView)OutputMerger.DepthStencil.Value).Height : 0;

            bool renderTargetHeightIsDirty = OutputMerger.RenderTargets.IsDirty || OutputMerger.DepthStencil.IsDirty;

            var bViewports = Rasterizer.Viewports;
            if (bViewports.IsDirty || renderTargetHeightIsDirty)
            {
                for (int i = 0; i < bViewports.CurrentCount; i++)
                {
                    var viewport = bViewports[i];
                    glPipeline.Viewports[i].Set(viewport.Left, renderTargetHeight - viewport.Top - viewport.Height,
                        viewport.Width, viewport.Height, viewport.MinDepth, viewport.MaxDepth);
                }
                glPipeline.Viewports.EnabledViewportCount = bViewports.CurrentCount;
                bViewports.Clean();
            }

            var bScissorRectangles = Rasterizer.ScissorRectangles;
            if (bScissorRectangles.IsDirty || renderTargetHeightIsDirty)
            {
                for (int i = 0; i < bScissorRectangles.CurrentCount; i++)
                {
                    var scissorRectangle = bScissorRectangles[i];
                    glPipeline.ScissorBoxes[i].Set(scissorRectangle.X, renderTargetHeight - scissorRectangle.Y, 
                        scissorRectangle.Width, scissorRectangle.Height);
                }
                bScissorRectangles.Clean();
            }
        }

        unsafe void ConsumeOutputMerger()
        {
            var bRenderTargets = OutputMerger.RenderTargets;
            if (bRenderTargets.IsDirty)
            {
                for (int i = 0; i < bRenderTargets.CurrentCount; i++)
                    ((CRenderTargetView)bRenderTargets[i]).SetSelf(glContext, glFramebufferForDrawing, i);
                glFramebufferForDrawing.DetachColorStartingFrom(glContext, bRenderTargets.CurrentCount);
                bRenderTargets.Clean();
            }

            var bDepthStencil = OutputMerger.DepthStencil;
            if (bDepthStencil.IsDirty)
            {
                if (bDepthStencil.Value != null)
                    ((CDepthStencilView)bDepthStencil.Value).SetSelf(glContext, glFramebufferForDrawing);
                else
                    glFramebufferForDrawing.Detach(glContext, FramebufferAttachmentPoint.DepthStencil);
                bDepthStencil.Clean();
            }

            var bUnorderedAccessResources = OutputMerger.UnorderedAccessResources;
            if (bUnorderedAccessResources.IsDirty || OutputMerger.InitialCountsChangedIndices.Count > 0)
            {
                // TODO
                //throw new NotImplementedException("UAVs are not yet supported by OGL implementation of Beholder");
                bUnorderedAccessResources.Clean();
                OutputMerger.InitialCountsChangedIndices.Clear();
            }

            var bBlendState = OutputMerger.BlendState;
            if (bBlendState.IsDirty)
            {
                ((CBlendState)bBlendState.Value ?? defaultBlendState).Apply(glPipeline);
                bBlendState.Clean();
            }

            var bBlendFactor = OutputMerger.BlendFactor;
            if (bBlendFactor.IsDirty)
            {
                var blendFactor = bBlendFactor.Value;
                glPipeline.Blend.BlendColor = *(OpenTK.Graphics.Color4*)& blendFactor;
                bBlendFactor.Clean();
            }

            var bSampleMask = OutputMerger.SampleMask;
            if (bSampleMask.IsDirty)
            {
                glPipeline.Blend.SampleMask = bSampleMask.Value;
                bSampleMask.Clean();
            }

            var bDepthStencilState = OutputMerger.DepthStencilState;
            if (bDepthStencilState.IsDirty)
            {
                ((CDepthStencilState)bDepthStencilState.Value ?? defaultDepthStencilState).Apply(glPipeline);
                bDepthStencilState.Clean();
            }

            var bStencilReference = OutputMerger.StencilReference;
            if (bStencilReference.IsDirty)
            {
                glPipeline.DepthStencil.Front.StencilRef = bStencilReference.Value;
                glPipeline.DepthStencil.Back.StencilRef = bStencilReference.Value;
                bStencilReference.Clean();
            }
        }

        BeginMode GetBeginMode()
        {
            BeginMode mode;
            int patchVertexCount;
            CtObjectGL.BeginMode(InputAssembler.PrimitiveTopology.Value, out mode, out patchVertexCount);
            return mode;
        }

        DrawElementsType GetDrawElementsType()
        {
            return CtObjectGL.DrawElementsType(InputAssembler.IndexSource.Value.Format);
        }

        public override void Draw(int vertexCount, int startVertexLocation)
        {
            PreDraw();
            var mode = GetBeginMode();
            glContext.DrawArrays(mode,startVertexLocation, vertexCount);
        }

        public override void DrawAuto()
        {
            throw new NotImplementedException();
        }

        public override void DrawIndexed(int indexCount, int startIndexLocation, int baseVertexLocation)
        {
            PreDraw();
            var mode = GetBeginMode();
            var indexType = GetDrawElementsType();

            if (baseVertexLocation == 0)
                glContext.DrawElements(mode, indexCount, indexType, startIndexLocation);
            else
                glContext.DrawElementsBaseVertex(mode, indexCount, indexType, startIndexLocation, baseVertexLocation);
        }

        public override void DrawIndexedInstanced(int indexCountPerInstance, int instanceCount, int startIndexLocation, int baseVertexLocation, int startInstanceLocation)
        {
            PreDraw();
            var mode = GetBeginMode();
            var indexType = GetDrawElementsType();

            if (startInstanceLocation == 0)
                if (baseVertexLocation == 0)
                    glContext.DrawElementsInstanced(mode, indexCountPerInstance, indexType, startIndexLocation, instanceCount);
                else
                    glContext.DrawElementsInstancedBaseVertex(mode, indexCountPerInstance, indexType, startIndexLocation, instanceCount, baseVertexLocation);
            else
                if (baseVertexLocation == 0)
                    glContext.DrawElementsInstancedBaseInstance(mode, indexCountPerInstance, indexType, startIndexLocation, instanceCount, startInstanceLocation);
                else
                    glContext.DrawElementsInstancedBaseVertexBaseInstance(mode, indexCountPerInstance, indexType, startIndexLocation, instanceCount, baseVertexLocation, startInstanceLocation);
        }

        public override void DrawIndexedInstancedIndirect(IBuffer bufferForArgs, int alignedByteOffsetForArgs)
        {
            PreDraw();
            var mode = GetBeginMode();
            var indexType = GetDrawElementsType();
            glPipeline.DrawIndirectBuffer = ((CBuffer)bufferForArgs).GLBuffer;
            glContext.DrawElementsIndirect(mode, indexType, alignedByteOffsetForArgs);
        }

        public override void DrawInstanced(int vertexCountPerInstance, int instanceCount, int startVertexLocation, int startInstanceLocation)
        {
            PreDraw();
            var mode = GetBeginMode();

            if (startInstanceLocation == 0)
                glContext.DrawArraysInstanced(mode, startVertexLocation, vertexCountPerInstance, instanceCount);
            else
                glContext.DrawArraysInstancedBaseInstance(mode, startVertexLocation, vertexCountPerInstance, instanceCount, startInstanceLocation);
        }

        public override void DrawInstancedIndirect(IBuffer bufferForArgs, int alignedByteOffsetForArgs)
        {
            PreDraw();
            var mode = GetBeginMode();
            glPipeline.DrawIndirectBuffer = ((CBuffer)bufferForArgs).GLBuffer;
            glContext.DrawArraysIndirect(mode, alignedByteOffsetForArgs);
        }
    }
}
