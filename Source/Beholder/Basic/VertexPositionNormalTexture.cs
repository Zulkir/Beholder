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
using System.Runtime.InteropServices;
using Beholder.Math;

namespace Beholder.Basic
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct VertexPositionNormalTexture
    {
        public const int SizeInBytes = 32;

        public Vector3 Position;
        public Vector3 Normal;
        public Vector2 TexCoord;

        public VertexPositionNormalTexture(Vector3 position, Vector3 normal, Vector2 texCoord)
        {
            Position = position;
            Normal = normal;
            TexCoord = texCoord;
        }

        public VertexPositionNormalTexture(ref Vector3 position, ref Vector3 normal, ref Vector2 texCoord)
        {
            Position = position;
            Normal = normal;
            TexCoord = texCoord;
        }

        public VertexPositionNormalTexture(float px, float py, float pz, float nx, float ny, float nz, float tu, float tv)
        {
            Position = new Vector3(px, py, pz);
            Normal = new Vector3(nx, ny, nz);
            TexCoord = new Vector2(tu, tv);
        }
    }
}
