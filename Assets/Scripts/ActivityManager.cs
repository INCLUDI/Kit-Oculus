using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static DataModel;
using DG.Tweening;

public class ActivityManager : MonoBehaviour
{
    private List<EventConfiguration> _eventList;

    private int _eventStep;
    private int _audioStep;

    public Transform dynamicObjects;

    public bool isFree = true;

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

    private void OnDestroy() 
    { 
        if (this == instance) 
        { 
            instance = null; 
        } 
    }

    // Start is called before the first frame update
    void Start()
    {
        TextAsset file = Resources.Load<TextAsset>(GameManager.instance.Getjson());
        _eventList = JsonUtility.FromJson<SceneConfiguration>(file.text).events;

        _audioStep = 0;
        _eventStep = 0;

        generateSceneObjectsFromEvent(_eventStep);

        playAudioSequence();
    }

    private void generateSceneObjectsFromEvent(int eventStep)
    {
        foreach (SceneObj obj in _eventList[eventStep].sceneObjs.grabbablesToActivate)
        {
            GameObject temp = ActivateObj(obj);
            temp.AddComponent<GrabbableManager>();
        }
        foreach (SceneObj obj in _eventList[eventStep].sceneObjs.targetsToActivate)
        {
            GameObject temp = ActivateObj(obj);
            temp.AddComponent<TargetManager>();
            temp.GetComponent<TargetManager>().hitCounter = 
                _eventList[_eventStep].parameters.numericParameter == 0 ? 1 : _eventList[_eventStep].parameters.numericParameter;
        }
        foreach (SceneObj obj in _eventList[eventStep].sceneObjs.othersToActivate)
        {
            ActivateObj(obj);
        }
    }

    private GameObject ActivateObj(SceneObj obj)
    {
        GameObject temp = (GameObject)Instantiate(Resources.Load(obj.path), new Vector3(obj.position.x, obj.position.y, obj.position.z),
                Quaternion.identity, dynamicObjects);
        temp.transform.localScale = new Vector3(obj.scale.x, obj.scale.y, obj.scale.z);
        temp.transform.rotation = Quaternion.Euler(obj.rotation.x, obj.rotation.y, obj.rotation.z);
        temp.name = obj.uid;
        return temp;
    }
    
    private void removeSceneObjectsFromEvent(int eventStep)
    {
        foreach (string toRemove in _eventList[eventStep].sceneObjs.grabbablesToDeactivate)
        {
            Destroy(GameObject.Find(toRemove));
        };
        foreach (string toRemove in _eventList[eventStep].sceneObjs.targetsToDeactivate)
        {
            Destroy(GameObject.Find(toRemove));
        };
        foreach (string toRemove in _eventList[eventStep].sceneObjs.othersToDeactivate)
        {
            Destroy(GameObject.Find(toRemove));
        }
    }

    public void nextEvent()
    {
        removeSceneObjectsFromEvent(_eventStep);

        if (_eventStep + 1 >= _eventList.Count)
        {
            GameManager.instance.NextScene();
        }
        else
        {
            _audioStep = 0;
            _eventStep++;

            generateSceneObjectsFromEvent(_eventStep);
            playAudioSequence();
        }
    }

    private void playAudioSequence()
    {
        EventManager.TriggerEvent("DisableInteraction");
        AudioManager.instance.playAudioFromString(_eventList[_eventStep].audioFeedback.audio[_audioStep], () => {
            if (_audioStep + 1 < _eventList[_eventStep].audioFeedback.audio.Count)
            {
                _audioStep++;
                playAudioSequence();
            }
            else
            {
                EventManager.TriggerEvent("stopTalking");
                EventManager.TriggerEvent("EnableInteraction");
            }
        });
    }


    public void checkCorrectObject(GameObject collidingObject, GameObject target)
    {
        Debug.Log(collidingObject, target);

        if (_eventList[_eventStep].type == "dragrelease")
        {
            List<string> correctGrabbables = _eventList[_eventStep].parameters.correctGrabbable;
            List<string> correctTargets = _eventList[_eventStep].parameters.correctTarget;

            if (correctGrabbables.Contains(collidingObject.name) && correctTargets.Contains(target.name))
            {
                collidingObject.GetComponent<Collider>().enabled = false;
                collidingObject.GetComponent<Rigidbody>().isKinematic = true;
                collidingObject.GetComponent<OVRGrabbable>().enabled = false;
                EventManager.TriggerEvent("ReleaseObject");

                SetFinalPosition(collidingObject);
                SetFinalRotation(collidingObject);

                AudioManager.instance.playAudioFromString(_eventList[_eventStep].audioFeedback.audioOk, () => {
                    nextEvent();
                });
            }
            else
            {
                AudioManager.instance.playAudioFromString(_eventList[_eventStep].audioFeedback.audioWrong);
            }
        }
        if (_eventList[_eventStep].type == "dragmove" && isFree) 
        {
            isFree = false;

            if (target.GetComponent<TargetManager>().hitCounter > 0)
            {
                target.GetComponent<TargetManager>().hitCounter--;
            }
            else
            {
                target.GetComponent<Collider>().enabled = false;
            }

            TargetManager[] targets = dynamicObjects.GetComponentsInChildren<TargetManager>();
            int totalCount = 0;
            foreach (TargetManager t in targets)
            {
                totalCount += t.hitCounter;
            }

            Debug.Log(totalCount);

            if (totalCount == 0)
            {
                foreach (string toRemove in _eventList[_eventStep].parameters.objsToDeactivate)
                {
                    Destroy(GameObject.Find(toRemove));
                };
                foreach (SceneObj obj in _eventList[_eventStep].parameters.objsToActivate)
                {
                    ActivateObj(obj);
                }

                AudioManager.instance.playAudioFromString(_eventList[_eventStep].audioFeedback.audioOk, () => {
                    nextEvent();
                });
            }

            isFree = true;
        }
    }

    private void SetFinalPosition(GameObject collidingObject)
    {
        CustomVector3 finalPosition = _eventList[_eventStep].parameters.objsToActivate[0].position;
        collidingObject.transform.DOMove(new Vector3(finalPosition.x, finalPosition.y, finalPosition.z), 1);
    }

    private void SetFinalRotation(GameObject collidingObject)
    {
        CustomVector3 finalRotation = _eventList[_eventStep].parameters.objsToActivate[0].rotation;
        collidingObject.transform.DORotate(new Vector3(finalRotation.x, finalRotation.y, finalRotation.z), 1);
    }


}
