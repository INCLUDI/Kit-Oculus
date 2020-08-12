using UnityEngine;
using System.Collections;
using YAProgressBar;
using System;

namespace YAProgressBar
{
    public abstract class BaseMeshGenerator
    {
        private int prevTreshold = -1;
        protected int maxElementsCount = 2048;

        public abstract LinearProgressBar.ElementShape ElementShape { get; }
        public abstract void GenerateLinearMesh(Mesh mesh, LinearProgressBar progressbarMesh);
        public abstract void GenerateCircularMesh(Mesh mesh, CircularProgressBar progressbarMesh);

        protected abstract int VertexCountPerElement { get; }
        protected abstract int IndicesCountPerElement { get; }
        protected abstract void FillElementIndices(int[] indicesArray, int indicesCurrentIndex, int startVertexIndex);
        protected int[] indices = new int[0];
        protected int[] emptyIndices = new int[3] { 0, 0, 0 };

        protected void FillIndices(int elementsCount)
        {
            indices = new int[elementsCount * IndicesCountPerElement];
            int indicesIndex = 0;
            int startVertexIndex = 0;
            for (int i = 0; i < elementsCount; i++)
            {
                startVertexIndex = i * VertexCountPerElement;
                FillElementIndices(indices, indicesIndex, startVertexIndex);
                indicesIndex += IndicesCountPerElement;
            }
        }

        public void SetMaterialDivider(Mesh mesh, float normalizedValue, bool inverse, int elementsCount, bool checkPrevTreshold = true)
        {
            if (0 < elementsCount && 0 < indices.Length)
            {
                normalizedValue = Mathf.Clamp01(normalizedValue);
                int treshold = Mathf.RoundToInt(Mathf.Lerp(0, elementsCount, normalizedValue));
                if (!checkPrevTreshold || (checkPrevTreshold && treshold != prevTreshold))///Optimisation Maybe cache?
                {
                    prevTreshold = treshold;

                    int[] indicesActive;
                    int[] indicesEmpty;

                    var activeIndicesCount = treshold * IndicesCountPerElement;
                    var emptyIndicesCount = (elementsCount - treshold) * IndicesCountPerElement;

                    if (0 < activeIndicesCount)
                    {
                        indicesActive = new int[activeIndicesCount];
                    }
                    else
                    {
                        indicesActive = emptyIndices;
                    }

                    if (0 < emptyIndicesCount)
                    {
                        indicesEmpty = new int[emptyIndicesCount];
                    }
                    else
                    {
                        indicesEmpty = emptyIndices;
                    }
                    
                    if (inverse)
                    {
                        if (0 < activeIndicesCount) Array.Copy(indices, emptyIndicesCount, indicesActive, 0, activeIndicesCount);
                        if (0 < emptyIndicesCount) Array.Copy(indices, indicesEmpty, emptyIndicesCount);
                    }
                    else
                    {
                        if (0 < activeIndicesCount) Array.Copy(indices, indicesActive, activeIndicesCount);
                        if (0 < emptyIndicesCount) Array.Copy(indices, activeIndicesCount, indicesEmpty, 0, emptyIndicesCount);
                    }

                    mesh.SetIndices(indicesActive, MeshTopology.Triangles, 0);
                    mesh.SetIndices(indicesEmpty, MeshTopology.Triangles, 1);
                }
            }
        }

        protected static void FillQuad(int[] indicesArray, int indicesCurrentIndex, int startVertexIndex)
        {
            indicesArray[indicesCurrentIndex + 0] = startVertexIndex + 0;
            indicesArray[indicesCurrentIndex + 1] = startVertexIndex + 1;
            indicesArray[indicesCurrentIndex + 2] = startVertexIndex + 3;

            indicesArray[indicesCurrentIndex + 3] = startVertexIndex + 1;
            indicesArray[indicesCurrentIndex + 4] = startVertexIndex + 2;
            indicesArray[indicesCurrentIndex + 5] = startVertexIndex + 3;
        }

        protected static void FillQuadInverse(int[] indicesArray, int indicesCurrentIndex, int startVertexIndex)
        {
            indicesArray[indicesCurrentIndex + 0] = startVertexIndex + 0;
            indicesArray[indicesCurrentIndex + 1] = startVertexIndex + 3;
            indicesArray[indicesCurrentIndex + 2] = startVertexIndex + 1;

            indicesArray[indicesCurrentIndex + 3] = startVertexIndex + 1;
            indicesArray[indicesCurrentIndex + 4] = startVertexIndex + 3;
            indicesArray[indicesCurrentIndex + 5] = startVertexIndex + 2;
        }

    }
}
