using UnityEngine;
using System.Collections;
using System;

namespace YAProgressBar
{
    public class CircularCurve : BaseCurve
    {
        /// <summary>
        /// Arc radius
        /// </summary>
        private float radius;

        /// <summary>
        /// Start angle of arc in degrees (0 degrees at 3 o'clock)  
        /// </summary>
        private float startAngle;

        /// <summary>
        /// End angle of arc in degrees
        /// </summary>
        private float endAngle;



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
                    InvokeOnChange();
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
                if (startAngle != value && value <= endAngle)
                {
                    startAngle = value;
                    InvokeOnChange();
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
                if (endAngle != value && startAngle <= value)
                {
                    endAngle = value;
                    InvokeOnChange();
                }
            }
        }

        public void Setup(float start, float end, float r)
        {
            startAngle = start;
            endAngle = end;
            radius = r;
        }

        public override bool IsClosed
        {
            get
            {
                return GetFullAngle() == 360;
            }
        }

        public override Vector3 GetPoint(float t, out Quaternion q)
        {
            float fullAngle = GetFullAngle();
            if (360 < fullAngle)
            {
                endAngle = startAngle + 360;
                fullAngle = 360;
            }
            float angle = startAngle + Mathf.Lerp(0, fullAngle, t);
            float angleRad = Mathf.Deg2Rad * angle;

            Vector3 p = Vector3.zero;
            switch (axes)
            {
                case PlaneAxes.XY:
                    q = Quaternion.Euler(0, 0, angle + 90);
                    p = new Vector3(radius * Mathf.Cos(angleRad), radius * Mathf.Sin(angleRad), 0);
                    break;
                case PlaneAxes.XZ:
                    q = Quaternion.Euler(0, angle, 0);
                    //p = new Vector3(radius * Mathf.Cos(angleRad), 0, radius * Mathf.Sin(angleRad));
                    p = new Vector3(radius * Mathf.Sin(angleRad), 0, radius * Mathf.Cos(angleRad));
                    break;
                case PlaneAxes.YZ:
                    q = Quaternion.Euler(90 + angle, 0, 0);
                    p = new Vector3(0, radius * Mathf.Cos(angleRad), radius * Mathf.Sin(angleRad));
                    break;
                default:
                    q = Quaternion.identity;
                    break;
            }

            //?
            return p;// transform.TransformPoint(
        }

        public override Vector3 GetPoint(float t, out Quaternion q, out Vector3 tangent)
        {
            float fullAngle = GetFullAngle();
            if (360 < fullAngle)
            {
                endAngle = startAngle + 360;
                fullAngle = 360;
            }
            float angle = startAngle + Mathf.Lerp(0, fullAngle, t);
            float angleRad = Mathf.Deg2Rad * angle;
            tangent = Vector3.zero;
            Vector3 p = Vector3.zero;
            float cos = Mathf.Cos(angleRad);
            float sin = Mathf.Sin(angleRad);
            switch (axes)
            {
                case PlaneAxes.XY:
                    q = Quaternion.Euler(0, 0, angle + 90);
                    p = new Vector3(radius * cos, radius * sin, 0);
                    tangent = new Vector3(sin, cos, 0);
                    break;
                case PlaneAxes.XZ:
                    q = Quaternion.Euler(0, angle, 0);
                    //p = new Vector3(radius * Mathf.Cos(angleRad), 0, radius * Mathf.Sin(angleRad));
                    p = new Vector3(radius * sin, 0, radius * cos);
                    tangent = new Vector3(cos, 0, sin);
                    break;
                case PlaneAxes.YZ:
                    q = Quaternion.Euler(90 + angle, 0, 0);
                    p = new Vector3(0, radius * cos, radius * sin);
                    tangent = new Vector3(0, sin, cos);
                    break;
                default:
                    q = Quaternion.identity;
                    break;
            }

            //?
            return p;// transform.TransformPoint(
        }

        public float GetFullAngle()
        {
            return endAngle - startAngle;
        }

        internal override float ComputeLength()
        {
            float fullAngle = GetFullAngle();
            if (360 < fullAngle)
            {
                endAngle = startAngle + 360;
                fullAngle = 360;
            }
            return 2 * Mathf.PI * Radius * fullAngle / 360f;
        }

        public override Vector3 GetPoint(float t, out Vector3 normal)
        {
            float fullAngle = GetFullAngle();
            if (360 < fullAngle)
            {
                endAngle = startAngle + 360;
                fullAngle = 360;
            }
            float angle = startAngle + Mathf.LerpUnclamped(0, fullAngle, t);
            float angleRad = Mathf.Deg2Rad * angle;

            Vector3 p = Vector3.zero;
            switch (axes)
            {
                case PlaneAxes.XY:
                    normal = new Vector3( Mathf.Cos(angleRad), Mathf.Sin(angleRad), 0);
                    p = normal * radius;
                    break;
                case PlaneAxes.XZ:
                    normal = new Vector3(Mathf.Sin(angleRad), 0, Mathf.Cos(angleRad));
                    p = normal * radius;
                    break;
                case PlaneAxes.YZ:
                    normal = new Vector3(0, Mathf.Cos(angleRad), Mathf.Sin(angleRad));
                    p = normal * radius;
                    break;
                default:
                    normal = Vector3.up;
                    break;
            }

            return p;
        }
    }
}
