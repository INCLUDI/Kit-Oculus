using UnityEngine;
using System.Collections;
using YAProgressBar;
using System;

namespace YAProgressBar
{
    public class QuadMeshGenerator : BaseMeshGenerator
    {
        public override LinearProgressBar.ElementShape ElementShape
        {
            get
            {
                return LinearProgressBar.ElementShape.Quad;
            }
        }

        protected override int VertexCountPerElement
        {
            get
            {
                return 4;
            }
        }

        protected override int IndicesCountPerElement
        {
            get
            {
                return VertexCountPerElement + 2;
            }
        }

        public bool FlipNormal { get; set; }

        public override void GenerateLinearMesh(Mesh mesh, LinearProgressBar progressbarMesh)
        {
            FlipNormal = progressbarMesh.FlipNormal;

            int elementsCount = progressbarMesh.ElementsCount;
            float lenght = progressbarMesh.Length;
            float height = progressbarMesh.Height;
            float elementGap = progressbarMesh.ElementGap;
            float startX = progressbarMesh.StartX;
            float startY = progressbarMesh.StartY;
            float skew = progressbarMesh.Skew;

            Vector2 uvCoordLowerLeft = new Vector2(0, 0);
            Vector2 uvCoordUpperLeft = new Vector2(0, 1);
            Vector2 uvCoordUpperRight = new Vector2(1, 1);
            Vector2 uvCoordLowerRight = new Vector2(1, 0);

            if (0 < elementsCount && elementsCount < maxElementsCount && 0 < height && 0 < lenght && elementGap < lenght)
            {
                FillIndices(elementsCount);

                var normals = new Vector3[elementsCount * VertexCountPerElement];
                var vertices = new Vector3[elementsCount * VertexCountPerElement];
                var uv = new Vector2[elementsCount * VertexCountPerElement];

                float elementSide = (lenght - elementGap * (elementsCount - 1)) / elementsCount;

                int startVertexIndex = 0;
                float elementX = 0;
                for (int i = 0; i < elementsCount; i++)
                {
                    startVertexIndex = i * VertexCountPerElement;
                    vertices[startVertexIndex + 0] = new Vector3(startX * lenght + elementX, startY * height, 0);
                    vertices[startVertexIndex + 1] = new Vector3(startX * lenght + elementX + skew, startY * height + height, 0);
                    vertices[startVertexIndex + 2] = new Vector3(startX * lenght + elementX + elementSide + skew, startY * height + height, 0);
                    vertices[startVertexIndex + 3] = new Vector3(startX * lenght + elementX + elementSide, startY * height, 0);

                    normals[startVertexIndex + 0] = Vector3.back;
                    normals[startVertexIndex + 1] = Vector3.back;
                    normals[startVertexIndex + 2] = Vector3.back;
                    normals[startVertexIndex + 3] = Vector3.back;

                    uv[startVertexIndex + 0] = uvCoordLowerLeft;
                    uv[startVertexIndex + 1] = uvCoordUpperLeft;
                    uv[startVertexIndex + 2] = uvCoordUpperRight;
                    uv[startVertexIndex + 3] = uvCoordLowerRight;

                    elementX += elementSide + elementGap;
                }

                mesh.Clear();
                mesh.subMeshCount = 2;
                mesh.vertices = vertices;
                mesh.normals = normals;
                mesh.uv = uv;
                mesh.RecalculateBounds();

                this.SetMaterialDivider(mesh, progressbarMesh.FillAmount, progressbarMesh.Inverse, progressbarMesh.ElementsCount, false);
            }
        }

        public override void GenerateCircularMesh(Mesh mesh, CircularProgressBar progressbarMesh)
        {
            FlipNormal = progressbarMesh.FlipNormal;

            BaseCurve curve = progressbarMesh.Curve;
            if (curve != null)
            {
                int elementsCount = progressbarMesh.ElementsCount;
                float skew = progressbarMesh.Skew;
                //float height = progressbarMesh.Height;
                float elementGap = progressbarMesh.ElementGap;
                float depth = progressbarMesh.Depth;
                float curveLength = curve.ComputeLength();

                Vector2 uvCoordLowerLeft = new Vector2(0, 0);
                Vector2 uvCoordUpperLeft = new Vector2(0, 1);
                Vector2 uvCoordUpperRight = new Vector2(1, 1);
                Vector2 uvCoordLowerRight = new Vector2(1, 0);

                float elementSide;
                float relativeElementSide;
                float relativeElementGap;
                float relativeElementSkew;

                if (curve.IsClosed)
                {
                    elementSide = (curveLength - elementGap * (elementsCount)) / elementsCount;
                }
                else
                {
                    elementSide = (curveLength - elementGap * (elementsCount - 1)) / elementsCount;
                }

                relativeElementSide = elementSide / curveLength;
                relativeElementGap = elementGap / curveLength;
                relativeElementSkew = skew / curveLength;

                if (0 < elementSide && 0 < elementsCount && elementsCount < maxElementsCount)
                {
                    FillIndices(elementsCount);

                    var vertices = new Vector3[elementsCount * VertexCountPerElement];
                    var uv = new Vector2[elementsCount * VertexCountPerElement];
                    var normals = new Vector3[elementsCount * VertexCountPerElement];

                    int startVertexIndex = 0;
                    Vector3 pStart;
                    Vector3 pEnd;
                    Vector3 pStart2;
                    Vector3 pEnd2;
                    Vector3 normalStart;
                    Vector3 normaEnd;
                    Vector3 normalStart2;
                    Vector3 normaEnd2;
                    float tStart = 0;
                    float tEnd = tStart + relativeElementSide;

                    for (int i = 0; i < elementsCount; i++)
                    {
                        pStart = curve.GetPoint(tStart, out normalStart);
                        pEnd = curve.GetPoint(tEnd, out normaEnd);

                        if (0.00001f < Mathf.Abs(skew))
                        {
                            pStart2 = curve.GetPoint(tStart + relativeElementSkew, out normalStart2) + Vector3.forward * depth;
                            pEnd2 = curve.GetPoint(tEnd + relativeElementSkew, out normaEnd2) + Vector3.forward * depth;
                        }
                        else
                        {
                            pStart2 = pStart + Vector3.forward * depth;
                            pEnd2 = pEnd + Vector3.forward * depth;
                            normalStart2 = normalStart;
                            normaEnd2 = normaEnd;
                        }

                        startVertexIndex = i * VertexCountPerElement;
                        ///Front
                        vertices[startVertexIndex + 0] = pStart;
                        vertices[startVertexIndex + 1] = pEnd;
                        vertices[startVertexIndex + 2] = pEnd2;
                        vertices[startVertexIndex + 3] = pStart2;

                        normals[startVertexIndex + 0] = normalStart;
                        normals[startVertexIndex + 1] = normaEnd;
                        normals[startVertexIndex + 2] = normaEnd2;
                        normals[startVertexIndex + 3] = normalStart2;

                        uv[startVertexIndex + 0] = uvCoordLowerLeft;
                        uv[startVertexIndex + 1] = uvCoordUpperLeft;
                        uv[startVertexIndex + 2] = uvCoordUpperRight;
                        uv[startVertexIndex + 3] = uvCoordLowerRight;

                        tStart = tEnd + relativeElementGap;
                        tEnd = tStart + relativeElementSide;
                    }

                    mesh.Clear();
                    mesh.subMeshCount = 2;
                    mesh.vertices = vertices;
                    mesh.normals = normals;
                    mesh.uv = uv;
                    mesh.RecalculateBounds();

                    this.SetMaterialDivider(mesh, progressbarMesh.FillAmount, progressbarMesh.Inverse, progressbarMesh.ElementsCount, false);
                }
            }
        }

        protected override void FillElementIndices(int[] indicesArray, int indicesCurrentIndex, int startVertexIndex)
        {
            if (FlipNormal)
            {
                FillQuad(indicesArray, indicesCurrentIndex, startVertexIndex);
            }
            else
            {
                FillQuadInverse(indicesArray, indicesCurrentIndex, startVertexIndex);
            }
        }
    }
}
