using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static DataModel;

public class ActivityManager : MonoBehaviour
{
    private List<EventConfiguration> _eventList;

    private int _eventStep;
    private int _audioStep;

    public Transform dynamicObjects;

    public bool readyForNextEvent = false;

    public bool isFree = true;
    // Start is called before the first frame update
    void Start()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        //Parso il json giusto
        TextAsset file = Resources.Load<TextAsset>(sceneName + "/" + sceneName);

        _eventList = JsonUtility.FromJson<SceneConfiguration>(file.text).events;

        _audioStep = 0;
        _eventStep = 0;
        // Instanzio gli oggetti nella scena 
        generateSceneObjectsFromEvent(_eventStep);

        playAudioSequence();
    }

    private void generateSceneObjectsFromEvent(int eventStep)
    {
        foreach (SceneObj obj in _eventList[eventStep].objsToActivate)
        {
            GameObject temp = (GameObject)Instantiate(Resources.Load(obj.path), new Vector3(obj.position.x, obj.position.y, obj.position.z),
                Quaternion.identity, dynamicObjects);
            temp.transform.localScale = new Vector3(obj.scale.x, obj.scale.y, obj.scale.z);
            temp.transform.rotation = Quaternion.Euler(obj.rotation.x, obj.rotation.y, obj.rotation.z);
            temp.name = obj.uid;
        }
        foreach (SceneObj obj in _eventList[eventStep].targetsToActivate)
        {
            GameObject temp = (GameObject)Instantiate(Resources.Load(obj.path), new Vector3(obj.position.x, obj.position.y, obj.position.z),
                Quaternion.identity, dynamicObjects);
            temp.transform.localScale = new Vector3(obj.scale.x, obj.scale.y, obj.scale.z);
            temp.transform.rotation = Quaternion.Euler(obj.rotation.x, obj.rotation.y, obj.rotation.z);
            temp.name = obj.uid;
        }
    }

    private void removeSceneObjectsFromEvent(int eventStep)
    {
        foreach (string toRemove in _eventList[eventStep].objsToDeactivate)
        {
            Destroy(GameObject.Find(toRemove));
        }
        foreach (string toRemove in _eventList[eventStep].targetsToDeactivate)
        {
            Destroy(GameObject.Find(toRemove));
        }
    }

    public void nextEvent()
    {
        removeSceneObjectsFromEvent(_eventStep);

        if (_eventStep + 1 >= _eventList.Count)
        {
            Debug.Log("Finito");
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
        AudioManager.instance.playAudioFromString(_eventList[_eventStep].audio[_audioStep], () => {
            if (_audioStep + 1 < _eventList[_eventStep].audio.Count)
            {
                _audioStep++;
                playAudioSequence();
            }
            else
            {
                EventManager.TriggerEvent("stopTalking");
            }
        });
    }


    public void checkCorrectObject(GameObject collidingObject)
    {
        if (_eventList[_eventStep].type == "dragrelease")
        {
            List<string> correctList = _eventList[_eventStep].parameters.correct;
            if (correctList.Contains(collidingObject.name))
            {   
                collidingObject.GetComponent<Collider>().enabled = false;
                collidingObject.GetComponent<Rigidbody>().isKinematic = true;
                collidingObject.GetComponent<Rigidbody>().useGravity = false;
                collidingObject.GetComponent<OVRGrabbable>().enabled = false;

                SetFinalPosition(collidingObject);
                SetFinalRotation(collidingObject);

                // Per rilasciare la mano o il controller
                EventManager.TriggerEvent("ReleaseObject");

                AudioManager.instance.playAudioFromString(_eventList[_eventStep].audioFeedback.audioOk, () => {
                    nextEvent();
                });
            }
            else
            {
                AudioManager.instance.playAudioFromString(_eventList[_eventStep].audioFeedback.audioWrong, () => {
                    UpdateIsBusy(true);
                });
            }

        }

    }

    private void SetFinalPosition(GameObject collidingObject)
    {
        CustomVector3 finalPosition = _eventList[_eventStep].parameters.finalPosition;
        collidingObject.transform.position = new Vector3(finalPosition.x, finalPosition.y, finalPosition.z);
    }

    private void SetFinalRotation(GameObject collidingObject)
    {
        CustomVector3 finalRotation = _eventList[_eventStep].parameters.finalRotation;
        collidingObject.transform.rotation = Quaternion.Euler(finalRotation.x, finalRotation.y, finalRotation.z);
    }

    private void UpdateIsBusy(bool isFree)
    {
        this.isFree = isFree;
    }
}
