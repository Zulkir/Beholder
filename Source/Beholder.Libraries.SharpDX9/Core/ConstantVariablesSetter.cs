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
using Beholder.Libraries.SharpDX9.Resources;
using Beholder.Utility.Structures;
using SharpDX.Direct3D9;

namespace Beholder.Libraries.SharpDX9.Core
{
    class ConstantVariablesSetter<T> where T : struct, IConvertible
    {
        readonly Device d3dDevice;
        readonly Action<Device, int, T[]> setConstants;
        readonly Pair<CBufferUniform<T>, int>[] registerOccupation;

        public ConstantVariablesSetter(Device d3dDevice, int registerCount, Action<Device, int, T[]> setConstants)
        {
            this.d3dDevice = d3dDevice;
            this.setConstants = setConstants;
            registerOccupation = new Pair<CBufferUniform<T>, int>[registerCount];
        }

        public void SetBuffer(CBufferUniform<T> buffer, int startRegister, int registerCount)
        {
            if (registerOccupation[startRegister].First == buffer && registerOccupation[startRegister].Second == buffer.Revision)
                return;

            if (registerOccupation[startRegister].First != null)
            {
                var bufferToClear = registerOccupation[startRegister].First;
                for (int i = startRegister - 1; i >= 0 && registerOccupation[i].First == bufferToClear; i--)
                    registerOccupation[i] = new Pair<CBufferUniform<T>, int>();
            }

            var pairToSet = new Pair<CBufferUniform<T>, int> { First = buffer, Second = buffer.Revision };
            for (int i = startRegister + 1; i < startRegister + registerCount - 1; i++)
                registerOccupation[i] = pairToSet;

            if (registerOccupation[startRegister + registerCount - 1].First != null)
            {
                var bufferToClear = registerOccupation[startRegister + registerCount - 1].First;
                for (int i = startRegister + registerCount; i < registerOccupation.Length && registerOccupation[i].First == bufferToClear; i++)
                    registerOccupation[i] = new Pair<CBufferUniform<T>, int>();
            }

            setConstants(d3dDevice, startRegister, buffer.Data);
        }

        public void OnDeviceReset()
        {
            for (int i = 0 ; i < registerOccupation.Length; i++)
                registerOccupation[i] = new Pair<CBufferUniform<T>, int>();
        }
    }
}
