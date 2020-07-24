using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static DataModel;

public class ActivityManager : MonoBehaviour
{
    private ActivityConfiguration _activityConfiguration;

    private int _eventGroupStep;
    private int _eventStep;

    private List<GameObject> _activityAssets;

    private bool _isFree;
    public bool IsFree
    {
        get => _isFree;
        set
        {
            _isFree = value;
            if (_isFree == true && eventGroupManager != null)
            {
                eventGroupManager.Ready();
            }
        }
    }

    [HideInInspector]
    public int hints;

    private Transform _dynamicObjects;

    public SpeechToText speechToText = new SpeechToText();

    public string ActivityPath { get => "Activities/" + _activityConfiguration.id; }
    private List<EventGroup> EventGroups { get => _activityConfiguration.eventGroups; }

    public EventGroup CurrentEventGroup { get => EventGroups[_eventGroupStep]; }
    private EventObjs EventGroupObjs { get => CurrentEventGroup.eventGroupObjs; }
    private List<string> InstructionIntro { get => CurrentEventGroup.instructionIntro; }
    private List<string> InstructionEnd { get => CurrentEventGroup.instructionEnd; }
    private int InteractablesToSpawn { get => CurrentEventGroup.interactablesToSpawn; }
    private int TargetsToSpawn { get => CurrentEventGroup.targetsToSpawn; }
    private bool InteractablesRandomSpawn { get => CurrentEventGroup.interactablesRandomSpawn; }
    private bool TargetsRandomSpawn { get => CurrentEventGroup.targetsRandomSpawn; }
    private List<CustomTransform> InteractablesSpawnPoints { get => CurrentEventGroup.interactablesSpawnPoints; }
    private List<CustomTransform> TargetsSpawnPoints { get => CurrentEventGroup.targetsSpawnPoints; }
    private int StepsToReproduce { get => CurrentEventGroup.stepsToReproduce; }
    private List<EventConfiguration> EventsInCurrentGroup { get; set; }

    public EventConfiguration CurrentEvent { get => EventsInCurrentGroup[_eventStep]; }
    public EventObjs EventObjs { get => CurrentEvent.eventObjs; }
    public EventParameters Parameters { get => CurrentEvent.parameters; }
    public List<string> Request { get => CurrentEvent.instructions.request; }


    private EventGroupManagerBase eventGroupManager;
    private void setEventGroupManager(string type)
    {
        eventGroupManager = (EventGroupManagerBase)ScriptableObject.CreateInstance(Type.GetType(type));
    }


    public static ActivityManager instance
    {
        get;
        private set;
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    private async void Start()
    {
        _dynamicObjects = new GameObject("DynamicObjects").transform;

        _eventGroupStep = 0;
        _eventStep = 0;

        _activityConfiguration = GameManager.instance.currentActivity;

        _activityAssets = await Addressables.LoadAssetsAsync<GameObject>(_activityConfiguration.id, null).Task as List<GameObject>;

        GameManager.instance.ActivityReady();
    }

    public void Initialize()
    {
        setEventGroupManager(CurrentEventGroup.type);
        EventsInCurrentGroup = eventGroupManager.SetEventsInCurrentGroup(CurrentEventGroup.events, CurrentEventGroup.randomEvents, StepsToReproduce);

        generateSceneObjects(EventGroupObjs);
        playInstructionSequence(InstructionIntro, 0, () =>
        {
            generateSceneObjects(EventObjs);
            playSingleInstruction(eventGroupManager.selectRequest(Request), "Talk");
        });
    }

    public void generateSceneObjects(EventObjs objs)
    {
        if (objs.interactablesToActivate != null && objs.interactablesToActivate.Count != 0)
        {
            generateObjCategory(objs.interactablesToActivate, InteractablesToSpawn, InteractablesSpawnPoints, InteractablesRandomSpawn, typeof(InteractableTrigger));
        }

        if (objs.targetsToActivate != null && objs.targetsToActivate.Count != 0)
        {
            generateObjCategory(objs.targetsToActivate, TargetsToSpawn, TargetsSpawnPoints, TargetsRandomSpawn, typeof(TargetTrigger));
        }

        if (objs.othersToActivate != null && objs.othersToActivate.Count != 0)
        {
            foreach (SceneObj obj in objs.othersToActivate)
            {
                InstantiateObj(obj);
            }
        }
    }

    private void generateObjCategory(List<SceneObj> objs, int objsToSpawn, List<CustomTransform> spawnPoints, bool randomSpawn, Type type)
    {
        objsToSpawn = objsToSpawn != 0 ? objsToSpawn : objs.Count;

        if (objsToSpawn <= objs.Count)
        {
            objs = eventGroupManager.randomizeObjects(objs, objsToSpawn);
        }

        List<GameObject> instantiatedObjs = new List<GameObject>();

        foreach (SceneObj obj in objs)
        {
            GameObject temp = InstantiateObj(obj);
            instantiatedObjs.Add(temp);
            temp.AddComponent(type);
        }

        if (spawnPoints != null && spawnPoints.Count > 0)
        {
            assignSpawnPoints(instantiatedObjs, spawnPoints);
        }

        if (randomSpawn)
        {
            randomizeObjSpawn(instantiatedObjs);
        }
    }


    public GameObject InstantiateObj(SceneObj obj)
    {
        GameObject temp = Instantiate(_activityAssets.Find(x => x.name == obj.prefab), _dynamicObjects);
        temp.transform.DOScale(new Vector3(0, 0, 0), 1f).From();

        temp.name = obj.name ?? obj.text ?? obj.prefab;
        if (obj.text != null)
        {
            temp.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = obj.text;
        }

        return temp;
    }

    public void assignSpawnPoints(List<GameObject> objs, List<CustomTransform> spawnPoints)
    {
        for (int i = 0; i < objs.Count; i++)
        {
            objs[i].transform.position = new Vector3(spawnPoints[i].position.x, spawnPoints[i].position.y, spawnPoints[i].position.z);
            objs[i].transform.rotation = Quaternion.Euler(spawnPoints[i].rotation.x, spawnPoints[i].rotation.y, spawnPoints[i].rotation.z);
        }
    }

    public void randomizeObjSpawn(List<GameObject> objs)
    {
        System.Random rnd = new System.Random();
        List<Tuple<Vector3, Quaternion>> shuffled = objs
            .Select(c => new Tuple<Vector3, Quaternion>(c.transform.position, c.transform.rotation))
            .OrderBy(c => rnd.Next())
            .ToList();

        for (int i = 0; i < objs.Count; i++)
        {
            objs[i].transform.position = shuffled[i].Item1;
            objs[i].transform.rotation = shuffled[i].Item2;
        }
    }

    public void removeSceneObjects(EventObjs objs)
    {
        if (objs.interactablesToDeactivate != null)
        {
            foreach (string toRemove in objs.interactablesToDeactivate)
            {
                GameObject temp = GameObject.Find(toRemove);
                if (temp != null)
                {
                    temp.transform.DOScale(new Vector3(0, 0, 0), 1f).OnComplete(() => Destroy(temp));
                }
            }
        }

        if (objs.targetsToDeactivate != null)
        {
            foreach (string toRemove in objs.targetsToDeactivate)
            {
                GameObject temp = GameObject.Find(toRemove);
                if (temp != null)
                {
                    temp.transform.DOScale(new Vector3(0, 0, 0), 1f).OnComplete(() => Destroy(temp));
                }
            }
        }

        if (objs.othersToDeactivate != null)
        {
            foreach (string toRemove in objs.othersToDeactivate)
            {
                GameObject temp = GameObject.Find(toRemove);
                if (temp != null)
                {
                    temp.transform.DOScale(new Vector3(0, 0, 0), 1f).OnComplete(() => Destroy(temp));
                }
            }
        }
    }

    private void nextEventGroup()
    {
        removeSceneObjects(EventGroupObjs);

        if (_eventGroupStep + 1 >= EventGroups.Count)
        {
            GameManager.instance.ActivityCompleted();
        }
        else
        {
            _eventStep = 0;
            _eventGroupStep++;

            setEventGroupManager(CurrentEventGroup.type);
            EventsInCurrentGroup = eventGroupManager.SetEventsInCurrentGroup(CurrentEventGroup.events, CurrentEventGroup.randomEvents, StepsToReproduce);

            generateSceneObjects(EventGroupObjs);
            playInstructionSequence(InstructionIntro, 0, () =>
            {
                generateSceneObjects(EventObjs);
                playSingleInstruction(eventGroupManager.selectRequest(Request), "Talk");
            });
        }
    }

    public void nextEvent()
    {
        hints = 0;
        removeSceneObjects(EventObjs);

        if (_eventStep + 1 >= EventsInCurrentGroup.Count)
        {
            playInstructionSequence(InstructionEnd, 0, () => nextEventGroup());
        }
        else
        {
            _eventStep++;

            generateSceneObjects(EventObjs);
            playSingleInstruction(eventGroupManager.selectRequest(Request), "Talk");
        }
    }

    public void playInstructionSequence(List<string> instructions, int index, UnityAction call = null)
    {
        IsFree = false;
        if (instructions == null || index == instructions.Count)
        {
            IsFree = call == null;
            call?.Invoke();
        }
        else
        {
            VirtualAssistantManager.instance.startTalking(instructions[index], "Talk", () =>
            {
                VirtualAssistantManager.instance.stopTalking("Talk", () =>
                {
                    index++;
                    playInstructionSequence(instructions, index, call);
                });
            });
        }
    }

    public void playSingleInstruction(string instruction, string animatorParam, UnityAction call = null)
    {
        if (instruction != null)
        {
            hints++;
            IsFree = false;
            VirtualAssistantManager.instance.startTalking(instruction, animatorParam, () =>
            {
                VirtualAssistantManager.instance.stopTalking(animatorParam, () =>
                {
                    IsFree = call == null;
                    call?.Invoke();
                });
            });
        }
        else
        {
            IsFree = true;
        }
    }

    public void checkCorrectAction(GameObject interactable)
    {
        if (IsFree)
        {
            eventGroupManager.checkCorrectAction(interactable);
        }
    }

    public void checkCorrectAction(GameObject target, GameObject interactable = null)
    {
        if (IsFree)
        {
            eventGroupManager.checkCorrectAction(target, interactable);
        }
    }

    public void checkCorrectAction(string answer)
    {
        if (IsFree)
        {
            eventGroupManager.checkCorrectAction(answer);
        }
    }

    public void AssistantTriggered()
    {
        if (IsFree && EventsInCurrentGroup != null)
        {
            playSingleInstruction(eventGroupManager.selectRequest(Request), "Talk");
        }
    }
}