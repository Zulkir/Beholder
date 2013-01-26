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
using System.Runtime.InteropServices;
using Beholder.Libraries.SharpDX9.Shaders;
using Beholder.Math;
using Beholder.Resources;
using Beholder.Utility;

namespace Beholder.Libraries.SharpDX9.Resources
{
    sealed class CBufferUniform<T> : CBuffer where T : struct, IConvertible
    {
        readonly T[] data;
        readonly ShaderRegisterType registerType;
        int revision;

        public T[] Data { get { return data; } }
        public int Revision { get { return revision; } }

        public CBufferUniform(ICDevice device, BufferDescription desc, SubresourceData? initialData, Action<CBufferUniform<T>> onRelease)
            : base(device, ref desc, b => onRelease((CBufferUniform<T>)b))
        {
            int sizeOfT = Marshal.SizeOf(typeof(T));
            data = new T[desc.SizeInBytes % sizeOfT == 0 ? desc.SizeInBytes / sizeOfT : desc.SizeInBytes / sizeOfT + 1];

            if (typeof(T) == typeof(float))
                registerType = ShaderRegisterType.C;
            else if (typeof(T) == typeof(int))
                registerType = ShaderRegisterType.I;
            else if (typeof(T) == typeof(bool))
                registerType = ShaderRegisterType.B;
            else
                throw new ShouldNeverHappenException();

            if (initialData.HasValue)
                SetSubresourceData(0, initialData.Value);
        }

        protected override void DisposeOfNative()
        {
        }

        public override unsafe void SetSubresourceData(int subresourceIndex, SubresourceData newData)
        {
            switch (registerType)
            {
                case ShaderRegisterType.C:
                    Marshal.Copy(newData.Pointer, (float[])(Array)data, 0, desc.SizeInBytes / sizeof(float));
                    break;
                case ShaderRegisterType.I:
                    Marshal.Copy(newData.Pointer, (int[])(Array)data, 0, desc.SizeInBytes / sizeof(int));
                    break;
                case ShaderRegisterType.B:
                    var src = (Bool32*)newData.Pointer;
                    for (int i = 0; i < data.Length; i++)
                        ((bool[])(Array)data)[i] = (bool)src[i];
                    break;
            }
            newData.UnpinPointer();
            revision++;
        }
    }
}
