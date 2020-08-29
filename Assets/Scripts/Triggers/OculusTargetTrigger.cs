using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OculusTargetTrigger : MonoBehaviour, ITargetTrigger
{
    public int HitCounter { get; set; }

    public float Timer { get; set; }
    public bool IstantaneousCollision { get; set; }

    void Start()
    {
        HitCounter = ActivityManager.instance.CurrentEvent.parameters.numericParameter;
        IstantaneousCollision = ActivityManager.instance.CurrentEventGroup.type != "DragHoldManager";
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IstantaneousCollision)
        {
            ActivityManager.instance.checkCorrectAction(gameObject, other.gameObject);
        }
        EventManager.TriggerEvent("CollisionStarted");
    }

    private void OnTriggerStay(Collider other)
    {
        if (!IstantaneousCollision)
        {
            Timer += Time.deltaTime;
            if (Timer > ActivityManager.instance.CurrentEvent.parameters.numericParameter)
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
