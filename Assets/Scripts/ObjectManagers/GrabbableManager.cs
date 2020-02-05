using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GrabbableManager : MonoBehaviour
{
    private OVRGrabbable grabbable;

    private Vector3 initialPosition;

    private void Awake()
    {
        grabbable = GetComponent<OVRGrabbable>();
        initialPosition = transform.position;
    }

    void Start()
    {
        EventManager.StartListening("EnableInteraction", EnableGrabbable);
        EventManager.StartListening("DisableInteraction", DisableGrabbable);
        EventManager.StartListening("DisableObject", DisableObject);
    }

    private void EnableGrabbable()
    {
        grabbable.enabled = true;
    }

    private void DisableGrabbable()
    {
        EventManager.TriggerEvent("ReleaseObject");
        grabbable.enabled = false;
        // transform.DOMove(initialPosition, 1);
    }

    private void DisableObject()
    {
        GetComponent<Collider>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<OVRGrabbable>().enabled = false;
        enabled = false;
        DisableGrabbable();
    }

    private void OnDestroy()
    {
        EventManager.StopListening("EnableInteraction", EnableGrabbable);
        EventManager.StopListening("DisableInteraction", DisableGrabbable);
        EventManager.StopListening("DisableObject", DisableObject);
    }
}
