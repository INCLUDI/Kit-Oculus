using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using static DataModel;
using static StatsManager;

public class DragHoldManager : EventGroupManagerBase
{
    private GameObject checkmark;
    private GameObject cross;
    
    public ProgressBarManager progressBarManager;

    public TargetTrigger[] ActiveTargets
    {
        get => ActivityManager.instance._dynamicObjects.GetComponentsInChildren<TargetTrigger>();
    }

    public List<float> Timers
    {
        get => ActiveTargets.Select(x => x.timer).ToList();
    }

    private void Awake()
    {
        checkmark = Instantiate(Resources.Load<GameObject>("Checkmark"));
        cross = Instantiate(Resources.Load<GameObject>("Cross"));
        progressBarManager = Instantiate(Resources.Load<GameObject>("ProgressBar")).GetComponent<ProgressBarManager>();

        EventManager.StartListening("CollisionOngoing", UpdateTimers);
    }

    public override List<EventConfiguration> SetEventsInCurrentGroup(List<EventConfiguration> events, bool randomEvents, int stepsToReproduce)
    {
        List<EventConfiguration> filtered = new List<EventConfiguration>();
        List<string> targets = new List<string>();
        List<string> interactables = new List<string>();
        int steps = stepsToReproduce == 0 ? events.Count() : stepsToReproduce;
        System.Random rnd = new System.Random();
        events = events.OrderBy(c => randomEvents ? rnd.Next() : 0).ToList();
        for (int i = 0; i < steps; i++)
        {
            events = events.Where(x => x.parameters.correctTargets.Except(targets).Count() != 0 && x.parameters.correctInteractables.Except(interactables).Count() != 0).ToList();
            EventConfiguration selected = events[0];
            filtered.Add(selected);
            targets.AddRange(selected.parameters.correctTargets);
            interactables.AddRange(selected.parameters.correctInteractables);
        }
        return filtered;
    }

    public override void checkCorrectAction(GameObject target, GameObject interactable)
    {
        if (ActivityManager.instance.Parameters.correctInteractables.Contains(interactable.name) &&
            ActivityManager.instance.Parameters.correctTargets.Contains(target.name))
        {
            interactable.GetComponent<Collider>().enabled = false;
            interactable.GetComponent<Rigidbody>().isKinematic = true;
            // interactable.GetComponent<OVRGrabbable>().enabled = false;
            EventManager.TriggerEvent("ReleaseObject");

            SetFinalPosition(interactable);
            SetFinalRotation(interactable);
            ParameterObjects();

            StartVisualFeedback(checkmark, interactable);
            ActivityManager.instance.playSingleInstruction(selectCorrect(Correct), "Correct", () =>
                StopVisualFeedback(checkmark, () => ActivityManager.instance.nextEvent()));

            SaveAction(request: ActivityManager.instance.Request, correctParameters: ActivityManager.instance.Parameters, action: new ActionParameters { interactable = interactable.name, target = target.name }, hints: ActivityManager.instance.hints);
        }
        else
        {
            StartVisualFeedback(cross, interactable);
            ActivityManager.instance.playSingleInstruction(selectWrong(Wrong, interactable.name), "Wrong",
                () => StopVisualFeedback(cross, () => ActivityManager.instance.IsFree = true));

            SaveAction(request: ActivityManager.instance.Request, correctParameters: ActivityManager.instance.Parameters, action: new ActionParameters { interactable = interactable.name, target = target.name }, hints: ActivityManager.instance.hints, error: true);
        }
    }


    public override string selectRequest(List<string> instructions)
    {
        string selected = base.selectRequest(instructions);
        return selected?.Replace(@"#request#", ActivityManager.instance.Parameters.correctInteractables[0]);
    }

    public override string selectCorrect(List<string> instructions, string param = null)
    {
        string selected = base.selectCorrect(instructions);
        return selected?.Replace(@"#request#", ActivityManager.instance.Parameters.correctInteractables[0]);
    }

    public override string selectWrong(List<string> instructions, string param = null)
    {
        string selected = base.selectWrong(instructions);
        return selected?.Replace(@"#wrong#", param);
    }

    private void StartVisualFeedback(GameObject feedback, GameObject interactable)
    {
        feedback.transform.position = Vector3.Lerp(interactable.transform.position, Camera.allCameras[0].transform.position, 0.3f);
        Vector3 relativePos = interactable.transform.position - Camera.allCameras[0].transform.position;
        feedback.transform.rotation = Quaternion.LookRotation(relativePos);
        _initialScale = feedback.transform.localScale;
        feedback.transform.localScale *= Vector3.Distance(interactable.transform.position, Camera.allCameras[0].transform.position);
        feedback.SetActive(true);
        feedback.transform.DOScale(new Vector3(0, 0, 0), 0.5f).From();
    }

    private void UpdateTimers()
    {
        progressBarManager.CurrentValue = Timers.Aggregate(0f, (acc, x) => acc + x);
    }

    public override void Ready()
    {
        progressBarManager.MaxValue = ActivityManager.instance.CurrentEvent.parameters.numericParameter;
    }
}
