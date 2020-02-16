﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GrabbableManager : MonoBehaviour
{
    [HideInInspector]
    public Vector3 initialPosition;
    [HideInInspector]
    public Quaternion initialRotation;
    [HideInInspector]
    public Vector3 initialScale;

    private void Awake()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        initialScale = transform.localScale;
    }

}
