using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static DataModel;
using static StatsManager;

public class DragMoveManager : EventGroupManagerBase
{
    private GameObject checkmark;
    private GameObject cross;
    private GameObject progressBar;

    public ProgressBarManager progressBarManager;

    public TargetTrigger[] ActiveTargets
    {
        get => ActivityManager.instance._dynamicObjects.GetComponentsInChildren<TargetTrigger>();
    }

    public List<int> HitCounters
    {
        get => ActiveTargets.Select(x => x.hitCounter).ToList();
    }

    private void Awake()
    {
        checkmark = Instantiate(Resources.Load<GameObject>("Checkmark"));
        cross = Instantiate(Resources.Load<GameObject>("Cross"));
        progressBar = Instantiate(Resources.Load<GameObject>("ProgressBar"));
        progressBarManager = progressBar.GetComponent<ProgressBarManager>();

        EventManager.StartListening("CollisionFinished", UpdateHitCounters);
    }

    private void OnDestroy()
    {
        EventManager.StopListening("CollisionFinished", UpdateHitCounters);
        Destroy(progressBarManager);
        Destroy(checkmark);
        Destroy(cross);
    }


    public override void checkCorrectAction(GameObject target, GameObject interactable)
    {
        if (ActivityManager.instance.Parameters.correctInteractables.Contains(interactable.name) &&
               ActivityManager.instance.Parameters.correctTargets.Contains(target.name))
        {
            if (target.GetComponent<TargetTrigger>().hitCounter > 0)
            {
                target.GetComponent<TargetTrigger>().hitCounter--;
            }
            else
            {
                target.GetComponent<Collider>().enabled = false;
            }

            TargetTrigger[] targets = GameObject.Find("DynamicObjects").GetComponentsInChildren<TargetTrigger>();
            int totalCount = 0;
            foreach (TargetTrigger t in targets)
            {
                totalCount += t.hitCounter;
            }

            if (totalCount == 0)
            {
                SetFinalPosition(interactable);
                SetFinalRotation(interactable);
                ParameterObjects();

                StartVisualFeedback(checkmark, interactable);
                ActivityManager.instance.playSingleInstruction(selectCorrect(Correct), "Correct", () =>
                    StopVisualFeedback(checkmark, () => ActivityManager.instance.nextEvent()));

                SaveAction(request: ActivityManager.instance.Request, hints: ActivityManager.instance.hints);
            }
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
    private void UpdateHitCounters()
    {
        progressBarManager.CurrentValue = progressBarManager.MaxValue - HitCounters.Aggregate(0f, (acc, x) => acc + x);
    }

    public override void Ready()
    {
        progressBarManager.MaxValue = HitCounters.Aggregate(0, (acc, x) => acc + x);
    }

}
