using System;
using System.Collections.Generic;
using UnityEngine;

public class DataModel : MonoBehaviour
{
    [Serializable]
    public class CustomVector3
    {
        public float x;
        public float y;
        public float z;
    }

    [Serializable]
    public class CustomTransform
    {
        public CustomVector3 position;
        public CustomVector3 rotation;
        public CustomVector3 scale;
    }


    [Serializable]
    public class SceneObj
    {
        public string prefab;
        public string name;
        public string text;
    }

    [Serializable]
    public class Instructions
    {
        public List<string> request;
        public List<string> correct;
        public List<string> wrong;
    }

    [Serializable]
    public class EventParameters
    {
        public List<string> correctInteractables;
        public List<string> correctTargets;
        public List<string> correctAnswers;
        public CustomTransform finalTransform;
        public List<SceneObj> objsToActivate;
        public List<string> objsToDeactivate;
        public int numericParameter;
    }

    [Serializable]
    public class EventObjs
    {
        public List<SceneObj> interactablesToActivate;
        public List<string> interactablesToDeactivate;
        public List<SceneObj> targetsToActivate;
        public List<string> targetsToDeactivate;
        public List<SceneObj> othersToActivate;
        public List<string> othersToDeactivate;
    }

    [Serializable]
    public class EventConfiguration
    {
        public EventObjs eventObjs;
        public EventParameters parameters;
        public Instructions instructions;
    }

    [Serializable]
    public class EventGroup
    {
        public string type;
        public EventObjs eventGroupObjs;
        public List<string> instructionIntro;
        public List<string> instructionEnd;
        public int stepsToReproduce;
        public bool randomEvents;
        public int interactablesToSpawn;
        public int targetsToSpawn;
        public bool interactablesRandomSpawn;
        public bool targetsRandomSpawn;
        public int timeout;
        public List<CustomTransform> interactablesSpawnPoints;
        public List<CustomTransform> targetsSpawnPoints;
        public List<EventConfiguration> events;
    }

    [Serializable]
    public class ActivityConfiguration
    {
        public string id;
        public string name;
        public string description;
        public string image;
        public int difficulty;
        public string scene;
        public string jsonPath;
        public CustomTransform assistantTransform;
        public CustomTransform playerTransform;
        public List<EventGroup> eventGroups;
    }
}