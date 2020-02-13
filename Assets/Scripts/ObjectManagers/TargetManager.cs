using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour
{
    public int hitCounter;

    private Collider target;

    public TargetManager(int hitCounter)
    {
        this.hitCounter = hitCounter;
    }

    private void Awake()
    {
        target = GetComponent<Collider>();
    }

    void Start()
    {
        EventManager.StartListening("EnableInteraction", EnableTarget);
        EventManager.StartListening("DisableInteraction", DisableTarget);
    }

    private void OnTriggerEnter(Collider other)
    {
        ActivityManager.instance.checkCorrectObject(other.gameObject, gameObject);
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
