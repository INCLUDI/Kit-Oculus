using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using static DataModel;

public class PointAssociateManager : EventGroupManagerBase
{
    private GameObject checkmark;
    private GameObject cross;

    private List<string> interactables = new List<string>();
    private List<string> targets = new List<string>();

    private List<int> indexes = new List<int>();

    private Dictionary<string, string> _correctCouples =
            ActivityManager.instance.Parameters.correctInteractables.Zip(ActivityManager.instance.Parameters.correctTargets, (k, v) => new { k, v })
            .ToDictionary(x => x.k, x => x.v);

    private string _selectedInteractable;
    private string SelectedInteractable
    {
        get => _selectedInteractable;
        set => _selectedInteractable = value;
    }

    private int index = 0;

    private void Awake()
    {
        checkmark = Instantiate(Resources.Load<GameObject>("Checkmark"));
        cross = Instantiate(Resources.Load<GameObject>("Cross"));
    }

    public override List<SceneObj> randomizeObjects(List<SceneObj> objs, int objectToSpawn)
    {
        List<SceneObj> filtered = new List<SceneObj>();
        System.Random rnd = new System.Random();

        if (indexes.Count == 0)
        {
            indexes = Enumerable.Range(0, objs.Count)
                .OrderBy(c => rnd.Next())
                .Take(objectToSpawn)
                .ToList();
            foreach (int index in indexes)
            {
                filtered.Add(objs[index]);
                interactables.Add(objs[index].name);
            }
        }
        else
        {
            foreach (int index in indexes.OrderBy(c => rnd.Next()).ToList())
            {
                filtered.Add(objs[index]);
                targets.Add(objs[index].name);
            }
            indexes.Clear();
        }
        return filtered;
    }

    public override void checkCorrectAction(GameObject interactable)
    {
        if (SelectedInteractable == null)
        {
            SelectedInteractable = interactable.name;
            interactable.SetActive(false);
            GameObject.Find("interactables").transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = interactable.name;
        }
    }

    public override void checkCorrectAction(GameObject target, GameObject interactable = null)
    {
        if (SelectedInteractable != null)
        {
            _correctCouples.TryGetValue(SelectedInteractable, out string correctTarget);
            if (target.name == correctTarget)
            {
                GameObject.Find("targets").transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = target.name;
                target.SetActive(false);
                index++;
                StartVisualFeedback(checkmark, target);
                ActivityManager.instance.playSingleInstruction(selectCorrect(Correct, SelectedInteractable + " " + target.name), "Correct", () =>
                {
                    SelectedInteractable = null;
                    GameObject.Find("interactables").transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "_____";
                    GameObject.Find("targets").transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "_____";

                    if (index == ActivityManager.instance.CurrentEventGroup.interactablesToSpawn)
                    {
                        index = 0;
                        StopVisualFeedback(checkmark, () => ActivityManager.instance.nextEvent());
                    }
                    else
                    {
                        StopVisualFeedback(checkmark, () => ActivityManager.instance.IsFree = true);
                    }
                });
            }
            else
            {
                StartVisualFeedback(cross, target);
                ActivityManager.instance.playSingleInstruction(selectWrong(Wrong), "Wrong", () => 
                    StopVisualFeedback(cross, () => ActivityManager.instance.IsFree = true));
            }
        }
    }

    public override string selectCorrect(List<string> instructions, string param = null)
    {
        string selected = base.selectCorrect(instructions);
        return selected.Replace(@"#correct#", param);
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
