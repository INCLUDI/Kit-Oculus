using UnityEngine;
using System.Collections;
using YAProgressBar;
using System;

namespace YAProgressBar
{
    [AddComponentMenu("Yet Another Progress Bar/Circular Progress Bar")]
    [RequireComponent(typeof(MeshFilter))]
    public class CircularProgressBar : ProgressBarBaseMesh
    {
        /// <summary>
        /// Arc radius
        /// </summary>
        [SerializeField]
        private float radius;

        /// <summary>
        /// Start angle of arc in degrees (0 degrees at 3 o'clock)
        /// </summary>
        [SerializeField]
        private float startAngle;

        /// <summary>
        /// End angle of arc in degrees
        /// </summary>
        [SerializeField]
        private float endAngle;

        private CircularCurve curve = new CircularCurve();

        public BaseCurve Curve
        {
            get { return curve; }
        }

        /// <summary>
        /// Arc radius
        /// </summary>
        public float Radius
        {
            get { return radius; }
            set
            {
                if (radius != value)
                {
                    radius = value;
                    Rebuild();
                }
            }
        }

        /// <summary>
        /// Start angle of arc in degrees (0 degrees at 3 o'clock)
        /// </summary>
        public float StartAngle
        {
            get { return startAngle; }
            set
            {
                if (startAngle != value)
                {
                    startAngle = value;
                    Rebuild();
                }
            }
        }

        /// <summary>
        /// End angle of arc in degrees
        /// </summary>
        public float EndAngle
        {
            get { return endAngle; }
            set
            {
                if (endAngle != value)
                {
                    endAngle = value;
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

            curve.Setup(startAngle, endAngle, radius);
            curve.CurvePlane = BaseCurve.PlaneAxes.XY;
            RecalculateAligment(false);

            meshGenerator.GenerateCircularMesh(mesh, this);
        }

        void OnValidate()
        {
            if (!IsInitialised)
            {
                Initialise();
            }
            
            curve.Setup(startAngle, endAngle, radius);
            curve.CurvePlane = BaseCurve.PlaneAxes.XY;

            if (meshGenerator == null || elementShape != meshGenerator.ElementShape)
            {
                meshGenerator = CreateMeshGenerator(elementShape);
            }

            RecalculateAligment(false);
            meshGenerator.GenerateCircularMesh(mesh, this);
        }

        void OnCurveChange()
        {
            RecalculateAligment(false);
            meshGenerator.GenerateCircularMesh(mesh, this);
        }

        protected override void Rebuild()
        {
            RecalculateAligment(false);
            meshGenerator.GenerateCircularMesh(mesh, this);
        }
    }
}