using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class OculusInteractableTrigger : InteractableTrigger
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

    private void Start()
    {
        XRGrabInteractable interactable = gameObject.AddComponent<XRGrabInteractable>();
        Transform pivot = transform.Find("Pivot");
        if (pivot != null)
        {
            interactable.attachTransform = pivot;
        }
    }
}
