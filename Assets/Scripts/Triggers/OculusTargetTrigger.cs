using Kit;
using Kit.Triggers;
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
        HitCounter = ActivityManager.instance.CurrentStep.parameters.numericParameter;
        IstantaneousCollision = ActivityManager.instance.CurrentStepGroup.type != "DragHoldManager";

        gameObject.layer = 9;
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
            if (Timer > ActivityManager.instance.CurrentStep.parameters.numericParameter)
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
