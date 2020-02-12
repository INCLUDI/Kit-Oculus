using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour
{
    public int hitCounter;

    private ActivityManager activityManager;
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
        activityManager = GameObject.Find("ActivityManager").GetComponent<ActivityManager>();

        EventManager.StartListening("EnableInteraction", EnableTarget);
        EventManager.StartListening("DisableInteraction", DisableTarget);
    }

    private void OnTriggerEnter(Collider other)
    {
        activityManager.checkCorrectObject(other.gameObject, this.gameObject);
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
