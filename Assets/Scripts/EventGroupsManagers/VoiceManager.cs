using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceManager : EventGroupManagerBase
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


    public override void Ready()
    {
        ActivityManager.instance.speechToText.StartListening();
    }

    public override void checkCorrectAction(string answer)
    {
        if (ActivityManager.instance.Parameters.correctAnswers.Contains(answer))
        {
            StartVisualFeedback(checkmark);
            ActivityManager.instance.playSingleInstruction(selectCorrect(Correct), "Correct", () =>
                StopVisualFeedback(checkmark, () => ActivityManager.instance.nextEvent()));
        }
        else
        {
            StartVisualFeedback(cross);
            ActivityManager.instance.playSingleInstruction(selectWrong(Wrong), "Wrong", 
                () => StopVisualFeedback(cross, () => ActivityManager.instance.IsFree = true));
        }
    }

    public override string selectRequest(List<string> instructions)
    {
        string selected = base.selectRequest(instructions);
        return selected?.Replace(@"#request#",
                ActivityManager.instance.EventObjs.othersToActivate[0].name ??
                ActivityManager.instance.EventObjs.othersToActivate[0].prefab);
    }

    public override string selectCorrect(List<string> instructions, string param = null)
    {
        string selected = base.selectCorrect(instructions);
        return selected?.Replace(@"#request#",
                ActivityManager.instance.EventObjs.othersToActivate[0].name ?? 
                ActivityManager.instance.EventObjs.othersToActivate[0].prefab);
    }

    private void StartVisualFeedback(GameObject feedback)
    {
        feedback.transform.position = Vector3.Lerp(Camera.allCameras[0].transform.forward, Camera.allCameras[0].transform.position, 0.3f);
        feedback.transform.rotation = Quaternion.LookRotation(Camera.allCameras[0].transform.forward - Camera.allCameras[0].transform.position);
        _initialScale = feedback.transform.localScale;
        feedback.transform.localScale *= Vector3.Distance(Camera.allCameras[0].transform.forward, Camera.allCameras[0].transform.position);
        feedback.SetActive(true);
        feedback.transform.DOScale(new Vector3(0, 0, 0), 0.5f).From();
    }
}

