using Kit;
using Kit.StepGroupManagers;
using Kit.Triggers;
using UnityEngine;

public class OculusTargetTrigger : MonoBehaviour, ITargetTrigger
{
    public int HitCounter { get; set; }

    public float Timer { get; set; }
    public bool IstantaneousCollision { get; set; }

    void Start()
    {
        HitCounter = StepGroupManagerBase.Instance.CurrentStep.parameters.numericParameter;
        IstantaneousCollision = StepGroupManagerBase.Instance.StepGroup.type != "DragHoldManager";

        gameObject.layer = 9;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IstantaneousCollision)
        {
            ActivityManager.Instance.StepGroupManager.CheckCorrectAction(gameObject, other.gameObject);
        }
        EventManager.TriggerEvent("CollisionStarted");
    }

    private void OnTriggerStay(Collider other)
    {
        if (!IstantaneousCollision)
        {
            Timer += Time.deltaTime;
            if (Timer > StepGroupManagerBase.Instance.CurrentStep.parameters.numericParameter)
            {
                StepGroupManagerBase.Instance.CheckCorrectAction(gameObject, other.gameObject);
            }
        }
        EventManager.TriggerEvent("CollisionOngoing");
    }

    private void OnTriggerExit(Collider other)
    {
        EventManager.TriggerEvent("CollisionFinished");
    }
}
