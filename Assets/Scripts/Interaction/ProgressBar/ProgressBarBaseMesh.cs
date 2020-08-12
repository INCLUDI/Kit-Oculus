using UnityEngine;
using System.Collections;
using System;

namespace YAProgressBar
{
    public abstract class ProgressBarBaseMesh : MonoBehaviour
    {
        public enum ElementShape
        {
            Quad,
            Box
        }

        /// <summary>
        /// Type of the element Quad - is flat, Box - is 3d element
        /// </summary>
        [SerializeField]
        protected ElementShape elementShape = ElementShape.Quad;
        protected BaseMeshGenerator meshGenerator;
        protected Mesh mesh;
        
        /// <summary>
        /// Elements count of progress bar (less elements - bigger steps for fill amount)
        /// </summary>
        [SerializeField]
        private int elementsCount;

        /// <summary>
        /// Distance between neighbor elements
        /// </summary>
        [SerializeField]
        private float elementGap;

        /// <summary>
        /// Height of element
        /// </summary>
        [SerializeField]
        private float height;

        // <summary>
        /// Amount of skewness of element. When zero elements are rectangular.
        /// </summary>
        [SerializeField]
        private float skew;

        // <summary>
        /// Depth of the element (length in Z direction). Works only for Box element type
        /// </summary>
        [SerializeField]
        private float depth;

        /// <summary>
        /// Change visible side of quads. Works only for Quad type of the element
        /// </summary>
        [SerializeField]
        private bool flipNormal;

        /// <summary>
        /// Inverse filling direction
        /// </summary>
        [SerializeField]
        private bool inverse;

        /// <summary>
        /// Progress value
        /// </summary>
        [Range(0, 1)]
        [SerializeField]
        float fillAmount;

        public enum XAlign
        {
            left,
            center,
            right
        }

        public enum YAlign
        {
            bottom,
            center,
            top
        }

        /// <summary>
        /// Align along X axis 
        /// </summary>
        [SerializeField]
        protected XAlign xAlign;

        /// <summary>
        /// Align along Y axis for XY and ZY planes and along Z axis for XZ plane primitives
        /// </summary>
        [SerializeField]
        protected YAlign yAlign;

        private float startX;
        private float startY;

        public XAlign AlignX
        {
            get { return xAlign; }
            set
            {
                if (xAlign != value)
                {
                    xAlign = value;
                    RecalculateAligment();
                }
            }
        }

        public YAlign AlignY
        {
            get { return yAlign; }
            set
            {
                if (yAlign != value)
                {
                    yAlign = value;
                    RecalculateAligment();
                }
            }
        }

        protected void RecalculateAligment(bool needRebuild = true)
        {
            switch (xAlign)
            {
                case XAlign.left:
                    startX = 0f;
                    break;
                case XAlign.center:
                    startX = -0.5f;
                    break;
                case XAlign.right:
                    startX = -1f;
                    break;
                default:
                    break;
            }

            switch (yAlign)
            {
                case YAlign.bottom:
                    startY = 0f;
                    break;
                case YAlign.center:
                    startY = -0.5f;
                    break;
                case YAlign.top:
                    startY = -1f;
                    break;
                default:
                    break;
            }

            if (needRebuild)
            {
                Rebuild();                
            }
        }

        public bool IsInitialised { get { return mesh != null && meshGenerator != null; } }

        /// <summary>
        /// Progress value
        /// </summary>
        public float FillAmount
        {
            get { return fillAmount; }
            set
            {
                fillAmount = value;
                meshGenerator.SetMaterialDivider(mesh, FillAmount, Inverse, ElementsCount, false);
            }
        }

        /// <summary>
        /// Elements count of progress bar (less elements - bigger steps for fill amount)
        /// </summary>
        public int ElementsCount
        {
            get { return elementsCount; }
            set
            {
                if (elementsCount != value)
                {
                    elementsCount = value;
                    Rebuild();
                }
            }
        }

        /// <summary>
        /// Distance between neighbor elements
        /// </summary>
        public float ElementGap
        {
            get { return elementGap; }
            set
            {
                if (elementGap != value)
                {
                    elementGap = value;
                    Rebuild();
                }
            }
        }

        /// <summary>
        /// Height of element
        /// </summary>
        public float Height
        {
            get { return height; }
            set
            {
                if(height != value)
                {
                    height = value;
                    Rebuild();
                }
            }
        }

        // <summary>
        /// Amount of skewness of element. When zero elements are rectangular.
        /// </summary>
        public float Skew
        {
            get { return skew; }
            set
            {
                if (skew != value)
                {
                    skew = value;
                    Rebuild();
                }
            }
        }

        public float StartX
        {
            get { return startX; }
        }

        public float StartY
        {
            get { return startY; }
        }

        // <summary>
        /// Depth of the element (length in Z direction). Works only for Box element type
        /// </summary>
        public float Depth
        {
            get { return depth; }
            set
            {
                if (depth != value)
                {
                    depth = value;
                    Rebuild();
                }
            }
        }

        /// <summary>
        /// Inverse filling direction
        /// </summary>
        public bool Inverse
        {
            get { return inverse; }
            set
            {
                if (inverse != value)
                {
                    inverse = value;
                    Rebuild();
                }
            }
        }

        /// <summary>
        /// Change visible side of quads. Works only for Quad type of the element
        /// </summary>
        public bool FlipNormal
        {
            get { return flipNormal; }
            set
            {
                if (flipNormal != value)
                {
                    flipNormal = value;
                    Rebuild();
                }
            }
        }

        protected abstract void Rebuild();

        protected BaseMeshGenerator CreateMeshGenerator(ElementShape elementShape)
        {
            if (elementShape == ElementShape.Quad)
            {
                return new QuadMeshGenerator();
            }
            else if (elementShape == ElementShape.Box)
            {
                return new BoxMeshGenerator();
            }
            return null;
        }

        protected void Initialise()
        {
            mesh = new Mesh();
            GetComponent<MeshFilter>().sharedMesh = mesh;
            mesh.subMeshCount = 2;

            meshGenerator = CreateMeshGenerator(elementShape);
        }
    }
}
