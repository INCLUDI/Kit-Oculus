using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour
{
    private Collider target;

    private void Awake()
    {
        target = GetComponent<Collider>();
    }

    void Start()
    {
        EventManager.StartListening("EnableInteraction", EnableTarget);
        EventManager.StartListening("DisableInteraction", DisableTarget);
    }

    private void EnableTarget()
    {
        target.enabled = true;
    }

    private void DisableTarget()
    {
        target.enabled = false;
    }
}
