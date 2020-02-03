using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        other.enabled = false;
        
        GameObject collidingObject = other.gameObject;
        collidingObject.transform.position = transform.position;
        collidingObject.transform.rotation = transform.rotation;
        collidingObject.GetComponent<Rigidbody>().useGravity = false;

        OVRGrabbable grabbable = gameObject.GetComponent<OVRGrabbable>();
        if (grabbable.isGrabbed)
        {
            gameObject.GetComponent<MeshRenderer>().enabled = true;
        }
        else
        gameObject.GetComponent<MeshRenderer>().enabled = false;

        OVRGrabber leftHand = GameObject.Find("CustomHandLeft").GetComponent<OVRGrabber>();
        OVRGrabber rightHand = GameObject.Find("CustomHandRight").GetComponent<OVRGrabber>();
    }
}
