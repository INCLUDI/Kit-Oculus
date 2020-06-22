using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static DataModel;

public class PointEventManager : EventGroupManagerBase
{
    private GameObject checkmark;
    private GameObject cross;

    private void Awake()
    {
        checkmark = Instantiate(Resources.Load<GameObject>("Checkmark"));
        cross = Instantiate(Resources.Load<GameObject>("Cross"));
    }

    public override List<SceneObj> randomizeObjects(List<SceneObj> objs, int objectsToSpawn)
    {
        List<SceneObj> filtered = new List<SceneObj>();
        filtered.Add(objs.Find(x => (x.name ?? x.prefab) == ActivityManager.instance.Parameters.correctInteractables[0]));
        System.Random rnd = new System.Random();
        filtered.AddRange(objs
            .Where(x => (x.name ?? x.prefab) != ActivityManager.instance.Parameters.correctInteractables[0])
            .OrderBy(x => rnd.Next())
            .Take(objectsToSpawn - 1)
            .ToList());
        return filtered;
    }

    public override void checkCorrectAction(GameObject interactable)
    {
        if (ActivityManager.instance.Parameters.correctInteractables.Contains(interactable.name))
        {
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
