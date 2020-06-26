using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetTrigger : TriggerBase
{
    public int hitCounter;

    //private Collider target;

    //private void Awake()
    //{
    //    target = GetComponent<Collider>();
    //}

    void Start()
    {
        hitCounter = ActivityManager.instance.CurrentEvent.parameters.numericParameter;
        //EventManager.StartListening("EnableInteraction", EnableTarget);
        //EventManager.StartListening("DisableInteraction", DisableTarget);
    }

    private void OnTriggerEnter(Collider other)
    {
        ActivityManager.instance.checkCorrectAction(gameObject, other.gameObject);
    }

    //private void EnableTarget()
    //{
    //    target.enabled = true;
    //}

    //private void DisableTarget()
    //{
    //    target.enabled = false;
    //}

}
