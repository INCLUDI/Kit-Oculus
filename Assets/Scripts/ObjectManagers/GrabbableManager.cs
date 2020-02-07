using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GrabbableManager : MonoBehaviour
{
    [HideInInspector]
    public Vector3 initialPosition;

    private void Awake()
    {
        initialPosition = transform.position;
    }

}
