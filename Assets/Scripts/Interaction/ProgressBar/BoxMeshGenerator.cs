using UnityEngine;
using System.Collections;
using YAProgressBar;
using System;

namespace YAProgressBar
{
    public class BoxMeshGenerator : BaseMeshGenerator
    {
        public override LinearProgressBar.ElementShape ElementShape
        {
            get
            {
                return LinearProgressBar.ElementShape.Box;
            }
        }

        protected override int VertexCountPerElement
        {
            get
            {
                return 24;
            }
        }

        protected override int IndicesCountPerElement
        {
            get
            {
                return VertexCountPerElement + 2 * 6;
            }
        }

        public override void GenerateLinearMesh(Mesh mesh, LinearProgressBar progressbarMesh)
        {
            int elementsCount = progressbarMesh.ElementsCount;
            float lenght = progressbarMesh.Length;
            float height = progressbarMesh.Height;
            float elementGap = progressbarMesh.ElementGap;
            float startX = progressbarMesh.StartX;
            float startY = progressbarMesh.StartY;
            float skew = progressbarMesh.Skew;
            float depth = progressbarMesh.Depth;

            if (0 < elementsCount && 0 < height && 0 < lenght && elementGap < lenght)
            {
                FillIndices(elementsCount);

                float elementSide = (lenght - elementGap * (elementsCount - 1)) / elementsCount;
                var vertices = new Vector3[elementsCount * VertexCountPerElement];
                var uv = new Vector2[elementsCount * VertexCountPerElement];
                var normals = new Vector3[elementsCount * VertexCountPerElement];

                Vector2 uvCoordLowerLeft = new Vector2(0, 0);
                Vector2 uvCoordUpperLeft = new Vector2(0, 1);
                Vector2 uvCoordUpperRight = new Vector2(1, 1);
                Vector2 uvCoordLowerRight = new Vector2(1, 0);

                int startVertexIndex = 0;
                float elementX = startX * lenght;
                float elementY = startY * height;
                for (int i = 0; i < elementsCount; i++)
                {
                    startVertexIndex = i * VertexCountPerElement;
                    ///Front
                    vertices[startVertexIndex + 0] = new Vector3(elementX, elementY, 0);
                    vertices[startVertexIndex + 1] = new Vector3(elementX + skew, elementY + height, 0);
                    vertices[startVertexIndex + 2] = new Vector3(elementX + elementSide + skew, elementY + height, 0);
                    vertices[startVertexIndex + 3] = new Vector3(elementX + elementSide, elementY, 0);
                    normals[startVertexIndex + 0] = Vector3.back;
                    normals[startVertexIndex + 1] = Vector3.back;
                    normals[startVertexIndex + 2] = Vector3.back;
                    normals[startVertexIndex + 3] = Vector3.back;

                    ///Top                    
                    vertices[startVertexIndex + 4] = new Vector3(elementX + skew, elementY + height, 0);
                    vertices[startVertexIndex + 5] = new Vector3(elementX + skew, elementY + height, depth);
                    vertices[startVertexIndex + 6] = new Vector3(elementX + elementSide + skew, elementY + height, depth);
                    vertices[startVertexIndex + 7] = new Vector3(elementX + elementSide + skew, elementY + height, 0);
                    normals[startVertexIndex + 4] = Vector3.up;
                    normals[startVertexIndex + 5] = Vector3.up;
                    normals[startVertexIndex + 6] = Vector3.up;
                    normals[startVertexIndex + 7] = Vector3.up;

                    ///Bottom                    
                    vertices[startVertexIndex + 8] = new Vector3(elementX, elementY, 0);
                    vertices[startVertexIndex + 9] = new Vector3(elementX + elementSide, elementY, 0);
                    vertices[startVertexIndex + 10] = new Vector3(elementX + elementSide, elementY, depth);
                    vertices[startVertexIndex + 11] = new Vector3(elementX, elementY, depth);
                    normals[startVertexIndex + 8] = Vector3.down;
                    normals[startVertexIndex + 9] = Vector3.down;
                    normals[startVertexIndex + 10] = Vector3.down;
                    normals[startVertexIndex + 11] = Vector3.down;

                    ///Left
                    vertices[startVertexIndex + 12] = new Vector3(elementX, elementY, 0);
                    vertices[startVertexIndex + 13] = new Vector3(elementX, elementY, depth);
                    vertices[startVertexIndex + 14] = new Vector3(elementX + skew, elementY + height, depth);
                    vertices[startVertexIndex + 15] = new Vector3(elementX + skew, elementY + height, 0);
                    normals[startVertexIndex + 12] = Vector3.left;
                    normals[startVertexIndex + 13] = Vector3.left;
                    normals[startVertexIndex + 14] = Vector3.left;
                    normals[startVertexIndex + 15] = Vector3.left;

                    ///Right
                    vertices[startVertexIndex + 16] = new Vector3(elementX + elementSide, elementY, 0);
                    vertices[startVertexIndex + 17] = new Vector3(elementX + elementSide + skew, elementY + height, 0);
                    vertices[startVertexIndex + 18] = new Vector3(elementX + elementSide + skew, elementY + height, depth);
                    vertices[startVertexIndex + 19] = new Vector3(elementX + elementSide, elementY, depth);
                    normals[startVertexIndex + 16] = Vector3.right;
                    normals[startVertexIndex + 17] = Vector3.right;
                    normals[startVertexIndex + 18] = Vector3.right;
                    normals[startVertexIndex + 19] = Vector3.right;

                    ///Back
                    vertices[startVertexIndex + 20] = new Vector3(elementX, elementY, depth);
                    vertices[startVertexIndex + 21] = new Vector3(elementX + elementSide, elementY, depth);
                    vertices[startVertexIndex + 22] = new Vector3(elementX + elementSide + skew, elementY + height, depth);
                    vertices[startVertexIndex + 23] = new Vector3(elementX + skew, elementY + height, depth);
                    normals[startVertexIndex + 20] = Vector3.forward;
                    normals[startVertexIndex + 21] = Vector3.forward;
                    normals[startVertexIndex + 22] = Vector3.forward;
                    normals[startVertexIndex + 23] = Vector3.forward;

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
            }

            this.SetMaterialDivider(mesh, progressbarMesh.FillAmount, progressbarMesh.Inverse, progressbarMesh.ElementsCount, false);
        }

        public override void GenerateCircularMesh(Mesh mesh, CircularProgressBar progressbarMesh)
        {
            var curve = progressbarMesh.Curve;

            int elementsCount = progressbarMesh.ElementsCount;
            float height = progressbarMesh.Height;
            float elementGap = progressbarMesh.ElementGap;
            float depth = progressbarMesh.Depth;
            float curveLength = curve.ComputeLength();
            float skew = progressbarMesh.Skew;

            float elementSide;
            float relativeElementSide;
            float relativeElementGap;
            float relativeElementSkew;
            if (curve != null)
            {
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
                    Vector3 normalStart;
                    Vector3 normalEnd;
                    Vector3 normalMid;
                    Vector3 normalStartSkew;
                    Vector3 normalEndSkew;
                    Vector3 normalMidSkew;
                    float tStart = 0;
                    float tEnd = tStart + relativeElementSide;
                    float tMiddle = (tEnd + tStart) / 2f;
                    Vector3 pStartFrontBottom;
                    Vector3 pEndFrontBottom;
                    Vector3 pStartFrontTop;
                    Vector3 pEndFrontTop;
                    Vector3 pStartBackBottom;
                    Vector3 pEndBackBottom;
                    Vector3 pStartBackTop;
                    Vector3 pEndBackTop;

                    for (int i = 0; i < elementsCount; i++)
                    {
                        pStart = curve.GetPoint(tStart, out normalStart);
                        pEnd = curve.GetPoint(tEnd, out normalEnd);

                        if (elementGap <= 0)///Lathe
                        {
                            pStartFrontBottom = pStart;
                            pEndFrontBottom = pEnd;
                            pStartFrontTop = pStart + normalStart * height;
                            pEndFrontTop = pEnd + normalEnd * height;

                            if (0.00001f < Mathf.Abs(skew))
                            {

                                var pStartSkew = curve.GetPoint(tStart + relativeElementSkew, out normalStartSkew);
                                var pEndSkew = curve.GetPoint(tEnd + relativeElementSkew, out normalEndSkew);

                                pStartBackBottom = pStartSkew + Vector3.forward * depth;
                                pEndBackBottom = pEndSkew + Vector3.forward * depth;
                                pStartBackTop = pStartBackBottom + normalStartSkew * height;
                                pEndBackTop = pEndBackBottom + normalEndSkew * height;
                            }
                            else
                            {
                                pStartBackBottom = pStartFrontBottom + Vector3.forward * depth;
                                pEndBackBottom = pEndFrontBottom + Vector3.forward * depth;
                                pStartBackTop = pStartBackBottom + normalStart * height;
                                pEndBackTop = pEndBackBottom + normalEnd * height;

                                normalStartSkew = normalStart;
                                normalEndSkew = normalEnd;
                            }
                        }
                        else
                        {
                            pStartFrontBottom = pStart;
                            pEndFrontBottom = pEnd;
                            curve.GetPoint(tMiddle, out normalMid);
                            pStartFrontTop = pStart + normalMid * height;
                            pEndFrontTop = pEnd + normalMid * height;

                            normalStart = normalMid;
                            normalEnd = normalMid;

                            if (0.00001f < Mathf.Abs(skew))
                            {
                                var pStartSkew = curve.GetPoint(tStart + relativeElementSkew, out normalStartSkew);
                                var pEndSkew = curve.GetPoint(tEnd + relativeElementSkew, out normalEndSkew);
                                curve.GetPoint((tStart + relativeElementSkew + tEnd + relativeElementSkew) / 2f, out normalMidSkew);

                                pStartBackBottom = pStartSkew + Vector3.forward * depth;
                                pEndBackBottom = pEndSkew + Vector3.forward * depth;
                                pStartBackTop = pStartBackBottom + normalMidSkew * height;
                                pEndBackTop = pEndBackBottom + normalMidSkew * height;

                                normalStartSkew = normalMidSkew;
                                normalEndSkew = normalMidSkew;
                            }
                            else
                            {
                                pStartBackBottom = pStartFrontBottom + Vector3.forward * depth;
                                pEndBackBottom = pEndFrontBottom + Vector3.forward * depth;
                                pStartBackTop = pStartBackBottom + normalMid * height;
                                pEndBackTop = pEndBackBottom + normalMid * height;

                                normalStartSkew = normalMid;
                                normalEndSkew = normalMid;
                            }
                        }
                        Vector3 leftSideNormal = (Vector3.Cross(pStartBackTop - pStartFrontTop, pStartFrontBottom - pStartFrontTop)).normalized;
                        Vector3 topNormal = (normalStart + normalEnd + normalStartSkew + normalEndSkew).normalized;

                        startVertexIndex = i * VertexCountPerElement;
                        ///Front
                        vertices[startVertexIndex + 0] = pStartFrontBottom;
                        vertices[startVertexIndex + 1] = pEndFrontBottom;
                        vertices[startVertexIndex + 2] = pEndFrontTop;
                        vertices[startVertexIndex + 3] = pStartFrontTop;
                        normals[startVertexIndex + 0] = Vector3.back;
                        normals[startVertexIndex + 1] = Vector3.back;
                        normals[startVertexIndex + 2] = Vector3.back;
                        normals[startVertexIndex + 3] = Vector3.back;

                        ///Top                    
                        vertices[startVertexIndex + 4] = pStartFrontTop;
                        vertices[startVertexIndex + 5] = pEndFrontTop;
                        vertices[startVertexIndex + 6] = pEndBackTop;
                        vertices[startVertexIndex + 7] = pStartBackTop;
                        normals[startVertexIndex + 4] = topNormal;
                        normals[startVertexIndex + 5] = topNormal;
                        normals[startVertexIndex + 6] = topNormal;
                        normals[startVertexIndex + 7] = topNormal;

                        ///Bottom                    
                        vertices[startVertexIndex + 8] = pStartFrontBottom;
                        vertices[startVertexIndex + 9] = pStartBackBottom;
                        vertices[startVertexIndex + 10] = pEndBackBottom;
                        vertices[startVertexIndex + 11] = pEndFrontBottom;
                        normals[startVertexIndex + 8] = -topNormal;
                        normals[startVertexIndex + 9] = -topNormal;
                        normals[startVertexIndex + 10] = -topNormal;
                        normals[startVertexIndex + 11] = -topNormal;

                        ///Left
                        vertices[startVertexIndex + 12] = pStartFrontBottom;
                        vertices[startVertexIndex + 13] = pStartFrontTop;
                        vertices[startVertexIndex + 14] = pStartBackTop;
                        vertices[startVertexIndex + 15] = pStartBackBottom;
                        normals[startVertexIndex + 12] = leftSideNormal;
                        normals[startVertexIndex + 13] = leftSideNormal;
                        normals[startVertexIndex + 14] = leftSideNormal;
                        normals[startVertexIndex + 15] = leftSideNormal;

                        ///Right
                        vertices[startVertexIndex + 16] = pEndFrontBottom;
                        vertices[startVertexIndex + 17] = pEndBackBottom;
                        vertices[startVertexIndex + 18] = pEndBackTop;
                        vertices[startVertexIndex + 19] = pEndFrontTop;
                        normals[startVertexIndex + 16] = -leftSideNormal;
                        normals[startVertexIndex + 17] = -leftSideNormal;
                        normals[startVertexIndex + 18] = -leftSideNormal;
                        normals[startVertexIndex + 19] = -leftSideNormal;

                        ///Back
                        vertices[startVertexIndex + 20] = pStartBackBottom;
                        vertices[startVertexIndex + 21] = pStartBackTop;
                        vertices[startVertexIndex + 22] = pEndBackTop;
                        vertices[startVertexIndex + 23] = pEndBackBottom;
                        normals[startVertexIndex + 20] = Vector3.forward;
                        normals[startVertexIndex + 21] = Vector3.forward;
                        normals[startVertexIndex + 22] = Vector3.forward;
                        normals[startVertexIndex + 23] = Vector3.forward;

                        tStart = tEnd + relativeElementGap;
                        tEnd = tStart + relativeElementSide;
                        tMiddle = (tEnd + tStart) / 2f;
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

        private static void FillFace(float height, float skew, float elementSide, Vector3[] vertices, int startVertexIndex, float elementX, float elementY)
        {
            vertices[startVertexIndex + 0] = new Vector3(elementX, elementY, 0);
            vertices[startVertexIndex + 1] = new Vector3(elementX + skew, elementY + height, 0);
            vertices[startVertexIndex + 2] = new Vector3(elementX + elementSide + skew, elementY + height, 0);
            vertices[startVertexIndex + 3] = new Vector3(elementX + elementSide, elementY, 0);
        }

        private static void FillBox(int[] indicesArray, int indicesCurrentIndex, int startVertexIndex)
        {
            FillQuad(indicesArray, indicesCurrentIndex + 0, startVertexIndex);
            FillQuad(indicesArray, indicesCurrentIndex + 6, startVertexIndex + 4);
            FillQuad(indicesArray, indicesCurrentIndex + 12, startVertexIndex + 8);
            FillQuad(indicesArray, indicesCurrentIndex + 18, startVertexIndex + 12);
            FillQuad(indicesArray, indicesCurrentIndex + 24, startVertexIndex + 16);
            FillQuad(indicesArray, indicesCurrentIndex + 30, startVertexIndex + 20);
        }

        protected override void FillElementIndices(int[] indicesArray, int indicesCurrentIndex, int startVertexIndex)
        {
            FillBox(indicesArray, indicesCurrentIndex, startVertexIndex);
        }
    }
}
