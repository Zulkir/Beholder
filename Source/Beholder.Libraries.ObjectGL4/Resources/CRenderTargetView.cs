﻿/*
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
using Beholder.Resources;
using Beholder.Utility.ForImplementations.Resources;
using Beholder.Utility.Helpers;
using ObjectGL.GL42;
using IResource = ObjectGL.GL42.IResource;

namespace Beholder.Libraries.ObjectGL4.Resources
{
    class CRenderTargetView : RenderTargetViewBase<ICDevice, ICResource>
    {
        readonly IResource glResource;

        public IResource GLResource { get { return glResource; } }

        public CRenderTargetView(ICResource resource, Renderbuffer glRenderbuffer, ref RenderTargetViewDescription desc)
            : base(resource.Device, resource, ref desc)
        {
            glResource = glRenderbuffer;
        }

        public CRenderTargetView(ICResource resource, Texture glTexture, ref RenderTargetViewDescription desc)
            : base(resource.Device, resource, ref desc)
        {
            glResource = glTexture;
        }

        public void SetSelf(Context glContext, Framebuffer glFramebuffer, int index)
        {
            switch (glResource.ResourceType)
            {
                case ResourceType.Buffer:
                    throw new InvalidOperationException("Trying to attach a buffer resource to a framebuffer.");
                case ResourceType.Renderbuffer:
                    glFramebuffer.AttachRenderbuffer(glContext, FramebufferAttachmentPoint.Color0 + index, (Renderbuffer)resource.GLResource);
                    return;
                case ResourceType.Texture1D:
                    glFramebuffer.AttachTextureImage(glContext, FramebufferAttachmentPoint.Color0 + index, (Texture1D)resource.GLResource, desc.MipSlice);
                    return;
                case ResourceType.Texture1DArray:
                    if (desc.ArraySize == 1)
                        glFramebuffer.AttachTextureImage(glContext, FramebufferAttachmentPoint.Color0 + index, (Texture1DArray)resource.GLResource, desc.MipSlice, desc.FirstArraySlice);
                    else if (desc.FirstArraySlice == 0 && desc.ArraySize == ((Texture1DArray)resource.GLResource).SliceCount)
                        glFramebuffer.AttachTextureAsLayeredImage(glContext, FramebufferAttachmentPoint.Color0 + index, (Texture1DArray)resource.GLResource, desc.MipSlice);
                    else
                        throw new NotSupportedException("Attaching several images as a single render target is only supported by OpenGL if the whole mip slice of a texture array is being attached");
                    return;
                case ResourceType.Texture2D:
                    glFramebuffer.AttachTextureImage(glContext, FramebufferAttachmentPoint.Color0 + index, (Texture2D)resource.GLResource, desc.MipSlice);
                    return;
                case ResourceType.Texture2DArray:
                    if (desc.ArraySize == 1)
                        glFramebuffer.AttachTextureImage(glContext, FramebufferAttachmentPoint.Color0 + index, (Texture2DArray)resource.GLResource, desc.MipSlice, desc.FirstArraySlice);
                    else if (desc.FirstArraySlice == 0 && desc.ArraySize == ((Texture2DArray)resource.GLResource).SliceCount)
                        glFramebuffer.AttachTextureAsLayeredImage(glContext, FramebufferAttachmentPoint.Color0 + index, (Texture2DArray)resource.GLResource, desc.MipSlice);
                    else
                        throw new NotSupportedException("Attaching several images as a single render target is only supported by OpenGL if the whole mip slice of a texture array is being attached");
                    return;
                case ResourceType.Texture2DMultisample:
                    glFramebuffer.AttachTextureImage(glContext, FramebufferAttachmentPoint.Color0 + index, (Texture2DMultisample)resource.GLResource);
                    return;
                case ResourceType.Texture2DMultisampleArray:
                    if (desc.ArraySize == 1)
                        glFramebuffer.AttachTextureImage(glContext, FramebufferAttachmentPoint.Color0 + index, (Texture2DMultisampleArray)resource.GLResource, desc.FirstArraySlice);
                    else if (desc.FirstArraySlice == 0 && desc.ArraySize == ((Texture2DMultisampleArray)resource.GLResource).SliceCount)
                        glFramebuffer.AttachTextureAsLayeredImage(glContext, FramebufferAttachmentPoint.Color0 + index, (Texture2DMultisampleArray)resource.GLResource);
                    else
                        throw new NotSupportedException("Attaching several images as a single render target is only supported by OpenGL if the whole mip slice of a texture array is being attached");
                    return;
                case ResourceType.Texture3D:
                    if (desc.DepthSliceCount == 1)
                        glFramebuffer.AttachTextureImage(glContext, FramebufferAttachmentPoint.Color0 + index, (Texture3D)resource.GLResource, desc.MipSlice, desc.FirstArraySlice);
                    else if (desc.FirstDepthSlice == 0 && desc.DepthSliceCount == TextureHelper.MipSize(desc.MipSlice, ((Texture3D)resource.GLResource).Depth))
                        glFramebuffer.AttachTextureAsLayeredImage(glContext, FramebufferAttachmentPoint.Color0 + index, (Texture3D)resource.GLResource, desc.MipSlice);
                    else
                        throw new NotSupportedException("Attaching several images as a single render target is only supported by OpenGL if the whole mip slice of a texture array is being attached");
                    return;
                case ResourceType.TextureBuffer:
                    throw new InvalidOperationException("Trying to attach a texture buffer resource to a framebuffer.");
                case ResourceType.TextureCubemap:
                    if (desc.ArraySize == 1)
                        glFramebuffer.AttachTextureImage(glContext, FramebufferAttachmentPoint.Color0 + index, (TextureCubemap)resource.GLResource, desc.MipSlice, CubemapFace.PositiveX + desc.FirstArraySlice);
                    else if (desc.FirstArraySlice == 0 && desc.ArraySize == 6)
                        glFramebuffer.AttachTextureAsLayeredImage(glContext, FramebufferAttachmentPoint.Color0 + index, (TextureCubemap)resource.GLResource, desc.MipSlice);
                    else
                        throw new NotSupportedException("Attaching several images as a single render target is only supported by OpenGL if the whole mip slice of a texture array is being attached");
                    return;
                case ResourceType.TextureCubemapArray:
                    if (desc.ArraySize == 1)
                        glFramebuffer.AttachTextureImage(glContext, FramebufferAttachmentPoint.Color0 + index, (TextureCubemapArray)resource.GLResource, desc.MipSlice, desc.FirstArraySlice / 6, CubemapFace.PositiveX + (desc.FirstArraySlice % 6));
                    else if (desc.FirstArraySlice == 0 && desc.ArraySize == 6 * ((TextureCubemapArray)resource.GLResource).SliceCount)
                        glFramebuffer.AttachTextureAsLayeredImage(glContext, FramebufferAttachmentPoint.Color0 + index, (TextureCubemapArray)resource.GLResource, desc.MipSlice);
                    else
                        throw new NotSupportedException("Attaching several images as a single render target is only supported by OpenGL if the whole mip slice of a texture array is being attached");
                    return;
                case ResourceType.TextureRectangle:
                    throw new InvalidOperationException("Trying to attach a texture rectangle resource to a framebuffer.");
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public bool DescEquals(ref RenderTargetViewDescription otherDesc)
        {
            return RenderTargetViewDescription.Equals(ref desc, ref otherDesc);
        }
    }
}
