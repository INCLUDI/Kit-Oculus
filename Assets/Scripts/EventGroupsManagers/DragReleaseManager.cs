﻿using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static DataModel;

public class DragReleaseManager : EventGroupManagerBase
{
    private GameObject checkmark;
    private GameObject cross;

    private void Awake()
    {
        checkmark = Instantiate(Resources.Load<GameObject>("Checkmark"));
        cross = Instantiate(Resources.Load<GameObject>("Cross"));
    }

    public override void checkCorrectAction(GameObject target, GameObject interactable)
    {
        if (ActivityManager.instance.Parameters.correctInteractables.Contains(interactable.name) && 
            ActivityManager.instance.Parameters.correctTargets.Contains(target.name))
        {
            interactable.GetComponent<Collider>().enabled = false;
            interactable.GetComponent<Rigidbody>().isKinematic = true;
            interactable.GetComponent<OVRGrabbable>().enabled = false;
            EventManager.TriggerEvent("ReleaseObject");

            SetFinalPosition(interactable);
            SetFinalRotation(interactable);
            ParameterObjects();

            StartVisualFeedback(checkmark, interactable);
            ActivityManager.instance.playSingleInstruction(selectCorrect(Correct), "Correct", () =>
                StopVisualFeedback(checkmark, () => ActivityManager.instance.nextEvent()));
        }
        else
        {
            StartVisualFeedback(cross, interactable);
            ActivityManager.instance.playSingleInstruction(selectWrong(Wrong, interactable.name), "Wrong",
                () => StopVisualFeedback(cross, () => ActivityManager.instance.IsFree = true));
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
}
