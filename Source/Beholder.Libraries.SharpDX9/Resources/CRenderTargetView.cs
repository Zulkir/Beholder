﻿/*
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

using Beholder.Resources;
using Beholder.Utility.ForImplementations.Resources;
using SharpDX.Direct3D9;

namespace Beholder.Libraries.SharpDX9.Resources
{
    public class CRenderTargetView : RenderTargetViewBase<ICDevice, ICResource>
    {
        Surface d3dSurface;

        public Surface D3DSurface { get { return d3dSurface; } set { d3dSurface = value; } }

        public CRenderTargetView(ICResource resource, Surface d3dSurface, 
            ref RenderTargetViewDescription desc)
            : base(resource.Device, resource, ref desc)
        {
            // todo: validate description

            this.d3dSurface = d3dSurface;
        }

        public bool DescEquals(ref RenderTargetViewDescription otherDesc) { return RenderTargetViewDescription.Equals(ref desc, ref otherDesc); }
    }
}