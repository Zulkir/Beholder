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
using Beholder.Core;
using Beholder.Shaders;
using Beholder.Shaders.Reflection;
using System.Linq;

namespace Beholder.Validation.Core
{
    class VertexLayout : DeviceChild<IVertexLayout>, IVertexLayout
    {
        public VertexLayout(IVertexLayout real) : base(real) { }

        public IVertexShader VertexShader { get { return Wrappers.Get(Real.VertexShader); } }
        public IReadOnlyList<VertexLayoutElement> Elements { get { return Real.Elements; } }

        public static void ValidateElements(IVertexShader vertexShader, VertexLayoutElement[] elements)
        {
            var adapter = vertexShader.Device.Adapter;
            var restrictions = adapter.Restrictions;

            var expectedLength = vertexShader.Reflection.Input.Sum(v => v.LayoutElementsRequired);
            if (elements.Length != expectedLength)
                throw new ArgumentException(string.Format("{0} layout elements expected for the given shader, but {1} were provided", expectedLength, elements.Length));

            int currentVariableIndex = 0;
            int currentVariableElementIndex = 0;

            for (int i = 0; i < elements.Length; i++)
            {
                var element = elements[i];
                Check.NotNegative(element.InputSlot, string.Format("elements[{0}].InputSlot", i));
                if (element.InputSlot >= restrictions.MaxVertexStreams)
                    throw new ArgumentException(string.Format("elements[{0}].InputSlot is {1}, while Device.Adapter.Restrictions.MaxVertexStreams is {2}", i, element.InputSlot, restrictions.MaxVertexStreams));
                Check.NotNegative(element.Offset, string.Format("elements[{0}].Offset", i));
                if (element.Offset >= restrictions.MaxVertexStreamElementCount * 4 * sizeof(float))
                    throw new ArgumentException(string.Format("elements[{0}].Offset is {1}, while Device.Adapter.Restrictions.MaxVertexStreamElementCount is {2}", i, element.Offset, restrictions.MaxVertexStreamElementCount));
                switch (element.VertexLayoutClassification)
                {
                    case VertexLayoutClassification.PerVertexData:
                        if (element.InstanceDataStepRate != 0)
                            throw new ArgumentException(string.Format("elements[{0}].InstanceDataStepRate is {1}, but element.VertexLayoutClassification is {2}", i, element.InstanceDataStepRate, element.VertexLayoutClassification));
                        break;
                    case VertexLayoutClassification.PerInstanceData:
                        if (element.InstanceDataStepRate <= 0)
                            throw new ArgumentException(string.Format("element[{0}].InstanceDataStepRate is {1}, but element.VertexLayoutClassification is {2}", i, element.InstanceDataStepRate, element.VertexLayoutClassification));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("element.VertexLayoutClassification");
                }

                var currentVariable = vertexShader.Reflection.Input[currentVariableIndex];
                if (currentVariable.Type.Class == ShaderValueTypeClass.Array && ((ShaderValueArrayType)currentVariable.Type).Length > currentVariableElementIndex + 1)
                {
                    currentVariableElementIndex++;
                }
                else
                {
                    currentVariableIndex++;
                    currentVariableElementIndex = 0;
                }

                if (!adapter.GetSupportedVertexElementFormats().Contains(element.Format))
                    throw new ArgumentException(string.Format("element[{0}].Format '{1}' is not supported by the given adapter", i, element.Format));
                if (!TypeIsCompatible(element.Format, currentVariable.BaseType))
                    throw new ArgumentException(string.Format("element[{0}].Format '{1}' is not compatible with the input variable base type '{2}", i, element.Format, currentVariable.BaseType));
            }
        }

        static bool TypeIsCompatible(ExplicitFormat format, ShaderValueBaseType valueBaseType)
        {
            bool isCompatible = false;
            switch (format)
            {
                case ExplicitFormat.R32G32B32A32_FLOAT:
                case ExplicitFormat.R16G16B16A16_FLOAT:
                case ExplicitFormat.R16G16B16A16_UNORM:
                case ExplicitFormat.R16G16B16A16_SNORM:
                case ExplicitFormat.R32G32B32_FLOAT:
                case ExplicitFormat.R32G32_FLOAT:
                case ExplicitFormat.R10G10B10A2_UNORM:
                case ExplicitFormat.R11G11B10_FLOAT:
                case ExplicitFormat.R8G8B8A8_UNORM:
                case ExplicitFormat.R8G8B8A8_SNORM:
                case ExplicitFormat.R16G16_FLOAT:
                case ExplicitFormat.R16G16_UNORM:
                case ExplicitFormat.R16G16_SNORM:
                case ExplicitFormat.R32_FLOAT:
                case ExplicitFormat.R8G8_UNORM:
                case ExplicitFormat.R8G8_SNORM:
                case ExplicitFormat.R16_FLOAT:
                case ExplicitFormat.R16_UNORM:
                case ExplicitFormat.R16_SNORM:
                case ExplicitFormat.R8_UNORM:
                case ExplicitFormat.R8_SNORM:
                case ExplicitFormat.B5G6R5_UNORM:
                case ExplicitFormat.B5G5R5A1_UNORM:
                case ExplicitFormat.B8G8R8A8_UNORM:
                case ExplicitFormat.B8G8R8X8_UNORM:
                    isCompatible = valueBaseType == ShaderValueBaseType.Float;
                    break;
                case ExplicitFormat.R32G32B32A32_UINT:
                case ExplicitFormat.R16G16B16A16_UINT:
                case ExplicitFormat.R32G32B32_UINT:
                case ExplicitFormat.R32G32_UINT:
                case ExplicitFormat.R10G10B10A2_UINT:
                case ExplicitFormat.R8G8B8A8_UINT:
                case ExplicitFormat.R16G16_UINT:
                case ExplicitFormat.R32_UINT:
                case ExplicitFormat.R8G8_UINT:
                case ExplicitFormat.R16_UINT:
                case ExplicitFormat.R8_UINT:
                    isCompatible = valueBaseType == ShaderValueBaseType.UInt;
                    break;
                case ExplicitFormat.R32G32B32A32_SINT:
                case ExplicitFormat.R16G16B16A16_SINT:
                case ExplicitFormat.R32G32B32_SINT:
                case ExplicitFormat.R32G32_SINT:
                case ExplicitFormat.R8G8B8A8_SINT:
                case ExplicitFormat.R16G16_SINT:
                case ExplicitFormat.R32_SINT:
                case ExplicitFormat.R8G8_SINT:
                case ExplicitFormat.R16_SINT:
                case ExplicitFormat.R8_SINT:
                    isCompatible = valueBaseType == ShaderValueBaseType.Int;
                    break;
            }

            return isCompatible;
        }
    }
}
