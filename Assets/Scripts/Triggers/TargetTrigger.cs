using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetTrigger : TriggerBase
{
    public int hitCounter;

    private float timer;
    public bool istantaneousCollision;

    //private Collider target;

    //private void Awake()
    //{
    //    target = GetComponent<Collider>();
    //}

    void Start()
    {
        hitCounter = ActivityManager.instance.CurrentEvent.parameters.numericParameter;
        istantaneousCollision = ActivityManager.instance.CurrentEventGroup.type != "DragHoldManager";
        //EventManager.StartListening("EnableInteraction", EnableTarget);
        //EventManager.StartListening("DisableInteraction", DisableTarget);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (istantaneousCollision)
        {
            ActivityManager.instance.checkCorrectAction(gameObject, other.gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!istantaneousCollision)
        {
            timer += Time.deltaTime;
            if (timer > ActivityManager.instance.CurrentEvent.parameters.numericParameter)
            {
                ActivityManager.instance.checkCorrectAction(gameObject, other.gameObject);
            }
        }
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
