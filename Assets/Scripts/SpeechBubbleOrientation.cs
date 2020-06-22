using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeechBubbleOrientation : MonoBehaviour
{
    void Update()
    {
        Vector3 relativePos = transform.position - Camera.allCameras[0].transform.position;
        Quaternion rotation = Quaternion.LookRotation(relativePos);
        transform.rotation = rotation;
    }
}
