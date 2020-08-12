using UnityEngine;
using System.Collections;
using YAProgressBar;
using System;

namespace YAProgressBar
{
    [AddComponentMenu("Yet Another Progress Bar/Linear Progress Bar")]
    [RequireComponent(typeof(MeshFilter))]
    public class LinearProgressBar : ProgressBarBaseMesh
    {
        /// <summary>
        /// Length of progress bar
        /// </summary>
        [SerializeField]
        private float length;

        /// <summary>
        /// Length of progress bar
        /// </summary>
        public float Length
        {
            get { return length; }
            set
            {
                if (length != value)
                {
                    length = value;
                    Rebuild();
                }
            }
        }

        void Awake()
        {
            if (!IsInitialised)
            {
                Initialise();
            }
            RecalculateAligment(false);
            meshGenerator.GenerateLinearMesh(mesh, this);
        }

       

        void OnValidate()
        {
            if (!IsInitialised)
            {
                Initialise();
            }

            if (meshGenerator == null || elementShape != meshGenerator.ElementShape)
            {
                meshGenerator = CreateMeshGenerator(elementShape);
            }

            RecalculateAligment(false);
            meshGenerator.GenerateLinearMesh(mesh, this);            
        }

        void OnCurveChange()
        {
            RecalculateAligment(false);
            meshGenerator.GenerateLinearMesh(mesh, this);
        }

        protected override void Rebuild()
        {
            RecalculateAligment(false);
            meshGenerator.GenerateLinearMesh(mesh, this);
        }
    }
}
