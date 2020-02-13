using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualAssistantManager : MonoBehaviour
{
    private Transform mainCamera;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GameObject.Find("OVRCameraRig").transform;

        EventManager.StartListening("startTalking", startTalking);
        EventManager.StartListening("stopTalking", stopTalking);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 relativePos = mainCamera.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(relativePos);
        rotation.x = 0f;
        rotation.z = 0f;
        transform.rotation = rotation;
    }

    public void startTalking()
    {
        animator.SetTrigger("StartTalking");
    }

    public void stopTalking()
    {
        animator.SetTrigger("StopTalking");
    }
}
