using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static DataModel;
using static StatsManager;

public class TouchManager : EventGroupManagerBase
{
    private GameObject checkmark;
    private GameObject cross;

    private void Awake()
    {
        checkmark = Instantiate(Resources.Load<GameObject>("Checkmark"));
        cross = Instantiate(Resources.Load<GameObject>("Cross"));
    }

    private void OnDestroy()
    {
        Destroy(checkmark);
        Destroy(cross);
    }


    public override void checkCorrectAction(GameObject selectable)
    {
        if (ActivityManager.instance.Parameters.correctSelectables.Contains(selectable.name))
        {
            SetFinalPosition(selectable);
            SetFinalRotation(selectable);
            ParameterObjects();

            StartVisualFeedback(checkmark, selectable);
            ActivityManager.instance.playSingleInstruction(selectCorrect(Correct), "Correct", () =>
                StopVisualFeedback(checkmark, () => ActivityManager.instance.nextEvent()));

            SaveAction(request: ActivityManager.instance.Request, correctParameters: ActivityManager.instance.Parameters, action: new ActionParameters { selectable = selectable.name }, hints: ActivityManager.instance.hints);
        }
        else
        {
            StartVisualFeedback(cross, selectable);
            ActivityManager.instance.playSingleInstruction(selectWrong(Wrong, selectable.name), "Wrong",
                () => StopVisualFeedback(cross, () => ActivityManager.instance.IsFree = true));

            SaveAction(request: ActivityManager.instance.Request, correctParameters: ActivityManager.instance.Parameters, action: new ActionParameters { selectable = selectable.name }, hints: ActivityManager.instance.hints, error: true);
        }
    }

    public override string selectRequest(List<string> instructions)
    {
        string selected = base.selectRequest(instructions);
        return selected?.Replace(@"#request#", ActivityManager.instance.Parameters.correctSelectables[0]);
    }

    public override string selectCorrect(List<string> instructions, string param = null)
    {
        string selected = base.selectCorrect(instructions);
        return selected?.Replace(@"#request#", ActivityManager.instance.Parameters.correctSelectables[0]);
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