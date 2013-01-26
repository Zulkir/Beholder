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

using Beholder.Resources;
using Beholder.Utility.ForImplementations.Resources;
using ObjectGL.GL42;
using IResource = Beholder.Resources.IResource;

namespace Beholder.Libraries.ObjectGL4.Resources
{
    class CShaderResourceView : ShaderResourceViewBase<ICDevice>
    {
        readonly ICResource resource;
        readonly Texture glTexture;

        public override IResource Resource { get { return resource; } }
        public Texture GLTexture { get { return glTexture; } }

        public CShaderResourceView(ICResource resource, Texture glTexture, ref ShaderResourceViewDescription desc)
            : base (resource.Device, ref desc) 
        { 
            // todo: OpenGL-specific desc validation
            this.resource = resource;
            this.glTexture = glTexture;
        }

        public bool DescEquals(ref ShaderResourceViewDescription otherDesc)
        {
            return ShaderResourceViewDescription.Equals(ref desc, ref otherDesc);
        }
    }
}
