using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PointReorderSentenceManager : EventGroupManagerBase
{
    private GameObject checkmark;
    private GameObject cross;

    private bool correct = true;
    private List<string> stack = new List<string>();

    private string Text
    {
        get => stack.Count == 0 ? ActivityManager.instance.EventObjs.targetsToActivate[0].text : string.Join(" ", stack);
    }

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


    public override void checkCorrectAction(GameObject interactable)
    {
        correct = correct && (interactable.name == ActivityManager.instance.Parameters.correctInteractables[stack.Count]);
        stack.Add(interactable.name);
        interactable.SetActive(false);

        GameObject sentence = GameObject.Find(ActivityManager.instance.EventObjs.targetsToActivate[0].name);
        sentence.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = Text;

        if (stack.Count == ActivityManager.instance.Parameters.correctInteractables.Count)
        {
            if (correct)
            {
                StartVisualFeedback(checkmark, interactable);
                ActivityManager.instance.playSingleInstruction(selectCorrect(Correct, Text), "Correct", () =>
                    StopVisualFeedback(checkmark, () => ActivityManager.instance.nextEvent()));
            }
            else
            {
                StartVisualFeedback(cross, interactable);
                ActivityManager.instance.playSingleInstruction(selectWrong(Wrong), "Wrong", () =>
                {
                    ActivityManager.instance.removeSceneObjects(ActivityManager.instance.EventObjs);
                    ActivityManager.instance.generateSceneObjects(ActivityManager.instance.EventObjs);
                    StopVisualFeedback(cross, () => ActivityManager.instance.IsFree = true);
                });
            }
            stack.Clear();
            correct = true;
        }
    }

    public override void checkCorrectAction(GameObject target, GameObject interactable = null)
    {
        ActivityManager.instance.removeSceneObjects(ActivityManager.instance.EventObjs);
        ActivityManager.instance.generateSceneObjects(ActivityManager.instance.EventObjs);
        stack.Clear();
        correct = true;
    }

    public override string selectCorrect(List<string> instructions, string param = null)
    {
        string selected = base.selectWrong(instructions);
        return selected.Replace(@"#sentence#", param);
    }

    private void StartVisualFeedback(GameObject feedback, GameObject interactable)
    {
        feedback.transform.position = interactable.transform.position;
        feedback.transform.rotation = interactable.transform.rotation;
        _initialScale = feedback.transform.localScale;
        feedback.transform.localScale *= Vector3.Distance(interactable.transform.position, Camera.allCameras[0].transform.position);
        feedback.SetActive(true);
        feedback.transform.DOScale(new Vector3(0, 0, 0), 0.5f).From();
    }
}

