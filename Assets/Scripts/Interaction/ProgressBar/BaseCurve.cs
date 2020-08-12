using UnityEngine;
using System.Collections;
using System;

public abstract class BaseCurve
{
    /// <summary>
    /// Axes which defines a plane
    /// </summary>
    public enum PlaneAxes
    {
        XY,
        XZ,
        YZ
    }

    /// <summary>
    /// Plane in which lies primitive
    /// </summary>
    [SerializeField]
    protected PlaneAxes axes;

    /// <summary>
    /// Plane in which lies primitive
    /// </summary>
    public PlaneAxes CurvePlane
    {
        get { return axes; }
        set
        {
            axes = value;
            InvokeOnChange();
        }
    }

    public bool FlipNormal { get; set; }
    public Action OnChange { get; internal set; }
    public abstract bool IsClosed { get; }

    public abstract Vector3 GetPoint(float t, out Quaternion q);
    public abstract Vector3 GetPoint(float t, out Vector3 normal);
    public abstract Vector3 GetPoint(float t, out Quaternion q, out Vector3 tangent);

    internal abstract float ComputeLength();
    //void OnValidate()
    //{
    //    InvokeOnChange();
    //}

    protected void InvokeOnChange()
    {
        if (OnChange != null)
        {
            OnChange();
        }
    }
    //{
    //    int i;
    //    if (t >= 1f)
    //    {
    //        t = 1f;
    //        i = points.Length - 4;
    //    }
    //    else
    //    {
    //        t = Mathf.Clamp01(t) * CurveCount;
    //        i = (int)t;
    //        t -= i;
    //        i *= 3;
    //    }
    //    return transform.TransformPoint(Bezier.GetPoint(points[i], points[i + 1], points[i + 2], points[i + 3], t));
    //}

}
