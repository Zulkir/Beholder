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

using Beholder.Shaders;
using Beholder.Shaders.Reflection;
using Beholder.Utility.Structures;
using NUnit.Framework;

namespace Beholder.Tests.Utility.ForImplementations.Shaders
{
    [TestFixture]
    public class ShaderParserTest
    {
        [Test]
        public void Meta()
        {
            var reflection = ShaderParser.Parse(@"
                %meta
                MyFirstParam = my_first_value
                MySecondParam = my_second_value
                MyThirdParam = my_third_value
                ");

            string value;
            Assert.That(reflection.TryGetMeta("MyFirstParam", out value), Is.True);
            Assert.That(value, Is.EqualTo("my_first_value"));

            Assert.That(reflection.TryGetMeta("MySecondParam", out value), Is.True);
            Assert.That(value, Is.EqualTo("my_second_value"));

            Assert.That(reflection.TryGetMeta("MyThirdParam", out value), Is.True);
            Assert.That(value, Is.EqualTo("my_third_value"));
        }

        [Test]
        public void MetaSeparated()
        {
            var reflection = ShaderParser.Parse(@"
                %meta
                MySeparatedParam = my separated value
                ");

            string value;
            Assert.That(reflection.TryGetMeta("MySeparatedParam", out value), Is.True);
            Assert.That(value, Is.EqualTo("my separated value"));
        }

        [Test]
        public void MetaNumerical()
        {
            var reflection = ShaderParser.Parse(@"
                %meta
                MyNumericalParam = 123.456
                ");

            string value;
            Assert.That(reflection.TryGetMeta("MyNumericalParam", out value), Is.True);
            Assert.That(value, Is.EqualTo("123.456"));
        }

        [Test]
        public void SpecialParametersEmpty()
        {
            var reflection = ShaderParser.Parse(@"
                %samplers
                sampler MySampler
                ");

            Assert.That(reflection.Samplers[0].Name, Is.EqualTo("MySampler"));
        }

        [Test]
        public void SpecialParametersSingle()
        {
            var reflection = ShaderParser.Parse(@"
                %samplers
                sampler MySampler : param1 = one
                ");

            string value;
            Assert.That(reflection.Samplers[0].TryGetSpecialParameter("param1", out value), Is.True);
            Assert.That(value, Is.EqualTo("one"));
        }

        [Test]
        public void SpecialParametersSingleCompact()
        {
            var reflection = ShaderParser.Parse(@"
                %samplers
                sampler MySampler:param1=one
                ");

            string value;
            Assert.That(reflection.Samplers[0].TryGetSpecialParameter("param1", out value), Is.True);
            Assert.That(value, Is.EqualTo("one"));
        }

        [Test]
        public void SpecialParametersMultiple()
        {
            var reflection = ShaderParser.Parse(@"
                %samplers
                sampler MySampler : param1 = one, param2 = two
                ");

            string value;
            Assert.That(reflection.Samplers[0].TryGetSpecialParameter("param1", out value), Is.True);
            Assert.That(value, Is.EqualTo("one"));
            Assert.That(reflection.Samplers[0].TryGetSpecialParameter("param2", out value), Is.True);
            Assert.That(value, Is.EqualTo("two"));
        }

        [Test]
        public void SpecialParametersMultipleCompact()
        {
            var reflection = ShaderParser.Parse(@"
                %samplers
                sampler MySampler:param1=one,param2=two
                ");

            string value;
            Assert.That(reflection.Samplers[0].TryGetSpecialParameter("param1", out value), Is.True);
            Assert.That(value, Is.EqualTo("one"));
            Assert.That(reflection.Samplers[0].TryGetSpecialParameter("param2", out value), Is.True);
            Assert.That(value, Is.EqualTo("two"));
        }

        [Test]
        public void SpecialParametersMultiword()
        {
            var reflection = ShaderParser.Parse(@"
                %samplers
                sampler MySampler : param1 = value of several words, param2 = some more owrds
                ");

            string value;
            Assert.That(reflection.Samplers[0].TryGetSpecialParameter("param1", out value), Is.True);
            Assert.That(value, Is.EqualTo("value of several words"));
            Assert.That(reflection.Samplers[0].TryGetSpecialParameter("param2", out value), Is.True);
            Assert.That(value, Is.EqualTo("some more owrds"));
        }

        [Test]
        public void SpecialParametersNumerical()
        {
            var reflection = ShaderParser.Parse(@"
                %samplers
                sampler MySampler : param1=123.456, param2=234.567
                ");

            string value;
            Assert.That(reflection.Samplers[0].TryGetSpecialParameter("param1", out value), Is.True);
            Assert.That(value, Is.EqualTo("123.456"));
            Assert.That(reflection.Samplers[0].TryGetSpecialParameter("param2", out value), Is.True);
            Assert.That(value, Is.EqualTo("234.567"));
        }

        [Test]
        public void SpecialParametersWithPercentSign()
        {
            var reflection = ShaderParser.Parse(@"
                %samplers
                sampler MySampler : param1=%one, param2=%two
                ");

            string value;
            Assert.That(reflection.Samplers[0].TryGetSpecialParameter("param1", out value), Is.True);
            Assert.That(value, Is.EqualTo("%one"));
            Assert.That(reflection.Samplers[0].TryGetSpecialParameter("param2", out value), Is.True);
            Assert.That(value, Is.EqualTo("%two"));
        }

        [Test]
        public void ValueTypes()
        {
            var reflection = ShaderParser.Parse(@"
                %structures 
                struct MyStruct
                float input0
                float3 input1
                float1x4 input2
                int2x4 input3
                uint2x3 input4
                snorm3x3 input5
                unorm4x3 input6
                double2x2 input7
                bool1x3 input8
                int input9[123]
                ");

            Assert.That(reflection.Structures[0].Elements[0].Type, Is.EqualTo(ShaderValueScalarType.Float));
            Assert.That(reflection.Structures[0].Elements[1].Type, Is.EqualTo(ShaderValueVectorType.Float3));
            Assert.That(reflection.Structures[0].Elements[2].Type, Is.EqualTo(ShaderValueMatrixType.Float1x4));
            Assert.That(reflection.Structures[0].Elements[3].Type, Is.EqualTo(ShaderValueMatrixType.Int2x4));
            Assert.That(reflection.Structures[0].Elements[4].Type, Is.EqualTo(ShaderValueMatrixType.UInt2x3));
            Assert.That(reflection.Structures[0].Elements[5].Type, Is.EqualTo(ShaderValueMatrixType.SNorm3x3));
            Assert.That(reflection.Structures[0].Elements[6].Type, Is.EqualTo(ShaderValueMatrixType.UNorm4x3));
            Assert.That(reflection.Structures[0].Elements[7].Type, Is.EqualTo(ShaderValueMatrixType.Double2x2));
            Assert.That(reflection.Structures[0].Elements[8].Type, Is.EqualTo(ShaderValueMatrixType.Bool1x3));
            Assert.That(reflection.Structures[0].Elements[9].Type, Is.EqualTo(new ShaderValueArrayType(ShaderValueScalarType.Int, 123)));
        }

        [Test]
        public void StructuresSimple()
        {
            var reflection = ShaderParser.Parse(@"
                %structures 
                struct MyStruct
                float field
                ");

            Assert.That(reflection.Structures.Count, Is.EqualTo(1));
            Assert.That(reflection.Structures[0].Name, Is.EqualTo("MyStruct"));
            Assert.That(reflection.Structures[0].Elements.Count, Is.EqualTo(1));
            Assert.That(reflection.Structures[0].Elements[0].Type, Is.EqualTo(ShaderValueScalarType.Float));
        }

        [Test]
        public void StructuresComplex()
        {
            var reflection = ShaderParser.Parse(@"
                %structures 
                struct MyStruct
                float3x4 field0
                int1x4 field1
                ");

            Assert.That(reflection.Structures.Count, Is.EqualTo(1));
            Assert.That(reflection.Structures[0].Name, Is.EqualTo("MyStruct"));
            Assert.That(reflection.Structures[0].Elements.Count, Is.EqualTo(2));
            Assert.That(reflection.Structures[0].Elements[0].Type, Is.EqualTo(ShaderValueMatrixType.Float3x4));
            Assert.That(reflection.Structures[0].Elements[1].Type, Is.EqualTo(ShaderValueMatrixType.Int1x4));
        }

        [Test]
        public void StructuresNested()
        {
            var reflection = ShaderParser.Parse(@"
                %structures 
                struct MyStruct
                    float field
                struct MyOtherStruct
                    int field
                    MyStruct structField
                ");

            Assert.That(reflection.Structures.Count, Is.EqualTo(2));

            Assert.That(reflection.Structures[0].Name, Is.EqualTo("MyStruct"));
            Assert.That(reflection.Structures[0].Elements.Count, Is.EqualTo(1));
            Assert.That(reflection.Structures[0].Elements[0].Type, Is.EqualTo(ShaderValueScalarType.Float));

            Assert.That(reflection.Structures[1].Name, Is.EqualTo("MyOtherStruct"));
            Assert.That(reflection.Structures[1].Elements.Count, Is.EqualTo(2));
            Assert.That(reflection.Structures[1].Elements[0].Type, Is.EqualTo(ShaderValueScalarType.Int));
            Assert.That(reflection.Structures[1].Elements[1].Type, Is.EqualTo(reflection.Structures[0]));
        }

        [Test]
        public void ObjectTypeClasses()
        {
            var reflection = ShaderParser.Parse(@"
                %srvs
                AppendStructuredBuffer <float4> var0
                Buffer <float4> var1
                ByteAddressBuffer <float4> var2
                ConsumeStructuredBuffer <float4> var3
                RWBuffer <float4> var4
                RWByteAddressBuffer <float4> var5
                RWStructuredBuffer <float4> var6
                RWTexture1D <float4> var7
                RWTexture1DArray <float4> var8
                RWTexture2D <float4> var9
                RWTexture2DArray <float4> var10
                RWTexture3D <float4> var11
                StructuredBuffer <float4> var12
                Texture1D <float4> var13
                Texture1DArray <float4> var14
                Texture2D <float4> var15
                Texture2DArray <float4> var16
                Texture2DMS <float4> var17
                Texture2DMSArray <float4> var18
                Texture3D <float4> var19
                ");

            Assert.That(reflection.Srvs[0].Type.Class, Is.EqualTo(ShaderObjectTypeClass.AppendStructuredBuffer));
            Assert.That(reflection.Srvs[1].Type.Class, Is.EqualTo(ShaderObjectTypeClass.Buffer));
            Assert.That(reflection.Srvs[2].Type.Class, Is.EqualTo(ShaderObjectTypeClass.ByteAddressBuffer));
            Assert.That(reflection.Srvs[3].Type.Class, Is.EqualTo(ShaderObjectTypeClass.ConsumeStructuredBuffer));
            Assert.That(reflection.Srvs[4].Type.Class, Is.EqualTo(ShaderObjectTypeClass.RWBuffer));
            Assert.That(reflection.Srvs[5].Type.Class, Is.EqualTo(ShaderObjectTypeClass.RWByteAddressBuffer));
            Assert.That(reflection.Srvs[6].Type.Class, Is.EqualTo(ShaderObjectTypeClass.RWStructuredBuffer));
            Assert.That(reflection.Srvs[7].Type.Class, Is.EqualTo(ShaderObjectTypeClass.RWTexture1D));
            Assert.That(reflection.Srvs[8].Type.Class, Is.EqualTo(ShaderObjectTypeClass.RWTexture1DArray));
            Assert.That(reflection.Srvs[9].Type.Class, Is.EqualTo(ShaderObjectTypeClass.RWTexture2D));
            Assert.That(reflection.Srvs[10].Type.Class, Is.EqualTo(ShaderObjectTypeClass.RWTexture2DArray));
            Assert.That(reflection.Srvs[11].Type.Class, Is.EqualTo(ShaderObjectTypeClass.RWTexture3D));
            Assert.That(reflection.Srvs[12].Type.Class, Is.EqualTo(ShaderObjectTypeClass.StructuredBuffer));
            Assert.That(reflection.Srvs[13].Type.Class, Is.EqualTo(ShaderObjectTypeClass.Texture1D));
            Assert.That(reflection.Srvs[14].Type.Class, Is.EqualTo(ShaderObjectTypeClass.Texture1DArray));
            Assert.That(reflection.Srvs[15].Type.Class, Is.EqualTo(ShaderObjectTypeClass.Texture2D));
            Assert.That(reflection.Srvs[16].Type.Class, Is.EqualTo(ShaderObjectTypeClass.Texture2DArray));
            Assert.That(reflection.Srvs[17].Type.Class, Is.EqualTo(ShaderObjectTypeClass.Texture2DMS));
            Assert.That(reflection.Srvs[18].Type.Class, Is.EqualTo(ShaderObjectTypeClass.Texture2DMSArray));
            Assert.That(reflection.Srvs[19].Type.Class, Is.EqualTo(ShaderObjectTypeClass.Texture3D));
        }

        [Test]
        public void ObjectTypes()
        {
            var reflection = ShaderParser.Parse(@"
                %srvs
                Texture2D<float4> var0
                Texture2D < float4 > var1
                Texture2DMS<float4,4> var2
                Texture2DMS < float4 , 4 > var3
                ");

            Assert.That(reflection.Srvs[0].Type.Class, Is.EqualTo(ShaderObjectTypeClass.Texture2D));
            Assert.That(reflection.Srvs[0].Type.ElementType, Is.EqualTo(ShaderValueVectorType.Float4));
            Assert.That(reflection.Srvs[0].Type.Samples, Is.EqualTo(null));

            Assert.That(reflection.Srvs[1].Type.Class, Is.EqualTo(ShaderObjectTypeClass.Texture2D));
            Assert.That(reflection.Srvs[1].Type.ElementType, Is.EqualTo(ShaderValueVectorType.Float4));
            Assert.That(reflection.Srvs[1].Type.Samples, Is.EqualTo(null));

            Assert.That(reflection.Srvs[2].Type.Class, Is.EqualTo(ShaderObjectTypeClass.Texture2DMS));
            Assert.That(reflection.Srvs[2].Type.ElementType, Is.EqualTo(ShaderValueVectorType.Float4));
            Assert.That(reflection.Srvs[2].Type.Samples, Is.EqualTo(4));

            Assert.That(reflection.Srvs[3].Type.Class, Is.EqualTo(ShaderObjectTypeClass.Texture2DMS));
            Assert.That(reflection.Srvs[3].Type.ElementType, Is.EqualTo(ShaderValueVectorType.Float4));
            Assert.That(reflection.Srvs[3].Type.Samples, Is.EqualTo(4));
        }

        [Test]
        public void FixedSampling()
        {
            var reflection = ShaderParser.Parse(@"
                %fixed_sampling
                TextureA : SamplerA
                TextureB : SamplerZ
                ");

            Assert.That(reflection.FixedSamplingPairs.Count, Is.EqualTo(2));
            Assert.That(reflection.FixedSamplingPairs[0].SrvName, Is.EqualTo("TextureA"));
            Assert.That(reflection.FixedSamplingPairs[0].SamplerName, Is.EqualTo("SamplerA"));
            Assert.That(reflection.FixedSamplingPairs[1].SrvName, Is.EqualTo("TextureB"));
            Assert.That(reflection.FixedSamplingPairs[1].SamplerName, Is.EqualTo("SamplerZ"));
        }

        [Test]
        public void Input()
        {
            var reflection = ShaderParser.Parse(@"
                %input
                float4 Position : p0 = zero
                float4x4 _Normal : p1 = one
                ");

            Assert.That(reflection.Input.Count, Is.EqualTo(2));
            
            Assert.That(reflection.Input[0].Name, Is.EqualTo("Position"));
            Assert.That(reflection.Input[1].Name, Is.EqualTo("_Normal"));

            Assert.That(reflection.Input[0].Type, Is.EqualTo(ShaderValueVectorType.Float4));
            Assert.That(reflection.Input[1].Type, Is.EqualTo(ShaderValueMatrixType.Float4x4));

            string value;
            Assert.That(reflection.Input[0].TryGetSpecialParameter("p0", out value), Is.True);
            Assert.That(value, Is.EqualTo("zero"));
            Assert.That(reflection.Input[1].TryGetSpecialParameter("p1", out value), Is.True);
            Assert.That(value, Is.EqualTo("one"));
        }

        [Test]
        public void InputExtra()
        {
            var reflection = ShaderParser.Parse(@"
                %input_extra
                int4 StrangePrimitiveID : p0 = zero
                int4x4 SomethingElse : p1 = one
                ");

            Assert.That(reflection.InputExtra.Count, Is.EqualTo(2));

            Assert.That(reflection.InputExtra[0].Name, Is.EqualTo("StrangePrimitiveID"));
            Assert.That(reflection.InputExtra[1].Name, Is.EqualTo("SomethingElse"));

            Assert.That(reflection.InputExtra[0].Type, Is.EqualTo(ShaderValueVectorType.Int4));
            Assert.That(reflection.InputExtra[1].Type, Is.EqualTo(ShaderValueMatrixType.Int4x4));

            string value;
            Assert.That(reflection.InputExtra[0].TryGetSpecialParameter("p0", out value), Is.True);
            Assert.That(value, Is.EqualTo("zero"));
            Assert.That(reflection.InputExtra[1].TryGetSpecialParameter("p1", out value), Is.True);
            Assert.That(value, Is.EqualTo("one"));
        }

        [Test]
        public void InputPatch()
        {
            var reflection = ShaderParser.Parse(@"
                %input_patch
                snorm4 TessSomething : p0 = zero
                snorm4x4 TessSomethingElse : p1 = one
                ");

            Assert.That(reflection.InputPatch.Count, Is.EqualTo(2));

            Assert.That(reflection.InputPatch[0].Name, Is.EqualTo("TessSomething"));
            Assert.That(reflection.InputPatch[1].Name, Is.EqualTo("TessSomethingElse"));

            Assert.That(reflection.InputPatch[0].Type, Is.EqualTo(ShaderValueVectorType.SNorm4));
            Assert.That(reflection.InputPatch[1].Type, Is.EqualTo(ShaderValueMatrixType.SNorm4x4));

            string value;
            Assert.That(reflection.InputPatch[0].TryGetSpecialParameter("p0", out value), Is.True);
            Assert.That(value, Is.EqualTo("zero"));
            Assert.That(reflection.InputPatch[1].TryGetSpecialParameter("p1", out value), Is.True);
            Assert.That(value, Is.EqualTo("one"));
        }

        [Test]
        public void InputPatchExtra()
        {
            var reflection = ShaderParser.Parse(@"
                %input_patch_extra
                double4 MyVar : p0 = zero
                double4x4 MyOtherBigDoubleVar : p1 = one
                ");

            Assert.That(reflection.InputPatchExtra.Count, Is.EqualTo(2));

            Assert.That(reflection.InputPatchExtra[0].Name, Is.EqualTo("MyVar"));
            Assert.That(reflection.InputPatchExtra[1].Name, Is.EqualTo("MyOtherBigDoubleVar"));

            Assert.That(reflection.InputPatchExtra[0].Type, Is.EqualTo(ShaderValueVectorType.Double4));
            Assert.That(reflection.InputPatchExtra[1].Type, Is.EqualTo(ShaderValueMatrixType.Double4x4));

            string value;
            Assert.That(reflection.InputPatchExtra[0].TryGetSpecialParameter("p0", out value), Is.True);
            Assert.That(value, Is.EqualTo("zero"));
            Assert.That(reflection.InputPatchExtra[1].TryGetSpecialParameter("p1", out value), Is.True);
            Assert.That(value, Is.EqualTo("one"));
        }

        [Test]
        public void Output()
        {
            var reflection = ShaderParser.Parse(@"
                %output
                float4 Position : p0 = zero
                float4x4 Normal : p1 = one
                ");

            Assert.That(reflection.Output.Count, Is.EqualTo(2));

            Assert.That(reflection.Output[0].Name, Is.EqualTo("Position"));
            Assert.That(reflection.Output[1].Name, Is.EqualTo("Normal"));

            Assert.That(reflection.Output[0].Type, Is.EqualTo(ShaderValueVectorType.Float4));
            Assert.That(reflection.Output[1].Type, Is.EqualTo(ShaderValueMatrixType.Float4x4));

            string value;
            Assert.That(reflection.Output[0].TryGetSpecialParameter("p0", out value), Is.True);
            Assert.That(value, Is.EqualTo("zero"));
            Assert.That(reflection.Output[1].TryGetSpecialParameter("p1", out value), Is.True);
            Assert.That(value, Is.EqualTo("one"));
        }

        [Test]
        public void OutputStream()
        {
            var reflection = ShaderParser.Parse(@"
                %output_streams
                stream
	                float4 Position : p0 = zero
	                float2 TexCoord : p1 = one
                stream
	                float3 Normal : p2 = two
	                float3 Color : p3 = three
	                uint PrimitiveID : p4 = four
                ");

            string value;

            Assert.That(reflection.OutputStreams.Count, Is.EqualTo(2));

            Assert.That(reflection.OutputStreams[0].Count, Is.EqualTo(2));
            Assert.That(reflection.OutputStreams[0][0].Name, Is.EqualTo("Position"));
            Assert.That(reflection.OutputStreams[0][1].Name, Is.EqualTo("TexCoord"));
            Assert.That(reflection.OutputStreams[0][0].Type, Is.EqualTo(ShaderValueVectorType.Float4));
            Assert.That(reflection.OutputStreams[0][1].Type, Is.EqualTo(ShaderValueVectorType.Float2));
            Assert.That(reflection.OutputStreams[0][0].TryGetSpecialParameter("p0", out value), Is.True);
            Assert.That(value, Is.EqualTo("zero"));
            Assert.That(reflection.OutputStreams[0][1].TryGetSpecialParameter("p1", out value), Is.True);
            Assert.That(value, Is.EqualTo("one"));

            Assert.That(reflection.OutputStreams[1].Count, Is.EqualTo(3));
            Assert.That(reflection.OutputStreams[1][0].Name, Is.EqualTo("Normal"));
            Assert.That(reflection.OutputStreams[1][1].Name, Is.EqualTo("Color"));
            Assert.That(reflection.OutputStreams[1][2].Name, Is.EqualTo("PrimitiveID"));
            Assert.That(reflection.OutputStreams[1][0].Type, Is.EqualTo(ShaderValueVectorType.Float3));
            Assert.That(reflection.OutputStreams[1][1].Type, Is.EqualTo(ShaderValueVectorType.Float3));
            Assert.That(reflection.OutputStreams[1][2].Type, Is.EqualTo(ShaderValueScalarType.UInt));
            Assert.That(reflection.OutputStreams[1][0].TryGetSpecialParameter("p2", out value), Is.True);
            Assert.That(value, Is.EqualTo("two"));
            Assert.That(reflection.OutputStreams[1][1].TryGetSpecialParameter("p3", out value), Is.True);
            Assert.That(value, Is.EqualTo("three"));
            Assert.That(reflection.OutputStreams[1][2].TryGetSpecialParameter("p4", out value), Is.True);
            Assert.That(value, Is.EqualTo("four"));
        }

        [Test]
        public void OutputPatch()
        {
            var reflection = ShaderParser.Parse(@"
                %output_patch
                float4 Position : p0 = zero
                float4x4 Normal : p1 = one
                ");

            Assert.That(reflection.OutputPatch.Count, Is.EqualTo(2));

            Assert.That(reflection.OutputPatch[0].Name, Is.EqualTo("Position"));
            Assert.That(reflection.OutputPatch[1].Name, Is.EqualTo("Normal"));

            Assert.That(reflection.OutputPatch[0].Type, Is.EqualTo(ShaderValueVectorType.Float4));
            Assert.That(reflection.OutputPatch[1].Type, Is.EqualTo(ShaderValueMatrixType.Float4x4));

            string value;
            Assert.That(reflection.OutputPatch[0].TryGetSpecialParameter("p0", out value), Is.True);
            Assert.That(value, Is.EqualTo("zero"));
            Assert.That(reflection.OutputPatch[1].TryGetSpecialParameter("p1", out value), Is.True);
            Assert.That(value, Is.EqualTo("one"));
        }

        [Test]
        public void Samplers()
        {
            var reflection = ShaderParser.Parse(@"
                %samplers
                    sampler var0 : p0 = zero 
                    sampler var1 : p1 = one 
                ");
            
            Assert.That(reflection.Samplers.Count, Is.EqualTo(2));
            Assert.That(reflection.Samplers[0].Name, Is.EqualTo("var0"));
            Assert.That(reflection.Samplers[1].Name, Is.EqualTo("var1"));

            string value;
            Assert.That(reflection.Samplers[0].TryGetSpecialParameter("p0", out value), Is.True);
            Assert.That(value, Is.EqualTo("zero"));
            Assert.That(reflection.Samplers[1].TryGetSpecialParameter("p1", out value), Is.True);
            Assert.That(value, Is.EqualTo("one"));
        }

        [Test]
        public void Srvs()
        {
            var reflection = ShaderParser.Parse(@"
                %srvs
                    Texture2DArray<uint3, 12> var0 : p0 = zero 
                    Buffer<snorm> var1 : p1 = one
                ");

            Assert.That(reflection.Srvs.Count, Is.EqualTo(2));
            Assert.That(reflection.Srvs[0].Name, Is.EqualTo("var0"));
            Assert.That(reflection.Srvs[1].Name, Is.EqualTo("var1"));

            Assert.That(reflection.Srvs[0].Type, Is.EqualTo(new ShaderObjectType(ShaderObjectTypeClass.Texture2DArray, ShaderValueVectorType.UInt3, 12)));
            Assert.That(reflection.Srvs[1].Type, Is.EqualTo(new ShaderObjectType(ShaderObjectTypeClass.Buffer, ShaderValueScalarType.SNorm)));

            string value;
            Assert.That(reflection.Srvs[0].TryGetSpecialParameter("p0", out value), Is.True);
            Assert.That(value, Is.EqualTo("zero"));
            Assert.That(reflection.Srvs[1].TryGetSpecialParameter("p1", out value), Is.True);
            Assert.That(value, Is.EqualTo("one"));
        }

        [Test]
        public void Uavs()
        {
            var reflection = ShaderParser.Parse(@"
                %uavs
                    RWTexture2DArray<uint3, 12> var0 : p0 = zero 
                    RWBuffer<snorm> var1 : p1 = one
                ");

            Assert.That(reflection.Uavs.Count, Is.EqualTo(2));
            Assert.That(reflection.Uavs[0].Name, Is.EqualTo("var0"));
            Assert.That(reflection.Uavs[1].Name, Is.EqualTo("var1"));

            Assert.That(reflection.Uavs[0].Type, Is.EqualTo(new ShaderObjectType(ShaderObjectTypeClass.RWTexture2DArray, ShaderValueVectorType.UInt3, 12)));
            Assert.That(reflection.Uavs[1].Type, Is.EqualTo(new ShaderObjectType(ShaderObjectTypeClass.RWBuffer, ShaderValueScalarType.SNorm)));

            string value;
            Assert.That(reflection.Uavs[0].TryGetSpecialParameter("p0", out value), Is.True);
            Assert.That(value, Is.EqualTo("zero"));
            Assert.That(reflection.Uavs[1].TryGetSpecialParameter("p1", out value), Is.True);
            Assert.That(value, Is.EqualTo("one"));
        }

        [Test]
        public void CodeGlobal()
        {
            var reflection = ShaderParser.Parse(@"
                %code_global
!@#$%^&*()_+|
    asd 123 sdkfj 902d i2jd 92k02jc2j f0i2 jc98eh c8eru n n32
       
        {:>{> > {> >{> :> {> {> {: >{ >: >:> > ");

            Assert.That(reflection.CodeGlobalLines.Count, Is.EqualTo(4));
            Assert.That(reflection.CodeGlobalLines[0], Is.EqualTo("!@#$%^&*()_+|"));
            Assert.That(reflection.CodeGlobalLines[1], Is.EqualTo("    asd 123 sdkfj 902d i2jd 92k02jc2j f0i2 jc98eh c8eru n n32"));
            Assert.That(reflection.CodeGlobalLines[2], Is.EqualTo(""));
            Assert.That(reflection.CodeGlobalLines[3], Is.EqualTo("        {:>{> > {> >{> :> {> {> {: >{ >: >:> >"));
        }

        [Test]
        public void CodeMain()
        {
            var reflection = ShaderParser.Parse(@"
                %code_main
!@#$%^&*()_+|
    asd 123 sdkfj 902d i2jd 92k02jc2j f0i2 jc98eh c8eru n n32
       
        {:>{> > {> >{> :> {> {> {: >{ >: >:> > ");

            Assert.That(reflection.CodeMainLines.Count, Is.EqualTo(4));
            Assert.That(reflection.CodeMainLines[0], Is.EqualTo("!@#$%^&*()_+|"));
            Assert.That(reflection.CodeMainLines[1], Is.EqualTo("    asd 123 sdkfj 902d i2jd 92k02jc2j f0i2 jc98eh c8eru n n32"));
            Assert.That(reflection.CodeMainLines[2], Is.EqualTo(""));
            Assert.That(reflection.CodeMainLines[3], Is.EqualTo("        {:>{> > {> >{> :> {> {> {: >{ >: >:> >"));
        }

        [Test]
        public void CodePatch()
        {
            var reflection = ShaderParser.Parse(@"
                %code_patch
!@#$%^&*()_+|
    asd 123 sdkfj 902d i2jd 92k02jc2j f0i2 jc98eh c8eru n n32
       
        {:>{> > {> >{> :> {> {> {: >{ >: >:> > ");

            Assert.That(reflection.CodePatchLines.Count, Is.EqualTo(4));
            Assert.That(reflection.CodePatchLines[0], Is.EqualTo("!@#$%^&*()_+|"));
            Assert.That(reflection.CodePatchLines[1], Is.EqualTo("    asd 123 sdkfj 902d i2jd 92k02jc2j f0i2 jc98eh c8eru n n32"));
            Assert.That(reflection.CodePatchLines[2], Is.EqualTo(""));
            Assert.That(reflection.CodePatchLines[3], Is.EqualTo("        {:>{> > {> >{> :> {> {> {: >{ >: >:> >"));
        }

        [Test]
        public void EverythingTogether()
        {
            var reflection = ShaderParser.Parse(@"
%meta
Name = OmniShader

%structures
struct SomeStruct
    float field0
    int field1

%ubuffers
ubuffer SomeUBuffer
    uint field0
    snorm field1

%samplers
sampler SamplerVar

%srvs
Texture2D SrvVar

%uavs
RWBuffer UavVar

%fixed_sampling
SrvVar : SamplerVar

%input
float InputVar

%input_extra
float2 InputExtraVar

%input_patch
float3x3 InputPatchVar

%input_patch_extra
int InputPatchExtraVar
                
%output
int3 OutputVar

%output_patch
int4x4 OutputPatchVar

%output_streams
stream
    unorm OutputStreamVar0
stream
    unorm4 OutputStreamVar1

%code_global
    global

    code
%code_main
    main

    code
%code_patch
    patch

    code");

            string value;
            Assert.That(reflection.TryGetMeta("Name", out value), Is.True);
            Assert.That(value, Is.EqualTo("OmniShader"));
            Assert.That(reflection.Structures[0], Is.EqualTo(new ShaderValueStructureType("SomeStruct", new[]
            {
                new Pair<ShaderValueType, string>(ShaderValueScalarType.Float, "field0"),
                new Pair<ShaderValueType, string>(ShaderValueScalarType.Int, "field1")
            })));
            Assert.That(reflection.UniformBuffers[0].Name, Is.EqualTo("SomeUBuffer"));
            Assert.That(reflection.Samplers[0].Name, Is.EqualTo("SamplerVar"));
            Assert.That(reflection.Srvs[0].Name, Is.EqualTo("SrvVar"));
            Assert.That(reflection.Uavs[0].Name, Is.EqualTo("UavVar"));
            Assert.That(reflection.FixedSamplingPairs[0].SrvName, Is.EqualTo("SrvVar"));
            Assert.That(reflection.FixedSamplingPairs[0].SamplerName, Is.EqualTo("SamplerVar"));
            Assert.That(reflection.Input[0].Name, Is.EqualTo("InputVar"));
            Assert.That(reflection.InputExtra[0].Name, Is.EqualTo("InputExtraVar"));
            Assert.That(reflection.InputPatch[0].Name, Is.EqualTo("InputPatchVar"));
            Assert.That(reflection.InputPatchExtra[0].Name, Is.EqualTo("InputPatchExtraVar"));
            Assert.That(reflection.Output[0].Name, Is.EqualTo("OutputVar"));
            Assert.That(reflection.OutputStreams[0][0].Name, Is.EqualTo("OutputStreamVar0"));
            Assert.That(reflection.OutputStreams[1][0].Name, Is.EqualTo("OutputStreamVar1"));
            CollectionAssert.AreEqual(reflection.CodeGlobalLines, new[] { "    global", "", "    code" });
            CollectionAssert.AreEqual(reflection.CodeMainLines, new[] { "    main", "", "    code" });
            CollectionAssert.AreEqual(reflection.CodePatchLines, new[] { "    patch", "", "    code" });
        }
    }
}