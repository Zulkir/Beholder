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
using Beholder.Math;
using Beholder.Resources;

namespace Beholder.Basic
{
    public class MeshFactory
    {
        protected enum MeshType
        {
            Billboard,
            Quad,
            Box,
            Sphere,
            Graph,
        }

        protected class MeshDescription : IEquatable<MeshDescription>
        {
            readonly MeshType type;
            readonly bool isRoom;
            readonly float[] parameters;
            readonly int hash;

            public MeshDescription(MeshType type, bool isRoom, params float[] parameters)
            {
                this.type = type;
                this.isRoom = isRoom;
                this.parameters = parameters;
                hash = isRoom ? 17 : 19;
                for (int i = 0; i < parameters.Length; i++)
                    hash = hash * 57 + parameters[i].GetHashCode();
            }

            public bool Equals(MeshDescription other)
            {
                if ((other == null) || (other.type != type) || (other.hash != hash) || (other.parameters.Length != parameters.Length) || (isRoom ^ other.isRoom))
                    return false;
                for (int i = 0; i < parameters.Length; i++)
                    if (parameters[i] != other.parameters[i]) return false;
                return true;
            }

            public override bool Equals(object obj)
            {
                return Equals(obj as MeshDescription);
            }

            public override int GetHashCode()
            {
                return hash;
            }
        }

        readonly IDevice device;
        readonly Handedness handedness;
        readonly Winding frontFace;
        readonly Dictionary<MeshDescription, Mesh> meshes;

        int counter = 0;

        public MeshFactory(IDevice device, Handedness handedness, Winding frontFace)
        {
            this.device = device;
            this.handedness = handedness;
            this.frontFace = frontFace;
            meshes = new Dictionary<MeshDescription, Mesh>();
        }

        public Mesh CreateBillboardYZ(float y, float z, float width, float height)
        {
            Mesh mesh;
            var desc = new MeshDescription(MeshType.Billboard, false, y, z, width, height);

            if (!meshes.TryGetValue(desc, out mesh))
            {
                float w = 0.5f * width;

                var vertexArray = new[]
                {
                    new VertexPositionNormalTexture(0.0f, -w, height, 1.0f, 0.0f, 0.0f, 0.0f, 0.0f),
                    new VertexPositionNormalTexture(0.0f, w, height, 1.0f, 0.0f, 0.0f, 1.0f, 0.0f),
                    new VertexPositionNormalTexture(0.0f, w, 0.0f, 1.0f, 0.0f, 0.0f, 1.0f, 1.0f),
                    new VertexPositionNormalTexture(0.0f, -w, 0.0f, 1.0f, 0.0f, 0.0f, 0.0f, 1.0f)
                };

                bool directIndexation =
                    (handedness == Handedness.Right && frontFace == Winding.Clockwise) ||
                    (handedness == Handedness.Left && frontFace == Winding.CounterClockwise);

                var indexArray = directIndexation ? new ushort[] { 0, 1, 2, 0, 2, 3 } : new ushort[] { 0, 2, 1, 0, 3, 2 };

                var vertexBuffer = device.Create.Buffer(new BufferDescription
                {
                    SizeInBytes = vertexArray.Length * VertexPositionNormalTexture.SizeInBytes,
                    Usage = Usage.Immutable,
                    BindFlags = BindFlags.VertexBuffer,
                    MiscFlags = MiscFlags.None,
                    ExtraFlags = ExtraFlags.None,
                    StructureByteStride = 0
                }, new SubresourceData(vertexArray));

                var indexBuffer = device.Create.Buffer(new BufferDescription
                {
                    SizeInBytes = indexArray.Length * sizeof(ushort),
                    Usage = Usage.Immutable,
                    BindFlags = BindFlags.IndexBuffer,
                    MiscFlags = MiscFlags.None,
                    ExtraFlags = ExtraFlags.SixteenBitIndices,
                    StructureByteStride = 0
                }, new SubresourceData(indexArray));

                counter++;
                mesh = new Mesh(new VertexSource(vertexBuffer, 0, VertexPositionNormalTexture.SizeInBytes), new IndexSource(indexBuffer, 0, IndexFormat.SixteenBit), new MeshSubset(6, 0, 0));
                meshes.Add(desc, mesh);
            }
            return mesh;
        }

        public Mesh CreateBox(bool isRoom, float width, float height, float depth, float uFrequency, float vFrequency, float uOffset, float vOffset)
        {
            Mesh mesh;
            var desc = new MeshDescription(MeshType.Box, isRoom, width, height, depth, uFrequency, vFrequency, uOffset, vOffset);

            if (!meshes.TryGetValue(desc, out mesh))
            {
                float w = 0.5f * width;
                float h = 0.5f * height;
                float d = 0.5f * depth;

                float u0 = isRoom ? 1.0f - uOffset : uOffset;
                float v0 = vOffset;
                float u1 = isRoom ? 1.0f - (uOffset + uFrequency) : uOffset + uFrequency;
                float v1 = vOffset + vFrequency;

                float n = isRoom ? -1.0f : 1.0f;

                var vertexArray = new VertexPositionNormalTexture[24];

                vertexArray[0] = new VertexPositionNormalTexture(w, -h, d, n, 0.0f, 0.0f, u0, v0);
                vertexArray[1] = new VertexPositionNormalTexture(w, h, d, n, 0.0f, 0.0f, u1, v0);
                vertexArray[2] = new VertexPositionNormalTexture(w, h, -d, n, 0.0f, 0.0f, u1, v1);
                vertexArray[3] = new VertexPositionNormalTexture(w, -h, -d, n, 0.0f, 0.0f, u0, v1);

                vertexArray[4] = new VertexPositionNormalTexture(w, h, d, 0.0f, n, 0.0f, u0, v0);
                vertexArray[5] = new VertexPositionNormalTexture(-w, h, d, 0.0f, n, 0.0f, u1, v0);
                vertexArray[6] = new VertexPositionNormalTexture(-w, h, -d, 0.0f, n, 0.0f, u1, v1);
                vertexArray[7] = new VertexPositionNormalTexture(w, h, -d, 0.0f, n, 0.0f, u0, v1);

                vertexArray[8] = new VertexPositionNormalTexture(-w, h, d, -n, 0.0f, 0.0f, u0, v0);
                vertexArray[9] = new VertexPositionNormalTexture(-w, -h, d, -n, 0.0f, 0.0f, u1, v0);
                vertexArray[10] = new VertexPositionNormalTexture(-w, -h, -d, -n, 0.0f, 0.0f, u1, v1);
                vertexArray[11] = new VertexPositionNormalTexture(-w, h, -d, -n, 0.0f, 0.0f, u0, v1);

                vertexArray[12] = new VertexPositionNormalTexture(-w, -h, d, 0.0f, -n, 0.0f, u0, v0);
                vertexArray[13] = new VertexPositionNormalTexture(w, -h, d, 0.0f, -n, 0.0f, u1, v0);
                vertexArray[14] = new VertexPositionNormalTexture(w, -h, -d, 0.0f, -n, 0.0f, u1, v1);
                vertexArray[15] = new VertexPositionNormalTexture(-w, -h, -d, 0.0f, -n, 0.0f, u0, v1);

                vertexArray[16] = new VertexPositionNormalTexture(-w, -h, d, 0.0f, 0.0f, n, u0, v0);
                vertexArray[17] = new VertexPositionNormalTexture(-w, h, d, 0.0f, 0.0f, n, u1, v0);
                vertexArray[18] = new VertexPositionNormalTexture(w, h, d, 0.0f, 0.0f, n, u1, v1);
                vertexArray[19] = new VertexPositionNormalTexture(w, -h, d, 0.0f, 0.0f, n, u0, v1);

                vertexArray[20] = new VertexPositionNormalTexture(-w, h, -d, 0.0f, 0.0f, -n, u0, v0);
                vertexArray[21] = new VertexPositionNormalTexture(-w, -h, -d, 0.0f, 0.0f, -n, u1, v0);
                vertexArray[22] = new VertexPositionNormalTexture(w, -h, -d, 0.0f, 0.0f, -n, u1, v1);
                vertexArray[23] = new VertexPositionNormalTexture(w, h, -d, 0.0f, 0.0f, -n, u0, v1);

                var indexArray = new ushort[36];

                bool directIndexation =
                    (handedness == Handedness.Right && frontFace == Winding.Clockwise) ||
                    (handedness == Handedness.Left && frontFace == Winding.CounterClockwise);

                if (isRoom) directIndexation = !directIndexation;

                if (directIndexation)
                {
                    int p = 0;
                    for (int i = 0; i < 6; i++)
                    {
                        int iFourTimes = i * 4;
                        indexArray[p++] = (ushort)iFourTimes;
                        indexArray[p++] = (ushort)(iFourTimes + 1);
                        indexArray[p++] = (ushort)(iFourTimes + 2);
                        indexArray[p++] = (ushort)(iFourTimes);
                        indexArray[p++] = (ushort)(iFourTimes + 2);
                        indexArray[p++] = (ushort)(iFourTimes + 3);
                    }
                }
                else
                {
                    int p = 0;
                    for (int i = 0; i < 6; i++)
                    {
                        int iFourTimes = i * 4;
                        indexArray[p++] = (ushort)(iFourTimes);
                        indexArray[p++] = (ushort)(iFourTimes + 2);
                        indexArray[p++] = (ushort)(iFourTimes + 1);
                        indexArray[p++] = (ushort)(iFourTimes);
                        indexArray[p++] = (ushort)(iFourTimes + 3);
                        indexArray[p++] = (ushort)(iFourTimes + 2);
                    }
                }

                var vertexBuffer = device.Create.Buffer(new BufferDescription
                {
                    SizeInBytes = vertexArray.Length * VertexPositionNormalTexture.SizeInBytes,
                    Usage = Usage.Immutable,
                    BindFlags = BindFlags.VertexBuffer,
                    MiscFlags = MiscFlags.None,
                    ExtraFlags = ExtraFlags.None,
                    StructureByteStride = 0
                }, new SubresourceData(vertexArray));

                var indexBuffer = device.Create.Buffer(new BufferDescription
                {
                    SizeInBytes = indexArray.Length * sizeof(ushort),
                    Usage = Usage.Immutable,
                    BindFlags = BindFlags.IndexBuffer,
                    MiscFlags = MiscFlags.None,
                    ExtraFlags = ExtraFlags.SixteenBitIndices,
                    StructureByteStride = 0
                }, new SubresourceData(indexArray));

                counter++;
                mesh = new Mesh(new VertexSource(vertexBuffer, 0, VertexPositionNormalTexture.SizeInBytes), new IndexSource(indexBuffer, 0, IndexFormat.SixteenBit), new MeshSubset(36, 0, 0));
                meshes.Add(desc, mesh);
            }
            return mesh;
        }

        public Mesh CreateCube(float size)
        {
            return CreateBox(false, size, size, size, 1.0f, 1.0f, 0.0f, 0.0f);
        }

        public Mesh CreateSphere(bool isRoom, float radius, int segments)
        {
            Mesh mesh;
            var desc = new MeshDescription(MeshType.Sphere, isRoom, radius, segments);

            if (!meshes.TryGetValue(desc, out mesh))
            {
                int uVerts = segments;
                int vVerts = segments / 2 + 1;
                float nCoef = isRoom ? -1.0f : 1.0f;

                int numVerts = uVerts * vVerts;

                var vertexArray = new VertexPositionNormalTexture[numVerts];

                int p = 0;
                for (int v = 0; v < vVerts; v++)
                {
                    for (int u = 0; u < uVerts; u++)
                    {
                        var texCoord = new Vector2
                        {
                            X = (float)u / (uVerts - 1),
                            Y = (float)v / (vVerts - 1)
                        };
                        float phi = 2.0f * MathHelper.Pi * texCoord.X;
                        float psi = MathHelper.Pi * texCoord.Y;
                        float sinPsi = MathHelper.Sin(psi);
                        var dir = new Vector3
                        {
                            X = MathHelper.Sin(phi) * sinPsi,
                            Y = MathHelper.Cos(psi),
                            Z = MathHelper.Cos(phi) * sinPsi
                        };
                        Vector3.Multiply(ref dir, radius, out vertexArray[p].Position);
                        Vector3.Multiply(ref dir, nCoef, out vertexArray[p].Normal);
                        vertexArray[p].TexCoord = texCoord;
                        
                        p++;
                    }
                }

                IBuffer vertexBuffer = device.Create.Buffer(new BufferDescription
                {
                    SizeInBytes = vertexArray.Length * VertexPositionNormalTexture.SizeInBytes,
                    Usage = Usage.Immutable,
                    BindFlags = BindFlags.VertexBuffer,
                    MiscFlags = MiscFlags.None,
                    ExtraFlags = ExtraFlags.None,
                    StructureByteStride = 0
                }, new SubresourceData(vertexArray));

                int numSquares = uVerts * (vVerts - 1);
                int numIndices = numSquares * 6;

                IBuffer indexBuffer;
                IndexFormat indexFormat;

                bool directIndexation =
                    (handedness == Handedness.Right && frontFace == Winding.Clockwise) ||
                    (handedness == Handedness.Left && frontFace == Winding.CounterClockwise);

                if (isRoom) directIndexation = !directIndexation;

                if (numVerts < 65536)
                {
                    var indexArray = new ushort[numIndices];
                    indexFormat = IndexFormat.SixteenBit;

                    if (directIndexation)
                    {
                        int q = 0;
                        for (int i = 0; i < numSquares; i++)
                        {
                            int secondIndex = i + 1 - uVerts * ((i + 1) / uVerts - i / uVerts);
                            indexArray[q++] = (ushort)i;
                            indexArray[q++] = (ushort)(secondIndex);
                            indexArray[q++] = (ushort)(i + uVerts);
                            indexArray[q++] = (ushort)(i + uVerts);
                            indexArray[q++] = (ushort)(secondIndex);
                            indexArray[q++] = (ushort)(secondIndex + uVerts);
                        }
                    }
                    else
                    {
                        int q = 0;
                        for (int i = 0; i < numSquares; i++)
                        {
                            int secondIndex = i + 1 - uVerts * ((i + 1) / uVerts - i / uVerts);
                            indexArray[q++] = (ushort)i;
                            indexArray[q++] = (ushort)(i + uVerts);
                            indexArray[q++] = (ushort)(secondIndex);
                            indexArray[q++] = (ushort)(i + uVerts);
                            indexArray[q++] = (ushort)(secondIndex + uVerts);
                            indexArray[q++] = (ushort)(secondIndex);
                        }
                    }

                    indexBuffer = device.Create.Buffer(new BufferDescription
                    {
                        SizeInBytes = indexArray.Length * sizeof(ushort),
                        Usage = Usage.Immutable,
                        BindFlags = BindFlags.IndexBuffer,
                        MiscFlags = MiscFlags.None,
                        ExtraFlags = ExtraFlags.SixteenBitIndices,
                        StructureByteStride = 0
                    }, new SubresourceData(indexArray));
                }
                else
                {
                    var indexArray = new int[numIndices];
                    indexFormat = IndexFormat.ThirtyTwoBit;

                    if (directIndexation)
                    {
                        int q = 0;
                        for (int i = 0; i < numSquares; i++)
                        {
                            int secondIndex = i + 1 - uVerts * ((i + 1) / uVerts - i / uVerts);
                            indexArray[q++] = i;
                            indexArray[q++] = secondIndex;
                            indexArray[q++] = i + uVerts;
                            indexArray[q++] = i + uVerts;
                            indexArray[q++] = secondIndex;
                            indexArray[q++] = secondIndex + uVerts;
                        }
                    }
                    else
                    {
                        int q = 0;
                        for (int i = 0; i < numSquares; i++)
                        {
                            int secondIndex = i + 1 - uVerts * ((i + 1) / uVerts - i / uVerts);
                            indexArray[q++] = i;
                            indexArray[q++] = i + uVerts;
                            indexArray[q++] = secondIndex;
                            indexArray[q++] = i + uVerts;
                            indexArray[q++] = secondIndex + uVerts;
                            indexArray[q++] = secondIndex;
                        }
                    }

                    indexBuffer = device.Create.Buffer(new BufferDescription
                    {
                        SizeInBytes = indexArray.Length * sizeof(int),
                        Usage = Usage.Immutable,
                        BindFlags = BindFlags.IndexBuffer,
                        MiscFlags = MiscFlags.None,
                        ExtraFlags = ExtraFlags.None,
                        StructureByteStride = 0
                    }, new SubresourceData(indexArray));
                }

                counter++;

                mesh = new Mesh(new VertexSource(vertexBuffer, 0, VertexPositionNormalTexture.SizeInBytes), new IndexSource(indexBuffer, 0, indexFormat), new MeshSubset(numSquares * 6, 0, 0));

                meshes.Add(desc, mesh);
            }
            return mesh;
        }

        public Mesh CreateGraphXY(Func<float, float, float> function, Rectangle definitionArea, float maxTrinagleSize, Vector2 textureTileSize)
        {
            Mesh mesh;
            var desc = new MeshDescription(MeshType.Graph, false, counter);

            if (!meshes.TryGetValue(desc, out mesh))
            {

                int wn = (int)(definitionArea.Width / maxTrinagleSize) + 1;
                int hn = (int)(definitionArea.Height / maxTrinagleSize) + 1;

                const float h = 1e-3f;

                var triSize = new Vector2(definitionArea.Width / wn, definitionArea.Height / hn);

                int wVerts = wn + 1;
                int hVerts = hn + 1;
                var vertexArray = new VertexPositionNormalTexture[wVerts * hVerts];

                var position = new Vector3();
                var tangent = new Vector3(1, 0, 0);
                var binormal = new Vector3(0, 1, 0);

                int p = 0;
                for (int i = 0; i < wVerts; i++)
                {
                    for (int j = 0; j < hVerts; j++)
                    {
                        position.X = definitionArea.X + j * triSize.X;
                        position.Y = definitionArea.Y + i * triSize.Y;
                        position.Z = function(position.X, position.Y);
                        vertexArray[p].Position = position;
                        tangent.Z = (function(position.X + h, position.Y) - function(position.X - h, position.Y)) /
                                    (2.0f * h);
                        binormal.Z = (function(position.X, position.Y + h) - function(position.X, position.Y - h)) /
                                     (2.0f * h);
                        Vector3.Cross(ref tangent, ref binormal, out vertexArray[p].Normal);
                        Vector3.NormalizeInPlace(ref vertexArray[p].Normal);
                        vertexArray[p].TexCoord.X = (position.X - definitionArea.X) / textureTileSize.X;
                        vertexArray[p].TexCoord.Y = (position.Y - definitionArea.Y) / textureTileSize.Y;

                        p++;
                    }
                }

                IBuffer vertexBuffer = device.Create.Buffer(new BufferDescription
                {
                    SizeInBytes = vertexArray.Length * VertexPositionNormalTexture.SizeInBytes,
                    Usage = Usage.Immutable,
                    BindFlags = BindFlags.VertexBuffer,
                    MiscFlags = MiscFlags.None,
                    ExtraFlags = ExtraFlags.None,
                    StructureByteStride = 0
                }, new SubresourceData(vertexArray));

                int numSquares = wn * hn;

                bool directIndexation =
                    (handedness == Handedness.Left && frontFace == Winding.Clockwise) ||
                    (handedness == Handedness.Right && frontFace == Winding.CounterClockwise);

                IBuffer indexBuffer;
                IndexFormat indexFormat;
                if (wVerts * hVerts < 65536)
                {
                    var indexArray = new ushort[numSquares * 6];
                    indexFormat = IndexFormat.SixteenBit;

                    int q = 0;
                    if (directIndexation)
                    {
                        for (int i = 0; i < numSquares; i++)
                        {
                            int row = i / wn;
                            int col = i - wn * row;
                            int topleftIndex = row * wVerts + col;
                            indexArray[q++] = (ushort)topleftIndex;
                            indexArray[q++] = (ushort)(topleftIndex + 1);
                            indexArray[q++] = (ushort)(topleftIndex + wVerts);
                            indexArray[q++] = (ushort)(topleftIndex + wVerts);
                            indexArray[q++] = (ushort)(topleftIndex + 1);
                            indexArray[q++] = (ushort)(topleftIndex + wVerts + 1);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < numSquares; i++)
                        {
                            int row = i / wn;
                            int col = i - wn * row;
                            int topleftIndex = row * wVerts + col;
                            indexArray[q++] = (ushort)topleftIndex;
                            indexArray[q++] = (ushort)(topleftIndex + wVerts);
                            indexArray[q++] = (ushort)(topleftIndex + 1);
                            indexArray[q++] = (ushort)(topleftIndex + wVerts);
                            indexArray[q++] = (ushort)(topleftIndex + wVerts + 1);
                            indexArray[q++] = (ushort)(topleftIndex + 1);
                        }
                    }

                    indexBuffer = device.Create.Buffer(new BufferDescription
                    {
                        SizeInBytes = indexArray.Length * sizeof(ushort),
                        Usage = Usage.Immutable,
                        BindFlags = BindFlags.IndexBuffer,
                        MiscFlags = MiscFlags.None,
                        ExtraFlags = ExtraFlags.SixteenBitIndices,
                        StructureByteStride = 0
                    }, new SubresourceData(indexArray));
                }
                else
                {
                    var indexArray = new int[numSquares * 6];
                    indexFormat = IndexFormat.ThirtyTwoBit;

                    int q = 0;
                    if (directIndexation)
                    {
                        for (int i = 0; i < numSquares; i++)
                        {
                            int row = i / wn;
                            int col = i - wn * row;
                            int topleftIndex = row * wVerts + col;
                            indexArray[q++] = topleftIndex;
                            indexArray[q++] = topleftIndex + 1;
                            indexArray[q++] = topleftIndex + wVerts;
                            indexArray[q++] = topleftIndex + wVerts;
                            indexArray[q++] = topleftIndex + 1;
                            indexArray[q++] = topleftIndex + wVerts + 1;
                        }
                    }
                    else
                    {
                        for (int i = 0; i < numSquares; i++)
                        {
                            int row = i / wn;
                            int col = i - wn * row;
                            int topleftIndex = row * wVerts + col;
                            indexArray[q++] = topleftIndex;
                            indexArray[q++] = topleftIndex + wVerts;
                            indexArray[q++] = topleftIndex + 1;
                            indexArray[q++] = topleftIndex + wVerts;
                            indexArray[q++] = topleftIndex + wVerts + 1;
                            indexArray[q++] = topleftIndex + 1;
                        }
                    }

                    indexBuffer = device.Create.Buffer(new BufferDescription
                    {
                        SizeInBytes = indexArray.Length * sizeof(int),
                        Usage = Usage.Immutable,
                        BindFlags = BindFlags.IndexBuffer,
                        MiscFlags = MiscFlags.None,
                        ExtraFlags = ExtraFlags.None,
                        StructureByteStride = 0
                    }, new SubresourceData(indexArray));
                }

                counter++;
                mesh = new Mesh(new VertexSource(vertexBuffer, 0, VertexPositionNormalTexture.SizeInBytes), new IndexSource(indexBuffer, 0, indexFormat), new MeshSubset(numSquares * 6, 0, 0));

                meshes.Add(desc, mesh);
            }

            return mesh;
        }

        public Mesh CreateGraphXZ(Func<float, float, float> function, Rectangle definitionArea, float maxTrinagleSize, Vector2 textureTileSize)
        {
            Mesh mesh;
            var desc = new MeshDescription(MeshType.Graph, false, counter);

            if (!meshes.TryGetValue(desc, out mesh))
            {

                int wn = (int) (definitionArea.Width/maxTrinagleSize) + 1;
                int hn = (int) (definitionArea.Height/maxTrinagleSize) + 1;

                const float h = 1e-3f;

                var triSize = new Vector2(definitionArea.Width/wn, definitionArea.Height/hn);

                int wVerts = wn + 1;
                int hVerts = hn + 1;

                var vertexArray = new VertexPositionNormalTexture[wVerts*hVerts];

                var position = new Vector3();
                var tangent = new Vector3(1, 0, 0);
                var binormal = new Vector3(0, 0, 1);

                int p = 0;
                for (int i = 0; i < wVerts; i++)
                {
                    for (int j = 0; j < hVerts; j++)
                    {
                        position.X = definitionArea.X + j*triSize.X;
                        position.Z = definitionArea.Y + i*triSize.Y;
                        position.Y = function(position.X, position.Z);
                        vertexArray[p].Position = position;
                        tangent.Y = (function(position.X + h, position.Z) - function(position.X - h, position.Z))/
                                    (2.0f*h);
                        binormal.Y = (function(position.X, position.Z + h) - function(position.X, position.Z - h))/
                                     (2.0f*h);
                        Vector3.Cross(ref binormal, ref tangent, out vertexArray[p].Normal);
                        Vector3.NormalizeInPlace(ref vertexArray[p].Normal);
                        vertexArray[p].TexCoord.X = (position.X - definitionArea.X)/textureTileSize.X;
                        vertexArray[p].TexCoord.Y = (position.Z - definitionArea.Y)/textureTileSize.Y;

                        p++;
                    }
                }

                IBuffer vertexBuffer = device.Create.Buffer(new BufferDescription
                {
                    SizeInBytes = vertexArray.Length * VertexPositionNormalTexture.SizeInBytes,
                    Usage = Usage.Immutable,
                    BindFlags = BindFlags.VertexBuffer,
                    MiscFlags = MiscFlags.None,
                    ExtraFlags = ExtraFlags.None,
                    StructureByteStride = 0
                }, new SubresourceData(vertexArray));

                int numSquares = wn*hn;

                bool directIndexation =
                    (handedness == Handedness.Right && frontFace == Winding.Clockwise) ||
                    (handedness == Handedness.Left && frontFace == Winding.CounterClockwise);

                IBuffer indexBuffer;
                IndexFormat indexFormat;

                if (wVerts*hVerts < 65536)
                {
                    var indexArray = new ushort[numSquares*6];
                    indexFormat = IndexFormat.SixteenBit;

                    int q = 0;
                    if (directIndexation)
                    {
                        for (int i = 0; i < numSquares; i++)
                        {
                            int row = i/wn;
                            int col = i - wn*row;
                            int topleftIndex = row*wVerts + col;
                            indexArray[q++] = (ushort) topleftIndex;
                            indexArray[q++] = (ushort) (topleftIndex + 1);
                            indexArray[q++] = (ushort) (topleftIndex + wVerts);
                            indexArray[q++] = (ushort) (topleftIndex + wVerts);
                            indexArray[q++] = (ushort) (topleftIndex + 1);
                            indexArray[q++] = (ushort) (topleftIndex + wVerts + 1);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < numSquares; i++)
                        {
                            int row = i/wn;
                            int col = i - wn*row;
                            int topleftIndex = row*wVerts + col;
                            indexArray[q++] = (ushort) topleftIndex;
                            indexArray[q++] = (ushort) (topleftIndex + wVerts);
                            indexArray[q++] = (ushort) (topleftIndex + 1);
                            indexArray[q++] = (ushort) (topleftIndex + wVerts);
                            indexArray[q++] = (ushort) (topleftIndex + wVerts + 1);
                            indexArray[q++] = (ushort) (topleftIndex + 1);
                        }
                    }

                    indexBuffer = device.Create.Buffer(new BufferDescription
                    {
                        SizeInBytes = indexArray.Length * sizeof(ushort),
                        Usage = Usage.Immutable,
                        BindFlags = BindFlags.IndexBuffer,
                        MiscFlags = MiscFlags.None,
                        ExtraFlags = ExtraFlags.SixteenBitIndices,
                        StructureByteStride = 0
                    }, new SubresourceData(indexArray));
                }
                else
                {
                    var indexArray = new int[numSquares*6];
                    indexFormat = IndexFormat.ThirtyTwoBit;

                    int q = 0;
                    if (directIndexation)
                    {
                        for (int i = 0; i < numSquares; i++)
                        {
                            int row = i/wn;
                            int col = i - wn*row;
                            int topleftIndex = row*wVerts + col;
                            indexArray[q++] = topleftIndex;
                            indexArray[q++] = topleftIndex + 1;
                            indexArray[q++] = topleftIndex + wVerts;
                            indexArray[q++] = topleftIndex + wVerts;
                            indexArray[q++] = topleftIndex + 1;
                            indexArray[q++] = topleftIndex + wVerts + 1;
                        }
                    }
                    else
                    {
                        for (int i = 0; i < numSquares; i++)
                        {
                            int row = i/wn;
                            int col = i - wn*row;
                            int topleftIndex = row*wVerts + col;
                            indexArray[q++] = topleftIndex;
                            indexArray[q++] = topleftIndex + wVerts;
                            indexArray[q++] = topleftIndex + 1;
                            indexArray[q++] = topleftIndex + wVerts;
                            indexArray[q++] = topleftIndex + wVerts + 1;
                            indexArray[q++] = topleftIndex + 1;
                        }
                    }

                    indexBuffer = device.Create.Buffer(new BufferDescription
                    {
                        SizeInBytes = indexArray.Length * sizeof(int),
                        Usage = Usage.Immutable,
                        BindFlags = BindFlags.IndexBuffer,
                        MiscFlags = MiscFlags.None,
                        ExtraFlags = ExtraFlags.None,
                        StructureByteStride = 0
                    }, new SubresourceData(indexArray));
                }

                counter++;
                mesh = new Mesh(new VertexSource(vertexBuffer, 0, VertexPositionNormalTexture.SizeInBytes), new IndexSource(indexBuffer, 0, indexFormat), new MeshSubset(numSquares * 6, 0, 0));

                meshes.Add(desc, mesh);
            }

            return mesh;
        }
    }
}
