using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetTrigger : TriggerBase
{
    public int hitCounter;

    public float timer;
    public bool istantaneousCollision;

    void Start()
    {
        hitCounter = ActivityManager.instance.CurrentEvent.parameters.numericParameter;
        istantaneousCollision = ActivityManager.instance.CurrentEventGroup.type != "DragHoldManager";
    }

    private void OnTriggerEnter(Collider other)
    {
        if (istantaneousCollision)
        {
            ActivityManager.instance.checkCorrectAction(gameObject, other.gameObject);
        }
        EventManager.TriggerEvent("CollisionStarted");
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
        EventManager.TriggerEvent("CollisionOngoing");
    }

    private void OnTriggerExit(Collider other)
    {
        EventManager.TriggerEvent("CollisionFinished");
    }
}
