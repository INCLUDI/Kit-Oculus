using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using static DataModel;

public class PointCompleteSentenceManager : EventGroupManagerBase
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


    public override List<SceneObj> randomizeObjects(List<SceneObj> objs, int objectsToSpawn)
    {
        List<SceneObj> filtered = new List<SceneObj>();
        filtered.Add(objs.Find(x => x.text == ActivityManager.instance.Parameters.correctInteractables[0]));
        System.Random rnd = new System.Random();
        filtered.AddRange(objs.Where(x => x.text != ActivityManager.instance.Parameters.correctInteractables[0])
            .OrderBy(c => rnd.Next())
            .Take(objectsToSpawn)
            .ToList());
        return filtered;
    }

    public override void checkCorrectAction(GameObject interactable)
    {
        if (ActivityManager.instance.Parameters.correctInteractables.Contains(interactable.name))
        {
            interactable.SetActive(false);
            StartVisualFeedback(checkmark, interactable);

            GameObject sentence = GameObject.Find(ActivityManager.instance.EventObjs.othersToDeactivate[0]);
            string completedSentence = sentence.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text.Replace("______", interactable.name);
            sentence.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = completedSentence;

            ActivityManager.instance.playSingleInstruction(selectCorrect(Correct, completedSentence), "Correct", 
                () => StopVisualFeedback(checkmark, () => ActivityManager.instance.nextEvent()));
        }
        else
        {
            StartVisualFeedback(cross, interactable);
            ActivityManager.instance.playSingleInstruction(selectWrong(Wrong, interactable.name), "Wrong", 
                () => StopVisualFeedback(cross, () => ActivityManager.instance.IsFree = true));
        }
    }

    public override string selectCorrect(List<string> instructions, string param = null)
    {
        string selected = base.selectCorrect(instructions);
        return selected.Replace(@"#sentence#", param);
    }

    public override string selectWrong(List<string> instructions, string param = null)
    {
        string selected = base.selectWrong(instructions);
        return selected.Replace(@"#wrong#", "\"" + param + "\"");
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
